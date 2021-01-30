using System;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000350 RID: 848
	internal class SecT283K1Curve : AbstractF2mCurve
	{
		// Token: 0x06001FF7 RID: 8183 RVA: 0x000AE804 File Offset: 0x000ACA04
		public SecT283K1Curve() : base(283, 5, 7, 12)
		{
			this.m_infinity = new SecT283K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE9AE2ED07577265DFF7F94451E061E163C61"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000AE87A File Offset: 0x000ACA7A
		protected override ECCurve CloneCurve()
		{
			return new SecT283K1Curve();
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000A868D File Offset: 0x000A688D
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x000AE881 File Offset: 0x000ACA81
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x000AE5CF File Offset: 0x000AC7CF
		public override int FieldSize
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000AE889 File Offset: 0x000ACA89
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT283FieldElement(x);
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000AE891 File Offset: 0x000ACA91
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT283K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000AE89C File Offset: 0x000ACA9C
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT283K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x000AE5CF File Offset: 0x000AC7CF
		public virtual int M
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public virtual int K1
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x000A85D4 File Offset: 0x000A67D4
		public virtual int K2
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x000AE7BC File Offset: 0x000AC9BC
		public virtual int K3
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x0400164B RID: 5707
		private const int SecT283K1_DEFAULT_COORDS = 6;

		// Token: 0x0400164C RID: 5708
		protected readonly SecT283K1Point m_infinity;
	}
}
