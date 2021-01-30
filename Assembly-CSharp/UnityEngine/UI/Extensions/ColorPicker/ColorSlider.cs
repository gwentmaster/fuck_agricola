using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001DE RID: 478
	[RequireComponent(typeof(Slider))]
	public class ColorSlider : MonoBehaviour
	{
		// Token: 0x0600121D RID: 4637 RVA: 0x0006F5B0 File Offset: 0x0006D7B0
		private void Awake()
		{
			this.slider = base.GetComponent<Slider>();
			this.ColorPicker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.SliderChanged));
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0006F620 File Offset: 0x0006D820
		private void OnDestroy()
		{
			this.ColorPicker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SliderChanged));
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0006F684 File Offset: 0x0006D884
		private void ColorChanged(Color newColor)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.R:
				this.slider.normalizedValue = newColor.r;
				return;
			case ColorValues.G:
				this.slider.normalizedValue = newColor.g;
				return;
			case ColorValues.B:
				this.slider.normalizedValue = newColor.b;
				return;
			case ColorValues.A:
				this.slider.normalizedValue = newColor.a;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0006F700 File Offset: 0x0006D900
		private void HSVChanged(float hue, float saturation, float value)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.Hue:
				this.slider.normalizedValue = hue;
				return;
			case ColorValues.Saturation:
				this.slider.normalizedValue = saturation;
				return;
			case ColorValues.Value:
				this.slider.normalizedValue = value;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0006F756 File Offset: 0x0006D956
		private void SliderChanged(float newValue)
		{
			if (this.listen)
			{
				newValue = this.slider.normalizedValue;
				this.ColorPicker.AssignColor(this.type, newValue);
			}
			this.listen = true;
		}

		// Token: 0x04001083 RID: 4227
		public ColorPickerControl ColorPicker;

		// Token: 0x04001084 RID: 4228
		public ColorValues type;

		// Token: 0x04001085 RID: 4229
		private Slider slider;

		// Token: 0x04001086 RID: 4230
		private bool listen = true;
	}
}
