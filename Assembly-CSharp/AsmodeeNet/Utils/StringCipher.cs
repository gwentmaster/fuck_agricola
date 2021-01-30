using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000665 RID: 1637
	public static class StringCipher
	{
		// Token: 0x06003C78 RID: 15480 RVA: 0x0012ACB4 File Offset: 0x00128EB4
		public static string Encrypt(string plainText, string passPhrase)
		{
			string result;
			try
			{
				byte[] array = StringCipher.Generate256BitsOfRandomEntropy();
				byte[] array2 = StringCipher.Generate256BitsOfRandomEntropy();
				byte[] bytes = Encoding.UTF8.GetBytes(plainText);
				byte[] bytes2 = new Rfc2898DeriveBytes(passPhrase, array, 1000).GetBytes(32);
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.BlockSize = 256;
					rijndaelManaged.Mode = CipherMode.CBC;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(bytes2, array2))
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
							{
								cryptoStream.Write(bytes, 0, bytes.Length);
								cryptoStream.FlushFinalBlock();
								byte[] inArray = array.Concat(array2).ToArray<byte>().Concat(memoryStream.ToArray()).ToArray<byte>();
								memoryStream.Close();
								cryptoStream.Close();
								result = Convert.ToBase64String(inArray);
							}
						}
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x0012ADF0 File Offset: 0x00128FF0
		public static string Decrypt(string cipherText, string passPhrase)
		{
			string result;
			try
			{
				byte[] array = Convert.FromBase64String(cipherText);
				byte[] salt = array.Take(32).ToArray<byte>();
				byte[] rgbIV = array.Skip(32).Take(32).ToArray<byte>();
				byte[] array2 = array.Skip(64).Take(array.Length - 64).ToArray<byte>();
				byte[] bytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000).GetBytes(32);
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.BlockSize = 256;
					rijndaelManaged.Mode = CipherMode.CBC;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(bytes, rgbIV))
					{
						using (MemoryStream memoryStream = new MemoryStream(array2))
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
							{
								byte[] array3 = new byte[array2.Length];
								int count = cryptoStream.Read(array3, 0, array3.Length);
								memoryStream.Close();
								cryptoStream.Close();
								result = Encoding.UTF8.GetString(array3, 0, count);
							}
						}
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0012AF88 File Offset: 0x00129188
		private static byte[] Generate256BitsOfRandomEntropy()
		{
			byte[] array = new byte[32];
			new RNGCryptoServiceProvider().GetBytes(array);
			return array;
		}

		// Token: 0x040026EC RID: 9964
		private const int Keysize = 256;

		// Token: 0x040026ED RID: 9965
		private const int DerivationIterations = 1000;
	}
}
