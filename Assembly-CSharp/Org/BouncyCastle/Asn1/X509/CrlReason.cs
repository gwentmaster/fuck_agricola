using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000524 RID: 1316
	public class CrlReason : DerEnumerated
	{
		// Token: 0x06003004 RID: 12292 RVA: 0x000F67A1 File Offset: 0x000F49A1
		public CrlReason(int reason) : base(reason)
		{
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000F67AA File Offset: 0x000F49AA
		public CrlReason(DerEnumerated reason) : base(reason.Value.IntValue)
		{
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x000F67C0 File Offset: 0x000F49C0
		public override string ToString()
		{
			int intValue = base.Value.IntValue;
			string str = (intValue < 0 || intValue > 10) ? "Invalid" : CrlReason.ReasonString[intValue];
			return "CrlReason: " + str;
		}

		// Token: 0x04001EB7 RID: 7863
		public const int Unspecified = 0;

		// Token: 0x04001EB8 RID: 7864
		public const int KeyCompromise = 1;

		// Token: 0x04001EB9 RID: 7865
		public const int CACompromise = 2;

		// Token: 0x04001EBA RID: 7866
		public const int AffiliationChanged = 3;

		// Token: 0x04001EBB RID: 7867
		public const int Superseded = 4;

		// Token: 0x04001EBC RID: 7868
		public const int CessationOfOperation = 5;

		// Token: 0x04001EBD RID: 7869
		public const int CertificateHold = 6;

		// Token: 0x04001EBE RID: 7870
		public const int RemoveFromCrl = 8;

		// Token: 0x04001EBF RID: 7871
		public const int PrivilegeWithdrawn = 9;

		// Token: 0x04001EC0 RID: 7872
		public const int AACompromise = 10;

		// Token: 0x04001EC1 RID: 7873
		private static readonly string[] ReasonString = new string[]
		{
			"Unspecified",
			"KeyCompromise",
			"CACompromise",
			"AffiliationChanged",
			"Superseded",
			"CessationOfOperation",
			"CertificateHold",
			"Unknown",
			"RemoveFromCrl",
			"PrivilegeWithdrawn",
			"AACompromise"
		};
	}
}
