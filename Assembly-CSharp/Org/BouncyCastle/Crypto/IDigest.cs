using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200037D RID: 893
	public interface IDigest
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06002200 RID: 8704
		string AlgorithmName { get; }

		// Token: 0x06002201 RID: 8705
		int GetDigestSize();

		// Token: 0x06002202 RID: 8706
		int GetByteLength();

		// Token: 0x06002203 RID: 8707
		void Update(byte input);

		// Token: 0x06002204 RID: 8708
		void BlockUpdate(byte[] input, int inOff, int length);

		// Token: 0x06002205 RID: 8709
		int DoFinal(byte[] output, int outOff);

		// Token: 0x06002206 RID: 8710
		void Reset();
	}
}
