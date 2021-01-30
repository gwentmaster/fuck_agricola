using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000100 RID: 256
public class UIP_GameSlot : MonoBehaviour
{
	// Token: 0x0600099F RID: 2463 RVA: 0x0004053A File Offset: 0x0003E73A
	public bool GetIsOwner()
	{
		return this.m_bOwner;
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00040542 File Offset: 0x0003E742
	public bool GetIsInvited()
	{
		return this.m_bInvited;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0004054A File Offset: 0x0003E74A
	public int GetNumPlayers()
	{
		return this.m_numPlayers;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00040552 File Offset: 0x0003E752
	public bool NeedsOnlineStatusUpdate()
	{
		return this.m_bNeedsOnlineStatusUpdate;
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x0004055A File Offset: 0x0003E75A
	public void SetAllRequestsProcessed()
	{
		this.m_bNeedsOnlineStatusUpdate = false;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00040563 File Offset: 0x0003E763
	public int[] GetOnlineStatusUpdateUserId()
	{
		return this.m_playerIDs;
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0004056B File Offset: 0x0003E76B
	public string GetShortSavePath()
	{
		return this.m_savePathShort;
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00040573 File Offset: 0x0003E773
	public void SetShortSavePath(string s)
	{
		this.m_savePathShort = s;
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x0004057C File Offset: 0x0003E77C
	public string GetFullSavePath()
	{
		return this.m_savePathFull;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00040584 File Offset: 0x0003E784
	public void SetFullSavePath(string s)
	{
		this.m_savePathFull = s;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x0004058D File Offset: 0x0003E78D
	public int GetSlotIndex()
	{
		return this.m_gameSlotIndex;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00040595 File Offset: 0x0003E795
	public void SetSlotIndex(int i)
	{
		this.m_gameSlotIndex = i;
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0004059E File Offset: 0x0003E79E
	public int GetDataSize()
	{
		return this.m_saveDataSize;
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x000405A6 File Offset: 0x0003E7A6
	public int GetDataVersion()
	{
		return this.m_worldDataVersion;
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x000405AE File Offset: 0x0003E7AE
	public uint GetNetworkGameID()
	{
		return this.m_GameID;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00003022 File Offset: 0x00001222
	private void Start()
	{
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000405B8 File Offset: 0x0003E7B8
	public bool IsVisible()
	{
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Vector3[] array = new Vector3[4];
		this.m_RectTransform.GetWorldCorners(array);
		for (int i = 0; i < 4; i++)
		{
			if (rect.Contains(array[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00040614 File Offset: 0x0003E814
	public void InitializeNetwork(Network network)
	{
		if (!this.m_bNetworkInitialized)
		{
			this.m_network = network;
			this.m_bNeedsOnlineStatusUpdate = false;
			network.AddNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
			this.m_bNetworkInitialized = true;
			return;
		}
		Debug.LogError("ERROR: Trying to call UIP_GameSlot::InitializeNetwork multiple times");
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00040650 File Offset: 0x0003E850
	private void Awake()
	{
		this.m_localPlayerID = Network.NetworkGetLocalID();
		for (int i = 0; i < this.m_playerInfoGameObjects.Length; i++)
		{
			if (this.m_OnlineStatusIndicatorPrefab != null && this.m_playerInfoGameObjects[i].m_playerStatusIndicatorRoot != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_OnlineStatusIndicatorPrefab);
				gameObject.transform.SetParent(this.m_playerInfoGameObjects[i].m_playerStatusIndicatorRoot.transform);
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x000406E0 File Offset: 0x0003E8E0
	private void OnDestroy()
	{
		if (this.m_bNetworkInitialized)
		{
			this.m_network.RemoveNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
			this.m_bNetworkInitialized = false;
		}
		if (this.m_matchmakingTimerCoroutine != null)
		{
			base.StopCoroutine(this.m_matchmakingTimerCoroutine);
			this.m_matchmakingTimerCoroutine = null;
		}
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x00040730 File Offset: 0x0003E930
	private void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		if (eventType == NetworkEvent.EventType.Event_UpdatedOnlineStatus && this.m_type == UIP_GameSlot.ESlotType.Available)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.m_playerIDs[i] == eventData)
				{
					PlayerOnlineStatus userOnlineStatus = (PlayerOnlineStatus)Network.GetUserOnlineStatus((uint)eventData);
					this.OnlineStatusIndicatorsSetActive(i, true);
					this.UpdatePlayerOnlineStatus(i, userOnlineStatus);
					return;
				}
			}
		}
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0004077C File Offset: 0x0003E97C
	public void SetClickListener(UIP_GameSlot.ClickCallback cb)
	{
		this.m_callback = cb;
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00040788 File Offset: 0x0003E988
	public void OnPressSlot()
	{
		if (this.m_bDeleteMode)
		{
			this.OnPressDelete();
			return;
		}
		UIP_GameSlot.ClickEventType evt = UIP_GameSlot.ClickEventType.Evt_Click_Unknown;
		switch (this.m_type)
		{
		case UIP_GameSlot.ESlotType.Active_Offline:
			evt = UIP_GameSlot.ClickEventType.Evt_Click_LoadOfflineGame;
			break;
		case UIP_GameSlot.ESlotType.Active_Online:
			if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_STAGING)
			{
				if (this.m_bReadyToStart && this.m_bOwner)
				{
					evt = UIP_GameSlot.ClickEventType.Evt_Click_NetworkLaunchGame;
				}
				else if (this.m_bInvited)
				{
					evt = UIP_GameSlot.ClickEventType.Evt_Click_NetworkAcceptInvite;
				}
			}
			else if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_PLAYING || this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_COMPLETED)
			{
				evt = UIP_GameSlot.ClickEventType.Evt_Click_NetworkResumeGame;
			}
			break;
		case UIP_GameSlot.ESlotType.Available:
			evt = UIP_GameSlot.ClickEventType.Evt_Click_NetworkJoinGame;
			break;
		}
		if (this.m_callback != null)
		{
			this.m_callback(this, evt);
		}
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0004081C File Offset: 0x0003EA1C
	public void OnPressDelete()
	{
		UIP_GameSlot.ClickEventType evt = UIP_GameSlot.ClickEventType.Evt_Click_Unknown;
		switch (this.m_type)
		{
		case UIP_GameSlot.ESlotType.Active_Offline:
			evt = UIP_GameSlot.ClickEventType.Evt_Click_DeleteOfflineGame;
			break;
		case UIP_GameSlot.ESlotType.Active_Online:
			if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_STAGING)
			{
				if (this.m_bOwner)
				{
					evt = UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteGame;
				}
				else
				{
					evt = UIP_GameSlot.ClickEventType.Evt_Click_NetworkWithdrawFromGame;
				}
			}
			else if (this.m_state != UIP_GameSlot.ENetworkGameState.E_GAMESTATE_COMPLETED)
			{
				int num = 0;
				if (this.m_save_struct.player1ID != 0 && this.m_save_struct.player1State != 4)
				{
					num++;
				}
				if (this.m_save_struct.player2ID != 0 && this.m_save_struct.player2State != 4)
				{
					num++;
				}
				if (this.m_save_struct.player3ID != 0 && this.m_save_struct.player3State != 4)
				{
					num++;
				}
				if (this.m_save_struct.player4ID != 0 && this.m_save_struct.player4State != 4)
				{
					num++;
				}
				if (this.m_save_struct.player5ID != 0 && this.m_save_struct.player5State != 4)
				{
					num++;
				}
				if (this.m_save_struct.player6ID != 0 && this.m_save_struct.player6State != 4)
				{
					num++;
				}
				evt = ((num > 1) ? UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGame : UIP_GameSlot.ClickEventType.Evt_Click_NetworkForfeitGameLastPlayer);
			}
			break;
		case UIP_GameSlot.ESlotType.Matchmaking:
			evt = UIP_GameSlot.ClickEventType.Evt_Click_NetworkDeleteMatchmakingGame;
			break;
		}
		if (this.m_callback != null)
		{
			this.m_callback(this, evt);
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00040960 File Offset: 0x0003EB60
	private void Update()
	{
		if (this.m_bDisplayPlayerTimers)
		{
			this.UpdatePlayerTimers();
		}
		if (this.m_matchmakingOpponentGameObjectIndex >= 0)
		{
			this.UpdateMatchmakingTimer();
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0004097F File Offset: 0x0003EB7F
	public ShortSaveStruct GetGameSlotData()
	{
		return this.m_save_struct;
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00003022 File Offset: 0x00001222
	public void HideDeleteButton()
	{
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00040988 File Offset: 0x0003EB88
	public void SetData(ShortSaveStruct save_struct, UIP_GameSlot.ESlotType slotType, InAppPurchaseWrapper iapManager)
	{
		this.m_playerIDs = new int[6];
		this.m_matchmakingOpponentGameObjectIndex = -1;
		this.m_bDeleteMode = false;
		this.m_bOwner = false;
		this.m_bReadyToStart = false;
		this.m_bInvited = false;
		this.m_bDisplayPlayerTimers = false;
		if (this.m_timerObj)
		{
			this.m_timerObj.SetActive(false);
		}
		if (this.m_timer != null)
		{
			this.m_timer.enabled = false;
		}
		if (this.m_roundText != null)
		{
			this.m_roundText.enabled = false;
		}
		this.m_defaultColor = 0U;
		int num = 12;
		for (int i = 0; i < this.m_setIcons.Length; i++)
		{
			if (this.m_setIcons[i].root != null)
			{
				this.m_setIcons[i].root.SetActive(((int)save_struct.deckFlags & 1 << i + num) != 0);
			}
			if (this.m_setIcons[i].lockIcon != null)
			{
				this.m_setIcons[i].lockIcon.SetActive(false);
			}
		}
		if (this.m_gameTypeImage != null && this.m_gameTypeSprites != null)
		{
			if (save_struct.gameType == 0 || save_struct.gameType == 7)
			{
				this.m_gameTypeImage.sprite = this.m_gameTypeSprites[0];
			}
			else if (save_struct.gameType == 1 || save_struct.gameType == 5)
			{
				this.m_gameTypeImage.sprite = this.m_gameTypeSprites[1];
			}
			else if (save_struct.gameType == 2)
			{
				this.m_gameTypeImage.sprite = this.m_gameTypeSprites[2];
			}
			else if (save_struct.gameType == 3 || save_struct.gameType == 6)
			{
				this.m_gameTypeImage.sprite = this.m_gameTypeSprites[3];
			}
		}
		for (int j = 0; j < this.m_playerInfoGameObjects.Length; j++)
		{
			this.m_playerInfoGameObjects[j].m_playerRootObj.SetActive(false);
			if (this.m_playerInfoGameObjects[j].m_playerText != null)
			{
				this.m_playerInfoGameObjects[j].m_playerText.enabled = false;
			}
			if (this.m_playerInfoGameObjects[j].m_playerRating != null)
			{
				this.m_playerInfoGameObjects[j].m_playerRating.gameObject.SetActive(false);
			}
			if (this.m_playerInfoGameObjects[j].m_activePlayerObject != null)
			{
				this.m_playerInfoGameObjects[j].m_activePlayerObject.SetActive(false);
			}
			if (this.m_playerInfoGameObjects[j].m_playerStatusIndicatorRoot != null)
			{
				this.m_playerInfoGameObjects[j].m_playerStatusIndicatorRoot.SetActive(false);
			}
		}
		this.m_save_struct = save_struct;
		this.m_type = slotType;
		this.m_state = UIP_GameSlot.ENetworkGameState.E_GAMESTATE_OFFLINE;
		this.m_GameID = (uint)save_struct.gameID;
		this.m_playerIDs[0] = save_struct.player1ID;
		this.m_playerIDs[1] = save_struct.player2ID;
		this.m_playerIDs[2] = save_struct.player3ID;
		this.m_playerIDs[3] = save_struct.player4ID;
		this.m_playerIDs[4] = save_struct.player5ID;
		this.m_playerIDs[5] = save_struct.player6ID;
		if (this.m_findGamePlayerCount != null)
		{
			uint num2 = save_struct.packedPlayerCount & 65535U;
			uint num3 = save_struct.packedPlayerCount >> 16 & 65535U;
			this.m_findGamePlayerCount.text = string.Format("{0}/{1}", num2, num3);
		}
		if (slotType == UIP_GameSlot.ESlotType.Active_Offline)
		{
			this.m_numPlayers = (int)save_struct.packedPlayerCount;
			int num4 = 0;
			while ((long)num4 < (long)((ulong)save_struct.packedPlayerCount))
			{
				if (num4 >= this.m_playerInfoGameObjects.Length)
				{
					break;
				}
				this.m_playerInfoGameObjects[num4].m_playerRootObj.SetActive(true);
				num4++;
			}
		}
		else
		{
			uint num5 = save_struct.packedPlayerCount >> 16 & 65535U;
			this.m_numPlayers = (int)num5;
			int num6 = 0;
			while ((long)num6 < (long)((ulong)num5) && num6 < 6)
			{
				this.m_playerInfoGameObjects[num6].m_playerRootObj.SetActive(true);
				num6++;
			}
			if (this.m_bNetworkInitialized)
			{
			}
		}
		if (slotType == UIP_GameSlot.ESlotType.Matchmaking)
		{
			uint num7 = save_struct.packedPlayerCount >> 16 & 65535U;
			this.m_matchmakingOpponentGameObjectIndex = (int)(num7 - 1U);
			if (this.m_playerInfoGameObjects[this.m_matchmakingOpponentGameObjectIndex].m_playerText != null)
			{
				this.m_playerInfoGameObjects[this.m_matchmakingOpponentGameObjectIndex].m_playerText.enabled = true;
			}
			this.m_matchmakingRatingDifference = save_struct.currentTurnPlayerIndex;
			this.m_matchmakingExpireTime = (this.m_matchmakingExpireTimeRemaining = save_struct.player1Timer);
			this.m_matchmakingTimerCoroutine = base.StartCoroutine(this.ProcessMatchmakingExpireTimer());
		}
		if (slotType == UIP_GameSlot.ESlotType.Active_Offline)
		{
			this.m_saveDataSize = save_struct.savedDataSize;
			this.m_worldDataVersion = save_struct.worldDataVersion;
		}
		else if (slotType == UIP_GameSlot.ESlotType.Available)
		{
			this.m_defaultColor = 3U;
			if (this.m_timer != null)
			{
				this.m_timer.enabled = true;
			}
			if (this.m_timerObj != null)
			{
				this.m_timerObj.SetActive(true);
			}
			this.m_timer.KeyText = PlayerTimers.GetTimeStringFromSeconds((int)save_struct.updateTime);
			this.m_localPlayerGameObjectsIndex = 0;
			for (int k = 0; k < this.m_playerIDs.Length; k++)
			{
				if (this.m_playerIDs[k] != 0 && (long)this.m_playerIDs[k] == (long)((ulong)this.m_localPlayerID))
				{
					this.m_localPlayerGameObjectsIndex = k;
					break;
				}
			}
			PlayerOnlineStatus status = (save_struct.player1Timer == 0U) ? PlayerOnlineStatus.STATUS_OFFLINE : PlayerOnlineStatus.STATUS_ONLINE;
			this.OnlineStatusIndicatorsSetActive(0, true);
			this.UpdatePlayerOnlineStatus(0, status);
			if (this.m_playerIDs[1] != 0)
			{
				status = ((save_struct.player2Timer == 0U) ? PlayerOnlineStatus.STATUS_OFFLINE : PlayerOnlineStatus.STATUS_ONLINE);
				this.OnlineStatusIndicatorsSetActive(1, true);
				this.UpdatePlayerOnlineStatus(1, status);
			}
			if (this.m_playerIDs[2] != 0)
			{
				status = ((save_struct.player3Timer == 0U) ? PlayerOnlineStatus.STATUS_OFFLINE : PlayerOnlineStatus.STATUS_ONLINE);
				this.OnlineStatusIndicatorsSetActive(2, true);
				this.UpdatePlayerOnlineStatus(2, status);
			}
			if (this.m_playerIDs[3] != 0)
			{
				status = ((save_struct.player4Timer == 0U) ? PlayerOnlineStatus.STATUS_OFFLINE : PlayerOnlineStatus.STATUS_ONLINE);
				this.OnlineStatusIndicatorsSetActive(3, true);
				this.UpdatePlayerOnlineStatus(3, status);
			}
			if (this.m_playerIDs[4] != 0)
			{
				status = ((save_struct.player5Timer == 0U) ? PlayerOnlineStatus.STATUS_OFFLINE : PlayerOnlineStatus.STATUS_ONLINE);
				this.OnlineStatusIndicatorsSetActive(4, true);
				this.UpdatePlayerOnlineStatus(4, status);
			}
			if (this.m_playerIDs[5] != 0)
			{
				status = ((save_struct.player6Timer == 0U) ? PlayerOnlineStatus.STATUS_OFFLINE : PlayerOnlineStatus.STATUS_ONLINE);
				this.OnlineStatusIndicatorsSetActive(5, true);
				this.UpdatePlayerOnlineStatus(5, status);
			}
		}
		else if (slotType == UIP_GameSlot.ESlotType.Matchmaking)
		{
			if (this.m_timer != null)
			{
				this.m_timer.enabled = true;
			}
			if (this.m_timerObj != null)
			{
				this.m_timerObj.SetActive(true);
			}
		}
		else if (slotType == UIP_GameSlot.ESlotType.Active_Online)
		{
			this.m_state = (UIP_GameSlot.ENetworkGameState)save_struct.gameState;
			this.m_bOwner = ((long)save_struct.soloGameCurrentScore == (long)((ulong)this.m_localPlayerID));
			this.m_localPlayerGameObjectsIndex = 0;
			for (int l = 0; l < 4; l++)
			{
				if (this.m_playerIDs[l] != 0 && (long)this.m_playerIDs[l] == (long)((ulong)this.m_localPlayerID))
				{
					this.m_localPlayerGameObjectsIndex = l;
					break;
				}
			}
			this.OnlineStatusIndicatorsSetActive(this.m_localPlayerGameObjectsIndex, false);
			this.UpdatePlayerOnlineStatus(this.m_localPlayerGameObjectsIndex, PlayerOnlineStatus.STATUS_ONLINE);
			if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_STAGING)
			{
				uint num8 = save_struct.packedPlayerCount & 65535U;
				uint num9 = save_struct.packedPlayerCount >> 16 & 65535U;
				this.m_bReadyToStart = (num8 >= num9);
				if (save_struct.player1ID == 0 || save_struct.player2ID == 0 || (num9 >= 3U && save_struct.player3ID == 0) || (num9 >= 4U && save_struct.player4ID == 0) || (num9 >= 5U && save_struct.player5ID == 0) || (num9 >= 6U && save_struct.player6ID == 0))
				{
					this.m_bReadyToStart = false;
				}
				if (save_struct.player1State == 2)
				{
					this.m_bReadyToStart = false;
					if ((long)save_struct.player1ID == (long)((ulong)this.m_localPlayerID))
					{
						this.m_bInvited = true;
					}
				}
				if (save_struct.player2State == 2)
				{
					this.m_bReadyToStart = false;
					if ((long)save_struct.player2ID == (long)((ulong)this.m_localPlayerID))
					{
						this.m_bInvited = true;
					}
				}
				if (save_struct.player3State == 2)
				{
					this.m_bReadyToStart = false;
					if ((long)save_struct.player3ID == (long)((ulong)this.m_localPlayerID))
					{
						this.m_bInvited = true;
					}
				}
				if (save_struct.player4State == 2)
				{
					this.m_bReadyToStart = false;
					if ((long)save_struct.player4ID == (long)((ulong)this.m_localPlayerID))
					{
						this.m_bInvited = true;
					}
				}
				if (save_struct.player5State == 2)
				{
					this.m_bReadyToStart = false;
					if ((long)save_struct.player5ID == (long)((ulong)this.m_localPlayerID))
					{
						this.m_bInvited = true;
					}
				}
				if (save_struct.player6State == 2)
				{
					this.m_bReadyToStart = false;
					if ((long)save_struct.player6ID == (long)((ulong)this.m_localPlayerID))
					{
						this.m_bInvited = true;
					}
				}
				if (this.m_timerObj != null)
				{
					this.m_timerObj.SetActive(true);
				}
				if (this.m_timer != null)
				{
					this.m_timer.enabled = true;
				}
			}
			else if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_PLAYING)
			{
				this.m_bDisplayPlayerTimers = true;
			}
		}
		string text = string.Empty;
		ushort num10 = 0;
		ushort num11 = 0;
		int num12 = 0;
		uint timeInSeconds = 0U;
		uint num13 = save_struct.packedPlayerCount & 15U;
		uint num14 = save_struct.packedPlayerCount >> 16 & 65535U;
		if (num14 == 0U)
		{
			num14 = num13;
		}
		int num15 = 0;
		while ((long)num15 < (long)((ulong)num14))
		{
			switch (num15)
			{
			case 0:
				text = save_struct.player1Name;
				num12 = save_struct.player1Faction;
				num10 = save_struct.player1Rating;
				num11 = save_struct.player1State;
				timeInSeconds = save_struct.player1Timer;
				break;
			case 1:
				text = save_struct.player2Name;
				num12 = save_struct.player2Faction;
				num10 = save_struct.player2Rating;
				num11 = save_struct.player2State;
				timeInSeconds = save_struct.player2Timer;
				break;
			case 2:
				text = save_struct.player3Name;
				num12 = save_struct.player3Faction;
				num10 = save_struct.player3Rating;
				num11 = save_struct.player3State;
				timeInSeconds = save_struct.player3Timer;
				break;
			case 3:
				text = save_struct.player4Name;
				num12 = save_struct.player4Faction;
				num10 = save_struct.player4Rating;
				num11 = save_struct.player4State;
				timeInSeconds = save_struct.player4Timer;
				break;
			case 4:
				text = save_struct.player5Name;
				num12 = save_struct.player5Faction;
				num10 = save_struct.player5Rating;
				num11 = save_struct.player5State;
				timeInSeconds = save_struct.player5Timer;
				break;
			case 5:
				text = save_struct.player6Name;
				num12 = save_struct.player6Faction;
				num10 = save_struct.player6Rating;
				num11 = save_struct.player6State;
				timeInSeconds = save_struct.player6Timer;
				break;
			}
			if (this.m_playerInfoGameObjects[num15].m_playerText != null)
			{
				this.m_playerInfoGameObjects[num15].m_playerText.enabled = true;
				this.m_playerInfoGameObjects[num15].m_playerText.text = text;
			}
			if (num12 >= 0 && num12 <= 6 && this.m_factionColors.Length > num12 && this.m_playerInfoGameObjects[num15].m_playerFactionImage != null)
			{
				this.m_playerInfoGameObjects[num15].m_playerFactionImage.color = this.m_factionColors[num12];
			}
			if (slotType != UIP_GameSlot.ESlotType.Active_Offline)
			{
				if (string.IsNullOrEmpty(text))
				{
					if (this.m_playerInfoGameObjects[num15].m_playerText != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_SlotOpen}");
					}
					if (this.m_playerInfoGameObjects[num15].m_playerRating != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerRating.gameObject.SetActive(false);
					}
					if (this.m_playerInfoGameObjects[num15].m_playerTimerObj != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerTimerObj.SetActive(false);
					}
					if (this.m_playerInfoGameObjects[num15].m_playerRatingObj != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerRatingObj.SetActive(false);
					}
					this.UpdatePlayerOnlineStatus(num15, PlayerOnlineStatus.STATUS_UNKNOWN);
					if (this.m_playerInfoGameObjects[num15].m_playerFactionImage != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerFactionImage.gameObject.SetActive(false);
					}
				}
				else
				{
					if (this.m_bDisplayPlayerTimers)
					{
						if (this.m_playerInfoGameObjects[num15].m_playerRating != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerRating.gameObject.SetActive(true);
						}
						if (this.m_playerInfoGameObjects[num15].m_playerTimerObj != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerTimerObj.SetActive(false);
						}
						if (this.m_playerInfoGameObjects[num15].m_playerRatingObj != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerRatingObj.SetActive(false);
						}
						if (this.m_playerInfoGameObjects[num15].m_playerRating != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerRating.text = this.GetTimeStringFromSeconds(timeInSeconds, true);
						}
					}
					else
					{
						if (this.m_playerInfoGameObjects[num15].m_playerRating != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerRating.gameObject.SetActive(true);
						}
						if (this.m_playerInfoGameObjects[num15].m_playerTimerObj != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerTimerObj.SetActive(false);
						}
						if (this.m_playerInfoGameObjects[num15].m_playerRatingObj != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerRatingObj.SetActive(true);
						}
						if (this.m_playerInfoGameObjects[num15].m_playerRating != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerRating.text = num10.ToString();
						}
					}
					if (num11 == 4)
					{
						if (this.m_playerInfoGameObjects[num15].m_playerText != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerText.color = this.m_forfeitTextColor;
						}
						if (this.m_playerInfoGameObjects[num15].m_playerRating != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerRating.color = this.m_forfeitTextColor;
							this.m_playerInfoGameObjects[num15].m_playerRating.text = "";
						}
						if (this.m_playerInfoGameObjects[num15].m_playerTimerObj != null)
						{
							this.m_playerInfoGameObjects[num15].m_playerTimerObj.SetActive(false);
						}
					}
					else if (slotType != UIP_GameSlot.ESlotType.Available && num11 == 2 && this.m_playerInfoGameObjects[num15].m_playerText != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerText.color = this.m_waitingOnInviteColor;
					}
				}
			}
			else
			{
				if (this.m_playerInfoGameObjects[num15].m_playerRating != null)
				{
					this.m_playerInfoGameObjects[num15].m_playerRating.gameObject.SetActive(false);
				}
				if (this.m_playerInfoGameObjects[num15].m_playerTimerObj != null)
				{
					this.m_playerInfoGameObjects[num15].m_playerTimerObj.SetActive(false);
				}
				if (this.m_playerInfoGameObjects[num15].m_playerRatingObj != null)
				{
					this.m_playerInfoGameObjects[num15].m_playerRatingObj.SetActive(false);
				}
			}
			if ((save_struct.decisionPlayerFlags & 1 << num15) != 0)
			{
				if (this.m_playerInfoGameObjects[num15].m_activePlayerObject != null)
				{
					this.m_playerInfoGameObjects[num15].m_activePlayerObject.SetActive(true);
				}
				if (slotType == UIP_GameSlot.ESlotType.Active_Online)
				{
					if (this.m_playerInfoGameObjects[num15].m_activePlayerObject != null)
					{
						this.m_playerInfoGameObjects[num15].m_activePlayerObject.SetActive(false);
					}
					if (this.m_playerInfoGameObjects[num15].m_playerTimerObj != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerTimerObj.SetActive(true);
					}
				}
				if ((long)this.m_playerIDs[num15] == (long)((ulong)this.m_localPlayerID) && slotType == UIP_GameSlot.ESlotType.Active_Online)
				{
					if (this.m_playerInfoGameObjects[num15].m_playerText != null)
					{
						this.m_playerInfoGameObjects[num15].m_playerText.color = this.m_localPlayerActiveTurnColor;
					}
				}
				else if (slotType == UIP_GameSlot.ESlotType.Active_Online && this.m_playerInfoGameObjects[num15].m_playerText != null)
				{
					this.m_playerInfoGameObjects[num15].m_playerText.color = this.m_nonLocalPlayerActiveTurnColor;
				}
			}
			num15++;
		}
		if (this.m_type == UIP_GameSlot.ESlotType.Active_Online || this.m_type == UIP_GameSlot.ESlotType.Active_Offline)
		{
			int roundNumber = save_struct.roundNumber;
			this.UpdateTurnText(roundNumber);
		}
		this.UpdateStatusText(save_struct.roundNumber);
		if (this.m_newChatIcon != null)
		{
			this.m_newChatIcon.SetActive(slotType == UIP_GameSlot.ESlotType.Active_Online && AgricolaLib.NetworkGetChatPosition(this.m_GameID) < AgricolaLib.NetworkGetLastChatMessageIndex(this.m_GameID));
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00041ABC File Offset: 0x0003FCBC
	public void TurnOnDeleteMode(bool bDeleteModeOn)
	{
		if (bDeleteModeOn)
		{
			if (this.m_colorizer != null)
			{
				this.m_colorizer.Colorize(1U);
			}
		}
		else
		{
			if (this.m_colorizer != null)
			{
				this.m_colorizer.Colorize(0U);
			}
			this.m_baseButton.enabled = false;
			this.m_baseButton.enabled = true;
		}
		this.m_bDeleteMode = bDeleteModeOn;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00003022 File Offset: 0x00001222
	public void TurnOnSelectedMode(bool bSelectModeOn)
	{
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00041B24 File Offset: 0x0003FD24
	private void UpdateTurnText(int turnNumber)
	{
		if (this.m_roundText != null)
		{
			this.m_roundText.enabled = true;
			if (turnNumber == 0)
			{
				turnNumber = 1;
			}
			this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}") + " " + turnNumber.ToString();
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x00041B7C File Offset: 0x0003FD7C
	private void UpdateStatusText(int roundNumber)
	{
		if (this.m_roundText == null)
		{
			return;
		}
		this.m_roundText.enabled = true;
		if (this.m_type == UIP_GameSlot.ESlotType.Available)
		{
			this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_JoinGame}");
			return;
		}
		if (this.m_type != UIP_GameSlot.ESlotType.Matchmaking)
		{
			if (this.m_type == UIP_GameSlot.ESlotType.Active_Online)
			{
				this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_MatchMaking}");
				if (this.m_bDeleteMode)
				{
					if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_STAGING)
					{
						if (this.m_bOwner)
						{
							this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Delete}");
							return;
						}
						this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Withdraw}");
						return;
					}
					else if (this.m_state != UIP_GameSlot.ENetworkGameState.E_GAMESTATE_COMPLETED)
					{
						this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Forfeit}");
						return;
					}
				}
				else if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_STAGING)
				{
					this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_WaitingPlayers}");
					if (this.m_bReadyToStart)
					{
						if (this.m_bOwner)
						{
							this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_StartGame}");
						}
						else
						{
							this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_WaitingHost}");
						}
					}
					if (this.m_bInvited)
					{
						this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Accept}");
						return;
					}
				}
				else
				{
					if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_PLAYING)
					{
						if (roundNumber <= 0)
						{
							roundNumber = 1;
						}
						this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}") + " " + roundNumber.ToString();
						return;
					}
					if (this.m_state == UIP_GameSlot.ENetworkGameState.E_GAMESTATE_COMPLETED)
					{
						this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Completed}");
						return;
					}
				}
			}
			else
			{
				UIP_GameSlot.ESlotType type = this.m_type;
			}
			return;
		}
		if (this.m_bDeleteMode)
		{
			this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_MatchMaking}");
			return;
		}
		this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_MatchMaking}");
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00041DA4 File Offset: 0x0003FFA4
	public bool GetIsHostOnline()
	{
		if (this.m_playerInfoGameObjects[0].m_playerStatusIndicatorImage != null)
		{
			return this.m_playerInfoGameObjects[0].m_playerStatusIndicatorImage.sprite == this.m_OnlineStatusImages[3];
		}
		return this.m_playerInfoGameObjects[0].m_playerStatusIndicatorObjs != null && this.m_playerInfoGameObjects[0].m_playerStatusIndicatorObjs.Length >= 4 && this.m_playerInfoGameObjects[0].m_playerStatusIndicatorObjs[3] != null && this.m_playerInfoGameObjects[0].m_playerStatusIndicatorObjs[3].activeSelf;
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00041E4C File Offset: 0x0004004C
	private void OnlineStatusIndicatorsSetActive(int playerGameObjectIndex, bool bActive)
	{
		if (this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorRoot != null)
		{
			this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorRoot.SetActive(bActive);
		}
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00041E80 File Offset: 0x00040080
	private void UpdatePlayerOnlineStatus(int playerGameObjectIndex, PlayerOnlineStatus status)
	{
		if (this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorImage != null)
		{
			this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorImage.sprite = this.m_OnlineStatusImages[(int)status];
			return;
		}
		if (this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorObjs != null && this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorObjs.Length >= 4)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorObjs[i] != null)
				{
					this.m_playerInfoGameObjects[playerGameObjectIndex].m_playerStatusIndicatorObjs[i].SetActive(status == (PlayerOnlineStatus)i);
				}
			}
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00041F34 File Offset: 0x00040134
	private void UpdatePlayerTimers()
	{
		foreach (GamePlayerTimer gamePlayerTimer in Network.GetGamePlayerTimers(this.m_GameID))
		{
			for (int i = 0; i < this.m_playerInfoGameObjects.Length; i++)
			{
				if ((ulong)gamePlayerTimer.userID == (ulong)((long)this.m_playerIDs[i]) && this.m_playerInfoGameObjects[i].m_playerTimerObj != null && this.m_playerInfoGameObjects[i].m_playerRating != null)
				{
					if (gamePlayerTimer.timerHours >= 24)
					{
						uint num = (uint)(gamePlayerTimer.timerHours / 24);
						this.m_playerInfoGameObjects[i].m_playerRating.text = num.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((num > 1U) ? "${Key_Days}" : "${Key_Day}");
					}
					else if (gamePlayerTimer.timerHours > 0)
					{
						TMP_Text playerRating = this.m_playerInfoGameObjects[i].m_playerRating;
						ushort num2 = gamePlayerTimer.timerHours;
						playerRating.text = num2.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((gamePlayerTimer.timerHours > 1) ? "${Key_Hours}" : "${Key_Hour}");
					}
					else if (gamePlayerTimer.timerMinutes > 0)
					{
						TMP_Text playerRating2 = this.m_playerInfoGameObjects[i].m_playerRating;
						ushort num2 = gamePlayerTimer.timerMinutes;
						playerRating2.text = num2.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((gamePlayerTimer.timerMinutes > 1) ? "${Key_Minutes}" : "${Key_Minute}");
					}
					else
					{
						TMP_Text playerRating3 = this.m_playerInfoGameObjects[i].m_playerRating;
						ushort num2 = gamePlayerTimer.timerSeconds;
						playerRating3.text = num2.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((gamePlayerTimer.timerSeconds > 1) ? "${Key_Seconds}" : "${Key_Second}");
					}
				}
			}
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0004215C File Offset: 0x0004035C
	private void UpdateMatchmakingTimer()
	{
		string text = (this.m_matchmakingRatingDifference > 0) ? ("+/-" + this.m_matchmakingRatingDifference.ToString()) : LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Ranking1}");
		string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Expires}") + " " + this.GetTimeStringFromSeconds(this.m_matchmakingExpireTimeRemaining, false);
		if (this.m_playerInfoGameObjects[this.m_matchmakingOpponentGameObjectIndex].m_playerText != null)
		{
			this.m_playerInfoGameObjects[this.m_matchmakingOpponentGameObjectIndex].m_playerText.text = text2;
		}
		if (this.m_playerInfoGameObjects[this.m_matchmakingOpponentGameObjectIndex].m_playerRating != null)
		{
			this.m_playerInfoGameObjects[this.m_matchmakingOpponentGameObjectIndex].m_playerRating.text = text;
		}
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00042234 File Offset: 0x00040434
	private IEnumerator ProcessMatchmakingExpireTimer()
	{
		float accumulatedTime = 0f;
		float previousTime = Time.time;
		while (this.m_matchmakingExpireTimeRemaining > 0U)
		{
			float time = Time.time;
			accumulatedTime += time - previousTime;
			previousTime = time;
			this.m_matchmakingExpireTimeRemaining = this.m_matchmakingExpireTime - (uint)accumulatedTime;
			if (this.m_matchmakingExpireTimeRemaining < 0U)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		this.m_matchmakingExpireTimeRemaining = 0U;
		yield break;
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00042244 File Offset: 0x00040444
	private string GetTimeStringFromSeconds(uint timeInSeconds, bool bShortHand)
	{
		uint num = timeInSeconds % 60U;
		uint num2 = timeInSeconds / 60U;
		uint num3 = num2 / 60U;
		num2 %= 60U;
		string result;
		if (bShortHand)
		{
			if (num3 >= 24U)
			{
				uint num4 = num3 / 24U;
				result = num4.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((num4 > 1U) ? "${Key_Days}" : "${Key_Day}");
			}
			else if (num3 > 0U)
			{
				result = num3.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((num3 > 1U) ? "${Key_Hours}" : "${Key_Hour}");
			}
			else if (num2 > 0U)
			{
				result = num2.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((num2 > 1U) ? "${Key_Minutes}" : "${Key_Minute}");
			}
			else
			{
				result = num.ToString() + " " + LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder((num > 1U) ? "${Key_Seconds}" : "${Key_Second}");
			}
		}
		else if (num3 >= 24U)
		{
			uint num5 = num3 / 24U;
			uint num6 = num3 - num5 * 24U;
			result = string.Concat(new object[]
			{
				num5,
				"d ",
				num6,
				"h"
			});
		}
		else
		{
			result = string.Concat(new string[]
			{
				num3.ToString().PadLeft(2, '0'),
				":",
				num2.ToString().PadLeft(2, '0'),
				":",
				num.ToString().PadLeft(2, '0')
			});
		}
		return result;
	}

	// Token: 0x04000A2D RID: 2605
	public Button m_baseButton;

	// Token: 0x04000A2E RID: 2606
	public UIP_GameSlot.PlayerInfoGameObjects[] m_playerInfoGameObjects;

	// Token: 0x04000A2F RID: 2607
	public GameObject m_OnlineStatusIndicatorPrefab;

	// Token: 0x04000A30 RID: 2608
	public GameObject m_timerObj;

	// Token: 0x04000A31 RID: 2609
	public UILocalizedText m_timer;

	// Token: 0x04000A32 RID: 2610
	public TextMeshProUGUI m_roundText;

	// Token: 0x04000A33 RID: 2611
	public Color m_forfeitTextColor;

	// Token: 0x04000A34 RID: 2612
	public Color m_localPlayerActiveTurnColor = Color.green;

	// Token: 0x04000A35 RID: 2613
	public Color m_nonLocalPlayerActiveTurnColor = Color.cyan;

	// Token: 0x04000A36 RID: 2614
	public Color m_waitingOnInviteColor = Color.yellow;

	// Token: 0x04000A37 RID: 2615
	public Color[] m_factionColors;

	// Token: 0x04000A38 RID: 2616
	public Image m_gameTypeImage;

	// Token: 0x04000A39 RID: 2617
	public Sprite[] m_gameTypeSprites;

	// Token: 0x04000A3A RID: 2618
	public UIP_GameSlot.SetIcon[] m_setIcons;

	// Token: 0x04000A3B RID: 2619
	public Sprite[] m_OnlineStatusImages;

	// Token: 0x04000A3C RID: 2620
	public GameObject m_newChatIcon;

	// Token: 0x04000A3D RID: 2621
	public TextMeshProUGUI m_findGamePlayerCount;

	// Token: 0x04000A3E RID: 2622
	public RectTransform m_RectTransform;

	// Token: 0x04000A3F RID: 2623
	public ColorByFaction m_colorizer;

	// Token: 0x04000A40 RID: 2624
	private Network m_network;

	// Token: 0x04000A41 RID: 2625
	private UIP_GameSlot.ClickCallback m_callback;

	// Token: 0x04000A42 RID: 2626
	private Coroutine m_matchmakingTimerCoroutine;

	// Token: 0x04000A43 RID: 2627
	private ShortSaveStruct m_save_struct;

	// Token: 0x04000A44 RID: 2628
	private UIP_GameSlot.ESlotType m_type;

	// Token: 0x04000A45 RID: 2629
	private UIP_GameSlot.ENetworkGameState m_state;

	// Token: 0x04000A46 RID: 2630
	private string m_savePathShort;

	// Token: 0x04000A47 RID: 2631
	private string m_savePathFull;

	// Token: 0x04000A48 RID: 2632
	private uint m_GameID;

	// Token: 0x04000A49 RID: 2633
	private int m_gameSlotIndex;

	// Token: 0x04000A4A RID: 2634
	private uint m_colors;

	// Token: 0x04000A4B RID: 2635
	private int m_saveDataSize;

	// Token: 0x04000A4C RID: 2636
	private int m_worldDataVersion;

	// Token: 0x04000A4D RID: 2637
	private int m_numPlayers;

	// Token: 0x04000A4E RID: 2638
	private int[] m_playerIDs;

	// Token: 0x04000A4F RID: 2639
	private int m_localPlayerGameObjectsIndex;

	// Token: 0x04000A50 RID: 2640
	private int m_matchmakingOpponentGameObjectIndex;

	// Token: 0x04000A51 RID: 2641
	private int m_matchmakingRatingDifference;

	// Token: 0x04000A52 RID: 2642
	private uint m_matchmakingExpireTime;

	// Token: 0x04000A53 RID: 2643
	private uint m_matchmakingExpireTimeRemaining;

	// Token: 0x04000A54 RID: 2644
	private uint m_localPlayerID;

	// Token: 0x04000A55 RID: 2645
	private uint m_defaultColor;

	// Token: 0x04000A56 RID: 2646
	private bool m_bDeleteMode;

	// Token: 0x04000A57 RID: 2647
	private bool m_bOwner;

	// Token: 0x04000A58 RID: 2648
	private bool m_bReadyToStart;

	// Token: 0x04000A59 RID: 2649
	private bool m_bInvited;

	// Token: 0x04000A5A RID: 2650
	private bool m_bDisplayPlayerTimers;

	// Token: 0x04000A5B RID: 2651
	private bool m_bNeedsOnlineStatusUpdate;

	// Token: 0x04000A5C RID: 2652
	private bool m_bNetworkInitialized;

	// Token: 0x020007CE RID: 1998
	public enum ClickEventType
	{
		// Token: 0x04002CFF RID: 11519
		Evt_Click_Unknown,
		// Token: 0x04002D00 RID: 11520
		Evt_Click_LoadOfflineGame,
		// Token: 0x04002D01 RID: 11521
		Evt_Click_DeleteOfflineGame,
		// Token: 0x04002D02 RID: 11522
		Evt_Click_NetworkLaunchGame,
		// Token: 0x04002D03 RID: 11523
		Evt_Click_NetworkAcceptInvite,
		// Token: 0x04002D04 RID: 11524
		Evt_Click_NetworkResumeGame,
		// Token: 0x04002D05 RID: 11525
		Evt_Click_NetworkJoinGame,
		// Token: 0x04002D06 RID: 11526
		Evt_Click_NetworkDeleteGame,
		// Token: 0x04002D07 RID: 11527
		Evt_Click_NetworkForfeitGame,
		// Token: 0x04002D08 RID: 11528
		Evt_Click_NetworkForfeitGameLastPlayer,
		// Token: 0x04002D09 RID: 11529
		Evt_Click_NetworkWithdrawFromGame,
		// Token: 0x04002D0A RID: 11530
		Evt_Click_NetworkDeleteMatchmakingGame
	}

	// Token: 0x020007CF RID: 1999
	public enum ESlotType
	{
		// Token: 0x04002D0C RID: 11532
		Active_Offline,
		// Token: 0x04002D0D RID: 11533
		Active_Online,
		// Token: 0x04002D0E RID: 11534
		Available,
		// Token: 0x04002D0F RID: 11535
		Matchmaking
	}

	// Token: 0x020007D0 RID: 2000
	public enum ENetworkGameState
	{
		// Token: 0x04002D11 RID: 11537
		E_GAMESTATE_OFFLINE,
		// Token: 0x04002D12 RID: 11538
		E_GAMESTATE_STAGING,
		// Token: 0x04002D13 RID: 11539
		E_GAMESTATE_PLAYING,
		// Token: 0x04002D14 RID: 11540
		E_GAMESTATE_COMPLETED,
		// Token: 0x04002D15 RID: 11541
		E_GAMESTATE_ARCHIVED
	}

	// Token: 0x020007D1 RID: 2001
	public enum ENetworkUserState
	{
		// Token: 0x04002D17 RID: 11543
		E_USERSTATE_OFFLINE,
		// Token: 0x04002D18 RID: 11544
		E_USERSTATE_READY,
		// Token: 0x04002D19 RID: 11545
		E_USERSTATE_INVITED,
		// Token: 0x04002D1A RID: 11546
		E_USERSTATE_PLAYING,
		// Token: 0x04002D1B RID: 11547
		E_USERSTATE_FORFEIT,
		// Token: 0x04002D1C RID: 11548
		E_USERSTATE_FINISHED
	}

	// Token: 0x020007D2 RID: 2002
	public enum PlayerSide
	{
		// Token: 0x04002D1E RID: 11550
		E_PLAYERSIDE_UNKNOWN,
		// Token: 0x04002D1F RID: 11551
		E_PLAYERSIDE_USSR,
		// Token: 0x04002D20 RID: 11552
		E_PLAYERSIDE_US
	}

	// Token: 0x020007D3 RID: 2003
	// (Invoke) Token: 0x0600433B RID: 17211
	public delegate void ClickCallback(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt);

	// Token: 0x020007D4 RID: 2004
	[Serializable]
	public struct PlayerInfoGameObjects
	{
		// Token: 0x04002D21 RID: 11553
		public GameObject m_playerRootObj;

		// Token: 0x04002D22 RID: 11554
		public TextMeshProUGUI m_playerText;

		// Token: 0x04002D23 RID: 11555
		public TextMeshProUGUI m_playerRating;

		// Token: 0x04002D24 RID: 11556
		public GameObject m_playerRatingObj;

		// Token: 0x04002D25 RID: 11557
		public GameObject m_playerTimerObj;

		// Token: 0x04002D26 RID: 11558
		public GameObject m_activePlayerObject;

		// Token: 0x04002D27 RID: 11559
		public GameObject m_playerStatusIndicatorRoot;

		// Token: 0x04002D28 RID: 11560
		public Image m_playerFactionImage;

		// Token: 0x04002D29 RID: 11561
		public Image m_playerStatusIndicatorImage;

		// Token: 0x04002D2A RID: 11562
		public GameObject[] m_playerStatusIndicatorObjs;
	}

	// Token: 0x020007D5 RID: 2005
	[Serializable]
	public struct SetIcon
	{
		// Token: 0x04002D2B RID: 11563
		public GameObject root;

		// Token: 0x04002D2C RID: 11564
		public GameObject lockIcon;
	}
}
