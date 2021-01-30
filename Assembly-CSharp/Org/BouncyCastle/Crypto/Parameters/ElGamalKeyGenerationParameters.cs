using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200042D RID: 1069
	public class ElGamalKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06002784 RID: 10116 RVA: 0x000C53D4 File Offset: 0x000C35D4
		public ElGamalKeyGenerationParameters(SecureRandom random, ElGamalParameters parameters) : base(random, ElGamalKeyGenerationParameters.GetStrength(parameters))
		{
			this.parameters = parameters;
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x000C53EA File Offset: 0x000C35EA
		public ElGamalParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000C53F2 File Offset: 0x000C35F2
		internal static int GetStrength(ElGamalParameters parameters)
		{
			if (parameters.L == 0)
			{
				return parameters.P.BitLength;
			}
			return parameters.L;
		}

		// Token: 0x04001A3F RID: 6719
		private readonly ElGamalParameters parameters;
	}
}
