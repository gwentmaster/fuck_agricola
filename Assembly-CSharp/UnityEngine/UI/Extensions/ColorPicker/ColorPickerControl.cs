using System;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001DB RID: 475
	public class ColorPickerControl : MonoBehaviour
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x0006F183 File Offset: 0x0006D383
		// (set) Token: 0x060011FE RID: 4606 RVA: 0x0006F1A4 File Offset: 0x0006D3A4
		public Color CurrentColor
		{
			get
			{
				return new Color(this._red, this._green, this._blue, this._alpha);
			}
			set
			{
				if (this.CurrentColor == value)
				{
					return;
				}
				this._red = value.r;
				this._green = value.g;
				this._blue = value.b;
				this._alpha = value.a;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0006F1FC File Offset: 0x0006D3FC
		private void Start()
		{
			this.SendChangedEvent();
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x0006F204 File Offset: 0x0006D404
		// (set) Token: 0x06001201 RID: 4609 RVA: 0x0006F20C File Offset: 0x0006D40C
		public float H
		{
			get
			{
				return this._hue;
			}
			set
			{
				if (this._hue == value)
				{
					return;
				}
				this._hue = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x0006F22B File Offset: 0x0006D42B
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x0006F233 File Offset: 0x0006D433
		public float S
		{
			get
			{
				return this._saturation;
			}
			set
			{
				if (this._saturation == value)
				{
					return;
				}
				this._saturation = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0006F252 File Offset: 0x0006D452
		// (set) Token: 0x06001205 RID: 4613 RVA: 0x0006F25A File Offset: 0x0006D45A
		public float V
		{
			get
			{
				return this._brightness;
			}
			set
			{
				if (this._brightness == value)
				{
					return;
				}
				this._brightness = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0006F279 File Offset: 0x0006D479
		// (set) Token: 0x06001207 RID: 4615 RVA: 0x0006F281 File Offset: 0x0006D481
		public float R
		{
			get
			{
				return this._red;
			}
			set
			{
				if (this._red == value)
				{
					return;
				}
				this._red = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x0006F2A0 File Offset: 0x0006D4A0
		// (set) Token: 0x06001209 RID: 4617 RVA: 0x0006F2A8 File Offset: 0x0006D4A8
		public float G
		{
			get
			{
				return this._green;
			}
			set
			{
				if (this._green == value)
				{
					return;
				}
				this._green = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0006F2C7 File Offset: 0x0006D4C7
		// (set) Token: 0x0600120B RID: 4619 RVA: 0x0006F2CF File Offset: 0x0006D4CF
		public float B
		{
			get
			{
				return this._blue;
			}
			set
			{
				if (this._blue == value)
				{
					return;
				}
				this._blue = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0006F2EE File Offset: 0x0006D4EE
		// (set) Token: 0x0600120D RID: 4621 RVA: 0x0006F2F6 File Offset: 0x0006D4F6
		private float A
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (this._alpha == value)
				{
					return;
				}
				this._alpha = value;
				this.SendChangedEvent();
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0006F310 File Offset: 0x0006D510
		private void RGBChanged()
		{
			HsvColor hsvColor = HSVUtil.ConvertRgbToHsv(this.CurrentColor);
			this._hue = hsvColor.NormalizedH;
			this._saturation = hsvColor.NormalizedS;
			this._brightness = hsvColor.NormalizedV;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0006F350 File Offset: 0x0006D550
		private void HSVChanged()
		{
			Color color = HSVUtil.ConvertHsvToRgb((double)(this._hue * 360f), (double)this._saturation, (double)this._brightness, this._alpha);
			this._red = color.r;
			this._green = color.g;
			this._blue = color.b;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0006F3A8 File Offset: 0x0006D5A8
		private void SendChangedEvent()
		{
			this.onValueChanged.Invoke(this.CurrentColor);
			this.onHSVChanged.Invoke(this._hue, this._saturation, this._brightness);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0006F3D8 File Offset: 0x0006D5D8
		public void AssignColor(ColorValues type, float value)
		{
			switch (type)
			{
			case ColorValues.R:
				this.R = value;
				return;
			case ColorValues.G:
				this.G = value;
				return;
			case ColorValues.B:
				this.B = value;
				return;
			case ColorValues.A:
				this.A = value;
				return;
			case ColorValues.Hue:
				this.H = value;
				return;
			case ColorValues.Saturation:
				this.S = value;
				return;
			case ColorValues.Value:
				this.V = value;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0006F440 File Offset: 0x0006D640
		public float GetValue(ColorValues type)
		{
			switch (type)
			{
			case ColorValues.R:
				return this.R;
			case ColorValues.G:
				return this.G;
			case ColorValues.B:
				return this.B;
			case ColorValues.A:
				return this.A;
			case ColorValues.Hue:
				return this.H;
			case ColorValues.Saturation:
				return this.S;
			case ColorValues.Value:
				return this.V;
			default:
				throw new NotImplementedException("");
			}
		}

		// Token: 0x04001075 RID: 4213
		private float _hue;

		// Token: 0x04001076 RID: 4214
		private float _saturation;

		// Token: 0x04001077 RID: 4215
		private float _brightness;

		// Token: 0x04001078 RID: 4216
		private float _red;

		// Token: 0x04001079 RID: 4217
		private float _green;

		// Token: 0x0400107A RID: 4218
		private float _blue;

		// Token: 0x0400107B RID: 4219
		private float _alpha = 1f;

		// Token: 0x0400107C RID: 4220
		public ColorChangedEvent onValueChanged = new ColorChangedEvent();

		// Token: 0x0400107D RID: 4221
		public HSVChangedEvent onHSVChanged = new HSVChangedEvent();
	}
}
