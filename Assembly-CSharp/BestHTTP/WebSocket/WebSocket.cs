using System;
using System.IO;
using System.Text;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket
{
	// Token: 0x02000589 RID: 1417
	public sealed class WebSocket
	{
		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060033CD RID: 13261 RVA: 0x0010597B File Offset: 0x00103B7B
		public bool IsOpen
		{
			get
			{
				return this.webSocket != null && !this.webSocket.IsClosed;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060033CE RID: 13262 RVA: 0x00105995 File Offset: 0x00103B95
		public int BufferedAmount
		{
			get
			{
				return this.webSocket.BufferedAmount;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060033CF RID: 13263 RVA: 0x001059A2 File Offset: 0x00103BA2
		// (set) Token: 0x060033D0 RID: 13264 RVA: 0x001059AA File Offset: 0x00103BAA
		public bool StartPingThread { get; set; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060033D1 RID: 13265 RVA: 0x001059B3 File Offset: 0x00103BB3
		// (set) Token: 0x060033D2 RID: 13266 RVA: 0x001059BB File Offset: 0x00103BBB
		public int PingFrequency { get; set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x001059C4 File Offset: 0x00103BC4
		// (set) Token: 0x060033D4 RID: 13268 RVA: 0x001059CC File Offset: 0x00103BCC
		public TimeSpan CloseAfterNoMesssage { get; set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060033D5 RID: 13269 RVA: 0x001059D5 File Offset: 0x00103BD5
		// (set) Token: 0x060033D6 RID: 13270 RVA: 0x001059DD File Offset: 0x00103BDD
		public HTTPRequest InternalRequest { get; private set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060033D7 RID: 13271 RVA: 0x001059E6 File Offset: 0x00103BE6
		// (set) Token: 0x060033D8 RID: 13272 RVA: 0x001059EE File Offset: 0x00103BEE
		public IExtension[] Extensions { get; private set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x001059F7 File Offset: 0x00103BF7
		public int Latency
		{
			get
			{
				return this.webSocket.Latency;
			}
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x00105A04 File Offset: 0x00103C04
		public WebSocket(Uri uri) : this(uri, string.Empty, string.Empty, Array.Empty<IExtension>())
		{
			this.Extensions = new IExtension[]
			{
				new PerMessageCompression(CompressionLevel.Default, false, false, 15, 15, 5)
			};
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x00105A44 File Offset: 0x00103C44
		public WebSocket(Uri uri, string origin, string protocol, params IExtension[] extensions)
		{
			this.PingFrequency = 1000;
			this.CloseAfterNoMesssage = TimeSpan.FromSeconds(10.0);
			if (uri.Port == -1)
			{
				uri = new Uri(string.Concat(new string[]
				{
					uri.Scheme,
					"://",
					uri.Host,
					":",
					uri.Scheme.Equals("wss", StringComparison.OrdinalIgnoreCase) ? "443" : "80",
					uri.GetRequestPathAndQueryURL()
				}));
			}
			this.InternalRequest = new HTTPRequest(uri, new OnRequestFinishedDelegate(this.OnInternalRequestCallback));
			this.InternalRequest.OnUpgraded = new OnRequestFinishedDelegate(this.OnInternalRequestUpgraded);
			if (uri.Port != 80)
			{
				this.InternalRequest.SetHeader("Host", uri.Host + ":" + uri.Port);
			}
			else
			{
				this.InternalRequest.SetHeader("Host", uri.Host);
			}
			this.InternalRequest.SetHeader("Upgrade", "websocket");
			this.InternalRequest.SetHeader("Connection", "keep-alive, Upgrade");
			this.InternalRequest.SetHeader("Sec-WebSocket-Key", this.GetSecKey(new object[]
			{
				this,
				this.InternalRequest,
				uri,
				new object()
			}));
			if (!string.IsNullOrEmpty(origin))
			{
				this.InternalRequest.SetHeader("Origin", origin);
			}
			this.InternalRequest.SetHeader("Sec-WebSocket-Version", "13");
			if (!string.IsNullOrEmpty(protocol))
			{
				this.InternalRequest.SetHeader("Sec-WebSocket-Protocol", protocol);
			}
			this.InternalRequest.SetHeader("Cache-Control", "no-cache");
			this.InternalRequest.SetHeader("Pragma", "no-cache");
			this.Extensions = extensions;
			this.InternalRequest.DisableCache = true;
			this.InternalRequest.DisableRetry = true;
			if (HTTPManager.Proxy != null)
			{
				this.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false, false, HTTPManager.Proxy.NonTransparentForHTTPS);
			}
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x00105C84 File Offset: 0x00103E84
		private void OnInternalRequestCallback(HTTPRequest req, HTTPResponse resp)
		{
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess || resp.StatusCode == 101)
				{
					HTTPManager.Logger.Information("WebSocket", string.Format("Request finished. Status Code: {0} Message: {1}", resp.StatusCode.ToString(), resp.Message));
					return;
				}
				text = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				break;
			case HTTPRequestStates.Error:
				text = "Request Finished with Error! " + ((req.Exception != null) ? ("Exception: " + req.Exception.Message + req.Exception.StackTrace) : string.Empty);
				break;
			case HTTPRequestStates.Aborted:
				text = "Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Processing the request Timed Out!";
				break;
			default:
				return;
			}
			if (this.OnError != null)
			{
				this.OnError(this, req.Exception);
			}
			if (this.OnErrorDesc != null)
			{
				this.OnErrorDesc(this, text);
			}
			if (this.OnError == null && this.OnErrorDesc == null)
			{
				HTTPManager.Logger.Error("WebSocket", text);
			}
			if (!req.IsKeepAlive && resp != null && resp is WebSocketResponse)
			{
				(resp as WebSocketResponse).CloseStream();
			}
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x00105DE0 File Offset: 0x00103FE0
		private void OnInternalRequestUpgraded(HTTPRequest req, HTTPResponse resp)
		{
			this.webSocket = (resp as WebSocketResponse);
			if (this.webSocket == null)
			{
				if (this.OnError != null)
				{
					this.OnError(this, req.Exception);
				}
				if (this.OnErrorDesc != null)
				{
					string reason = string.Empty;
					if (req.Exception != null)
					{
						reason = req.Exception.Message + " " + req.Exception.StackTrace;
					}
					this.OnErrorDesc(this, reason);
				}
				return;
			}
			this.webSocket.WebSocket = this;
			if (this.Extensions != null)
			{
				for (int i = 0; i < this.Extensions.Length; i++)
				{
					IExtension extension = this.Extensions[i];
					try
					{
						if (extension != null && !extension.ParseNegotiation(this.webSocket))
						{
							this.Extensions[i] = null;
						}
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("WebSocket", "ParseNegotiation", ex);
						this.Extensions[i] = null;
					}
				}
			}
			if (this.OnOpen != null)
			{
				try
				{
					this.OnOpen(this);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("WebSocket", "OnOpen", ex2);
				}
			}
			this.webSocket.OnText = delegate(WebSocketResponse ws, string msg)
			{
				if (this.OnMessage != null)
				{
					this.OnMessage(this, msg);
				}
			};
			this.webSocket.OnBinary = delegate(WebSocketResponse ws, byte[] bin)
			{
				if (this.OnBinary != null)
				{
					this.OnBinary(this, bin);
				}
			};
			this.webSocket.OnClosed = delegate(WebSocketResponse ws, ushort code, string msg)
			{
				if (this.OnClosed != null)
				{
					this.OnClosed(this, code, msg);
				}
			};
			if (this.OnIncompleteFrame != null)
			{
				this.webSocket.OnIncompleteFrame = delegate(WebSocketResponse ws, WebSocketFrameReader frame)
				{
					if (this.OnIncompleteFrame != null)
					{
						this.OnIncompleteFrame(this, frame);
					}
				};
			}
			if (this.StartPingThread)
			{
				this.webSocket.StartPinging(Math.Max(this.PingFrequency, 100));
			}
			this.webSocket.StartReceive();
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x00105FA8 File Offset: 0x001041A8
		public void Open()
		{
			if (this.requestSent)
			{
				throw new InvalidOperationException("Open already called! You can't reuse this WebSocket instance!");
			}
			if (this.Extensions != null)
			{
				try
				{
					for (int i = 0; i < this.Extensions.Length; i++)
					{
						IExtension extension = this.Extensions[i];
						if (extension != null)
						{
							extension.AddNegotiation(this.InternalRequest);
						}
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("WebSocket", "Open", ex);
				}
			}
			this.InternalRequest.Send();
			this.requestSent = true;
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x00106038 File Offset: 0x00104238
		public void Send(string message)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Send(message);
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x0010604F File Offset: 0x0010424F
		public void Send(byte[] buffer)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Send(buffer);
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x00106066 File Offset: 0x00104266
		public void Send(byte[] buffer, ulong offset, ulong count)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Send(buffer, offset, count);
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x0010607F File Offset: 0x0010427F
		public void Send(WebSocketFrame frame)
		{
			if (this.IsOpen)
			{
				this.webSocket.Send(frame);
			}
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x00106095 File Offset: 0x00104295
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Close();
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x001060AB File Offset: 0x001042AB
		public void Close(ushort code, string message)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Close(code, message);
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x001060C4 File Offset: 0x001042C4
		public static byte[] EncodeCloseData(ushort code, string message)
		{
			int byteCount = Encoding.UTF8.GetByteCount(message);
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(2 + byteCount))
			{
				byte[] bytes = BitConverter.GetBytes(code);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(bytes, 0, bytes.Length);
				}
				memoryStream.Write(bytes, 0, bytes.Length);
				bytes = Encoding.UTF8.GetBytes(message);
				memoryStream.Write(bytes, 0, bytes.Length);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x00106144 File Offset: 0x00104344
		private string GetSecKey(object[] from)
		{
			byte[] array = new byte[16];
			int num = 0;
			for (int i = 0; i < from.Length; i++)
			{
				byte[] bytes = BitConverter.GetBytes(from[i].GetHashCode());
				int num2 = 0;
				while (num2 < bytes.Length && num < array.Length)
				{
					array[num++] = bytes[num2];
					num2++;
				}
			}
			return Convert.ToBase64String(array);
		}

		// Token: 0x040021F1 RID: 8689
		public OnWebSocketOpenDelegate OnOpen;

		// Token: 0x040021F2 RID: 8690
		public OnWebSocketMessageDelegate OnMessage;

		// Token: 0x040021F3 RID: 8691
		public OnWebSocketBinaryDelegate OnBinary;

		// Token: 0x040021F4 RID: 8692
		public OnWebSocketClosedDelegate OnClosed;

		// Token: 0x040021F5 RID: 8693
		public OnWebSocketErrorDelegate OnError;

		// Token: 0x040021F6 RID: 8694
		public OnWebSocketErrorDescriptionDelegate OnErrorDesc;

		// Token: 0x040021F7 RID: 8695
		public OnWebSocketIncompleteFrameDelegate OnIncompleteFrame;

		// Token: 0x040021F8 RID: 8696
		private bool requestSent;

		// Token: 0x040021F9 RID: 8697
		private WebSocketResponse webSocket;
	}
}
