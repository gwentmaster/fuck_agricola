using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200069B RID: 1691
	[Serializable]
	public class JsonAchievement
	{
		// Token: 0x0400275D RID: 10077
		public int id;

		// Token: 0x0400275E RID: 10078
		public string tag;

		// Token: 0x0400275F RID: 10079
		public string status;

		// Token: 0x04002760 RID: 10080
		public string unicity;

		// Token: 0x04002761 RID: 10081
		public string game;

		// Token: 0x04002762 RID: 10082
		public string type;

		// Token: 0x04002763 RID: 10083
		public bool secret;

		// Token: 0x04002764 RID: 10084
		public int treasure;

		// Token: 0x04002765 RID: 10085
		public int category;

		// Token: 0x04002766 RID: 10086
		public string picture;

		// Token: 0x04002767 RID: 10087
		public string ribbon;

		// Token: 0x04002768 RID: 10088
		public JsonAchievement.Text[] texts;

		// Token: 0x020009A5 RID: 2469
		[Serializable]
		public class Text
		{
			// Token: 0x040032AC RID: 12972
			public string lang;

			// Token: 0x040032AD RID: 12973
			public string name;

			// Token: 0x040032AE RID: 12974
			public string description;
		}
	}
}
