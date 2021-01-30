using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200040F RID: 1039
	public interface IDsaKCalculator
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06002697 RID: 9879
		bool IsDeterministic { get; }

		// Token: 0x06002698 RID: 9880
		void Init(BigInteger n, SecureRandom random);

		// Token: 0x06002699 RID: 9881
		void Init(BigInteger n, BigInteger d, byte[] message);

		// Token: 0x0600269A RID: 9882
		BigInteger NextK();
	}
}
