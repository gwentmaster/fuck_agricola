using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000531 RID: 1329
	public class CrlEntry : Asn1Encodable
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x000F7ECC File Offset: 0x000F60CC
		public CrlEntry(Asn1Sequence seq)
		{
			if (seq.Count < 2 || seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.seq = seq;
			this.userCertificate = DerInteger.GetInstance(seq[0]);
			this.revocationDate = Time.GetInstance(seq[1]);
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x000F7F37 File Offset: 0x000F6137
		public DerInteger UserCertificate
		{
			get
			{
				return this.userCertificate;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x000F7F3F File Offset: 0x000F613F
		public Time RevocationDate
		{
			get
			{
				return this.revocationDate;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06003077 RID: 12407 RVA: 0x000F7F47 File Offset: 0x000F6147
		public X509Extensions Extensions
		{
			get
			{
				if (this.crlEntryExtensions == null && this.seq.Count == 3)
				{
					this.crlEntryExtensions = X509Extensions.GetInstance(this.seq[2]);
				}
				return this.crlEntryExtensions;
			}
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000F7F7C File Offset: 0x000F617C
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x04001EFA RID: 7930
		internal Asn1Sequence seq;

		// Token: 0x04001EFB RID: 7931
		internal DerInteger userCertificate;

		// Token: 0x04001EFC RID: 7932
		internal Time revocationDate;

		// Token: 0x04001EFD RID: 7933
		internal X509Extensions crlEntryExtensions;
	}
}
