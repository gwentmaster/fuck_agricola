using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200011B RID: 283
public class UI_Intro : MonoBehaviour
{
	// Token: 0x06000A91 RID: 2705 RVA: 0x00046010 File Offset: 0x00044210
	public void OnEnterMenu()
	{
		ScreenManager.s_onStartScreen = "MainMenu";
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0004601C File Offset: 0x0004421C
	public void OnDoubleClick(BaseEventData baseData)
	{
		if (((PointerEventData)baseData).clickCount != 2)
		{
			return;
		}
		this.m_backgroundAnimator.Play("Reset");
		ScreenManager.instance.GoToScene("MainMenu", true);
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00046050 File Offset: 0x00044250
	public void OnButtonPress()
	{
		if (this.m_delayedPressTrigger != null)
		{
			this.m_backgroundAnimator.Play("Reset");
			ScreenManager.instance.GoToScene("MainMenu", true);
			this.m_delayedPressTrigger = null;
			return;
		}
		this.m_delayedPressTrigger = base.StartCoroutine(this.DelayedPressTrigger());
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0004609F File Offset: 0x0004429F
	public void OnAnimEnd()
	{
		this.m_backgroundAnimator.Play("Reset");
		ScreenManager.instance.GoToScene("MainMenu", true);
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x000460C1 File Offset: 0x000442C1
	private IEnumerator DelayedPressTrigger()
	{
		yield return new WaitForSeconds(this.m_doubleClickThreshold);
		this.m_delayedPressTrigger = null;
		yield break;
	}

	// Token: 0x04000B30 RID: 2864
	public Animator m_backgroundAnimator;

	// Token: 0x04000B31 RID: 2865
	public float m_doubleClickThreshold = 0.7f;

	// Token: 0x04000B32 RID: 2866
	private Coroutine m_delayedPressTrigger;
}
