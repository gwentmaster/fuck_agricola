using System;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FC RID: 1020
	public interface TlsServer : TlsPeer
	{
		// Token: 0x06002597 RID: 9623
		void Init(TlsServerContext context);

		// Token: 0x06002598 RID: 9624
		void NotifyClientVersion(ProtocolVersion clientVersion);

		// Token: 0x06002599 RID: 9625
		void NotifyFallback(bool isFallback);

		// Token: 0x0600259A RID: 9626
		void NotifyOfferedCipherSuites(int[] offeredCipherSuites);

		// Token: 0x0600259B RID: 9627
		void NotifyOfferedCompressionMethods(byte[] offeredCompressionMethods);

		// Token: 0x0600259C RID: 9628
		void ProcessClientExtensions(IDictionary clientExtensions);

		// Token: 0x0600259D RID: 9629
		ProtocolVersion GetServerVersion();

		// Token: 0x0600259E RID: 9630
		int GetSelectedCipherSuite();

		// Token: 0x0600259F RID: 9631
		byte GetSelectedCompressionMethod();

		// Token: 0x060025A0 RID: 9632
		IDictionary GetServerExtensions();

		// Token: 0x060025A1 RID: 9633
		IList GetServerSupplementalData();

		// Token: 0x060025A2 RID: 9634
		TlsCredentials GetCredentials();

		// Token: 0x060025A3 RID: 9635
		CertificateStatus GetCertificateStatus();

		// Token: 0x060025A4 RID: 9636
		TlsKeyExchange GetKeyExchange();

		// Token: 0x060025A5 RID: 9637
		CertificateRequest GetCertificateRequest();

		// Token: 0x060025A6 RID: 9638
		void ProcessClientSupplementalData(IList clientSupplementalData);

		// Token: 0x060025A7 RID: 9639
		void NotifyClientCertificate(Certificate clientCertificate);

		// Token: 0x060025A8 RID: 9640
		NewSessionTicket GetNewSessionTicket();
	}
}
