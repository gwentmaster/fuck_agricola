using System;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004BA RID: 1210
	public class Sha256Digest : GeneralDigest
	{
		// Token: 0x06002D07 RID: 11527 RVA: 0x000ECC80 File Offset: 0x000EAE80
		public Sha256Digest()
		{
			this.initHs();
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000ECC9B File Offset: 0x000EAE9B
		public Sha256Digest(Sha256Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000ECCB8 File Offset: 0x000EAEB8
		private void CopyIn(Sha256Digest t)
		{
			base.CopyIn(t);
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			this.H5 = t.H5;
			this.H6 = t.H6;
			this.H7 = t.H7;
			this.H8 = t.H8;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06002D0A RID: 11530 RVA: 0x000ECD53 File Offset: 0x000EAF53
		public override string AlgorithmName
		{
			get
			{
				return "SHA-256";
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000E1FAB File Offset: 0x000E01AB
		public override int GetDigestSize()
		{
			return 32;
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000ECD5C File Offset: 0x000EAF5C
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

		// Token: 0x06002D0D RID: 11533 RVA: 0x000ECD98 File Offset: 0x000EAF98
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.ProcessBlock();
			}
			this.X[14] = (uint)((ulong)bitLength >> 32);
			this.X[15] = (uint)bitLength;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000ECDC4 File Offset: 0x000EAFC4
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt32_To_BE(this.H1, output, outOff);
			Pack.UInt32_To_BE(this.H2, output, outOff + 4);
			Pack.UInt32_To_BE(this.H3, output, outOff + 8);
			Pack.UInt32_To_BE(this.H4, output, outOff + 12);
			Pack.UInt32_To_BE(this.H5, output, outOff + 16);
			Pack.UInt32_To_BE(this.H6, output, outOff + 20);
			Pack.UInt32_To_BE(this.H7, output, outOff + 24);
			Pack.UInt32_To_BE(this.H8, output, outOff + 28);
			this.Reset();
			return 32;
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000ECE5A File Offset: 0x000EB05A
		public override void Reset()
		{
			base.Reset();
			this.initHs();
			this.xOff = 0;
			Array.Clear(this.X, 0, this.X.Length);
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000ECE84 File Offset: 0x000EB084
		private void initHs()
		{
			this.H1 = 1779033703U;
			this.H2 = 3144134277U;
			this.H3 = 1013904242U;
			this.H4 = 2773480762U;
			this.H5 = 1359893119U;
			this.H6 = 2600822924U;
			this.H7 = 528734635U;
			this.H8 = 1541459225U;
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000ECEEC File Offset: 0x000EB0EC
		internal override void ProcessBlock()
		{
			for (int i = 16; i <= 63; i++)
			{
				this.X[i] = Sha256Digest.Theta1(this.X[i - 2]) + this.X[i - 7] + Sha256Digest.Theta0(this.X[i - 15]) + this.X[i - 16];
			}
			uint num = this.H1;
			uint num2 = this.H2;
			uint num3 = this.H3;
			uint num4 = this.H4;
			uint num5 = this.H5;
			uint num6 = this.H6;
			uint num7 = this.H7;
			uint num8 = this.H8;
			int num9 = 0;
			for (int j = 0; j < 8; j++)
			{
				num8 += Sha256Digest.Sum1Ch(num5, num6, num7) + Sha256Digest.K[num9] + this.X[num9];
				num4 += num8;
				num8 += Sha256Digest.Sum0Maj(num, num2, num3);
				num9++;
				num7 += Sha256Digest.Sum1Ch(num4, num5, num6) + Sha256Digest.K[num9] + this.X[num9];
				num3 += num7;
				num7 += Sha256Digest.Sum0Maj(num8, num, num2);
				num9++;
				num6 += Sha256Digest.Sum1Ch(num3, num4, num5) + Sha256Digest.K[num9] + this.X[num9];
				num2 += num6;
				num6 += Sha256Digest.Sum0Maj(num7, num8, num);
				num9++;
				num5 += Sha256Digest.Sum1Ch(num2, num3, num4) + Sha256Digest.K[num9] + this.X[num9];
				num += num5;
				num5 += Sha256Digest.Sum0Maj(num6, num7, num8);
				num9++;
				num4 += Sha256Digest.Sum1Ch(num, num2, num3) + Sha256Digest.K[num9] + this.X[num9];
				num8 += num4;
				num4 += Sha256Digest.Sum0Maj(num5, num6, num7);
				num9++;
				num3 += Sha256Digest.Sum1Ch(num8, num, num2) + Sha256Digest.K[num9] + this.X[num9];
				num7 += num3;
				num3 += Sha256Digest.Sum0Maj(num4, num5, num6);
				num9++;
				num2 += Sha256Digest.Sum1Ch(num7, num8, num) + Sha256Digest.K[num9] + this.X[num9];
				num6 += num2;
				num2 += Sha256Digest.Sum0Maj(num3, num4, num5);
				num9++;
				num += Sha256Digest.Sum1Ch(num6, num7, num8) + Sha256Digest.K[num9] + this.X[num9];
				num5 += num;
				num += Sha256Digest.Sum0Maj(num2, num3, num4);
				num9++;
			}
			this.H1 += num;
			this.H2 += num2;
			this.H3 += num3;
			this.H4 += num4;
			this.H5 += num5;
			this.H6 += num6;
			this.H7 += num7;
			this.H8 += num8;
			this.xOff = 0;
			Array.Clear(this.X, 0, 16);
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000ED1F9 File Offset: 0x000EB3F9
		private static uint Sum1Ch(uint x, uint y, uint z)
		{
			return ((x >> 6 | x << 26) ^ (x >> 11 | x << 21) ^ (x >> 25 | x << 7)) + ((x & y) ^ (~x & z));
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000ED21F File Offset: 0x000EB41F
		private static uint Sum0Maj(uint x, uint y, uint z)
		{
			return ((x >> 2 | x << 30) ^ (x >> 13 | x << 19) ^ (x >> 22 | x << 10)) + ((x & y) ^ (x & z) ^ (y & z));
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000ECC10 File Offset: 0x000EAE10
		private static uint Theta0(uint x)
		{
			return (x >> 7 | x << 25) ^ (x >> 18 | x << 14) ^ x >> 3;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000ECC28 File Offset: 0x000EAE28
		private static uint Theta1(uint x)
		{
			return (x >> 17 | x << 15) ^ (x >> 19 | x << 13) ^ x >> 10;
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000ED249 File Offset: 0x000EB449
		public override IMemoable Copy()
		{
			return new Sha256Digest(this);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000ED254 File Offset: 0x000EB454
		public override void Reset(IMemoable other)
		{
			Sha256Digest t = (Sha256Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x04001D9C RID: 7580
		private const int DigestLength = 32;

		// Token: 0x04001D9D RID: 7581
		private uint H1;

		// Token: 0x04001D9E RID: 7582
		private uint H2;

		// Token: 0x04001D9F RID: 7583
		private uint H3;

		// Token: 0x04001DA0 RID: 7584
		private uint H4;

		// Token: 0x04001DA1 RID: 7585
		private uint H5;

		// Token: 0x04001DA2 RID: 7586
		private uint H6;

		// Token: 0x04001DA3 RID: 7587
		private uint H7;

		// Token: 0x04001DA4 RID: 7588
		private uint H8;

		// Token: 0x04001DA5 RID: 7589
		private uint[] X = new uint[64];

		// Token: 0x04001DA6 RID: 7590
		private int xOff;

		// Token: 0x04001DA7 RID: 7591
		private static readonly uint[] K = new uint[]
		{
			1116352408U,
			1899447441U,
			3049323471U,
			3921009573U,
			961987163U,
			1508970993U,
			2453635748U,
			2870763221U,
			3624381080U,
			310598401U,
			607225278U,
			1426881987U,
			1925078388U,
			2162078206U,
			2614888103U,
			3248222580U,
			3835390401U,
			4022224774U,
			264347078U,
			604807628U,
			770255983U,
			1249150122U,
			1555081692U,
			1996064986U,
			2554220882U,
			2821834349U,
			2952996808U,
			3210313671U,
			3336571891U,
			3584528711U,
			113926993U,
			338241895U,
			666307205U,
			773529912U,
			1294757372U,
			1396182291U,
			1695183700U,
			1986661051U,
			2177026350U,
			2456956037U,
			2730485921U,
			2820302411U,
			3259730800U,
			3345764771U,
			3516065817U,
			3600352804U,
			4094571909U,
			275423344U,
			430227734U,
			506948616U,
			659060556U,
			883997877U,
			958139571U,
			1322822218U,
			1537002063U,
			1747873779U,
			1955562222U,
			2024104815U,
			2227730452U,
			2361852424U,
			2428436474U,
			2756734187U,
			3204031479U,
			3329325298U
		};
	}
}
