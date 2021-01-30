using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using AsmodeeNet.Analytics;
using TMPro;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class Popup_EndGame : PopupBase
{
	// Token: 0x06000598 RID: 1432 RVA: 0x0002A51E File Offset: 0x0002871E
	public bool GetEndGameHasStarted()
	{
		return this.m_bEndGameHasStarted;
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0002A528 File Offset: 0x00028728
	public void StartEndGameSequence()
	{
		if (this.m_tutorialWorkerPanel != null)
		{
			this.m_tutorialWorkerPanel.SetActive(false);
		}
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		List<int> list = new List<int>();
		int num = -9999;
		int num2 = -9999;
		this.m_numPlayers = 0;
		this.m_bSoloMode = (AgricolaLib.GetGamePlayerCount() == 1 && !AgricolaLib.GetIsTutorialGame());
		this.m_soloModeRestartGameButton.SetActive(false);
		this.m_soloModeNextGameButton.SetActive(false);
		this.m_endGameButton.SetActive(false);
		this.m_farmButton.SetActive(false);
		this.m_scoreBreakdownButton.SetActive(false);
		this.m_rootWindow.SetActive(true);
		this.m_occSelectWindow.SetActive(false);
		if (this.m_bSoloMode)
		{
			int localPlayerIndex = AgricolaLib.GetLocalPlayerIndex();
			this.m_localPlayerSlotIndex = 0;
			this.m_playerSlots.Add(this.m_soloModeCell);
			this.m_playerSlots[this.m_numPlayers].playerIndex = localPlayerIndex;
			AgricolaLib.GetGamePlayerScoreState(this.m_playerSlots[this.m_numPlayers].playerIndex, this.m_bufPtr, 1024);
			this.m_playerSlots[this.m_numPlayers].scoreState = (GamePlayerScoreState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerScoreState));
			this.m_playerSlots[this.m_numPlayers].currentTotal = 0;
			this.m_playerSlots[this.m_numPlayers].totalScore.text = "0";
			this.m_playerSlots[this.m_numPlayers].bWinner = false;
			this.m_playerSlots[this.m_numPlayers].name.text = this.m_playerSlots[this.m_numPlayers].scoreState.displayName;
			if (this.m_playerSlots[this.m_numPlayers].colorizer != null)
			{
				this.m_playerSlots[this.m_numPlayers].colorizer.Colorize((uint)this.m_playerSlots[this.m_numPlayers].scoreState.playerFaction);
			}
			int num3 = 0;
			while (num3 < this.m_playerSlots[this.m_numPlayers].scoreCatScore.Length && num3 < this.m_playerSlots[this.m_numPlayers].scoreState.score.Length)
			{
				this.m_playerSlots[this.m_numPlayers].scoreCatScore[num3].text = string.Empty;
				this.m_playerSlots[this.m_numPlayers].scoreCatScore[num3].color = ((this.m_playerSlots[this.m_numPlayers].scoreState.score[num3] >= 0) ? this.m_positiveScoreColor : this.m_negativeScoreColor);
				num3++;
			}
			GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
			if ((long)this.m_playerSlots[this.m_numPlayers].scoreState.total_points >= (long)((ulong)AgricolaLib.GetSoloSeriesPointRequirement((uint)gameParameters.soloGameCount)))
			{
				this.m_playerSlots[this.m_numPlayers].bWinner = true;
			}
			this.m_soloModeCurrentRequiredScore.text = AgricolaLib.GetSoloSeriesPointRequirement((uint)gameParameters.soloGameCount).ToString();
			this.m_soloModeNextRequiredScore.text = AgricolaLib.GetSoloSeriesPointRequirement((uint)(gameParameters.soloGameCount + 1)).ToString();
			this.m_numPlayers++;
			for (int i = 0; i < this.m_selectedOccs.Length; i++)
			{
				DragTargetZone component = this.m_selectedOccs[i].GetComponent<DragTargetZone>();
				if (component != null)
				{
					component.AddOnDropCallback(new DragTargetZone.OnDropCallback(this.OnDropCallback));
				}
			}
		}
		else
		{
			for (int j = 0; j < 6; j++)
			{
				int localOpponentPlayerIndex = AgricolaLib.GetLocalOpponentPlayerIndex(j);
				if (localOpponentPlayerIndex == AgricolaLib.GetLocalPlayerIndex())
				{
					this.m_localPlayerSlotIndex = j;
				}
				if (localOpponentPlayerIndex != 0)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_playerSlotPrefab);
					this.m_playerSlots.Add(gameObject.GetComponent<Popup_EndGameCell>());
					gameObject.transform.SetParent(this.m_playerSlotLocator);
					gameObject.transform.localScale = Vector3.one;
					this.m_playerSlots[this.m_numPlayers].currentTotal = 0;
					this.m_playerSlots[this.m_numPlayers].totalScore.text = "0";
					this.m_playerSlots[this.m_numPlayers].playerIndex = localOpponentPlayerIndex;
					AgricolaLib.GetGamePlayerScoreState(this.m_playerSlots[this.m_numPlayers].playerIndex, this.m_bufPtr, 1024);
					this.m_playerSlots[this.m_numPlayers].scoreState = (GamePlayerScoreState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerScoreState));
					this.m_playerSlots[this.m_numPlayers].bWinner = false;
					this.m_playerSlots[this.m_numPlayers].name.text = this.m_playerSlots[this.m_numPlayers].scoreState.displayName;
					if (this.m_playerSlots[this.m_numPlayers].colorizer != null)
					{
						this.m_playerSlots[this.m_numPlayers].colorizer.Colorize((uint)this.m_playerSlots[this.m_numPlayers].scoreState.playerFaction);
					}
					int num4 = 0;
					while (num4 < this.m_playerSlots[this.m_numPlayers].scoreCatScore.Length && num4 < this.m_playerSlots[this.m_numPlayers].scoreState.score.Length)
					{
						this.m_playerSlots[this.m_numPlayers].scoreCatScore[num4].text = string.Empty;
						this.m_playerSlots[this.m_numPlayers].scoreCatScore[num4].color = ((this.m_playerSlots[this.m_numPlayers].scoreState.score[num4] >= 0) ? this.m_positiveScoreColor : this.m_negativeScoreColor);
						num4++;
					}
					this.m_playerSlots[this.m_numPlayers].scoreCatScore[14].text = string.Empty;
					this.m_playerSlots[this.m_numPlayers].scoreCatScore[14].color = this.m_positiveScoreColor;
					if (this.m_playerSlots[this.m_numPlayers].scoreState.total_points > num || (this.m_playerSlots[this.m_numPlayers].scoreState.total_points == num && this.m_playerSlots[this.m_numPlayers].scoreState.total_resources > num2))
					{
						list.Clear();
						list.Add(this.m_numPlayers);
						num2 = this.m_playerSlots[this.m_numPlayers].scoreState.total_resources;
						num = this.m_playerSlots[this.m_numPlayers].scoreState.total_points;
					}
					else if (this.m_playerSlots[this.m_numPlayers].scoreState.total_points == num && this.m_playerSlots[this.m_numPlayers].scoreState.total_resources == num2)
					{
						list.Add(this.m_numPlayers);
					}
					this.m_numPlayers++;
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				this.m_playerSlots[list[k]].bWinner = true;
			}
			this.m_bWinnersTied = (list.Count > 1);
		}
		this.m_normalModeWindow.SetActive(!this.m_bSoloMode);
		this.m_soloModeWindow.SetActive(this.m_bSoloMode);
		this.m_bEndGameHasStarted = true;
		string input_text = "${Key_Skip}";
		string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
		this.m_bottomButtonText.text = text;
		this.m_bIsFinalAnimPlaying = true;
		this.m_currentAnimStep = -1;
		if (AgricolaLib.GetIsTutorialGame())
		{
			this.m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
			this.m_tutorial = this.m_gameController.GetTutorial();
			this.m_bCurrentAnimStepComplete = true;
			this.m_rematchButton.SetActive(false);
		}
		else
		{
			this.SetupNextAnimationStep();
		}
		this.m_hDataBuffer.Free();
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0002ADD4 File Offset: 0x00028FD4
	private void SetupNextAnimationStep()
	{
		int num = this.m_currentAnimStep + 1;
		this.m_currentAnimStep = num;
		if (num > (this.m_bSoloMode ? 13 : 14))
		{
			base.StopAllCoroutines();
			if (this.m_bSoloMode)
			{
				this.m_lightBarAnimator_Solo.SetTrigger("ForceQuit");
			}
			else
			{
				this.m_lightBarAnimator.SetTrigger("ForceQuit");
			}
			this.ShowFinalScoring();
			return;
		}
		base.StartCoroutine(this.AnimationStep(this.m_currentAnimStep));
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0002AE4C File Offset: 0x0002904C
	private void Update()
	{
		if (AgricolaLib.GetIsTutorialGame() && this.m_tutorial != null)
		{
			if (this.m_bIsFinalAnimPlaying && this.m_bCurrentAnimStepComplete)
			{
				TutorialStep currentStep = this.m_tutorial.GetCurrentStep();
				if ((currentStep.m_StepFlags & 134217728U) != 0U)
				{
					this.m_gameController.AdvanceTutorial();
					return;
				}
				if ((currentStep.m_StepFlags & 67108864U) != 0U)
				{
					this.m_bCurrentAnimStepComplete = false;
					this.SetupNextAnimationStep();
					this.m_gameController.AdvanceTutorial();
					return;
				}
			}
		}
		else if (this.m_bIsFinalAnimPlaying && this.m_bCurrentAnimStepComplete)
		{
			this.m_bCurrentAnimStepComplete = false;
			this.SetupNextAnimationStep();
		}
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0002AEE4 File Offset: 0x000290E4
	public void ShowFinalScoring()
	{
		for (int i = 0; i < this.m_playerSlots.Count; i++)
		{
			if (this.m_playerSlots[i].playerIndex != 0)
			{
				int num = 0;
				while (num < this.m_playerSlots[i].scoreCatScore.Length && num < this.m_playerSlots[i].scoreState.score.Length)
				{
					this.m_playerSlots[i].scoreCatScore[num].text = this.m_playerSlots[i].scoreState.score[num].ToString();
					num++;
				}
				if (!this.m_bSoloMode)
				{
					this.m_playerSlots[i].scoreCatScore[14].text = this.m_playerSlots[i].scoreState.total_resources.ToString();
				}
				this.m_playerSlots[i].totalScore.text = this.m_playerSlots[i].scoreState.total_points.ToString();
				this.m_playerSlots[i].totalScore.color = (this.m_playerSlots[i].bWinner ? this.m_leaderFinalScoreColor : ((this.m_playerSlots[i].scoreState.total_points >= 0) ? this.m_positiveScoreColor : this.m_negativeScoreColor));
			}
		}
		this.m_topBanner.KeyText = "${Key_Skip}";
		this.m_rematchButton.SetActive(AgricolaLib.GetIsOnlineGame());
		string input_text = "${Key_Rematch}";
		string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
		this.m_bottomButtonText.text = text;
		this.m_bIsFinalAnimPlaying = false;
		this.m_soloModeRestartGameButton.SetActive(this.m_bSoloMode);
		this.m_soloModeNextGameButton.SetActive(this.m_bSoloMode && this.m_playerSlots[0].bWinner);
		this.m_endGameButton.SetActive(true);
		this.m_farmButton.SetActive(true);
		this.m_scoreBreakdownButton.SetActive(true);
		if (this.m_audio != null)
		{
			for (int j = 0; j < this.m_playerSlots.Count; j++)
			{
				if (this.m_playerSlots[j].playerIndex == AgricolaLib.GetLocalPlayerIndex())
				{
					this.m_audio.PlayEndGameMusic(this.m_playerSlots[j].bWinner);
				}
			}
		}
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0002B15C File Offset: 0x0002935C
	public void OnSoloRestartGamePressed()
	{
		if (ScreenManager.s_fullFilename != string.Empty)
		{
			File.Delete(ScreenManager.s_fullFilename);
		}
		ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
		GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
		uint gameRandomSeed = AgricolaLib.GetGameRandomSeed();
		AppPlayerData[] array = new AppPlayerData[2];
		array[0].name = currentProfile.name;
		array[0].id = 100;
		array[0].userRating = 0;
		array[0].playerParameters = default(PlayerParameters);
		array[0].playerParameters.avatarColorIndex = currentProfile.factionIndex;
		array[0].userAvatar = (ushort)(currentProfile.gameAvatar1 + 10 * currentProfile.factionIndex);
		array[0].playerType = 0;
		array[0].playerParameters.avatar1 = currentProfile.gameAvatar1 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar2 = currentProfile.gameAvatar2 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar3 = currentProfile.gameAvatar3 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar4 = currentProfile.gameAvatar4 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar5 = currentProfile.gameAvatar5 + 10 * currentProfile.factionIndex;
		array[0].aiDifficultyLevel = 0;
		array[0].networkPlayerState = 0;
		array[0].networkPlayerTimer = 0U;
		AgricolaGame.HandleLeaveGameAnalytics("restart", 0);
		AgricolaLib.ExitCurrentGame();
		string text = ThirdPartyManager.GenerateOfflineMatchID(100);
		PlayerPrefs.SetString("OfflineMatchID_100", text);
		string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameParameters.deckFlags);
		AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "solo_series", string.Empty, activatedDlc, 1, 0, null, "restart", null, string.Empty, false, false, null, null, null, null, null, null, null);
		AgricolaLib.StartGame(ref gameParameters, 1, array, gameRandomSeed);
		ScreenManager.instance.LoadIntoGameScreen(1);
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0002B3C8 File Offset: 0x000295C8
	public void OnSoloNextGamePressed()
	{
		GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
		bool flag = false;
		int num = 7;
		for (int i = 0; i < gameParameters.soloGameStartOccupations.Length; i++)
		{
			if (gameParameters.soloGameStartOccupations[i] == 0)
			{
				num = i;
				flag = true;
				break;
			}
		}
		int[] array = new int[32];
		GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr pInstanceIDs = gchandle.AddrOfPinnedObject();
		int instanceList = AgricolaLib.GetInstanceList(10, AgricolaLib.GetLocalPlayerIndex(), pInstanceIDs, 32);
		if (instanceList <= num)
		{
			flag = false;
		}
		if (!flag)
		{
			gchandle.Free();
			this.m_selectedOccCard = 0;
			this.StartNextSoloGame();
			return;
		}
		this.m_rootWindow.SetActive(false);
		this.m_occSelectWindow.SetActive(true);
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		AgricolaCardManager cardManager = component.GetCardManager();
		component.GetDragManager().m_DraggingCardsLayer = this.m_dragLayer;
		component.GetMagnifyManager().SetOverridePanelObject(this.m_magnifyPanel);
		component.GetMagnifyManager().SetUseOverrideLayer(true);
		component.GetMagnifyManager().SetOverrideLayerObject(this.m_animationLayer);
		component.GetMagnifyManager().AddOnMagnifyCallback(new MagnifyManager.MagnifyCallback(this.OnMagnifyCallback));
		int num2 = 0;
		for (int j = 0; j < gameParameters.soloGameStartOccupations.Length; j++)
		{
			if (gameParameters.soloGameStartOccupations[j] != 0)
			{
				GameObject gameObject = cardManager.CreateCardFromCompressedNumber((uint)gameParameters.soloGameStartOccupations[j], false, true);
				if (gameObject != null)
				{
					gameObject.SetActive(true);
					AnimateObject component2 = gameObject.GetComponent<AnimateObject>();
					if (component2 != null)
					{
						this.m_selectedOccs[num2++].PlaceAnimateObject(component2, true, true, false);
					}
				}
			}
		}
		DragTargetZone component3 = this.m_selectedOccs[num2].GetComponent<DragTargetZone>();
		if (component3 != null)
		{
			component3.SetDragSelectionHint(1, Color.green, 0);
		}
		num2 = 0;
		for (int k = 0; k < instanceList; k++)
		{
			int instanceID = array[k];
			GameObject gameObject2 = cardManager.CreateCardFromInstanceID(instanceID, false);
			if (gameObject2 != null)
			{
				AgricolaCard component4 = gameObject2.GetComponent<AgricolaCard>();
				bool flag2 = false;
				for (int l = 0; l < gameParameters.soloGameStartOccupations.Length; l++)
				{
					if ((uint)gameParameters.soloGameStartOccupations[l] == component4.GetCompressedDeckCardValue())
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					gameObject2.SetActive(true);
					this.m_selectableOccCards.Add(component4);
					AnimateObject component5 = gameObject2.GetComponent<AnimateObject>();
					if (component5 != null)
					{
						this.m_selectableOccs[num2++].PlaceAnimateObject(component5, true, true, false);
					}
					component4.SetSelectable(true, Color.green);
				}
				else
				{
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
		}
		gchandle.Free();
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0002B660 File Offset: 0x00029860
	private void OnMagnifyCallback(CardObject magnifyCard)
	{
		this.m_lastMagnifiedCard = null;
		AgricolaCard component = magnifyCard.GetComponent<AgricolaCard>();
		if (component != null)
		{
			for (int i = 0; i < this.m_selectableOccCards.Count; i++)
			{
				if (this.m_selectableOccCards[i] == component)
				{
					this.m_lastMagnifiedCard = this.m_selectableOccCards[i];
					break;
				}
			}
		}
		if (this.m_soloMagnifySelectableButton != null)
		{
			this.m_soloMagnifySelectableButton.SetActive(this.m_lastMagnifiedCard != null);
		}
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0002B6E7 File Offset: 0x000298E7
	public void OnNextSoloMagnifyButton()
	{
		if (this.m_lastMagnifiedCard != null)
		{
			this.m_selectedOccCard = (ushort)this.m_lastMagnifiedCard.GetCompressedDeckCardValue();
			this.StartNextSoloGame();
		}
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0002B710 File Offset: 0x00029910
	private void OnDropCallback(DragObject drag, ushort selectionHint)
	{
		if (selectionHint == 1 && drag != null)
		{
			AgricolaCard component = drag.GetComponent<AgricolaCard>();
			if (component != null)
			{
				this.m_selectedOccCard = (ushort)component.GetCompressedDeckCardValue();
				this.StartNextSoloGame();
			}
		}
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0002B750 File Offset: 0x00029950
	private void StartNextSoloGame()
	{
		if (ScreenManager.s_fullFilename != string.Empty)
		{
			File.Delete(ScreenManager.s_fullFilename);
		}
		this.HandleAchievements();
		GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
		ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
		int soloSeriesPointRequirement = (int)AgricolaLib.GetSoloSeriesPointRequirement((uint)gameParameters.soloGameCount);
		currentProfile.soloCurrentStreak = (uint)gameParameters.soloGameCount;
		currentProfile.soloGameScores[(int)(gameParameters.soloGameCount - 1)] = this.m_playerSlots[0].scoreState.total_points;
		if ((long)this.m_playerSlots[0].scoreState.total_points > (long)((ulong)currentProfile.soloTopScore))
		{
			currentProfile.soloTopScore = (uint)this.m_playerSlots[0].scoreState.total_points;
		}
		if ((uint)gameParameters.soloGameCount > currentProfile.soloTopStreak)
		{
			currentProfile.soloTopStreak = (uint)gameParameters.soloGameCount;
		}
		ProfileManager.instance.Save();
		gameParameters.soloGameCount += 1;
		gameParameters.soloGameStartFood = (ushort)Math.Max(0, (this.m_playerSlots[0].scoreState.total_points - soloSeriesPointRequirement) / 2);
		if (this.m_selectedOccCard != 0)
		{
			for (int i = 0; i < gameParameters.soloGameStartOccupations.Length; i++)
			{
				if (gameParameters.soloGameStartOccupations[i] == 0)
				{
					gameParameters.soloGameStartOccupations[i] = this.m_selectedOccCard;
					break;
				}
			}
		}
		AppPlayerData[] array = new AppPlayerData[2];
		array[0].name = currentProfile.name;
		array[0].id = 100;
		array[0].userRating = 0;
		array[0].playerParameters = default(PlayerParameters);
		array[0].playerParameters.avatarColorIndex = currentProfile.factionIndex;
		array[0].userAvatar = (ushort)(currentProfile.gameAvatar1 + 10 * currentProfile.factionIndex);
		array[0].playerType = 0;
		array[0].playerParameters.avatar1 = currentProfile.gameAvatar1 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar2 = currentProfile.gameAvatar2 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar3 = currentProfile.gameAvatar3 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar4 = currentProfile.gameAvatar4 + 10 * currentProfile.factionIndex;
		array[0].playerParameters.avatar5 = currentProfile.gameAvatar5 + 10 * currentProfile.factionIndex;
		array[0].aiDifficultyLevel = 0;
		array[0].networkPlayerState = 0;
		array[0].networkPlayerTimer = 0U;
		uint randomSeed = (uint)new System.Random().Next();
		AgricolaGame.HandleLeaveGameAnalytics("match_completed", 2);
		AgricolaLib.ExitCurrentGame();
		string text = ThirdPartyManager.GenerateOfflineMatchID(100);
		PlayerPrefs.SetString("OfflineMatchID_100", text);
		string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameParameters.deckFlags);
		AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "solo_series", string.Empty, activatedDlc, 1, 0, null, "previous_map", null, string.Empty, false, false, null, null, null, null, null, null, null);
		AgricolaLib.StartGame(ref gameParameters, 1, array, randomSeed);
		ScreenManager.instance.LoadIntoGameScreen(1);
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0002BAD4 File Offset: 0x00029CD4
	public void OnEndGamePressed()
	{
		this.HandleAchievements();
		if (AgricolaLib.GetIsOnlineGame())
		{
			AgricolaGame.HandleLeaveGameAnalytics("match_completed", this.m_playerSlots[this.m_localPlayerSlotIndex].bWinner ? (this.m_bWinnersTied ? 1 : 2) : 0);
			AgricolaLib.NetworkGameFinished();
			ScreenManager.s_onStartScreen = "OnlineLobby";
			AgricolaLib.ExitCurrentGame();
			ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
			return;
		}
		if (AgricolaLib.GetGamePlayerCount() > 1)
		{
			ProfileManager instance = ProfileManager.instance;
			bool flag = false;
			for (int i = 0; i < this.m_playerSlots.Count; i++)
			{
				ProfileManager.OfflineProfileEntry profile = instance.GetProfile(this.m_playerSlots[i].name.text);
				if (profile != null)
				{
					profile.completed += 1U;
					bool bWinner = this.m_playerSlots[i].bWinner;
					flag = (flag || bWinner);
					switch (this.m_numPlayers)
					{
					case 2:
						profile.wins_2p += (bWinner ? 1U : 0U);
						profile.losses_2p += (bWinner ? 0U : 1U);
						break;
					case 3:
						profile.wins_3p += (bWinner ? 1U : 0U);
						profile.losses_3p += (bWinner ? 0U : 1U);
						break;
					case 4:
						profile.wins_4p += (bWinner ? 1U : 0U);
						profile.losses_4p += (bWinner ? 0U : 1U);
						break;
					case 5:
						profile.wins_5p += (bWinner ? 1U : 0U);
						profile.losses_5p += (bWinner ? 0U : 1U);
						break;
					case 6:
						profile.wins_6p += (bWinner ? 1U : 0U);
						profile.losses_6p += (bWinner ? 0U : 1U);
						break;
					}
				}
			}
			instance.Save();
			if (ScreenManager.s_fullFilename != string.Empty && ScreenManager.s_shortFilename != string.Empty)
			{
				File.Delete(ScreenManager.s_fullFilename);
				File.Delete(ScreenManager.s_shortFilename);
			}
			AgricolaGame.HandleLeaveGameAnalytics("match_completed", flag ? (this.m_bWinnersTied ? 1 : 2) : 0);
			ScreenManager.s_onStartScreen = "OfflineLobby";
			AgricolaLib.ExitCurrentGame();
			ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
			return;
		}
		AgricolaGame.HandleLeaveGameAnalytics("quit", -1);
		ScreenManager.s_onStartScreen = "OfflineLobby";
		AgricolaLib.ExitCurrentGame();
		ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0002BD6C File Offset: 0x00029F6C
	public void OnRematchPressed()
	{
		if (this.m_bIsFinalAnimPlaying)
		{
			base.StopAllCoroutines();
			this.m_lightBarAnimator.SetTrigger("ForceQuit");
			this.ShowFinalScoring();
			return;
		}
		this.HandleAchievements();
		if (AgricolaLib.GetIsOnlineGame())
		{
			AgricolaGame.HandleLeaveGameAnalytics("match_completed", this.m_playerSlots[this.m_localPlayerSlotIndex].bWinner ? 2 : 0);
			AgricolaLib.NetworkRematchGame(AgricolaLib.GetCurrentGameID());
			ScreenManager.s_onStartScreen = "OnlineLobby";
			AgricolaLib.ExitCurrentGame();
			ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
		}
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0002BDFC File Offset: 0x00029FFC
	private void HandleAchievements()
	{
		bool flag = true;
		AchievementManagerWrapper instance = AchievementManagerWrapper.instance;
		if (AgricolaLib.GetIsHotseatGame())
		{
			flag = false;
		}
		else if (AgricolaLib.GetIsTutorialGame())
		{
			instance.ClearQueue();
			flag = false;
		}
		if (!flag)
		{
			return;
		}
		if (this.m_playerSlots[this.m_localPlayerSlotIndex].bWinner && this.m_numPlayers > 1)
		{
			instance.IncrementAchievement(EAchievements.Win1Game, 1L);
			instance.IncrementAchievement(EAchievements.Win10Games, 1L);
		}
		GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
		bool flag2 = gameParameters.gameType == 0 || gameParameters.gameType == 7;
		if (flag2 && this.m_playerSlots[this.m_localPlayerSlotIndex].scoreState.total_points >= 30)
		{
			instance.IncrementAchievement(EAchievements.Score30InFamily, 1L);
		}
		if (!flag2 && this.m_playerSlots[this.m_localPlayerSlotIndex].scoreState.total_points >= 50)
		{
			instance.IncrementAchievement(EAchievements.Score50InBasic, 1L);
		}
		if (this.m_bSoloMode)
		{
			if (this.m_playerSlots[this.m_localPlayerSlotIndex].scoreState.total_points >= 90)
			{
				instance.IncrementAchievement(EAchievements.Score90InSoloSeries, 1L);
			}
			if (this.m_playerSlots[this.m_localPlayerSlotIndex].bWinner)
			{
				if (gameParameters.soloGameCount == 1)
				{
					instance.IncrementAchievement(EAchievements.Win1stSoloSeries, 1L);
				}
				else if (gameParameters.soloGameCount == 8)
				{
					instance.IncrementAchievement(EAchievements.Win8thSoloSeries, 1L);
				}
			}
		}
		GCHandle gchandle = GCHandle.Alloc(new byte[32], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.GetGamePlayerEndgameAchievementState(this.m_playerSlots[this.m_localPlayerSlotIndex].playerIndex, intPtr, 32);
		GamePlayerEndgameAchievementState gamePlayerEndgameAchievementState = (GamePlayerEndgameAchievementState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerEndgameAchievementState));
		if (this.m_numPlayers == 2 && gamePlayerEndgameAchievementState.stoneHouseFiveRooms)
		{
			instance.IncrementAchievement(EAchievements.TwoPlayer5StoneRooms, 1L);
		}
		else if (this.m_numPlayers == 3 && this.m_playerSlots[this.m_localPlayerSlotIndex].scoreState.score[13] >= 10)
		{
			instance.IncrementAchievement(EAchievements.ThreePlayer10BonusPoints, 1L);
		}
		else if (this.m_numPlayers == 4 && gamePlayerEndgameAchievementState.sixOccupations)
		{
			instance.IncrementAchievement(EAchievements.FourPlayer6Occupations, 1L);
		}
		else if (this.m_numPlayers == 5 && gamePlayerEndgameAchievementState.fiveFamilyNoBegging)
		{
			instance.IncrementAchievement(EAchievements.FivePlayer5FamilyNoBegging, 1L);
		}
		gchandle.Free();
		instance.CommitQueue();
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0002C03C File Offset: 0x0002A23C
	private IEnumerator AnimationStep(int currentStep)
	{
		if (this.m_bSoloMode && currentStep == 0)
		{
			this.m_lightBarAnimator_Solo.SetTrigger("Step");
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		if (this.m_bSoloMode)
		{
			this.m_lightBarAnimator_Solo.SetTrigger("Step");
		}
		else
		{
			this.m_lightBarAnimator.SetTrigger("Step");
		}
		yield return new WaitForSeconds(this.m_animWaitTimeForBar);
		switch (currentStep)
		{
		case 0:
			this.m_topBanner.KeyText = "${Key_Scoring_Fields}";
			break;
		case 1:
			this.m_topBanner.KeyText = "${Key_Scoring_Pastures}";
			break;
		case 2:
			this.m_topBanner.KeyText = "${Key_Scoring_Grains}";
			break;
		case 3:
			this.m_topBanner.KeyText = "${Key_Scoring_Veggies}";
			break;
		case 4:
			this.m_topBanner.KeyText = "${Key_Scoring_Sheep}";
			break;
		case 5:
			this.m_topBanner.KeyText = "${Key_Scoring_Boars}";
			break;
		case 6:
			this.m_topBanner.KeyText = "${Key_Scoring_Cattle}";
			break;
		case 7:
			this.m_topBanner.KeyText = "${Key_Scoring_EmptySpace}";
			break;
		case 8:
			this.m_topBanner.KeyText = "${Key_Scoring_FencedStables}";
			break;
		case 9:
			this.m_topBanner.KeyText = "${Key_Scoring_HouseTiles}";
			break;
		case 10:
			this.m_topBanner.KeyText = "${Key_Scoring_Family}";
			break;
		case 11:
			this.m_topBanner.KeyText = "${Key_Scoring_Begging}";
			break;
		case 12:
			this.m_topBanner.KeyText = "${Key_Scoring_Improvements}";
			break;
		case 13:
			this.m_topBanner.KeyText = "${Key_Scoring_BonusPoints}";
			break;
		case 14:
			this.m_topBanner.KeyText = "${Key_Scoring_TotalResources}";
			break;
		default:
			Debug.LogError("Unexpected anim index on end game animation");
			yield break;
		}
		int[] desiredScores = new int[this.m_numPlayers];
		int num;
		for (int i = 0; i < this.m_numPlayers; i = num)
		{
			if (currentStep == 14)
			{
				this.m_playerSlots[i].scoreCatScore[currentStep].text = this.m_playerSlots[i].scoreState.total_resources.ToString();
				desiredScores[i] = this.m_playerSlots[i].currentTotal;
			}
			else
			{
				this.m_playerSlots[i].scoreCatScore[currentStep].text = this.m_playerSlots[i].scoreState.score[currentStep].ToString();
				desiredScores[i] = this.m_playerSlots[i].currentTotal + this.m_playerSlots[i].scoreState.score[currentStep];
			}
			yield return new WaitForSeconds(this.m_animRevealTime);
			num = i + 1;
		}
		bool bDone = false;
		while (!bDone)
		{
			int num2 = -9999;
			bDone = true;
			for (int j = 0; j < this.m_numPlayers; j++)
			{
				if (this.m_playerSlots[j].currentTotal != desiredScores[j])
				{
					bDone = false;
					this.m_playerSlots[j].currentTotal += ((this.m_playerSlots[j].currentTotal < desiredScores[j]) ? 1 : -1);
					this.m_playerSlots[j].totalScore.text = this.m_playerSlots[j].currentTotal.ToString();
				}
				if (this.m_playerSlots[j].currentTotal > num2)
				{
					num2 = this.m_playerSlots[j].currentTotal;
				}
			}
			for (int k = 0; k < this.m_numPlayers; k++)
			{
				if (this.m_playerSlots[k].currentTotal >= num2)
				{
					this.m_playerSlots[k].totalScore.color = this.m_leaderFinalScoreColor;
				}
				else
				{
					this.m_playerSlots[k].totalScore.color = ((this.m_playerSlots[k].currentTotal >= 0) ? this.m_positiveScoreColor : this.m_negativeScoreColor);
				}
			}
			yield return new WaitForSeconds(this.m_animTickTime);
		}
		yield return new WaitForSeconds(this.m_animBetweenStepWaitTime);
		this.m_bCurrentAnimStepComplete = true;
		yield break;
	}

	// Token: 0x0400052F RID: 1327
	private const int k_maxDataSize = 1024;

	// Token: 0x04000530 RID: 1328
	public GameObject m_playerSlotPrefab;

	// Token: 0x04000531 RID: 1329
	public Transform m_playerSlotLocator;

	// Token: 0x04000532 RID: 1330
	public GameObject m_dragLayer;

	// Token: 0x04000533 RID: 1331
	public GameObject m_animationLayer;

	// Token: 0x04000534 RID: 1332
	public float m_animWaitTimeForBar = 0.75f;

	// Token: 0x04000535 RID: 1333
	public float m_animRevealTime = 0.25f;

	// Token: 0x04000536 RID: 1334
	public float m_animTickTime = 0.016666668f;

	// Token: 0x04000537 RID: 1335
	public float m_animBetweenStepWaitTime = 0.75f;

	// Token: 0x04000538 RID: 1336
	public UILocalizedText m_topBanner;

	// Token: 0x04000539 RID: 1337
	public Animator m_lightBarAnimator;

	// Token: 0x0400053A RID: 1338
	public Animator m_lightBarAnimator_Solo;

	// Token: 0x0400053B RID: 1339
	public GameObject m_farmButton;

	// Token: 0x0400053C RID: 1340
	public GameObject m_scoreBreakdownButton;

	// Token: 0x0400053D RID: 1341
	public GameObject m_normalModeWindow;

	// Token: 0x0400053E RID: 1342
	public GameObject m_soloModeWindow;

	// Token: 0x0400053F RID: 1343
	public Popup_EndGameCell m_soloModeCell;

	// Token: 0x04000540 RID: 1344
	public GameObject m_soloModeRestartGameButton;

	// Token: 0x04000541 RID: 1345
	public GameObject m_soloModeNextGameButton;

	// Token: 0x04000542 RID: 1346
	public GameObject m_endGameButton;

	// Token: 0x04000543 RID: 1347
	public TextMeshProUGUI m_soloModeCurrentRequiredScore;

	// Token: 0x04000544 RID: 1348
	public TextMeshProUGUI m_soloModeNextRequiredScore;

	// Token: 0x04000545 RID: 1349
	public GameObject m_soloMagnifySelectableButton;

	// Token: 0x04000546 RID: 1350
	public GameObject m_rootWindow;

	// Token: 0x04000547 RID: 1351
	public GameObject m_occSelectWindow;

	// Token: 0x04000548 RID: 1352
	public AgricolaAnimationLocator[] m_selectableOccs;

	// Token: 0x04000549 RID: 1353
	public AgricolaAnimationLocator[] m_selectedOccs;

	// Token: 0x0400054A RID: 1354
	public GameObject m_magnifyPanel;

	// Token: 0x0400054B RID: 1355
	public GameObject m_tutorialWorkerPanel;

	// Token: 0x0400054C RID: 1356
	public AudioSettingsHandler m_audio;

	// Token: 0x0400054D RID: 1357
	public Color m_positiveScoreColor = Color.white;

	// Token: 0x0400054E RID: 1358
	public Color m_negativeScoreColor = Color.red;

	// Token: 0x0400054F RID: 1359
	public Color m_leaderFinalScoreColor = Color.yellow;

	// Token: 0x04000550 RID: 1360
	public TextMeshProUGUI m_bottomButtonText;

	// Token: 0x04000551 RID: 1361
	public GameObject m_rematchButton;

	// Token: 0x04000552 RID: 1362
	private List<Popup_EndGameCell> m_playerSlots = new List<Popup_EndGameCell>();

	// Token: 0x04000553 RID: 1363
	private List<AgricolaCard> m_selectableOccCards = new List<AgricolaCard>();

	// Token: 0x04000554 RID: 1364
	private AgricolaCard m_lastMagnifiedCard;

	// Token: 0x04000555 RID: 1365
	private int m_numPlayers;

	// Token: 0x04000556 RID: 1366
	private int m_currentAnimStep = -1;

	// Token: 0x04000557 RID: 1367
	private int m_localPlayerSlotIndex = -1;

	// Token: 0x04000558 RID: 1368
	private bool m_bWinnersTied;

	// Token: 0x04000559 RID: 1369
	private bool m_bCurrentAnimStepComplete;

	// Token: 0x0400055A RID: 1370
	private ushort m_selectedOccCard;

	// Token: 0x0400055B RID: 1371
	private bool m_bIsFinalAnimPlaying;

	// Token: 0x0400055C RID: 1372
	private bool m_bEndGameHasStarted;

	// Token: 0x0400055D RID: 1373
	private bool m_bSoloMode;

	// Token: 0x0400055E RID: 1374
	private AgricolaGame m_gameController;

	// Token: 0x0400055F RID: 1375
	private Tutorial m_tutorial;

	// Token: 0x04000560 RID: 1376
	private byte[] m_dataBuffer;

	// Token: 0x04000561 RID: 1377
	private GCHandle m_hDataBuffer;

	// Token: 0x04000562 RID: 1378
	private IntPtr m_bufPtr;
}
