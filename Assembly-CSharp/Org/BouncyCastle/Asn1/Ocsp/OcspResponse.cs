using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200054A RID: 1354
	public class OcspResponse : Asn1Encodable
	{
		// Token: 0x0600314F RID: 12623 RVA: 0x000FD1DD File Offset: 0x000FB3DD
		public static OcspResponse GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return OcspResponse.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000FD1EC File Offset: 0x000FB3EC
		public static OcspResponse GetInstance(object obj)
		{
			if (obj == null || obj is OcspResponse)
			{
				return (OcspResponse)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OcspResponse((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000FD239 File Offset: 0x000FB439
		public OcspResponse(OcspResponseStatus responseStatus, ResponseBytes responseBytes)
		{
			if (responseStatus == null)
			{
				throw new ArgumentNullException("responseStatus");
			}
			this.responseStatus = responseStatus;
			this.responseBytes = responseBytes;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000FD25D File Offset: 0x000FB45D
		private OcspResponse(Asn1Sequence seq)
		{
			this.responseStatus = new OcspResponseStatus(DerEnumerated.GetInstance(seq[0]));
			if (seq.Count == 2)
			{
				this.responseBytes = ResponseBytes.GetInstance((Asn1TaggedObject)seq[1], true);
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06003153 RID: 12627 RVA: 0x000FD29D File Offset: 0x000FB49D
		public OcspResponseStatus ResponseStatus
		{
			get
			{
				return this.responseStatus;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06003154 RID: 12628 RVA: 0x000FD2A5 File Offset: 0x000FB4A5
		public ResponseBytes ResponseBytes
		{
			get
			{
				return this.responseBytes;
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000FD2B0 File Offset: 0x000FB4B0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.responseStatus
			});
			if (this.responseBytes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.responseBytes)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x0400206D RID: 8301
		private readonly OcspResponseStatus responseStatus;

		// Token: 0x0400206E RID: 8302
		private readonly ResponseBytes responseBytes;
	}
}
