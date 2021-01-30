using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D0 RID: 1232
	public class Asn1OutputStream : DerOutputStream
	{
		// Token: 0x06002DBA RID: 11706 RVA: 0x000F0111 File Offset: 0x000EE311
		public Asn1OutputStream(Stream os) : base(os)
		{
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000F011C File Offset: 0x000EE31C
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
			throw new IOException("object not Asn1Encodable");
		}
	}
}
