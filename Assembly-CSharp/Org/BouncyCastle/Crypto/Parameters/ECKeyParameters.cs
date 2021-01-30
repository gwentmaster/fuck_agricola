using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200042A RID: 1066
	public abstract class ECKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06002768 RID: 10088 RVA: 0x000C502C File Offset: 0x000C322C
		protected ECKeyParameters(string algorithm, bool isPrivate, ECDomainParameters parameters) : base(isPrivate)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.algorithm = ECKeyParameters.VerifyAlgorithmName(algorithm);
			this.parameters = parameters;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000C5064 File Offset: 0x000C3264
		protected ECKeyParameters(string algorithm, bool isPrivate, DerObjectIdentifier publicKeyParamSet) : base(isPrivate)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			this.algorithm = ECKeyParameters.VerifyAlgorithmName(algorithm);
			this.parameters = ECKeyParameters.LookupParameters(publicKeyParamSet);
			this.publicKeyParamSet = publicKeyParamSet;
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x0600276A RID: 10090 RVA: 0x000C50B3 File Offset: 0x000C32B3
		public string AlgorithmName
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x000C50BB File Offset: 0x000C32BB
		public ECDomainParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600276C RID: 10092 RVA: 0x000C50C3 File Offset: 0x000C32C3
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000C50CC File Offset: 0x000C32CC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECDomainParameters ecdomainParameters = obj as ECDomainParameters;
			return ecdomainParameters != null && this.Equals(ecdomainParameters);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000C50F2 File Offset: 0x000C32F2
		protected bool Equals(ECKeyParameters other)
		{
			return this.parameters.Equals(other.parameters) && base.Equals(other);
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000C5110 File Offset: 0x000C3310
		public override int GetHashCode()
		{
			return this.parameters.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000C5124 File Offset: 0x000C3324
		internal ECKeyGenerationParameters CreateKeyGenerationParameters(SecureRandom random)
		{
			if (this.publicKeyParamSet != null)
			{
				return new ECKeyGenerationParameters(this.publicKeyParamSet, random);
			}
			return new ECKeyGenerationParameters(this.parameters, random);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000C5147 File Offset: 0x000C3347
		internal static string VerifyAlgorithmName(string algorithm)
		{
			string result = Platform.ToUpperInvariant(algorithm);
			if (Array.IndexOf<string>(ECKeyParameters.algorithms, algorithm, 0, ECKeyParameters.algorithms.Length) < 0)
			{
				throw new ArgumentException("unrecognised algorithm: " + algorithm, "algorithm");
			}
			return result;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000C517C File Offset: 0x000C337C
		internal static ECDomainParameters LookupParameters(DerObjectIdentifier publicKeyParamSet)
		{
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			ECDomainParameters ecdomainParameters = ECGost3410NamedCurves.GetByOid(publicKeyParamSet);
			if (ecdomainParameters == null)
			{
				X9ECParameters x9ECParameters = ECKeyPairGenerator.FindECCurveByOid(publicKeyParamSet);
				if (x9ECParameters == null)
				{
					throw new ArgumentException("OID is not a valid public key parameter set", "publicKeyParamSet");
				}
				ecdomainParameters = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N, x9ECParameters.H, x9ECParameters.GetSeed());
			}
			return ecdomainParameters;
		}

		// Token: 0x04001A39 RID: 6713
		private static readonly string[] algorithms = new string[]
		{
			"EC",
			"ECDSA",
			"ECDH",
			"ECDHC",
			"ECGOST3410",
			"ECMQV"
		};

		// Token: 0x04001A3A RID: 6714
		private readonly string algorithm;

		// Token: 0x04001A3B RID: 6715
		private readonly ECDomainParameters parameters;

		// Token: 0x04001A3C RID: 6716
		private readonly DerObjectIdentifier publicKeyParamSet;
	}
}
