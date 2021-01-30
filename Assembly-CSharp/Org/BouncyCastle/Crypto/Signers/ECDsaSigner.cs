using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000408 RID: 1032
	public class ECDsaSigner : IDsa
	{
		// Token: 0x06002666 RID: 9830 RVA: 0x000C1458 File Offset: 0x000BF658
		public ECDsaSigner()
		{
			this.kCalculator = new RandomDsaKCalculator();
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000C146B File Offset: 0x000BF66B
		public ECDsaSigner(IDsaKCalculator kCalculator)
		{
			this.kCalculator = kCalculator;
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06002668 RID: 9832 RVA: 0x000C147A File Offset: 0x000BF67A
		public virtual string AlgorithmName
		{
			get
			{
				return "ECDSA";
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000C1484 File Offset: 0x000BF684
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			SecureRandom provided = null;
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
					provided = parametersWithRandom.Random;
					parameters = parametersWithRandom.Parameters;
				}
				if (!(parameters is ECPrivateKeyParameters))
				{
					throw new InvalidKeyException("EC private key required for signing");
				}
				this.key = (ECPrivateKeyParameters)parameters;
			}
			else
			{
				if (!(parameters is ECPublicKeyParameters))
				{
					throw new InvalidKeyException("EC public key required for verification");
				}
				this.key = (ECPublicKeyParameters)parameters;
			}
			this.random = this.InitSecureRandom(forSigning && !this.kCalculator.IsDeterministic, provided);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000C1514 File Offset: 0x000BF714
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			ECDomainParameters parameters = this.key.Parameters;
			BigInteger n = parameters.N;
			BigInteger bigInteger = this.CalculateE(n, message);
			BigInteger d = ((ECPrivateKeyParameters)this.key).D;
			if (this.kCalculator.IsDeterministic)
			{
				this.kCalculator.Init(n, d, message);
			}
			else
			{
				this.kCalculator.Init(n, this.random);
			}
			ECMultiplier ecmultiplier = this.CreateBasePointMultiplier();
			BigInteger bigInteger3;
			BigInteger bigInteger4;
			for (;;)
			{
				BigInteger bigInteger2 = this.kCalculator.NextK();
				bigInteger3 = ecmultiplier.Multiply(parameters.G, bigInteger2).Normalize().AffineXCoord.ToBigInteger().Mod(n);
				if (bigInteger3.SignValue != 0)
				{
					bigInteger4 = bigInteger2.ModInverse(n).Multiply(bigInteger.Add(d.Multiply(bigInteger3))).Mod(n);
					if (bigInteger4.SignValue != 0)
					{
						break;
					}
				}
			}
			return new BigInteger[]
			{
				bigInteger3,
				bigInteger4
			};
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000C1600 File Offset: 0x000BF800
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			BigInteger n = this.key.Parameters.N;
			if (r.SignValue < 1 || s.SignValue < 1 || r.CompareTo(n) >= 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			BigInteger bigInteger = this.CalculateE(n, message);
			BigInteger val = s.ModInverse(n);
			BigInteger a = bigInteger.Multiply(val).Mod(n);
			BigInteger b = r.Multiply(val).Mod(n);
			ECPoint g = this.key.Parameters.G;
			ECPoint q = ((ECPublicKeyParameters)this.key).Q;
			ECPoint ecpoint = ECAlgorithms.SumOfTwoMultiplies(g, a, q, b);
			if (ecpoint.IsInfinity)
			{
				return false;
			}
			ECCurve curve = ecpoint.Curve;
			if (curve != null)
			{
				BigInteger cofactor = curve.Cofactor;
				if (cofactor != null && cofactor.CompareTo(ECDsaSigner.Eight) <= 0)
				{
					ECFieldElement denominator = this.GetDenominator(curve.CoordinateSystem, ecpoint);
					if (denominator != null && !denominator.IsZero)
					{
						ECFieldElement xcoord = ecpoint.XCoord;
						while (curve.IsValidFieldElement(r))
						{
							if (curve.FromBigInteger(r).Multiply(denominator).Equals(xcoord))
							{
								return true;
							}
							r = r.Add(n);
						}
						return false;
					}
				}
			}
			return ecpoint.Normalize().AffineXCoord.ToBigInteger().Mod(n).Equals(r);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000C1748 File Offset: 0x000BF948
		protected virtual BigInteger CalculateE(BigInteger n, byte[] message)
		{
			int num = message.Length * 8;
			BigInteger bigInteger = new BigInteger(1, message);
			if (n.BitLength < num)
			{
				bigInteger = bigInteger.ShiftRight(num - n.BitLength);
			}
			return bigInteger;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000C177C File Offset: 0x000BF97C
		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000C1783 File Offset: 0x000BF983
		protected virtual ECFieldElement GetDenominator(int coordinateSystem, ECPoint p)
		{
			switch (coordinateSystem)
			{
			case 1:
			case 6:
			case 7:
				return p.GetZCoord(0);
			case 2:
			case 3:
			case 4:
				return p.GetZCoord(0).Square();
			}
			return null;
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000C1447 File Offset: 0x000BF647
		protected virtual SecureRandom InitSecureRandom(bool needed, SecureRandom provided)
		{
			if (!needed)
			{
				return null;
			}
			if (provided == null)
			{
				return new SecureRandom();
			}
			return provided;
		}

		// Token: 0x040019B0 RID: 6576
		private static readonly BigInteger Eight = BigInteger.ValueOf(8L);

		// Token: 0x040019B1 RID: 6577
		protected readonly IDsaKCalculator kCalculator;

		// Token: 0x040019B2 RID: 6578
		protected ECKeyParameters key;

		// Token: 0x040019B3 RID: 6579
		protected SecureRandom random;
	}
}
