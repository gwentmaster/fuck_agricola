using System;
using System.Collections;
using System.Collections.Generic;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003DE RID: 990
	public interface TlsClient : TlsPeer
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06002443 RID: 9283
		// (set) Token: 0x06002444 RID: 9284
		List<string> HostNames { get; set; }

		// Token: 0x06002445 RID: 9285
		void Init(TlsClientContext context);

		// Token: 0x06002446 RID: 9286
		TlsSession GetSessionToResume();

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06002447 RID: 9287
		ProtocolVersion ClientHelloRecordLayerVersion { get; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06002448 RID: 9288
		ProtocolVersion ClientVersion { get; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06002449 RID: 9289
		bool IsFallback { get; }

		// Token: 0x0600244A RID: 9290
		int[] GetCipherSuites();

		// Token: 0x0600244B RID: 9291
		byte[] GetCompressionMethods();

		// Token: 0x0600244C RID: 9292
		IDictionary GetClientExtensions();

		// Token: 0x0600244D RID: 9293
		void NotifyServerVersion(ProtocolVersion selectedVersion);

		// Token: 0x0600244E RID: 9294
		void NotifySessionID(byte[] sessionID);

		// Token: 0x0600244F RID: 9295
		void NotifySelectedCipherSuite(int selectedCipherSuite);

		// Token: 0x06002450 RID: 9296
		void NotifySelectedCompressionMethod(byte selectedCompressionMethod);

		// Token: 0x06002451 RID: 9297
		void ProcessServerExtensions(IDictionary serverExtensions);

		// Token: 0x06002452 RID: 9298
		void ProcessServerSupplementalData(IList serverSupplementalData);

		// Token: 0x06002453 RID: 9299
		TlsKeyExchange GetKeyExchange();

		// Token: 0x06002454 RID: 9300
		TlsAuthentication GetAuthentication();

		// Token: 0x06002455 RID: 9301
		IList GetClientSupplementalData();

		// Token: 0x06002456 RID: 9302
		void NotifyNewSessionTicket(NewSessionTicket newSessionTicket);
	}
}
