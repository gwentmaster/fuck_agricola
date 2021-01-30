using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001D9 RID: 473
	[RequireComponent(typeof(Image))]
	public class ColorImage : MonoBehaviour
	{
		// Token: 0x060011F1 RID: 4593 RVA: 0x0006EF8B File Offset: 0x0006D18B
		private void Awake()
		{
			this.image = base.GetComponent<Image>();
			this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0006EFB5 File Offset: 0x0006D1B5
		private void OnDestroy()
		{
			this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0006EFD3 File Offset: 0x0006D1D3
		private void ColorChanged(Color newColor)
		{
			this.image.color = newColor;
		}

		// Token: 0x0400106C RID: 4204
		public ColorPickerControl picker;

		// Token: 0x0400106D RID: 4205
		private Image image;
	}
}
