using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E6 RID: 998
	public abstract class TlsDHUtilities
	{
		// Token: 0x06002482 RID: 9346 RVA: 0x000BA9AF File Offset: 0x000B8BAF
		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000BA9C0 File Offset: 0x000B8BC0
		private static DHParameters FromSafeP(string hexP)
		{
			BigInteger bigInteger = TlsDHUtilities.FromHex(hexP);
			BigInteger q = bigInteger.ShiftRight(1);
			return new DHParameters(bigInteger, TlsDHUtilities.Two, q);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000BA9E6 File Offset: 0x000B8BE6
		public static void AddNegotiatedDheGroupsClientExtension(IDictionary extensions, byte[] dheGroups)
		{
			extensions[ExtensionType.negotiated_ff_dhe_groups] = TlsDHUtilities.CreateNegotiatedDheGroupsClientExtension(dheGroups);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000BA9FE File Offset: 0x000B8BFE
		public static void AddNegotiatedDheGroupsServerExtension(IDictionary extensions, byte dheGroup)
		{
			extensions[ExtensionType.negotiated_ff_dhe_groups] = TlsDHUtilities.CreateNegotiatedDheGroupsServerExtension(dheGroup);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000BAA18 File Offset: 0x000B8C18
		public static byte[] GetNegotiatedDheGroupsClientExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, ExtensionType.negotiated_ff_dhe_groups);
			if (extensionData != null)
			{
				return TlsDHUtilities.ReadNegotiatedDheGroupsClientExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000BAA3C File Offset: 0x000B8C3C
		public static short GetNegotiatedDheGroupsServerExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, ExtensionType.negotiated_ff_dhe_groups);
			if (extensionData != null)
			{
				return (short)TlsDHUtilities.ReadNegotiatedDheGroupsServerExtension(extensionData);
			}
			return -1;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000BAA60 File Offset: 0x000B8C60
		public static byte[] CreateNegotiatedDheGroupsClientExtension(byte[] dheGroups)
		{
			if (dheGroups == null || dheGroups.Length < 1 || dheGroups.Length > 255)
			{
				throw new TlsFatalAlert(80);
			}
			return TlsUtilities.EncodeUint8ArrayWithUint8Length(dheGroups);
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x000BAA83 File Offset: 0x000B8C83
		public static byte[] CreateNegotiatedDheGroupsServerExtension(byte dheGroup)
		{
			return new byte[]
			{
				dheGroup
			};
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000BAA90 File Offset: 0x000B8C90
		public static byte[] ReadNegotiatedDheGroupsClientExtension(byte[] extensionData)
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
			byte[] result = TlsUtilities.ReadUint8Array((int)b, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000BAAD1 File Offset: 0x000B8CD1
		public static byte ReadNegotiatedDheGroupsServerExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			if (extensionData.Length != 1)
			{
				throw new TlsFatalAlert(50);
			}
			return extensionData[0];
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000BAAF2 File Offset: 0x000B8CF2
		public static DHParameters GetParametersForDHEGroup(short dheGroup)
		{
			switch (dheGroup)
			{
			case 0:
				return TlsDHUtilities.draft_ffdhe2432;
			case 1:
				return TlsDHUtilities.draft_ffdhe3072;
			case 2:
				return TlsDHUtilities.draft_ffdhe4096;
			case 3:
				return TlsDHUtilities.draft_ffdhe6144;
			case 4:
				return TlsDHUtilities.draft_ffdhe8192;
			default:
				return null;
			}
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000BAB30 File Offset: 0x000B8D30
		public static bool ContainsDheCipherSuites(int[] cipherSuites)
		{
			for (int i = 0; i < cipherSuites.Length; i++)
			{
				if (TlsDHUtilities.IsDheCipherSuite(cipherSuites[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000BAB58 File Offset: 0x000B8D58
		public static bool IsDheCipherSuite(int cipherSuite)
		{
			if (cipherSuite <= 171)
			{
				if (cipherSuite <= 69)
				{
					if (cipherSuite <= 51)
					{
						if (cipherSuite - 17 > 5 && cipherSuite != 45 && cipherSuite - 50 > 1)
						{
							return false;
						}
					}
					else if (cipherSuite - 56 > 1 && cipherSuite != 64 && cipherSuite - 68 > 1)
					{
						return false;
					}
				}
				else if (cipherSuite <= 136)
				{
					if (cipherSuite != 103 && cipherSuite - 106 > 1 && cipherSuite - 135 > 1)
					{
						return false;
					}
				}
				else if (cipherSuite - 142 > 3)
				{
					switch (cipherSuite)
					{
					case 153:
					case 154:
					case 158:
					case 159:
					case 162:
					case 163:
						break;
					case 155:
					case 156:
					case 157:
					case 160:
					case 161:
						return false;
					default:
						if (cipherSuite - 170 > 1)
						{
							return false;
						}
						break;
					}
				}
			}
			else if (cipherSuite <= 49297)
			{
				if (cipherSuite <= 196)
				{
					if (cipherSuite - 178 > 3 && cipherSuite - 189 > 1 && cipherSuite - 195 > 1)
					{
						return false;
					}
				}
				else if (cipherSuite - 49276 > 1 && cipherSuite - 49280 > 1 && cipherSuite - 49296 > 1)
				{
					return false;
				}
			}
			else if (cipherSuite <= 52394)
			{
				if (cipherSuite - 49302 > 1)
				{
					switch (cipherSuite)
					{
					case 49310:
					case 49311:
					case 49314:
					case 49315:
					case 49318:
					case 49319:
					case 49322:
					case 49323:
						break;
					case 49312:
					case 49313:
					case 49316:
					case 49317:
					case 49320:
					case 49321:
						return false;
					default:
						if (cipherSuite != 52394)
						{
							return false;
						}
						break;
					}
				}
			}
			else if (cipherSuite != 52397 && cipherSuite - 65280 > 1 && cipherSuite - 65298 > 1)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000BAD20 File Offset: 0x000B8F20
		public static bool AreCompatibleParameters(DHParameters a, DHParameters b)
		{
			return a.P.Equals(b.P) && a.G.Equals(b.G) && (a.Q == null || b.Q == null || a.Q.Equals(b.Q));
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000BAD78 File Offset: 0x000B8F78
		public static byte[] CalculateDHBasicAgreement(DHPublicKeyParameters publicKey, DHPrivateKeyParameters privateKey)
		{
			DHBasicAgreement dhbasicAgreement = new DHBasicAgreement();
			dhbasicAgreement.Init(privateKey);
			return BigIntegers.AsUnsignedByteArray(dhbasicAgreement.CalculateAgreement(publicKey));
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000BAD91 File Offset: 0x000B8F91
		public static AsymmetricCipherKeyPair GenerateDHKeyPair(SecureRandom random, DHParameters dhParams)
		{
			DHBasicKeyPairGenerator dhbasicKeyPairGenerator = new DHBasicKeyPairGenerator();
			dhbasicKeyPairGenerator.Init(new DHKeyGenerationParameters(random, dhParams));
			return dhbasicKeyPairGenerator.GenerateKeyPair();
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000BADAA File Offset: 0x000B8FAA
		public static DHPrivateKeyParameters GenerateEphemeralClientKeyExchange(SecureRandom random, DHParameters dhParams, Stream output)
		{
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = TlsDHUtilities.GenerateDHKeyPair(random, dhParams);
			TlsDHUtilities.WriteDHParameter(((DHPublicKeyParameters)asymmetricCipherKeyPair.Public).Y, output);
			return (DHPrivateKeyParameters)asymmetricCipherKeyPair.Private;
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000BADD3 File Offset: 0x000B8FD3
		public static DHPrivateKeyParameters GenerateEphemeralServerKeyExchange(SecureRandom random, DHParameters dhParams, Stream output)
		{
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = TlsDHUtilities.GenerateDHKeyPair(random, dhParams);
			new ServerDHParams((DHPublicKeyParameters)asymmetricCipherKeyPair.Public).Encode(output);
			return (DHPrivateKeyParameters)asymmetricCipherKeyPair.Private;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000BADFC File Offset: 0x000B8FFC
		public static DHParameters ValidateDHParameters(DHParameters parameters)
		{
			BigInteger p = parameters.P;
			BigInteger g = parameters.G;
			if (!p.IsProbablePrime(2))
			{
				throw new TlsFatalAlert(47);
			}
			if (g.CompareTo(TlsDHUtilities.Two) < 0 || g.CompareTo(p.Subtract(TlsDHUtilities.Two)) > 0)
			{
				throw new TlsFatalAlert(47);
			}
			return parameters;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000BAE54 File Offset: 0x000B9054
		public static DHPublicKeyParameters ValidateDHPublicKey(DHPublicKeyParameters key)
		{
			DHParameters dhparameters = TlsDHUtilities.ValidateDHParameters(key.Parameters);
			BigInteger y = key.Y;
			if (y.CompareTo(TlsDHUtilities.Two) < 0 || y.CompareTo(dhparameters.P.Subtract(TlsDHUtilities.Two)) > 0)
			{
				throw new TlsFatalAlert(47);
			}
			return key;
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000BAEA4 File Offset: 0x000B90A4
		public static BigInteger ReadDHParameter(Stream input)
		{
			return new BigInteger(1, TlsUtilities.ReadOpaque16(input));
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000BAEB2 File Offset: 0x000B90B2
		public static void WriteDHParameter(BigInteger x, Stream output)
		{
			TlsUtilities.WriteOpaque16(BigIntegers.AsUnsignedByteArray(x), output);
		}

		// Token: 0x0400193E RID: 6462
		internal static readonly BigInteger Two = BigInteger.Two;

		// Token: 0x0400193F RID: 6463
		private static readonly string draft_ffdhe2432_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE13098533C8B3FFFFFFFFFFFFFFFF";

		// Token: 0x04001940 RID: 6464
		internal static readonly DHParameters draft_ffdhe2432 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe2432_p);

		// Token: 0x04001941 RID: 6465
		private static readonly string draft_ffdhe3072_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B66C62E37FFFFFFFFFFFFFFFF";

		// Token: 0x04001942 RID: 6466
		internal static readonly DHParameters draft_ffdhe3072 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe3072_p);

		// Token: 0x04001943 RID: 6467
		private static readonly string draft_ffdhe4096_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E655F6AFFFFFFFFFFFFFFFF";

		// Token: 0x04001944 RID: 6468
		internal static readonly DHParameters draft_ffdhe4096 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe4096_p);

		// Token: 0x04001945 RID: 6469
		private static readonly string draft_ffdhe6144_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E0DD9020BFD64B645036C7A4E677D2C38532A3A23BA4442CAF53EA63BB454329B7624C8917BDD64B1C0FD4CB38E8C334C701C3ACDAD0657FCCFEC719B1F5C3E4E46041F388147FB4CFDB477A52471F7A9A96910B855322EDB6340D8A00EF092350511E30ABEC1FFF9E3A26E7FB29F8C183023C3587E38DA0077D9B4763E4E4B94B2BBC194C6651E77CAF992EEAAC0232A281BF6B3A739C1226116820AE8DB5847A67CBEF9C9091B462D538CD72B03746AE77F5E62292C311562A846505DC82DB854338AE49F5235C95B91178CCF2DD5CACEF403EC9D1810C6272B045B3B71F9DC6B80D63FDD4A8E9ADB1E6962A69526D43161C1A41D570D7938DAD4A40E329CD0E40E65FFFFFFFFFFFFFFFF";

		// Token: 0x04001946 RID: 6470
		internal static readonly DHParameters draft_ffdhe6144 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe6144_p);

		// Token: 0x04001947 RID: 6471
		private static readonly string draft_ffdhe8192_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E0DD9020BFD64B645036C7A4E677D2C38532A3A23BA4442CAF53EA63BB454329B7624C8917BDD64B1C0FD4CB38E8C334C701C3ACDAD0657FCCFEC719B1F5C3E4E46041F388147FB4CFDB477A52471F7A9A96910B855322EDB6340D8A00EF092350511E30ABEC1FFF9E3A26E7FB29F8C183023C3587E38DA0077D9B4763E4E4B94B2BBC194C6651E77CAF992EEAAC0232A281BF6B3A739C1226116820AE8DB5847A67CBEF9C9091B462D538CD72B03746AE77F5E62292C311562A846505DC82DB854338AE49F5235C95B91178CCF2DD5CACEF403EC9D1810C6272B045B3B71F9DC6B80D63FDD4A8E9ADB1E6962A69526D43161C1A41D570D7938DAD4A40E329CCFF46AAA36AD004CF600C8381E425A31D951AE64FDB23FCEC9509D43687FEB69EDD1CC5E0B8CC3BDF64B10EF86B63142A3AB8829555B2F747C932665CB2C0F1CC01BD70229388839D2AF05E454504AC78B7582822846C0BA35C35F5C59160CC046FD8251541FC68C9C86B022BB7099876A460E7451A8A93109703FEE1C217E6C3826E52C51AA691E0E423CFC99E9E31650C1217B624816CDAD9A95F9D5B8019488D9C0A0A1FE3075A577E23183F81D4A3F2FA4571EFC8CE0BA8A4FE8B6855DFE72B0A66EDED2FBABFBE58A30FAFABE1C5D71A87E2F741EF8C1FE86FEA6BBFDE530677F0D97D11D49F7A8443D0822E506A9F4614E011E2A94838FF88CD68C8BB7C5C6424CFFFFFFFFFFFFFFFF";

		// Token: 0x04001948 RID: 6472
		internal static readonly DHParameters draft_ffdhe8192 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe8192_p);
	}
}
