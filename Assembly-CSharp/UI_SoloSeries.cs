using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using AsmodeeNet.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x0200012C RID: 300
public class UI_SoloSeries : MonoBehaviour
{
	// Token: 0x06000B95 RID: 2965 RVA: 0x00050B28 File Offset: 0x0004ED28
	public void OnEnterMenu()
	{
		if (this.m_bHandlePopup)
		{
			this.m_bHandlePopup = false;
			return;
		}
		if (this.m_graphDots.Count == 0)
		{
			for (int i = 0; i < 64; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_xPosPrefab);
				if (gameObject != null)
				{
					gameObject.transform.SetParent(this.m_xPosContainer.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					this.m_graphDots.Add(gameObject.GetComponent<UIP_SoloSeriesGraphX>());
				}
			}
		}
		for (int j = 0; j < this.m_cards.Count; j++)
		{
			UnityEngine.Object.Destroy(this.m_cards[j]);
		}
		this.m_cards.Clear();
		this.m_bIgnoreToggles = true;
		this.RetreiveSettings();
		this.m_bIgnoreToggles = false;
		this.OnGameTypeChanged();
		ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
		this.m_filePathShortGame = UIC_OfflineGameList.GetShortFileName_Solo(currentProfile.idNumber);
		this.m_filePathFullGame = UIC_OfflineGameList.GetFullFileName_Solo(currentProfile.idNumber);
		this.m_playerName.text = currentProfile.name;
		if (File.Exists(this.m_filePathFullGame) && File.Exists(this.m_filePathShortGame))
		{
			this.m_bNeedToCreateGame = false;
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(this.m_filePathShortGame, FileMode.Open);
			try
			{
				this.m_saveData = (ShortSaveStruct)binaryFormatter.Deserialize(fileStream);
			}
			catch (SerializationException)
			{
				fileStream.Close();
				Debug.LogError("Unable to get save data with filename: " + this.m_filePathShortGame);
				return;
			}
			fileStream.Close();
			this.m_noGamesLabel.SetActive(this.m_saveData.soloGameCount <= 1);
			this.m_graphObj.SetActive(this.m_saveData.soloGameCount > 1);
			this.m_scoreObj.SetActive(true);
			this.m_currentScore.text = this.m_saveData.soloGameCurrentScore.ToString();
			this.m_currentRequired.text = AgricolaLib.GetSoloSeriesPointRequirement((uint)this.m_saveData.soloGameCount).ToString();
			this.m_restartGameButton.SetActive(true);
			this.m_restartSeriesButton.SetActive(true);
			this.m_deckSelectLabel.SetActive(false);
			this.m_deckSelect.SetActive(false);
			this.m_m_saveDataSize = this.m_saveData.savedDataSize;
			this.m_worldDataVersion = this.m_saveData.worldDataVersion;
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Txt_Game}");
			string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Stage}");
			string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}");
			if (this.m_saveData.roundNumber == 0)
			{
				this.m_saveData.roundNumber = 1;
			}
			int num = 1;
			if (this.m_saveData.roundNumber >= 5 && this.m_saveData.roundNumber <= 7)
			{
				num = 2;
			}
			else if (this.m_saveData.roundNumber >= 8 && this.m_saveData.roundNumber <= 9)
			{
				num = 3;
			}
			else if (this.m_saveData.roundNumber >= 10 && this.m_saveData.roundNumber <= 11)
			{
				num = 4;
			}
			else if (this.m_saveData.roundNumber >= 12 && this.m_saveData.roundNumber <= 13)
			{
				num = 5;
			}
			else if (this.m_saveData.roundNumber == 14)
			{
				num = 6;
			}
			object[] args = new object[]
			{
				text,
				this.m_saveData.soloGameCount,
				text2,
				num,
				text3,
				this.m_saveData.roundNumber
			};
			this.m_gameState.text = string.Format("{0} {1}, {2} {3}, {4} {5}", args);
			if (this.m_saveData.soloGameCount > 1)
			{
				if (this.m_cardManager != null && this.m_cardLocators.Length >= this.m_saveData.soloGameStartOccupations.Length)
				{
					for (int k = 0; k < this.m_saveData.soloGameStartOccupations.Length; k++)
					{
						if (this.m_saveData.soloGameStartOccupations[k] != 0)
						{
							GameObject gameObject2 = this.m_cardManager.CreateCardFromCompressedNumber((uint)this.m_saveData.soloGameStartOccupations[k], false, true);
							if (gameObject2 != null)
							{
								gameObject2.SetActive(true);
								gameObject2.GetComponent<AgricolaCard>().ShowHalfCard(-1f);
								gameObject2.transform.SetParent(this.m_cardLocators[k].transform);
								gameObject2.transform.localPosition = Vector3.zero;
								gameObject2.transform.localScale = new Vector3(this.m_cardScale, this.m_cardScale, 1f);
								this.m_cards.Add(gameObject2);
							}
						}
					}
				}
				int soloSeriesPointRequirement = (int)AgricolaLib.GetSoloSeriesPointRequirement((uint)this.m_saveData.soloGameCount);
				int num2 = (soloSeriesPointRequirement > this.m_saveData.soloGameCurrentScore) ? soloSeriesPointRequirement : this.m_saveData.soloGameCurrentScore;
				int num3 = 0;
				while (num3 < currentProfile.soloGameScores.Length && num3 < (int)(this.m_saveData.soloGameCount - 1))
				{
					if (currentProfile.soloGameScores[num3] > num2)
					{
						num2 = currentProfile.soloGameScores[num3];
					}
					num3++;
				}
				float num4 = 45f;
				float num5 = (num2 > 90) ? 15f : ((num2 > 70) ? 10f : 5f);
				float num6 = num4 + num5;
				float num7 = num6 + (float)this.m_verticalLabels.Length * num5 - num4;
				ushort num8 = (this.m_saveData.soloGameCount <= 8) ? 8 : this.m_saveData.soloGameCount;
				float num9 = (float)num8;
				bool flag = (int)num8 <= this.m_maxGameLabels;
				bool flag2 = false;
				for (int l = 0; l < this.m_verticalLabels.Length; l++)
				{
					this.m_verticalLabels[l].text = ((int)(num6 + (float)l * num5)).ToString();
				}
				Vector2[] array = new Vector2[(int)(this.m_saveData.soloGameCount + 1)];
				array[0] = Vector2.zero;
				int m;
				for (m = 0; m < (int)this.m_saveData.soloGameCount; m++)
				{
					this.m_graphDots[m].SetActive(true);
					this.m_graphDots[m].SetDotText(((m == (int)(this.m_saveData.soloGameCount - 1)) ? this.m_saveData.soloGameCurrentScore : currentProfile.soloGameScores[m]).ToString());
					this.m_graphDots[m].ShowDot(flag || m == (int)(this.m_saveData.soloGameCount - 1));
					this.m_graphDots[m].SetIsCurrentGame(m == (int)(this.m_saveData.soloGameCount - 1));
					if (flag || flag2 || m == (int)(this.m_saveData.soloGameCount - 1))
					{
						this.m_graphDots[m].SetLabelText((m + 1).ToString());
					}
					else
					{
						this.m_graphDots[m].SetLabelText(string.Empty);
					}
					float x = (float)(m + 1) / num9;
					float num10 = Mathf.Clamp01(((float)currentProfile.soloGameScores[m] - num4) / num7);
					this.m_graphDots[m].Reposition(num10);
					array[m + 1] = new Vector2(x, num10);
					flag2 = !flag2;
				}
				while (m < this.m_graphDots.Count)
				{
					this.m_graphDots[m].SetActive(m < 8);
					this.m_graphDots[m].ShowDot(false);
					this.m_graphDots[m].SetLabelText((m < 8) ? (m + 1).ToString() : string.Empty);
					this.m_graphDots[m].SetIsCurrentGame(false);
					m++;
				}
				this.m_lineRenderer.Points = array;
				return;
			}
		}
		else
		{
			this.m_bNeedToCreateGame = true;
			this.m_noGamesLabel.SetActive(false);
			this.m_graphObj.SetActive(false);
			this.m_scoreObj.SetActive(false);
			this.m_restartGameButton.SetActive(false);
			this.m_restartSeriesButton.SetActive(false);
			this.m_deckSelectLabel.SetActive(false);
			this.m_deckSelect.SetActive(false);
			string text4 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Txt_Game}");
			string text5 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Stage}");
			string text6 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}");
			object[] args2 = new object[]
			{
				text4,
				1,
				text5,
				1,
				text6,
				1
			};
			this.m_gameState.text = string.Format("{0} {1}, {2} {3}, {4} {5}", args2);
		}
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x000513FC File Offset: 0x0004F5FC
	public void OnExitMenu(bool bUnderPopup)
	{
		if (this.m_bHandlePopup || bUnderPopup)
		{
			return;
		}
		for (int i = 0; i < this.m_cards.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_cards[i]);
		}
		this.m_cards.Clear();
		this.StoreSettings();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x0005144C File Offset: 0x0004F64C
	public void OnGameTypeChanged()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		this.m_setFlags = 0U;
		int num = 12;
		for (int i = 0; i < this.m_deckToggles.Length; i++)
		{
			if (this.m_deckToggles[i].interactable && this.m_deckToggles[i].isOn)
			{
				this.m_setFlags |= 1U << i + num;
			}
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x000514B4 File Offset: 0x0004F6B4
	public void OnStartGamePressed()
	{
		if (this.m_bNeedToCreateGame)
		{
			GameObject scene = ScreenManager.instance.GetScene(this.m_AvatarSelectionScreenName);
			if (scene != null)
			{
				this.m_bHandlePopup = true;
				scene.GetComponent<UI_AvatarSelect>().SetProfile(ProfileManager.instance.GetCurrentProfile(), new UI_AvatarSelect.AvatarCallback(this.HandleBackFromAvatarSelect), false, true, false);
				ScreenManager.instance.PushScene(this.m_AvatarSelectionScreenName);
				return;
			}
		}
		else
		{
			byte[] array = File.ReadAllBytes(this.m_filePathFullGame);
			if (array.Length == this.m_m_saveDataSize)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
				string text = PlayerPrefs.GetString("OfflineMatchID_" + (100L + currentProfile.idNumber).ToString(), string.Empty);
				if (text == string.Empty)
				{
					text = ThirdPartyManager.GenerateOfflineMatchID((int)(100L + currentProfile.idNumber));
					PlayerPrefs.SetString("OfflineMatchID_" + (100L + currentProfile.idNumber).ToString(), text);
				}
				string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)this.m_saveData.deckFlags);
				AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "solo_series", string.Empty, activatedDlc, 1, 0, null, "resume", null, string.Empty, false, false, null, null, null, null, null, null, null);
				AgricolaLib.ResumeGame(intPtr, array.Length, this.m_worldDataVersion);
				Marshal.FreeHGlobal(intPtr);
			}
			ScreenManager.s_shortFilename = this.m_filePathShortGame;
			ScreenManager.s_fullFilename = this.m_filePathFullGame;
			ScreenManager.instance.LoadIntoGameScreen(1);
		}
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00051688 File Offset: 0x0004F888
	private void HandleBackFromAvatarSelect(ProfileManager.OfflineProfileEntry profile, bool bConfirm)
	{
		if (bConfirm)
		{
			ProfileManager.OfflineProfileEntry currentProfileRef = ProfileManager.instance.GetCurrentProfileRef();
			currentProfileRef.factionIndex = profile.factionIndex;
			currentProfileRef.gameAvatar1 = profile.gameAvatar1;
			currentProfileRef.gameAvatar2 = profile.gameAvatar2;
			currentProfileRef.gameAvatar3 = profile.gameAvatar3;
			currentProfileRef.gameAvatar4 = profile.gameAvatar4;
			currentProfileRef.gameAvatar5 = profile.gameAvatar5;
			ProfileManager.instance.Save();
			GameParameters gameParameters = default(GameParameters);
			gameParameters.gameType = 4;
			gameParameters.deckFlags = 16384;
			gameParameters.soloGameStartFood = 0;
			gameParameters.soloGameStartOccupations = new ushort[7];
			for (int i = 0; i < 7; i++)
			{
				gameParameters.soloGameStartOccupations[i] = 0;
			}
			gameParameters.soloGameCount = 1;
			gameParameters.deckFlags = 16384;
			AppPlayerData[] array = new AppPlayerData[2];
			array[0].name = currentProfileRef.name;
			array[0].id = 100;
			array[0].userRating = 0;
			array[0].playerParameters = default(PlayerParameters);
			array[0].playerParameters.avatarColorIndex = currentProfileRef.factionIndex;
			array[0].userAvatar = (ushort)(currentProfileRef.gameAvatar1 + 10 * currentProfileRef.factionIndex);
			array[0].playerType = 0;
			array[0].playerParameters.avatar1 = currentProfileRef.gameAvatar1 + 10 * currentProfileRef.factionIndex;
			array[0].playerParameters.avatar2 = currentProfileRef.gameAvatar2 + 10 * currentProfileRef.factionIndex;
			array[0].playerParameters.avatar3 = currentProfileRef.gameAvatar3 + 10 * currentProfileRef.factionIndex;
			array[0].playerParameters.avatar4 = currentProfileRef.gameAvatar4 + 10 * currentProfileRef.factionIndex;
			array[0].playerParameters.avatar5 = currentProfileRef.gameAvatar5 + 10 * currentProfileRef.factionIndex;
			array[0].aiDifficultyLevel = 0;
			array[0].networkPlayerState = 0;
			array[0].networkPlayerTimer = 0U;
			System.Random random = new System.Random();
			ScreenManager.s_shortFilename = this.m_filePathShortGame;
			ScreenManager.s_fullFilename = this.m_filePathFullGame;
			uint randomSeed = (uint)random.Next();
			ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
			string text = ThirdPartyManager.GenerateOfflineMatchID((int)(100L + currentProfile.idNumber));
			PlayerPrefs.SetString("OfflineMatchID_" + (100L + currentProfile.idNumber).ToString(), text);
			string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameParameters.deckFlags);
			AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "solo_series", string.Empty, activatedDlc, 1, 0, null, "create", null, string.Empty, false, false, null, null, null, null, null, null, null);
			AgricolaLib.StartGame(ref gameParameters, 1, array, randomSeed);
			ScreenManager.instance.LoadIntoGameScreen(1);
		}
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x000519B0 File Offset: 0x0004FBB0
	public void OnResetSeriesPressed()
	{
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bHandlePopup = true;
				component.Setup(new UI_ConfirmPopup.ClickCallback(this.ResetSeries), "${Key_ForfeitSoloSeries}", UI_ConfirmPopup.ButtonFormat.TwoButtons);
				ScreenManager.instance.PushScene("ConfirmPopup");
			}
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00051A14 File Offset: 0x0004FC14
	public void OnResetGamePressed()
	{
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bHandlePopup = true;
				component.Setup(new UI_ConfirmPopup.ClickCallback(this.ResetGame), "${Key_RetrySoloSeriesGame}", UI_ConfirmPopup.ButtonFormat.TwoButtons);
				ScreenManager.instance.PushScene("ConfirmPopup");
			}
		}
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x00051A78 File Offset: 0x0004FC78
	private void ResetGame(bool bConfirm)
	{
		if (this.m_filePathFullGame != string.Empty)
		{
			File.Delete(this.m_filePathFullGame);
		}
		ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
		GameParameters gameParameters = default(GameParameters);
		gameParameters.gameType = this.m_saveData.gameType;
		gameParameters.deckFlags = this.m_saveData.deckFlags;
		gameParameters.soloGameStartFood = this.m_saveData.soloGameStartFood;
		gameParameters.soloGameStartOccupations = new ushort[7];
		for (int i = 0; i < 7; i++)
		{
			gameParameters.soloGameStartOccupations[i] = this.m_saveData.soloGameStartOccupations[i];
		}
		gameParameters.soloGameCount = this.m_saveData.soloGameCount;
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
		ScreenManager.s_shortFilename = this.m_filePathShortGame;
		ScreenManager.s_fullFilename = this.m_filePathFullGame;
		ProfileManager.OfflineProfileEntry currentProfile2 = ProfileManager.instance.GetCurrentProfile();
		string text = ThirdPartyManager.GenerateOfflineMatchID((int)(100L + currentProfile2.idNumber));
		PlayerPrefs.SetString("OfflineMatchID_" + (100L + currentProfile2.idNumber).ToString(), text);
		string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)gameParameters.deckFlags);
		AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "solo_series", string.Empty, activatedDlc, 1, 0, null, "resume", null, string.Empty, false, false, null, null, null, null, null, null, null);
		AgricolaLib.StartGame(ref gameParameters, 1, array, this.m_saveData.soloGameRandomSeed);
		AnalyticsEvents.LogMatchStopEvent(1, 0, "restart", null, null, null);
		AgricolaLib.ExitCurrentGame();
		ScreenManager.instance.PopScene();
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00051DA8 File Offset: 0x0004FFA8
	private void ResetSeries(bool bConfirm)
	{
		if (bConfirm)
		{
			ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
			string text = PlayerPrefs.GetString("OfflineMatchID_" + (100L + currentProfile.idNumber).ToString(), string.Empty);
			if (text == string.Empty)
			{
				text = ThirdPartyManager.GenerateOfflineMatchID((int)(100L + currentProfile.idNumber));
				PlayerPrefs.SetString("OfflineMatchID_" + (100L + currentProfile.idNumber).ToString(), text);
			}
			string[] activatedDlc = ThirdPartyManager.GenerateDlcString((uint)this.m_saveData.deckFlags);
			AnalyticsEvents.LogMatchStartEvent(text, string.Empty, "solo_series", string.Empty, activatedDlc, 1, 0, null, "resume", null, string.Empty, false, false, null, null, null, null, null, null, null);
			AnalyticsEvents.LogMatchStopEvent(1, 0, "resign", new MATCH_STOP.player_result?(MATCH_STOP.player_result.defeat), null, null);
			if (this.m_filePathFullGame != string.Empty)
			{
				File.Delete(this.m_filePathFullGame);
			}
			if (this.m_filePathShortGame != string.Empty)
			{
				File.Delete(this.m_filePathShortGame);
			}
			ProfileManager.OfflineProfileEntry currentProfileRef = ProfileManager.instance.GetCurrentProfileRef();
			currentProfileRef.soloCurrentStreak = 0U;
			for (int i = 0; i < currentProfileRef.soloGameScores.Length; i++)
			{
				currentProfileRef.soloGameScores[i] = 0;
			}
			ProfileManager.instance.Save();
			ScreenManager.instance.PopScene();
		}
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00051F54 File Offset: 0x00050154
	private void RetreiveSettings()
	{
		int @int = PlayerPrefs.GetInt("OfflineSolo_DeckA", 1);
		this.m_deckToggles[0].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineSolo_DeckB", 1);
		this.m_deckToggles[1].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineSolo_DeckC", 0);
		this.m_deckToggles[2].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OfflineSolo_DeckD", 0);
		this.m_deckToggles[3].isOn = (@int == 1);
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x00051FD8 File Offset: 0x000501D8
	private void StoreSettings()
	{
		PlayerPrefs.SetInt("OfflineSolo_DeckA", this.m_deckToggles[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineSolo_DeckB", this.m_deckToggles[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineSolo_DeckC", this.m_deckToggles[2].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OfflineSolo_DeckD", this.m_deckToggles[3].isOn ? 1 : 0);
	}

	// Token: 0x04000C86 RID: 3206
	public AgricolaCardManager m_cardManager;

	// Token: 0x04000C87 RID: 3207
	public GameObject[] m_cardLocators;

	// Token: 0x04000C88 RID: 3208
	public float m_cardScale = 0.75f;

	// Token: 0x04000C89 RID: 3209
	public TextMeshProUGUI m_playerName;

	// Token: 0x04000C8A RID: 3210
	public TextMeshProUGUI m_gameState;

	// Token: 0x04000C8B RID: 3211
	[Header("No games variables")]
	public GameObject m_deckSelectLabel;

	// Token: 0x04000C8C RID: 3212
	public GameObject m_deckSelect;

	// Token: 0x04000C8D RID: 3213
	public Toggle[] m_deckToggles;

	// Token: 0x04000C8E RID: 3214
	public string m_AvatarSelectionScreenName = "FamilySelection";

	// Token: 0x04000C8F RID: 3215
	[Header("Graph variables")]
	public GameObject m_xPosPrefab;

	// Token: 0x04000C90 RID: 3216
	public GameObject m_xPosContainer;

	// Token: 0x04000C91 RID: 3217
	public GameObject m_graphObj;

	// Token: 0x04000C92 RID: 3218
	public UILineRenderer m_lineRenderer;

	// Token: 0x04000C93 RID: 3219
	public TextMeshProUGUI[] m_verticalLabels;

	// Token: 0x04000C94 RID: 3220
	public int m_maxGameLabels = 24;

	// Token: 0x04000C95 RID: 3221
	[Header("Other variables")]
	public GameObject m_noGamesLabel;

	// Token: 0x04000C96 RID: 3222
	public GameObject m_scoreObj;

	// Token: 0x04000C97 RID: 3223
	public TextMeshProUGUI m_currentScore;

	// Token: 0x04000C98 RID: 3224
	public TextMeshProUGUI m_currentRequired;

	// Token: 0x04000C99 RID: 3225
	public GameObject m_restartSeriesButton;

	// Token: 0x04000C9A RID: 3226
	public GameObject m_restartGameButton;

	// Token: 0x04000C9B RID: 3227
	private List<UIP_SoloSeriesGraphX> m_graphDots = new List<UIP_SoloSeriesGraphX>();

	// Token: 0x04000C9C RID: 3228
	private List<GameObject> m_cards = new List<GameObject>();

	// Token: 0x04000C9D RID: 3229
	private ShortSaveStruct m_saveData;

	// Token: 0x04000C9E RID: 3230
	private string m_filePathShortGame = string.Empty;

	// Token: 0x04000C9F RID: 3231
	private string m_filePathFullGame = string.Empty;

	// Token: 0x04000CA0 RID: 3232
	private bool m_bHandlePopup;

	// Token: 0x04000CA1 RID: 3233
	private bool m_bNeedToCreateGame;

	// Token: 0x04000CA2 RID: 3234
	private bool m_bIgnoreToggles;

	// Token: 0x04000CA3 RID: 3235
	private uint m_setFlags;

	// Token: 0x04000CA4 RID: 3236
	private int m_m_saveDataSize;

	// Token: 0x04000CA5 RID: 3237
	private int m_worldDataVersion;
}
