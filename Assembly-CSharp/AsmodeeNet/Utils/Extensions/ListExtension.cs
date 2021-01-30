using System;
using System.Collections.Generic;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x0200067B RID: 1659
	public static class ListExtension
	{
		// Token: 0x06003CD4 RID: 15572 RVA: 0x0012C02C File Offset: 0x0012A22C
		public static T First<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				return items[0];
			}
			return default(!!0);
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0012C058 File Offset: 0x0012A258
		public static T Last<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				return items[items.Count - 1];
			}
			return default(!!0);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0012C08C File Offset: 0x0012A28C
		public static T RemoveFirst<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				T result = items[0];
				items.RemoveAt(0);
				return result;
			}
			return default(!!0);
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0012C0C0 File Offset: 0x0012A2C0
		public static T RemoveLast<T>(this List<T> items)
		{
			if (items != null && items.Count > 0)
			{
				T result = items[items.Count - 1];
				items.RemoveAt(items.Count - 1);
				return result;
			}
			return default(!!0);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0012C100 File Offset: 0x0012A300
		public static int? Max(this List<int?> items)
		{
			int? num = null;
			if (items != null && items.Count > 0)
			{
				int i = 0;
				while (i < items.Count)
				{
					if (num == null)
					{
						goto IL_4D;
					}
					int? num2 = items[i];
					int? num3 = num;
					if (num2.GetValueOrDefault() > num3.GetValueOrDefault() & (num2 != null & num3 != null))
					{
						goto IL_4D;
					}
					IL_55:
					i++;
					continue;
					IL_4D:
					num = items[i];
					goto IL_55;
				}
			}
			return num;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0012C170 File Offset: 0x0012A370
		public static int Max(this List<int> items)
		{
			int num = -1;
			if (items != null && items.Count > 0)
			{
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i] > num)
					{
						num = items[i];
					}
				}
			}
			return num;
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x0012C1B0 File Offset: 0x0012A3B0
		public static int Sum(this List<int> items)
		{
			int num = 0;
			if (items != null && items.Count > 0)
			{
				for (int i = 0; i < items.Count; i++)
				{
					num += items[i];
				}
			}
			return num;
		}
	}
}
