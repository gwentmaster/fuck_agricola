using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006B8 RID: 1720
	public class PaginatedResult<T> where T : class
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x0012EC76 File Offset: 0x0012CE76
		// (set) Token: 0x06003DAC RID: 15788 RVA: 0x0012EC7E File Offset: 0x0012CE7E
		public Action<Action<PaginatedResult<T>, WebError>> Next { get; private set; }

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06003DAD RID: 15789 RVA: 0x0012EC87 File Offset: 0x0012CE87
		// (set) Token: 0x06003DAE RID: 15790 RVA: 0x0012EC8F File Offset: 0x0012CE8F
		public Action<Action<PaginatedResult<T>, WebError>> Prev { get; private set; }

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003DAF RID: 15791 RVA: 0x0012EC98 File Offset: 0x0012CE98
		// (set) Token: 0x06003DB0 RID: 15792 RVA: 0x0012ECA0 File Offset: 0x0012CEA0
		public Action<Action<PaginatedResult<T>, WebError>> First { get; private set; }

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06003DB1 RID: 15793 RVA: 0x0012ECA9 File Offset: 0x0012CEA9
		// (set) Token: 0x06003DB2 RID: 15794 RVA: 0x0012ECB1 File Offset: 0x0012CEB1
		public Action<Action<PaginatedResult<T>, WebError>> Last { get; private set; }

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x0012ECBA File Offset: 0x0012CEBA
		// (set) Token: 0x06003DB4 RID: 15796 RVA: 0x0012ECC2 File Offset: 0x0012CEC2
		public int TotalElement { get; private set; }

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x0012ECCB File Offset: 0x0012CECB
		// (set) Token: 0x06003DB6 RID: 15798 RVA: 0x0012ECD3 File Offset: 0x0012CED3
		public T[] Elements { get; private set; }

		// Token: 0x06003DB7 RID: 15799 RVA: 0x0012ECDC File Offset: 0x0012CEDC
		public PaginatedResult(int totalElement, T[] elements, Action<Action<PaginatedResult<T>, WebError>> next, Action<Action<PaginatedResult<T>, WebError>> prev, Action<Action<PaginatedResult<T>, WebError>> first, Action<Action<PaginatedResult<T>, WebError>> last)
		{
			this.TotalElement = totalElement;
			this.Elements = elements;
			this.Next = next;
			this.Prev = prev;
			this.First = first;
			this.Last = last;
		}
	}
}
