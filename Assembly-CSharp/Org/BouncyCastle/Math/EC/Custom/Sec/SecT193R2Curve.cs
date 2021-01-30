using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000342 RID: 834
	internal class SecT193R2Curve : AbstractF2mCurve
	{
		// Token: 0x06001EF8 RID: 7928 RVA: 0x000AAAF0 File Offset: 0x000A8CF0
		public SecT193R2Curve() : base(193, 15, 0, 0)
		{
			this.m_infinity = new SecT193R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("0163F35A5137C2CE3EA6ED8667190B0BC43ECD69977702709B")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00C9BB9E8927D4D64C377E2AB2856A5B16E3EFB7F61D4316AE")));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000015AAB561B005413CCD4EE99D5"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000AAB7A File Offset: 0x000A8D7A
		protected override ECCurve CloneCurve()
		{
			return new SecT193R2Curve();
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001EFB RID: 7931 RVA: 0x000AAB81 File Offset: 0x000A8D81
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x000AA1CD File Offset: 0x000A83CD
		public override int FieldSize
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x000AA495 File Offset: 0x000A8695
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT193FieldElement(x);
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x000AAB89 File Offset: 0x000A8D89
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT193R2Point(this, x, y, withCompression);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000AAB94 File Offset: 0x000A8D94
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT193R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x000AA1CD File Offset: 0x000A83CD
		public virtual int M
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x000AA3B5 File Offset: 0x000A85B5
		public virtual int K1
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04001639 RID: 5689
		private const int SecT193R2_DEFAULT_COORDS = 6;

		// Token: 0x0400163A RID: 5690
		protected readonly SecT193R2Point m_infinity;
	}
}
