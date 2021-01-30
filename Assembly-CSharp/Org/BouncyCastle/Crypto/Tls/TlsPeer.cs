using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F7 RID: 1015
	public interface TlsPeer
	{
		// Token: 0x0600253E RID: 9534
		bool ShouldUseGmtUnixTime();

		// Token: 0x0600253F RID: 9535
		void NotifySecureRenegotiation(bool secureRenegotiation);

		// Token: 0x06002540 RID: 9536
		TlsCompression GetCompression();

		// Token: 0x06002541 RID: 9537
		TlsCipher GetCipher();

		// Token: 0x06002542 RID: 9538
		void NotifyAlertRaised(byte alertLevel, byte alertDescription, string message, Exception cause);

		// Token: 0x06002543 RID: 9539
		void NotifyAlertReceived(byte alertLevel, byte alertDescription);

		// Token: 0x06002544 RID: 9540
		void NotifyHandshakeComplete();
	}
}
