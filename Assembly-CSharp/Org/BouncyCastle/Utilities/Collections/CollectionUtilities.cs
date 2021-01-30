using System;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002AD RID: 685
	public abstract class CollectionUtilities
	{
		// Token: 0x060016A5 RID: 5797 RVA: 0x000818C8 File Offset: 0x0007FAC8
		public static void AddRange(IList to, IEnumerable range)
		{
			foreach (object value in range)
			{
				to.Add(value);
			}
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00081918 File Offset: 0x0007FB18
		public static bool CheckElementsAreOfType(IEnumerable e, Type t)
		{
			foreach (object o in e)
			{
				if (!t.IsInstanceOfType(o))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00035D67 File Offset: 0x00033F67
		public static IDictionary ReadOnly(IDictionary d)
		{
			return d;
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00035D67 File Offset: 0x00033F67
		public static IList ReadOnly(IList l)
		{
			return l;
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00035D67 File Offset: 0x00033F67
		public static ISet ReadOnly(ISet s)
		{
			return s;
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00081970 File Offset: 0x0007FB70
		public static string ToString(IEnumerable c)
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			IEnumerator enumerator = c.GetEnumerator();
			if (enumerator.MoveNext())
			{
				stringBuilder.Append(enumerator.Current.ToString());
				while (enumerator.MoveNext())
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(enumerator.Current.ToString());
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}
}
