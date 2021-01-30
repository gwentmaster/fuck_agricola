using System;

namespace Helpshift
{
	// Token: 0x02000254 RID: 596
	public class HelpshiftUser
	{
		// Token: 0x060012F9 RID: 4857 RVA: 0x00071EB3 File Offset: 0x000700B3
		private HelpshiftUser(string identifier, string email, string name, string authToken)
		{
			this.identifier = identifier;
			this.email = email;
			this.name = name;
			this.authToken = authToken;
		}

		// Token: 0x040012E4 RID: 4836
		public readonly string identifier;

		// Token: 0x040012E5 RID: 4837
		public readonly string email;

		// Token: 0x040012E6 RID: 4838
		public readonly string name;

		// Token: 0x040012E7 RID: 4839
		public readonly string authToken;

		// Token: 0x02000875 RID: 2165
		public sealed class Builder
		{
			// Token: 0x060044FC RID: 17660 RVA: 0x00143459 File Offset: 0x00141659
			public Builder(string identifier, string email)
			{
				this.email = email;
				this.identifier = identifier;
			}

			// Token: 0x060044FD RID: 17661 RVA: 0x0014346F File Offset: 0x0014166F
			public HelpshiftUser.Builder setName(string name)
			{
				this.name = name;
				return this;
			}

			// Token: 0x060044FE RID: 17662 RVA: 0x00143479 File Offset: 0x00141679
			public HelpshiftUser.Builder setAuthToken(string authToken)
			{
				this.authToken = authToken;
				return this;
			}

			// Token: 0x060044FF RID: 17663 RVA: 0x00143483 File Offset: 0x00141683
			public HelpshiftUser build()
			{
				return new HelpshiftUser(this.identifier, this.email, this.name, this.authToken);
			}

			// Token: 0x04002F12 RID: 12050
			private string identifier;

			// Token: 0x04002F13 RID: 12051
			private string email;

			// Token: 0x04002F14 RID: 12052
			private string name;

			// Token: 0x04002F15 RID: 12053
			private string authToken;
		}
	}
}
