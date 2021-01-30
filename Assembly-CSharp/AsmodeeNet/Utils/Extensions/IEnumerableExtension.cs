using System;
using System.Collections.Generic;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x0200067A RID: 1658
	public static class IEnumerableExtension
	{
		// Token: 0x06003CD3 RID: 15571 RVA: 0x0012BFEC File Offset: 0x0012A1EC
		public static T First<T>(this IEnumerable<T> items)
		{
			T result;
			using (IEnumerator<T> enumerator = items.GetEnumerator())
			{
				enumerator.MoveNext();
				result = enumerator.Current;
			}
			return result;
		}
	}
}
