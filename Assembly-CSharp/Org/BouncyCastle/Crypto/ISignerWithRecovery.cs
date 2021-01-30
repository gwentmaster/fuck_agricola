using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000381 RID: 897
	public interface ISignerWithRecovery : ISigner
	{
		// Token: 0x06002217 RID: 8727
		bool HasFullMessage();

		// Token: 0x06002218 RID: 8728
		byte[] GetRecoveredMessage();

		// Token: 0x06002219 RID: 8729
		void UpdateWithRecoveredMessage(byte[] signature);
	}
}
