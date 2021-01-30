using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F4 RID: 1012
	public class TlsMac
	{
		// Token: 0x0600252F RID: 9519 RVA: 0x000BC5F8 File Offset: 0x000BA7F8
		public TlsMac(TlsContext context, IDigest digest, byte[] key, int keyOff, int keyLen)
		{
			this.context = context;
			KeyParameter keyParameter = new KeyParameter(key, keyOff, keyLen);
			this.secret = Arrays.Clone(keyParameter.GetKey());
			if (digest is LongDigest)
			{
				this.digestBlockSize = 128;
				this.digestOverhead = 16;
			}
			else
			{
				this.digestBlockSize = 64;
				this.digestOverhead = 8;
			}
			if (TlsUtilities.IsSsl(context))
			{
				this.mac = new Ssl3Mac(digest);
				if (digest.GetDigestSize() == 20)
				{
					this.digestOverhead = 4;
				}
			}
			else
			{
				this.mac = new HMac(digest);
			}
			this.mac.Init(keyParameter);
			this.macLength = this.mac.GetMacSize();
			if (context.SecurityParameters.truncatedHMac)
			{
				this.macLength = Math.Min(this.macLength, 10);
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06002530 RID: 9520 RVA: 0x000BC6CA File Offset: 0x000BA8CA
		public virtual byte[] MacSecret
		{
			get
			{
				return this.secret;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x000BC6D2 File Offset: 0x000BA8D2
		public virtual int Size
		{
			get
			{
				return this.macLength;
			}
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000BC6DC File Offset: 0x000BA8DC
		public virtual byte[] CalculateMac(long seqNo, byte type, byte[] message, int offset, int length)
		{
			ProtocolVersion serverVersion = this.context.ServerVersion;
			bool isSsl = serverVersion.IsSsl;
			byte[] array = new byte[isSsl ? 11 : 13];
			TlsUtilities.WriteUint64(seqNo, array, 0);
			TlsUtilities.WriteUint8(type, array, 8);
			if (!isSsl)
			{
				TlsUtilities.WriteVersion(serverVersion, array, 9);
			}
			TlsUtilities.WriteUint16(length, array, array.Length - 2);
			this.mac.BlockUpdate(array, 0, array.Length);
			this.mac.BlockUpdate(message, offset, length);
			return this.Truncate(MacUtilities.DoFinal(this.mac));
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000BC764 File Offset: 0x000BA964
		public virtual byte[] CalculateMacConstantTime(long seqNo, byte type, byte[] message, int offset, int length, int fullLength, byte[] dummyData)
		{
			byte[] result = this.CalculateMac(seqNo, type, message, offset, length);
			int num = TlsUtilities.IsSsl(this.context) ? 11 : 13;
			int num2 = this.GetDigestBlockCount(num + fullLength) - this.GetDigestBlockCount(num + length);
			while (--num2 >= 0)
			{
				this.mac.BlockUpdate(dummyData, 0, this.digestBlockSize);
			}
			this.mac.Update(dummyData[0]);
			this.mac.Reset();
			return result;
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000BC7E2 File Offset: 0x000BA9E2
		protected virtual int GetDigestBlockCount(int inputLength)
		{
			return (inputLength + this.digestOverhead) / this.digestBlockSize;
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000BC7F3 File Offset: 0x000BA9F3
		protected virtual byte[] Truncate(byte[] bs)
		{
			if (bs.Length <= this.macLength)
			{
				return bs;
			}
			return Arrays.CopyOf(bs, this.macLength);
		}

		// Token: 0x0400195B RID: 6491
		protected readonly TlsContext context;

		// Token: 0x0400195C RID: 6492
		protected readonly byte[] secret;

		// Token: 0x0400195D RID: 6493
		protected readonly IMac mac;

		// Token: 0x0400195E RID: 6494
		protected readonly int digestBlockSize;

		// Token: 0x0400195F RID: 6495
		protected readonly int digestOverhead;

		// Token: 0x04001960 RID: 6496
		protected readonly int macLength;
	}
}
