using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002AE RID: 686
	public sealed class EmptyEnumerable : IEnumerable
	{
		// Token: 0x060016AC RID: 5804 RVA: 0x00003425 File Offset: 0x00001625
		private EmptyEnumerable()
		{
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x000819E0 File Offset: 0x0007FBE0
		public IEnumerator GetEnumerator()
		{
			return EmptyEnumerator.Instance;
		}

		// Token: 0x0400151B RID: 5403
		public static readonly IEnumerable Instance = new EmptyEnumerable();
	}
}
