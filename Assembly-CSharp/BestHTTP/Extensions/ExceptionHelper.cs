using System;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F1 RID: 1521
	public static class ExceptionHelper
	{
		// Token: 0x060037E1 RID: 14305 RVA: 0x00112B60 File Offset: 0x00110D60
		public static Exception ServerClosedTCPStream()
		{
			return new Exception("TCP Stream closed unexpectedly by the remote server");
		}
	}
}
