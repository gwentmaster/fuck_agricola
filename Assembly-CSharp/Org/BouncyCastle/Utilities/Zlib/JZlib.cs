using System;

namespace Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000292 RID: 658
	public sealed class JZlib
	{
		// Token: 0x060015D7 RID: 5591 RVA: 0x0007F3A9 File Offset: 0x0007D5A9
		public static string version()
		{
			return "1.0.7";
		}

		// Token: 0x04001497 RID: 5271
		private const string _version = "1.0.7";

		// Token: 0x04001498 RID: 5272
		public const int Z_NO_COMPRESSION = 0;

		// Token: 0x04001499 RID: 5273
		public const int Z_BEST_SPEED = 1;

		// Token: 0x0400149A RID: 5274
		public const int Z_BEST_COMPRESSION = 9;

		// Token: 0x0400149B RID: 5275
		public const int Z_DEFAULT_COMPRESSION = -1;

		// Token: 0x0400149C RID: 5276
		public const int Z_FILTERED = 1;

		// Token: 0x0400149D RID: 5277
		public const int Z_HUFFMAN_ONLY = 2;

		// Token: 0x0400149E RID: 5278
		public const int Z_DEFAULT_STRATEGY = 0;

		// Token: 0x0400149F RID: 5279
		public const int Z_NO_FLUSH = 0;

		// Token: 0x040014A0 RID: 5280
		public const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x040014A1 RID: 5281
		public const int Z_SYNC_FLUSH = 2;

		// Token: 0x040014A2 RID: 5282
		public const int Z_FULL_FLUSH = 3;

		// Token: 0x040014A3 RID: 5283
		public const int Z_FINISH = 4;

		// Token: 0x040014A4 RID: 5284
		public const int Z_OK = 0;

		// Token: 0x040014A5 RID: 5285
		public const int Z_STREAM_END = 1;

		// Token: 0x040014A6 RID: 5286
		public const int Z_NEED_DICT = 2;

		// Token: 0x040014A7 RID: 5287
		public const int Z_ERRNO = -1;

		// Token: 0x040014A8 RID: 5288
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x040014A9 RID: 5289
		public const int Z_DATA_ERROR = -3;

		// Token: 0x040014AA RID: 5290
		public const int Z_MEM_ERROR = -4;

		// Token: 0x040014AB RID: 5291
		public const int Z_BUF_ERROR = -5;

		// Token: 0x040014AC RID: 5292
		public const int Z_VERSION_ERROR = -6;
	}
}
