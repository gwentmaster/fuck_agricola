using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000502 RID: 1282
	public class DerTaggedObject : Asn1TaggedObject
	{
		// Token: 0x06002F33 RID: 12083 RVA: 0x000F400E File Offset: 0x000F220E
		public DerTaggedObject(int tagNo, Asn1Encodable obj) : base(tagNo, obj)
		{
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000F4018 File Offset: 0x000F2218
		public DerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj) : base(explicitly, tagNo, obj)
		{
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000F4023 File Offset: 0x000F2223
		public DerTaggedObject(int tagNo) : base(false, tagNo, DerSequence.Empty)
		{
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000F4034 File Offset: 0x000F2234
		internal override void Encode(DerOutputStream derOut)
		{
			if (base.IsEmpty())
			{
				derOut.WriteEncoded(160, this.tagNo, new byte[0]);
				return;
			}
			byte[] derEncoded = this.obj.GetDerEncoded();
			if (this.explicitly)
			{
				derOut.WriteEncoded(160, this.tagNo, derEncoded);
				return;
			}
			int flags = (int)((derEncoded[0] & 32) | 128);
			derOut.WriteTag(flags, this.tagNo);
			derOut.Write(derEncoded, 1, derEncoded.Length - 1);
		}
	}
}
