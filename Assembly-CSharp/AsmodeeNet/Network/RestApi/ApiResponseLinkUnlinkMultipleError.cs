using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A6 RID: 1702
	[Serializable]
	public class ApiResponseLinkUnlinkMultipleError : ApiResponseError
	{
		// Token: 0x04002784 RID: 10116
		public ApiResponseLinkUnlinkMultipleError.Details error_details;

		// Token: 0x020009AF RID: 2479
		[Serializable]
		public class Details
		{
			// Token: 0x040032BE RID: 12990
			public ApiResponseLinkUnlinkMultipleError.Details.AddDetails[] add;

			// Token: 0x040032BF RID: 12991
			public ApiResponseLinkUnlinkMultipleError.Details.RemoveDetails[] remove;

			// Token: 0x02000A6D RID: 2669
			[Serializable]
			public class RemoveDetails
			{
				// Token: 0x0400350B RID: 13579
				public int partner;

				// Token: 0x0400350C RID: 13580
				public string partner_user;

				// Token: 0x0400350D RID: 13581
				public string error_code;

				// Token: 0x0400350E RID: 13582
				public string error_description;
			}

			// Token: 0x02000A6E RID: 2670
			[Serializable]
			public class AddDetails : ApiResponseLinkUnlinkMultipleError.Details.RemoveDetails
			{
				// Token: 0x0400350F RID: 13583
				public ApiResponseLinkUnlinkMultipleError.Details.AddDetails.ExtraDetails error_details;

				// Token: 0x02000A7A RID: 2682
				[Serializable]
				public class ExtraDetails
				{
					// Token: 0x04003551 RID: 13649
					public int conflict_id;

					// Token: 0x04003552 RID: 13650
					public string conflict_login;
				}
			}
		}
	}
}
