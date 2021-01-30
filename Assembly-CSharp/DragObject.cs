using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000D1 RID: 209
public class DragObject : AnimateObject, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
	// Token: 0x060007B8 RID: 1976 RVA: 0x000374E3 File Offset: 0x000356E3
	public void SetDragManager(DragManager manager)
	{
		this.m_DragManager = manager;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x000374EC File Offset: 0x000356EC
	public DragManager GetDragManager()
	{
		return this.m_DragManager;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x000374F4 File Offset: 0x000356F4
	public ushort GetDragSelectionID()
	{
		if (this.m_DragSelectionOverrideID == 0)
		{
			return this.m_DragSelectionID;
		}
		return this.m_DragSelectionOverrideID;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0003750B File Offset: 0x0003570B
	public void SetDragSelectionID(ushort selection_id)
	{
		this.m_DragSelectionID = selection_id;
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00037514 File Offset: 0x00035714
	public void SetDragSelectionOverrideID(ushort selection_id)
	{
		this.m_DragSelectionOverrideID = selection_id;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x0003751D File Offset: 0x0003571D
	public ushort GetDragSelectionHint()
	{
		return this.m_DragSelectionHint;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00037525 File Offset: 0x00035725
	public void SetDragSelectionHint(ushort selection_hint)
	{
		this.m_DragSelectionHint = selection_hint;
		if (this.m_OnDragHintCallback != null)
		{
			this.m_OnDragHintCallback(this, null);
		}
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00037543 File Offset: 0x00035743
	public virtual void SetIsDraggable(bool bDraggable)
	{
		this.m_bIsDraggable = bDraggable;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0003754C File Offset: 0x0003574C
	public void SetSendHorizontalDragToParentScrollRect(bool bSend)
	{
		this.m_bSendHorizontalDragToParentScrollRect = bSend;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00037555 File Offset: 0x00035755
	public void SetSendVerticalDragToParentScrollRect(bool bSend)
	{
		this.m_bSendVerticalDragToParentScrollRect = bSend;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00037560 File Offset: 0x00035760
	public GameObject GetReturnPlaceholder(bool bCreateIfNecessary = true)
	{
		if (this.m_ReturnPlaceholder == null && bCreateIfNecessary)
		{
			this.m_ReturnPlaceholder = new GameObject();
			this.m_ReturnPlaceholder.name = "[return] " + base.name;
			this.m_ReturnPlaceholder.AddComponent<AnimatePlaceholder>().SetOwner(this);
			LayoutElement component = base.GetComponent<LayoutElement>();
			if (component != null)
			{
				LayoutElement layoutElement = this.m_ReturnPlaceholder.AddComponent<LayoutElement>();
				layoutElement.minWidth = component.minWidth;
				layoutElement.minHeight = component.minHeight;
				layoutElement.preferredWidth = component.preferredWidth;
				layoutElement.preferredHeight = component.preferredHeight;
				layoutElement.flexibleWidth = 0f;
				layoutElement.flexibleHeight = 0f;
			}
		}
		return this.m_ReturnPlaceholder;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0003761D File Offset: 0x0003581D
	public void DestroyReturnPlaceholder()
	{
		if (this.m_ReturnPlaceholder != null)
		{
			UnityEngine.Object.Destroy(this.m_ReturnPlaceholder);
			this.m_ReturnPlaceholder = null;
		}
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00037640 File Offset: 0x00035840
	private void UpdateReturnPlaceholder()
	{
		if (this.m_ReturnPlaceholder != null)
		{
			LayoutElement component = base.GetComponent<LayoutElement>();
			if (component != null)
			{
				float preferredWidth = component.preferredWidth;
				float preferredHeight = component.preferredHeight;
				float num = Mathf.Sqrt(preferredWidth * preferredWidth + preferredHeight * preferredHeight);
				Transform parent = base.transform.parent;
				Vector3 vector = parent.InverseTransformPoint(base.transform.position);
				Vector3 vector2 = parent.InverseTransformPoint(this.m_ReturnPlaceholder.transform.position);
				float num2 = vector.x - vector2.x;
				float num3 = vector.y - vector2.y;
				float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
				float num5 = num * 1.2f;
				float x = parent.lossyScale.x;
				float x2 = this.m_ReturnPlaceholder.transform.parent.lossyScale.x;
				num5 *= x2 / x;
				float num6 = num4 / num5;
				if (num6 > 1f)
				{
					num6 = 1f;
				}
				float num7 = 1f - num6;
				LayoutElement component2 = this.m_ReturnPlaceholder.GetComponent<LayoutElement>();
				if (component2 != null)
				{
					component2.minWidth = preferredWidth * num7;
					component2.minHeight = preferredHeight * num7;
					component2.preferredWidth = preferredWidth * num7;
					component2.preferredHeight = preferredHeight * num7;
				}
			}
		}
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00037799 File Offset: 0x00035999
	public bool GetReturnToParent()
	{
		return this.m_bReturnToParent;
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000377A1 File Offset: 0x000359A1
	public void ClearReturnToParent()
	{
		this.m_bReturnToParent = false;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000377AC File Offset: 0x000359AC
	public void AnimateToReturnPlaceholder()
	{
		if (this.m_ReturnPlaceholder == null)
		{
			return;
		}
		AnimationManager animationManager = base.GetAnimationManager();
		if (animationManager != null)
		{
			if (animationManager.StartAnimationToPlaceholder(this, this.m_ReturnPlaceholder, 0, 0, 0f, 0f, true))
			{
				animationManager.SetAnimationRatesReturnDragObject(this);
			}
			this.m_ReturnPlaceholder = null;
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00037804 File Offset: 0x00035A04
	public void AssignToReturnPlaceholder()
	{
		if (this.m_ReturnPlaceholder == null)
		{
			return;
		}
		AnimationLocator component = this.m_ReturnPlaceholder.transform.parent.GetComponent<AnimationLocator>();
		if (component != null)
		{
			base.PlaceOnAnimationLocator(component, true, true, this.m_ReturnPlaceholder.transform.GetSiblingIndex());
			this.DestroyReturnPlaceholder();
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0003785E File Offset: 0x00035A5E
	public void AddOverrideDragCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OverrideDragCallback = (DragObject.DragObjectCallback)Delegate.Combine(this.m_OverrideDragCallback, callback);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00037877 File Offset: 0x00035A77
	public void RemoveOverrideDragCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OverrideDragCallback = (DragObject.DragObjectCallback)Delegate.Remove(this.m_OverrideDragCallback, callback);
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00037890 File Offset: 0x00035A90
	public void AddOnBeginDragCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OnBeginDragCallback = (DragObject.DragObjectCallback)Delegate.Combine(this.m_OnBeginDragCallback, callback);
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000378A9 File Offset: 0x00035AA9
	public void RemoveOnBeginDragCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OnBeginDragCallback = (DragObject.DragObjectCallback)Delegate.Remove(this.m_OnBeginDragCallback, callback);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000378C2 File Offset: 0x00035AC2
	public void AddOnEndDragCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OnEndDragCallback = (DragObject.DragObjectCallback)Delegate.Combine(this.m_OnEndDragCallback, callback);
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000378DB File Offset: 0x00035ADB
	public void RemoveOnEndDragCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OnEndDragCallback = (DragObject.DragObjectCallback)Delegate.Remove(this.m_OnEndDragCallback, callback);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x000378F4 File Offset: 0x00035AF4
	public void AddOnDragHintCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OnDragHintCallback = (DragObject.DragObjectCallback)Delegate.Combine(this.m_OnDragHintCallback, callback);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0003790D File Offset: 0x00035B0D
	public void RemoveOnDragHintCallback(DragObject.DragObjectCallback callback)
	{
		this.m_OnDragHintCallback = (DragObject.DragObjectCallback)Delegate.Remove(this.m_OnDragHintCallback, callback);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00037928 File Offset: 0x00035B28
	public void OnBeginDrag(PointerEventData eventData)
	{
		Transform parent;
		if ((this.m_bSendHorizontalDragToParentScrollRect && Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) * 1.2f) || (this.m_bSendVerticalDragToParentScrollRect && Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x) * 1.2f))
		{
			ScrollRect scrollRect = null;
			parent = base.transform.parent;
			while (parent != null)
			{
				scrollRect = parent.gameObject.GetComponent<ScrollRect>();
				if (scrollRect != null)
				{
					break;
				}
				parent = parent.transform.parent;
			}
			if (scrollRect != null)
			{
				RectTransform viewport = scrollRect.viewport;
				Vector3[] array = new Vector3[4];
				viewport.GetWorldCorners(array);
				RectTransform content = scrollRect.content;
				Vector3[] array2 = new Vector3[4];
				content.GetWorldCorners(array2);
				bool flag = false;
				if (this.m_bSendHorizontalDragToParentScrollRect)
				{
					float num = Mathf.Abs(array[2].x - array[0].x);
					flag = (Mathf.Abs(array2[2].x - array2[0].x) > num);
				}
				else if (this.m_bSendVerticalDragToParentScrollRect)
				{
					float num2 = Mathf.Abs(array[2].y - array[0].y);
					flag = (Mathf.Abs(array2[2].y - array2[0].y) > num2);
				}
				if (flag)
				{
					eventData.pointerDrag = scrollRect.gameObject;
					scrollRect.OnBeginDrag(eventData);
					return;
				}
			}
		}
		if (!this.m_bIsDraggable)
		{
			eventData.pointerDrag = null;
			eventData.dragging = false;
			return;
		}
		if (this.m_OverrideDragCallback != null)
		{
			this.m_OverrideDragCallback(this, eventData);
			return;
		}
		this.m_bIsDragging = true;
		this.SetDragSelectionHint(0);
		GameObject returnPlaceholder = this.GetReturnPlaceholder(true);
		returnPlaceholder.transform.position = base.transform.position;
		returnPlaceholder.transform.rotation = base.transform.rotation;
		returnPlaceholder.transform.SetParent(base.transform.parent);
		returnPlaceholder.transform.SetSiblingIndex(base.transform.GetSiblingIndex());
		this.m_bReturnToParent = true;
		this.m_DragReturnToParent = base.transform.parent;
		Vector3 position = eventData.position;
		this.m_DragCanvas = null;
		if (this.m_DragManager != null && this.m_DragManager.m_DraggingCardsLayer != null)
		{
			base.transform.SetParent(this.m_DragManager.m_DraggingCardsLayer.transform);
		}
		else
		{
			base.transform.SetParent(base.transform.parent.parent);
		}
		base.InheritCurrentScale();
		if (this.m_DragCanvas == null && eventData.pointerCurrentRaycast.module != null)
		{
			this.m_DragCanvas = eventData.pointerCurrentRaycast.module.GetComponent<Canvas>();
			if (this.m_DragCanvas != null)
			{
				Canvas canvas = this.m_DragCanvas;
				while (canvas.transform.parent != null && (canvas = canvas.transform.parent.GetComponentInParent<Canvas>()) != null)
				{
					this.m_DragCanvas = canvas;
				}
			}
		}
		if (this.m_DragCanvas == null && this.m_DragReturnToParent != null)
		{
			Canvas canvas2 = this.m_DragReturnToParent.GetComponentInParent<Canvas>();
			if (canvas2 != null)
			{
				Canvas canvas3 = canvas2;
				while (canvas3.transform.parent != null && (canvas3 = canvas3.transform.parent.GetComponentInParent<Canvas>()) != null)
				{
					canvas2 = canvas3;
				}
				this.m_DragCanvas = canvas2;
				Debug.Log("parent canvas " + this.m_DragCanvas.name);
			}
		}
		if (this.m_DragCanvas != null && this.m_DragCanvas.renderMode == RenderMode.ScreenSpaceCamera)
		{
			position.z = this.m_DragCanvas.planeDistance;
			Camera worldCamera = this.m_DragCanvas.worldCamera;
			if (worldCamera != null)
			{
				position = worldCamera.ScreenToWorldPoint(position);
			}
		}
		parent = base.transform.parent;
		if (parent != null)
		{
			Vector3 a = parent.InverseTransformPoint(position);
			this.m_DragOffset = a - base.transform.localPosition;
		}
		else
		{
			this.m_DragOffset = Vector3.zero + new Vector3(0f, 0f, this.m_DragZLevelOffset);
		}
		this.m_TargetDragZOffset = new Vector3(0f, 0f, this.m_DragZLevelOffset);
		this.m_bTargetDragOffset = true;
		this.m_TargetDragOffsetTime = 0.25f;
		this.m_TargetDragOffset = Vector3.zero;
		if (this.m_OnBeginDragCallback != null)
		{
			this.m_OnBeginDragCallback(this, eventData);
		}
		if (this.m_DragManager != null)
		{
			this.m_DragManager.BeginDrag(this);
		}
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00037E10 File Offset: 0x00036010
	public void OnEndDrag(PointerEventData eventData)
	{
		this.m_bIsDragging = false;
		this.m_DragCanvas = null;
		this.m_DragOffset = Vector3.zero;
		this.m_bTargetDragOffset = false;
		this.m_TargetDragOffsetTime = 0f;
		this.m_TargetDragOffset = Vector3.zero;
		if (this.m_OnEndDragCallback != null)
		{
			this.m_OnEndDragCallback(this, eventData);
		}
		if (this.m_DragManager != null)
		{
			this.m_DragManager.EndDrag(this);
		}
		if (this.m_DragReturnToParent != null && this.m_ReturnPlaceholder != null)
		{
			if (this.m_bReturnToParent)
			{
				if (base.GetAnimationManager() != null)
				{
					if (this.m_DragReturnToParent != this.m_ReturnPlaceholder.transform.parent)
					{
						this.m_ReturnPlaceholder.transform.SetParent(this.m_DragReturnToParent);
					}
					this.AnimateToReturnPlaceholder();
				}
				else
				{
					if (this.m_DragReturnToParent != this.m_ReturnPlaceholder.transform.parent)
					{
						base.transform.SetParent(this.m_DragReturnToParent.transform.parent);
					}
					else
					{
						int siblingIndex = this.m_ReturnPlaceholder.transform.GetSiblingIndex();
						base.transform.SetParent(this.m_DragReturnToParent.parent);
						base.transform.SetSiblingIndex(siblingIndex);
					}
					this.DestroyReturnPlaceholder();
				}
				this.m_bReturnToParent = false;
			}
			else
			{
				this.DestroyReturnPlaceholder();
			}
		}
		this.SetDragSelectionHint(0);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00037F84 File Offset: 0x00036184
	public void OnDrag(PointerEventData eventData)
	{
		if (!this.m_bIsDragging)
		{
			return;
		}
		if (eventData != null)
		{
			Vector3 position = eventData.position;
			if (this.m_DragCanvas != null && this.m_DragCanvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				position.z = this.m_DragCanvas.planeDistance;
				Camera worldCamera = this.m_DragCanvas.worldCamera;
				if (worldCamera != null)
				{
					position = worldCamera.ScreenToWorldPoint(position);
				}
			}
			Transform parent = base.transform.parent;
			if (parent != null)
			{
				Vector3 a = parent.InverseTransformPoint(position);
				base.transform.localPosition = a - this.m_DragOffset + this.m_TargetDragZOffset;
			}
			else
			{
				base.transform.position = position;
			}
		}
		if (this.m_ReturnPlaceholder != null)
		{
			this.UpdateReturnPlaceholder();
		}
		if (this.m_DragManager != null)
		{
			this.m_DragManager.UpdateDrag(this);
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00038072 File Offset: 0x00036272
	public void OnCancelDrag(PointerEventData eventData)
	{
		if (!this.m_bIsDragging)
		{
			return;
		}
		this.SetDragSelectionHint(0);
		this.OnEndDrag(eventData);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0003808C File Offset: 0x0003628C
	public void UpdateDrag()
	{
		if (this.m_bTargetDragOffset)
		{
			float deltaTime = Time.deltaTime;
			if (deltaTime >= this.m_TargetDragOffsetTime)
			{
				Vector3 b = this.m_TargetDragOffset - this.m_DragOffset;
				base.transform.localPosition = base.transform.localPosition - b;
				this.m_DragOffset = this.m_TargetDragOffset;
				this.m_bTargetDragOffset = false;
				this.m_TargetDragOffsetTime = 0f;
				return;
			}
			float d = deltaTime / this.m_TargetDragOffsetTime;
			Vector3 b2 = (this.m_TargetDragOffset - this.m_DragOffset) * d;
			base.transform.localPosition = base.transform.localPosition - b2;
			this.m_DragOffset += b2;
			this.m_TargetDragOffsetTime -= deltaTime;
		}
	}

	// Token: 0x040008C3 RID: 2243
	private DragManager m_DragManager;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	private ushort m_DragSelectionID;

	// Token: 0x040008C5 RID: 2245
	[SerializeField]
	private ushort m_DragSelectionOverrideID;

	// Token: 0x040008C6 RID: 2246
	[SerializeField]
	private ushort m_DragSelectionHint;

	// Token: 0x040008C7 RID: 2247
	[SerializeField]
	private float m_DragZLevelOffset;

	// Token: 0x040008C8 RID: 2248
	private GameObject m_ReturnPlaceholder;

	// Token: 0x040008C9 RID: 2249
	private bool m_bReturnToParent;

	// Token: 0x040008CA RID: 2250
	private Transform m_DragReturnToParent;

	// Token: 0x040008CB RID: 2251
	private Canvas m_DragCanvas;

	// Token: 0x040008CC RID: 2252
	private Vector3 m_DragOffset = Vector3.zero;

	// Token: 0x040008CD RID: 2253
	private bool m_bTargetDragOffset;

	// Token: 0x040008CE RID: 2254
	private float m_TargetDragOffsetTime;

	// Token: 0x040008CF RID: 2255
	private Vector3 m_TargetDragOffset = Vector3.zero;

	// Token: 0x040008D0 RID: 2256
	private Vector3 m_TargetDragZOffset = Vector3.zero;

	// Token: 0x040008D1 RID: 2257
	private DragObject.DragObjectCallback m_OverrideDragCallback;

	// Token: 0x040008D2 RID: 2258
	private DragObject.DragObjectCallback m_OnBeginDragCallback;

	// Token: 0x040008D3 RID: 2259
	private DragObject.DragObjectCallback m_OnEndDragCallback;

	// Token: 0x040008D4 RID: 2260
	private DragObject.DragObjectCallback m_OnDragHintCallback;

	// Token: 0x040008D5 RID: 2261
	public bool m_bIsDraggable = true;

	// Token: 0x040008D6 RID: 2262
	public bool m_bSendHorizontalDragToParentScrollRect;

	// Token: 0x040008D7 RID: 2263
	public bool m_bSendVerticalDragToParentScrollRect;

	// Token: 0x040008D8 RID: 2264
	public bool m_bIsDragging;

	// Token: 0x040008D9 RID: 2265
	private const float k_SendDragToParentScrollRectThreshold = 1.2f;

	// Token: 0x0200079C RID: 1948
	// (Invoke) Token: 0x0600429B RID: 17051
	public delegate void DragObjectCallback(DragObject dragObject, PointerEventData eventData);
}
