using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020004C2 RID: 1218
	public class ECDHBasicAgreement : IBasicAgreement
	{
		// Token: 0x06002D6D RID: 11629 RVA: 0x000EF30C File Offset: 0x000ED50C
		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			this.privKey = (ECPrivateKeyParameters)parameters;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000EF32F File Offset: 0x000ED52F
		public virtual int GetFieldSize()
		{
			return (this.privKey.Parameters.Curve.FieldSize + 7) / 8;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000EF34C File Offset: 0x000ED54C
		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)pubKey;
			if (!ecpublicKeyParameters.Parameters.Equals(this.privKey.Parameters))
			{
				throw new InvalidOperationException("ECDH public key has wrong domain parameters");
			}
			ECPoint ecpoint = ecpublicKeyParameters.Q.Multiply(this.privKey.D).Normalize();
			if (ecpoint.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid agreement value for ECDH");
			}
			return ecpoint.AffineXCoord.ToBigInteger();
		}

		// Token: 0x04001DDC RID: 7644
		protected internal ECPrivateKeyParameters privKey;
	}
}
