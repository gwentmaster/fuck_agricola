using System;
using System.Text;

namespace BestHTTP.Forms
{
	// Token: 0x020005E8 RID: 1512
	public class HTTPFieldData
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x00111C8B File Offset: 0x0010FE8B
		// (set) Token: 0x0600378C RID: 14220 RVA: 0x00111C93 File Offset: 0x0010FE93
		public string Name { get; set; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600378D RID: 14221 RVA: 0x00111C9C File Offset: 0x0010FE9C
		// (set) Token: 0x0600378E RID: 14222 RVA: 0x00111CA4 File Offset: 0x0010FEA4
		public string FileName { get; set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600378F RID: 14223 RVA: 0x00111CAD File Offset: 0x0010FEAD
		// (set) Token: 0x06003790 RID: 14224 RVA: 0x00111CB5 File Offset: 0x0010FEB5
		public string MimeType { get; set; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06003791 RID: 14225 RVA: 0x00111CBE File Offset: 0x0010FEBE
		// (set) Token: 0x06003792 RID: 14226 RVA: 0x00111CC6 File Offset: 0x0010FEC6
		public Encoding Encoding { get; set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06003793 RID: 14227 RVA: 0x00111CCF File Offset: 0x0010FECF
		// (set) Token: 0x06003794 RID: 14228 RVA: 0x00111CD7 File Offset: 0x0010FED7
		public string Text { get; set; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06003795 RID: 14229 RVA: 0x00111CE0 File Offset: 0x0010FEE0
		// (set) Token: 0x06003796 RID: 14230 RVA: 0x00111CE8 File Offset: 0x0010FEE8
		public byte[] Binary { get; set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x00111CF4 File Offset: 0x0010FEF4
		public byte[] Payload
		{
			get
			{
				if (this.Binary != null)
				{
					return this.Binary;
				}
				if (this.Encoding == null)
				{
					this.Encoding = Encoding.UTF8;
				}
				return this.Binary = this.Encoding.GetBytes(this.Text);
			}
		}
	}
}
