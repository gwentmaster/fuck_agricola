using System;

namespace Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x02000550 RID: 1360
	public abstract class MiscObjectIdentifiers
	{
		// Token: 0x040020A3 RID: 8355
		public static readonly DerObjectIdentifier Netscape = new DerObjectIdentifier("2.16.840.1.113730.1");

		// Token: 0x040020A4 RID: 8356
		public static readonly DerObjectIdentifier NetscapeCertType = MiscObjectIdentifiers.Netscape.Branch("1");

		// Token: 0x040020A5 RID: 8357
		public static readonly DerObjectIdentifier NetscapeBaseUrl = MiscObjectIdentifiers.Netscape.Branch("2");

		// Token: 0x040020A6 RID: 8358
		public static readonly DerObjectIdentifier NetscapeRevocationUrl = MiscObjectIdentifiers.Netscape.Branch("3");

		// Token: 0x040020A7 RID: 8359
		public static readonly DerObjectIdentifier NetscapeCARevocationUrl = MiscObjectIdentifiers.Netscape.Branch("4");

		// Token: 0x040020A8 RID: 8360
		public static readonly DerObjectIdentifier NetscapeRenewalUrl = MiscObjectIdentifiers.Netscape.Branch("7");

		// Token: 0x040020A9 RID: 8361
		public static readonly DerObjectIdentifier NetscapeCAPolicyUrl = MiscObjectIdentifiers.Netscape.Branch("8");

		// Token: 0x040020AA RID: 8362
		public static readonly DerObjectIdentifier NetscapeSslServerName = MiscObjectIdentifiers.Netscape.Branch("12");

		// Token: 0x040020AB RID: 8363
		public static readonly DerObjectIdentifier NetscapeCertComment = MiscObjectIdentifiers.Netscape.Branch("13");

		// Token: 0x040020AC RID: 8364
		public static readonly DerObjectIdentifier Verisign = new DerObjectIdentifier("2.16.840.1.113733.1");

		// Token: 0x040020AD RID: 8365
		public static readonly DerObjectIdentifier VerisignCzagExtension = MiscObjectIdentifiers.Verisign.Branch("6.3");

		// Token: 0x040020AE RID: 8366
		public static readonly DerObjectIdentifier VerisignPrivate_6_9 = MiscObjectIdentifiers.Verisign.Branch("6.9");

		// Token: 0x040020AF RID: 8367
		public static readonly DerObjectIdentifier VerisignOnSiteJurisdictionHash = MiscObjectIdentifiers.Verisign.Branch("6.11");

		// Token: 0x040020B0 RID: 8368
		public static readonly DerObjectIdentifier VerisignBitString_6_13 = MiscObjectIdentifiers.Verisign.Branch("6.13");

		// Token: 0x040020B1 RID: 8369
		public static readonly DerObjectIdentifier VerisignDnbDunsNumber = MiscObjectIdentifiers.Verisign.Branch("6.15");

		// Token: 0x040020B2 RID: 8370
		public static readonly DerObjectIdentifier VerisignIssStrongCrypto = MiscObjectIdentifiers.Verisign.Branch("8.1");

		// Token: 0x040020B3 RID: 8371
		public static readonly string Novell = "2.16.840.1.113719";

		// Token: 0x040020B4 RID: 8372
		public static readonly DerObjectIdentifier NovellSecurityAttribs = new DerObjectIdentifier(MiscObjectIdentifiers.Novell + ".1.9.4.1");

		// Token: 0x040020B5 RID: 8373
		public static readonly string Entrust = "1.2.840.113533.7";

		// Token: 0x040020B6 RID: 8374
		public static readonly DerObjectIdentifier EntrustVersionExtension = new DerObjectIdentifier(MiscObjectIdentifiers.Entrust + ".65.0");

		// Token: 0x040020B7 RID: 8375
		public static readonly DerObjectIdentifier as_sys_sec_alg_ideaCBC = new DerObjectIdentifier("1.3.6.1.4.1.188.7.1.1.2");

		// Token: 0x040020B8 RID: 8376
		public static readonly DerObjectIdentifier cryptlib = new DerObjectIdentifier("1.3.6.1.4.1.3029");

		// Token: 0x040020B9 RID: 8377
		public static readonly DerObjectIdentifier cryptlib_algorithm = MiscObjectIdentifiers.cryptlib.Branch("1");

		// Token: 0x040020BA RID: 8378
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_ECB = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.1");

		// Token: 0x040020BB RID: 8379
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_CBC = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.2");

		// Token: 0x040020BC RID: 8380
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_CFB = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.3");

		// Token: 0x040020BD RID: 8381
		public static readonly DerObjectIdentifier cryptlib_algorithm_blowfish_OFB = MiscObjectIdentifiers.cryptlib_algorithm.Branch("1.4");

		// Token: 0x040020BE RID: 8382
		public static readonly DerObjectIdentifier blake2 = new DerObjectIdentifier("1.3.6.1.4.1.1722.12.2");

		// Token: 0x040020BF RID: 8383
		public static readonly DerObjectIdentifier id_blake2b160 = MiscObjectIdentifiers.blake2.Branch("1.5");

		// Token: 0x040020C0 RID: 8384
		public static readonly DerObjectIdentifier id_blake2b256 = MiscObjectIdentifiers.blake2.Branch("1.8");

		// Token: 0x040020C1 RID: 8385
		public static readonly DerObjectIdentifier id_blake2b384 = MiscObjectIdentifiers.blake2.Branch("1.12");

		// Token: 0x040020C2 RID: 8386
		public static readonly DerObjectIdentifier id_blake2b512 = MiscObjectIdentifiers.blake2.Branch("1.16");
	}
}
