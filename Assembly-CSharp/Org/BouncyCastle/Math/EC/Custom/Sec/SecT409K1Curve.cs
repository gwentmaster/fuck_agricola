using System;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000356 RID: 854
	internal class SecT409K1Curve : AbstractF2mCurve
	{
		// Token: 0x0600205C RID: 8284 RVA: 0x000B0054 File Offset: 0x000AE254
		public SecT409K1Curve() : base(409, 87, 0, 0)
		{
			this.m_infinity = new SecT409K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE5F83B2D4EA20400EC4557D5ED3E3E7CA5B4B5C83B8E01E5FCF"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000B00CA File Offset: 0x000AE2CA
		protected override ECCurve CloneCurve()
		{
			return new SecT409K1Curve();
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000A868D File Offset: 0x000A688D
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x000B00D1 File Offset: 0x000AE2D1
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06002061 RID: 8289 RVA: 0x000AFE24 File Offset: 0x000AE024
		public override int FieldSize
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000B00D9 File Offset: 0x000AE2D9
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT409FieldElement(x);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000B00E1 File Offset: 0x000AE2E1
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT409K1Point(this, x, y, withCompression);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000B00EC File Offset: 0x000AE2EC
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT409K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06002065 RID: 8293 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x000AFE24 File Offset: 0x000AE024
		public virtual int M
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000B000D File Offset: 0x000AE20D
		public virtual int K1
		{
			get
			{
				return 87;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04001652 RID: 5714
		private const int SecT409K1_DEFAULT_COORDS = 6;

		// Token: 0x04001653 RID: 5715
		protected readonly SecT409K1Point m_infinity;
	}
}
