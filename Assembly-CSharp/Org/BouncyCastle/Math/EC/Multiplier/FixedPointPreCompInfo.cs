using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002EF RID: 751
	public class FixedPointPreCompInfo : PreCompInfo
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001A7F RID: 6783 RVA: 0x00099911 File Offset: 0x00097B11
		// (set) Token: 0x06001A80 RID: 6784 RVA: 0x00099919 File Offset: 0x00097B19
		public virtual ECPoint[] PreComp
		{
			get
			{
				return this.m_preComp;
			}
			set
			{
				this.m_preComp = value;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001A81 RID: 6785 RVA: 0x00099922 File Offset: 0x00097B22
		// (set) Token: 0x06001A82 RID: 6786 RVA: 0x0009992A File Offset: 0x00097B2A
		public virtual int Width
		{
			get
			{
				return this.m_width;
			}
			set
			{
				this.m_width = value;
			}
		}

		// Token: 0x0400158F RID: 5519
		protected ECPoint[] m_preComp;

		// Token: 0x04001590 RID: 5520
		protected int m_width = -1;
	}
}
