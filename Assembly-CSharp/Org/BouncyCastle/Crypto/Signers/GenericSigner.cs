using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200040D RID: 1037
	public class GenericSigner : ISigner
	{
		// Token: 0x06002689 RID: 9865 RVA: 0x000C2147 File Offset: 0x000C0347
		public GenericSigner(IAsymmetricBlockCipher engine, IDigest digest)
		{
			this.engine = engine;
			this.digest = digest;
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x0600268A RID: 9866 RVA: 0x000C2160 File Offset: 0x000C0360
		public virtual string AlgorithmName
		{
			get
			{
				return string.Concat(new string[]
				{
					"Generic(",
					this.engine.AlgorithmName,
					"/",
					this.digest.AlgorithmName,
					")"
				});
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000C21AC File Offset: 0x000C03AC
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
				throw new InvalidKeyException("Signing requires private key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification requires public key.");
			}
			this.Reset();
			this.engine.Init(forSigning, parameters);
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000C2221 File Offset: 0x000C0421
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000C222F File Offset: 0x000C042F
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.digest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000C2240 File Offset: 0x000C0440
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("GenericSigner not initialised for signature generation.");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000C2290 File Offset: 0x000C0490
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("GenericSigner not initialised for verification");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			bool result;
			try
			{
				byte[] array2 = this.engine.ProcessBlock(signature, 0, signature.Length);
				if (array2.Length < array.Length)
				{
					byte[] array3 = new byte[array.Length];
					Array.Copy(array2, 0, array3, array3.Length - array2.Length, array2.Length);
					array2 = array3;
				}
				result = Arrays.ConstantTimeAreEqual(array2, array);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000C2328 File Offset: 0x000C0528
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x040019BE RID: 6590
		private readonly IAsymmetricBlockCipher engine;

		// Token: 0x040019BF RID: 6591
		private readonly IDigest digest;

		// Token: 0x040019C0 RID: 6592
		private bool forSigning;
	}
}
