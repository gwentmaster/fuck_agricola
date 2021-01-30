using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000398 RID: 920
	public abstract class AbstractTlsSigner : TlsSigner
	{
		// Token: 0x060022DC RID: 8924 RVA: 0x000B6156 File Offset: 0x000B4356
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000B615F File Offset: 0x000B435F
		public virtual byte[] GenerateRawSignature(AsymmetricKeyParameter privateKey, byte[] md5AndSha1)
		{
			return this.GenerateRawSignature(null, privateKey, md5AndSha1);
		}

		// Token: 0x060022DE RID: 8926
		public abstract byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash);

		// Token: 0x060022DF RID: 8927 RVA: 0x000B616A File Offset: 0x000B436A
		public virtual bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1)
		{
			return this.VerifyRawSignature(null, sigBytes, publicKey, md5AndSha1);
		}

		// Token: 0x060022E0 RID: 8928
		public abstract bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash);

		// Token: 0x060022E1 RID: 8929 RVA: 0x000B6176 File Offset: 0x000B4376
		public virtual ISigner CreateSigner(AsymmetricKeyParameter privateKey)
		{
			return this.CreateSigner(null, privateKey);
		}

		// Token: 0x060022E2 RID: 8930
		public abstract ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey);

		// Token: 0x060022E3 RID: 8931 RVA: 0x000B6180 File Offset: 0x000B4380
		public virtual ISigner CreateVerifyer(AsymmetricKeyParameter publicKey)
		{
			return this.CreateVerifyer(null, publicKey);
		}

		// Token: 0x060022E4 RID: 8932
		public abstract ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey);

		// Token: 0x060022E5 RID: 8933
		public abstract bool IsValidPublicKey(AsymmetricKeyParameter publicKey);

		// Token: 0x040016B7 RID: 5815
		protected TlsContext mContext;
	}
}
