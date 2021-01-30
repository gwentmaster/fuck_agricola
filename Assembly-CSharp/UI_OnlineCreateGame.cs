using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000122 RID: 290
public class UI_OnlineCreateGame : UI_NetworkBase
{
	// Token: 0x06000AEC RID: 2796 RVA: 0x0004AA3C File Offset: 0x00048C3C
	private void Awake()
	{
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		this.m_defaultDraggable = default(FriendInfo);
		this.m_defaultDraggable.displayName = "(OPEN)";
		this.m_defaultDraggable.userAvatar = 0;
		this.m_defaultDraggable.userID = 0U;
		this.m_defaultDraggable.userRating = 0;
		for (int i = 1; i < this.m_maxPlayers; i++)
		{
			this.PopulateSlot(this.m_defaultDraggable, i);
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0004AAB8 File Offset: 0x00048CB8
	public void OnEnterMenu()
	{
		if (this.m_bIgnoreNextEnter)
		{
			this.m_bIgnoreNextEnter = false;
			this.UpdatePlayerStates();
			return;
		}
		this.PopulateSlot(new FriendInfo
		{
			displayName = UI_NetworkBase.m_localPlayerProfile.displayName,
			userAvatar = UI_NetworkBase.m_localPlayerProfile.userAvatar,
			userID = UI_NetworkBase.m_localPlayerProfile.userID,
			userRating = UI_NetworkBase.m_localPlayerProfile.userGameStats.userRating
		}, 0);
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.DisableToggleSoundEffects();
		}
		this.m_bIgnoreToggles = true;
		this.RetreiveSettings();
		this.m_bIgnoreToggles = false;
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.EnableToggleSoundEffects();
		}
		this.UpdateTimerLabel();
		this.OnGameTypeChanged();
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0004AB96 File Offset: 0x00048D96
	public void OnExitMenu(bool bUnderPopup)
	{
		if (bUnderPopup || this.m_bIgnoreNextEnter)
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

	// Token: 0x06000AEF RID: 2799 RVA: 0x0004ABC8 File Offset: 0x00048DC8
	private void PopulateSlot(FriendInfo friend, int index)
	{
		if (index >= 0 && index < this.m_playerSlots.Length)
		{
			this.m_playerSlots[index].name.text = friend.displayName;
			this.m_playerSlots[index].rating.text = ((friend.userID == 0U) ? "???" : friend.userRating.ToString());
			this.m_playerSlots[index].playerID = friend.userID;
			this.m_playerSlots[index].humanRoot.SetActive(friend.userID > 0U);
			this.m_playerSlots[index].openRoot.SetActive(friend.userID == 0U);
			if (index == 0)
			{
				this.m_playerSlots[index].avatar.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar1), true);
				this.m_playerSlots[index].avatar2.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar2), true);
				this.m_playerSlots[index].m_colorizer.Colorize((uint)this.m_localProfile.factionIndex);
			}
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0004AD18 File Offset: 0x00048F18
	public void OnInviteButtonPressed()
	{
		GameObject scene = ScreenManager.instance.GetScene("FriendsList");
		if (scene != null)
		{
			UI_FriendList component = scene.GetComponent<UI_FriendList>();
			if (component != null)
			{
				component.SetInviteCallback(new UI_FriendList.InviteCallback(this.HandleInvites));
				int inviteIndex = 3;
				for (int i = 1; i < this.m_maxPlayers; i++)
				{
					if (!this.m_playerSlots[i].nameObj.activeSelf || this.m_playerSlots[i].openRoot.activeSelf)
					{
						inviteIndex = i;
						break;
					}
				}
				this.m_bIgnoreNextEnter = true;
				this.m_inviteIndex = inviteIndex;
				ScreenManager.instance.PushScene("FriendsList");
			}
		}
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0004ADC8 File Offset: 0x00048FC8
	public void HandleInvites(string displayName, uint userID, int rating)
	{
		for (int i = 0; i < this.m_maxPlayers; i++)
		{
			if (this.m_playerSlots[i].playerID != 0U && this.m_playerSlots[i].playerID == userID)
			{
				this.m_inviteIndex = -1;
				break;
			}
		}
		if (this.m_inviteIndex != -1)
		{
			this.m_playerSlots[this.m_inviteIndex].name.text = displayName;
			this.m_playerSlots[this.m_inviteIndex].nameObj.SetActive(true);
			this.m_playerSlots[this.m_inviteIndex].rating.text = rating.ToString();
			this.m_playerSlots[this.m_inviteIndex].playerID = userID;
			this.m_playerSlots[this.m_inviteIndex].playerTypeObj[0].SetActive(true);
			this.m_playerSlots[this.m_inviteIndex].playerTypeObj[1].SetActive(false);
			this.m_playerSlots[this.m_inviteIndex].playerTypeObj[2].SetActive(false);
			this.m_playerSlots[this.m_inviteIndex].humanRoot.SetActive(true);
			this.m_playerSlots[this.m_inviteIndex].openRoot.SetActive(false);
			this.m_inviteIndex = -1;
		}
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0004AF2C File Offset: 0x0004912C
	public void OnCreateGameButtonPressed()
	{
		this.m_delayCoroutine = base.StartCoroutine(this.ProcessDelayTime(this.m_minDialogDisplayTime));
		this.CreateGame();
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bHandlePopup = true;
				component.Setup(null, "Key_CreatingGame", UI_ConfirmPopup.ButtonFormat.NoButtons);
				ScreenManager.instance.PushScene("ConfirmPopup");
			}
		}
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0004AFA2 File Offset: 0x000491A2
	public void HandleCreateCanceled()
	{
		if (this.m_delayCoroutine != null)
		{
			base.StopCoroutine(this.m_delayCoroutine);
			this.m_delayCoroutine = null;
		}
		this.m_bMinDialogDisplayTimeReached = false;
		this.m_bProcessCreateGameReply = false;
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0004AFCD File Offset: 0x000491CD
	private IEnumerator HandleBackFromPopup()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		ScreenManager.instance.PopScene();
		yield break;
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0004AFD5 File Offset: 0x000491D5
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
		if (this.m_bProcessCreateGameReply)
		{
			this.ProcessCreateGameReply(this.m_createGameReplyData);
		}
		yield break;
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkStart()
	{
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00003022 File Offset: 0x00001222
	protected override void NetworkOnDestroy()
	{
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0004AFEB File Offset: 0x000491EB
	protected override void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		base.NetworkEventCallback(eventType, eventData);
		if (eventType == NetworkEvent.EventType.Event_CreateGameReply)
		{
			if (this.m_bMinDialogDisplayTimeReached)
			{
				this.ProcessCreateGameReply(eventData);
				return;
			}
			this.m_createGameReplyData = eventData;
			this.m_bProcessCreateGameReply = true;
		}
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0004B018 File Offset: 0x00049218
	private void ProcessCreateGameReply(int eventData)
	{
		string messageKey = "";
		this.m_bCreateSuccess = true;
		switch (eventData)
		{
		case -1:
			messageKey = "${Key_CreatingGame}";
			break;
		case 0:
			this.m_bProcessCreateGameReply = false;
			messageKey = "${Key_CreatedGame}";
			base.StartCoroutine(this.DelayCloseAfterSuccess());
			break;
		case 1:
			this.m_bProcessCreateGameReply = false;
			messageKey = "${Key_UnableCreateGame}";
			this.m_bCreateSuccess = false;
			break;
		case 2:
			this.m_bProcessCreateGameReply = false;
			messageKey = "${Key_TooMany}";
			this.m_bCreateSuccess = false;
			break;
		}
		GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
		if (scene != null)
		{
			UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
			if (component)
			{
				this.m_bHandlePopup = true;
				component.Setup(null, messageKey, this.m_bCreateSuccess ? UI_ConfirmPopup.ButtonFormat.NoButtons : UI_ConfirmPopup.ButtonFormat.OneButton);
			}
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0004B0DF File Offset: 0x000492DF
	private IEnumerator DelayCloseAfterSuccess()
	{
		yield return new WaitForSeconds(1f);
		if (this.m_bkgAnimator != null)
		{
			this.m_bkgAnimator.Play("CreateOnline_OnlineLobby");
		}
		ScreenManager.instance.GoToScene("OnlineLobby", true);
		yield break;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0004B0EE File Offset: 0x000492EE
	public void OnTimerButtonPressed()
	{
		this.m_gameTimeIndex += 1U;
		if ((ulong)this.m_gameTimeIndex >= (ulong)((long)PlayerTimers.s_playerTimerOptions.Length))
		{
			this.m_gameTimeIndex = 0U;
		}
		this.UpdateTimerLabel();
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0004B11C File Offset: 0x0004931C
	private void UpdateTimerLabel()
	{
		this.m_timerValueText.KeyText = PlayerTimers.s_playerTimerText[(int)this.m_gameTimeIndex];
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0004B138 File Offset: 0x00049338
	public void OnPlayerPressed(int index)
	{
		if (index == 0)
		{
			GameObject scene = ScreenManager.instance.GetScene(this.m_AvatarSelectionScreenName);
			if (scene != null)
			{
				this.m_bIgnoreNextEnter = true;
				scene.GetComponent<UI_AvatarSelect>().SetProfile(this.m_localProfile, new UI_AvatarSelect.AvatarCallback(this.HandleBackFromAvatarSelect), false, true, false);
				ScreenManager.instance.PushScene(this.m_AvatarSelectionScreenName);
			}
		}
		else if (!this.m_playerSlots[index].nameObj.activeSelf)
		{
			this.m_playerSlots[index].playerTypeObj[0].SetActive(true);
			this.m_playerSlots[index].playerTypeObj[1].SetActive(false);
			this.m_playerSlots[index].playerTypeObj[2].SetActive(false);
			this.m_playerSlots[index].nameObj.SetActive(true);
			this.PopulateSlot(this.m_defaultDraggable, index);
		}
		else
		{
			this.PopulateSlot(this.m_defaultDraggable, index);
			this.m_playerSlots[index].humanRoot.SetActive(false);
			this.m_playerSlots[index].playerTypeObj[0].SetActive(false);
			this.m_playerSlots[index].playerTypeObj[1].SetActive(true);
			this.m_playerSlots[index].playerTypeObj[2].SetActive(true);
			this.m_playerSlots[index].nameObj.SetActive(false);
		}
		this.UpdatePlayerStates();
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0004B2BC File Offset: 0x000494BC
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
			this.m_playerSlots[0].avatar.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar1), true);
			this.m_playerSlots[0].avatar2.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar2), true);
			this.m_playerSlots[0].m_colorizer.Colorize((uint)this.m_localProfile.factionIndex);
		}
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0004B3D8 File Offset: 0x000495D8
	public void OnGameTypeChanged()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_typeToggles[0].isOn)
		{
			this.m_bIgnoreToggles = true;
			this.m_typeToggles[1].isOn = true;
			this.m_typeToggles[0].isOn = false;
			this.m_gameType = 1U;
			this.m_bIgnoreToggles = false;
		}
		else if (this.m_typeToggles[1].isOn)
		{
			this.m_gameType = 1U;
		}
		else if (this.m_typeToggles[2].isOn)
		{
			this.m_gameType = 3U;
		}
		else if (this.m_typeToggles[3].isOn)
		{
			this.m_gameType = 2U;
		}
		uint setFlags = this.m_setFlags;
		this.m_setFlags = 0U;
		int num = 12;
		for (int i = 0; i < this.m_setToggles.Length; i++)
		{
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
		this.UpdatePlayerStates();
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0004B520 File Offset: 0x00049720
	public void UpdatePlayerStates()
	{
		this.m_playerSlots[0].m_colorizer.Colorize((uint)this.m_localProfile.factionIndex);
		int num = 1;
		bool interactable = false;
		for (int i = 1; i < this.m_maxPlayers; i++)
		{
			if (this.m_playerSlots[i].nameObj.activeSelf)
			{
				num++;
			}
			if (!this.m_playerSlots[i].nameObj.activeSelf || this.m_playerSlots[i].openRoot.activeSelf)
			{
				interactable = true;
			}
			uint factionIndex = (uint)(((int)this.m_localProfile.factionIndex == i) ? 0 : i);
			this.m_playerSlots[i].m_colorizer.Colorize(factionIndex);
		}
		if (this.m_createButton != null)
		{
			this.m_createButton.SetActive(num > 1);
		}
		if (this.m_inviteButton != null)
		{
			this.m_inviteButton.interactable = interactable;
		}
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0004B618 File Offset: 0x00049818
	public void CreateGame()
	{
		if (1 < 0)
		{
			return;
		}
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
		uint num = 1U;
		int num2 = 0;
		uint[] array = new uint[4];
		for (int j = 1; j < this.m_maxPlayers; j++)
		{
			if (this.m_playerSlots[j].nameObj.activeSelf)
			{
				if (this.m_playerSlots[j].playerID != 0U)
				{
					array[num2++] = this.m_playerSlots[j].playerID;
				}
				num += 1U;
			}
		}
		array[num2] = 0U;
		Network.SendPlayerParameters(new GamePlayerParameters
		{
			avatar1 = this.m_localProfile.gameAvatar1 + 10 * this.m_localProfile.factionIndex,
			avatar2 = this.m_localProfile.gameAvatar2 + 10 * this.m_localProfile.factionIndex,
			avatar3 = this.m_localProfile.gameAvatar3 + 10 * this.m_localProfile.factionIndex,
			avatar4 = this.m_localProfile.gameAvatar4 + 10 * this.m_localProfile.factionIndex,
			avatar5 = this.m_localProfile.gameAvatar5 + 10 * this.m_localProfile.factionIndex,
			avatarColorIndex = this.m_localProfile.factionIndex
		});
		Network.CreateGame(num, array, (uint)PlayerTimers.s_playerTimerOptions[(int)this.m_gameTimeIndex], gameParameters);
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0004B804 File Offset: 0x00049A04
	private void RetreiveSettings()
	{
		int @int = PlayerPrefs.GetInt("OnlineCreateGame_DeckA", 1);
		this.m_setToggles[0].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OnlineCreateGame_DeckB", 0);
		this.m_setToggles[1].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OnlineCreateGame_FamilyGame", 0);
		this.m_typeToggles[0].isOn = false;
		@int = PlayerPrefs.GetInt("OnlineCreateGame_Random", 1);
		this.m_typeToggles[1].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OnlineCreateGame_Discard", 0);
		this.m_typeToggles[2].isOn = (@int == 1);
		@int = PlayerPrefs.GetInt("OnlineCreateGame_Draft", 0);
		this.m_typeToggles[3].isOn = (@int == 1);
		for (int i = 0; i < this.m_setLocks.Length; i++)
		{
			if (this.m_setLocks[i] != null)
			{
				this.m_setToggles[i].interactable = true;
				this.m_setLocks[i].SetActive(false);
			}
		}
		for (int j = 1; j < this.m_maxPlayers; j++)
		{
			if (PlayerPrefs.GetInt("OnlineCreateGame_Slot" + j.ToString() + "_Type", 0) == 1)
			{
				this.m_playerSlots[j].playerTypeObj[0].SetActive(true);
				this.m_playerSlots[j].playerTypeObj[1].SetActive(false);
				this.m_playerSlots[j].playerTypeObj[2].SetActive(false);
				this.m_playerSlots[j].nameObj.SetActive(true);
				this.PopulateSlot(this.m_defaultDraggable, j);
			}
			else
			{
				this.PopulateSlot(this.m_defaultDraggable, j);
				this.m_playerSlots[j].humanRoot.SetActive(false);
				this.m_playerSlots[j].playerTypeObj[0].SetActive(false);
				this.m_playerSlots[j].playerTypeObj[1].SetActive(true);
				this.m_playerSlots[j].playerTypeObj[2].SetActive(true);
				this.m_playerSlots[j].nameObj.SetActive(false);
			}
		}
		this.m_localProfile.factionIndex = (byte)PlayerPrefs.GetInt("OnlineCreateGame_FactionIndex", 0);
		this.m_localProfile.gameAvatar1 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar1", 1);
		this.m_localProfile.gameAvatar2 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar2", 2);
		this.m_localProfile.gameAvatar3 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar3", 3);
		this.m_localProfile.gameAvatar4 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar4", 4);
		this.m_localProfile.gameAvatar5 = (byte)PlayerPrefs.GetInt("OnlineCreateGame_Avatar5", 5);
		this.m_playerSlots[0].avatar.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar1), true);
		this.m_playerSlots[0].avatar2.SetAvatar((int)(10 * this.m_localProfile.factionIndex + this.m_localProfile.gameAvatar2), true);
		this.m_playerSlots[0].m_colorizer.Colorize((uint)this.m_localProfile.factionIndex);
		this.UpdatePlayerStates();
		this.OnGameTypeChanged();
		this.m_gameTimeIndex = (uint)PlayerPrefs.GetInt("OnlineCreateGame_Timer", 0);
		if ((ulong)this.m_gameTimeIndex >= (ulong)((long)PlayerTimers.s_playerTimerOptions.Length))
		{
			this.m_gameTimeIndex = 0U;
		}
		this.UpdateTimerLabel();
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0004BB74 File Offset: 0x00049D74
	private void StoreSettings()
	{
		PlayerPrefs.SetInt("OnlineCreateGame_DeckA", this.m_setToggles[0].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OnlineCreateGame_DeckB", this.m_setToggles[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OnlineCreateGame_FamilyGame", 0);
		PlayerPrefs.SetInt("OnlineCreateGame_Random", this.m_typeToggles[1].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OnlineCreateGame_Discard", this.m_typeToggles[2].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OnlineCreateGame_Draft", this.m_typeToggles[3].isOn ? 1 : 0);
		PlayerPrefs.SetInt("OnlineCreateGame_Timer", (int)this.m_gameTimeIndex);
		PlayerPrefs.SetInt("OnlineCreateGame_FactionIndex", (int)this.m_localProfile.factionIndex);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar1", (int)this.m_localProfile.gameAvatar1);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar2", (int)this.m_localProfile.gameAvatar2);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar3", (int)this.m_localProfile.gameAvatar3);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar4", (int)this.m_localProfile.gameAvatar4);
		PlayerPrefs.SetInt("OnlineCreateGame_Avatar5", (int)this.m_localProfile.gameAvatar5);
		for (int i = 1; i < this.m_maxPlayers; i++)
		{
			if (!this.m_playerSlots[i].nameObj.activeSelf)
			{
				PlayerPrefs.SetInt("OnlineCreateGame_Slot" + i.ToString() + "_Type", 0);
			}
			else
			{
				PlayerPrefs.SetInt("OnlineCreateGame_Slot" + i.ToString() + "_Type", 1);
			}
		}
	}

	// Token: 0x04000BA5 RID: 2981
	public UI_OnlineCreateGame.PlayerSlot[] m_playerSlots;

	// Token: 0x04000BA6 RID: 2982
	public Toggle[] m_typeToggles;

	// Token: 0x04000BA7 RID: 2983
	public Toggle[] m_setToggles;

	// Token: 0x04000BA8 RID: 2984
	public GameObject[] m_setLocks;

	// Token: 0x04000BA9 RID: 2985
	public GameObject m_createButton;

	// Token: 0x04000BAA RID: 2986
	public Button m_inviteButton;

	// Token: 0x04000BAB RID: 2987
	public string m_AvatarSelectionScreenName = "FamilySelection";

	// Token: 0x04000BAC RID: 2988
	public UILocalizedText m_timerValueText;

	// Token: 0x04000BAD RID: 2989
	public float m_minDialogDisplayTime;

	// Token: 0x04000BAE RID: 2990
	public Animator m_bkgAnimator;

	// Token: 0x04000BAF RID: 2991
	private ProfileManager.OfflineProfileEntry m_localProfile = new ProfileManager.OfflineProfileEntry();

	// Token: 0x04000BB0 RID: 2992
	private FriendInfo m_defaultDraggable;

	// Token: 0x04000BB1 RID: 2993
	private uint m_gameTimeIndex;

	// Token: 0x04000BB2 RID: 2994
	private Coroutine m_delayCoroutine;

	// Token: 0x04000BB3 RID: 2995
	private int m_createGameReplyData;

	// Token: 0x04000BB4 RID: 2996
	private bool m_bMinDialogDisplayTimeReached;

	// Token: 0x04000BB5 RID: 2997
	private bool m_bProcessCreateGameReply;

	// Token: 0x04000BB6 RID: 2998
	private bool m_bHandlePopup;

	// Token: 0x04000BB7 RID: 2999
	private bool m_bIgnoreNextEnter;

	// Token: 0x04000BB8 RID: 3000
	private bool m_bCreateSuccess;

	// Token: 0x04000BB9 RID: 3001
	private bool m_bIgnoreToggles;

	// Token: 0x04000BBA RID: 3002
	private uint m_setFlags;

	// Token: 0x04000BBB RID: 3003
	private uint m_gameType;

	// Token: 0x04000BBC RID: 3004
	private int m_inviteIndex;

	// Token: 0x04000BBD RID: 3005
	private int m_maxPlayers = 4;

	// Token: 0x04000BBE RID: 3006
	private InAppPurchaseWrapper m_InAppPurchase;

	// Token: 0x020007F7 RID: 2039
	[Serializable]
	public struct PlayerSlot
	{
		// Token: 0x04002D99 RID: 11673
		public GameObject humanRoot;

		// Token: 0x04002D9A RID: 11674
		public GameObject openRoot;

		// Token: 0x04002D9B RID: 11675
		public Avatar_UI avatar;

		// Token: 0x04002D9C RID: 11676
		public Avatar_UI avatar2;

		// Token: 0x04002D9D RID: 11677
		public TextMeshProUGUI name;

		// Token: 0x04002D9E RID: 11678
		public TextMeshProUGUI rating;

		// Token: 0x04002D9F RID: 11679
		public GameObject nameObj;

		// Token: 0x04002DA0 RID: 11680
		public GameObject[] playerTypeObj;

		// Token: 0x04002DA1 RID: 11681
		public ColorByFaction m_colorizer;

		// Token: 0x04002DA2 RID: 11682
		[HideInInspector]
		public uint playerID;
	}
}
