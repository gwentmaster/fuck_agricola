using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x02000262 RID: 610
	public interface IOrderedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06001355 RID: 4949
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x06001356 RID: 4950
		void Insert(int index, object key, object value);

		// Token: 0x06001357 RID: 4951
		void RemoveAt(int index);

		// Token: 0x17000162 RID: 354
		object this[int index]
		{
			get;
			set;
		}
	}
}
