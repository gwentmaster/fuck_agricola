using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F5 RID: 1525
	public sealed class HeartbeatManager
	{
		// Token: 0x060037F2 RID: 14322 RVA: 0x00112E94 File Offset: 0x00111094
		public void Subscribe(IHeartbeat heartbeat)
		{
			List<IHeartbeat> heartbeats = this.Heartbeats;
			lock (heartbeats)
			{
				if (!this.Heartbeats.Contains(heartbeat))
				{
					this.Heartbeats.Add(heartbeat);
				}
			}
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x00112EE8 File Offset: 0x001110E8
		public void Unsubscribe(IHeartbeat heartbeat)
		{
			List<IHeartbeat> heartbeats = this.Heartbeats;
			lock (heartbeats)
			{
				this.Heartbeats.Remove(heartbeat);
			}
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x00112F30 File Offset: 0x00111130
		public void Update()
		{
			if (this.LastUpdate == DateTime.MinValue)
			{
				this.LastUpdate = DateTime.UtcNow;
				return;
			}
			TimeSpan dif = DateTime.UtcNow - this.LastUpdate;
			this.LastUpdate = DateTime.UtcNow;
			int num = 0;
			List<IHeartbeat> heartbeats = this.Heartbeats;
			lock (heartbeats)
			{
				if (this.UpdateArray == null || this.UpdateArray.Length < this.Heartbeats.Count)
				{
					Array.Resize<IHeartbeat>(ref this.UpdateArray, this.Heartbeats.Count);
				}
				this.Heartbeats.CopyTo(0, this.UpdateArray, 0, this.Heartbeats.Count);
				num = this.Heartbeats.Count;
			}
			for (int i = 0; i < num; i++)
			{
				try
				{
					this.UpdateArray[i].OnHeartbeatUpdate(dif);
				}
				catch
				{
				}
			}
		}

		// Token: 0x040023C9 RID: 9161
		private List<IHeartbeat> Heartbeats = new List<IHeartbeat>();

		// Token: 0x040023CA RID: 9162
		private IHeartbeat[] UpdateArray;

		// Token: 0x040023CB RID: 9163
		private DateTime LastUpdate = DateTime.MinValue;
	}
}
