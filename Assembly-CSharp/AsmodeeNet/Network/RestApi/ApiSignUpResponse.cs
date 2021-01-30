using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006AC RID: 1708
	[Serializable]
	public class ApiSignUpResponse
	{
		// Token: 0x0400279D RID: 10141
		public bool error;

		// Token: 0x0400279E RID: 10142
		public int status;

		// Token: 0x0400279F RID: 10143
		public ApiSignUpResponse.Data data;

		// Token: 0x020009B7 RID: 2487
		[Serializable]
		public class Data
		{
			// Token: 0x040032CE RID: 13006
			public int user_id;

			// Token: 0x040032CF RID: 13007
			public string login_name;
		}
	}
}
