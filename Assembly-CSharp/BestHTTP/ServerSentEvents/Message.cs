using System;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x020005E2 RID: 1506
	public sealed class Message
	{
		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06003720 RID: 14112 RVA: 0x00110940 File Offset: 0x0010EB40
		// (set) Token: 0x06003721 RID: 14113 RVA: 0x00110948 File Offset: 0x0010EB48
		public string Id { get; internal set; }

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06003722 RID: 14114 RVA: 0x00110951 File Offset: 0x0010EB51
		// (set) Token: 0x06003723 RID: 14115 RVA: 0x00110959 File Offset: 0x0010EB59
		public string Event { get; internal set; }

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06003724 RID: 14116 RVA: 0x00110962 File Offset: 0x0010EB62
		// (set) Token: 0x06003725 RID: 14117 RVA: 0x0011096A File Offset: 0x0010EB6A
		public string Data { get; internal set; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06003726 RID: 14118 RVA: 0x00110973 File Offset: 0x0010EB73
		// (set) Token: 0x06003727 RID: 14119 RVA: 0x0011097B File Offset: 0x0010EB7B
		public TimeSpan Retry { get; internal set; }

		// Token: 0x06003728 RID: 14120 RVA: 0x00110984 File Offset: 0x0010EB84
		public override string ToString()
		{
			return string.Format("\"{0}\": \"{1}\"", this.Event, this.Data);
		}
	}
}
