using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C0 RID: 1728
	[Serializable]
	public class ShowcaseProduct
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003DDE RID: 15838 RVA: 0x0012F3EC File Offset: 0x0012D5EC
		// (set) Token: 0x06003DDF RID: 15839 RVA: 0x0012F3F4 File Offset: 0x0012D5F4
		public int Id { get; private set; }

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x0012F3FD File Offset: 0x0012D5FD
		// (set) Token: 0x06003DE1 RID: 15841 RVA: 0x0012F405 File Offset: 0x0012D605
		public string Name { get; private set; }

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x0012F40E File Offset: 0x0012D60E
		// (set) Token: 0x06003DE3 RID: 15843 RVA: 0x0012F416 File Offset: 0x0012D616
		public string Description { get; private set; }

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x0012F41F File Offset: 0x0012D61F
		// (set) Token: 0x06003DE5 RID: 15845 RVA: 0x0012F427 File Offset: 0x0012D627
		public ProductStatus Status { get; private set; }

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x0012F430 File Offset: 0x0012D630
		// (set) Token: 0x06003DE7 RID: 15847 RVA: 0x0012F438 File Offset: 0x0012D638
		public string IconUrl { get; private set; }

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06003DE8 RID: 15848 RVA: 0x0012F441 File Offset: 0x0012D641
		// (set) Token: 0x06003DE9 RID: 15849 RVA: 0x0012F449 File Offset: 0x0012D649
		public string BannerUrl { get; private set; }

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06003DEA RID: 15850 RVA: 0x0012F452 File Offset: 0x0012D652
		// (set) Token: 0x06003DEB RID: 15851 RVA: 0x0012F45A File Offset: 0x0012D65A
		public string ShopDigitalUrl { get; private set; }

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x0012F463 File Offset: 0x0012D663
		// (set) Token: 0x06003DED RID: 15853 RVA: 0x0012F46B File Offset: 0x0012D66B
		public string ShopPhysicalUrl { get; private set; }

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06003DEE RID: 15854 RVA: 0x0012F474 File Offset: 0x0012D674
		// (set) Token: 0x06003DEF RID: 15855 RVA: 0x0012F47C File Offset: 0x0012D67C
		public ShowcaseTile Tile { get; private set; }

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x0012F485 File Offset: 0x0012D685
		// (set) Token: 0x06003DF1 RID: 15857 RVA: 0x0012F48D File Offset: 0x0012D68D
		public ShowcaseImage[] Images { get; private set; }

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x0012F496 File Offset: 0x0012D696
		// (set) Token: 0x06003DF3 RID: 15859 RVA: 0x0012F49E File Offset: 0x0012D69E
		public ShowcaseVideo[] Videos { get; private set; }

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x0012F4A7 File Offset: 0x0012D6A7
		// (set) Token: 0x06003DF5 RID: 15861 RVA: 0x0012F4AF File Offset: 0x0012D6AF
		public ShowcaseAward[] Awards { get; private set; }

		// Token: 0x06003DF6 RID: 15862 RVA: 0x0012F4B8 File Offset: 0x0012D6B8
		public ShowcaseProduct(ApiShowcaseProduct raw)
		{
			this.jsonProduct = raw;
			this.Id = raw.id;
			this.Name = raw.name;
			this.Description = raw.description;
			if (raw.status != null)
			{
				this.Status = (ProductStatus)Enum.Parse(typeof(ProductStatus), raw.status);
			}
			this.IconUrl = raw.icon_url;
			this.BannerUrl = raw.banner_url;
			this.ShopDigitalUrl = raw.shop_digital_url;
			this.ShopPhysicalUrl = raw.shop_physical_url;
			this.Tile = new ShowcaseTile(raw.tile);
			this.Images = (from x in raw.images
			select new ShowcaseImage(x)).ToArray<ShowcaseImage>();
			this.Videos = (from x in raw.videos
			select new ShowcaseVideo(x)).ToArray<ShowcaseVideo>();
			this.Awards = (from x in raw.awards
			select new ShowcaseAward(x)).ToArray<ShowcaseAward>();
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x0012F5FE File Offset: 0x0012D7FE
		public string ToJson()
		{
			return JsonUtility.ToJson(this.jsonProduct);
		}

		// Token: 0x04002805 RID: 10245
		private ApiShowcaseProduct jsonProduct;
	}
}
