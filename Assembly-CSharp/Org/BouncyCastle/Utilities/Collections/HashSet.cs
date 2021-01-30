using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002B1 RID: 689
	public class HashSet : ISet, ICollection, IEnumerable
	{
		// Token: 0x060016B6 RID: 5814 RVA: 0x00081A35 File Offset: 0x0007FC35
		public HashSet()
		{
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00081A48 File Offset: 0x0007FC48
		public HashSet(IEnumerable s)
		{
			foreach (object o in s)
			{
				this.Add(o);
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00081AA8 File Offset: 0x0007FCA8
		public virtual void Add(object o)
		{
			this.impl[o] = null;
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00081AB8 File Offset: 0x0007FCB8
		public virtual void AddAll(IEnumerable e)
		{
			foreach (object o in e)
			{
				this.Add(o);
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00081B08 File Offset: 0x0007FD08
		public virtual void Clear()
		{
			this.impl.Clear();
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x00081B15 File Offset: 0x0007FD15
		public virtual bool Contains(object o)
		{
			return this.impl.Contains(o);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00081B23 File Offset: 0x0007FD23
		public virtual void CopyTo(Array array, int index)
		{
			this.impl.Keys.CopyTo(array, index);
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x00081B37 File Offset: 0x0007FD37
		public virtual int Count
		{
			get
			{
				return this.impl.Count;
			}
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x00081B44 File Offset: 0x0007FD44
		public virtual IEnumerator GetEnumerator()
		{
			return this.impl.Keys.GetEnumerator();
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x00081B56 File Offset: 0x0007FD56
		public virtual bool IsEmpty
		{
			get
			{
				return this.impl.Count == 0;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00081B66 File Offset: 0x0007FD66
		public virtual bool IsFixedSize
		{
			get
			{
				return this.impl.IsFixedSize;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060016C1 RID: 5825 RVA: 0x00081B73 File Offset: 0x0007FD73
		public virtual bool IsReadOnly
		{
			get
			{
				return this.impl.IsReadOnly;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x00081B80 File Offset: 0x0007FD80
		public virtual bool IsSynchronized
		{
			get
			{
				return this.impl.IsSynchronized;
			}
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00081B8D File Offset: 0x0007FD8D
		public virtual void Remove(object o)
		{
			this.impl.Remove(o);
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00081B9C File Offset: 0x0007FD9C
		public virtual void RemoveAll(IEnumerable e)
		{
			foreach (object o in e)
			{
				this.Remove(o);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00081BEC File Offset: 0x0007FDEC
		public virtual object SyncRoot
		{
			get
			{
				return this.impl.SyncRoot;
			}
		}

		// Token: 0x0400151E RID: 5406
		private readonly IDictionary impl = Platform.CreateHashtable();
	}
}
