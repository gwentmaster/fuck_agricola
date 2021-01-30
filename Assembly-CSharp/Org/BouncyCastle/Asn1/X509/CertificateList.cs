using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000525 RID: 1317
	public class CertificateList : Asn1Encodable
	{
		// Token: 0x06003008 RID: 12296 RVA: 0x000F686F File Offset: 0x000F4A6F
		public static CertificateList GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return CertificateList.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000F687D File Offset: 0x000F4A7D
		public static CertificateList GetInstance(object obj)
		{
			if (obj is CertificateList)
			{
				return (CertificateList)obj;
			}
			if (obj != null)
			{
				return new CertificateList(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000F68A0 File Offset: 0x000F4AA0
		private CertificateList(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("sequence wrong size for CertificateList", "seq");
			}
			this.tbsCertList = TbsCertificateList.GetInstance(seq[0]);
			this.sigAlgID = AlgorithmIdentifier.GetInstance(seq[1]);
			this.sig = DerBitString.GetInstance(seq[2]);
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x000F6902 File Offset: 0x000F4B02
		public TbsCertificateList TbsCertList
		{
			get
			{
				return this.tbsCertList;
			}
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x000F690A File Offset: 0x000F4B0A
		public CrlEntry[] GetRevokedCertificates()
		{
			return this.tbsCertList.GetRevokedCertificates();
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000F6917 File Offset: 0x000F4B17
		public IEnumerable GetRevokedCertificateEnumeration()
		{
			return this.tbsCertList.GetRevokedCertificateEnumeration();
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x000F6924 File Offset: 0x000F4B24
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.sigAlgID;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600300F RID: 12303 RVA: 0x000F692C File Offset: 0x000F4B2C
		public DerBitString Signature
		{
			get
			{
				return this.sig;
			}
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000F6934 File Offset: 0x000F4B34
		public byte[] GetSignatureOctets()
		{
			return this.sig.GetOctets();
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x000F6941 File Offset: 0x000F4B41
		public int Version
		{
			get
			{
				return this.tbsCertList.Version;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x000F694E File Offset: 0x000F4B4E
		public X509Name Issuer
		{
			get
			{
				return this.tbsCertList.Issuer;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x000F695B File Offset: 0x000F4B5B
		public Time ThisUpdate
		{
			get
			{
				return this.tbsCertList.ThisUpdate;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000F6968 File Offset: 0x000F4B68
		public Time NextUpdate
		{
			get
			{
				return this.tbsCertList.NextUpdate;
			}
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x000F6975 File Offset: 0x000F4B75
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.tbsCertList,
				this.sigAlgID,
				this.sig
			});
		}

		// Token: 0x04001EC2 RID: 7874
		private readonly TbsCertificateList tbsCertList;

		// Token: 0x04001EC3 RID: 7875
		private readonly AlgorithmIdentifier sigAlgID;

		// Token: 0x04001EC4 RID: 7876
		private readonly DerBitString sig;
	}
}
