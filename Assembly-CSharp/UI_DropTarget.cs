using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000108 RID: 264
public class UI_DropTarget : MonoBehaviour, IDropHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x06000A09 RID: 2569 RVA: 0x00043332 File Offset: 0x00041532
	protected virtual void Start()
	{
		if (this.reparentTarget == null)
		{
			this.reparentTarget = base.transform;
		}
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0004334E File Offset: 0x0004154E
	public void OnEnable()
	{
		this.currentTargetColor = this.normalColor;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0004335C File Offset: 0x0004155C
	public void OnDrop(PointerEventData data)
	{
		this.currentTargetColor = this.normalColor;
		this.m_OnColorChangeEvent.Invoke(this, this.currentTargetColor);
		this.m_OnPointerExitEvent.Invoke(this);
		UI_DragSource droppedObject = this.GetDroppedObject(data);
		if (droppedObject != null)
		{
			this.m_OnDropEvent.Invoke(droppedObject, this);
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x000433B1 File Offset: 0x000415B1
	public void OnPointerEnter(PointerEventData data)
	{
		if (this.GetDroppedObject(data) != null)
		{
			this.currentTargetColor = this.highlightColor;
			this.m_OnColorChangeEvent.Invoke(this, this.currentTargetColor);
			this.m_OnPointerEnterEvent.Invoke(this);
		}
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x000433EC File Offset: 0x000415EC
	public void OnPointerExit(PointerEventData data)
	{
		this.currentTargetColor = this.normalColor;
		this.m_OnColorChangeEvent.Invoke(this, this.currentTargetColor);
		this.m_OnPointerExitEvent.Invoke(this);
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00043418 File Offset: 0x00041618
	private UI_DragSource GetDroppedObject(PointerEventData data)
	{
		GameObject pointerDrag = data.pointerDrag;
		if (pointerDrag == null)
		{
			return null;
		}
		UI_DragSource component = pointerDrag.GetComponent<UI_DragSource>();
		if (component == null)
		{
			return null;
		}
		return component;
	}

	// Token: 0x04000A9B RID: 2715
	public GameObject targetObject;

	// Token: 0x04000A9C RID: 2716
	public Color normalColor = Color.white;

	// Token: 0x04000A9D RID: 2717
	public Color highlightColor = Color.yellow;

	// Token: 0x04000A9E RID: 2718
	private Color currentTargetColor;

	// Token: 0x04000A9F RID: 2719
	public Transform reparentTarget;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private UI_DropTarget.DropObjectEvent m_OnDropEvent = new UI_DropTarget.DropObjectEvent();

	// Token: 0x04000AA1 RID: 2721
	[SerializeField]
	private UI_DropTarget.TargetColorChangeEvent m_OnColorChangeEvent = new UI_DropTarget.TargetColorChangeEvent();

	// Token: 0x04000AA2 RID: 2722
	[SerializeField]
	private UI_DropTarget.PointerEnterEvent m_OnPointerEnterEvent = new UI_DropTarget.PointerEnterEvent();

	// Token: 0x04000AA3 RID: 2723
	[SerializeField]
	private UI_DropTarget.PointerExitEvent m_OnPointerExitEvent = new UI_DropTarget.PointerExitEvent();

	// Token: 0x020007DE RID: 2014
	[Serializable]
	public class DropObjectEvent : UnityEvent<UI_DragSource, UI_DropTarget>
	{
	}

	// Token: 0x020007DF RID: 2015
	[Serializable]
	public class TargetColorChangeEvent : UnityEvent<UI_DropTarget, Color>
	{
	}

	// Token: 0x020007E0 RID: 2016
	[Serializable]
	public class PointerEnterEvent : UnityEvent<UI_DropTarget>
	{
	}

	// Token: 0x020007E1 RID: 2017
	[Serializable]
	public class PointerExitEvent : UnityEvent<UI_DropTarget>
	{
	}
}
