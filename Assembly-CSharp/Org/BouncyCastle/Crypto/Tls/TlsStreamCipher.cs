using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000404 RID: 1028
	public class TlsStreamCipher : TlsCipher
	{
		// Token: 0x060025CF RID: 9679 RVA: 0x000BE31C File Offset: 0x000BC51C
		public TlsStreamCipher(TlsContext context, IStreamCipher clientWriteCipher, IStreamCipher serverWriteCipher, IDigest clientWriteDigest, IDigest serverWriteDigest, int cipherKeySize, bool usesNonce)
		{
			bool isServer = context.IsServer;
			this.context = context;
			this.usesNonce = usesNonce;
			this.encryptCipher = clientWriteCipher;
			this.decryptCipher = serverWriteCipher;
			int num = 2 * cipherKeySize + clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
			byte[] key = TlsUtilities.CalculateKeyBlock(context, num);
			int num2 = 0;
			TlsMac tlsMac = new TlsMac(context, clientWriteDigest, key, num2, clientWriteDigest.GetDigestSize());
			num2 += clientWriteDigest.GetDigestSize();
			TlsMac tlsMac2 = new TlsMac(context, serverWriteDigest, key, num2, serverWriteDigest.GetDigestSize());
			num2 += serverWriteDigest.GetDigestSize();
			KeyParameter keyParameter = new KeyParameter(key, num2, cipherKeySize);
			num2 += cipherKeySize;
			KeyParameter keyParameter2 = new KeyParameter(key, num2, cipherKeySize);
			num2 += cipherKeySize;
			if (num2 != num)
			{
				throw new TlsFatalAlert(80);
			}
			ICipherParameters parameters;
			ICipherParameters parameters2;
			if (isServer)
			{
				this.writeMac = tlsMac2;
				this.readMac = tlsMac;
				this.encryptCipher = serverWriteCipher;
				this.decryptCipher = clientWriteCipher;
				parameters = keyParameter2;
				parameters2 = keyParameter;
			}
			else
			{
				this.writeMac = tlsMac;
				this.readMac = tlsMac2;
				this.encryptCipher = clientWriteCipher;
				this.decryptCipher = serverWriteCipher;
				parameters = keyParameter;
				parameters2 = keyParameter2;
			}
			if (usesNonce)
			{
				byte[] iv = new byte[8];
				parameters = new ParametersWithIV(parameters, iv);
				parameters2 = new ParametersWithIV(parameters2, iv);
			}
			this.encryptCipher.Init(true, parameters);
			this.decryptCipher.Init(false, parameters2);
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000BE462 File Offset: 0x000BC662
		public virtual int GetPlaintextLimit(int ciphertextLimit)
		{
			return ciphertextLimit - this.writeMac.Size;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000BE474 File Offset: 0x000BC674
		public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
		{
			if (this.usesNonce)
			{
				this.UpdateIV(this.encryptCipher, true, seqNo);
			}
			byte[] array = new byte[len + this.writeMac.Size];
			this.encryptCipher.ProcessBytes(plaintext, offset, len, array, 0);
			byte[] array2 = this.writeMac.CalculateMac(seqNo, type, plaintext, offset, len);
			this.encryptCipher.ProcessBytes(array2, 0, array2.Length, array, len);
			return array;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000BE4E4 File Offset: 0x000BC6E4
		public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
		{
			if (this.usesNonce)
			{
				this.UpdateIV(this.decryptCipher, false, seqNo);
			}
			int size = this.readMac.Size;
			if (len < size)
			{
				throw new TlsFatalAlert(50);
			}
			int num = len - size;
			byte[] array = new byte[len];
			this.decryptCipher.ProcessBytes(ciphertext, offset, len, array, 0);
			this.CheckMac(seqNo, type, array, num, len, array, 0, num);
			return Arrays.CopyOfRange(array, 0, num);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000BE558 File Offset: 0x000BC758
		protected virtual void CheckMac(long seqNo, byte type, byte[] recBuf, int recStart, int recEnd, byte[] calcBuf, int calcOff, int calcLen)
		{
			byte[] a = Arrays.CopyOfRange(recBuf, recStart, recEnd);
			byte[] b = this.readMac.CalculateMac(seqNo, type, calcBuf, calcOff, calcLen);
			if (!Arrays.ConstantTimeAreEqual(a, b))
			{
				throw new TlsFatalAlert(20);
			}
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000BE594 File Offset: 0x000BC794
		protected virtual void UpdateIV(IStreamCipher cipher, bool forEncryption, long seqNo)
		{
			byte[] array = new byte[8];
			TlsUtilities.WriteUint64(seqNo, array, 0);
			cipher.Init(forEncryption, new ParametersWithIV(null, array));
		}

		// Token: 0x0400199D RID: 6557
		protected readonly TlsContext context;

		// Token: 0x0400199E RID: 6558
		protected readonly IStreamCipher encryptCipher;

		// Token: 0x0400199F RID: 6559
		protected readonly IStreamCipher decryptCipher;

		// Token: 0x040019A0 RID: 6560
		protected readonly TlsMac writeMac;

		// Token: 0x040019A1 RID: 6561
		protected readonly TlsMac readMac;

		// Token: 0x040019A2 RID: 6562
		protected readonly bool usesNonce;
	}
}
