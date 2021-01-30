using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000200 RID: 512
	public class UpdateRadialValue : MonoBehaviour
	{
		// Token: 0x060012AC RID: 4780 RVA: 0x00003022 File Offset: 0x00001222
		private void Start()
		{
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00071248 File Offset: 0x0006F448
		public void UpdateSliderValue()
		{
			float value;
			float.TryParse(this.input.text, out value);
			this.slider.Value = value;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00071274 File Offset: 0x0006F474
		public void UpdateSliderAndle()
		{
			int num;
			int.TryParse(this.input.text, out num);
			this.slider.Angle = (float)num;
		}

		// Token: 0x040010D3 RID: 4307
		public InputField input;

		// Token: 0x040010D4 RID: 4308
		public RadialSlider slider;
	}
}
