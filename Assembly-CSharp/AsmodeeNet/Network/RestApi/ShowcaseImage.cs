using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C3 RID: 1731
	[Serializable]
	public class ShowcaseImage
	{
		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x0012F6AC File Offset: 0x0012D8AC
		// (set) Token: 0x06003E05 RID: 15877 RVA: 0x0012F6B4 File Offset: 0x0012D8B4
		public string ImageUrl { get; private set; }

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06003E06 RID: 15878 RVA: 0x0012F6BD File Offset: 0x0012D8BD
		// (set) Token: 0x06003E07 RID: 15879 RVA: 0x0012F6C5 File Offset: 0x0012D8C5
		public string ThumbUrl { get; private set; }

		// Token: 0x06003E08 RID: 15880 RVA: 0x0012F6CE File Offset: 0x0012D8CE
		public ShowcaseImage(ApiShowcaseProduct.Image raw)
		{
			this.ImageUrl = raw.image_url;
			this.ThumbUrl = raw.thumb_url;
		}
	}
}
