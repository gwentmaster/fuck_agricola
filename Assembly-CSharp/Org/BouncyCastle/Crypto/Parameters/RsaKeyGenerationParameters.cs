using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000445 RID: 1093
	public class RsaKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x060027EA RID: 10218 RVA: 0x000C5E6A File Offset: 0x000C406A
		public RsaKeyGenerationParameters(BigInteger publicExponent, SecureRandom random, int strength, int certainty) : base(random, strength)
		{
			this.publicExponent = publicExponent;
			this.certainty = certainty;
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060027EB RID: 10219 RVA: 0x000C5E83 File Offset: 0x000C4083
		public BigInteger PublicExponent
		{
			get
			{
				return this.publicExponent;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x000C5E8B File Offset: 0x000C408B
		public int Certainty
		{
			get
			{
				return this.certainty;
			}
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000C5E94 File Offset: 0x000C4094
		public override bool Equals(object obj)
		{
			RsaKeyGenerationParameters rsaKeyGenerationParameters = obj as RsaKeyGenerationParameters;
			return rsaKeyGenerationParameters != null && this.certainty == rsaKeyGenerationParameters.certainty && this.publicExponent.Equals(rsaKeyGenerationParameters.publicExponent);
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000C5ED0 File Offset: 0x000C40D0
		public override int GetHashCode()
		{
			return this.certainty.GetHashCode() ^ this.publicExponent.GetHashCode();
		}

		// Token: 0x04001A6B RID: 6763
		private readonly BigInteger publicExponent;

		// Token: 0x04001A6C RID: 6764
		private readonly int certainty;
	}
}
