using System;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x02000555 RID: 1365
	public abstract class CryptoProObjectIdentifiers
	{
		// Token: 0x040020D0 RID: 8400
		public const string GostID = "1.2.643.2.2";

		// Token: 0x040020D1 RID: 8401
		public static readonly DerObjectIdentifier GostR3411 = new DerObjectIdentifier("1.2.643.2.2.9");

		// Token: 0x040020D2 RID: 8402
		public static readonly DerObjectIdentifier GostR3411Hmac = new DerObjectIdentifier("1.2.643.2.2.10");

		// Token: 0x040020D3 RID: 8403
		public static readonly DerObjectIdentifier GostR28147Cbc = new DerObjectIdentifier("1.2.643.2.2.21");

		// Token: 0x040020D4 RID: 8404
		public static readonly DerObjectIdentifier ID_Gost28147_89_CryptoPro_A_ParamSet = new DerObjectIdentifier("1.2.643.2.2.31.1");

		// Token: 0x040020D5 RID: 8405
		public static readonly DerObjectIdentifier GostR3410x94 = new DerObjectIdentifier("1.2.643.2.2.20");

		// Token: 0x040020D6 RID: 8406
		public static readonly DerObjectIdentifier GostR3410x2001 = new DerObjectIdentifier("1.2.643.2.2.19");

		// Token: 0x040020D7 RID: 8407
		public static readonly DerObjectIdentifier GostR3411x94WithGostR3410x94 = new DerObjectIdentifier("1.2.643.2.2.4");

		// Token: 0x040020D8 RID: 8408
		public static readonly DerObjectIdentifier GostR3411x94WithGostR3410x2001 = new DerObjectIdentifier("1.2.643.2.2.3");

		// Token: 0x040020D9 RID: 8409
		public static readonly DerObjectIdentifier GostR3411x94CryptoProParamSet = new DerObjectIdentifier("1.2.643.2.2.30.1");

		// Token: 0x040020DA RID: 8410
		public static readonly DerObjectIdentifier GostR3410x94CryptoProA = new DerObjectIdentifier("1.2.643.2.2.32.2");

		// Token: 0x040020DB RID: 8411
		public static readonly DerObjectIdentifier GostR3410x94CryptoProB = new DerObjectIdentifier("1.2.643.2.2.32.3");

		// Token: 0x040020DC RID: 8412
		public static readonly DerObjectIdentifier GostR3410x94CryptoProC = new DerObjectIdentifier("1.2.643.2.2.32.4");

		// Token: 0x040020DD RID: 8413
		public static readonly DerObjectIdentifier GostR3410x94CryptoProD = new DerObjectIdentifier("1.2.643.2.2.32.5");

		// Token: 0x040020DE RID: 8414
		public static readonly DerObjectIdentifier GostR3410x94CryptoProXchA = new DerObjectIdentifier("1.2.643.2.2.33.1");

		// Token: 0x040020DF RID: 8415
		public static readonly DerObjectIdentifier GostR3410x94CryptoProXchB = new DerObjectIdentifier("1.2.643.2.2.33.2");

		// Token: 0x040020E0 RID: 8416
		public static readonly DerObjectIdentifier GostR3410x94CryptoProXchC = new DerObjectIdentifier("1.2.643.2.2.33.3");

		// Token: 0x040020E1 RID: 8417
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProA = new DerObjectIdentifier("1.2.643.2.2.35.1");

		// Token: 0x040020E2 RID: 8418
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProB = new DerObjectIdentifier("1.2.643.2.2.35.2");

		// Token: 0x040020E3 RID: 8419
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProC = new DerObjectIdentifier("1.2.643.2.2.35.3");

		// Token: 0x040020E4 RID: 8420
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProXchA = new DerObjectIdentifier("1.2.643.2.2.36.0");

		// Token: 0x040020E5 RID: 8421
		public static readonly DerObjectIdentifier GostR3410x2001CryptoProXchB = new DerObjectIdentifier("1.2.643.2.2.36.1");

		// Token: 0x040020E6 RID: 8422
		public static readonly DerObjectIdentifier GostElSgDH3410Default = new DerObjectIdentifier("1.2.643.2.2.36.0");

		// Token: 0x040020E7 RID: 8423
		public static readonly DerObjectIdentifier GostElSgDH3410x1 = new DerObjectIdentifier("1.2.643.2.2.36.1");
	}
}
