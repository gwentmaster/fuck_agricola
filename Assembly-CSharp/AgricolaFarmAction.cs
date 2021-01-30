using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000037 RID: 55
[RequireComponent(typeof(DragObject))]
public class AgricolaFarmAction : MonoBehaviour
{
	// Token: 0x060002DA RID: 730 RVA: 0x000134DC File Offset: 0x000116DC
	public void SetWorkerManager(AgricolaWorkerManager worker_manager)
	{
		this.m_WorkerManager = worker_manager;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x000134E8 File Offset: 0x000116E8
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

	// Token: 0x060002DC RID: 732 RVA: 0x0001356F File Offset: 0x0001176F
	public ushort GetFarmActionHint()
	{
		return this.m_FarmActionHint;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00013577 File Offset: 0x00011777
	public void SetFarmActionHint(ushort hint)
	{
		this.m_FarmActionHint = hint;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00013580 File Offset: 0x00011780
	public int GetOwnerInstanceID()
	{
		return this.m_OwnerInstanceID;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00013588 File Offset: 0x00011788
	public int GetFactionIndex()
	{
		return this.m_FactionIndex;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00013590 File Offset: 0x00011790
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

	// Token: 0x060002E1 RID: 737 RVA: 0x000135C1 File Offset: 0x000117C1
	public void SetAvatar(int avatarIndex)
	{
		if (this.m_WorkerAvatar != null)
		{
			this.m_WorkerAvatar.SetAvatar(avatarIndex, false);
		}
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000135DE File Offset: 0x000117DE
	public bool IsSelectable()
	{
		return this.m_bSelectable;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000135E8 File Offset: 0x000117E8
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

	// Token: 0x060002E4 RID: 740 RVA: 0x00013674 File Offset: 0x00011874
	public void UpdateSelectionState(bool bHighlight)
	{
		if (this.m_FarmActionHint == 0 || !bHighlight)
		{
			this.SetSelectable(false, Color.white);
			return;
		}
		DragObject component = base.GetComponent<DragObject>();
		if (component != null)
		{
			component.SetDragSelectionOverrideID(0);
		}
		if (GameOptions.IsSelectableHint(this.m_FarmActionHint))
		{
			DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)this.m_FarmActionHint);
			if (dragSelectionHintDefinition != null)
			{
				this.SetSelectable(true, dragSelectionHintDefinition.m_HighlightColor);
				return;
			}
		}
		this.SetSelectable(false, Color.white);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x000136E8 File Offset: 0x000118E8
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

	// Token: 0x060002E6 RID: 742 RVA: 0x00013778 File Offset: 0x00011978
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

	// Token: 0x060002E7 RID: 743 RVA: 0x000137CE File Offset: 0x000119CE
	public bool IsCurrentlyDragging()
	{
		return this.m_bCurrentlyDragging;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x000137D8 File Offset: 0x000119D8
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

	// Token: 0x060002E9 RID: 745 RVA: 0x0001383C File Offset: 0x00011A3C
	private void BeginDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = true;
		this.SetTargetScale((this.m_WorkerManager != null) ? this.m_WorkerManager.GetDragWorkerTargetScale() : 1f);
		if (dragObject != null)
		{
			dragObject.SetDragSelectionHint(0);
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0001387C File Offset: 0x00011A7C
	private void EndDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.m_bCurrentlyDragging = false;
		if (eventData != null && eventData.pointerEnter != null)
		{
			ushort dragSelectionID = dragObject.GetDragSelectionID();
			ushort dragSelectionHint = dragObject.GetDragSelectionHint();
			if (dragSelectionID != 0 && dragSelectionHint != 0)
			{
				bool flag;
				if (dragSelectionHint == 40977 || dragSelectionHint == 40976 || dragSelectionHint == 40980)
				{
					flag = GameOptions.SelectOptionByHintWithData(dragSelectionHint, (uint)(dragSelectionID - 1));
				}
				else if (dragSelectionHint == 40984)
				{
					flag = GameOptions.SelectOptionByHintWithData(dragSelectionHint, 1U << (int)(dragSelectionID - 1));
				}
				else
				{
					flag = GameOptions.SelectOptionByInstanceIDAndHint(dragSelectionID, dragSelectionHint);
				}
				if (flag)
				{
					dragObject.ClearReturnToParent();
				}
			}
		}
		if (dragObject != null)
		{
			dragObject.SetDragSelectionHint(0);
		}
		this.UpdateSelectionState(true);
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001391C File Offset: 0x00011B1C
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

	// Token: 0x060002EC RID: 748 RVA: 0x0001396C File Offset: 0x00011B6C
	public void SetTargetScale(float target_scale)
	{
		AnimateObject component = base.GetComponent<AnimateObject>();
		if (component != null)
		{
			component.SetTargetScale(target_scale);
		}
	}

	// Token: 0x0400023D RID: 573
	[SerializeField]
	private ColorByFaction m_Colorizer;

	// Token: 0x0400023E RID: 574
	[SerializeField]
	private Avatar_UI m_WorkerAvatar;

	// Token: 0x0400023F RID: 575
	[SerializeField]
	private GameObject m_WorkerGlow;

	// Token: 0x04000240 RID: 576
	private ushort m_FarmActionHint;

	// Token: 0x04000241 RID: 577
	private int m_OwnerInstanceID;

	// Token: 0x04000242 RID: 578
	private int m_FactionIndex;

	// Token: 0x04000243 RID: 579
	private Color m_FactionColor;

	// Token: 0x04000244 RID: 580
	private bool m_bSelectable;

	// Token: 0x04000245 RID: 581
	private ECardDragType m_DragType = ECardDragType.Always;

	// Token: 0x04000246 RID: 582
	private bool m_bCurrentlyDragging;

	// Token: 0x04000247 RID: 583
	private AgricolaWorkerManager m_WorkerManager;
}
