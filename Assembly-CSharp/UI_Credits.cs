using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class UI_Credits : MonoBehaviour
{
	// Token: 0x06000A7C RID: 2684 RVA: 0x0004590B File Offset: 0x00043B0B
	private void LateUpdate()
	{
		if (this.m_bCheckUpdate && this.m_scrollRect.m_ScrollRect.verticalNormalizedPosition <= 0f)
		{
			this.m_bCheckUpdate = false;
			ScreenManager.instance.PopScene();
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0004593D File Offset: 0x00043B3D
	public void OnEnterMenu()
	{
		this.m_scrollRect.ResetScroll();
		this.m_bCheckUpdate = true;
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00045951 File Offset: 0x00043B51
	public void OnExitMenu(bool bUnderPopup)
	{
		this.m_bCheckUpdate = false;
	}

	// Token: 0x04000B1E RID: 2846
	public ScrollRect_AutoScroll m_scrollRect;

	// Token: 0x04000B1F RID: 2847
	private bool m_bCheckUpdate = true;
}
