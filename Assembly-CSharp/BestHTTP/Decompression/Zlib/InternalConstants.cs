using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x02000609 RID: 1545
	internal static class InternalConstants
	{
		// Token: 0x040024AA RID: 9386
		internal static readonly int MAX_BITS = 15;

		// Token: 0x040024AB RID: 9387
		internal static readonly int BL_CODES = 19;

		// Token: 0x040024AC RID: 9388
		internal static readonly int D_CODES = 30;

		// Token: 0x040024AD RID: 9389
		internal static readonly int LITERALS = 256;

		// Token: 0x040024AE RID: 9390
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x040024AF RID: 9391
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x040024B0 RID: 9392
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x040024B1 RID: 9393
		internal static readonly int REP_3_6 = 16;

		// Token: 0x040024B2 RID: 9394
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x040024B3 RID: 9395
		internal static readonly int REPZ_11_138 = 18;
	}
}
