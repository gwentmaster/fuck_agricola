using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F6 RID: 246
[RequireComponent(typeof(ScrollRect))]
public class ScrollRect_AutoScroll : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000925 RID: 2341 RVA: 0x0003DF6B File Offset: 0x0003C16B
	private void Awake()
	{
		this.m_ScrollRect = base.gameObject.GetComponent<ScrollRect>();
		if (this.m_ScrollRect == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0003DF94 File Offset: 0x0003C194
	private void Update()
	{
		if (!this.m_bTouching)
		{
			this.m_ScrollRect.verticalNormalizedPosition -= this.m_AutoScrollVelocity * Time.deltaTime;
			if (this.m_ScrollRect.verticalNormalizedPosition <= 0f)
			{
				this.m_ScrollRect.verticalNormalizedPosition = 0f;
			}
		}
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0003DFE9 File Offset: 0x0003C1E9
	public void ResetScroll()
	{
		this.m_ScrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0003DFFB File Offset: 0x0003C1FB
	public void OnPointerDown(PointerEventData eventData)
	{
		this.m_bTouching = true;
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0003E004 File Offset: 0x0003C204
	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_bTouching = false;
	}

	// Token: 0x040009B5 RID: 2485
	public float m_AutoScrollVelocity;

	// Token: 0x040009B6 RID: 2486
	private bool m_bTouching;

	// Token: 0x040009B7 RID: 2487
	[HideInInspector]
	public ScrollRect m_ScrollRect;
}
