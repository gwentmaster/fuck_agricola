using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F5 RID: 1013
	public class TlsNullCipher : TlsCipher
	{
		// Token: 0x06002536 RID: 9526 RVA: 0x000BC80E File Offset: 0x000BAA0E
		public TlsNullCipher(TlsContext context)
		{
			this.context = context;
			this.writeMac = null;
			this.readMac = null;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000BC82C File Offset: 0x000BAA2C
		public TlsNullCipher(TlsContext context, IDigest clientWriteDigest, IDigest serverWriteDigest)
		{
			if (clientWriteDigest == null != (serverWriteDigest == null))
			{
				throw new TlsFatalAlert(80);
			}
			this.context = context;
			TlsMac tlsMac = null;
			TlsMac tlsMac2 = null;
			if (clientWriteDigest != null)
			{
				int num = clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
				byte[] key = TlsUtilities.CalculateKeyBlock(context, num);
				int num2 = 0;
				tlsMac = new TlsMac(context, clientWriteDigest, key, num2, clientWriteDigest.GetDigestSize());
				num2 += clientWriteDigest.GetDigestSize();
				tlsMac2 = new TlsMac(context, serverWriteDigest, key, num2, serverWriteDigest.GetDigestSize());
				num2 += serverWriteDigest.GetDigestSize();
				if (num2 != num)
				{
					throw new TlsFatalAlert(80);
				}
			}
			if (context.IsServer)
			{
				this.writeMac = tlsMac2;
				this.readMac = tlsMac;
				return;
			}
			this.writeMac = tlsMac;
			this.readMac = tlsMac2;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000BC8E4 File Offset: 0x000BAAE4
		public virtual int GetPlaintextLimit(int ciphertextLimit)
		{
			int num = ciphertextLimit;
			if (this.writeMac != null)
			{
				num -= this.writeMac.Size;
			}
			return num;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000BC90C File Offset: 0x000BAB0C
		public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
		{
			if (this.writeMac == null)
			{
				return Arrays.CopyOfRange(plaintext, offset, offset + len);
			}
			byte[] array = this.writeMac.CalculateMac(seqNo, type, plaintext, offset, len);
			byte[] array2 = new byte[len + array.Length];
			Array.Copy(plaintext, offset, array2, 0, len);
			Array.Copy(array, 0, array2, len, array.Length);
			return array2;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000BC968 File Offset: 0x000BAB68
		public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
		{
			if (this.readMac == null)
			{
				return Arrays.CopyOfRange(ciphertext, offset, offset + len);
			}
			int size = this.readMac.Size;
			if (len < size)
			{
				throw new TlsFatalAlert(50);
			}
			int num = len - size;
			byte[] a = Arrays.CopyOfRange(ciphertext, offset + num, offset + len);
			byte[] b = this.readMac.CalculateMac(seqNo, type, ciphertext, offset, num);
			if (!Arrays.ConstantTimeAreEqual(a, b))
			{
				throw new TlsFatalAlert(20);
			}
			return Arrays.CopyOfRange(ciphertext, offset, offset + num);
		}

		// Token: 0x04001961 RID: 6497
		protected readonly TlsContext context;

		// Token: 0x04001962 RID: 6498
		protected readonly TlsMac writeMac;

		// Token: 0x04001963 RID: 6499
		protected readonly TlsMac readMac;
	}
}
