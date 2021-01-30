using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000123 RID: 291
public class UI_OnlineFindGame : UI_NetworkBase
{
	// Token: 0x06000B05 RID: 2821 RVA: 0x0004BD34 File Offset: 0x00049F34
	public void OnEnterMenu()
	{
		if (this.m_bEnterFromPopup)
		{
			this.m_bEnterFromPopup = false;
			return;
		}
		if (!Network.m_Network.m_bConnectedToServer)
		{
			base.TryNetworkReconnect();
			return;
		}
		this.m_onlineGameList.Initialize(Network.m_Network, new UIC_OnlineGameList.ClickCallback(this.HandleClickOnSlot), this);
		this.m_onlineGameList.SetRefreshCallback(new UIC_OnlineGameList.ListRefreshCallback(this.UpdateToggles));
		this.m_bIgnoreToggles = true;
		this.RetrieveSettings();
		this.m_bIgnoreToggles = false;
		this.EnableFindGameList();
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0004BDB2 File Offset: 0x00049FB2
	public void OnExitMenu(bool bUnderPopup)
	{
		if (bUnderPopup)
		{
			return;
		}
		this.StoreSettings();
		base.StopAllCoroutines();
		this.m_onlineGameList.Destroy();
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0004BDD0 File Offset: 0x00049FD0
	private void RetrieveSettings()
	{
		int @int = PlayerPrefs.GetInt("FindGame_2Players", 1);
		this.m_ToggleFilterPlayers[0].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_3Players", 1);
		this.m_ToggleFilterPlayers[1].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_4Players", 1);
		this.m_ToggleFilterPlayers[2].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_Family", 1);
		this.m_ToggleFilterType[0].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_Draft", 1);
		this.m_ToggleFilterType[1].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_Discard", 1);
		this.m_ToggleFilterType[2].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_Random", 1);
		this.m_ToggleFilterType[3].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_Short", 1);
		this.m_ToggleFilterTimers[0].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_Medium", 1);
		this.m_ToggleFilterTimers[1].isOn = (@int != 0);
		@int = PlayerPrefs.GetInt("FindGame_Long", 1);
		this.m_ToggleFilterTimers[2].isOn = (@int != 0);
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0004BF00 File Offset: 0x0004A100
	private void StoreSettings()
	{
		PlayerPrefs.SetInt("FindGame_2Players", this.m_ToggleFilterPlayers[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_3Players", this.m_ToggleFilterPlayers[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_4Players", this.m_ToggleFilterPlayers[2].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_Family", this.m_ToggleFilterType[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_Draft", this.m_ToggleFilterType[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_Discard", this.m_ToggleFilterType[2].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_Random", this.m_ToggleFilterType[3].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_Short", this.m_ToggleFilterTimers[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_Medium", this.m_ToggleFilterTimers[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("FindGame_Long", this.m_ToggleFilterTimers[2].isOn ? 1 : 0);
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00003022 File Offset: 0x00001222
	public void OnConfirmButtonPressed(bool bConfirm)
	{
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0004C02F File Offset: 0x0004A22F
	public void HandlePlayerToggle(int lastPressedIndex)
	{
		this.CheckToggleList(this.m_ToggleFilterPlayers, lastPressedIndex);
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0004C03E File Offset: 0x0004A23E
	public void HandleTypeToggle(int lastPressedIndex)
	{
		this.CheckToggleList(this.m_ToggleFilterType, lastPressedIndex);
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0004C04D File Offset: 0x0004A24D
	public void HandleTimerToggle(int lastPressedIndex)
	{
		this.CheckToggleList(this.m_ToggleFilterTimers, lastPressedIndex);
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0004C05C File Offset: 0x0004A25C
	private void CheckToggleList(Toggle[] toggleList, int lastPressedIndex)
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < toggleList.Length; i++)
		{
			if (toggleList[i].isOn)
			{
				num++;
			}
		}
		if (num == 0)
		{
			this.m_bIgnoreToggles = true;
			toggleList[lastPressedIndex].isOn = true;
			this.m_bIgnoreToggles = false;
		}
		this.UpdateToggles();
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0004C0B4 File Offset: 0x0004A2B4
	private void UpdateToggles()
	{
		uint gameCount = (uint)this.m_onlineGameList.GetGameCount();
		for (uint num = 0U; num < gameCount; num += 1U)
		{
			UIP_GameSlot gameAtIndex = this.m_onlineGameList.GetGameAtIndex(num);
			ShortSaveStruct gameSlotData = gameAtIndex.GetGameSlotData();
			if (gameAtIndex != null)
			{
				int num2 = gameAtIndex.GetNumPlayers() - 2;
				bool flag = this.m_ToggleFilterPlayers[num2].isOn;
				if (flag)
				{
					int gameType = (int)gameSlotData.gameType;
					if (gameType == 0 && !this.m_ToggleFilterType[0].isOn)
					{
						flag = false;
					}
					else if (gameType == 2 && !this.m_ToggleFilterType[1].isOn)
					{
						flag = false;
					}
					else if (gameType == 3 && !this.m_ToggleFilterType[2].isOn)
					{
						flag = false;
					}
					else if (gameType == 1 && !this.m_ToggleFilterType[3].isOn)
					{
						flag = false;
					}
				}
				if (flag)
				{
					uint player1Timer = gameSlotData.player1Timer;
					if (player1Timer < 86400U && !this.m_ToggleFilterTimers[0].isOn)
					{
						flag = false;
					}
					else if (player1Timer >= 86400U && player1Timer <= 604800U && !this.m_ToggleFilterTimers[1].isOn)
					{
						flag = false;
					}
					else if (player1Timer > 604800U && !this.m_ToggleFilterTimers[2].isOn)
					{
						flag = false;
					}
				}
				gameAtIndex.gameObject.SetActive(flag);
			}
		}
		this.m_onlineGameList.UpdateNoGameDisplay();
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0004C202 File Offset: 0x0004A402
	public void OnRefreshPressed()
	{
		this.EnableFindGameList();
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0004C20A File Offset: 0x0004A40A
	public void IgnoreNetworkEvents()
	{
		if (base.gameObject.activeSelf)
		{
			this.m_bIgnoreNetworkEvents = true;
		}
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x0004C220 File Offset: 0x0004A420
	public void EnableFindGameList()
	{
		if (this.m_onlineGameList != null)
		{
			this.m_onlineGameList.EnableGameList(true, false, true);
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0004C238 File Offset: 0x0004A438
	private void HandleClickOnSlot(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt)
	{
		if (GameObject.FindGameObjectWithTag("IAP Manager") == null)
		{
			return;
		}
		slot.GetGameSlotData();
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
				if (this.m_bkgAnimator != null)
				{
					this.m_bkgAnimator.Play("FindGame_GameDetails");
				}
				this.m_joinGameScreen.SetGameData(slot.GetGameSlotData(), false, true);
				ScreenManager.instance.PushScene("Join/View Game");
				goto IL_2A5;
			}
			AgricolaLib.NetworkLaunchGame(slot.GetNetworkGameID());
			goto IL_2A5;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkAcceptInvite:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			if (this.m_joinGameScreen != null)
			{
				if (this.m_bkgAnimator != null)
				{
					this.m_bkgAnimator.Play("FindGame_GameDetails");
				}
				this.m_joinGameScreen.SetGameData(slot.GetGameSlotData(), true, false);
				ScreenManager.instance.PushScene("Join/View Game");
				goto IL_2A5;
			}
			text = "Do you want to accept the game invite?";
			goto IL_2A5;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkResumeGame:
			AgricolaLib.NetworkResumeGame((int)slot.GetNetworkGameID());
			ScreenManager.instance.LoadIntoGameScreen(slot.GetGameSlotData().roundNumber);
			goto IL_2A5;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_DeleteGamePrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_2A5;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_ForfeitPrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_2A5;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGameLastPlayer:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_ForfeitAllOpp";
			slot.TurnOnDeleteMode(true);
			goto IL_2A5;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkWithdrawFromGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_WithdrawPrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_2A5;
		case UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteMatchmakingGame:
			if (this.m_selectSlotAudio != null)
			{
				this.m_selectSlotAudio.Play();
			}
			text = "Key_DeleteRequestPrompt";
			slot.TurnOnDeleteMode(true);
			goto IL_2A5;
		}
		if (this.m_selectSlotAudio != null)
		{
			this.m_selectSlotAudio.Play();
		}
		if (this.m_joinGameScreen != null)
		{
			if (this.m_bkgAnimator != null)
			{
				this.m_bkgAnimator.Play("FindGame_GameDetails");
			}
			this.m_joinGameScreen.SetGameData(slot.GetGameSlotData(), false, false);
			ScreenManager.instance.PushScene("Join/View Game");
		}
		IL_2A5:
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

	// Token: 0x06000B15 RID: 2837 RVA: 0x0004C557 File Offset: 0x0004A757
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
	}

	// Token: 0x04000BBF RID: 3007
	public UIC_OnlineGameList m_onlineGameList;

	// Token: 0x04000BC0 RID: 3008
	public Toggle[] m_ToggleFilterPlayers;

	// Token: 0x04000BC1 RID: 3009
	public Toggle[] m_ToggleFilterType;

	// Token: 0x04000BC2 RID: 3010
	public Toggle[] m_ToggleFilterTimers;

	// Token: 0x04000BC3 RID: 3011
	public AudioSource m_selectSlotAudio;

	// Token: 0x04000BC4 RID: 3012
	public AudioSource m_toggleAudio;

	// Token: 0x04000BC5 RID: 3013
	public UI_OnlineJoinViewGame m_joinGameScreen;

	// Token: 0x04000BC6 RID: 3014
	public Animator m_bkgAnimator;

	// Token: 0x04000BC7 RID: 3015
	private bool m_bIgnoreToggles;

	// Token: 0x04000BC8 RID: 3016
	private bool m_bEnterFromPopup;

	// Token: 0x04000BC9 RID: 3017
	private UIP_GameSlot.ClickEventType m_lastGameSlotClickEvent;

	// Token: 0x04000BCA RID: 3018
	private UIP_GameSlot m_lastGameSlotSelected;

	// Token: 0x04000BCB RID: 3019
	private uint m_playerFilter = 2U;

	// Token: 0x04000BCC RID: 3020
	private uint m_timerIndex;

	// Token: 0x020007FB RID: 2043
	[Serializable]
	public struct TimerValue
	{
		// Token: 0x04002DAE RID: 11694
		public uint minTimeInSeconds;

		// Token: 0x04002DAF RID: 11695
		public uint maxTimeInSeconds;

		// Token: 0x04002DB0 RID: 11696
		public string text;
	}
}
