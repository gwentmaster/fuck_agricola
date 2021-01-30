using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C3 RID: 963
	public class LegacyTlsAuthentication : TlsAuthentication
	{
		// Token: 0x060023AE RID: 9134 RVA: 0x000B7CF4 File Offset: 0x000B5EF4
		public LegacyTlsAuthentication(Uri targetUri, ICertificateVerifyer verifyer, IClientCredentialsProvider prov)
		{
			this.TargetUri = targetUri;
			this.verifyer = verifyer;
			this.credProvider = prov;
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x000B7D11 File Offset: 0x000B5F11
		public virtual void NotifyServerCertificate(Certificate serverCertificate)
		{
			if (!this.verifyer.IsValid(this.TargetUri, serverCertificate.GetCertificateList()))
			{
				throw new TlsFatalAlert(90);
			}
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000B7D34 File Offset: 0x000B5F34
		public virtual TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest)
		{
			if (this.credProvider != null)
			{
				return this.credProvider.GetClientCredentials(context, certificateRequest);
			}
			return null;
		}

		// Token: 0x0400189E RID: 6302
		protected ICertificateVerifyer verifyer;

		// Token: 0x0400189F RID: 6303
		protected IClientCredentialsProvider credProvider;

		// Token: 0x040018A0 RID: 6304
		protected Uri TargetUri;
	}
}
