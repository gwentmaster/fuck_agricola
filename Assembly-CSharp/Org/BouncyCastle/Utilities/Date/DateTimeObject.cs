using System;

namespace Org.BouncyCastle.Utilities.Date
{
	// Token: 0x020002AB RID: 683
	public sealed class DateTimeObject
	{
		// Token: 0x0600169D RID: 5789 RVA: 0x00081802 File Offset: 0x0007FA02
		public DateTimeObject(DateTime dt)
		{
			this.dt = dt;
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x00081811 File Offset: 0x0007FA11
		public DateTime Value
		{
			get
			{
				return this.dt;
			}
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0008181C File Offset: 0x0007FA1C
		public override string ToString()
		{
			return this.dt.ToString();
		}

		// Token: 0x04001519 RID: 5401
		private readonly DateTime dt;
	}
}
