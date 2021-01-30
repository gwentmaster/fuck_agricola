using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004BE RID: 1214
	public class ShakeDigest : KeccakDigest, IXof, IDigest
	{
		// Token: 0x06002D34 RID: 11572 RVA: 0x000ED9B0 File Offset: 0x000EBBB0
		private static int CheckBitLength(int bitLength)
		{
			if (bitLength == 128 || bitLength == 256)
			{
				return bitLength;
			}
			throw new ArgumentException(bitLength + " not supported for SHAKE", "bitLength");
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000ED9DE File Offset: 0x000EBBDE
		public ShakeDigest() : this(128)
		{
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000ED9EB File Offset: 0x000EBBEB
		public ShakeDigest(int bitLength) : base(ShakeDigest.CheckBitLength(bitLength))
		{
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000EBDD2 File Offset: 0x000E9FD2
		public ShakeDigest(ShakeDigest source) : base(source)
		{
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x000ED9F9 File Offset: 0x000EBBF9
		public override string AlgorithmName
		{
			get
			{
				return "SHAKE" + this.fixedOutputLength;
			}
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000EDA10 File Offset: 0x000EBC10
		public override int DoFinal(byte[] output, int outOff)
		{
			return this.DoFinal(output, outOff, this.GetDigestSize());
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000EDA20 File Offset: 0x000EBC20
		public virtual int DoFinal(byte[] output, int outOff, int outLen)
		{
			this.DoOutput(output, outOff, outLen);
			this.Reset();
			return outLen;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000EDA33 File Offset: 0x000EBC33
		public virtual int DoOutput(byte[] output, int outOff, int outLen)
		{
			if (!this.squeezing)
			{
				this.Absorb(new byte[]
				{
					15
				}, 0, 4L);
			}
			this.Squeeze(output, outOff, (long)outLen * 8L);
			return outLen;
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000EDA5F File Offset: 0x000EBC5F
		protected override int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
		{
			return this.DoFinal(output, outOff, this.GetDigestSize(), partialByte, partialBits);
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000EDA74 File Offset: 0x000EBC74
		protected virtual int DoFinal(byte[] output, int outOff, int outLen, byte partialByte, int partialBits)
		{
			if (partialBits < 0 || partialBits > 7)
			{
				throw new ArgumentException("must be in the range [0,7]", "partialBits");
			}
			int num = ((int)partialByte & (1 << partialBits) - 1) | 15 << partialBits;
			int num2 = partialBits + 4;
			if (num2 >= 8)
			{
				this.oneByte[0] = (byte)num;
				this.Absorb(this.oneByte, 0, 8L);
				num2 -= 8;
				num >>= 8;
			}
			if (num2 > 0)
			{
				this.oneByte[0] = (byte)num;
				this.Absorb(this.oneByte, 0, (long)num2);
			}
			this.Squeeze(output, outOff, (long)outLen * 8L);
			this.Reset();
			return outLen;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000EDB0C File Offset: 0x000EBD0C
		public override IMemoable Copy()
		{
			return new ShakeDigest(this);
		}
	}
}
