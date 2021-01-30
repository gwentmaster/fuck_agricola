using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000373 RID: 883
	public interface IAsymmetricBlockCipher
	{
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060021D5 RID: 8661
		string AlgorithmName { get; }

		// Token: 0x060021D6 RID: 8662
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x060021D7 RID: 8663
		int GetInputBlockSize();

		// Token: 0x060021D8 RID: 8664
		int GetOutputBlockSize();

		// Token: 0x060021D9 RID: 8665
		byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen);
	}
}
