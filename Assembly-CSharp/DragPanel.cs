using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000E9 RID: 233
public class DragPanel : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
{
	// Token: 0x06000890 RID: 2192 RVA: 0x0003B789 File Offset: 0x00039989
	public void Start()
	{
		this.m_localTransform = (base.transform as RectTransform);
		this.m_bClampLeft = false;
		this.m_bClampRight = false;
		this.m_bClampTop = false;
		this.m_bClampBottom = false;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0003B7B8 File Offset: 0x000399B8
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.m_bClampLeft = false;
		this.m_bClampRight = false;
		this.m_bClampTop = false;
		this.m_bClampBottom = false;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_localTransform, eventData.position, eventData.pressEventCamera, out this.m_pointerOffset);
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0003B7F4 File Offset: 0x000399F4
	public void OnDrag(PointerEventData eventData)
	{
		if (this.m_localTransform == null)
		{
			return;
		}
		Vector2 a;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_parentTransform, eventData.position, eventData.pressEventCamera, out a))
		{
			this.m_localTransform.localPosition = a - this.m_pointerOffset;
			this.ClampToWindow();
			Vector2 v = this.m_localTransform.localPosition;
			if (this.m_bClampRight)
			{
				v.x = this.m_parentTransform.rect.width * 0.5f - this.m_localTransform.rect.width * (1f - this.m_localTransform.pivot.x);
			}
			else if (this.m_bClampLeft)
			{
				v.x = -this.m_parentTransform.rect.width * 0.5f + this.m_localTransform.rect.width * this.m_localTransform.pivot.x;
			}
			if (this.m_bClampTop)
			{
				v.y = this.m_parentTransform.rect.height * 0.5f - this.m_localTransform.rect.height * (1f - this.m_localTransform.pivot.y);
			}
			else if (this.m_bClampBottom)
			{
				v.y = -this.m_parentTransform.rect.height * 0.5f + this.m_localTransform.rect.height * this.m_localTransform.pivot.y;
			}
			this.m_localTransform.localPosition = v;
		}
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0003B9B8 File Offset: 0x00039BB8
	private void ClampToWindow()
	{
		Vector3[] array = new Vector3[4];
		Vector3[] array2 = new Vector3[4];
		this.m_parentTransform.GetWorldCorners(array);
		this.m_localTransform.GetWorldCorners(array2);
		if (array2[2].x > array[2].x)
		{
			this.m_bClampRight = true;
		}
		else if (this.m_bClampRight)
		{
			this.m_bClampRight = false;
		}
		else if (array2[0].x < array[0].x)
		{
			this.m_bClampLeft = true;
		}
		else if (this.m_bClampLeft)
		{
			this.m_bClampLeft = false;
		}
		if (array2[2].y > array[2].y)
		{
			this.m_bClampTop = true;
			return;
		}
		if (this.m_bClampTop)
		{
			this.m_bClampTop = false;
			return;
		}
		if (array2[0].y < array[0].y)
		{
			this.m_bClampBottom = true;
			return;
		}
		if (this.m_bClampBottom)
		{
			this.m_bClampBottom = false;
		}
	}

	// Token: 0x04000951 RID: 2385
	public RectTransform m_parentTransform;

	// Token: 0x04000952 RID: 2386
	private RectTransform m_localTransform;

	// Token: 0x04000953 RID: 2387
	private Vector2 m_pointerOffset;

	// Token: 0x04000954 RID: 2388
	private bool m_bClampLeft;

	// Token: 0x04000955 RID: 2389
	private bool m_bClampRight;

	// Token: 0x04000956 RID: 2390
	private bool m_bClampTop;

	// Token: 0x04000957 RID: 2391
	private bool m_bClampBottom;
}
