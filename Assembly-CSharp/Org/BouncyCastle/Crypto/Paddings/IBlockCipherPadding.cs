using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x02000448 RID: 1096
	public interface IBlockCipherPadding
	{
		// Token: 0x060027FE RID: 10238
		void Init(SecureRandom random);

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060027FF RID: 10239
		string PaddingName { get; }

		// Token: 0x06002800 RID: 10240
		int AddPadding(byte[] input, int inOff);

		// Token: 0x06002801 RID: 10241
		int PadCount(byte[] input);
	}
}
