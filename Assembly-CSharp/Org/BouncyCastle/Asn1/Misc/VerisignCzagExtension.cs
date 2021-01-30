using System;

namespace Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x02000553 RID: 1363
	public class VerisignCzagExtension : DerIA5String
	{
		// Token: 0x06003177 RID: 12663 RVA: 0x000FDCF8 File Offset: 0x000FBEF8
		public VerisignCzagExtension(DerIA5String str) : base(str.GetString())
		{
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x000FDD18 File Offset: 0x000FBF18
		public override string ToString()
		{
			return "VerisignCzagExtension: " + this.GetString();
		}
	}
}
