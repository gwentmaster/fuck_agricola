using System;

namespace Org.BouncyCastle.Asn1.Oiw
{
	// Token: 0x02000549 RID: 1353
	public abstract class OiwObjectIdentifiers
	{
		// Token: 0x04002061 RID: 8289
		public static readonly DerObjectIdentifier MD4WithRsa = new DerObjectIdentifier("1.3.14.3.2.2");

		// Token: 0x04002062 RID: 8290
		public static readonly DerObjectIdentifier MD5WithRsa = new DerObjectIdentifier("1.3.14.3.2.3");

		// Token: 0x04002063 RID: 8291
		public static readonly DerObjectIdentifier MD4WithRsaEncryption = new DerObjectIdentifier("1.3.14.3.2.4");

		// Token: 0x04002064 RID: 8292
		public static readonly DerObjectIdentifier DesEcb = new DerObjectIdentifier("1.3.14.3.2.6");

		// Token: 0x04002065 RID: 8293
		public static readonly DerObjectIdentifier DesCbc = new DerObjectIdentifier("1.3.14.3.2.7");

		// Token: 0x04002066 RID: 8294
		public static readonly DerObjectIdentifier DesOfb = new DerObjectIdentifier("1.3.14.3.2.8");

		// Token: 0x04002067 RID: 8295
		public static readonly DerObjectIdentifier DesCfb = new DerObjectIdentifier("1.3.14.3.2.9");

		// Token: 0x04002068 RID: 8296
		public static readonly DerObjectIdentifier DesEde = new DerObjectIdentifier("1.3.14.3.2.17");

		// Token: 0x04002069 RID: 8297
		public static readonly DerObjectIdentifier IdSha1 = new DerObjectIdentifier("1.3.14.3.2.26");

		// Token: 0x0400206A RID: 8298
		public static readonly DerObjectIdentifier DsaWithSha1 = new DerObjectIdentifier("1.3.14.3.2.27");

		// Token: 0x0400206B RID: 8299
		public static readonly DerObjectIdentifier Sha1WithRsa = new DerObjectIdentifier("1.3.14.3.2.29");

		// Token: 0x0400206C RID: 8300
		public static readonly DerObjectIdentifier ElGamalAlgorithm = new DerObjectIdentifier("1.3.14.7.2.1.1");
	}
}
