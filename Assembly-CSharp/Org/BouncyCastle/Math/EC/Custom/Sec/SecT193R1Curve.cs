using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000340 RID: 832
	internal class SecT193R1Curve : AbstractF2mCurve
	{
		// Token: 0x06001EE0 RID: 7904 RVA: 0x000AA3FC File Offset: 0x000A85FC
		public SecT193R1Curve() : base(193, 15, 0, 0)
		{
			this.m_infinity = new SecT193R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("0017858FEB7A98975169E171F77B4087DE098AC8A911DF7B01")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00FDFB49BFE6C3A89FACADAA7A1E5BBC7CC1C2E5D831478814")));
			this.m_order = new BigInteger(1, Hex.Decode("01000000000000000000000000C7F34A778F443ACC920EBA49"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000AA486 File Offset: 0x000A8686
		protected override ECCurve CloneCurve()
		{
			return new SecT193R1Curve();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000AA48D File Offset: 0x000A868D
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x000AA1CD File Offset: 0x000A83CD
		public override int FieldSize
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000AA495 File Offset: 0x000A8695
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT193FieldElement(x);
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000AA49D File Offset: 0x000A869D
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT193R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000AA4A8 File Offset: 0x000A86A8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT193R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000AA1CD File Offset: 0x000A83CD
		public virtual int M
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001EEA RID: 7914 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x000AA3B5 File Offset: 0x000A85B5
		public virtual int K1
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04001637 RID: 5687
		private const int SecT193R1_DEFAULT_COORDS = 6;

		// Token: 0x04001638 RID: 5688
		protected readonly SecT193R1Point m_infinity;
	}
}
