using System;
using System.Collections;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000544 RID: 1348
	public class DHParameter : Asn1Encodable
	{
		// Token: 0x0600312C RID: 12588 RVA: 0x000FC37B File Offset: 0x000FA57B
		public DHParameter(BigInteger p, BigInteger g, int l)
		{
			this.p = new DerInteger(p);
			this.g = new DerInteger(g);
			if (l != 0)
			{
				this.l = new DerInteger(l);
			}
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000FC3AC File Offset: 0x000FA5AC
		public DHParameter(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.p = (DerInteger)enumerator.Current;
			enumerator.MoveNext();
			this.g = (DerInteger)enumerator.Current;
			if (enumerator.MoveNext())
			{
				this.l = (DerInteger)enumerator.Current;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x000FC40F File Offset: 0x000FA60F
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x000FC41C File Offset: 0x000FA61C
		public BigInteger G
		{
			get
			{
				return this.g.PositiveValue;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x000FC429 File Offset: 0x000FA629
		public BigInteger L
		{
			get
			{
				if (this.l != null)
				{
					return this.l.PositiveValue;
				}
				return null;
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000FC440 File Offset: 0x000FA640
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.p,
				this.g
			});
			if (this.l != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.l
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04001FC7 RID: 8135
		internal DerInteger p;

		// Token: 0x04001FC8 RID: 8136
		internal DerInteger g;

		// Token: 0x04001FC9 RID: 8137
		internal DerInteger l;
	}
}
