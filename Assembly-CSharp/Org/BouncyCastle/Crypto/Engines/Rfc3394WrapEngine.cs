using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000498 RID: 1176
	public class Rfc3394WrapEngine : IWrapper
	{
		// Token: 0x06002AF3 RID: 10995 RVA: 0x000D9D72 File Offset: 0x000D7F72
		public Rfc3394WrapEngine(IBlockCipher engine)
		{
			this.engine = engine;
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x000D9D98 File Offset: 0x000D7F98
		public virtual void Init(bool forWrapping, ICipherParameters parameters)
		{
			this.forWrapping = forWrapping;
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			if (parameters is KeyParameter)
			{
				this.param = (KeyParameter)parameters;
				return;
			}
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] array = parametersWithIV.GetIV();
				if (array.Length != 8)
				{
					throw new ArgumentException("IV length not equal to 8", "parameters");
				}
				this.iv = array;
				this.param = (KeyParameter)parametersWithIV.Parameters;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x000D9E1A File Offset: 0x000D801A
		public virtual string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName;
			}
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000D9E28 File Offset: 0x000D8028
		public virtual byte[] Wrap(byte[] input, int inOff, int inLen)
		{
			if (!this.forWrapping)
			{
				throw new InvalidOperationException("not set for wrapping");
			}
			int num = inLen / 8;
			if (num * 8 != inLen)
			{
				throw new DataLengthException("wrap data must be a multiple of 8 bytes");
			}
			byte[] array = new byte[inLen + this.iv.Length];
			byte[] array2 = new byte[8 + this.iv.Length];
			Array.Copy(this.iv, 0, array, 0, this.iv.Length);
			Array.Copy(input, inOff, array, this.iv.Length, inLen);
			this.engine.Init(true, this.param);
			for (int num2 = 0; num2 != 6; num2++)
			{
				for (int i = 1; i <= num; i++)
				{
					Array.Copy(array, 0, array2, 0, this.iv.Length);
					Array.Copy(array, 8 * i, array2, this.iv.Length, 8);
					this.engine.ProcessBlock(array2, 0, array2, 0);
					int num3 = num * num2 + i;
					int num4 = 1;
					while (num3 != 0)
					{
						byte b = (byte)num3;
						byte[] array3 = array2;
						int num5 = this.iv.Length - num4;
						array3[num5] ^= b;
						num3 = (int)((uint)num3 >> 8);
						num4++;
					}
					Array.Copy(array2, 0, array, 0, 8);
					Array.Copy(array2, 8, array, 8 * i, 8);
				}
			}
			return array;
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000D9F68 File Offset: 0x000D8168
		public virtual byte[] Unwrap(byte[] input, int inOff, int inLen)
		{
			if (this.forWrapping)
			{
				throw new InvalidOperationException("not set for unwrapping");
			}
			int num = inLen / 8;
			if (num * 8 != inLen)
			{
				throw new InvalidCipherTextException("unwrap data must be a multiple of 8 bytes");
			}
			byte[] array = new byte[inLen - this.iv.Length];
			byte[] array2 = new byte[this.iv.Length];
			byte[] array3 = new byte[8 + this.iv.Length];
			Array.Copy(input, inOff, array2, 0, this.iv.Length);
			Array.Copy(input, inOff + this.iv.Length, array, 0, inLen - this.iv.Length);
			this.engine.Init(false, this.param);
			num--;
			for (int i = 5; i >= 0; i--)
			{
				for (int j = num; j >= 1; j--)
				{
					Array.Copy(array2, 0, array3, 0, this.iv.Length);
					Array.Copy(array, 8 * (j - 1), array3, this.iv.Length, 8);
					int num2 = num * i + j;
					int num3 = 1;
					while (num2 != 0)
					{
						byte b = (byte)num2;
						byte[] array4 = array3;
						int num4 = this.iv.Length - num3;
						array4[num4] ^= b;
						num2 = (int)((uint)num2 >> 8);
						num3++;
					}
					this.engine.ProcessBlock(array3, 0, array3, 0);
					Array.Copy(array3, 0, array2, 0, 8);
					Array.Copy(array3, 8, array, 8 * (j - 1), 8);
				}
			}
			if (!Arrays.ConstantTimeAreEqual(array2, this.iv))
			{
				throw new InvalidCipherTextException("checksum failed");
			}
			return array;
		}

		// Token: 0x04001C4C RID: 7244
		private readonly IBlockCipher engine;

		// Token: 0x04001C4D RID: 7245
		private KeyParameter param;

		// Token: 0x04001C4E RID: 7246
		private bool forWrapping;

		// Token: 0x04001C4F RID: 7247
		private byte[] iv = new byte[]
		{
			166,
			166,
			166,
			166,
			166,
			166,
			166,
			166
		};
	}
}
