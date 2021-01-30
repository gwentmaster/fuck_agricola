using System;
using System.Runtime.InteropServices;
using GameEvent;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000053 RID: 83
public class DraftInterface : MonoBehaviour
{
	// Token: 0x060004AA RID: 1194 RVA: 0x00003022 File Offset: 0x00001222
	public void SetEnabled(bool bEnabled)
	{
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00024AE0 File Offset: 0x00022CE0
	public void SetPlayerIndex(int player_index, int player_instance_id)
	{
		this.m_PlayerIndex = player_index;
		this.m_PlayerInstanceID = player_instance_id;
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00024AF0 File Offset: 0x00022CF0
	private void Awake()
	{
		this.m_DraftPileCount = 0;
		this.m_DraftPileList = new int[24];
		this.m_hDraftPileBuffer = GCHandle.Alloc(this.m_DraftPileList, GCHandleType.Pinned);
		this.m_DraftBuffer = this.m_hDraftPileBuffer.AddrOfPinnedObject();
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00003022 File Offset: 0x00001222
	private void Start()
	{
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00024B29 File Offset: 0x00022D29
	private void OnDestroy()
	{
		this.m_hDraftPileBuffer.Free();
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00024B36 File Offset: 0x00022D36
	public void RegisterEventHandlers(GameEventBuffer game_event_buffer)
	{
		if (game_event_buffer == null)
		{
			return;
		}
		game_event_buffer.RegisterEventHandler(25, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventDraftMode));
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00024B50 File Offset: 0x00022D50
	public AgricolaAnimationLocator FindDraftPileAnimationLocator(AgricolaCard animate_card, bool bMini)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return null;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.m_DraftPileCount; i++)
		{
			int instanceID = this.m_DraftPileList[i];
			GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(instanceID, true);
			if (!(cardFromInstanceID == null))
			{
				AgricolaCard component = cardFromInstanceID.GetComponent<AgricolaCard>();
				if (!(component == null))
				{
					if (component.GetCardType() == 1)
					{
						if (component == animate_card)
						{
							if (bMini)
							{
								if (this.m_LocatorsDraftMinorImprovementMini != null && num < this.m_LocatorsDraftMinorImprovementMini.Length)
								{
									return this.m_LocatorsDraftMinorImprovementMini[num];
								}
								break;
							}
							else
							{
								if (this.m_LocatorsDraftMinorImprovement != null && num < this.m_LocatorsDraftMinorImprovement.Length)
								{
									return this.m_LocatorsDraftMinorImprovement[num];
								}
								break;
							}
						}
						else
						{
							num++;
						}
					}
					if (component.GetCardType() == 2)
					{
						if (component == animate_card)
						{
							if (bMini)
							{
								if (this.m_LocatorsDraftOccupationMini != null && num2 < this.m_LocatorsDraftOccupationMini.Length)
								{
									return this.m_LocatorsDraftOccupationMini[num2];
								}
								break;
							}
							else
							{
								if (this.m_LocatorsDraftOccupation != null && num2 < this.m_LocatorsDraftOccupation.Length)
								{
									return this.m_LocatorsDraftOccupation[num2];
								}
								break;
							}
						}
						else
						{
							num2++;
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00024C70 File Offset: 0x00022E70
	public AgricolaAnimationLocator FindDraftSelectedAnimationLocator(AgricolaCard animate_card)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return null;
		}
		int[] array = new int[16];
		GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr pInstanceIDs = gchandle.AddrOfPinnedObject();
		int instanceList = AgricolaLib.GetInstanceList(13, this.m_PlayerIndex, pInstanceIDs, 16);
		AgricolaAnimationLocator result = null;
		for (int i = 0; i < instanceList; i++)
		{
			int instanceID = array[i];
			GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(instanceID, true);
			if (!(cardFromInstanceID == null))
			{
				AgricolaCard component = cardFromInstanceID.GetComponent<AgricolaCard>();
				if (!(component == null) && component == animate_card)
				{
					if (this.m_LocatorsDraftSelected != null && i < this.m_LocatorsDraftSelected.Length)
					{
						result = this.m_LocatorsDraftSelected[i];
						break;
					}
					break;
				}
			}
		}
		gchandle.Free();
		return result;
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00024D30 File Offset: 0x00022F30
	private void HandleEventDraftMode(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		DraftMode draftMode = (DraftMode)Marshal.PtrToStructure(event_buffer, typeof(DraftMode));
		if (draftMode.draft_player_instance_id != 0 && draftMode.draft_player_instance_id != this.m_PlayerInstanceID)
		{
			return;
		}
		if (draftMode.draft_mode == 1)
		{
			if (draftMode.draft_type == 2)
			{
				if (this.m_DraftSelectedRoot != null)
				{
					this.m_DraftSelectedRoot.SetActive(true);
				}
				if (this.m_DiscardTrashCanRoot != null)
				{
					this.m_DiscardTrashCanRoot.SetActive(false);
				}
				this.RebuilDraftSelected();
			}
			else if (draftMode.draft_type == 3)
			{
				if (this.m_DraftSelectedRoot != null)
				{
					this.m_DraftSelectedRoot.SetActive(false);
				}
				if (this.m_DiscardTrashCanRoot != null)
				{
					this.m_DiscardTrashCanRoot.SetActive(true);
				}
			}
			this.RebuilLocalPlayerDraftPile();
			return;
		}
		if (draftMode.draft_mode == 2)
		{
			this.UpdateDraftPile();
			return;
		}
		if (draftMode.draft_mode == 3)
		{
			this.RebuilDraftSelected();
			return;
		}
		if (draftMode.draft_mode == 0)
		{
			this.ClearDraftOccupation();
			this.ClearDraftMinorImprovement();
			this.ClearDraftSelected();
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00024E39 File Offset: 0x00023039
	private void UpdateDraftPile()
	{
		this.m_DraftPileCount = AgricolaLib.GetInstanceList(14, this.m_PlayerIndex, this.m_DraftBuffer, 24);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00024E58 File Offset: 0x00023058
	private void ClearDraftOccupation()
	{
		if (this.m_CardManager == null || this.m_LocatorsDraftOccupation == null)
		{
			return;
		}
		for (int i = 0; i < this.m_LocatorsDraftOccupation.Length; i++)
		{
			int j = 0;
			while (j < this.m_LocatorsDraftOccupation[i].transform.childCount)
			{
				AgricolaCard component = this.m_LocatorsDraftOccupation[i].transform.GetChild(j).gameObject.GetComponent<AgricolaCard>();
				if (component != null)
				{
					this.m_CardManager.PlaceCardInCardLimbo(component);
				}
				else
				{
					j++;
				}
			}
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00024EE4 File Offset: 0x000230E4
	private void ClearDraftMinorImprovement()
	{
		if (this.m_CardManager == null || this.m_LocatorsDraftMinorImprovement == null)
		{
			return;
		}
		for (int i = 0; i < this.m_LocatorsDraftMinorImprovement.Length; i++)
		{
			int j = 0;
			while (j < this.m_LocatorsDraftMinorImprovement[i].transform.childCount)
			{
				AgricolaCard component = this.m_LocatorsDraftMinorImprovement[i].transform.GetChild(j).gameObject.GetComponent<AgricolaCard>();
				if (component != null)
				{
					this.m_CardManager.PlaceCardInCardLimbo(component);
				}
				else
				{
					j++;
				}
			}
		}
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00024F70 File Offset: 0x00023170
	private void ClearDraftSelected()
	{
		if (this.m_CardManager == null || this.m_LocatorsDraftSelected == null)
		{
			return;
		}
		for (int i = 0; i < this.m_LocatorsDraftSelected.Length; i++)
		{
			int j = 0;
			while (j < this.m_LocatorsDraftSelected[i].transform.childCount)
			{
				AgricolaCard component = this.m_LocatorsDraftSelected[i].transform.GetChild(j).gameObject.GetComponent<AgricolaCard>();
				if (component != null)
				{
					this.m_CardManager.PlaceCardInCardLimbo(component);
				}
				else
				{
					j++;
				}
			}
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00024FFC File Offset: 0x000231FC
	private void RebuilLocalPlayerDraftPile()
	{
		if (this.m_PlayerIndex == 0 || this.m_CardManager == null || this.m_LocatorsDraftOccupation == null || this.m_LocatorsDraftMinorImprovement == null)
		{
			return;
		}
		this.ClearDraftOccupation();
		this.ClearDraftMinorImprovement();
		this.UpdateDraftPile();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.m_DraftPileCount; i++)
		{
			int instanceID = this.m_DraftPileList[i];
			GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(instanceID, true);
			if (!(cardFromInstanceID == null))
			{
				AgricolaCard component = cardFromInstanceID.GetComponent<AgricolaCard>();
				if (!(component == null))
				{
					AgricolaAnimationLocator agricolaAnimationLocator = null;
					if (component.GetCardType() == 1 && num2 < this.m_LocatorsDraftMinorImprovement.Length)
					{
						agricolaAnimationLocator = this.m_LocatorsDraftMinorImprovement[num2++];
					}
					if (component.GetCardType() == 2 && num < this.m_LocatorsDraftOccupation.Length)
					{
						agricolaAnimationLocator = this.m_LocatorsDraftOccupation[num++];
					}
					if (!(agricolaAnimationLocator == null))
					{
						AnimateObject component2 = cardFromInstanceID.GetComponent<AnimateObject>();
						if (component2 != null && !component2.IsAnimating())
						{
							cardFromInstanceID.SetActive(true);
							agricolaAnimationLocator.PlaceAnimateObject(component2, true, true, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0002511C File Offset: 0x0002331C
	private void RebuilDraftSelected()
	{
		if (this.m_CardManager == null || this.m_LocatorsDraftSelected == null)
		{
			return;
		}
		this.ClearDraftSelected();
		int[] array = new int[16];
		GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr pInstanceIDs = gchandle.AddrOfPinnedObject();
		int instanceList = AgricolaLib.GetInstanceList(13, this.m_PlayerIndex, pInstanceIDs, 16);
		for (int i = 0; i < instanceList; i++)
		{
			int instanceID = array[i];
			GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID(instanceID, true);
			if (!(cardFromInstanceID == null))
			{
				AnimateObject component = cardFromInstanceID.GetComponent<AnimateObject>();
				if (component != null && !component.IsAnimating())
				{
					cardFromInstanceID.SetActive(true);
					if (i < this.m_LocatorsDraftSelected.Length && this.m_LocatorsDraftSelected[i] != null)
					{
						this.m_LocatorsDraftSelected[i].PlaceAnimateObject(component, true, true, false);
					}
				}
			}
		}
		gchandle.Free();
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x000251FA File Offset: 0x000233FA
	public void RebuildAnimationManager(AnimationManager animation_manager)
	{
		if (animation_manager == null)
		{
			return;
		}
		if (this.m_LocatorDiscard != null)
		{
			animation_manager.SetAnimationLocator(7, 0, this.m_LocatorDiscard);
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00025223 File Offset: 0x00023423
	public void RebuildInterface()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.RebuilLocalPlayerDraftPile();
		if (((GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters))).gameType == 2)
		{
			this.RebuilDraftSelected();
		}
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x00003022 File Offset: 0x00001222
	public void UpdateGameOptionsSelectionState(bool bHighlight)
	{
	}

	// Token: 0x0400042B RID: 1067
	private const int k_maxDraftPileCount = 24;

	// Token: 0x0400042C RID: 1068
	private const int k_maxSelectedCardCount = 16;

	// Token: 0x0400042D RID: 1069
	[SerializeField]
	private AgricolaCardManager m_CardManager;

	// Token: 0x0400042E RID: 1070
	[SerializeField]
	private GameObject m_DraftOccupationRoot;

	// Token: 0x0400042F RID: 1071
	[SerializeField]
	private AgricolaAnimationLocator[] m_LocatorsDraftOccupation;

	// Token: 0x04000430 RID: 1072
	[SerializeField]
	private AgricolaAnimationLocator[] m_LocatorsDraftOccupationMini;

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private GameObject m_DraftMinorImprovementRoot;

	// Token: 0x04000432 RID: 1074
	[SerializeField]
	private AgricolaAnimationLocator[] m_LocatorsDraftMinorImprovement;

	// Token: 0x04000433 RID: 1075
	[SerializeField]
	private AgricolaAnimationLocator[] m_LocatorsDraftMinorImprovementMini;

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private GameObject m_DraftSelectedRoot;

	// Token: 0x04000435 RID: 1077
	[SerializeField]
	private AgricolaAnimationLocator[] m_LocatorsDraftSelected;

	// Token: 0x04000436 RID: 1078
	[SerializeField]
	private GameObject m_DiscardTrashCanRoot;

	// Token: 0x04000437 RID: 1079
	[SerializeField]
	private AgricolaAnimationLocator m_LocatorDiscard;

	// Token: 0x04000438 RID: 1080
	private int m_PlayerIndex;

	// Token: 0x04000439 RID: 1081
	private int m_PlayerInstanceID;

	// Token: 0x0400043A RID: 1082
	private int m_DraftPileCount;

	// Token: 0x0400043B RID: 1083
	private int[] m_DraftPileList;

	// Token: 0x0400043C RID: 1084
	private GCHandle m_hDraftPileBuffer;

	// Token: 0x0400043D RID: 1085
	private IntPtr m_DraftBuffer;
}
