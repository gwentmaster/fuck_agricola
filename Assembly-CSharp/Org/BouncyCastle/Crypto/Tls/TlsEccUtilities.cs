using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.Field;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003EE RID: 1006
	public abstract class TlsEccUtilities
	{
		// Token: 0x060024C8 RID: 9416 RVA: 0x000BB861 File Offset: 0x000B9A61
		public static void AddSupportedEllipticCurvesExtension(IDictionary extensions, int[] namedCurves)
		{
			extensions[10] = TlsEccUtilities.CreateSupportedEllipticCurvesExtension(namedCurves);
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000BB876 File Offset: 0x000B9A76
		public static void AddSupportedPointFormatsExtension(IDictionary extensions, byte[] ecPointFormats)
		{
			extensions[11] = TlsEccUtilities.CreateSupportedPointFormatsExtension(ecPointFormats);
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000BB88C File Offset: 0x000B9A8C
		public static int[] GetSupportedEllipticCurvesExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 10);
			if (extensionData != null)
			{
				return TlsEccUtilities.ReadSupportedEllipticCurvesExtension(extensionData);
			}
			return null;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000BB8B0 File Offset: 0x000B9AB0
		public static byte[] GetSupportedPointFormatsExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 11);
			if (extensionData != null)
			{
				return TlsEccUtilities.ReadSupportedPointFormatsExtension(extensionData);
			}
			return null;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000BB8D1 File Offset: 0x000B9AD1
		public static byte[] CreateSupportedEllipticCurvesExtension(int[] namedCurves)
		{
			if (namedCurves == null || namedCurves.Length < 1)
			{
				throw new TlsFatalAlert(80);
			}
			return TlsUtilities.EncodeUint16ArrayWithUint16Length(namedCurves);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000BB8EA File Offset: 0x000B9AEA
		public static byte[] CreateSupportedPointFormatsExtension(byte[] ecPointFormats)
		{
			if (ecPointFormats == null || !Arrays.Contains(ecPointFormats, 0))
			{
				ecPointFormats = Arrays.Append(ecPointFormats, 0);
			}
			return TlsUtilities.EncodeUint8ArrayWithUint8Length(ecPointFormats);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000BB908 File Offset: 0x000B9B08
		public static int[] ReadSupportedEllipticCurvesExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			int num = TlsUtilities.ReadUint16(memoryStream);
			if (num < 2 || (num & 1) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			int[] result = TlsUtilities.ReadUint16Array(num / 2, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000BB954 File Offset: 0x000B9B54
		public static byte[] ReadSupportedPointFormatsExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			byte b = TlsUtilities.ReadUint8(memoryStream);
			if (b < 1)
			{
				throw new TlsFatalAlert(50);
			}
			byte[] array = TlsUtilities.ReadUint8Array((int)b, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			if (!Arrays.Contains(array, 0))
			{
				throw new TlsFatalAlert(47);
			}
			return array;
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000BB9A6 File Offset: 0x000B9BA6
		public static string GetNameOfNamedCurve(int namedCurve)
		{
			if (!TlsEccUtilities.IsSupportedNamedCurve(namedCurve))
			{
				return null;
			}
			return TlsEccUtilities.CurveNames[namedCurve - 1];
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000BB9BC File Offset: 0x000B9BBC
		public static ECDomainParameters GetParametersForNamedCurve(int namedCurve)
		{
			string nameOfNamedCurve = TlsEccUtilities.GetNameOfNamedCurve(namedCurve);
			if (nameOfNamedCurve == null)
			{
				return null;
			}
			X9ECParameters byName = CustomNamedCurves.GetByName(nameOfNamedCurve);
			if (byName == null)
			{
				byName = ECNamedCurveTable.GetByName(nameOfNamedCurve);
				if (byName == null)
				{
					return null;
				}
			}
			return new ECDomainParameters(byName.Curve, byName.G, byName.N, byName.H, byName.GetSeed());
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000BBA0E File Offset: 0x000B9C0E
		public static bool HasAnySupportedNamedCurves()
		{
			return TlsEccUtilities.CurveNames.Length != 0;
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000BBA1C File Offset: 0x000B9C1C
		public static bool ContainsEccCipherSuites(int[] cipherSuites)
		{
			for (int i = 0; i < cipherSuites.Length; i++)
			{
				if (TlsEccUtilities.IsEccCipherSuite(cipherSuites[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000BBA44 File Offset: 0x000B9C44
		public static bool IsEccCipherSuite(int cipherSuite)
		{
			if (cipherSuite <= 49307)
			{
				if (cipherSuite <= 49211)
				{
					if (cipherSuite - 49153 > 24 && cipherSuite - 49187 > 24)
					{
						return false;
					}
				}
				else if (cipherSuite - 49266 > 7 && cipherSuite - 49286 > 7 && cipherSuite - 49306 > 1)
				{
					return false;
				}
			}
			else if (cipherSuite <= 52393)
			{
				if (cipherSuite - 49324 > 3 && cipherSuite - 52392 > 1)
				{
					return false;
				}
			}
			else if (cipherSuite != 52396 && cipherSuite - 65282 > 3 && cipherSuite - 65300 > 1)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000BBAD6 File Offset: 0x000B9CD6
		public static bool AreOnSameCurve(ECDomainParameters a, ECDomainParameters b)
		{
			return a != null && a.Equals(b);
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x000BBAE4 File Offset: 0x000B9CE4
		public static bool IsSupportedNamedCurve(int namedCurve)
		{
			return namedCurve > 0 && namedCurve <= TlsEccUtilities.CurveNames.Length;
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000BBAFC File Offset: 0x000B9CFC
		public static bool IsCompressionPreferred(byte[] ecPointFormats, byte compressionFormat)
		{
			if (ecPointFormats == null)
			{
				return false;
			}
			foreach (byte b in ecPointFormats)
			{
				if (b == 0)
				{
					return false;
				}
				if (b == compressionFormat)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000BBB2C File Offset: 0x000B9D2C
		public static byte[] SerializeECFieldElement(int fieldSize, BigInteger x)
		{
			return BigIntegers.AsUnsignedByteArray((fieldSize + 7) / 8, x);
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000BBB3C File Offset: 0x000B9D3C
		public static byte[] SerializeECPoint(byte[] ecPointFormats, ECPoint point)
		{
			ECCurve curve = point.Curve;
			bool compressed = false;
			if (ECAlgorithms.IsFpCurve(curve))
			{
				compressed = TlsEccUtilities.IsCompressionPreferred(ecPointFormats, 1);
			}
			else if (ECAlgorithms.IsF2mCurve(curve))
			{
				compressed = TlsEccUtilities.IsCompressionPreferred(ecPointFormats, 2);
			}
			return point.GetEncoded(compressed);
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x000BBB7B File Offset: 0x000B9D7B
		public static byte[] SerializeECPublicKey(byte[] ecPointFormats, ECPublicKeyParameters keyParameters)
		{
			return TlsEccUtilities.SerializeECPoint(ecPointFormats, keyParameters.Q);
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x000BBB8C File Offset: 0x000B9D8C
		public static BigInteger DeserializeECFieldElement(int fieldSize, byte[] encoding)
		{
			int num = (fieldSize + 7) / 8;
			if (encoding.Length != num)
			{
				throw new TlsFatalAlert(50);
			}
			return new BigInteger(1, encoding);
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000BBBB4 File Offset: 0x000B9DB4
		public static ECPoint DeserializeECPoint(byte[] ecPointFormats, ECCurve curve, byte[] encoding)
		{
			if (encoding == null || encoding.Length < 1)
			{
				throw new TlsFatalAlert(47);
			}
			byte b;
			switch (encoding[0])
			{
			case 2:
			case 3:
				if (ECAlgorithms.IsF2mCurve(curve))
				{
					b = 2;
					goto IL_69;
				}
				if (ECAlgorithms.IsFpCurve(curve))
				{
					b = 1;
					goto IL_69;
				}
				throw new TlsFatalAlert(47);
			case 4:
				b = 0;
				goto IL_69;
			}
			throw new TlsFatalAlert(47);
			IL_69:
			if (b != 0 && (ecPointFormats == null || !Arrays.Contains(ecPointFormats, b)))
			{
				throw new TlsFatalAlert(47);
			}
			return curve.DecodePoint(encoding);
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000BBC48 File Offset: 0x000B9E48
		public static ECPublicKeyParameters DeserializeECPublicKey(byte[] ecPointFormats, ECDomainParameters curve_params, byte[] encoding)
		{
			ECPublicKeyParameters result;
			try
			{
				result = new ECPublicKeyParameters(TlsEccUtilities.DeserializeECPoint(ecPointFormats, curve_params.Curve, encoding), curve_params);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			return result;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000BBC88 File Offset: 0x000B9E88
		public static byte[] CalculateECDHBasicAgreement(ECPublicKeyParameters publicKey, ECPrivateKeyParameters privateKey)
		{
			ECDHBasicAgreement ecdhbasicAgreement = new ECDHBasicAgreement();
			ecdhbasicAgreement.Init(privateKey);
			BigInteger n = ecdhbasicAgreement.CalculateAgreement(publicKey);
			return BigIntegers.AsUnsignedByteArray(ecdhbasicAgreement.GetFieldSize(), n);
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000BBCB4 File Offset: 0x000B9EB4
		public static AsymmetricCipherKeyPair GenerateECKeyPair(SecureRandom random, ECDomainParameters ecParams)
		{
			ECKeyPairGenerator eckeyPairGenerator = new ECKeyPairGenerator();
			eckeyPairGenerator.Init(new ECKeyGenerationParameters(ecParams, random));
			return eckeyPairGenerator.GenerateKeyPair();
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000BBCD0 File Offset: 0x000B9ED0
		public static ECPrivateKeyParameters GenerateEphemeralClientKeyExchange(SecureRandom random, byte[] ecPointFormats, ECDomainParameters ecParams, Stream output)
		{
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = TlsEccUtilities.GenerateECKeyPair(random, ecParams);
			ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)asymmetricCipherKeyPair.Public;
			TlsEccUtilities.WriteECPoint(ecPointFormats, ecpublicKeyParameters.Q, output);
			return (ECPrivateKeyParameters)asymmetricCipherKeyPair.Private;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000BBD08 File Offset: 0x000B9F08
		internal static ECPrivateKeyParameters GenerateEphemeralServerKeyExchange(SecureRandom random, int[] namedCurves, byte[] ecPointFormats, Stream output)
		{
			int num = -1;
			if (namedCurves == null)
			{
				num = 23;
			}
			else
			{
				foreach (int num2 in namedCurves)
				{
					if (NamedCurve.IsValid(num2) && TlsEccUtilities.IsSupportedNamedCurve(num2))
					{
						num = num2;
						break;
					}
				}
			}
			ECDomainParameters ecdomainParameters = null;
			if (num >= 0)
			{
				ecdomainParameters = TlsEccUtilities.GetParametersForNamedCurve(num);
			}
			else if (Arrays.Contains(namedCurves, 65281))
			{
				ecdomainParameters = TlsEccUtilities.GetParametersForNamedCurve(23);
			}
			else if (Arrays.Contains(namedCurves, 65282))
			{
				ecdomainParameters = TlsEccUtilities.GetParametersForNamedCurve(10);
			}
			if (ecdomainParameters == null)
			{
				throw new TlsFatalAlert(80);
			}
			if (num < 0)
			{
				TlsEccUtilities.WriteExplicitECParameters(ecPointFormats, ecdomainParameters, output);
			}
			else
			{
				TlsEccUtilities.WriteNamedECParameters(num, output);
			}
			return TlsEccUtilities.GenerateEphemeralClientKeyExchange(random, ecPointFormats, ecdomainParameters, output);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x00035D67 File Offset: 0x00033F67
		public static ECPublicKeyParameters ValidateECPublicKey(ECPublicKeyParameters key)
		{
			return key;
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000BBDAC File Offset: 0x000B9FAC
		public static int ReadECExponent(int fieldSize, Stream input)
		{
			BigInteger bigInteger = TlsEccUtilities.ReadECParameter(input);
			if (bigInteger.BitLength < 32)
			{
				int intValue = bigInteger.IntValue;
				if (intValue > 0 && intValue < fieldSize)
				{
					return intValue;
				}
			}
			throw new TlsFatalAlert(47);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000BBDE2 File Offset: 0x000B9FE2
		public static BigInteger ReadECFieldElement(int fieldSize, Stream input)
		{
			return TlsEccUtilities.DeserializeECFieldElement(fieldSize, TlsUtilities.ReadOpaque8(input));
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000BBDF0 File Offset: 0x000B9FF0
		public static BigInteger ReadECParameter(Stream input)
		{
			return new BigInteger(1, TlsUtilities.ReadOpaque8(input));
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000BBE00 File Offset: 0x000BA000
		public static ECDomainParameters ReadECParameters(int[] namedCurves, byte[] ecPointFormats, Stream input)
		{
			ECDomainParameters result;
			try
			{
				switch (TlsUtilities.ReadUint8(input))
				{
				case 1:
				{
					TlsEccUtilities.CheckNamedCurve(namedCurves, 65281);
					BigInteger bigInteger = TlsEccUtilities.ReadECParameter(input);
					BigInteger a = TlsEccUtilities.ReadECFieldElement(bigInteger.BitLength, input);
					BigInteger b = TlsEccUtilities.ReadECFieldElement(bigInteger.BitLength, input);
					byte[] encoding = TlsUtilities.ReadOpaque8(input);
					BigInteger bigInteger2 = TlsEccUtilities.ReadECParameter(input);
					BigInteger bigInteger3 = TlsEccUtilities.ReadECParameter(input);
					ECCurve curve = new FpCurve(bigInteger, a, b, bigInteger2, bigInteger3);
					ECPoint g = TlsEccUtilities.DeserializeECPoint(ecPointFormats, curve, encoding);
					result = new ECDomainParameters(curve, g, bigInteger2, bigInteger3);
					break;
				}
				case 2:
				{
					TlsEccUtilities.CheckNamedCurve(namedCurves, 65282);
					int num = TlsUtilities.ReadUint16(input);
					byte b2 = TlsUtilities.ReadUint8(input);
					if (!ECBasisType.IsValid(b2))
					{
						throw new TlsFatalAlert(47);
					}
					int num2 = TlsEccUtilities.ReadECExponent(num, input);
					int k = -1;
					int k2 = -1;
					if (b2 == 2)
					{
						k = TlsEccUtilities.ReadECExponent(num, input);
						k2 = TlsEccUtilities.ReadECExponent(num, input);
					}
					BigInteger a2 = TlsEccUtilities.ReadECFieldElement(num, input);
					BigInteger b3 = TlsEccUtilities.ReadECFieldElement(num, input);
					byte[] encoding2 = TlsUtilities.ReadOpaque8(input);
					BigInteger bigInteger4 = TlsEccUtilities.ReadECParameter(input);
					BigInteger bigInteger5 = TlsEccUtilities.ReadECParameter(input);
					ECCurve curve2 = (b2 == 2) ? new F2mCurve(num, num2, k, k2, a2, b3, bigInteger4, bigInteger5) : new F2mCurve(num, num2, a2, b3, bigInteger4, bigInteger5);
					ECPoint g2 = TlsEccUtilities.DeserializeECPoint(ecPointFormats, curve2, encoding2);
					result = new ECDomainParameters(curve2, g2, bigInteger4, bigInteger5);
					break;
				}
				case 3:
				{
					int namedCurve = TlsUtilities.ReadUint16(input);
					if (!NamedCurve.RefersToASpecificNamedCurve(namedCurve))
					{
						throw new TlsFatalAlert(47);
					}
					TlsEccUtilities.CheckNamedCurve(namedCurves, namedCurve);
					result = TlsEccUtilities.GetParametersForNamedCurve(namedCurve);
					break;
				}
				default:
					throw new TlsFatalAlert(47);
				}
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			return result;
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000BBFC0 File Offset: 0x000BA1C0
		private static void CheckNamedCurve(int[] namedCurves, int namedCurve)
		{
			if (namedCurves != null && !Arrays.Contains(namedCurves, namedCurve))
			{
				throw new TlsFatalAlert(47);
			}
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000BBFD6 File Offset: 0x000BA1D6
		public static void WriteECExponent(int k, Stream output)
		{
			TlsEccUtilities.WriteECParameter(BigInteger.ValueOf((long)k), output);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000BBFE5 File Offset: 0x000BA1E5
		public static void WriteECFieldElement(ECFieldElement x, Stream output)
		{
			TlsUtilities.WriteOpaque8(x.GetEncoded(), output);
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000BBFF3 File Offset: 0x000BA1F3
		public static void WriteECFieldElement(int fieldSize, BigInteger x, Stream output)
		{
			TlsUtilities.WriteOpaque8(TlsEccUtilities.SerializeECFieldElement(fieldSize, x), output);
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000BC002 File Offset: 0x000BA202
		public static void WriteECParameter(BigInteger x, Stream output)
		{
			TlsUtilities.WriteOpaque8(BigIntegers.AsUnsignedByteArray(x), output);
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000BC010 File Offset: 0x000BA210
		public static void WriteExplicitECParameters(byte[] ecPointFormats, ECDomainParameters ecParameters, Stream output)
		{
			ECCurve curve = ecParameters.Curve;
			if (ECAlgorithms.IsFpCurve(curve))
			{
				TlsUtilities.WriteUint8(1, output);
				TlsEccUtilities.WriteECParameter(curve.Field.Characteristic, output);
			}
			else
			{
				if (!ECAlgorithms.IsF2mCurve(curve))
				{
					throw new ArgumentException("'ecParameters' not a known curve type");
				}
				int[] exponentsPresent = ((IPolynomialExtensionField)curve.Field).MinimalPolynomial.GetExponentsPresent();
				TlsUtilities.WriteUint8(2, output);
				int i = exponentsPresent[exponentsPresent.Length - 1];
				TlsUtilities.CheckUint16(i);
				TlsUtilities.WriteUint16(i, output);
				if (exponentsPresent.Length == 3)
				{
					TlsUtilities.WriteUint8(1, output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[1], output);
				}
				else
				{
					if (exponentsPresent.Length != 5)
					{
						throw new ArgumentException("Only trinomial and pentomial curves are supported");
					}
					TlsUtilities.WriteUint8(2, output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[1], output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[2], output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[3], output);
				}
			}
			TlsEccUtilities.WriteECFieldElement(curve.A, output);
			TlsEccUtilities.WriteECFieldElement(curve.B, output);
			TlsUtilities.WriteOpaque8(TlsEccUtilities.SerializeECPoint(ecPointFormats, ecParameters.G), output);
			TlsEccUtilities.WriteECParameter(ecParameters.N, output);
			TlsEccUtilities.WriteECParameter(ecParameters.H, output);
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000BC11B File Offset: 0x000BA31B
		public static void WriteECPoint(byte[] ecPointFormats, ECPoint point, Stream output)
		{
			TlsUtilities.WriteOpaque8(TlsEccUtilities.SerializeECPoint(ecPointFormats, point), output);
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000BC12A File Offset: 0x000BA32A
		public static void WriteNamedECParameters(int namedCurve, Stream output)
		{
			if (!NamedCurve.RefersToASpecificNamedCurve(namedCurve))
			{
				throw new TlsFatalAlert(80);
			}
			TlsUtilities.WriteUint8(3, output);
			TlsUtilities.CheckUint16(namedCurve);
			TlsUtilities.WriteUint16(namedCurve, output);
		}

		// Token: 0x04001959 RID: 6489
		private static readonly string[] CurveNames = new string[]
		{
			"sect163k1",
			"sect163r1",
			"sect163r2",
			"sect193r1",
			"sect193r2",
			"sect233k1",
			"sect233r1",
			"sect239k1",
			"sect283k1",
			"sect283r1",
			"sect409k1",
			"sect409r1",
			"sect571k1",
			"sect571r1",
			"secp160k1",
			"secp160r1",
			"secp160r2",
			"secp192k1",
			"secp192r1",
			"secp224k1",
			"secp224r1",
			"secp256k1",
			"secp256r1",
			"secp384r1",
			"secp521r1",
			"brainpoolP256r1",
			"brainpoolP384r1",
			"brainpoolP512r1"
		};
	}
}
