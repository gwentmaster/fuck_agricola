using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200041F RID: 1055
	public class DHValidationParameters
	{
		// Token: 0x06002720 RID: 10016 RVA: 0x000C4764 File Offset: 0x000C2964
		public DHValidationParameters(byte[] seed, int counter)
		{
			if (seed == null)
			{
				throw new ArgumentNullException("seed");
			}
			this.seed = (byte[])seed.Clone();
			this.counter = counter;
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000C4792 File Offset: 0x000C2992
		public byte[] GetSeed()
		{
			return (byte[])this.seed.Clone();
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x000C47A4 File Offset: 0x000C29A4
		public int Counter
		{
			get
			{
				return this.counter;
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000C47AC File Offset: 0x000C29AC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHValidationParameters dhvalidationParameters = obj as DHValidationParameters;
			return dhvalidationParameters != null && this.Equals(dhvalidationParameters);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000C47D2 File Offset: 0x000C29D2
		protected bool Equals(DHValidationParameters other)
		{
			return this.counter == other.counter && Arrays.AreEqual(this.seed, other.seed);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000C47F8 File Offset: 0x000C29F8
		public override int GetHashCode()
		{
			return this.counter.GetHashCode() ^ Arrays.GetHashCode(this.seed);
		}

		// Token: 0x04001A21 RID: 6689
		private readonly byte[] seed;

		// Token: 0x04001A22 RID: 6690
		private readonly int counter;
	}
}
