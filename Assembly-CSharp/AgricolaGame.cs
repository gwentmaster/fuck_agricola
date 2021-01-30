using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using AsmodeeNet.Analytics;
using GameData;
using GameEvent;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000041 RID: 65
public class AgricolaGame : MonoBehaviour
{
	// Token: 0x0600035A RID: 858 RVA: 0x00015DD4 File Offset: 0x00013FD4
	public Tutorial GetTutorial()
	{
		return this.m_Tutorial;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00015DDC File Offset: 0x00013FDC
	public DateTime GetTutorialStepStartTime()
	{
		return this.m_TutorialStepTimeStart;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00015DE4 File Offset: 0x00013FE4
	public AnimationManager GetAnimationManager()
	{
		return this.m_AnimationManager;
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00015DEC File Offset: 0x00013FEC
	public DragManager GetDragManager()
	{
		return this.m_DragManager;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00015DF4 File Offset: 0x00013FF4
	public AgricolaCardManager GetCardManager()
	{
		return this.m_CardManager;
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00015DFC File Offset: 0x00013FFC
	public AgricolaCardInPlayManager GetCardInPlayManager()
	{
		return this.m_CardInPlayManager;
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00015E04 File Offset: 0x00014004
	public AgricolaMagnifyManager GetMagnifyManager()
	{
		return this.m_MagnifyManager;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00015E0C File Offset: 0x0001400C
	public AgricolaWorkerManager GetWorkerManager()
	{
		return this.m_WorkerManager;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00015E14 File Offset: 0x00014014
	public PopupManager GetPopupManager()
	{
		return this.m_PopupManager;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00015E1C File Offset: 0x0001401C
	public AgricolaAnimationLocator GetLocatorResolvePosition()
	{
		return this.m_LocatorResolvePosition;
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00015E24 File Offset: 0x00014024
	public ExcessAnimalTray GetExcessAnimalTray()
	{
		return this.m_LowerHudExcessAnimalDisplay;
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00015E2C File Offset: 0x0001402C
	public AgricolaFarm GetFarm()
	{
		return this.m_AgricolaFarm;
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00015E34 File Offset: 0x00014034
	public PlayerDisplay_UpperHud GetUpperHud()
	{
		return this.m_UpperHudTokens;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00015E3C File Offset: 0x0001403C
	public AgricolaAudioHandlerIngame GetAudioHandler()
	{
		return this.m_AudioHandler;
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00015E44 File Offset: 0x00014044
	public int GetLocalPlayerIndex()
	{
		return this.m_LocalPlayerIndex;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00015E4C File Offset: 0x0001404C
	public int GetLocalPlayerInstanceID()
	{
		return this.m_LocalPlayerInstanceID;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00015E54 File Offset: 0x00014054
	public int GetLocalPlayerColorIndex()
	{
		return this.m_LocalPlayerFactionIndex;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00015E5C File Offset: 0x0001405C
	public bool GetIsPaused()
	{
		return this.m_bPaused;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00015E64 File Offset: 0x00014064
	public static ShortSaveStruct GetLastSavedState()
	{
		return AgricolaGame.m_lastSavedState;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00015E6B File Offset: 0x0001406B
	public global::PlayerData GetPlayerInterfaceByLocalPlayerOrder(int order_index)
	{
		if (order_index == 0)
		{
			return this.m_LocalPlayerData;
		}
		return null;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00015E78 File Offset: 0x00014078
	public global::PlayerData GetPlayerInterfaceByInstanceID(int playerInstanceID)
	{
		if (this.m_LocalPlayerData != null && this.m_LocalPlayerData.GetPlayerInstanceID() == playerInstanceID)
		{
			return this.m_LocalPlayerData;
		}
		return null;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00015E9E File Offset: 0x0001409E
	public bool GetIsEndGamePopupActive()
	{
		return this.m_Popup_EndGame != null && this.m_Popup_EndGame.isActiveAndEnabled;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00015EBC File Offset: 0x000140BC
	[MonoPInvokeCallback(typeof(AgricolaLib.SaveWorldDataDelegate))]
	public static void OnSaveData(IntPtr pSaveData, int size, IntPtr pShortSaveStruct)
	{
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		if (ScreenManager.s_shortFilename == string.Empty || ScreenManager.s_fullFilename == string.Empty)
		{
			return;
		}
		byte[] array = new byte[size];
		Marshal.Copy(pSaveData, array, 0, size);
		ShortSaveStruct shortSaveStruct = (ShortSaveStruct)Marshal.PtrToStructure(pShortSaveStruct, typeof(ShortSaveStruct));
		ShortSaveStruct shortSaveStruct2 = default(ShortSaveStruct);
		shortSaveStruct2.playdekHeader = "PLAYDEK";
		shortSaveStruct2.saveFileVersionNumber = 1U;
		shortSaveStruct2.packedPlayerCount = shortSaveStruct.packedPlayerCount;
		shortSaveStruct2.player1Name = shortSaveStruct.player1Name;
		shortSaveStruct2.player2Name = shortSaveStruct.player2Name;
		shortSaveStruct2.player3Name = shortSaveStruct.player3Name;
		shortSaveStruct2.player4Name = shortSaveStruct.player4Name;
		shortSaveStruct2.player5Name = shortSaveStruct.player5Name;
		shortSaveStruct2.player6Name = shortSaveStruct.player6Name;
		shortSaveStruct2.player1Faction = shortSaveStruct.player1Faction;
		shortSaveStruct2.player2Faction = shortSaveStruct.player2Faction;
		shortSaveStruct2.player3Faction = shortSaveStruct.player3Faction;
		shortSaveStruct2.player4Faction = shortSaveStruct.player4Faction;
		shortSaveStruct2.player5Faction = shortSaveStruct.player5Faction;
		shortSaveStruct2.player6Faction = shortSaveStruct.player6Faction;
		shortSaveStruct2.player1State = shortSaveStruct.player1State;
		shortSaveStruct2.player2State = shortSaveStruct.player2State;
		shortSaveStruct2.player3State = shortSaveStruct.player3State;
		shortSaveStruct2.player4State = shortSaveStruct.player4State;
		shortSaveStruct2.player5State = shortSaveStruct.player5State;
		shortSaveStruct2.player6State = shortSaveStruct.player6State;
		shortSaveStruct2.gameID = shortSaveStruct.gameID;
		shortSaveStruct2.gameType = shortSaveStruct.gameType;
		shortSaveStruct2.roundNumber = shortSaveStruct.roundNumber;
		shortSaveStruct2.deckFlags = shortSaveStruct.deckFlags;
		shortSaveStruct2.rematchGameID = shortSaveStruct.rematchGameID;
		shortSaveStruct2.soloGameCurrentScore = shortSaveStruct.soloGameCurrentScore;
		shortSaveStruct2.soloGameStartFood = shortSaveStruct.soloGameStartFood;
		shortSaveStruct2.soloGameCount = shortSaveStruct.soloGameCount;
		shortSaveStruct2.soloGameRandomSeed = shortSaveStruct.soloGameRandomSeed;
		shortSaveStruct2.soloGameStartOccupations = new ushort[7];
		for (int i = 0; i < 7; i++)
		{
			shortSaveStruct2.soloGameStartOccupations[i] = shortSaveStruct.soloGameStartOccupations[i];
		}
		shortSaveStruct2.decisionPlayerFlags = shortSaveStruct.decisionPlayerFlags;
		shortSaveStruct2.currentTurnPlayerIndex = shortSaveStruct.currentTurnPlayerIndex;
		shortSaveStruct2.worldDataVersion = shortSaveStruct.worldDataVersion;
		shortSaveStruct2.savedDataSize = size;
		shortSaveStruct2.playdekFooter = "PLAYDEK";
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(ScreenManager.s_shortFilename);
		binaryFormatter.Serialize(fileStream, shortSaveStruct2);
		fileStream.Close();
		File.WriteAllBytes(ScreenManager.s_fullFilename, array);
		AgricolaGame.m_lastSavedState = shortSaveStruct2;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00016146 File Offset: 0x00014346
	private void InitializeAgricolaLib()
	{
		Debug.Log("Initializing AgricolaLib...");
		AgricolaLib.Initialize(Path.Combine(Application.streamingAssetsPath, "Lua"), SystemInfo.processorCount);
		AgricolaLib.SetGameOptionsListener(null);
		AgricolaDebug.Initialize();
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00016178 File Offset: 0x00014378
	private void StartGame()
	{
		GameParameters gameParameters = default(GameParameters);
		gameParameters.gameType = 0;
		gameParameters.deckFlags = 0;
		gameParameters.soloGameStartFood = 0;
		gameParameters.soloGameStartOccupations = new ushort[7];
		for (int i = 0; i < 7; i++)
		{
			gameParameters.soloGameStartOccupations[i] = 0;
		}
		gameParameters.soloGameCount = 0;
		AppPlayerData[] array = new AppPlayerData[2];
		int num = 0;
		int num2 = 1;
		array[num].name = "Player 1";
		array[num].id = 123;
		array[num].userAvatar = 0;
		array[num].userRating = 0;
		array[num].playerType = 0;
		array[num].aiDifficultyLevel = 0;
		array[num].networkPlayerState = 0;
		array[num].networkPlayerTimer = 0U;
		array[num].playerParameters = new PlayerParameters
		{
			avatar1 = 1,
			avatar2 = 1,
			avatar3 = 1,
			avatar4 = 1,
			avatar5 = 1,
			avatarColorIndex = 1
		};
		array[num2].name = "Player 2";
		array[num2].id = 456;
		array[num2].userAvatar = 0;
		array[num2].userRating = 0;
		array[num2].playerType = 2;
		array[num2].aiDifficultyLevel = 1;
		array[num2].networkPlayerState = 0;
		array[num2].networkPlayerTimer = 0U;
		array[num2].playerParameters = new PlayerParameters
		{
			avatar1 = 1,
			avatar2 = 1,
			avatar3 = 1,
			avatar4 = 1,
			avatar5 = 1,
			avatarColorIndex = 1
		};
		System.Random random = new System.Random();
		AgricolaLib.StartGame(ref gameParameters, 2, array, (uint)random.Next());
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00016360 File Offset: 0x00014560
	private void Awake()
	{
		this.m_bInitialized = false;
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.AddOnBeginAnimationCallback(new AnimationManager.AnimationManagerCallback(this.OnBeginAnimationCallback));
			this.m_AnimationManager.AddOnEndAnimationCallback(new AnimationManager.AnimationManagerCallback(this.OnEndAnimationCallback));
		}
		if (this.m_PopupManager != null)
		{
			this.m_PopupManager.AddOnCurrentPopupChangedCallback(new PopupManager.PopupChangedCallback(this.OnCurrentPopupChanged));
		}
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		this.m_cardList = new int[128];
		this.m_hCardInstanceBuffer = GCHandle.Alloc(this.m_cardList, GCHandleType.Pinned);
		this.m_cardBuffer = this.m_hCardInstanceBuffer.AddrOfPinnedObject();
		this.m_GameEventBuffer = new GameEventBuffer();
		this.m_GameEventBuffer.Init();
		this.m_GameEventBuffer.SetUpdateDelegate(new GameEventBuffer.GameEventBufferUpdateDelegate(AgricolaLib.UpdateGame));
		this.m_GameEventBuffer.RegisterEventHandler(1, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventPause));
		this.m_GameEventBuffer.RegisterEventHandler(6, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventUpdatePlayerHand));
		this.m_GameEventBuffer.RegisterEventHandler(5, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventUpdateDisplay));
		this.m_GameEventBuffer.RegisterEventHandler(28, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventGainedAnimals));
		this.m_GameEventBuffer.RegisterEventHandler(27, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventMarkFarmTileDirty));
		this.m_GameEventBuffer.RegisterEventHandler(26, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventForceOptionPopupRestriction));
		this.m_GameEventBuffer.RegisterEventHandler(19, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventTutorialAISelectedOption));
		this.m_GameEventBuffer.RegisterEventHandler(29, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventMajorImprovementOwnerChange));
		this.m_GameEventBuffer.RegisterEventHandler(51, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventTookBeggingCards));
		this.m_GameEventBuffer.RegisterEventHandler(47, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventCommitPlayerDecision));
		this.m_GameEventBuffer.RegisterEventHandler(46, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventLoadProgress));
		this.m_GameEventBuffer.RegisterEventHandler(48, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventAchievementData));
		this.m_GameEventBuffer.RegisterEventHandler(31, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventGameOver));
		this.m_GameEventBuffer.RegisterEventHandler(23, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventRoundAnnounce));
		this.m_GameEventBuffer.RegisterEventHandler(24, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventHarvestPhase));
		this.m_GameEventBuffer.RegisterEventHandler(25, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventDraftMode));
		this.m_GameEventBuffer.RegisterEventHandler(65, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventOutputMessage));
		if (this.m_DragManager != null)
		{
			this.m_DragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.BeginDragCallback));
			this.m_DragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.EndDragCallback));
		}
		if (this.m_AudioHandler != null)
		{
			this.m_AudioHandler.RegisterEventHandlers(this.m_GameEventBuffer);
			this.m_AudioHandler.RegisterMagnifyManager(this.m_MagnifyManager);
		}
		this.optionPopupRestrictions.bUseRestriction = false;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00016686 File Offset: 0x00014886
	private void OnDestroy()
	{
		this.m_hDataBuffer.Free();
		this.m_hCardInstanceBuffer.Free();
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000166A0 File Offset: 0x000148A0
	private void Start()
	{
		if (this.m_OptionPromptAnimator != null)
		{
			this.m_OptionPromptAnimator.SetBool("isHidden", true);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetDoneButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetEndTurnButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetConfirmButtonVisible(false);
		}
		if (this.m_HudLeft != null)
		{
			this.m_HudLeft.SetUndoButtonVisible(false);
		}
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.RegisterEventHandlers(this.m_GameEventBuffer);
		}
		if (this.m_BuildingManager != null)
		{
			this.m_BuildingManager.RegisterEventHandlers(this.m_GameEventBuffer);
		}
		if (this.m_LocalPlayerWorkerTray != null)
		{
			this.m_LocalPlayerWorkerTray.RegisterEventHandlers(this.m_GameEventBuffer);
			this.m_LocalPlayerWorkerTray.SetupAnimationManager(this.m_AnimationManager);
		}
		if (this.m_CardInPlayManager != null)
		{
			this.m_CardInPlayManager.RegisterEventHandlers(this.m_GameEventBuffer);
		}
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.RegisterEventHandlers(this.m_GameEventBuffer);
		}
		if (this.m_DraftInterface != null)
		{
			this.m_DraftInterface.RegisterEventHandlers(this.m_GameEventBuffer);
		}
		if (this.m_MagnifyManager != null)
		{
			this.m_MagnifyManager.SetEnableNeighborButtons(true);
			this.m_MagnifyManager.SetEnableMagnifyActionButtons(true);
			this.m_MagnifyManager.AddOnMagnifyCallback(new MagnifyManager.MagnifyCallback(this.OnMagnifyCard));
			this.m_MagnifyManager.AddOnUnmagnifyCallback(new MagnifyManager.MagnifyCallback(this.OnUnmagnifyCard));
		}
		this.Initialize();
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0001685C File Offset: 0x00014A5C
	public void Initialize()
	{
		if (AgricolaLib.GetIsOnlineGame())
		{
			this.m_OnlineGameID = AgricolaLib.GetCurrentGameID();
		}
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		GameOptions.Initialize(this.m_AgricolaFarm);
		AgricolaLib.SetSaveDataFunc(new AgricolaLib.SaveWorldDataDelegate(AgricolaGame.OnSaveData));
		this.m_bPaused = false;
		this.SwitchToPlayer(this.m_LocalPlayerIndex, true);
		this.RebuildGame();
		if (this.m_TutorialRoot != null)
		{
			this.m_TutorialRoot.SetActive(false);
		}
		if (AgricolaLib.GetIsTutorialGame())
		{
			this.StartTutorial();
		}
		else
		{
			if (this.m_TutorialRoot != null)
			{
				UnityEngine.Object.Destroy(this.m_TutorialRoot);
				this.m_TutorialRoot = null;
			}
			this.m_TutorialInterfaceCallouts = null;
		}
		if (this.m_Popup_Chat != null)
		{
			if (AgricolaLib.GetIsOnlineGame())
			{
				uint currentGameID = AgricolaLib.GetCurrentGameID();
				this.m_Popup_Chat.GetComponent<Popup_Chat>().UpdateDisplay(currentGameID);
				this.m_Popup_Chat.SetActive(true);
				this.m_Button_Chat.SetActive(true);
			}
			else
			{
				this.m_Popup_Chat.SetActive(false);
				this.m_Button_Chat.SetActive(false);
			}
		}
		this.m_bInitialized = true;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00016978 File Offset: 0x00014B78
	private void StartTutorial()
	{
		if (this.m_MagnifyManager != null)
		{
			this.m_MagnifyManager.SetEnableNeighborButtons(false);
			this.m_MagnifyManager.SetEnableMagnifyActionButtons(false);
		}
		this.m_TutorialElementsActive = new List<GameObject>();
		this.m_Tutorial = new Tutorial();
		this.m_Tutorial.Start();
		this.m_TutorialStepTimeStart = DateTime.Now;
		this.SetupTutorialStep();
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000169E0 File Offset: 0x00014BE0
	private bool PauseSimulationForTutorial()
	{
		if (this.m_Tutorial == null)
		{
			return false;
		}
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if (currentStep != null)
		{
			if (currentStep.m_StepType == TutorialStepType.TextPopup || currentStep.m_StepType == TutorialStepType.TextPopupHelpButton || currentStep.m_StepType == TutorialStepType.TextPopupTownButton)
			{
				return (currentStep.m_StepFlags & 1U) == 0U;
			}
			if (currentStep.m_StepType == TutorialStepType.ContinuePrompt)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00016A3C File Offset: 0x00014C3C
	private void ClearActiveTutorialElements()
	{
		if (this.m_TutorialElementsActive != null)
		{
			foreach (GameObject gameObject in this.m_TutorialElementsActive)
			{
				gameObject.SetActive(false);
			}
			this.m_TutorialElementsActive.Clear();
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00016AA0 File Offset: 0x00014CA0
	private string GetLocalizedStringPCAttempt(string raw_string)
	{
		string input_text = raw_string.Replace("}", "_pc}");
		string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
		if (text == string.Empty)
		{
			text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(raw_string);
		}
		return text;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00016AE4 File Offset: 0x00014CE4
	private void ShowTutorialPanel(TutorialStep current_step, bool bActivateConfirmButton)
	{
		if (this.m_TutorialPanel != null)
		{
			RectTransform component = this.m_TutorialPanel.GetComponent<RectTransform>();
			if (component != null)
			{
				TutorialPanelLayout tutorialPanelLayout = this.m_Tutorial.GetTutorialPanelLayout();
				if (tutorialPanelLayout != null)
				{
					component.anchorMin = tutorialPanelLayout.m_AnchorMin;
					component.anchorMax = tutorialPanelLayout.m_AnchorMax;
					component.anchoredPosition = tutorialPanelLayout.m_AnchoredPosition;
					component.sizeDelta = tutorialPanelLayout.m_SizeDelta;
				}
				else
				{
					component.anchorMin = new Vector2(0.5f, 0.5f);
					component.anchorMax = new Vector2(0.5f, 0.5f);
					component.anchoredPosition = new Vector2(0f, 0f);
					component.sizeDelta = new Vector2(500f, 250f);
				}
			}
			if (this.m_TutorialRoot != null)
			{
				Canvas component2 = this.m_TutorialRoot.GetComponent<Canvas>();
				if (component2 != null)
				{
					if ((current_step.m_StepFlags & 2147483648U) != 0U)
					{
						component2.sortingLayerName = "Log";
					}
					else
					{
						component2.sortingLayerName = "InGameUI";
					}
				}
				this.m_TutorialRoot.SetActive(true);
				this.m_TutorialElementsActive.Add(this.m_TutorialRoot);
			}
			if ((current_step.m_StepFlags & 268435456U) != 0U)
			{
				if (this.m_TutorialPanelBaseRoot != null && this.m_TutorialPanelBaseRoot.activeSelf)
				{
					this.m_TutorialPanelBaseRoot.SetActive(false);
				}
				if (this.m_TutorialPanelSplitRoot != null && !this.m_TutorialPanelSplitRoot.activeSelf)
				{
					this.m_TutorialPanelSplitRoot.SetActive(true);
				}
				if (this.m_TutorialPanelLabelSplitL != null)
				{
					string localizedStringPCAttempt = this.GetLocalizedStringPCAttempt(current_step.m_TutorialText);
					this.m_TutorialPanelLabelSplitL.SetText(localizedStringPCAttempt);
				}
				if (this.m_TutorialPanelLabelSplitR != null)
				{
					string localizedStringPCAttempt2 = this.GetLocalizedStringPCAttempt(current_step.m_SelectionInstanceName);
					this.m_TutorialPanelLabelSplitR.SetText(localizedStringPCAttempt2);
				}
				if (this.m_TutorialPanelPromptSplit != null)
				{
					string localizedStringPCAttempt3 = this.GetLocalizedStringPCAttempt(current_step.m_TutorialPrompt);
					this.m_TutorialPanelPromptSplit.SetText(localizedStringPCAttempt3);
				}
			}
			else
			{
				if (this.m_TutorialPanelBaseRoot != null && !this.m_TutorialPanelBaseRoot.activeSelf)
				{
					this.m_TutorialPanelBaseRoot.SetActive(true);
				}
				if (this.m_TutorialPanelSplitRoot != null && this.m_TutorialPanelSplitRoot.activeSelf)
				{
					this.m_TutorialPanelSplitRoot.SetActive(false);
				}
				if (this.m_TutorialPanelText_Label != null)
				{
					string localizedStringPCAttempt4 = this.GetLocalizedStringPCAttempt(current_step.m_TutorialText);
					this.m_TutorialPanelText_Label.SetText(localizedStringPCAttempt4);
				}
				if (this.m_TutorialPanelPrompt != null)
				{
					string localizedStringPCAttempt5 = this.GetLocalizedStringPCAttempt(current_step.m_TutorialPrompt);
					this.m_TutorialPanelPrompt.SetText(localizedStringPCAttempt5);
				}
			}
		}
		if (this.m_TutorialPanelText_ButtonConfirm != null)
		{
			this.m_TutorialPanelText_ButtonConfirm.SetActive(false);
		}
		if (this.m_TutorialPanelButton != null)
		{
			this.m_TutorialPanelButton.interactable = (bActivateConfirmButton && (current_step.m_StepFlags & 1U) == 0U);
		}
		if (this.m_TutorialPanelContinueButton != null)
		{
			this.m_TutorialPanelContinueButton.SetActive(current_step.m_StepType == TutorialStepType.ContinuePrompt);
		}
		if (this.m_TutorialPanelExitButton != null)
		{
			this.m_TutorialPanelExitButton.SetActive(current_step.m_StepType == TutorialStepType.ContinuePrompt);
		}
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00016E20 File Offset: 0x00015020
	public static void HandleLeaveGameAnalytics(string reason, int winner)
	{
		MATCH_STOP.player_result value = MATCH_STOP.player_result.victory;
		if (winner == 1)
		{
			value = MATCH_STOP.player_result.draw;
		}
		else if (winner == 0)
		{
			value = MATCH_STOP.player_result.defeat;
		}
		int gamePlayerAICount = AgricolaLib.GetGamePlayerAICount();
		int playerCountHuman = AgricolaLib.GetGamePlayerCount() - gamePlayerAICount;
		int currentRound = AgricolaLib.GetCurrentRound();
		gamePlayerAICount = AgricolaLib.GetGamePlayerAICount();
		playerCountHuman = AgricolaLib.GetGamePlayerCount() - gamePlayerAICount;
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		int localPlayerIndex = AgricolaLib.GetLocalPlayerIndex();
		AgricolaLib.GetGamePlayerScoreState(localPlayerIndex, intPtr, 1024);
		GamePlayerScoreState gamePlayerScoreState = (GamePlayerScoreState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerScoreState));
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["player_score_fields"] = gamePlayerScoreState.score[0];
		dictionary["player_score_pastures"] = gamePlayerScoreState.score[1];
		dictionary["player_score_grain"] = gamePlayerScoreState.score[2];
		dictionary["player_score_vegetables"] = gamePlayerScoreState.score[3];
		dictionary["player_score_sheep"] = gamePlayerScoreState.score[4];
		dictionary["player_score_wild_boar"] = gamePlayerScoreState.score[5];
		dictionary["player_score_cattle"] = gamePlayerScoreState.score[6];
		dictionary["player_score_unused_spaces"] = gamePlayerScoreState.score[7];
		dictionary["player_score_fenced_stables"] = gamePlayerScoreState.score[8];
		dictionary["player_score_rooms"] = gamePlayerScoreState.score[9];
		dictionary["player_score_family"] = gamePlayerScoreState.score[10];
		dictionary["player_score_begging_cards"] = gamePlayerScoreState.score[11];
		dictionary["player_score_cards"] = gamePlayerScoreState.score[12];
		dictionary["player_score_bonus_points"] = gamePlayerScoreState.score[13];
		dictionary["player_score_total"] = gamePlayerScoreState.total_points;
		dictionary["player_rank"] = AgricolaLib.GetGameResultsPosition(localPlayerIndex) + 1;
		dictionary["player_play_order"] = component.GetUpperHud().FindPlayerTurnOrderByPlayerIndex(localPlayerIndex);
		gchandle.Free();
		if (winner != -1)
		{
			AnalyticsEvents.LogMatchStopEvent(playerCountHuman, gamePlayerAICount, reason, new MATCH_STOP.player_result?(value), new int?(currentRound), dictionary);
			return;
		}
		AnalyticsEvents.LogMatchStopEvent(playerCountHuman, gamePlayerAICount, reason, null, new int?(currentRound), dictionary);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x000170C0 File Offset: 0x000152C0
	private void ShowTutorialCallouts(TutorialStep current_step)
	{
		if (current_step.m_Callouts == null)
		{
			return;
		}
		foreach (TutorialCallout tutorialCallout in current_step.m_Callouts)
		{
			if (tutorialCallout.m_CalloutType == TutorialCalloutType.VisibleElement)
			{
				if (this.m_TutorialInterfaceCallouts != null && tutorialCallout.m_CalloutIndex >= 0 && tutorialCallout.m_CalloutIndex < this.m_TutorialInterfaceCallouts.Length && this.m_TutorialInterfaceCallouts[tutorialCallout.m_CalloutIndex] != null)
				{
					this.m_TutorialInterfaceCallouts[tutorialCallout.m_CalloutIndex].SetActive(true);
					this.m_TutorialElementsActive.Add(this.m_TutorialInterfaceCallouts[tutorialCallout.m_CalloutIndex]);
				}
			}
			else if (tutorialCallout.m_CalloutType == TutorialCalloutType.MapCalloutPositionAtBuilding)
			{
				if (this.m_TutorialInterfaceCallouts[0] != null && this.m_BuildingManager != null && this.m_TutorialCalloutsRoot != null)
				{
					GameObject defaultBuildingObject = this.m_BuildingManager.GetDefaultBuildingObject(tutorialCallout.m_CalloutIndex);
					if (defaultBuildingObject != null)
					{
						this.m_TutorialInterfaceCallouts[0].transform.SetParent(defaultBuildingObject.transform, true);
						this.m_TutorialInterfaceCallouts[0].transform.localPosition = Vector3.zero;
						this.m_TutorialInterfaceCallouts[0].transform.SetParent(this.m_TutorialCalloutsRoot.transform, true);
						this.m_TutorialInterfaceCallouts[0].transform.SetAsFirstSibling();
					}
				}
			}
			else if (tutorialCallout.m_CalloutType == TutorialCalloutType.MapSwap)
			{
				if (tutorialCallout.m_CalloutIndex == 0)
				{
					if (this.m_AgricolaFarm.gameObject.activeSelf)
					{
						this.SetTown();
					}
				}
				else
				{
					this.SetFarmToPlayer(tutorialCallout.m_CalloutIndex, AgricolaLib.GetPlayerInstanceIDFromIndex(tutorialCallout.m_CalloutIndex));
				}
			}
			else if (tutorialCallout.m_CalloutType == TutorialCalloutType.MapAnimate)
			{
				TransformMap transformMap = this.m_AgricolaFarm.gameObject.activeSelf ? this.m_AgricolaFarmMapController : this.m_AgricolaTownMapController;
				if (transformMap != null)
				{
					transformMap.AutoAnimateToPosition(tutorialCallout.m_MapPosition, tutorialCallout.m_mapAnimTime);
				}
			}
		}
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00003022 File Offset: 0x00001222
	public void ActivateTutorialElementsMagnify()
	{
	}

	// Token: 0x0600037F RID: 895 RVA: 0x000172C0 File Offset: 0x000154C0
	private void OnMagnifyCard(CardObject magnifyCard)
	{
		if (this.m_Tutorial != null && !this.m_Tutorial.IsCompleted())
		{
			TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
			if (currentStep.m_StepType == TutorialStepType.TextPopup && (currentStep.m_StepFlags & 4194304U) != 0U)
			{
				ushort selectionID = currentStep.GetSelectionID();
				if (selectionID == 0 || (int)selectionID == magnifyCard.GetCardInstanceID())
				{
					this.AdvanceTutorial();
				}
			}
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00017320 File Offset: 0x00015520
	private void OnUnmagnifyCard(CardObject magnifyCard)
	{
		if (this.m_Tutorial != null && !this.m_Tutorial.IsCompleted())
		{
			TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
			if (currentStep.m_StepType == TutorialStepType.TextPopup && (currentStep.m_StepFlags & 8388608U) != 0U)
			{
				ushort selectionID = currentStep.GetSelectionID();
				if (selectionID == 0 || (int)selectionID == magnifyCard.GetCardInstanceID())
				{
					this.AdvanceTutorial();
				}
			}
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00003022 File Offset: 0x00001222
	private void HandleTutorialAchievements()
	{
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00017380 File Offset: 0x00015580
	private void HandleTutorialEnd()
	{
		this.HandleTutorialAchievements();
		int @int = PlayerPrefs.GetInt("CompletedTutorial", 0);
		PlayerPrefs.SetInt("CompletedTutorial", @int | 1 << Tutorial.s_CurrentTutorialIndex);
		ProfileManager.instance.GetCurrentProfile().tutorialDone |= 1U << Tutorial.s_CurrentTutorialIndex;
		ProfileManager.instance.Save();
		TimeSpan timeSpan = DateTime.Now - this.m_TutorialStepTimeStart;
		this.m_TutorialStepTimeStart = DateTime.Now;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["tutorial_type"] = Tutorial.GetTutorialName(Tutorial.s_CurrentTutorialIndex);
		dictionary["tutorial_version"] = "1";
		AnalyticsEvents.LogTutorialStepEvent(this.m_Tutorial.GenerateCurrentStepName(), (float)(this.m_Tutorial.GetCurrentUserActionStepNumber() + 1), TUTORIAL_STEP.step_status.completed, (int)timeSpan.TotalSeconds, true, dictionary);
		if (this.m_MagnifyManager != null)
		{
			this.m_MagnifyManager.SetEnableNeighborButtons(true);
			this.m_MagnifyManager.SetEnableMagnifyActionButtons(true);
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00017476 File Offset: 0x00015676
	private void HandleTutorialEventGameOver()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		if ((this.m_Tutorial.GetCurrentStep().m_StepFlags & 16384U) != 0U)
		{
			this.AdvanceTutorial();
		}
	}

	// Token: 0x06000384 RID: 900 RVA: 0x000174AC File Offset: 0x000156AC
	private void SetupTutorialStep()
	{
		if (this.m_Tutorial == null)
		{
			return;
		}
		if (this.m_Tutorial.IsCompleted())
		{
			AgricolaLib.CommitTemporaryMoveBuffer();
			this.UpdateGameOptionsSelectionState(true);
			this.HandleTutorialEnd();
			this.m_Tutorial = null;
			this.m_bAdvanceTutorial = false;
			return;
		}
		this.m_bAdvanceTutorial = false;
		this.ClearActiveTutorialElements();
		this.UpdateGameOptionsSelectionState(true);
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if (this.m_PopupManager != null)
		{
			this.m_PopupManager.HandleTutorialFlags(currentStep.m_StepFlags);
		}
		if (currentStep.m_StepType == TutorialStepType.ExitTutorial)
		{
			this.HandleTutorialEnd();
			AgricolaGame.HandleLeaveGameAnalytics("match_completed", -1);
			ScreenManager.s_onStartScreen = "Tutorials";
			AgricolaLib.ExitCurrentGame();
			ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
			return;
		}
		if (currentStep.m_StepType == TutorialStepType.UserAction)
		{
			this.ShowTutorialPanel(currentStep, false);
		}
		else if (currentStep.m_StepType != TutorialStepType.OpponentAction)
		{
			if (currentStep.m_StepType == TutorialStepType.TextPopup || currentStep.m_StepType == TutorialStepType.TextPopupHelpButton || currentStep.m_StepType == TutorialStepType.TextPopupTownButton)
			{
				this.ShowTutorialPanel(currentStep, true);
			}
			else if (currentStep.m_StepType == TutorialStepType.ContinuePrompt)
			{
				this.ShowTutorialPanel(currentStep, true);
			}
			else
			{
				TutorialStepType stepType = currentStep.m_StepType;
			}
		}
		if ((currentStep.m_StepFlags & 33554432U) != 0U)
		{
			if (this.m_familyImprovementButton != null && this.m_familyImprovementButton.activeSelf)
			{
				this.m_familyImprovementButton.GetComponent<Button>().onClick.Invoke();
			}
			this.m_MajorImprovementTray.SetTrayState(true);
		}
		if ((currentStep.m_StepFlags & 1048576U) != 0U)
		{
			ushort selectionID = currentStep.GetSelectionID();
			GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID((int)selectionID, false);
			if (cardFromInstanceID != null)
			{
				AgricolaCard component = cardFromInstanceID.GetComponent<AgricolaCard>();
				if (component != null && this.m_MagnifyManager != null)
				{
					CardObject magnifiedCard = this.m_MagnifyManager.GetMagnifiedCard();
					if (component != magnifiedCard)
					{
						AgricolaCard agricolaCard = magnifiedCard as AgricolaCard;
						if (agricolaCard != null)
						{
							agricolaCard.Unmagnify(true, true);
						}
						component.Magnify(true, false);
					}
				}
			}
		}
		uint num = currentStep.m_StepFlags & 2097152U;
		if ((currentStep.m_StepFlags & 4U) != 0U && this.m_HudRight != null)
		{
			this.m_HudRight.SetEndTurnButtonVisible(true);
		}
		this.ShowTutorialCallouts(currentStep);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x000176D4 File Offset: 0x000158D4
	public void HandleTutorialGameOptionSelection()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		if (this.m_Tutorial.GetCurrentStep().m_StepType != TutorialStepType.UserAction)
		{
			return;
		}
		this.AdvanceTutorial();
	}

	// Token: 0x06000386 RID: 902 RVA: 0x00017708 File Offset: 0x00015908
	public void AdvanceTutorial()
	{
		if (this.m_Tutorial == null)
		{
			this.m_bAdvanceTutorial = false;
			return;
		}
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if ((currentStep.m_StepFlags & 8U) != 0U && this.m_PopupManager != null)
		{
			this.m_PopupManager.SetPopup(EPopups.NONE);
		}
		if (currentStep.m_StepType == TutorialStepType.UserAction)
		{
			TimeSpan timeSpan = DateTime.Now - this.m_TutorialStepTimeStart;
			this.m_TutorialStepTimeStart = DateTime.Now;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["tutorial_type"] = Tutorial.GetTutorialName(Tutorial.s_CurrentTutorialIndex);
			dictionary["tutorial_version"] = "1";
			int @int = PlayerPrefs.GetInt("CompletedTutorial", 0);
			AnalyticsEvents.LogTutorialStepEvent(this.m_Tutorial.GenerateCurrentStepName(), (float)this.m_Tutorial.GetCurrentUserActionStepNumber(), TUTORIAL_STEP.step_status.completed, (int)timeSpan.TotalSeconds, (@int & 1 << Tutorial.s_CurrentTutorialIndex) != 0, dictionary);
		}
		this.m_Tutorial.Advance();
		this.SetupTutorialStep();
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000177F8 File Offset: 0x000159F8
	public void BackupTutorial()
	{
		if (this.m_Tutorial == null)
		{
			return;
		}
		this.m_Tutorial.Backup();
		this.SetupTutorialStep();
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00017814 File Offset: 0x00015A14
	private void UpdateTutorial()
	{
		if (this.m_Tutorial == null)
		{
			return;
		}
		if (this.m_bAdvanceTutorial)
		{
			this.AdvanceTutorial();
			return;
		}
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if (currentStep.m_StepType == TutorialStepType.Wait)
		{
			if ((currentStep.m_StepFlags & 8192U) != 0U)
			{
				if (this.m_AnimationManager != null && !this.m_AnimationManager.IsAnimationToDestination((int)currentStep.m_SelectionHint))
				{
					this.AdvanceTutorial();
					return;
				}
			}
			else if ((currentStep.m_StepFlags & 32768U) != 0U)
			{
				if (GameOptions.m_OptionCount > 0)
				{
					this.AdvanceTutorial();
					return;
				}
			}
			else if ((currentStep.m_StepFlags & 131072U) != 0U)
			{
				TransformMap transformMap = this.m_AgricolaFarm.gameObject.activeSelf ? this.m_AgricolaFarmMapController : this.m_AgricolaTownMapController;
				if (transformMap != null && !transformMap.GetIsAnimating())
				{
					this.AdvanceTutorial();
					return;
				}
			}
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x000178EC File Offset: 0x00015AEC
	public void OnActionConfirmButtonTutorialPressed()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if ((currentStep.m_StepFlags & 1U) != 0U)
		{
			return;
		}
		if (currentStep.m_StepType == TutorialStepType.TextPopup)
		{
			this.AdvanceTutorial();
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00017938 File Offset: 0x00015B38
	public void OnActionHelpButtonTutorialPressed()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if ((currentStep.m_StepFlags & 1U) != 0U)
		{
			return;
		}
		if (currentStep.m_StepType == TutorialStepType.TextPopupHelpButton)
		{
			this.AdvanceTutorial();
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00017984 File Offset: 0x00015B84
	public void OnActionTownButtonTutorialPressed()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if ((currentStep.m_StepFlags & 1U) != 0U)
		{
			return;
		}
		if (currentStep.m_StepType == TutorialStepType.TextPopupTownButton)
		{
			this.AdvanceTutorial();
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x000179D0 File Offset: 0x00015BD0
	public void OnButtonPressedTutorialContinue()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
		if (currentStep.m_StepType == TutorialStepType.ContinuePrompt)
		{
			TutorialStep[] array = null;
			if (array != null)
			{
				this.HandleTutorialAchievements();
				Tutorial.s_CurrentTutorialIndex = (int)currentStep.m_SelectionID;
				Tutorial.m_CurrentTutorialSteps = array;
				this.m_Tutorial.Start();
				this.SetupTutorialStep();
			}
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00017A35 File Offset: 0x00015C35
	public void OnButtonPressedTutorialExit()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		if (this.m_Tutorial.GetCurrentStep().m_StepType == TutorialStepType.ContinuePrompt)
		{
			this.AdvanceTutorial();
		}
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00017A66 File Offset: 0x00015C66
	public void OnButtonPressedTutorialLogButton()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		if ((this.m_Tutorial.GetCurrentStep().m_StepFlags & 262144U) != 0U)
		{
			this.AdvanceTutorial();
		}
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00017A9C File Offset: 0x00015C9C
	public void OnButtonPressedTutorialLogBack()
	{
		if (this.m_Tutorial == null || this.m_Tutorial.IsCompleted())
		{
			return;
		}
		if ((this.m_Tutorial.GetCurrentStep().m_StepFlags & 524288U) != 0U)
		{
			this.AdvanceTutorial();
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00003022 File Offset: 0x00001222
	public void OnBeginAnimationCallback(AnimateObject animateObject, uint animationFlags, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00003022 File Offset: 0x00001222
	public void OnEndAnimationCallback(AnimateObject animateObject, uint animationFlags, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00003022 File Offset: 0x00001222
	public void OnApplicationPause(bool bPaused)
	{
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00017AD4 File Offset: 0x00015CD4
	private void Update()
	{
		if (!this.m_bInitialized || this.m_bPaused)
		{
			return;
		}
		if (this.m_bEndGameWaitingForCommit && !AgricolaLib.HasTemporaryMoveBuffer())
		{
			this.HandleTutorialEventGameOver();
			if (this.m_Popup_EndGame != null && !this.m_Popup_EndGame.GetEndGameHasStarted())
			{
				this.m_Popup_EndGame.gameObject.SetActive(true);
				this.m_Popup_EndGame.StartEndGameSequence();
				if (this.m_HudRight != null)
				{
					this.m_HudRight.SetEndTurnButtonVisible(true);
				}
				this.m_bEndGameWaitingForCommit = false;
			}
		}
		if (AgricolaLib.GetIsOnlineGame() && this.m_Popup_Forfeit != null && !this.m_Popup_Forfeit.activeSelf && LoadLevelSplashScreen.instance.IsLoadSequenceComplete())
		{
			AgricolaLib.GetGamePlayerInfo(this.m_LocalPlayerIndex, this.m_bufPtr, 1024);
			if (((GamePlayerInfo)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerInfo))).forfeit != 0U)
			{
				this.m_Popup_Forfeit.SetActive(true);
				this.m_bForfeitLastPlayer = false;
				if (!LoadLevelSplashScreen.instance.IsLoadSequenceComplete())
				{
					LoadLevelSplashScreen.instance.MarkLoadComplete(1);
				}
			}
		}
		if (this.m_MagnifyManager != null && this.m_MagnifyManager.PauseForMagnifyManager())
		{
			return;
		}
		if (this.m_LowerHudOppResourceDisplay != null && this.m_LowerHudExcessAnimalDisplay != null && this.m_AgricolaFarm != null)
		{
			TrayToggle component = this.m_LowerHudExcessAnimalDisplay.GetComponent<TrayToggle>();
			TrayToggle component2 = this.m_LowerHudOppResourceDisplay.GetComponent<TrayToggle>();
			if (this.m_AgricolaFarm.gameObject.activeSelf && this.m_AgricolaFarm.GetDisplayedPlayerIndex() != this.m_LocalPlayerIndex)
			{
				component2.SetTrayState(true);
				component.SetTrayState(false);
			}
			else if (this.m_AgricolaFarm.GetIsDraggingAnimal() || this.m_LowerHudExcessAnimalDisplay.GetHasExcessAnimals())
			{
				component2.SetTrayState(false);
				component.SetTrayState(true);
			}
			else
			{
				component2.SetTrayState(false);
				component.SetTrayState(false);
			}
		}
		bool hasAnimatingObject = this.m_AnimationManager.GetHasAnimatingObject();
		if (this.m_Button_FarmToTown != null)
		{
			this.m_Button_FarmToTown.interactable = !hasAnimatingObject;
		}
		if (this.m_Button_TownToFarm != null)
		{
			this.m_Button_TownToFarm.interactable = !hasAnimatingObject;
		}
		bool flag = true;
		if (AgricolaGame.m_LoadingScreenHoldUpdateCount > 0U)
		{
			flag = false;
			if ((AgricolaGame.m_LoadingScreenHoldUpdateCount -= 1U) == 0U)
			{
				LoadLevelSplashScreen.instance.MarkLoadComplete(1);
			}
		}
		if (this.m_OnlineGameID != 0U && this.m_OnlineGameID != AgricolaLib.GetCurrentGameID())
		{
			flag = false;
		}
		if (!this.m_bWaitingAfterTurn && (this.m_AnimationManager == null || !this.m_AnimationManager.PauseForAnimationManager()))
		{
			if (this.m_PopupManager.BlockGameUpdate() && LoadLevelSplashScreen.instance.IsLoadSequenceComplete())
			{
				flag = false;
			}
			if (this.m_PauseSimulationTimer > 0f)
			{
				this.m_PauseSimulationTimer -= Time.deltaTime;
				flag = false;
			}
			if ((this.m_Button_Commit != null && this.m_Button_Commit.activeInHierarchy) || this.PauseSimulationForTutorial())
			{
				flag = false;
			}
			if (this.m_Popup_Forfeit != null && this.m_Popup_Forfeit.activeSelf)
			{
				flag = false;
			}
			if (flag)
			{
				this.m_GameEventBuffer.Update();
			}
			if (this.m_GameEventBuffer.GetEventsRemainingToProcess() == 0 && !this.m_PopupManager.BlockGameUpdate())
			{
				if (GameOptions.Update())
				{
					this.UpdateGameOptionsSelectionState(true);
				}
				int localPlayerIndex = AgricolaLib.GetLocalPlayerIndex();
				if (localPlayerIndex != this.m_LocalPlayerIndex)
				{
					this.m_bWaitingAfterTurn = true;
					this.m_LocalPlayerIndex = localPlayerIndex;
					AgricolaLib.GetGamePlayerState(localPlayerIndex, this.m_bufPtr, 1024);
					GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
					this.m_LocalPlayerInstanceID = gamePlayerState.playerInstanceID;
					this.m_LocalPlayerFactionIndex = gamePlayerState.playerFaction;
					this.m_HotseatScreen.SetIsVisible(true);
					this.m_HotseatScreen.SetData(gamePlayerState.displayName, localPlayerIndex);
				}
			}
		}
		if (this.m_HudLeft != null)
		{
			if (this.m_Tutorial != null && !this.m_Tutorial.IsCompleted())
			{
				TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
				this.m_HudLeft.SetUndoButtonVisible((currentStep.m_StepFlags & 2U) > 0U);
			}
			else
			{
				this.m_HudLeft.SetUndoButtonVisible(AgricolaLib.HasTemporaryMoveBuffer() && !this.m_DragManager.IsDraggingObject() && !this.m_PopupManager.HasActiveOrHiddenPopup());
			}
		}
		if (this.m_AgricolaFarm.GetIsFencingMode() && this.m_HudRight != null)
		{
			this.m_HudRight.SetDoneButtonVisible(this.m_AgricolaFarm.GetIsFencingArrangementValid() || GameOptions.IsSelectableHint(40990));
		}
		if (this.m_Button_Commit != null && this.m_Button_Commit.activeSelf && this.m_HudRight != null && !AgricolaLib.HasTemporaryMoveBuffer())
		{
			this.m_HudRight.SetConfirmButtonVisible(false);
		}
		if (this.m_Tutorial != null)
		{
			this.UpdateTutorial();
		}
		this.UpdateDrawPileCounts();
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00017FC0 File Offset: 0x000161C0
	private unsafe void UpdateDrawPileCounts()
	{
		GameDeckCounts gameDeckCounts = default(GameDeckCounts);
		AgricolaLib.GetGameDeckCounts(&gameDeckCounts, sizeof(GameDeckCounts));
		if (this.m_TextRoundNumber != null)
		{
			this.m_RoundNumber = gameDeckCounts.round_number;
			this.m_TextRoundNumber.text = gameDeckCounts.round_number.ToString();
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00018018 File Offset: 0x00016218
	public void SetFarmToPlayer(int playerIndex, int playerInstanceID)
	{
		if (this.m_AgricolaFarm != null && (!this.m_AgricolaFarm.gameObject.activeSelf || this.m_AgricolaFarm.GetDisplayedPlayerIndex() != playerIndex))
		{
			if (this.m_Button_TownToFarm != null)
			{
				this.m_Button_TownToFarm.onClick.Invoke();
			}
			this.m_AgricolaFarm.RebuildFarm(playerIndex, playerInstanceID);
		}
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0001807E File Offset: 0x0001627E
	public void SetTown()
	{
		if (this.m_Button_FarmToTown != null)
		{
			this.m_Button_FarmToTown.onClick.Invoke();
		}
	}

	// Token: 0x06000397 RID: 919 RVA: 0x000180A0 File Offset: 0x000162A0
	private void BeginDragCallback(DragObject dragObject)
	{
		if (dragObject.GetComponent<AgricolaWorker>() != null)
		{
			if (this.m_AgricolaFarm.gameObject.activeSelf)
			{
				this.SetTown();
			}
			return;
		}
		if (dragObject.GetComponent<AgricolaFarmAction>() != null)
		{
			if (!this.m_AgricolaFarm.gameObject.activeSelf || this.m_AgricolaFarm.GetDisplayedPlayerIndex() != AgricolaLib.GetLocalPlayerIndex())
			{
				this.SetFarmToPlayer(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			}
			return;
		}
		if (dragObject.GetComponent<AgricolaResource>() != null)
		{
			if (!this.m_AgricolaFarm.gameObject.activeSelf || this.m_AgricolaFarm.GetDisplayedPlayerIndex() != AgricolaLib.GetLocalPlayerIndex())
			{
				this.SetFarmToPlayer(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			}
			return;
		}
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00018160 File Offset: 0x00016360
	private void EndDragCallback(DragObject dragObject)
	{
		ushort dragSelectionHint = dragObject.GetDragSelectionHint();
		if (dragSelectionHint - 40982 <= 1 && this.m_MajorImprovementTray != null)
		{
			if (this.m_familyImprovementButton != null && this.m_familyImprovementButton.activeSelf)
			{
				this.m_familyImprovementButton.GetComponent<Button>().onClick.Invoke();
				return;
			}
			if (this.m_improvementCloseButton != null)
			{
				this.m_improvementCloseButton.onClick.Invoke();
			}
		}
	}

	// Token: 0x06000399 RID: 921 RVA: 0x000181DC File Offset: 0x000163DC
	private void HandleEventPause(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		OutputEventPause outputEventPause = (OutputEventPause)Marshal.PtrToStructure(event_buffer, typeof(OutputEventPause));
		if (outputEventPause.exclude_player_index != this.m_LocalPlayerIndex)
		{
			if (outputEventPause.pause_type == 1)
			{
				float num = outputEventPause.pause_timer * 0.4f + 0.4f;
				if (num > this.m_PauseSimulationTimer)
				{
					this.m_PauseSimulationTimer = num;
					return;
				}
			}
			else if (outputEventPause.pause_type == 2)
			{
				if (this.m_AnimationManager != null && this.m_AnimationManager.AddWaitForAllAnimation() && event_feedback != null)
				{
					event_feedback.bBreakFromUpdateLoop = true;
					return;
				}
			}
			else if (outputEventPause.pause_type == 3)
			{
				if (this.m_AnimationManager != null && this.m_AnimationManager.AddWaitForDestination((int)outputEventPause.animation_data) && event_feedback != null)
				{
					event_feedback.bBreakFromUpdateLoop = true;
					return;
				}
			}
			else
			{
				Debug.LogError("OutputEventPause event unknown: " + outputEventPause.pause_type.ToString() + ":" + outputEventPause.animation_data.ToString());
			}
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x000182D0 File Offset: 0x000164D0
	private void HandleEventMajorImprovementOwnerChange(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		MajorImpOwnerChange majorImpOwnerChange = (MajorImpOwnerChange)Marshal.PtrToStructure(event_buffer, typeof(MajorImpOwnerChange));
		if (majorImpOwnerChange.owner_id == 0)
		{
			if (this.m_CardManager != null)
			{
				GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(majorImpOwnerChange.major_imp_instance_id | AgricolaCardManager.s_MajImpDisplayCardMask, false);
				if (cardFromInstanceID != null)
				{
					AgricolaCard component = cardFromInstanceID.GetComponent<AgricolaCard>();
					if (component != null)
					{
						component.ClearMajorImprovementOwnerToken();
						this.m_CardManager.PlaceCardInCardLimbo(component);
					}
				}
				cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(majorImpOwnerChange.major_imp_instance_id, true);
				if (cardFromInstanceID != null)
				{
					AnimateObject component2 = cardFromInstanceID.GetComponent<AnimateObject>();
					if (component2 != null && !component2.IsAnimating())
					{
						cardFromInstanceID.SetActive(true);
						this.m_LocatorMajorImprovementTray[majorImpOwnerChange.major_imp_index].PlaceAnimateObject(component2, true, true, false);
						return;
					}
				}
			}
		}
		else if (this.m_CardManager != null)
		{
			GameObject cardFromInstanceID2 = this.m_CardManager.GetCardFromInstanceID(majorImpOwnerChange.major_imp_instance_id | AgricolaCardManager.s_MajImpDisplayCardMask, true);
			if (cardFromInstanceID2 != null)
			{
				AnimateObject component3 = cardFromInstanceID2.GetComponent<AnimateObject>();
				if (component3 != null && this.m_LocatorMajorImprovementTray.Length > majorImpOwnerChange.major_imp_index && this.m_LocatorMajorImprovementTray[majorImpOwnerChange.major_imp_index] != null)
				{
					cardFromInstanceID2.SetActive(true);
					this.m_LocatorMajorImprovementTray[majorImpOwnerChange.major_imp_index].PlaceAnimateObject(component3, true, true, false);
				}
				AgricolaLib.GetGamePlayerState(majorImpOwnerChange.owner_id, this.m_bufPtr, 1024);
				GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
				AgricolaWorker agricolaWorker = this.m_WorkerManager.CreateTemporaryWorker(0, gamePlayerState.playerFaction);
				if (agricolaWorker != null)
				{
					agricolaWorker.gameObject.SetActive(true);
					agricolaWorker.SetAvatar(gamePlayerState.playerAvatar);
					agricolaWorker.SetDragType(ECardDragType.Never);
					agricolaWorker.SetSelectable(false, Color.white);
					AgricolaCard component4 = cardFromInstanceID2.GetComponent<AgricolaCard>();
					if (component4 != null)
					{
						component4.SetCardInstanceID(0);
						component4.SetMajorImprovementOwnerToken(agricolaWorker.gameObject);
					}
				}
			}
		}
	}

	// Token: 0x0600039B RID: 923 RVA: 0x000184EC File Offset: 0x000166EC
	private void HandleEventTookBeggingCards(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		TookBeggingCards tookBeggingCards = (TookBeggingCards)Marshal.PtrToStructure(event_buffer, typeof(TookBeggingCards));
		if (this.m_AgricolaFarm != null && this.m_AgricolaFarm.GetDisplayedPlayerInstanceID() == tookBeggingCards.playerIndex)
		{
			this.m_AgricolaFarm.UpdateBeggingCardSign(tookBeggingCards.beggingCardCount);
		}
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00018544 File Offset: 0x00016744
	private void HandleEventUpdatePlayerHand(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		UpdatePlayerHand updatePlayerHand = (UpdatePlayerHand)Marshal.PtrToStructure(event_buffer, typeof(UpdatePlayerHand));
		if (updatePlayerHand.player_instance_id == 0 || updatePlayerHand.player_instance_id == this.m_LocalPlayerInstanceID)
		{
			this.RebuildLocalPlayerHand();
		}
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00018584 File Offset: 0x00016784
	private void HandleEventUpdateDisplay(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		UpdateDisplay updateDisplay = (UpdateDisplay)Marshal.PtrToStructure(event_buffer, typeof(UpdateDisplay));
		if (updateDisplay.display_type == 1)
		{
			if (this.m_AgricolaFarm.gameObject.activeSelf)
			{
				this.SetTown();
				return;
			}
		}
		else if (updateDisplay.display_type == 2 && (!this.m_AgricolaFarm.gameObject.activeSelf || this.m_AgricolaFarm.GetDisplayedPlayerIndex() != updateDisplay.player_index))
		{
			this.SetFarmToPlayer(updateDisplay.player_index, updateDisplay.player_instance_id);
		}
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0001860C File Offset: 0x0001680C
	private void HandleEventGainedAnimals(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		GainedAnimals gainedAnimals = (GainedAnimals)Marshal.PtrToStructure(event_buffer, typeof(GainedAnimals));
		if (this.m_AgricolaFarm != null)
		{
			this.SetFarmToPlayer(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			if (this.m_AgricolaFarm.GetActiveAnimalsCount((EResourceType)gainedAnimals.resource_type) < gainedAnimals.current_total)
			{
				if (gainedAnimals.container_index != -1)
				{
					int num = this.m_AgricolaFarm.ForcePlaceAnimalsInContainer((EResourceType)gainedAnimals.resource_type, gainedAnimals.resource_count, gainedAnimals.container_index);
					if (num > 0 && this.m_LowerHudExcessAnimalDisplay != null)
					{
						this.m_LowerHudExcessAnimalDisplay.ModifyExcessAnimalCount((EResourceType)gainedAnimals.resource_type, num);
						return;
					}
				}
				else
				{
					int num2 = this.m_AgricolaFarm.AttemptToPlaceAnimals((EResourceType)gainedAnimals.resource_type, gainedAnimals.resource_count);
					if (num2 > 0 && this.m_LowerHudExcessAnimalDisplay != null)
					{
						this.m_LowerHudExcessAnimalDisplay.ModifyExcessAnimalCount((EResourceType)gainedAnimals.resource_type, num2);
						return;
					}
				}
			}
			else if (this.m_AgricolaFarm.GetActiveAnimalsCount((EResourceType)gainedAnimals.resource_type) > gainedAnimals.current_total)
			{
				for (int i = this.m_AgricolaFarm.GetActiveAnimalsCount((EResourceType)gainedAnimals.resource_type) - gainedAnimals.current_total; i > 0; i--)
				{
					this.m_AgricolaFarm.RemoveAnimalFromContainer((EResourceType)gainedAnimals.resource_type);
				}
			}
		}
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001874C File Offset: 0x0001694C
	private void HandleEventMarkFarmTileDirty(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		MarkDirtyFarmTile markDirtyFarmTile = (MarkDirtyFarmTile)Marshal.PtrToStructure(event_buffer, typeof(MarkDirtyFarmTile));
		if (this.m_AgricolaFarm != null && this.m_AgricolaFarm.GetDisplayedPlayerIndex() == markDirtyFarmTile.player_instance_id)
		{
			this.m_AgricolaFarm.MarkFarmTileForUpdate(markDirtyFarmTile.farm_tile);
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x000187A4 File Offset: 0x000169A4
	private void HandleEventForceOptionPopupRestriction(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		ForceOptionPopupConvertRestriction forceOptionPopupConvertRestriction = (ForceOptionPopupConvertRestriction)Marshal.PtrToStructure(event_buffer, typeof(ForceOptionPopupConvertRestriction));
		this.SetOptionPopupRestriction(forceOptionPopupConvertRestriction.instanceID, forceOptionPopupConvertRestriction.hint, (EResourceType)forceOptionPopupConvertRestriction.resource, forceOptionPopupConvertRestriction.resourceUseLimit);
		this.UpdateGameOptionsSelectionState(true);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x000187EC File Offset: 0x000169EC
	private void HandleEventTutorialAISelectedOption(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		TutorialAISelectedOption tutorialAISelectedOption = (TutorialAISelectedOption)Marshal.PtrToStructure(event_buffer, typeof(TutorialAISelectedOption));
		if (this.m_Tutorial != null && !this.m_Tutorial.IsCompleted())
		{
			TutorialStep currentStep = this.m_Tutorial.GetCurrentStep();
			if (currentStep.m_StepType == TutorialStepType.Wait && currentStep.IsWaitForAIAction() && currentStep.m_SelectionHint == tutorialAISelectedOption.selection_hint && currentStep.GetSelectionID() == tutorialAISelectedOption.selection_id)
			{
				this.AdvanceTutorial();
			}
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00018864 File Offset: 0x00016A64
	private void HandleEventCommitPlayerDecision(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		CommitPlayerDecision commitPlayerDecision = (CommitPlayerDecision)Marshal.PtrToStructure(event_buffer, typeof(CommitPlayerDecision));
		if (this.m_Tutorial != null)
		{
			AgricolaLib.CommitTemporaryMoveBuffer();
			AchievementManagerWrapper.instance.CommitQueue();
			return;
		}
		if (event_feedback != null)
		{
			event_feedback.bBreakFromUpdateLoop = true;
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetConfirmButtonVisible(true);
		}
		if (this.m_OptionPromptAnimator != null)
		{
			this.m_OptionPromptAnimator.SetBool("isHidden", false);
		}
		if (this.m_OptionPromptText != null)
		{
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Opt_CommitYourDecision}");
			this.m_OptionPromptText.SetText(text);
			this.m_bShowingConfirmPrompt = true;
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00018914 File Offset: 0x00016B14
	private void HandleEventAchievementData(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		GameEvent.AchievementData achievementData = (GameEvent.AchievementData)Marshal.PtrToStructure(event_buffer, typeof(GameEvent.AchievementData));
		AchievementManagerWrapper.instance.IncrementAchievement((EAchievements)achievementData.achievementID, (long)achievementData.data);
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00018950 File Offset: 0x00016B50
	private void HandleEventOutputMessage(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		OutputMessage outputMessage = (OutputMessage)Marshal.PtrToStructure(event_buffer, typeof(OutputMessage));
		if (this.m_Popup_TextLog != null)
		{
			this.m_Popup_TextLog.AddTextLogLine(outputMessage.message, outputMessage.messageIndex);
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00018998 File Offset: 0x00016B98
	private void HandleEventGameOver(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		GameOver gameOver = (GameOver)Marshal.PtrToStructure(event_buffer, typeof(GameOver));
		if (AgricolaLib.HasTemporaryMoveBuffer())
		{
			this.m_bEndGameWaitingForCommit = true;
			if (this.m_HudRight != null)
			{
				this.m_HudRight.SetConfirmButtonVisible(true);
			}
			if (this.m_OptionPromptAnimator != null)
			{
				this.m_OptionPromptAnimator.SetBool("isHidden", false);
			}
			if (this.m_OptionPromptText != null)
			{
				string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Opt_CommitYourDecision}");
				this.m_OptionPromptText.SetText(text);
				this.m_bShowingConfirmPrompt = true;
			}
		}
		else
		{
			this.HandleTutorialEventGameOver();
			if (this.m_Popup_EndGame != null && !this.m_Popup_EndGame.GetEndGameHasStarted())
			{
				this.m_Popup_EndGame.gameObject.SetActive(true);
				this.m_Popup_EndGame.StartEndGameSequence();
				if (this.m_HudRight != null)
				{
					this.m_HudRight.SetEndTurnButtonVisible(true);
				}
			}
		}
		if (event_feedback != null)
		{
			event_feedback.bBreakFromUpdateLoop = true;
		}
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00018A98 File Offset: 0x00016C98
	private void HandleEventRoundAnnounce(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		TurnNumber turnNumber = (TurnNumber)Marshal.PtrToStructure(event_buffer, typeof(TurnNumber));
		if (this.m_BuildingManager != null)
		{
			this.m_BuildingManager.RebuildBuildingList();
			this.m_BuildingManager.RebuildAnimationManager(this.m_AnimationManager);
		}
		if (this.m_UpperHudTokens != null)
		{
			this.m_UpperHudTokens.ReorderTokensBasedOnTurnOrder();
		}
		EAgricolaSeason roundSeason = AgricolaLib.GetRoundSeason(turnNumber.roundNumber);
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.SetSeason(roundSeason);
		}
		if (this.m_AgricolaTownSeasonController != null)
		{
			this.m_AgricolaTownSeasonController.SetSeason(roundSeason);
		}
		if (!AgricolaLib.GetIsTutorialGame())
		{
			this.SetTown();
			RoundAnnouncements roundAnnouncements = this.m_PopupManager.GetPopup(EPopups.ANNOUNCE_ROUND) as RoundAnnouncements;
			if (roundAnnouncements != null)
			{
				this.m_PopupManager.SetPopup(EPopups.ANNOUNCE_ROUND);
				roundAnnouncements.ShowRoundAnnounce((uint)turnNumber.roundNumber, turnNumber.playerName, turnNumber.playerFaction, this.m_CardManager, turnNumber.actionCardInstanceID);
			}
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00018B98 File Offset: 0x00016D98
	private void HandleEventHarvestPhase(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		HarvestPhase harvestPhase = (HarvestPhase)Marshal.PtrToStructure(event_buffer, typeof(HarvestPhase));
		if (harvestPhase.harvest_phase == 1 && (!this.m_AgricolaFarm.gameObject.activeSelf || this.m_AgricolaFarm.GetDisplayedPlayerIndex() != AgricolaLib.GetLocalPlayerIndex()))
		{
			this.SetFarmToPlayer(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
		}
		EAgricolaSeason season = EAgricolaSeason.AUTUMN;
		if (harvestPhase.harvest_phase == 2)
		{
			season = EAgricolaSeason.WINTER;
		}
		else if (harvestPhase.harvest_phase == 3)
		{
			season = EAgricolaSeason.SPRING;
		}
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.SetSeason(season);
		}
		if (this.m_AgricolaTownSeasonController != null)
		{
			this.m_AgricolaTownSeasonController.SetSeason(season);
		}
		RoundAnnouncements roundAnnouncements = this.m_PopupManager.GetPopup(EPopups.ANNOUNCE_ROUND) as RoundAnnouncements;
		if (roundAnnouncements != null && harvestPhase.play_anim != 0)
		{
			this.m_PopupManager.SetPopup(EPopups.ANNOUNCE_ROUND);
			roundAnnouncements.StartHarvestAnnounce(harvestPhase.harvest_phase);
		}
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00018C84 File Offset: 0x00016E84
	private void HandleEventDraftMode(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		if (((DraftMode)Marshal.PtrToStructure(event_buffer, typeof(DraftMode))).draft_mode != 0)
		{
			if (this.m_DraftInterface != null)
			{
				this.m_DraftInterface.gameObject.SetActive(true);
			}
			if (this.m_LocalPlayerData != null)
			{
				this.m_LocalPlayerData.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			if (this.m_DraftInterface != null)
			{
				this.m_DraftInterface.gameObject.SetActive(false);
			}
			if (this.m_LocalPlayerData != null)
			{
				this.m_LocalPlayerData.gameObject.SetActive(true);
			}
			this.RebuildLocalPlayerHand();
		}
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00018D30 File Offset: 0x00016F30
	private void HandleEventLoadProgress(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		LoadProgress loadProgress = (LoadProgress)Marshal.PtrToStructure(event_buffer, typeof(LoadProgress));
		if (loadProgress.progress == 2f)
		{
			if (AgricolaLib.NetworkGetCurrentGameFinished() && this.m_Popup_EndGame != null && !this.m_Popup_EndGame.GetEndGameHasStarted())
			{
				this.m_Popup_EndGame.StartEndGameSequence();
				if (this.m_HudRight != null)
				{
					this.m_HudRight.SetEndTurnButtonVisible(true);
				}
			}
			if (event_feedback != null)
			{
				event_feedback.bBreakFromUpdateLoop = true;
			}
			AgricolaGame.m_LoadingScreenHoldUpdateCount = 3U;
			this.RebuildGame();
			GameOptions.ResendGameOptionsList();
			AchievementManagerWrapper.instance.SetEnabled(!AgricolaLib.GetIsHotseatGame() && !AgricolaLib.GetIsTutorialGame());
			if (AgricolaLib.GetIsHotseatGame())
			{
				GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				int newLocalPlayerID = AgricolaLib.GetNewLocalPlayerID();
				AgricolaLib.GetGamePlayerState(newLocalPlayerID, intPtr, 1024);
				GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerState));
				this.m_bWaitingAfterTurn = true;
				this.m_HotseatScreen.SetIsVisible(true);
				this.m_HotseatScreen.SetData(gamePlayerState.displayName, newLocalPlayerID);
				gchandle.Free();
				return;
			}
		}
		else
		{
			LoadLevelSplashScreen.instance.SetProgressDisplay(loadProgress.progress, 1);
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00018E70 File Offset: 0x00017070
	public void UpdateGameOptionsSelectionState(bool bHighlight)
	{
		if (this.m_Tutorial != null)
		{
			this.m_Tutorial.UpdateGameOptions();
		}
		if (this.m_CardManager != null)
		{
			this.m_CardManager.UpdateSelectionState(bHighlight, true);
		}
		if (this.m_WorkerManager != null)
		{
			this.m_WorkerManager.UpdateSelectionState(bHighlight);
		}
		if (this.m_BuildingManager != null)
		{
			this.m_BuildingManager.UpdateSelectionState(bHighlight);
		}
		if (this.m_CardInPlayManager != null)
		{
			this.m_CardInPlayManager.UpdateSelectionState(bHighlight);
		}
		if (this.m_LocalPlayerWorkerTray != null)
		{
			this.m_LocalPlayerWorkerTray.UpdateGameOptionsSelectionState(bHighlight);
		}
		if (GameOptions.IsSelectableHint(40994) && this.m_AgricolaFarm != null)
		{
			if (!this.m_AgricolaFarm.gameObject.activeSelf || this.m_AgricolaFarm.GetDisplayedPlayerIndex() != AgricolaLib.GetLocalPlayerIndex())
			{
				this.SetFarmToPlayer(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			}
			this.m_AgricolaFarm.SetFeedingMode(true, bHighlight);
		}
		if (this.m_LocalPlayerData != null)
		{
			this.m_LocalPlayerData.UpdateGameOptionsSelectionState(bHighlight);
		}
		if (this.m_DraftInterface != null)
		{
			this.m_DraftInterface.UpdateGameOptionsSelectionState(bHighlight);
		}
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.UpdateSelectionState(bHighlight);
			if (bHighlight)
			{
				this.m_AgricolaFarm.CheckForUpdatedAnimalContainers();
			}
		}
		if (this.m_PopupManager != null && !this.m_PopupManager.HasActiveOrHiddenPopup())
		{
			AgricolaOptionPopup agricolaOptionPopup = this.m_PopupManager.GetPopup(EPopups.OPTION_SELECTION) as AgricolaOptionPopup;
			if (agricolaOptionPopup != null)
			{
				if (this.optionPopupRestrictions.bUseRestriction)
				{
					if (agricolaOptionPopup.DisplayOptionList(AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_SHOW_ALL, this, this.optionPopupRestrictions))
					{
						this.m_PopupManager.SetPopup(EPopups.OPTION_SELECTION);
					}
					this.optionPopupRestrictions.bUseRestriction = false;
				}
				else if (agricolaOptionPopup.DisplayOptionList(AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_SHOW_NON_FOOD, this, this.optionPopupRestrictions))
				{
					this.m_PopupManager.SetPopup(EPopups.OPTION_SELECTION);
				}
			}
		}
		if (GameOptions.IsSelectableHint(40981) && this.m_AgricolaFarm != null)
		{
			if (!this.m_AgricolaFarm.gameObject.activeSelf || this.m_AgricolaFarm.GetDisplayedPlayerIndex() != AgricolaLib.GetLocalPlayerIndex())
			{
				this.SetFarmToPlayer(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			}
			this.m_AgricolaFarm.SetFencingMode(true);
		}
		if (GameOptions.IsSelectableHint(40982) || GameOptions.IsSelectableHint(40983))
		{
			bool flag = false;
			int instanceList = AgricolaLib.GetInstanceList(8, 0, this.m_cardBuffer, 128);
			for (int i = 0; i < instanceList; i++)
			{
				if (GameOptions.IsSelectableInstanceID((ushort)this.m_cardList[i]))
				{
					flag = true;
					break;
				}
			}
			if (this.m_MajorImprovementTray != null)
			{
				if (this.m_familyImprovementButton != null && this.m_familyImprovementButton.activeSelf)
				{
					this.m_familyImprovementButton.GetComponent<Button>().onClick.Invoke();
				}
				else if (this.m_improvementFullMajorButton != null && flag)
				{
					this.m_improvementFullMajorButton.onClick.Invoke();
				}
				else if (this.m_improvementFullMinorButton != null && !flag)
				{
					this.m_improvementFullMinorButton.onClick.Invoke();
				}
				this.m_MajorImprovementTray.SetTrayState(true);
			}
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetEndTurnButtonVisible((bHighlight && GameOptions.IsSelectableHint(40961)) || (this.m_Popup_EndGame != null && this.m_Popup_EndGame.GetEndGameHasStarted()));
			bool doneButtonVisible = bHighlight & ((GameOptions.IsSelectableHint(40960) && !this.m_PopupManager.HasActiveOrHiddenPopup()) || GameOptions.IsSelectableHint(40989) || GameOptions.IsSelectableHint(40986) || GameOptions.IsSelectableHint(40987) || GameOptions.IsSelectableHint(40988) || GameOptions.IsSelectableHint(40994) || GameOptions.IsSelectableHint(40995) || GameOptions.IsSelectableHint(40993) || this.m_AgricolaFarm.GetIsFencingArrangementValid() || GameOptions.IsSelectableHint(40990));
			this.m_HudRight.SetDoneButtonVisible(doneButtonVisible);
		}
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.SetAreAnimalsDraggable();
		}
		if (!this.m_bShowingConfirmPrompt)
		{
			if (this.m_OptionPromptAnimator != null)
			{
				this.m_OptionPromptAnimator.SetBool("isHidden", GameOptions.m_OptionPrompt == string.Empty);
			}
			if (this.m_OptionPromptText != null)
			{
				string optionPrompt = GameOptions.m_OptionPrompt;
				string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(optionPrompt);
				this.m_OptionPromptText.SetText(text);
				this.m_bShowingConfirmPrompt = false;
			}
		}
	}

	// Token: 0x060003AB RID: 939 RVA: 0x000192FE File Offset: 0x000174FE
	public void SetOptionPopupRestriction(int instanceID, ushort selectionHint, EResourceType resource, int resourceUseLimit)
	{
		this.optionPopupRestrictions.bUseRestriction = true;
		this.optionPopupRestrictions.instanceID = instanceID;
		this.optionPopupRestrictions.selectionHint = selectionHint;
		this.optionPopupRestrictions.resource = resource;
		this.optionPopupRestrictions.resourceUseLimit = resourceUseLimit;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00019340 File Offset: 0x00017540
	private AgricolaResource CreatePlaceResource(GameObject prefab, GameObject locator, int option_index)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		AnimateObject component = gameObject.GetComponent<AnimateObject>();
		if (component != null && this.m_AnimationManager != null)
		{
			component.SetAnimationManager(this.m_AnimationManager);
		}
		if (locator != null)
		{
			gameObject.transform.SetParent(locator.transform, true);
			gameObject.transform.position = locator.transform.position;
			gameObject.transform.rotation = locator.transform.rotation;
		}
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(true);
		DragObject component2 = gameObject.GetComponent<DragObject>();
		if (component2 != null && this.m_DragManager != null)
		{
			component2.SetDragManager(this.m_DragManager);
		}
		AgricolaResource component3 = gameObject.GetComponent<AgricolaResource>();
		if (component3 != null)
		{
			component3.AddOnEndDragCallback(new DragObject.DragObjectCallback(this.EndDragTavernResourceCallback));
			component3.Colorize((uint)this.GetLocalPlayerColorIndex());
			component3.SetGameOptionIndex(option_index);
			component3.SetIsDraggable(true);
			component3.ActivateHighlight(true);
		}
		return component3;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001944A File Offset: 0x0001764A
	private void EndDragTavernResourceCallback(DragObject dragObject, PointerEventData eventData)
	{
		dragObject == null;
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00019454 File Offset: 0x00017654
	public void OnButtonPressedNoAction()
	{
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetDoneButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetEndTurnButtonVisible(false);
		}
		if (this.m_HudLeft != null)
		{
			this.m_HudLeft.SetUndoButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetConfirmButtonVisible(false);
		}
		if (this.m_OptionPromptAnimator != null)
		{
			this.m_OptionPromptAnimator.SetBool("isHidden", true);
		}
		if (this.m_OptionPromptText != null)
		{
			this.m_OptionPromptText.SetText(string.Empty);
			this.m_bShowingConfirmPrompt = false;
		}
		if (GameOptions.IsSelectableHint(40960))
		{
			GameOptions.SelectOptionByHint(40960);
			return;
		}
		if (GameOptions.IsSelectableHint(40989))
		{
			GameOptions.SelectOptionByHint(40989);
			return;
		}
		if (GameOptions.IsSelectableHint(40986))
		{
			GameOptions.SelectOptionByHint(40986);
			return;
		}
		if (GameOptions.IsSelectableHint(40987))
		{
			GameOptions.SelectOptionByHint(40987);
			return;
		}
		if (GameOptions.IsSelectableHint(40988))
		{
			GameOptions.SelectOptionByHint(40988);
			return;
		}
		if (GameOptions.IsSelectableHint(40993))
		{
			GameOptions.SelectOptionByHint(40993);
			return;
		}
		if (GameOptions.IsSelectableHint(40995))
		{
			GameOptions.SelectOptionByHint(40995);
			if (this.m_LowerHudExcessAnimalDisplay != null)
			{
				this.m_LowerHudExcessAnimalDisplay.Reset();
				return;
			}
		}
		else if (GameOptions.IsSelectableHint(40994))
		{
			AgricolaLib.GetGamePlayerState(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
			GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
			int feedingAmountUsed = this.m_AgricolaFarm.GetFeedingAmountUsed();
			int foodRequirement = gamePlayerState.foodRequirement;
			int resourceCountFood = gamePlayerState.resourceCountFood;
			int num = (foodRequirement < resourceCountFood) ? foodRequirement : resourceCountFood;
			if (feedingAmountUsed < foodRequirement && this.m_Popup_ConfirmAction_Text != null && PlayerPrefs.GetInt("Option_Confirmations", 1) != 0 && !AgricolaLib.GetIsTutorialGame())
			{
				string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_ChooseTakeBegging}");
				text = text.Replace("%d", (foodRequirement - feedingAmountUsed).ToString());
				this.m_Popup_ConfirmAction_Text.text = text;
				this.m_PopupManager.SetPopup(EPopups.CONFIRM);
				return;
			}
			if (feedingAmountUsed < num)
			{
				GameOptions.SelectOptionByHintWithData(40994, (uint)(num - feedingAmountUsed));
			}
			else
			{
				GameOptions.SelectOptionByHint(40994);
			}
			this.m_AgricolaFarm.SetFeedingMode(false, false);
			AgricolaLib.CommitTemporaryMoveBuffer();
			AchievementManagerWrapper.instance.CommitQueue();
			return;
		}
		else
		{
			if (this.m_AgricolaFarm != null && this.m_AgricolaFarm.GetIsFencingArrangementValid())
			{
				this.m_AgricolaFarm.SubmitFencedPastures(this.m_GameEventBuffer);
				return;
			}
			if (GameOptions.IsSelectableHint(40990))
			{
				this.m_AgricolaFarm.SetFencingMode(false);
				GameOptions.SelectOptionByHint(40990);
			}
		}
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001972C File Offset: 0x0001792C
	public void OnConfirmPopupConfirmation()
	{
		if (GameOptions.IsSelectableHint(40961))
		{
			if (this.m_AgricolaFarm != null && this.m_AgricolaFarm.HasAnimalPlacementToCommit())
			{
				this.m_AgricolaFarm.SubmitAnimalPlacement();
			}
			if (this.m_LowerHudExcessAnimalDisplay != null)
			{
				this.m_LowerHudExcessAnimalDisplay.Reset();
			}
			GameOptions.SelectOptionByHint(40961);
			AgricolaLib.CommitTemporaryMoveBuffer();
			AchievementManagerWrapper.instance.CommitQueue();
			return;
		}
		if (GameOptions.IsSelectableHint(40994))
		{
			AgricolaLib.GetGamePlayerState(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
			GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
			int feedingAmountUsed = this.m_AgricolaFarm.GetFeedingAmountUsed();
			int foodRequirement = gamePlayerState.foodRequirement;
			int resourceCountFood = gamePlayerState.resourceCountFood;
			int num = (foodRequirement < resourceCountFood) ? foodRequirement : resourceCountFood;
			if (feedingAmountUsed < num)
			{
				GameOptions.SelectOptionByHintWithData(40994, (uint)(num - feedingAmountUsed));
			}
			else
			{
				GameOptions.SelectOptionByHint(40994);
			}
			this.m_AgricolaFarm.SetFeedingMode(false, false);
			AgricolaLib.CommitTemporaryMoveBuffer();
			AchievementManagerWrapper.instance.CommitQueue();
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0001983C File Offset: 0x00017A3C
	public void OnButtonPressedEndTurn()
	{
		if (this.m_Tutorial != null && !this.m_Tutorial.IsCompleted() && (this.m_Tutorial.GetCurrentStep().m_StepFlags & 4U) != 0U)
		{
			return;
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetDoneButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetEndTurnButtonVisible(false);
		}
		if (this.m_HudLeft != null)
		{
			this.m_HudLeft.SetUndoButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetConfirmButtonVisible(false);
		}
		if (this.m_Popup_EndGame != null && this.m_Popup_EndGame.GetEndGameHasStarted())
		{
			this.m_Popup_EndGame.gameObject.SetActive(true);
			if (this.m_HudRight != null)
			{
				this.m_HudRight.SetEndTurnButtonVisible(true);
				return;
			}
		}
		else
		{
			if (this.m_OptionPromptAnimator != null)
			{
				this.m_OptionPromptAnimator.SetBool("isHidden", true);
			}
			if (this.m_OptionPromptText != null)
			{
				this.m_OptionPromptText.SetText(string.Empty);
				this.m_bShowingConfirmPrompt = false;
			}
			if (GameOptions.IsSelectableHint(40961))
			{
				if (this.m_Popup_ConfirmAction_Text != null && PlayerPrefs.GetInt("Option_Confirmations", 1) != 0 && !AgricolaLib.GetIsTutorialGame() && this.m_LowerHudExcessAnimalDisplay != null && this.m_LowerHudExcessAnimalDisplay.GetHasExcessAnimals())
				{
					this.m_Popup_ConfirmAction_Text.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_ChooseExcessAnimals}");
					this.m_PopupManager.SetPopup(EPopups.CONFIRM);
					return;
				}
				if (this.m_AgricolaFarm != null && this.m_AgricolaFarm.HasAnimalPlacementToCommit())
				{
					this.m_AgricolaFarm.SubmitAnimalPlacement();
				}
				if (this.m_LowerHudExcessAnimalDisplay != null)
				{
					this.m_LowerHudExcessAnimalDisplay.Reset();
				}
				GameOptions.SelectOptionByHint(40961);
				AgricolaLib.CommitTemporaryMoveBuffer();
				AchievementManagerWrapper.instance.CommitQueue();
			}
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00019A38 File Offset: 0x00017C38
	public void OnButtonPressedCommit()
	{
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetDoneButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetEndTurnButtonVisible(false);
		}
		if (this.m_HudLeft != null)
		{
			this.m_HudLeft.SetUndoButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetConfirmButtonVisible(false);
		}
		if (this.m_OptionPromptAnimator != null)
		{
			this.m_OptionPromptAnimator.SetBool("isHidden", true);
		}
		if (this.m_OptionPromptText != null)
		{
			this.m_OptionPromptText.SetText(string.Empty);
			this.m_bShowingConfirmPrompt = false;
		}
		AgricolaLib.CommitTemporaryMoveBuffer();
		AchievementManagerWrapper.instance.CommitQueue();
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00019B00 File Offset: 0x00017D00
	public void OnButtonPressedUndo()
	{
		if (this.m_Tutorial != null && !this.m_Tutorial.IsCompleted())
		{
			return;
		}
		this.m_PopupManager.SetPopup(EPopups.NONE);
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetDoneButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetEndTurnButtonVisible(false);
		}
		if (this.m_HudLeft != null)
		{
			this.m_HudLeft.SetUndoButtonVisible(false);
		}
		if (this.m_HudRight != null)
		{
			this.m_HudRight.SetConfirmButtonVisible(false);
		}
		if (this.m_OptionPromptText != null)
		{
			this.m_OptionPromptText.SetText(string.Empty);
			this.m_bShowingConfirmPrompt = false;
		}
		if (this.m_GameEventBuffer != null)
		{
			this.m_GameEventBuffer.Reset();
		}
		GameOptions.Reset();
		AgricolaLib.RevertTemporaryMoveBuffer();
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.SetFeedingMode(false, false);
		}
		this.RebuildGame();
		AchievementManagerWrapper.instance.ClearQueue();
		GameOptions.ResendGameOptionsList();
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00019C0C File Offset: 0x00017E0C
	public void OnButtonPressedPlayRoundAnnounce()
	{
		if (AgricolaLib.GetIsTutorialGame() || this.m_PopupManager.GetActivePopup() != EPopups.NONE)
		{
			return;
		}
		RoundAnnouncements roundAnnouncements = this.m_PopupManager.GetPopup(EPopups.ANNOUNCE_ROUND) as RoundAnnouncements;
		if (roundAnnouncements != null && this.m_UpperHudTokens != null)
		{
			AgricolaLib.GetGamePlayerState(AgricolaLib.GetStartingPlayerIndex(), this.m_bufPtr, 1024);
			GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
			this.m_PopupManager.SetPopup(EPopups.ANNOUNCE_ROUND);
			roundAnnouncements.ShowRoundAnnounce((uint)AgricolaLib.GetCurrentRound(), gamePlayerState.displayName, gamePlayerState.playerFaction, null, 0);
		}
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00019CB0 File Offset: 0x00017EB0
	public void OnForfeitPopupButtonPressed()
	{
		AgricolaGame.HandleLeaveGameAnalytics("time_up", 0);
		AgricolaLib.NetworkForfeitGame(AgricolaLib.GetCurrentGameID(), this.m_bForfeitLastPlayer);
		AchievementManagerWrapper.instance.ClearQueue();
		ScreenManager.s_onStartScreen = "OnlineLobby";
		ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00003022 File Offset: 0x00001222
	public void OnCurrentPopupChanged(EPopups newPopup, EPopups oldPopup)
	{
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00019CFE File Offset: 0x00017EFE
	public void OnHotseatScreenPressed()
	{
		if (this.m_bWaitingAfterTurn)
		{
			this.SwitchToPlayer(this.m_LocalPlayerIndex, false);
			this.m_HotseatScreen.SetIsVisible(false);
			this.m_bWaitingAfterTurn = false;
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00019D28 File Offset: 0x00017F28
	public void PauseButtonPressed()
	{
		if (!this.m_bPaused)
		{
			this.m_bPaused = true;
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00019D39 File Offset: 0x00017F39
	public void ResumeButtonPressed()
	{
		if (this.m_bPaused)
		{
			this.m_bPaused = false;
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00003022 File Offset: 0x00001222
	public void SummaryScreenStart()
	{
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00003022 File Offset: 0x00001222
	public void SummaryScreenEnd()
	{
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00019D4C File Offset: 0x00017F4C
	private void SwitchToPlayer(int playerID, bool bInitialize)
	{
		GameOptions.Reset();
		this.m_LocalPlayerIndex = AgricolaLib.GetLocalPlayerIndex();
		this.m_LocalPlayerInstanceID = AgricolaLib.GetLocalPlayerInstanceID();
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.GetGamePlayerState(this.m_LocalPlayerIndex, intPtr, 1024);
		GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerState));
		this.m_LocalPlayerFactionIndex = gamePlayerState.playerFaction;
		Popup_Chat popup_Chat = null;
		if (this.m_Popup_Chat != null && AgricolaLib.GetIsOnlineGame())
		{
			popup_Chat = this.m_Popup_Chat.GetComponent<Popup_Chat>();
		}
		if (this.m_LocalPlayerData != null)
		{
			this.m_LocalPlayerData.SetPlayerIndex(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			this.m_LocalPlayerData.SetFactionIndex(gamePlayerState.playerFaction);
			if (popup_Chat != null)
			{
				popup_Chat.ClearFactionDictionary();
				popup_Chat.AddFactionIndex((uint)this.m_LocalPlayerIndex, gamePlayerState.playerFaction);
			}
		}
		if (this.m_DraftInterface != null)
		{
			this.m_DraftInterface.SetPlayerIndex(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
		}
		int num = 1;
		for (;;)
		{
			int localOpponentPlayerIndex = AgricolaLib.GetLocalOpponentPlayerIndex(num);
			if (localOpponentPlayerIndex == 0)
			{
				break;
			}
			AgricolaLib.GetGamePlayerState(localOpponentPlayerIndex, intPtr, 1024);
			GamePlayerState gamePlayerState2 = (GamePlayerState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerState));
			if (popup_Chat != null)
			{
				popup_Chat.AddFactionIndex((uint)localOpponentPlayerIndex, gamePlayerState2.playerFaction);
			}
			num++;
		}
		if (this.m_LocalPlayerWorkerTray != null)
		{
			Sprite factionSprite = null;
			if (this.m_BuildingManager != null)
			{
				factionSprite = this.m_BuildingManager.GetBuildingFactionSprite(gamePlayerState.playerFaction);
			}
			this.m_LocalPlayerWorkerTray.SetPlayerIndex(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			this.m_LocalPlayerWorkerTray.SetFactionIndex(gamePlayerState.playerFaction, factionSprite);
		}
		gchandle.Free();
		this.RebuildGame();
		GameOptions.ResendGameOptionsList();
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00019F24 File Offset: 0x00018124
	private void RebuildGame()
	{
		this.m_PauseSimulationTimer = 0f;
		if (this.m_PopupManager != null)
		{
			this.m_PopupManager.SetPopup(EPopups.NONE);
		}
		if (this.m_Popup_Chat != null && AgricolaLib.GetIsOnlineGame())
		{
			AgricolaLib.GetGamePlayerInfo(this.m_LocalPlayerIndex, this.m_bufPtr, 1024);
			GamePlayerInfo gamePlayerInfo = (GamePlayerInfo)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerInfo));
			this.m_Popup_Chat.GetComponent<Popup_Chat>().SetLocalUsername(gamePlayerInfo.displayName);
		}
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.DestroyAllAnimatingResources();
		}
		if (this.m_CardManager != null)
		{
			this.m_CardManager.PlaceAllCardsInLimbo();
		}
		if (this.m_CardInPlayManager != null)
		{
			this.m_CardInPlayManager.PlaceAllCardsInPlayInLimbo();
		}
		if (this.m_WorkerManager != null)
		{
			this.m_WorkerManager.PlaceAllWorkersInLimbo();
		}
		if (this.m_Popup_TextLog != null)
		{
			this.m_Popup_TextLog.RebuildTextList();
		}
		if (this.m_CardInPlayManager != null)
		{
			this.m_CardInPlayManager.RebuildCardInPlayList();
		}
		if (this.m_BuildingManager != null)
		{
			this.m_BuildingManager.RebuildBuildingList();
		}
		if (this.m_WorkerManager != null)
		{
			this.m_WorkerManager.RebuildWorkerList();
		}
		this.RebuildResolvingCardList();
		this.RebuildInterface();
		this.RebuildAnimationManager();
		this.RebuildPopups();
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0001A094 File Offset: 0x00018294
	public void RebuildResolvingCardList()
	{
		if (this.m_CardManager == null || this.m_LocatorResolvePosition == null)
		{
			return;
		}
		int i = 0;
		while (i < this.m_LocatorResolvePosition.transform.childCount)
		{
			AgricolaCard component = this.m_LocatorResolvePosition.transform.GetChild(i).gameObject.GetComponent<AgricolaCard>();
			if (component != null)
			{
				this.m_CardManager.PlaceCardInCardLimbo(component);
			}
			else
			{
				i++;
			}
		}
		int[] array = new int[16];
		GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr pInstanceIDs = gchandle.AddrOfPinnedObject();
		int instanceList = AgricolaLib.GetInstanceList(15, 0, pInstanceIDs, 16);
		for (i = 0; i < instanceList; i++)
		{
			int instanceID = array[i];
			GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(instanceID, true);
			if (!(cardFromInstanceID == null))
			{
				AnimateObject component2 = cardFromInstanceID.GetComponent<AnimateObject>();
				if (component2 != null)
				{
					cardFromInstanceID.SetActive(true);
					this.m_LocatorResolvePosition.PlaceAnimateObject(component2, true, true, false);
				}
			}
		}
		gchandle.Free();
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0001A194 File Offset: 0x00018394
	private void RebuildInterface()
	{
		if (this.m_OptionPromptAnimator != null)
		{
			this.m_OptionPromptAnimator.SetBool("isHidden", true);
		}
		if (this.m_OptionPromptText != null)
		{
			this.m_OptionPromptText.SetText(string.Empty);
			this.m_bShowingConfirmPrompt = false;
		}
		if (this.m_UpperHudTokens != null)
		{
			this.m_UpperHudTokens.Init();
			this.m_UpperHudTokens.AddPlayer(this.m_LocalPlayerIndex, this.m_LocalPlayerInstanceID);
			int num = 1;
			for (;;)
			{
				int localOpponentPlayerIndex = AgricolaLib.GetLocalOpponentPlayerIndex(num);
				if (localOpponentPlayerIndex == 0)
				{
					break;
				}
				int localOpponentInstanceID = AgricolaLib.GetLocalOpponentInstanceID(num);
				this.m_UpperHudTokens.AddPlayer(localOpponentPlayerIndex, localOpponentInstanceID);
				num++;
			}
			AgricolaLib.GetGamePlayerInfo(this.m_LocalPlayerIndex, this.m_bufPtr, 1024);
			GamePlayerInfo gamePlayerInfo = (GamePlayerInfo)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerInfo));
			this.m_UpperHudTokens.SetLocalPlayerName(gamePlayerInfo.displayName);
		}
		this.RebuildMajorImprovementsList(this.m_CardManager);
		this.RebuildLocalPlayerHand();
		if (this.m_DraftInterface != null)
		{
			this.m_DraftInterface.RebuildInterface();
		}
		EAgricolaSeason season = AgricolaLib.GetRoundSeason(AgricolaLib.GetCurrentRound());
		int currentHarvestMode = AgricolaLib.GetCurrentHarvestMode();
		if (currentHarvestMode == 2)
		{
			season = EAgricolaSeason.AUTUMN;
		}
		else if (currentHarvestMode == 3)
		{
			season = EAgricolaSeason.WINTER;
		}
		else if (currentHarvestMode == 4)
		{
			season = EAgricolaSeason.SPRING;
		}
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.SetSeason(season);
		}
		if (this.m_AgricolaTownSeasonController != null)
		{
			this.m_AgricolaTownSeasonController.SetSeason(season);
		}
		GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
		if (this.m_familyImprovementButton != null)
		{
			this.m_familyImprovementButton.SetActive(gameParameters.deckFlags == 0);
		}
		if (this.m_fullImprovementButtons != null && this.m_fullImprovementButtons.Length != 0)
		{
			for (int i = 0; i < this.m_fullImprovementButtons.Length; i++)
			{
				if (this.m_fullImprovementButtons[i] != null)
				{
					this.m_fullImprovementButtons[i].SetActive(gameParameters.deckFlags > 0);
				}
			}
		}
		if (this.m_LocalPlayerWorkerTray != null)
		{
			this.m_LocalPlayerWorkerTray.RebuildInterface();
		}
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.ClearAnimalContainerSubmission();
			if (this.m_AgricolaFarm.gameObject.activeSelf)
			{
				this.m_AgricolaFarm.RebuildFarm();
				return;
			}
			this.m_AgricolaFarm.RebuildFarm(0, 0);
		}
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001A3FC File Offset: 0x000185FC
	public void ResetMajorImprovementsTray()
	{
		if (this.m_LocatorMajorImprovementTray == null || this.m_CardManager == null)
		{
			return;
		}
		for (int i = 0; i < this.m_LocatorMajorImprovementTray.Length; i++)
		{
			int j = 0;
			while (j < this.m_LocatorMajorImprovementTray[i].transform.childCount)
			{
				AgricolaCard component = this.m_LocatorMajorImprovementTray[i].transform.GetChild(j).gameObject.GetComponent<AgricolaCard>();
				if (component != null)
				{
					this.m_CardManager.PlaceCardInCardLimbo(component);
				}
				else
				{
					j++;
				}
			}
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001A488 File Offset: 0x00018688
	private void RebuildMajorImprovementsList(AgricolaCardManager cardManager)
	{
		if (this.m_LocatorMajorImprovementTray == null || cardManager == null)
		{
			return;
		}
		this.ResetMajorImprovementsTray();
		int instanceList = AgricolaLib.GetInstanceList(8, 0, this.m_cardBuffer, 128);
		for (int i = 0; i < instanceList; i++)
		{
			int num = this.m_cardList[i];
			int majorImprovementOwnerIndex = AgricolaLib.GetMajorImprovementOwnerIndex((ushort)num);
			if (majorImprovementOwnerIndex != 0)
			{
				GameObject cardFromInstanceID = cardManager.GetCardFromInstanceID(num | AgricolaCardManager.s_MajImpDisplayCardMask, true);
				if (!(cardFromInstanceID == null))
				{
					AnimateObject component = cardFromInstanceID.GetComponent<AnimateObject>();
					if (component != null)
					{
						cardFromInstanceID.SetActive(true);
						this.m_LocatorMajorImprovementTray[i].PlaceAnimateObject(component, true, true, false);
					}
					AgricolaLib.GetGamePlayerState(majorImprovementOwnerIndex, this.m_bufPtr, 1024);
					GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
					AgricolaWorker agricolaWorker = this.m_WorkerManager.CreateTemporaryWorker(0, gamePlayerState.playerFaction);
					if (agricolaWorker != null)
					{
						agricolaWorker.gameObject.SetActive(true);
						agricolaWorker.SetAvatar(gamePlayerState.playerAvatar);
						agricolaWorker.SetDragType(ECardDragType.Never);
						agricolaWorker.SetSelectable(false, Color.white);
						AgricolaCard component2 = cardFromInstanceID.GetComponent<AgricolaCard>();
						if (component2 != null)
						{
							component2.SetCardInstanceID(0);
							component2.SetMajorImprovementOwnerToken(agricolaWorker.gameObject);
						}
					}
				}
			}
			else
			{
				GameObject cardFromInstanceID2 = cardManager.GetCardFromInstanceID(num, true);
				if (!(cardFromInstanceID2 == null))
				{
					AnimateObject component3 = cardFromInstanceID2.GetComponent<AnimateObject>();
					if (component3 != null)
					{
						cardFromInstanceID2.SetActive(true);
						this.m_LocatorMajorImprovementTray[i].PlaceAnimateObject(component3, true, true, false);
					}
				}
			}
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0001A620 File Offset: 0x00018820
	private void RebuildLocalPlayerHand()
	{
		if (this.m_LocatorLocalPlayerHand == null || this.m_CardManager == null)
		{
			return;
		}
		for (int i = 0; i < this.m_LocatorLocalPlayerHand.Length; i++)
		{
			int j = 0;
			while (j < this.m_LocatorLocalPlayerHand[i].transform.childCount)
			{
				AgricolaCard component = this.m_LocatorLocalPlayerHand[i].transform.GetChild(j).gameObject.GetComponent<AgricolaCard>();
				if (component != null)
				{
					this.m_CardManager.PlaceCardInCardLimbo(component);
				}
				else
				{
					j++;
				}
			}
		}
		int instanceList = AgricolaLib.GetInstanceList(13, this.m_LocalPlayerIndex, this.m_cardBuffer, 128);
		int k;
		for (k = 0; k < instanceList; k++)
		{
			int instanceID = this.m_cardList[k];
			GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(instanceID, true);
			if (!(cardFromInstanceID == null))
			{
				AnimateObject component2 = cardFromInstanceID.GetComponent<AnimateObject>();
				if (component2 != null)
				{
					cardFromInstanceID.SetActive(true);
					if (k < this.m_LocatorLocalPlayerHand.Length && this.m_LocatorLocalPlayerHand[k] != null)
					{
						this.m_LocatorLocalPlayerHand[k].gameObject.SetActive(true);
						this.m_LocatorLocalPlayerHand[k].PlaceAnimateObject(component2, true, true, false);
					}
				}
			}
		}
		while (k < this.m_LocatorLocalPlayerHand.Length)
		{
			if (this.m_LocatorLocalPlayerHand[k] != null)
			{
				this.m_LocatorLocalPlayerHand[k].gameObject.SetActive(false);
			}
			k++;
		}
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0001A78C File Offset: 0x0001898C
	private void RebuildAnimationManager()
	{
		if (this.m_AnimationManager == null)
		{
			return;
		}
		this.m_AnimationManager.RemoveAllAnimationLocators();
		this.m_AnimationManager.RebuildAnimationManager();
		if (this.m_LocatorResolvePosition != null)
		{
			this.m_AnimationManager.SetAnimationLocator(4, 0, this.m_LocatorResolvePosition);
		}
		if (this.m_BuildingManager != null)
		{
			this.m_BuildingManager.RebuildAnimationManager(this.m_AnimationManager);
		}
		if (this.m_LocalPlayerWorkerTray != null)
		{
			this.m_LocalPlayerWorkerTray.RebuildAnimationManager(this.m_AnimationManager);
		}
		if (this.m_UpperHudTokens != null)
		{
			this.m_UpperHudTokens.RebuildAnimationManager(this.m_AnimationManager);
		}
		if (this.m_CardInPlayManager != null)
		{
			this.m_CardInPlayManager.RebuildAnimationManager(this.m_AnimationManager);
		}
		if (this.m_AgricolaFarm != null)
		{
			this.m_AgricolaFarm.RegisterAnimationLocators(this.m_AnimationManager);
		}
		if (this.m_LocalPlayerData != null)
		{
			this.m_LocalPlayerData.RebuildAnimationManager(this.m_AnimationManager);
		}
		if (this.m_LowerHudExcessAnimalDisplay != null)
		{
			this.m_LowerHudExcessAnimalDisplay.Reset();
		}
		if (this.m_DraftInterface != null)
		{
			this.m_DraftInterface.RebuildAnimationManager(this.m_AnimationManager);
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00003022 File Offset: 0x00001222
	private void RebuildPopups()
	{
	}

	// Token: 0x040002B5 RID: 693
	private const int k_maxDataSize = 1024;

	// Token: 0x040002B6 RID: 694
	private const int k_maxCardCount = 128;

	// Token: 0x040002B7 RID: 695
	private const int k_maxResolveCardCount = 16;

	// Token: 0x040002B8 RID: 696
	[SerializeField]
	private AgricolaCardManager m_CardManager;

	// Token: 0x040002B9 RID: 697
	[SerializeField]
	private DragManager m_DragManager;

	// Token: 0x040002BA RID: 698
	[SerializeField]
	private AgricolaAnimationManager m_AnimationManager;

	// Token: 0x040002BB RID: 699
	[SerializeField]
	private AgricolaMagnifyManager m_MagnifyManager;

	// Token: 0x040002BC RID: 700
	[SerializeField]
	private AgricolaBuildingManager m_BuildingManager;

	// Token: 0x040002BD RID: 701
	[SerializeField]
	private AgricolaWorkerManager m_WorkerManager;

	// Token: 0x040002BE RID: 702
	[SerializeField]
	private AgricolaCardInPlayManager m_CardInPlayManager;

	// Token: 0x040002BF RID: 703
	[SerializeField]
	private PopupManager m_PopupManager;

	// Token: 0x040002C0 RID: 704
	[SerializeField]
	private AgricolaAudioHandlerIngame m_AudioHandler;

	// Token: 0x040002C1 RID: 705
	[SerializeField]
	private global::PlayerData m_LocalPlayerData;

	// Token: 0x040002C2 RID: 706
	[SerializeField]
	private DraftInterface m_DraftInterface;

	// Token: 0x040002C3 RID: 707
	[SerializeField]
	private AgricolaTownSeasonController m_AgricolaTownSeasonController;

	// Token: 0x040002C4 RID: 708
	[SerializeField]
	private AgricolaFarm m_AgricolaFarm;

	// Token: 0x040002C5 RID: 709
	[SerializeField]
	private TransformMap m_AgricolaFarmMapController;

	// Token: 0x040002C6 RID: 710
	[SerializeField]
	private TransformMap m_AgricolaTownMapController;

	// Token: 0x040002C7 RID: 711
	[SerializeField]
	private AgricolaWorkerTray m_LocalPlayerWorkerTray;

	// Token: 0x040002C8 RID: 712
	[SerializeField]
	private AgricolaAnimationLocator m_LocatorResolvePosition;

	// Token: 0x040002C9 RID: 713
	[SerializeField]
	private TrayToggle m_MajorImprovementTray;

	// Token: 0x040002CA RID: 714
	[SerializeField]
	private AgricolaAnimationLocator[] m_LocatorMajorImprovementTray;

	// Token: 0x040002CB RID: 715
	[SerializeField]
	private AgricolaAnimationLocator[] m_LocatorLocalPlayerHand;

	// Token: 0x040002CC RID: 716
	[SerializeField]
	private GameObject m_familyImprovementButton;

	// Token: 0x040002CD RID: 717
	[SerializeField]
	private GameObject[] m_fullImprovementButtons;

	// Token: 0x040002CE RID: 718
	[SerializeField]
	private Button m_improvementCloseButton;

	// Token: 0x040002CF RID: 719
	[SerializeField]
	private Button m_improvementFullMajorButton;

	// Token: 0x040002D0 RID: 720
	[SerializeField]
	private Button m_improvementFullMinorButton;

	// Token: 0x040002D1 RID: 721
	[SerializeField]
	private Text m_TextRoundNumber;

	// Token: 0x040002D2 RID: 722
	[SerializeField]
	private PlayerDisplay_UpperHud m_UpperHudTokens;

	// Token: 0x040002D3 RID: 723
	[SerializeField]
	private GameObject m_LowerHudOppResourceDisplay;

	// Token: 0x040002D4 RID: 724
	[SerializeField]
	private ExcessAnimalTray m_LowerHudExcessAnimalDisplay;

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	public GameObject m_OptionPrompt;

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	public Animator m_OptionPromptAnimator;

	// Token: 0x040002D7 RID: 727
	[SerializeField]
	public TextMeshProUGUI m_OptionPromptText;

	// Token: 0x040002D8 RID: 728
	private bool m_bShowingConfirmPrompt;

	// Token: 0x040002D9 RID: 729
	[SerializeField]
	private TextMeshProUGUI m_Popup_ConfirmAction_Text;

	// Token: 0x040002DA RID: 730
	[SerializeField]
	private GameObject m_Button_Commit;

	// Token: 0x040002DB RID: 731
	[SerializeField]
	private HudSideController_Right m_HudRight;

	// Token: 0x040002DC RID: 732
	[SerializeField]
	private HudSideController_Left m_HudLeft;

	// Token: 0x040002DD RID: 733
	[SerializeField]
	private Button m_Button_FarmToTown;

	// Token: 0x040002DE RID: 734
	[SerializeField]
	private Button m_Button_TownToFarm;

	// Token: 0x040002DF RID: 735
	[SerializeField]
	private GameObject m_Popup_Chat;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	private GameObject m_Button_Chat;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private GameObject m_Popup_Forfeit;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private Popup_TextLog m_Popup_TextLog;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	private Popup_EndGame m_Popup_EndGame;

	// Token: 0x040002E4 RID: 740
	[SerializeField]
	private HotseatOverlay m_HotseatScreen;

	// Token: 0x040002E5 RID: 741
	[SerializeField]
	private GameObject m_TutorialRoot;

	// Token: 0x040002E6 RID: 742
	[SerializeField]
	private GameObject m_TutorialPanel;

	// Token: 0x040002E7 RID: 743
	[SerializeField]
	private GameObject m_TutorialCalloutsRoot;

	// Token: 0x040002E8 RID: 744
	[SerializeField]
	private GameObject m_TutorialPanelBaseRoot;

	// Token: 0x040002E9 RID: 745
	[SerializeField]
	private TextMeshProUGUI m_TutorialPanelText_Label;

	// Token: 0x040002EA RID: 746
	[SerializeField]
	private TextMeshProUGUI m_TutorialPanelPrompt;

	// Token: 0x040002EB RID: 747
	[SerializeField]
	private GameObject m_TutorialPanelSplitRoot;

	// Token: 0x040002EC RID: 748
	[SerializeField]
	private TextMeshProUGUI m_TutorialPanelLabelSplitL;

	// Token: 0x040002ED RID: 749
	[SerializeField]
	private TextMeshProUGUI m_TutorialPanelLabelSplitR;

	// Token: 0x040002EE RID: 750
	[SerializeField]
	private TextMeshProUGUI m_TutorialPanelPromptSplit;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private GameObject m_TutorialPanelText_ButtonConfirm;

	// Token: 0x040002F0 RID: 752
	[SerializeField]
	private Button m_TutorialPanelButton;

	// Token: 0x040002F1 RID: 753
	[SerializeField]
	private GameObject m_TutorialPanelContinueButton;

	// Token: 0x040002F2 RID: 754
	[SerializeField]
	private GameObject m_TutorialPanelExitButton;

	// Token: 0x040002F3 RID: 755
	[SerializeField]
	private AnimationLocator m_TutorialMagnifyCardLocator;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	private GameObject m_TutorialMagnifyCardPanel;

	// Token: 0x040002F5 RID: 757
	[SerializeField]
	private GameObject[] m_TutorialInterfaceCallouts;

	// Token: 0x040002F6 RID: 758
	[SerializeField]
	private GameObject[] m_TutorialInterfaceCalloutPrefabs;

	// Token: 0x040002F7 RID: 759
	private uint m_OnlineGameID;

	// Token: 0x040002F8 RID: 760
	private int m_LocalPlayerIndex;

	// Token: 0x040002F9 RID: 761
	private int m_LocalPlayerInstanceID;

	// Token: 0x040002FA RID: 762
	private int m_LocalPlayerFactionIndex;

	// Token: 0x040002FB RID: 763
	private int m_RoundNumber;

	// Token: 0x040002FC RID: 764
	private byte[] m_dataBuffer;

	// Token: 0x040002FD RID: 765
	private GCHandle m_hDataBuffer;

	// Token: 0x040002FE RID: 766
	private IntPtr m_bufPtr;

	// Token: 0x040002FF RID: 767
	private int[] m_cardList;

	// Token: 0x04000300 RID: 768
	private GCHandle m_hCardInstanceBuffer;

	// Token: 0x04000301 RID: 769
	private IntPtr m_cardBuffer;

	// Token: 0x04000302 RID: 770
	private GameEventBuffer m_GameEventBuffer;

	// Token: 0x04000303 RID: 771
	private static ShortSaveStruct m_lastSavedState;

	// Token: 0x04000304 RID: 772
	private AgricolaOptionPopup.OptionPopupRestriction optionPopupRestrictions;

	// Token: 0x04000305 RID: 773
	private bool m_bPaused;

	// Token: 0x04000306 RID: 774
	private bool m_bInitialized;

	// Token: 0x04000307 RID: 775
	private bool m_bWaitingAfterTurn;

	// Token: 0x04000308 RID: 776
	private bool m_bEndGameWaitingForCommit;

	// Token: 0x04000309 RID: 777
	private bool m_bForfeitLastPlayer;

	// Token: 0x0400030A RID: 778
	private static uint m_LoadingScreenHoldUpdateCount;

	// Token: 0x0400030B RID: 779
	private float m_PauseSimulationTimer;

	// Token: 0x0400030C RID: 780
	private Tutorial m_Tutorial;

	// Token: 0x0400030D RID: 781
	private bool m_bAdvanceTutorial;

	// Token: 0x0400030E RID: 782
	private List<GameObject> m_TutorialElementsActive;

	// Token: 0x0400030F RID: 783
	private DateTime m_TutorialStepTimeStart = DateTime.Now;
}
