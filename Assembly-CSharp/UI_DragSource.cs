using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000107 RID: 263
public class UI_DragSource : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060009F7 RID: 2551 RVA: 0x00042BD0 File Offset: 0x00040DD0
	// (remove) Token: 0x060009F8 RID: 2552 RVA: 0x00042C08 File Offset: 0x00040E08
	[HideInInspector]
	public event UI_DragSource.DragEventHandler m_DragStart;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060009F9 RID: 2553 RVA: 0x00042C40 File Offset: 0x00040E40
	// (remove) Token: 0x060009FA RID: 2554 RVA: 0x00042C78 File Offset: 0x00040E78
	[HideInInspector]
	public event UI_DragSource.DragEventHandler m_DragContinue;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060009FB RID: 2555 RVA: 0x00042CB0 File Offset: 0x00040EB0
	// (remove) Token: 0x060009FC RID: 2556 RVA: 0x00042CE8 File Offset: 0x00040EE8
	[HideInInspector]
	public event UI_DragSource.DragEventHandler m_DragEnd;

	// Token: 0x060009FD RID: 2557 RVA: 0x00042D20 File Offset: 0x00040F20
	protected virtual void Update()
	{
		if (!this.m_isDragging && this.m_isStartRestriction && this.restriction == UI_DragSource.Restriction.PressAndHold && Time.time - this.m_startDragTime > this.pressAndHoldDelay)
		{
			if (this.m_restrictPosDelta.SqrMagnitude() < 100f)
			{
				this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.Ok;
				return;
			}
			this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.Failed;
		}
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00042D7B File Offset: 0x00040F7B
	public GameObject GetDraggingObject()
	{
		return this.m_DraggingObject;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00042D84 File Offset: 0x00040F84
	public void OnPointerDown(PointerEventData eventData)
	{
		this.m_isDragging = false;
		this.m_isStartRestriction = false;
		this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.None;
		if (this.restriction == UI_DragSource.Restriction.PressAndHold)
		{
			this.m_isStartRestriction = true;
			this.m_startDragPos = eventData.position;
			this.m_startDragTime = Time.time;
			this.m_restrictCam = eventData.pressEventCamera;
			this.m_restrictPosDelta = Vector2.zero;
			this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.Checking;
		}
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00042DEB File Offset: 0x00040FEB
	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_isStartRestriction = false;
		this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.None;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00042DFC File Offset: 0x00040FFC
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!this.m_isStartRestriction)
		{
			this.m_isDragging = false;
			if (this.restriction == UI_DragSource.Restriction.None)
			{
				this.StartDragging(eventData.position, eventData.pressEventCamera);
			}
			else
			{
				this.m_isStartRestriction = true;
				this.m_startDragPos = eventData.position;
				this.m_startDragTime = Time.time;
				this.m_restrictCam = eventData.pressEventCamera;
				this.m_restrictPosDelta = Vector2.zero;
				this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.None;
			}
		}
		if (this.m_isDragging && this.m_DragStart != null)
		{
			this.m_DragStart(this, eventData);
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00042E90 File Offset: 0x00041090
	public void OnDrag(PointerEventData data)
	{
		if (!this.m_isDragging && this.m_DragContinue != null)
		{
			this.m_DragContinue(this, data);
		}
		if (!this.m_isDragging && this.m_isStartRestriction)
		{
			this.m_restrictPosDelta = data.position - this.m_startDragPos;
			bool flag = false;
			if (this.restriction == UI_DragSource.Restriction.Horizontal)
			{
				if (Mathf.Abs(this.m_restrictPosDelta.x) > 50f)
				{
					if (Mathf.Abs(this.m_restrictPosDelta.y) < Mathf.Abs(this.m_restrictPosDelta.x))
					{
						flag = true;
					}
					else
					{
						this.m_isStartRestriction = false;
					}
				}
			}
			else if (this.restriction == UI_DragSource.Restriction.Vertical)
			{
				if (Mathf.Abs(this.m_restrictPosDelta.y) > 50f)
				{
					if (Mathf.Abs(this.m_restrictPosDelta.x) < Mathf.Abs(this.m_restrictPosDelta.y))
					{
						flag = true;
					}
					else
					{
						this.m_isStartRestriction = false;
					}
				}
			}
			else if (this.restriction == UI_DragSource.Restriction.PressAndHold)
			{
				if (this.m_restrictPressHoldState == UI_DragSource.PressAndHoldState.Ok)
				{
					flag = true;
				}
				else if (this.m_restrictPressHoldState == UI_DragSource.PressAndHoldState.Failed)
				{
					this.m_isStartRestriction = false;
					this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.None;
				}
			}
			if (flag)
			{
				this.m_isStartRestriction = false;
				this.StartDragging(data.position, data.pressEventCamera);
			}
		}
		if (this.m_DraggingObject != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00042FEC File Offset: 0x000411EC
	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.m_DragEnd != null)
		{
			this.m_DragEnd(this, eventData);
		}
		if (!this.cloneOnDrag && this.instanceOnDrag == null)
		{
			if (base.transform.parent == this.mCanvas && this.mParent != null)
			{
				base.transform.parent = this.mParent;
			}
		}
		else if (this.m_DraggingObject != null)
		{
			UnityEngine.Object.Destroy(this.m_DraggingObject);
		}
		this.m_DraggingObject = null;
		this.m_isDragging = false;
		this.m_isStartRestriction = false;
		this.m_restrictPressHoldState = UI_DragSource.PressAndHoldState.None;
		this.mParent = null;
		this.mCanvas = null;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000430A0 File Offset: 0x000412A0
	private void StartDragging(Vector2 position, Camera eventCamera)
	{
		this.m_isDragging = true;
		Canvas canvas = UI_DragSource.FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			return;
		}
		this.m_DraggingPlane = (canvas.transform as RectTransform);
		this.mCanvas = canvas.transform;
		if (this.cloneOnDrag)
		{
			this.m_DraggingObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			this.m_DraggingObject.transform.localScale = Vector3.one;
			RectTransform component = this.m_DraggingObject.GetComponent<RectTransform>();
			component.offsetMax = ((RectTransform)base.transform).offsetMax;
			component.offsetMin = ((RectTransform)base.transform).offsetMin;
			component.sizeDelta = ((RectTransform)base.transform).sizeDelta;
			component.anchoredPosition = ((RectTransform)base.transform).anchoredPosition;
			component.anchoredPosition3D = ((RectTransform)base.transform).anchoredPosition3D;
			Image component2 = this.m_DraggingObject.GetComponent<Image>();
			if (component2 != null)
			{
				component2.raycastTarget = false;
			}
		}
		else if (this.instanceOnDrag != null)
		{
			if (this.m_DraggingObject != null)
			{
				UnityEngine.Object.Destroy(this.m_DraggingObject);
			}
			this.m_DraggingObject = UnityEngine.Object.Instantiate<GameObject>(this.instanceOnDrag);
		}
		else
		{
			this.m_DraggingObject = base.gameObject;
			this.mParent = base.transform.parent;
		}
		this.m_DraggingObject.transform.SetParent(canvas.transform, false);
		this.m_DraggingObject.transform.SetAsLastSibling();
		CanvasGroup component3 = this.m_DraggingObject.GetComponent<CanvasGroup>();
		if (component3 != null)
		{
			component3.blocksRaycasts = false;
		}
		this.SetDraggedPosition(position, eventCamera);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00043250 File Offset: 0x00041450
	private void SetDraggedPosition(Vector2 position, Camera eventCamera)
	{
		RectTransform component = this.m_DraggingObject.GetComponent<RectTransform>();
		Vector3 position2;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, position, eventCamera, out position2))
		{
			component.position = position2;
			component.rotation = this.m_DraggingPlane.rotation;
		}
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00043292 File Offset: 0x00041492
	private void SetDraggedPosition(PointerEventData data)
	{
		this.SetDraggedPosition(data.position, data.pressEventCamera);
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x000432A8 File Offset: 0x000414A8
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return default(!!0);
		}
		T component = go.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		Transform parent = go.transform.parent;
		while (parent != null && component == null)
		{
			component = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return component;
	}

	// Token: 0x04000A89 RID: 2697
	public UI_DragSource.Restriction restriction;

	// Token: 0x04000A8A RID: 2698
	public bool cloneOnDrag = true;

	// Token: 0x04000A8B RID: 2699
	public GameObject instanceOnDrag;

	// Token: 0x04000A8C RID: 2700
	[HideInInspector]
	public float pressAndHoldDelay = 1f;

	// Token: 0x04000A90 RID: 2704
	private GameObject m_DraggingObject;

	// Token: 0x04000A91 RID: 2705
	private RectTransform m_DraggingPlane;

	// Token: 0x04000A92 RID: 2706
	protected Transform mParent;

	// Token: 0x04000A93 RID: 2707
	protected Transform mCanvas;

	// Token: 0x04000A94 RID: 2708
	protected bool m_isDragging;

	// Token: 0x04000A95 RID: 2709
	protected Vector2 m_startDragPos;

	// Token: 0x04000A96 RID: 2710
	protected float m_startDragTime;

	// Token: 0x04000A97 RID: 2711
	protected bool m_isStartRestriction;

	// Token: 0x04000A98 RID: 2712
	protected Camera m_restrictCam;

	// Token: 0x04000A99 RID: 2713
	protected Vector2 m_restrictPosDelta;

	// Token: 0x04000A9A RID: 2714
	protected UI_DragSource.PressAndHoldState m_restrictPressHoldState;

	// Token: 0x020007DB RID: 2011
	public enum Restriction
	{
		// Token: 0x04002D39 RID: 11577
		None,
		// Token: 0x04002D3A RID: 11578
		Horizontal,
		// Token: 0x04002D3B RID: 11579
		Vertical,
		// Token: 0x04002D3C RID: 11580
		PressAndHold
	}

	// Token: 0x020007DC RID: 2012
	// (Invoke) Token: 0x06004353 RID: 17235
	public delegate void DragEventHandler(UI_DragSource e, PointerEventData a);

	// Token: 0x020007DD RID: 2013
	protected enum PressAndHoldState
	{
		// Token: 0x04002D3E RID: 11582
		None,
		// Token: 0x04002D3F RID: 11583
		Ok,
		// Token: 0x04002D40 RID: 11584
		Failed,
		// Token: 0x04002D41 RID: 11585
		Checking
	}
}
