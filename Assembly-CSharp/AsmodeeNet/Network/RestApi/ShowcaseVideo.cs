using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C4 RID: 1732
	[Serializable]
	public class ShowcaseVideo
	{
		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06003E09 RID: 15881 RVA: 0x0012F6EE File Offset: 0x0012D8EE
		// (set) Token: 0x06003E0A RID: 15882 RVA: 0x0012F6F6 File Offset: 0x0012D8F6
		public string VideoUrl { get; private set; }

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06003E0B RID: 15883 RVA: 0x0012F6FF File Offset: 0x0012D8FF
		// (set) Token: 0x06003E0C RID: 15884 RVA: 0x0012F707 File Offset: 0x0012D907
		public string ThumbUrl { get; private set; }

		// Token: 0x06003E0D RID: 15885 RVA: 0x0012F710 File Offset: 0x0012D910
		public ShowcaseVideo(ApiShowcaseProduct.Video raw)
		{
			this.VideoUrl = raw.video_url;
			this.ThumbUrl = raw.thumb_url;
		}
	}
}
