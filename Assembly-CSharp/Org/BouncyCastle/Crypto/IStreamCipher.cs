using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000383 RID: 899
	public interface IStreamCipher
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x0600221C RID: 8732
		string AlgorithmName { get; }

		// Token: 0x0600221D RID: 8733
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x0600221E RID: 8734
		byte ReturnByte(byte input);

		// Token: 0x0600221F RID: 8735
		void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);

		// Token: 0x06002220 RID: 8736
		void Reset();
	}
}
