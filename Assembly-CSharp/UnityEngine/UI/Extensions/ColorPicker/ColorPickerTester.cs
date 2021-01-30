using System;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020001DD RID: 477
	public class ColorPickerTester : MonoBehaviour
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x0006F570 File Offset: 0x0006D770
		private void Awake()
		{
			this.pickerRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0006F57E File Offset: 0x0006D77E
		private void Start()
		{
			this.picker.onValueChanged.AddListener(delegate(Color color)
			{
				this.pickerRenderer.material.color = color;
			});
		}

		// Token: 0x04001081 RID: 4225
		public Renderer pickerRenderer;

		// Token: 0x04001082 RID: 4226
		public ColorPickerControl picker;
	}
}
