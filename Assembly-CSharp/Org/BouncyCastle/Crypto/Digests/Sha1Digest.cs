using System;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004B8 RID: 1208
	public class Sha1Digest : GeneralDigest
	{
		// Token: 0x06002CE5 RID: 11493 RVA: 0x000EBE88 File Offset: 0x000EA088
		public Sha1Digest()
		{
			this.Reset();
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000EBEA3 File Offset: 0x000EA0A3
		public Sha1Digest(Sha1Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000EBEC0 File Offset: 0x000EA0C0
		private void CopyIn(Sha1Digest t)
		{
			base.CopyIn(t);
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			this.H5 = t.H5;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x000EBF37 File Offset: 0x000EA137
		public override string AlgorithmName
		{
			get
			{
				return "SHA-1";
			}
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000CDF82 File Offset: 0x000CC182
		public override int GetDigestSize()
		{
			return 20;
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000EBF40 File Offset: 0x000EA140
		internal override void ProcessWord(byte[] input, int inOff)
		{
			this.X[this.xOff] = Pack.BE_To_UInt32(input, inOff);
			int num = this.xOff + 1;
			this.xOff = num;
			if (num == 16)
			{
				this.ProcessBlock();
			}
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000EBF7C File Offset: 0x000EA17C
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.ProcessBlock();
			}
			this.X[14] = (uint)((ulong)bitLength >> 32);
			this.X[15] = (uint)bitLength;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000EBFA8 File Offset: 0x000EA1A8
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt32_To_BE(this.H1, output, outOff);
			Pack.UInt32_To_BE(this.H2, output, outOff + 4);
			Pack.UInt32_To_BE(this.H3, output, outOff + 8);
			Pack.UInt32_To_BE(this.H4, output, outOff + 12);
			Pack.UInt32_To_BE(this.H5, output, outOff + 16);
			this.Reset();
			return 20;
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000EC010 File Offset: 0x000EA210
		public override void Reset()
		{
			base.Reset();
			this.H1 = 1732584193U;
			this.H2 = 4023233417U;
			this.H3 = 2562383102U;
			this.H4 = 271733878U;
			this.H5 = 3285377520U;
			this.xOff = 0;
			Array.Clear(this.X, 0, this.X.Length);
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000E4BA6 File Offset: 0x000E2DA6
		private static uint F(uint u, uint v, uint w)
		{
			return (u & v) | (~u & w);
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000E4BBA File Offset: 0x000E2DBA
		private static uint H(uint u, uint v, uint w)
		{
			return u ^ v ^ w;
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000EC075 File Offset: 0x000EA275
		private static uint G(uint u, uint v, uint w)
		{
			return (u & v) | (u & w) | (v & w);
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000EC084 File Offset: 0x000EA284
		internal override void ProcessBlock()
		{
			for (int i = 16; i < 80; i++)
			{
				uint num = this.X[i - 3] ^ this.X[i - 8] ^ this.X[i - 14] ^ this.X[i - 16];
				this.X[i] = (num << 1 | num >> 31);
			}
			uint num2 = this.H1;
			uint num3 = this.H2;
			uint num4 = this.H3;
			uint num5 = this.H4;
			uint num6 = this.H5;
			int num7 = 0;
			for (int j = 0; j < 4; j++)
			{
				num6 += (num2 << 5 | num2 >> 27) + Sha1Digest.F(num3, num4, num5) + this.X[num7++] + 1518500249U;
				num3 = (num3 << 30 | num3 >> 2);
				num5 += (num6 << 5 | num6 >> 27) + Sha1Digest.F(num2, num3, num4) + this.X[num7++] + 1518500249U;
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + Sha1Digest.F(num6, num2, num3) + this.X[num7++] + 1518500249U;
				num6 = (num6 << 30 | num6 >> 2);
				num3 += (num4 << 5 | num4 >> 27) + Sha1Digest.F(num5, num6, num2) + this.X[num7++] + 1518500249U;
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + Sha1Digest.F(num4, num5, num6) + this.X[num7++] + 1518500249U;
				num4 = (num4 << 30 | num4 >> 2);
			}
			for (int k = 0; k < 4; k++)
			{
				num6 += (num2 << 5 | num2 >> 27) + Sha1Digest.H(num3, num4, num5) + this.X[num7++] + 1859775393U;
				num3 = (num3 << 30 | num3 >> 2);
				num5 += (num6 << 5 | num6 >> 27) + Sha1Digest.H(num2, num3, num4) + this.X[num7++] + 1859775393U;
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + Sha1Digest.H(num6, num2, num3) + this.X[num7++] + 1859775393U;
				num6 = (num6 << 30 | num6 >> 2);
				num3 += (num4 << 5 | num4 >> 27) + Sha1Digest.H(num5, num6, num2) + this.X[num7++] + 1859775393U;
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + Sha1Digest.H(num4, num5, num6) + this.X[num7++] + 1859775393U;
				num4 = (num4 << 30 | num4 >> 2);
			}
			for (int l = 0; l < 4; l++)
			{
				num6 += (num2 << 5 | num2 >> 27) + Sha1Digest.G(num3, num4, num5) + this.X[num7++] + 2400959708U;
				num3 = (num3 << 30 | num3 >> 2);
				num5 += (num6 << 5 | num6 >> 27) + Sha1Digest.G(num2, num3, num4) + this.X[num7++] + 2400959708U;
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + Sha1Digest.G(num6, num2, num3) + this.X[num7++] + 2400959708U;
				num6 = (num6 << 30 | num6 >> 2);
				num3 += (num4 << 5 | num4 >> 27) + Sha1Digest.G(num5, num6, num2) + this.X[num7++] + 2400959708U;
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + Sha1Digest.G(num4, num5, num6) + this.X[num7++] + 2400959708U;
				num4 = (num4 << 30 | num4 >> 2);
			}
			for (int m = 0; m < 4; m++)
			{
				num6 += (num2 << 5 | num2 >> 27) + Sha1Digest.H(num3, num4, num5) + this.X[num7++] + 3395469782U;
				num3 = (num3 << 30 | num3 >> 2);
				num5 += (num6 << 5 | num6 >> 27) + Sha1Digest.H(num2, num3, num4) + this.X[num7++] + 3395469782U;
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + Sha1Digest.H(num6, num2, num3) + this.X[num7++] + 3395469782U;
				num6 = (num6 << 30 | num6 >> 2);
				num3 += (num4 << 5 | num4 >> 27) + Sha1Digest.H(num5, num6, num2) + this.X[num7++] + 3395469782U;
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + Sha1Digest.H(num4, num5, num6) + this.X[num7++] + 3395469782U;
				num4 = (num4 << 30 | num4 >> 2);
			}
			this.H1 += num2;
			this.H2 += num3;
			this.H3 += num4;
			this.H4 += num5;
			this.H5 += num6;
			this.xOff = 0;
			Array.Clear(this.X, 0, 16);
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000EC5D4 File Offset: 0x000EA7D4
		public override IMemoable Copy()
		{
			return new Sha1Digest(this);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000EC5DC File Offset: 0x000EA7DC
		public override void Reset(IMemoable other)
		{
			Sha1Digest t = (Sha1Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x04001D84 RID: 7556
		private const int DigestLength = 20;

		// Token: 0x04001D85 RID: 7557
		private uint H1;

		// Token: 0x04001D86 RID: 7558
		private uint H2;

		// Token: 0x04001D87 RID: 7559
		private uint H3;

		// Token: 0x04001D88 RID: 7560
		private uint H4;

		// Token: 0x04001D89 RID: 7561
		private uint H5;

		// Token: 0x04001D8A RID: 7562
		private uint[] X = new uint[80];

		// Token: 0x04001D8B RID: 7563
		private int xOff;

		// Token: 0x04001D8C RID: 7564
		private const uint Y1 = 1518500249U;

		// Token: 0x04001D8D RID: 7565
		private const uint Y2 = 1859775393U;

		// Token: 0x04001D8E RID: 7566
		private const uint Y3 = 2400959708U;

		// Token: 0x04001D8F RID: 7567
		private const uint Y4 = 3395469782U;
	}
}
