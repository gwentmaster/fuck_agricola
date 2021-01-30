using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000031 RID: 49
public class AgricolaCard : CardObject, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x060001FC RID: 508 RVA: 0x0000B4C0 File Offset: 0x000096C0
	public bool IsShowingCardBack()
	{
		return this.m_bShowingCardBack;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000B4C8 File Offset: 0x000096C8
	public string GetCardName()
	{
		return this.m_CardName;
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000B4D0 File Offset: 0x000096D0
	public void SetCardName(string name)
	{
		this.m_CardName = name;
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000B4D9 File Offset: 0x000096D9
	public int GetCardType()
	{
		return this.m_CardType;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000B4E1 File Offset: 0x000096E1
	public short GetCardOrchardRow()
	{
		return this.m_CardOrchardRow;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000B4E1 File Offset: 0x000096E1
	public short GetCardOrchardSize()
	{
		return this.m_CardOrchardRow;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000B4E9 File Offset: 0x000096E9
	public short GetIntrigueType()
	{
		return this.m_IntrigueType;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000B4F1 File Offset: 0x000096F1
	public uint GetCardDeck()
	{
		return this.m_DeckNumber;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000B4F9 File Offset: 0x000096F9
	public uint GetCardnumber()
	{
		return this.m_CardNumber;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000B501 File Offset: 0x00009701
	public int GetCardUniqueIndex()
	{
		return this.m_UniqueIndex;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000B509 File Offset: 0x00009709
	public uint GetCompressedDeckCardValue()
	{
		return this.m_CardNumber | this.m_DeckNumber << 10;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000B51B File Offset: 0x0000971B
	public void SetCardType(int cardType, short cardOrchardRow)
	{
		this.m_CardType = cardType;
		this.m_CardOrchardRow = cardOrchardRow;
		this.m_bMatchWidthOnActiveDisplay = true;
		this.m_bMatchHeightOnActiveDisplay = false;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000B539 File Offset: 0x00009739
	public void SetCardOrchardSize(short orchardSize)
	{
		this.m_CardOrchardSize = orchardSize;
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000B542 File Offset: 0x00009742
	public void SetCardDeckNumber(uint deckNumber)
	{
		this.m_DeckNumber = deckNumber;
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000B54B File Offset: 0x0000974B
	public void SetCardNumber(uint cardNumber)
	{
		this.m_CardNumber = cardNumber;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000B554 File Offset: 0x00009754
	public void SetCardUniqueIndex(int unique_index)
	{
		this.m_UniqueIndex = unique_index;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000B55D File Offset: 0x0000975D
	private void OnDestroy()
	{
		this.RemoveCardImage();
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000B568 File Offset: 0x00009768
	public static GameObject CreateFromPrefab(GameObject cardPrefab)
	{
		if (cardPrefab == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cardPrefab);
		if (gameObject == null)
		{
			return null;
		}
		AgricolaCard component = gameObject.GetComponent<AgricolaCard>();
		if (component != null && component.m_CardBackFullCard != null)
		{
			component.m_CardBackFullCard.SetActive(false);
		}
		return gameObject;
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000B5C0 File Offset: 0x000097C0
	public void MergeCardFrontFullFromPrefab(GameObject cardFrontFullPrefab)
	{
		if (cardFrontFullPrefab == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cardFrontFullPrefab);
		if (gameObject != null)
		{
			if (this.m_CardFrontParent != null)
			{
				gameObject.transform.SetParent(this.m_CardFrontParent.transform);
			}
			else
			{
				gameObject.transform.SetParent(base.transform);
			}
			gameObject.transform.SetAsFirstSibling();
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(false);
			this.m_CardFrontFullCard = gameObject;
			this.m_LayoutCardFullFront = gameObject.GetComponent<AgricolaCardLayout>();
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000B654 File Offset: 0x00009854
	public void MergeCardFrontHalfFromPrefab(GameObject cardFrontHalfPrefab)
	{
		if (cardFrontHalfPrefab == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cardFrontHalfPrefab);
		if (gameObject != null)
		{
			if (this.m_CardFrontParent != null)
			{
				gameObject.transform.SetParent(this.m_CardFrontParent.transform);
			}
			else
			{
				gameObject.transform.SetParent(base.transform);
			}
			gameObject.transform.SetAsFirstSibling();
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(false);
			this.m_CardFrontHalfCard = gameObject;
			this.m_LayoutCardHalfFront = gameObject.GetComponent<AgricolaCardLayout>();
			if (this.m_LayoutCardHalfFront != null)
			{
				this.m_MajorImpovementHHShadebox = this.m_LayoutCardHalfFront.GetMajImpHHShadeObj();
				this.m_MajorImpovementHHTokenLocator = this.m_LayoutCardHalfFront.GetMajImpHHShaTokenLocator();
				if (this.m_MajorImpovementHHShadebox != null && this.m_MajorImpovementHHTokenLocator != null)
				{
					this.m_LayoutCardHalfFront = null;
				}
			}
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000B740 File Offset: 0x00009940
	public void MergeCardFrontIconFromPrefab(GameObject cardFrontIconPrefab)
	{
		if (cardFrontIconPrefab == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cardFrontIconPrefab);
		if (gameObject != null)
		{
			if (this.m_CardFrontParent != null)
			{
				gameObject.transform.SetParent(this.m_CardFrontParent.transform);
			}
			else
			{
				gameObject.transform.SetParent(base.transform);
			}
			gameObject.transform.SetAsLastSibling();
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(false);
			this.m_CardFrontIconCard = gameObject;
			this.m_LayoutCardIconFront = gameObject.GetComponent<AgricolaCardLayout>();
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000B7D4 File Offset: 0x000099D4
	public void MergeCardBackHalfFromPrefab(GameObject cardBackHalfPrefab)
	{
		if (cardBackHalfPrefab == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cardBackHalfPrefab);
		if (gameObject != null)
		{
			if (this.m_CardFrontParent != null)
			{
				gameObject.transform.SetParent(this.m_CardFrontParent.transform);
			}
			else
			{
				gameObject.transform.SetParent(base.transform);
			}
			gameObject.transform.SetAsLastSibling();
			gameObject.SetActive(false);
			this.m_CardBackHalfCard = gameObject;
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000B84C File Offset: 0x00009A4C
	public void MergeCardGlowFullFromPrefab(GameObject glowPrefab)
	{
		if (glowPrefab == null)
		{
			return;
		}
		if (this.m_CardGlowFull != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_CardGlowFull);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(glowPrefab);
		if (this.m_CardFrontParent != null)
		{
			gameObject.transform.SetParent(this.m_CardFrontParent.transform);
		}
		else
		{
			gameObject.transform.SetParent(base.transform);
		}
		gameObject.transform.SetAsFirstSibling();
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(false);
		this.m_CardGlowFull = gameObject;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000B8E4 File Offset: 0x00009AE4
	public void MergeCardGlowHalfFromPrefab(GameObject glowPrefab)
	{
		if (glowPrefab == null)
		{
			return;
		}
		if (this.m_CardGlowHalf != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_CardGlowHalf);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(glowPrefab);
		if (this.m_CardFrontParent != null)
		{
			gameObject.transform.SetParent(this.m_CardFrontParent.transform);
		}
		else
		{
			gameObject.transform.SetParent(base.transform);
		}
		gameObject.transform.SetAsFirstSibling();
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(false);
		this.m_CardGlowHalf = gameObject;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000B97B File Offset: 0x00009B7B
	public bool HasLoadCompleted()
	{
		return this.m_ResourceCardImage == null || this.m_ResourceCardImage.HasLoadCompleted();
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000B992 File Offset: 0x00009B92
	public void AddOnLoadCompletedCallback(ResourceEntry.ResourceLoadCallback callback)
	{
		if (this.m_ResourceCardImage != null && !this.HasLoadCompleted())
		{
			this.m_ResourceCardImage.AddOnLoadCompletedCallback(callback);
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000B9B0 File Offset: 0x00009BB0
	public void SetCardImage(ResourceEntry resourceCardImage)
	{
		this.RemoveCardImage();
		if (resourceCardImage == null)
		{
			return;
		}
		this.m_ResourceCardImage = resourceCardImage;
		this.m_ResourceCardImage.IncrementCount();
		if (resourceCardImage.HasLoadCompleted())
		{
			this.HandleCardImageLoadCompleted(this.m_ResourceCardImage);
			return;
		}
		resourceCardImage.AddOnLoadCompletedCallback(new ResourceEntry.ResourceLoadCallback(this.HandleCardImageLoadCompleted));
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000BA00 File Offset: 0x00009C00
	private void HandleCardImageLoadCompleted(ResourceEntry resourceEntry)
	{
		if (this == null)
		{
			return;
		}
		Sprite resource = this.m_ResourceCardImage.GetResource<Sprite>();
		if (resource == null)
		{
			return;
		}
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardImage(resource);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardImage(resource);
		}
		if (this.m_LayoutCardIconFront != null)
		{
			this.m_LayoutCardIconFront.SetCardImage(resource);
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000BA7C File Offset: 0x00009C7C
	private void RemoveCardImage()
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardImage(null);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardImage(null);
		}
		if (this.m_LayoutCardIconFront != null)
		{
			this.m_LayoutCardIconFront.SetCardImage(null);
		}
		if (this.m_ResourceCardImage != null)
		{
			this.m_ResourceCardImage.ReleaseResourceEntry();
			this.m_ResourceCardImage = null;
		}
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000BAF1 File Offset: 0x00009CF1
	public void SetCardDeckIcon(Sprite deck_icon)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardDeckIcon(deck_icon);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardDeckIcon(deck_icon);
		}
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000BB27 File Offset: 0x00009D27
	public void SetCardPlayerCountIcon(Sprite player_count_icon)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardPlayerCountIcon(player_count_icon);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardPlayerCountIcon(player_count_icon);
		}
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000BB5D File Offset: 0x00009D5D
	public void SetCardCategoryIcon(Sprite category_icon)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardCategoryIcon(category_icon);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardCategoryIcon(category_icon);
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000BB93 File Offset: 0x00009D93
	public void SetCardBonusPointIcon(bool bBonusPointIcon)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardBonusPointIcon(bBonusPointIcon);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardBonusPointIcon(bBonusPointIcon);
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000BBC9 File Offset: 0x00009DC9
	public void SetVictoryPointCount(int victoryPointCount, Sprite victoryPointIcon)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetVictoryPointCount(victoryPointCount, victoryPointIcon);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetVictoryPointCount(victoryPointCount, victoryPointIcon);
		}
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000BC01 File Offset: 0x00009E01
	public void SetCardFramePassArrows(bool bShowArrows)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardFramePassArrows(bShowArrows);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardFramePassArrows(bShowArrows);
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000BC37 File Offset: 0x00009E37
	public void SetCardPassText(bool bShowText)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardPassText(bShowText);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardPassText(bShowText);
		}
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000BC6D File Offset: 0x00009E6D
	public void HideResourceCost()
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.HideResourceCost();
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.HideResourceCost();
		}
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000BCA1 File Offset: 0x00009EA1
	public void ShowResourceCost(byte[] resourceCost)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.ShowResourceCost(resourceCost);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.ShowResourceCost(resourceCost);
		}
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000BCD7 File Offset: 0x00009ED7
	public void SetCardNumberText(string card_number)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardNumberText(card_number);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardNumberText(card_number);
		}
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000BD0D File Offset: 0x00009F0D
	public void SetCardTitle(string card_title)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardTitle(card_title);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardTitle(card_title);
		}
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000BD43 File Offset: 0x00009F43
	public void SetCardTextEffect(string card_effect_text)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardTextEffect(card_effect_text);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardTextEffect(card_effect_text);
		}
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000BD79 File Offset: 0x00009F79
	public void SetCardTextRequirement(string card_requirement_text)
	{
		if (this.m_LayoutCardFullFront != null)
		{
			this.m_LayoutCardFullFront.SetCardTextRequirement(card_requirement_text);
		}
		if (this.m_LayoutCardHalfFront != null)
		{
			this.m_LayoutCardHalfFront.SetCardTextRequirement(card_requirement_text);
		}
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000BDB0 File Offset: 0x00009FB0
	private void Awake()
	{
		if (this.m_CardFrontFullCard != null)
		{
			this.m_CardFrontFullCard.SetActive(this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_FULL);
		}
		if (this.m_CardFrontHalfCard != null)
		{
			this.m_CardFrontHalfCard.SetActive(this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_HALF);
		}
		if (this.m_CardFrontIconCard != null)
		{
			this.m_CardFrontIconCard.SetActive(this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_ICON);
		}
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000BE24 File Offset: 0x0000A024
	public override void SetupCallbacks(CardManager card_manager)
	{
		base.SetupCallbacks(card_manager);
		AnimateObject component = base.GetComponent<AnimateObject>();
		if (component != null)
		{
			component.AddOnBeginAnimationCallback(new AnimateObject.AnimationCallback(this.BeginAnimationCallback));
			component.AddOnEndAnimationCallback(new AnimateObject.AnimationCallback(this.EndAnimationCallback));
			if (this.m_CardBackRotation != null)
			{
				component.SetInternalRotationNode(this.m_CardBackRotation);
			}
		}
		DragObject component2 = base.GetComponent<DragObject>();
		if (component2 != null)
		{
			component2.AddOnBeginDragCallback(new DragObject.DragObjectCallback(this.BeginDragCallback));
			component2.AddOnEndDragCallback(new DragObject.DragObjectCallback(this.EndDragCallback));
			component2.AddOnDragHintCallback(new DragObject.DragObjectCallback(this.DragHintCallback));
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000BECC File Offset: 0x0000A0CC
	private void SetActiveGlow()
	{
		if (this.m_ActiveGlowDisplay != null)
		{
			if (this.m_bGlowActive)
			{
				GlowColorAdjust component = this.m_ActiveGlowDisplay.GetComponent<GlowColorAdjust>();
				if (component != null)
				{
					component.SetColor(this.m_GlowColor);
				}
			}
			this.m_ActiveGlowDisplay.SetActive(this.m_bGlowActive);
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000BF21 File Offset: 0x0000A121
	public void SetGlowOverride(bool bActive, Color glowColor, bool bOverrideColor = false)
	{
		this.m_bGlowActive = bActive;
		this.m_GlowColor = glowColor;
		this.m_bOverrideGlowColor = bOverrideColor;
		this.SetActiveGlow();
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000BF40 File Offset: 0x0000A140
	public override void SetSelectable(bool bSelectable, Color highlightColor)
	{
		base.SetSelectable(bSelectable, highlightColor);
		this.m_bGlowActive = bSelectable;
		if (!this.m_bOverrideGlowColor)
		{
			this.m_GlowColor = highlightColor;
		}
		else
		{
			this.m_bGlowActive = true;
		}
		this.SetActiveGlow();
		DragObject component = base.GetComponent<DragObject>();
		if (component != null && !this.m_bShowingCardBack && this.m_DragType == ECardDragType.Selectable)
		{
			component.SetIsDraggable(base.IsSelectable());
		}
		if (bSelectable && this.m_bCurrentlyDragging && component != null)
		{
			DragManager dragManager = component.GetDragManager();
			if (dragManager != null)
			{
				dragManager.SetDragTargetZones(component.GetDragSelectionID());
			}
		}
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000BFD8 File Offset: 0x0000A1D8
	private void BeginAnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		ELocatorHiddenStatus elocatorHiddenStatus = ELocatorHiddenStatus.VISIBLE;
		AgricolaAnimationLocator agricolaAnimationLocator = sourceLocator as AgricolaAnimationLocator;
		if (agricolaAnimationLocator != null)
		{
			elocatorHiddenStatus = agricolaAnimationLocator.HiddenToLocalPlayer();
		}
		AgricolaAnimationLocator agricolaAnimationLocator2 = destinationLocator as AgricolaAnimationLocator;
		if (agricolaAnimationLocator2 != null)
		{
			ELocatorHiddenStatus elocatorHiddenStatus2 = agricolaAnimationLocator2.HiddenToLocalPlayer();
			if (agricolaAnimationLocator2.GetCardDisplayState() != ECardDisplayState.DISPLAY_NONE)
			{
				bool bCardBackEnabled = false;
				switch (elocatorHiddenStatus2)
				{
				case ELocatorHiddenStatus.VISIBLE:
					bCardBackEnabled = false;
					break;
				case ELocatorHiddenStatus.HIDDEN:
					bCardBackEnabled = true;
					break;
				case ELocatorHiddenStatus.CUSTOM:
					bCardBackEnabled = agricolaAnimationLocator2.IsHidden(this);
					break;
				case ELocatorHiddenStatus.UNDEFINED:
					bCardBackEnabled = (elocatorHiddenStatus == ELocatorHiddenStatus.HIDDEN || (elocatorHiddenStatus == ELocatorHiddenStatus.CUSTOM && agricolaAnimationLocator.IsHidden(this)));
					break;
				}
				this.DisplayCardBack(bCardBackEnabled, false);
				ECardDisplayState displayState = agricolaAnimationLocator2.GetCardDisplayState();
				float num = agricolaAnimationLocator2.GetCardDisplayScale();
				if (this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_FULL && this.m_ActiveCardFront != null)
				{
					RectTransform component = this.m_ActiveCardFront.GetComponent<RectTransform>();
					if (component != null)
					{
						if (this.m_bMatchWidthOnActiveDisplay)
						{
							float width = component.rect.width;
							GameObject cardDisplay = this.GetCardDisplay(displayState, this.m_bShowingCardBack);
							if (cardDisplay != null)
							{
								RectTransform component2 = cardDisplay.GetComponent<RectTransform>();
								if (component2 != null)
								{
									float num2 = component2.rect.width * num / width;
									displayState = ECardDisplayState.DISPLAY_FULL;
									num = num2;
								}
							}
						}
						else if (this.m_bMatchHeightOnActiveDisplay)
						{
							float height = component.rect.height;
							GameObject cardDisplay2 = this.GetCardDisplay(displayState, this.m_bShowingCardBack);
							if (cardDisplay2 != null)
							{
								RectTransform component3 = cardDisplay2.GetComponent<RectTransform>();
								if (component3 != null)
								{
									float num3 = component3.rect.height * num / height;
									displayState = ECardDisplayState.DISPLAY_FULL;
									num = num3;
								}
							}
						}
					}
				}
				this.SetCardDisplayState(displayState, -1f);
				if (num > 0f)
				{
					if (base.transform.parent != null)
					{
						float x = agricolaAnimationLocator2.transform.lossyScale.x;
						float x2 = base.transform.parent.lossyScale.x;
						this.SetTargetScale(num * (x / x2));
						return;
					}
					this.SetTargetScale(num);
				}
				return;
			}
		}
		this.DisplayCardBack(elocatorHiddenStatus == ELocatorHiddenStatus.HIDDEN, false);
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000C1FC File Offset: 0x0000A3FC
	private void EndAnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		if (base.IsMagnifying())
		{
			base.FinishMagnify();
			if (this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_ICON)
			{
				if (!base.IsMagnified())
				{
					this.FinishUnmagnifyFromIcon();
				}
				return;
			}
		}
		AgricolaAnimationLocator agricolaAnimationLocator = destinationLocator as AgricolaAnimationLocator;
		if (agricolaAnimationLocator != null)
		{
			switch (agricolaAnimationLocator.HiddenToLocalPlayer())
			{
			case ELocatorHiddenStatus.VISIBLE:
				this.DisplayCardBack(false, true);
				break;
			case ELocatorHiddenStatus.HIDDEN:
				this.DisplayCardBack(true, true);
				break;
			case ELocatorHiddenStatus.CUSTOM:
			{
				bool bCardBackEnabled = agricolaAnimationLocator.IsHidden(this);
				this.DisplayCardBack(bCardBackEnabled, true);
				break;
			}
			}
			this.SetDragType(agricolaAnimationLocator.GetCardDragType());
			if (agricolaAnimationLocator.GetCardDisplayState() != ECardDisplayState.DISPLAY_NONE)
			{
				this.SetCardDisplayState(agricolaAnimationLocator.GetCardDisplayState(), agricolaAnimationLocator.GetCardDisplayScale());
			}
			DragObject component = base.GetComponent<DragObject>();
			if (component != null)
			{
				component.SetSendHorizontalDragToParentScrollRect(agricolaAnimationLocator.InterceptHorizontalCardDrag());
				component.SetSendVerticalDragToParentScrollRect(agricolaAnimationLocator.InterceptVerticalCardDrag());
			}
		}
		base.UpdateSelectionState(true, false);
		Transform parent = base.transform.parent;
		if (parent != null)
		{
			GameObject gameObject = parent.gameObject;
			if (gameObject != null)
			{
				gameObject.GetComponent<AgricolaBuilding>() != null;
				AgricolaActionSpace component2 = gameObject.GetComponent<AgricolaActionSpace>();
				if (component2 != null)
				{
					component2.UpdateSelectionState(true);
				}
			}
		}
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000C329 File Offset: 0x0000A529
	public bool IsCurrentlyDragging()
	{
		return this.m_bCurrentlyDragging;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000C334 File Offset: 0x0000A534
	public void SetDragType(ECardDragType dragType)
	{
		if (this.m_DragType == dragType)
		{
			return;
		}
		this.m_DragType = dragType;
		DragObject component = base.GetComponent<DragObject>();
		if (component != null)
		{
			if (this.m_bShowingCardBack)
			{
				component.SetIsDraggable(false);
				return;
			}
			switch (this.m_DragType)
			{
			case ECardDragType.Never:
				component.SetIsDraggable(false);
				return;
			case ECardDragType.Always:
				component.SetIsDraggable(true);
				return;
			case ECardDragType.Selectable:
				component.SetIsDraggable(base.IsSelectable());
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000C3A8 File Offset: 0x0000A5A8
	private void BeginDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = true;
		this.ShowFullCard(-1f);
		AgricolaCardManager agricolaCardManager = this.m_CardManager as AgricolaCardManager;
		this.SetTargetScale((agricolaCardManager != null) ? agricolaCardManager.GetDragCardScale() : 1f);
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
	private void EndDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = false;
		AgricolaGame component = GameObject.Find("/AgricolaGame").GetComponent<AgricolaGame>();
		if (eventData != null && eventData.pointerEnter != null)
		{
			ushort dragSelectionID = dragObject.GetDragSelectionID();
			ushort dragSelectionHint = dragObject.GetDragSelectionHint();
			if (dragSelectionID != 0 && dragSelectionHint != 0 && GameOptions.SelectOptionByInstanceIDAndHint(dragSelectionID, dragSelectionHint))
			{
				bool flag = false;
				DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)dragSelectionHint);
				if (dragSelectionHintDefinition != null)
				{
					if (dragSelectionHintDefinition.m_bUseTargetZoneInstanceID)
					{
						dragObject.SetDragSelectionOverrideID(0);
					}
					flag = dragSelectionHintDefinition.m_bDragReturnOnSelection;
				}
				if (!flag)
				{
					dragObject.ClearReturnToParent();
				}
			}
		}
		component.UpdateGameOptionsSelectionState(true);
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000C474 File Offset: 0x0000A674
	private void DragHintCallback(DragObject dragObject, PointerEventData eventData)
	{
		if (this.m_bCurrentlyDragging && dragObject != null)
		{
			AgricolaCardManager agricolaCardManager = this.m_CardManager as AgricolaCardManager;
			int dragSelectionHint = (int)dragObject.GetDragSelectionHint();
			if (dragSelectionHint != 0)
			{
				DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition(dragSelectionHint);
				if (dragSelectionHintDefinition != null)
				{
					this.SetSelectable(true, dragSelectionHintDefinition.m_HighlightColor);
					this.SetTargetScale((agricolaCardManager != null) ? agricolaCardManager.GetDragCardTargetScale() : 1f);
					return;
				}
			}
			this.SetSelectable(false, Color.white);
			this.SetTargetScale((agricolaCardManager != null) ? agricolaCardManager.GetDragCardScale() : 1f);
		}
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000C504 File Offset: 0x0000A704
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.m_ActiveDisplayState != ECardDisplayState.DISPLAY_ICON && !base.IsMagnifying())
		{
			if (base.IsMagnified())
			{
				this.Unmagnify(false, true);
				return;
			}
			this.Magnify(false, false);
		}
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000C532 File Offset: 0x0000A732
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_ICON)
		{
			this.OnHover(true);
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000C544 File Offset: 0x0000A744
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_ICON)
		{
			this.OnHover(false);
		}
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerUp(PointerEventData eventData)
	{
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000C556 File Offset: 0x0000A756
	public void OnHover(bool isOver)
	{
		if (this.m_ActiveDisplayState == ECardDisplayState.DISPLAY_ICON)
		{
			if (isOver && !base.IsMagnified())
			{
				this.MagnifyFromIcon();
				return;
			}
			if (!isOver && base.IsMagnified())
			{
				this.UnmagnifyFromIcon();
			}
		}
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000C584 File Offset: 0x0000A784
	public void SetTargetScale(float target_scale)
	{
		AnimateObject component = base.GetComponent<AnimateObject>();
		if (component != null)
		{
			component.SetTargetScale(target_scale);
		}
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000C5A8 File Offset: 0x0000A7A8
	public GameObject GetFullCardFront()
	{
		return this.m_CardFrontFullCard;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000C5B0 File Offset: 0x0000A7B0
	public GameObject GetHalfCardFront()
	{
		return this.m_CardFrontHalfCard;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000C5B8 File Offset: 0x0000A7B8
	public GameObject GetIconCardFront()
	{
		return this.m_CardFrontIconCard;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000C5C0 File Offset: 0x0000A7C0
	private GameObject GetCardDisplay(ECardDisplayState displayState, bool bCardBackEnabled)
	{
		switch (displayState)
		{
		case ECardDisplayState.DISPLAY_FULL:
			if (!bCardBackEnabled)
			{
				return this.m_CardFrontFullCard;
			}
			return this.m_CardBackFullCard;
		case ECardDisplayState.DISPLAY_HALF:
			if (!bCardBackEnabled)
			{
				return this.m_CardFrontHalfCard;
			}
			return this.m_CardBackHalfCard;
		case ECardDisplayState.DISPLAY_ICON:
			return this.m_CardFrontIconCard;
		default:
			return null;
		}
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000C610 File Offset: 0x0000A810
	private void SetActiveDisplay(GameObject cardFront, GameObject cardBack, GameObject cardGlow, float setCurrentScale = -1f)
	{
		AnimateObject component = base.GetComponent<AnimateObject>();
		if (component != null)
		{
			if (setCurrentScale >= 0f)
			{
				component.SetCurrentScale(setCurrentScale);
				component.SetTargetScale(setCurrentScale);
			}
			else if (this.m_ActiveCardFront != null && this.m_ActiveCardFront != cardFront)
			{
				RectTransform component2 = this.m_ActiveCardFront.GetComponent<RectTransform>();
				if (component2 != null)
				{
					if (this.m_bMatchWidthOnActiveDisplay)
					{
						float num = component2.rect.width * component.GetCurrentScale();
						if (cardFront != null)
						{
							RectTransform component3 = cardFront.GetComponent<RectTransform>();
							if (component3 != null)
							{
								float width = component3.rect.width;
								float num2 = num / width;
								component.SetCurrentScale(num2);
								component.SetTargetScale(num2);
							}
						}
					}
					else if (this.m_bMatchHeightOnActiveDisplay)
					{
						float num3 = component2.rect.height * component.GetCurrentScale();
						if (cardFront != null)
						{
							RectTransform component4 = cardFront.GetComponent<RectTransform>();
							if (component4 != null)
							{
								float height = component4.rect.height;
								float num4 = num3 / height;
								component.SetCurrentScale(num4);
								component.SetTargetScale(num4);
							}
						}
					}
				}
			}
		}
		if (this.m_ActiveGlowDisplay != null && this.m_ActiveGlowDisplay != cardGlow)
		{
			this.m_ActiveGlowDisplay.SetActive(false);
		}
		if (this.m_ActiveCardFront != null && this.m_ActiveCardFront != cardFront)
		{
			this.m_ActiveCardFront.SetActive(false);
		}
		if (this.m_ActiveCardBack != null && this.m_ActiveCardBack != cardBack)
		{
			this.m_ActiveCardBack.SetActive(false);
		}
		this.m_ActiveGlowDisplay = cardGlow;
		this.SetActiveGlow();
		this.m_ActiveCardFront = cardFront;
		if (this.m_ActiveCardFront != null)
		{
			LayoutElement component5 = base.GetComponent<LayoutElement>();
			if (component5 != null)
			{
				RectTransform component6 = this.m_ActiveCardFront.GetComponent<RectTransform>();
				if (component6 != null)
				{
					float width2 = component6.rect.width;
					float height2 = component6.rect.height;
					component5.minWidth = width2;
					component5.minHeight = height2;
					component5.preferredWidth = width2;
					component5.preferredHeight = height2;
				}
			}
			this.m_ActiveCardFront.SetActive(true);
		}
		this.m_ActiveCardBack = cardBack;
		if (this.m_ActiveCardBack != null)
		{
			this.m_ActiveCardBack.SetActive(true);
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000C888 File Offset: 0x0000AA88
	public void ShowFullCard(float setCurrentScale = -1f)
	{
		GameObject cardFrontFullCard = this.m_CardFrontFullCard;
		GameObject cardBackFullCard = this.m_CardBackFullCard;
		GameObject cardGlowFull = this.m_CardGlowFull;
		this.SetActiveDisplay(cardFrontFullCard, cardBackFullCard, cardGlowFull, setCurrentScale);
		this.m_ActiveDisplayState = ECardDisplayState.DISPLAY_FULL;
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000C8BC File Offset: 0x0000AABC
	public void ShowHalfCard(float setCurrentScale = -1f)
	{
		if (this.m_CardFrontHalfCard == null)
		{
			this.ShowFullCard(0.6f * setCurrentScale);
			return;
		}
		GameObject cardFrontHalfCard = this.m_CardFrontHalfCard;
		GameObject cardBackHalfCard = this.m_CardBackHalfCard;
		GameObject cardGlowHalf = this.m_CardGlowHalf;
		this.SetActiveDisplay(cardFrontHalfCard, cardBackHalfCard, cardGlowHalf, setCurrentScale);
		this.m_ActiveDisplayState = ECardDisplayState.DISPLAY_HALF;
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000C90C File Offset: 0x0000AB0C
	public void ShowIconCard(float setCurrentScale = -1f)
	{
		GameObject cardFrontIconCard = this.m_CardFrontIconCard;
		GameObject cardBackHalfCard = this.m_CardBackHalfCard;
		GameObject cardGlowHalf = this.m_CardGlowHalf;
		this.SetActiveDisplay(cardFrontIconCard, cardBackHalfCard, cardGlowHalf, setCurrentScale);
		this.m_ActiveDisplayState = ECardDisplayState.DISPLAY_ICON;
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000C940 File Offset: 0x0000AB40
	private void SetCardDisplayState(ECardDisplayState displayState, float setCurrentScale = -1f)
	{
		if (this.m_ActiveDisplayState == displayState)
		{
			if (setCurrentScale >= 0f)
			{
				AnimateObject component = base.GetComponent<AnimateObject>();
				if (component != null)
				{
					component.SetCurrentScale(setCurrentScale);
					component.SetTargetScale(setCurrentScale);
				}
			}
			return;
		}
		switch (displayState)
		{
		case ECardDisplayState.DISPLAY_FULL:
			this.ShowFullCard(setCurrentScale);
			return;
		case ECardDisplayState.DISPLAY_HALF:
			this.ShowHalfCard(setCurrentScale);
			return;
		case ECardDisplayState.DISPLAY_ICON:
			this.ShowIconCard(setCurrentScale);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000C9AC File Offset: 0x0000ABAC
	public void DisplayCardBack(bool bCardBackEnabled, bool bImmediate = false)
	{
		if (this.m_bShowingCardBack != bCardBackEnabled)
		{
			this.m_bShowingCardBack = bCardBackEnabled;
			switch (this.m_ActiveDisplayState)
			{
			case ECardDisplayState.DISPLAY_FULL:
				this.ShowFullCard(-1f);
				break;
			case ECardDisplayState.DISPLAY_HALF:
				this.ShowHalfCard(-1f);
				break;
			case ECardDisplayState.DISPLAY_ICON:
				this.ShowIconCard(-1f);
				break;
			}
			if (this.m_bShowingCardBack)
			{
				DragObject component = base.GetComponent<DragObject>();
				if (component != null)
				{
					component.SetIsDraggable(false);
				}
			}
			AnimateObject component2 = base.GetComponent<AnimateObject>();
			if (component2 != null)
			{
				Vector3 rotation = this.m_bShowingCardBack ? this.m_CardBackTargetRotation : Vector3.zero;
				component2.SetTargetInternalRotation(rotation, bImmediate);
			}
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000CA5B File Offset: 0x0000AC5B
	public override bool CanMagnify(bool bAllowShowingCardBack)
	{
		return !this.m_bShowingCardBack || bAllowShowingCardBack;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000CA68 File Offset: 0x0000AC68
	public void SetMajorImprovementOwnerToken(GameObject playerToken)
	{
		this.ClearMajorImprovementOwnerToken();
		if (this.m_MajorImpovementHHShadebox != null && this.m_MajorImpovementHHTokenLocator != null)
		{
			this.m_MajorImpovementHHShadebox.SetActive(true);
			playerToken.transform.SetParent(this.m_MajorImpovementHHTokenLocator.transform);
			playerToken.transform.localPosition = Vector3.zero;
			playerToken.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000CADC File Offset: 0x0000ACDC
	public void ClearMajorImprovementOwnerToken()
	{
		if (this.m_MajorImpovementHHShadebox != null && this.m_MajorImpovementHHTokenLocator != null)
		{
			this.m_MajorImpovementHHShadebox.SetActive(false);
			for (int i = 0; i < this.m_MajorImpovementHHTokenLocator.transform.childCount; i++)
			{
				Transform child = this.m_MajorImpovementHHTokenLocator.transform.GetChild(i);
				if (child != null)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000CB54 File Offset: 0x0000AD54
	public void MagnifyFromIcon()
	{
		base.SetIsMagnified(true);
		base.SetIsMagnifying(true);
		GameObject cardFrontFullCard = this.m_CardFrontFullCard;
		this.m_MagnifiedFromIconStartScale = cardFrontFullCard.transform.localScale.x;
		Transform child = cardFrontFullCard.transform.GetChild(0);
		if (child != null)
		{
			Image component = child.gameObject.GetComponent<Image>();
			if (component != null)
			{
				component.raycastTarget = false;
			}
		}
		LayoutElement layoutElement = cardFrontFullCard.GetComponent<LayoutElement>();
		LayoutElement component2 = base.GetComponent<LayoutElement>();
		if (layoutElement == null)
		{
			layoutElement = cardFrontFullCard.AddComponent<LayoutElement>();
		}
		layoutElement.minWidth = component2.minWidth;
		layoutElement.minHeight = component2.minHeight;
		layoutElement.preferredWidth = component2.preferredWidth;
		layoutElement.preferredHeight = component2.preferredHeight;
		layoutElement.flexibleWidth = 0f;
		layoutElement.flexibleHeight = 0f;
		layoutElement.enabled = false;
		DragObject dragObject = cardFrontFullCard.GetComponent<DragObject>();
		if (dragObject == null)
		{
			dragObject = cardFrontFullCard.AddComponent<DragObject>();
		}
		dragObject.SetIsDraggable(false);
		if (dragObject != null)
		{
			GameObject returnPlaceholder = dragObject.GetReturnPlaceholder(true);
			returnPlaceholder.transform.SetParent(cardFrontFullCard.transform.parent);
			returnPlaceholder.transform.SetSiblingIndex(cardFrontFullCard.transform.GetSiblingIndex());
			returnPlaceholder.transform.position = cardFrontFullCard.transform.position;
			returnPlaceholder.transform.localScale = cardFrontFullCard.transform.localScale;
		}
		this.m_MagnifiedFromTargetScale = 0.1f;
		AnimationLocator iconMagnifyLocator = this.m_CardManager.GetComponent<AgricolaCardManager>().GetIconMagnifyLocator();
		if (iconMagnifyLocator != null)
		{
			cardFrontFullCard.SetActive(true);
			AnimateObject component3 = cardFrontFullCard.GetComponent<AnimateObject>();
			if (component3 != null)
			{
				component3.AddOnEndAnimationCallback(new AnimateObject.AnimationCallback(this.EndAnimationCallback));
				component3.SetCurrentScale(0.1f);
				component3.SetTargetScale(1f);
				AnimationManager animationManager = component3.GetAnimationManager();
				if (animationManager == null)
				{
					component3.SetAnimationManager(base.GetComponent<AnimateObject>().GetAnimationManager());
					animationManager = component3.GetAnimationManager();
				}
				if (animationManager != null)
				{
					AnimationLayer defaultAnimationLayer = animationManager.GetDefaultAnimationLayer();
					if (defaultAnimationLayer != null)
					{
						dragObject.transform.SetParent(defaultAnimationLayer.gameObject.transform);
					}
					iconMagnifyLocator.PlaceAnimateObject(component3, true, true, false);
					cardFrontFullCard.transform.localScale = Vector3.one;
				}
			}
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
	public void UnmagnifyFromIcon()
	{
		base.SetIsMagnified(false);
		base.SetIsMagnifying(true);
		this.SetCardDisplayState(ECardDisplayState.DISPLAY_ICON, -1f);
		Transform child = this.m_CardFrontFullCard.transform.GetChild(0);
		if (child != null)
		{
			Image component = child.gameObject.GetComponent<Image>();
			if (component != null)
			{
				component.raycastTarget = false;
			}
		}
		DragObject component2 = this.m_CardFrontFullCard.GetComponent<DragObject>();
		if (component2 != null)
		{
			this.m_CardFrontFullCard.transform.SetParent(base.transform);
			this.m_CardFrontFullCard.transform.SetSiblingIndex(component2.GetReturnPlaceholder(true).transform.GetSiblingIndex());
			component2.DestroyReturnPlaceholder();
			this.FinishUnmagnifyFromIcon();
		}
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000CE60 File Offset: 0x0000B060
	public void FinishUnmagnifyFromIcon()
	{
		base.SetIsMagnifying(false);
		base.SetIsMagnified(false);
		GameObject cardFrontFullCard = this.m_CardFrontFullCard;
		cardFrontFullCard.transform.localScale = new Vector3(this.m_MagnifiedFromIconStartScale, this.m_MagnifiedFromIconStartScale, 1f);
		DragObject component = cardFrontFullCard.GetComponent<DragObject>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		cardFrontFullCard.SetActive(false);
	}

	// Token: 0x04000185 RID: 389
	public const ushort k_cardFlagMajorImprovement = 1;

	// Token: 0x04000186 RID: 390
	public const ushort k_cardFlagPassToLeft = 16;

	// Token: 0x04000187 RID: 391
	public const ushort k_cardFlagHasResourceCost = 32;

	// Token: 0x04000188 RID: 392
	public const ushort k_cardFlagBonusPoints = 64;

	// Token: 0x04000189 RID: 393
	[SerializeField]
	private GameObject m_CardFrontFullCard;

	// Token: 0x0400018A RID: 394
	[SerializeField]
	private GameObject m_CardFrontHalfCard;

	// Token: 0x0400018B RID: 395
	[SerializeField]
	private GameObject m_CardFrontIconCard;

	// Token: 0x0400018C RID: 396
	[SerializeField]
	private GameObject m_CardBackFullCard;

	// Token: 0x0400018D RID: 397
	[SerializeField]
	private GameObject m_CardBackHalfCard;

	// Token: 0x0400018E RID: 398
	[SerializeField]
	private GameObject m_CardGlowFull;

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private GameObject m_CardGlowHalf;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	private GameObject m_CardBackRotation;

	// Token: 0x04000191 RID: 401
	[SerializeField]
	private GameObject m_CardFrontParent;

	// Token: 0x04000192 RID: 402
	[SerializeField]
	private AgricolaCardLayout m_LayoutCardFullFront;

	// Token: 0x04000193 RID: 403
	[SerializeField]
	private AgricolaCardLayout m_LayoutCardHalfFront;

	// Token: 0x04000194 RID: 404
	[SerializeField]
	private AgricolaCardLayout m_LayoutCardIconFront;

	// Token: 0x04000195 RID: 405
	private GameObject m_MajorImpovementHHShadebox;

	// Token: 0x04000196 RID: 406
	private GameObject m_MajorImpovementHHTokenLocator;

	// Token: 0x04000197 RID: 407
	private ResourceEntry m_ResourceCardImage;

	// Token: 0x04000198 RID: 408
	private ECardDisplayState m_ActiveDisplayState;

	// Token: 0x04000199 RID: 409
	private bool m_bShowingCardBack;

	// Token: 0x0400019A RID: 410
	private Vector3 m_CardBackTargetRotation = new Vector3(0f, 180f, 180f);

	// Token: 0x0400019B RID: 411
	private float m_MagnifiedFromIconStartScale;

	// Token: 0x0400019C RID: 412
	private float m_MagnifiedFromTargetScale = 1f;

	// Token: 0x0400019D RID: 413
	private bool m_bGlowActive;

	// Token: 0x0400019E RID: 414
	private bool m_bOverrideGlowColor;

	// Token: 0x0400019F RID: 415
	private Color m_GlowColor = Color.white;

	// Token: 0x040001A0 RID: 416
	private GameObject m_ActiveCardFront;

	// Token: 0x040001A1 RID: 417
	private GameObject m_ActiveCardBack;

	// Token: 0x040001A2 RID: 418
	private GameObject m_ActiveGlowDisplay;

	// Token: 0x040001A3 RID: 419
	private bool m_bMatchWidthOnActiveDisplay;

	// Token: 0x040001A4 RID: 420
	private bool m_bMatchHeightOnActiveDisplay;

	// Token: 0x040001A5 RID: 421
	private ECardDragType m_DragType = ECardDragType.Always;

	// Token: 0x040001A6 RID: 422
	private bool m_bCurrentlyDragging;

	// Token: 0x040001A7 RID: 423
	private string m_CardName;

	// Token: 0x040001A8 RID: 424
	private int m_CardType;

	// Token: 0x040001A9 RID: 425
	private short m_CardOrchardRow;

	// Token: 0x040001AA RID: 426
	private short m_CardOrchardSize;

	// Token: 0x040001AB RID: 427
	private short m_IntrigueType;

	// Token: 0x040001AC RID: 428
	private uint m_DeckNumber;

	// Token: 0x040001AD RID: 429
	private uint m_CardNumber;

	// Token: 0x040001AE RID: 430
	private int m_UniqueIndex = -1;
}
