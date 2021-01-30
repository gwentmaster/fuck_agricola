using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000427 RID: 1063
	public class DsaValidationParameters
	{
		// Token: 0x06002751 RID: 10065 RVA: 0x000C4D90 File Offset: 0x000C2F90
		public DsaValidationParameters(byte[] seed, int counter) : this(seed, counter, -1)
		{
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000C4D9B File Offset: 0x000C2F9B
		public DsaValidationParameters(byte[] seed, int counter, int usageIndex)
		{
			if (seed == null)
			{
				throw new ArgumentNullException("seed");
			}
			this.seed = (byte[])seed.Clone();
			this.counter = counter;
			this.usageIndex = usageIndex;
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000C4DD0 File Offset: 0x000C2FD0
		public virtual byte[] GetSeed()
		{
			return (byte[])this.seed.Clone();
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06002754 RID: 10068 RVA: 0x000C4DE2 File Offset: 0x000C2FE2
		public virtual int Counter
		{
			get
			{
				return this.counter;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x000C4DEA File Offset: 0x000C2FEA
		public virtual int UsageIndex
		{
			get
			{
				return this.usageIndex;
			}
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000C4DF4 File Offset: 0x000C2FF4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaValidationParameters dsaValidationParameters = obj as DsaValidationParameters;
			return dsaValidationParameters != null && this.Equals(dsaValidationParameters);
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000C4E1A File Offset: 0x000C301A
		protected virtual bool Equals(DsaValidationParameters other)
		{
			return this.counter == other.counter && Arrays.AreEqual(this.seed, other.seed);
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000C4E40 File Offset: 0x000C3040
		public override int GetHashCode()
		{
			return this.counter.GetHashCode() ^ Arrays.GetHashCode(this.seed);
		}

		// Token: 0x04001A2F RID: 6703
		private readonly byte[] seed;

		// Token: 0x04001A30 RID: 6704
		private readonly int counter;

		// Token: 0x04001A31 RID: 6705
		private readonly int usageIndex;
	}
}
