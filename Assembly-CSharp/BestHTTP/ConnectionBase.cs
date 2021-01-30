using System;
using System.Threading;

namespace BestHTTP
{
	// Token: 0x02000563 RID: 1379
	internal abstract class ConnectionBase : IDisposable
	{
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x000FF1D9 File Offset: 0x000FD3D9
		// (set) Token: 0x060031FE RID: 12798 RVA: 0x000FF1E1 File Offset: 0x000FD3E1
		public string ServerAddress { get; protected set; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x000FF1EA File Offset: 0x000FD3EA
		// (set) Token: 0x06003200 RID: 12800 RVA: 0x000FF1F2 File Offset: 0x000FD3F2
		public HTTPConnectionStates State { get; protected set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x000FF1FB File Offset: 0x000FD3FB
		public bool IsFree
		{
			get
			{
				return this.State == HTTPConnectionStates.Initial || this.State == HTTPConnectionStates.Free;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x000FF210 File Offset: 0x000FD410
		public bool IsActive
		{
			get
			{
				return this.State > HTTPConnectionStates.Initial && this.State < HTTPConnectionStates.Free;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x000FF226 File Offset: 0x000FD426
		// (set) Token: 0x06003204 RID: 12804 RVA: 0x000FF22E File Offset: 0x000FD42E
		public HTTPRequest CurrentRequest { get; protected set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x000FF237 File Offset: 0x000FD437
		public virtual bool IsRemovable
		{
			get
			{
				return this.IsFree && DateTime.UtcNow - this.LastProcessTime > HTTPManager.MaxConnectionIdleTime;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x000FF25D File Offset: 0x000FD45D
		// (set) Token: 0x06003207 RID: 12807 RVA: 0x000FF265 File Offset: 0x000FD465
		public DateTime StartTime { get; protected set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06003208 RID: 12808 RVA: 0x000FF26E File Offset: 0x000FD46E
		// (set) Token: 0x06003209 RID: 12809 RVA: 0x000FF276 File Offset: 0x000FD476
		public DateTime TimedOutStart { get; protected set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x000FF27F File Offset: 0x000FD47F
		// (set) Token: 0x0600320B RID: 12811 RVA: 0x000FF287 File Offset: 0x000FD487
		protected HTTPProxy Proxy { get; set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x000FF290 File Offset: 0x000FD490
		public bool HasProxy
		{
			get
			{
				return this.Proxy != null;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x000FF29B File Offset: 0x000FD49B
		// (set) Token: 0x0600320E RID: 12814 RVA: 0x000FF2A3 File Offset: 0x000FD4A3
		public Uri LastProcessedUri { get; protected set; }

		// Token: 0x0600320F RID: 12815 RVA: 0x000FF2AC File Offset: 0x000FD4AC
		public ConnectionBase(string serverAddress) : this(serverAddress, true)
		{
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x000FF2B6 File Offset: 0x000FD4B6
		public ConnectionBase(string serverAddress, bool threaded)
		{
			this.ServerAddress = serverAddress;
			this.State = HTTPConnectionStates.Initial;
			this.LastProcessTime = DateTime.UtcNow;
			this.IsThreaded = threaded;
		}

		// Token: 0x06003211 RID: 12817
		internal abstract void Abort(HTTPConnectionStates hTTPConnectionStates);

		// Token: 0x06003212 RID: 12818 RVA: 0x000FF2E0 File Offset: 0x000FD4E0
		internal void Process(HTTPRequest request)
		{
			if (this.State == HTTPConnectionStates.Processing)
			{
				throw new Exception("Connection already processing a request!");
			}
			this.StartTime = DateTime.MaxValue;
			this.State = HTTPConnectionStates.Processing;
			this.CurrentRequest = request;
			if (this.IsThreaded)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadFunc));
				return;
			}
			this.ThreadFunc(null);
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x00003022 File Offset: 0x00001222
		protected virtual void ThreadFunc(object param)
		{
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x000FF340 File Offset: 0x000FD540
		internal void HandleProgressCallback()
		{
			if (this.CurrentRequest.OnProgress != null && this.CurrentRequest.DownloadProgressChanged)
			{
				try
				{
					this.CurrentRequest.OnProgress(this.CurrentRequest, this.CurrentRequest.Downloaded, this.CurrentRequest.DownloadLength);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnProgress", ex);
				}
				this.CurrentRequest.DownloadProgressChanged = false;
			}
			if (this.CurrentRequest.OnUploadProgress != null && this.CurrentRequest.UploadProgressChanged)
			{
				try
				{
					this.CurrentRequest.OnUploadProgress(this.CurrentRequest, this.CurrentRequest.Uploaded, this.CurrentRequest.UploadLength);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnUploadProgress", ex2);
				}
				this.CurrentRequest.UploadProgressChanged = false;
			}
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000FF444 File Offset: 0x000FD644
		internal void HandleCallback()
		{
			try
			{
				this.HandleProgressCallback();
				if (this.State == HTTPConnectionStates.Upgraded)
				{
					if (this.CurrentRequest != null && this.CurrentRequest.Response != null && this.CurrentRequest.Response.IsUpgraded)
					{
						this.CurrentRequest.UpgradeCallback();
					}
					this.State = HTTPConnectionStates.WaitForProtocolShutdown;
				}
				else
				{
					this.CurrentRequest.CallCallback();
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("ConnectionBase", "HandleCallback", ex);
			}
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x000FF4D0 File Offset: 0x000FD6D0
		internal void Recycle(HTTPConnectionRecycledDelegate onConnectionRecycled)
		{
			this.OnConnectionRecycled = onConnectionRecycled;
			if (this.State <= HTTPConnectionStates.Initial || this.State >= HTTPConnectionStates.WaitForProtocolShutdown || this.State == HTTPConnectionStates.Redirected)
			{
				this.RecycleNow();
			}
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x000FF4FC File Offset: 0x000FD6FC
		protected void RecycleNow()
		{
			if (this.State == HTTPConnectionStates.TimedOut || this.State == HTTPConnectionStates.Closed)
			{
				this.LastProcessTime = DateTime.MinValue;
			}
			this.State = HTTPConnectionStates.Free;
			this.CurrentRequest = null;
			if (this.OnConnectionRecycled != null)
			{
				this.OnConnectionRecycled(this);
				this.OnConnectionRecycled = null;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x000FF550 File Offset: 0x000FD750
		// (set) Token: 0x06003219 RID: 12825 RVA: 0x000FF558 File Offset: 0x000FD758
		private protected bool IsDisposed { protected get; private set; }

		// Token: 0x0600321A RID: 12826 RVA: 0x000FF561 File Offset: 0x000FD761
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x000FF570 File Offset: 0x000FD770
		protected virtual void Dispose(bool disposing)
		{
			this.IsDisposed = true;
		}

		// Token: 0x04002115 RID: 8469
		protected DateTime LastProcessTime;

		// Token: 0x04002116 RID: 8470
		protected HTTPConnectionRecycledDelegate OnConnectionRecycled;

		// Token: 0x04002117 RID: 8471
		private bool IsThreaded;
	}
}
