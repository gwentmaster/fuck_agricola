using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200048F RID: 1167
	public class IesEngine
	{
		// Token: 0x06002A8E RID: 10894 RVA: 0x000D7374 File Offset: 0x000D5574
		public IesEngine(IBasicAgreement agree, IDerivationFunction kdf, IMac mac)
		{
			this.agree = agree;
			this.kdf = kdf;
			this.mac = mac;
			this.macBuf = new byte[mac.GetMacSize()];
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000D73A2 File Offset: 0x000D55A2
		public IesEngine(IBasicAgreement agree, IDerivationFunction kdf, IMac mac, BufferedBlockCipher cipher)
		{
			this.agree = agree;
			this.kdf = kdf;
			this.mac = mac;
			this.macBuf = new byte[mac.GetMacSize()];
			this.cipher = cipher;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000D73D8 File Offset: 0x000D55D8
		public virtual void Init(bool forEncryption, ICipherParameters privParameters, ICipherParameters pubParameters, ICipherParameters iesParameters)
		{
			this.forEncryption = forEncryption;
			this.privParam = privParameters;
			this.pubParam = pubParameters;
			this.param = (IesParameters)iesParameters;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000D73FC File Offset: 0x000D55FC
		private byte[] DecryptBlock(byte[] in_enc, int inOff, int inLen, byte[] z)
		{
			KdfParameters kdfParameters = new KdfParameters(z, this.param.GetDerivationV());
			int macKeySize = this.param.MacKeySize;
			this.kdf.Init(kdfParameters);
			if (inLen < this.mac.GetMacSize())
			{
				throw new InvalidCipherTextException("Length of input must be greater than the MAC");
			}
			inLen -= this.mac.GetMacSize();
			byte[] array2;
			KeyParameter parameters;
			if (this.cipher == null)
			{
				byte[] array = this.GenerateKdfBytes(kdfParameters, inLen + macKeySize / 8);
				array2 = new byte[inLen];
				for (int num = 0; num != inLen; num++)
				{
					array2[num] = (in_enc[inOff + num] ^ array[num]);
				}
				parameters = new KeyParameter(array, inLen, macKeySize / 8);
			}
			else
			{
				int cipherKeySize = ((IesWithCipherParameters)this.param).CipherKeySize;
				byte[] key = this.GenerateKdfBytes(kdfParameters, cipherKeySize / 8 + macKeySize / 8);
				this.cipher.Init(false, new KeyParameter(key, 0, cipherKeySize / 8));
				array2 = this.cipher.DoFinal(in_enc, inOff, inLen);
				parameters = new KeyParameter(key, cipherKeySize / 8, macKeySize / 8);
			}
			byte[] encodingV = this.param.GetEncodingV();
			this.mac.Init(parameters);
			this.mac.BlockUpdate(in_enc, inOff, inLen);
			this.mac.BlockUpdate(encodingV, 0, encodingV.Length);
			this.mac.DoFinal(this.macBuf, 0);
			inOff += inLen;
			if (!Arrays.ConstantTimeAreEqual(Arrays.CopyOfRange(in_enc, inOff, inOff + this.macBuf.Length), this.macBuf))
			{
				throw new InvalidCipherTextException("Invalid MAC.");
			}
			return array2;
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000D7580 File Offset: 0x000D5780
		private byte[] EncryptBlock(byte[] input, int inOff, int inLen, byte[] z)
		{
			KdfParameters kParam = new KdfParameters(z, this.param.GetDerivationV());
			int macKeySize = this.param.MacKeySize;
			byte[] array2;
			int num;
			KeyParameter parameters;
			if (this.cipher == null)
			{
				byte[] array = this.GenerateKdfBytes(kParam, inLen + macKeySize / 8);
				array2 = new byte[inLen + this.mac.GetMacSize()];
				num = inLen;
				for (int num2 = 0; num2 != inLen; num2++)
				{
					array2[num2] = (input[inOff + num2] ^ array[num2]);
				}
				parameters = new KeyParameter(array, inLen, macKeySize / 8);
			}
			else
			{
				int cipherKeySize = ((IesWithCipherParameters)this.param).CipherKeySize;
				byte[] key = this.GenerateKdfBytes(kParam, cipherKeySize / 8 + macKeySize / 8);
				this.cipher.Init(true, new KeyParameter(key, 0, cipherKeySize / 8));
				num = this.cipher.GetOutputSize(inLen);
				byte[] array3 = new byte[num];
				int num3 = this.cipher.ProcessBytes(input, inOff, inLen, array3, 0);
				num3 += this.cipher.DoFinal(array3, num3);
				array2 = new byte[num3 + this.mac.GetMacSize()];
				num = num3;
				Array.Copy(array3, 0, array2, 0, num3);
				parameters = new KeyParameter(key, cipherKeySize / 8, macKeySize / 8);
			}
			byte[] encodingV = this.param.GetEncodingV();
			this.mac.Init(parameters);
			this.mac.BlockUpdate(array2, 0, num);
			this.mac.BlockUpdate(encodingV, 0, encodingV.Length);
			this.mac.DoFinal(array2, num);
			return array2;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000D7708 File Offset: 0x000D5908
		private byte[] GenerateKdfBytes(KdfParameters kParam, int length)
		{
			byte[] array = new byte[length];
			this.kdf.Init(kParam);
			this.kdf.GenerateBytes(array, 0, array.Length);
			return array;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000D773C File Offset: 0x000D593C
		public virtual byte[] ProcessBlock(byte[] input, int inOff, int inLen)
		{
			this.agree.Init(this.privParam);
			BigInteger n = this.agree.CalculateAgreement(this.pubParam);
			byte[] array = BigIntegers.AsUnsignedByteArray(this.agree.GetFieldSize(), n);
			byte[] result;
			try
			{
				result = (this.forEncryption ? this.EncryptBlock(input, inOff, inLen, array) : this.DecryptBlock(input, inOff, inLen, array));
			}
			finally
			{
				Array.Clear(array, 0, array.Length);
			}
			return result;
		}

		// Token: 0x04001C11 RID: 7185
		private readonly IBasicAgreement agree;

		// Token: 0x04001C12 RID: 7186
		private readonly IDerivationFunction kdf;

		// Token: 0x04001C13 RID: 7187
		private readonly IMac mac;

		// Token: 0x04001C14 RID: 7188
		private readonly BufferedBlockCipher cipher;

		// Token: 0x04001C15 RID: 7189
		private readonly byte[] macBuf;

		// Token: 0x04001C16 RID: 7190
		private bool forEncryption;

		// Token: 0x04001C17 RID: 7191
		private ICipherParameters privParam;

		// Token: 0x04001C18 RID: 7192
		private ICipherParameters pubParam;

		// Token: 0x04001C19 RID: 7193
		private IesParameters param;
	}
}
