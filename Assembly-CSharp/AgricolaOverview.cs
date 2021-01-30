using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000057 RID: 87
public class AgricolaOverview : MonoBehaviour
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x00025F75 File Offset: 0x00024175
	public void ResetMagnifyManager()
	{
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		component.GetMagnifyManager().SetOverridePanelObject(this.m_magnifyPanel);
		component.GetMagnifyManager().SetUseOverrideLayer(true);
		component.GetMagnifyManager().SetOverrideLayerObject(this.m_animationLayer);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00025FB4 File Offset: 0x000241B4
	public void Enter()
	{
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		if (this.m_cardManager == null)
		{
			Debug.LogError("No connection to card manager or upper HUD");
			return;
		}
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		component.GetMagnifyManager().SetOverridePanelObject(this.m_magnifyPanel);
		component.GetMagnifyManager().SetUseOverrideLayer(true);
		component.GetMagnifyManager().SetOverrideLayerObject(this.m_animationLayer);
		int playerIndex = AgricolaLib.GetLocalPlayerIndex();
		this.m_numPlayers = 0;
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			this.m_playerSlots[i].playerIndex = playerIndex;
			if (this.m_playerSlots[i].playerIndex != 0)
			{
				foreach (GameObject gameObject in this.m_playerSlots[i].isActiveNodes)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(true);
					}
				}
				AgricolaLib.GetGamePlayerState(playerIndex, this.m_bufPtr, 1024);
				GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
				this.m_playerSlots[i].tokenColorizer.Colorize((uint)gamePlayerState.playerFaction);
				this.m_playerSlots[i].bkgColorizer.Colorize((uint)gamePlayerState.playerFaction);
				this.m_playerSlots[i].playerName.text = gamePlayerState.displayName;
				this.m_playerSlots[i].avatar.SetAvatar(gamePlayerState.playerAvatar, true);
				this.m_playerSlots[i].playerFaction = gamePlayerState.playerFaction;
				this.m_playerSlots[i].logSelectedPlayer = true;
				this.m_numPlayers++;
			}
			else
			{
				foreach (GameObject gameObject2 in this.m_playerSlots[i].isActiveNodes)
				{
					if (gameObject2 != null)
					{
						gameObject2.SetActive(false);
					}
				}
				this.m_playerSlots[i].bkgColorizer.Colorize(6U);
			}
			playerIndex = AgricolaLib.GetLocalOpponentPlayerIndex(i + 1);
		}
		this.CheckToggles();
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0002620C File Offset: 0x0002440C
	public void Exit()
	{
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			if (this.m_playerSlots[i].overviewCell != null)
			{
				this.m_playerSlots[i].overviewCell.Exit();
			}
		}
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		component.GetMagnifyManager().SetOverridePanelObject(null);
		component.GetMagnifyManager().SetUseOverrideLayer(false);
		component.GetMagnifyManager().SetOverrideLayerObject(null);
		for (int j = 0; j < this.m_calendarCells.Count; j++)
		{
			this.m_calendarCells[j].Exit();
		}
		for (int k = 0; k < this.m_createdObjs.Count; k++)
		{
			if (this.m_createdObjs[k] != null)
			{
				UnityEngine.Object.Destroy(this.m_createdObjs[k]);
			}
		}
		this.m_createdObjs.Clear();
		this.m_logActionCells.Clear();
		this.m_currentMode = AgricolaOverview.OverviewMode.None;
		this.m_bVisitedOverview = false;
		this.m_bVisitedCalendar = false;
		this.m_bVisitedLog = false;
		this.m_hDataBuffer.Free();
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0002632C File Offset: 0x0002452C
	private void Update()
	{
		if (this.m_loadingScreen.activeSelf)
		{
			this.m_currentLoadingTime += Time.deltaTime;
			if (this.m_bLoadComplete && this.m_currentLoadingTime >= this.m_MinimumLoadingTime)
			{
				this.m_logZoomInRoot.SetActive(this.m_bLogZoomedIn);
				this.m_logZoomOutRoot.SetActive(!this.m_bLogZoomedIn);
				this.SetPlayerSlotTogglesAvailable(this.m_bLogZoomedIn);
				this.UpdateLogCellVisibility();
				this.m_loadingScreen.SetActive(false);
				this.m_bLoadComplete = false;
			}
		}
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x000263B8 File Offset: 0x000245B8
	public void CheckToggles()
	{
		int num = 0;
		AgricolaOverview.OverviewMode overviewMode = AgricolaOverview.OverviewMode.None;
		if (this.m_overviewToggle.isOn)
		{
			overviewMode = AgricolaOverview.OverviewMode.Overview;
			num++;
		}
		if (this.m_calenderToggle.isOn)
		{
			overviewMode = AgricolaOverview.OverviewMode.Calendar;
			num++;
		}
		if (this.m_logToggle.isOn)
		{
			overviewMode = AgricolaOverview.OverviewMode.Log;
			num++;
		}
		if (num == 1)
		{
			if (overviewMode == AgricolaOverview.OverviewMode.Overview)
			{
				this.SetToOverviewMode();
				return;
			}
			if (overviewMode == AgricolaOverview.OverviewMode.Calendar)
			{
				this.SetToCalendarMode();
				return;
			}
			if (overviewMode == AgricolaOverview.OverviewMode.Log)
			{
				this.SetToLogMode();
			}
		}
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00026428 File Offset: 0x00024628
	private void SetToOverviewMode()
	{
		if (this.m_currentMode == AgricolaOverview.OverviewMode.Overview)
		{
			return;
		}
		this.SetPlayerSlotTogglesAvailable(false);
		this.m_currentMode = AgricolaOverview.OverviewMode.Overview;
		this.m_titleCard.KeyText = "${Key_Overview}";
		if (this.m_overviewRoot != null)
		{
			this.m_overviewRoot.SetActive(true);
		}
		if (this.m_calendarRoot != null)
		{
			this.m_calendarRoot.SetActive(false);
		}
		if (this.m_logRoot != null)
		{
			this.m_logRoot.SetActive(false);
		}
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			if (this.m_playerSlots[i].playerIndex == 0)
			{
				if (this.m_playerSlots[i].overviewCell != null)
				{
					this.m_playerSlots[i].overviewCell.gameObject.SetActive(false);
				}
			}
			else
			{
				if (this.m_playerSlots[i].overviewCell == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_overviewPrefab);
					this.m_playerSlots[i].overviewCell = gameObject.GetComponent<AgricolaOverview_OverviewCell>();
					gameObject.transform.SetParent(this.m_playerSlots[i].overviewCellLocator.transform);
					gameObject.transform.localPosition = Vector3.zero;
					RectTransform component = gameObject.GetComponent<RectTransform>();
					component.offsetMin = Vector2.zero;
					component.offsetMax = Vector2.zero;
					gameObject.transform.localScale = Vector3.one;
				}
				this.m_playerSlots[i].overviewCell.gameObject.SetActive(true);
				if (!this.m_bVisitedOverview)
				{
					this.m_playerSlots[i].overviewCell.SetupDisplay(this.m_playerSlots[i].playerIndex, this.m_cardManager);
				}
			}
		}
		this.m_bVisitedOverview = true;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00026604 File Offset: 0x00024804
	private void SetToCalendarMode()
	{
		if (this.m_currentMode == AgricolaOverview.OverviewMode.Calendar)
		{
			return;
		}
		this.SetPlayerSlotTogglesAvailable(false);
		this.m_currentMode = AgricolaOverview.OverviewMode.Calendar;
		this.m_titleCard.KeyText = "${Key_Calendar}";
		if (this.m_overviewRoot != null)
		{
			this.m_overviewRoot.SetActive(false);
		}
		if (this.m_calendarRoot != null)
		{
			this.m_calendarRoot.SetActive(true);
		}
		if (this.m_logRoot != null)
		{
			this.m_logRoot.SetActive(false);
		}
		if (!this.m_bVisitedCalendar)
		{
			if (this.m_calendarCells.Count == 0)
			{
				for (int i = 2; i <= 14; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_calendarPrefab);
					gameObject.transform.SetParent(this.m_calendarLocator.transform);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					AgricolaOverview_CalendarCell component = gameObject.GetComponent<AgricolaOverview_CalendarCell>();
					this.m_calendarCells.Add(component);
					component.SetupCell(i, this.m_numPlayers);
				}
			}
			int currentRound = AgricolaLib.GetCurrentRound();
			for (int j = 0; j < this.m_numPlayers; j++)
			{
				for (int k = 0; k < this.m_calendarCells.Count; k++)
				{
					if (this.m_calendarCells[k].GetRoundNumber() > currentRound)
					{
						this.m_calendarCells[k].SetCalendarData(this.m_playerSlots[j].playerIndex, j, this.m_cardManager);
					}
				}
			}
		}
		this.m_bVisitedCalendar = true;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00026784 File Offset: 0x00024984
	private void SetToLogMode()
	{
		if (this.m_currentMode == AgricolaOverview.OverviewMode.Log)
		{
			return;
		}
		this.SetPlayerSlotTogglesAvailable(false);
		this.m_currentMode = AgricolaOverview.OverviewMode.Log;
		this.m_titleCard.KeyText = "${Key_GameLog}";
		if (this.m_overviewRoot != null)
		{
			this.m_overviewRoot.SetActive(false);
		}
		if (this.m_calendarRoot != null)
		{
			this.m_calendarRoot.SetActive(false);
		}
		if (this.m_logRoot != null)
		{
			this.m_logRoot.SetActive(true);
		}
		if (!this.m_bVisitedLog)
		{
			this.m_logZoomInRoot.SetActive(false);
			this.m_logZoomOutRoot.SetActive(false);
			this.m_bLoadComplete = false;
			this.m_currentLoadingTime = 0f;
			this.m_loadingScreen.SetActive(true);
			base.StartCoroutine(this.LoadLogAsync());
		}
		else
		{
			this.m_logZoomInRoot.SetActive(this.m_bLogZoomedIn);
			this.m_logZoomOutRoot.SetActive(!this.m_bLogZoomedIn);
			this.SetPlayerSlotTogglesAvailable(this.m_bLogZoomedIn);
			this.UpdateLogCellVisibility();
		}
		this.m_bVisitedLog = true;
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00026894 File Offset: 0x00024A94
	private void SetPlayerSlotTogglesAvailable(bool bAvailable)
	{
		this.m_bIgnoreToggles = true;
		for (int i = 0; i < this.m_playerSlots.Length; i++)
		{
			this.m_playerSlots[i].toggle.isOn = (bAvailable && this.m_playerSlots[i].logSelectedPlayer);
			this.m_playerSlots[i].toggle.interactable = bAvailable;
		}
		this.m_bIgnoreToggles = false;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00026908 File Offset: 0x00024B08
	public void OnDoubleClick(BaseEventData baseData)
	{
		if (((PointerEventData)baseData).clickCount != 2 || !this.m_logRoot.activeSelf)
		{
			return;
		}
		this.m_bLogZoomedIn = !this.m_bLogZoomedIn;
		this.m_logZoomInRoot.SetActive(this.m_bLogZoomedIn);
		this.m_logZoomOutRoot.SetActive(!this.m_bLogZoomedIn);
		this.SetPlayerSlotTogglesAvailable(this.m_bLogZoomedIn);
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00026974 File Offset: 0x00024B74
	public void OnPlayerTokenToggle(int index)
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		this.m_playerSlots[index].logSelectedPlayer = this.m_playerSlots[index].toggle.isOn;
		int num = 0;
		for (int i = 0; i < this.m_numPlayers; i++)
		{
			if (this.m_playerSlots[i].logSelectedPlayer)
			{
				num++;
			}
		}
		if (num == 0)
		{
			this.m_bIgnoreToggles = true;
			this.m_playerSlots[index].toggle.isOn = true;
			this.m_playerSlots[index].logSelectedPlayer = true;
			this.m_bIgnoreToggles = false;
			return;
		}
		this.UpdateLogCellVisibility();
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00026A1C File Offset: 0x00024C1C
	private void UpdateLogCellVisibility()
	{
		AgricolaOverview_LogInAction agricolaOverview_LogInAction = null;
		for (int i = 0; i < this.m_logActionCells.Count; i++)
		{
			int lineIndex = this.m_logActionCells[i].GetLineIndex();
			if (!this.m_playerSlots[lineIndex].logSelectedPlayer)
			{
				this.m_logActionCells[i].gameObject.SetActive(false);
			}
			else
			{
				this.m_logActionCells[i].gameObject.SetActive(true);
				if (agricolaOverview_LogInAction == null || agricolaOverview_LogInAction.GetPlayerIndex() != this.m_logActionCells[i].GetPlayerIndex() || agricolaOverview_LogInAction.GetRoundNumber() != this.m_logActionCells[i].GetRoundNumber())
				{
					this.m_logActionCells[i].SetTokenVisible(true);
				}
				else
				{
					this.m_logActionCells[i].SetTokenVisible(false);
				}
				agricolaOverview_LogInAction = this.m_logActionCells[i];
			}
		}
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00026B0D File Offset: 0x00024D0D
	private IEnumerator LoadLogAsync()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		int currentRound = -1;
		int logEntryCount = AgricolaLib.GetGameTurnLogCount();
		List<AgricolaOverview_LogOutColumn> zoomOutColumns = new List<AgricolaOverview_LogOutColumn>();
		int lastZoomedOutInstanceID = 0;
		string lastZoomedOutCardName = string.Empty;
		string empty = string.Empty;
		AgricolaOverview_LogInAction lastZoomedInCell = null;
		int num3;
		for (int i = 0; i < logEntryCount; i = num3)
		{
			if (AgricolaLib.GetGameTurnLogBuffer(i, this.m_bufPtr, 1024) != 0)
			{
				GameTurnLogEntry gameTurnLogEntry = (GameTurnLogEntry)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameTurnLogEntry));
				int num = -1;
				for (int j = 0; j < this.m_numPlayers; j++)
				{
					if (this.m_playerSlots[j].playerIndex == (int)gameTurnLogEntry.player)
					{
						num = j;
						break;
					}
				}
				if (currentRound != (int)gameTurnLogEntry.round)
				{
					lastZoomedOutInstanceID = 0;
					lastZoomedOutCardName = string.Empty;
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_logZoomOutRoundPrefab);
					if (gameObject != null)
					{
						this.m_createdObjs.Add(gameObject);
						gameObject.transform.SetParent(this.m_logZoomOutLocator.transform);
						gameObject.transform.localScale = Vector3.one;
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.GetComponent<AgricolaOverview_LogSmallRoundCell>().SetRoundNumber((int)gameTurnLogEntry.round);
					}
					for (int k = 0; k < this.m_numPlayers; k++)
					{
						this.m_playerSlots[k].logZoomOutNextIndex = ((k < num) ? (zoomOutColumns.Count + 1) : zoomOutColumns.Count);
					}
				}
				if (gameTurnLogEntry.logType == 4)
				{
					bool flag = false;
					if ((gameTurnLogEntry.cardInstanceID != 0 && (int)gameTurnLogEntry.cardInstanceID == lastZoomedOutInstanceID) || (gameTurnLogEntry.cardName != string.Empty && string.Compare(gameTurnLogEntry.cardName, lastZoomedOutCardName) == 0))
					{
						flag = true;
					}
					if (!flag)
					{
						lastZoomedOutInstanceID = (int)gameTurnLogEntry.cardInstanceID;
						lastZoomedOutCardName = gameTurnLogEntry.cardName;
						if (this.m_playerSlots[num].logZoomOutNextIndex >= zoomOutColumns.Count)
						{
							GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_logZoomOutColumnPrefab);
							if (gameObject2 != null)
							{
								this.m_createdObjs.Add(gameObject2);
								gameObject2.transform.SetParent(this.m_logZoomOutLocator.transform);
								gameObject2.transform.localScale = Vector3.one;
								gameObject2.transform.localPosition = Vector3.zero;
								zoomOutColumns.Add(gameObject2.GetComponent<AgricolaOverview_LogOutColumn>());
							}
						}
						List<AgricolaOverview_LogOutColumn> list = zoomOutColumns;
						AgricolaOverview.PlayerSlot[] playerSlots = this.m_playerSlots;
						int num2 = num;
						num3 = playerSlots[num2].logZoomOutNextIndex;
						playerSlots[num2].logZoomOutNextIndex = num3 + 1;
						AgricolaOverview_LogOutColumn agricolaOverview_LogOutColumn = list[num3];
						GameObject gameObject3 = null;
						if (gameTurnLogEntry.cardInstanceID != 0)
						{
							gameObject3 = this.m_cardManager.CreateTemporaryCardFromInstanceID((int)gameTurnLogEntry.cardInstanceID);
						}
						else if (gameTurnLogEntry.cardName != string.Empty)
						{
							gameObject3 = this.m_cardManager.CreateCardFromName(gameTurnLogEntry.cardName, false, true);
						}
						if (gameObject3 != null)
						{
							this.m_createdObjs.Add(gameObject3);
							agricolaOverview_LogOutColumn.SetCard(gameObject3, num);
						}
						else if (gameTurnLogEntry.cardInstanceID != 0)
						{
							Debug.LogError("Unable to create card with instance ID: " + gameTurnLogEntry.cardInstanceID.ToString());
						}
						else if (gameTurnLogEntry.cardName != string.Empty)
						{
							Debug.LogError("Unable to create card with name " + gameTurnLogEntry.cardName);
						}
						else
						{
							Debug.LogError("Log entry with no card attached!");
						}
					}
				}
				if (currentRound != (int)gameTurnLogEntry.round)
				{
					string empty2 = string.Empty;
					lastZoomedInCell = null;
					GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.m_logZoomInRoundStartPrefab);
					if (gameObject4 != null)
					{
						this.m_createdObjs.Add(gameObject4);
						gameObject4.transform.SetParent(this.m_logZoomInLocator.transform);
						gameObject4.transform.localScale = Vector3.one;
						gameObject4.transform.localPosition = Vector3.zero;
						gameObject4.GetComponent<AgricolaOverview_LogInRoundStart>().SetRound((int)gameTurnLogEntry.round);
					}
					GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(this.m_logZoomInCalendarPrefab);
					if (gameObject5 != null)
					{
						gameObject5.transform.SetParent(this.m_logZoomInLocator.transform);
						gameObject5.transform.localScale = Vector3.one;
						gameObject5.transform.localPosition = Vector3.zero;
						AgricolaOverview_CalendarCell component = gameObject5.GetComponent<AgricolaOverview_CalendarCell>();
						component.SetupCell((int)gameTurnLogEntry.round, this.m_numPlayers);
						component.SetShowRecievedOnly(true);
						bool flag2 = false;
						for (int l = 0; l < this.m_numPlayers; l++)
						{
							flag2 |= component.SetCalendarData(this.m_playerSlots[l].playerIndex, l, this.m_cardManager);
						}
						if (flag2)
						{
							this.m_createdObjs.Add(gameObject5);
						}
						else
						{
							UnityEngine.Object.Destroy(gameObject5);
						}
					}
				}
				bool flag3 = false;
				if (lastZoomedInCell != null && (long)lastZoomedInCell.GetPlayerIndex() == (long)((ulong)gameTurnLogEntry.player) && lastZoomedInCell.GetRoundNumber() == (int)gameTurnLogEntry.round && ((gameTurnLogEntry.cardInstanceID != 0 && (int)gameTurnLogEntry.cardInstanceID == lastZoomedInCell.GetCardInstanceID()) || (gameTurnLogEntry.cardName != string.Empty && string.Compare(gameTurnLogEntry.cardName, lastZoomedInCell.GetCardName()) == 0) || gameTurnLogEntry.logType == 5))
				{
					flag3 = true;
					lastZoomedInCell.AddResources(gameTurnLogEntry.scoreChange);
					if (gameTurnLogEntry.logType == 5 && gameTurnLogEntry.cardInstanceID != 0)
					{
						lastZoomedInCell.AddResCard((int)gameTurnLogEntry.cardInstanceID, this.m_cardManager);
					}
				}
				if (!flag3)
				{
					GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(this.m_logZoomInActionPrefab);
					this.m_createdObjs.Add(gameObject6);
					gameObject6.transform.SetParent(this.m_logZoomInLocator.transform);
					gameObject6.transform.localScale = Vector3.one;
					gameObject6.transform.localPosition = Vector3.zero;
					AgricolaOverview_LogInAction component2 = gameObject6.GetComponent<AgricolaOverview_LogInAction>();
					component2.SetupCell((int)gameTurnLogEntry.player, (int)gameTurnLogEntry.round, this.m_playerSlots[num].playerFaction, this.m_playerSlots[num].avatar.GetIndex(), num);
					component2.AddResources(gameTurnLogEntry.scoreChange);
					GameObject gameObject7 = null;
					if (gameTurnLogEntry.cardInstanceID != 0)
					{
						gameObject7 = this.m_cardManager.CreateTemporaryCardFromInstanceID((int)gameTurnLogEntry.cardInstanceID);
					}
					else if (gameTurnLogEntry.cardName != string.Empty)
					{
						gameObject7 = this.m_cardManager.CreateCardFromName(gameTurnLogEntry.cardName, false, true);
					}
					if (gameObject7 != null)
					{
						this.m_createdObjs.Add(gameObject7);
						component2.SetActionCard(gameObject7, (int)gameTurnLogEntry.cardInstanceID, gameTurnLogEntry.cardName);
					}
					else if (gameTurnLogEntry.cardInstanceID != 0)
					{
						Debug.LogError("Unable to create card with instance ID: " + gameTurnLogEntry.cardInstanceID.ToString());
					}
					else if (gameTurnLogEntry.cardName != string.Empty)
					{
						Debug.LogError("Unable to create card with name " + gameTurnLogEntry.cardName);
					}
					else
					{
						Debug.LogError("Log entry with no card attached!");
					}
					this.m_logActionCells.Add(component2);
					lastZoomedInCell = component2;
				}
				currentRound = (int)gameTurnLogEntry.round;
				if (i % 5 == 0)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			num3 = i + 1;
		}
		this.m_bLoadComplete = true;
		yield break;
	}

	// Token: 0x04000450 RID: 1104
	private const int k_maxDataSize = 1024;

	// Token: 0x04000451 RID: 1105
	public AgricolaCardManager m_cardManager;

	// Token: 0x04000452 RID: 1106
	[Space(10f)]
	public AgricolaOverview.PlayerSlot[] m_playerSlots;

	// Token: 0x04000453 RID: 1107
	public GameObject m_magnifyPanel;

	// Token: 0x04000454 RID: 1108
	public GameObject m_animationLayer;

	// Token: 0x04000455 RID: 1109
	[Space(10f)]
	public Toggle m_overviewToggle;

	// Token: 0x04000456 RID: 1110
	public Toggle m_calenderToggle;

	// Token: 0x04000457 RID: 1111
	public Toggle m_logToggle;

	// Token: 0x04000458 RID: 1112
	public UILocalizedText m_titleCard;

	// Token: 0x04000459 RID: 1113
	public GameObject m_loadingScreen;

	// Token: 0x0400045A RID: 1114
	public float m_MinimumLoadingTime = 1.5f;

	// Token: 0x0400045B RID: 1115
	[Space(10f)]
	public GameObject m_overviewPrefab;

	// Token: 0x0400045C RID: 1116
	public GameObject m_overviewRoot;

	// Token: 0x0400045D RID: 1117
	[Space(10f)]
	public GameObject m_calendarPrefab;

	// Token: 0x0400045E RID: 1118
	public GameObject m_calendarRoot;

	// Token: 0x0400045F RID: 1119
	public GameObject m_calendarLocator;

	// Token: 0x04000460 RID: 1120
	[Space(10f)]
	public GameObject m_logRoot;

	// Token: 0x04000461 RID: 1121
	public GameObject m_logZoomOutRoot;

	// Token: 0x04000462 RID: 1122
	public GameObject m_logZoomInRoot;

	// Token: 0x04000463 RID: 1123
	public GameObject m_logZoomOutRoundPrefab;

	// Token: 0x04000464 RID: 1124
	public GameObject m_logZoomOutColumnPrefab;

	// Token: 0x04000465 RID: 1125
	public GameObject m_logZoomOutLocator;

	// Token: 0x04000466 RID: 1126
	public GameObject m_logZoomInRoundStartPrefab;

	// Token: 0x04000467 RID: 1127
	public GameObject m_logZoomInCalendarPrefab;

	// Token: 0x04000468 RID: 1128
	public GameObject m_logZoomInActionPrefab;

	// Token: 0x04000469 RID: 1129
	public GameObject m_logZoomInLocator;

	// Token: 0x0400046A RID: 1130
	private AgricolaOverview.OverviewMode m_currentMode;

	// Token: 0x0400046B RID: 1131
	private bool m_bVisitedOverview;

	// Token: 0x0400046C RID: 1132
	private bool m_bVisitedCalendar;

	// Token: 0x0400046D RID: 1133
	private bool m_bVisitedLog;

	// Token: 0x0400046E RID: 1134
	private bool m_bLogZoomedIn;

	// Token: 0x0400046F RID: 1135
	private bool m_bLoadComplete;

	// Token: 0x04000470 RID: 1136
	private bool m_bIgnoreToggles;

	// Token: 0x04000471 RID: 1137
	private float m_currentLoadingTime;

	// Token: 0x04000472 RID: 1138
	private int m_numPlayers;

	// Token: 0x04000473 RID: 1139
	private List<AgricolaOverview_CalendarCell> m_calendarCells = new List<AgricolaOverview_CalendarCell>();

	// Token: 0x04000474 RID: 1140
	private List<AgricolaOverview_LogInAction> m_logActionCells = new List<AgricolaOverview_LogInAction>();

	// Token: 0x04000475 RID: 1141
	private List<GameObject> m_createdObjs = new List<GameObject>();

	// Token: 0x04000476 RID: 1142
	private byte[] m_dataBuffer;

	// Token: 0x04000477 RID: 1143
	private GCHandle m_hDataBuffer;

	// Token: 0x04000478 RID: 1144
	private IntPtr m_bufPtr;

	// Token: 0x02000772 RID: 1906
	private enum OverviewMode
	{
		// Token: 0x04002BAA RID: 11178
		None,
		// Token: 0x04002BAB RID: 11179
		Overview,
		// Token: 0x04002BAC RID: 11180
		Calendar,
		// Token: 0x04002BAD RID: 11181
		Log
	}

	// Token: 0x02000773 RID: 1907
	private enum ELogType
	{
		// Token: 0x04002BAF RID: 11183
		E_LOGTYPE_ACTION,
		// Token: 0x04002BB0 RID: 11184
		E_LOGTYPE_DRAFT,
		// Token: 0x04002BB1 RID: 11185
		E_LOGTYPE_CONVERSION,
		// Token: 0x04002BB2 RID: 11186
		E_LOGTYPE_DISCARD,
		// Token: 0x04002BB3 RID: 11187
		E_LOGTYPE_ASSIGNWORK,
		// Token: 0x04002BB4 RID: 11188
		E_LOGTYPE_APPEND,
		// Token: 0x04002BB5 RID: 11189
		E_LOGTYPE_COUNT
	}

	// Token: 0x02000774 RID: 1908
	[Serializable]
	public struct PlayerSlot
	{
		// Token: 0x04002BB6 RID: 11190
		public GameObject[] isActiveNodes;

		// Token: 0x04002BB7 RID: 11191
		public TextMeshProUGUI playerName;

		// Token: 0x04002BB8 RID: 11192
		public ColorByFaction tokenColorizer;

		// Token: 0x04002BB9 RID: 11193
		public Avatar_UI avatar;

		// Token: 0x04002BBA RID: 11194
		public ColorByFaction bkgColorizer;

		// Token: 0x04002BBB RID: 11195
		public Toggle toggle;

		// Token: 0x04002BBC RID: 11196
		[HideInInspector]
		public int playerIndex;

		// Token: 0x04002BBD RID: 11197
		[HideInInspector]
		public int playerFaction;

		// Token: 0x04002BBE RID: 11198
		[HideInInspector]
		public bool logSelectedPlayer;

		// Token: 0x04002BBF RID: 11199
		public GameObject overviewCellLocator;

		// Token: 0x04002BC0 RID: 11200
		[HideInInspector]
		public AgricolaOverview_OverviewCell overviewCell;

		// Token: 0x04002BC1 RID: 11201
		[HideInInspector]
		public int logZoomOutNextIndex;
	}
}
