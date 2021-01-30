using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002B2 RID: 690
	public interface ISet : ICollection, IEnumerable
	{
		// Token: 0x060016C6 RID: 5830
		void Add(object o);

		// Token: 0x060016C7 RID: 5831
		void AddAll(IEnumerable e);

		// Token: 0x060016C8 RID: 5832
		void Clear();

		// Token: 0x060016C9 RID: 5833
		bool Contains(object o);

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060016CA RID: 5834
		bool IsEmpty { get; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060016CB RID: 5835
		bool IsFixedSize { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060016CC RID: 5836
		bool IsReadOnly { get; }

		// Token: 0x060016CD RID: 5837
		void Remove(object o);

		// Token: 0x060016CE RID: 5838
		void RemoveAll(IEnumerable e);
	}
}
