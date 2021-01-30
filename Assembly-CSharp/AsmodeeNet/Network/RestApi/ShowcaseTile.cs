using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C1 RID: 1729
	[Serializable]
	public class ShowcaseTile
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06003DF8 RID: 15864 RVA: 0x0012F60B File Offset: 0x0012D80B
		// (set) Token: 0x06003DF9 RID: 15865 RVA: 0x0012F613 File Offset: 0x0012D813
		public int Width { get; private set; }

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x0012F61C File Offset: 0x0012D81C
		// (set) Token: 0x06003DFB RID: 15867 RVA: 0x0012F624 File Offset: 0x0012D824
		public int Height { get; private set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x0012F62D File Offset: 0x0012D82D
		// (set) Token: 0x06003DFD RID: 15869 RVA: 0x0012F635 File Offset: 0x0012D835
		public string ImageUrl { get; private set; }

		// Token: 0x06003DFE RID: 15870 RVA: 0x0012F63E File Offset: 0x0012D83E
		public ShowcaseTile(ApiShowcaseProduct.Tile raw)
		{
			this.Width = raw.width;
			this.Height = raw.height;
			this.ImageUrl = raw.image_url;
		}
	}
}
