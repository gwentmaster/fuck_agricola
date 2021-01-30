using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200043C RID: 1084
	public class MqvPrivateParameters : ICipherParameters
	{
		// Token: 0x060027C8 RID: 10184 RVA: 0x000C5B46 File Offset: 0x000C3D46
		public MqvPrivateParameters(ECPrivateKeyParameters staticPrivateKey, ECPrivateKeyParameters ephemeralPrivateKey) : this(staticPrivateKey, ephemeralPrivateKey, null)
		{
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x000C5B54 File Offset: 0x000C3D54
		public MqvPrivateParameters(ECPrivateKeyParameters staticPrivateKey, ECPrivateKeyParameters ephemeralPrivateKey, ECPublicKeyParameters ephemeralPublicKey)
		{
			if (staticPrivateKey == null)
			{
				throw new ArgumentNullException("staticPrivateKey");
			}
			if (ephemeralPrivateKey == null)
			{
				throw new ArgumentNullException("ephemeralPrivateKey");
			}
			ECDomainParameters parameters = staticPrivateKey.Parameters;
			if (!parameters.Equals(ephemeralPrivateKey.Parameters))
			{
				throw new ArgumentException("Static and ephemeral private keys have different domain parameters");
			}
			if (ephemeralPublicKey == null)
			{
				ephemeralPublicKey = new ECPublicKeyParameters(parameters.G.Multiply(ephemeralPrivateKey.D), parameters);
			}
			else if (!parameters.Equals(ephemeralPublicKey.Parameters))
			{
				throw new ArgumentException("Ephemeral public key has different domain parameters");
			}
			this.staticPrivateKey = staticPrivateKey;
			this.ephemeralPrivateKey = ephemeralPrivateKey;
			this.ephemeralPublicKey = ephemeralPublicKey;
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060027CA RID: 10186 RVA: 0x000C5BEF File Offset: 0x000C3DEF
		public virtual ECPrivateKeyParameters StaticPrivateKey
		{
			get
			{
				return this.staticPrivateKey;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060027CB RID: 10187 RVA: 0x000C5BF7 File Offset: 0x000C3DF7
		public virtual ECPrivateKeyParameters EphemeralPrivateKey
		{
			get
			{
				return this.ephemeralPrivateKey;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060027CC RID: 10188 RVA: 0x000C5BFF File Offset: 0x000C3DFF
		public virtual ECPublicKeyParameters EphemeralPublicKey
		{
			get
			{
				return this.ephemeralPublicKey;
			}
		}

		// Token: 0x04001A5A RID: 6746
		private readonly ECPrivateKeyParameters staticPrivateKey;

		// Token: 0x04001A5B RID: 6747
		private readonly ECPrivateKeyParameters ephemeralPrivateKey;

		// Token: 0x04001A5C RID: 6748
		private readonly ECPublicKeyParameters ephemeralPublicKey;
	}
}
