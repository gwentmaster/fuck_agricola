using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E2 RID: 1250
	public class BerSequence : DerSequence
	{
		// Token: 0x06002E1F RID: 11807 RVA: 0x000F1060 File Offset: 0x000EF260
		public new static BerSequence FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new BerSequence(v);
			}
			return BerSequence.Empty;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000F1077 File Offset: 0x000EF277
		public BerSequence()
		{
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000F107F File Offset: 0x000EF27F
		public BerSequence(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000F1088 File Offset: 0x000EF288
		public BerSequence(params Asn1Encodable[] v) : base(v)
		{
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000F1091 File Offset: 0x000EF291
		public BerSequence(Asn1EncodableVector v) : base(v)
		{
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000F109C File Offset: 0x000EF29C
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(48);
				derOut.WriteByte(128);
				foreach (object obj in this)
				{
					Asn1Encodable obj2 = (Asn1Encodable)obj;
					derOut.WriteObject(obj2);
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}

		// Token: 0x04001E15 RID: 7701
		public new static readonly BerSequence Empty = new BerSequence();
	}
}
