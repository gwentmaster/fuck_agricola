using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A9 RID: 425
	public abstract class Menu : MonoBehaviour
	{
		// Token: 0x06001078 RID: 4216
		public abstract void OnBackPressed();

		// Token: 0x04000F6E RID: 3950
		[Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
		public bool DestroyWhenClosed = true;

		// Token: 0x04000F6F RID: 3951
		[Tooltip("Disable menus that are under this one in the stack")]
		public bool DisableMenusUnderneath = true;
	}
}
