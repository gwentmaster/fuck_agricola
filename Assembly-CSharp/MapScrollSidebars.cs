using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000067 RID: 103
[RequireComponent(typeof(Image))]
public class MapScrollSidebars : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerUpHandler, IDropHandler
{
	// Token: 0x06000538 RID: 1336 RVA: 0x000285EE File Offset: 0x000267EE
	private void Awake()
	{
		this.m_collider = base.gameObject.GetComponent<Image>();
		if (this.m_collider != null)
		{
			this.m_collider.raycastTarget = false;
		}
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0002861C File Offset: 0x0002681C
	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
		if (gameObject != null)
		{
			AgricolaGame component = gameObject.GetComponent<AgricolaGame>();
			if (component != null)
			{
				DragManager dragManager = component.GetDragManager();
				if (dragManager != null)
				{
					dragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.DragStartCallback));
					dragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.DragEndCallback));
				}
			}
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00028681 File Offset: 0x00026881
	public void DragStartCallback(DragObject obj)
	{
		if (this.m_collider != null)
		{
			this.m_collider.raycastTarget = true;
		}
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0002869D File Offset: 0x0002689D
	public void DragEndCallback(DragObject obj)
	{
		if (this.m_collider != null)
		{
			this.m_collider.raycastTarget = false;
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x000286B9 File Offset: 0x000268B9
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.dragging)
		{
			this.m_bScrolling = true;
		}
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x000286CA File Offset: 0x000268CA
	public void OnPointerExit(PointerEventData eventData)
	{
		this.m_bScrolling = false;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x000286D3 File Offset: 0x000268D3
	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_bScrolling = false;
		if (this.m_collider != null)
		{
			this.m_collider.raycastTarget = false;
		}
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x000286D3 File Offset: 0x000268D3
	public void OnDrop(PointerEventData eventData)
	{
		this.m_bScrolling = false;
		if (this.m_collider != null)
		{
			this.m_collider.raycastTarget = false;
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x000286F8 File Offset: 0x000268F8
	private void Update()
	{
		if (this.m_bScrolling && this.m_maps != null)
		{
			for (int i = 0; i < this.m_maps.Length; i++)
			{
				if (this.m_maps[i] != null)
				{
					this.m_maps[i].ForceMoveMap(this.m_scrollPerFrame);
				}
			}
		}
	}

	// Token: 0x040004EB RID: 1259
	[SerializeField]
	private TransformMap[] m_maps;

	// Token: 0x040004EC RID: 1260
	[SerializeField]
	private Vector3 m_scrollPerFrame;

	// Token: 0x040004ED RID: 1261
	private Image m_collider;

	// Token: 0x040004EE RID: 1262
	private bool m_bScrolling;
}
