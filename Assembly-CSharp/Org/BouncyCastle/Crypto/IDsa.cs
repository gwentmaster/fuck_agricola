using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200037A RID: 890
	public interface IDsa
	{
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060021F9 RID: 8697
		string AlgorithmName { get; }

		// Token: 0x060021FA RID: 8698
		void Init(bool forSigning, ICipherParameters parameters);

		// Token: 0x060021FB RID: 8699
		BigInteger[] GenerateSignature(byte[] message);

		// Token: 0x060021FC RID: 8700
		bool VerifySignature(byte[] message, BigInteger r, BigInteger s);
	}
}
