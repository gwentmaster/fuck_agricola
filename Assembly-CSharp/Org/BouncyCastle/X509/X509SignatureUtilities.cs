using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;

namespace Org.BouncyCastle.X509
{
	// Token: 0x02000281 RID: 641
	internal class X509SignatureUtilities
	{
		// Token: 0x0600151C RID: 5404 RVA: 0x00078B7E File Offset: 0x00076D7E
		internal static void SetSignatureParameters(ISigner signature, Asn1Encodable parameters)
		{
			if (parameters != null)
			{
				X509SignatureUtilities.derNull.Equals(parameters);
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00078B90 File Offset: 0x00076D90
		internal static string GetSignatureName(AlgorithmIdentifier sigAlgId)
		{
			Asn1Encodable parameters = sigAlgId.Parameters;
			if (parameters != null && !X509SignatureUtilities.derNull.Equals(parameters))
			{
				if (sigAlgId.Algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss))
				{
					return X509SignatureUtilities.GetDigestAlgName(RsassaPssParameters.GetInstance(parameters).HashAlgorithm.Algorithm) + "withRSAandMGF1";
				}
				if (sigAlgId.Algorithm.Equals(X9ObjectIdentifiers.ECDsaWithSha2))
				{
					return X509SignatureUtilities.GetDigestAlgName((DerObjectIdentifier)Asn1Sequence.GetInstance(parameters)[0]) + "withECDSA";
				}
			}
			return sigAlgId.Algorithm.Id;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00078C24 File Offset: 0x00076E24
		private static string GetDigestAlgName(DerObjectIdentifier digestAlgOID)
		{
			if (PkcsObjectIdentifiers.MD5.Equals(digestAlgOID))
			{
				return "MD5";
			}
			if (OiwObjectIdentifiers.IdSha1.Equals(digestAlgOID))
			{
				return "SHA1";
			}
			if (NistObjectIdentifiers.IdSha224.Equals(digestAlgOID))
			{
				return "SHA224";
			}
			if (NistObjectIdentifiers.IdSha256.Equals(digestAlgOID))
			{
				return "SHA256";
			}
			if (NistObjectIdentifiers.IdSha384.Equals(digestAlgOID))
			{
				return "SHA384";
			}
			if (NistObjectIdentifiers.IdSha512.Equals(digestAlgOID))
			{
				return "SHA512";
			}
			if (TeleTrusTObjectIdentifiers.RipeMD128.Equals(digestAlgOID))
			{
				return "RIPEMD128";
			}
			if (TeleTrusTObjectIdentifiers.RipeMD160.Equals(digestAlgOID))
			{
				return "RIPEMD160";
			}
			if (TeleTrusTObjectIdentifiers.RipeMD256.Equals(digestAlgOID))
			{
				return "RIPEMD256";
			}
			if (CryptoProObjectIdentifiers.GostR3411.Equals(digestAlgOID))
			{
				return "GOST3411";
			}
			return digestAlgOID.Id;
		}

		// Token: 0x04001393 RID: 5011
		private static readonly Asn1Null derNull = DerNull.Instance;
	}
}
