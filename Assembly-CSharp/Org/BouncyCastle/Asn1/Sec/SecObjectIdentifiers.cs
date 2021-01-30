using System;
using Org.BouncyCastle.Asn1.X9;

namespace Org.BouncyCastle.Asn1.Sec
{
	// Token: 0x02000542 RID: 1346
	public abstract class SecObjectIdentifiers
	{
		// Token: 0x04001FA3 RID: 8099
		public static readonly DerObjectIdentifier EllipticCurve = new DerObjectIdentifier("1.3.132.0");

		// Token: 0x04001FA4 RID: 8100
		public static readonly DerObjectIdentifier SecT163k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".1");

		// Token: 0x04001FA5 RID: 8101
		public static readonly DerObjectIdentifier SecT163r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".2");

		// Token: 0x04001FA6 RID: 8102
		public static readonly DerObjectIdentifier SecT239k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".3");

		// Token: 0x04001FA7 RID: 8103
		public static readonly DerObjectIdentifier SecT113r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".4");

		// Token: 0x04001FA8 RID: 8104
		public static readonly DerObjectIdentifier SecT113r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".5");

		// Token: 0x04001FA9 RID: 8105
		public static readonly DerObjectIdentifier SecP112r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".6");

		// Token: 0x04001FAA RID: 8106
		public static readonly DerObjectIdentifier SecP112r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".7");

		// Token: 0x04001FAB RID: 8107
		public static readonly DerObjectIdentifier SecP160r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".8");

		// Token: 0x04001FAC RID: 8108
		public static readonly DerObjectIdentifier SecP160k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".9");

		// Token: 0x04001FAD RID: 8109
		public static readonly DerObjectIdentifier SecP256k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".10");

		// Token: 0x04001FAE RID: 8110
		public static readonly DerObjectIdentifier SecT163r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".15");

		// Token: 0x04001FAF RID: 8111
		public static readonly DerObjectIdentifier SecT283k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".16");

		// Token: 0x04001FB0 RID: 8112
		public static readonly DerObjectIdentifier SecT283r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".17");

		// Token: 0x04001FB1 RID: 8113
		public static readonly DerObjectIdentifier SecT131r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".22");

		// Token: 0x04001FB2 RID: 8114
		public static readonly DerObjectIdentifier SecT131r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".23");

		// Token: 0x04001FB3 RID: 8115
		public static readonly DerObjectIdentifier SecT193r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".24");

		// Token: 0x04001FB4 RID: 8116
		public static readonly DerObjectIdentifier SecT193r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".25");

		// Token: 0x04001FB5 RID: 8117
		public static readonly DerObjectIdentifier SecT233k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".26");

		// Token: 0x04001FB6 RID: 8118
		public static readonly DerObjectIdentifier SecT233r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".27");

		// Token: 0x04001FB7 RID: 8119
		public static readonly DerObjectIdentifier SecP128r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".28");

		// Token: 0x04001FB8 RID: 8120
		public static readonly DerObjectIdentifier SecP128r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".29");

		// Token: 0x04001FB9 RID: 8121
		public static readonly DerObjectIdentifier SecP160r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".30");

		// Token: 0x04001FBA RID: 8122
		public static readonly DerObjectIdentifier SecP192k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".31");

		// Token: 0x04001FBB RID: 8123
		public static readonly DerObjectIdentifier SecP224k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".32");

		// Token: 0x04001FBC RID: 8124
		public static readonly DerObjectIdentifier SecP224r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".33");

		// Token: 0x04001FBD RID: 8125
		public static readonly DerObjectIdentifier SecP384r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".34");

		// Token: 0x04001FBE RID: 8126
		public static readonly DerObjectIdentifier SecP521r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".35");

		// Token: 0x04001FBF RID: 8127
		public static readonly DerObjectIdentifier SecT409k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".36");

		// Token: 0x04001FC0 RID: 8128
		public static readonly DerObjectIdentifier SecT409r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".37");

		// Token: 0x04001FC1 RID: 8129
		public static readonly DerObjectIdentifier SecT571k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".38");

		// Token: 0x04001FC2 RID: 8130
		public static readonly DerObjectIdentifier SecT571r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".39");

		// Token: 0x04001FC3 RID: 8131
		public static readonly DerObjectIdentifier SecP192r1 = X9ObjectIdentifiers.Prime192v1;

		// Token: 0x04001FC4 RID: 8132
		public static readonly DerObjectIdentifier SecP256r1 = X9ObjectIdentifiers.Prime256v1;
	}
}
