using System;

namespace Org.BouncyCastle.Asn1.Iana
{
	// Token: 0x02000554 RID: 1364
	public abstract class IanaObjectIdentifiers
	{
		// Token: 0x040020CB RID: 8395
		public static readonly DerObjectIdentifier IsakmpOakley = new DerObjectIdentifier("1.3.6.1.5.5.8.1");

		// Token: 0x040020CC RID: 8396
		public static readonly DerObjectIdentifier HmacMD5 = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".1");

		// Token: 0x040020CD RID: 8397
		public static readonly DerObjectIdentifier HmacSha1 = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".2");

		// Token: 0x040020CE RID: 8398
		public static readonly DerObjectIdentifier HmacTiger = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".3");

		// Token: 0x040020CF RID: 8399
		public static readonly DerObjectIdentifier HmacRipeMD160 = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".4");
	}
}
