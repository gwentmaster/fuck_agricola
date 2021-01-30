using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200037B RID: 891
	public interface IDerivationFunction
	{
		// Token: 0x060021FD RID: 8701
		void Init(IDerivationParameters parameters);

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060021FE RID: 8702
		IDigest Digest { get; }

		// Token: 0x060021FF RID: 8703
		int GenerateBytes(byte[] output, int outOff, int length);
	}
}
