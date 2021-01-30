using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000410 RID: 1040
	public class Iso9796d2Signer : ISignerWithRecovery, ISigner
	{
		// Token: 0x0600269B RID: 9883 RVA: 0x000C26D4 File Offset: 0x000C08D4
		public byte[] GetRecoveredMessage()
		{
			return this.recoveredMessage;
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000C26DC File Offset: 0x000C08DC
		public Iso9796d2Signer(IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit)
		{
			this.cipher = cipher;
			this.digest = digest;
			if (isImplicit)
			{
				this.trailer = 188;
				return;
			}
			if (IsoTrailers.NoTrailerAvailable(digest))
			{
				throw new ArgumentException("no valid trailer", "digest");
			}
			this.trailer = IsoTrailers.GetTrailer(digest);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000C2730 File Offset: 0x000C0930
		public Iso9796d2Signer(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, false)
		{
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x000C273B File Offset: 0x000C093B
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "withISO9796-2S1";
			}
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000C2754 File Offset: 0x000C0954
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)parameters;
			this.cipher.Init(forSigning, rsaKeyParameters);
			this.keyBits = rsaKeyParameters.Modulus.BitLength;
			this.block = new byte[(this.keyBits + 7) / 8];
			if (this.trailer == 188)
			{
				this.mBuf = new byte[this.block.Length - this.digest.GetDigestSize() - 2];
			}
			else
			{
				this.mBuf = new byte[this.block.Length - this.digest.GetDigestSize() - 3];
			}
			this.Reset();
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000C27F4 File Offset: 0x000C09F4
		private bool IsSameAs(byte[] a, byte[] b)
		{
			int num;
			if (this.messageLength > this.mBuf.Length)
			{
				if (this.mBuf.Length > b.Length)
				{
					return false;
				}
				num = this.mBuf.Length;
			}
			else
			{
				if (this.messageLength != b.Length)
				{
					return false;
				}
				num = b.Length;
			}
			bool result = true;
			for (int num2 = 0; num2 != num; num2++)
			{
				if (a[num2] != b[num2])
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x000C2855 File Offset: 0x000C0A55
		private void ClearBlock(byte[] block)
		{
			Array.Clear(block, 0, block.Length);
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x000C2864 File Offset: 0x000C0A64
		public virtual void UpdateWithRecoveredMessage(byte[] signature)
		{
			byte[] array = this.cipher.ProcessBlock(signature, 0, signature.Length);
			if (((array[0] & 192) ^ 64) != 0)
			{
				throw new InvalidCipherTextException("malformed signature");
			}
			if (((array[array.Length - 1] & 15) ^ 12) != 0)
			{
				throw new InvalidCipherTextException("malformed signature");
			}
			int num;
			if (((array[array.Length - 1] & 255) ^ 188) == 0)
			{
				num = 1;
			}
			else
			{
				int num2 = (int)(array[array.Length - 2] & byte.MaxValue) << 8 | (int)(array[array.Length - 1] & byte.MaxValue);
				if (IsoTrailers.NoTrailerAvailable(this.digest))
				{
					throw new ArgumentException("unrecognised hash in signature");
				}
				if (num2 != IsoTrailers.GetTrailer(this.digest))
				{
					throw new InvalidOperationException("signer initialised with wrong digest for trailer " + num2);
				}
				num = 2;
			}
			int num3 = 0;
			while (num3 != array.Length && ((array[num3] & 15) ^ 10) != 0)
			{
				num3++;
			}
			num3++;
			int num4 = array.Length - num - this.digest.GetDigestSize();
			if (num4 - num3 <= 0)
			{
				throw new InvalidCipherTextException("malformed block");
			}
			if ((array[0] & 32) == 0)
			{
				this.fullMessage = true;
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			else
			{
				this.fullMessage = false;
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			this.preSig = signature;
			this.preBlock = array;
			this.digest.BlockUpdate(this.recoveredMessage, 0, this.recoveredMessage.Length);
			this.messageLength = this.recoveredMessage.Length;
			this.recoveredMessage.CopyTo(this.mBuf, 0);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000C2A19 File Offset: 0x000C0C19
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
			if (this.messageLength < this.mBuf.Length)
			{
				this.mBuf[this.messageLength] = input;
			}
			this.messageLength++;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000C2A54 File Offset: 0x000C0C54
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (length > 0 && this.messageLength < this.mBuf.Length)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
			this.digest.BlockUpdate(input, inOff, length);
			this.messageLength += length;
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000C2AA8 File Offset: 0x000C0CA8
		public virtual void Reset()
		{
			this.digest.Reset();
			this.messageLength = 0;
			this.ClearBlock(this.mBuf);
			if (this.recoveredMessage != null)
			{
				this.ClearBlock(this.recoveredMessage);
			}
			this.recoveredMessage = null;
			this.fullMessage = false;
			if (this.preSig != null)
			{
				this.preSig = null;
				this.ClearBlock(this.preBlock);
				this.preBlock = null;
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000C2B18 File Offset: 0x000C0D18
		public virtual byte[] GenerateSignature()
		{
			int digestSize = this.digest.GetDigestSize();
			int num;
			int num2;
			if (this.trailer == 188)
			{
				num = 8;
				num2 = this.block.Length - digestSize - 1;
				this.digest.DoFinal(this.block, num2);
				this.block[this.block.Length - 1] = 188;
			}
			else
			{
				num = 16;
				num2 = this.block.Length - digestSize - 2;
				this.digest.DoFinal(this.block, num2);
				this.block[this.block.Length - 2] = (byte)((uint)this.trailer >> 8);
				this.block[this.block.Length - 1] = (byte)this.trailer;
			}
			int num3 = (digestSize + this.messageLength) * 8 + num + 4 - this.keyBits;
			byte b;
			if (num3 > 0)
			{
				int num4 = this.messageLength - (num3 + 7) / 8;
				b = 96;
				num2 -= num4;
				Array.Copy(this.mBuf, 0, this.block, num2, num4);
			}
			else
			{
				b = 64;
				num2 -= this.messageLength;
				Array.Copy(this.mBuf, 0, this.block, num2, this.messageLength);
			}
			if (num2 - 1 > 0)
			{
				for (int num5 = num2 - 1; num5 != 0; num5--)
				{
					this.block[num5] = 187;
				}
				byte[] array = this.block;
				int num6 = num2 - 1;
				array[num6] ^= 1;
				this.block[0] = 11;
				byte[] array2 = this.block;
				int num7 = 0;
				array2[num7] |= b;
			}
			else
			{
				this.block[0] = 10;
				byte[] array3 = this.block;
				int num8 = 0;
				array3[num8] |= b;
			}
			byte[] result = this.cipher.ProcessBlock(this.block, 0, this.block.Length);
			this.ClearBlock(this.mBuf);
			this.ClearBlock(this.block);
			return result;
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000C2CE8 File Offset: 0x000C0EE8
		public virtual bool VerifySignature(byte[] signature)
		{
			byte[] array;
			if (this.preSig == null)
			{
				try
				{
					array = this.cipher.ProcessBlock(signature, 0, signature.Length);
					goto IL_52;
				}
				catch (Exception)
				{
					return false;
				}
			}
			if (!Arrays.AreEqual(this.preSig, signature))
			{
				throw new InvalidOperationException("updateWithRecoveredMessage called on different signature");
			}
			array = this.preBlock;
			this.preSig = null;
			this.preBlock = null;
			IL_52:
			if (((array[0] & 192) ^ 64) != 0)
			{
				return this.ReturnFalse(array);
			}
			if (((array[array.Length - 1] & 15) ^ 12) != 0)
			{
				return this.ReturnFalse(array);
			}
			int num;
			if (((array[array.Length - 1] & 255) ^ 188) == 0)
			{
				num = 1;
			}
			else
			{
				int num2 = (int)(array[array.Length - 2] & byte.MaxValue) << 8 | (int)(array[array.Length - 1] & byte.MaxValue);
				if (IsoTrailers.NoTrailerAvailable(this.digest))
				{
					throw new ArgumentException("unrecognised hash in signature");
				}
				if (num2 != IsoTrailers.GetTrailer(this.digest))
				{
					throw new InvalidOperationException("signer initialised with wrong digest for trailer " + num2);
				}
				num = 2;
			}
			int num3 = 0;
			while (num3 != array.Length && ((array[num3] & 15) ^ 10) != 0)
			{
				num3++;
			}
			num3++;
			byte[] array2 = new byte[this.digest.GetDigestSize()];
			int num4 = array.Length - num - array2.Length;
			if (num4 - num3 <= 0)
			{
				return this.ReturnFalse(array);
			}
			if ((array[0] & 32) == 0)
			{
				this.fullMessage = true;
				if (this.messageLength > num4 - num3)
				{
					return this.ReturnFalse(array);
				}
				this.digest.Reset();
				this.digest.BlockUpdate(array, num3, num4 - num3);
				this.digest.DoFinal(array2, 0);
				bool flag = true;
				for (int num5 = 0; num5 != array2.Length; num5++)
				{
					byte[] array3 = array;
					int num6 = num4 + num5;
					array3[num6] ^= array2[num5];
					if (array[num4 + num5] != 0)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					return this.ReturnFalse(array);
				}
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			else
			{
				this.fullMessage = false;
				this.digest.DoFinal(array2, 0);
				bool flag2 = true;
				for (int num7 = 0; num7 != array2.Length; num7++)
				{
					byte[] array4 = array;
					int num8 = num4 + num7;
					array4[num8] ^= array2[num7];
					if (array[num4 + num7] != 0)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					return this.ReturnFalse(array);
				}
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			if (this.messageLength != 0 && !this.IsSameAs(this.mBuf, this.recoveredMessage))
			{
				return this.ReturnFalse(array);
			}
			this.ClearBlock(this.mBuf);
			this.ClearBlock(array);
			return true;
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000C2FB4 File Offset: 0x000C11B4
		private bool ReturnFalse(byte[] block)
		{
			this.ClearBlock(this.mBuf);
			this.ClearBlock(block);
			return false;
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000C2FCA File Offset: 0x000C11CA
		public virtual bool HasFullMessage()
		{
			return this.fullMessage;
		}

		// Token: 0x040019C5 RID: 6597
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerImplicit = 188;

		// Token: 0x040019C6 RID: 6598
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerRipeMD160 = 12748;

		// Token: 0x040019C7 RID: 6599
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerRipeMD128 = 13004;

		// Token: 0x040019C8 RID: 6600
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha1 = 13260;

		// Token: 0x040019C9 RID: 6601
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha256 = 13516;

		// Token: 0x040019CA RID: 6602
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha512 = 13772;

		// Token: 0x040019CB RID: 6603
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha384 = 14028;

		// Token: 0x040019CC RID: 6604
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerWhirlpool = 14284;

		// Token: 0x040019CD RID: 6605
		private IDigest digest;

		// Token: 0x040019CE RID: 6606
		private IAsymmetricBlockCipher cipher;

		// Token: 0x040019CF RID: 6607
		private int trailer;

		// Token: 0x040019D0 RID: 6608
		private int keyBits;

		// Token: 0x040019D1 RID: 6609
		private byte[] block;

		// Token: 0x040019D2 RID: 6610
		private byte[] mBuf;

		// Token: 0x040019D3 RID: 6611
		private int messageLength;

		// Token: 0x040019D4 RID: 6612
		private bool fullMessage;

		// Token: 0x040019D5 RID: 6613
		private byte[] recoveredMessage;

		// Token: 0x040019D6 RID: 6614
		private byte[] preSig;

		// Token: 0x040019D7 RID: 6615
		private byte[] preBlock;
	}
}
