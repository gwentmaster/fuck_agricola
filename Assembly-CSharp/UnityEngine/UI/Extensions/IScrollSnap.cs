using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200019E RID: 414
	internal interface IScrollSnap
	{
		// Token: 0x06000FD6 RID: 4054
		void ChangePage(int page);

		// Token: 0x06000FD7 RID: 4055
		void SetLerp(bool value);

		// Token: 0x06000FD8 RID: 4056
		int CurrentPage();

		// Token: 0x06000FD9 RID: 4057
		void StartScreenChange();
	}
}
