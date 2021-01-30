using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001FF RID: 511
	public class PauseMenu : SimpleMenu<PauseMenu>
	{
		// Token: 0x060012AA RID: 4778 RVA: 0x00071229 File Offset: 0x0006F429
		public void OnQuitPressed()
		{
			SimpleMenu<PauseMenu>.Hide();
			Object.Destroy(base.gameObject);
			SimpleMenu<GameMenu>.Hide();
		}
	}
}
