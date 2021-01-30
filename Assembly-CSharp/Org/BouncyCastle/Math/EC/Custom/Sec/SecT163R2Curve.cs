using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200033C RID: 828
	internal class SecT163R2Curve : AbstractF2mCurve
	{
		// Token: 0x06001E94 RID: 7828 RVA: 0x000A9398 File Offset: 0x000A7598
		public SecT163R2Curve() : base(163, 3, 6, 7)
		{
			this.m_infinity = new SecT163R2Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("020A601907B8C953CA1481EB10512F78744A3205FD")));
			this.m_order = new BigInteger(1, Hex.Decode("040000000000000000000292FE77E70C12A4234C33"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x000A9416 File Offset: 0x000A7616
		protected override ECCurve CloneCurve()
		{
			return new SecT163R2Curve();
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x000A941D File Offset: 0x000A761D
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x000A869C File Offset: 0x000A689C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT163FieldElement(x);
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x000A9425 File Offset: 0x000A7625
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT163R2Point(this, x, y, withCompression);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x000A9430 File Offset: 0x000A7630
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT163R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001E9C RID: 7836 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001E9E RID: 7838 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x000A85D1 File Offset: 0x000A67D1
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x000A85D4 File Offset: 0x000A67D4
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x04001632 RID: 5682
		private const int SecT163R2_DEFAULT_COORDS = 6;

		// Token: 0x04001633 RID: 5683
		protected readonly SecT163R2Point m_infinity;
	}
}
