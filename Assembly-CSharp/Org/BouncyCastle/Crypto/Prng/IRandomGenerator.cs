using System;

namespace Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x02000418 RID: 1048
	public interface IRandomGenerator
	{
		// Token: 0x060026EF RID: 9967
		void AddSeedMaterial(byte[] seed);

		// Token: 0x060026F0 RID: 9968
		void AddSeedMaterial(long seed);

		// Token: 0x060026F1 RID: 9969
		void NextBytes(byte[] bytes);

		// Token: 0x060026F2 RID: 9970
		void NextBytes(byte[] bytes, int start, int len);
	}
}
