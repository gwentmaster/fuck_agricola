using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.Oiw
{
	// Token: 0x02000548 RID: 1352
	public class ElGamalParameter : Asn1Encodable
	{
		// Token: 0x06003148 RID: 12616 RVA: 0x000FD06F File Offset: 0x000FB26F
		public ElGamalParameter(BigInteger p, BigInteger g)
		{
			this.p = new DerInteger(p);
			this.g = new DerInteger(g);
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000FD090 File Offset: 0x000FB290
		public ElGamalParameter(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.p = DerInteger.GetInstance(seq[0]);
			this.g = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600314A RID: 12618 RVA: 0x000FD0E0 File Offset: 0x000FB2E0
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x000FD0ED File Offset: 0x000FB2ED
		public BigInteger G
		{
			get
			{
				return this.g.PositiveValue;
			}
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000FD0FA File Offset: 0x000FB2FA
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.p,
				this.g
			});
		}

		// Token: 0x0400205F RID: 8287
		internal DerInteger p;

		// Token: 0x04002060 RID: 8288
		internal DerInteger g;
	}
}
