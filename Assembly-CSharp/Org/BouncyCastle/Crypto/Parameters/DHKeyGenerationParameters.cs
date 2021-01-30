using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200041A RID: 1050
	public class DHKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x060026F9 RID: 9977 RVA: 0x000C42D6 File Offset: 0x000C24D6
		public DHKeyGenerationParameters(SecureRandom random, DHParameters parameters) : base(random, DHKeyGenerationParameters.GetStrength(parameters))
		{
			this.parameters = parameters;
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060026FA RID: 9978 RVA: 0x000C42EC File Offset: 0x000C24EC
		public DHParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000C42F4 File Offset: 0x000C24F4
		internal static int GetStrength(DHParameters parameters)
		{
			if (parameters.L == 0)
			{
				return parameters.P.BitLength;
			}
			return parameters.L;
		}

		// Token: 0x04001A14 RID: 6676
		private readonly DHParameters parameters;
	}
}
