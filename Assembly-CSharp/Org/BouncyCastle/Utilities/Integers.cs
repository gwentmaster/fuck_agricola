using System;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x02000287 RID: 647
	public abstract class Integers
	{
		// Token: 0x06001567 RID: 5479 RVA: 0x000799B5 File Offset: 0x00077BB5
		public static int RotateLeft(int i, int distance)
		{
			return i << distance ^ (int)((uint)i >> -distance);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x000799C5 File Offset: 0x00077BC5
		public static int RotateRight(int i, int distance)
		{
			return (int)((uint)i >> distance ^ (uint)((uint)i << -distance));
		}
	}
}
