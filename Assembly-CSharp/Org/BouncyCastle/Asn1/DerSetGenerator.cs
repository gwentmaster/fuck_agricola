using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004EB RID: 1259
	public class DerSetGenerator : DerGenerator
	{
		// Token: 0x06002E56 RID: 11862 RVA: 0x000F19CA File Offset: 0x000EFBCA
		public DerSetGenerator(Stream outStream) : base(outStream)
		{
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000F19DE File Offset: 0x000EFBDE
		public DerSetGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000F19F4 File Offset: 0x000EFBF4
		public override void AddObject(Asn1Encodable obj)
		{
			new DerOutputStream(this._bOut).WriteObject(obj);
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000F1A07 File Offset: 0x000EFC07
		public override Stream GetRawOutputStream()
		{
			return this._bOut;
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000F1A0F File Offset: 0x000EFC0F
		public override void Close()
		{
			base.WriteDerEncoded(49, this._bOut.ToArray());
		}

		// Token: 0x04001E25 RID: 7717
		private readonly MemoryStream _bOut = new MemoryStream();
	}
}
