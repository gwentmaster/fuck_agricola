using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class UI_OnlineProfile : UI_NetworkBase
{
	// Token: 0x06000B44 RID: 2884 RVA: 0x0004E47C File Offset: 0x0004C67C
	public void OnEnterMenu()
	{
		if (this.m_requestOnlineStatusArray == null)
		{
			this.m_requestOnlineStatusArray = new uint[2];
		}
		this.m_hUserDataBuffer = GCHandle.Alloc(this.m_requestOnlineStatusArray, GCHandleType.Pinned);
		if (this.m_bIgnoreNextEnter)
		{
			this.m_bIgnoreNextEnter = false;
			this.SetFriendButtonState();
			return;
		}
		this.DisplayPlayerProfile((this.m_requestedFriendId != 0U) ? this.m_requestedFriendId : AgricolaLib.NetworkGetLocalID());
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x0004E4E0 File Offset: 0x0004C6E0
	public void OnExitMenu(bool bUnderPopup)
	{
		if (bUnderPopup)
		{
			return;
		}
		if (this.m_delayCoroutine != null)
		{
			base.StopCoroutine(this.m_delayCoroutine);
			this.m_delayCoroutine = null;
		}
		this.m_hUserDataBuffer.Free();
		this.m_requestedFriendId = 0U;
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x0004E513 File Offset: 0x0004C713
	public void SetDisplayedPlayerID(uint userID)
	{
		this.m_requestedFriendId = userID;
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x0004E51C File Offset: 0x0004C71C
	private void DisplayPlayerProfile(uint userID)
	{
		if (!Network.m_Network.m_bConnectedToServer)
		{
			return;
		}
		this.m_delayCoroutine = null;
		this.m_bMinDialogDisplayTimeReached = false;
		this.m_bProcessFriendRequestReply = false;
		this.m_bDisplayingLocal = (Network.NetworkGetLocalID() == userID);
		if (this.m_bDisplayingLocal)
		{
			base.RequestLocalPlayerProfile(null);
			this.SetOnlineStatus(PlayerOnlineStatus.STATUS_ONLINE);
		}
		else
		{
			this.SetOnlineStatus(PlayerOnlineStatus.STATUS_UNKNOWN);
			this.m_requestOnlineStatusArray[0] = userID;
			Network.RequestUsersOnlineStatus(this.m_hUserDataBuffer.AddrOfPinnedObject(), 1);
			Network.RequestNetworkPlayerProfile(userID);
		}
		this.m_requestedFriendId = userID;
		this.m_bDisplayingFriend = false;
		List<FriendInfo> friendsList = Network.GetFriendsList();
		for (int i = 0; i < friendsList.Count; i++)
		{
			if (friendsList[i].userID == userID)
			{
				this.m_bDisplayingFriend = true;
				break;
			}
		}
		this.m_LoadingBase.SetActive(true);
		this.m_InfoBase.SetActive(false);
		this.m_addRemoveFriendButtonBase.SetActive(false);
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0004E5FC File Offset: 0x0004C7FC
	public void SetFriendButtonState()
	{
		if (this.m_addRemoveFriendButtonBase != null)
		{
			this.m_addRemoveFriendButtonBase.SetActive(!this.m_bDisplayingLocal);
			this.m_addRemoveFriendButtonText.KeyText = (this.m_bDisplayingFriend ? "Key_RemoveFriend" : "Key_AddFriend");
		}
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x0004E64A File Offset: 0x0004C84A
	public bool GetIsFriend()
	{
		return this.m_bDisplayingFriend;
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x0004E654 File Offset: 0x0004C854
	public void FriendButtonClicked()
	{
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bIgnoreNextEnter = true;
				string messageKey = this.m_bDisplayingFriend ? "Key_RemoveFriendPrompt" : "Key_AddFriendPrompt";
				component.Setup(new UI_ConfirmPopup.ClickCallback(this.FriendButtonClickedConfirm), messageKey, UI_ConfirmPopup.ButtonFormat.TwoButtons);
				ScreenManager.instance.PushScene("ConfirmPopup");
			}
		}
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0004E6C8 File Offset: 0x0004C8C8
	public void FriendButtonClickedConfirm(bool bConfirm)
	{
		if (!bConfirm)
		{
			return;
		}
		if (this.m_bDisplayingFriend)
		{
			if (this.m_requestedFriendId != 0U)
			{
				GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
				if (scene != null)
				{
					UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
					if (component)
					{
						this.m_bIgnoreNextEnter = true;
						component.Setup(null, "Key_RemovingFriend", UI_ConfirmPopup.ButtonFormat.NoButtons);
						ScreenManager.instance.PushScene("ConfirmPopup");
					}
				}
				this.m_delayCoroutine = base.StartCoroutine(this.ProcessDelayTime(this.m_minDialogDisplayTime));
				Network.RemoveFriendWithUserId(this.m_requestedFriendId);
				return;
			}
		}
		else if (this.m_requestedFriendId != 0U)
		{
			int num = Network.AddFriendFromUsername(this.m_displayNameLabel.text);
			if (num != 0)
			{
				GameObject scene2 = ScreenManager.instance.GetScene("ConfirmPopup");
				if (scene2 != null)
				{
					UI_ConfirmPopup component2 = scene2.GetComponent<UI_ConfirmPopup>();
					if (component2)
					{
						this.m_bIgnoreNextEnter = true;
						component2.Setup(null, "Key_AddingFriend", UI_ConfirmPopup.ButtonFormat.NoButtons);
						ScreenManager.instance.PushScene("ConfirmPopup");
					}
				}
				this.m_delayCoroutine = base.StartCoroutine(this.ProcessDelayTime(this.m_minDialogDisplayTime));
				return;
			}
			this.ProcessFriendRequestReply(num);
		}
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0004E7EC File Offset: 0x0004C9EC
	private void ProcessFriendRequestReply(int eventData)
	{
		this.m_bProcessFriendRequestReply = false;
		string text = string.Empty;
		switch (eventData)
		{
		case 1:
			text = "Key_FriendAddSuccess";
			this.m_bDisplayingFriend = true;
			this.SetFriendButtonState();
			break;
		case 2:
			text = "Key_FriendRmvSuccess";
			this.m_bDisplayingFriend = false;
			this.SetFriendButtonState();
			break;
		case 3:
			text = "Key_FriendNotFound";
			break;
		case 4:
			text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_DuplicateFriendFound}");
			text = text.Replace("%s", "{0}");
			text = string.Format(text, this.m_displayNameLabel.text);
			break;
		case 5:
			text = "Key_FriendUpdateFailed";
			break;
		}
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bIgnoreNextEnter = true;
				component.Setup(null, text, UI_ConfirmPopup.ButtonFormat.OneButton);
				if (!ScreenManager.instance.GetIsSceneInStack("ConfirmPopup"))
				{
					ScreenManager.instance.PushScene("ConfirmPopup");
				}
			}
		}
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0004E8EC File Offset: 0x0004CAEC
	private void DisplayProfileInfo(NetworkPlayerProfileInfo profileInfo)
	{
		this.m_LoadingBase.SetActive(false);
		this.m_InfoBase.SetActive(true);
		if (this.m_avatar != null)
		{
			this.m_avatar.SetAvatar((int)((profileInfo.userAvatar != 0) ? profileInfo.userAvatar : 1), false);
		}
		if (profileInfo.userAvatar == 0 && this.m_bDisplayingLocal)
		{
			AgricolaLib.NetworkSendLocalAvatarIndex(1U);
		}
		this.m_displayNameLabel.text = profileInfo.displayName;
		this.m_ratingNumLabel.text = profileInfo.userGameStats.userRating.ToString();
		this.m_gamesCompletedNumLabel.text = profileInfo.userGameStats.completedGames.ToString();
		this.m_inProgressNumLabel.text = profileInfo.inProgressGames.ToString();
		this.m_forfeitsNumLabel.text = profileInfo.userGameStats.forfeits.ToString();
		this.m_2PlayerRecordLabel.text = string.Format("{0} - {1}", profileInfo.userGameStats.wins[0], profileInfo.userGameStats.losses[0]);
		this.m_3PlayerRecordLabel.text = string.Format("{0} - {1}", profileInfo.userGameStats.wins[1], profileInfo.userGameStats.losses[1]);
		this.m_4PlayerRecordLabel.text = string.Format("{0} - {1}", profileInfo.userGameStats.wins[2], profileInfo.userGameStats.losses[2]);
		this.SetFriendButtonState();
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0004EA80 File Offset: 0x0004CC80
	private void SetOnlineStatus(PlayerOnlineStatus status)
	{
		for (int i = 0; i < this.m_onlineStatusObjs.Length; i++)
		{
			if (this.m_onlineStatusObjs[i] = null)
			{
				this.m_onlineStatusObjs[i].SetActive(i == (int)status);
			}
		}
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0004EAC8 File Offset: 0x0004CCC8
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
		if (eventType == NetworkEvent.EventType.Event_UpdatedPlayerProfile)
		{
			if (this.m_requestedFriendId == (uint)eventData)
			{
				if (this.m_bDisplayingLocal)
				{
					NetworkPlayerProfileInfo profileInfo;
					Network.GetLocalPlayerProfileInfo(out profileInfo);
					this.DisplayProfileInfo(profileInfo);
					return;
				}
				NetworkPlayerProfileInfo profileInfo2;
				Network.GetRemotePlayerProfileInfo((int)this.m_requestedFriendId, out profileInfo2);
				this.DisplayProfileInfo(profileInfo2);
				return;
			}
		}
		else if (eventType == NetworkEvent.EventType.Event_UpdatedOnlineStatus)
		{
			PlayerOnlineStatus userOnlineStatus = (PlayerOnlineStatus)Network.GetUserOnlineStatus((uint)eventData);
			if (eventData == (int)this.m_requestedFriendId)
			{
				this.SetOnlineStatus(userOnlineStatus);
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

	// Token: 0x06000B52 RID: 2898 RVA: 0x0004EB59 File Offset: 0x0004CD59
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

	// Token: 0x04000C08 RID: 3080
	public float m_minDialogDisplayTime;

	// Token: 0x04000C09 RID: 3081
	public GameObject m_LoadingBase;

	// Token: 0x04000C0A RID: 3082
	public GameObject m_InfoBase;

	// Token: 0x04000C0B RID: 3083
	public GameObject m_addRemoveFriendButtonBase;

	// Token: 0x04000C0C RID: 3084
	public UILocalizedText m_addRemoveFriendButtonText;

	// Token: 0x04000C0D RID: 3085
	public GameObject[] m_onlineStatusObjs;

	// Token: 0x04000C0E RID: 3086
	public TextMeshProUGUI m_displayNameLabel;

	// Token: 0x04000C0F RID: 3087
	public TextMeshProUGUI m_ratingNumLabel;

	// Token: 0x04000C10 RID: 3088
	public TextMeshProUGUI m_gamesCompletedNumLabel;

	// Token: 0x04000C11 RID: 3089
	public TextMeshProUGUI m_forfeitsNumLabel;

	// Token: 0x04000C12 RID: 3090
	public TextMeshProUGUI m_inProgressNumLabel;

	// Token: 0x04000C13 RID: 3091
	public TextMeshProUGUI m_2PlayerRecordLabel;

	// Token: 0x04000C14 RID: 3092
	public TextMeshProUGUI m_3PlayerRecordLabel;

	// Token: 0x04000C15 RID: 3093
	public TextMeshProUGUI m_4PlayerRecordLabel;

	// Token: 0x04000C16 RID: 3094
	public TextMeshProUGUI m_5PlayerRecordLabel;

	// Token: 0x04000C17 RID: 3095
	public TextMeshProUGUI m_6PlayerRecordLabel;

	// Token: 0x04000C18 RID: 3096
	public Avatar_UI m_avatar;

	// Token: 0x04000C19 RID: 3097
	private uint[] m_requestOnlineStatusArray;

	// Token: 0x04000C1A RID: 3098
	private GCHandle m_hUserDataBuffer;

	// Token: 0x04000C1B RID: 3099
	private bool m_bDisplayingFriend;

	// Token: 0x04000C1C RID: 3100
	private bool m_bDisplayingLocal;

	// Token: 0x04000C1D RID: 3101
	private bool m_bIgnoreNextEnter;

	// Token: 0x04000C1E RID: 3102
	private uint m_requestedFriendId;

	// Token: 0x04000C1F RID: 3103
	private int m_processFriendRequestReplyData;

	// Token: 0x04000C20 RID: 3104
	private bool m_bMinDialogDisplayTimeReached;

	// Token: 0x04000C21 RID: 3105
	private bool m_bProcessFriendRequestReply;

	// Token: 0x04000C22 RID: 3106
	private Coroutine m_delayCoroutine;
}
