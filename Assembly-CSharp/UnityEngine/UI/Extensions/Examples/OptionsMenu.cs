using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001FE RID: 510
	public class OptionsMenu : SimpleMenu<OptionsMenu>
	{
		// Token: 0x060012A8 RID: 4776 RVA: 0x0007120F File Offset: 0x0006F40F
		public void OnMagicButtonPressed()
		{
			AwesomeMenu.Show(this.Slider.value);
		}

		// Token: 0x040010D2 RID: 4306
		public Slider Slider;
	}
}
