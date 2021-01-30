using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200043A RID: 1082
	public class KdfParameters : IDerivationParameters
	{
		// Token: 0x060027C2 RID: 10178 RVA: 0x000C5A7D File Offset: 0x000C3C7D
		public KdfParameters(byte[] shared, byte[] iv)
		{
			this.shared = shared;
			this.iv = iv;
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000C5A93 File Offset: 0x000C3C93
		public byte[] GetSharedSecret()
		{
			return this.shared;
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000C5A9B File Offset: 0x000C3C9B
		public byte[] GetIV()
		{
			return this.iv;
		}

		// Token: 0x04001A57 RID: 6743
		private byte[] iv;

		// Token: 0x04001A58 RID: 6744
		private byte[] shared;
	}
}
