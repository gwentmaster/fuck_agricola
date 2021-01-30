using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200043D RID: 1085
	public class MqvPublicParameters : ICipherParameters
	{
		// Token: 0x060027CD RID: 10189 RVA: 0x000C5C08 File Offset: 0x000C3E08
		public MqvPublicParameters(ECPublicKeyParameters staticPublicKey, ECPublicKeyParameters ephemeralPublicKey)
		{
			if (staticPublicKey == null)
			{
				throw new ArgumentNullException("staticPublicKey");
			}
			if (ephemeralPublicKey == null)
			{
				throw new ArgumentNullException("ephemeralPublicKey");
			}
			if (!staticPublicKey.Parameters.Equals(ephemeralPublicKey.Parameters))
			{
				throw new ArgumentException("Static and ephemeral public keys have different domain parameters");
			}
			this.staticPublicKey = staticPublicKey;
			this.ephemeralPublicKey = ephemeralPublicKey;
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060027CE RID: 10190 RVA: 0x000C5C63 File Offset: 0x000C3E63
		public virtual ECPublicKeyParameters StaticPublicKey
		{
			get
			{
				return this.staticPublicKey;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060027CF RID: 10191 RVA: 0x000C5C6B File Offset: 0x000C3E6B
		public virtual ECPublicKeyParameters EphemeralPublicKey
		{
			get
			{
				return this.ephemeralPublicKey;
			}
		}

		// Token: 0x04001A5D RID: 6749
		private readonly ECPublicKeyParameters staticPublicKey;

		// Token: 0x04001A5E RID: 6750
		private readonly ECPublicKeyParameters ephemeralPublicKey;
	}
}
