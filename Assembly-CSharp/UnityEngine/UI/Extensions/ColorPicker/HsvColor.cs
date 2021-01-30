using System;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001E2 RID: 482
	public struct HsvColor
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x0006FFA8 File Offset: 0x0006E1A8
		// (set) Token: 0x06001230 RID: 4656 RVA: 0x0006FFB7 File Offset: 0x0006E1B7
		public float NormalizedH
		{
			get
			{
				return (float)this.H / 360f;
			}
			set
			{
				this.H = (double)value * 360.0;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x0006FFCB File Offset: 0x0006E1CB
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x0006FFD4 File Offset: 0x0006E1D4
		public float NormalizedS
		{
			get
			{
				return (float)this.S;
			}
			set
			{
				this.S = (double)value;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x0006FFDE File Offset: 0x0006E1DE
		// (set) Token: 0x06001234 RID: 4660 RVA: 0x0006FFE7 File Offset: 0x0006E1E7
		public float NormalizedV
		{
			get
			{
				return (float)this.V;
			}
			set
			{
				this.V = (double)value;
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0006FFF1 File Offset: 0x0006E1F1
		public HsvColor(double h, double s, double v)
		{
			this.H = h;
			this.S = s;
			this.V = v;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00070008 File Offset: 0x0006E208
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{",
				this.H.ToString("f2"),
				",",
				this.S.ToString("f2"),
				",",
				this.V.ToString("f2"),
				"}"
			});
		}

		// Token: 0x04001093 RID: 4243
		public double H;

		// Token: 0x04001094 RID: 4244
		public double S;

		// Token: 0x04001095 RID: 4245
		public double V;
	}
}
