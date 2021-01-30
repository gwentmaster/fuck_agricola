using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000378 RID: 888
	public interface IBufferedCipher
	{
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060021E7 RID: 8679
		string AlgorithmName { get; }

		// Token: 0x060021E8 RID: 8680
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x060021E9 RID: 8681
		int GetBlockSize();

		// Token: 0x060021EA RID: 8682
		int GetOutputSize(int inputLen);

		// Token: 0x060021EB RID: 8683
		int GetUpdateOutputSize(int inputLen);

		// Token: 0x060021EC RID: 8684
		byte[] ProcessByte(byte input);

		// Token: 0x060021ED RID: 8685
		int ProcessByte(byte input, byte[] output, int outOff);

		// Token: 0x060021EE RID: 8686
		byte[] ProcessBytes(byte[] input);

		// Token: 0x060021EF RID: 8687
		byte[] ProcessBytes(byte[] input, int inOff, int length);

		// Token: 0x060021F0 RID: 8688
		int ProcessBytes(byte[] input, byte[] output, int outOff);

		// Token: 0x060021F1 RID: 8689
		int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);

		// Token: 0x060021F2 RID: 8690
		byte[] DoFinal();

		// Token: 0x060021F3 RID: 8691
		byte[] DoFinal(byte[] input);

		// Token: 0x060021F4 RID: 8692
		byte[] DoFinal(byte[] input, int inOff, int length);

		// Token: 0x060021F5 RID: 8693
		int DoFinal(byte[] output, int outOff);

		// Token: 0x060021F6 RID: 8694
		int DoFinal(byte[] input, byte[] output, int outOff);

		// Token: 0x060021F7 RID: 8695
		int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff);

		// Token: 0x060021F8 RID: 8696
		void Reset();
	}
}
