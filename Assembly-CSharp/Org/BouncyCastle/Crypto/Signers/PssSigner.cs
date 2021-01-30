using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000412 RID: 1042
	public class PssSigner : ISigner
	{
		// Token: 0x060026AF RID: 9903 RVA: 0x000C30F5 File Offset: 0x000C12F5
		public static PssSigner CreateRawSigner(IAsymmetricBlockCipher cipher, IDigest digest)
		{
			return new PssSigner(cipher, new NullDigest(), digest, digest, digest.GetDigestSize(), null, 188);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000C3110 File Offset: 0x000C1310
		public static PssSigner CreateRawSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen, byte trailer)
		{
			return new PssSigner(cipher, new NullDigest(), contentDigest, mgfDigest, saltLen, null, trailer);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000C3123 File Offset: 0x000C1323
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, digest.GetDigestSize())
		{
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000C3133 File Offset: 0x000C1333
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLen) : this(cipher, digest, saltLen, 188)
		{
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000C3143 File Offset: 0x000C1343
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, byte[] salt) : this(cipher, digest, digest, digest, salt.Length, salt, 188)
		{
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000C3158 File Offset: 0x000C1358
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen) : this(cipher, contentDigest, mgfDigest, saltLen, 188)
		{
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000C316A File Offset: 0x000C136A
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, byte[] salt) : this(cipher, contentDigest, contentDigest, mgfDigest, salt.Length, salt, 188)
		{
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000C3181 File Offset: 0x000C1381
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLen, byte trailer) : this(cipher, digest, digest, saltLen, 188)
		{
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000C3192 File Offset: 0x000C1392
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen, byte trailer) : this(cipher, contentDigest, contentDigest, mgfDigest, saltLen, null, trailer)
		{
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000C31A4 File Offset: 0x000C13A4
		private PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest1, IDigest contentDigest2, IDigest mgfDigest, int saltLen, byte[] salt, byte trailer)
		{
			this.cipher = cipher;
			this.contentDigest1 = contentDigest1;
			this.contentDigest2 = contentDigest2;
			this.mgfDigest = mgfDigest;
			this.hLen = contentDigest2.GetDigestSize();
			this.mgfhLen = mgfDigest.GetDigestSize();
			this.sLen = saltLen;
			this.sSet = (salt != null);
			if (this.sSet)
			{
				this.salt = salt;
			}
			else
			{
				this.salt = new byte[saltLen];
			}
			this.mDash = new byte[8 + saltLen + this.hLen];
			this.trailer = trailer;
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060026B9 RID: 9913 RVA: 0x000C323D File Offset: 0x000C143D
		public virtual string AlgorithmName
		{
			get
			{
				return this.mgfDigest.AlgorithmName + "withRSAandMGF1";
			}
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000C3254 File Offset: 0x000C1454
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				parameters = parametersWithRandom.Parameters;
				this.random = parametersWithRandom.Random;
			}
			else if (forSigning)
			{
				this.random = new SecureRandom();
			}
			this.cipher.Init(forSigning, parameters);
			RsaKeyParameters rsaKeyParameters;
			if (parameters is RsaBlindingParameters)
			{
				rsaKeyParameters = ((RsaBlindingParameters)parameters).PublicKey;
			}
			else
			{
				rsaKeyParameters = (RsaKeyParameters)parameters;
			}
			this.emBits = rsaKeyParameters.Modulus.BitLength - 1;
			if (this.emBits < 8 * this.hLen + 8 * this.sLen + 9)
			{
				throw new ArgumentException("key too small for specified hash and salt lengths");
			}
			this.block = new byte[(this.emBits + 7) / 8];
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000C2855 File Offset: 0x000C0A55
		private void ClearBlock(byte[] block)
		{
			Array.Clear(block, 0, block.Length);
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000C330D File Offset: 0x000C150D
		public virtual void Update(byte input)
		{
			this.contentDigest1.Update(input);
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000C331B File Offset: 0x000C151B
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.contentDigest1.BlockUpdate(input, inOff, length);
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000C332B File Offset: 0x000C152B
		public virtual void Reset()
		{
			this.contentDigest1.Reset();
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000C3338 File Offset: 0x000C1538
		public virtual byte[] GenerateSignature()
		{
			this.contentDigest1.DoFinal(this.mDash, this.mDash.Length - this.hLen - this.sLen);
			if (this.sLen != 0)
			{
				if (!this.sSet)
				{
					this.random.NextBytes(this.salt);
				}
				this.salt.CopyTo(this.mDash, this.mDash.Length - this.sLen);
			}
			byte[] array = new byte[this.hLen];
			this.contentDigest2.BlockUpdate(this.mDash, 0, this.mDash.Length);
			this.contentDigest2.DoFinal(array, 0);
			this.block[this.block.Length - this.sLen - 1 - this.hLen - 1] = 1;
			this.salt.CopyTo(this.block, this.block.Length - this.sLen - this.hLen - 1);
			byte[] array2 = this.MaskGeneratorFunction1(array, 0, array.Length, this.block.Length - this.hLen - 1);
			for (int num = 0; num != array2.Length; num++)
			{
				byte[] array3 = this.block;
				int num2 = num;
				array3[num2] ^= array2[num];
			}
			byte[] array4 = this.block;
			int num3 = 0;
			array4[num3] &= (byte)(255 >> this.block.Length * 8 - this.emBits);
			array.CopyTo(this.block, this.block.Length - this.hLen - 1);
			this.block[this.block.Length - 1] = this.trailer;
			byte[] result = this.cipher.ProcessBlock(this.block, 0, this.block.Length);
			this.ClearBlock(this.block);
			return result;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000C34F4 File Offset: 0x000C16F4
		public virtual bool VerifySignature(byte[] signature)
		{
			this.contentDigest1.DoFinal(this.mDash, this.mDash.Length - this.hLen - this.sLen);
			byte[] array = this.cipher.ProcessBlock(signature, 0, signature.Length);
			array.CopyTo(this.block, this.block.Length - array.Length);
			if (this.block[this.block.Length - 1] != this.trailer)
			{
				this.ClearBlock(this.block);
				return false;
			}
			byte[] array2 = this.MaskGeneratorFunction1(this.block, this.block.Length - this.hLen - 1, this.hLen, this.block.Length - this.hLen - 1);
			for (int num = 0; num != array2.Length; num++)
			{
				byte[] array3 = this.block;
				int num2 = num;
				array3[num2] ^= array2[num];
			}
			byte[] array4 = this.block;
			int num3 = 0;
			array4[num3] &= (byte)(255 >> this.block.Length * 8 - this.emBits);
			for (int num4 = 0; num4 != this.block.Length - this.hLen - this.sLen - 2; num4++)
			{
				if (this.block[num4] != 0)
				{
					this.ClearBlock(this.block);
					return false;
				}
			}
			if (this.block[this.block.Length - this.hLen - this.sLen - 2] != 1)
			{
				this.ClearBlock(this.block);
				return false;
			}
			if (this.sSet)
			{
				Array.Copy(this.salt, 0, this.mDash, this.mDash.Length - this.sLen, this.sLen);
			}
			else
			{
				Array.Copy(this.block, this.block.Length - this.sLen - this.hLen - 1, this.mDash, this.mDash.Length - this.sLen, this.sLen);
			}
			this.contentDigest2.BlockUpdate(this.mDash, 0, this.mDash.Length);
			this.contentDigest2.DoFinal(this.mDash, this.mDash.Length - this.hLen);
			int num5 = this.block.Length - this.hLen - 1;
			for (int num6 = this.mDash.Length - this.hLen; num6 != this.mDash.Length; num6++)
			{
				if ((this.block[num5] ^ this.mDash[num6]) != 0)
				{
					this.ClearBlock(this.mDash);
					this.ClearBlock(this.block);
					return false;
				}
				num5++;
			}
			this.ClearBlock(this.mDash);
			this.ClearBlock(this.block);
			return true;
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000C3797 File Offset: 0x000C1997
		private void ItoOSP(int i, byte[] sp)
		{
			sp[0] = (byte)((uint)i >> 24);
			sp[1] = (byte)((uint)i >> 16);
			sp[2] = (byte)((uint)i >> 8);
			sp[3] = (byte)i;
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x000C37B8 File Offset: 0x000C19B8
		private byte[] MaskGeneratorFunction1(byte[] Z, int zOff, int zLen, int length)
		{
			byte[] array = new byte[length];
			byte[] array2 = new byte[this.mgfhLen];
			byte[] array3 = new byte[4];
			int i = 0;
			this.mgfDigest.Reset();
			while (i < length / this.mgfhLen)
			{
				this.ItoOSP(i, array3);
				this.mgfDigest.BlockUpdate(Z, zOff, zLen);
				this.mgfDigest.BlockUpdate(array3, 0, array3.Length);
				this.mgfDigest.DoFinal(array2, 0);
				array2.CopyTo(array, i * this.mgfhLen);
				i++;
			}
			if (i * this.mgfhLen < length)
			{
				this.ItoOSP(i, array3);
				this.mgfDigest.BlockUpdate(Z, zOff, zLen);
				this.mgfDigest.BlockUpdate(array3, 0, array3.Length);
				this.mgfDigest.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, i * this.mgfhLen, array.Length - i * this.mgfhLen);
			}
			return array;
		}

		// Token: 0x040019E4 RID: 6628
		public const byte TrailerImplicit = 188;

		// Token: 0x040019E5 RID: 6629
		private readonly IDigest contentDigest1;

		// Token: 0x040019E6 RID: 6630
		private readonly IDigest contentDigest2;

		// Token: 0x040019E7 RID: 6631
		private readonly IDigest mgfDigest;

		// Token: 0x040019E8 RID: 6632
		private readonly IAsymmetricBlockCipher cipher;

		// Token: 0x040019E9 RID: 6633
		private SecureRandom random;

		// Token: 0x040019EA RID: 6634
		private int hLen;

		// Token: 0x040019EB RID: 6635
		private int mgfhLen;

		// Token: 0x040019EC RID: 6636
		private int sLen;

		// Token: 0x040019ED RID: 6637
		private bool sSet;

		// Token: 0x040019EE RID: 6638
		private int emBits;

		// Token: 0x040019EF RID: 6639
		private byte[] salt;

		// Token: 0x040019F0 RID: 6640
		private byte[] mDash;

		// Token: 0x040019F1 RID: 6641
		private byte[] block;

		// Token: 0x040019F2 RID: 6642
		private byte trailer;
	}
}
