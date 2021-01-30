using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Encodings
{
	// Token: 0x020004A9 RID: 1193
	public class Pkcs1Encoding : IAsymmetricBlockCipher
	{
		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x000E130F File Offset: 0x000DF50F
		// (set) Token: 0x06002BC9 RID: 11209 RVA: 0x000E1318 File Offset: 0x000DF518
		public static bool StrictLengthEnabled
		{
			get
			{
				return Pkcs1Encoding.strictLengthEnabled[0];
			}
			set
			{
				Pkcs1Encoding.strictLengthEnabled[0] = value;
			}
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x000E1324 File Offset: 0x000DF524
		static Pkcs1Encoding()
		{
			string environmentVariable = Platform.GetEnvironmentVariable("Org.BouncyCastle.Pkcs1.Strict");
			Pkcs1Encoding.strictLengthEnabled = new bool[]
			{
				environmentVariable == null || environmentVariable.Equals("true")
			};
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000E135B File Offset: 0x000DF55B
		public Pkcs1Encoding(IAsymmetricBlockCipher cipher)
		{
			this.engine = cipher;
			this.useStrictLength = Pkcs1Encoding.StrictLengthEnabled;
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000E137C File Offset: 0x000DF57C
		public Pkcs1Encoding(IAsymmetricBlockCipher cipher, int pLen)
		{
			this.engine = cipher;
			this.useStrictLength = Pkcs1Encoding.StrictLengthEnabled;
			this.pLen = pLen;
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000E13A4 File Offset: 0x000DF5A4
		public Pkcs1Encoding(IAsymmetricBlockCipher cipher, byte[] fallback)
		{
			this.engine = cipher;
			this.useStrictLength = Pkcs1Encoding.StrictLengthEnabled;
			this.fallback = fallback;
			this.pLen = fallback.Length;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x000E13D5 File Offset: 0x000DF5D5
		public IAsymmetricBlockCipher GetUnderlyingCipher()
		{
			return this.engine;
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06002BCF RID: 11215 RVA: 0x000E13DD File Offset: 0x000DF5DD
		public string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName + "/PKCS1Padding";
			}
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000E13F4 File Offset: 0x000DF5F4
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			AsymmetricKeyParameter asymmetricKeyParameter;
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				this.random = parametersWithRandom.Random;
				asymmetricKeyParameter = (AsymmetricKeyParameter)parametersWithRandom.Parameters;
			}
			else
			{
				this.random = new SecureRandom();
				asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
			}
			this.engine.Init(forEncryption, parameters);
			this.forPrivateKey = asymmetricKeyParameter.IsPrivate;
			this.forEncryption = forEncryption;
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000E145C File Offset: 0x000DF65C
		public int GetInputBlockSize()
		{
			int inputBlockSize = this.engine.GetInputBlockSize();
			if (!this.forEncryption)
			{
				return inputBlockSize;
			}
			return inputBlockSize - 10;
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000E1484 File Offset: 0x000DF684
		public int GetOutputBlockSize()
		{
			int outputBlockSize = this.engine.GetOutputBlockSize();
			if (!this.forEncryption)
			{
				return outputBlockSize - 10;
			}
			return outputBlockSize;
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000E14AB File Offset: 0x000DF6AB
		public byte[] ProcessBlock(byte[] input, int inOff, int length)
		{
			if (!this.forEncryption)
			{
				return this.DecodeBlock(input, inOff, length);
			}
			return this.EncodeBlock(input, inOff, length);
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000E14C8 File Offset: 0x000DF6C8
		private byte[] EncodeBlock(byte[] input, int inOff, int inLen)
		{
			if (inLen > this.GetInputBlockSize())
			{
				throw new ArgumentException("input data too large", "inLen");
			}
			byte[] array = new byte[this.engine.GetInputBlockSize()];
			if (this.forPrivateKey)
			{
				array[0] = 1;
				for (int num = 1; num != array.Length - inLen - 1; num++)
				{
					array[num] = byte.MaxValue;
				}
			}
			else
			{
				this.random.NextBytes(array);
				array[0] = 2;
				for (int num2 = 1; num2 != array.Length - inLen - 1; num2++)
				{
					while (array[num2] == 0)
					{
						array[num2] = (byte)this.random.NextInt();
					}
				}
			}
			array[array.Length - inLen - 1] = 0;
			Array.Copy(input, inOff, array, array.Length - inLen, inLen);
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000E1588 File Offset: 0x000DF788
		private static int CheckPkcs1Encoding(byte[] encoded, int pLen)
		{
			int num = 0;
			num |= (int)(encoded[0] ^ 2);
			int num2 = encoded.Length - (pLen + 1);
			for (int i = 1; i < num2; i++)
			{
				int num3 = (int)encoded[i];
				num3 |= num3 >> 1;
				num3 |= num3 >> 2;
				num3 |= num3 >> 4;
				num |= (num3 & 1) - 1;
			}
			num |= (int)encoded[encoded.Length - (pLen + 1)];
			num |= num >> 1;
			num |= num >> 2;
			num |= num >> 4;
			return ~((num & 1) - 1);
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000E15F8 File Offset: 0x000DF7F8
		private byte[] DecodeBlockOrRandom(byte[] input, int inOff, int inLen)
		{
			if (!this.forPrivateKey)
			{
				throw new InvalidCipherTextException("sorry, this method is only for decryption, not for signing");
			}
			byte[] array = this.engine.ProcessBlock(input, inOff, inLen);
			byte[] array2;
			if (this.fallback == null)
			{
				array2 = new byte[this.pLen];
				this.random.NextBytes(array2);
			}
			else
			{
				array2 = this.fallback;
			}
			if (array.Length < this.GetOutputBlockSize())
			{
				throw new InvalidCipherTextException("block truncated");
			}
			if (this.useStrictLength && array.Length != this.engine.GetOutputBlockSize())
			{
				throw new InvalidCipherTextException("block incorrect size");
			}
			int num = Pkcs1Encoding.CheckPkcs1Encoding(array, this.pLen);
			byte[] array3 = new byte[this.pLen];
			for (int i = 0; i < this.pLen; i++)
			{
				array3[i] = (byte)(((int)array[i + (array.Length - this.pLen)] & ~num) | ((int)array2[i] & num));
			}
			return array3;
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000E16D8 File Offset: 0x000DF8D8
		private byte[] DecodeBlock(byte[] input, int inOff, int inLen)
		{
			if (this.pLen != -1)
			{
				return this.DecodeBlockOrRandom(input, inOff, inLen);
			}
			byte[] array = this.engine.ProcessBlock(input, inOff, inLen);
			if (array.Length < this.GetOutputBlockSize())
			{
				throw new InvalidCipherTextException("block truncated");
			}
			byte b = array[0];
			if (b != 1 && b != 2)
			{
				throw new InvalidCipherTextException("unknown block type");
			}
			if (this.useStrictLength && array.Length != this.engine.GetOutputBlockSize())
			{
				throw new InvalidCipherTextException("block incorrect size");
			}
			int num;
			for (num = 1; num != array.Length; num++)
			{
				byte b2 = array[num];
				if (b2 == 0)
				{
					break;
				}
				if (b == 1 && b2 != 255)
				{
					throw new InvalidCipherTextException("block padding incorrect");
				}
			}
			num++;
			if (num > array.Length || num < 10)
			{
				throw new InvalidCipherTextException("no data in block");
			}
			byte[] array2 = new byte[array.Length - num];
			Array.Copy(array, num, array2, 0, array2.Length);
			return array2;
		}

		// Token: 0x04001CE4 RID: 7396
		public const string StrictLengthEnabledProperty = "Org.BouncyCastle.Pkcs1.Strict";

		// Token: 0x04001CE5 RID: 7397
		private const int HeaderLength = 10;

		// Token: 0x04001CE6 RID: 7398
		private static readonly bool[] strictLengthEnabled;

		// Token: 0x04001CE7 RID: 7399
		private SecureRandom random;

		// Token: 0x04001CE8 RID: 7400
		private IAsymmetricBlockCipher engine;

		// Token: 0x04001CE9 RID: 7401
		private bool forEncryption;

		// Token: 0x04001CEA RID: 7402
		private bool forPrivateKey;

		// Token: 0x04001CEB RID: 7403
		private bool useStrictLength;

		// Token: 0x04001CEC RID: 7404
		private int pLen = -1;

		// Token: 0x04001CED RID: 7405
		private byte[] fallback;
	}
}
