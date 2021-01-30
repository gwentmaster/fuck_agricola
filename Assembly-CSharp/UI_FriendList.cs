using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011A RID: 282
public class UI_FriendList : UI_NetworkBase, INotifyParentMenu
{
	// Token: 0x06000A80 RID: 2688 RVA: 0x00003022 File Offset: 0x00001222
	private void Awake()
	{
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0004596C File Offset: 0x00043B6C
	public void OnEnterMenu()
	{
		if (this.m_bIgnoreNextEnter)
		{
			this.m_bIgnoreNextEnter = false;
			return;
		}
		this.m_delayCoroutine = null;
		this.m_bMinDialogDisplayTimeReached = false;
		this.m_bProcessFriendRequestReply = false;
		Network.ClearMonitorGame();
		this.m_friendsList.Initialize(Network.m_Network, new UIC_FriendsList.ClickCallback(this.HandleClickOnSlot), this);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x000459C0 File Offset: 0x00043BC0
	public void OnExitMenu(bool bUnderPopup)
	{
		if (this.m_bIgnoreNextEnter || bUnderPopup)
		{
			return;
		}
		if (this.m_delayCoroutine != null)
		{
			base.StopCoroutine(this.m_delayCoroutine);
			this.m_delayCoroutine = null;
		}
		this.m_friendsList.Destroy();
		this.m_Callback = null;
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x000459FC File Offset: 0x00043BFC
	public void Update()
	{
		if (this.m_nameRenamePopup.activeSelf)
		{
			if (!string.IsNullOrEmpty(this.m_nameRenameField.text) && this.m_emailField.interactable)
			{
				this.m_emailField.text = string.Empty;
				this.m_emailField.interactable = false;
			}
			else if (string.IsNullOrEmpty(this.m_nameRenameField.text) && !this.m_emailField.interactable)
			{
				this.m_emailField.interactable = true;
			}
			if (!string.IsNullOrEmpty(this.m_emailField.text) && this.m_nameRenameField.interactable)
			{
				this.m_nameRenameField.text = string.Empty;
				this.m_nameRenameField.interactable = false;
			}
			else if (string.IsNullOrEmpty(this.m_emailField.text) && !this.m_nameRenameField.interactable)
			{
				this.m_nameRenameField.interactable = true;
			}
			if (!this.m_nameRenameConfirmButton.interactable && (!string.IsNullOrEmpty(this.m_nameRenameField.text) || !string.IsNullOrEmpty(this.m_emailField.text)))
			{
				this.m_nameRenameConfirmButton.interactable = true;
				return;
			}
			if (this.m_nameRenamePopup.activeSelf && string.IsNullOrEmpty(this.m_nameRenameField.text) && string.IsNullOrEmpty(this.m_emailField.text))
			{
				this.m_nameRenameConfirmButton.interactable = false;
			}
		}
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00045B64 File Offset: 0x00043D64
	public void SetInviteCallback(UI_FriendList.InviteCallback callback)
	{
		this.m_Callback = callback;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00045B6D File Offset: 0x00043D6D
	public void OnAddFriendPressed()
	{
		this.OnNameRenamePressed(true);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00045B78 File Offset: 0x00043D78
	public void Notified(PopupNotificationType type, string childPopupName, object data = null)
	{
		switch (type)
		{
		case PopupNotificationType.PopupOpened:
		case PopupNotificationType.PopupData:
		case PopupNotificationType.PopupClosedCancel:
			break;
		case PopupNotificationType.PopupClosedConfirm:
			if (childPopupName.Equals("Add Friend") && (!string.IsNullOrEmpty(this.m_nameRenameField.text) || !string.IsNullOrEmpty(this.m_emailField.text)) && Network.m_Network.m_bConnectedToServer)
			{
				int num;
				if (!string.IsNullOrEmpty(this.m_nameRenameField.text))
				{
					num = Network.AddFriendFromUsername(this.m_nameRenameField.text);
				}
				else
				{
					num = ((Network.AddFriendFromEmail(this.m_emailField.text) != 0) ? 0 : 5);
					this.m_nameRenameField.text = this.m_emailField.text;
					this.m_emailField.text = string.Empty;
				}
				if (num == 0)
				{
					GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
					if (scene != null)
					{
						UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
						if (component)
						{
							component.Setup(null, "Key_AddingFriend", UI_ConfirmPopup.ButtonFormat.NoButtons);
							if (!this.m_bIgnoreNextEnter)
							{
								this.m_bIgnoreNextEnter = true;
								ScreenManager.instance.PushScene("ConfirmPopup");
							}
						}
					}
					this.m_delayCoroutine = base.StartCoroutine(this.ProcessDelayTime(this.m_minDialogDisplayTime));
					return;
				}
				this.ProcessFriendRequestReply(num);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00045CBC File Offset: 0x00043EBC
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
		if (eventType == NetworkEvent.EventType.Event_UpdatedPlayerProfile)
		{
			if (this.m_requestedFriendId == (uint)eventData)
			{
				this.m_requestedFriendId = 0U;
				return;
			}
		}
		else if (eventType == NetworkEvent.EventType.Event_FriendRequestReply)
		{
			if (this.m_bMinDialogDisplayTimeReached)
			{
				this.ProcessFriendRequestReply(eventData);
				return;
			}
			this.m_processFriendRequestReplyData = eventData;
			this.m_bProcessFriendRequestReply = true;
		}
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00045D0C File Offset: 0x00043F0C
	private void OnNameRenamePressed(bool bIsNewProfile)
	{
		this.m_nameRenameField.text = string.Empty;
		this.m_nameRenameField.interactable = true;
		this.m_emailField.text = string.Empty;
		this.m_emailField.interactable = true;
		this.m_nameRenamePopup.SetActive(true);
		this.m_nameRenameConfirmButton.interactable = false;
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00045D69 File Offset: 0x00043F69
	private IEnumerator ProcessDelayTime(float totalDelayTime)
	{
		float accumulatedTime = 0f;
		float previousTime = Time.time;
		this.m_bMinDialogDisplayTimeReached = false;
		while (accumulatedTime < totalDelayTime)
		{
			accumulatedTime += Time.time - previousTime;
			previousTime = Time.time;
			yield return new WaitForEndOfFrame();
		}
		this.m_bMinDialogDisplayTimeReached = true;
		if (this.m_bProcessFriendRequestReply)
		{
			this.ProcessFriendRequestReply(this.m_processFriendRequestReplyData);
		}
		yield break;
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00045D80 File Offset: 0x00043F80
	private void HandleClickOnSlot(UIP_FriendSlot slot, UIP_FriendSlot.ClickEventType evt)
	{
		if (slot != null && Network.m_Network.m_bConnectedToServer)
		{
			if (slot.GetFriendInfo().userRating > 0)
			{
				if (this.m_Callback != null)
				{
					FriendInfo friendInfo = slot.GetFriendInfo();
					this.m_Callback(friendInfo.displayName, friendInfo.userID, (int)friendInfo.userRating);
					ScreenManager.instance.PopScene();
				}
				else
				{
					GameObject scene = ScreenManager.instance.GetScene("OnlineProfile");
					if (scene != null)
					{
						UI_OnlineProfile component = scene.GetComponent<UI_OnlineProfile>();
						if (component != null)
						{
							component.SetDisplayedPlayerID(slot.GetFriendInfo().userID);
							ScreenManager.instance.PushScene("OnlineProfile");
						}
					}
				}
				this.m_currentFriendDisplayedID = slot.GetFriendInfo().userID;
				this.m_friendsList.SetSelectedSlot(slot);
				return;
			}
			GameObject scene2 = ScreenManager.instance.GetScene("ConfirmPopup");
			if (scene2 != null)
			{
				UI_ConfirmPopup component2 = scene2.GetComponent<UI_ConfirmPopup>();
				if (component2)
				{
					this.m_bIgnoreNextEnter = true;
					string messageKey = "Key_FriendNotConnected";
					component2.Setup(null, messageKey, UI_ConfirmPopup.ButtonFormat.OneButton);
					ScreenManager.instance.PushScene("ConfirmPopup");
				}
			}
			this.m_currentFriendDisplayedID = slot.GetFriendInfo().userID;
			this.m_friendsList.SetSelectedSlot(slot);
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00045EC8 File Offset: 0x000440C8
	private void RequestFriendProfile(FriendInfo friendInfo)
	{
		if (this.m_currentFriendDisplayedID != friendInfo.userID)
		{
			this.m_currentFriendDisplayName = friendInfo.displayName;
			this.m_currentFriendDisplayedID = friendInfo.userID;
			if (friendInfo.userAvatar > 0 && friendInfo.userRating != 0)
			{
				this.DisplayTopUserInfo(friendInfo.displayName, friendInfo.userRating, "");
				Network.RequestNetworkPlayerProfile(friendInfo.userID);
				this.m_requestedFriendId = friendInfo.userID;
				return;
			}
			this.DisplayTopUserInfo(friendInfo.displayName, 0, "No user profile found.");
		}
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x00003022 File Offset: 0x00001222
	private void DisplayTopUserInfo(string name, ushort rating, string altString)
	{
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x00045F50 File Offset: 0x00044150
	private void ProcessFriendRequestReply(int eventData)
	{
		this.m_bProcessFriendRequestReply = false;
		string messageKey = string.Empty;
		switch (eventData)
		{
		case 1:
			messageKey = "Key_AddFriendSuccess";
			break;
		case 2:
			this.m_friendsList.Remove(this.m_currentFriendDisplayedID);
			messageKey = "Key_FriendRmvSuccess";
			break;
		case 3:
			messageKey = "Key_AddFriendNotFound";
			break;
		case 4:
			messageKey = "Key_AddFriendAlready";
			break;
		case 5:
			messageKey = "Key_AddFriendFailed";
			break;
		}
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				component.Setup(null, messageKey, UI_ConfirmPopup.ButtonFormat.OneButton);
				if (!this.m_bIgnoreNextEnter)
				{
					this.m_bIgnoreNextEnter = true;
					ScreenManager.instance.PushScene("ConfirmPopup");
				}
			}
		}
	}

	// Token: 0x04000B20 RID: 2848
	public float m_minDialogDisplayTime;

	// Token: 0x04000B21 RID: 2849
	public UIC_FriendsList m_friendsList;

	// Token: 0x04000B22 RID: 2850
	public GameObject m_nameRenamePopup;

	// Token: 0x04000B23 RID: 2851
	public TMP_InputField m_nameRenameField;

	// Token: 0x04000B24 RID: 2852
	public TMP_InputField m_emailField;

	// Token: 0x04000B25 RID: 2853
	public Button m_nameRenameConfirmButton;

	// Token: 0x04000B26 RID: 2854
	private UI_FriendList.InviteCallback m_Callback;

	// Token: 0x04000B27 RID: 2855
	private Coroutine m_delayCoroutine;

	// Token: 0x04000B28 RID: 2856
	private string m_currentFriendDisplayName;

	// Token: 0x04000B29 RID: 2857
	private uint m_currentFriendDisplayedID;

	// Token: 0x04000B2A RID: 2858
	private uint m_requestedFriendId;

	// Token: 0x04000B2B RID: 2859
	private int m_processFriendRequestReplyData;

	// Token: 0x04000B2C RID: 2860
	private bool m_bMinDialogDisplayTimeReached;

	// Token: 0x04000B2D RID: 2861
	private bool m_bProcessFriendRequestReply;

	// Token: 0x04000B2E RID: 2862
	private bool m_bIgnoreToggles;

	// Token: 0x04000B2F RID: 2863
	private bool m_bIgnoreNextEnter;

	// Token: 0x020007F3 RID: 2035
	// (Invoke) Token: 0x06004395 RID: 17301
	public delegate void InviteCallback(string displayName, uint userID, int rating);
}
