using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000439 RID: 1081
	public class IesWithCipherParameters : IesParameters
	{
		// Token: 0x060027C0 RID: 10176 RVA: 0x000C5A62 File Offset: 0x000C3C62
		public IesWithCipherParameters(byte[] derivation, byte[] encoding, int macKeySize, int cipherKeySize) : base(derivation, encoding, macKeySize)
		{
			this.cipherKeySize = cipherKeySize;
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x000C5A75 File Offset: 0x000C3C75
		public int CipherKeySize
		{
			get
			{
				return this.cipherKeySize;
			}
		}

		// Token: 0x04001A56 RID: 6742
		private int cipherKeySize;
	}
}
