using System;
using System.Collections;
using AsmodeeNet.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000124 RID: 292
public class UI_OnlineGames : UI_NetworkBase
{
	// Token: 0x06000B17 RID: 2839 RVA: 0x0004C570 File Offset: 0x0004A770
	public void OnEnterMenu()
	{
		if (this.m_bEnterFromPopup)
		{
			this.m_bEnterFromPopup = false;
			base.StartCoroutine(this.DelayDeleteMode());
			return;
		}
		if (!Network.m_Network.m_bConnectedToServer)
		{
			base.TryNetworkReconnect();
			return;
		}
		this.m_onlineGameList.Initialize(Network.m_Network, new UIC_OnlineGameList.ClickCallback(this.HandleClickOnSlot), this);
		Network.ClearMonitorGame();
		if (UI_NetworkBase.m_localPlayerProfile.userID == UI_NetworkBase.m_localUserID)
		{
			this.DisplayLocalPlayerProfile(UI_NetworkBase.m_localPlayerProfile.userID == UI_NetworkBase.m_localUserID);
		}
		else
		{
			base.RequestLocalPlayerProfile(null);
		}
		this.m_bIgnoreToggles = true;
		this.m_Toggle_DeleteMode.isOn = false;
		if (this.m_Toggle_ActiveGameList != null)
		{
			this.m_Toggle_ActiveGameList.isOn = true;
		}
		this.m_bIgnoreToggles = false;
		this.m_bDeleteMode = false;
		this.EnableActiveGameList();
		this.ToggleDeleteMode();
		if (this.m_deleteDisabled != null)
		{
			this.m_deleteDisabled.SetActive(this.m_onlineGameList.GetGameCount() == 0);
		}
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0004C66F File Offset: 0x0004A86F
	public void OnExitMenu(bool bUnderPopup)
	{
		if (bUnderPopup)
		{
			return;
		}
		base.StopAllCoroutines();
		this.m_onlineGameList.Destroy();
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0004C688 File Offset: 0x0004A888
	public void OnConfirmButtonPressed(bool bConfirm)
	{
		if (this.m_lastGameSlotSelected == null)
		{
			this.m_lastGameSlotSelected = this.m_onlineGameList.GetGameWithNetworkID(this.m_lastGameSlotSelectedID);
			if (this.m_lastGameSlotSelected == null)
			{
				return;
			}
		}
		this.m_lastGameSlotSelected.TurnOnDeleteMode(false);
		if (bConfirm)
		{
			switch (this.m_lastGameSlotClickEvent)
			{
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkLaunchGame:
				AgricolaLib.NetworkLaunchGame(this.m_lastGameSlotSelected.GetNetworkGameID());
				break;
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkAcceptInvite:
				AgricolaLib.NetworkAcceptGameInvite(this.m_lastGameSlotSelected.GetNetworkGameID());
				break;
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkResumeGame:
			{
				ShortSaveStruct gameSlotData = this.m_lastGameSlotSelected.GetGameSlotData();
				string matchSessionId = "Agr_" + this.m_lastGameSlotSelected.GetNetworkGameID().ToString();
				string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameSlotData.deckFlags);
				string launchMethod = "resume";
				if (!AgricolaLib.NetworkGameHasLocalPlayerMoves(this.m_lastGameSlotSelected.GetNetworkGameID()))
				{
					if (this.m_lastGameSlotSelected.GetIsOwner())
					{
						launchMethod = "create";
					}
					else if (this.m_lastGameSlotSelected.GetIsInvited())
					{
						launchMethod = "join_invite";
					}
					else
					{
						launchMethod = "join";
					}
				}
				AnalyticsEvents.LogMatchStartEvent(matchSessionId, string.Empty, "online", string.Empty, activatedDlc, this.m_lastGameSlotSelected.GetNumPlayers(), 0, null, launchMethod, new int?((int)gameSlotData.updateTime), string.Empty, true, false, new bool?(true), null, null, null, null, null, null);
				AgricolaLib.NetworkResumeGame((int)this.m_lastGameSlotSelected.GetNetworkGameID());
				ScreenManager.instance.LoadIntoGameScreen(this.m_lastGameSlotSelected.GetGameSlotData().roundNumber);
				break;
			}
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteGame:
				AgricolaLib.NetworkDeleteGame(this.m_lastGameSlotSelected.GetNetworkGameID());
				this.m_onlineGameList.RemoveGameSlotFromList(this.m_lastGameSlotSelected);
				if (this.m_deleteDisabled != null)
				{
					this.m_deleteDisabled.SetActive(this.m_onlineGameList.GetGameCount() == 0);
				}
				break;
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGame:
			{
				ShortSaveStruct gameSlotData2 = this.m_lastGameSlotSelected.GetGameSlotData();
				string matchSessionId2 = "Agr_" + this.m_lastGameSlotSelected.GetNetworkGameID().ToString();
				string[] activatedDlc2 = ThirdPartyManager.GenerateDlcString((uint)gameSlotData2.deckFlags);
				string launchMethod2 = "resume";
				if (!AgricolaLib.NetworkGameHasLocalPlayerMoves(this.m_lastGameSlotSelected.GetNetworkGameID()))
				{
					if (this.m_lastGameSlotSelected.GetIsOwner())
					{
						launchMethod2 = "create";
					}
					else if (this.m_lastGameSlotSelected.GetIsInvited())
					{
						launchMethod2 = "join_invite";
					}
					else
					{
						launchMethod2 = "join";
					}
				}
				AnalyticsEvents.LogMatchStartEvent(matchSessionId2, string.Empty, "online", string.Empty, activatedDlc2, this.m_lastGameSlotSelected.GetNumPlayers(), 0, null, launchMethod2, new int?((int)gameSlotData2.updateTime), string.Empty, true, false, new bool?(true), null, null, null, null, null, null);
				AnalyticsEvents.LogMatchStopEvent(this.m_lastGameSlotSelected.GetNumPlayers(), 0, "resign", new MATCH_STOP.player_result?(MATCH_STOP.player_result.defeat), null, null);
				AgricolaLib.NetworkForfeitGame(this.m_lastGameSlotSelected.GetNetworkGameID(), false);
				this.m_onlineGameList.RemoveGameSlotFromList(this.m_lastGameSlotSelected);
				if (this.m_deleteDisabled != null)
				{
					this.m_deleteDisabled.SetActive(this.m_onlineGameList.GetGameCount() == 0);
				}
				break;
			}
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGameLastPlayer:
				AgricolaLib.NetworkForfeitGame(this.m_lastGameSlotSelected.GetNetworkGameID(), true);
				this.m_onlineGameList.RemoveGameSlotFromList(this.m_lastGameSlotSelected);
				if (this.m_deleteDisabled != null)
				{
					this.m_deleteDisabled.SetActive(this.m_onlineGameList.GetGameCount() == 0);
				}
				break;
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkWithdrawFromGame:
				AgricolaLib.NetworkWithdrawFromGame(this.m_lastGameSlotSelected.GetNetworkGameID());
				this.m_onlineGameList.RemoveGameSlotFromList(this.m_lastGameSlotSelected);
				if (this.m_deleteDisabled != null)
				{
					this.m_deleteDisabled.SetActive(this.m_onlineGameList.GetGameCount() == 0);
				}
				break;
			case UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteMatchmakingGame:
				AgricolaLib.NetworkDeleteMatchmakingGame(this.m_lastGameSlotSelected.GetNetworkGameID());
				this.m_onlineGameList.RemoveGameSlotFromList(this.m_lastGameSlotSelected);
				if (this.m_deleteDisabled != null)
				{
					this.m_deleteDisabled.SetActive(this.m_onlineGameList.GetGameCount() == 0);
				}
				break;
			}
		}
		this.m_onlineGameList.SetAllDeleteMode(this.m_bDeleteMode);
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0004CB24 File Offset: 0x0004AD24
	public void OnGameListTogglePressed()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_toggleAudio != null)
		{
			this.m_toggleAudio.Play();
		}
		if (this.m_Toggle_ActiveGameList != null && this.m_Toggle_ActiveGameList.isOn && this.m_Toggle_CompletedGameList != null && !this.m_Toggle_CompletedGameList.isOn)
		{
			this.EnableActiveGameList();
			return;
		}
		if (this.m_Toggle_ActiveGameList != null && !this.m_Toggle_ActiveGameList.isOn && this.m_Toggle_CompletedGameList != null && this.m_Toggle_CompletedGameList.isOn)
		{
			this.EnableCompletedGameList();
		}
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0004CBCC File Offset: 0x0004ADCC
	public void ToggleDeleteMode()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		this.m_bDeleteMode = this.m_Toggle_DeleteMode.isOn;
		this.m_onlineGameList.SetAllDeleteMode(this.m_bDeleteMode);
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0004C20A File Offset: 0x0004A40A
	public void IgnoreNetworkEvents()
	{
		if (base.gameObject.activeSelf)
		{
			this.m_bIgnoreNetworkEvents = true;
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0004CBF9 File Offset: 0x0004ADF9
	public void EnableActiveGameList()
	{
		if (this.m_onlineGameList != null)
		{
			this.m_onlineGameList.EnableGameList(true, false, false);
		}
		if (this.m_ActiveListText != null)
		{
			this.m_ActiveListText.KeyText = "Active Games & Matchmaking Requests";
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0004CC2F File Offset: 0x0004AE2F
	public void EnableCompletedGameList()
	{
		if (this.m_onlineGameList != null)
		{
			this.m_onlineGameList.EnableGameList(true, true, false);
		}
		if (this.m_ActiveListText != null)
		{
			this.m_ActiveListText.KeyText = "Completed Games";
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0004CC65 File Offset: 0x0004AE65
	private IEnumerator CheckForNewChat(bool bIsCompleteList)
	{
		bool flag = true;
		for (;;)
		{
			yield return new WaitForSeconds((float)(60 + ((bIsCompleteList && flag) ? 30 : 0)));
			flag = false;
		}
		yield break;
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0004CC74 File Offset: 0x0004AE74
	private void HandleClickOnSlot(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt)
	{
		if (GameObject.FindGameObjectWithTag("IAP Manager") == null)
		{
			return;
		}
		string text = string.Empty;
		switch (evt)
		{
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkLaunchGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			if (this.m_joinGameScreen != null)
			{
				this.m_joinGameScreen.SetGameData(slot.GetGameSlotData(), false, true);
				ScreenManager.instance.PushScene("Join/View Game");
				goto IL_331;
			}
			AgricolaLib.NetworkLaunchGame(slot.GetNetworkGameID());
			goto IL_331;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkAcceptInvite:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			if (this.m_joinGameScreen != null)
			{
				this.m_joinGameScreen.SetGameData(slot.GetGameSlotData(), true, false);
				ScreenManager.instance.PushScene("Join/View Game");
				goto IL_331;
			}
			text = "Do you want to accept the game invite?";
			goto IL_331;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkResumeGame:
		{
			ShortSaveStruct gameSlotData = slot.GetGameSlotData();
			string matchSessionId = "Agr_" + slot.GetNetworkGameID().ToString();
			string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameSlotData.deckFlags);
			string launchMethod = "resume";
			if (!AgricolaLib.NetworkGameHasLocalPlayerMoves(slot.GetNetworkGameID()) && gameSlotData.roundNumber <= 1)
			{
				if (gameSlotData.rematchGameID != 0)
				{
					launchMethod = "rematch";
				}
				else if (slot.GetIsOwner())
				{
					launchMethod = "create";
				}
				else if (slot.GetIsInvited())
				{
					launchMethod = "join_invite";
				}
				else
				{
					launchMethod = "join";
				}
			}
			AnalyticsEvents.LogMatchStartEvent(matchSessionId, string.Empty, "online", string.Empty, activatedDlc, slot.GetNumPlayers(), 0, null, launchMethod, new int?((int)gameSlotData.updateTime), string.Empty, true, false, new bool?(true), null, null, null, null, null, null);
			AgricolaLib.NetworkResumeGame((int)slot.GetNetworkGameID());
			ScreenManager.instance.LoadIntoGameScreen(slot.GetGameSlotData().roundNumber);
			goto IL_331;
		}
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_DeleteGamePrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_331;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_ForfeitPrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_331;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGameLastPlayer:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_ForfeitAllOpp";
			slot.TurnOnDeleteMode(true);
			goto IL_331;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkWithdrawFromGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_DeleteGamePrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_331;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteMatchmakingGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_DeleteRequestPrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_331;
		}
		if (this.m_selectSlotAudio != null)
		{
			this.m_selectSlotAudio.Play();
		}
		if (this.m_joinGameScreen != null)
		{
			this.m_joinGameScreen.SetGameData(slot.GetGameSlotData(), false, false);
			ScreenManager.instance.PushScene("Join/View Game");
		}
		IL_331:
		if (text != string.Empty)
		{
			GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
			if (scene != null)
			{
				UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
				if (component)
				{
					this.m_bEnterFromPopup = true;
					component.Setup(new UI_ConfirmPopup.ClickCallback(this.OnConfirmButtonPressed), text, UI_ConfirmPopup.ButtonFormat.TwoButtons);
					ScreenManager.instance.PushScene("ConfirmPopup");
				}
			}
		}
		this.m_lastGameSlotClickEvent = evt;
		this.m_lastGameSlotSelected = slot;
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0004D025 File Offset: 0x0004B225
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
		if (eventType == NetworkEvent.EventType.Event_UpdatedPlayerProfile && (long)eventData == (long)((ulong)UI_NetworkBase.m_localUserID))
		{
			this.DisplayLocalPlayerProfile(true);
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0004D048 File Offset: 0x0004B248
	private void Update()
	{
		if (this.m_deleteDisabled != null)
		{
			bool flag = this.m_onlineGameList.GetGameCount() == 0;
			if (this.m_deleteDisabled.activeSelf != flag)
			{
				this.m_deleteDisabled.SetActive(flag);
			}
			if (flag && this.m_Toggle_DeleteMode.isOn)
			{
				this.m_Toggle_DeleteMode.isOn = false;
			}
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0004D0A8 File Offset: 0x0004B2A8
	private IEnumerator DelayDeleteMode()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		this.ToggleDeleteMode();
		yield break;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0004D0B8 File Offset: 0x0004B2B8
	private void DisplayLocalPlayerProfile(bool bVisible)
	{
		if (this.m_userAvatar == null || this.m_userNameLabel == null || this.m_userRatingLabel == null)
		{
			return;
		}
		this.m_userAvatar.enabled = bVisible;
		this.m_userNameLabel.enabled = bVisible;
		this.m_userRatingLabel.enabled = bVisible;
		if (bVisible)
		{
			this.m_userNameLabel.text = UI_NetworkBase.m_localPlayerProfile.displayName;
			this.m_userRatingLabel.text = UI_NetworkBase.m_localPlayerProfile.userGameStats.userRating.ToString();
			this.m_userAvatar.SetAvatar((int)UI_NetworkBase.m_localPlayerProfile.userAvatar, false);
			return;
		}
		base.RequestLocalPlayerProfile(null);
	}

	// Token: 0x04000BCD RID: 3021
	public UIC_OnlineGameList m_onlineGameList;

	// Token: 0x04000BCE RID: 3022
	public TextMeshProUGUI m_userNameLabel;

	// Token: 0x04000BCF RID: 3023
	public TextMeshProUGUI m_userRatingLabel;

	// Token: 0x04000BD0 RID: 3024
	public Avatar_UI m_userAvatar;

	// Token: 0x04000BD1 RID: 3025
	public Toggle m_Toggle_ActiveGameList;

	// Token: 0x04000BD2 RID: 3026
	public Toggle m_Toggle_CompletedGameList;

	// Token: 0x04000BD3 RID: 3027
	public Toggle m_Toggle_DeleteMode;

	// Token: 0x04000BD4 RID: 3028
	public GameObject m_deleteDisabled;

	// Token: 0x04000BD5 RID: 3029
	public UILocalizedText m_ActiveListText;

	// Token: 0x04000BD6 RID: 3030
	public AudioSource m_selectSlotAudio;

	// Token: 0x04000BD7 RID: 3031
	public AudioSource m_toggleAudio;

	// Token: 0x04000BD8 RID: 3032
	public UI_OnlineJoinViewGame m_joinGameScreen;

	// Token: 0x04000BD9 RID: 3033
	private bool m_bIgnoreToggles;

	// Token: 0x04000BDA RID: 3034
	private bool m_bEnterFromPopup;

	// Token: 0x04000BDB RID: 3035
	private bool m_bDeleteMode;

	// Token: 0x04000BDC RID: 3036
	private UIP_GameSlot.ClickEventType m_lastGameSlotClickEvent;

	// Token: 0x04000BDD RID: 3037
	private UIP_GameSlot m_lastGameSlotSelected;

	// Token: 0x04000BDE RID: 3038
	private uint m_lastGameSlotSelectedID;
}
