using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D4 RID: 468
	[AddComponentMenu("UI/Extensions/UI Scrollrect Occlusion")]
	public class UI_ScrollRectOcclusion : MonoBehaviour
	{
		// Token: 0x060011DC RID: 4572 RVA: 0x0006E6FA File Offset: 0x0006C8FA
		private void Awake()
		{
			if (this.InitByUser)
			{
				return;
			}
			this.Init();
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0006E70C File Offset: 0x0006C90C
		public void Init()
		{
			if (base.GetComponent<ScrollRect>() != null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
				this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
				this._isHorizontal = this._scrollRect.horizontal;
				this._isVertical = this._scrollRect.vertical;
				for (int i = 0; i < this._scrollRect.content.childCount; i++)
				{
					this.items.Add(this._scrollRect.content.GetChild(i).GetComponent<RectTransform>());
				}
				if (this._scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
				{
					this._verticalLayoutGroup = this._scrollRect.content.GetComponent<VerticalLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
				{
					this._horizontalLayoutGroup = this._scrollRect.content.GetComponent<HorizontalLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<GridLayoutGroup>() != null)
				{
					this._gridLayoutGroup = this._scrollRect.content.GetComponent<GridLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<ContentSizeFitter>() != null)
				{
					this._contentSizeFitter = this._scrollRect.content.GetComponent<ContentSizeFitter>();
					return;
				}
			}
			else
			{
				Debug.LogError("UI_ScrollRectOcclusion => No ScrollRect component found");
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0006E874 File Offset: 0x0006CA74
		private void DisableGridComponents()
		{
			if (this._isVertical)
			{
				this._disableMarginY = this._scrollRect.GetComponent<RectTransform>().rect.height / 2f + this.items[0].sizeDelta.y;
			}
			if (this._isHorizontal)
			{
				this._disableMarginX = this._scrollRect.GetComponent<RectTransform>().rect.width / 2f + this.items[0].sizeDelta.x;
			}
			if (this._verticalLayoutGroup)
			{
				this._verticalLayoutGroup.enabled = false;
			}
			if (this._horizontalLayoutGroup)
			{
				this._horizontalLayoutGroup.enabled = false;
			}
			if (this._contentSizeFitter)
			{
				this._contentSizeFitter.enabled = false;
			}
			if (this._gridLayoutGroup)
			{
				this._gridLayoutGroup.enabled = false;
			}
			this.hasDisabledGridComponents = true;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0006E974 File Offset: 0x0006CB74
		public void OnScroll(Vector2 pos)
		{
			if (!this.hasDisabledGridComponents)
			{
				this.DisableGridComponents();
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this._isVertical && this._isHorizontal)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y < -this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y > this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x < -this._disableMarginX || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x > this._disableMarginX)
					{
						this.items[i].gameObject.SetActive(false);
					}
					else
					{
						this.items[i].gameObject.SetActive(true);
					}
				}
				else
				{
					if (this._isVertical)
					{
						if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y < -this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y > this._disableMarginY)
						{
							this.items[i].gameObject.SetActive(false);
						}
						else
						{
							this.items[i].gameObject.SetActive(true);
						}
					}
					if (this._isHorizontal)
					{
						if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x < -this._disableMarginX || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x > this._disableMarginX)
						{
							this.items[i].gameObject.SetActive(false);
						}
						else
						{
							this.items[i].gameObject.SetActive(true);
						}
					}
				}
			}
		}

		// Token: 0x0400104F RID: 4175
		public bool InitByUser;

		// Token: 0x04001050 RID: 4176
		private ScrollRect _scrollRect;

		// Token: 0x04001051 RID: 4177
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x04001052 RID: 4178
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x04001053 RID: 4179
		private HorizontalLayoutGroup _horizontalLayoutGroup;

		// Token: 0x04001054 RID: 4180
		private GridLayoutGroup _gridLayoutGroup;

		// Token: 0x04001055 RID: 4181
		private bool _isVertical;

		// Token: 0x04001056 RID: 4182
		private bool _isHorizontal;

		// Token: 0x04001057 RID: 4183
		private float _disableMarginX;

		// Token: 0x04001058 RID: 4184
		private float _disableMarginY;

		// Token: 0x04001059 RID: 4185
		private bool hasDisabledGridComponents;

		// Token: 0x0400105A RID: 4186
		private List<RectTransform> items = new List<RectTransform>();
	}
}
