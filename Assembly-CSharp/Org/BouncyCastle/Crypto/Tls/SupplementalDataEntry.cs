using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D7 RID: 983
	public class SupplementalDataEntry
	{
		// Token: 0x0600242A RID: 9258 RVA: 0x000B8EAF File Offset: 0x000B70AF
		public SupplementalDataEntry(int dataType, byte[] data)
		{
			this.mDataType = dataType;
			this.mData = data;
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x000B8EC5 File Offset: 0x000B70C5
		public virtual int DataType
		{
			get
			{
				return this.mDataType;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x000B8ECD File Offset: 0x000B70CD
		public virtual byte[] Data
		{
			get
			{
				return this.mData;
			}
		}

		// Token: 0x0400191D RID: 6429
		protected readonly int mDataType;

		// Token: 0x0400191E RID: 6430
		protected readonly byte[] mData;
	}
}
