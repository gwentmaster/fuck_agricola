using System;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200054C RID: 1356
	public class ResponderID : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003158 RID: 12632 RVA: 0x000FD2FC File Offset: 0x000FB4FC
		public static ResponderID GetInstance(object obj)
		{
			if (obj == null || obj is ResponderID)
			{
				return (ResponderID)obj;
			}
			if (obj is DerOctetString)
			{
				return new ResponderID((DerOctetString)obj);
			}
			if (!(obj is Asn1TaggedObject))
			{
				return new ResponderID(X509Name.GetInstance(obj));
			}
			Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
			if (asn1TaggedObject.TagNo == 1)
			{
				return new ResponderID(X509Name.GetInstance(asn1TaggedObject, true));
			}
			return new ResponderID(Asn1OctetString.GetInstance(asn1TaggedObject, true));
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x000FD36C File Offset: 0x000FB56C
		public ResponderID(Asn1OctetString id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x000FD36C File Offset: 0x000FB56C
		public ResponderID(X509Name id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x000FD389 File Offset: 0x000FB589
		public static ResponderID GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return ResponderID.GetInstance(obj.GetObject());
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000FD396 File Offset: 0x000FB596
		public virtual byte[] GetKeyHash()
		{
			if (this.id is Asn1OctetString)
			{
				return ((Asn1OctetString)this.id).GetOctets();
			}
			return null;
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x000FD3B7 File Offset: 0x000FB5B7
		public virtual X509Name Name
		{
			get
			{
				if (this.id is Asn1OctetString)
				{
					return null;
				}
				return X509Name.GetInstance(this.id);
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x000FD3D3 File Offset: 0x000FB5D3
		public override Asn1Object ToAsn1Object()
		{
			if (this.id is Asn1OctetString)
			{
				return new DerTaggedObject(true, 2, this.id);
			}
			return new DerTaggedObject(true, 1, this.id);
		}

		// Token: 0x04002075 RID: 8309
		private readonly Asn1Encodable id;
	}
}
