using System;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000470 RID: 1136
	public class ISO9797Alg3Mac : IMac
	{
		// Token: 0x06002967 RID: 10599 RVA: 0x000CCA9A File Offset: 0x000CAC9A
		public ISO9797Alg3Mac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8, null)
		{
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000CCAAC File Offset: 0x000CACAC
		public ISO9797Alg3Mac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, cipher.GetBlockSize() * 8, padding)
		{
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000CCABE File Offset: 0x000CACBE
		public ISO9797Alg3Mac(IBlockCipher cipher, int macSizeInBits) : this(cipher, macSizeInBits, null)
		{
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000CCACC File Offset: 0x000CACCC
		public ISO9797Alg3Mac(IBlockCipher cipher, int macSizeInBits, IBlockCipherPadding padding)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			if (!(cipher is DesEngine))
			{
				throw new ArgumentException("cipher must be instance of DesEngine");
			}
			this.cipher = new CbcBlockCipher(cipher);
			this.padding = padding;
			this.macSize = macSizeInBits / 8;
			this.mac = new byte[cipher.GetBlockSize()];
			this.buf = new byte[cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600296B RID: 10603 RVA: 0x000CCB47 File Offset: 0x000CAD47
		public string AlgorithmName
		{
			get
			{
				return "ISO9797Alg3";
			}
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000CCB50 File Offset: 0x000CAD50
		public void Init(ICipherParameters parameters)
		{
			this.Reset();
			if (!(parameters is KeyParameter) && !(parameters is ParametersWithIV))
			{
				throw new ArgumentException("parameters must be an instance of KeyParameter or ParametersWithIV");
			}
			KeyParameter keyParameter;
			if (parameters is KeyParameter)
			{
				keyParameter = (KeyParameter)parameters;
			}
			else
			{
				keyParameter = (KeyParameter)((ParametersWithIV)parameters).Parameters;
			}
			byte[] key = keyParameter.GetKey();
			KeyParameter parameters2;
			if (key.Length == 16)
			{
				parameters2 = new KeyParameter(key, 0, 8);
				this.lastKey2 = new KeyParameter(key, 8, 8);
				this.lastKey3 = parameters2;
			}
			else
			{
				if (key.Length != 24)
				{
					throw new ArgumentException("Key must be either 112 or 168 bit long");
				}
				parameters2 = new KeyParameter(key, 0, 8);
				this.lastKey2 = new KeyParameter(key, 8, 8);
				this.lastKey3 = new KeyParameter(key, 16, 8);
			}
			if (parameters is ParametersWithIV)
			{
				this.cipher.Init(true, new ParametersWithIV(parameters2, ((ParametersWithIV)parameters).GetIV()));
				return;
			}
			this.cipher.Init(true, parameters2);
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000CCC3B File Offset: 0x000CAE3B
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000CCC44 File Offset: 0x000CAE44
		public void Update(byte input)
		{
			if (this.bufOff == this.buf.Length)
			{
				this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
				this.bufOff = 0;
			}
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000CCC9C File Offset: 0x000CAE9C
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int blockSize = this.cipher.GetBlockSize();
			int num = 0;
			int num2 = blockSize - this.bufOff;
			if (len > num2)
			{
				Array.Copy(input, inOff, this.buf, this.bufOff, num2);
				num += this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
				this.bufOff = 0;
				len -= num2;
				inOff += num2;
				while (len > blockSize)
				{
					num += this.cipher.ProcessBlock(input, inOff, this.mac, 0);
					len -= blockSize;
					inOff += blockSize;
				}
			}
			Array.Copy(input, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000CCD60 File Offset: 0x000CAF60
		public int DoFinal(byte[] output, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			if (this.padding == null)
			{
				while (this.bufOff < blockSize)
				{
					byte[] array = this.buf;
					int num = this.bufOff;
					this.bufOff = num + 1;
					array[num] = 0;
				}
			}
			else
			{
				if (this.bufOff == blockSize)
				{
					this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
					this.bufOff = 0;
				}
				this.padding.AddPadding(this.buf, this.bufOff);
			}
			this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
			DesEngine desEngine = new DesEngine();
			desEngine.Init(false, this.lastKey2);
			desEngine.ProcessBlock(this.mac, 0, this.mac, 0);
			desEngine.Init(true, this.lastKey3);
			desEngine.ProcessBlock(this.mac, 0, this.mac, 0);
			Array.Copy(this.mac, 0, output, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000CCE6B File Offset: 0x000CB06B
		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x04001B28 RID: 6952
		private byte[] mac;

		// Token: 0x04001B29 RID: 6953
		private byte[] buf;

		// Token: 0x04001B2A RID: 6954
		private int bufOff;

		// Token: 0x04001B2B RID: 6955
		private IBlockCipher cipher;

		// Token: 0x04001B2C RID: 6956
		private IBlockCipherPadding padding;

		// Token: 0x04001B2D RID: 6957
		private int macSize;

		// Token: 0x04001B2E RID: 6958
		private KeyParameter lastKey2;

		// Token: 0x04001B2F RID: 6959
		private KeyParameter lastKey3;
	}
}
