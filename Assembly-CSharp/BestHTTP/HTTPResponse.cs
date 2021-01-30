using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x02000579 RID: 1401
	public class HTTPResponse : IDisposable
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x001033AB File Offset: 0x001015AB
		// (set) Token: 0x06003340 RID: 13120 RVA: 0x001033B3 File Offset: 0x001015B3
		public int VersionMajor { get; protected set; }

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x001033BC File Offset: 0x001015BC
		// (set) Token: 0x06003342 RID: 13122 RVA: 0x001033C4 File Offset: 0x001015C4
		public int VersionMinor { get; protected set; }

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06003343 RID: 13123 RVA: 0x001033CD File Offset: 0x001015CD
		// (set) Token: 0x06003344 RID: 13124 RVA: 0x001033D5 File Offset: 0x001015D5
		public int StatusCode { get; protected set; }

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06003345 RID: 13125 RVA: 0x001033DE File Offset: 0x001015DE
		public bool IsSuccess
		{
			get
			{
				return (this.StatusCode >= 200 && this.StatusCode < 300) || this.StatusCode == 304;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06003346 RID: 13126 RVA: 0x00103409 File Offset: 0x00101609
		// (set) Token: 0x06003347 RID: 13127 RVA: 0x00103411 File Offset: 0x00101611
		public string Message { get; protected set; }

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x0010341A File Offset: 0x0010161A
		// (set) Token: 0x06003349 RID: 13129 RVA: 0x00103422 File Offset: 0x00101622
		public bool IsStreamed { get; protected set; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x0010342B File Offset: 0x0010162B
		// (set) Token: 0x0600334B RID: 13131 RVA: 0x00103433 File Offset: 0x00101633
		public bool IsStreamingFinished { get; internal set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x0010343C File Offset: 0x0010163C
		// (set) Token: 0x0600334D RID: 13133 RVA: 0x00103444 File Offset: 0x00101644
		public bool IsFromCache { get; internal set; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x0010344D File Offset: 0x0010164D
		// (set) Token: 0x0600334F RID: 13135 RVA: 0x00103455 File Offset: 0x00101655
		public HTTPCacheFileInfo CacheFileInfo { get; internal set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x0010345E File Offset: 0x0010165E
		// (set) Token: 0x06003351 RID: 13137 RVA: 0x00103466 File Offset: 0x00101666
		public bool IsCacheOnly { get; private set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06003352 RID: 13138 RVA: 0x0010346F File Offset: 0x0010166F
		// (set) Token: 0x06003353 RID: 13139 RVA: 0x00103477 File Offset: 0x00101677
		public Dictionary<string, List<string>> Headers { get; protected set; }

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x00103480 File Offset: 0x00101680
		// (set) Token: 0x06003355 RID: 13141 RVA: 0x00103488 File Offset: 0x00101688
		public byte[] Data { get; internal set; }

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06003356 RID: 13142 RVA: 0x00103491 File Offset: 0x00101691
		// (set) Token: 0x06003357 RID: 13143 RVA: 0x00103499 File Offset: 0x00101699
		public bool IsUpgraded { get; protected set; }

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06003358 RID: 13144 RVA: 0x001034A2 File Offset: 0x001016A2
		// (set) Token: 0x06003359 RID: 13145 RVA: 0x001034AA File Offset: 0x001016AA
		public List<Cookie> Cookies { get; internal set; }

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600335A RID: 13146 RVA: 0x001034B4 File Offset: 0x001016B4
		public string DataAsText
		{
			get
			{
				if (this.Data == null)
				{
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(this.dataAsText))
				{
					return this.dataAsText;
				}
				return this.dataAsText = Encoding.UTF8.GetString(this.Data, 0, this.Data.Length);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600335B RID: 13147 RVA: 0x00103508 File Offset: 0x00101708
		public Texture2D DataAsTexture2D
		{
			get
			{
				if (this.Data == null)
				{
					return null;
				}
				if (this.texture != null)
				{
					return this.texture;
				}
				this.texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
				this.texture.LoadImage(this.Data);
				return this.texture;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x0600335C RID: 13148 RVA: 0x0010355B File Offset: 0x0010175B
		// (set) Token: 0x0600335D RID: 13149 RVA: 0x00103563 File Offset: 0x00101763
		public bool IsClosedManually { get; protected set; }

		// Token: 0x0600335E RID: 13150 RVA: 0x0010356C File Offset: 0x0010176C
		internal HTTPResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
		{
			this.baseRequest = request;
			this.Stream = stream;
			this.IsStreamed = isStreamed;
			this.IsFromCache = isFromCache;
			this.IsCacheOnly = request.CacheOnly;
			this.IsClosedManually = false;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x001035BC File Offset: 0x001017BC
		internal virtual bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
		{
			string text = string.Empty;
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("Receive. forceReadRawContentLength: '{0:N0}', readPayloadData: '{1:N0}'", forceReadRawContentLength, readPayloadData));
			}
			try
			{
				text = HTTPResponse.ReadTo(this.Stream, 32);
			}
			catch
			{
				if (!this.baseRequest.DisableRetry)
				{
					HTTPManager.Logger.Warning("HTTPResponse", string.Format("{0} - Failed to read Status Line! Retry is enabled, returning with false.", this.baseRequest.CurrentUri.ToString()));
					return false;
				}
				HTTPManager.Logger.Warning("HTTPResponse", string.Format("{0} - Failed to read Status Line! Retry is disabled, re-throwing exception.", this.baseRequest.CurrentUri.ToString()));
				throw;
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("Status Line: '{0}'", text));
			}
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[]
				{
					'/',
					'.'
				});
				this.VersionMajor = int.Parse(array[1]);
				this.VersionMinor = int.Parse(array[2]);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("HTTP Version: '{0}.{1}'", this.VersionMajor.ToString(), this.VersionMinor.ToString()));
				}
				string text2 = HTTPResponse.NoTrimReadTo(this.Stream, 32, 10);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("Status Code: '{0}'", text2));
				}
				int statusCode;
				if (this.baseRequest.DisableRetry)
				{
					statusCode = int.Parse(text2);
				}
				else if (!int.TryParse(text2, out statusCode))
				{
					return false;
				}
				this.StatusCode = statusCode;
				if (text2.Length > 0 && (byte)text2[text2.Length - 1] != 10 && (byte)text2[text2.Length - 1] != 13)
				{
					this.Message = HTTPResponse.ReadTo(this.Stream, 10);
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging(string.Format("Status Message: '{0}'", this.Message));
					}
				}
				else
				{
					HTTPManager.Logger.Warning("HTTPResponse", string.Format("{0} - Skipping Status Message reading!", this.baseRequest.CurrentUri.ToString()));
					this.Message = string.Empty;
				}
				this.ReadHeaders(this.Stream);
				this.IsUpgraded = (this.StatusCode == 101 && (this.HasHeaderWithValue("connection", "upgrade") || this.HasHeader("upgrade")));
				if (this.IsUpgraded && HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging("Request Upgraded!");
				}
				return !readPayloadData || this.ReadPayload(forceReadRawContentLength);
			}
			if (!this.baseRequest.DisableRetry)
			{
				return false;
			}
			throw new Exception("Remote server closed the connection before sending response header!");
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x0010388C File Offset: 0x00101A8C
		protected bool ReadPayload(int forceReadRawContentLength)
		{
			if (forceReadRawContentLength != -1)
			{
				this.IsFromCache = true;
				this.ReadRaw(this.Stream, (long)forceReadRawContentLength);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging("ReadPayload Finished!");
				}
				return true;
			}
			if ((this.StatusCode >= 100 && this.StatusCode < 200) || this.StatusCode == 204 || this.StatusCode == 304 || this.baseRequest.MethodType == HTTPMethods.Head)
			{
				return true;
			}
			if (this.HasHeaderWithValue("transfer-encoding", "chunked"))
			{
				this.ReadChunked(this.Stream);
			}
			else
			{
				List<string> headerValues = this.GetHeaderValues("content-length");
				List<string> headerValues2 = this.GetHeaderValues("content-range");
				if (headerValues != null && headerValues2 == null)
				{
					this.ReadRaw(this.Stream, long.Parse(headerValues[0]));
				}
				else if (headerValues2 != null)
				{
					if (headerValues != null)
					{
						this.ReadRaw(this.Stream, long.Parse(headerValues[0]));
					}
					else
					{
						HTTPRange range = this.GetRange();
						this.ReadRaw(this.Stream, (long)(range.LastBytePos - range.FirstBytePos + 1));
					}
				}
				else
				{
					this.ReadUnknownSize(this.Stream);
				}
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging("ReadPayload Finished!");
			}
			return true;
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x001039D0 File Offset: 0x00101BD0
		protected void ReadHeaders(Stream stream)
		{
			string text = HTTPResponse.ReadTo(stream, 58, 10).Trim();
			while (text != string.Empty)
			{
				string text2 = HTTPResponse.ReadTo(stream, 10);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("Header - '{0}': '{1}'", text, text2));
				}
				this.AddHeader(text, text2);
				text = HTTPResponse.ReadTo(stream, 58, 10);
			}
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x00103A38 File Offset: 0x00101C38
		protected void AddHeader(string name, string value)
		{
			name = name.ToLower();
			if (this.Headers == null)
			{
				this.Headers = new Dictionary<string, List<string>>();
			}
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list))
			{
				this.Headers.Add(name, list = new List<string>(1));
			}
			list.Add(value);
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x00103A8C File Offset: 0x00101C8C
		public List<string> GetHeaderValues(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			name = name.ToLower();
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list) || list.Count == 0)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x00103AC8 File Offset: 0x00101CC8
		public string GetFirstHeaderValue(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			name = name.ToLower();
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list) || list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x00103B08 File Offset: 0x00101D08
		public bool HasHeaderWithValue(string headerName, string value)
		{
			List<string> headerValues = this.GetHeaderValues(headerName);
			if (headerValues == null)
			{
				return false;
			}
			for (int i = 0; i < headerValues.Count; i++)
			{
				if (string.Compare(headerValues[i], value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x00103B46 File Offset: 0x00101D46
		public bool HasHeader(string headerName)
		{
			return this.GetHeaderValues(headerName) != null;
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x00103B54 File Offset: 0x00101D54
		public HTTPRange GetRange()
		{
			List<string> headerValues = this.GetHeaderValues("content-range");
			if (headerValues == null)
			{
				return null;
			}
			string[] array = headerValues[0].Split(new char[]
			{
				' ',
				'-',
				'/'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array[1] == "*")
			{
				return new HTTPRange(int.Parse(array[2]));
			}
			return new HTTPRange(int.Parse(array[1]), int.Parse(array[2]), (array[3] != "*") ? int.Parse(array[3]) : -1);
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x00103BE0 File Offset: 0x00101DE0
		public static string ReadTo(Stream stream, byte blocker)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = stream.ReadByte();
				while (num != (int)blocker && num != -1)
				{
					memoryStream.WriteByte((byte)num);
					num = stream.ReadByte();
				}
				result = memoryStream.ToArray().AsciiToString().Trim();
			}
			return result;
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x00103C44 File Offset: 0x00101E44
		public static string ReadTo(Stream stream, byte blocker1, byte blocker2)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = stream.ReadByte();
				while (num != (int)blocker1 && num != (int)blocker2 && num != -1)
				{
					memoryStream.WriteByte((byte)num);
					num = stream.ReadByte();
				}
				result = memoryStream.ToArray().AsciiToString().Trim();
			}
			return result;
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x00103CAC File Offset: 0x00101EAC
		public static string NoTrimReadTo(Stream stream, byte blocker1, byte blocker2)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = stream.ReadByte();
				while (num != (int)blocker1 && num != (int)blocker2 && num != -1)
				{
					memoryStream.WriteByte((byte)num);
					num = stream.ReadByte();
				}
				result = memoryStream.ToArray().AsciiToString();
			}
			return result;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x00103D0C File Offset: 0x00101F0C
		protected int ReadChunkLength(Stream stream)
		{
			string text = HTTPResponse.ReadTo(stream, 10).Split(new char[]
			{
				';'
			})[0];
			int result;
			if (int.TryParse(text, NumberStyles.AllowHexSpecifier, null, out result))
			{
				return result;
			}
			throw new Exception(string.Format("Can't parse '{0}' as a hex number!", text));
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x00103D58 File Offset: 0x00101F58
		protected void ReadChunked(Stream stream)
		{
			this.BeginReceiveStreamFragments();
			string firstHeaderValue = this.GetFirstHeaderValue("Content-Length");
			bool flag = !string.IsNullOrEmpty(firstHeaderValue);
			int num = 0;
			if (flag)
			{
				flag = int.TryParse(firstHeaderValue, out num);
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("ReadChunked - hasContentLengthHeader: {0}, contentLengthHeader: {1} realLength: {2:N0}", flag.ToString(), firstHeaderValue, num));
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num2 = this.ReadChunkLength(stream);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("chunkLength: {0:N0}", num2));
				}
				byte[] array = new byte[num2];
				int num3 = 0;
				this.baseRequest.DownloadLength = (long)(flag ? num : num2);
				this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
				string text = this.IsFromCache ? null : this.GetFirstHeaderValue("content-encoding");
				bool flag2 = !string.IsNullOrEmpty(text) && text == "gzip";
				while (num2 != 0)
				{
					if (array.Length < num2)
					{
						Array.Resize<byte>(ref array, num2);
					}
					int num4 = 0;
					do
					{
						int num5 = stream.Read(array, num4, num2 - num4);
						if (num5 <= 0)
						{
							goto Block_11;
						}
						num4 += num5;
						this.baseRequest.Downloaded += (long)num5;
						this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					}
					while (num4 < num2);
					if (this.baseRequest.UseStreaming)
					{
						this.WaitWhileHasFragments();
						if (flag2)
						{
							byte[] array2 = this.Decompress(array, 0, num4);
							this.FeedStreamFragment(array2, 0, array2.Length);
						}
						else
						{
							this.FeedStreamFragment(array, 0, num4);
						}
					}
					else
					{
						memoryStream.Write(array, 0, num4);
					}
					HTTPResponse.ReadTo(stream, 10);
					num3 += num4;
					num2 = this.ReadChunkLength(stream);
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging(string.Format("chunkLength: {0:N0}", num2));
					}
					if (!flag)
					{
						this.baseRequest.DownloadLength += (long)num2;
					}
					this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					continue;
					Block_11:
					throw ExceptionHelper.ServerClosedTCPStream();
				}
				if (this.baseRequest.UseStreaming)
				{
					this.FlushRemainingFragmentBuffer();
				}
				this.ReadHeaders(stream);
				if (!this.baseRequest.UseStreaming)
				{
					this.Data = this.DecodeStream(memoryStream);
				}
			}
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x00103FEC File Offset: 0x001021EC
		internal void ReadRaw(Stream stream, long contentLength)
		{
			this.BeginReceiveStreamFragments();
			this.baseRequest.DownloadLength = contentLength;
			this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("ReadRaw - contentLength: {0:N0}", contentLength));
			}
			string text = this.IsFromCache ? null : this.GetFirstHeaderValue("content-encoding");
			bool flag = !string.IsNullOrEmpty(text) && text == "gzip";
			if (!this.baseRequest.UseStreaming && contentLength > 2147483646L)
			{
				throw new OverflowException("You have to use STREAMING to download files bigger than 2GB!");
			}
			using (MemoryStream memoryStream = new MemoryStream(this.baseRequest.UseStreaming ? 0 : ((int)contentLength)))
			{
				byte[] array = new byte[Math.Max(this.baseRequest.StreamFragmentSize, 4096)];
				while (contentLength > 0L)
				{
					int num = 0;
					do
					{
						int val = (int)Math.Min(2147483646U, (uint)contentLength);
						int num2 = stream.Read(array, num, Math.Min(val, array.Length - num));
						if (num2 <= 0)
						{
							goto Block_10;
						}
						num += num2;
						contentLength -= (long)num2;
						this.baseRequest.Downloaded += (long)num2;
						this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					}
					while (num < array.Length && contentLength > 0L);
					if (!this.baseRequest.UseStreaming)
					{
						memoryStream.Write(array, 0, num);
						continue;
					}
					this.WaitWhileHasFragments();
					if (flag)
					{
						byte[] array2 = this.Decompress(array, 0, num);
						this.FeedStreamFragment(array2, 0, array2.Length);
						continue;
					}
					this.FeedStreamFragment(array, 0, num);
					continue;
					Block_10:
					throw ExceptionHelper.ServerClosedTCPStream();
				}
				if (this.baseRequest.UseStreaming)
				{
					this.FlushRemainingFragmentBuffer();
				}
				if (!this.baseRequest.UseStreaming)
				{
					this.Data = this.DecodeStream(memoryStream);
				}
			}
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x001041F8 File Offset: 0x001023F8
		protected void ReadUnknownSize(Stream stream)
		{
			string text = this.IsFromCache ? null : this.GetFirstHeaderValue("content-encoding");
			bool flag = !string.IsNullOrEmpty(text) && text == "gzip";
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] array = new byte[Math.Max(this.baseRequest.StreamFragmentSize, 4096)];
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("ReadUnknownSize - buffer size: {0:N0}", array.Length));
				}
				int num2;
				do
				{
					int num = 0;
					do
					{
						num2 = 0;
						NetworkStream networkStream = stream as NetworkStream;
						if (networkStream != null && this.baseRequest.EnableSafeReadOnUnknownContentLength)
						{
							for (int i = num; i < array.Length; i++)
							{
								if (!networkStream.DataAvailable)
								{
									break;
								}
								int num3 = stream.ReadByte();
								if (num3 < 0)
								{
									break;
								}
								array[i] = (byte)num3;
								num2++;
							}
						}
						else
						{
							num2 = stream.Read(array, num, array.Length - num);
						}
						num += num2;
						this.baseRequest.Downloaded += (long)num2;
						this.baseRequest.DownloadLength = this.baseRequest.Downloaded;
						this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					}
					while (num < array.Length && num2 > 0);
					if (this.baseRequest.UseStreaming)
					{
						this.WaitWhileHasFragments();
						if (flag)
						{
							byte[] array2 = this.Decompress(array, 0, num);
							this.FeedStreamFragment(array2, 0, array2.Length);
						}
						else
						{
							this.FeedStreamFragment(array, 0, num);
						}
					}
					else
					{
						memoryStream.Write(array, 0, num);
					}
				}
				while (num2 > 0);
				if (this.baseRequest.UseStreaming)
				{
					this.FlushRemainingFragmentBuffer();
				}
				if (!this.baseRequest.UseStreaming)
				{
					this.Data = this.DecodeStream(memoryStream);
				}
			}
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x001043E8 File Offset: 0x001025E8
		protected byte[] DecodeStream(MemoryStream streamToDecode)
		{
			streamToDecode.Seek(0L, SeekOrigin.Begin);
			List<string> list = this.IsFromCache ? null : this.GetHeaderValues("content-encoding");
			if (list == null)
			{
				return streamToDecode.ToArray();
			}
			string a = list[0];
			Stream stream;
			if (!(a == "gzip"))
			{
				if (!(a == "deflate"))
				{
					return streamToDecode.ToArray();
				}
				stream = new DeflateStream(streamToDecode, CompressionMode.Decompress);
			}
			else
			{
				stream = new GZipStream(streamToDecode, CompressionMode.Decompress);
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream((int)streamToDecode.Length))
			{
				byte[] array = new byte[1024];
				int count;
				while ((count = stream.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x001044C4 File Offset: 0x001026C4
		private byte[] Decompress(byte[] data, int offset, int count)
		{
			if (this.decompressorInputStream == null)
			{
				this.decompressorInputStream = new MemoryStream(count);
			}
			this.decompressorInputStream.Write(data, offset, count);
			this.decompressorInputStream.Position = 0L;
			if (this.decompressorGZipStream == null)
			{
				this.decompressorGZipStream = new GZipStream(this.decompressorInputStream, CompressionMode.Decompress, BestHTTP.Decompression.Zlib.CompressionLevel.Default, true);
				this.decompressorGZipStream.FlushMode = FlushType.Sync;
			}
			if (this.decompressorOutputStream == null)
			{
				this.decompressorOutputStream = new MemoryStream();
			}
			this.decompressorOutputStream.SetLength(0L);
			if (this.copyBuffer == null)
			{
				this.copyBuffer = new byte[1024];
			}
			int count2;
			while ((count2 = this.decompressorGZipStream.Read(this.copyBuffer, 0, this.copyBuffer.Length)) != 0)
			{
				this.decompressorOutputStream.Write(this.copyBuffer, 0, count2);
			}
			this.decompressorGZipStream.SetLength(0L);
			return this.decompressorOutputStream.ToArray();
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x001045AC File Offset: 0x001027AC
		protected void BeginReceiveStreamFragments()
		{
			if (!this.baseRequest.DisableCache && this.baseRequest.UseStreaming && !this.IsFromCache && HTTPCacheService.IsCacheble(this.baseRequest.CurrentUri, this.baseRequest.MethodType, this))
			{
				this.cacheStream = HTTPCacheService.PrepareStreamed(this.baseRequest.CurrentUri, this);
			}
			this.allFragmentSize = 0;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x00104618 File Offset: 0x00102818
		protected void FeedStreamFragment(byte[] buffer, int pos, int length)
		{
			if (this.fragmentBuffer == null)
			{
				this.fragmentBuffer = new byte[this.baseRequest.StreamFragmentSize];
				this.fragmentBufferDataLength = 0;
			}
			if (this.fragmentBufferDataLength + length <= this.baseRequest.StreamFragmentSize)
			{
				Array.Copy(buffer, pos, this.fragmentBuffer, this.fragmentBufferDataLength, length);
				this.fragmentBufferDataLength += length;
				if (this.fragmentBufferDataLength == this.baseRequest.StreamFragmentSize)
				{
					this.AddStreamedFragment(this.fragmentBuffer);
					this.fragmentBuffer = null;
					this.fragmentBufferDataLength = 0;
					return;
				}
			}
			else
			{
				int num = this.baseRequest.StreamFragmentSize - this.fragmentBufferDataLength;
				this.FeedStreamFragment(buffer, pos, num);
				this.FeedStreamFragment(buffer, pos + num, length - num);
			}
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x001046D8 File Offset: 0x001028D8
		protected void FlushRemainingFragmentBuffer()
		{
			if (this.fragmentBuffer != null)
			{
				Array.Resize<byte>(ref this.fragmentBuffer, this.fragmentBufferDataLength);
				this.AddStreamedFragment(this.fragmentBuffer);
				this.fragmentBuffer = null;
				this.fragmentBufferDataLength = 0;
			}
			if (this.cacheStream != null)
			{
				this.cacheStream.Dispose();
				this.cacheStream = null;
				HTTPCacheService.SetBodyLength(this.baseRequest.CurrentUri, this.allFragmentSize);
			}
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00104748 File Offset: 0x00102948
		protected void AddStreamedFragment(byte[] buffer)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				if (!this.IsCacheOnly)
				{
					if (this.streamedFragments == null)
					{
						this.streamedFragments = new List<byte[]>();
					}
					this.streamedFragments.Add(buffer);
				}
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("AddStreamedFragment buffer length: {0:N0} streamedFragments: {1:N0}", buffer.Length, this.streamedFragments.Count));
				}
				if (this.cacheStream != null)
				{
					this.cacheStream.Write(buffer, 0, buffer.Length);
					this.allFragmentSize += buffer.Length;
				}
			}
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x00104808 File Offset: 0x00102A08
		protected void WaitWhileHasFragments()
		{
			while (this.baseRequest.UseStreaming && this.HasStreamedFragments())
			{
				Thread.Sleep(16);
			}
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00104828 File Offset: 0x00102A28
		public List<byte[]> GetStreamedFragments()
		{
			object syncRoot = this.SyncRoot;
			List<byte[]> result;
			lock (syncRoot)
			{
				if (this.streamedFragments == null || this.streamedFragments.Count == 0)
				{
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging("GetStreamedFragments - no fragments, returning with null");
					}
					result = null;
				}
				else
				{
					List<byte[]> list = new List<byte[]>(this.streamedFragments);
					this.streamedFragments.Clear();
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging(string.Format("GetStreamedFragments - returning with {0:N0} fragments", list.Count.ToString()));
					}
					result = list;
				}
			}
			return result;
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x001048D8 File Offset: 0x00102AD8
		internal bool HasStreamedFragments()
		{
			object syncRoot = this.SyncRoot;
			bool result;
			lock (syncRoot)
			{
				result = (this.streamedFragments != null && this.streamedFragments.Count > 0);
			}
			return result;
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x00104930 File Offset: 0x00102B30
		internal void FinishStreaming()
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging("FinishStreaming");
			}
			this.IsStreamingFinished = true;
			this.Dispose();
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x00104956 File Offset: 0x00102B56
		private void VerboseLogging(string str)
		{
			HTTPManager.Logger.Verbose("HTTPResponse", "'" + this.baseRequest.CurrentUri.ToString() + "' - " + str);
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x00104987 File Offset: 0x00102B87
		public void Dispose()
		{
			if (this.cacheStream != null)
			{
				this.cacheStream.Dispose();
				this.cacheStream = null;
			}
		}

		// Token: 0x040021A1 RID: 8609
		internal const byte CR = 13;

		// Token: 0x040021A2 RID: 8610
		internal const byte LF = 10;

		// Token: 0x040021A3 RID: 8611
		public const int MinBufferSize = 4096;

		// Token: 0x040021B1 RID: 8625
		protected string dataAsText;

		// Token: 0x040021B2 RID: 8626
		protected Texture2D texture;

		// Token: 0x040021B4 RID: 8628
		internal HTTPRequest baseRequest;

		// Token: 0x040021B5 RID: 8629
		protected Stream Stream;

		// Token: 0x040021B6 RID: 8630
		protected List<byte[]> streamedFragments;

		// Token: 0x040021B7 RID: 8631
		protected object SyncRoot = new object();

		// Token: 0x040021B8 RID: 8632
		protected byte[] fragmentBuffer;

		// Token: 0x040021B9 RID: 8633
		protected int fragmentBufferDataLength;

		// Token: 0x040021BA RID: 8634
		protected Stream cacheStream;

		// Token: 0x040021BB RID: 8635
		protected int allFragmentSize;

		// Token: 0x040021BC RID: 8636
		private MemoryStream decompressorInputStream;

		// Token: 0x040021BD RID: 8637
		private MemoryStream decompressorOutputStream;

		// Token: 0x040021BE RID: 8638
		private GZipStream decompressorGZipStream;

		// Token: 0x040021BF RID: 8639
		private byte[] copyBuffer;
	}
}
