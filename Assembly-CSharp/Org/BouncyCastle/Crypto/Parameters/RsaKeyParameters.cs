using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000446 RID: 1094
	public class RsaKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x060027EF RID: 10223 RVA: 0x000C5EF8 File Offset: 0x000C40F8
		public RsaKeyParameters(bool isPrivate, BigInteger modulus, BigInteger exponent) : base(isPrivate)
		{
			if (modulus == null)
			{
				throw new ArgumentNullException("modulus");
			}
			if (exponent == null)
			{
				throw new ArgumentNullException("exponent");
			}
			if (modulus.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA modulus", "modulus");
			}
			if (exponent.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA exponent", "exponent");
			}
			this.modulus = modulus;
			this.exponent = exponent;
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x000C5F68 File Offset: 0x000C4168
		public BigInteger Modulus
		{
			get
			{
				return this.modulus;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060027F1 RID: 10225 RVA: 0x000C5F70 File Offset: 0x000C4170
		public BigInteger Exponent
		{
			get
			{
				return this.exponent;
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000C5F78 File Offset: 0x000C4178
		public override bool Equals(object obj)
		{
			RsaKeyParameters rsaKeyParameters = obj as RsaKeyParameters;
			return rsaKeyParameters != null && (rsaKeyParameters.IsPrivate == base.IsPrivate && rsaKeyParameters.Modulus.Equals(this.modulus)) && rsaKeyParameters.Exponent.Equals(this.exponent);
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000C5FC8 File Offset: 0x000C41C8
		public override int GetHashCode()
		{
			return this.modulus.GetHashCode() ^ this.exponent.GetHashCode() ^ base.IsPrivate.GetHashCode();
		}

		// Token: 0x04001A6D RID: 6765
		private readonly BigInteger modulus;

		// Token: 0x04001A6E RID: 6766
		private readonly BigInteger exponent;
	}
}
