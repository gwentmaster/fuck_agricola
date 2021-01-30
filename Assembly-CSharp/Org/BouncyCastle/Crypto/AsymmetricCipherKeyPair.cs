using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000367 RID: 871
	public class AsymmetricCipherKeyPair
	{
		// Token: 0x06002167 RID: 8551 RVA: 0x000B4458 File Offset: 0x000B2658
		public AsymmetricCipherKeyPair(AsymmetricKeyParameter publicParameter, AsymmetricKeyParameter privateParameter)
		{
			if (publicParameter.IsPrivate)
			{
				throw new ArgumentException("Expected a public key", "publicParameter");
			}
			if (!privateParameter.IsPrivate)
			{
				throw new ArgumentException("Expected a private key", "privateParameter");
			}
			this.publicParameter = publicParameter;
			this.privateParameter = privateParameter;
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000B44A9 File Offset: 0x000B26A9
		public AsymmetricKeyParameter Public
		{
			get
			{
				return this.publicParameter;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06002169 RID: 8553 RVA: 0x000B44B1 File Offset: 0x000B26B1
		public AsymmetricKeyParameter Private
		{
			get
			{
				return this.privateParameter;
			}
		}

		// Token: 0x04001678 RID: 5752
		private readonly AsymmetricKeyParameter publicParameter;

		// Token: 0x04001679 RID: 5753
		private readonly AsymmetricKeyParameter privateParameter;
	}
}
