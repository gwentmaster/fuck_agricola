using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
public abstract class UI_NetworkBase : MonoBehaviour
{
	// Token: 0x06000A15 RID: 2581
	protected abstract void NetworkStart();

	// Token: 0x06000A16 RID: 2582
	protected abstract void NetworkOnDestroy();

	// Token: 0x06000A17 RID: 2583 RVA: 0x00043559 File Offset: 0x00041759
	private void OnEnable()
	{
		this.m_bIgnoreNetworkEvents = false;
		this.AddNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		this.NetworkStart();
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0004357B File Offset: 0x0004177B
	private void OnDisable()
	{
		this.RemoveNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		this.NetworkOnDestroy();
		this.m_bIgnoreNetworkEvents = false;
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0004359D File Offset: 0x0004179D
	private void AddNetworkEventHandler(Network.NetworkEventDelegate d)
	{
		if (Network.m_Network != null)
		{
			Network.m_Network.AddNetworkEventHandler(d);
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x000435B7 File Offset: 0x000417B7
	private void RemoveNetworkEventHandler(Network.NetworkEventDelegate d)
	{
		if (Network.m_Network != null)
		{
			Network.m_Network.RemoveNetworkEventHandler(d);
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x000435D1 File Offset: 0x000417D1
	private void ClearLocalPlayerProfile()
	{
		UI_NetworkBase.m_localUserID = 0U;
		UI_NetworkBase.m_bRequestingLocalPlayerProfile = false;
		this.m_profileUpdatedCallback = null;
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x000435E6 File Offset: 0x000417E6
	protected bool RequestLocalPlayerProfile(UI_NetworkBase.LocalPlayerProfileUpdatedCallback callback)
	{
		if (!UI_NetworkBase.m_bRequestingLocalPlayerProfile)
		{
			Network.RequestNetworkPlayerProfile(UI_NetworkBase.m_localUserID);
			UI_NetworkBase.m_bRequestingLocalPlayerProfile = true;
		}
		if (this.m_profileUpdatedCallback == null)
		{
			this.m_profileUpdatedCallback = callback;
		}
		else if (callback != null)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00043618 File Offset: 0x00041818
	protected void HandleUpdatedPlayerProfileEvent(int eventData)
	{
		if (UI_NetworkBase.m_localUserID == (uint)eventData)
		{
			Network.GetLocalPlayerProfileInfo(out UI_NetworkBase.m_localPlayerProfile);
			UI_NetworkBase.m_bRequestingLocalPlayerProfile = false;
			bool flag = false;
			if (UI_NetworkBase.m_localPlayerProfile.userAvatar == 0)
			{
				UI_NetworkBase.m_localPlayerProfile.userAvatar = 1;
				flag = true;
			}
			if (flag)
			{
				Network.SendLocalAvatarIndex((uint)UI_NetworkBase.m_localPlayerProfile.userAvatar);
			}
			if (this.m_profileUpdatedCallback != null)
			{
				this.m_profileUpdatedCallback();
				this.m_profileUpdatedCallback = null;
			}
		}
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x00043688 File Offset: 0x00041888
	protected virtual void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		if (this.m_bIgnoreNetworkEvents)
		{
			return;
		}
		if (eventType != NetworkEvent.EventType.Event_LoginComplete)
		{
			if (eventType != NetworkEvent.EventType.Event_ConnectionLost)
			{
				if (eventType != NetworkEvent.EventType.Event_UpdatedPlayerProfile)
				{
					return;
				}
				this.HandleUpdatedPlayerProfileEvent(eventData);
			}
			else
			{
				UI_NetworkBase.m_bRequestingLocalPlayerProfile = false;
				if (Network.m_Network.m_bServerConnectionLost)
				{
					this.TryNetworkReconnect();
					return;
				}
			}
			return;
		}
		UI_NetworkBase.m_localUserID = (uint)eventData;
		UI_NetworkBase.m_bRequestingLocalPlayerProfile = false;
		this.RequestLocalPlayerProfile(null);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x000436E1 File Offset: 0x000418E1
	protected void TryNetworkReconnect()
	{
		this.ClearLocalPlayerProfile();
		ScreenManager.instance.PushScene("Connecting");
	}

	// Token: 0x04000AA8 RID: 2728
	protected static NetworkPlayerProfileInfo m_localPlayerProfile;

	// Token: 0x04000AA9 RID: 2729
	protected static uint m_localUserID;

	// Token: 0x04000AAA RID: 2730
	protected bool m_bIgnoreNetworkEvents;

	// Token: 0x04000AAB RID: 2731
	protected static bool m_bRequestingLocalPlayerProfile;

	// Token: 0x04000AAC RID: 2732
	private UI_NetworkBase.LocalPlayerProfileUpdatedCallback m_profileUpdatedCallback;

	// Token: 0x020007E2 RID: 2018
	// (Invoke) Token: 0x0600435B RID: 17243
	public delegate void LocalPlayerProfileUpdatedCallback();
}
