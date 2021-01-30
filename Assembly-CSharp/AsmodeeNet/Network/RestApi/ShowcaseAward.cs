using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C2 RID: 1730
	[Serializable]
	public class ShowcaseAward
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06003DFF RID: 15871 RVA: 0x0012F66A File Offset: 0x0012D86A
		// (set) Token: 0x06003E00 RID: 15872 RVA: 0x0012F672 File Offset: 0x0012D872
		public string Name { get; private set; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06003E01 RID: 15873 RVA: 0x0012F67B File Offset: 0x0012D87B
		// (set) Token: 0x06003E02 RID: 15874 RVA: 0x0012F683 File Offset: 0x0012D883
		public string ImageUrl { get; private set; }

		// Token: 0x06003E03 RID: 15875 RVA: 0x0012F68C File Offset: 0x0012D88C
		public ShowcaseAward(ApiShowcaseProduct.Award raw)
		{
			this.Name = raw.name;
			this.ImageUrl = raw.image_url;
		}
	}
}
