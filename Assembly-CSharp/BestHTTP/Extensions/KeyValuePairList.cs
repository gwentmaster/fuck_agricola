using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F6 RID: 1526
	public class KeyValuePairList
	{
		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x00113052 File Offset: 0x00111252
		// (set) Token: 0x060037F7 RID: 14327 RVA: 0x0011305A File Offset: 0x0011125A
		public List<HeaderValue> Values { get; protected set; }

		// Token: 0x060037F8 RID: 14328 RVA: 0x00113064 File Offset: 0x00111264
		public bool TryGet(string valueKeyName, out HeaderValue param)
		{
			param = null;
			for (int i = 0; i < this.Values.Count; i++)
			{
				if (string.CompareOrdinal(this.Values[i].Key, valueKeyName) == 0)
				{
					param = this.Values[i];
					return true;
				}
			}
			return false;
		}
	}
}
