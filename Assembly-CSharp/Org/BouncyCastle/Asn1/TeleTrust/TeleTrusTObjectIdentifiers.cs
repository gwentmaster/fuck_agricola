using System;

namespace Org.BouncyCastle.Asn1.TeleTrust
{
	// Token: 0x02000540 RID: 1344
	public sealed class TeleTrusTObjectIdentifiers
	{
		// Token: 0x06003117 RID: 12567 RVA: 0x00003425 File Offset: 0x00001625
		private TeleTrusTObjectIdentifiers()
		{
		}

		// Token: 0x04001F84 RID: 8068
		public static readonly DerObjectIdentifier TeleTrusTAlgorithm = new DerObjectIdentifier("1.3.36.3");

		// Token: 0x04001F85 RID: 8069
		public static readonly DerObjectIdentifier RipeMD160 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTAlgorithm + ".2.1");

		// Token: 0x04001F86 RID: 8070
		public static readonly DerObjectIdentifier RipeMD128 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTAlgorithm + ".2.2");

		// Token: 0x04001F87 RID: 8071
		public static readonly DerObjectIdentifier RipeMD256 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTAlgorithm + ".2.3");

		// Token: 0x04001F88 RID: 8072
		public static readonly DerObjectIdentifier TeleTrusTRsaSignatureAlgorithm = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTAlgorithm + ".3.1");

		// Token: 0x04001F89 RID: 8073
		public static readonly DerObjectIdentifier RsaSignatureWithRipeMD160 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTRsaSignatureAlgorithm + ".2");

		// Token: 0x04001F8A RID: 8074
		public static readonly DerObjectIdentifier RsaSignatureWithRipeMD128 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTRsaSignatureAlgorithm + ".3");

		// Token: 0x04001F8B RID: 8075
		public static readonly DerObjectIdentifier RsaSignatureWithRipeMD256 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTRsaSignatureAlgorithm + ".4");

		// Token: 0x04001F8C RID: 8076
		public static readonly DerObjectIdentifier ECSign = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTAlgorithm + ".3.2");

		// Token: 0x04001F8D RID: 8077
		public static readonly DerObjectIdentifier ECSignWithSha1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.ECSign + ".1");

		// Token: 0x04001F8E RID: 8078
		public static readonly DerObjectIdentifier ECSignWithRipeMD160 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.ECSign + ".2");

		// Token: 0x04001F8F RID: 8079
		public static readonly DerObjectIdentifier EccBrainpool = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.TeleTrusTAlgorithm + ".3.2.8");

		// Token: 0x04001F90 RID: 8080
		public static readonly DerObjectIdentifier EllipticCurve = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.EccBrainpool + ".1");

		// Token: 0x04001F91 RID: 8081
		public static readonly DerObjectIdentifier VersionOne = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.EllipticCurve + ".1");

		// Token: 0x04001F92 RID: 8082
		public static readonly DerObjectIdentifier BrainpoolP160R1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".1");

		// Token: 0x04001F93 RID: 8083
		public static readonly DerObjectIdentifier BrainpoolP160T1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".2");

		// Token: 0x04001F94 RID: 8084
		public static readonly DerObjectIdentifier BrainpoolP192R1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".3");

		// Token: 0x04001F95 RID: 8085
		public static readonly DerObjectIdentifier BrainpoolP192T1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".4");

		// Token: 0x04001F96 RID: 8086
		public static readonly DerObjectIdentifier BrainpoolP224R1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".5");

		// Token: 0x04001F97 RID: 8087
		public static readonly DerObjectIdentifier BrainpoolP224T1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".6");

		// Token: 0x04001F98 RID: 8088
		public static readonly DerObjectIdentifier BrainpoolP256R1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".7");

		// Token: 0x04001F99 RID: 8089
		public static readonly DerObjectIdentifier BrainpoolP256T1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".8");

		// Token: 0x04001F9A RID: 8090
		public static readonly DerObjectIdentifier BrainpoolP320R1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".9");

		// Token: 0x04001F9B RID: 8091
		public static readonly DerObjectIdentifier BrainpoolP320T1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".10");

		// Token: 0x04001F9C RID: 8092
		public static readonly DerObjectIdentifier BrainpoolP384R1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".11");

		// Token: 0x04001F9D RID: 8093
		public static readonly DerObjectIdentifier BrainpoolP384T1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".12");

		// Token: 0x04001F9E RID: 8094
		public static readonly DerObjectIdentifier BrainpoolP512R1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".13");

		// Token: 0x04001F9F RID: 8095
		public static readonly DerObjectIdentifier BrainpoolP512T1 = new DerObjectIdentifier(TeleTrusTObjectIdentifiers.VersionOne + ".14");
	}
}
