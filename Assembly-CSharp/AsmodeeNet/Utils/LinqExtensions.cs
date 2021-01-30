using System;
using System.Collections.Generic;
using System.Linq;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200066A RID: 1642
	public static class LinqExtensions
	{
		// Token: 0x06003C85 RID: 15493 RVA: 0x0012B2DD File Offset: 0x001294DD
		public static IEnumerable<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			return first.Except(second).Union(second.Except(first));
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x0012B2F2 File Offset: 0x001294F2
		public static IEnumerable<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer)
		{
			return first.Except(second, comparer).Union(second.Except(first, comparer));
		}
	}
}
