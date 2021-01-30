using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F3 RID: 1011
	public interface TlsKeyExchange
	{
		// Token: 0x06002520 RID: 9504
		void Init(TlsContext context);

		// Token: 0x06002521 RID: 9505
		void SkipServerCredentials();

		// Token: 0x06002522 RID: 9506
		void ProcessServerCredentials(TlsCredentials serverCredentials);

		// Token: 0x06002523 RID: 9507
		void ProcessServerCertificate(Certificate serverCertificate);

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06002524 RID: 9508
		bool RequiresServerKeyExchange { get; }

		// Token: 0x06002525 RID: 9509
		byte[] GenerateServerKeyExchange();

		// Token: 0x06002526 RID: 9510
		void SkipServerKeyExchange();

		// Token: 0x06002527 RID: 9511
		void ProcessServerKeyExchange(Stream input);

		// Token: 0x06002528 RID: 9512
		void ValidateCertificateRequest(CertificateRequest certificateRequest);

		// Token: 0x06002529 RID: 9513
		void SkipClientCredentials();

		// Token: 0x0600252A RID: 9514
		void ProcessClientCredentials(TlsCredentials clientCredentials);

		// Token: 0x0600252B RID: 9515
		void ProcessClientCertificate(Certificate clientCertificate);

		// Token: 0x0600252C RID: 9516
		void GenerateClientKeyExchange(Stream output);

		// Token: 0x0600252D RID: 9517
		void ProcessClientKeyExchange(Stream input);

		// Token: 0x0600252E RID: 9518
		byte[] GeneratePremasterSecret();
	}
}
