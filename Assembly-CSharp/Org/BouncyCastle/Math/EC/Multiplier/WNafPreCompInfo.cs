using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002F4 RID: 756
	public class WNafPreCompInfo : PreCompInfo
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x00099C94 File Offset: 0x00097E94
		// (set) Token: 0x06001A8F RID: 6799 RVA: 0x00099C9C File Offset: 0x00097E9C
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

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x00099CA5 File Offset: 0x00097EA5
		// (set) Token: 0x06001A91 RID: 6801 RVA: 0x00099CAD File Offset: 0x00097EAD
		public virtual ECPoint[] PreCompNeg
		{
			get
			{
				return this.m_preCompNeg;
			}
			set
			{
				this.m_preCompNeg = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x00099CB6 File Offset: 0x00097EB6
		// (set) Token: 0x06001A93 RID: 6803 RVA: 0x00099CBE File Offset: 0x00097EBE
		public virtual ECPoint Twice
		{
			get
			{
				return this.m_twice;
			}
			set
			{
				this.m_twice = value;
			}
		}

		// Token: 0x04001594 RID: 5524
		protected ECPoint[] m_preComp;

		// Token: 0x04001595 RID: 5525
		protected ECPoint[] m_preCompNeg;

		// Token: 0x04001596 RID: 5526
		protected ECPoint m_twice;
	}
}
