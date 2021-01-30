using System;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004BB RID: 1211
	public class Sha384Digest : LongDigest
	{
		// Token: 0x06002D19 RID: 11545 RVA: 0x000ED288 File Offset: 0x000EB488
		public Sha384Digest()
		{
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000ED290 File Offset: 0x000EB490
		public Sha384Digest(Sha384Digest t) : base(t)
		{
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06002D1B RID: 11547 RVA: 0x000ED299 File Offset: 0x000EB499
		public override string AlgorithmName
		{
			get
			{
				return "SHA-384";
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000ED2A0 File Offset: 0x000EB4A0
		public override int GetDigestSize()
		{
			return 48;
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000ED2A4 File Offset: 0x000EB4A4
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt64_To_BE(this.H1, output, outOff);
			Pack.UInt64_To_BE(this.H2, output, outOff + 8);
			Pack.UInt64_To_BE(this.H3, output, outOff + 16);
			Pack.UInt64_To_BE(this.H4, output, outOff + 24);
			Pack.UInt64_To_BE(this.H5, output, outOff + 32);
			Pack.UInt64_To_BE(this.H6, output, outOff + 40);
			this.Reset();
			return 48;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000ED31C File Offset: 0x000EB51C
		public override void Reset()
		{
			base.Reset();
			this.H1 = 14680500436340154072UL;
			this.H2 = 7105036623409894663UL;
			this.H3 = 10473403895298186519UL;
			this.H4 = 1526699215303891257UL;
			this.H5 = 7436329637833083697UL;
			this.H6 = 10282925794625328401UL;
			this.H7 = 15784041429090275239UL;
			this.H8 = 5167115440072839076UL;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000ED3A7 File Offset: 0x000EB5A7
		public override IMemoable Copy()
		{
			return new Sha384Digest(this);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000ED3B0 File Offset: 0x000EB5B0
		public override void Reset(IMemoable other)
		{
			Sha384Digest t = (Sha384Digest)other;
			base.CopyIn(t);
		}

		// Token: 0x04001DA8 RID: 7592
		private const int DigestLength = 48;
	}
}
