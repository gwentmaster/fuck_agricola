using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200038F RID: 911
	public abstract class AbstractTlsAgreementCredentials : AbstractTlsCredentials, TlsAgreementCredentials, TlsCredentials
	{
		// Token: 0x06002272 RID: 8818
		public abstract byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
	}
}
