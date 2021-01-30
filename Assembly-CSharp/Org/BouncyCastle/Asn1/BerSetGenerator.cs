using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004DB RID: 1243
	public class BerSetGenerator : BerGenerator
	{
		// Token: 0x06002E03 RID: 11779 RVA: 0x000F0C8E File Offset: 0x000EEE8E
		public BerSetGenerator(Stream outStream) : base(outStream)
		{
			base.WriteBerHeader(49);
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000F0C9F File Offset: 0x000EEE9F
		public BerSetGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
			base.WriteBerHeader(49);
		}
	}
}
