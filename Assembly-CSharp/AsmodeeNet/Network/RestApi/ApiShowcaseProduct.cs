using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006AB RID: 1707
	[Serializable]
	public class ApiShowcaseProduct
	{
		// Token: 0x04002791 RID: 10129
		public int id;

		// Token: 0x04002792 RID: 10130
		public string name;

		// Token: 0x04002793 RID: 10131
		public string description;

		// Token: 0x04002794 RID: 10132
		public string status;

		// Token: 0x04002795 RID: 10133
		public string icon_url;

		// Token: 0x04002796 RID: 10134
		public string banner_url;

		// Token: 0x04002797 RID: 10135
		public string shop_digital_url;

		// Token: 0x04002798 RID: 10136
		public string shop_physical_url;

		// Token: 0x04002799 RID: 10137
		public ApiShowcaseProduct.Tile tile;

		// Token: 0x0400279A RID: 10138
		public ApiShowcaseProduct.Image[] images;

		// Token: 0x0400279B RID: 10139
		public ApiShowcaseProduct.Video[] videos;

		// Token: 0x0400279C RID: 10140
		public ApiShowcaseProduct.Award[] awards;

		// Token: 0x020009B3 RID: 2483
		[Serializable]
		public class Tile
		{
			// Token: 0x040032C5 RID: 12997
			public int width;

			// Token: 0x040032C6 RID: 12998
			public int height;

			// Token: 0x040032C7 RID: 12999
			public string image_url;
		}

		// Token: 0x020009B4 RID: 2484
		[Serializable]
		public class Award
		{
			// Token: 0x040032C8 RID: 13000
			public string name;

			// Token: 0x040032C9 RID: 13001
			public string image_url;
		}

		// Token: 0x020009B5 RID: 2485
		[Serializable]
		public class Image
		{
			// Token: 0x040032CA RID: 13002
			public string image_url;

			// Token: 0x040032CB RID: 13003
			public string thumb_url;
		}

		// Token: 0x020009B6 RID: 2486
		[Serializable]
		public class Video
		{
			// Token: 0x040032CC RID: 13004
			public string video_url;

			// Token: 0x040032CD RID: 13005
			public string thumb_url;
		}
	}
}
