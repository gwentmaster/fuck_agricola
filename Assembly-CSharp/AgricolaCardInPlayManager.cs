using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameData;
using GameEvent;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000033 RID: 51
public class AgricolaCardInPlayManager : MonoBehaviour
{
	// Token: 0x06000264 RID: 612 RVA: 0x0000D9E8 File Offset: 0x0000BBE8
	public List<AgricolaCardInPlay> GetPlayerCardInPlayList(int playerInstanceID)
	{
		List<AgricolaCardInPlay> result;
		if (this.s_PlayerCardInPlayLists != null && this.s_PlayerCardInPlayLists.TryGetValue(playerInstanceID, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000DA10 File Offset: 0x0000BC10
	private void Awake()
	{
		this.m_CardInPlayListBuffer = new int[64];
		this.m_CardInPlayListBufferHandle = GCHandle.Alloc(this.m_CardInPlayListBuffer, GCHandleType.Pinned);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000DA31 File Offset: 0x0000BC31
	private void Start()
	{
		if (this.m_DragManager != null)
		{
			this.m_DragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.BeginDragCallback));
			this.m_DragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.EndDragCallback));
		}
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000DA6F File Offset: 0x0000BC6F
	private void OnDestroy()
	{
		this.m_CardInPlayListBufferHandle.Free();
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0000DA7C File Offset: 0x0000BC7C
	public void RegisterEventHandlers(GameEventBuffer game_event_buffer)
	{
		if (game_event_buffer == null)
		{
			return;
		}
		game_event_buffer.RegisterEventHandler(8, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventCardInPlayStatus));
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000DA98 File Offset: 0x0000BC98
	public GameObject CreateCardInPlayFromInstanceID(int cardInPlayInstanceID)
	{
		GameObject gameObject = null;
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.GetInstanceData(9, cardInPlayInstanceID, intPtr, 1024);
		CardInPlayData cardInPlayData = (CardInPlayData)Marshal.PtrToStructure(intPtr, typeof(CardInPlayData));
		if ((int)cardInPlayData.cardinplay_instance_id == cardInPlayInstanceID && this.m_CardInPlayPrefab != null)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_CardInPlayPrefab);
			if (gameObject != null)
			{
				gameObject.name = "Card in Play: " + cardInPlayInstanceID.ToString();
				AgricolaCardInPlay component = gameObject.GetComponent<AgricolaCardInPlay>();
				if (component != null)
				{
					component.SetCardInPlayInstanceID((int)cardInPlayData.cardinplay_instance_id);
					if (cardInPlayData.sourcecard_instance_id != 0 && this.m_CardManager != null)
					{
						GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID((int)cardInPlayData.sourcecard_instance_id, true);
						if (cardFromInstanceID != null)
						{
							AgricolaCard component2 = cardFromInstanceID.GetComponent<AgricolaCard>();
							if (component2 != null)
							{
								component.SetSourceCard(component2);
								if (component2.GetCardOrchardSize() > 1)
								{
									int cardUniqueIndex = component2.GetCardUniqueIndex();
									if (cardUniqueIndex == 1)
									{
										GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_ImprovementFieldPerfab);
										if (gameObject2 != null)
										{
											component.AddLinkedTile(gameObject2.GetComponent<AgricolaImprovementBase>());
											gameObject2.transform.SetParent(this.m_CardInPlayLimbo.transform, false);
											gameObject2.SetActive(false);
										}
									}
								}
							}
						}
					}
					component.SetOwnerInstanceID((int)cardInPlayData.owner_instance_id);
					component.SetCardOrchardRow((int)cardInPlayData.card_orchard_row);
				}
				for (int i = 0; i < cardInPlayData.convert_count; i++)
				{
					if (cardInPlayData.convert_instance_id_list[i] != 0)
					{
						component.AddConvertInstance((short)cardInPlayData.convert_instance_id_list[i], (short)cardInPlayData.convert_resourcetype_list[i]);
					}
				}
				gameObject.SetActive(true);
				this.m_MasterCardInPlayList.Add(cardInPlayInstanceID, gameObject);
			}
		}
		gchandle.Free();
		return gameObject;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000DC7C File Offset: 0x0000BE7C
	public GameObject GetCardInPlayFromInstanceID(int instanceID, bool bCreateIfNecessary = false)
	{
		GameObject gameObject = (GameObject)this.m_MasterCardInPlayList[instanceID];
		if (gameObject == null && bCreateIfNecessary)
		{
			gameObject = this.CreateCardInPlayFromInstanceID(instanceID);
		}
		return gameObject;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
	public void UpdateSelectionState(bool bHighlight)
	{
		foreach (object obj in this.m_MasterCardInPlayList.Values)
		{
			AgricolaCardInPlay component = ((GameObject)obj).GetComponent<AgricolaCardInPlay>();
			if (component != null)
			{
				component.UpdateSelectionState(bHighlight);
			}
		}
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000DD20 File Offset: 0x0000BF20
	public void BeginDragCallback(DragObject dragObject)
	{
		if (dragObject.GetComponent<AgricolaResource>() != null)
		{
			foreach (object obj in this.m_MasterCardInPlayList.Values)
			{
				AgricolaCardInPlay component = ((GameObject)obj).GetComponent<AgricolaCardInPlay>();
				if (component != null)
				{
					component.BeginDragCallback(dragObject);
				}
			}
		}
		if (dragObject.GetComponent<AgricolaAnimal>() != null)
		{
			foreach (object obj2 in this.m_MasterCardInPlayList.Values)
			{
				AgricolaCardInPlay component2 = ((GameObject)obj2).GetComponent<AgricolaCardInPlay>();
				if (component2 != null)
				{
					component2.BeginDragCallback(dragObject);
				}
			}
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000DE04 File Offset: 0x0000C004
	public void EndDragCallback(DragObject dragObject)
	{
		if (dragObject.GetComponent<AgricolaResource>() != null)
		{
			foreach (object obj in this.m_MasterCardInPlayList.Values)
			{
				AgricolaCardInPlay component = ((GameObject)obj).GetComponent<AgricolaCardInPlay>();
				if (component != null)
				{
					component.EndDragCallback(dragObject);
				}
			}
		}
		if (dragObject.GetComponent<AgricolaAnimal>() != null)
		{
			foreach (object obj2 in this.m_MasterCardInPlayList.Values)
			{
				AgricolaCardInPlay component2 = ((GameObject)obj2).GetComponent<AgricolaCardInPlay>();
				if (component2 != null)
				{
					component2.EndDragCallback(dragObject);
				}
			}
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000DEE8 File Offset: 0x0000C0E8
	private void HandleEventCardInPlayStatus(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		CardInPlayStatus cardInPlayStatus = (CardInPlayStatus)Marshal.PtrToStructure(event_buffer, typeof(CardInPlayStatus));
		int owner_instance_id = cardInPlayStatus.owner_instance_id;
		List<AgricolaCardInPlay> list;
		if (!this.s_PlayerCardInPlayLists.TryGetValue(owner_instance_id, out list))
		{
			list = new List<AgricolaCardInPlay>();
			this.s_PlayerCardInPlayLists.Add(owner_instance_id, list);
		}
		GameObject cardInPlayFromInstanceID = this.GetCardInPlayFromInstanceID(cardInPlayStatus.cardinplay_instance_id, true);
		if (cardInPlayFromInstanceID != null)
		{
			AgricolaCardInPlay component = cardInPlayFromInstanceID.GetComponent<AgricolaCardInPlay>();
			if (component != null)
			{
				if (cardInPlayStatus.inplay == 0)
				{
					list.Remove(component);
					return;
				}
				if (!list.Contains(component))
				{
					list.Add(component);
				}
			}
		}
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000DF84 File Offset: 0x0000C184
	public uint GetConvertResourceFlags()
	{
		uint num = 0U;
		foreach (ushort convertInstanceID in GameOptions.GetSelectionIDsForHint(40992, false))
		{
			foreach (object obj in this.m_MasterCardInPlayList.Values)
			{
				AgricolaCardInPlay component = ((GameObject)obj).GetComponent<AgricolaCardInPlay>();
				if (component != null)
				{
					int convertInstanceResourceType = component.GetConvertInstanceResourceType(convertInstanceID);
					if (convertInstanceResourceType >= 0)
					{
						num |= 1U << convertInstanceResourceType;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000E04C File Offset: 0x0000C24C
	public bool PlaceCardInPlayInLimbo(AgricolaCardInPlay cardinplay)
	{
		if (this.m_CardInPlayLimbo == null || cardinplay == null)
		{
			return false;
		}
		cardinplay.SetPlayerToken(null);
		cardinplay.transform.SetParent(this.m_CardInPlayLimbo.transform, false);
		cardinplay.gameObject.SetActive(false);
		List<AgricolaImprovementBase> linkedTiles = cardinplay.GetLinkedTiles();
		if (linkedTiles != null)
		{
			for (int i = 0; i < linkedTiles.Count; i++)
			{
				if (linkedTiles[i] != null)
				{
					linkedTiles[i].transform.SetParent(this.m_CardInPlayLimbo.transform, false);
					linkedTiles[i].gameObject.SetActive(false);
				}
			}
		}
		return true;
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
	public void PlaceAllCardsInPlayInLimbo()
	{
		if (this.m_CardInPlayLimbo == null)
		{
			return;
		}
		foreach (object obj in this.m_MasterCardInPlayList.Values)
		{
			GameObject gameObject = (GameObject)obj;
			AgricolaCardInPlay component = gameObject.GetComponent<AgricolaCardInPlay>();
			if (component != null)
			{
				component.SetPlayerToken(null);
			}
			gameObject.transform.SetParent(this.m_CardInPlayLimbo.transform, false);
			gameObject.SetActive(false);
			if (component != null)
			{
				List<AgricolaImprovementBase> linkedTiles = component.GetLinkedTiles();
				if (linkedTiles != null)
				{
					for (int i = 0; i < linkedTiles.Count; i++)
					{
						if (linkedTiles[i] != null)
						{
							linkedTiles[i].transform.SetParent(this.m_CardInPlayLimbo.transform, false);
							linkedTiles[i].gameObject.SetActive(false);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000E1FC File Offset: 0x0000C3FC
	public void RebuildCardInPlayList()
	{
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		gchandle.AddrOfPinnedObject();
		IntPtr pInstanceIDs = this.m_CardInPlayListBufferHandle.AddrOfPinnedObject();
		List<AgricolaCardInPlay> list = new List<AgricolaCardInPlay>();
		int num = 0;
		for (;;)
		{
			int localOpponentPlayerIndex = AgricolaLib.GetLocalOpponentPlayerIndex(num);
			if (localOpponentPlayerIndex == 0)
			{
				break;
			}
			int localOpponentInstanceID = AgricolaLib.GetLocalOpponentInstanceID(num);
			List<AgricolaCardInPlay> list2;
			if (!this.s_PlayerCardInPlayLists.TryGetValue(localOpponentInstanceID, out list2))
			{
				list2 = new List<AgricolaCardInPlay>();
				this.s_PlayerCardInPlayLists.Add(localOpponentInstanceID, list2);
			}
			foreach (AgricolaCardInPlay item in list2)
			{
				list.Add(item);
			}
			list2.Clear();
			int instanceList = AgricolaLib.GetInstanceList(9, localOpponentPlayerIndex, pInstanceIDs, 64);
			for (int i = 0; i < instanceList; i++)
			{
				int instanceID = this.m_CardInPlayListBuffer[i];
				GameObject cardInPlayFromInstanceID = this.GetCardInPlayFromInstanceID(instanceID, true);
				if (cardInPlayFromInstanceID != null)
				{
					AgricolaCardInPlay component = cardInPlayFromInstanceID.GetComponent<AgricolaCardInPlay>();
					if (component != null && !list2.Contains(component))
					{
						list2.Add(component);
						while (list.Remove(component))
						{
						}
					}
				}
			}
			num++;
		}
		gchandle.Free();
		foreach (AgricolaCardInPlay agricolaCardInPlay in list)
		{
			agricolaCardInPlay.SetSourceCard(null);
			this.m_MasterCardInPlayList.Remove(agricolaCardInPlay.GetCardInPlayInstanceID());
			UnityEngine.Object.Destroy(agricolaCardInPlay.gameObject);
		}
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
	public void RebuildAnimationManager(AgricolaAnimationManager animation_manager)
	{
		if (animation_manager == null)
		{
			return;
		}
		foreach (object obj in this.m_MasterCardInPlayList.Values)
		{
			AgricolaCardInPlay component = ((GameObject)obj).GetComponent<AgricolaCardInPlay>();
			if (component != null)
			{
				component.RebuildAnimationManager(animation_manager);
			}
		}
	}

	// Token: 0x040001C1 RID: 449
	private const int k_maxDataSize = 1024;

	// Token: 0x040001C2 RID: 450
	private const int k_maxCardInPlayCount = 64;

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	protected AgricolaCardManager m_CardManager;

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	protected DragManager m_DragManager;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private GameObject m_CardInPlayPrefab;

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private GameObject m_ImprovementFieldPerfab;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private GameObject m_ImprovementPasturePrefab;

	// Token: 0x040001C8 RID: 456
	[SerializeField]
	private GameObject m_CardInPlayLimbo;

	// Token: 0x040001C9 RID: 457
	private Hashtable m_MasterCardInPlayList = new Hashtable();

	// Token: 0x040001CA RID: 458
	private Dictionary<int, List<AgricolaCardInPlay>> s_PlayerCardInPlayLists = new Dictionary<int, List<AgricolaCardInPlay>>();

	// Token: 0x040001CB RID: 459
	private int[] m_CardInPlayListBuffer;

	// Token: 0x040001CC RID: 460
	private GCHandle m_CardInPlayListBufferHandle;
}
