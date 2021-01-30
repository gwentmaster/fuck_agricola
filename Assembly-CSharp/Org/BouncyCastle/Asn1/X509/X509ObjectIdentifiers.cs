using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200053C RID: 1340
	public abstract class X509ObjectIdentifiers
	{
		// Token: 0x04001F69 RID: 8041
		internal const string ID = "2.5.4";

		// Token: 0x04001F6A RID: 8042
		public static readonly DerObjectIdentifier CommonName = new DerObjectIdentifier("2.5.4.3");

		// Token: 0x04001F6B RID: 8043
		public static readonly DerObjectIdentifier CountryName = new DerObjectIdentifier("2.5.4.6");

		// Token: 0x04001F6C RID: 8044
		public static readonly DerObjectIdentifier LocalityName = new DerObjectIdentifier("2.5.4.7");

		// Token: 0x04001F6D RID: 8045
		public static readonly DerObjectIdentifier StateOrProvinceName = new DerObjectIdentifier("2.5.4.8");

		// Token: 0x04001F6E RID: 8046
		public static readonly DerObjectIdentifier Organization = new DerObjectIdentifier("2.5.4.10");

		// Token: 0x04001F6F RID: 8047
		public static readonly DerObjectIdentifier OrganizationalUnitName = new DerObjectIdentifier("2.5.4.11");

		// Token: 0x04001F70 RID: 8048
		public static readonly DerObjectIdentifier id_at_telephoneNumber = new DerObjectIdentifier("2.5.4.20");

		// Token: 0x04001F71 RID: 8049
		public static readonly DerObjectIdentifier id_at_name = new DerObjectIdentifier("2.5.4.41");

		// Token: 0x04001F72 RID: 8050
		public static readonly DerObjectIdentifier IdSha1 = new DerObjectIdentifier("1.3.14.3.2.26");

		// Token: 0x04001F73 RID: 8051
		public static readonly DerObjectIdentifier RipeMD160 = new DerObjectIdentifier("1.3.36.3.2.1");

		// Token: 0x04001F74 RID: 8052
		public static readonly DerObjectIdentifier RipeMD160WithRsaEncryption = new DerObjectIdentifier("1.3.36.3.3.1.2");

		// Token: 0x04001F75 RID: 8053
		public static readonly DerObjectIdentifier IdEARsa = new DerObjectIdentifier("2.5.8.1.1");

		// Token: 0x04001F76 RID: 8054
		public static readonly DerObjectIdentifier IdPkix = new DerObjectIdentifier("1.3.6.1.5.5.7");

		// Token: 0x04001F77 RID: 8055
		public static readonly DerObjectIdentifier IdPE = new DerObjectIdentifier(X509ObjectIdentifiers.IdPkix + ".1");

		// Token: 0x04001F78 RID: 8056
		public static readonly DerObjectIdentifier IdAD = new DerObjectIdentifier(X509ObjectIdentifiers.IdPkix + ".48");

		// Token: 0x04001F79 RID: 8057
		public static readonly DerObjectIdentifier IdADCAIssuers = new DerObjectIdentifier(X509ObjectIdentifiers.IdAD + ".2");

		// Token: 0x04001F7A RID: 8058
		public static readonly DerObjectIdentifier IdADOcsp = new DerObjectIdentifier(X509ObjectIdentifiers.IdAD + ".1");

		// Token: 0x04001F7B RID: 8059
		public static readonly DerObjectIdentifier OcspAccessMethod = X509ObjectIdentifiers.IdADOcsp;

		// Token: 0x04001F7C RID: 8060
		public static readonly DerObjectIdentifier CrlAccessMethod = X509ObjectIdentifiers.IdADCAIssuers;
	}
}
