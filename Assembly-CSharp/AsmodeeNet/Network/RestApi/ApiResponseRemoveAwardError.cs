using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A5 RID: 1701
	[Serializable]
	public class ApiResponseRemoveAwardError : ApiResponseError
	{
		// Token: 0x04002783 RID: 10115
		public ApiResponseRemoveAwardError.Details error_details;

		// Token: 0x020009AE RID: 2478
		[Serializable]
		public class Details
		{
			// Token: 0x040032BC RID: 12988
			public string[] not_owned_awards;

			// Token: 0x040032BD RID: 12989
			public string[] invalid_awards;
		}
	}
}
