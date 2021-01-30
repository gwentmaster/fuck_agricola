using System;
using AsmodeeNet.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200011E RID: 286
public class UI_OfflineCreateGame : MonoBehaviour
{
	// Token: 0x06000AC3 RID: 2755 RVA: 0x00048804 File Offset: 0x00046A04
	private void Awake()
	{
		if (!this.m_bInitialized)
		{
			this.m_bInitialized = true;
			this.m_playerSlots[0].dragSource.m_DragStart += this.StartDragPlayer1;
			this.m_playerSlots[1].dragSource.m_DragStart += this.StartDragPlayer2;
			this.m_playerSlots[2].dragSource.m_DragStart += this.StartDragPlayer3;
			this.m_playerSlots[3].dragSource.m_DragStart += this.StartDragPlayer4;
			this.m_playerSlots[4].dragSource.m_DragStart += this.StartDragPlayer5;
		}
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x000488D0 File Offset: 0x00046AD0
	public void OnEnterMenu()
	{
		if (this.m_bIgnoreNextEnter)
		{
			this.m_bIgnoreNextEnter = false;
			return;
		}
		this.m_draggingIndex = -1;
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.DisableToggleSoundEffects();
		}
		this.m_bIgnoreToggles = true;
		this.RetreiveSettings();
		this.m_bIgnoreToggles = false;
		this.OnGameTypeChanged();
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.EnableToggleSoundEffects();
		}
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00048950 File Offset: 0x00046B50
	private string GetAIName(int slotIndex, int difficulty)
	{
		string input_text = "${" + this.AINameKeys[difficulty, slotIndex] + "}";
		string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
		if (!(text != string.Empty))
		{
			return this.AINameKeys[difficulty, slotIndex];
		}
		return text;
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x000489A4 File Offset: 0x00046BA4
	public void HandleBackFromAvatarSelect(ProfileManager.OfflineProfileEntry profile, bool bConfirm)
	{
		if (bConfirm)
		{
			int factionIndex = (int)profile.factionIndex;
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar1", (int)profile.gameAvatar1);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar2", (int)profile.gameAvatar2);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar3", (int)profile.gameAvatar3);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar4", (int)profile.gameAvatar4);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar5", (int)profile.gameAvatar5);
			PlayerPrefs.SetString("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Name", profile.name);
			this.m_playerSlots[factionIndex].playerType = PlayerPrefs.GetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Type", 0);
			this.m_playerSlots[factionIndex].profile.name = PlayerPrefs.GetString("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Name", "Player " + (factionIndex + 1).ToString());
			this.m_playerSlots[factionIndex].profile.gameAvatar1 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar1", 1);
			this.m_playerSlots[factionIndex].profile.gameAvatar2 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar2", 2);
			this.m_playerSlots[factionIndex].profile.gameAvatar3 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar3", 3);
			this.m_playerSlots[factionIndex].profile.gameAvatar4 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar4", 4);
			this.m_playerSlots[factionIndex].profile.gameAvatar5 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_Avatar5", 5);
			this.m_playerSlots[factionIndex].bIsLocalPlayer = (PlayerPrefs.GetInt("OfflineCreateGame_Slot" + factionIndex.ToString() + "_IsLocal", (factionIndex == 0) ? 1 : 0) == 1);
			this.OnGameTypeChanged();
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00048C38 File Offset: 0x00046E38
	public void HandleClickOnSlotHuman(int slotIndex)
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_playerSlots[slotIndex].bIsLocalPlayer)
		{
			this.UpdatePlayerStates();
			return;
		}
		if (this.m_playerSlots[slotIndex].playerType > 0)
		{
			this.m_playerSlots[slotIndex].playerType = 0;
			this.m_playerSlots[slotIndex].name.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Player" + (slotIndex + 1).ToString() + "}");
		}
		else if (this.CountActiveSlots() > 2)
		{
			this.m_playerSlots[slotIndex].playerType = 4;
		}
		this.OnGameTypeChanged();
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x00048CEC File Offset: 0x00046EEC
	public void HandleClickOnSlotAI(int slotIndex)
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_playerSlots[slotIndex].bIsLocalPlayer)
		{
			return;
		}
		if (this.m_playerSlots[slotIndex].playerType == 0 || this.m_playerSlots[slotIndex].playerType == 4)
		{
			this.m_playerSlots[slotIndex].playerType = 1;
		}
		else if (this.CountActiveSlots() > 2)
		{
			this.m_playerSlots[slotIndex].playerType = 4;
		}
		this.OnGameTypeChanged();
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x00048D74 File Offset: 0x00046F74
	public void HandleClickOnSlotName(int slotIndex)
	{
		if (this.m_playerSlots[slotIndex].playerType == 0)
		{
			this.m_playerSlots[slotIndex].profile.factionIndex = (byte)slotIndex;
			GameObject scene = ScreenManager.instance.GetScene(this.m_AvatarSelectionScreenName);
			if (scene != null)
			{
				scene.GetComponent<UI_AvatarSelect>().SetProfile(this.m_playerSlots[slotIndex].profile, new UI_AvatarSelect.AvatarCallback(this.HandleBackFromAvatarSelect), true, false, false);
				ScreenManager.instance.PushScene(this.m_AvatarSelectionScreenName);
				return;
			}
		}
		else
		{
			if (this.m_playerSlots[slotIndex].playerType >= 1 && this.m_playerSlots[slotIndex].playerType <= 3)
			{
				this.HandleClickOnSlotAIDifficulty(slotIndex);
				return;
			}
			this.HandleClickOnSlotHuman(slotIndex);
		}
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00048E3C File Offset: 0x0004703C
	private int CountActiveSlots()
	{
		int num = 0;
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			if (this.m_playerSlots[i].playerType != 4)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00048E78 File Offset: 0x00047078
	private void HandleClickOnSlotAIDifficulty(int slotIndex)
	{
		if (this.m_playerSlots[slotIndex].playerType == 3)
		{
			this.m_playerSlots[slotIndex].playerType = 1;
		}
		else
		{
			UI_OfflineCreateGame.PlayerSlot[] playerSlots = this.m_playerSlots;
			playerSlots[slotIndex].playerType = playerSlots[slotIndex].playerType + 1;
		}
		this.OnGameTypeChanged();
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00048EC9 File Offset: 0x000470C9
	public void OnExitMenu(bool bUnderPopup)
	{
		if (this.m_bIgnoreNextEnter)
		{
			return;
		}
		this.StoreSettings();
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00048EDA File Offset: 0x000470DA
	public void StartDragPlayer1(UI_DragSource e, PointerEventData a)
	{
		this.StartDrag(e, 0);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00048EE4 File Offset: 0x000470E4
	public void StartDragPlayer2(UI_DragSource e, PointerEventData a)
	{
		this.StartDrag(e, 1);
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00048EEE File Offset: 0x000470EE
	public void StartDragPlayer3(UI_DragSource e, PointerEventData a)
	{
		this.StartDrag(e, 2);
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00048EF8 File Offset: 0x000470F8
	public void StartDragPlayer4(UI_DragSource e, PointerEventData a)
	{
		this.StartDrag(e, 3);
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00048F02 File Offset: 0x00047102
	public void StartDragPlayer5(UI_DragSource e, PointerEventData a)
	{
		this.StartDrag(e, 4);
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00048F0C File Offset: 0x0004710C
	public void StartDragPlayer6(UI_DragSource e, PointerEventData a)
	{
		this.StartDrag(e, 5);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00048F18 File Offset: 0x00047118
	private void StartDrag(UI_DragSource dragSource, int playerIndex)
	{
		if (this.m_playerSlots[playerIndex].playerType == 4)
		{
			dragSource.OnEndDrag(null);
		}
		GameObject draggingObject = dragSource.GetDraggingObject();
		if (draggingObject != null)
		{
			int avatarIndex = 0;
			if (this.m_playerSlots[playerIndex].playerType == 0)
			{
				avatarIndex = (int)this.m_playerSlots[playerIndex].profile.gameAvatar1 + 10 * playerIndex;
			}
			else if (this.m_playerSlots[playerIndex].playerType == 1)
			{
				avatarIndex = 62;
			}
			else if (this.m_playerSlots[playerIndex].playerType == 2)
			{
				avatarIndex = 61;
			}
			else if (this.m_playerSlots[playerIndex].playerType == 3)
			{
				avatarIndex = 63;
			}
			draggingObject.GetComponent<UIP_CreateGameDragToken>().Setup(avatarIndex, (uint)playerIndex);
		}
		this.m_draggingIndex = playerIndex;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00048FE4 File Offset: 0x000471E4
	public void HandleDrop(int dropIndex)
	{
		if (this.m_draggingIndex != -1 && this.m_draggingIndex != dropIndex)
		{
			int playerType = this.m_playerSlots[this.m_draggingIndex].playerType;
			this.m_playerSlots[this.m_draggingIndex].playerType = this.m_playerSlots[dropIndex].playerType;
			this.m_playerSlots[dropIndex].playerType = playerType;
			bool bIsLocalPlayer = this.m_playerSlots[this.m_draggingIndex].bIsLocalPlayer;
			this.m_playerSlots[this.m_draggingIndex].bIsLocalPlayer = this.m_playerSlots[dropIndex].bIsLocalPlayer;
			this.m_playerSlots[dropIndex].bIsLocalPlayer = bIsLocalPlayer;
			ProfileManager.OfflineProfileEntry profile = this.m_playerSlots[this.m_draggingIndex].profile;
			this.m_playerSlots[this.m_draggingIndex].profile = this.m_playerSlots[dropIndex].profile;
			this.m_playerSlots[dropIndex].profile = profile;
		}
		this.m_draggingIndex = -1;
		this.UpdatePlayerStates();
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00049108 File Offset: 0x00047308
	public void OnGameTypeChanged()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_familyGameToggle.isOn)
		{
			this.m_bIgnoreToggles = true;
			this.m_setToggles[0].isOn = true;
			this.m_familyGameToggle.isOn = false;
			this.m_bIgnoreToggles = false;
		}
		uint setFlags = this.m_setFlags;
		this.m_setFlags = 0U;
		int num = 12;
		for (int i = 0; i < this.m_setToggles.Length; i++)
		{
			this.m_setToggles[i].interactable = true;
			if (this.m_setToggles[i].interactable && this.m_setToggles[i].isOn)
			{
				this.m_setFlags |= 1U << i + num;
			}
		}
		if (this.m_setFlags == 0U)
		{
			this.m_bIgnoreToggles = true;
			for (int j = 0; j < this.m_setToggles.Length; j++)
			{
				if ((setFlags & 1U << j + num) != 0U)
				{
					this.m_setToggles[j].isOn = true;
				}
			}
			this.m_setFlags = setFlags;
			this.m_bIgnoreToggles = false;
		}
		if (this.m_standardGameToggles[0].isOn)
		{
			this.m_gameType = 1U;
		}
		else if (this.m_standardGameToggles[1].isOn)
		{
			this.m_gameType = 3U;
		}
		else if (this.m_standardGameToggles[2].isOn)
		{
			this.m_gameType = 2U;
		}
		this.UpdatePlayerStates();
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00049250 File Offset: 0x00047450
	public void UpdatePlayerStates()
	{
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			if (!(this.m_playerSlots[i].root != null) || this.m_playerSlots[i].root.activeSelf)
			{
				this.m_bIgnoreToggles = true;
				this.m_playerSlots[i].humanToggle.isOn = (this.m_playerSlots[i].playerType == 0);
				this.m_playerSlots[i].aiToggle.isOn = (this.m_playerSlots[i].playerType >= 1 && this.m_playerSlots[i].playerType <= 3);
				this.m_bIgnoreToggles = false;
				this.m_playerSlots[i].humanRoot.SetActive(this.m_playerSlots[i].playerType == 0);
				this.m_playerSlots[i].humanAvatar1.SetAvatar((int)this.m_playerSlots[i].profile.gameAvatar1 + 10 * i, true);
				this.m_playerSlots[i].humanAvatar2.SetAvatar((int)this.m_playerSlots[i].profile.gameAvatar2 + 10 * i, true);
				this.m_playerSlots[i].aiRoot.SetActive(this.m_playerSlots[i].playerType >= 1 && this.m_playerSlots[i].playerType <= 3);
				this.m_playerSlots[i].openSlotObject.SetActive(this.m_playerSlots[i].playerType == 4);
				if (this.m_playerSlots[i].playerType == 0)
				{
					this.m_playerSlots[i].name.text = this.m_playerSlots[i].profile.name;
				}
				else if (this.m_playerSlots[i].playerType == 1)
				{
					this.m_playerSlots[i].aiDifficulty.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_AIEasyName}");
					this.m_playerSlots[i].aiAvatar.SetAvatar(62, true);
					this.m_playerSlots[i].name.text = this.GetAIName(i, this.m_playerSlots[i].playerType - 1);
				}
				else if (this.m_playerSlots[i].playerType == 2)
				{
					this.m_playerSlots[i].aiDifficulty.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_AIMedName}");
					this.m_playerSlots[i].aiAvatar.SetAvatar(61, true);
					this.m_playerSlots[i].name.text = this.GetAIName(i, this.m_playerSlots[i].playerType - 1);
				}
				else if (this.m_playerSlots[i].playerType == 3)
				{
					this.m_playerSlots[i].aiDifficulty.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_AIHardName}");
					this.m_playerSlots[i].aiAvatar.SetAvatar(63, true);
					this.m_playerSlots[i].name.text = this.GetAIName(i, this.m_playerSlots[i].playerType - 1);
				}
				else
				{
					this.m_playerSlots[i].name.text = string.Empty;
				}
			}
		}
		int num = this.CountActiveSlots();
		if (this.m_createGameButton != null)
		{
			this.m_createGameButton.SetActive(num >= 2);
		}
		if (num < 2)
		{
			int num2 = 2 - num;
			for (int j = 0; j < this.m_playerSlots.Length; j++)
			{
				if (num2 > 0 && this.m_playerSlots[j].playerType == 4)
				{
					this.HandleClickOnSlotHuman(j);
					num2--;
				}
			}
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x00049678 File Offset: 0x00047878
	public void CreateGame()
	{
		int emptySlot = UIC_OfflineGameList.GetEmptySlot();
		if (emptySlot < 0)
		{
			return;
		}
		if (this.CountActiveSlots() < 2)
		{
			if (this.m_createGameButton != null)
			{
				this.m_createGameButton.SetActive(false);
			}
			return;
		}
		GameParameters gameParameters = default(GameParameters);
		gameParameters.gameType = (ushort)this.m_gameType;
		gameParameters.deckFlags = 16384;
		if (this.m_gameType == 0U)
		{
			gameParameters.deckFlags = 0;
		}
		gameParameters.soloGameStartFood = 0;
		gameParameters.soloGameStartOccupations = new ushort[7];
		for (int i = 0; i < 7; i++)
		{
			gameParameters.soloGameStartOccupations[i] = 0;
		}
		gameParameters.soloGameCount = 0;
		if (this.m_gameType == 1U || this.m_gameType == 2U || this.m_gameType == 3U)
		{
			gameParameters.deckFlags = 16384;
		}
		string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameParameters.deckFlags);
		AppPlayerData[] array = new AppPlayerData[this.m_playerSlots.Length];
		int num = 0;
		int num2 = 0;
		bool flag = false;
		string text = string.Empty;
		bool flag2 = false;
		for (int j = 0; j < this.m_playerSlots.Length; j++)
		{
			if (this.m_playerSlots[j].playerType != 4 && !(this.m_playerSlots[j].root == null) && this.m_playerSlots[j].root.activeInHierarchy)
			{
				array[num].name = this.m_playerSlots[j].name.text;
				if (array[num].name == string.Empty)
				{
					array[num].name = "Player " + (1 + j).ToString();
				}
				array[num].id = 100 + num;
				array[num].userRating = 0;
				array[num].playerParameters = default(PlayerParameters);
				array[num].playerParameters.avatarColorIndex = (byte)j;
				if (this.m_playerSlots[j].playerType == 0)
				{
					array[num].userAvatar = (ushort)this.m_playerSlots[j].humanAvatar1.GetIndex();
					if (flag)
					{
						array[num].playerType = 1;
					}
					else
					{
						array[num].playerType = 0;
						flag = true;
					}
					array[num].playerParameters.avatar1 = (byte)((int)this.m_playerSlots[j].profile.gameAvatar1 + 10 * j);
					array[num].playerParameters.avatar2 = (byte)((int)this.m_playerSlots[j].profile.gameAvatar2 + 10 * j);
					array[num].playerParameters.avatar3 = (byte)((int)this.m_playerSlots[j].profile.gameAvatar3 + 10 * j);
					array[num].playerParameters.avatar4 = (byte)((int)this.m_playerSlots[j].profile.gameAvatar4 + 10 * j);
					array[num].playerParameters.avatar5 = (byte)((int)this.m_playerSlots[j].profile.gameAvatar5 + 10 * j);
					num2++;
					array[num].aiDifficultyLevel = 0;
				}
				else
				{
					array[num].playerParameters.avatar1 = (byte)(1 + 10 * j);
					array[num].playerParameters.avatar2 = (byte)(2 + 10 * j);
					array[num].playerParameters.avatar3 = (byte)(3 + 10 * j);
					array[num].playerParameters.avatar4 = (byte)(4 + 10 * j);
					array[num].playerParameters.avatar5 = (byte)(5 + 10 * j);
					array[num].userAvatar = (ushort)(1 + 10 * j);
					array[num].playerType = 2;
					if (this.m_playerSlots[j].playerType == 1)
					{
						array[num].aiDifficultyLevel = 0;
						text = text + (flag2 ? "," : "") + "easy";
						flag2 = true;
					}
					else if (this.m_playerSlots[j].playerType == 2)
					{
						array[num].aiDifficultyLevel = 1;
						text = text + (flag2 ? "," : "") + "medium";
						flag2 = true;
					}
					else if (this.m_playerSlots[j].playerType == 3)
					{
						array[num].aiDifficultyLevel = 2;
						text = text + (flag2 ? "," : "") + "hard";
						flag2 = true;
					}
				}
				array[num].networkPlayerState = 0;
				array[num].networkPlayerTimer = 0U;
				num++;
			}
		}
		string text2 = ThirdPartyManager.GenerateOfflineMatchID(emptySlot);
		PlayerPrefs.SetString("OfflineMatchID_" + emptySlot.ToString(), text2);
		AnalyticsEvents.LogMatchStartEvent(text2, string.Empty, (num2 > 1) ? "passandplay" : "solo", string.Empty, activatedDlc, num2, num - num2, null, "create", null, text, false, false, null, null, null, null, null, null, null);
		PlayerPrefs.SetString("ActiveOfflineMatchID", text2);
		System.Random random = new System.Random();
		ScreenManager.s_shortFilename = UIC_OfflineGameList.GetShortFileName(emptySlot);
		ScreenManager.s_fullFilename = UIC_OfflineGameList.GetFullFileName(emptySlot);
		uint randomSeed = (uint)random.Next();
		AgricolaLib.StartGame(ref gameParameters, num, array, randomSeed);
		this.StoreSettings();
		ScreenManager.instance.LoadIntoGameScreen(1);
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00049C7C File Offset: 0x00047E7C
	private void RetreiveSettings()
	{
		int @int = PlayerPrefs.GetInt("OfflineCreateGame_DeckE", 1);
		this.m_setToggles[0].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineCreateGame_DeckI", 0);
		this.m_setToggles[1].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineCreateGame_DeckK", 0);
		this.m_setToggles[2].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineCreateGame_DeckG", 0);
		this.m_setToggles[3].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineCreateGame_FamilyGame", 0);
		this.m_familyGameToggle.isOn = false;
		@int = PlayerPrefs.GetInt("OfflineCreateGame_Random", 1);
		this.m_standardGameToggles[0].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineCreateGame_Discard", 0);
		this.m_standardGameToggles[1].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineCreateGame_Draft", 0);
		this.m_standardGameToggles[2].isOn = (@int == 1);
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			this.m_playerSlots[i].playerType = PlayerPrefs.GetInt("OfflineCreateGame_Slot" + i.ToString() + "_Type", 0);
			this.m_playerSlots[i].profile.gameAvatar1 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar1", 1);
			this.m_playerSlots[i].profile.gameAvatar2 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar2", 2);
			this.m_playerSlots[i].profile.gameAvatar3 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar3", 3);
			this.m_playerSlots[i].profile.gameAvatar4 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar4", 4);
			this.m_playerSlots[i].profile.gameAvatar5 = (byte)PlayerPrefs.GetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar5", 5);
			this.m_playerSlots[i].bIsLocalPlayer = (PlayerPrefs.GetInt("OfflineCreateGame_Slot" + i.ToString() + "_IsLocal", (i == 0) ? 1 : 0) == 1);
		}
		this.m_playerSlots[2].playerType = 4;
		this.m_playerSlots[0].profile.name = PlayerPrefs.GetString("OfflineCreateGame_Slot0_Name", LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Player1}"));
		this.m_playerSlots[1].profile.name = PlayerPrefs.GetString("OfflineCreateGame_Slot1_Name", LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Player2}"));
		this.m_playerSlots[2].profile.name = PlayerPrefs.GetString("OfflineCreateGame_Slot2_Name", LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Player5}"));
		this.m_playerSlots[3].profile.name = PlayerPrefs.GetString("OfflineCreateGame_Slot3_Name", LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Player3}"));
		this.m_playerSlots[4].profile.name = PlayerPrefs.GetString("OfflineCreateGame_Slot4_Name", LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Player4}"));
		this.OnGameTypeChanged();
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00049FF0 File Offset: 0x000481F0
	private void StoreSettings()
	{
		PlayerPrefs.SetInt("OfflineCreateGame_DeckE", this.m_setToggles[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineCreateGame_DeckI", this.m_setToggles[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineCreateGame_DeckK", this.m_setToggles[2].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineCreateGame_DeckG", this.m_setToggles[3].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineCreateGame_FamilyGame", 0);
		PlayerPrefs.SetInt("OfflineCreateGame_Random", this.m_standardGameToggles[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineCreateGame_Discard", this.m_standardGameToggles[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineCreateGame_Draft", this.m_standardGameToggles[2].isOn ? 1 : 0);
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + i.ToString() + "_Type", this.m_playerSlots[i].playerType);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar1", (int)this.m_playerSlots[i].profile.gameAvatar1);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar2", (int)this.m_playerSlots[i].profile.gameAvatar2);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar3", (int)this.m_playerSlots[i].profile.gameAvatar3);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar4", (int)this.m_playerSlots[i].profile.gameAvatar4);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + i.ToString() + "_Avatar5", (int)this.m_playerSlots[i].profile.gameAvatar5);
			PlayerPrefs.SetString("OfflineCreateGame_Slot" + i.ToString() + "_Name", this.m_playerSlots[i].profile.name);
			PlayerPrefs.SetInt("OfflineCreateGame_Slot" + i.ToString() + "_IsLocal", this.m_playerSlots[i].bIsLocalPlayer ? 1 : 0);
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x0004A270 File Offset: 0x00048470
	public UI_OfflineCreateGame()
	{
		string[,] array = new string[3, 5];
		array[0, 0] = "Key_AIEasyRed";
		array[0, 1] = "Key_AIEasyPurple";
		array[0, 2] = "Key_AIEasyGreen";
		array[0, 3] = "Key_AIEasyBlue";
		array[0, 4] = "Key_AIEasyTan";
		array[1, 0] = "Key_AIMedRed";
		array[1, 1] = "Key_AIMedPurple";
		array[1, 2] = "Key_AIMedGreen";
		array[1, 3] = "Key_AIMedBlue";
		array[1, 4] = "Key_AIMedTan";
		array[2, 0] = "Key_AIHardRed";
		array[2, 1] = "Key_AIHardPurple";
		array[2, 2] = "Key_AIHardGreen";
		array[2, 3] = "Key_AIHardBlue";
		array[2, 4] = "Key_AIHardTan";
		this.AINameKeys = array;
		this.m_AvatarSelectionScreenName = "FamilySelection";
		this.m_draggingIndex = -1;
		base..ctor();
	}

	// Token: 0x04000B81 RID: 2945
	private string[,] AINameKeys;

	// Token: 0x04000B82 RID: 2946
	public string m_AvatarSelectionScreenName;

	// Token: 0x04000B83 RID: 2947
	public UI_OfflineCreateGame.PlayerSlot[] m_playerSlots;

	// Token: 0x04000B84 RID: 2948
	public GameObject m_createGameButton;

	// Token: 0x04000B85 RID: 2949
	public Toggle[] m_setToggles;

	// Token: 0x04000B86 RID: 2950
	public Toggle m_familyGameToggle;

	// Token: 0x04000B87 RID: 2951
	public Toggle[] m_standardGameToggles;

	// Token: 0x04000B88 RID: 2952
	private bool m_bIgnoreNextEnter;

	// Token: 0x04000B89 RID: 2953
	private InAppPurchaseWrapper m_InAppPurchase;

	// Token: 0x04000B8A RID: 2954
	private uint m_avatarSlotIndex;

	// Token: 0x04000B8B RID: 2955
	private uint m_setFlags;

	// Token: 0x04000B8C RID: 2956
	private uint m_gameType;

	// Token: 0x04000B8D RID: 2957
	private bool m_bIgnoreToggles;

	// Token: 0x04000B8E RID: 2958
	private bool m_bInitialized;

	// Token: 0x04000B8F RID: 2959
	private int m_draggingIndex;

	// Token: 0x020007F6 RID: 2038
	[Serializable]
	public struct PlayerSlot
	{
		// Token: 0x04002D89 RID: 11657
		public GameObject root;

		// Token: 0x04002D8A RID: 11658
		public GameObject humanRoot;

		// Token: 0x04002D8B RID: 11659
		public Toggle humanToggle;

		// Token: 0x04002D8C RID: 11660
		public Avatar_UI humanAvatar1;

		// Token: 0x04002D8D RID: 11661
		public Avatar_UI humanAvatar2;

		// Token: 0x04002D8E RID: 11662
		public GameObject aiRoot;

		// Token: 0x04002D8F RID: 11663
		public Toggle aiToggle;

		// Token: 0x04002D90 RID: 11664
		public Avatar_UI aiAvatar;

		// Token: 0x04002D91 RID: 11665
		public TextMeshProUGUI aiDifficulty;

		// Token: 0x04002D92 RID: 11666
		public GameObject openSlotObject;

		// Token: 0x04002D93 RID: 11667
		public TextMeshProUGUI name;

		// Token: 0x04002D94 RID: 11668
		public UI_DragSource dragSource;

		// Token: 0x04002D95 RID: 11669
		public UI_DropTarget dropTarget;

		// Token: 0x04002D96 RID: 11670
		[HideInInspector]
		public int playerType;

		// Token: 0x04002D97 RID: 11671
		[HideInInspector]
		public bool bIsLocalPlayer;

		// Token: 0x04002D98 RID: 11672
		[HideInInspector]
		public ProfileManager.OfflineProfileEntry profile;
	}
}
