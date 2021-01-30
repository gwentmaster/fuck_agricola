using System;

namespace Org.BouncyCastle.Asn1.Nist
{
	// Token: 0x0200054F RID: 1359
	public sealed class NistObjectIdentifiers
	{
		// Token: 0x0600316E RID: 12654 RVA: 0x00003425 File Offset: 0x00001625
		private NistObjectIdentifiers()
		{
		}

		// Token: 0x0400207A RID: 8314
		public static readonly DerObjectIdentifier NistAlgorithm = new DerObjectIdentifier("2.16.840.1.101.3.4");

		// Token: 0x0400207B RID: 8315
		public static readonly DerObjectIdentifier HashAlgs = NistObjectIdentifiers.NistAlgorithm.Branch("2");

		// Token: 0x0400207C RID: 8316
		public static readonly DerObjectIdentifier IdSha256 = NistObjectIdentifiers.HashAlgs.Branch("1");

		// Token: 0x0400207D RID: 8317
		public static readonly DerObjectIdentifier IdSha384 = NistObjectIdentifiers.HashAlgs.Branch("2");

		// Token: 0x0400207E RID: 8318
		public static readonly DerObjectIdentifier IdSha512 = NistObjectIdentifiers.HashAlgs.Branch("3");

		// Token: 0x0400207F RID: 8319
		public static readonly DerObjectIdentifier IdSha224 = NistObjectIdentifiers.HashAlgs.Branch("4");

		// Token: 0x04002080 RID: 8320
		public static readonly DerObjectIdentifier IdSha512_224 = NistObjectIdentifiers.HashAlgs.Branch("5");

		// Token: 0x04002081 RID: 8321
		public static readonly DerObjectIdentifier IdSha512_256 = NistObjectIdentifiers.HashAlgs.Branch("6");

		// Token: 0x04002082 RID: 8322
		public static readonly DerObjectIdentifier IdSha3_224 = NistObjectIdentifiers.HashAlgs.Branch("7");

		// Token: 0x04002083 RID: 8323
		public static readonly DerObjectIdentifier IdSha3_256 = NistObjectIdentifiers.HashAlgs.Branch("8");

		// Token: 0x04002084 RID: 8324
		public static readonly DerObjectIdentifier IdSha3_384 = NistObjectIdentifiers.HashAlgs.Branch("9");

		// Token: 0x04002085 RID: 8325
		public static readonly DerObjectIdentifier IdSha3_512 = NistObjectIdentifiers.HashAlgs.Branch("10");

		// Token: 0x04002086 RID: 8326
		public static readonly DerObjectIdentifier IdShake128 = NistObjectIdentifiers.HashAlgs.Branch("11");

		// Token: 0x04002087 RID: 8327
		public static readonly DerObjectIdentifier IdShake256 = NistObjectIdentifiers.HashAlgs.Branch("12");

		// Token: 0x04002088 RID: 8328
		public static readonly DerObjectIdentifier Aes = new DerObjectIdentifier(NistObjectIdentifiers.NistAlgorithm + ".1");

		// Token: 0x04002089 RID: 8329
		public static readonly DerObjectIdentifier IdAes128Ecb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".1");

		// Token: 0x0400208A RID: 8330
		public static readonly DerObjectIdentifier IdAes128Cbc = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".2");

		// Token: 0x0400208B RID: 8331
		public static readonly DerObjectIdentifier IdAes128Ofb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".3");

		// Token: 0x0400208C RID: 8332
		public static readonly DerObjectIdentifier IdAes128Cfb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".4");

		// Token: 0x0400208D RID: 8333
		public static readonly DerObjectIdentifier IdAes128Wrap = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".5");

		// Token: 0x0400208E RID: 8334
		public static readonly DerObjectIdentifier IdAes128Gcm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".6");

		// Token: 0x0400208F RID: 8335
		public static readonly DerObjectIdentifier IdAes128Ccm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".7");

		// Token: 0x04002090 RID: 8336
		public static readonly DerObjectIdentifier IdAes192Ecb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".21");

		// Token: 0x04002091 RID: 8337
		public static readonly DerObjectIdentifier IdAes192Cbc = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".22");

		// Token: 0x04002092 RID: 8338
		public static readonly DerObjectIdentifier IdAes192Ofb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".23");

		// Token: 0x04002093 RID: 8339
		public static readonly DerObjectIdentifier IdAes192Cfb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".24");

		// Token: 0x04002094 RID: 8340
		public static readonly DerObjectIdentifier IdAes192Wrap = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".25");

		// Token: 0x04002095 RID: 8341
		public static readonly DerObjectIdentifier IdAes192Gcm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".26");

		// Token: 0x04002096 RID: 8342
		public static readonly DerObjectIdentifier IdAes192Ccm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".27");

		// Token: 0x04002097 RID: 8343
		public static readonly DerObjectIdentifier IdAes256Ecb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".41");

		// Token: 0x04002098 RID: 8344
		public static readonly DerObjectIdentifier IdAes256Cbc = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".42");

		// Token: 0x04002099 RID: 8345
		public static readonly DerObjectIdentifier IdAes256Ofb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".43");

		// Token: 0x0400209A RID: 8346
		public static readonly DerObjectIdentifier IdAes256Cfb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".44");

		// Token: 0x0400209B RID: 8347
		public static readonly DerObjectIdentifier IdAes256Wrap = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".45");

		// Token: 0x0400209C RID: 8348
		public static readonly DerObjectIdentifier IdAes256Gcm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".46");

		// Token: 0x0400209D RID: 8349
		public static readonly DerObjectIdentifier IdAes256Ccm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".47");

		// Token: 0x0400209E RID: 8350
		public static readonly DerObjectIdentifier IdDsaWithSha2 = new DerObjectIdentifier(NistObjectIdentifiers.NistAlgorithm + ".3");

		// Token: 0x0400209F RID: 8351
		public static readonly DerObjectIdentifier DsaWithSha224 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".1");

		// Token: 0x040020A0 RID: 8352
		public static readonly DerObjectIdentifier DsaWithSha256 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".2");

		// Token: 0x040020A1 RID: 8353
		public static readonly DerObjectIdentifier DsaWithSha384 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".3");

		// Token: 0x040020A2 RID: 8354
		public static readonly DerObjectIdentifier DsaWithSha512 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".4");
	}
}
