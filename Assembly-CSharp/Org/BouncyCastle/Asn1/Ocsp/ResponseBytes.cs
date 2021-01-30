using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200054D RID: 1357
	public class ResponseBytes : Asn1Encodable
	{
		// Token: 0x0600315F RID: 12639 RVA: 0x000FD3FD File Offset: 0x000FB5FD
		public static ResponseBytes GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return ResponseBytes.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x000FD40C File Offset: 0x000FB60C
		public static ResponseBytes GetInstance(object obj)
		{
			if (obj == null || obj is ResponseBytes)
			{
				return (ResponseBytes)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ResponseBytes((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x000FD459 File Offset: 0x000FB659
		public ResponseBytes(DerObjectIdentifier responseType, Asn1OctetString response)
		{
			if (responseType == null)
			{
				throw new ArgumentNullException("responseType");
			}
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			this.responseType = responseType;
			this.response = response;
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x000FD48C File Offset: 0x000FB68C
		private ResponseBytes(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.responseType = DerObjectIdentifier.GetInstance(seq[0]);
			this.response = Asn1OctetString.GetInstance(seq[1]);
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x000FD4DC File Offset: 0x000FB6DC
		public DerObjectIdentifier ResponseType
		{
			get
			{
				return this.responseType;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x000FD4E4 File Offset: 0x000FB6E4
		public Asn1OctetString Response
		{
			get
			{
				return this.response;
			}
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x000FD4EC File Offset: 0x000FB6EC
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.responseType,
				this.response
			});
		}

		// Token: 0x04002076 RID: 8310
		private readonly DerObjectIdentifier responseType;

		// Token: 0x04002077 RID: 8311
		private readonly Asn1OctetString response;
	}
}
