using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001FC RID: 508
	public class GameMenu : SimpleMenu<GameMenu>
	{
		// Token: 0x060012A2 RID: 4770 RVA: 0x000711EA File Offset: 0x0006F3EA
		public override void OnBackPressed()
		{
			SimpleMenu<PauseMenu>.Show();
		}
	}
}
