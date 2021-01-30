using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x020005F8 RID: 1528
	internal enum BlockState
	{
		// Token: 0x040023CE RID: 9166
		NeedMore,
		// Token: 0x040023CF RID: 9167
		BlockDone,
		// Token: 0x040023D0 RID: 9168
		FinishStarted,
		// Token: 0x040023D1 RID: 9169
		FinishDone
	}
}
