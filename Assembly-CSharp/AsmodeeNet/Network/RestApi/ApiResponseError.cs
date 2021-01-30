using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A4 RID: 1700
	[Serializable]
	public class ApiResponseError : WebError
	{
		// Token: 0x04002781 RID: 10113
		public string error_description;

		// Token: 0x04002782 RID: 10114
		public string error_code;
	}
}
