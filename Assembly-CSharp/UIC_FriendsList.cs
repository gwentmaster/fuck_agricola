using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F8 RID: 248
[Serializable]
public class UIC_FriendsList
{
	// Token: 0x06000930 RID: 2352 RVA: 0x0003E2B4 File Offset: 0x0003C4B4
	public void Initialize(Network network, UIC_FriendsList.ClickCallback cb, MonoBehaviour monoBehaviourInstance)
	{
		if (this.m_offlineProfileManager == null)
		{
			this.m_offlineProfileManager = ProfileManager.instance;
		}
		this.m_network = network;
		this.m_clickCallback = cb;
		this.m_monoBehaviourInstance = monoBehaviourInstance;
		this.m_friendSlots = new List<UIP_FriendSlot>();
		this.m_requestOnlineStatusArray = new uint[UIC_FriendsList.m_maxOnlineStatusRequest];
		this.m_hUserDataBuffer = GCHandle.Alloc(this.m_requestOnlineStatusArray, GCHandleType.Pinned);
		this.m_bCheckingForEndScroll = false;
		this.m_scrollView.onValueChanged.AddListener(new UnityAction<Vector2>(this.ScrollRectChanged));
		if (Network.m_Network.m_bConnectedToServer)
		{
			network.AddNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
			this.BuildFriendList();
			this.BeginCheckingForOnlineStatusRefresh();
			return;
		}
		this.RebuildOfflineList(-1);
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0003E374 File Offset: 0x0003C574
	public void Destroy()
	{
		if (Network.m_Network.m_bConnectedToServer)
		{
			this.m_network.RemoveNetworkEventHandler(new Network.NetworkEventDelegate(this.NetworkEventCallback));
		}
		this.ClearFriendList();
		this.m_clickCallback = null;
		if (this.m_scrollRectCoroutine != null)
		{
			this.m_monoBehaviourInstance.StopCoroutine(this.m_scrollRectCoroutine);
			this.m_scrollRectCoroutine = null;
		}
		this.m_hUserDataBuffer.Free();
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0003E3DC File Offset: 0x0003C5DC
	public UIP_FriendSlot GetFriendSlotByDisplayName(string displayName)
	{
		if (Network.m_Network.m_bConnectedToServer)
		{
			for (int i = 0; i < this.m_friendSlots.Count; i++)
			{
				if (this.m_friendSlots[i].GetFriendInfo().displayName == displayName)
				{
					return this.m_friendSlots[i];
				}
			}
		}
		else
		{
			for (int j = 0; j < this.m_friendSlots.Count; j++)
			{
				if (this.m_friendSlots[j].GetOfflineProfile().name == displayName)
				{
					return this.m_friendSlots[j];
				}
			}
		}
		return null;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0003E47C File Offset: 0x0003C67C
	public void Remove(uint friendUserID)
	{
		if (Network.m_Network.m_bConnectedToServer)
		{
			for (int i = 0; i < this.m_friendSlots.Count; i++)
			{
				if (this.m_friendSlots[i].GetUserID() == friendUserID)
				{
					UnityEngine.Object.Destroy(this.m_friendSlots[i].gameObject);
					this.m_friendSlots.RemoveAt(i);
					return;
				}
			}
			return;
		}
		UnityEngine.Object.Destroy(this.m_friendSlots[(int)friendUserID].gameObject);
		this.m_friendSlots.RemoveAt((int)friendUserID);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0003E508 File Offset: 0x0003C708
	public void SetOverrideFactionColor(uint factionIndex)
	{
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			this.m_friendSlots[i].SetFactionColor(factionIndex);
		}
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0003E540 File Offset: 0x0003C740
	public void ClearSelected()
	{
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			this.m_friendSlots[i].SetIsSelected(false);
		}
		this.m_selectedIndex = -1;
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0003E57C File Offset: 0x0003C77C
	public void SetDeleteMode(bool bDeleteMode)
	{
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			this.m_friendSlots[i].SetDeleteMode(bDeleteMode);
		}
		if (!bDeleteMode)
		{
			this.SetSelected(this.m_selectedIndex);
		}
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0003E5C0 File Offset: 0x0003C7C0
	public UIP_FriendSlot GetSelectedSlot()
	{
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			if (this.m_friendSlots[i].GetIsSelected())
			{
				return this.m_friendSlots[i];
			}
		}
		return null;
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0003E604 File Offset: 0x0003C804
	public void SetSelectedSlot(UIP_FriendSlot slot)
	{
		if (slot == null)
		{
			return;
		}
		if (this.m_maxSelected == 1)
		{
			this.SetSelected(this.m_friendSlots.IndexOf(slot));
			return;
		}
		this.ToggleSelected(this.m_friendSlots.IndexOf(slot));
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0003E640 File Offset: 0x0003C840
	public void SetSelected(int index)
	{
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			this.m_friendSlots[i].SetIsSelected(i == index);
		}
		this.m_selectedIndex = index;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0003E67F File Offset: 0x0003C87F
	public bool GetIsSelected(int index)
	{
		return this.m_friendSlots[index].GetIsSelected();
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0003E694 File Offset: 0x0003C894
	public void ToggleSelected(int index)
	{
		int num = 0;
		this.m_friendSlots[index].SetIsSelected(!this.m_friendSlots[index].GetIsSelected());
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			if (this.m_friendSlots[index].GetIsSelected())
			{
				num++;
			}
		}
		if (num > this.m_maxSelected)
		{
			this.m_friendSlots[index].SetIsSelected(false);
		}
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0003E710 File Offset: 0x0003C910
	private void HandleSlotOnBeginDrag(UI_DragSource e, PointerEventData a)
	{
		if (this.m_scrollView != null)
		{
			this.m_scrollView.OnBeginDrag(a);
			this.m_OnBeginDragEvent.Invoke(e);
			this.m_bWasDragSuccessful = false;
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0003E73F File Offset: 0x0003C93F
	private void HandleSlotOnDragging(UI_DragSource e, PointerEventData a)
	{
		if (this.m_scrollView != null)
		{
			this.m_scrollView.OnDrag(a);
		}
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0003E75C File Offset: 0x0003C95C
	private void HandleSlotOnEndDrag(UI_DragSource e, PointerEventData a)
	{
		if (this.m_scrollView != null)
		{
			this.m_scrollView.OnEndDrag(a);
			this.m_OnEndDragEvent.Invoke(e);
			if (this.m_bWasDragSuccessful)
			{
				this.m_bWasDragSuccessful = false;
				this.m_friendListBase.SetActive(false);
			}
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0003E7AA File Offset: 0x0003C9AA
	private void HandleClickOnSlot(UIP_FriendSlot slot, UIP_FriendSlot.ClickEventType evt)
	{
		if (this.m_clickCallback != null)
		{
			this.m_clickCallback(slot, evt);
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0003E7C4 File Offset: 0x0003C9C4
	public void RebuildOfflineList(int overrideFactionIndex = -1)
	{
		this.ClearFriendList();
		int num = this.m_offlineProfileManager.Count();
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_friendSlotPrefab);
			UIP_FriendSlot component = gameObject.GetComponent<UIP_FriendSlot>();
			if (overrideFactionIndex != -1)
			{
				component.SetOverrideFactionIndex(overrideFactionIndex);
			}
			component.SetOfflineData(this.m_offlineProfileManager.Get(i), i);
			component.SetClickListener(new UIP_FriendSlot.ClickCallback(this.HandleClickOnSlot));
			if (component.m_dragSource != null)
			{
				component.m_dragSource.enabled = this.m_allowDragDrop;
				if (this.m_allowDragDrop)
				{
					component.m_dragSource.m_DragStart += this.HandleSlotOnBeginDrag;
					component.m_dragSource.m_DragContinue += this.HandleSlotOnDragging;
					component.m_dragSource.m_DragEnd += this.HandleSlotOnEndDrag;
				}
			}
			this.m_friendSlots.Add(component);
			gameObject.transform.SetParent(this.m_contentContainer);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0003E908 File Offset: 0x0003CB08
	private void NetworkEventCallback(NetworkEvent.EventType eventType, int eventData)
	{
		if (eventType == NetworkEvent.EventType.Event_UpdatedFriendsList)
		{
			this.BuildFriendList();
			return;
		}
		if (eventType == NetworkEvent.EventType.Event_UpdatedOnlineStatus)
		{
			PlayerOnlineStatus userOnlineStatus = (PlayerOnlineStatus)Network.GetUserOnlineStatus((uint)eventData);
			int friendsListItemIndex = this.GetFriendsListItemIndex((uint)eventData);
			if (friendsListItemIndex >= 0)
			{
				this.m_friendSlots[friendsListItemIndex].SetOnlineStatus(userOnlineStatus);
			}
		}
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0003E94D File Offset: 0x0003CB4D
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
		this.RefreshOnlineStatus();
		this.m_bCheckingForEndScroll = false;
		yield break;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0003E95C File Offset: 0x0003CB5C
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

	// Token: 0x06000944 RID: 2372 RVA: 0x0003E996 File Offset: 0x0003CB96
	private void ScrollRectChanged(Vector2 data)
	{
		this.m_scrollChangeCounter += 1U;
		this.BeginCheckingForOnlineStatusRefresh();
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0003E9AC File Offset: 0x0003CBAC
	public void RefreshOnlineStatus()
	{
		int num = 0;
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			bool flag = this.m_friendSlots[i].IsVisible();
			bool flag2 = this.m_friendSlots[i].GetOnlineStatus() == PlayerOnlineStatus.STATUS_UNKNOWN;
			bool flag3 = this.m_friendSlots[i].GetFriendInfo().userRating > 0;
			if (flag && flag2 && flag3)
			{
				this.m_requestOnlineStatusArray[num++] = this.m_friendSlots[i].GetFriendInfo().userID;
				if (num >= UIC_FriendsList.m_maxOnlineStatusRequest)
				{
					break;
				}
			}
		}
		if (num > 0)
		{
			Network.RequestUsersOnlineStatus(this.m_hUserDataBuffer.AddrOfPinnedObject(), num);
		}
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0003EA5C File Offset: 0x0003CC5C
	private void ClearFriendList()
	{
		if (this.m_friendSlots == null)
		{
			return;
		}
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_friendSlots[i].gameObject);
		}
		this.m_friendSlots.Clear();
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0003EAAC File Offset: 0x0003CCAC
	private void BuildFriendList()
	{
		this.ClearFriendList();
		List<FriendInfo> friendsList = Network.GetFriendsList();
		friendsList.Sort(delegate(FriendInfo x, FriendInfo y)
		{
			if (x.userRating < y.userRating)
			{
				return 1;
			}
			if (x.userRating > y.userRating)
			{
				return -1;
			}
			return x.displayName.CompareTo(y.displayName);
		});
		for (int i = 0; i < friendsList.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_friendSlotPrefab);
			UIP_FriendSlot component = gameObject.GetComponent<UIP_FriendSlot>();
			component.SetFriendData(friendsList[i]);
			component.SetOnlineStatus(PlayerOnlineStatus.STATUS_UNKNOWN);
			component.SetClickListener(new UIP_FriendSlot.ClickCallback(this.HandleClickOnSlot));
			if (component.m_dragSource != null)
			{
				component.m_dragSource.enabled = this.m_allowDragDrop;
				if (this.m_allowDragDrop)
				{
					component.m_dragSource.m_DragStart += this.HandleSlotOnBeginDrag;
					component.m_dragSource.m_DragContinue += this.HandleSlotOnDragging;
					component.m_dragSource.m_DragEnd += this.HandleSlotOnEndDrag;
				}
			}
			this.m_friendSlots.Add(component);
			gameObject.transform.SetParent(this.m_contentContainer);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
		}
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0003EC08 File Offset: 0x0003CE08
	private int GetFriendsListItemIndex(uint userID)
	{
		for (int i = 0; i < this.m_friendSlots.Count; i++)
		{
			if (this.m_friendSlots[i].GetFriendInfo().userID == userID)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x040009C2 RID: 2498
	public GameObject m_friendSlotPrefab;

	// Token: 0x040009C3 RID: 2499
	public GameObject m_friendListBase;

	// Token: 0x040009C4 RID: 2500
	public Transform m_contentContainer;

	// Token: 0x040009C5 RID: 2501
	public ScrollRect m_scrollView;

	// Token: 0x040009C6 RID: 2502
	public bool m_allowDragDrop;

	// Token: 0x040009C7 RID: 2503
	[SerializeField]
	public UIC_FriendsList.OnDragEvent m_OnBeginDragEvent = new UIC_FriendsList.OnDragEvent();

	// Token: 0x040009C8 RID: 2504
	[SerializeField]
	public UIC_FriendsList.OnDragEvent m_OnEndDragEvent = new UIC_FriendsList.OnDragEvent();

	// Token: 0x040009C9 RID: 2505
	[HideInInspector]
	public bool m_bWasDragSuccessful;

	// Token: 0x040009CA RID: 2506
	private static readonly int m_maxOnlineStatusRequest = 12;

	// Token: 0x040009CB RID: 2507
	private ProfileManager m_offlineProfileManager;

	// Token: 0x040009CC RID: 2508
	private Network m_network;

	// Token: 0x040009CD RID: 2509
	private UIC_FriendsList.ClickCallback m_clickCallback;

	// Token: 0x040009CE RID: 2510
	private MonoBehaviour m_monoBehaviourInstance;

	// Token: 0x040009CF RID: 2511
	private List<UIP_FriendSlot> m_friendSlots;

	// Token: 0x040009D0 RID: 2512
	private uint[] m_requestOnlineStatusArray;

	// Token: 0x040009D1 RID: 2513
	private GCHandle m_hUserDataBuffer;

	// Token: 0x040009D2 RID: 2514
	private Coroutine m_scrollRectCoroutine;

	// Token: 0x040009D3 RID: 2515
	private uint m_scrollChangeCounter;

	// Token: 0x040009D4 RID: 2516
	private bool m_bCheckingForEndScroll;

	// Token: 0x040009D5 RID: 2517
	private int m_selectedIndex = -1;

	// Token: 0x040009D6 RID: 2518
	private int m_maxSelected = 1;

	// Token: 0x020007BE RID: 1982
	[Serializable]
	public class OnDragEvent : UnityEvent<UI_DragSource>
	{
	}

	// Token: 0x020007BF RID: 1983
	// (Invoke) Token: 0x060042F8 RID: 17144
	public delegate void ClickCallback(UIP_FriendSlot slot, UIP_FriendSlot.ClickEventType evt);

	// Token: 0x020007C0 RID: 1984
	// (Invoke) Token: 0x060042FC RID: 17148
	public delegate void DragCallback(UIP_FriendSlot slot, UIP_FriendSlot.ClickEventType evt);
}
