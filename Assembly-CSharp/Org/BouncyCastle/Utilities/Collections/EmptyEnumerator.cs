using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002AF RID: 687
	public sealed class EmptyEnumerator : IEnumerator
	{
		// Token: 0x060016AF RID: 5807 RVA: 0x00003425 File Offset: 0x00001625
		private EmptyEnumerator()
		{
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0002A062 File Offset: 0x00028262
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00003022 File Offset: 0x00001222
		public void Reset()
		{
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x000819F3 File Offset: 0x0007FBF3
		public object Current
		{
			get
			{
				throw new InvalidOperationException("No elements");
			}
		}

		// Token: 0x0400151C RID: 5404
		public static readonly IEnumerator Instance = new EmptyEnumerator();
	}
}
