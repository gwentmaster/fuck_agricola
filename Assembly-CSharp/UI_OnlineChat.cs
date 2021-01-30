using System;

// Token: 0x02000121 RID: 289
public class UI_OnlineChat : UI_NetworkBase
{
	// Token: 0x06000AE5 RID: 2789 RVA: 0x00003022 File Offset: 0x00001222
	private void Start()
	{
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0004A9FD File Offset: 0x00048BFD
	public void OnEnterMenu()
	{
		if (this.m_Popup_Chat != null)
		{
			this.m_Popup_Chat.UpdateDisplay(0U);
			this.m_Popup_Chat.SetLocalUsername(UI_NetworkBase.m_localPlayerProfile.displayName);
			this.m_Popup_Chat.DisplayLocalProfile();
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x00003022 File Offset: 0x00001222
	public void OnExitMenu(bool bUnderPopup)
	{
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
	}

	// Token: 0x04000BA4 RID: 2980
	public Popup_Chat m_Popup_Chat;
}
