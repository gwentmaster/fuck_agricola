using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200032E RID: 814
	internal class SecT113R2Curve : AbstractF2mCurve
	{
		// Token: 0x06001DB3 RID: 7603 RVA: 0x000A5BC8 File Offset: 0x000A3DC8
		public SecT113R2Curve() : base(113, 9, 0, 0)
		{
			this.m_infinity = new SecT113R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("00689918DBEC7E5A0DD6DFC0AA55C7")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0095E9A9EC9B297BD4BF36E059184F")));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000108789B2496AF93"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000A5C4F File Offset: 0x000A3E4F
		protected override ECCurve CloneCurve()
		{
			return new SecT113R2Curve();
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x000A5C56 File Offset: 0x000A3E56
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x000A5127 File Offset: 0x000A3327
		public override int FieldSize
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000A5403 File Offset: 0x000A3603
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT113FieldElement(x);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000A5C5E File Offset: 0x000A3E5E
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT113R2Point(this, x, y, withCompression);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x000A5C69 File Offset: 0x000A3E69
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT113R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001DBB RID: 7611 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x000A5127 File Offset: 0x000A3327
		public virtual int M
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06001DBD RID: 7613 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001DBE RID: 7614 RVA: 0x000A531C File Offset: 0x000A351C
		public virtual int K1
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001DBF RID: 7615 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04001620 RID: 5664
		private const int SecT113R2_DEFAULT_COORDS = 6;

		// Token: 0x04001621 RID: 5665
		protected readonly SecT113R2Point m_infinity;
	}
}
