using System;
using AsmodeeNet.Analytics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011F RID: 287
public class UI_OfflineGameList : MonoBehaviour
{
	// Token: 0x06000ADB RID: 2779 RVA: 0x0004A368 File Offset: 0x00048568
	public void OnEnterMenu()
	{
		if (this.m_Toggle_DeleteMode != null)
		{
			this.m_Toggle_DeleteMode.isOn = false;
		}
		this.m_bDeleteMode = false;
		this.m_gameList.Initialize("", new UIC_OfflineGameList.ClickCallback(this.HandleClickOnSlot), this);
		if (this.m_deleteDisabled != null)
		{
			this.m_deleteDisabled.SetActive(this.m_gameList.GetNumGames() == 0);
		}
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0004A3DA File Offset: 0x000485DA
	public void OnExitMenu(bool bUnderPopup)
	{
		this.m_gameList.Destroy();
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0004A3E8 File Offset: 0x000485E8
	private void HandleClickOnSlot(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt)
	{
		int slotIndex = slot.GetSlotIndex();
		if (evt == UIP_GameSlot.ClickEventType.Evt_Click_LoadOfflineGame)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("IAP Manager");
			if (gameObject == null)
			{
				return;
			}
			if (gameObject.GetComponent<InAppPurchaseWrapper>() == null)
			{
				return;
			}
			ShortSaveStruct gameSlotData = slot.GetGameSlotData();
			string @string = PlayerPrefs.GetString("OfflineMatchID_" + slotIndex.ToString(), "");
			int num = 0;
			int playerCountAi = 0;
			string difficulty = ThirdPartyManager.GenerateAIDifficultyString(gameSlotData, out num, out playerCountAi);
			string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameSlotData.deckFlags);
			AnalyticsEvents.LogMatchStartEvent(@string, string.Empty, (num > 1) ? "passandplay" : "solo", string.Empty, activatedDlc, num, playerCountAi, null, "resume", null, difficulty, false, false, null, null, null, null, null, null, null);
			PlayerPrefs.SetString("ActiveOfflineMatchID", @string);
			this.m_gameList.LoadGame(slotIndex);
			ScreenManager.s_shortFilename = slot.GetShortSavePath();
			ScreenManager.s_fullFilename = slot.GetFullSavePath();
			ScreenManager.instance.LoadIntoGameScreen(1);
		}
		else if (evt == UIP_GameSlot.ClickEventType.Evt_Click_DeleteOfflineGame)
		{
			if (this.m_DeleteGame != null)
			{
				this.m_DeleteGame.Play();
			}
			if (this.m_confirmPopup != null)
			{
				this.m_confirmSlot = slotIndex;
				this.m_confirmPopup.SetActive(true);
			}
			else
			{
				ShortSaveStruct gameSlotData2 = slot.GetGameSlotData();
				string string2 = PlayerPrefs.GetString("OfflineMatchID_" + slotIndex.ToString(), "");
				int num2 = 0;
				int playerCountAi2 = 0;
				string difficulty2 = ThirdPartyManager.GenerateAIDifficultyString(gameSlotData2, out num2, out playerCountAi2);
				string[] activatedDlc2 = ThirdPartyManager.GenerateDlcString((uint)gameSlotData2.deckFlags);
				AnalyticsEvents.LogMatchStartEvent(string2, string.Empty, (num2 > 1) ? "passandplay" : "solo", string.Empty, activatedDlc2, num2, playerCountAi2, null, "resume", null, difficulty2, false, false, null, null, null, null, null, null, null);
				AnalyticsEvents.LogMatchStopEvent(num2, playerCountAi2, "resign", new MATCH_STOP.player_result?(MATCH_STOP.player_result.defeat), null, null);
				this.m_gameList.DeleteGame(slotIndex);
				if (this.m_deleteDisabled != null)
				{
					this.m_deleteDisabled.SetActive(this.m_gameList.GetNumGames() == 0);
				}
			}
		}
		this.m_lastGameSlotSelected = slot;
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0004A684 File Offset: 0x00048884
	public void OnConfirmPopupPressed(bool bConfirm)
	{
		if (bConfirm)
		{
			ShortSaveStruct gameSlotData = this.m_gameList.GetGameFromIndex(this.m_confirmSlot).GetGameSlotData();
			string @string = PlayerPrefs.GetString("OfflineMatchID_" + this.m_confirmSlot.ToString(), "");
			int num = 0;
			int playerCountAi = 0;
			string difficulty = ThirdPartyManager.GenerateAIDifficultyString(gameSlotData, out num, out playerCountAi);
			string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameSlotData.deckFlags);
			AnalyticsEvents.LogMatchStartEvent(@string, string.Empty, (num > 1) ? "passandplay" : "solo", string.Empty, activatedDlc, num, playerCountAi, null, "resume", null, difficulty, false, false, null, null, null, null, null, null, null);
			AnalyticsEvents.LogMatchStopEvent(num, playerCountAi, "resign", new MATCH_STOP.player_result?(MATCH_STOP.player_result.defeat), null, null);
			this.m_gameList.DeleteGame(this.m_confirmSlot);
			if (this.m_deleteDisabled != null)
			{
				this.m_deleteDisabled.SetActive(this.m_gameList.GetNumGames() == 0);
			}
			int @int = PlayerPrefs.GetInt("OfflineStats_Forfeits", 0);
			PlayerPrefs.SetInt("OfflineStats_Forfeits", @int + 1);
		}
		this.m_confirmPopup.SetActive(false);
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0004A7E4 File Offset: 0x000489E4
	public void ToggleDeleteMode()
	{
		if (this.m_Toggle_DeleteMode != null)
		{
			this.m_bDeleteMode = this.m_Toggle_DeleteMode.isOn;
		}
		else
		{
			this.m_bDeleteMode = !this.m_bDeleteMode;
		}
		this.m_gameList.SetAllDeleteMode(this.m_bDeleteMode);
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0004A834 File Offset: 0x00048A34
	public static uint CreateSelectionNameHash(string text)
	{
		uint num = 5381U;
		int i = 0;
		while (i < text.Length)
		{
			uint num2 = (uint)text[i++];
			num = (num << 5) + num + num2;
		}
		return num;
	}

	// Token: 0x04000B90 RID: 2960
	public UIC_OfflineGameList m_gameList;

	// Token: 0x04000B91 RID: 2961
	public AudioSource m_startGame;

	// Token: 0x04000B92 RID: 2962
	public AudioSource m_DeleteGame;

	// Token: 0x04000B93 RID: 2963
	public GameObject m_confirmPopup;

	// Token: 0x04000B94 RID: 2964
	public GameObject m_deleteDisabled;

	// Token: 0x04000B95 RID: 2965
	public Toggle m_Toggle_DeleteMode;

	// Token: 0x04000B96 RID: 2966
	private int m_confirmSlot;

	// Token: 0x04000B97 RID: 2967
	private bool m_bDeleteMode;

	// Token: 0x04000B98 RID: 2968
	private UIP_GameSlot m_lastGameSlotSelected;
}
