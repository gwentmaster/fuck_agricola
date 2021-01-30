using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using AsmodeeNet.Analytics;
using TMPro;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class AgricolaPause : UI_OfflineLobby
{
	// Token: 0x06000429 RID: 1065 RVA: 0x00021284 File Offset: 0x0001F484
	private void Awake()
	{
		this.m_OpenParameterId = Animator.StringToHash("isHidden");
		UnityEngine.Object.Destroy(this.m_cardGalleryScreen.GetComponent<RegisterMenu>());
		UnityEngine.Object.Destroy(this.m_rulebookScreen.GetComponent<RegisterMenu>());
		UnityEngine.Object.Destroy(this.m_optionsScreen.GetComponent<RegisterMenu>());
		this.m_cardGalleryScreen.GetComponent<UI_FrontEndAndInGameScene>().SetIsInGame(true);
		this.m_rulebookScreen.GetComponent<UI_FrontEndAndInGameScene>().SetIsInGame(true);
		this.m_optionsScreen.GetComponent<UI_FrontEndAndInGameScene>().SetIsInGame(true);
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00021304 File Offset: 0x0001F504
	public override void OnEnterMenu()
	{
		base.StartCoroutine(this.TurnOnPause());
		GameObject[] array;
		if (AgricolaLib.GetIsOnlineGame())
		{
			NetworkPlayerProfileInfo networkPlayerProfileInfo;
			Network.GetLocalPlayerProfileInfo(out networkPlayerProfileInfo);
			this.m_profileName.text = networkPlayerProfileInfo.displayName;
			this.m_inProgressNumLabel.text = networkPlayerProfileInfo.inProgressGames.ToString();
			this.m_forfeitsNumLabel.text = networkPlayerProfileInfo.userGameStats.forfeits.ToString();
			this.m_completedGames.text = networkPlayerProfileInfo.userGameStats.completedGames.ToString();
			this.m_ratingNumLabel.text = networkPlayerProfileInfo.userGameStats.userRating.ToString();
			this.m_soloTopScore.text = string.Empty;
			this.m_2pRecord.text = string.Format("{0} - {1}", networkPlayerProfileInfo.userGameStats.wins[0], networkPlayerProfileInfo.userGameStats.losses[0]);
			this.m_3pRecord.text = string.Format("{0} - {1}", networkPlayerProfileInfo.userGameStats.wins[1], networkPlayerProfileInfo.userGameStats.losses[1]);
			this.m_4pRecord.text = string.Format("{0} - {1}", networkPlayerProfileInfo.userGameStats.wins[2], networkPlayerProfileInfo.userGameStats.losses[2]);
			this.m_5pRecord.text = string.Format("{0} - {1}", networkPlayerProfileInfo.userGameStats.wins[3], networkPlayerProfileInfo.userGameStats.losses[3]);
			this.m_6pRecord.text = string.Format("{0} - {1}", networkPlayerProfileInfo.userGameStats.wins[4], networkPlayerProfileInfo.userGameStats.losses[4]);
			array = this.m_onlineOnlyObjs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			array = this.m_offlineOnlyObjs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			this.m_soloModeObject.SetActive(false);
			return;
		}
		base.OnEnterMenu();
		array = this.m_onlineOnlyObjs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = this.m_offlineOnlyObjs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		if (AgricolaLib.GetGamePlayerCount() == 1 && !AgricolaLib.GetIsTutorialGame())
		{
			if (this.m_forfeitButtonKey != null && this.m_forfeitConfirmKey != null)
			{
				this.m_forfeitButtonKey.KeyText = "${Key_RestartGame}";
				this.m_forfeitConfirmKey.KeyText = "${Key_RetrySoloSeriesGame}";
			}
			this.m_soloModeObject.SetActive(true);
			GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			AgricolaLib.GetGamePlayerScoreState(AgricolaLib.GetLocalPlayerIndex(), intPtr, 1024);
			GamePlayerScoreState gamePlayerScoreState = (GamePlayerScoreState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerScoreState));
			this.m_soloModeCurrentScore.text = gamePlayerScoreState.total_points.ToString();
			this.m_soloModeScoreTarget.text = AgricolaLib.GetSoloSeriesPointRequirement((uint)gamePlayerScoreState.soloGameCount).ToString();
			gchandle.Free();
			return;
		}
		this.m_soloModeObject.SetActive(false);
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00021658 File Offset: 0x0001F858
	public void OnExitGamePressed()
	{
		AchievementManagerWrapper.instance.ClearQueue();
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		if (AgricolaLib.GetIsTutorialGame())
		{
			TimeSpan timeSpan = DateTime.Now - component.GetTutorialStepStartTime();
			Tutorial tutorial = component.GetTutorial();
			if (tutorial != null && !tutorial.IsCompleted())
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["tutorial_type"] = Tutorial.GetTutorialName(Tutorial.s_CurrentTutorialIndex);
				dictionary["tutorial_version"] = "1";
				int @int = PlayerPrefs.GetInt("CompletedTutorial", 0);
				AnalyticsEvents.LogTutorialStepEvent(tutorial.GenerateCurrentStepName(), (float)tutorial.GetCurrentUserActionStepNumber(), TUTORIAL_STEP.step_status.quit, (int)timeSpan.TotalSeconds, (@int & 1 << Tutorial.s_CurrentTutorialIndex) != 0, dictionary);
			}
		}
		AgricolaGame.HandleLeaveGameAnalytics("quit", -1);
		if (Network.m_Network != null && Network.m_Network.m_bConnectedToServer)
		{
			ScreenManager.s_onStartScreen = "OnlineLobby";
		}
		else
		{
			ScreenManager.s_onStartScreen = "OfflineLobby";
		}
		AgricolaLib.ExitCurrentGame();
		ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00021760 File Offset: 0x0001F960
	public void OnForfeitConfirmPressed()
	{
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		if (AgricolaLib.GetIsTutorialGame())
		{
			TimeSpan timeSpan = DateTime.Now - component.GetTutorialStepStartTime();
			Tutorial tutorial = component.GetTutorial();
			if (tutorial != null && !tutorial.IsCompleted())
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["tutorial_type"] = Tutorial.GetTutorialName(Tutorial.s_CurrentTutorialIndex);
				dictionary["tutorial_version"] = "1";
				int @int = PlayerPrefs.GetInt("CompletedTutorial", 0);
				AnalyticsEvents.LogTutorialStepEvent(tutorial.GenerateCurrentStepName(), (float)tutorial.GetCurrentUserActionStepNumber(), TUTORIAL_STEP.step_status.quit, (int)timeSpan.TotalSeconds, (@int & 1 << Tutorial.s_CurrentTutorialIndex) != 0, dictionary);
			}
		}
		AchievementManagerWrapper.instance.CommitQueue();
		if (Network.m_Network != null && Network.m_Network.m_bConnectedToServer)
		{
			ScreenManager.s_onStartScreen = "OnlineLobby";
			AgricolaGame.HandleLeaveGameAnalytics("resign", 0);
		}
		else
		{
			ScreenManager.s_onStartScreen = "OfflineLobby";
			if (AgricolaLib.GetGamePlayerCount() == 1 && !AgricolaLib.GetIsTutorialGame())
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
				AgricolaLib.ExitCurrentGame();
				string text = ThirdPartyManager.GenerateOfflineMatchID(100);
				PlayerPrefs.SetString("OfflineMatchID_100", text);
				string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameParameters.deckFlags);
				AgricolaGame.HandleLeaveGameAnalytics("restart", -1);
				AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "solo_series", string.Empty, activatedDlc, 1, 0, null, "restart", null, string.Empty, false, false, null, null, null, null, null, null, null);
				AgricolaLib.StartGame(ref gameParameters, 1, array, gameRandomSeed);
				ScreenManager.instance.LoadIntoGameScreen(1);
				return;
			}
			if (ScreenManager.s_shortFilename != string.Empty && ScreenManager.s_fullFilename != string.Empty)
			{
				File.Delete(ScreenManager.s_shortFilename);
				File.Delete(ScreenManager.s_fullFilename);
			}
			AgricolaGame.HandleLeaveGameAnalytics("resign", 0);
		}
		AgricolaLib.NetworkForfeitGame(AgricolaLib.GetCurrentGameID(), false);
		AgricolaLib.ExitCurrentGame();
		ScreenManager.instance.GoToFrontEndScreens(false, 0f, false);
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00021B50 File Offset: 0x0001FD50
	public void OnPauseButtonPressed()
	{
		if (this.m_currentScreen != 0)
		{
			if (this.m_currentScreen == 1)
			{
				this.m_cardGalleryAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffCardGallery());
			}
			else if (this.m_currentScreen == 2)
			{
				this.m_rulebookAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffRulebook());
			}
			else if (this.m_currentScreen == 3)
			{
				this.m_optionsAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffOptionsScreen());
			}
			if (this.m_pauseScreenAnimator != null)
			{
				this.m_pauseScreenAnimator.gameObject.SetActive(true);
			}
			base.StartCoroutine(this.TurnOnPause());
			this.m_currentScreen = 0;
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00021C18 File Offset: 0x0001FE18
	public void OnCardGalleryButtonPressed()
	{
		if (this.m_currentScreen != 1)
		{
			if (this.m_currentScreen == 0)
			{
				if (this.m_pauseScreenAnimator != null)
				{
					this.m_pauseScreenAnimator.SetBool(this.m_OpenParameterId, true);
					base.StartCoroutine(this.TurnOffPause());
				}
			}
			else if (this.m_currentScreen == 2)
			{
				this.m_rulebookAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffRulebook());
			}
			else if (this.m_currentScreen == 3)
			{
				this.m_optionsAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffOptionsScreen());
			}
			this.m_cardGalleryAnimator.gameObject.SetActive(true);
			base.StartCoroutine(this.TurnOnCardGallery());
			this.m_currentScreen = 1;
		}
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00021CE0 File Offset: 0x0001FEE0
	public void OnRulebookButtonPressed()
	{
		if (this.m_currentScreen != 2)
		{
			if (this.m_currentScreen == 0)
			{
				if (this.m_pauseScreenAnimator != null)
				{
					this.m_pauseScreenAnimator.SetBool(this.m_OpenParameterId, true);
					base.StartCoroutine(this.TurnOffPause());
				}
			}
			else if (this.m_currentScreen == 1)
			{
				this.m_cardGalleryAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffCardGallery());
			}
			else if (this.m_currentScreen == 3)
			{
				this.m_optionsAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffOptionsScreen());
			}
			this.m_rulebookAnimator.gameObject.SetActive(true);
			base.StartCoroutine(this.TurnOnRulebook());
			this.m_currentScreen = 2;
		}
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00021DA8 File Offset: 0x0001FFA8
	public void OnOptionsButtonPressed()
	{
		if (this.m_currentScreen != 3)
		{
			if (this.m_currentScreen == 0)
			{
				if (this.m_pauseScreenAnimator != null)
				{
					this.m_pauseScreenAnimator.SetBool(this.m_OpenParameterId, true);
					base.StartCoroutine(this.TurnOffPause());
				}
			}
			else if (this.m_currentScreen == 1)
			{
				this.m_cardGalleryAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffCardGallery());
			}
			else if (this.m_currentScreen == 2)
			{
				this.m_rulebookAnimator.SetBool(this.m_OpenParameterId, true);
				base.StartCoroutine(this.TurnOffRulebook());
			}
			this.m_optionsAnimator.gameObject.SetActive(true);
			base.StartCoroutine(this.TurnOnOptionsScreen());
			this.m_currentScreen = 3;
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00021E6F File Offset: 0x0002006F
	private IEnumerator TurnOnCardGallery()
	{
		yield return new WaitForEndOfFrame();
		this.m_cardGalleryAnimator.SetBool(this.m_OpenParameterId, false);
		this.m_cardGalleryScreen.OnMenuEnter();
		yield break;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00021E7E File Offset: 0x0002007E
	private IEnumerator TurnOffCardGallery()
	{
		yield return new WaitForSeconds(this.m_waitTimeToDisable);
		this.m_cardGalleryScreen.OnMenuExit(false);
		this.m_cardGalleryScreen.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00021E8D File Offset: 0x0002008D
	private IEnumerator TurnOnRulebook()
	{
		yield return new WaitForEndOfFrame();
		this.m_rulebookAnimator.SetBool(this.m_OpenParameterId, false);
		this.m_rulebookScreen.OnMenuEnter();
		yield break;
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00021E9C File Offset: 0x0002009C
	private IEnumerator TurnOffRulebook()
	{
		yield return new WaitForSeconds(this.m_waitTimeToDisable);
		this.m_rulebookScreen.OnMenuExit(false);
		this.m_rulebookScreen.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00021EAB File Offset: 0x000200AB
	private IEnumerator TurnOnOptionsScreen()
	{
		yield return new WaitForEndOfFrame();
		this.m_optionsAnimator.SetBool(this.m_OpenParameterId, false);
		this.m_optionsScreen.OnEnterMenu();
		yield break;
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00021EBA File Offset: 0x000200BA
	private IEnumerator TurnOffOptionsScreen()
	{
		yield return new WaitForSeconds(this.m_waitTimeToDisable);
		this.m_optionsScreen.OnExitMenu(false);
		this.m_optionsScreen.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00021EC9 File Offset: 0x000200C9
	private IEnumerator TurnOnPause()
	{
		yield return new WaitForEndOfFrame();
		if (this.m_pauseScreenAnimator != null)
		{
			this.m_pauseScreenAnimator.SetBool(this.m_OpenParameterId, false);
		}
		yield break;
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00021ED8 File Offset: 0x000200D8
	private IEnumerator TurnOffPause()
	{
		yield return new WaitForSeconds(this.m_waitTimeToDisable);
		if (this.m_pauseScreenAnimator != null)
		{
			this.m_pauseScreenAnimator.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x040003B9 RID: 953
	private const int k_maxDataSize = 1024;

	// Token: 0x040003BA RID: 954
	public GameObject[] m_onlineOnlyObjs;

	// Token: 0x040003BB RID: 955
	public GameObject[] m_offlineOnlyObjs;

	// Token: 0x040003BC RID: 956
	public GameObject m_soloModeObject;

	// Token: 0x040003BD RID: 957
	public TextMeshProUGUI m_soloModeCurrentScore;

	// Token: 0x040003BE RID: 958
	public TextMeshProUGUI m_soloModeScoreTarget;

	// Token: 0x040003BF RID: 959
	public TextMeshProUGUI m_forfeitsNumLabel;

	// Token: 0x040003C0 RID: 960
	public TextMeshProUGUI m_inProgressNumLabel;

	// Token: 0x040003C1 RID: 961
	public TextMeshProUGUI m_ratingNumLabel;

	// Token: 0x040003C2 RID: 962
	public UILocalizedText m_forfeitButtonKey;

	// Token: 0x040003C3 RID: 963
	public UILocalizedText m_forfeitConfirmKey;

	// Token: 0x040003C4 RID: 964
	public Animator m_cardGalleryAnimator;

	// Token: 0x040003C5 RID: 965
	public UI_CardGallery m_cardGalleryScreen;

	// Token: 0x040003C6 RID: 966
	public Animator m_rulebookAnimator;

	// Token: 0x040003C7 RID: 967
	public UI_Rulebook m_rulebookScreen;

	// Token: 0x040003C8 RID: 968
	public Animator m_optionsAnimator;

	// Token: 0x040003C9 RID: 969
	public UI_SettingsMenu m_optionsScreen;

	// Token: 0x040003CA RID: 970
	public Animator m_pauseScreenAnimator;

	// Token: 0x040003CB RID: 971
	public float m_waitTimeToDisable = 0.25f;

	// Token: 0x040003CC RID: 972
	private int m_OpenParameterId;

	// Token: 0x040003CD RID: 973
	private int m_currentScreen;
}
