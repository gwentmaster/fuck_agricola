using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004FB RID: 1275
	public class DerOctetString : Asn1OctetString
	{
		// Token: 0x06002EFE RID: 12030 RVA: 0x000F38A1 File Offset: 0x000F1AA1
		public DerOctetString(byte[] str) : base(str)
		{
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000F38AA File Offset: 0x000F1AAA
		public DerOctetString(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000F38B3 File Offset: 0x000F1AB3
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(4, this.str);
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000F38C2 File Offset: 0x000F1AC2
		internal static void Encode(DerOutputStream derOut, byte[] bytes, int offset, int length)
		{
			derOut.WriteEncoded(4, bytes, offset, length);
		}
	}
}
