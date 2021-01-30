using System;
using System.IO;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000382 RID: 898
	public interface IStreamCalculator
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x0600221A RID: 8730
		Stream Stream { get; }

		// Token: 0x0600221B RID: 8731
		object GetResult();
	}
}
