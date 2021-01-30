using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004AF RID: 1199
	public class MD2Digest : IDigest, IMemoable
	{
		// Token: 0x06002C49 RID: 11337 RVA: 0x000E3C99 File Offset: 0x000E1E99
		public MD2Digest()
		{
			this.Reset();
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000E3CCE File Offset: 0x000E1ECE
		public MD2Digest(MD2Digest t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000E3D04 File Offset: 0x000E1F04
		private void CopyIn(MD2Digest t)
		{
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
			Array.Copy(t.M, 0, this.M, 0, t.M.Length);
			this.mOff = t.mOff;
			Array.Copy(t.C, 0, this.C, 0, t.C.Length);
			this.COff = t.COff;
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000E3D86 File Offset: 0x000E1F86
		public string AlgorithmName
		{
			get
			{
				return "MD2";
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000C8990 File Offset: 0x000C6B90
		public int GetDigestSize()
		{
			return 16;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000C8990 File Offset: 0x000C6B90
		public int GetByteLength()
		{
			return 16;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000E3D90 File Offset: 0x000E1F90
		public int DoFinal(byte[] output, int outOff)
		{
			byte b = (byte)(this.M.Length - this.mOff);
			for (int i = this.mOff; i < this.M.Length; i++)
			{
				this.M[i] = b;
			}
			this.ProcessChecksum(this.M);
			this.ProcessBlock(this.M);
			this.ProcessBlock(this.C);
			Array.Copy(this.X, this.xOff, output, outOff, 16);
			this.Reset();
			return 16;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x000E3E10 File Offset: 0x000E2010
		public void Reset()
		{
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0;
			}
			this.mOff = 0;
			for (int num2 = 0; num2 != this.M.Length; num2++)
			{
				this.M[num2] = 0;
			}
			this.COff = 0;
			for (int num3 = 0; num3 != this.C.Length; num3++)
			{
				this.C[num3] = 0;
			}
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x000E3E88 File Offset: 0x000E2088
		public void Update(byte input)
		{
			byte[] m = this.M;
			int num = this.mOff;
			this.mOff = num + 1;
			m[num] = input;
			if (this.mOff == 16)
			{
				this.ProcessChecksum(this.M);
				this.ProcessBlock(this.M);
				this.mOff = 0;
			}
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x000E3ED8 File Offset: 0x000E20D8
		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (this.mOff != 0)
			{
				if (length <= 0)
				{
					break;
				}
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
			while (length > 16)
			{
				Array.Copy(input, inOff, this.M, 0, 16);
				this.ProcessChecksum(this.M);
				this.ProcessBlock(this.M);
				length -= 16;
				inOff += 16;
			}
			while (length > 0)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x000E3F5C File Offset: 0x000E215C
		internal void ProcessChecksum(byte[] m)
		{
			int num = (int)this.C[15];
			for (int i = 0; i < 16; i++)
			{
				byte[] c = this.C;
				int num2 = i;
				c[num2] ^= MD2Digest.S[((int)m[i] ^ num) & 255];
				num = (int)this.C[i];
			}
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000E3FAC File Offset: 0x000E21AC
		internal void ProcessBlock(byte[] m)
		{
			for (int i = 0; i < 16; i++)
			{
				this.X[i + 16] = m[i];
				this.X[i + 32] = (m[i] ^ this.X[i]);
			}
			int num = 0;
			for (int j = 0; j < 18; j++)
			{
				for (int k = 0; k < 48; k++)
				{
					byte[] x = this.X;
					int num2 = k;
					num = (int)(x[num2] ^= MD2Digest.S[num]);
					num &= 255;
				}
				num = (num + j) % 256;
			}
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x000E4038 File Offset: 0x000E2238
		public IMemoable Copy()
		{
			return new MD2Digest(this);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x000E4040 File Offset: 0x000E2240
		public void Reset(IMemoable other)
		{
			MD2Digest t = (MD2Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x04001D29 RID: 7465
		private const int DigestLength = 16;

		// Token: 0x04001D2A RID: 7466
		private const int BYTE_LENGTH = 16;

		// Token: 0x04001D2B RID: 7467
		private byte[] X = new byte[48];

		// Token: 0x04001D2C RID: 7468
		private int xOff;

		// Token: 0x04001D2D RID: 7469
		private byte[] M = new byte[16];

		// Token: 0x04001D2E RID: 7470
		private int mOff;

		// Token: 0x04001D2F RID: 7471
		private byte[] C = new byte[16];

		// Token: 0x04001D30 RID: 7472
		private int COff;

		// Token: 0x04001D31 RID: 7473
		private static readonly byte[] S = new byte[]
		{
			41,
			46,
			67,
			201,
			162,
			216,
			124,
			1,
			61,
			54,
			84,
			161,
			236,
			240,
			6,
			19,
			98,
			167,
			5,
			243,
			192,
			199,
			115,
			140,
			152,
			147,
			43,
			217,
			188,
			76,
			130,
			202,
			30,
			155,
			87,
			60,
			253,
			212,
			224,
			22,
			103,
			66,
			111,
			24,
			138,
			23,
			229,
			18,
			190,
			78,
			196,
			214,
			218,
			158,
			222,
			73,
			160,
			251,
			245,
			142,
			187,
			47,
			238,
			122,
			169,
			104,
			121,
			145,
			21,
			178,
			7,
			63,
			148,
			194,
			16,
			137,
			11,
			34,
			95,
			33,
			128,
			127,
			93,
			154,
			90,
			144,
			50,
			39,
			53,
			62,
			204,
			231,
			191,
			247,
			151,
			3,
			byte.MaxValue,
			25,
			48,
			179,
			72,
			165,
			181,
			209,
			215,
			94,
			146,
			42,
			172,
			86,
			170,
			198,
			79,
			184,
			56,
			210,
			150,
			164,
			125,
			182,
			118,
			252,
			107,
			226,
			156,
			116,
			4,
			241,
			69,
			157,
			112,
			89,
			100,
			113,
			135,
			32,
			134,
			91,
			207,
			101,
			230,
			45,
			168,
			2,
			27,
			96,
			37,
			173,
			174,
			176,
			185,
			246,
			28,
			70,
			97,
			105,
			52,
			64,
			126,
			15,
			85,
			71,
			163,
			35,
			221,
			81,
			175,
			58,
			195,
			92,
			249,
			206,
			186,
			197,
			234,
			38,
			44,
			83,
			13,
			110,
			133,
			40,
			132,
			9,
			211,
			223,
			205,
			244,
			65,
			129,
			77,
			82,
			106,
			220,
			55,
			200,
			108,
			193,
			171,
			250,
			36,
			225,
			123,
			8,
			12,
			189,
			177,
			74,
			120,
			136,
			149,
			139,
			227,
			99,
			232,
			109,
			233,
			203,
			213,
			254,
			59,
			0,
			29,
			57,
			242,
			239,
			183,
			14,
			102,
			88,
			208,
			228,
			166,
			119,
			114,
			248,
			235,
			117,
			75,
			10,
			49,
			68,
			80,
			180,
			143,
			237,
			31,
			26,
			219,
			153,
			141,
			51,
			159,
			17,
			131,
			20
		};
	}
}
