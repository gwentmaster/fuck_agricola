using System;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E3 RID: 995
	public interface TlsContext
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600246A RID: 9322
		IRandomGenerator NonceRandomGenerator { get; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600246B RID: 9323
		SecureRandom SecureRandom { get; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600246C RID: 9324
		SecurityParameters SecurityParameters { get; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600246D RID: 9325
		bool IsServer { get; }

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600246E RID: 9326
		ProtocolVersion ClientVersion { get; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600246F RID: 9327
		ProtocolVersion ServerVersion { get; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06002470 RID: 9328
		TlsSession ResumableSession { get; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06002471 RID: 9329
		// (set) Token: 0x06002472 RID: 9330
		object UserObject { get; set; }

		// Token: 0x06002473 RID: 9331
		byte[] ExportKeyingMaterial(string asciiLabel, byte[] context_value, int length);
	}
}
