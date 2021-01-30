using System;
using System.Collections;

namespace PlatformSupport.Collections.Specialized
{
	// Token: 0x02000560 RID: 1376
	internal sealed class ReadOnlyList : IList, ICollection, IEnumerable
	{
		// Token: 0x060031C3 RID: 12739 RVA: 0x000FEC9A File Offset: 0x000FCE9A
		internal ReadOnlyList(IList list)
		{
			this._list = list;
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060031C4 RID: 12740 RVA: 0x000FECA9 File Offset: 0x000FCEA9
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060031C5 RID: 12741 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x000FECB6 File Offset: 0x000FCEB6
		public bool IsSynchronized
		{
			get
			{
				return this._list.IsSynchronized;
			}
		}

		// Token: 0x170005E8 RID: 1512
		public object this[int index]
		{
			get
			{
				return this._list[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060031CA RID: 12746 RVA: 0x000FECD1 File Offset: 0x000FCED1
		public object SyncRoot
		{
			get
			{
				return this._list.SyncRoot;
			}
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x0007F71F File Offset: 0x0007D91F
		public int Add(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x0007F71F File Offset: 0x0007D91F
		public void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x000FECDE File Offset: 0x000FCEDE
		public bool Contains(object value)
		{
			return this._list.Contains(value);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000FECEC File Offset: 0x000FCEEC
		public void CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000FECFB File Offset: 0x000FCEFB
		public IEnumerator GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000FED08 File Offset: 0x000FCF08
		public int IndexOf(object value)
		{
			return this._list.IndexOf(value);
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x0007F71F File Offset: 0x0007D91F
		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x0007F71F File Offset: 0x0007D91F
		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x0007F71F File Offset: 0x0007D91F
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002106 RID: 8454
		private readonly IList _list;
	}
}
