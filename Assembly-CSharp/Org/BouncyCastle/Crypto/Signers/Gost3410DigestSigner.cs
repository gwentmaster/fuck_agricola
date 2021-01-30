using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200040B RID: 1035
	public class Gost3410DigestSigner : ISigner
	{
		// Token: 0x0600267C RID: 9852 RVA: 0x000C1CC3 File Offset: 0x000BFEC3
		public Gost3410DigestSigner(IDsa signer, IDigest digest)
		{
			this.dsaSigner = signer;
			this.digest = digest;
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600267D RID: 9853 RVA: 0x000C1CD9 File Offset: 0x000BFED9
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "with" + this.dsaSigner.AlgorithmName;
			}
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000C1CFC File Offset: 0x000BFEFC
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			AsymmetricKeyParameter asymmetricKeyParameter;
			if (parameters is ParametersWithRandom)
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters;
			}
			else
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
			}
			if (forSigning && !asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Signing Requires Private Key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification Requires Public Key.");
			}
			this.Reset();
			this.dsaSigner.Init(forSigning, parameters);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000C1D71 File Offset: 0x000BFF71
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000C1D7F File Offset: 0x000BFF7F
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.digest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000C1D90 File Offset: 0x000BFF90
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("GOST3410DigestSigner not initialised for signature generation.");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			byte[] result;
			try
			{
				BigInteger[] array2 = this.dsaSigner.GenerateSignature(array);
				byte[] array3 = new byte[64];
				byte[] array4 = array2[0].ToByteArrayUnsigned();
				byte[] array5 = array2[1].ToByteArrayUnsigned();
				array5.CopyTo(array3, 32 - array5.Length);
				array4.CopyTo(array3, 64 - array4.Length);
				result = array3;
			}
			catch (Exception ex)
			{
				throw new SignatureException(ex.Message, ex);
			}
			return result;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000C1E38 File Offset: 0x000C0038
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("DSADigestSigner not initialised for verification");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			BigInteger r;
			BigInteger s;
			try
			{
				r = new BigInteger(1, signature, 32, 32);
				s = new BigInteger(1, signature, 0, 32);
			}
			catch (Exception exception)
			{
				throw new SignatureException("error decoding signature bytes.", exception);
			}
			return this.dsaSigner.VerifySignature(array, r, s);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000C1EBC File Offset: 0x000C00BC
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x040019B9 RID: 6585
		private readonly IDigest digest;

		// Token: 0x040019BA RID: 6586
		private readonly IDsa dsaSigner;

		// Token: 0x040019BB RID: 6587
		private bool forSigning;
	}
}
