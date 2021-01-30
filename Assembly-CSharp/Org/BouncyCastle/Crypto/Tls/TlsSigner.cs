using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000401 RID: 1025
	public interface TlsSigner
	{
		// Token: 0x060025B4 RID: 9652
		void Init(TlsContext context);

		// Token: 0x060025B5 RID: 9653
		byte[] GenerateRawSignature(AsymmetricKeyParameter privateKey, byte[] md5AndSha1);

		// Token: 0x060025B6 RID: 9654
		byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash);

		// Token: 0x060025B7 RID: 9655
		bool VerifyRawSignature(byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] md5AndSha1);

		// Token: 0x060025B8 RID: 9656
		bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash);

		// Token: 0x060025B9 RID: 9657
		ISigner CreateSigner(AsymmetricKeyParameter privateKey);

		// Token: 0x060025BA RID: 9658
		ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey);

		// Token: 0x060025BB RID: 9659
		ISigner CreateVerifyer(AsymmetricKeyParameter publicKey);

		// Token: 0x060025BC RID: 9660
		ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey);

		// Token: 0x060025BD RID: 9661
		bool IsValidPublicKey(AsymmetricKeyParameter publicKey);
	}
}
