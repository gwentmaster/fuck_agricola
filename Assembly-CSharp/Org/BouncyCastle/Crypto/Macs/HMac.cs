using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200046F RID: 1135
	public class HMac : IMac
	{
		// Token: 0x0600295D RID: 10589 RVA: 0x000CC790 File Offset: 0x000CA990
		public HMac(IDigest digest)
		{
			this.digest = digest;
			this.digestSize = digest.GetDigestSize();
			this.blockLength = digest.GetByteLength();
			this.inputPad = new byte[this.blockLength];
			this.outputBuf = new byte[this.blockLength + this.digestSize];
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600295E RID: 10590 RVA: 0x000CC7EB File Offset: 0x000CA9EB
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "/HMAC";
			}
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000CC802 File Offset: 0x000CAA02
		public virtual IDigest GetUnderlyingDigest()
		{
			return this.digest;
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000CC80C File Offset: 0x000CAA0C
		public virtual void Init(ICipherParameters parameters)
		{
			this.digest.Reset();
			byte[] key = ((KeyParameter)parameters).GetKey();
			int num = key.Length;
			if (num > this.blockLength)
			{
				this.digest.BlockUpdate(key, 0, num);
				this.digest.DoFinal(this.inputPad, 0);
				num = this.digestSize;
			}
			else
			{
				Array.Copy(key, 0, this.inputPad, 0, num);
			}
			Array.Clear(this.inputPad, num, this.blockLength - num);
			Array.Copy(this.inputPad, 0, this.outputBuf, 0, this.blockLength);
			HMac.XorPad(this.inputPad, this.blockLength, 54);
			HMac.XorPad(this.outputBuf, this.blockLength, 92);
			if (this.digest is IMemoable)
			{
				this.opadState = ((IMemoable)this.digest).Copy();
				((IDigest)this.opadState).BlockUpdate(this.outputBuf, 0, this.blockLength);
			}
			this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
			if (this.digest is IMemoable)
			{
				this.ipadState = ((IMemoable)this.digest).Copy();
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000CC946 File Offset: 0x000CAB46
		public virtual int GetMacSize()
		{
			return this.digestSize;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000CC94E File Offset: 0x000CAB4E
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000CC95C File Offset: 0x000CAB5C
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.digest.BlockUpdate(input, inOff, len);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000CC96C File Offset: 0x000CAB6C
		public virtual int DoFinal(byte[] output, int outOff)
		{
			this.digest.DoFinal(this.outputBuf, this.blockLength);
			if (this.opadState != null)
			{
				((IMemoable)this.digest).Reset(this.opadState);
				this.digest.BlockUpdate(this.outputBuf, this.blockLength, this.digest.GetDigestSize());
			}
			else
			{
				this.digest.BlockUpdate(this.outputBuf, 0, this.outputBuf.Length);
			}
			int result = this.digest.DoFinal(output, outOff);
			Array.Clear(this.outputBuf, this.blockLength, this.digestSize);
			if (this.ipadState != null)
			{
				((IMemoable)this.digest).Reset(this.ipadState);
				return result;
			}
			this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
			return result;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000CCA4A File Offset: 0x000CAC4A
		public virtual void Reset()
		{
			this.digest.Reset();
			this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000CCA74 File Offset: 0x000CAC74
		private static void XorPad(byte[] pad, int len, byte n)
		{
			for (int i = 0; i < len; i++)
			{
				int num = i;
				pad[num] ^= n;
			}
		}

		// Token: 0x04001B1F RID: 6943
		private const byte IPAD = 54;

		// Token: 0x04001B20 RID: 6944
		private const byte OPAD = 92;

		// Token: 0x04001B21 RID: 6945
		private readonly IDigest digest;

		// Token: 0x04001B22 RID: 6946
		private readonly int digestSize;

		// Token: 0x04001B23 RID: 6947
		private readonly int blockLength;

		// Token: 0x04001B24 RID: 6948
		private IMemoable ipadState;

		// Token: 0x04001B25 RID: 6949
		private IMemoable opadState;

		// Token: 0x04001B26 RID: 6950
		private readonly byte[] inputPad;

		// Token: 0x04001B27 RID: 6951
		private readonly byte[] outputBuf;
	}
}
