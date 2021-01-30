using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x0200060F RID: 1551
	public static class ZlibConstants
	{
		// Token: 0x040024E6 RID: 9446
		public const int WindowBitsMax = 15;

		// Token: 0x040024E7 RID: 9447
		public const int WindowBitsDefault = 15;

		// Token: 0x040024E8 RID: 9448
		public const int Z_OK = 0;

		// Token: 0x040024E9 RID: 9449
		public const int Z_STREAM_END = 1;

		// Token: 0x040024EA RID: 9450
		public const int Z_NEED_DICT = 2;

		// Token: 0x040024EB RID: 9451
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x040024EC RID: 9452
		public const int Z_DATA_ERROR = -3;

		// Token: 0x040024ED RID: 9453
		public const int Z_BUF_ERROR = -5;

		// Token: 0x040024EE RID: 9454
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x040024EF RID: 9455
		public const int WorkingBufferSizeMin = 1024;
	}
}
