using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000384 RID: 900
	public interface IVerifier
	{
		// Token: 0x06002221 RID: 8737
		bool IsVerified(byte[] data);

		// Token: 0x06002222 RID: 8738
		bool IsVerified(byte[] source, int off, int length);
	}
}
