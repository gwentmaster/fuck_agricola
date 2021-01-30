﻿using System;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000497 RID: 1175
	public class Rfc3211WrapEngine : IWrapper
	{
		// Token: 0x06002AEE RID: 10990 RVA: 0x000D9A73 File Offset: 0x000D7C73
		public Rfc3211WrapEngine(IBlockCipher engine)
		{
			this.engine = new CbcBlockCipher(engine);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000D9A88 File Offset: 0x000D7C88
		public virtual void Init(bool forWrapping, ICipherParameters param)
		{
			this.forWrapping = forWrapping;
			if (param is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
				this.rand = parametersWithRandom.Random;
				this.param = (ParametersWithIV)parametersWithRandom.Parameters;
				return;
			}
			if (forWrapping)
			{
				this.rand = new SecureRandom();
			}
			this.param = (ParametersWithIV)param;
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000D9AE3 File Offset: 0x000D7CE3
		public virtual string AlgorithmName
		{
			get
			{
				return this.engine.GetUnderlyingCipher().AlgorithmName + "/RFC3211Wrap";
			}
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000D9B00 File Offset: 0x000D7D00
		public virtual byte[] Wrap(byte[] inBytes, int inOff, int inLen)
		{
			if (!this.forWrapping)
			{
				throw new InvalidOperationException("not set for wrapping");
			}
			this.engine.Init(true, this.param);
			int blockSize = this.engine.GetBlockSize();
			byte[] array;
			if (inLen + 4 < blockSize * 2)
			{
				array = new byte[blockSize * 2];
			}
			else
			{
				array = new byte[((inLen + 4) % blockSize == 0) ? (inLen + 4) : (((inLen + 4) / blockSize + 1) * blockSize)];
			}
			array[0] = (byte)inLen;
			array[1] = ~inBytes[inOff];
			array[2] = ~inBytes[inOff + 1];
			array[3] = ~inBytes[inOff + 2];
			Array.Copy(inBytes, inOff, array, 4, inLen);
			this.rand.NextBytes(array, inLen + 4, array.Length - inLen - 4);
			for (int i = 0; i < array.Length; i += blockSize)
			{
				this.engine.ProcessBlock(array, i, array, i);
			}
			for (int j = 0; j < array.Length; j += blockSize)
			{
				this.engine.ProcessBlock(array, j, array, j);
			}
			return array;
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000D9BEC File Offset: 0x000D7DEC
		public virtual byte[] Unwrap(byte[] inBytes, int inOff, int inLen)
		{
			if (this.forWrapping)
			{
				throw new InvalidOperationException("not set for unwrapping");
			}
			int blockSize = this.engine.GetBlockSize();
			if (inLen < 2 * blockSize)
			{
				throw new InvalidCipherTextException("input too short");
			}
			byte[] array = new byte[inLen];
			byte[] array2 = new byte[blockSize];
			Array.Copy(inBytes, inOff, array, 0, inLen);
			Array.Copy(inBytes, inOff, array2, 0, array2.Length);
			this.engine.Init(false, new ParametersWithIV(this.param.Parameters, array2));
			for (int i = blockSize; i < array.Length; i += blockSize)
			{
				this.engine.ProcessBlock(array, i, array, i);
			}
			Array.Copy(array, array.Length - array2.Length, array2, 0, array2.Length);
			this.engine.Init(false, new ParametersWithIV(this.param.Parameters, array2));
			this.engine.ProcessBlock(array, 0, array, 0);
			this.engine.Init(false, this.param);
			for (int j = 0; j < array.Length; j += blockSize)
			{
				this.engine.ProcessBlock(array, j, array, j);
			}
			if ((int)(array[0] & 255) > array.Length - 4)
			{
				throw new InvalidCipherTextException("wrapped key corrupted");
			}
			byte[] array3 = new byte[(int)(array[0] & byte.MaxValue)];
			Array.Copy(array, 4, array3, 0, (int)array[0]);
			int num = 0;
			for (int num2 = 0; num2 != 3; num2++)
			{
				byte b = ~array[1 + num2];
				num |= (int)(b ^ array3[num2]);
			}
			if (num != 0)
			{
				throw new InvalidCipherTextException("wrapped key fails checksum");
			}
			return array3;
		}

		// Token: 0x04001C48 RID: 7240
		private CbcBlockCipher engine;

		// Token: 0x04001C49 RID: 7241
		private ParametersWithIV param;

		// Token: 0x04001C4A RID: 7242
		private bool forWrapping;

		// Token: 0x04001C4B RID: 7243
		private SecureRandom rand;
	}
}
