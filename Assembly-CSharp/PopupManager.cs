using System;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class PopupManager : MonoBehaviour
{
	// Token: 0x06000583 RID: 1411 RVA: 0x0002A081 File Offset: 0x00028281
	public void AddOnCurrentPopupChangedCallback(PopupManager.PopupChangedCallback callback)
	{
		this.m_OnCurrentPopupChangedCallback = (PopupManager.PopupChangedCallback)Delegate.Combine(this.m_OnCurrentPopupChangedCallback, callback);
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0002A09A File Offset: 0x0002829A
	public void RemoveOnCurrentPopupChangedCallback(PopupManager.PopupChangedCallback callback)
	{
		this.m_OnCurrentPopupChangedCallback = (PopupManager.PopupChangedCallback)Delegate.Remove(this.m_OnCurrentPopupChangedCallback, callback);
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0002A0B3 File Offset: 0x000282B3
	public EPopups GetActivePopup()
	{
		return this.m_ActivePopup;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0002A0BB File Offset: 0x000282BB
	public EPopups GetHiddenPopup()
	{
		return this.m_HiddenPopup;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0002A0C3 File Offset: 0x000282C3
	public bool HasActivePopup()
	{
		return this.m_ActivePopup > EPopups.NONE;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0002A0CE File Offset: 0x000282CE
	public bool HasHiddenPopup()
	{
		return this.m_HiddenPopup > EPopups.NONE;
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0002A0D9 File Offset: 0x000282D9
	public bool HasActiveOrHiddenPopup()
	{
		return this.m_ActivePopup != EPopups.NONE || this.m_HiddenPopup > EPopups.NONE;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0002A0EE File Offset: 0x000282EE
	public PopupBase GetPopup(EPopups popupType)
	{
		return this.m_Popups[(int)popupType];
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0002A0F8 File Offset: 0x000282F8
	public bool BlockGameUpdate()
	{
		return (this.m_ActivePopup != EPopups.NONE && this.m_Popups[(int)this.m_ActivePopup] != null && this.m_Popups[(int)this.m_ActivePopup].BlockGameUpdate()) || (this.m_HiddenPopup != EPopups.NONE && this.m_Popups[(int)this.m_HiddenPopup] != null && this.m_Popups[(int)this.m_HiddenPopup].BlockGameUpdate());
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0002A16C File Offset: 0x0002836C
	private void Start()
	{
		for (int i = 0; i < this.m_Popups.Length; i++)
		{
			if (this.m_Popups[i] != null)
			{
				this.m_Popups[i].SetPopupManager(this);
				this.m_Popups[i].gameObject.SetActive(false);
			}
		}
		this.m_PopupBaseCamera = this.m_BlurCameraSteam;
		if (this.m_BlurCameraMobile != null)
		{
			this.m_BlurCameraMobile.SetActive(false);
		}
		if (this.m_BlurCameraSteam != null)
		{
			this.m_BlurCameraSteam.SetActive(false);
		}
		if (this.m_PopupBasePanel != null)
		{
			this.m_PopupBasePanel.SetActive(false);
		}
		if (this.m_PopupBaseCamera != null)
		{
			this.m_PopupBaseCamera.SetActive(false);
		}
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0002A234 File Offset: 0x00028434
	public void SetPopup(EPopups popupType)
	{
		if (popupType == this.m_ActivePopup || (popupType == this.m_HiddenPopup && popupType != EPopups.NONE))
		{
			return;
		}
		if (this.m_PopupBasePanel != null)
		{
			this.m_PopupBasePanel.SetActive(popupType != EPopups.NONE || this.m_HiddenPopup > EPopups.NONE);
		}
		if (this.m_PopupBaseCamera != null)
		{
			this.m_PopupBaseCamera.SetActive(popupType > EPopups.NONE);
		}
		EPopups activePopup = this.m_ActivePopup;
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].DismissPopup();
		}
		if (this.m_Popups[(int)popupType] != null)
		{
			this.m_ActivePopup = popupType;
			this.m_Popups[(int)popupType].ActivatePopup();
		}
		else
		{
			this.m_ActivePopup = EPopups.NONE;
		}
		if (this.m_OnCurrentPopupChangedCallback != null)
		{
			this.m_OnCurrentPopupChangedCallback(this.m_ActivePopup, activePopup);
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0002A314 File Offset: 0x00028514
	public bool HideActivePopup(PopupBase popup)
	{
		if (this.m_Popups[(int)this.m_ActivePopup] == popup && this.m_HiddenPopup == EPopups.NONE)
		{
			this.m_HiddenPopup = this.m_ActivePopup;
			this.m_ActivePopup = EPopups.NONE;
			if (this.m_PopupBaseCamera != null)
			{
				this.m_PopupBaseCamera.SetActive(false);
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0002A370 File Offset: 0x00028570
	public bool RestoreHiddenPopup(PopupBase popup)
	{
		if (this.m_Popups[(int)this.m_HiddenPopup] == popup && this.m_ActivePopup == EPopups.NONE)
		{
			this.m_ActivePopup = this.m_HiddenPopup;
			this.m_HiddenPopup = EPopups.NONE;
			if (this.m_PopupBaseCamera != null)
			{
				this.m_PopupBaseCamera.SetActive(true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0002A3CC File Offset: 0x000285CC
	public void RebuildPopups()
	{
		if (this.m_Popups[(int)this.m_HiddenPopup] != null)
		{
			this.m_Popups[(int)this.m_HiddenPopup].RebuildPopup();
		}
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].RebuildPopup();
		}
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0002A427 File Offset: 0x00028627
	public void HandleTutorialFlags(uint tutorialFlags)
	{
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].HandleTutorialFlags(tutorialFlags);
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0002A451 File Offset: 0x00028651
	public void OnPopupActionButtonCancel()
	{
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].OnActionButtonCancel();
		}
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0002A47A File Offset: 0x0002867A
	public void OnPopupActionButtonCheck()
	{
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].OnActionButtonCheck();
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0002A4A3 File Offset: 0x000286A3
	public void OnPopupActionButtonCommit()
	{
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].OnActionButtonCommit();
		}
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0002A4CC File Offset: 0x000286CC
	public void OnPopupActionButtonRoll()
	{
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].OnActionButtonRoll();
		}
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0002A4F5 File Offset: 0x000286F5
	public void OnPopupActionButtonGeneric()
	{
		if (this.m_Popups[(int)this.m_ActivePopup] != null)
		{
			this.m_Popups[(int)this.m_ActivePopup].OnActionButtonGeneric();
		}
	}

	// Token: 0x04000527 RID: 1319
	[SerializeField]
	private PopupBase[] m_Popups;

	// Token: 0x04000528 RID: 1320
	public GameObject m_PopupBasePanel;

	// Token: 0x04000529 RID: 1321
	public GameObject m_BlurCameraMobile;

	// Token: 0x0400052A RID: 1322
	public GameObject m_BlurCameraSteam;

	// Token: 0x0400052B RID: 1323
	private PopupManager.PopupChangedCallback m_OnCurrentPopupChangedCallback;

	// Token: 0x0400052C RID: 1324
	private GameObject m_PopupBaseCamera;

	// Token: 0x0400052D RID: 1325
	private EPopups m_ActivePopup;

	// Token: 0x0400052E RID: 1326
	private EPopups m_HiddenPopup;

	// Token: 0x0200077F RID: 1919
	// (Invoke) Token: 0x06004221 RID: 16929
	public delegate void PopupChangedCallback(EPopups newPopup, EPopups oldPopup);
}
