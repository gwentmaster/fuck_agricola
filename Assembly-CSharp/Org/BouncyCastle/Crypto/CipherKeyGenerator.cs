using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000370 RID: 880
	public class CipherKeyGenerator
	{
		// Token: 0x060021C8 RID: 8648 RVA: 0x000B50A5 File Offset: 0x000B32A5
		public CipherKeyGenerator()
		{
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000B50B4 File Offset: 0x000B32B4
		internal CipherKeyGenerator(int defaultStrength)
		{
			if (defaultStrength < 1)
			{
				throw new ArgumentException("strength must be a positive value", "defaultStrength");
			}
			this.defaultStrength = defaultStrength;
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x000B50DE File Offset: 0x000B32DE
		public int DefaultStrength
		{
			get
			{
				return this.defaultStrength;
			}
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000B50E6 File Offset: 0x000B32E6
		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.uninitialised = false;
			this.engineInit(parameters);
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000B5104 File Offset: 0x000B3304
		protected virtual void engineInit(KeyGenerationParameters parameters)
		{
			this.random = parameters.Random;
			this.strength = (parameters.Strength + 7) / 8;
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000B5124 File Offset: 0x000B3324
		public byte[] GenerateKey()
		{
			if (this.uninitialised)
			{
				if (this.defaultStrength < 1)
				{
					throw new InvalidOperationException("Generator has not been initialised");
				}
				this.uninitialised = false;
				this.engineInit(new KeyGenerationParameters(new SecureRandom(), this.defaultStrength));
			}
			return this.engineGenerateKey();
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000B5170 File Offset: 0x000B3370
		protected virtual byte[] engineGenerateKey()
		{
			return SecureRandom.GetNextBytes(this.random, this.strength);
		}

		// Token: 0x04001688 RID: 5768
		protected internal SecureRandom random;

		// Token: 0x04001689 RID: 5769
		protected internal int strength;

		// Token: 0x0400168A RID: 5770
		private bool uninitialised = true;

		// Token: 0x0400168B RID: 5771
		private int defaultStrength;
	}
}
