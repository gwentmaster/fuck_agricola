using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000396 RID: 918
	public abstract class AbstractTlsPeer : TlsPeer
	{
		// Token: 0x060022B6 RID: 8886 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool ShouldUseGmtUnixTime()
		{
			return false;
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000B5D74 File Offset: 0x000B3F74
		public virtual void NotifySecureRenegotiation(bool secureRenegotiation)
		{
			if (!secureRenegotiation)
			{
				throw new TlsFatalAlert(40);
			}
		}

		// Token: 0x060022B8 RID: 8888
		public abstract TlsCompression GetCompression();

		// Token: 0x060022B9 RID: 8889
		public abstract TlsCipher GetCipher();

		// Token: 0x060022BA RID: 8890 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void NotifyAlertRaised(byte alertLevel, byte alertDescription, string message, Exception cause)
		{
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void NotifyAlertReceived(byte alertLevel, byte alertDescription)
		{
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void NotifyHandshakeComplete()
		{
		}
	}
}
