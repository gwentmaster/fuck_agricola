using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004B7 RID: 1207
	public class Sha3Digest : KeccakDigest
	{
		// Token: 0x06002CDD RID: 11485 RVA: 0x000EBD64 File Offset: 0x000E9F64
		private static int CheckBitLength(int bitLength)
		{
			if (bitLength <= 256)
			{
				if (bitLength != 224 && bitLength != 256)
				{
					goto IL_2C;
				}
			}
			else if (bitLength != 384 && bitLength != 512)
			{
				goto IL_2C;
			}
			return bitLength;
			IL_2C:
			throw new ArgumentException(bitLength + " not supported for SHA-3", "bitLength");
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000EBDB7 File Offset: 0x000E9FB7
		public Sha3Digest() : this(256)
		{
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000EBDC4 File Offset: 0x000E9FC4
		public Sha3Digest(int bitLength) : base(Sha3Digest.CheckBitLength(bitLength))
		{
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000EBDD2 File Offset: 0x000E9FD2
		public Sha3Digest(Sha3Digest source) : base(source)
		{
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06002CE1 RID: 11489 RVA: 0x000EBDDB File Offset: 0x000E9FDB
		public override string AlgorithmName
		{
			get
			{
				return "SHA3-" + this.fixedOutputLength;
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000EBDF2 File Offset: 0x000E9FF2
		public override int DoFinal(byte[] output, int outOff)
		{
			this.Absorb(new byte[]
			{
				2
			}, 0, 2L);
			return base.DoFinal(output, outOff);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000EBE10 File Offset: 0x000EA010
		protected override int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
		{
			if (partialBits < 0 || partialBits > 7)
			{
				throw new ArgumentException("must be in the range [0,7]", "partialBits");
			}
			int num = ((int)partialByte & (1 << partialBits) - 1) | 2 << partialBits;
			int num2 = partialBits + 2;
			if (num2 >= 8)
			{
				this.oneByte[0] = (byte)num;
				this.Absorb(this.oneByte, 0, 8L);
				num2 -= 8;
				num >>= 8;
			}
			return base.DoFinal(output, outOff, (byte)num, num2);
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000EBE80 File Offset: 0x000EA080
		public override IMemoable Copy()
		{
			return new Sha3Digest(this);
		}
	}
}
