using System;

namespace Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x02000552 RID: 1362
	public class NetscapeRevocationUrl : DerIA5String
	{
		// Token: 0x06003175 RID: 12661 RVA: 0x000FDCF8 File Offset: 0x000FBEF8
		public NetscapeRevocationUrl(DerIA5String str) : base(str.GetString())
		{
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x000FDD06 File Offset: 0x000FBF06
		public override string ToString()
		{
			return "NetscapeRevocationUrl: " + this.GetString();
		}
	}
}
