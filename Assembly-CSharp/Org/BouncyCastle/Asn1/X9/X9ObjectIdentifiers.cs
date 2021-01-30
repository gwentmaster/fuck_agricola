using System;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200051F RID: 1311
	public abstract class X9ObjectIdentifiers
	{
		// Token: 0x04001E70 RID: 7792
		internal const string AnsiX962 = "1.2.840.10045";

		// Token: 0x04001E71 RID: 7793
		public static readonly DerObjectIdentifier ansi_X9_62 = new DerObjectIdentifier("1.2.840.10045");

		// Token: 0x04001E72 RID: 7794
		public static readonly DerObjectIdentifier IdFieldType = X9ObjectIdentifiers.ansi_X9_62.Branch("1");

		// Token: 0x04001E73 RID: 7795
		public static readonly DerObjectIdentifier PrimeField = X9ObjectIdentifiers.IdFieldType.Branch("1");

		// Token: 0x04001E74 RID: 7796
		public static readonly DerObjectIdentifier CharacteristicTwoField = X9ObjectIdentifiers.IdFieldType.Branch("2");

		// Token: 0x04001E75 RID: 7797
		public static readonly DerObjectIdentifier GNBasis = X9ObjectIdentifiers.CharacteristicTwoField.Branch("3.1");

		// Token: 0x04001E76 RID: 7798
		public static readonly DerObjectIdentifier TPBasis = X9ObjectIdentifiers.CharacteristicTwoField.Branch("3.2");

		// Token: 0x04001E77 RID: 7799
		public static readonly DerObjectIdentifier PPBasis = X9ObjectIdentifiers.CharacteristicTwoField.Branch("3.3");

		// Token: 0x04001E78 RID: 7800
		[Obsolete("Use 'id_ecSigType' instead")]
		public const string IdECSigType = "1.2.840.10045.4";

		// Token: 0x04001E79 RID: 7801
		public static readonly DerObjectIdentifier id_ecSigType = X9ObjectIdentifiers.ansi_X9_62.Branch("4");

		// Token: 0x04001E7A RID: 7802
		public static readonly DerObjectIdentifier ECDsaWithSha1 = X9ObjectIdentifiers.id_ecSigType.Branch("1");

		// Token: 0x04001E7B RID: 7803
		[Obsolete("Use 'id_publicKeyType' instead")]
		public const string IdPublicKeyType = "1.2.840.10045.2";

		// Token: 0x04001E7C RID: 7804
		public static readonly DerObjectIdentifier id_publicKeyType = X9ObjectIdentifiers.ansi_X9_62.Branch("2");

		// Token: 0x04001E7D RID: 7805
		public static readonly DerObjectIdentifier IdECPublicKey = X9ObjectIdentifiers.id_publicKeyType.Branch("1");

		// Token: 0x04001E7E RID: 7806
		public static readonly DerObjectIdentifier ECDsaWithSha2 = X9ObjectIdentifiers.id_ecSigType.Branch("3");

		// Token: 0x04001E7F RID: 7807
		public static readonly DerObjectIdentifier ECDsaWithSha224 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("1");

		// Token: 0x04001E80 RID: 7808
		public static readonly DerObjectIdentifier ECDsaWithSha256 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("2");

		// Token: 0x04001E81 RID: 7809
		public static readonly DerObjectIdentifier ECDsaWithSha384 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("3");

		// Token: 0x04001E82 RID: 7810
		public static readonly DerObjectIdentifier ECDsaWithSha512 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("4");

		// Token: 0x04001E83 RID: 7811
		public static readonly DerObjectIdentifier EllipticCurve = X9ObjectIdentifiers.ansi_X9_62.Branch("3");

		// Token: 0x04001E84 RID: 7812
		public static readonly DerObjectIdentifier CTwoCurve = X9ObjectIdentifiers.EllipticCurve.Branch("0");

		// Token: 0x04001E85 RID: 7813
		public static readonly DerObjectIdentifier C2Pnb163v1 = X9ObjectIdentifiers.CTwoCurve.Branch("1");

		// Token: 0x04001E86 RID: 7814
		public static readonly DerObjectIdentifier C2Pnb163v2 = X9ObjectIdentifiers.CTwoCurve.Branch("2");

		// Token: 0x04001E87 RID: 7815
		public static readonly DerObjectIdentifier C2Pnb163v3 = X9ObjectIdentifiers.CTwoCurve.Branch("3");

		// Token: 0x04001E88 RID: 7816
		public static readonly DerObjectIdentifier C2Pnb176w1 = X9ObjectIdentifiers.CTwoCurve.Branch("4");

		// Token: 0x04001E89 RID: 7817
		public static readonly DerObjectIdentifier C2Tnb191v1 = X9ObjectIdentifiers.CTwoCurve.Branch("5");

		// Token: 0x04001E8A RID: 7818
		public static readonly DerObjectIdentifier C2Tnb191v2 = X9ObjectIdentifiers.CTwoCurve.Branch("6");

		// Token: 0x04001E8B RID: 7819
		public static readonly DerObjectIdentifier C2Tnb191v3 = X9ObjectIdentifiers.CTwoCurve.Branch("7");

		// Token: 0x04001E8C RID: 7820
		public static readonly DerObjectIdentifier C2Onb191v4 = X9ObjectIdentifiers.CTwoCurve.Branch("8");

		// Token: 0x04001E8D RID: 7821
		public static readonly DerObjectIdentifier C2Onb191v5 = X9ObjectIdentifiers.CTwoCurve.Branch("9");

		// Token: 0x04001E8E RID: 7822
		public static readonly DerObjectIdentifier C2Pnb208w1 = X9ObjectIdentifiers.CTwoCurve.Branch("10");

		// Token: 0x04001E8F RID: 7823
		public static readonly DerObjectIdentifier C2Tnb239v1 = X9ObjectIdentifiers.CTwoCurve.Branch("11");

		// Token: 0x04001E90 RID: 7824
		public static readonly DerObjectIdentifier C2Tnb239v2 = X9ObjectIdentifiers.CTwoCurve.Branch("12");

		// Token: 0x04001E91 RID: 7825
		public static readonly DerObjectIdentifier C2Tnb239v3 = X9ObjectIdentifiers.CTwoCurve.Branch("13");

		// Token: 0x04001E92 RID: 7826
		public static readonly DerObjectIdentifier C2Onb239v4 = X9ObjectIdentifiers.CTwoCurve.Branch("14");

		// Token: 0x04001E93 RID: 7827
		public static readonly DerObjectIdentifier C2Onb239v5 = X9ObjectIdentifiers.CTwoCurve.Branch("15");

		// Token: 0x04001E94 RID: 7828
		public static readonly DerObjectIdentifier C2Pnb272w1 = X9ObjectIdentifiers.CTwoCurve.Branch("16");

		// Token: 0x04001E95 RID: 7829
		public static readonly DerObjectIdentifier C2Pnb304w1 = X9ObjectIdentifiers.CTwoCurve.Branch("17");

		// Token: 0x04001E96 RID: 7830
		public static readonly DerObjectIdentifier C2Tnb359v1 = X9ObjectIdentifiers.CTwoCurve.Branch("18");

		// Token: 0x04001E97 RID: 7831
		public static readonly DerObjectIdentifier C2Pnb368w1 = X9ObjectIdentifiers.CTwoCurve.Branch("19");

		// Token: 0x04001E98 RID: 7832
		public static readonly DerObjectIdentifier C2Tnb431r1 = X9ObjectIdentifiers.CTwoCurve.Branch("20");

		// Token: 0x04001E99 RID: 7833
		public static readonly DerObjectIdentifier PrimeCurve = X9ObjectIdentifiers.EllipticCurve.Branch("1");

		// Token: 0x04001E9A RID: 7834
		public static readonly DerObjectIdentifier Prime192v1 = X9ObjectIdentifiers.PrimeCurve.Branch("1");

		// Token: 0x04001E9B RID: 7835
		public static readonly DerObjectIdentifier Prime192v2 = X9ObjectIdentifiers.PrimeCurve.Branch("2");

		// Token: 0x04001E9C RID: 7836
		public static readonly DerObjectIdentifier Prime192v3 = X9ObjectIdentifiers.PrimeCurve.Branch("3");

		// Token: 0x04001E9D RID: 7837
		public static readonly DerObjectIdentifier Prime239v1 = X9ObjectIdentifiers.PrimeCurve.Branch("4");

		// Token: 0x04001E9E RID: 7838
		public static readonly DerObjectIdentifier Prime239v2 = X9ObjectIdentifiers.PrimeCurve.Branch("5");

		// Token: 0x04001E9F RID: 7839
		public static readonly DerObjectIdentifier Prime239v3 = X9ObjectIdentifiers.PrimeCurve.Branch("6");

		// Token: 0x04001EA0 RID: 7840
		public static readonly DerObjectIdentifier Prime256v1 = X9ObjectIdentifiers.PrimeCurve.Branch("7");

		// Token: 0x04001EA1 RID: 7841
		public static readonly DerObjectIdentifier IdDsa = new DerObjectIdentifier("1.2.840.10040.4.1");

		// Token: 0x04001EA2 RID: 7842
		public static readonly DerObjectIdentifier IdDsaWithSha1 = new DerObjectIdentifier("1.2.840.10040.4.3");

		// Token: 0x04001EA3 RID: 7843
		public static readonly DerObjectIdentifier X9x63Scheme = new DerObjectIdentifier("1.3.133.16.840.63.0");

		// Token: 0x04001EA4 RID: 7844
		public static readonly DerObjectIdentifier DHSinglePassStdDHSha1KdfScheme = X9ObjectIdentifiers.X9x63Scheme.Branch("2");

		// Token: 0x04001EA5 RID: 7845
		public static readonly DerObjectIdentifier DHSinglePassCofactorDHSha1KdfScheme = X9ObjectIdentifiers.X9x63Scheme.Branch("3");

		// Token: 0x04001EA6 RID: 7846
		public static readonly DerObjectIdentifier MqvSinglePassSha1KdfScheme = X9ObjectIdentifiers.X9x63Scheme.Branch("16");

		// Token: 0x04001EA7 RID: 7847
		public static readonly DerObjectIdentifier ansi_x9_42 = new DerObjectIdentifier("1.2.840.10046");

		// Token: 0x04001EA8 RID: 7848
		public static readonly DerObjectIdentifier DHPublicNumber = X9ObjectIdentifiers.ansi_x9_42.Branch("2.1");

		// Token: 0x04001EA9 RID: 7849
		public static readonly DerObjectIdentifier X9x42Schemes = X9ObjectIdentifiers.ansi_x9_42.Branch("2.3");

		// Token: 0x04001EAA RID: 7850
		public static readonly DerObjectIdentifier DHStatic = X9ObjectIdentifiers.X9x42Schemes.Branch("1");

		// Token: 0x04001EAB RID: 7851
		public static readonly DerObjectIdentifier DHEphem = X9ObjectIdentifiers.X9x42Schemes.Branch("2");

		// Token: 0x04001EAC RID: 7852
		public static readonly DerObjectIdentifier DHOneFlow = X9ObjectIdentifiers.X9x42Schemes.Branch("3");

		// Token: 0x04001EAD RID: 7853
		public static readonly DerObjectIdentifier DHHybrid1 = X9ObjectIdentifiers.X9x42Schemes.Branch("4");

		// Token: 0x04001EAE RID: 7854
		public static readonly DerObjectIdentifier DHHybrid2 = X9ObjectIdentifiers.X9x42Schemes.Branch("5");

		// Token: 0x04001EAF RID: 7855
		public static readonly DerObjectIdentifier DHHybridOneFlow = X9ObjectIdentifiers.X9x42Schemes.Branch("6");

		// Token: 0x04001EB0 RID: 7856
		public static readonly DerObjectIdentifier Mqv2 = X9ObjectIdentifiers.X9x42Schemes.Branch("7");

		// Token: 0x04001EB1 RID: 7857
		public static readonly DerObjectIdentifier Mqv1 = X9ObjectIdentifiers.X9x42Schemes.Branch("8");
	}
}
