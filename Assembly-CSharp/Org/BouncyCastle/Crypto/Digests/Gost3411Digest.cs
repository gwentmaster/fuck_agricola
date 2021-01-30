using System;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004AB RID: 1195
	public class Gost3411Digest : IDigest, IMemoable
	{
		// Token: 0x06002BE5 RID: 11237 RVA: 0x000E1CA4 File Offset: 0x000DFEA4
		private static byte[][] MakeC()
		{
			byte[][] array = new byte[4][];
			for (int i = 0; i < 4; i++)
			{
				array[i] = new byte[32];
			}
			return array;
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000E1CD0 File Offset: 0x000DFED0
		public Gost3411Digest()
		{
			this.sBox = Gost28147Engine.GetSBox("D-A");
			this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
			this.Reset();
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000E1DD0 File Offset: 0x000DFFD0
		public Gost3411Digest(byte[] sBoxParam)
		{
			this.sBox = Arrays.Clone(sBoxParam);
			this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
			this.Reset();
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000E1ECC File Offset: 0x000E00CC
		public Gost3411Digest(Gost3411Digest t)
		{
			this.Reset(t);
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x000E1FA4 File Offset: 0x000E01A4
		public string AlgorithmName
		{
			get
			{
				return "Gost3411";
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000E1FAB File Offset: 0x000E01AB
		public int GetDigestSize()
		{
			return 32;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000E1FB0 File Offset: 0x000E01B0
		public void Update(byte input)
		{
			byte[] array = this.xBuf;
			int num = this.xBufOff;
			this.xBufOff = num + 1;
			array[num] = input;
			if (this.xBufOff == this.xBuf.Length)
			{
				this.sumByteArray(this.xBuf);
				this.processBlock(this.xBuf, 0);
				this.xBufOff = 0;
			}
			this.byteCount += 1UL;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000E2018 File Offset: 0x000E0218
		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (this.xBufOff != 0)
			{
				if (length <= 0)
				{
					break;
				}
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
			while (length > this.xBuf.Length)
			{
				Array.Copy(input, inOff, this.xBuf, 0, this.xBuf.Length);
				this.sumByteArray(this.xBuf);
				this.processBlock(this.xBuf, 0);
				inOff += this.xBuf.Length;
				length -= this.xBuf.Length;
				this.byteCount += (ulong)this.xBuf.Length;
			}
			while (length > 0)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000E20CC File Offset: 0x000E02CC
		private byte[] P(byte[] input)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				this.K[num++] = input[i];
				this.K[num++] = input[8 + i];
				this.K[num++] = input[16 + i];
				this.K[num++] = input[24 + i];
			}
			return this.K;
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000E2134 File Offset: 0x000E0334
		private byte[] A(byte[] input)
		{
			for (int i = 0; i < 8; i++)
			{
				this.a[i] = (input[i] ^ input[i + 8]);
			}
			Array.Copy(input, 8, input, 0, 24);
			Array.Copy(this.a, 0, input, 24, 8);
			return input;
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000E217B File Offset: 0x000E037B
		private void E(byte[] key, byte[] s, int sOff, byte[] input, int inOff)
		{
			this.cipher.Init(true, new KeyParameter(key));
			this.cipher.ProcessBlock(input, inOff, s, sOff);
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000E21A4 File Offset: 0x000E03A4
		private void fw(byte[] input)
		{
			Gost3411Digest.cpyBytesToShort(input, this.wS);
			this.w_S[15] = (this.wS[0] ^ this.wS[1] ^ this.wS[2] ^ this.wS[3] ^ this.wS[12] ^ this.wS[15]);
			Array.Copy(this.wS, 1, this.w_S, 0, 15);
			Gost3411Digest.cpyShortToBytes(this.w_S, input);
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000E2220 File Offset: 0x000E0420
		private void processBlock(byte[] input, int inOff)
		{
			Array.Copy(input, inOff, this.M, 0, 32);
			this.H.CopyTo(this.U, 0);
			this.M.CopyTo(this.V, 0);
			for (int i = 0; i < 32; i++)
			{
				this.W[i] = (this.U[i] ^ this.V[i]);
			}
			this.E(this.P(this.W), this.S, 0, this.H, 0);
			for (int j = 1; j < 4; j++)
			{
				byte[] array = this.A(this.U);
				for (int k = 0; k < 32; k++)
				{
					this.U[k] = (array[k] ^ this.C[j][k]);
				}
				this.V = this.A(this.A(this.V));
				for (int l = 0; l < 32; l++)
				{
					this.W[l] = (this.U[l] ^ this.V[l]);
				}
				this.E(this.P(this.W), this.S, j * 8, this.H, j * 8);
			}
			for (int m = 0; m < 12; m++)
			{
				this.fw(this.S);
			}
			for (int n = 0; n < 32; n++)
			{
				this.S[n] = (this.S[n] ^ this.M[n]);
			}
			this.fw(this.S);
			for (int num = 0; num < 32; num++)
			{
				this.S[num] = (this.H[num] ^ this.S[num]);
			}
			for (int num2 = 0; num2 < 61; num2++)
			{
				this.fw(this.S);
			}
			Array.Copy(this.S, 0, this.H, 0, this.H.Length);
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000E2414 File Offset: 0x000E0614
		private void finish()
		{
			Pack.UInt64_To_LE(this.byteCount * 8UL, this.L);
			while (this.xBufOff != 0)
			{
				this.Update(0);
			}
			this.processBlock(this.L, 0);
			this.processBlock(this.Sum, 0);
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x000E2460 File Offset: 0x000E0660
		public int DoFinal(byte[] output, int outOff)
		{
			this.finish();
			this.H.CopyTo(output, outOff);
			this.Reset();
			return 32;
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000E2480 File Offset: 0x000E0680
		public void Reset()
		{
			this.byteCount = 0UL;
			this.xBufOff = 0;
			Array.Clear(this.H, 0, this.H.Length);
			Array.Clear(this.L, 0, this.L.Length);
			Array.Clear(this.M, 0, this.M.Length);
			Array.Clear(this.C[1], 0, this.C[1].Length);
			Array.Clear(this.C[3], 0, this.C[3].Length);
			Array.Clear(this.Sum, 0, this.Sum.Length);
			Array.Clear(this.xBuf, 0, this.xBuf.Length);
			Gost3411Digest.C2.CopyTo(this.C[2], 0);
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000E2544 File Offset: 0x000E0744
		private void sumByteArray(byte[] input)
		{
			int num = 0;
			for (int num2 = 0; num2 != this.Sum.Length; num2++)
			{
				int num3 = (int)((this.Sum[num2] & byte.MaxValue) + (input[num2] & byte.MaxValue)) + num;
				this.Sum[num2] = (byte)num3;
				num = num3 >> 8;
			}
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000E2590 File Offset: 0x000E0790
		private static void cpyBytesToShort(byte[] S, short[] wS)
		{
			for (int i = 0; i < S.Length / 2; i++)
			{
				wS[i] = (short)(((int)S[i * 2 + 1] << 8 & 65280) | (int)(S[i * 2] & byte.MaxValue));
			}
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000E25CC File Offset: 0x000E07CC
		private static void cpyShortToBytes(short[] wS, byte[] S)
		{
			for (int i = 0; i < S.Length / 2; i++)
			{
				S[i * 2 + 1] = (byte)(wS[i] >> 8);
				S[i * 2] = (byte)wS[i];
			}
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000E1FAB File Offset: 0x000E01AB
		public int GetByteLength()
		{
			return 32;
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000E25FF File Offset: 0x000E07FF
		public IMemoable Copy()
		{
			return new Gost3411Digest(this);
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000E2608 File Offset: 0x000E0808
		public void Reset(IMemoable other)
		{
			Gost3411Digest gost3411Digest = (Gost3411Digest)other;
			this.sBox = gost3411Digest.sBox;
			this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
			this.Reset();
			Array.Copy(gost3411Digest.H, 0, this.H, 0, gost3411Digest.H.Length);
			Array.Copy(gost3411Digest.L, 0, this.L, 0, gost3411Digest.L.Length);
			Array.Copy(gost3411Digest.M, 0, this.M, 0, gost3411Digest.M.Length);
			Array.Copy(gost3411Digest.Sum, 0, this.Sum, 0, gost3411Digest.Sum.Length);
			Array.Copy(gost3411Digest.C[1], 0, this.C[1], 0, gost3411Digest.C[1].Length);
			Array.Copy(gost3411Digest.C[2], 0, this.C[2], 0, gost3411Digest.C[2].Length);
			Array.Copy(gost3411Digest.C[3], 0, this.C[3], 0, gost3411Digest.C[3].Length);
			Array.Copy(gost3411Digest.xBuf, 0, this.xBuf, 0, gost3411Digest.xBuf.Length);
			this.xBufOff = gost3411Digest.xBufOff;
			this.byteCount = gost3411Digest.byteCount;
		}

		// Token: 0x04001CF3 RID: 7411
		private const int DIGEST_LENGTH = 32;

		// Token: 0x04001CF4 RID: 7412
		private byte[] H = new byte[32];

		// Token: 0x04001CF5 RID: 7413
		private byte[] L = new byte[32];

		// Token: 0x04001CF6 RID: 7414
		private byte[] M = new byte[32];

		// Token: 0x04001CF7 RID: 7415
		private byte[] Sum = new byte[32];

		// Token: 0x04001CF8 RID: 7416
		private byte[][] C = Gost3411Digest.MakeC();

		// Token: 0x04001CF9 RID: 7417
		private byte[] xBuf = new byte[32];

		// Token: 0x04001CFA RID: 7418
		private int xBufOff;

		// Token: 0x04001CFB RID: 7419
		private ulong byteCount;

		// Token: 0x04001CFC RID: 7420
		private readonly IBlockCipher cipher = new Gost28147Engine();

		// Token: 0x04001CFD RID: 7421
		private byte[] sBox;

		// Token: 0x04001CFE RID: 7422
		private byte[] K = new byte[32];

		// Token: 0x04001CFF RID: 7423
		private byte[] a = new byte[8];

		// Token: 0x04001D00 RID: 7424
		internal short[] wS = new short[16];

		// Token: 0x04001D01 RID: 7425
		internal short[] w_S = new short[16];

		// Token: 0x04001D02 RID: 7426
		internal byte[] S = new byte[32];

		// Token: 0x04001D03 RID: 7427
		internal byte[] U = new byte[32];

		// Token: 0x04001D04 RID: 7428
		internal byte[] V = new byte[32];

		// Token: 0x04001D05 RID: 7429
		internal byte[] W = new byte[32];

		// Token: 0x04001D06 RID: 7430
		private static readonly byte[] C2 = new byte[]
		{
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			byte.MaxValue
		};
	}
}
