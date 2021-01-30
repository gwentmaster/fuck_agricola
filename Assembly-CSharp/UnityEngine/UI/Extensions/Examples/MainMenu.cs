using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001FD RID: 509
	public class MainMenu : SimpleMenu<MainMenu>
	{
		// Token: 0x060012A4 RID: 4772 RVA: 0x000711F9 File Offset: 0x0006F3F9
		public void OnPlayPressed()
		{
			SimpleMenu<GameMenu>.Show();
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00071200 File Offset: 0x0006F400
		public void OnOptionsPressed()
		{
			SimpleMenu<OptionsMenu>.Show();
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000487B9 File Offset: 0x000469B9
		public override void OnBackPressed()
		{
			Application.Quit();
		}
	}
}
