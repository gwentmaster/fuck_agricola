using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F8 RID: 1272
	public class DerNull : Asn1Null
	{
		// Token: 0x06002ED9 RID: 11993 RVA: 0x000F3197 File Offset: 0x000F1397
		[Obsolete("Use static Instance object")]
		public DerNull()
		{
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x000F3197 File Offset: 0x000F1397
		protected internal DerNull(int dummy)
		{
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000F31AB File Offset: 0x000F13AB
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(5, this.zeroBytes);
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000F31BA File Offset: 0x000F13BA
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			return asn1Object is DerNull;
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x00074B9B File Offset: 0x00072D9B
		protected override int Asn1GetHashCode()
		{
			return -1;
		}

		// Token: 0x04001E3B RID: 7739
		public static readonly DerNull Instance = new DerNull(0);

		// Token: 0x04001E3C RID: 7740
		private byte[] zeroBytes = new byte[0];
	}
}
