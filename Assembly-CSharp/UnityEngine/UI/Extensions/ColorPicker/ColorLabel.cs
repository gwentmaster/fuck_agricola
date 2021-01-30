using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001DA RID: 474
	[RequireComponent(typeof(Text))]
	public class ColorLabel : MonoBehaviour
	{
		// Token: 0x060011F5 RID: 4597 RVA: 0x0006EFE1 File Offset: 0x0006D1E1
		private void Awake()
		{
			this.label = base.GetComponent<Text>();
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0006EFF0 File Offset: 0x0006D1F0
		private void OnEnable()
		{
			if (Application.isPlaying && this.picker != null)
			{
				this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0006F04C File Offset: 0x0006D24C
		private void OnDestroy()
		{
			if (this.picker != null)
			{
				this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0006F09F File Offset: 0x0006D29F
		private void ColorChanged(Color color)
		{
			this.UpdateValue();
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0006F09F File Offset: 0x0006D29F
		private void HSVChanged(float hue, float sateration, float value)
		{
			this.UpdateValue();
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0006F0A8 File Offset: 0x0006D2A8
		private void UpdateValue()
		{
			if (this.picker == null)
			{
				this.label.text = this.prefix + "-";
				return;
			}
			float value = this.minValue + this.picker.GetValue(this.type) * (this.maxValue - this.minValue);
			this.label.text = this.prefix + this.ConvertToDisplayString(value);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0006F124 File Offset: 0x0006D324
		private string ConvertToDisplayString(float value)
		{
			if (this.precision > 0)
			{
				return value.ToString("f " + this.precision);
			}
			return Mathf.FloorToInt(value).ToString();
		}

		// Token: 0x0400106E RID: 4206
		public ColorPickerControl picker;

		// Token: 0x0400106F RID: 4207
		public ColorValues type;

		// Token: 0x04001070 RID: 4208
		public string prefix = "R: ";

		// Token: 0x04001071 RID: 4209
		public float minValue;

		// Token: 0x04001072 RID: 4210
		public float maxValue = 255f;

		// Token: 0x04001073 RID: 4211
		public int precision;

		// Token: 0x04001074 RID: 4212
		private Text label;
	}
}
