using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200052F RID: 1327
	public class ReasonFlags : DerBitString
	{
		// Token: 0x06003069 RID: 12393 RVA: 0x000F0A6C File Offset: 0x000EEC6C
		public ReasonFlags(int reasons) : base(reasons)
		{
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000F7BE9 File Offset: 0x000F5DE9
		public ReasonFlags(DerBitString reasons) : base(reasons.GetBytes(), reasons.PadBits)
		{
		}

		// Token: 0x04001EEF RID: 7919
		public const int Unused = 128;

		// Token: 0x04001EF0 RID: 7920
		public const int KeyCompromise = 64;

		// Token: 0x04001EF1 RID: 7921
		public const int CACompromise = 32;

		// Token: 0x04001EF2 RID: 7922
		public const int AffiliationChanged = 16;

		// Token: 0x04001EF3 RID: 7923
		public const int Superseded = 8;

		// Token: 0x04001EF4 RID: 7924
		public const int CessationOfOperation = 4;

		// Token: 0x04001EF5 RID: 7925
		public const int CertificateHold = 2;

		// Token: 0x04001EF6 RID: 7926
		public const int PrivilegeWithdrawn = 1;

		// Token: 0x04001EF7 RID: 7927
		public const int AACompromise = 32768;
	}
}
