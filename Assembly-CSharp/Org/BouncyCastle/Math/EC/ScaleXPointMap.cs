using System;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002EB RID: 747
	public class ScaleXPointMap : ECPointMap
	{
		// Token: 0x06001A76 RID: 6774 RVA: 0x000997E1 File Offset: 0x000979E1
		public ScaleXPointMap(ECFieldElement scale)
		{
			this.scale = scale;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x000997F0 File Offset: 0x000979F0
		public virtual ECPoint Map(ECPoint p)
		{
			return p.ScaleX(this.scale);
		}

		// Token: 0x0400158E RID: 5518
		protected readonly ECFieldElement scale;
	}
}
