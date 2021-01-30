using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000D2 RID: 210
public class DragTargetZone : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDropHandler
{
	// Token: 0x060007D7 RID: 2007 RVA: 0x0003818D File Offset: 0x0003638D
	public void AddOnDropCallback(DragTargetZone.OnDropCallback callback)
	{
		this.m_OnDropCallback = (DragTargetZone.OnDropCallback)Delegate.Combine(this.m_OnDropCallback, callback);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x000381A6 File Offset: 0x000363A6
	public void RemoveOnDropCallback(DragTargetZone.OnDropCallback callback)
	{
		this.m_OnDropCallback = (DragTargetZone.OnDropCallback)Delegate.Remove(this.m_OnDropCallback, callback);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x000381BF File Offset: 0x000363BF
	public void Awake()
	{
		this.SetDragSelectionHint(this.m_DragSelectionHint, Color.white, this.m_DragSelectionOverrideID);
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x000381D8 File Offset: 0x000363D8
	public ushort GetDragTargetInstanceID()
	{
		return this.m_DragTargetInstanceID;
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x000381E0 File Offset: 0x000363E0
	public void SetDragTargetInstanceID(ushort instance_id)
	{
		this.m_DragTargetInstanceID = instance_id;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x000381EC File Offset: 0x000363EC
	public void SetDragSelectionHint(ushort hint, Color hintColor, ushort overrideID = 0)
	{
		this.m_DragSelectionHint = hint;
		this.m_DragSelectionOverrideID = overrideID;
		bool active = this.m_DragSelectionHint > 0;
		if (this.m_TargetGlowObject != null)
		{
			this.m_TargetGlowObject.SetActive(active);
			GlowColorAdjust component = this.m_TargetGlowObject.GetComponent<GlowColorAdjust>();
			if (component != null)
			{
				component.SetColor(hintColor);
			}
		}
		if (this.m_ActivateList != null)
		{
			for (int i = 0; i < this.m_ActivateList.Length; i++)
			{
				this.m_ActivateList[i].SetActive(active);
			}
		}
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00038270 File Offset: 0x00036470
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			DragObject component = eventData.pointerDrag.GetComponent<DragObject>();
			if (component != null && this.m_DragSelectionHint != 0)
			{
				component.SetDragSelectionHint(this.m_DragSelectionHint);
			}
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x000382B4 File Offset: 0x000364B4
	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			DragObject component = eventData.pointerDrag.GetComponent<DragObject>();
			if (component != null && component.GetDragSelectionHint() == this.m_DragSelectionHint)
			{
				component.SetDragSelectionHint(0);
			}
		}
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x000382FC File Offset: 0x000364FC
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}
		if (this.m_DragSelectionHint != 0)
		{
			DragObject component = eventData.pointerDrag.GetComponent<DragObject>();
			if (component != null)
			{
				if (this.m_DragSelectionOverrideID != 0)
				{
					component.SetDragSelectionOverrideID(this.m_DragSelectionOverrideID);
				}
				component.SetDragSelectionHint(this.m_DragSelectionHint);
				if (this.m_OnDropCallback != null)
				{
					this.m_OnDropCallback(component, this.m_DragSelectionHint);
				}
			}
		}
	}

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private ushort m_DragTargetInstanceID;

	// Token: 0x040008DB RID: 2267
	private ushort m_DragSelectionHint;

	// Token: 0x040008DC RID: 2268
	private ushort m_DragSelectionOverrideID;

	// Token: 0x040008DD RID: 2269
	[SerializeField]
	private GameObject m_TargetGlowObject;

	// Token: 0x040008DE RID: 2270
	[SerializeField]
	private GameObject[] m_ActivateList;

	// Token: 0x040008DF RID: 2271
	private DragTargetZone.OnDropCallback m_OnDropCallback;

	// Token: 0x0200079D RID: 1949
	// (Invoke) Token: 0x0600429F RID: 17055
	public delegate void OnDropCallback(DragObject drag, ushort selectionHint);
}
