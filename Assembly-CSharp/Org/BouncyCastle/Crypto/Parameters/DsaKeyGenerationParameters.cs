using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000422 RID: 1058
	public class DsaKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06002737 RID: 10039 RVA: 0x000C4AD0 File Offset: 0x000C2CD0
		public DsaKeyGenerationParameters(SecureRandom random, DsaParameters parameters) : base(random, parameters.P.BitLength - 1)
		{
			this.parameters = parameters;
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x000C4AED File Offset: 0x000C2CED
		public DsaParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04001A27 RID: 6695
		private readonly DsaParameters parameters;
	}
}
