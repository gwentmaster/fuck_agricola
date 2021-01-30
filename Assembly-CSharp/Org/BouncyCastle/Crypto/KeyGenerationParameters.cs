using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200038A RID: 906
	public class KeyGenerationParameters
	{
		// Token: 0x0600222F RID: 8751 RVA: 0x000B519E File Offset: 0x000B339E
		public KeyGenerationParameters(SecureRandom random, int strength)
		{
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			if (strength < 1)
			{
				throw new ArgumentException("strength must be a positive value", "strength");
			}
			this.random = random;
			this.strength = strength;
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x000B51D6 File Offset: 0x000B33D6
		public SecureRandom Random
		{
			get
			{
				return this.random;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06002231 RID: 8753 RVA: 0x000B51DE File Offset: 0x000B33DE
		public int Strength
		{
			get
			{
				return this.strength;
			}
		}

		// Token: 0x0400168C RID: 5772
		private SecureRandom random;

		// Token: 0x0400168D RID: 5773
		private int strength;
	}
}
