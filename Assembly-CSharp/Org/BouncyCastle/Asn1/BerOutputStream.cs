using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E1 RID: 1249
	public class BerOutputStream : DerOutputStream
	{
		// Token: 0x06002E1D RID: 11805 RVA: 0x000F0111 File Offset: 0x000EE311
		public BerOutputStream(Stream os) : base(os)
		{
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000F1010 File Offset: 0x000EF210
		[Obsolete("Use version taking an Asn1Encodable arg instead")]
		public override void WriteObject(object obj)
		{
			if (obj == null)
			{
				base.WriteNull();
				return;
			}
			if (obj is Asn1Object)
			{
				((Asn1Object)obj).Encode(this);
				return;
			}
			if (obj is Asn1Encodable)
			{
				((Asn1Encodable)obj).ToAsn1Object().Encode(this);
				return;
			}
			throw new IOException("object not BerEncodable");
		}
	}
}
