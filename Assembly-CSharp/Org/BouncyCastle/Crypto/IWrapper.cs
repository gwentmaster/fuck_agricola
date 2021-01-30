using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000387 RID: 903
	public interface IWrapper
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06002226 RID: 8742
		string AlgorithmName { get; }

		// Token: 0x06002227 RID: 8743
		void Init(bool forWrapping, ICipherParameters parameters);

		// Token: 0x06002228 RID: 8744
		byte[] Wrap(byte[] input, int inOff, int length);

		// Token: 0x06002229 RID: 8745
		byte[] Unwrap(byte[] input, int inOff, int length);
	}
}
