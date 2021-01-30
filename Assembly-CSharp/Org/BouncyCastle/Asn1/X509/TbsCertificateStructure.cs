using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000533 RID: 1331
	public class TbsCertificateStructure : Asn1Encodable
	{
		// Token: 0x06003086 RID: 12422 RVA: 0x000F81F8 File Offset: 0x000F63F8
		public static TbsCertificateStructure GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return TbsCertificateStructure.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x000F8206 File Offset: 0x000F6406
		public static TbsCertificateStructure GetInstance(object obj)
		{
			if (obj is TbsCertificateStructure)
			{
				return (TbsCertificateStructure)obj;
			}
			if (obj != null)
			{
				return new TbsCertificateStructure(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000F8228 File Offset: 0x000F6428
		internal TbsCertificateStructure(Asn1Sequence seq)
		{
			int num = 0;
			this.seq = seq;
			if (seq[0] is DerTaggedObject)
			{
				this.version = DerInteger.GetInstance((Asn1TaggedObject)seq[0], true);
			}
			else
			{
				num = -1;
				this.version = new DerInteger(0);
			}
			this.serialNumber = DerInteger.GetInstance(seq[num + 1]);
			this.signature = AlgorithmIdentifier.GetInstance(seq[num + 2]);
			this.issuer = X509Name.GetInstance(seq[num + 3]);
			Asn1Sequence asn1Sequence = (Asn1Sequence)seq[num + 4];
			this.startDate = Time.GetInstance(asn1Sequence[0]);
			this.endDate = Time.GetInstance(asn1Sequence[1]);
			this.subject = X509Name.GetInstance(seq[num + 5]);
			this.subjectPublicKeyInfo = SubjectPublicKeyInfo.GetInstance(seq[num + 6]);
			for (int i = seq.Count - (num + 6) - 1; i > 0; i--)
			{
				DerTaggedObject derTaggedObject = (DerTaggedObject)seq[num + 6 + i];
				switch (derTaggedObject.TagNo)
				{
				case 1:
					this.issuerUniqueID = DerBitString.GetInstance(derTaggedObject, false);
					break;
				case 2:
					this.subjectUniqueID = DerBitString.GetInstance(derTaggedObject, false);
					break;
				case 3:
					this.extensions = X509Extensions.GetInstance(derTaggedObject);
					break;
				}
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06003089 RID: 12425 RVA: 0x000F8382 File Offset: 0x000F6582
		public int Version
		{
			get
			{
				return this.version.Value.IntValue + 1;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x000F8396 File Offset: 0x000F6596
		public DerInteger VersionNumber
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x0600308B RID: 12427 RVA: 0x000F839E File Offset: 0x000F659E
		public DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x000F83A6 File Offset: 0x000F65A6
		public AlgorithmIdentifier Signature
		{
			get
			{
				return this.signature;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x0600308D RID: 12429 RVA: 0x000F83AE File Offset: 0x000F65AE
		public X509Name Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000F83B6 File Offset: 0x000F65B6
		public Time StartDate
		{
			get
			{
				return this.startDate;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x000F83BE File Offset: 0x000F65BE
		public Time EndDate
		{
			get
			{
				return this.endDate;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x000F83C6 File Offset: 0x000F65C6
		public X509Name Subject
		{
			get
			{
				return this.subject;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x000F83CE File Offset: 0x000F65CE
		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return this.subjectPublicKeyInfo;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x000F83D6 File Offset: 0x000F65D6
		public DerBitString IssuerUniqueID
		{
			get
			{
				return this.issuerUniqueID;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x000F83DE File Offset: 0x000F65DE
		public DerBitString SubjectUniqueID
		{
			get
			{
				return this.subjectUniqueID;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06003094 RID: 12436 RVA: 0x000F83E6 File Offset: 0x000F65E6
		public X509Extensions Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000F83EE File Offset: 0x000F65EE
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x04001F06 RID: 7942
		internal Asn1Sequence seq;

		// Token: 0x04001F07 RID: 7943
		internal DerInteger version;

		// Token: 0x04001F08 RID: 7944
		internal DerInteger serialNumber;

		// Token: 0x04001F09 RID: 7945
		internal AlgorithmIdentifier signature;

		// Token: 0x04001F0A RID: 7946
		internal X509Name issuer;

		// Token: 0x04001F0B RID: 7947
		internal Time startDate;

		// Token: 0x04001F0C RID: 7948
		internal Time endDate;

		// Token: 0x04001F0D RID: 7949
		internal X509Name subject;

		// Token: 0x04001F0E RID: 7950
		internal SubjectPublicKeyInfo subjectPublicKeyInfo;

		// Token: 0x04001F0F RID: 7951
		internal DerBitString issuerUniqueID;

		// Token: 0x04001F10 RID: 7952
		internal DerBitString subjectUniqueID;

		// Token: 0x04001F11 RID: 7953
		internal X509Extensions extensions;
	}
}
