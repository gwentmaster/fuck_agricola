using System;
using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000450 RID: 1104
	internal class X509Utilities
	{
		// Token: 0x06002828 RID: 10280 RVA: 0x000C6830 File Offset: 0x000C4A30
		static X509Utilities()
		{
			X509Utilities.algorithms.Add("MD2WITHRSAENCRYPTION", PkcsObjectIdentifiers.MD2WithRsaEncryption);
			X509Utilities.algorithms.Add("MD2WITHRSA", PkcsObjectIdentifiers.MD2WithRsaEncryption);
			X509Utilities.algorithms.Add("MD5WITHRSAENCRYPTION", PkcsObjectIdentifiers.MD5WithRsaEncryption);
			X509Utilities.algorithms.Add("MD5WITHRSA", PkcsObjectIdentifiers.MD5WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA1WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha1WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA1WITHRSA", PkcsObjectIdentifiers.Sha1WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA224WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha224WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA224WITHRSA", PkcsObjectIdentifiers.Sha224WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA256WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha256WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA256WITHRSA", PkcsObjectIdentifiers.Sha256WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA384WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha384WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA384WITHRSA", PkcsObjectIdentifiers.Sha384WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA512WITHRSAENCRYPTION", PkcsObjectIdentifiers.Sha512WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA512WITHRSA", PkcsObjectIdentifiers.Sha512WithRsaEncryption);
			X509Utilities.algorithms.Add("SHA1WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			X509Utilities.algorithms.Add("SHA224WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			X509Utilities.algorithms.Add("SHA256WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			X509Utilities.algorithms.Add("SHA384WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			X509Utilities.algorithms.Add("SHA512WITHRSAANDMGF1", PkcsObjectIdentifiers.IdRsassaPss);
			X509Utilities.algorithms.Add("RIPEMD160WITHRSAENCRYPTION", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD160);
			X509Utilities.algorithms.Add("RIPEMD160WITHRSA", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD160);
			X509Utilities.algorithms.Add("RIPEMD128WITHRSAENCRYPTION", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD128);
			X509Utilities.algorithms.Add("RIPEMD128WITHRSA", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD128);
			X509Utilities.algorithms.Add("RIPEMD256WITHRSAENCRYPTION", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD256);
			X509Utilities.algorithms.Add("RIPEMD256WITHRSA", TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD256);
			X509Utilities.algorithms.Add("SHA1WITHDSA", X9ObjectIdentifiers.IdDsaWithSha1);
			X509Utilities.algorithms.Add("DSAWITHSHA1", X9ObjectIdentifiers.IdDsaWithSha1);
			X509Utilities.algorithms.Add("SHA224WITHDSA", NistObjectIdentifiers.DsaWithSha224);
			X509Utilities.algorithms.Add("SHA256WITHDSA", NistObjectIdentifiers.DsaWithSha256);
			X509Utilities.algorithms.Add("SHA384WITHDSA", NistObjectIdentifiers.DsaWithSha384);
			X509Utilities.algorithms.Add("SHA512WITHDSA", NistObjectIdentifiers.DsaWithSha512);
			X509Utilities.algorithms.Add("SHA1WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha1);
			X509Utilities.algorithms.Add("ECDSAWITHSHA1", X9ObjectIdentifiers.ECDsaWithSha1);
			X509Utilities.algorithms.Add("SHA224WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha224);
			X509Utilities.algorithms.Add("SHA256WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha256);
			X509Utilities.algorithms.Add("SHA384WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha384);
			X509Utilities.algorithms.Add("SHA512WITHECDSA", X9ObjectIdentifiers.ECDsaWithSha512);
			X509Utilities.algorithms.Add("GOST3411WITHGOST3410", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94);
			X509Utilities.algorithms.Add("GOST3411WITHGOST3410-94", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94);
			X509Utilities.algorithms.Add("GOST3411WITHECGOST3410", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);
			X509Utilities.algorithms.Add("GOST3411WITHECGOST3410-2001", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);
			X509Utilities.algorithms.Add("GOST3411WITHGOST3410-2001", CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);
			X509Utilities.noParams.Add(X9ObjectIdentifiers.ECDsaWithSha1);
			X509Utilities.noParams.Add(X9ObjectIdentifiers.ECDsaWithSha224);
			X509Utilities.noParams.Add(X9ObjectIdentifiers.ECDsaWithSha256);
			X509Utilities.noParams.Add(X9ObjectIdentifiers.ECDsaWithSha384);
			X509Utilities.noParams.Add(X9ObjectIdentifiers.ECDsaWithSha512);
			X509Utilities.noParams.Add(X9ObjectIdentifiers.IdDsaWithSha1);
			X509Utilities.noParams.Add(NistObjectIdentifiers.DsaWithSha224);
			X509Utilities.noParams.Add(NistObjectIdentifiers.DsaWithSha256);
			X509Utilities.noParams.Add(NistObjectIdentifiers.DsaWithSha384);
			X509Utilities.noParams.Add(NistObjectIdentifiers.DsaWithSha512);
			X509Utilities.noParams.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94);
			X509Utilities.noParams.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001);
			AlgorithmIdentifier hashAlgId = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance);
			X509Utilities.exParams.Add("SHA1WITHRSAANDMGF1", X509Utilities.CreatePssParams(hashAlgId, 20));
			AlgorithmIdentifier hashAlgId2 = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha224, DerNull.Instance);
			X509Utilities.exParams.Add("SHA224WITHRSAANDMGF1", X509Utilities.CreatePssParams(hashAlgId2, 28));
			AlgorithmIdentifier hashAlgId3 = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha256, DerNull.Instance);
			X509Utilities.exParams.Add("SHA256WITHRSAANDMGF1", X509Utilities.CreatePssParams(hashAlgId3, 32));
			AlgorithmIdentifier hashAlgId4 = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha384, DerNull.Instance);
			X509Utilities.exParams.Add("SHA384WITHRSAANDMGF1", X509Utilities.CreatePssParams(hashAlgId4, 48));
			AlgorithmIdentifier hashAlgId5 = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha512, DerNull.Instance);
			X509Utilities.exParams.Add("SHA512WITHRSAANDMGF1", X509Utilities.CreatePssParams(hashAlgId5, 64));
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000C6D28 File Offset: 0x000C4F28
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

		// Token: 0x0600282A RID: 10282 RVA: 0x000C6DFC File Offset: 0x000C4FFC
		internal static string GetSignatureName(AlgorithmIdentifier sigAlgId)
		{
			Asn1Encodable parameters = sigAlgId.Parameters;
			if (parameters != null && !X509Utilities.derNull.Equals(parameters))
			{
				if (sigAlgId.Algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss))
				{
					return X509Utilities.GetDigestAlgName(RsassaPssParameters.GetInstance(parameters).HashAlgorithm.Algorithm) + "withRSAandMGF1";
				}
				if (sigAlgId.Algorithm.Equals(X9ObjectIdentifiers.ECDsaWithSha2))
				{
					return X509Utilities.GetDigestAlgName((DerObjectIdentifier)Asn1Sequence.GetInstance(parameters)[0]) + "withECDSA";
				}
			}
			return sigAlgId.Algorithm.Id;
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000C6E90 File Offset: 0x000C5090
		private static RsassaPssParameters CreatePssParams(AlgorithmIdentifier hashAlgId, int saltSize)
		{
			return new RsassaPssParameters(hashAlgId, new AlgorithmIdentifier(PkcsObjectIdentifiers.IdMgf1, hashAlgId), new DerInteger(saltSize), new DerInteger(1));
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000C6EAF File Offset: 0x000C50AF
		internal static DerObjectIdentifier GetAlgorithmOid(string algorithmName)
		{
			algorithmName = Platform.ToUpperInvariant(algorithmName);
			if (X509Utilities.algorithms.Contains(algorithmName))
			{
				return (DerObjectIdentifier)X509Utilities.algorithms[algorithmName];
			}
			return new DerObjectIdentifier(algorithmName);
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000C6EE0 File Offset: 0x000C50E0
		internal static AlgorithmIdentifier GetSigAlgID(DerObjectIdentifier sigOid, string algorithmName)
		{
			if (X509Utilities.noParams.Contains(sigOid))
			{
				return new AlgorithmIdentifier(sigOid);
			}
			algorithmName = Platform.ToUpperInvariant(algorithmName);
			if (X509Utilities.exParams.Contains(algorithmName))
			{
				return new AlgorithmIdentifier(sigOid, (Asn1Encodable)X509Utilities.exParams[algorithmName]);
			}
			return new AlgorithmIdentifier(sigOid, DerNull.Instance);
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000C6F38 File Offset: 0x000C5138
		internal static IEnumerable GetAlgNames()
		{
			return new EnumerableProxy(X509Utilities.algorithms.Keys);
		}

		// Token: 0x04001A78 RID: 6776
		private static readonly Asn1Null derNull = DerNull.Instance;

		// Token: 0x04001A79 RID: 6777
		private static readonly IDictionary algorithms = Platform.CreateHashtable();

		// Token: 0x04001A7A RID: 6778
		private static readonly IDictionary exParams = Platform.CreateHashtable();

		// Token: 0x04001A7B RID: 6779
		private static readonly ISet noParams = new HashSet();
	}
}
