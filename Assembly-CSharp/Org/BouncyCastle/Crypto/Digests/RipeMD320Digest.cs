﻿using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004B6 RID: 1206
	public class RipeMD320Digest : GeneralDigest
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06002CCA RID: 11466 RVA: 0x000E9977 File Offset: 0x000E7B77
		public override string AlgorithmName
		{
			get
			{
				return "RIPEMD320";
			}
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000E997E File Offset: 0x000E7B7E
		public override int GetDigestSize()
		{
			return 40;
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000E9982 File Offset: 0x000E7B82
		public RipeMD320Digest()
		{
			this.Reset();
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000E999D File Offset: 0x000E7B9D
		public RipeMD320Digest(RipeMD320Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000E99BC File Offset: 0x000E7BBC
		private void CopyIn(RipeMD320Digest t)
		{
			base.CopyIn(t);
			this.H0 = t.H0;
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			this.H5 = t.H5;
			this.H6 = t.H6;
			this.H7 = t.H7;
			this.H8 = t.H8;
			this.H9 = t.H9;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000E9A70 File Offset: 0x000E7C70
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

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000E9ADA File Offset: 0x000E7CDA
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.ProcessBlock();
			}
			this.X[14] = (int)(bitLength & (long)((ulong)-1));
			this.X[15] = (int)((ulong)bitLength >> 32);
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000E41BC File Offset: 0x000E23BC
		private void UnpackWord(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)word;
			outBytes[outOff + 1] = (byte)((uint)word >> 8);
			outBytes[outOff + 2] = (byte)((uint)word >> 16);
			outBytes[outOff + 3] = (byte)((uint)word >> 24);
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000E9B08 File Offset: 0x000E7D08
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			this.UnpackWord(this.H0, output, outOff);
			this.UnpackWord(this.H1, output, outOff + 4);
			this.UnpackWord(this.H2, output, outOff + 8);
			this.UnpackWord(this.H3, output, outOff + 12);
			this.UnpackWord(this.H4, output, outOff + 16);
			this.UnpackWord(this.H5, output, outOff + 20);
			this.UnpackWord(this.H6, output, outOff + 24);
			this.UnpackWord(this.H7, output, outOff + 28);
			this.UnpackWord(this.H8, output, outOff + 32);
			this.UnpackWord(this.H9, output, outOff + 36);
			this.Reset();
			return 40;
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000E9BC8 File Offset: 0x000E7DC8
		public override void Reset()
		{
			base.Reset();
			this.H0 = 1732584193;
			this.H1 = -271733879;
			this.H2 = -1732584194;
			this.H3 = 271733878;
			this.H4 = -1009589776;
			this.H5 = 1985229328;
			this.H6 = -19088744;
			this.H7 = -1985229329;
			this.H8 = 19088743;
			this.H9 = 1009589775;
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0;
			}
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000D7CBE File Offset: 0x000D5EBE
		private int RL(int x, int n)
		{
			return x << n | (int)((uint)x >> 32 - n);
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000E42B5 File Offset: 0x000E24B5
		private int F1(int x, int y, int z)
		{
			return x ^ y ^ z;
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000E429E File Offset: 0x000E249E
		private int F2(int x, int y, int z)
		{
			return (x & y) | (~x & z);
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000E58E2 File Offset: 0x000E3AE2
		private int F3(int x, int y, int z)
		{
			return (x | ~y) ^ z;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000E58EA File Offset: 0x000E3AEA
		private int F4(int x, int y, int z)
		{
			return (x & z) | (y & ~z);
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000E6881 File Offset: 0x000E4A81
		private int F5(int x, int y, int z)
		{
			return x ^ (y | ~z);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000E9C6C File Offset: 0x000E7E6C
		internal override void ProcessBlock()
		{
			int num = this.H0;
			int num2 = this.H1;
			int num3 = this.H2;
			int num4 = this.H3;
			int num5 = this.H4;
			int num6 = this.H5;
			int num7 = this.H6;
			int num8 = this.H7;
			int num9 = this.H8;
			int num10 = this.H9;
			num = this.RL(num + this.F1(num2, num3, num4) + this.X[0], 11) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F1(num, num2, num3) + this.X[1], 14) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F1(num5, num, num2) + this.X[2], 15) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F1(num4, num5, num) + this.X[3], 12) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F1(num3, num4, num5) + this.X[4], 5) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F1(num2, num3, num4) + this.X[5], 8) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F1(num, num2, num3) + this.X[6], 7) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F1(num5, num, num2) + this.X[7], 9) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F1(num4, num5, num) + this.X[8], 11) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F1(num3, num4, num5) + this.X[9], 13) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F1(num2, num3, num4) + this.X[10], 14) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F1(num, num2, num3) + this.X[11], 15) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F1(num5, num, num2) + this.X[12], 6) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F1(num4, num5, num) + this.X[13], 7) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F1(num3, num4, num5) + this.X[14], 9) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F1(num2, num3, num4) + this.X[15], 8) + num5;
			num3 = this.RL(num3, 10);
			num6 = this.RL(num6 + this.F5(num7, num8, num9) + this.X[5] + 1352829926, 8) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F5(num6, num7, num8) + this.X[14] + 1352829926, 9) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F5(num10, num6, num7) + this.X[7] + 1352829926, 9) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F5(num9, num10, num6) + this.X[0] + 1352829926, 11) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F5(num8, num9, num10) + this.X[9] + 1352829926, 13) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F5(num7, num8, num9) + this.X[2] + 1352829926, 15) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F5(num6, num7, num8) + this.X[11] + 1352829926, 15) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F5(num10, num6, num7) + this.X[4] + 1352829926, 5) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F5(num9, num10, num6) + this.X[13] + 1352829926, 7) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F5(num8, num9, num10) + this.X[6] + 1352829926, 7) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F5(num7, num8, num9) + this.X[15] + 1352829926, 8) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F5(num6, num7, num8) + this.X[8] + 1352829926, 11) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F5(num10, num6, num7) + this.X[1] + 1352829926, 14) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F5(num9, num10, num6) + this.X[10] + 1352829926, 14) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F5(num8, num9, num10) + this.X[3] + 1352829926, 12) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F5(num7, num8, num9) + this.X[12] + 1352829926, 6) + num10;
			num8 = this.RL(num8, 10);
			int num11 = num;
			num = num6;
			num6 = num11;
			num5 = this.RL(num5 + this.F2(num, num2, num3) + this.X[7] + 1518500249, 7) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F2(num5, num, num2) + this.X[4] + 1518500249, 6) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F2(num4, num5, num) + this.X[13] + 1518500249, 8) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F2(num3, num4, num5) + this.X[1] + 1518500249, 13) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F2(num2, num3, num4) + this.X[10] + 1518500249, 11) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F2(num, num2, num3) + this.X[6] + 1518500249, 9) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F2(num5, num, num2) + this.X[15] + 1518500249, 7) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F2(num4, num5, num) + this.X[3] + 1518500249, 15) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F2(num3, num4, num5) + this.X[12] + 1518500249, 7) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F2(num2, num3, num4) + this.X[0] + 1518500249, 12) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F2(num, num2, num3) + this.X[9] + 1518500249, 15) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F2(num5, num, num2) + this.X[5] + 1518500249, 9) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F2(num4, num5, num) + this.X[2] + 1518500249, 11) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F2(num3, num4, num5) + this.X[14] + 1518500249, 7) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F2(num2, num3, num4) + this.X[11] + 1518500249, 13) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F2(num, num2, num3) + this.X[8] + 1518500249, 12) + num4;
			num2 = this.RL(num2, 10);
			num10 = this.RL(num10 + this.F4(num6, num7, num8) + this.X[6] + 1548603684, 9) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F4(num10, num6, num7) + this.X[11] + 1548603684, 13) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F4(num9, num10, num6) + this.X[3] + 1548603684, 15) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F4(num8, num9, num10) + this.X[7] + 1548603684, 7) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F4(num7, num8, num9) + this.X[0] + 1548603684, 12) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F4(num6, num7, num8) + this.X[13] + 1548603684, 8) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F4(num10, num6, num7) + this.X[5] + 1548603684, 9) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F4(num9, num10, num6) + this.X[10] + 1548603684, 11) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F4(num8, num9, num10) + this.X[14] + 1548603684, 7) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F4(num7, num8, num9) + this.X[15] + 1548603684, 7) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F4(num6, num7, num8) + this.X[8] + 1548603684, 12) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F4(num10, num6, num7) + this.X[12] + 1548603684, 7) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F4(num9, num10, num6) + this.X[4] + 1548603684, 6) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F4(num8, num9, num10) + this.X[9] + 1548603684, 15) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F4(num7, num8, num9) + this.X[1] + 1548603684, 13) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F4(num6, num7, num8) + this.X[2] + 1548603684, 11) + num9;
			num7 = this.RL(num7, 10);
			int num12 = num2;
			num2 = num7;
			num7 = num12;
			num4 = this.RL(num4 + this.F3(num5, num, num2) + this.X[3] + 1859775393, 11) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F3(num4, num5, num) + this.X[10] + 1859775393, 13) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F3(num3, num4, num5) + this.X[14] + 1859775393, 6) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F3(num2, num3, num4) + this.X[4] + 1859775393, 7) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F3(num, num2, num3) + this.X[9] + 1859775393, 14) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F3(num5, num, num2) + this.X[15] + 1859775393, 9) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F3(num4, num5, num) + this.X[8] + 1859775393, 13) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F3(num3, num4, num5) + this.X[1] + 1859775393, 15) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F3(num2, num3, num4) + this.X[2] + 1859775393, 14) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F3(num, num2, num3) + this.X[7] + 1859775393, 8) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F3(num5, num, num2) + this.X[0] + 1859775393, 13) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F3(num4, num5, num) + this.X[6] + 1859775393, 6) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F3(num3, num4, num5) + this.X[13] + 1859775393, 5) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F3(num2, num3, num4) + this.X[11] + 1859775393, 12) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F3(num, num2, num3) + this.X[5] + 1859775393, 7) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F3(num5, num, num2) + this.X[12] + 1859775393, 5) + num3;
			num = this.RL(num, 10);
			num9 = this.RL(num9 + this.F3(num10, num6, num7) + this.X[15] + 1836072691, 9) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F3(num9, num10, num6) + this.X[5] + 1836072691, 7) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F3(num8, num9, num10) + this.X[1] + 1836072691, 15) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F3(num7, num8, num9) + this.X[3] + 1836072691, 11) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F3(num6, num7, num8) + this.X[7] + 1836072691, 8) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F3(num10, num6, num7) + this.X[14] + 1836072691, 6) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F3(num9, num10, num6) + this.X[6] + 1836072691, 6) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F3(num8, num9, num10) + this.X[9] + 1836072691, 14) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F3(num7, num8, num9) + this.X[11] + 1836072691, 12) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F3(num6, num7, num8) + this.X[8] + 1836072691, 13) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F3(num10, num6, num7) + this.X[12] + 1836072691, 5) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F3(num9, num10, num6) + this.X[2] + 1836072691, 14) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F3(num8, num9, num10) + this.X[10] + 1836072691, 13) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F3(num7, num8, num9) + this.X[0] + 1836072691, 13) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F3(num6, num7, num8) + this.X[4] + 1836072691, 7) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F3(num10, num6, num7) + this.X[13] + 1836072691, 5) + num8;
			num6 = this.RL(num6, 10);
			int num13 = num3;
			num3 = num8;
			num8 = num13;
			num3 = this.RL(num3 + this.F4(num4, num5, num) + this.X[1] + -1894007588, 11) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F4(num3, num4, num5) + this.X[9] + -1894007588, 12) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F4(num2, num3, num4) + this.X[11] + -1894007588, 14) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F4(num, num2, num3) + this.X[10] + -1894007588, 15) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F4(num5, num, num2) + this.X[0] + -1894007588, 14) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F4(num4, num5, num) + this.X[8] + -1894007588, 15) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F4(num3, num4, num5) + this.X[12] + -1894007588, 9) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F4(num2, num3, num4) + this.X[4] + -1894007588, 8) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F4(num, num2, num3) + this.X[13] + -1894007588, 9) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F4(num5, num, num2) + this.X[3] + -1894007588, 14) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F4(num4, num5, num) + this.X[7] + -1894007588, 5) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F4(num3, num4, num5) + this.X[15] + -1894007588, 6) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F4(num2, num3, num4) + this.X[14] + -1894007588, 8) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F4(num, num2, num3) + this.X[5] + -1894007588, 6) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F4(num5, num, num2) + this.X[6] + -1894007588, 5) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F4(num4, num5, num) + this.X[2] + -1894007588, 12) + num2;
			num5 = this.RL(num5, 10);
			num8 = this.RL(num8 + this.F2(num9, num10, num6) + this.X[8] + 2053994217, 15) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F2(num8, num9, num10) + this.X[6] + 2053994217, 5) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F2(num7, num8, num9) + this.X[4] + 2053994217, 8) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F2(num6, num7, num8) + this.X[1] + 2053994217, 11) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F2(num10, num6, num7) + this.X[3] + 2053994217, 14) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F2(num9, num10, num6) + this.X[11] + 2053994217, 14) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F2(num8, num9, num10) + this.X[15] + 2053994217, 6) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F2(num7, num8, num9) + this.X[0] + 2053994217, 14) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F2(num6, num7, num8) + this.X[5] + 2053994217, 6) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F2(num10, num6, num7) + this.X[12] + 2053994217, 9) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F2(num9, num10, num6) + this.X[2] + 2053994217, 12) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F2(num8, num9, num10) + this.X[13] + 2053994217, 9) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F2(num7, num8, num9) + this.X[9] + 2053994217, 12) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F2(num6, num7, num8) + this.X[7] + 2053994217, 5) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F2(num10, num6, num7) + this.X[10] + 2053994217, 15) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F2(num9, num10, num6) + this.X[14] + 2053994217, 8) + num7;
			num10 = this.RL(num10, 10);
			int num14 = num4;
			num4 = num9;
			num9 = num14;
			num2 = this.RL(num2 + this.F5(num3, num4, num5) + this.X[4] + -1454113458, 9) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F5(num2, num3, num4) + this.X[0] + -1454113458, 15) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F5(num, num2, num3) + this.X[5] + -1454113458, 5) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F5(num5, num, num2) + this.X[9] + -1454113458, 11) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F5(num4, num5, num) + this.X[7] + -1454113458, 6) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F5(num3, num4, num5) + this.X[12] + -1454113458, 8) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F5(num2, num3, num4) + this.X[2] + -1454113458, 13) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F5(num, num2, num3) + this.X[10] + -1454113458, 12) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F5(num5, num, num2) + this.X[14] + -1454113458, 5) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F5(num4, num5, num) + this.X[1] + -1454113458, 12) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F5(num3, num4, num5) + this.X[3] + -1454113458, 13) + num;
			num4 = this.RL(num4, 10);
			num = this.RL(num + this.F5(num2, num3, num4) + this.X[8] + -1454113458, 14) + num5;
			num3 = this.RL(num3, 10);
			num5 = this.RL(num5 + this.F5(num, num2, num3) + this.X[11] + -1454113458, 11) + num4;
			num2 = this.RL(num2, 10);
			num4 = this.RL(num4 + this.F5(num5, num, num2) + this.X[6] + -1454113458, 8) + num3;
			num = this.RL(num, 10);
			num3 = this.RL(num3 + this.F5(num4, num5, num) + this.X[15] + -1454113458, 5) + num2;
			num5 = this.RL(num5, 10);
			num2 = this.RL(num2 + this.F5(num3, num4, num5) + this.X[13] + -1454113458, 6) + num;
			num4 = this.RL(num4, 10);
			num7 = this.RL(num7 + this.F1(num8, num9, num10) + this.X[12], 8) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F1(num7, num8, num9) + this.X[15], 5) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F1(num6, num7, num8) + this.X[10], 12) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F1(num10, num6, num7) + this.X[4], 9) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F1(num9, num10, num6) + this.X[1], 12) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F1(num8, num9, num10) + this.X[5], 5) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F1(num7, num8, num9) + this.X[8], 14) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F1(num6, num7, num8) + this.X[7], 6) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F1(num10, num6, num7) + this.X[6], 8) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F1(num9, num10, num6) + this.X[2], 13) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F1(num8, num9, num10) + this.X[13], 6) + num6;
			num9 = this.RL(num9, 10);
			num6 = this.RL(num6 + this.F1(num7, num8, num9) + this.X[14], 5) + num10;
			num8 = this.RL(num8, 10);
			num10 = this.RL(num10 + this.F1(num6, num7, num8) + this.X[0], 15) + num9;
			num7 = this.RL(num7, 10);
			num9 = this.RL(num9 + this.F1(num10, num6, num7) + this.X[3], 13) + num8;
			num6 = this.RL(num6, 10);
			num8 = this.RL(num8 + this.F1(num9, num10, num6) + this.X[9], 11) + num7;
			num10 = this.RL(num10, 10);
			num7 = this.RL(num7 + this.F1(num8, num9, num10) + this.X[11], 11) + num6;
			num9 = this.RL(num9, 10);
			this.H0 += num;
			this.H1 += num2;
			this.H2 += num3;
			this.H3 += num4;
			this.H4 += num10;
			this.H5 += num6;
			this.H6 += num7;
			this.H7 += num8;
			this.H8 += num9;
			this.H9 += num5;
			this.xOff = 0;
			for (int num15 = 0; num15 != this.X.Length; num15++)
			{
				this.X[num15] = 0;
			}
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000EBD3D File Offset: 0x000E9F3D
		public override IMemoable Copy()
		{
			return new RipeMD320Digest(this);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000EBD48 File Offset: 0x000E9F48
		public override void Reset(IMemoable other)
		{
			RipeMD320Digest t = (RipeMD320Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x04001D77 RID: 7543
		private const int DigestLength = 40;

		// Token: 0x04001D78 RID: 7544
		private int H0;

		// Token: 0x04001D79 RID: 7545
		private int H1;

		// Token: 0x04001D7A RID: 7546
		private int H2;

		// Token: 0x04001D7B RID: 7547
		private int H3;

		// Token: 0x04001D7C RID: 7548
		private int H4;

		// Token: 0x04001D7D RID: 7549
		private int H5;

		// Token: 0x04001D7E RID: 7550
		private int H6;

		// Token: 0x04001D7F RID: 7551
		private int H7;

		// Token: 0x04001D80 RID: 7552
		private int H8;

		// Token: 0x04001D81 RID: 7553
		private int H9;

		// Token: 0x04001D82 RID: 7554
		private int[] X = new int[16];

		// Token: 0x04001D83 RID: 7555
		private int xOff;
	}
}
