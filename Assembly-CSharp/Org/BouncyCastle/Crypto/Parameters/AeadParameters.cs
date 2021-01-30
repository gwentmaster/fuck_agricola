using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000419 RID: 1049
	public class AeadParameters : ICipherParameters
	{
		// Token: 0x060026F3 RID: 9971 RVA: 0x000C4285 File Offset: 0x000C2485
		public AeadParameters(KeyParameter key, int macSize, byte[] nonce) : this(key, macSize, nonce, null)
		{
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000C4291 File Offset: 0x000C2491
		public AeadParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText)
		{
			this.key = key;
			this.nonce = nonce;
			this.macSize = macSize;
			this.associatedText = associatedText;
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x000C42B6 File Offset: 0x000C24B6
		public virtual KeyParameter Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x000C42BE File Offset: 0x000C24BE
		public virtual int MacSize
		{
			get
			{
				return this.macSize;
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000C42C6 File Offset: 0x000C24C6
		public virtual byte[] GetAssociatedText()
		{
			return this.associatedText;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000C42CE File Offset: 0x000C24CE
		public virtual byte[] GetNonce()
		{
			return this.nonce;
		}

		// Token: 0x04001A10 RID: 6672
		private readonly byte[] associatedText;

		// Token: 0x04001A11 RID: 6673
		private readonly byte[] nonce;

		// Token: 0x04001A12 RID: 6674
		private readonly KeyParameter key;

		// Token: 0x04001A13 RID: 6675
		private readonly int macSize;
	}
}
