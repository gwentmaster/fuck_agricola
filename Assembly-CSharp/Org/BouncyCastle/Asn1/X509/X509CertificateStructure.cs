using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000535 RID: 1333
	public class X509CertificateStructure : Asn1Encodable
	{
		// Token: 0x0600309E RID: 12446 RVA: 0x000F85B4 File Offset: 0x000F67B4
		public static X509CertificateStructure GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return X509CertificateStructure.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000F85C2 File Offset: 0x000F67C2
		public static X509CertificateStructure GetInstance(object obj)
		{
			if (obj is X509CertificateStructure)
			{
				return (X509CertificateStructure)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new X509CertificateStructure(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000F85E4 File Offset: 0x000F67E4
		public X509CertificateStructure(TbsCertificateStructure tbsCert, AlgorithmIdentifier sigAlgID, DerBitString sig)
		{
			if (tbsCert == null)
			{
				throw new ArgumentNullException("tbsCert");
			}
			if (sigAlgID == null)
			{
				throw new ArgumentNullException("sigAlgID");
			}
			if (sig == null)
			{
				throw new ArgumentNullException("sig");
			}
			this.tbsCert = tbsCert;
			this.sigAlgID = sigAlgID;
			this.sig = sig;
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000F8638 File Offset: 0x000F6838
		private X509CertificateStructure(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("sequence wrong size for a certificate", "seq");
			}
			this.tbsCert = TbsCertificateStructure.GetInstance(seq[0]);
			this.sigAlgID = AlgorithmIdentifier.GetInstance(seq[1]);
			this.sig = DerBitString.GetInstance(seq[2]);
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060030A2 RID: 12450 RVA: 0x000F869A File Offset: 0x000F689A
		public TbsCertificateStructure TbsCertificate
		{
			get
			{
				return this.tbsCert;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x000F86A2 File Offset: 0x000F68A2
		public int Version
		{
			get
			{
				return this.tbsCert.Version;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060030A4 RID: 12452 RVA: 0x000F86AF File Offset: 0x000F68AF
		public DerInteger SerialNumber
		{
			get
			{
				return this.tbsCert.SerialNumber;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x000F86BC File Offset: 0x000F68BC
		public X509Name Issuer
		{
			get
			{
				return this.tbsCert.Issuer;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x000F86C9 File Offset: 0x000F68C9
		public Time StartDate
		{
			get
			{
				return this.tbsCert.StartDate;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x000F86D6 File Offset: 0x000F68D6
		public Time EndDate
		{
			get
			{
				return this.tbsCert.EndDate;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060030A8 RID: 12456 RVA: 0x000F86E3 File Offset: 0x000F68E3
		public X509Name Subject
		{
			get
			{
				return this.tbsCert.Subject;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060030A9 RID: 12457 RVA: 0x000F86F0 File Offset: 0x000F68F0
		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return this.tbsCert.SubjectPublicKeyInfo;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060030AA RID: 12458 RVA: 0x000F86FD File Offset: 0x000F68FD
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.sigAlgID;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x000F8705 File Offset: 0x000F6905
		public DerBitString Signature
		{
			get
			{
				return this.sig;
			}
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000F870D File Offset: 0x000F690D
		public byte[] GetSignatureOctets()
		{
			return this.sig.GetOctets();
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000F871A File Offset: 0x000F691A
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.tbsCert,
				this.sigAlgID,
				this.sig
			});
		}

		// Token: 0x04001F13 RID: 7955
		private readonly TbsCertificateStructure tbsCert;

		// Token: 0x04001F14 RID: 7956
		private readonly AlgorithmIdentifier sigAlgID;

		// Token: 0x04001F15 RID: 7957
		private readonly DerBitString sig;
	}
}
