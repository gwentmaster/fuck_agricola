using System;
using System.IO;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FB RID: 1019
	public abstract class TlsRsaUtilities
	{
		// Token: 0x06002594 RID: 9620 RVA: 0x000BDF98 File Offset: 0x000BC198
		public static byte[] GenerateEncryptedPreMasterSecret(TlsContext context, RsaKeyParameters rsaServerPublicKey, Stream output)
		{
			byte[] array = new byte[48];
			context.SecureRandom.NextBytes(array);
			TlsUtilities.WriteVersion(context.ClientVersion, array, 0);
			Pkcs1Encoding pkcs1Encoding = new Pkcs1Encoding(new RsaBlindedEngine());
			pkcs1Encoding.Init(true, new ParametersWithRandom(rsaServerPublicKey, context.SecureRandom));
			try
			{
				byte[] array2 = pkcs1Encoding.ProcessBlock(array, 0, array.Length);
				if (TlsUtilities.IsSsl(context))
				{
					output.Write(array2, 0, array2.Length);
				}
				else
				{
					TlsUtilities.WriteOpaque16(array2, output);
				}
			}
			catch (InvalidCipherTextException alertCause)
			{
				throw new TlsFatalAlert(80, alertCause);
			}
			return array;
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000BE02C File Offset: 0x000BC22C
		public static byte[] SafeDecryptPreMasterSecret(TlsContext context, RsaKeyParameters rsaServerPrivateKey, byte[] encryptedPreMasterSecret)
		{
			ProtocolVersion clientVersion = context.ClientVersion;
			bool flag = false;
			byte[] array = new byte[48];
			context.SecureRandom.NextBytes(array);
			byte[] array2 = Arrays.Clone(array);
			try
			{
				Pkcs1Encoding pkcs1Encoding = new Pkcs1Encoding(new RsaBlindedEngine(), array);
				pkcs1Encoding.Init(false, new ParametersWithRandom(rsaServerPrivateKey, context.SecureRandom));
				array2 = pkcs1Encoding.ProcessBlock(encryptedPreMasterSecret, 0, encryptedPreMasterSecret.Length);
			}
			catch (Exception)
			{
			}
			if (!flag || !clientVersion.IsEqualOrEarlierVersionOf(ProtocolVersion.TLSv10))
			{
				int num = (clientVersion.MajorVersion ^ (int)(array2[0] & byte.MaxValue)) | (clientVersion.MinorVersion ^ (int)(array2[1] & byte.MaxValue));
				int num2 = num | num >> 1;
				int num3 = num2 | num2 >> 2;
				int num4 = ~(((num3 | num3 >> 4) & 1) - 1);
				for (int i = 0; i < 48; i++)
				{
					array2[i] = (byte)(((int)array2[i] & ~num4) | ((int)array[i] & num4));
				}
			}
			return array2;
		}
	}
}
