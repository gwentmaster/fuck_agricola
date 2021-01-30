using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000443 RID: 1091
	public class RC5Parameters : KeyParameter
	{
		// Token: 0x060027E5 RID: 10213 RVA: 0x000C5E04 File Offset: 0x000C4004
		public RC5Parameters(byte[] key, int rounds) : base(key)
		{
			if (key.Length > 255)
			{
				throw new ArgumentException("RC5 key length can be no greater than 255");
			}
			this.rounds = rounds;
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000C5E29 File Offset: 0x000C4029
		public int Rounds
		{
			get
			{
				return this.rounds;
			}
		}

		// Token: 0x04001A68 RID: 6760
		private readonly int rounds;
	}
}
