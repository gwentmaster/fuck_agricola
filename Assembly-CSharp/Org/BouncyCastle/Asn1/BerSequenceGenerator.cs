using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D9 RID: 1241
	public class BerSequenceGenerator : BerGenerator
	{
		// Token: 0x06002DFE RID: 11774 RVA: 0x000F0C3C File Offset: 0x000EEE3C
		public BerSequenceGenerator(Stream outStream) : base(outStream)
		{
			base.WriteBerHeader(48);
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000F0C4D File Offset: 0x000EEE4D
		public BerSequenceGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
			base.WriteBerHeader(48);
		}
	}
}
