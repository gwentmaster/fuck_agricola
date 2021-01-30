using System;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004BC RID: 1212
	public class Sha512Digest : LongDigest
	{
		// Token: 0x06002D21 RID: 11553 RVA: 0x000ED288 File Offset: 0x000EB488
		public Sha512Digest()
		{
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000ED290 File Offset: 0x000EB490
		public Sha512Digest(Sha512Digest t) : base(t)
		{
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06002D23 RID: 11555 RVA: 0x000ED3CB File Offset: 0x000EB5CB
		public override string AlgorithmName
		{
			get
			{
				return "SHA-512";
			}
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000E2948 File Offset: 0x000E0B48
		public override int GetDigestSize()
		{
			return 64;
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000ED3D4 File Offset: 0x000EB5D4
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt64_To_BE(this.H1, output, outOff);
			Pack.UInt64_To_BE(this.H2, output, outOff + 8);
			Pack.UInt64_To_BE(this.H3, output, outOff + 16);
			Pack.UInt64_To_BE(this.H4, output, outOff + 24);
			Pack.UInt64_To_BE(this.H5, output, outOff + 32);
			Pack.UInt64_To_BE(this.H6, output, outOff + 40);
			Pack.UInt64_To_BE(this.H7, output, outOff + 48);
			Pack.UInt64_To_BE(this.H8, output, outOff + 56);
			this.Reset();
			return 64;
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000ED46C File Offset: 0x000EB66C
		public override void Reset()
		{
			base.Reset();
			this.H1 = 7640891576956012808UL;
			this.H2 = 13503953896175478587UL;
			this.H3 = 4354685564936845355UL;
			this.H4 = 11912009170470909681UL;
			this.H5 = 5840696475078001361UL;
			this.H6 = 11170449401992604703UL;
			this.H7 = 2270897969802886507UL;
			this.H8 = 6620516959819538809UL;
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000ED4F7 File Offset: 0x000EB6F7
		public override IMemoable Copy()
		{
			return new Sha512Digest(this);
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000ED500 File Offset: 0x000EB700
		public override void Reset(IMemoable other)
		{
			Sha512Digest t = (Sha512Digest)other;
			base.CopyIn(t);
		}

		// Token: 0x04001DA9 RID: 7593
		private const int DigestLength = 64;
	}
}
