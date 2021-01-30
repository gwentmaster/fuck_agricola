using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class DragManager : MonoBehaviour
{
	// Token: 0x060007A7 RID: 1959 RVA: 0x0003712F File Offset: 0x0003532F
	public void AddOnBeginDragCallback(DragManager.DragManagerCallback callback)
	{
		this.m_OnBeginDragCallback = (DragManager.DragManagerCallback)Delegate.Combine(this.m_OnBeginDragCallback, callback);
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00037148 File Offset: 0x00035348
	public void RemoveOnBeginDragCallback(DragManager.DragManagerCallback callback)
	{
		this.m_OnBeginDragCallback = (DragManager.DragManagerCallback)Delegate.Remove(this.m_OnBeginDragCallback, callback);
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x00037161 File Offset: 0x00035361
	public void AddOnUpdateDragCallback(DragManager.DragManagerCallback callback)
	{
		this.m_OnUpdateDragCallback = (DragManager.DragManagerCallback)Delegate.Combine(this.m_OnUpdateDragCallback, callback);
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0003717A File Offset: 0x0003537A
	public void RemoveOnUpdateDragCallback(DragManager.DragManagerCallback callback)
	{
		this.m_OnUpdateDragCallback = (DragManager.DragManagerCallback)Delegate.Remove(this.m_OnUpdateDragCallback, callback);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00037193 File Offset: 0x00035393
	public void AddOnEndDragCallback(DragManager.DragManagerCallback callback)
	{
		this.m_OnEndDragCallback = (DragManager.DragManagerCallback)Delegate.Combine(this.m_OnEndDragCallback, callback);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x000371AC File Offset: 0x000353AC
	public void RemoveOnEndDragCallback(DragManager.DragManagerCallback callback)
	{
		this.m_OnEndDragCallback = (DragManager.DragManagerCallback)Delegate.Remove(this.m_OnEndDragCallback, callback);
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x000371C5 File Offset: 0x000353C5
	public void AddAdditionalDragTargetZone(DragTargetZone add_zone)
	{
		if (!this.m_AdditionalDragTargetZones.Contains(add_zone))
		{
			this.m_AdditionalDragTargetZones.Add(add_zone);
		}
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x000371E1 File Offset: 0x000353E1
	public bool IsDraggingObject()
	{
		return this.m_DraggingObject != null;
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000371EF File Offset: 0x000353EF
	public DragObject GetDraggingObject()
	{
		return this.m_DraggingObject;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x000371F8 File Offset: 0x000353F8
	public void BeginDrag(DragObject drag_object)
	{
		this.m_DraggingObject = drag_object;
		ushort dragSelectionID = drag_object.GetDragSelectionID();
		this.SetDragTargetZones(dragSelectionID);
		if (this.m_OnBeginDragCallback != null)
		{
			this.m_OnBeginDragCallback(drag_object);
		}
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0003722E File Offset: 0x0003542E
	public void EndDrag(DragObject drag_object)
	{
		if (this.m_DraggingObject == drag_object)
		{
			this.m_DraggingObject = null;
		}
		this.ClearDragTargetZones();
		if (this.m_OnEndDragCallback != null)
		{
			this.m_OnEndDragCallback(drag_object);
		}
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0003725F File Offset: 0x0003545F
	public void UpdateDrag(DragObject drag_object)
	{
		if (this.m_OnUpdateDragCallback != null)
		{
			this.m_OnUpdateDragCallback(drag_object);
		}
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x00037275 File Offset: 0x00035475
	private void Update()
	{
		if (this.m_DraggingObject != null)
		{
			this.m_DraggingObject.UpdateDrag();
		}
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00037290 File Offset: 0x00035490
	public void OnApplicationFocus(bool focusStatus)
	{
		if (!this.IsDraggingObject())
		{
			return;
		}
		this.m_DraggingObject.OnCancelDrag(null);
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x000372A8 File Offset: 0x000354A8
	public void ClearDragTargetZones()
	{
		for (int i = 0; i < this.m_DragTargetZones.Length; i++)
		{
			if (this.m_DragTargetZones[i] != null)
			{
				this.m_DragTargetZones[i].SetDragSelectionHint(0, Color.white, 0);
			}
		}
		foreach (DragTargetZone dragTargetZone in this.m_AdditionalDragTargetZones)
		{
			dragTargetZone.SetDragSelectionHint(0, Color.white, 0);
		}
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00037338 File Offset: 0x00035538
	public void SetDragTargetZones(ushort selectionID)
	{
		this.ClearDragTargetZones();
		List<ushort> selectionHints = GameOptions.GetSelectionHints(selectionID);
		foreach (ushort num in selectionHints)
		{
			DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)num);
			if (dragSelectionHintDefinition != null)
			{
				int dragTargetZoneIndex = dragSelectionHintDefinition.m_DragTargetZoneIndex;
				if (dragTargetZoneIndex < this.m_DragTargetZones.Length && this.m_DragTargetZones[dragTargetZoneIndex] != null)
				{
					this.m_DragTargetZones[dragTargetZoneIndex].SetDragSelectionHint(num, dragSelectionHintDefinition.m_HighlightColor, 0);
				}
				int dragTargetZoneShortcut = dragSelectionHintDefinition.m_DragTargetZoneShortcut;
				if (dragTargetZoneShortcut != 0 && selectionHints.Count == 1 && dragTargetZoneShortcut < this.m_DragTargetZones.Length && this.m_DragTargetZones[dragTargetZoneShortcut] != null)
				{
					this.m_DragTargetZones[dragTargetZoneShortcut].SetDragSelectionHint(num, dragSelectionHintDefinition.m_HighlightColor, 0);
				}
			}
		}
		foreach (DragTargetZone dragTargetZone in this.m_AdditionalDragTargetZones)
		{
			ushort dragTargetInstanceID = dragTargetZone.GetDragTargetInstanceID();
			selectionHints = GameOptions.GetSelectionHints(dragTargetInstanceID);
			foreach (ushort num2 in selectionHints)
			{
				DragSelectionHintDefinition dragSelectionHintDefinition2 = InterfaceSelectionHints.FindSelectionHintDefinition((int)num2);
				if (dragSelectionHintDefinition2 != null && dragSelectionHintDefinition2.m_bUseTargetZoneInstanceID)
				{
					dragTargetZone.SetDragSelectionHint(num2, dragSelectionHintDefinition2.m_HighlightColor, dragTargetInstanceID);
				}
			}
		}
	}

	// Token: 0x040008BC RID: 2236
	[SerializeField]
	public GameObject m_DraggingCardsLayer;

	// Token: 0x040008BD RID: 2237
	[SerializeField]
	public DragTargetZone[] m_DragTargetZones;

	// Token: 0x040008BE RID: 2238
	private DragManager.DragManagerCallback m_OnBeginDragCallback;

	// Token: 0x040008BF RID: 2239
	private DragManager.DragManagerCallback m_OnUpdateDragCallback;

	// Token: 0x040008C0 RID: 2240
	private DragManager.DragManagerCallback m_OnEndDragCallback;

	// Token: 0x040008C1 RID: 2241
	private List<DragTargetZone> m_AdditionalDragTargetZones = new List<DragTargetZone>();

	// Token: 0x040008C2 RID: 2242
	private DragObject m_DraggingObject;

	// Token: 0x0200079B RID: 1947
	// (Invoke) Token: 0x06004297 RID: 17047
	public delegate void DragManagerCallback(DragObject dragObject);
}
