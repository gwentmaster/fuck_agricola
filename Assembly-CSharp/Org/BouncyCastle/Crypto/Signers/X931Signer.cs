using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000415 RID: 1045
	public class X931Signer : ISigner
	{
		// Token: 0x060026D4 RID: 9940 RVA: 0x000C3C7C File Offset: 0x000C1E7C
		public X931Signer(IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit)
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

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000C3CD0 File Offset: 0x000C1ED0
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "with" + this.cipher.AlgorithmName + "/X9.31";
			}
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x000C3CF7 File Offset: 0x000C1EF7
		public X931Signer(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, false)
		{
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x000C3D04 File Offset: 0x000C1F04
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.kParam = (RsaKeyParameters)parameters;
			this.cipher.Init(forSigning, this.kParam);
			this.keyBits = this.kParam.Modulus.BitLength;
			this.block = new byte[(this.keyBits + 7) / 8];
			this.Reset();
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x000C2855 File Offset: 0x000C0A55
		private void ClearBlock(byte[] block)
		{
			Array.Clear(block, 0, block.Length);
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x000C3D60 File Offset: 0x000C1F60
		public virtual void Update(byte b)
		{
			this.digest.Update(b);
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000C3D6E File Offset: 0x000C1F6E
		public virtual void BlockUpdate(byte[] input, int off, int len)
		{
			this.digest.BlockUpdate(input, off, len);
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x000C3D7E File Offset: 0x000C1F7E
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x000C3D8C File Offset: 0x000C1F8C
		public virtual byte[] GenerateSignature()
		{
			this.CreateSignatureBlock();
			BigInteger bigInteger = new BigInteger(1, this.cipher.ProcessBlock(this.block, 0, this.block.Length));
			this.ClearBlock(this.block);
			bigInteger = bigInteger.Min(this.kParam.Modulus.Subtract(bigInteger));
			return BigIntegers.AsUnsignedByteArray((this.kParam.Modulus.BitLength + 7) / 8, bigInteger);
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000C3E00 File Offset: 0x000C2000
		private void CreateSignatureBlock()
		{
			int digestSize = this.digest.GetDigestSize();
			int num;
			if (this.trailer == 188)
			{
				num = this.block.Length - digestSize - 1;
				this.digest.DoFinal(this.block, num);
				this.block[this.block.Length - 1] = 188;
			}
			else
			{
				num = this.block.Length - digestSize - 2;
				this.digest.DoFinal(this.block, num);
				this.block[this.block.Length - 2] = (byte)(this.trailer >> 8);
				this.block[this.block.Length - 1] = (byte)this.trailer;
			}
			this.block[0] = 107;
			for (int num2 = num - 2; num2 != 0; num2--)
			{
				this.block[num2] = 187;
			}
			this.block[num - 1] = 186;
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000C3EE4 File Offset: 0x000C20E4
		public virtual bool VerifySignature(byte[] signature)
		{
			try
			{
				this.block = this.cipher.ProcessBlock(signature, 0, signature.Length);
			}
			catch (Exception)
			{
				return false;
			}
			BigInteger bigInteger = new BigInteger(1, this.block);
			BigInteger n;
			if ((bigInteger.IntValue & 15) == 12)
			{
				n = bigInteger;
			}
			else
			{
				bigInteger = this.kParam.Modulus.Subtract(bigInteger);
				if ((bigInteger.IntValue & 15) != 12)
				{
					return false;
				}
				n = bigInteger;
			}
			this.CreateSignatureBlock();
			byte[] b = BigIntegers.AsUnsignedByteArray(this.block.Length, n);
			bool result = Arrays.ConstantTimeAreEqual(this.block, b);
			this.ClearBlock(this.block);
			this.ClearBlock(b);
			return result;
		}

		// Token: 0x040019FA RID: 6650
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_IMPLICIT = 188;

		// Token: 0x040019FB RID: 6651
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_RIPEMD160 = 12748;

		// Token: 0x040019FC RID: 6652
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_RIPEMD128 = 13004;

		// Token: 0x040019FD RID: 6653
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA1 = 13260;

		// Token: 0x040019FE RID: 6654
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA256 = 13516;

		// Token: 0x040019FF RID: 6655
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA512 = 13772;

		// Token: 0x04001A00 RID: 6656
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA384 = 14028;

		// Token: 0x04001A01 RID: 6657
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_WHIRLPOOL = 14284;

		// Token: 0x04001A02 RID: 6658
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA224 = 14540;

		// Token: 0x04001A03 RID: 6659
		private IDigest digest;

		// Token: 0x04001A04 RID: 6660
		private IAsymmetricBlockCipher cipher;

		// Token: 0x04001A05 RID: 6661
		private RsaKeyParameters kParam;

		// Token: 0x04001A06 RID: 6662
		private int trailer;

		// Token: 0x04001A07 RID: 6663
		private int keyBits;

		// Token: 0x04001A08 RID: 6664
		private byte[] block;
	}
}
