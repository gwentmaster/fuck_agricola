using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket
{
	// Token: 0x0200058A RID: 1418
	public sealed class WebSocketResponse : HTTPResponse, IHeartbeat, IProtocol
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x001061FD File Offset: 0x001043FD
		// (set) Token: 0x060033EC RID: 13292 RVA: 0x00106205 File Offset: 0x00104405
		public WebSocket WebSocket { get; internal set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x0010620E File Offset: 0x0010440E
		public bool IsClosed
		{
			get
			{
				return this.closed;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060033EE RID: 13294 RVA: 0x00106218 File Offset: 0x00104418
		// (set) Token: 0x060033EF RID: 13295 RVA: 0x00106220 File Offset: 0x00104420
		public TimeSpan PingFrequnecy { get; private set; }

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060033F0 RID: 13296 RVA: 0x00106229 File Offset: 0x00104429
		// (set) Token: 0x060033F1 RID: 13297 RVA: 0x00106231 File Offset: 0x00104431
		public ushort MaxFragmentSize { get; private set; }

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060033F2 RID: 13298 RVA: 0x0010623A File Offset: 0x0010443A
		public int BufferedAmount
		{
			get
			{
				return this._bufferedAmount;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060033F3 RID: 13299 RVA: 0x00106242 File Offset: 0x00104442
		// (set) Token: 0x060033F4 RID: 13300 RVA: 0x0010624A File Offset: 0x0010444A
		public int Latency { get; private set; }

		// Token: 0x060033F5 RID: 13301 RVA: 0x00106254 File Offset: 0x00104454
		internal WebSocketResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache) : base(request, stream, isStreamed, isFromCache)
		{
			base.IsClosedManually = true;
			this.closed = false;
			this.MaxFragmentSize = 32767;
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x001062F0 File Offset: 0x001044F0
		internal void StartReceive()
		{
			if (base.IsUpgraded)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveThreadFunc));
			}
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x0010630C File Offset: 0x0010450C
		internal void CloseStream()
		{
			ConnectionBase connectionWith = HTTPManager.GetConnectionWith(this.baseRequest);
			if (connectionWith != null)
			{
				connectionWith.Abort(HTTPConnectionStates.Closed);
			}
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x00106330 File Offset: 0x00104530
		public void Send(string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message must not be null!");
			}
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Text, bytes));
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0010636C File Offset: 0x0010456C
		public void Send(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data must not be null!");
			}
			WebSocketFrame webSocketFrame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Binary, data);
			if (webSocketFrame.Data != null && webSocketFrame.Data.Length > (int)this.MaxFragmentSize)
			{
				WebSocketFrame[] array = webSocketFrame.Fragment(this.MaxFragmentSize);
				object sendLock = this.SendLock;
				lock (sendLock)
				{
					this.Send(webSocketFrame);
					if (array != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							this.Send(array[i]);
						}
					}
					return;
				}
			}
			this.Send(webSocketFrame);
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x00106414 File Offset: 0x00104614
		public void Send(byte[] data, ulong offset, ulong count)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data must not be null!");
			}
			if (offset + count > (ulong)((long)data.Length))
			{
				throw new ArgumentOutOfRangeException("offset + count >= data.Length");
			}
			WebSocketFrame webSocketFrame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Binary, data, offset, count, true, true);
			if (webSocketFrame.Data != null && webSocketFrame.Data.Length > (int)this.MaxFragmentSize)
			{
				WebSocketFrame[] array = webSocketFrame.Fragment(this.MaxFragmentSize);
				object sendLock = this.SendLock;
				lock (sendLock)
				{
					this.Send(webSocketFrame);
					if (array != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							this.Send(array[i]);
						}
					}
					return;
				}
			}
			this.Send(webSocketFrame);
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x001064D4 File Offset: 0x001046D4
		public void Send(WebSocketFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame is null!");
			}
			if (this.closed || this.closeSent)
			{
				return;
			}
			object sendLock = this.SendLock;
			lock (sendLock)
			{
				this.unsentFrames.Add(frame);
				if (!this.sendThreadCreated)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendThreadFunc));
					this.sendThreadCreated = true;
				}
			}
			Interlocked.Add(ref this._bufferedAmount, (frame.Data != null) ? frame.Data.Length : 0);
			this.newFrameSignal.Set();
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x00106590 File Offset: 0x00104790
		public void Insert(WebSocketFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame is null!");
			}
			if (this.closed || this.closeSent)
			{
				return;
			}
			object sendLock = this.SendLock;
			lock (sendLock)
			{
				this.unsentFrames.Insert(0, frame);
				if (!this.sendThreadCreated)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendThreadFunc));
					this.sendThreadCreated = true;
				}
			}
			Interlocked.Add(ref this._bufferedAmount, (frame.Data != null) ? frame.Data.Length : 0);
			this.newFrameSignal.Set();
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x0010664C File Offset: 0x0010484C
		public void SendNow(WebSocketFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame is null!");
			}
			if (this.closed || this.closeSent)
			{
				return;
			}
			byte[] array = frame.Get();
			this.Stream.Write(array, 0, array.Length);
			this.Stream.Flush();
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x0010669E File Offset: 0x0010489E
		public void Close()
		{
			this.Close(1000, "Bye!");
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x001066B0 File Offset: 0x001048B0
		public void Close(ushort code, string msg)
		{
			if (this.closed)
			{
				return;
			}
			object sendLock = this.SendLock;
			lock (sendLock)
			{
				this.unsentFrames.Clear();
			}
			Interlocked.Exchange(ref this._bufferedAmount, 0);
			this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.ConnectionClose, WebSocket.EncodeCloseData(code, msg)));
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x00106728 File Offset: 0x00104928
		public void StartPinging(int frequency)
		{
			if (frequency < 100)
			{
				throw new ArgumentException("frequency must be at least 100 milliseconds!");
			}
			this.PingFrequnecy = TimeSpan.FromMilliseconds((double)frequency);
			this.lastMessage = DateTime.UtcNow;
			this.SendPing();
			HTTPManager.Heartbeats.Subscribe(this);
			HTTPUpdateDelegator.OnApplicationForegroundStateChanged = (Action<bool>)Delegate.Combine(HTTPUpdateDelegator.OnApplicationForegroundStateChanged, new Action<bool>(this.OnApplicationForegroundStateChanged));
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x00106790 File Offset: 0x00104990
		private void SendThreadFunc(object param)
		{
			List<WebSocketFrame> list = new List<WebSocketFrame>();
			try
			{
				while (!this.closed && !this.closeSent)
				{
					this.newFrameSignal.WaitOne();
					try
					{
						object sendLock = this.SendLock;
						lock (sendLock)
						{
							for (int i = this.unsentFrames.Count - 1; i >= 0; i--)
							{
								list.Add(this.unsentFrames[i]);
							}
							this.unsentFrames.Clear();
							goto IL_D5;
						}
						IL_69:
						WebSocketFrame webSocketFrame = list[list.Count - 1];
						list.RemoveAt(list.Count - 1);
						if (!this.closeSent)
						{
							byte[] array = webSocketFrame.Get();
							this.Stream.Write(array, 0, array.Length);
							if (webSocketFrame.Type == WebSocketFrameTypes.ConnectionClose)
							{
								this.closeSent = true;
							}
						}
						Interlocked.Add(ref this._bufferedAmount, -webSocketFrame.Data.Length);
						IL_D5:
						if (list.Count > 0)
						{
							goto IL_69;
						}
						this.Stream.Flush();
					}
					catch (Exception exception)
					{
						if (HTTPUpdateDelegator.IsCreated)
						{
							this.baseRequest.Exception = exception;
							this.baseRequest.State = HTTPRequestStates.Error;
						}
						else
						{
							this.baseRequest.State = HTTPRequestStates.Aborted;
						}
						this.closed = true;
					}
				}
			}
			finally
			{
				this.sendThreadCreated = false;
				HTTPManager.Logger.Information("WebSocketResponse", "SendThread - Closed!");
			}
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x00106948 File Offset: 0x00104B48
		private void ReceiveThreadFunc(object param)
		{
			try
			{
				while (!this.closed)
				{
					try
					{
						WebSocketFrameReader webSocketFrameReader = new WebSocketFrameReader();
						webSocketFrameReader.Read(this.Stream);
						this.lastMessage = DateTime.UtcNow;
						if (webSocketFrameReader.HasMask)
						{
							this.Close(1002, "Protocol Error: masked frame received from server!");
						}
						else if (!webSocketFrameReader.IsFinal)
						{
							if (this.OnIncompleteFrame == null)
							{
								this.IncompleteFrames.Add(webSocketFrameReader);
							}
							else
							{
								object frameLock = this.FrameLock;
								lock (frameLock)
								{
									this.CompletedFrames.Add(webSocketFrameReader);
								}
							}
						}
						else
						{
							object frameLock;
							switch (webSocketFrameReader.Type)
							{
							case WebSocketFrameTypes.Continuation:
								if (this.OnIncompleteFrame == null)
								{
									webSocketFrameReader.Assemble(this.IncompleteFrames);
									this.IncompleteFrames.Clear();
								}
								else
								{
									frameLock = this.FrameLock;
									lock (frameLock)
									{
										this.CompletedFrames.Add(webSocketFrameReader);
										continue;
									}
								}
								break;
							case WebSocketFrameTypes.Text:
							case WebSocketFrameTypes.Binary:
								break;
							case (WebSocketFrameTypes)3:
							case (WebSocketFrameTypes)4:
							case (WebSocketFrameTypes)5:
							case (WebSocketFrameTypes)6:
							case (WebSocketFrameTypes)7:
								continue;
							case WebSocketFrameTypes.ConnectionClose:
								goto IL_1CC;
							case WebSocketFrameTypes.Ping:
								goto IL_14F;
							case WebSocketFrameTypes.Pong:
								try
								{
									long num = BitConverter.ToInt64(webSocketFrameReader.Data, 0);
									TimeSpan timeSpan = TimeSpan.FromTicks(this.lastMessage.Ticks - num);
									this.rtts.Add((int)timeSpan.TotalMilliseconds);
									this.Latency = this.CalculateLatency();
									continue;
								}
								catch
								{
									continue;
								}
								goto IL_1CC;
							default:
								continue;
							}
							webSocketFrameReader.DecodeWithExtensions(this.WebSocket);
							frameLock = this.FrameLock;
							lock (frameLock)
							{
								this.CompletedFrames.Add(webSocketFrameReader);
								continue;
							}
							IL_14F:
							if (!this.closeSent && !this.closed)
							{
								this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Pong, webSocketFrameReader.Data));
								continue;
							}
							continue;
							IL_1CC:
							this.CloseFrame = webSocketFrameReader;
							if (!this.closeSent)
							{
								this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.ConnectionClose, null));
							}
							this.closed = true;
						}
					}
					catch (ThreadAbortException)
					{
						this.IncompleteFrames.Clear();
						this.baseRequest.State = HTTPRequestStates.Aborted;
						this.closed = true;
						this.newFrameSignal.Set();
					}
					catch (Exception exception)
					{
						if (HTTPUpdateDelegator.IsCreated)
						{
							this.baseRequest.Exception = exception;
							this.baseRequest.State = HTTPRequestStates.Error;
						}
						else
						{
							this.baseRequest.State = HTTPRequestStates.Aborted;
						}
						this.closed = true;
						this.newFrameSignal.Set();
					}
				}
			}
			finally
			{
				HTTPManager.Heartbeats.Unsubscribe(this);
				HTTPUpdateDelegator.OnApplicationForegroundStateChanged = (Action<bool>)Delegate.Remove(HTTPUpdateDelegator.OnApplicationForegroundStateChanged, new Action<bool>(this.OnApplicationForegroundStateChanged));
				HTTPManager.Logger.Information("WebSocketResponse", "ReceiveThread - Closed!");
			}
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x00106CC4 File Offset: 0x00104EC4
		void IProtocol.HandleEvents()
		{
			object frameLock = this.FrameLock;
			lock (frameLock)
			{
				for (int i = 0; i < this.CompletedFrames.Count; i++)
				{
					WebSocketFrameReader webSocketFrameReader = this.CompletedFrames[i];
					try
					{
						switch (webSocketFrameReader.Type)
						{
						case WebSocketFrameTypes.Continuation:
							break;
						case WebSocketFrameTypes.Text:
							if (webSocketFrameReader.IsFinal)
							{
								if (this.OnText != null)
								{
									this.OnText(this, webSocketFrameReader.DataAsText);
									goto IL_9F;
								}
								goto IL_9F;
							}
							break;
						case WebSocketFrameTypes.Binary:
							if (webSocketFrameReader.IsFinal)
							{
								if (this.OnBinary != null)
								{
									this.OnBinary(this, webSocketFrameReader.Data);
									goto IL_9F;
								}
								goto IL_9F;
							}
							break;
						default:
							goto IL_9F;
						}
						if (this.OnIncompleteFrame != null)
						{
							this.OnIncompleteFrame(this, webSocketFrameReader);
						}
						IL_9F:;
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("WebSocketResponse", "HandleEvents", ex);
					}
				}
				this.CompletedFrames.Clear();
			}
			if (this.IsClosed && this.OnClosed != null && this.baseRequest.State == HTTPRequestStates.Processing)
			{
				try
				{
					ushort arg = 0;
					string arg2 = string.Empty;
					if (this.CloseFrame != null && this.CloseFrame.Data != null && this.CloseFrame.Data.Length >= 2)
					{
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse(this.CloseFrame.Data, 0, 2);
						}
						arg = BitConverter.ToUInt16(this.CloseFrame.Data, 0);
						if (this.CloseFrame.Data.Length > 2)
						{
							arg2 = Encoding.UTF8.GetString(this.CloseFrame.Data, 2, this.CloseFrame.Data.Length - 2);
						}
					}
					this.OnClosed(this, arg, arg2);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("WebSocketResponse", "HandleEvents - OnClosed", ex2);
				}
			}
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x00106EC8 File Offset: 0x001050C8
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow - this.lastPing >= this.PingFrequnecy)
			{
				this.SendPing();
			}
			if (utcNow - (this.lastMessage + this.PingFrequnecy) > this.WebSocket.CloseAfterNoMesssage)
			{
				HTTPManager.Logger.Information("WebSocketResponse", string.Format("No message received in the given time! Closing WebSocket. LastMessage: {0}, PingFrequency: {1}, Close After: {2}, Now: {3}", new object[]
				{
					this.lastMessage,
					this.PingFrequnecy,
					this.WebSocket.CloseAfterNoMesssage,
					utcNow
				}));
				this.CloseWithError("No message received in the given time!");
			}
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00106F85 File Offset: 0x00105185
		private void OnApplicationForegroundStateChanged(bool isPaused)
		{
			if (!isPaused)
			{
				this.lastMessage = DateTime.UtcNow;
			}
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x00106F98 File Offset: 0x00105198
		private void SendPing()
		{
			this.lastPing = DateTime.UtcNow;
			try
			{
				byte[] bytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
				WebSocketFrame frame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Ping, bytes);
				this.Insert(frame);
			}
			catch
			{
				HTTPManager.Logger.Information("WebSocketResponse", "Error while sending PING message! Closing WebSocket.");
				this.CloseWithError("Error while sending PING message!");
			}
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x00107010 File Offset: 0x00105210
		private void CloseWithError(string message)
		{
			this.baseRequest.Exception = new Exception(message);
			this.baseRequest.State = HTTPRequestStates.Error;
			this.closed = true;
			HTTPManager.Heartbeats.Unsubscribe(this);
			HTTPUpdateDelegator.OnApplicationForegroundStateChanged = (Action<bool>)Delegate.Remove(HTTPUpdateDelegator.OnApplicationForegroundStateChanged, new Action<bool>(this.OnApplicationForegroundStateChanged));
			this.newFrameSignal.Set();
			this.CloseStream();
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x00107080 File Offset: 0x00105280
		private int CalculateLatency()
		{
			if (this.rtts.Count == 0)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.rtts.Count; i++)
			{
				num += this.rtts[i];
			}
			return num / this.rtts.Count;
		}

		// Token: 0x040021FA RID: 8698
		public static int RTTBufferCapacity = 5;

		// Token: 0x040021FC RID: 8700
		public Action<WebSocketResponse, string> OnText;

		// Token: 0x040021FD RID: 8701
		public Action<WebSocketResponse, byte[]> OnBinary;

		// Token: 0x040021FE RID: 8702
		public Action<WebSocketResponse, WebSocketFrameReader> OnIncompleteFrame;

		// Token: 0x040021FF RID: 8703
		public Action<WebSocketResponse, ushort, string> OnClosed;

		// Token: 0x04002202 RID: 8706
		private int _bufferedAmount;

		// Token: 0x04002204 RID: 8708
		private List<WebSocketFrameReader> IncompleteFrames = new List<WebSocketFrameReader>();

		// Token: 0x04002205 RID: 8709
		private List<WebSocketFrameReader> CompletedFrames = new List<WebSocketFrameReader>();

		// Token: 0x04002206 RID: 8710
		private WebSocketFrameReader CloseFrame;

		// Token: 0x04002207 RID: 8711
		private object FrameLock = new object();

		// Token: 0x04002208 RID: 8712
		private object SendLock = new object();

		// Token: 0x04002209 RID: 8713
		private List<WebSocketFrame> unsentFrames = new List<WebSocketFrame>();

		// Token: 0x0400220A RID: 8714
		private AutoResetEvent newFrameSignal = new AutoResetEvent(false);

		// Token: 0x0400220B RID: 8715
		private volatile bool sendThreadCreated;

		// Token: 0x0400220C RID: 8716
		private volatile bool closeSent;

		// Token: 0x0400220D RID: 8717
		private volatile bool closed;

		// Token: 0x0400220E RID: 8718
		private DateTime lastPing = DateTime.MinValue;

		// Token: 0x0400220F RID: 8719
		private DateTime lastMessage = DateTime.MinValue;

		// Token: 0x04002210 RID: 8720
		private CircularBuffer<int> rtts = new CircularBuffer<int>(WebSocketResponse.RTTBufferCapacity);
	}
}
