using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using BestHTTP.Authentication;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using BestHTTP.PlatformSupport.TcpClient.General;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;

namespace BestHTTP
{
	// Token: 0x02000568 RID: 1384
	internal sealed class HTTPConnection : ConnectionBase
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x000FFAA0 File Offset: 0x000FDCA0
		public override bool IsRemovable
		{
			get
			{
				return base.IsRemovable || (base.IsFree && this.KeepAlive != null && DateTime.UtcNow - this.LastProcessTime >= this.KeepAlive.TimeOut);
			}
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x000FF7C1 File Offset: 0x000FD9C1
		internal HTTPConnection(string serverAddress) : base(serverAddress)
		{
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x000FFAEC File Offset: 0x000FDCEC
		protected override void ThreadFunc(object param)
		{
			bool flag = false;
			bool flag2 = false;
			RetryCauses retryCauses = RetryCauses.None;
			try
			{
				if (!base.HasProxy && base.CurrentRequest.HasProxy)
				{
					base.Proxy = base.CurrentRequest.Proxy;
				}
				if (!this.TryLoadAllFromCache())
				{
					if (this.Client != null && !this.Client.IsConnected())
					{
						this.Close();
					}
					for (;;)
					{
						if (retryCauses == RetryCauses.Reconnect)
						{
							this.Close();
							Thread.Sleep(100);
						}
						base.LastProcessedUri = base.CurrentRequest.CurrentUri;
						retryCauses = RetryCauses.None;
						this.Connect();
						if (base.State == HTTPConnectionStates.AbortRequested)
						{
							break;
						}
						if (!base.CurrentRequest.DisableCache)
						{
							HTTPCacheService.SetHeaders(base.CurrentRequest);
						}
						bool flag3 = false;
						try
						{
							this.Client.NoDelay = base.CurrentRequest.TryToMinimizeTCPLatency;
							base.CurrentRequest.SendOutTo(this.Stream);
							flag3 = true;
						}
						catch (Exception ex)
						{
							this.Close();
							if (base.State == HTTPConnectionStates.TimedOut || base.State == HTTPConnectionStates.AbortRequested)
							{
								throw new Exception("AbortRequested");
							}
							if (flag || base.CurrentRequest.DisableRetry)
							{
								throw ex;
							}
							flag = true;
							retryCauses = RetryCauses.Reconnect;
						}
						if (flag3)
						{
							bool flag4 = this.Receive();
							if (base.State == HTTPConnectionStates.TimedOut || base.State == HTTPConnectionStates.AbortRequested)
							{
								goto IL_135;
							}
							if (!flag4 && !flag && !base.CurrentRequest.DisableRetry)
							{
								flag = true;
								retryCauses = RetryCauses.Reconnect;
							}
							if (base.CurrentRequest.Response != null)
							{
								if (base.CurrentRequest.IsCookiesEnabled)
								{
									CookieJar.Set(base.CurrentRequest.Response);
								}
								int num = base.CurrentRequest.Response.StatusCode;
								if (num <= 308)
								{
									if (num - 301 <= 1 || num - 307 <= 1)
									{
										if (base.CurrentRequest.RedirectCount < base.CurrentRequest.MaxRedirects)
										{
											HTTPRequest currentRequest = base.CurrentRequest;
											num = currentRequest.RedirectCount;
											currentRequest.RedirectCount = num + 1;
											string firstHeaderValue = base.CurrentRequest.Response.GetFirstHeaderValue("location");
											if (string.IsNullOrEmpty(firstHeaderValue))
											{
												goto IL_434;
											}
											Uri redirectUri = this.GetRedirectUri(firstHeaderValue);
											if (HTTPManager.Logger.Level == Loglevels.All)
											{
												HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Redirected to Location: '{1}' redirectUri: '{1}'", base.CurrentRequest.CurrentUri.ToString(), firstHeaderValue, redirectUri));
											}
											if (!base.CurrentRequest.CallOnBeforeRedirection(redirectUri))
											{
												HTTPManager.Logger.Information("HTTPConnection", "OnBeforeRedirection returned False");
											}
											else
											{
												base.CurrentRequest.RemoveHeader("Host");
												base.CurrentRequest.SetHeader("Referer", base.CurrentRequest.CurrentUri.ToString());
												base.CurrentRequest.RedirectUri = redirectUri;
												base.CurrentRequest.Response = null;
												bool flag5 = base.CurrentRequest.IsRedirected = true;
												flag2 = flag5;
											}
										}
									}
								}
								else if (num != 401)
								{
									if (num == 407)
									{
										if (base.CurrentRequest.HasProxy)
										{
											string text = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("proxy-authenticate"));
											if (!string.IsNullOrEmpty(text))
											{
												Digest orCreate = DigestStore.GetOrCreate(base.CurrentRequest.Proxy.Address);
												orCreate.ParseChallange(text);
												if (base.CurrentRequest.Proxy.Credentials != null && orCreate.IsUriProtected(base.CurrentRequest.Proxy.Address) && (!base.CurrentRequest.HasHeader("Proxy-Authorization") || orCreate.Stale))
												{
													retryCauses = RetryCauses.ProxyAuthenticate;
												}
											}
										}
									}
								}
								else
								{
									string text2 = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("www-authenticate"));
									if (!string.IsNullOrEmpty(text2))
									{
										Digest orCreate2 = DigestStore.GetOrCreate(base.CurrentRequest.CurrentUri);
										orCreate2.ParseChallange(text2);
										if (base.CurrentRequest.Credentials != null && orCreate2.IsUriProtected(base.CurrentRequest.CurrentUri) && (!base.CurrentRequest.HasHeader("Authorization") || orCreate2.Stale))
										{
											retryCauses = RetryCauses.Authenticate;
										}
									}
								}
								this.TryStoreInCache();
								if (base.CurrentRequest.Response == null || !base.CurrentRequest.Response.IsClosedManually)
								{
									bool flag6 = base.CurrentRequest.Response == null || base.CurrentRequest.Response.HasHeaderWithValue("connection", "close");
									bool flag7 = !base.CurrentRequest.IsKeepAlive;
									if (flag6 || flag7)
									{
										this.Close();
									}
									else if (base.CurrentRequest.Response != null)
									{
										List<string> headerValues = base.CurrentRequest.Response.GetHeaderValues("keep-alive");
										if (headerValues != null && headerValues.Count > 0)
										{
											if (this.KeepAlive == null)
											{
												this.KeepAlive = new KeepAliveHeader();
											}
											this.KeepAlive.Parse(headerValues);
										}
									}
								}
							}
						}
						if (retryCauses == RetryCauses.None)
						{
							goto Block_45;
						}
					}
					throw new Exception("AbortRequested");
					IL_135:
					throw new Exception("AbortRequested");
					IL_434:
					throw new MissingFieldException(string.Format("Got redirect status({0}) without 'location' header!", base.CurrentRequest.Response.StatusCode.ToString()));
					Block_45:;
				}
			}
			catch (TimeoutException exception)
			{
				base.CurrentRequest.Response = null;
				base.CurrentRequest.Exception = exception;
				base.CurrentRequest.State = HTTPRequestStates.ConnectionTimedOut;
				this.Close();
			}
			catch (Exception exception2)
			{
				if (base.CurrentRequest != null)
				{
					if (base.CurrentRequest.UseStreaming)
					{
						HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri, true);
					}
					base.CurrentRequest.Response = null;
					switch (base.State)
					{
					case HTTPConnectionStates.AbortRequested:
					case HTTPConnectionStates.Closed:
						base.CurrentRequest.State = HTTPRequestStates.Aborted;
						break;
					case HTTPConnectionStates.TimedOut:
						base.CurrentRequest.State = HTTPRequestStates.TimedOut;
						break;
					default:
						base.CurrentRequest.Exception = exception2;
						base.CurrentRequest.State = HTTPRequestStates.Error;
						break;
					}
				}
				this.Close();
			}
			finally
			{
				if (base.CurrentRequest != null)
				{
					object locker = HTTPManager.Locker;
					lock (locker)
					{
						if (base.CurrentRequest != null && base.CurrentRequest.Response != null && base.CurrentRequest.Response.IsUpgraded)
						{
							base.State = HTTPConnectionStates.Upgraded;
						}
						else
						{
							base.State = (flag2 ? HTTPConnectionStates.Redirected : ((this.Client == null) ? HTTPConnectionStates.Closed : HTTPConnectionStates.WaitForRecycle));
						}
						if (base.CurrentRequest.State == HTTPRequestStates.Processing && (base.State == HTTPConnectionStates.Closed || base.State == HTTPConnectionStates.WaitForRecycle))
						{
							if (base.CurrentRequest.Response != null)
							{
								base.CurrentRequest.State = HTTPRequestStates.Finished;
							}
							else
							{
								base.CurrentRequest.Exception = new Exception(string.Format("Remote server closed the connection before sending response header! Previous request state: {0}. Connection state: {1}", base.CurrentRequest.State.ToString(), base.State.ToString()));
								base.CurrentRequest.State = HTTPRequestStates.Error;
							}
						}
						if (base.CurrentRequest.State == HTTPRequestStates.ConnectionTimedOut)
						{
							base.State = HTTPConnectionStates.Closed;
						}
						this.LastProcessTime = DateTime.UtcNow;
						if (this.OnConnectionRecycled != null)
						{
							base.RecycleNow();
						}
					}
					HTTPCacheService.SaveLibrary();
					CookieJar.Persist();
				}
			}
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x001002A8 File Offset: 0x000FE4A8
		private void Connect()
		{
			Uri uri = base.CurrentRequest.HasProxy ? base.CurrentRequest.Proxy.Address : base.CurrentRequest.CurrentUri;
			if (this.Client == null)
			{
				this.Client = new TcpClient();
			}
			if (!this.Client.Connected)
			{
				this.Client.ConnectTimeout = base.CurrentRequest.ConnectTimeout;
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPConnection", string.Format("'{0}' - Connecting to {1}:{2}", base.CurrentRequest.CurrentUri.ToString(), uri.Host, uri.Port.ToString()));
				}
				this.Client.Connect(uri.Host, uri.Port);
				if (HTTPManager.Logger.Level <= Loglevels.Information)
				{
					HTTPManager.Logger.Information("HTTPConnection", "Connected to " + uri.Host + ":" + uri.Port.ToString());
				}
			}
			else if (HTTPManager.Logger.Level <= Loglevels.Information)
			{
				HTTPManager.Logger.Information("HTTPConnection", "Already connected to " + uri.Host + ":" + uri.Port.ToString());
			}
			base.StartTime = DateTime.UtcNow;
			if (this.Stream == null)
			{
				bool flag = HTTPProtocolFactory.IsSecureProtocol(base.CurrentRequest.CurrentUri);
				this.Stream = this.Client.GetStream();
				if (base.HasProxy && (!base.Proxy.IsTransparent || (flag && base.Proxy.NonTransparentForHTTPS)))
				{
					BinaryWriter binaryWriter = new BinaryWriter(this.Stream);
					for (;;)
					{
						bool flag2 = false;
						string text = string.Format("CONNECT {0}:{1} HTTP/1.1", base.CurrentRequest.CurrentUri.Host, base.CurrentRequest.CurrentUri.Port);
						HTTPManager.Logger.Information("HTTPConnection", "Sending " + text);
						binaryWriter.SendAsASCII(text);
						binaryWriter.Write(HTTPRequest.EOL);
						binaryWriter.SendAsASCII("Proxy-Connection: Keep-Alive");
						binaryWriter.Write(HTTPRequest.EOL);
						binaryWriter.SendAsASCII("Connection: Keep-Alive");
						binaryWriter.Write(HTTPRequest.EOL);
						binaryWriter.SendAsASCII(string.Format("Host: {0}:{1}", base.CurrentRequest.CurrentUri.Host, base.CurrentRequest.CurrentUri.Port));
						binaryWriter.Write(HTTPRequest.EOL);
						if (base.HasProxy && base.Proxy.Credentials != null)
						{
							switch (base.Proxy.Credentials.Type)
							{
							case AuthenticationTypes.Unknown:
							case AuthenticationTypes.Digest:
							{
								Digest digest = DigestStore.Get(base.Proxy.Address);
								if (digest != null)
								{
									string text2 = digest.GenerateResponseHeader(base.CurrentRequest, base.Proxy.Credentials);
									if (!string.IsNullOrEmpty(text2))
									{
										binaryWriter.Write(string.Format("Proxy-Authorization: {0}", text2).GetASCIIBytes());
										binaryWriter.Write(HTTPRequest.EOL);
									}
								}
								break;
							}
							case AuthenticationTypes.Basic:
								binaryWriter.Write(string.Format("Proxy-Authorization: {0}", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(base.Proxy.Credentials.UserName + ":" + base.Proxy.Credentials.Password))).GetASCIIBytes());
								binaryWriter.Write(HTTPRequest.EOL);
								break;
							}
						}
						binaryWriter.Write(HTTPRequest.EOL);
						binaryWriter.Flush();
						base.CurrentRequest.ProxyResponse = new HTTPResponse(base.CurrentRequest, this.Stream, false, false);
						if (!base.CurrentRequest.ProxyResponse.Receive(-1, true))
						{
							break;
						}
						if (HTTPManager.Logger.Level <= Loglevels.Information)
						{
							HTTPManager.Logger.Information("HTTPConnection", string.Concat(new object[]
							{
								"Proxy returned - status code: ",
								base.CurrentRequest.ProxyResponse.StatusCode,
								" message: ",
								base.CurrentRequest.ProxyResponse.Message
							}));
						}
						int statusCode = base.CurrentRequest.ProxyResponse.StatusCode;
						if (statusCode == 407)
						{
							string text3 = DigestStore.FindBest(base.CurrentRequest.ProxyResponse.GetHeaderValues("proxy-authenticate"));
							if (!string.IsNullOrEmpty(text3))
							{
								Digest orCreate = DigestStore.GetOrCreate(base.Proxy.Address);
								orCreate.ParseChallange(text3);
								if (base.Proxy.Credentials != null && orCreate.IsUriProtected(base.Proxy.Address) && (!base.CurrentRequest.HasHeader("Proxy-Authorization") || orCreate.Stale))
								{
									flag2 = true;
								}
							}
						}
						else if (!base.CurrentRequest.ProxyResponse.IsSuccess)
						{
							goto Block_23;
						}
						if (!flag2)
						{
							goto IL_53A;
						}
					}
					throw new Exception("Connection to the Proxy Server failed!");
					Block_23:
					throw new Exception(string.Format("Proxy returned Status Code: \"{0}\", Message: \"{1}\" and Response: {2}", base.CurrentRequest.ProxyResponse.StatusCode, base.CurrentRequest.ProxyResponse.Message, base.CurrentRequest.ProxyResponse.DataAsText));
				}
				IL_53A:
				if (flag)
				{
					if (base.CurrentRequest.UseAlternateSSL)
					{
						TlsClientProtocol tlsClientProtocol = new TlsClientProtocol(this.Client.GetStream(), new SecureRandom());
						List<string> list = new List<string>(1);
						list.Add(base.CurrentRequest.CurrentUri.Host);
						TlsClientProtocol tlsClientProtocol2 = tlsClientProtocol;
						Uri currentUri = base.CurrentRequest.CurrentUri;
						ICertificateVerifyer verifyer;
						if (base.CurrentRequest.CustomCertificateVerifyer != null)
						{
							verifyer = base.CurrentRequest.CustomCertificateVerifyer;
						}
						else
						{
							ICertificateVerifyer certificateVerifyer = new AlwaysValidVerifyer();
							verifyer = certificateVerifyer;
						}
						tlsClientProtocol2.Connect(new LegacyTlsClient(currentUri, verifyer, base.CurrentRequest.CustomClientCredentialsProvider, list));
						this.Stream = tlsClientProtocol.Stream;
						return;
					}
					SslStream sslStream = new SslStream(this.Client.GetStream(), false, (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => base.CurrentRequest.CallCustomCertificationValidator(cert, chain));
					if (!sslStream.IsAuthenticated)
					{
						sslStream.AuthenticateAsClient(base.CurrentRequest.CurrentUri.Host);
					}
					this.Stream = sslStream;
				}
			}
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x001008D8 File Offset: 0x000FEAD8
		private bool Receive()
		{
			SupportedProtocols protocol = (base.CurrentRequest.ProtocolHandler == SupportedProtocols.Unknown) ? HTTPProtocolFactory.GetProtocolFromUri(base.CurrentRequest.CurrentUri) : base.CurrentRequest.ProtocolHandler;
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Receive - protocol: {1}", base.CurrentRequest.CurrentUri.ToString(), protocol.ToString()));
			}
			base.CurrentRequest.Response = HTTPProtocolFactory.Get(protocol, base.CurrentRequest, this.Stream, base.CurrentRequest.UseStreaming, false);
			if (!base.CurrentRequest.Response.Receive(-1, true))
			{
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Receive - Failed! Response will be null, returning with false.", base.CurrentRequest.CurrentUri.ToString()));
				}
				base.CurrentRequest.Response = null;
				return false;
			}
			if (base.CurrentRequest.Response.StatusCode == 304 && !base.CurrentRequest.DisableCache)
			{
				if (base.CurrentRequest.IsRedirected)
				{
					if (!this.LoadFromCache(base.CurrentRequest.RedirectUri))
					{
						this.LoadFromCache(base.CurrentRequest.Uri);
					}
				}
				else
				{
					this.LoadFromCache(base.CurrentRequest.Uri);
				}
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Receive - Finished Successfully!", base.CurrentRequest.CurrentUri.ToString()));
			}
			return true;
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x00100A74 File Offset: 0x000FEC74
		private bool LoadFromCache(Uri uri)
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - LoadFromCache for Uri: {1}", base.CurrentRequest.CurrentUri.ToString(), uri.ToString()));
			}
			HTTPCacheFileInfo entity = HTTPCacheService.GetEntity(uri);
			if (entity == null)
			{
				HTTPManager.Logger.Warning("HTTPConnection", string.Format("{0} - LoadFromCache for Uri: {1} - Cached entity not found!", base.CurrentRequest.CurrentUri.ToString(), uri.ToString()));
				return false;
			}
			base.CurrentRequest.Response.CacheFileInfo = entity;
			int num;
			using (Stream bodyStream = entity.GetBodyStream(out num))
			{
				if (bodyStream == null)
				{
					return false;
				}
				if (!base.CurrentRequest.Response.HasHeader("content-length"))
				{
					base.CurrentRequest.Response.Headers.Add("content-length", new List<string>(1)
					{
						num.ToString()
					});
				}
				base.CurrentRequest.Response.IsFromCache = true;
				if (!base.CurrentRequest.CacheOnly)
				{
					base.CurrentRequest.Response.ReadRaw(bodyStream, (long)num);
				}
			}
			return true;
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x00100BB0 File Offset: 0x000FEDB0
		private bool TryLoadAllFromCache()
		{
			if (base.CurrentRequest.DisableCache || !HTTPCacheService.IsSupported)
			{
				return false;
			}
			try
			{
				if (HTTPCacheService.IsCachedEntityExpiresInTheFuture(base.CurrentRequest))
				{
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - TryLoadAllFromCache - whole response loading from cache", base.CurrentRequest.CurrentUri.ToString()));
					}
					base.CurrentRequest.Response = HTTPCacheService.GetFullResponse(base.CurrentRequest);
					if (base.CurrentRequest.Response != null)
					{
						return true;
					}
				}
			}
			catch
			{
				HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri, true);
			}
			return false;
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x00100C68 File Offset: 0x000FEE68
		private void TryStoreInCache()
		{
			if (!base.CurrentRequest.UseStreaming && !base.CurrentRequest.DisableCache && base.CurrentRequest.Response != null && HTTPCacheService.IsSupported && HTTPCacheService.IsCacheble(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response))
			{
				if (base.CurrentRequest.IsRedirected)
				{
					HTTPCacheService.Store(base.CurrentRequest.Uri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
					return;
				}
				HTTPCacheService.Store(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
			}
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x00100D34 File Offset: 0x000FEF34
		private Uri GetRedirectUri(string location)
		{
			Uri uri = null;
			try
			{
				uri = new Uri(location);
				if (uri.IsFile || uri.AbsolutePath == location)
				{
					uri = null;
				}
			}
			catch (UriFormatException)
			{
				uri = null;
			}
			if (uri == null)
			{
				Uri uri2 = base.CurrentRequest.Uri;
				uri = new UriBuilder(uri2.Scheme, uri2.Host, uri2.Port, location).Uri;
			}
			return uri;
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x00100DB0 File Offset: 0x000FEFB0
		internal override void Abort(HTTPConnectionStates newState)
		{
			base.State = newState;
			HTTPConnectionStates state = base.State;
			if (state == HTTPConnectionStates.TimedOut)
			{
				base.TimedOutStart = DateTime.UtcNow;
			}
			if (this.Stream != null)
			{
				this.Stream.Dispose();
			}
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x00100DF0 File Offset: 0x000FEFF0
		private void Close()
		{
			this.KeepAlive = null;
			base.LastProcessedUri = null;
			if (this.Client != null)
			{
				try
				{
					this.Client.Close();
				}
				catch
				{
				}
				finally
				{
					this.Stream = null;
					this.Client = null;
				}
			}
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x00100E50 File Offset: 0x000FF050
		protected override void Dispose(bool disposing)
		{
			this.Close();
			base.Dispose(disposing);
		}

		// Token: 0x04002122 RID: 8482
		private TcpClient Client;

		// Token: 0x04002123 RID: 8483
		private Stream Stream;

		// Token: 0x04002124 RID: 8484
		private KeepAliveHeader KeepAlive;
	}
}
