using System;
using System.IO;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200045A RID: 1114
	public class CcmBlockCipher : IAeadBlockCipher
	{
		// Token: 0x06002860 RID: 10336 RVA: 0x000C7420 File Offset: 0x000C5620
		public CcmBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.macBlock = new byte[CcmBlockCipher.BlockSize];
			if (cipher.GetBlockSize() != CcmBlockCipher.BlockSize)
			{
				throw new ArgumentException("cipher required with a block size of " + CcmBlockCipher.BlockSize + ".");
			}
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000C748C File Offset: 0x000C568C
		public virtual IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000C7494 File Offset: 0x000C5694
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			ICipherParameters cipherParameters;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				this.nonce = aeadParameters.GetNonce();
				this.initialAssociatedText = aeadParameters.GetAssociatedText();
				this.macSize = aeadParameters.MacSize / 8;
				cipherParameters = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("invalid parameters passed to CCM");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				this.nonce = parametersWithIV.GetIV();
				this.initialAssociatedText = null;
				this.macSize = this.macBlock.Length / 2;
				cipherParameters = parametersWithIV.Parameters;
			}
			if (cipherParameters != null)
			{
				this.keyParam = cipherParameters;
			}
			if (this.nonce == null || this.nonce.Length < 7 || this.nonce.Length > 13)
			{
				throw new ArgumentException("nonce must have length from 7 to 13 octets");
			}
			this.Reset();
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06002863 RID: 10339 RVA: 0x000C7566 File Offset: 0x000C5766
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/CCM";
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000C757D File Offset: 0x000C577D
		public virtual int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000C758A File Offset: 0x000C578A
		public virtual void ProcessAadByte(byte input)
		{
			this.associatedText.WriteByte(input);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000C7598 File Offset: 0x000C5798
		public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
		{
			this.associatedText.Write(inBytes, inOff, len);
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000C75A8 File Offset: 0x000C57A8
		public virtual int ProcessByte(byte input, byte[] outBytes, int outOff)
		{
			this.data.WriteByte(input);
			return 0;
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000C75B7 File Offset: 0x000C57B7
		public virtual int ProcessBytes(byte[] inBytes, int inOff, int inLen, byte[] outBytes, int outOff)
		{
			Check.DataLength(inBytes, inOff, inLen, "Input buffer too short");
			this.data.Write(inBytes, inOff, inLen);
			return 0;
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000C75D8 File Offset: 0x000C57D8
		public virtual int DoFinal(byte[] outBytes, int outOff)
		{
			byte[] buffer = this.data.GetBuffer();
			int inLen = (int)this.data.Position;
			int result = this.ProcessPacket(buffer, 0, inLen, outBytes, outOff);
			this.Reset();
			return result;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000C760F File Offset: 0x000C580F
		public virtual void Reset()
		{
			this.cipher.Reset();
			this.associatedText.SetLength(0L);
			this.data.SetLength(0L);
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000C7636 File Offset: 0x000C5836
		public virtual byte[] GetMac()
		{
			return Arrays.CopyOfRange(this.macBlock, 0, this.macSize);
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int GetUpdateOutputSize(int len)
		{
			return 0;
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000C764C File Offset: 0x000C584C
		public virtual int GetOutputSize(int len)
		{
			int num = (int)this.data.Length + len;
			if (this.forEncryption)
			{
				return num + this.macSize;
			}
			if (num >= this.macSize)
			{
				return num - this.macSize;
			}
			return 0;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000C768C File Offset: 0x000C588C
		public virtual byte[] ProcessPacket(byte[] input, int inOff, int inLen)
		{
			byte[] array;
			if (this.forEncryption)
			{
				array = new byte[inLen + this.macSize];
			}
			else
			{
				if (inLen < this.macSize)
				{
					throw new InvalidCipherTextException("data too short");
				}
				array = new byte[inLen - this.macSize];
			}
			this.ProcessPacket(input, inOff, inLen, array, 0);
			return array;
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000C76E0 File Offset: 0x000C58E0
		public virtual int ProcessPacket(byte[] input, int inOff, int inLen, byte[] output, int outOff)
		{
			if (this.keyParam == null)
			{
				throw new InvalidOperationException("CCM cipher unitialized.");
			}
			int num = this.nonce.Length;
			int num2 = 15 - num;
			if (num2 < 4)
			{
				int num3 = 1 << 8 * num2;
				if (inLen >= num3)
				{
					throw new InvalidOperationException("CCM packet too large for choice of q.");
				}
			}
			byte[] array = new byte[CcmBlockCipher.BlockSize];
			array[0] = (byte)(num2 - 1 & 7);
			this.nonce.CopyTo(array, 1);
			IBlockCipher blockCipher = new SicBlockCipher(this.cipher);
			blockCipher.Init(this.forEncryption, new ParametersWithIV(this.keyParam, array));
			int i = inOff;
			int num4 = outOff;
			int num5;
			if (this.forEncryption)
			{
				num5 = inLen + this.macSize;
				Check.OutputLength(output, outOff, num5, "Output buffer too short.");
				this.CalculateMac(input, inOff, inLen, this.macBlock);
				byte[] array2 = new byte[CcmBlockCipher.BlockSize];
				blockCipher.ProcessBlock(this.macBlock, 0, array2, 0);
				while (i < inOff + inLen - CcmBlockCipher.BlockSize)
				{
					blockCipher.ProcessBlock(input, i, output, num4);
					num4 += CcmBlockCipher.BlockSize;
					i += CcmBlockCipher.BlockSize;
				}
				byte[] array3 = new byte[CcmBlockCipher.BlockSize];
				Array.Copy(input, i, array3, 0, inLen + inOff - i);
				blockCipher.ProcessBlock(array3, 0, array3, 0);
				Array.Copy(array3, 0, output, num4, inLen + inOff - i);
				Array.Copy(array2, 0, output, outOff + inLen, this.macSize);
			}
			else
			{
				if (inLen < this.macSize)
				{
					throw new InvalidCipherTextException("data too short");
				}
				num5 = inLen - this.macSize;
				Check.OutputLength(output, outOff, num5, "Output buffer too short.");
				Array.Copy(input, inOff + num5, this.macBlock, 0, this.macSize);
				blockCipher.ProcessBlock(this.macBlock, 0, this.macBlock, 0);
				for (int num6 = this.macSize; num6 != this.macBlock.Length; num6++)
				{
					this.macBlock[num6] = 0;
				}
				while (i < inOff + num5 - CcmBlockCipher.BlockSize)
				{
					blockCipher.ProcessBlock(input, i, output, num4);
					num4 += CcmBlockCipher.BlockSize;
					i += CcmBlockCipher.BlockSize;
				}
				byte[] array4 = new byte[CcmBlockCipher.BlockSize];
				Array.Copy(input, i, array4, 0, num5 - (i - inOff));
				blockCipher.ProcessBlock(array4, 0, array4, 0);
				Array.Copy(array4, 0, output, num4, num5 - (i - inOff));
				byte[] b = new byte[CcmBlockCipher.BlockSize];
				this.CalculateMac(output, outOff, num5, b);
				if (!Arrays.ConstantTimeAreEqual(this.macBlock, b))
				{
					throw new InvalidCipherTextException("mac check in CCM failed");
				}
			}
			return num5;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000C7978 File Offset: 0x000C5B78
		private int CalculateMac(byte[] data, int dataOff, int dataLen, byte[] macBlock)
		{
			IMac mac = new CbcBlockCipherMac(this.cipher, this.macSize * 8);
			mac.Init(this.keyParam);
			byte[] array = new byte[16];
			if (this.HasAssociatedText())
			{
				byte[] array2 = array;
				int num = 0;
				array2[num] |= 64;
			}
			byte[] array3 = array;
			int num2 = 0;
			array3[num2] |= (byte)(((mac.GetMacSize() - 2) / 2 & 7) << 3);
			byte[] array4 = array;
			int num3 = 0;
			array4[num3] |= (byte)(15 - this.nonce.Length - 1 & 7);
			Array.Copy(this.nonce, 0, array, 1, this.nonce.Length);
			int i = dataLen;
			int num4 = 1;
			while (i > 0)
			{
				array[array.Length - num4] = (byte)(i & 255);
				i >>= 8;
				num4++;
			}
			mac.BlockUpdate(array, 0, array.Length);
			if (this.HasAssociatedText())
			{
				int associatedTextLength = this.GetAssociatedTextLength();
				int num5;
				if (associatedTextLength < 65280)
				{
					mac.Update((byte)(associatedTextLength >> 8));
					mac.Update((byte)associatedTextLength);
					num5 = 2;
				}
				else
				{
					mac.Update(byte.MaxValue);
					mac.Update(254);
					mac.Update((byte)(associatedTextLength >> 24));
					mac.Update((byte)(associatedTextLength >> 16));
					mac.Update((byte)(associatedTextLength >> 8));
					mac.Update((byte)associatedTextLength);
					num5 = 6;
				}
				if (this.initialAssociatedText != null)
				{
					mac.BlockUpdate(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
				}
				if (this.associatedText.Position > 0L)
				{
					byte[] buffer = this.associatedText.GetBuffer();
					int len = (int)this.associatedText.Position;
					mac.BlockUpdate(buffer, 0, len);
				}
				num5 = (num5 + associatedTextLength) % 16;
				if (num5 != 0)
				{
					for (int j = num5; j < 16; j++)
					{
						mac.Update(0);
					}
				}
			}
			mac.BlockUpdate(data, dataOff, dataLen);
			return mac.DoFinal(macBlock, 0);
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000C7B41 File Offset: 0x000C5D41
		private int GetAssociatedTextLength()
		{
			return (int)this.associatedText.Length + ((this.initialAssociatedText == null) ? 0 : this.initialAssociatedText.Length);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000C7B63 File Offset: 0x000C5D63
		private bool HasAssociatedText()
		{
			return this.GetAssociatedTextLength() > 0;
		}

		// Token: 0x04001A90 RID: 6800
		private static readonly int BlockSize = 16;

		// Token: 0x04001A91 RID: 6801
		private readonly IBlockCipher cipher;

		// Token: 0x04001A92 RID: 6802
		private readonly byte[] macBlock;

		// Token: 0x04001A93 RID: 6803
		private bool forEncryption;

		// Token: 0x04001A94 RID: 6804
		private byte[] nonce;

		// Token: 0x04001A95 RID: 6805
		private byte[] initialAssociatedText;

		// Token: 0x04001A96 RID: 6806
		private int macSize;

		// Token: 0x04001A97 RID: 6807
		private ICipherParameters keyParam;

		// Token: 0x04001A98 RID: 6808
		private readonly MemoryStream associatedText = new MemoryStream();

		// Token: 0x04001A99 RID: 6809
		private readonly MemoryStream data = new MemoryStream();
	}
}
