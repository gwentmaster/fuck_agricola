using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000129 RID: 297
public class UI_QuickMatch : UI_NetworkBase
{
	// Token: 0x06000B60 RID: 2912 RVA: 0x00003022 File Offset: 0x00001222
	private void Start()
	{
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0004F1C0 File Offset: 0x0004D3C0
	public void OnEnterMenu()
	{
		this.m_bIgnoreToggles = true;
		this.RetreiveSettings();
		this.m_bIgnoreToggles = false;
		this.UpdateRankingLabel();
		this.UpdateTimerLabel();
		this.UpdateWaitTimerLabel();
		this.UpdateNumPlayersLabel();
		this.OnGameTypeChanged();
		this.m_delayCoroutine = null;
		this.m_bMinDialogDisplayTimeReached = false;
		this.m_bProcessMatchmakingReply = false;
		this.m_userNameLabel.text = UI_NetworkBase.m_localPlayerProfile.displayName;
		this.m_userRatingLabel.text = UI_NetworkBase.m_localPlayerProfile.userGameStats.userRating.ToString();
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0004F248 File Offset: 0x0004D448
	public void OnExitMenu(bool bUnderPopup)
	{
		if (bUnderPopup)
		{
			return;
		}
		this.StoreSettings();
		if (this.m_delayCoroutine != null)
		{
			base.StopCoroutine(this.m_delayCoroutine);
			this.m_delayCoroutine = null;
		}
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0004F270 File Offset: 0x0004D470
	public void OnPlayerPressed()
	{
		GameObject scene = ScreenManager.instance.GetScene(this.m_AvatarSelectionScreenName);
		if (scene != null)
		{
			scene.GetComponent<UI_AvatarSelect>().SetProfile(this.m_localProfile, new UI_AvatarSelect.AvatarCallback(this.HandleBackFromAvatarSelect), false, true, false);
			ScreenManager.instance.PushScene(this.m_AvatarSelectionScreenName);
		}
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0004F2C8 File Offset: 0x0004D4C8
	public void HandleBackFromAvatarSelect(ProfileManager.OfflineProfileEntry profile, bool bConfirm)
	{
		if (bConfirm)
		{
			this.m_localProfile = profile;
			PlayerPrefs.SetInt("OnlineCreateGame_FactionIndex", (int)this.m_localProfile.factionIndex);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar1", (int)this.m_localProfile.gameAvatar1);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar2", (int)this.m_localProfile.gameAvatar2);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar3", (int)this.m_localProfile.gameAvatar3);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar4", (int)this.m_localProfile.gameAvatar4);
			PlayerPrefs.SetInt("OnlineCreateGame_Avatar5", (int)this.m_localProfile.gameAvatar5);
			this.m_avatar.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar1), true);
			this.m_avatar2.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar2), true);
			this.m_colorizer.Colorize((uint)this.m_localProfile.factionIndex);
		}
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0004F3C2 File Offset: 0x0004D5C2
	public void OnRankingButtonPressed()
	{
		this.m_rankSpread++;
		if (this.m_rankSpread < 0 || this.m_rankSpread >= this.s_QuickMatchRateOptions.Length)
		{
			this.m_rankSpread = 0;
		}
		this.UpdateRankingLabel();
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0004F3F8 File Offset: 0x0004D5F8
	public void OnTimerButtonPressed()
	{
		this.m_gameTime++;
		if (this.m_gameTime < 0 || this.m_gameTime >= PlayerTimers.s_playerTimerOptions.Length)
		{
			this.m_gameTime = 0;
		}
		this.UpdateTimerLabel();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0004F42D File Offset: 0x0004D62D
	public void OnWaitTimerButtonPressed()
	{
		this.m_waitTime++;
		if (this.m_waitTime < 0 || this.m_waitTime >= this.s_PlayerWaitTimeOptions.Length)
		{
			this.m_waitTime = 0;
		}
		this.UpdateWaitTimerLabel();
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0004F463 File Offset: 0x0004D663
	public void OnPlayerCountButtonPressed()
	{
		this.m_numPlayers++;
		if (this.m_numPlayers > 4)
		{
			this.m_numPlayers = 2;
		}
		this.UpdateNumPlayersLabel();
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0004F48C File Offset: 0x0004D68C
	public void OnGameTypeChanged()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_gameTypeToggles[0].isOn)
		{
			this.m_bIgnoreToggles = true;
			this.m_gameTypeToggles[1].isOn = true;
			this.m_gameTypeToggles[0].isOn = false;
			this.m_gameType = 1U;
			this.m_bIgnoreToggles = false;
		}
		else if (this.m_gameTypeToggles[1].isOn)
		{
			this.m_gameType = 1U;
		}
		else if (this.m_gameTypeToggles[2].isOn)
		{
			this.m_gameType = 3U;
		}
		else if (this.m_gameTypeToggles[3].isOn)
		{
			this.m_gameType = 2U;
		}
		uint setFlags = this.m_setFlags;
		this.m_setFlags = 0U;
		for (int i = 0; i < this.m_deckToggles.Length; i++)
		{
			if (this.m_deckToggles[i].interactable && this.m_deckToggles[i].isOn)
			{
				this.m_setFlags |= 1U << i;
			}
		}
		if (this.m_setFlags == 0U)
		{
			this.m_bIgnoreToggles = true;
			for (int j = 0; j < this.m_deckToggles.Length; j++)
			{
				if ((setFlags & 1U << j) != 0U)
				{
					this.m_deckToggles[j].isOn = true;
				}
			}
			this.m_setFlags = setFlags;
			this.m_bIgnoreToggles = false;
		}
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0004F5C4 File Offset: 0x0004D7C4
	public void OnCreateMatchmakingButtonPressed()
	{
		this.m_delayCoroutine = base.StartCoroutine(this.ProcessDelayTime(this.m_minDialogDisplayTime));
		this.CreateQuickMatchGame();
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				component.Setup(null, "Key_QuickMatchCreating", UI_ConfirmPopup.ButtonFormat.NoButtons);
				ScreenManager.instance.PushScene("ConfirmPopup");
			}
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0004F633 File Offset: 0x0004D833
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
		if (eventType == NetworkEvent.EventType.Event_MatchmakingReply)
		{
			if (this.m_bMinDialogDisplayTimeReached)
			{
				this.ProcessMatchmakingReply(eventData);
				return;
			}
			this.m_matchmakingReplyData = eventData;
			this.m_bProcessMatchmakingReply = true;
		}
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0004F660 File Offset: 0x0004D860
	private void UpdateRankingLabel()
	{
		this.m_RankingValue.KeyText = this.s_QuickMatchRateOptionsText[this.m_rankSpread].ToString();
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0004F67F File Offset: 0x0004D87F
	private void UpdateTimerLabel()
	{
		this.m_TimerValue.KeyText = PlayerTimers.s_playerTimerText[this.m_gameTime];
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0004F698 File Offset: 0x0004D898
	private void UpdateWaitTimerLabel()
	{
		this.m_ExpireValue.KeyText = this.s_PlayerWaitTimeText[this.m_waitTime];
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0004F6B2 File Offset: 0x0004D8B2
	private void UpdateNumPlayersLabel()
	{
		this.m_NumPlayersText.text = this.m_numPlayers.ToString();
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x0004F6CC File Offset: 0x0004D8CC
	private void ProcessMatchmakingReply(int eventData)
	{
		string messageKey = "";
		bool flag = true;
		switch (eventData)
		{
		case -1:
			messageKey = "Key_QuickMatchCreating";
			break;
		case 0:
			this.m_bProcessMatchmakingReply = false;
			messageKey = "${Key_CreatedGame}";
			base.StartCoroutine(this.DelayCloseAfterSuccess());
			break;
		case 1:
			messageKey = "Key_UnableCreateMatch";
			flag = false;
			break;
		case 2:
			messageKey = "Key_TooMany";
			flag = false;
			break;
		}
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				component.Setup(null, messageKey, flag ? UI_ConfirmPopup.ButtonFormat.NoButtons : UI_ConfirmPopup.ButtonFormat.OneButton);
			}
		}
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0004F76D File Offset: 0x0004D96D
	private IEnumerator DelayCloseAfterSuccess()
	{
		yield return new WaitForSeconds(1f);
		if (this.m_bkgAnimator != null)
		{
			this.m_bkgAnimator.Play("OnlineGames_OnlineLobby");
		}
		ScreenManager.instance.GoToScene("OnlineLobby", true);
		yield break;
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0004F77C File Offset: 0x0004D97C
	private void CreateQuickMatchGame()
	{
		GameParameters gameParameters = default(GameParameters);
		gameParameters.gameType = (ushort)this.m_gameType;
		gameParameters.deckFlags = ((this.m_gameType != 0U) ? 16384 : 0);
		gameParameters.soloGameCount = 0;
		gameParameters.soloGameStartFood = 0;
		gameParameters.soloGameStartOccupations = new ushort[7];
		for (int i = 0; i < 7; i++)
		{
			gameParameters.soloGameStartOccupations[i] = 0;
		}
		if (this.m_gameType == 1U || this.m_gameType == 2U || this.m_gameType == 3U)
		{
			gameParameters.deckFlags = 16384;
		}
		Network.SendPlayerParameters(new GamePlayerParameters
		{
			avatar1 = this.m_localProfile.gameAvatar1 + 10 * this.m_localProfile.factionIndex,
			avatar2 = this.m_localProfile.gameAvatar2 + 10 * this.m_localProfile.factionIndex,
			avatar3 = this.m_localProfile.gameAvatar3 + 10 * this.m_localProfile.factionIndex,
			avatar4 = this.m_localProfile.gameAvatar4 + 10 * this.m_localProfile.factionIndex,
			avatar5 = this.m_localProfile.gameAvatar5 + 10 * this.m_localProfile.factionIndex,
			avatarColorIndex = this.m_localProfile.factionIndex
		});
		Network.RequestMatchmaking((uint)this.m_numPlayers, (uint)PlayerTimers.s_playerTimerOptions[this.m_gameTime], (uint)this.s_QuickMatchRateOptions[this.m_rankSpread], (uint)this.s_PlayerWaitTimeOptions[this.m_waitTime], gameParameters);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0004F90C File Offset: 0x0004DB0C
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
		if (this.m_bProcessMatchmakingReply)
		{
			this.ProcessMatchmakingReply(this.m_matchmakingReplyData);
		}
		yield break;
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0004F924 File Offset: 0x0004DB24
	private void RetreiveSettings()
	{
		int @int = PlayerPrefs.GetInt("matchmaking_RatingSpread", this.s_QuickMatchRateOptions[0]);
		this.m_rankSpread = PlayerTimers.GetClosestValue(@int, this.s_QuickMatchRateOptions);
		if (this.m_rankSpread < 0 || this.m_rankSpread >= this.s_QuickMatchRateOptions.Length)
		{
			this.m_rankSpread = 0;
		}
		int int2 = PlayerPrefs.GetInt("matchmaking_Timer", PlayerTimers.s_playerTimerOptions[0]);
		this.m_gameTime = PlayerTimers.GetClosestValue(int2, PlayerTimers.s_playerTimerOptions);
		if (this.m_gameTime < 0 || this.m_gameTime >= PlayerTimers.s_playerTimerOptions.Length)
		{
			this.m_gameTime = 0;
		}
		int int3 = PlayerPrefs.GetInt("matchmaking_WaitTime", this.s_PlayerWaitTimeOptions[0]);
		this.m_waitTime = PlayerTimers.GetClosestValue(int3, this.s_PlayerWaitTimeOptions);
		if (this.m_waitTime < 0 || this.m_waitTime >= this.s_PlayerWaitTimeOptions.Length)
		{
			this.m_waitTime = 0;
		}
		this.m_numPlayers = PlayerPrefs.GetInt("matchmaking_NumPlayers", 2);
		int int4 = PlayerPrefs.GetInt("Matchmaking_DeckA", 1);
		this.m_deckToggles[0].isOn = (int4 == 1);
		int4 = PlayerPrefs.GetInt("Matchmaking_DeckB", 0);
		this.m_deckToggles[1].isOn = (int4 == 1);
		int4 = PlayerPrefs.GetInt("Matchmaking_FamilyGame", 0);
		this.m_gameTypeToggles[0].isOn = false;
		int4 = PlayerPrefs.GetInt("Matchmaking_Random", 1);
		this.m_gameTypeToggles[1].isOn = (int4 == 1);
		int4 = PlayerPrefs.GetInt("Matchmaking_Discard", 0);
		this.m_gameTypeToggles[2].isOn = (int4 == 1);
		int4 = PlayerPrefs.GetInt("Matchmaking_Draft", 0);
		this.m_gameTypeToggles[3].isOn = (int4 == 1);
		this.m_localProfile.factionIndex = (byte)PlayerPrefs.GetInt("OnlineCreateGame_FactionIndex", 0);
		this.m_localProfile.gameAvatar1 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar1", 1);
		this.m_localProfile.gameAvatar2 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar2", 2);
		this.m_localProfile.gameAvatar3 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar3", 3);
		this.m_localProfile.gameAvatar4 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar4", 4);
		this.m_localProfile.gameAvatar5 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar5", 5);
		this.m_avatar.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar1), true);
		this.m_avatar2.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar2), true);
		this.m_colorizer.Colorize((uint)this.m_localProfile.factionIndex);
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0004FBA8 File Offset: 0x0004DDA8
	private void StoreSettings()
	{
		PlayerPrefs.SetInt("matchmaking_RatingSpread", this.s_QuickMatchRateOptions[this.m_rankSpread]);
		PlayerPrefs.SetInt("matchmaking_Timer", PlayerTimers.s_playerTimerOptions[this.m_gameTime]);
		PlayerPrefs.SetInt("matchmaking_WaitTime", this.s_PlayerWaitTimeOptions[this.m_waitTime]);
		PlayerPrefs.SetInt("matchmaking_NumPlayers", this.m_numPlayers);
		PlayerPrefs.SetInt("Matchmaking_DeckA", this.m_deckToggles[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("Matchmaking_DeckB", this.m_deckToggles[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("Matchmaking_FamilyGame", 0);
		PlayerPrefs.SetInt("Matchmaking_Random", this.m_gameTypeToggles[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("Matchmaking_Discard", this.m_gameTypeToggles[2].isOn ? 1 : 0);
		PlayerPrefs.SetInt("Matchmaking_Draft", this.m_gameTypeToggles[3].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OnlineCreateGame_FactionIndex", (int)this.m_localProfile.factionIndex);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar1", (int)this.m_localProfile.gameAvatar1);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar2", (int)this.m_localProfile.gameAvatar2);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar3", (int)this.m_localProfile.gameAvatar3);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar4", (int)this.m_localProfile.gameAvatar4);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar5", (int)this.m_localProfile.gameAvatar5);
	}

	// Token: 0x04000C37 RID: 3127
	public UILocalizedText m_RankingValue;

	// Token: 0x04000C38 RID: 3128
	public UILocalizedText m_TimerValue;

	// Token: 0x04000C39 RID: 3129
	public UILocalizedText m_ExpireValue;

	// Token: 0x04000C3A RID: 3130
	public TextMeshProUGUI m_NumPlayersText;

	// Token: 0x04000C3B RID: 3131
	public Toggle[] m_gameTypeToggles;

	// Token: 0x04000C3C RID: 3132
	public Toggle[] m_deckToggles;

	// Token: 0x04000C3D RID: 3133
	public float m_minDialogDisplayTime;

	// Token: 0x04000C3E RID: 3134
	public TextMeshProUGUI m_userNameLabel;

	// Token: 0x04000C3F RID: 3135
	public TextMeshProUGUI m_userRatingLabel;

	// Token: 0x04000C40 RID: 3136
	public Avatar_UI m_avatar;

	// Token: 0x04000C41 RID: 3137
	public Avatar_UI m_avatar2;

	// Token: 0x04000C42 RID: 3138
	public ColorByFaction m_colorizer;

	// Token: 0x04000C43 RID: 3139
	public string m_AvatarSelectionScreenName = "FamilySelection";

	// Token: 0x04000C44 RID: 3140
	public Animator m_bkgAnimator;

	// Token: 0x04000C45 RID: 3141
	private Coroutine m_delayCoroutine;

	// Token: 0x04000C46 RID: 3142
	private readonly int[] s_QuickMatchRateOptions = new int[]
	{
		0,
		100,
		200,
		300,
		400,
		500
	};

	// Token: 0x04000C47 RID: 3143
	private readonly string[] s_QuickMatchRateOptionsText = new string[]
	{
		"Any",
		"+/- 100",
		"+/- 200",
		"+/- 300",
		"+/- 400",
		"+/- 500"
	};

	// Token: 0x04000C48 RID: 3144
	private readonly int[] s_PlayerWaitTimeOptions = new int[]
	{
		900,
		3600,
		7200,
		43200,
		86400,
		259200,
		604800
	};

	// Token: 0x04000C49 RID: 3145
	private readonly string[] s_PlayerWaitTimeText = new string[]
	{
		"15 ${Key_Minutes}",
		"1 ${Key_Hour}",
		"2 ${Key_Hours}",
		"12 ${Key_Hours}",
		"1 ${Key_Day}",
		"3 ${Key_Days}",
		"7 ${Key_Days}"
	};

	// Token: 0x04000C4A RID: 3146
	private int m_rankSpread;

	// Token: 0x04000C4B RID: 3147
	private int m_gameTime;

	// Token: 0x04000C4C RID: 3148
	private int m_waitTime;

	// Token: 0x04000C4D RID: 3149
	private int m_numPlayers;

	// Token: 0x04000C4E RID: 3150
	private uint m_gameType;

	// Token: 0x04000C4F RID: 3151
	private uint m_setFlags;

	// Token: 0x04000C50 RID: 3152
	private int m_matchmakingReplyData;

	// Token: 0x04000C51 RID: 3153
	private bool m_bMinDialogDisplayTimeReached;

	// Token: 0x04000C52 RID: 3154
	private bool m_bProcessMatchmakingReply;

	// Token: 0x04000C53 RID: 3155
	private bool m_bIgnoreToggles;

	// Token: 0x04000C54 RID: 3156
	private ProfileManager.OfflineProfileEntry m_localProfile = new ProfileManager.OfflineProfileEntry();

	// Token: 0x04000C55 RID: 3157
	private InAppPurchaseWrapper m_InAppPurchase;
}
