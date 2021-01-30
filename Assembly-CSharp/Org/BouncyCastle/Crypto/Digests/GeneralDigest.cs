using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004AC RID: 1196
	public abstract class GeneralDigest : IDigest, IMemoable
	{
		// Token: 0x06002BFC RID: 11260 RVA: 0x000E2761 File Offset: 0x000E0961
		internal GeneralDigest()
		{
			this.xBuf = new byte[4];
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000E2775 File Offset: 0x000E0975
		internal GeneralDigest(GeneralDigest t)
		{
			this.xBuf = new byte[t.xBuf.Length];
			this.CopyIn(t);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000E2797 File Offset: 0x000E0997
		protected void CopyIn(GeneralDigest t)
		{
			Array.Copy(t.xBuf, 0, this.xBuf, 0, t.xBuf.Length);
			this.xBufOff = t.xBufOff;
			this.byteCount = t.byteCount;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000E27CC File Offset: 0x000E09CC
		public void Update(byte input)
		{
			byte[] array = this.xBuf;
			int num = this.xBufOff;
			this.xBufOff = num + 1;
			array[num] = input;
			if (this.xBufOff == this.xBuf.Length)
			{
				this.ProcessWord(this.xBuf, 0);
				this.xBufOff = 0;
			}
			this.byteCount += 1L;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000E2828 File Offset: 0x000E0A28
		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			length = Math.Max(0, length);
			int i = 0;
			if (this.xBufOff != 0)
			{
				while (i < length)
				{
					byte[] array = this.xBuf;
					int num = this.xBufOff;
					this.xBufOff = num + 1;
					array[num] = input[inOff + i++];
					if (this.xBufOff == 4)
					{
						this.ProcessWord(this.xBuf, 0);
						this.xBufOff = 0;
						break;
					}
				}
			}
			int num2 = (length - i & -4) + i;
			while (i < num2)
			{
				this.ProcessWord(input, inOff + i);
				i += 4;
			}
			while (i < length)
			{
				byte[] array2 = this.xBuf;
				int num = this.xBufOff;
				this.xBufOff = num + 1;
				array2[num] = input[inOff + i++];
			}
			this.byteCount += (long)length;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000E28E4 File Offset: 0x000E0AE4
		public void Finish()
		{
			long bitLength = this.byteCount << 3;
			this.Update(128);
			while (this.xBufOff != 0)
			{
				this.Update(0);
			}
			this.ProcessLength(bitLength);
			this.ProcessBlock();
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000E2923 File Offset: 0x000E0B23
		public virtual void Reset()
		{
			this.byteCount = 0L;
			this.xBufOff = 0;
			Array.Clear(this.xBuf, 0, this.xBuf.Length);
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x000E2948 File Offset: 0x000E0B48
		public int GetByteLength()
		{
			return 64;
		}

		// Token: 0x06002C04 RID: 11268
		internal abstract void ProcessWord(byte[] input, int inOff);

		// Token: 0x06002C05 RID: 11269
		internal abstract void ProcessLength(long bitLength);

		// Token: 0x06002C06 RID: 11270
		internal abstract void ProcessBlock();

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06002C07 RID: 11271
		public abstract string AlgorithmName { get; }

		// Token: 0x06002C08 RID: 11272
		public abstract int GetDigestSize();

		// Token: 0x06002C09 RID: 11273
		public abstract int DoFinal(byte[] output, int outOff);

		// Token: 0x06002C0A RID: 11274
		public abstract IMemoable Copy();

		// Token: 0x06002C0B RID: 11275
		public abstract void Reset(IMemoable t);

		// Token: 0x04001D07 RID: 7431
		private const int BYTE_LENGTH = 64;

		// Token: 0x04001D08 RID: 7432
		private byte[] xBuf;

		// Token: 0x04001D09 RID: 7433
		private int xBufOff;

		// Token: 0x04001D0A RID: 7434
		private long byteCount;
	}
}
