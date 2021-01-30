using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000032 RID: 50
public class AgricolaCardInPlay : CardInPlay, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDropHandler
{
	// Token: 0x0600024A RID: 586 RVA: 0x0000CF11 File Offset: 0x0000B111
	private void Awake()
	{
		if (this.m_DragHighlight != null)
		{
			this.m_DragHighlight.SetActive(false);
		}
		if (this.m_SignRoot != null)
		{
			this.m_SignRoot.SetActive(false);
		}
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000CF48 File Offset: 0x0000B148
	private void OnDestroy()
	{
		if (this.m_bSetDataBuffer)
		{
			this.m_bSetDataBuffer = false;
			this.m_hDataBuffer.Free();
		}
		if (this.m_linkedTiles != null)
		{
			for (int i = 0; i < this.m_linkedTiles.Count; i++)
			{
				UnityEngine.Object.Destroy(this.m_linkedTiles[i].gameObject);
			}
		}
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000CFA4 File Offset: 0x0000B1A4
	public override void SetSourceCard(CardObject source_card)
	{
		if (this.m_SourceCard != null)
		{
			DragObject component = this.m_SourceCard.GetComponent<DragObject>();
			if (component != null)
			{
				component.SetDragSelectionOverrideID(0);
			}
			AgricolaCard component2 = this.m_SourceCard.GetComponent<AgricolaCard>();
			if (component2 != null && component2.GetCardOrchardSize() > 1 && this.m_linkedTiles == null)
			{
				this.m_linkedTiles = new List<AgricolaImprovementBase>();
			}
		}
		base.SetSourceCard(source_card);
		if (source_card != null)
		{
			DragObject component3 = source_card.GetComponent<DragObject>();
			if (component3 != null)
			{
				component3.SetDragSelectionOverrideID((ushort)base.GetCardInPlayInstanceID());
			}
		}
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000D03C File Offset: 0x0000B23C
	public void SetPlayerToken(GameObject playerToken)
	{
		if (this.m_ownerToken != null)
		{
			UnityEngine.Object.Destroy(this.m_ownerToken);
			this.m_ownerToken = null;
		}
		if (playerToken != null && this.m_TokenLocator != null)
		{
			this.m_ownerToken = playerToken;
			this.m_ownerToken.transform.SetParent(this.m_TokenLocator.transform);
			this.m_ownerToken.transform.localPosition = Vector3.zero;
			this.m_ownerToken.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000D0CC File Offset: 0x0000B2CC
	public AgricolaAnimationLocator GetCardLocator()
	{
		return this.m_CardLocator;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
	public void AddLinkedTile(AgricolaImprovementBase tile)
	{
		if (this.m_linkedTiles == null)
		{
			Debug.LogWarning("Linking a tile to a card in play that should not need it!");
			this.m_linkedTiles = new List<AgricolaImprovementBase>();
		}
		if (this.m_linkedTiles != null && tile != null)
		{
			this.m_linkedTiles.Add(tile);
			AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
			tile.SetLinkedCard(this.m_SourceCard.GetComponent<AgricolaCard>(), this);
			tile.SetDragDropCallbacks(component.GetDragManager());
		}
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000D149 File Offset: 0x0000B349
	public List<AgricolaImprovementBase> GetLinkedTiles()
	{
		return this.m_linkedTiles;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000D151 File Offset: 0x0000B351
	public int GetOwnerInstanceID()
	{
		return this.m_OwnerInstanceID;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000D159 File Offset: 0x0000B359
	public void SetOwnerInstanceID(int ownerInstanceID)
	{
		this.m_OwnerInstanceID = ownerInstanceID;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000D162 File Offset: 0x0000B362
	public int GetCardOrchardRow()
	{
		return this.m_CardOrchardRow;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000D16A File Offset: 0x0000B36A
	public void SetCardOrchardRow(int cardOrchardRow)
	{
		this.m_CardOrchardRow = cardOrchardRow;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000D174 File Offset: 0x0000B374
	public void AddConvertInstance(short convertInstanceID, short convertCostResourceType)
	{
		if (this.m_ConvertInstanceList == null)
		{
			this.m_ConvertInstanceList = new List<AgricolaCardInPlay.ConvertInstance>();
		}
		for (int i = 0; i < this.m_ConvertInstanceList.Count; i++)
		{
			AgricolaCardInPlay.ConvertInstance convertInstance = this.m_ConvertInstanceList[i];
			if (convertInstance.convertInstanceID == convertInstanceID)
			{
				convertInstance.convertCostResourceType = convertCostResourceType;
				return;
			}
		}
		AgricolaCardInPlay.ConvertInstance item = default(AgricolaCardInPlay.ConvertInstance);
		item.convertInstanceID = convertInstanceID;
		item.convertCostResourceType = convertCostResourceType;
		this.m_ConvertInstanceList.Add(item);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
	public int GetConvertInstanceResourceType(ushort convertInstanceID)
	{
		if (this.m_ConvertInstanceList != null)
		{
			for (int i = 0; i < this.m_ConvertInstanceList.Count; i++)
			{
				AgricolaCardInPlay.ConvertInstance convertInstance = this.m_ConvertInstanceList[i];
				if (convertInstance.convertInstanceID == (short)convertInstanceID)
				{
					return (int)convertInstance.convertCostResourceType;
				}
			}
		}
		return -1;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000D239 File Offset: 0x0000B439
	public void AddAbilityInstance(int abilityInstanceID)
	{
		if (this.m_AbilityInstanceList == null)
		{
			this.m_AbilityInstanceList = new List<int>();
		}
		if (!this.m_AbilityInstanceList.Contains(abilityInstanceID))
		{
			this.m_AbilityInstanceList.Add(abilityInstanceID);
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000D268 File Offset: 0x0000B468
	public bool IsAbilityInstance(int abilityInstanceID)
	{
		return this.m_AbilityInstanceList != null && this.m_AbilityInstanceList.Contains(abilityInstanceID);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000D280 File Offset: 0x0000B480
	public void PlaceSourceCard()
	{
		if (this.m_CardLocator == null)
		{
			return;
		}
		CardObject sourceCard = base.GetSourceCard();
		if (sourceCard == null)
		{
			return;
		}
		AnimateObject component = sourceCard.GetComponent<AnimateObject>();
		if (component == null)
		{
			return;
		}
		this.m_CardLocator.PlaceAnimateObject(component, true, true, false);
		component.gameObject.SetActive(true);
		RectTransform component2 = component.gameObject.GetComponent<RectTransform>();
		if (component2 != null)
		{
			component2.anchorMin = new Vector2(0.5f, 0.5f);
			component2.anchorMax = new Vector2(0.5f, 0.5f);
			component2.anchoredPosition = new Vector2(0f, 0f);
		}
		this.UpdateSignDisplay();
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000D334 File Offset: 0x0000B534
	private void UpdateSignDisplay()
	{
		if (this.m_SourceCard == null || this.m_SignRoot == null)
		{
			return;
		}
		AgricolaCard component = this.m_SourceCard.GetComponent<AgricolaCard>();
		if (component == null || component.GetCardUniqueIndex() == -1)
		{
			this.m_SignRoot.SetActive(false);
			return;
		}
		if (!this.m_bSetDataBuffer)
		{
			this.m_bSetDataBuffer = true;
			this.m_dataBuffer = new byte[64];
			this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
			this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		}
		if (AgricolaLib.GetGameCardUniqueData((uint)base.GetCardInPlayInstanceID(), this.m_bufPtr, 64) != 0)
		{
			UniqueCardData uniqueCardData = (UniqueCardData)Marshal.PtrToStructure(this.m_bufPtr, typeof(UniqueCardData));
			if (component.GetCardUniqueIndex() == 1)
			{
				this.m_SignRoot.SetActive(false);
				if (this.m_linkedTiles != null && this.m_linkedTiles.Count > 0 && this.m_linkedTiles[0] != null)
				{
					int resType = uniqueCardData.resourceType & 255;
					int resCount = uniqueCardData.resourceType >> 8 & 255;
					this.m_linkedTiles[0].UpdateFieldData(resType, resCount, uniqueCardData.textValue);
					return;
				}
			}
			else
			{
				this.m_SignRoot.SetActive(uniqueCardData.useTextValue != 0 || uniqueCardData.useResourceDisplay > 0);
				if (this.m_SignGlow != null)
				{
					this.m_SignGlow.SetActive(uniqueCardData.setGlowActive > 0);
				}
				if (this.m_SignText != null)
				{
					this.m_SignText.gameObject.SetActive(uniqueCardData.useTextValue > 0);
					this.m_SignText.text = uniqueCardData.textValue.ToString();
				}
				if (this.m_SignResources != null && this.m_SignResources.Length != 0)
				{
					for (int i = 0; i < this.m_SignResources.Length; i++)
					{
						if (this.m_SignResources[i] != null)
						{
							this.m_SignResources[i].SetActive(uniqueCardData.useResourceDisplay != 0 && uniqueCardData.resourceType == i);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000D554 File Offset: 0x0000B754
	public override void UpdateSelectionState(bool bHighlight)
	{
		if (this.m_SourceCard == null)
		{
			return;
		}
		this.UpdateSignDisplay();
		if (this.m_linkedTiles != null && this.m_linkedTiles.Count > 0)
		{
			for (int i = 0; i < this.m_linkedTiles.Count; i++)
			{
				if (this.m_linkedTiles[i] != null)
				{
					this.m_linkedTiles[i].UpdateSelectionState(bHighlight);
				}
			}
		}
		DragObject component = this.m_SourceCard.GetComponent<DragObject>();
		if (bHighlight)
		{
			if (this.m_AbilityInstanceList != null)
			{
				foreach (int num in this.m_AbilityInstanceList)
				{
					foreach (ushort selectionHint in GameOptions.GetSelectionHints((ushort)num))
					{
						DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)selectionHint);
						if (dragSelectionHintDefinition != null)
						{
							if (component != null)
							{
								component.SetDragSelectionOverrideID((ushort)num);
							}
							this.m_SourceCard.SetSelectable(true, dragSelectionHintDefinition.m_HighlightColor);
							return;
						}
					}
				}
			}
			if (component != null)
			{
				component.SetDragSelectionOverrideID((ushort)base.GetCardInPlayInstanceID());
			}
			foreach (ushort selectionHint2 in GameOptions.GetSelectionHints((ushort)base.GetCardInPlayInstanceID()))
			{
				DragSelectionHintDefinition dragSelectionHintDefinition2 = InterfaceSelectionHints.FindSelectionHintDefinition((int)selectionHint2);
				if (dragSelectionHintDefinition2 != null)
				{
					this.m_SourceCard.SetSelectable(true, dragSelectionHintDefinition2.m_HighlightColor);
					break;
				}
			}
		}
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000D70C File Offset: 0x0000B90C
	public void BeginDragCallback(DragObject dragObject)
	{
		int num = -1;
		AgricolaResource component = dragObject.GetComponent<AgricolaResource>();
		AgricolaAnimal component2 = dragObject.GetComponent<AgricolaAnimal>();
		if (component != null)
		{
			num = component.GetResourceType();
		}
		else if (component2 != null)
		{
			num = (int)component2.GetAnimalType();
		}
		if (num != -1 && this.m_ConvertInstanceList != null)
		{
			for (int i = 0; i < this.m_ConvertInstanceList.Count; i++)
			{
				AgricolaCardInPlay.ConvertInstance convertInstance = this.m_ConvertInstanceList[i];
				if ((int)convertInstance.convertCostResourceType == num && GameOptions.IsSelectableInstanceIDWithHint((ushort)convertInstance.convertInstanceID, 40992))
				{
					if (this.m_DragHighlight != null)
					{
						this.m_DragHighlight.SetActive(true);
					}
					return;
				}
			}
		}
		if (this.m_DragHighlight != null)
		{
			this.m_DragHighlight.SetActive(false);
		}
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000D7CE File Offset: 0x0000B9CE
	public void EndDragCallback(DragObject dragObject)
	{
		if (this.m_DragHighlight != null)
		{
			this.m_DragHighlight.SetActive(false);
		}
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000D7EC File Offset: 0x0000B9EC
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}
		int num = -1;
		AgricolaResource component = eventData.pointerDrag.GetComponent<AgricolaResource>();
		AgricolaAnimal component2 = eventData.pointerDrag.GetComponent<AgricolaAnimal>();
		if (component != null)
		{
			num = component.GetResourceType();
		}
		else if (component2 != null)
		{
			num = (int)component2.GetAnimalType();
		}
		if (num != -1 && this.m_ConvertInstanceList != null)
		{
			for (int i = 0; i < this.m_ConvertInstanceList.Count; i++)
			{
				AgricolaCardInPlay.ConvertInstance convertInstance = this.m_ConvertInstanceList[i];
				if ((int)convertInstance.convertCostResourceType == num && GameOptions.IsSelectableInstanceIDWithHint((ushort)convertInstance.convertInstanceID, 40992))
				{
					DragObject component3 = eventData.pointerDrag.GetComponent<DragObject>();
					if (component3 != null)
					{
						component3.SetDragSelectionOverrideID((ushort)convertInstance.convertInstanceID);
						component3.SetDragSelectionHint(40992);
					}
					return;
				}
			}
		}
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}
		DragObject component = eventData.pointerDrag.GetComponent<DragObject>();
		if (component != null)
		{
			component.SetDragSelectionOverrideID(0);
			component.SetDragSelectionHint(0);
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000D908 File Offset: 0x0000BB08
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}
		AgricolaResource component = eventData.pointerDrag.GetComponent<AgricolaResource>();
		if (component != null)
		{
			int resourceType = component.GetResourceType();
			if (this.m_ConvertInstanceList != null)
			{
				for (int i = 0; i < this.m_ConvertInstanceList.Count; i++)
				{
					AgricolaCardInPlay.ConvertInstance convertInstance = this.m_ConvertInstanceList[i];
					if ((int)convertInstance.convertCostResourceType == resourceType && GameOptions.IsSelectableInstanceIDWithHint((ushort)convertInstance.convertInstanceID, 40992))
					{
						component.SetDestroyOnDragEnd(true);
					}
				}
			}
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000D98E File Offset: 0x0000BB8E
	public void RebuildAnimationManager(AgricolaAnimationManager animation_manager)
	{
		if (animation_manager == null)
		{
			return;
		}
		if (this.m_CardLocator != null)
		{
			animation_manager.SetAnimationLocator(15, base.GetCardInPlayInstanceID(), this.m_CardLocator);
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000D9BD File Offset: 0x0000BBBD
	public void RemoveFromAnimationManager(AgricolaAnimationManager animation_manager)
	{
		if (animation_manager != null)
		{
			animation_manager.RemoveAnimationLocator(15, base.GetCardInPlayInstanceID());
		}
	}

	// Token: 0x040001AF RID: 431
	private const int k_maxDataSize = 64;

	// Token: 0x040001B0 RID: 432
	[SerializeField]
	private AgricolaAnimationLocator m_CardLocator;

	// Token: 0x040001B1 RID: 433
	[SerializeField]
	private GameObject m_TokenLocator;

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	private GameObject m_SignRoot;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	private GameObject m_SignGlow;

	// Token: 0x040001B4 RID: 436
	[SerializeField]
	private TextMeshProUGUI m_SignText;

	// Token: 0x040001B5 RID: 437
	[SerializeField]
	private GameObject[] m_SignResources;

	// Token: 0x040001B6 RID: 438
	[SerializeField]
	private GameObject m_DragHighlight;

	// Token: 0x040001B7 RID: 439
	private GameObject m_ownerToken;

	// Token: 0x040001B8 RID: 440
	private int m_OwnerInstanceID;

	// Token: 0x040001B9 RID: 441
	private int m_CardOrchardRow = -1;

	// Token: 0x040001BA RID: 442
	private List<AgricolaCardInPlay.ConvertInstance> m_ConvertInstanceList;

	// Token: 0x040001BB RID: 443
	private List<int> m_AbilityInstanceList;

	// Token: 0x040001BC RID: 444
	private List<AgricolaImprovementBase> m_linkedTiles;

	// Token: 0x040001BD RID: 445
	private bool m_bSetDataBuffer;

	// Token: 0x040001BE RID: 446
	private byte[] m_dataBuffer;

	// Token: 0x040001BF RID: 447
	private GCHandle m_hDataBuffer;

	// Token: 0x040001C0 RID: 448
	private IntPtr m_bufPtr;

	// Token: 0x02000758 RID: 1880
	public struct ConvertInstance
	{
		// Token: 0x04002B61 RID: 11105
		public short convertInstanceID;

		// Token: 0x04002B62 RID: 11106
		public short convertCostResourceType;
	}
}
