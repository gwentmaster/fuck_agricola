using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200037E RID: 894
	public interface IMac
	{
		// Token: 0x06002207 RID: 8711
		void Init(ICipherParameters parameters);

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06002208 RID: 8712
		string AlgorithmName { get; }

		// Token: 0x06002209 RID: 8713
		int GetMacSize();

		// Token: 0x0600220A RID: 8714
		void Update(byte input);

		// Token: 0x0600220B RID: 8715
		void BlockUpdate(byte[] input, int inOff, int len);

		// Token: 0x0600220C RID: 8716
		int DoFinal(byte[] output, int outOff);

		// Token: 0x0600220D RID: 8717
		void Reset();
	}
}
