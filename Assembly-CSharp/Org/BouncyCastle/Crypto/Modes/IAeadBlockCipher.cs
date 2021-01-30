using System;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000460 RID: 1120
	public interface IAeadBlockCipher
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060028B9 RID: 10425
		string AlgorithmName { get; }

		// Token: 0x060028BA RID: 10426
		IBlockCipher GetUnderlyingCipher();

		// Token: 0x060028BB RID: 10427
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x060028BC RID: 10428
		int GetBlockSize();

		// Token: 0x060028BD RID: 10429
		void ProcessAadByte(byte input);

		// Token: 0x060028BE RID: 10430
		void ProcessAadBytes(byte[] inBytes, int inOff, int len);

		// Token: 0x060028BF RID: 10431
		int ProcessByte(byte input, byte[] outBytes, int outOff);

		// Token: 0x060028C0 RID: 10432
		int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff);

		// Token: 0x060028C1 RID: 10433
		int DoFinal(byte[] outBytes, int outOff);

		// Token: 0x060028C2 RID: 10434
		byte[] GetMac();

		// Token: 0x060028C3 RID: 10435
		int GetUpdateOutputSize(int len);

		// Token: 0x060028C4 RID: 10436
		int GetOutputSize(int len);

		// Token: 0x060028C5 RID: 10437
		void Reset();
	}
}
