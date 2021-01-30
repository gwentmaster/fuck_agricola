using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004B0 RID: 1200
	public class MD4Digest : GeneralDigest
	{
		// Token: 0x06002C58 RID: 11352 RVA: 0x000E4077 File Offset: 0x000E2277
		public MD4Digest()
		{
			this.Reset();
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x000E4092 File Offset: 0x000E2292
		public MD4Digest(MD4Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x000E40B0 File Offset: 0x000E22B0
		private void CopyIn(MD4Digest t)
		{
			base.CopyIn(t);
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x000E411B File Offset: 0x000E231B
		public override string AlgorithmName
		{
			get
			{
				return "MD4";
			}
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x000C8990 File Offset: 0x000C6B90
		public override int GetDigestSize()
		{
			return 16;
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x000E4124 File Offset: 0x000E2324
		internal override void ProcessWord(byte[] input, int inOff)
		{
			int[] x = this.X;
			int num = this.xOff;
			this.xOff = num + 1;
			x[num] = ((int)(input[inOff] & byte.MaxValue) | (int)(input[inOff + 1] & byte.MaxValue) << 8 | (int)(input[inOff + 2] & byte.MaxValue) << 16 | (int)(input[inOff + 3] & byte.MaxValue) << 24);
			if (this.xOff == 16)
			{
				this.ProcessBlock();
			}
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000E418E File Offset: 0x000E238E
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.ProcessBlock();
			}
			this.X[14] = (int)(bitLength & (long)((ulong)-1));
			this.X[15] = (int)((ulong)bitLength >> 32);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000E41BC File Offset: 0x000E23BC
		private void UnpackWord(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)word;
			outBytes[outOff + 1] = (byte)((uint)word >> 8);
			outBytes[outOff + 2] = (byte)((uint)word >> 16);
			outBytes[outOff + 3] = (byte)((uint)word >> 24);
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000E41E0 File Offset: 0x000E23E0
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			this.UnpackWord(this.H1, output, outOff);
			this.UnpackWord(this.H2, output, outOff + 4);
			this.UnpackWord(this.H3, output, outOff + 8);
			this.UnpackWord(this.H4, output, outOff + 12);
			this.Reset();
			return 16;
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x000E423C File Offset: 0x000E243C
		public override void Reset()
		{
			base.Reset();
			this.H1 = 1732584193;
			this.H2 = -271733879;
			this.H3 = -1732584194;
			this.H4 = 271733878;
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0;
			}
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000D7CBE File Offset: 0x000D5EBE
		private int RotateLeft(int x, int n)
		{
			return x << n | (int)((uint)x >> 32 - n);
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000E429E File Offset: 0x000E249E
		private int F(int u, int v, int w)
		{
			return (u & v) | (~u & w);
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000E42A8 File Offset: 0x000E24A8
		private int G(int u, int v, int w)
		{
			return (u & v) | (u & w) | (v & w);
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000E42B5 File Offset: 0x000E24B5
		private int H(int u, int v, int w)
		{
			return u ^ v ^ w;
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000E42BC File Offset: 0x000E24BC
		internal override void ProcessBlock()
		{
			int num = this.H1;
			int num2 = this.H2;
			int num3 = this.H3;
			int num4 = this.H4;
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[0], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[1], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[2], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[3], 19);
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[4], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[5], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[6], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[7], 19);
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[8], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[9], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[10], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[11], 19);
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[12], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[13], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[14], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[15], 19);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[0] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[4] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[8] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[12] + 1518500249, 13);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[1] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[5] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[9] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[13] + 1518500249, 13);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[2] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[6] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[10] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[14] + 1518500249, 13);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[3] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[7] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[11] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[15] + 1518500249, 13);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[0] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[8] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[4] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[12] + 1859775393, 15);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[2] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[10] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[6] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[14] + 1859775393, 15);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[1] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[9] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[5] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[13] + 1859775393, 15);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[3] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[11] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[7] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[15] + 1859775393, 15);
			this.H1 += num;
			this.H2 += num2;
			this.H3 += num3;
			this.H4 += num4;
			this.xOff = 0;
			for (int num5 = 0; num5 != this.X.Length; num5++)
			{
				this.X[num5] = 0;
			}
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000E4976 File Offset: 0x000E2B76
		public override IMemoable Copy()
		{
			return new MD4Digest(this);
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000E4980 File Offset: 0x000E2B80
		public override void Reset(IMemoable other)
		{
			MD4Digest t = (MD4Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x04001D32 RID: 7474
		private const int DigestLength = 16;

		// Token: 0x04001D33 RID: 7475
		private int H1;

		// Token: 0x04001D34 RID: 7476
		private int H2;

		// Token: 0x04001D35 RID: 7477
		private int H3;

		// Token: 0x04001D36 RID: 7478
		private int H4;

		// Token: 0x04001D37 RID: 7479
		private int[] X = new int[16];

		// Token: 0x04001D38 RID: 7480
		private int xOff;

		// Token: 0x04001D39 RID: 7481
		private const int S11 = 3;

		// Token: 0x04001D3A RID: 7482
		private const int S12 = 7;

		// Token: 0x04001D3B RID: 7483
		private const int S13 = 11;

		// Token: 0x04001D3C RID: 7484
		private const int S14 = 19;

		// Token: 0x04001D3D RID: 7485
		private const int S21 = 3;

		// Token: 0x04001D3E RID: 7486
		private const int S22 = 5;

		// Token: 0x04001D3F RID: 7487
		private const int S23 = 9;

		// Token: 0x04001D40 RID: 7488
		private const int S24 = 13;

		// Token: 0x04001D41 RID: 7489
		private const int S31 = 3;

		// Token: 0x04001D42 RID: 7490
		private const int S32 = 9;

		// Token: 0x04001D43 RID: 7491
		private const int S33 = 11;

		// Token: 0x04001D44 RID: 7492
		private const int S34 = 15;
	}
}
