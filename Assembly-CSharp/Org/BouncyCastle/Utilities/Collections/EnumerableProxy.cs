using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002B0 RID: 688
	public sealed class EnumerableProxy : IEnumerable
	{
		// Token: 0x060016B4 RID: 5812 RVA: 0x00081A0B File Offset: 0x0007FC0B
		public EnumerableProxy(IEnumerable inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			this.inner = inner;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x00081A28 File Offset: 0x0007FC28
		public IEnumerator GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		// Token: 0x0400151D RID: 5405
		private readonly IEnumerable inner;
	}
}
