using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000368 RID: 872
	public abstract class AsymmetricKeyParameter : ICipherParameters
	{
		// Token: 0x0600216A RID: 8554 RVA: 0x000B44B9 File Offset: 0x000B26B9
		protected AsymmetricKeyParameter(bool privateKey)
		{
			this.privateKey = privateKey;
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x000B44C8 File Offset: 0x000B26C8
		public bool IsPrivate
		{
			get
			{
				return this.privateKey;
			}
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000B44D0 File Offset: 0x000B26D0
		public override bool Equals(object obj)
		{
			AsymmetricKeyParameter asymmetricKeyParameter = obj as AsymmetricKeyParameter;
			return asymmetricKeyParameter != null && this.Equals(asymmetricKeyParameter);
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000B44F0 File Offset: 0x000B26F0
		protected bool Equals(AsymmetricKeyParameter other)
		{
			return this.privateKey == other.privateKey;
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x000B4500 File Offset: 0x000B2700
		public override int GetHashCode()
		{
			return this.privateKey.GetHashCode();
		}

		// Token: 0x0400167A RID: 5754
		private readonly bool privateKey;
	}
}
