using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000673 RID: 1651
	public class SystemRandomGenerator : RandomGenerator
	{
		// Token: 0x06003CBE RID: 15550 RVA: 0x0012BC34 File Offset: 0x00129E34
		public SystemRandomGenerator()
		{
			this._random = new Random();
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x0012BC47 File Offset: 0x00129E47
		public SystemRandomGenerator(uint seed)
		{
			base.Seed = seed;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x0012BC56 File Offset: 0x00129E56
		public override uint Rand()
		{
			this._seed = (uint)this._random.Next();
			return this._seed;
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0012BC6F File Offset: 0x00129E6F
		public override uint Range(uint min, uint max)
		{
			this._seed = (uint)this._random.Next((int)min, (int)max);
			return this._seed;
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x0012BC8A File Offset: 0x00129E8A
		protected override void SeedUpdated()
		{
			this._random = new Random((int)this._seed);
		}

		// Token: 0x04002705 RID: 9989
		private Random _random;
	}
}
