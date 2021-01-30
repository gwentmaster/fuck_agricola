using System;

namespace BestHTTP
{
	// Token: 0x0200056F RID: 1391
	public sealed class HTTPRange
	{
		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x00101DAB File Offset: 0x000FFFAB
		// (set) Token: 0x0600328A RID: 12938 RVA: 0x00101DB3 File Offset: 0x000FFFB3
		public int FirstBytePos { get; private set; }

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x00101DBC File Offset: 0x000FFFBC
		// (set) Token: 0x0600328C RID: 12940 RVA: 0x00101DC4 File Offset: 0x000FFFC4
		public int LastBytePos { get; private set; }

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x00101DCD File Offset: 0x000FFFCD
		// (set) Token: 0x0600328E RID: 12942 RVA: 0x00101DD5 File Offset: 0x000FFFD5
		public int ContentLength { get; private set; }

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x00101DDE File Offset: 0x000FFFDE
		// (set) Token: 0x06003290 RID: 12944 RVA: 0x00101DE6 File Offset: 0x000FFFE6
		public bool IsValid { get; private set; }

		// Token: 0x06003291 RID: 12945 RVA: 0x00101DEF File Offset: 0x000FFFEF
		internal HTTPRange()
		{
			this.ContentLength = -1;
			this.IsValid = false;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00101E05 File Offset: 0x00100005
		internal HTTPRange(int contentLength)
		{
			this.ContentLength = contentLength;
			this.IsValid = false;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00101E1C File Offset: 0x0010001C
		internal HTTPRange(int firstBytePosition, int lastBytePosition, int contentLength)
		{
			this.FirstBytePos = firstBytePosition;
			this.LastBytePos = lastBytePosition;
			this.ContentLength = contentLength;
			this.IsValid = (this.FirstBytePos <= this.LastBytePos && this.ContentLength > this.LastBytePos);
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00101E6C File Offset: 0x0010006C
		public override string ToString()
		{
			return string.Format("{0}-{1}/{2} (valid: {3})", new object[]
			{
				this.FirstBytePos,
				this.LastBytePos,
				this.ContentLength,
				this.IsValid
			});
		}
	}
}
