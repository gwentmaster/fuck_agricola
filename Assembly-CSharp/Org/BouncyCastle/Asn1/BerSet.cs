using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E3 RID: 1251
	public class BerSet : DerSet
	{
		// Token: 0x06002E26 RID: 11814 RVA: 0x000F1138 File Offset: 0x000EF338
		public new static BerSet FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new BerSet(v);
			}
			return BerSet.Empty;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000F114F File Offset: 0x000EF34F
		internal new static BerSet FromVector(Asn1EncodableVector v, bool needsSorting)
		{
			if (v.Count >= 1)
			{
				return new BerSet(v, needsSorting);
			}
			return BerSet.Empty;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000F1167 File Offset: 0x000EF367
		public BerSet()
		{
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000F116F File Offset: 0x000EF36F
		public BerSet(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000F1178 File Offset: 0x000EF378
		public BerSet(Asn1EncodableVector v) : base(v, false)
		{
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000F1182 File Offset: 0x000EF382
		internal BerSet(Asn1EncodableVector v, bool needsSorting) : base(v, needsSorting)
		{
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000F118C File Offset: 0x000EF38C
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(49);
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

		// Token: 0x04001E16 RID: 7702
		public new static readonly BerSet Empty = new BerSet();
	}
}
