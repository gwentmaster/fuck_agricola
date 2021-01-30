using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000FA RID: 250
[Serializable]
public class UIC_OnlineGameList
{
	// Token: 0x0600095E RID: 2398 RVA: 0x0003F1FC File Offset: 0x0003D3FC
	public void Initialize(Network network, UIC_OnlineGameList.ClickCallback cb, MonoBehaviour monoBehaviourInstance)
	{
		this.m_clickCallback = cb;
		this.m_network = network;
		this.m_monoBehaviourInstance = monoBehaviourInstance;
		this.m_bGameListEnabled = true;
		this.m_bCheckingForEndScroll = false;
		if (this.m_gameSlots == null)
		{
			this.m_gameSlots = new List<UIP_GameSlot>();
		}
		if (!this.m_bShowFindGames)
		{
			this.RefreshGameList();
			this.m_monoBehaviourInstance.StartCoroutine(this.ResetScrollPosition());
			this.m_monoBehaviourInstance.StartCoroutine(this.UpdateRect());
		}
		if (this.m_network != null)
		{
			this.m_requestOnlineStatusArray = new uint[UIC_OnlineGameList.m_maxOnlineStatusRequest];
			this.m_hUserDataBuffer = GCHandle.Alloc(this.m_requestOnlineStatusArray, GCHandleType.Pinned);
			network.AddNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
			this.m_scrollView.onValueChanged.AddListener(new UnityAction<Vector2>(this.ScrollRectChanged));
			this.BeginCheckingForOnlineStatusRefresh();
			this.RefreshOnlineStatus(true);
		}
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0003F2DC File Offset: 0x0003D4DC
	public void SetRefreshCallback(UIC_OnlineGameList.ListRefreshCallback cb)
	{
		this.m_refreshCallback = cb;
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0003F2E8 File Offset: 0x0003D4E8
	public void Destroy()
	{
		if (this.m_scrollRectCoroutine != null)
		{
			this.m_monoBehaviourInstance.StopCoroutine(this.m_scrollRectCoroutine);
			this.m_scrollRectCoroutine = null;
		}
		if (this.m_network != null)
		{
			this.m_network.RemoveNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		}
		this.ClearGameList();
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0003F340 File Offset: 0x0003D540
	public void UpdateNoGameDisplay()
	{
		if (this.m_noGamesDisplay == null)
		{
			return;
		}
		bool flag = true;
		int num = 0;
		if (this.m_gameSlots != null)
		{
			using (List<UIP_GameSlot>.Enumerator enumerator = this.m_gameSlots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.gameObject.activeSelf)
					{
						flag = false;
						num++;
					}
				}
			}
		}
		if (flag || this.m_gameSlots == null || num == 0)
		{
			this.m_noGamesDisplay.SetActive(true);
			if (this.m_noGamesText != null)
			{
				if (this.m_bBuildAvailableGameList)
				{
					this.m_noGamesText.KeyText = "${Key_FindGameSearching}";
					return;
				}
				if (this.m_bShowFindGames)
				{
					this.m_noGamesText.KeyText = "${Key_FindGameListEmpty}";
					return;
				}
				this.m_noGamesText.KeyText = (this.m_bShowCompletedGames ? "${Key_NoCompletedGames}" : "${Key_ActiveGames}");
				return;
			}
		}
		else
		{
			this.m_noGamesDisplay.SetActive(false);
		}
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0003F440 File Offset: 0x0003D640
	public void EnableGameList(bool bEnable, bool bShowCompletedGames = false, bool bShowFindGameList = false)
	{
		this.m_bGameListEnabled = bEnable;
		if (bEnable)
		{
			this.m_bShowCompletedGames = bShowCompletedGames;
			this.m_bShowFindGames = bShowFindGameList;
			if (this.m_bShowFindGames)
			{
				this.m_bBuildAvailableGameList = true;
				this.ClearGameList();
				this.m_monoBehaviourInstance.StartCoroutine(this.ProcessDelayTime(this.m_minDialogDisplayTime));
				Network.RefreshAvailableGames();
			}
			else
			{
				this.RefreshGameList();
			}
			this.m_monoBehaviourInstance.StartCoroutine(this.ResetScrollPosition());
			this.m_monoBehaviourInstance.StartCoroutine(this.UpdateRect());
			return;
		}
		this.ClearGameList();
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0003F4CA File Offset: 0x0003D6CA
	public void RemoveGameSlotFromList(UIP_GameSlot slot)
	{
		if (this.m_gameSlots.Remove(slot))
		{
			UnityEngine.Object.Destroy(slot.gameObject);
			this.UpdateNoGameDisplay();
		}
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0003F4EC File Offset: 0x0003D6EC
	public void SetAllDeleteMode(bool bDeleteMode)
	{
		this.m_bDeleteMode = bDeleteMode;
		for (int i = 0; i < this.m_gameSlots.Count; i++)
		{
			this.m_gameSlots[i].TurnOnDeleteMode(bDeleteMode);
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0003F528 File Offset: 0x0003D728
	public int GetGameCount()
	{
		return this.m_gameSlots.Count;
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0003F535 File Offset: 0x0003D735
	public UIP_GameSlot GetGameAtIndex(uint index)
	{
		if ((ulong)index < (ulong)((long)this.m_gameSlots.Count))
		{
			return this.m_gameSlots[(int)index];
		}
		return null;
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0003F558 File Offset: 0x0003D758
	public UIP_GameSlot GetGameWithNetworkID(uint networkID)
	{
		for (int i = 0; i < this.m_gameSlots.Count; i++)
		{
			if (this.m_gameSlots[i] != null && this.m_gameSlots[i].GetNetworkGameID() == networkID)
			{
				return this.m_gameSlots[i];
			}
		}
		return null;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0003F5B1 File Offset: 0x0003D7B1
	private IEnumerator UpdateRect()
	{
		yield return new WaitForEndOfFrame();
		this.m_monoBehaviourInstance.StartCoroutine(this.ResetScrollPosition());
		yield break;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0003F5C0 File Offset: 0x0003D7C0
	private void HandleClickOnSlot(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt)
	{
		if (this.m_clickCallback != null)
		{
			this.m_clickCallback(slot, evt);
		}
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0003F5D7 File Offset: 0x0003D7D7
	private void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		if (this.m_bGameListEnabled && (eventType == NetworkEvent.EventType.Event_UpdatedGameList || eventType == NetworkEvent.EventType.Event_UpdatedMatchmakingList))
		{
			this.RefreshGameList();
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0003F5F0 File Offset: 0x0003D7F0
	private IEnumerator ProcessDelayTime(float totalDelayTime)
	{
		float accumulatedTime = 0f;
		float previousTime = Time.time;
		while (accumulatedTime < totalDelayTime)
		{
			accumulatedTime += Time.time - previousTime;
			previousTime = Time.time;
			yield return new WaitForEndOfFrame();
		}
		if (this.m_bBuildAvailableGameList)
		{
			this.m_bBuildAvailableGameList = false;
			this.RefreshGameList();
		}
		yield break;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0003F606 File Offset: 0x0003D806
	public void SetFindGameFilterFlags(uint playerFlags, uint timerFlags)
	{
		this.m_availableListVisiblePlayerFlags = playerFlags;
		this.m_availableListVisibleTimerFlags = timerFlags;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0003F618 File Offset: 0x0003D818
	private void ClearGameList()
	{
		if (this.m_gameSlots == null)
		{
			return;
		}
		for (int i = 0; i < this.m_gameSlots.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_gameSlots[i].gameObject);
		}
		this.m_gameSlots.Clear();
		this.UpdateNoGameDisplay();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0003F66C File Offset: 0x0003D86C
	private void RefreshGameList()
	{
		this.ClearGameList();
		if (GameObject.FindGameObjectWithTag("IAP Manager") == null)
		{
			Debug.LogError("Unable to get IAP manager object");
			return;
		}
		List<ShortSaveStruct> list;
		if (this.m_bShowCompletedGames)
		{
			list = Network.GetCompletedGameList();
		}
		else if (this.m_bShowFindGames)
		{
			list = Network.GetAvailableGameList(this.m_availableListVisiblePlayerFlags, this.m_availableListVisibleTimerFlags);
		}
		else
		{
			list = Network.GetActiveGameList();
		}
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_gameSlotPrefab);
			UIP_GameSlot component = gameObject.GetComponent<UIP_GameSlot>();
			component.InitializeNetwork(this.m_network);
			component.SetData(list[i], this.m_bShowFindGames ? UIP_GameSlot.ESlotType.Available : UIP_GameSlot.ESlotType.Active_Online, null);
			component.SetClickListener(new UIP_GameSlot.ClickCallback(this.HandleClickOnSlot));
			if (this.m_bShowCompletedGames)
			{
				component.HideDeleteButton();
			}
			gameObject.transform.SetParent(this.m_contentContainer);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
			if (this.m_bShowFindGames && component.GetIsHostOnline())
			{
				gameObject.transform.SetAsFirstSibling();
			}
			this.m_gameSlots.Add(component);
		}
		this.m_monoBehaviourInstance.StartCoroutine(this.ResetScrollPosition());
		if (this.m_refreshCallback != null)
		{
			this.m_refreshCallback();
		}
		if (!this.m_bShowCompletedGames && !this.m_bShowFindGames)
		{
			List<ShortSaveStruct> matchmakingList = Network.GetMatchmakingList();
			for (int j = 0; j < matchmakingList.Count; j++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_gameSlotPrefab);
				UIP_GameSlot component2 = gameObject2.GetComponent<UIP_GameSlot>();
				component2.InitializeNetwork(this.m_network);
				component2.SetData(matchmakingList[j], UIP_GameSlot.ESlotType.Matchmaking, null);
				component2.SetClickListener(new UIP_GameSlot.ClickCallback(this.HandleClickOnSlot));
				gameObject2.transform.SetParent(this.m_contentContainer);
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.transform.localPosition = new Vector3(gameObject2.transform.localPosition.x, gameObject2.transform.localPosition.y, 0f);
				this.m_gameSlots.Add(component2);
			}
		}
		this.UpdateNoGameDisplay();
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0003F8D0 File Offset: 0x0003DAD0
	private void RefreshOnlineStatus(bool bIgnoreVisibility)
	{
		int num = 0;
		for (int i = 0; i < this.m_gameSlots.Count; i++)
		{
			bool flag = bIgnoreVisibility || this.m_gameSlots[i].IsVisible();
			bool flag2 = this.m_gameSlots[i].NeedsOnlineStatusUpdate();
			if (flag && flag2)
			{
				int[] onlineStatusUpdateUserId = this.m_gameSlots[i].GetOnlineStatusUpdateUserId();
				for (int j = 0; j < onlineStatusUpdateUserId.Length; j++)
				{
					bool flag3 = false;
					for (int k = 0; k < num; k++)
					{
						if (this.m_requestOnlineStatusArray[k] == (uint)onlineStatusUpdateUserId[j] || AgricolaLib.NetworkGetLocalID() == (uint)onlineStatusUpdateUserId[j])
						{
							flag3 = true;
							break;
						}
					}
					if (!flag3)
					{
						if (num >= UIC_OnlineGameList.m_maxOnlineStatusRequest)
						{
							break;
						}
						this.m_requestOnlineStatusArray[num++] = (uint)onlineStatusUpdateUserId[j];
						if (j == onlineStatusUpdateUserId.Length - 1)
						{
							this.m_gameSlots[i].SetAllRequestsProcessed();
						}
					}
				}
			}
		}
		if (num > 0)
		{
			Network.RequestUsersOnlineStatus(this.m_hUserDataBuffer.AddrOfPinnedObject(), num);
		}
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0003F9CC File Offset: 0x0003DBCC
	private IEnumerator CheckForEndScroll()
	{
		this.m_bCheckingForEndScroll = true;
		uint lastChangeValue = this.m_scrollChangeCounter - 1U;
		int sameCount = 3;
		while (sameCount > 0)
		{
			if (lastChangeValue == this.m_scrollChangeCounter)
			{
				int num = sameCount - 1;
				sameCount = num;
			}
			else
			{
				sameCount = 3;
				lastChangeValue = this.m_scrollChangeCounter;
			}
			yield return new WaitForEndOfFrame();
		}
		this.RefreshOnlineStatus(false);
		this.m_bCheckingForEndScroll = false;
		yield break;
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0003F9DB File Offset: 0x0003DBDB
	private void BeginCheckingForOnlineStatusRefresh()
	{
		if (!this.m_bCheckingForEndScroll)
		{
			if (this.m_scrollRectCoroutine != null)
			{
				this.m_monoBehaviourInstance.StopCoroutine(this.m_scrollRectCoroutine);
			}
			this.m_scrollRectCoroutine = this.m_monoBehaviourInstance.StartCoroutine(this.CheckForEndScroll());
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0003FA15 File Offset: 0x0003DC15
	private void ScrollRectChanged(Vector2 data)
	{
		this.m_scrollChangeCounter += 1U;
		this.BeginCheckingForOnlineStatusRefresh();
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0003FA2B File Offset: 0x0003DC2B
	private IEnumerator ResetScrollPosition()
	{
		yield return new WaitForEndOfFrame();
		this.m_scrollView.horizontalNormalizedPosition = 0f;
		yield break;
	}

	// Token: 0x040009DF RID: 2527
	public GameObject m_gameSlotPrefab;

	// Token: 0x040009E0 RID: 2528
	public Transform m_contentContainer;

	// Token: 0x040009E1 RID: 2529
	public ScrollRect m_scrollView;

	// Token: 0x040009E2 RID: 2530
	public GameObject m_noGamesDisplay;

	// Token: 0x040009E3 RID: 2531
	public UILocalizedText m_noGamesText;

	// Token: 0x040009E4 RID: 2532
	public float m_minDialogDisplayTime;

	// Token: 0x040009E5 RID: 2533
	private static readonly int m_maxOnlineStatusRequest = 48;

	// Token: 0x040009E6 RID: 2534
	private List<UIP_GameSlot> m_gameSlots;

	// Token: 0x040009E7 RID: 2535
	private UIC_OnlineGameList.ClickCallback m_clickCallback;

	// Token: 0x040009E8 RID: 2536
	private UIC_OnlineGameList.ListRefreshCallback m_refreshCallback;

	// Token: 0x040009E9 RID: 2537
	private Network m_network;

	// Token: 0x040009EA RID: 2538
	private MonoBehaviour m_monoBehaviourInstance;

	// Token: 0x040009EB RID: 2539
	private Coroutine m_scrollRectCoroutine;

	// Token: 0x040009EC RID: 2540
	private uint[] m_requestOnlineStatusArray;

	// Token: 0x040009ED RID: 2541
	private GCHandle m_hUserDataBuffer;

	// Token: 0x040009EE RID: 2542
	private uint m_scrollChangeCounter;

	// Token: 0x040009EF RID: 2543
	private bool m_bCheckingForEndScroll;

	// Token: 0x040009F0 RID: 2544
	private int m_findGameFilterFlags;

	// Token: 0x040009F1 RID: 2545
	private bool m_bGameListEnabled;

	// Token: 0x040009F2 RID: 2546
	private bool m_bShowCompletedGames;

	// Token: 0x040009F3 RID: 2547
	private bool m_bShowFindGames;

	// Token: 0x040009F4 RID: 2548
	private bool m_bDeleteMode;

	// Token: 0x040009F5 RID: 2549
	private bool m_bBuildAvailableGameList;

	// Token: 0x040009F6 RID: 2550
	private uint m_availableListVisiblePlayerFlags;

	// Token: 0x040009F7 RID: 2551
	private uint m_availableListVisibleTimerFlags;

	// Token: 0x020007C5 RID: 1989
	// (Invoke) Token: 0x06004313 RID: 17171
	public delegate void ClickCallback(UIP_GameSlot slot, UIP_GameSlot.ClickEventType evt);

	// Token: 0x020007C6 RID: 1990
	// (Invoke) Token: 0x06004317 RID: 17175
	public delegate void ListRefreshCallback();
}
