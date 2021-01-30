using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000380 RID: 896
	public interface ISigner
	{
		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06002210 RID: 8720
		string AlgorithmName { get; }

		// Token: 0x06002211 RID: 8721
		void Init(bool forSigning, ICipherParameters parameters);

		// Token: 0x06002212 RID: 8722
		void Update(byte input);

		// Token: 0x06002213 RID: 8723
		void BlockUpdate(byte[] input, int inOff, int length);

		// Token: 0x06002214 RID: 8724
		byte[] GenerateSignature();

		// Token: 0x06002215 RID: 8725
		bool VerifySignature(byte[] signature);

		// Token: 0x06002216 RID: 8726
		void Reset();
	}
}
