using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004DF RID: 1247
	public class BerApplicationSpecificParser : IAsn1ApplicationSpecificParser, IAsn1Convertible
	{
		// Token: 0x06002E0F RID: 11791 RVA: 0x000F0DAA File Offset: 0x000EEFAA
		internal BerApplicationSpecificParser(int tag, Asn1StreamParser parser)
		{
			this.tag = tag;
			this.parser = parser;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000F0DC0 File Offset: 0x000EEFC0
		public IAsn1Convertible ReadObject()
		{
			return this.parser.ReadObject();
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000F0DCD File Offset: 0x000EEFCD
		public Asn1Object ToAsn1Object()
		{
			return new BerApplicationSpecific(this.tag, this.parser.ReadVector());
		}

		// Token: 0x04001E11 RID: 7697
		private readonly int tag;

		// Token: 0x04001E12 RID: 7698
		private readonly Asn1StreamParser parser;
	}
}
