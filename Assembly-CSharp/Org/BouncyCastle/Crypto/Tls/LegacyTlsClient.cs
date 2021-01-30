using System;
using System.Collections.Generic;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C4 RID: 964
	public sealed class LegacyTlsClient : DefaultTlsClient
	{
		// Token: 0x060023B1 RID: 9137 RVA: 0x000B7D4D File Offset: 0x000B5F4D
		public LegacyTlsClient(Uri targetUri, ICertificateVerifyer verifyer, IClientCredentialsProvider prov, List<string> hostNames)
		{
			this.TargetUri = targetUri;
			this.verifyer = verifyer;
			this.credProvider = prov;
			base.HostNames = hostNames;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000B7D72 File Offset: 0x000B5F72
		public override TlsAuthentication GetAuthentication()
		{
			return new LegacyTlsAuthentication(this.TargetUri, this.verifyer, this.credProvider);
		}

		// Token: 0x040018A1 RID: 6305
		private readonly Uri TargetUri;

		// Token: 0x040018A2 RID: 6306
		private readonly ICertificateVerifyer verifyer;

		// Token: 0x040018A3 RID: 6307
		private readonly IClientCredentialsProvider credProvider;
	}
}
