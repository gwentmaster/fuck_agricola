using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001DC RID: 476
	public class ColorPickerPresets : MonoBehaviour
	{
		// Token: 0x06001214 RID: 4628 RVA: 0x0006F4D5 File Offset: 0x0006D6D5
		private void Awake()
		{
			this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0006F4F4 File Offset: 0x0006D6F4
		public void CreatePresetButton()
		{
			for (int i = 0; i < this.presets.Length; i++)
			{
				if (!this.presets[i].activeSelf)
				{
					this.presets[i].SetActive(true);
					this.presets[i].GetComponent<Image>().color = this.picker.CurrentColor;
					return;
				}
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0006F54F File Offset: 0x0006D74F
		public void PresetSelect(Image sender)
		{
			this.picker.CurrentColor = sender.color;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0006F562 File Offset: 0x0006D762
		private void ColorChanged(Color color)
		{
			this.createPresetImage.color = color;
		}

		// Token: 0x0400107E RID: 4222
		public ColorPickerControl picker;

		// Token: 0x0400107F RID: 4223
		public GameObject[] presets;

		// Token: 0x04001080 RID: 4224
		public Image createPresetImage;
	}
}
