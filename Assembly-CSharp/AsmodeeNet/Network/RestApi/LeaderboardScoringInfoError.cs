using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006B6 RID: 1718
	public class LeaderboardScoringInfoError : WebError
	{
		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x0012EC44 File Offset: 0x0012CE44
		// (set) Token: 0x06003DA8 RID: 15784 RVA: 0x0012EC4C File Offset: 0x0012CE4C
		public Builder<LeaderboardScoringInfo>.BuilderErrors[] BuildingErrors { get; private set; }

		// Token: 0x06003DA9 RID: 15785 RVA: 0x0012EC55 File Offset: 0x0012CE55
		public LeaderboardScoringInfoError(Builder<LeaderboardScoringInfo>.BuilderErrors[] errors)
		{
			this.status = -1;
			this.error = "Error when building the result.";
			this.BuildingErrors = errors;
		}
	}
}
