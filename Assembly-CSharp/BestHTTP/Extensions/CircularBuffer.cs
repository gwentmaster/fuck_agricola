using System;

namespace BestHTTP.Extensions
{
	// Token: 0x020005EF RID: 1519
	public sealed class CircularBuffer<T>
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060037BC RID: 14268 RVA: 0x0011243E File Offset: 0x0011063E
		// (set) Token: 0x060037BD RID: 14269 RVA: 0x00112446 File Offset: 0x00110646
		public int Capacity { get; private set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x0011244F File Offset: 0x0011064F
		// (set) Token: 0x060037BF RID: 14271 RVA: 0x00112457 File Offset: 0x00110657
		public int Count { get; private set; }

		// Token: 0x17000753 RID: 1875
		public T this[int idx]
		{
			get
			{
				int num = (this.startIdx + idx) % this.Capacity;
				return this.buffer[num];
			}
			set
			{
				int num = (this.startIdx + idx) % this.Capacity;
				this.buffer[num] = value;
			}
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x001124B6 File Offset: 0x001106B6
		public CircularBuffer(int capacity)
		{
			this.Capacity = capacity;
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x001124C8 File Offset: 0x001106C8
		public void Add(T element)
		{
			if (this.buffer == null)
			{
				this.buffer = new !0[this.Capacity];
			}
			this.buffer[this.endIdx] = element;
			this.endIdx = (this.endIdx + 1) % this.Capacity;
			if (this.endIdx == this.startIdx)
			{
				this.startIdx = (this.startIdx + 1) % this.Capacity;
			}
			this.Count = Math.Min(this.Count + 1, this.Capacity);
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x00112554 File Offset: 0x00110754
		public void Clear()
		{
			this.startIdx = (this.endIdx = 0);
		}

		// Token: 0x040023C3 RID: 9155
		private T[] buffer;

		// Token: 0x040023C4 RID: 9156
		private int startIdx;

		// Token: 0x040023C5 RID: 9157
		private int endIdx;
	}
}
