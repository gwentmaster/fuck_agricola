using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000377 RID: 887
	public interface IBlockResult
	{
		// Token: 0x060021E5 RID: 8677
		byte[] Collect();

		// Token: 0x060021E6 RID: 8678
		int Collect(byte[] destination, int offset);
	}
}
