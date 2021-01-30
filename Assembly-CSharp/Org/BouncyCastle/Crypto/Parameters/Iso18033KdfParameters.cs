using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000437 RID: 1079
	public class Iso18033KdfParameters : IDerivationParameters
	{
		// Token: 0x060027BA RID: 10170 RVA: 0x000C5A16 File Offset: 0x000C3C16
		public Iso18033KdfParameters(byte[] seed)
		{
			this.seed = seed;
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000C5A25 File Offset: 0x000C3C25
		public byte[] GetSeed()
		{
			return this.seed;
		}

		// Token: 0x04001A52 RID: 6738
		private byte[] seed;
	}
}
