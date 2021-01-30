using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006EC RID: 1772
	public class UserError : WebError
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06003EA9 RID: 16041 RVA: 0x001325F0 File Offset: 0x001307F0
		// (set) Token: 0x06003EAA RID: 16042 RVA: 0x001325F8 File Offset: 0x001307F8
		public Builder<User>.BuilderErrors[] BuildingErrors { get; private set; }

		// Token: 0x06003EAB RID: 16043 RVA: 0x00132601 File Offset: 0x00130801
		public UserError(Builder<User>.BuilderErrors[] errors)
		{
			this.status = -1;
			this.error = "Error when building the result.";
			this.BuildingErrors = errors;
		}
	}
}
