using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200046E RID: 1134
	public class Gost28147Mac : IMac
	{
		// Token: 0x0600294F RID: 10575 RVA: 0x000CC230 File Offset: 0x000CA430
		public Gost28147Mac()
		{
			this.mac = new byte[8];
			this.buf = new byte[8];
			this.bufOff = 0;
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000CC284 File Offset: 0x000CA484
		private static int[] generateWorkingKey(byte[] userKey)
		{
			if (userKey.Length != 32)
			{
				throw new ArgumentException("Key length invalid. Key needs to be 32 byte - 256 bit!!!");
			}
			int[] array = new int[8];
			for (int num = 0; num != 8; num++)
			{
				array[num] = Gost28147Mac.bytesToint(userKey, num * 4);
			}
			return array;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000CC2C4 File Offset: 0x000CA4C4
		public void Init(ICipherParameters parameters)
		{
			this.Reset();
			this.buf = new byte[8];
			if (parameters is ParametersWithSBox)
			{
				ParametersWithSBox parametersWithSBox = (ParametersWithSBox)parameters;
				parametersWithSBox.GetSBox().CopyTo(this.S, 0);
				if (parametersWithSBox.Parameters != null)
				{
					this.workingKey = Gost28147Mac.generateWorkingKey(((KeyParameter)parametersWithSBox.Parameters).GetKey());
					return;
				}
				return;
			}
			else
			{
				if (parameters is KeyParameter)
				{
					this.workingKey = Gost28147Mac.generateWorkingKey(((KeyParameter)parameters).GetKey());
					return;
				}
				throw new ArgumentException("invalid parameter passed to Gost28147 init - " + Platform.GetTypeName(parameters));
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06002952 RID: 10578 RVA: 0x000CC35D File Offset: 0x000CA55D
		public string AlgorithmName
		{
			get
			{
				return "Gost28147Mac";
			}
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000CC364 File Offset: 0x000CA564
		public int GetMacSize()
		{
			return 4;
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000CC368 File Offset: 0x000CA568
		private int gost28147_mainStep(int n1, int key)
		{
			int num = key + n1;
			int num2 = (int)this.S[num & 15] + ((int)this.S[16 + (num >> 4 & 15)] << 4) + ((int)this.S[32 + (num >> 8 & 15)] << 8) + ((int)this.S[48 + (num >> 12 & 15)] << 12) + ((int)this.S[64 + (num >> 16 & 15)] << 16) + ((int)this.S[80 + (num >> 20 & 15)] << 20) + ((int)this.S[96 + (num >> 24 & 15)] << 24) + ((int)this.S[112 + (num >> 28 & 15)] << 28);
			int num3 = num2 << 11;
			int num4 = (int)((uint)num2 >> 21);
			return num3 | num4;
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000CC420 File Offset: 0x000CA620
		private void gost28147MacFunc(int[] workingKey, byte[] input, int inOff, byte[] output, int outOff)
		{
			int num = Gost28147Mac.bytesToint(input, inOff);
			int num2 = Gost28147Mac.bytesToint(input, inOff + 4);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					int num3 = num;
					num = (num2 ^ this.gost28147_mainStep(num, workingKey[j]));
					num2 = num3;
				}
			}
			Gost28147Mac.intTobytes(num, output, outOff);
			Gost28147Mac.intTobytes(num2, output, outOff + 4);
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000CC47C File Offset: 0x000CA67C
		private static int bytesToint(byte[] input, int inOff)
		{
			return (int)((long)((long)input[inOff + 3] << 24) & (long)((ulong)-16777216)) + ((int)input[inOff + 2] << 16 & 16711680) + ((int)input[inOff + 1] << 8 & 65280) + (int)(input[inOff] & byte.MaxValue);
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000CC4B6 File Offset: 0x000CA6B6
		private static void intTobytes(int num, byte[] output, int outOff)
		{
			output[outOff + 3] = (byte)(num >> 24);
			output[outOff + 2] = (byte)(num >> 16);
			output[outOff + 1] = (byte)(num >> 8);
			output[outOff] = (byte)num;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000CC4DC File Offset: 0x000CA6DC
		private static byte[] CM5func(byte[] buf, int bufOff, byte[] mac)
		{
			byte[] array = new byte[buf.Length - bufOff];
			Array.Copy(buf, bufOff, array, 0, mac.Length);
			for (int num = 0; num != mac.Length; num++)
			{
				array[num] ^= mac[num];
			}
			return array;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000CC51C File Offset: 0x000CA71C
		public void Update(byte input)
		{
			if (this.bufOff == this.buf.Length)
			{
				byte[] array = new byte[this.buf.Length];
				Array.Copy(this.buf, 0, array, 0, this.mac.Length);
				if (this.firstStep)
				{
					this.firstStep = false;
				}
				else
				{
					array = Gost28147Mac.CM5func(this.buf, 0, this.mac);
				}
				this.gost28147MacFunc(this.workingKey, array, 0, this.mac, 0);
				this.bufOff = 0;
			}
			byte[] array2 = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array2[num] = input;
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000CC5B8 File Offset: 0x000CA7B8
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int num = 8 - this.bufOff;
			if (len > num)
			{
				Array.Copy(input, inOff, this.buf, this.bufOff, num);
				byte[] array = new byte[this.buf.Length];
				Array.Copy(this.buf, 0, array, 0, this.mac.Length);
				if (this.firstStep)
				{
					this.firstStep = false;
				}
				else
				{
					array = Gost28147Mac.CM5func(this.buf, 0, this.mac);
				}
				this.gost28147MacFunc(this.workingKey, array, 0, this.mac, 0);
				this.bufOff = 0;
				len -= num;
				inOff += num;
				while (len > 8)
				{
					array = Gost28147Mac.CM5func(input, inOff, this.mac);
					this.gost28147MacFunc(this.workingKey, array, 0, this.mac, 0);
					len -= 8;
					inOff += 8;
				}
			}
			Array.Copy(input, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000CC6BC File Offset: 0x000CA8BC
		public int DoFinal(byte[] output, int outOff)
		{
			while (this.bufOff < 8)
			{
				byte[] array = this.buf;
				int num = this.bufOff;
				this.bufOff = num + 1;
				array[num] = 0;
			}
			byte[] array2 = new byte[this.buf.Length];
			Array.Copy(this.buf, 0, array2, 0, this.mac.Length);
			if (this.firstStep)
			{
				this.firstStep = false;
			}
			else
			{
				array2 = Gost28147Mac.CM5func(this.buf, 0, this.mac);
			}
			this.gost28147MacFunc(this.workingKey, array2, 0, this.mac, 0);
			Array.Copy(this.mac, this.mac.Length / 2 - 4, output, outOff, 4);
			this.Reset();
			return 4;
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000CC76B File Offset: 0x000CA96B
		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.firstStep = true;
		}

		// Token: 0x04001B17 RID: 6935
		private const int blockSize = 8;

		// Token: 0x04001B18 RID: 6936
		private const int macSize = 4;

		// Token: 0x04001B19 RID: 6937
		private int bufOff;

		// Token: 0x04001B1A RID: 6938
		private byte[] buf;

		// Token: 0x04001B1B RID: 6939
		private byte[] mac;

		// Token: 0x04001B1C RID: 6940
		private bool firstStep = true;

		// Token: 0x04001B1D RID: 6941
		private int[] workingKey;

		// Token: 0x04001B1E RID: 6942
		private byte[] S = new byte[]
		{
			9,
			6,
			3,
			2,
			8,
			11,
			1,
			7,
			10,
			4,
			14,
			15,
			12,
			0,
			13,
			5,
			3,
			7,
			14,
			9,
			8,
			10,
			15,
			0,
			5,
			2,
			6,
			12,
			11,
			4,
			13,
			1,
			14,
			4,
			6,
			2,
			11,
			3,
			13,
			8,
			12,
			15,
			5,
			10,
			0,
			7,
			1,
			9,
			14,
			7,
			10,
			12,
			13,
			1,
			3,
			9,
			0,
			2,
			11,
			4,
			15,
			8,
			5,
			6,
			11,
			5,
			1,
			9,
			8,
			13,
			15,
			0,
			14,
			4,
			2,
			3,
			12,
			7,
			10,
			6,
			3,
			10,
			13,
			12,
			1,
			2,
			0,
			11,
			7,
			5,
			9,
			4,
			8,
			15,
			14,
			6,
			1,
			13,
			2,
			9,
			7,
			10,
			6,
			0,
			8,
			12,
			4,
			5,
			15,
			3,
			11,
			14,
			11,
			10,
			15,
			5,
			0,
			12,
			14,
			8,
			6,
			2,
			3,
			9,
			1,
			7,
			13,
			4
		};
	}
}
