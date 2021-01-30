using System;

// Token: 0x020000EF RID: 239
public interface INotifyParentMenu
{
	// Token: 0x060008A8 RID: 2216
	void Notified(PopupNotificationType type, string popupName, object data = null);
}
