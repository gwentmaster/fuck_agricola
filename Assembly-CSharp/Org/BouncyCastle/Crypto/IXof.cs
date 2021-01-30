using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000388 RID: 904
	public interface IXof : IDigest
	{
		// Token: 0x0600222A RID: 8746
		int DoFinal(byte[] output, int outOff, int outLen);

		// Token: 0x0600222B RID: 8747
		int DoOutput(byte[] output, int outOff, int outLen);
	}
}
