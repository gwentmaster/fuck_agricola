using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200048C RID: 1164
	public class HC128Engine : IStreamCipher
	{
		// Token: 0x06002A5F RID: 10847 RVA: 0x000D637A File Offset: 0x000D457A
		private static uint F1(uint x)
		{
			return HC128Engine.RotateRight(x, 7) ^ HC128Engine.RotateRight(x, 18) ^ x >> 3;
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000D6390 File Offset: 0x000D4590
		private static uint F2(uint x)
		{
			return HC128Engine.RotateRight(x, 17) ^ HC128Engine.RotateRight(x, 19) ^ x >> 10;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000D63A8 File Offset: 0x000D45A8
		private uint G1(uint x, uint y, uint z)
		{
			return (HC128Engine.RotateRight(x, 10) ^ HC128Engine.RotateRight(z, 23)) + HC128Engine.RotateRight(y, 8);
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000D63C3 File Offset: 0x000D45C3
		private uint G2(uint x, uint y, uint z)
		{
			return (HC128Engine.RotateLeft(x, 10) ^ HC128Engine.RotateLeft(z, 23)) + HC128Engine.RotateLeft(y, 8);
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000D63DE File Offset: 0x000D45DE
		private static uint RotateLeft(uint x, int bits)
		{
			return x << bits | x >> -bits;
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000D63EE File Offset: 0x000D45EE
		private static uint RotateRight(uint x, int bits)
		{
			return x >> bits | x << -bits;
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000D63FE File Offset: 0x000D45FE
		private uint H1(uint x)
		{
			return this.q[(int)(x & 255U)] + this.q[(int)((x >> 16 & 255U) + 256U)];
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000D6426 File Offset: 0x000D4626
		private uint H2(uint x)
		{
			return this.p[(int)(x & 255U)] + this.p[(int)((x >> 16 & 255U) + 256U)];
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000D644E File Offset: 0x000D464E
		private static uint Mod1024(uint x)
		{
			return x & 1023U;
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000D6457 File Offset: 0x000D4657
		private static uint Mod512(uint x)
		{
			return x & 511U;
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000D6460 File Offset: 0x000D4660
		private static uint Dim(uint x, uint y)
		{
			return HC128Engine.Mod512(x - y);
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000D646C File Offset: 0x000D466C
		private uint Step()
		{
			uint num = HC128Engine.Mod512(this.cnt);
			uint result;
			if (this.cnt < 512U)
			{
				this.p[(int)num] += this.G1(this.p[(int)HC128Engine.Dim(num, 3U)], this.p[(int)HC128Engine.Dim(num, 10U)], this.p[(int)HC128Engine.Dim(num, 511U)]);
				result = (this.H1(this.p[(int)HC128Engine.Dim(num, 12U)]) ^ this.p[(int)num]);
			}
			else
			{
				this.q[(int)num] += this.G2(this.q[(int)HC128Engine.Dim(num, 3U)], this.q[(int)HC128Engine.Dim(num, 10U)], this.q[(int)HC128Engine.Dim(num, 511U)]);
				result = (this.H2(this.q[(int)HC128Engine.Dim(num, 12U)]) ^ this.q[(int)num]);
			}
			this.cnt = HC128Engine.Mod1024(this.cnt + 1U);
			return result;
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000D6570 File Offset: 0x000D4770
		private void Init()
		{
			if (this.key.Length != 16)
			{
				throw new ArgumentException("The key must be 128 bits long");
			}
			this.idx = 0;
			this.cnt = 0U;
			uint[] array = new uint[1280];
			for (int i = 0; i < 16; i++)
			{
				array[i >> 2] |= (uint)((uint)this.key[i] << 8 * (i & 3));
			}
			Array.Copy(array, 0, array, 4, 4);
			int num = 0;
			while (num < this.iv.Length && num < 16)
			{
				array[(num >> 2) + 8] |= (uint)((uint)this.iv[num] << 8 * (num & 3));
				num++;
			}
			Array.Copy(array, 8, array, 12, 4);
			for (uint num2 = 16U; num2 < 1280U; num2 += 1U)
			{
				array[(int)num2] = HC128Engine.F2(array[(int)(num2 - 2U)]) + array[(int)(num2 - 7U)] + HC128Engine.F1(array[(int)(num2 - 15U)]) + array[(int)(num2 - 16U)] + num2;
			}
			Array.Copy(array, 256, this.p, 0, 512);
			Array.Copy(array, 768, this.q, 0, 512);
			for (int j = 0; j < 512; j++)
			{
				this.p[j] = this.Step();
			}
			for (int k = 0; k < 512; k++)
			{
				this.q[k] = this.Step();
			}
			this.cnt = 0U;
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x000D66D9 File Offset: 0x000D48D9
		public virtual string AlgorithmName
		{
			get
			{
				return "HC-128";
			}
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000D66E0 File Offset: 0x000D48E0
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			ICipherParameters cipherParameters = parameters;
			if (parameters is ParametersWithIV)
			{
				this.iv = ((ParametersWithIV)parameters).GetIV();
				cipherParameters = ((ParametersWithIV)parameters).Parameters;
			}
			else
			{
				this.iv = new byte[0];
			}
			if (cipherParameters is KeyParameter)
			{
				this.key = ((KeyParameter)cipherParameters).GetKey();
				this.Init();
				this.initialised = true;
				return;
			}
			throw new ArgumentException("Invalid parameter passed to HC128 init - " + Platform.GetTypeName(parameters), "parameters");
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x000D6765 File Offset: 0x000D4965
		private byte GetByte()
		{
			if (this.idx == 0)
			{
				Pack.UInt32_To_LE(this.Step(), this.buf);
			}
			byte result = this.buf[this.idx];
			this.idx = (this.idx + 1 & 3);
			return result;
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000D67A0 File Offset: 0x000D49A0
		public virtual void ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			if (!this.initialised)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(input, inOff, len, "input buffer too short");
			Check.OutputLength(output, outOff, len, "output buffer too short");
			for (int i = 0; i < len; i++)
			{
				output[outOff + i] = (input[inOff + i] ^ this.GetByte());
			}
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000D6807 File Offset: 0x000D4A07
		public virtual void Reset()
		{
			this.Init();
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x000D680F File Offset: 0x000D4A0F
		public virtual byte ReturnByte(byte input)
		{
			return input ^ this.GetByte();
		}

		// Token: 0x04001BFD RID: 7165
		private uint[] p = new uint[512];

		// Token: 0x04001BFE RID: 7166
		private uint[] q = new uint[512];

		// Token: 0x04001BFF RID: 7167
		private uint cnt;

		// Token: 0x04001C00 RID: 7168
		private byte[] key;

		// Token: 0x04001C01 RID: 7169
		private byte[] iv;

		// Token: 0x04001C02 RID: 7170
		private bool initialised;

		// Token: 0x04001C03 RID: 7171
		private byte[] buf = new byte[4];

		// Token: 0x04001C04 RID: 7172
		private int idx;
	}
}
