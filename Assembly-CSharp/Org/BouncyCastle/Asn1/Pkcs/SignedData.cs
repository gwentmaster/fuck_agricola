using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000547 RID: 1351
	public class SignedData : Asn1Encodable
	{
		// Token: 0x0600313E RID: 12606 RVA: 0x000FCE6C File Offset: 0x000FB06C
		public static SignedData GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			SignedData signedData = obj as SignedData;
			if (signedData != null)
			{
				return signedData;
			}
			return new SignedData(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000FCE95 File Offset: 0x000FB095
		public SignedData(DerInteger _version, Asn1Set _digestAlgorithms, ContentInfo _contentInfo, Asn1Set _certificates, Asn1Set _crls, Asn1Set _signerInfos)
		{
			this.version = _version;
			this.digestAlgorithms = _digestAlgorithms;
			this.contentInfo = _contentInfo;
			this.certificates = _certificates;
			this.crls = _crls;
			this.signerInfos = _signerInfos;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000FCECC File Offset: 0x000FB0CC
		private SignedData(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.version = (DerInteger)enumerator.Current;
			enumerator.MoveNext();
			this.digestAlgorithms = (Asn1Set)enumerator.Current;
			enumerator.MoveNext();
			this.contentInfo = ContentInfo.GetInstance(enumerator.Current);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Asn1Object asn1Object = (Asn1Object)obj;
				if (asn1Object is DerTaggedObject)
				{
					DerTaggedObject derTaggedObject = (DerTaggedObject)asn1Object;
					int tagNo = derTaggedObject.TagNo;
					if (tagNo != 0)
					{
						if (tagNo != 1)
						{
							throw new ArgumentException("unknown tag value " + derTaggedObject.TagNo);
						}
						this.crls = Asn1Set.GetInstance(derTaggedObject, false);
					}
					else
					{
						this.certificates = Asn1Set.GetInstance(derTaggedObject, false);
					}
				}
				else
				{
					this.signerInfos = (Asn1Set)asn1Object;
				}
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x000FCFA8 File Offset: 0x000FB1A8
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x000FCFB0 File Offset: 0x000FB1B0
		public Asn1Set DigestAlgorithms
		{
			get
			{
				return this.digestAlgorithms;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x000FCFB8 File Offset: 0x000FB1B8
		public ContentInfo ContentInfo
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x000FCFC0 File Offset: 0x000FB1C0
		public Asn1Set Certificates
		{
			get
			{
				return this.certificates;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x000FCFC8 File Offset: 0x000FB1C8
		public Asn1Set Crls
		{
			get
			{
				return this.crls;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x000FCFD0 File Offset: 0x000FB1D0
		public Asn1Set SignerInfos
		{
			get
			{
				return this.signerInfos;
			}
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x000FCFD8 File Offset: 0x000FB1D8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.digestAlgorithms,
				this.contentInfo
			});
			if (this.certificates != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.certificates)
				});
			}
			if (this.crls != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.crls)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.signerInfos
			});
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04002059 RID: 8281
		private readonly DerInteger version;

		// Token: 0x0400205A RID: 8282
		private readonly Asn1Set digestAlgorithms;

		// Token: 0x0400205B RID: 8283
		private readonly ContentInfo contentInfo;

		// Token: 0x0400205C RID: 8284
		private readonly Asn1Set certificates;

		// Token: 0x0400205D RID: 8285
		private readonly Asn1Set crls;

		// Token: 0x0400205E RID: 8286
		private readonly Asn1Set signerInfos;
	}
}
