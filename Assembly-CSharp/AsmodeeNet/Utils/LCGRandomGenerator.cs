using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000668 RID: 1640
	public class LCGRandomGenerator : RandomGenerator
	{
		// Token: 0x06003C7F RID: 15487 RVA: 0x0012AFDC File Offset: 0x001291DC
		public LCGRandomGenerator()
		{
			ulong num = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			this._seed = (uint)(num & (ulong)-1);
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0012B01D File Offset: 0x0012921D
		public LCGRandomGenerator(uint seed)
		{
			this._seed = seed;
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0012B02C File Offset: 0x0012922C
		public override uint Rand()
		{
			ulong num = 1664525UL * (ulong)this._seed + 1013904223UL;
			this._seed = (uint)(num & (ulong)-1);
			return this._seed;
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x0012B060 File Offset: 0x00129260
		public override uint Range(uint min, uint max)
		{
			uint num = (uint)Math.Abs((long)((ulong)(max - min)));
			return this.Rand() % num + min;
		}
	}
}
