using System;
using AsmodeeNet.Analytics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012E RID: 302
public class UI_TutorialSelect : MonoBehaviour
{
	// Token: 0x06000BA2 RID: 2978 RVA: 0x000520D8 File Offset: 0x000502D8
	public void OnMenuEnter()
	{
		if (this.m_backgroundAnimator != null && this.m_backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("Intro"))
		{
			this.m_backgroundAnimator.Play("Reset_Tutorials");
		}
		uint tutorialDone = ProfileManager.instance.GetCurrentProfile().tutorialDone;
		for (int i = 0; i < this.m_buttons.Length; i++)
		{
			if (this.m_buttons[i].m_animator != null)
			{
				this.m_buttons[i].m_animator.Play(((long)i == (long)((ulong)this.m_currentButtonIndex)) ? this.openAnimation : this.closeAnimation);
			}
			if (this.m_buttons[i].m_completedTutorial != null)
			{
				this.m_buttons[i].m_completedTutorial.SetActive(((ulong)tutorialDone & (ulong)(1L << (UI_TutorialSelect.m_TutorialEntries[i].m_TutorialIndex & 31))) > 0UL);
			}
		}
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00003022 File Offset: 0x00001222
	public void OnMenuExit()
	{
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x000521D8 File Offset: 0x000503D8
	public void HandleToggleChange()
	{
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < this.m_buttons.Length; i++)
		{
			if (this.m_buttons[i].m_toggle != null && this.m_buttons[i].m_toggle.isOn)
			{
				num++;
				num2 = i;
			}
		}
		if (num == 1 && num2 > -1 && (long)num2 != (long)((ulong)this.m_currentButtonIndex))
		{
			if (this.m_buttons[(int)this.m_currentButtonIndex].m_animator != null)
			{
				this.m_buttons[(int)this.m_currentButtonIndex].m_animator.Play(this.closeAnimation);
			}
			if (this.m_buttons[num2].m_animator != null)
			{
				this.m_buttons[num2].m_animator.Play(this.openAnimation);
			}
			this.m_currentButtonIndex = (uint)num2;
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x000522C8 File Offset: 0x000504C8
	private void BuildTutorialActionList(TutorialStep[] tutorialSteps, ref int tutorialSetupStepCount, ref TutorialAIStep[] tutorialSetupSteps, ref int tutorialAIStepCount, ref TutorialAIStep[] tutorialAISteps, bool bConvertUserActions)
	{
		foreach (TutorialStep tutorialStep in tutorialSteps)
		{
			if (tutorialStep.m_StepType == TutorialStepType.OpponentAction)
			{
				tutorialAISteps[tutorialAIStepCount].selectionHint = tutorialStep.m_SelectionHint;
				tutorialAISteps[tutorialAIStepCount].selectionID = tutorialStep.m_SelectionID;
				tutorialSetupSteps[tutorialSetupStepCount].selectionInstanceType = 0;
				tutorialSetupSteps[tutorialSetupStepCount].selectionInstanceNameHash = 0U;
				tutorialAIStepCount++;
			}
			else if (tutorialStep.m_StepType == TutorialStepType.SetupAction)
			{
				if ((tutorialStep.m_StepFlags & 16777216U) == 0U || bConvertUserActions)
				{
					tutorialSetupSteps[tutorialSetupStepCount].selectionHint = tutorialStep.m_SelectionHint;
					tutorialSetupSteps[tutorialSetupStepCount].selectionID = tutorialStep.m_SelectionID;
					tutorialSetupSteps[tutorialSetupStepCount].selectionOptionalData = tutorialStep.m_SelectionOptionalData;
					tutorialSetupSteps[tutorialSetupStepCount].selectionInstanceType = tutorialStep.m_SelectionInstanceType;
					tutorialSetupSteps[tutorialSetupStepCount].selectionInstanceNameHash = AgricolaLib.GetChecksumString(tutorialStep.m_SelectionInstanceName);
					tutorialSetupStepCount++;
				}
			}
			else if (tutorialStep.m_StepType == TutorialStepType.UserAction && bConvertUserActions)
			{
				tutorialSetupSteps[tutorialSetupStepCount].selectionHint = tutorialStep.m_SelectionHint;
				tutorialSetupSteps[tutorialSetupStepCount].selectionID = tutorialStep.m_SelectionID;
				tutorialSetupSteps[tutorialSetupStepCount].selectionOptionalData = tutorialStep.m_SelectionOptionalData;
				tutorialSetupSteps[tutorialSetupStepCount].selectionInstanceType = tutorialStep.m_SelectionInstanceType;
				tutorialSetupSteps[tutorialSetupStepCount].selectionInstanceNameHash = AgricolaLib.GetChecksumString(tutorialStep.m_SelectionInstanceName);
				tutorialSetupStepCount++;
			}
			else if (tutorialStep.m_StepType == TutorialStepType.SetupScript)
			{
				int selectionID = (int)tutorialStep.m_SelectionID;
				for (int j = 0; j < UI_TutorialSelect.m_TutorialEntries.Length; j++)
				{
					TutorialEntry tutorialEntry = UI_TutorialSelect.m_TutorialEntries[j];
					if (tutorialEntry.m_TutorialIndex == selectionID)
					{
						this.BuildTutorialActionList(tutorialEntry.m_TutorialSteps, ref tutorialSetupStepCount, ref tutorialSetupSteps, ref tutorialAIStepCount, ref tutorialAISteps, true);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00003022 File Offset: 0x00001222
	public void HandleBackButton()
	{
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x000524AC File Offset: 0x000506AC
	public void StartTutorial()
	{
		int tutorialSlot = UIC_OfflineGameList.GetTutorialSlot();
		if (tutorialSlot < 0)
		{
			return;
		}
		ScreenManager.s_shortFilename = UIC_OfflineGameList.GetShortFileName(tutorialSlot);
		ScreenManager.s_fullFilename = UIC_OfflineGameList.GetFullFileName(tutorialSlot);
		TutorialEntry tutorialEntry = null;
		if (this.m_currentButtonIndex >= 0U && (ulong)this.m_currentButtonIndex < (ulong)((long)UI_TutorialSelect.m_TutorialEntries.Length))
		{
			tutorialEntry = UI_TutorialSelect.m_TutorialEntries[(int)this.m_currentButtonIndex];
		}
		Tutorial.m_CurrentTutorialSteps = tutorialEntry.m_TutorialSteps;
		Tutorial.s_CurrentTutorialIndex = tutorialEntry.m_TutorialIndex;
		TutorialStep[] currentTutorialSteps = Tutorial.m_CurrentTutorialSteps;
		string text = ThirdPartyManager.GenerateOfflineMatchID(tutorialSlot);
		PlayerPrefs.SetString("OfflineMatchID_" + tutorialSlot.ToString(), text);
		AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "tutorial", string.Empty, null, 1, 0, null, "create", null, string.Empty, false, true, null, null, null, null, null, null, null);
		GameParameters gameParameters = default(GameParameters);
		gameParameters.gameType = tutorialEntry.m_TutorialGameType;
		AppPlayerData[] array = new AppPlayerData[4];
		int numPlayers = 0;
		if (this.m_currentButtonIndex <= 4U)
		{
			numPlayers = 1;
			array[0].name = "Player";
			array[0].id = 1;
			array[0].userAvatar = 1;
			array[0].userRating = 0;
			array[0].playerParameters = default(PlayerParameters);
			array[0].playerParameters.avatar1 = 1;
			array[0].playerParameters.avatar2 = 2;
			array[0].playerParameters.avatar3 = 3;
			array[0].playerParameters.avatar4 = 4;
			array[0].playerParameters.avatar5 = 5;
			array[0].playerParameters.avatarColorIndex = 3;
			array[0].playerType = 0;
			array[0].aiDifficultyLevel = 0;
			array[0].networkPlayerState = 0;
			array[0].networkPlayerTimer = 0U;
		}
		else if (this.m_currentButtonIndex == 6U)
		{
			numPlayers = 3;
			array[2].name = "Player";
			array[2].id = 1;
			array[2].userAvatar = 1;
			array[2].userRating = 0;
			array[2].playerParameters = default(PlayerParameters);
			array[2].playerParameters.avatar1 = 1;
			array[2].playerParameters.avatar2 = 2;
			array[2].playerParameters.avatar3 = 3;
			array[2].playerParameters.avatar4 = 4;
			array[2].playerParameters.avatar5 = 5;
			array[2].playerParameters.avatarColorIndex = 1;
			array[2].playerType = 0;
			array[2].aiDifficultyLevel = 0;
			array[2].networkPlayerState = 0;
			array[2].networkPlayerTimer = 0U;
			array[0].name = "AI Player 1";
			array[0].id = 2;
			array[0].userAvatar = 2;
			array[0].userRating = 0;
			array[0].playerParameters = default(PlayerParameters);
			array[0].playerParameters.avatar1 = 1;
			array[0].playerParameters.avatar2 = 2;
			array[0].playerParameters.avatar3 = 3;
			array[0].playerParameters.avatar4 = 4;
			array[0].playerParameters.avatar5 = 5;
			array[0].playerParameters.avatarColorIndex = 5;
			array[0].playerType = 2;
			array[0].aiDifficultyLevel = 3;
			array[0].networkPlayerState = 0;
			array[0].networkPlayerTimer = 0U;
			array[1].name = "AI Player 2";
			array[1].id = 3;
			array[1].userAvatar = 3;
			array[1].userRating = 0;
			array[1].playerParameters = default(PlayerParameters);
			array[1].playerParameters.avatar1 = 1;
			array[1].playerParameters.avatar2 = 2;
			array[1].playerParameters.avatar3 = 3;
			array[1].playerParameters.avatar4 = 4;
			array[1].playerParameters.avatar5 = 5;
			array[1].playerParameters.avatarColorIndex = 2;
			array[1].playerType = 2;
			array[1].aiDifficultyLevel = 3;
			array[1].networkPlayerState = 0;
			array[1].networkPlayerTimer = 0U;
		}
		else if (this.m_currentButtonIndex == 7U)
		{
			numPlayers = 2;
			array[1].name = "Player";
			array[1].id = 1;
			array[1].userAvatar = 1;
			array[1].userRating = 0;
			array[1].playerParameters = default(PlayerParameters);
			array[1].playerParameters.avatar1 = 1;
			array[1].playerParameters.avatar2 = 2;
			array[1].playerParameters.avatar3 = 3;
			array[1].playerParameters.avatar4 = 4;
			array[1].playerParameters.avatar5 = 5;
			array[1].playerParameters.avatarColorIndex = 4;
			array[1].playerType = 0;
			array[1].aiDifficultyLevel = 0;
			array[1].networkPlayerState = 0;
			array[1].networkPlayerTimer = 0U;
			array[0].name = "AI Player 1";
			array[0].id = 2;
			array[0].userAvatar = 2;
			array[0].userRating = 0;
			array[0].playerParameters = default(PlayerParameters);
			array[0].playerParameters.avatar1 = 1;
			array[0].playerParameters.avatar2 = 2;
			array[0].playerParameters.avatar3 = 3;
			array[0].playerParameters.avatar4 = 4;
			array[0].playerParameters.avatar5 = 5;
			array[0].playerParameters.avatarColorIndex = 5;
			array[0].playerType = 2;
			array[0].aiDifficultyLevel = 3;
			array[0].networkPlayerState = 0;
			array[0].networkPlayerTimer = 0U;
		}
		uint randomSeed = 1U;
		int drawDeckOrderCount = 0;
		ushort[] array2 = new ushort[256];
		int randomDieResultCount = 0;
		byte[] array3 = new byte[256];
		foreach (TutorialStep tutorialStep in currentTutorialSteps)
		{
			if (tutorialStep.m_StepType == TutorialStepType.DrawCardFromDeck)
			{
				array2[drawDeckOrderCount++] = tutorialStep.m_SelectionID;
			}
			else if (tutorialStep.m_StepType == TutorialStepType.DieRollResult)
			{
				array3[randomDieResultCount++] = (byte)tutorialStep.m_SelectionHint;
				if (tutorialStep.m_SelectionID >= 1 && tutorialStep.m_SelectionID <= 6)
				{
					array3[randomDieResultCount++] = (byte)tutorialStep.m_SelectionID;
				}
			}
		}
		int tutorialAIStepCount = 0;
		TutorialAIStep[] tutorialAISteps = new TutorialAIStep[1024];
		int tutorialSetupStepCount = 0;
		TutorialAIStep[] tutorialSetupSteps = new TutorialAIStep[1024];
		this.BuildTutorialActionList(currentTutorialSteps, ref tutorialSetupStepCount, ref tutorialSetupSteps, ref tutorialAIStepCount, ref tutorialAISteps, false);
		AgricolaLib.StartTutorial(ref gameParameters, numPlayers, array, tutorialEntry.m_TutorialIndex, randomSeed, tutorialSetupStepCount, tutorialSetupSteps, tutorialAIStepCount, tutorialAISteps, drawDeckOrderCount, array2, randomDieResultCount, array3);
		int[] array4 = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			5,
			2,
			8
		};
		ScreenManager.instance.LoadIntoGameScreen(array4[(int)this.m_currentButtonIndex]);
	}

	// Token: 0x04000CA9 RID: 3241
	private static TutorialEntry[] m_TutorialEntries = new TutorialEntry[]
	{
		new TutorialEntry(1, 0, TutorialSteps1.m_TutorialSteps),
		new TutorialEntry(2, 0, TutorialSteps2.m_TutorialSteps),
		new TutorialEntry(3, 0, TutorialSteps3.m_TutorialSteps),
		new TutorialEntry(4, 0, TutorialSteps4.m_TutorialSteps),
		new TutorialEntry(7, 0, TutorialSteps7.m_TutorialSteps),
		new TutorialEntry(5, 0, TutorialSteps5.m_TutorialSteps),
		new TutorialEntry(6, 0, TutorialSteps6.m_TutorialSteps)
	};

	// Token: 0x04000CAA RID: 3242
	public Animator m_backgroundAnimator;

	// Token: 0x04000CAB RID: 3243
	public UI_TutorialSelect.TutorialButton[] m_buttons;

	// Token: 0x04000CAC RID: 3244
	public string openAnimation;

	// Token: 0x04000CAD RID: 3245
	public string closeAnimation;

	// Token: 0x04000CAE RID: 3246
	private uint m_currentButtonIndex;

	// Token: 0x0200080A RID: 2058
	[Serializable]
	public struct TutorialButton
	{
		// Token: 0x04002DF6 RID: 11766
		public Animator m_animator;

		// Token: 0x04002DF7 RID: 11767
		public Toggle m_toggle;

		// Token: 0x04002DF8 RID: 11768
		public GameObject m_completedTutorial;
	}
}
