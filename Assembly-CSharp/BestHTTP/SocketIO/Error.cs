using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x02000596 RID: 1430
	public sealed class Error
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600344B RID: 13387 RVA: 0x00107D3A File Offset: 0x00105F3A
		// (set) Token: 0x0600344C RID: 13388 RVA: 0x00107D42 File Offset: 0x00105F42
		public SocketIOErrors Code { get; private set; }

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600344D RID: 13389 RVA: 0x00107D4B File Offset: 0x00105F4B
		// (set) Token: 0x0600344E RID: 13390 RVA: 0x00107D53 File Offset: 0x00105F53
		public string Message { get; private set; }

		// Token: 0x0600344F RID: 13391 RVA: 0x00107D5C File Offset: 0x00105F5C
		public Error(SocketIOErrors code, string msg)
		{
			this.Code = code;
			this.Message = msg;
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x00107D74 File Offset: 0x00105F74
		public override string ToString()
		{
			return string.Format("Code: {0} Message: \"{1}\"", this.Code.ToString(), this.Message);
		}
	}
}
