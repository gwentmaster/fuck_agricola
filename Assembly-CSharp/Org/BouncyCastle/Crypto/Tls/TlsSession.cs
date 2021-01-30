using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FF RID: 1023
	public interface TlsSession
	{
		// Token: 0x060025AB RID: 9643
		SessionParameters ExportSessionParameters();

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060025AC RID: 9644
		byte[] SessionID { get; }

		// Token: 0x060025AD RID: 9645
		void Invalidate();

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060025AE RID: 9646
		bool IsResumable { get; }
	}
}
