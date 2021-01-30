using System;
using System.Collections.Generic;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200066D RID: 1645
	public abstract class RandomGenerator
	{
		// Token: 0x06003C8B RID: 15499
		public abstract uint Rand();

		// Token: 0x06003C8C RID: 15500
		public abstract uint Range(uint min, uint max);

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003C8D RID: 15501 RVA: 0x0012B449 File Offset: 0x00129649
		// (set) Token: 0x06003C8E RID: 15502 RVA: 0x0012B451 File Offset: 0x00129651
		public uint Seed
		{
			get
			{
				return this._seed;
			}
			set
			{
				this._seed = value;
				this.SeedUpdated();
			}
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x00003022 File Offset: 0x00001222
		protected virtual void SeedUpdated()
		{
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x0012B460 File Offset: 0x00129660
		public int RandomIndex<T>(IList<T> list)
		{
			int count = list.Count;
			if (count == 0)
			{
				throw new ArgumentOutOfRangeException("Empty list");
			}
			return (int)((ulong)this.Rand() % (ulong)((long)count));
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x0012B490 File Offset: 0x00129690
		public T ObjectAtRandomIndex<T>(IList<T> list)
		{
			T result;
			try
			{
				int index = this.RandomIndex<T>(list);
				result = list[index];
			}
			catch
			{
				result = default(!!0);
			}
			return result;
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x0012B4D0 File Offset: 0x001296D0
		public void Shuffle<T>(IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				int index = (int)((ulong)this.Rand() % (ulong)((long)i--));
				T value = list[i];
				list[i] = list[index];
				list[index] = value;
			}
		}

		// Token: 0x040026EF RID: 9967
		protected uint _seed;
	}
}
