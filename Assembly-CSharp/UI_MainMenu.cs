using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class UI_MainMenu : MonoBehaviour
{
	// Token: 0x06000ABE RID: 2750 RVA: 0x00003022 File Offset: 0x00001222
	public void Awake()
	{
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0004876C File Offset: 0x0004696C
	public void OnEnterMenu()
	{
		this.m_bkgTaps = 0;
		if (this.m_backgroundAnimator != null && this.m_backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("Intro"))
		{
			this.m_backgroundAnimator.Play("Reset");
		}
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x000487B9 File Offset: 0x000469B9
	public void OnExitButtonPress()
	{
		Application.Quit();
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x000487C0 File Offset: 0x000469C0
	public void OnBkgTap()
	{
		int num = this.m_bkgTaps + 1;
		this.m_bkgTaps = num;
		if (num == 10)
		{
			this.m_bkgTaps = 0;
			CoreApplication.Instance.AnalyticsManager.LogEvent("FIND_ME", new Dictionary<string, object>());
		}
	}

	// Token: 0x04000B7E RID: 2942
	public Animator m_backgroundAnimator;

	// Token: 0x04000B7F RID: 2943
	public GameObject m_QuitGameButton;

	// Token: 0x04000B80 RID: 2944
	private int m_bkgTaps;
}
