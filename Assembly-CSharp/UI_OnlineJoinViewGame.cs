using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000125 RID: 293
public class UI_OnlineJoinViewGame : UI_NetworkBase
{
	// Token: 0x06000B28 RID: 2856 RVA: 0x0004D16C File Offset: 0x0004B36C
	public void OnEnterMenu()
	{
		this.m_delayCoroutine = null;
		this.m_bMinDialogDisplayTimeReached = false;
		this.m_bProcessCreateGameReply = false;
		if (this.m_bHandlePopup)
		{
			this.m_bHandlePopup = false;
			base.StartCoroutine(this.HandleBackFromPopup());
			this.m_bCreateSuccess = false;
			return;
		}
		if (this.m_bJoinNextEnter)
		{
			this.m_bJoinNextEnter = false;
			base.StartCoroutine(this.DelayJoinAfterSuccess());
		}
		this.SetGameData();
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0004D1D4 File Offset: 0x0004B3D4
	private void SetGameData()
	{
		if (this.m_requestOnlineStatusArray == null)
		{
			this.m_requestOnlineStatusArray = new uint[4];
		}
		FriendInfo friendInfo = default(FriendInfo);
		friendInfo.displayName = "(OPEN)";
		friendInfo.userAvatar = 0;
		friendInfo.userID = 0U;
		friendInfo.userRating = 0;
		if (!this.m_bAlloced)
		{
			this.m_hUserDataBuffer = GCHandle.Alloc(this.m_requestOnlineStatusArray, GCHandleType.Pinned);
			this.m_bAlloced = true;
		}
		uint num = this.m_gameData.packedPlayerCount >> 16 & 65535U;
		this.m_startGameObj.SetActive(this.m_bIsPlayerInvited || (this.m_bIsPlayerOwner && this.m_bIsGameReadyToStart) || !this.m_bIsPlayerInGame);
		if (this.m_bIsPlayerInvited || !this.m_bIsPlayerInGame)
		{
			this.m_startGameText.KeyText = "${Key_JoinGame}";
		}
		else
		{
			this.m_startGameText.KeyText = "${Key_ViewGame}";
		}
		int num2 = 0;
		for (uint num3 = 0U; num3 < num; num3 += 1U)
		{
			FriendInfo friendInfo2 = default(FriendInfo);
			int colorIndex = 6;
			int avatar = 0;
			UIP_GameSlot.ENetworkUserState enetworkUserState = UIP_GameSlot.ENetworkUserState.E_USERSTATE_OFFLINE;
			switch (num3)
			{
			case 0U:
				friendInfo2.displayName = this.m_gameData.player1Name;
				friendInfo2.userAvatar = (ushort)(this.m_gameData.player1Avatar >> 16 & 65535);
				avatar = (this.m_gameData.player1Avatar & 65535);
				friendInfo2.userID = (uint)this.m_gameData.player1ID;
				friendInfo2.userRating = this.m_gameData.player1Rating;
				enetworkUserState = (UIP_GameSlot.ENetworkUserState)this.m_gameData.player1State;
				colorIndex = this.m_gameData.player1Faction;
				break;
			case 1U:
				friendInfo2.displayName = this.m_gameData.player2Name;
				friendInfo2.userAvatar = (ushort)(this.m_gameData.player2Avatar >> 16 & 65535);
				avatar = (this.m_gameData.player2Avatar & 65535);
				friendInfo2.userID = (uint)this.m_gameData.player2ID;
				friendInfo2.userRating = this.m_gameData.player2Rating;
				enetworkUserState = (UIP_GameSlot.ENetworkUserState)this.m_gameData.player2State;
				colorIndex = this.m_gameData.player2Faction;
				break;
			case 2U:
				friendInfo2.displayName = this.m_gameData.player3Name;
				friendInfo2.userAvatar = (ushort)(this.m_gameData.player3Avatar >> 16 & 65535);
				avatar = (this.m_gameData.player3Avatar & 65535);
				friendInfo2.userID = (uint)this.m_gameData.player3ID;
				friendInfo2.userRating = this.m_gameData.player3Rating;
				enetworkUserState = (UIP_GameSlot.ENetworkUserState)this.m_gameData.player3State;
				colorIndex = this.m_gameData.player3Faction;
				break;
			case 3U:
				friendInfo2.displayName = this.m_gameData.player4Name;
				friendInfo2.userAvatar = (ushort)(this.m_gameData.player4Avatar >> 16 & 65535);
				avatar = (this.m_gameData.player4Avatar & 65535);
				friendInfo2.userID = (uint)this.m_gameData.player4ID;
				friendInfo2.userRating = this.m_gameData.player4Rating;
				enetworkUserState = (UIP_GameSlot.ENetworkUserState)this.m_gameData.player4State;
				colorIndex = this.m_gameData.player4Faction;
				break;
			case 4U:
				friendInfo2.displayName = this.m_gameData.player5Name;
				friendInfo2.userAvatar = (ushort)(this.m_gameData.player5Avatar >> 16 & 65535);
				avatar = (this.m_gameData.player5Avatar & 65535);
				friendInfo2.userID = (uint)this.m_gameData.player5ID;
				friendInfo2.userRating = this.m_gameData.player5Rating;
				colorIndex = this.m_gameData.player5Faction;
				break;
			case 5U:
				friendInfo2.displayName = this.m_gameData.player6Name;
				friendInfo2.userAvatar = (ushort)(this.m_gameData.player6Avatar >> 16 & 65535);
				avatar = (this.m_gameData.player6Avatar & 65535);
				friendInfo2.userID = (uint)this.m_gameData.player6ID;
				friendInfo2.userRating = this.m_gameData.player6Rating;
				colorIndex = this.m_gameData.player6Faction;
				break;
			}
			if (friendInfo2.userID == 0U)
			{
				friendInfo2 = friendInfo;
				avatar = 0;
				colorIndex = 6;
			}
			else
			{
				this.m_requestOnlineStatusArray[num2++] = friendInfo2.userID;
			}
			if (this.m_playerSlots[(int)num3].onlineStatusObj != null)
			{
				this.m_playerSlots[(int)num3].onlineStatusObj.SetActive(false);
			}
			if (this.m_playerSlots[(int)num3].invitedObj != null)
			{
				this.m_playerSlots[(int)num3].invitedObj.SetActive(enetworkUserState == UIP_GameSlot.ENetworkUserState.E_USERSTATE_INVITED);
			}
			this.PopulateSlot(friendInfo2, (int)num3, avatar, colorIndex);
		}
		this.m_playerSlots[2].root.SetActive(num >= 3U);
		this.m_playerSlots[3].root.SetActive(num >= 4U);
		this.m_playerSlots[4].root.SetActive(num >= 5U);
		this.m_playerSlots[5].root.SetActive(num >= 6U);
		if (num2 > 0)
		{
			Network.RequestUsersOnlineStatus(this.m_hUserDataBuffer.AddrOfPinnedObject(), num2);
		}
		uint deckFlags = (uint)this.m_gameData.deckFlags;
		for (int i = 0; i < this.m_setToggles.Length; i++)
		{
			this.m_setToggles[i].interactable = false;
			this.m_setToggles[i].isOn = ((1L << (i & 31) & (long)((ulong)deckFlags)) != 0L);
		}
		this.m_typeToggles[0].isOn = (this.m_gameData.gameType == 0);
		this.m_typeToggles[1].isOn = (this.m_gameData.gameType == 1);
		this.m_typeToggles[2].isOn = (this.m_gameData.gameType == 3);
		this.m_typeToggles[3].isOn = (this.m_gameData.gameType == 2);
		for (int j = 0; j < this.m_setLocks.Length; j++)
		{
			if (this.m_setLocks[j] != null)
			{
				this.m_setLocks[j].SetActive(false);
			}
		}
		this.UpdateTimerLabel();
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0004D81C File Offset: 0x0004BA1C
	public void OnExitMenu(bool bUnderPopup)
	{
		if (bUnderPopup || this.m_bHandlePopup)
		{
			return;
		}
		if (this.m_delayCoroutine != null)
		{
			base.StopCoroutine(this.m_delayCoroutine);
			this.m_delayCoroutine = null;
		}
		if (this.m_bAlloced)
		{
			this.m_hUserDataBuffer.Free();
			this.m_bAlloced = false;
		}
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x0004D86A File Offset: 0x0004BA6A
	private IEnumerator HandleBackFromPopup()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (this.m_bkgAnimator != null)
		{
			if (ScreenManager.instance.GetIsSceneInStack("FindGame"))
			{
				this.m_bkgAnimator.Play("CreateOnline_OnlineGames");
			}
			else
			{
				this.m_bkgAnimator.Play("CreateOnline_OnlineGames");
			}
		}
		ScreenManager.instance.GoToScene("OnlineGameList", true);
		yield break;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x0004D87C File Offset: 0x0004BA7C
	private void PopulateSlot(FriendInfo friend, int index, int avatar2, int colorIndex)
	{
		if (index >= 0 && index < this.m_playerSlots.Length)
		{
			this.m_playerSlots[index].name.text = friend.displayName;
			this.m_playerSlots[index].rating.text = friend.userRating.ToString();
			this.m_playerSlots[index].avatar.SetAvatar((int)friend.userAvatar, true);
			this.m_playerSlots[index].avatar2.SetAvatar(avatar2, true);
			this.m_playerSlots[index].playerID = friend.userID;
			this.m_playerSlots[index].openRoot.SetActive(friend.userID == 0U);
			this.m_playerSlots[index].humanRoot.SetActive(friend.userID > 0U);
			this.m_playerSlots[index].m_colorizer.Colorize((uint)colorIndex);
		}
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0004D980 File Offset: 0x0004BB80
	public void OnPlayerPressed(int index)
	{
		GameObject scene = ScreenManager.instance.GetScene("OnlineProfile");
		if (scene != null && this.m_playerSlots[index].playerID != 0U)
		{
			scene.GetComponent<UI_OnlineProfile>().SetDisplayedPlayerID(this.m_playerSlots[index].playerID);
			ScreenManager.instance.PushScene("OnlineProfile");
		}
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0004D9E4 File Offset: 0x0004BBE4
	public void SetGameData(ShortSaveStruct data, bool bIsPlayerInvited, bool bIsGameReadyToStart)
	{
		this.m_gameData = data;
		this.m_bIsPlayerOwner = ((long)data.player1ID == (long)((ulong)UI_NetworkBase.m_localPlayerProfile.userID));
		this.m_bIsPlayerInvited = bIsPlayerInvited;
		this.m_bIsGameReadyToStart = bIsGameReadyToStart;
		if (this.m_bIsPlayerOwner)
		{
			this.m_bIsPlayerInGame = true;
			return;
		}
		if (this.m_bIsPlayerInvited)
		{
			this.m_bIsPlayerInGame = false;
			return;
		}
		if ((long)data.player2ID == (long)((ulong)UI_NetworkBase.m_localPlayerProfile.userID) || (long)data.player3ID == (long)((ulong)UI_NetworkBase.m_localPlayerProfile.userID) || (long)data.player4ID == (long)((ulong)UI_NetworkBase.m_localPlayerProfile.userID))
		{
			this.m_bIsPlayerInGame = true;
			return;
		}
		this.m_bIsPlayerInGame = false;
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0004DA8B File Offset: 0x0004BC8B
	public void OnConfirmPopup(bool bConfirm)
	{
		if (bConfirm)
		{
			if (this.m_bIsPlayerOwner)
			{
				AgricolaLib.NetworkDeleteGame((uint)this.m_gameData.gameID);
				return;
			}
			AgricolaLib.NetworkWithdrawFromGame((uint)this.m_gameData.gameID);
		}
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0004DABC File Offset: 0x0004BCBC
	public void OnJoinGameButtonPressed(bool bConfirm)
	{
		if (this.m_bIsPlayerOwner && this.m_bIsGameReadyToStart)
		{
			AgricolaLib.NetworkLaunchGame((uint)this.m_gameData.gameID);
			ScreenManager.instance.GoToScene("OnlineLobby", true);
			return;
		}
		if (bConfirm)
		{
			Network.SendPlayerParameters(new GamePlayerParameters
			{
				avatar1 = this.m_localProfile.gameAvatar1 + 10 * this.m_localProfile.factionIndex,
				avatar2 = this.m_localProfile.gameAvatar2 + 10 * this.m_localProfile.factionIndex,
				avatar3 = this.m_localProfile.gameAvatar3 + 10 * this.m_localProfile.factionIndex,
				avatar4 = this.m_localProfile.gameAvatar4 + 10 * this.m_localProfile.factionIndex,
				avatar5 = this.m_localProfile.gameAvatar5 + 10 * this.m_localProfile.factionIndex,
				avatarColorIndex = this.m_localProfile.factionIndex
			});
			if (this.m_bIsPlayerInvited)
			{
				AgricolaLib.NetworkAcceptGameInvite((uint)this.m_gameData.gameID);
				GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
				if (scene != null)
				{
					UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
					if (component)
					{
						this.m_bHandlePopup = true;
						component.Setup(null, "Key_JoinSuccess", UI_ConfirmPopup.ButtonFormat.NoButtons);
						base.StartCoroutine(this.DelayCloseAfterSuccess());
						ScreenManager.instance.PushScene("ConfirmPopup");
						return;
					}
				}
			}
			else if (!this.m_bIsPlayerInGame)
			{
				this.m_delayCoroutine = base.StartCoroutine(this.ProcessDelayTime(this.m_minDialogDisplayTime));
				GameObject scene2 = ScreenManager.instance.GetScene("ConfirmPopup");
				if (scene2 != null)
				{
					UI_ConfirmPopup component2 = scene2.GetComponent<UI_ConfirmPopup>();
					if (component2)
					{
						this.m_bHandlePopup = true;
						component2.Setup(null, "Key_Joining", UI_ConfirmPopup.ButtonFormat.NoButtons);
						ScreenManager.instance.PushScene("ConfirmPopup");
					}
				}
				AgricolaLib.NetworkJoinGame(this.m_gameData.gameID);
				this.m_savedAnalyticsString = "GameDetails_JoinGame";
				return;
			}
		}
		else
		{
			GameObject scene3 = ScreenManager.instance.GetScene(this.m_AvatarSelectionScreenName);
			if (scene3 != null)
			{
				this.m_localProfile.name = UI_NetworkBase.m_localPlayerProfile.displayName;
				this.m_localProfile.factionIndex = (byte)PlayerPrefs.GetInt("OnlineCreateGame_FactionIndex", 0);
				this.m_localProfile.gameAvatar1 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar1", 1);
				this.m_localProfile.gameAvatar2 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar2", 2);
				this.m_localProfile.gameAvatar3 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar3", 3);
				this.m_localProfile.gameAvatar4 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar4", 4);
				this.m_localProfile.gameAvatar5 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar5", 5);
				this.m_bJoinNextEnter = false;
				scene3.GetComponent<UI_AvatarSelect>().SetProfile(this.m_localProfile, new UI_AvatarSelect.AvatarCallback(this.HandleBackFromAvatarSelect), false, true, false);
				ScreenManager.instance.PushScene(this.m_AvatarSelectionScreenName);
			}
		}
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0004DDC8 File Offset: 0x0004BFC8
	public void HandleBackFromAvatarSelect(ProfileManager.OfflineProfileEntry profile, bool bConfirm)
	{
		if (bConfirm)
		{
			this.m_bJoinNextEnter = true;
			this.m_localProfile = profile;
			PlayerPrefs.SetInt("OnlineCreateGame_FactionIndex", (int)this.m_localProfile.factionIndex);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar1", (int)this.m_localProfile.gameAvatar1);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar2", (int)this.m_localProfile.gameAvatar2);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar3", (int)this.m_localProfile.gameAvatar3);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar4", (int)this.m_localProfile.gameAvatar4);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar5", (int)this.m_localProfile.gameAvatar5);
		}
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0004DE67 File Offset: 0x0004C067
	public void HandleCreateCanceled()
	{
		if (this.m_delayCoroutine != null)
		{
			base.StopCoroutine(this.m_delayCoroutine);
			this.m_delayCoroutine = null;
		}
		this.m_bMinDialogDisplayTimeReached = false;
		this.m_bProcessCreateGameReply = false;
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0004DE94 File Offset: 0x0004C094
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
		if (eventType == NetworkEvent.EventType.Event_JoinGameReply)
		{
			if (this.m_bMinDialogDisplayTimeReached)
			{
				this.ProcessJoinGameReply(eventData);
				return;
			}
			this.m_createGameReplyData = eventData;
			this.m_bProcessCreateGameReply = true;
			return;
		}
		else
		{
			if (eventType == NetworkEvent.EventType.Event_UpdatedOnlineStatus)
			{
				int userOnlineStatus = Network.GetUserOnlineStatus((uint)eventData);
				for (int i = 0; i < this.m_playerSlots.Length; i++)
				{
					if (this.m_playerSlots[i].playerID == (uint)eventData && this.m_playerSlots[i].onlineStatusObj != null)
					{
						this.m_playerSlots[i].onlineStatusObj.SetActive(true);
						this.m_playerSlots[i].onlineStatusImage.sprite = this.m_OnlineStatusImages[userOnlineStatus];
						return;
					}
				}
				return;
			}
			if (eventType == NetworkEvent.EventType.Event_UpdatedGameList && this.m_gameData.gameID != 0)
			{
				List<ShortSaveStruct> activeGameList = Network.GetActiveGameList();
				for (int j = 0; j < activeGameList.Count; j++)
				{
					if (this.m_gameData.gameID == activeGameList[j].gameID && !this.m_gameData.Equals(activeGameList[j]))
					{
						this.m_gameData = activeGameList[j];
						uint num = this.m_gameData.packedPlayerCount & 65535U;
						uint num2 = this.m_gameData.packedPlayerCount >> 16 & 65535U;
						this.m_bIsGameReadyToStart = (num >= num2);
						this.SetGameData();
					}
				}
			}
			return;
		}
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0004E013 File Offset: 0x0004C213
	private void UpdateTimerLabel()
	{
		this.m_timerValueText.KeyText = PlayerTimers.GetTimeStringFromSeconds((int)this.m_gameData.updateTime);
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0004E030 File Offset: 0x0004C230
	private IEnumerator DelayCloseAfterSuccess()
	{
		yield return new WaitForSeconds(1f);
		this.m_bHandlePopup = false;
		if (this.m_bkgAnimator != null)
		{
			if (ScreenManager.instance.GetIsSceneInStack("FindGame"))
			{
				this.m_bkgAnimator.Play("CreateOnline_OnlineGames");
			}
			else
			{
				this.m_bkgAnimator.Play("CreateOnline_OnlineGames");
			}
		}
		ScreenManager.instance.GoToScene("OnlineGameList", true);
		yield break;
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x0004E03F File Offset: 0x0004C23F
	private IEnumerator DelayJoinAfterSuccess()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		this.OnJoinGameButtonPressed(true);
		yield break;
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x0004E050 File Offset: 0x0004C250
	private void ProcessJoinGameReply(int eventData)
	{
		string messageKey = "";
		this.m_bCreateSuccess = true;
		switch (eventData)
		{
		case -1:
			messageKey = "Key_Joining";
			break;
		case 0:
			this.m_bProcessCreateGameReply = false;
			messageKey = "Key_JoinSuccess";
			base.StartCoroutine(this.DelayCloseAfterSuccess());
			break;
		case 1:
			this.m_bProcessCreateGameReply = true;
			messageKey = "Key_NoExist";
			this.m_bCreateSuccess = false;
			break;
		case 2:
			this.m_bProcessCreateGameReply = true;
			messageKey = "Key_FullGame";
			this.m_bCreateSuccess = false;
			break;
		}
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bHandlePopup = true;
				component.Setup(null, messageKey, this.m_bCreateSuccess ? UI_ConfirmPopup.ButtonFormat.NoButtons : UI_ConfirmPopup.ButtonFormat.OneButton);
			}
		}
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x0004E117 File Offset: 0x0004C317
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
		if (this.m_bProcessCreateGameReply)
		{
			this.ProcessJoinGameReply(this.m_createGameReplyData);
		}
		yield break;
	}

	// Token: 0x04000BDF RID: 3039
	public UI_OnlineJoinViewGame.PlayerSlot[] m_playerSlots;

	// Token: 0x04000BE0 RID: 3040
	public Toggle[] m_typeToggles;

	// Token: 0x04000BE1 RID: 3041
	public Toggle[] m_setToggles;

	// Token: 0x04000BE2 RID: 3042
	public GameObject[] m_setLocks;

	// Token: 0x04000BE3 RID: 3043
	public Sprite[] m_OnlineStatusImages;

	// Token: 0x04000BE4 RID: 3044
	public UILocalizedText m_timerValueText;

	// Token: 0x04000BE5 RID: 3045
	public GameObject m_startGameObj;

	// Token: 0x04000BE6 RID: 3046
	public UILocalizedText m_startGameText;

	// Token: 0x04000BE7 RID: 3047
	public float m_minDialogDisplayTime = 2f;

	// Token: 0x04000BE8 RID: 3048
	public string m_AvatarSelectionScreenName = "FamilySelection";

	// Token: 0x04000BE9 RID: 3049
	public Animator m_bkgAnimator;

	// Token: 0x04000BEA RID: 3050
	private Coroutine m_delayCoroutine;

	// Token: 0x04000BEB RID: 3051
	private int m_createGameReplyData;

	// Token: 0x04000BEC RID: 3052
	private bool m_bMinDialogDisplayTimeReached;

	// Token: 0x04000BED RID: 3053
	private bool m_bProcessCreateGameReply;

	// Token: 0x04000BEE RID: 3054
	private ShortSaveStruct m_gameData;

	// Token: 0x04000BEF RID: 3055
	private bool m_bHandlePopup;

	// Token: 0x04000BF0 RID: 3056
	private bool m_bJoinNextEnter;

	// Token: 0x04000BF1 RID: 3057
	private bool m_bCreateSuccess;

	// Token: 0x04000BF2 RID: 3058
	private bool m_bIsGameReadyToStart;

	// Token: 0x04000BF3 RID: 3059
	private bool m_bIsPlayerOwner;

	// Token: 0x04000BF4 RID: 3060
	private bool m_bIsPlayerInvited;

	// Token: 0x04000BF5 RID: 3061
	private bool m_bIsPlayerInGame;

	// Token: 0x04000BF6 RID: 3062
	private uint m_factionSelected;

	// Token: 0x04000BF7 RID: 3063
	private uint[] m_requestOnlineStatusArray;

	// Token: 0x04000BF8 RID: 3064
	private GCHandle m_hUserDataBuffer;

	// Token: 0x04000BF9 RID: 3065
	private bool m_bAlloced;

	// Token: 0x04000BFA RID: 3066
	private string m_savedAnalyticsString;

	// Token: 0x04000BFB RID: 3067
	private ProfileManager.OfflineProfileEntry m_localProfile = new ProfileManager.OfflineProfileEntry();

	// Token: 0x04000BFC RID: 3068
	private InAppPurchaseWrapper m_InAppPurchase;

	// Token: 0x020007FE RID: 2046
	[Serializable]
	public struct PlayerSlot
	{
		// Token: 0x04002DB7 RID: 11703
		public GameObject root;

		// Token: 0x04002DB8 RID: 11704
		public GameObject humanRoot;

		// Token: 0x04002DB9 RID: 11705
		public GameObject openRoot;

		// Token: 0x04002DBA RID: 11706
		public Avatar_UI avatar;

		// Token: 0x04002DBB RID: 11707
		public Avatar_UI avatar2;

		// Token: 0x04002DBC RID: 11708
		public TextMeshProUGUI name;

		// Token: 0x04002DBD RID: 11709
		public TextMeshProUGUI rating;

		// Token: 0x04002DBE RID: 11710
		public GameObject nameObj;

		// Token: 0x04002DBF RID: 11711
		public ColorByFaction m_colorizer;

		// Token: 0x04002DC0 RID: 11712
		[HideInInspector]
		public uint playerID;

		// Token: 0x04002DC1 RID: 11713
		public GameObject invitedObj;

		// Token: 0x04002DC2 RID: 11714
		public GameObject onlineStatusObj;

		// Token: 0x04002DC3 RID: 11715
		public Image onlineStatusImage;
	}
}
