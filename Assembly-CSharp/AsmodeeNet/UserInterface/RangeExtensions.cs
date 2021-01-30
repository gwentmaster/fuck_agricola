using System;
using UnityEngine.SocialPlatforms;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000640 RID: 1600
	internal static class RangeExtensions
	{
		// Token: 0x06003AFD RID: 15101 RVA: 0x00125689 File Offset: 0x00123889
		public static int Last(this Range range)
		{
			if (range.count == 0)
			{
				throw new InvalidOperationException("Empty range has no to()");
			}
			return range.from + range.count - 1;
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x001256AD File Offset: 0x001238AD
		public static bool Contains(this Range range, int num)
		{
			return num >= range.from && num < range.from + range.count;
		}
	}
}
