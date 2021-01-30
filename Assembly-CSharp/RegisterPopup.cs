using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class RegisterPopup : MonoBehaviour
{
	// Token: 0x060008F6 RID: 2294 RVA: 0x0003D490 File Offset: 0x0003B690
	private void Awake()
	{
		this.m_notify = null;
		if (this.popupParent != null)
		{
			MonoBehaviour[] components = this.popupParent.GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				this.m_notify = (components[i] as INotifyParentMenu);
				if (this.m_notify != null)
				{
					break;
				}
			}
		}
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0003D4E2 File Offset: 0x0003B6E2
	public void OpenPopup()
	{
		base.gameObject.SetActive(true);
		if (this.m_notify != null)
		{
			this.m_notify.Notified(PopupNotificationType.PopupOpened, this.popupName, null);
		}
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0003D50B File Offset: 0x0003B70B
	public void PopupData(GameObject data)
	{
		if (this.m_notify != null)
		{
			this.m_notify.Notified(PopupNotificationType.PopupData, this.popupName, data);
		}
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0003D528 File Offset: 0x0003B728
	public void ClosePopupConfirm()
	{
		base.gameObject.SetActive(false);
		if (this.m_notify != null)
		{
			this.m_notify.Notified(PopupNotificationType.PopupClosedConfirm, this.popupName, null);
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0003D551 File Offset: 0x0003B751
	public void ClosePopupCancel()
	{
		base.gameObject.SetActive(false);
		if (this.m_notify != null)
		{
			this.m_notify.Notified(PopupNotificationType.PopupClosedCancel, this.popupName, null);
		}
	}

	// Token: 0x04000996 RID: 2454
	public GameObject popupParent;

	// Token: 0x04000997 RID: 2455
	public string popupName;

	// Token: 0x04000998 RID: 2456
	private INotifyParentMenu m_notify;
}
