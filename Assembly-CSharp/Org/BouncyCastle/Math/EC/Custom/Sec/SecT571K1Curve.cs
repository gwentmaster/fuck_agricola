using System;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035C RID: 860
	internal class SecT571K1Curve : AbstractF2mCurve
	{
		// Token: 0x060020C2 RID: 8386 RVA: 0x000B1770 File Offset: 0x000AF970
		public SecT571K1Curve() : base(571, 2, 5, 10)
		{
			this.m_infinity = new SecT571K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("020000000000000000000000000000000000000000000000000000000000000000000000131850E1F19A63E4B391A8DB917F4138B630D84BE5D639381E91DEB45CFE778F637C1001"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000B17E6 File Offset: 0x000AF9E6
		protected override ECCurve CloneCurve()
		{
			return new SecT571K1Curve();
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000A868D File Offset: 0x000A688D
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060020C6 RID: 8390 RVA: 0x000B17ED File Offset: 0x000AF9ED
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x000B1541 File Offset: 0x000AF741
		public override int FieldSize
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000B17F5 File Offset: 0x000AF9F5
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT571FieldElement(x);
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000B17FD File Offset: 0x000AF9FD
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT571K1Point(this, x, y, withCompression);
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000B1808 File Offset: 0x000AFA08
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT571K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060020CC RID: 8396 RVA: 0x000B1541 File Offset: 0x000AF741
		public virtual int M
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060020CE RID: 8398 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060020CF RID: 8399 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public virtual int K2
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060020D0 RID: 8400 RVA: 0x000B1729 File Offset: 0x000AF929
		public virtual int K3
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x0400165A RID: 5722
		private const int SecT571K1_DEFAULT_COORDS = 6;

		// Token: 0x0400165B RID: 5723
		protected readonly SecT571K1Point m_infinity;
	}
}
