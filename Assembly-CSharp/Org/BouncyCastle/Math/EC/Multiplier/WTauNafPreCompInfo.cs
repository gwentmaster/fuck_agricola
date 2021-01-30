using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002F7 RID: 759
	public class WTauNafPreCompInfo : PreCompInfo
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x0009A719 File Offset: 0x00098919
		// (set) Token: 0x06001AAC RID: 6828 RVA: 0x0009A721 File Offset: 0x00098921
		public virtual AbstractF2mPoint[] PreComp
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

		// Token: 0x0400159D RID: 5533
		protected AbstractF2mPoint[] m_preComp;
	}
}
