using System;

namespace Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000543 RID: 1347
	public class ContentInfo : Asn1Encodable
	{
		// Token: 0x06003126 RID: 12582 RVA: 0x000FC2A4 File Offset: 0x000FA4A4
		public static ContentInfo GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			ContentInfo contentInfo = obj as ContentInfo;
			if (contentInfo != null)
			{
				return contentInfo;
			}
			return new ContentInfo(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x000FC2CD File Offset: 0x000FA4CD
		private ContentInfo(Asn1Sequence seq)
		{
			this.contentType = (DerObjectIdentifier)seq[0];
			if (seq.Count > 1)
			{
				this.content = ((Asn1TaggedObject)seq[1]).GetObject();
			}
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000FC307 File Offset: 0x000FA507
		public ContentInfo(DerObjectIdentifier contentType, Asn1Encodable content)
		{
			this.contentType = contentType;
			this.content = content;
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x000FC31D File Offset: 0x000FA51D
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x000FC325 File Offset: 0x000FA525
		public Asn1Encodable Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000FC330 File Offset: 0x000FA530
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.contentType
			});
			if (this.content != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new BerTaggedObject(0, this.content)
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04001FC5 RID: 8133
		private readonly DerObjectIdentifier contentType;

		// Token: 0x04001FC6 RID: 8134
		private readonly Asn1Encodable content;
	}
}
