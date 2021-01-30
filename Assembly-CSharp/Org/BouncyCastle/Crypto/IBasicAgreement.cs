using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000375 RID: 885
	public interface IBasicAgreement
	{
		// Token: 0x060021DC RID: 8668
		void Init(ICipherParameters parameters);

		// Token: 0x060021DD RID: 8669
		int GetFieldSize();

		// Token: 0x060021DE RID: 8670
		BigInteger CalculateAgreement(ICipherParameters pubKey);
	}
}
