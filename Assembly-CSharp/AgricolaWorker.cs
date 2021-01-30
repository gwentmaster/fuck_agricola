using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200004E RID: 78
[RequireComponent(typeof(DragObject))]
public class AgricolaWorker : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x06000464 RID: 1124 RVA: 0x00022F11 File Offset: 0x00021111
	public void SetWorkerManager(AgricolaWorkerManager worker_manager)
	{
		this.m_WorkerManager = worker_manager;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x00022F1C File Offset: 0x0002111C
	private void Awake()
	{
		AnimateObject component = base.GetComponent<AnimateObject>();
		if (component != null)
		{
			component.AddOnBeginAnimationCallback(new AnimateObject.AnimationCallback(this.BeginAnimationCallback));
			component.AddOnEndAnimationCallback(new AnimateObject.AnimationCallback(this.EndAnimationCallback));
		}
		DragObject component2 = base.GetComponent<DragObject>();
		if (component2 != null)
		{
			component2.AddOnBeginDragCallback(new DragObject.DragObjectCallback(this.BeginDragCallback));
			component2.AddOnEndDragCallback(new DragObject.DragObjectCallback(this.EndDragCallback));
			component2.AddOnDragHintCallback(new DragObject.DragObjectCallback(this.DragHintCallback));
		}
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00022FA3 File Offset: 0x000211A3
	public int GetWorkerInstanceID()
	{
		return this.m_WorkerInstanceID;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00022FAC File Offset: 0x000211AC
	public void SetWorkerInstanceID(int instanceID)
	{
		this.m_WorkerInstanceID = instanceID;
		DragObject component = base.GetComponent<DragObject>();
		if (component != null)
		{
			component.SetDragSelectionID((ushort)instanceID);
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00022FD8 File Offset: 0x000211D8
	public bool IsTemporaryWorker()
	{
		return this.m_bTemporaryWorker;
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00022FE0 File Offset: 0x000211E0
	public void SetTemporaryWorker(bool bTemporary)
	{
		this.m_bTemporaryWorker = bTemporary;
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00022FE9 File Offset: 0x000211E9
	public bool IsAmbassdor()
	{
		return this.m_bAmbassdor;
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x00022FF1 File Offset: 0x000211F1
	public void SetAmbassdor(bool bAmbassdor)
	{
		this.m_bAmbassdor = bAmbassdor;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00022FFA File Offset: 0x000211FA
	public int GetOwnerInstanceID()
	{
		return this.m_OwnerInstanceID;
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00023002 File Offset: 0x00021202
	public int GetFactionIndex()
	{
		return this.m_FactionIndex;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0002300A File Offset: 0x0002120A
	public void SetOwner(int ownerInstanceID, int factionIndex, int avatarIndex)
	{
		this.m_OwnerInstanceID = ownerInstanceID;
		this.m_FactionIndex = factionIndex;
		if (this.m_Colorizer != null)
		{
			this.m_Colorizer.Colorize((uint)factionIndex);
		}
		this.SetAvatar(avatarIndex);
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0002303B File Offset: 0x0002123B
	public void SetAvatar(int avatarIndex)
	{
		if (this.m_WorkerAvatar != null)
		{
			this.m_WorkerAvatar.SetAvatar(avatarIndex, false);
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00023058 File Offset: 0x00021258
	public bool IsSelectable()
	{
		return this.m_bSelectable;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00023060 File Offset: 0x00021260
	public void SetSelectable(bool bSelectable, Color highlightColor)
	{
		this.m_bSelectable = bSelectable;
		DragObject component = base.GetComponent<DragObject>();
		if (component != null && this.m_DragType == ECardDragType.Selectable)
		{
			component.SetIsDraggable(this.m_bSelectable);
		}
		if (this.m_WorkerGlow != null)
		{
			if (this.m_bSelectable)
			{
				Image component2 = this.m_WorkerGlow.GetComponent<Image>();
				if (component2 != null)
				{
					component2.color = highlightColor;
				}
				this.m_WorkerGlow.SetActive(this.m_bSelectable);
				return;
			}
			this.m_WorkerGlow.SetActive(false);
		}
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000230EC File Offset: 0x000212EC
	public void UpdateSelectionState(bool bHighlight)
	{
		if (this.m_WorkerInstanceID == 0 || !bHighlight)
		{
			this.SetSelectable(false, Color.white);
			return;
		}
		DragObject component = base.GetComponent<DragObject>();
		if (component != null)
		{
			component.SetDragSelectionOverrideID(0);
		}
		foreach (ushort selectionHint in GameOptions.GetSelectionHints((ushort)this.m_WorkerInstanceID))
		{
			DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)selectionHint);
			if (dragSelectionHintDefinition != null)
			{
				this.SetSelectable(true, dragSelectionHintDefinition.m_HighlightColor);
				return;
			}
		}
		this.SetSelectable(false, Color.white);
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x00023190 File Offset: 0x00021390
	public void SetShadeOverlayVisible(bool bActive)
	{
		if (this.m_ShadeCover != null)
		{
			this.m_ShadeCover.SetActive(bActive);
		}
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x000231AC File Offset: 0x000213AC
	public void SelectWorker()
	{
		ushort instanceID = (ushort)this.GetWorkerInstanceID();
		if (GameOptions.IsSelectableInstanceID(instanceID))
		{
			GameOptions.SelectOptionByInstanceID(instanceID);
		}
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00003022 File Offset: 0x00001222
	public void OnPointerUp(PointerEventData eventData)
	{
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000231D0 File Offset: 0x000213D0
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.IsSelectable())
		{
			this.SelectWorker();
		}
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x000231E0 File Offset: 0x000213E0
	private void BeginAnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		sourceLocator as AgricolaAnimationLocator != null;
		AgricolaAnimationLocator agricolaAnimationLocator = destinationLocator as AgricolaAnimationLocator;
		if (agricolaAnimationLocator != null && agricolaAnimationLocator.GetCardDisplayState() != ECardDisplayState.DISPLAY_NONE)
		{
			float cardDisplayScale = agricolaAnimationLocator.GetCardDisplayScale();
			if (cardDisplayScale > 0f)
			{
				if (base.transform.parent != null)
				{
					float x = agricolaAnimationLocator.transform.lossyScale.x;
					float x2 = base.transform.parent.lossyScale.x;
					this.SetTargetScale(cardDisplayScale * (x / x2));
					return;
				}
				this.SetTargetScale(cardDisplayScale);
			}
			return;
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00023270 File Offset: 0x00021470
	private void EndAnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		AgricolaAnimationLocator agricolaAnimationLocator = destinationLocator as AgricolaAnimationLocator;
		if (agricolaAnimationLocator != null)
		{
			this.SetDragType(agricolaAnimationLocator.GetCardDragType());
			if (agricolaAnimationLocator.GetCardDisplayState() != ECardDisplayState.DISPLAY_NONE)
			{
				float cardDisplayScale = agricolaAnimationLocator.GetCardDisplayScale();
				AnimateObject component = base.GetComponent<AnimateObject>();
				if (component != null)
				{
					component.SetTargetScale(cardDisplayScale);
					component.SetCurrentScale(cardDisplayScale);
				}
			}
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x000232C6 File Offset: 0x000214C6
	public bool IsCurrentlyDragging()
	{
		return this.m_bCurrentlyDragging;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000232D0 File Offset: 0x000214D0
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
			switch (this.m_DragType)
			{
			case ECardDragType.Never:
				component.SetIsDraggable(false);
				return;
			case ECardDragType.Always:
				component.SetIsDraggable(true);
				return;
			case ECardDragType.Selectable:
				component.SetIsDraggable(this.IsSelectable());
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00023334 File Offset: 0x00021534
	private void BeginDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = true;
		this.SetTargetScale((this.m_WorkerManager != null) ? this.m_WorkerManager.GetDragWorkerTargetScale() : 1f);
		if (dragObject != null)
		{
			dragObject.SetDragSelectionHint(0);
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00023374 File Offset: 0x00021574
	private void EndDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = false;
		if (eventData != null && eventData.pointerEnter != null)
		{
			ushort dragSelectionID = dragObject.GetDragSelectionID();
			ushort dragSelectionHint = dragObject.GetDragSelectionHint();
			if (dragSelectionID != 0 && dragSelectionHint != 0 && GameOptions.SelectOptionByInstanceIDAndHint(dragSelectionID, dragSelectionHint))
			{
				dragObject.ClearReturnToParent();
				if (this.IsTemporaryWorker())
				{
					AnimateObject component = base.GetComponent<AnimateObject>();
					if (component != null)
					{
						AnimationManager animationManager = component.GetAnimationManager();
						if (animationManager != null && animationManager.StartAnimation(component, 9, this.m_OwnerInstanceID, 21, (int)dragSelectionID, 0U, Vector3.zero, 0f, 0f, true))
						{
							AgricolaAnimationManager agricolaAnimationManager = animationManager as AgricolaAnimationManager;
							if (agricolaAnimationManager != null)
							{
								agricolaAnimationManager.SetAnimationRatesWorker(component);
							}
							component.SetDestroyAfterAnimation();
						}
					}
				}
			}
		}
		if (dragObject != null)
		{
			dragObject.SetDragSelectionHint(0);
		}
		this.UpdateSelectionState(true);
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00023448 File Offset: 0x00021648
	private void DragHintCallback(DragObject dragObject, PointerEventData eventData)
	{
		if (this.m_bCurrentlyDragging && dragObject != null)
		{
			int dragSelectionHint = (int)dragObject.GetDragSelectionHint();
			if (dragSelectionHint == 0)
			{
				this.SetSelectable(false, Color.white);
				return;
			}
			DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition(dragSelectionHint);
			if (dragSelectionHintDefinition != null)
			{
				this.SetSelectable(true, dragSelectionHintDefinition.m_HighlightColor);
				return;
			}
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00023498 File Offset: 0x00021698
	public void SetTargetScale(float target_scale)
	{
		AnimateObject component = base.GetComponent<AnimateObject>();
		if (component != null)
		{
			component.SetTargetScale(target_scale);
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x000234BC File Offset: 0x000216BC
	public void AddFeedingBubble(AgricolaFeedingBubble bubble)
	{
		if (this.m_feedingBubble == null && this.m_FeedingBubbleLocator != null)
		{
			this.m_feedingBubble = bubble;
			this.m_feedingBubble.transform.SetParent(this.m_FeedingBubbleLocator.transform);
			this.m_feedingBubble.transform.localPosition = Vector3.zero;
			this.m_feedingBubble.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00023531 File Offset: 0x00021731
	public void SetFeedingBubbleVisible(bool bVisible)
	{
		if (this.m_FeedingBubbleLocator != null)
		{
			this.m_FeedingBubbleLocator.SetActive(bVisible);
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0002354D File Offset: 0x0002174D
	public AgricolaFeedingBubble GetFeedingBubble()
	{
		return this.m_feedingBubble;
	}

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private ColorByFaction m_Colorizer;

	// Token: 0x040003F9 RID: 1017
	[SerializeField]
	private Avatar_UI m_WorkerAvatar;

	// Token: 0x040003FA RID: 1018
	[SerializeField]
	private GameObject m_WorkerGlow;

	// Token: 0x040003FB RID: 1019
	[SerializeField]
	private GameObject m_ShadeCover;

	// Token: 0x040003FC RID: 1020
	[SerializeField]
	private GameObject m_FeedingBubbleLocator;

	// Token: 0x040003FD RID: 1021
	private int m_WorkerInstanceID;

	// Token: 0x040003FE RID: 1022
	private bool m_bTemporaryWorker;

	// Token: 0x040003FF RID: 1023
	private bool m_bAmbassdor;

	// Token: 0x04000400 RID: 1024
	private int m_OwnerInstanceID;

	// Token: 0x04000401 RID: 1025
	private int m_FactionIndex;

	// Token: 0x04000402 RID: 1026
	private Color m_FactionColor;

	// Token: 0x04000403 RID: 1027
	private bool m_bSelectable;

	// Token: 0x04000404 RID: 1028
	private ECardDragType m_DragType = ECardDragType.Always;

	// Token: 0x04000405 RID: 1029
	private bool m_bCurrentlyDragging;

	// Token: 0x04000406 RID: 1030
	private AgricolaWorkerManager m_WorkerManager;

	// Token: 0x04000407 RID: 1031
	private AgricolaFeedingBubble m_feedingBubble;
}
