using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001BB RID: 443
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/UI Window Base")]
	public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06001126 RID: 4390 RVA: 0x0006B748 File Offset: 0x00069948
		private void Start()
		{
			this.m_transform = base.GetComponent<RectTransform>();
			this.m_originalCoods = this.m_transform.position;
			this.m_canvas = base.GetComponentInParent<Canvas>();
			this.m_canvasRectTransform = this.m_canvas.GetComponent<RectTransform>();
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0006B784 File Offset: 0x00069984
		private void Update()
		{
			if (UIWindowBase.ResetCoords)
			{
				this.resetCoordinatePosition();
			}
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0006B794 File Offset: 0x00069994
		public void OnDrag(PointerEventData eventData)
		{
			if (this._isDragging)
			{
				Vector3 b = this.ScreenToCanvas(eventData.position) - this.ScreenToCanvas(eventData.position - eventData.delta);
				this.m_transform.localPosition += b;
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0006B7F4 File Offset: 0x000699F4
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == null)
			{
				return;
			}
			if (eventData.pointerCurrentRaycast.gameObject.name == base.name)
			{
				this._isDragging = true;
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0006B83F File Offset: 0x00069A3F
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isDragging = false;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0006B848 File Offset: 0x00069A48
		private void resetCoordinatePosition()
		{
			this.m_transform.position = this.m_originalCoods;
			UIWindowBase.ResetCoords = false;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0006B864 File Offset: 0x00069A64
		private Vector3 ScreenToCanvas(Vector3 screenPosition)
		{
			Vector2 sizeDelta = this.m_canvasRectTransform.sizeDelta;
			Vector3 vector;
			Vector2 vector2;
			Vector2 vector3;
			if (this.m_canvas.renderMode == RenderMode.ScreenSpaceOverlay || (this.m_canvas.renderMode == RenderMode.ScreenSpaceCamera && this.m_canvas.worldCamera == null))
			{
				vector = screenPosition;
				vector2 = Vector2.zero;
				vector3 = sizeDelta;
			}
			else
			{
				Ray ray = this.m_canvas.worldCamera.ScreenPointToRay(screenPosition);
				Plane plane = new Plane(this.m_canvasRectTransform.forward, this.m_canvasRectTransform.position);
				float d;
				if (!plane.Raycast(ray, out d))
				{
					throw new Exception("Is it practically possible?");
				}
				Vector3 position = ray.origin + ray.direction * d;
				vector = this.m_canvasRectTransform.InverseTransformPoint(position);
				vector2 = -Vector2.Scale(sizeDelta, this.m_canvasRectTransform.pivot);
				vector3 = Vector2.Scale(sizeDelta, Vector2.one - this.m_canvasRectTransform.pivot);
			}
			vector.x = Mathf.Clamp(vector.x, vector2.x + (float)this.KeepWindowInCanvas, vector3.x - (float)this.KeepWindowInCanvas);
			vector.y = Mathf.Clamp(vector.y, vector2.y + (float)this.KeepWindowInCanvas, vector3.y - (float)this.KeepWindowInCanvas);
			return vector;
		}

		// Token: 0x04000FE1 RID: 4065
		private RectTransform m_transform;

		// Token: 0x04000FE2 RID: 4066
		private bool _isDragging;

		// Token: 0x04000FE3 RID: 4067
		public static bool ResetCoords;

		// Token: 0x04000FE4 RID: 4068
		private Vector3 m_originalCoods = Vector3.zero;

		// Token: 0x04000FE5 RID: 4069
		private Canvas m_canvas;

		// Token: 0x04000FE6 RID: 4070
		private RectTransform m_canvasRectTransform;

		// Token: 0x04000FE7 RID: 4071
		public int KeepWindowInCanvas = 5;
	}
}
