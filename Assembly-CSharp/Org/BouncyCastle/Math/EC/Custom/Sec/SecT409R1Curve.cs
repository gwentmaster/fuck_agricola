using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000358 RID: 856
	internal class SecT409R1Curve : AbstractF2mCurve
	{
		// Token: 0x06002075 RID: 8309 RVA: 0x000B0700 File Offset: 0x000AE900
		public SecT409R1Curve() : base(409, 87, 0, 0)
		{
			this.m_infinity = new SecT409R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0021A5C2C8EE9FEB5C4B9A753B7B476B7FD6422EF1F3DD674761FA99D6AC27C8A9A197B272822F6CD57A55AA4F50AE317B13545F")));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000000000000000000000000000000001E2AAD6A612F33307BE5FA47C3C9E052F838164CD37D9A21173"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000B077F File Offset: 0x000AE97F
		protected override ECCurve CloneCurve()
		{
			return new SecT409R1Curve();
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x000B0786 File Offset: 0x000AE986
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x000AFE24 File Offset: 0x000AE024
		public override int FieldSize
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000B00D9 File Offset: 0x000AE2D9
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT409FieldElement(x);
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000B078E File Offset: 0x000AE98E
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT409R1Point(this, x, y, withCompression);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000B0799 File Offset: 0x000AE999
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT409R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x000AFE24 File Offset: 0x000AE024
		public virtual int M
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x000B000D File Offset: 0x000AE20D
		public virtual int K1
		{
			get
			{
				return 87;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06002081 RID: 8321 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04001654 RID: 5716
		private const int SecT409R1_DEFAULT_COORDS = 6;

		// Token: 0x04001655 RID: 5717
		protected readonly SecT409R1Point m_infinity;
	}
}
