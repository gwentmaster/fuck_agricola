using System;
using System.Collections;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E4 RID: 1252
	public class BerTaggedObject : DerTaggedObject
	{
		// Token: 0x06002E2E RID: 11822 RVA: 0x000F1228 File Offset: 0x000EF428
		public BerTaggedObject(int tagNo, Asn1Encodable obj) : base(tagNo, obj)
		{
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000F1232 File Offset: 0x000EF432
		public BerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj) : base(explicitly, tagNo, obj)
		{
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000F123D File Offset: 0x000EF43D
		public BerTaggedObject(int tagNo) : base(false, tagNo, BerSequence.Empty)
		{
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000F124C File Offset: 0x000EF44C
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteTag(160, this.tagNo);
				derOut.WriteByte(128);
				if (!base.IsEmpty())
				{
					if (!this.explicitly)
					{
						IEnumerable enumerable;
						if (this.obj is Asn1OctetString)
						{
							if (this.obj is BerOctetString)
							{
								enumerable = (BerOctetString)this.obj;
							}
							else
							{
								enumerable = new BerOctetString(((Asn1OctetString)this.obj).GetOctets());
							}
						}
						else if (this.obj is Asn1Sequence)
						{
							enumerable = (Asn1Sequence)this.obj;
						}
						else
						{
							if (!(this.obj is Asn1Set))
							{
								throw Platform.CreateNotImplementedException(Platform.GetTypeName(this.obj));
							}
							enumerable = (Asn1Set)this.obj;
						}
						using (IEnumerator enumerator = enumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Asn1Encodable obj2 = (Asn1Encodable)obj;
								derOut.WriteObject(obj2);
							}
							goto IL_10F;
						}
					}
					derOut.WriteObject(this.obj);
				}
				IL_10F:
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}
	}
}
