using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D9 RID: 985
	public interface TlsAgreementCredentials : TlsCredentials
	{
		// Token: 0x06002433 RID: 9267
		byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
	}
}
