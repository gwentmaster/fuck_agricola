using System;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200051B RID: 1307
	public class X9ECPoint : Asn1Encodable
	{
		// Token: 0x06002FCC RID: 12236 RVA: 0x000F5AD8 File Offset: 0x000F3CD8
		public X9ECPoint(ECPoint p) : this(p, false)
		{
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000F5AE2 File Offset: 0x000F3CE2
		public X9ECPoint(ECPoint p, bool compressed)
		{
			this.p = p.Normalize();
			this.encoding = new DerOctetString(p.GetEncoded(compressed));
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000F5B08 File Offset: 0x000F3D08
		public X9ECPoint(ECCurve c, byte[] encoding)
		{
			this.c = c;
			this.encoding = new DerOctetString(Arrays.Clone(encoding));
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000F5B28 File Offset: 0x000F3D28
		public X9ECPoint(ECCurve c, Asn1OctetString s) : this(c, s.GetOctets())
		{
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000F5B37 File Offset: 0x000F3D37
		public byte[] GetPointEncoding()
		{
			return Arrays.Clone(this.encoding.GetOctets());
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x000F5B49 File Offset: 0x000F3D49
		public ECPoint Point
		{
			get
			{
				if (this.p == null)
				{
					this.p = this.c.DecodePoint(this.encoding.GetOctets()).Normalize();
				}
				return this.p;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x000F5B7C File Offset: 0x000F3D7C
		public bool IsPointCompressed
		{
			get
			{
				byte[] octets = this.encoding.GetOctets();
				return octets != null && octets.Length != 0 && (octets[0] == 2 || octets[0] == 3);
			}
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000F5BAC File Offset: 0x000F3DAC
		public override Asn1Object ToAsn1Object()
		{
			return this.encoding;
		}

		// Token: 0x04001E6A RID: 7786
		private readonly Asn1OctetString encoding;

		// Token: 0x04001E6B RID: 7787
		private ECCurve c;

		// Token: 0x04001E6C RID: 7788
		private ECPoint p;
	}
}
