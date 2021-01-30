using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000376 RID: 886
	public interface IBlockCipher
	{
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060021DF RID: 8671
		string AlgorithmName { get; }

		// Token: 0x060021E0 RID: 8672
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x060021E1 RID: 8673
		int GetBlockSize();

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060021E2 RID: 8674
		bool IsPartialBlockOkay { get; }

		// Token: 0x060021E3 RID: 8675
		int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff);

		// Token: 0x060021E4 RID: 8676
		void Reset();
	}
}
