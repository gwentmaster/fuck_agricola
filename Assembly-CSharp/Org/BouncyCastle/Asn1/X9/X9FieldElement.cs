using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200051C RID: 1308
	public class X9FieldElement : Asn1Encodable
	{
		// Token: 0x06002FD4 RID: 12244 RVA: 0x000F5BB4 File Offset: 0x000F3DB4
		public X9FieldElement(ECFieldElement f)
		{
			this.f = f;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000F5BC3 File Offset: 0x000F3DC3
		public X9FieldElement(BigInteger p, Asn1OctetString s) : this(new FpFieldElement(p, new BigInteger(1, s.GetOctets())))
		{
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000F5BDD File Offset: 0x000F3DDD
		public X9FieldElement(int m, int k1, int k2, int k3, Asn1OctetString s) : this(new F2mFieldElement(m, k1, k2, k3, new BigInteger(1, s.GetOctets())))
		{
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x000F5BFC File Offset: 0x000F3DFC
		public ECFieldElement Value
		{
			get
			{
				return this.f;
			}
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000F5C04 File Offset: 0x000F3E04
		public override Asn1Object ToAsn1Object()
		{
			int byteLength = X9IntegerConverter.GetByteLength(this.f);
			return new DerOctetString(X9IntegerConverter.IntegerToBytes(this.f.ToBigInteger(), byteLength));
		}

		// Token: 0x04001E6D RID: 7789
		private ECFieldElement f;
	}
}
