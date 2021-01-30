using System;
using System.Runtime.InteropServices;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x02000607 RID: 1543
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	internal class ZlibException : Exception
	{
		// Token: 0x0600388A RID: 14474 RVA: 0x000735AD File Offset: 0x000717AD
		public ZlibException()
		{
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x00073619 File Offset: 0x00071819
		public ZlibException(string s) : base(s)
		{
		}
	}
}
