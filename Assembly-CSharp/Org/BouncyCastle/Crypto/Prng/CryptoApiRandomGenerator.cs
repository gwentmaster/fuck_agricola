using System;
using System.Security.Cryptography;

namespace Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x02000416 RID: 1046
	public class CryptoApiRandomGenerator : IRandomGenerator
	{
		// Token: 0x060026DF RID: 9951 RVA: 0x000C3F98 File Offset: 0x000C2198
		public CryptoApiRandomGenerator() : this(new RNGCryptoServiceProvider())
		{
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x000C3FA5 File Offset: 0x000C21A5
		public CryptoApiRandomGenerator(RandomNumberGenerator rng)
		{
			this.rndProv = rng;
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void AddSeedMaterial(byte[] seed)
		{
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void AddSeedMaterial(long seed)
		{
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000C3FB4 File Offset: 0x000C21B4
		public virtual void NextBytes(byte[] bytes)
		{
			this.rndProv.GetBytes(bytes);
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000C3FC4 File Offset: 0x000C21C4
		public virtual void NextBytes(byte[] bytes, int start, int len)
		{
			if (start < 0)
			{
				throw new ArgumentException("Start offset cannot be negative", "start");
			}
			if (bytes.Length < start + len)
			{
				throw new ArgumentException("Byte array too small for requested offset and length");
			}
			if (bytes.Length == len && start == 0)
			{
				this.NextBytes(bytes);
				return;
			}
			byte[] array = new byte[len];
			this.NextBytes(array);
			Array.Copy(array, 0, bytes, start, len);
		}

		// Token: 0x04001A09 RID: 6665
		private readonly RandomNumberGenerator rndProv;
	}
}
