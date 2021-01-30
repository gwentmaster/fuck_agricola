using System;
using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002B3 RID: 691
	public sealed class DigestUtilities
	{
		// Token: 0x060016CF RID: 5839 RVA: 0x00003425 File Offset: 0x00001625
		private DigestUtilities()
		{
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00081BFC File Offset: 0x0007FDFC
		static DigestUtilities()
		{
			((DigestUtilities.DigestAlgorithm)Enums.GetArbitraryValue(typeof(DigestUtilities.DigestAlgorithm))).ToString();
			DigestUtilities.algorithms[PkcsObjectIdentifiers.MD2.Id] = "MD2";
			DigestUtilities.algorithms[PkcsObjectIdentifiers.MD4.Id] = "MD4";
			DigestUtilities.algorithms[PkcsObjectIdentifiers.MD5.Id] = "MD5";
			DigestUtilities.algorithms["SHA1"] = "SHA-1";
			DigestUtilities.algorithms[OiwObjectIdentifiers.IdSha1.Id] = "SHA-1";
			DigestUtilities.algorithms["SHA224"] = "SHA-224";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha224.Id] = "SHA-224";
			DigestUtilities.algorithms["SHA256"] = "SHA-256";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha256.Id] = "SHA-256";
			DigestUtilities.algorithms["SHA384"] = "SHA-384";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha384.Id] = "SHA-384";
			DigestUtilities.algorithms["SHA512"] = "SHA-512";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha512.Id] = "SHA-512";
			DigestUtilities.algorithms["SHA512/224"] = "SHA-512/224";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha512_224.Id] = "SHA-512/224";
			DigestUtilities.algorithms["SHA512/256"] = "SHA-512/256";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha512_256.Id] = "SHA-512/256";
			DigestUtilities.algorithms["RIPEMD-128"] = "RIPEMD128";
			DigestUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD128.Id] = "RIPEMD128";
			DigestUtilities.algorithms["RIPEMD-160"] = "RIPEMD160";
			DigestUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD160.Id] = "RIPEMD160";
			DigestUtilities.algorithms["RIPEMD-256"] = "RIPEMD256";
			DigestUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD256.Id] = "RIPEMD256";
			DigestUtilities.algorithms["RIPEMD-320"] = "RIPEMD320";
			DigestUtilities.algorithms[CryptoProObjectIdentifiers.GostR3411.Id] = "GOST3411";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_224.Id] = "SHA3-224";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_256.Id] = "SHA3-256";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_384.Id] = "SHA3-384";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_512.Id] = "SHA3-512";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdShake128.Id] = "SHAKE128";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdShake256.Id] = "SHAKE256";
			DigestUtilities.oids["MD2"] = PkcsObjectIdentifiers.MD2;
			DigestUtilities.oids["MD4"] = PkcsObjectIdentifiers.MD4;
			DigestUtilities.oids["MD5"] = PkcsObjectIdentifiers.MD5;
			DigestUtilities.oids["SHA-1"] = OiwObjectIdentifiers.IdSha1;
			DigestUtilities.oids["SHA-224"] = NistObjectIdentifiers.IdSha224;
			DigestUtilities.oids["SHA-256"] = NistObjectIdentifiers.IdSha256;
			DigestUtilities.oids["SHA-384"] = NistObjectIdentifiers.IdSha384;
			DigestUtilities.oids["SHA-512"] = NistObjectIdentifiers.IdSha512;
			DigestUtilities.oids["SHA-512/224"] = NistObjectIdentifiers.IdSha512_224;
			DigestUtilities.oids["SHA-512/256"] = NistObjectIdentifiers.IdSha512_256;
			DigestUtilities.oids["SHA3-224"] = NistObjectIdentifiers.IdSha3_224;
			DigestUtilities.oids["SHA3-256"] = NistObjectIdentifiers.IdSha3_256;
			DigestUtilities.oids["SHA3-384"] = NistObjectIdentifiers.IdSha3_384;
			DigestUtilities.oids["SHA3-512"] = NistObjectIdentifiers.IdSha3_512;
			DigestUtilities.oids["SHAKE128"] = NistObjectIdentifiers.IdShake128;
			DigestUtilities.oids["SHAKE256"] = NistObjectIdentifiers.IdShake256;
			DigestUtilities.oids["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
			DigestUtilities.oids["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
			DigestUtilities.oids["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
			DigestUtilities.oids["GOST3411"] = CryptoProObjectIdentifiers.GostR3411;
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x000820A0 File Offset: 0x000802A0
		public static DerObjectIdentifier GetObjectIdentifier(string mechanism)
		{
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			mechanism = Platform.ToUpperInvariant(mechanism);
			string text = (string)DigestUtilities.algorithms[mechanism];
			if (text != null)
			{
				mechanism = text;
			}
			return (DerObjectIdentifier)DigestUtilities.oids[mechanism];
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x000820EA File Offset: 0x000802EA
		public static ICollection Algorithms
		{
			get
			{
				return DigestUtilities.oids.Keys;
			}
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x000820F6 File Offset: 0x000802F6
		public static IDigest GetDigest(DerObjectIdentifier id)
		{
			return DigestUtilities.GetDigest(id.Id);
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00082104 File Offset: 0x00080304
		public static IDigest GetDigest(string algorithm)
		{
			string text = Platform.ToUpperInvariant(algorithm);
			string text2 = (string)DigestUtilities.algorithms[text];
			if (text2 == null)
			{
				text2 = text;
			}
			try
			{
				switch ((DigestUtilities.DigestAlgorithm)Enums.GetEnumValue(typeof(DigestUtilities.DigestAlgorithm), text2))
				{
				case DigestUtilities.DigestAlgorithm.GOST3411:
					return new Gost3411Digest();
				case DigestUtilities.DigestAlgorithm.KECCAK_224:
					return new KeccakDigest(224);
				case DigestUtilities.DigestAlgorithm.KECCAK_256:
					return new KeccakDigest(256);
				case DigestUtilities.DigestAlgorithm.KECCAK_288:
					return new KeccakDigest(288);
				case DigestUtilities.DigestAlgorithm.KECCAK_384:
					return new KeccakDigest(384);
				case DigestUtilities.DigestAlgorithm.KECCAK_512:
					return new KeccakDigest(512);
				case DigestUtilities.DigestAlgorithm.MD2:
					return new MD2Digest();
				case DigestUtilities.DigestAlgorithm.MD4:
					return new MD4Digest();
				case DigestUtilities.DigestAlgorithm.MD5:
					return new MD5Digest();
				case DigestUtilities.DigestAlgorithm.RIPEMD128:
					return new RipeMD128Digest();
				case DigestUtilities.DigestAlgorithm.RIPEMD160:
					return new RipeMD160Digest();
				case DigestUtilities.DigestAlgorithm.RIPEMD256:
					return new RipeMD256Digest();
				case DigestUtilities.DigestAlgorithm.RIPEMD320:
					return new RipeMD320Digest();
				case DigestUtilities.DigestAlgorithm.SHA_1:
					return new Sha1Digest();
				case DigestUtilities.DigestAlgorithm.SHA_224:
					return new Sha224Digest();
				case DigestUtilities.DigestAlgorithm.SHA_256:
					return new Sha256Digest();
				case DigestUtilities.DigestAlgorithm.SHA_384:
					return new Sha384Digest();
				case DigestUtilities.DigestAlgorithm.SHA_512:
					return new Sha512Digest();
				case DigestUtilities.DigestAlgorithm.SHA_512_224:
					return new Sha512tDigest(224);
				case DigestUtilities.DigestAlgorithm.SHA_512_256:
					return new Sha512tDigest(256);
				case DigestUtilities.DigestAlgorithm.SHA3_224:
					return new Sha3Digest(224);
				case DigestUtilities.DigestAlgorithm.SHA3_256:
					return new Sha3Digest(256);
				case DigestUtilities.DigestAlgorithm.SHA3_384:
					return new Sha3Digest(384);
				case DigestUtilities.DigestAlgorithm.SHA3_512:
					return new Sha3Digest(512);
				case DigestUtilities.DigestAlgorithm.SHAKE128:
					return new ShakeDigest(128);
				case DigestUtilities.DigestAlgorithm.SHAKE256:
					return new ShakeDigest(256);
				case DigestUtilities.DigestAlgorithm.TIGER:
					return new TigerDigest();
				case DigestUtilities.DigestAlgorithm.WHIRLPOOL:
					return new WhirlpoolDigest();
				}
			}
			catch (ArgumentException)
			{
			}
			throw new SecurityUtilityException("Digest " + text2 + " not recognised.");
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00082354 File Offset: 0x00080554
		public static string GetAlgorithmName(DerObjectIdentifier oid)
		{
			return (string)DigestUtilities.algorithms[oid.Id];
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x0008236B File Offset: 0x0008056B
		public static byte[] CalculateDigest(string algorithm, byte[] input)
		{
			IDigest digest = DigestUtilities.GetDigest(algorithm);
			digest.BlockUpdate(input, 0, input.Length);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00082384 File Offset: 0x00080584
		public static byte[] DoFinal(IDigest digest)
		{
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			return array;
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000823A7 File Offset: 0x000805A7
		public static byte[] DoFinal(IDigest digest, byte[] input)
		{
			digest.BlockUpdate(input, 0, input.Length);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x0400151F RID: 5407
		private static readonly IDictionary algorithms = Platform.CreateHashtable();

		// Token: 0x04001520 RID: 5408
		private static readonly IDictionary oids = Platform.CreateHashtable();

		// Token: 0x02000880 RID: 2176
		private enum DigestAlgorithm
		{
			// Token: 0x04002F3E RID: 12094
			GOST3411,
			// Token: 0x04002F3F RID: 12095
			KECCAK_224,
			// Token: 0x04002F40 RID: 12096
			KECCAK_256,
			// Token: 0x04002F41 RID: 12097
			KECCAK_288,
			// Token: 0x04002F42 RID: 12098
			KECCAK_384,
			// Token: 0x04002F43 RID: 12099
			KECCAK_512,
			// Token: 0x04002F44 RID: 12100
			MD2,
			// Token: 0x04002F45 RID: 12101
			MD4,
			// Token: 0x04002F46 RID: 12102
			MD5,
			// Token: 0x04002F47 RID: 12103
			RIPEMD128,
			// Token: 0x04002F48 RID: 12104
			RIPEMD160,
			// Token: 0x04002F49 RID: 12105
			RIPEMD256,
			// Token: 0x04002F4A RID: 12106
			RIPEMD320,
			// Token: 0x04002F4B RID: 12107
			SHA_1,
			// Token: 0x04002F4C RID: 12108
			SHA_224,
			// Token: 0x04002F4D RID: 12109
			SHA_256,
			// Token: 0x04002F4E RID: 12110
			SHA_384,
			// Token: 0x04002F4F RID: 12111
			SHA_512,
			// Token: 0x04002F50 RID: 12112
			SHA_512_224,
			// Token: 0x04002F51 RID: 12113
			SHA_512_256,
			// Token: 0x04002F52 RID: 12114
			SHA3_224,
			// Token: 0x04002F53 RID: 12115
			SHA3_256,
			// Token: 0x04002F54 RID: 12116
			SHA3_384,
			// Token: 0x04002F55 RID: 12117
			SHA3_512,
			// Token: 0x04002F56 RID: 12118
			SHAKE128,
			// Token: 0x04002F57 RID: 12119
			SHAKE256,
			// Token: 0x04002F58 RID: 12120
			TIGER,
			// Token: 0x04002F59 RID: 12121
			WHIRLPOOL
		}
	}
}
