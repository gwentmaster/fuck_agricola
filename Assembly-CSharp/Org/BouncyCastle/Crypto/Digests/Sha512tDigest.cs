using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004BD RID: 1213
	public class Sha512tDigest : LongDigest
	{
		// Token: 0x06002D29 RID: 11561 RVA: 0x000ED51C File Offset: 0x000EB71C
		public Sha512tDigest(int bitLength)
		{
			if (bitLength >= 512)
			{
				throw new ArgumentException("cannot be >= 512", "bitLength");
			}
			if (bitLength % 8 != 0)
			{
				throw new ArgumentException("needs to be a multiple of 8", "bitLength");
			}
			if (bitLength == 384)
			{
				throw new ArgumentException("cannot be 384 use SHA384 instead", "bitLength");
			}
			this.digestLength = bitLength / 8;
			this.tIvGenerate(this.digestLength * 8);
			this.Reset();
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000ED591 File Offset: 0x000EB791
		public Sha512tDigest(Sha512tDigest t) : base(t)
		{
			this.digestLength = t.digestLength;
			this.Reset(t);
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06002D2B RID: 11563 RVA: 0x000ED5AD File Offset: 0x000EB7AD
		public override string AlgorithmName
		{
			get
			{
				return "SHA-512/" + this.digestLength * 8;
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000ED5C6 File Offset: 0x000EB7C6
		public override int GetDigestSize()
		{
			return this.digestLength;
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000ED5D0 File Offset: 0x000EB7D0
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Sha512tDigest.UInt64_To_BE(this.H1, output, outOff, this.digestLength);
			Sha512tDigest.UInt64_To_BE(this.H2, output, outOff + 8, this.digestLength - 8);
			Sha512tDigest.UInt64_To_BE(this.H3, output, outOff + 16, this.digestLength - 16);
			Sha512tDigest.UInt64_To_BE(this.H4, output, outOff + 24, this.digestLength - 24);
			Sha512tDigest.UInt64_To_BE(this.H5, output, outOff + 32, this.digestLength - 32);
			Sha512tDigest.UInt64_To_BE(this.H6, output, outOff + 40, this.digestLength - 40);
			Sha512tDigest.UInt64_To_BE(this.H7, output, outOff + 48, this.digestLength - 48);
			Sha512tDigest.UInt64_To_BE(this.H8, output, outOff + 56, this.digestLength - 56);
			this.Reset();
			return this.digestLength;
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000ED6B0 File Offset: 0x000EB8B0
		public override void Reset()
		{
			base.Reset();
			this.H1 = this.H1t;
			this.H2 = this.H2t;
			this.H3 = this.H3t;
			this.H4 = this.H4t;
			this.H5 = this.H5t;
			this.H6 = this.H6t;
			this.H7 = this.H7t;
			this.H8 = this.H8t;
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000ED724 File Offset: 0x000EB924
		private void tIvGenerate(int bitLength)
		{
			this.H1 = 14964410163792538797UL;
			this.H2 = 2216346199247487646UL;
			this.H3 = 11082046791023156622UL;
			this.H4 = 65953792586715988UL;
			this.H5 = 17630457682085488500UL;
			this.H6 = 4512832404995164602UL;
			this.H7 = 13413544941332994254UL;
			this.H8 = 18322165818757711068UL;
			base.Update(83);
			base.Update(72);
			base.Update(65);
			base.Update(45);
			base.Update(53);
			base.Update(49);
			base.Update(50);
			base.Update(47);
			if (bitLength > 100)
			{
				base.Update((byte)(bitLength / 100 + 48));
				bitLength %= 100;
				base.Update((byte)(bitLength / 10 + 48));
				bitLength %= 10;
				base.Update((byte)(bitLength + 48));
			}
			else if (bitLength > 10)
			{
				base.Update((byte)(bitLength / 10 + 48));
				bitLength %= 10;
				base.Update((byte)(bitLength + 48));
			}
			else
			{
				base.Update((byte)(bitLength + 48));
			}
			base.Finish();
			this.H1t = this.H1;
			this.H2t = this.H2;
			this.H3t = this.H3;
			this.H4t = this.H4;
			this.H5t = this.H5;
			this.H6t = this.H6;
			this.H7t = this.H7;
			this.H8t = this.H8;
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000ED8BA File Offset: 0x000EBABA
		private static void UInt64_To_BE(ulong n, byte[] bs, int off, int max)
		{
			if (max > 0)
			{
				Sha512tDigest.UInt32_To_BE((uint)(n >> 32), bs, off, max);
				if (max > 4)
				{
					Sha512tDigest.UInt32_To_BE((uint)n, bs, off + 4, max - 4);
				}
			}
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000ED8E0 File Offset: 0x000EBAE0
		private static void UInt32_To_BE(uint n, byte[] bs, int off, int max)
		{
			int num = Math.Min(4, max);
			while (--num >= 0)
			{
				int num2 = 8 * (3 - num);
				bs[off + num] = (byte)(n >> num2);
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000ED911 File Offset: 0x000EBB11
		public override IMemoable Copy()
		{
			return new Sha512tDigest(this);
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000ED91C File Offset: 0x000EBB1C
		public override void Reset(IMemoable other)
		{
			Sha512tDigest sha512tDigest = (Sha512tDigest)other;
			if (this.digestLength != sha512tDigest.digestLength)
			{
				throw new MemoableResetException("digestLength inappropriate in other");
			}
			base.CopyIn(sha512tDigest);
			this.H1t = sha512tDigest.H1t;
			this.H2t = sha512tDigest.H2t;
			this.H3t = sha512tDigest.H3t;
			this.H4t = sha512tDigest.H4t;
			this.H5t = sha512tDigest.H5t;
			this.H6t = sha512tDigest.H6t;
			this.H7t = sha512tDigest.H7t;
			this.H8t = sha512tDigest.H8t;
		}

		// Token: 0x04001DAA RID: 7594
		private const ulong A5 = 11936128518282651045UL;

		// Token: 0x04001DAB RID: 7595
		private readonly int digestLength;

		// Token: 0x04001DAC RID: 7596
		private ulong H1t;

		// Token: 0x04001DAD RID: 7597
		private ulong H2t;

		// Token: 0x04001DAE RID: 7598
		private ulong H3t;

		// Token: 0x04001DAF RID: 7599
		private ulong H4t;

		// Token: 0x04001DB0 RID: 7600
		private ulong H5t;

		// Token: 0x04001DB1 RID: 7601
		private ulong H6t;

		// Token: 0x04001DB2 RID: 7602
		private ulong H7t;

		// Token: 0x04001DB3 RID: 7603
		private ulong H8t;
	}
}
