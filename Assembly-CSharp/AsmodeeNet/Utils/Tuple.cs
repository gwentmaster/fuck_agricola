using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000675 RID: 1653
	public class Tuple<T1, T2>
	{
		// Token: 0x06003CC8 RID: 15560 RVA: 0x0012BD8A File Offset: 0x00129F8A
		public Tuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		// Token: 0x04002707 RID: 9991
		public T1 Item1;

		// Token: 0x04002708 RID: 9992
		public T2 Item2;
	}
}
