using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020004C1 RID: 1217
	public class DHBasicAgreement : IBasicAgreement
	{
		// Token: 0x06002D69 RID: 11625 RVA: 0x000EF238 File Offset: 0x000ED438
		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			if (!(parameters is DHPrivateKeyParameters))
			{
				throw new ArgumentException("DHEngine expects DHPrivateKeyParameters");
			}
			this.key = (DHPrivateKeyParameters)parameters;
			this.dhParams = this.key.Parameters;
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000EF28A File Offset: 0x000ED48A
		public virtual int GetFieldSize()
		{
			return (this.key.Parameters.P.BitLength + 7) / 8;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000EF2A8 File Offset: 0x000ED4A8
		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("Agreement algorithm not initialised");
			}
			DHPublicKeyParameters dhpublicKeyParameters = (DHPublicKeyParameters)pubKey;
			if (!dhpublicKeyParameters.Parameters.Equals(this.dhParams))
			{
				throw new ArgumentException("Diffie-Hellman public key has wrong parameters.");
			}
			return dhpublicKeyParameters.Y.ModPow(this.key.X, this.dhParams.P);
		}

		// Token: 0x04001DDA RID: 7642
		private DHPrivateKeyParameters key;

		// Token: 0x04001DDB RID: 7643
		private DHParameters dhParams;
	}
}
