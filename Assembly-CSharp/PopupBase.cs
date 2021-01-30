using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class PopupBase : MonoBehaviour
{
	// Token: 0x06000574 RID: 1396 RVA: 0x0002A059 File Offset: 0x00028259
	public void SetPopupManager(PopupManager manager)
	{
		this.m_PopupManager = manager;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0002A062 File Offset: 0x00028262
	public virtual bool GetIsHidden()
	{
		return false;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0002A065 File Offset: 0x00028265
	public virtual void ActivatePopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0002A073 File Offset: 0x00028273
	public virtual void DismissPopup()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void RebuildPopup()
	{
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0000900B File Offset: 0x0000720B
	public virtual bool BlockGameUpdate()
	{
		return true;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void HandleTutorialFlags(uint tutorialFlags)
	{
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void OnActionButtonCancel()
	{
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void OnActionButtonCheck()
	{
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void OnActionButtonCommit()
	{
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void OnActionButtonRoll()
	{
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void OnActionButtonGeneric()
	{
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void EndAnnounce()
	{
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void StartTimer()
	{
	}

	// Token: 0x04000526 RID: 1318
	[HideInInspector]
	protected PopupManager m_PopupManager;
}
