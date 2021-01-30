using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000348 RID: 840
	internal class SecT233R1Curve : AbstractF2mCurve
	{
		// Token: 0x06001F5D RID: 8029 RVA: 0x000AC34C File Offset: 0x000AA54C
		public SecT233R1Curve() : base(233, 74, 0, 0)
		{
			this.m_infinity = new SecT233R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0066647EDE6C332C7F8C0923BB58213B333B20E9CE4281FE115F7D8F90AD")));
			this.m_order = new BigInteger(1, Hex.Decode("01000000000000000000000000000013E974E72F8A6922031D2603CFE0D7"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x000AC3CB File Offset: 0x000AA5CB
		protected override ECCurve CloneCurve()
		{
			return new SecT233R1Curve();
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x000AC3D2 File Offset: 0x000AA5D2
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001F61 RID: 8033 RVA: 0x000ABA71 File Offset: 0x000A9C71
		public override int FieldSize
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000ABD1D File Offset: 0x000A9F1D
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT233FieldElement(x);
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000AC3DA File Offset: 0x000AA5DA
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT233R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000AC3E5 File Offset: 0x000AA5E5
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT233R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001F65 RID: 8037 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x000ABA71 File Offset: 0x000A9C71
		public virtual int M
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001F68 RID: 8040 RVA: 0x000ABC59 File Offset: 0x000A9E59
		public virtual int K1
		{
			get
			{
				return 74;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001F69 RID: 8041 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04001640 RID: 5696
		private const int SecT233R1_DEFAULT_COORDS = 6;

		// Token: 0x04001641 RID: 5697
		protected readonly SecT233R1Point m_infinity;
	}
}
