using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000042 RID: 66
public abstract class AgricolaImprovementBase : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x060003C6 RID: 966
	public abstract void DragStartCallback(DragObject dragObject);

	// Token: 0x060003C7 RID: 967
	public abstract void DragEndCallback(DragObject dragObject);

	// Token: 0x060003C8 RID: 968
	public abstract void OnDropCallback(DragObject dragObject, ushort selectionHint);

	// Token: 0x060003C9 RID: 969 RVA: 0x0001A8E5 File Offset: 0x00018AE5
	public void SetLinkedCard(AgricolaCard card, AgricolaCardInPlay cardInPlay)
	{
		this.m_linkedCard = card;
		this.m_linkedCardInPlay = cardInPlay;
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001A8F8 File Offset: 0x00018AF8
	public void SetDragDropCallbacks(DragManager dragManager)
	{
		if (dragManager != null)
		{
			dragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.DragStartCallback));
			dragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.DragEndCallback));
			this.m_dragManager = dragManager;
		}
		if (this.m_dragTarget != null)
		{
			this.m_dragTarget.AddOnDropCallback(new DragTargetZone.OnDropCallback(this.OnDropCallback));
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001A964 File Offset: 0x00018B64
	public void OnDestroy()
	{
		if (this.m_dragManager != null)
		{
			this.m_dragManager.RemoveOnBeginDragCallback(new DragManager.DragManagerCallback(this.DragStartCallback));
			this.m_dragManager.RemoveOnEndDragCallback(new DragManager.DragManagerCallback(this.DragEndCallback));
		}
		if (this.m_dragTarget != null)
		{
			this.m_dragTarget.RemoveOnDropCallback(new DragTargetZone.OnDropCallback(this.OnDropCallback));
		}
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void UpdateFieldData(int resType, int resCount, int sowingIndex)
	{
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void UpdatePastureData(int sheepCount, int boarCount, int cattleCount)
	{
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void UpdateSelectionState(bool bHighlight)
	{
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001A9D5 File Offset: 0x00018BD5
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.m_linkedCard != null)
		{
			this.m_linkedCard.OnPointerClick(eventData);
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0001A9F1 File Offset: 0x00018BF1
	public void SetGlowActive(bool bActive)
	{
		if (this.m_glowObj != null)
		{
			this.m_glowObj.SetActive(bActive);
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001AA0D File Offset: 0x00018C0D
	public void SetDragSelectionHint(ushort hint, Color hintColor, ushort overrideID = 0)
	{
		if (this.m_dragTarget != null)
		{
			this.m_dragTarget.SetDragSelectionHint(hint, hintColor, overrideID);
		}
	}

	// Token: 0x04000310 RID: 784
	[SerializeField]
	private GameObject m_glowObj;

	// Token: 0x04000311 RID: 785
	[SerializeField]
	private DragTargetZone m_dragTarget;

	// Token: 0x04000312 RID: 786
	protected AgricolaCard m_linkedCard;

	// Token: 0x04000313 RID: 787
	protected AgricolaCardInPlay m_linkedCardInPlay;

	// Token: 0x04000314 RID: 788
	private DragManager m_dragManager;
}
