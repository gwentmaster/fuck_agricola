using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D3 RID: 467
	[AddComponentMenu("UI/Extensions/UI Infinite Scroll")]
	public class UI_InfiniteScroll : MonoBehaviour
	{
		// Token: 0x060011D7 RID: 4567 RVA: 0x0006E0F5 File Offset: 0x0006C2F5
		private void Awake()
		{
			if (!this.InitByUser)
			{
				this.Init();
			}
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0006E108 File Offset: 0x0006C308
		public void Init()
		{
			if (base.GetComponent<ScrollRect>() != null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
				this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
				this._scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
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
				}
				this._isHorizontal = this._scrollRect.horizontal;
				this._isVertical = this._scrollRect.vertical;
				if (this._isHorizontal && this._isVertical)
				{
					Debug.LogError("UI_InfiniteScroll doesn't support scrolling in both directions, plase choose one direction (horizontal or vertical)");
				}
				this._itemCount = this._scrollRect.content.childCount;
				return;
			}
			Debug.LogError("UI_InfiniteScroll => No ScrollRect component found");
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0006E2AC File Offset: 0x0006C4AC
		private void DisableGridComponents()
		{
			if (this._isVertical)
			{
				this._recordOffsetY = this.items[0].GetComponent<RectTransform>().anchoredPosition.y - this.items[1].GetComponent<RectTransform>().anchoredPosition.y;
				this._disableMarginY = this._recordOffsetY * (float)this._itemCount / 2f;
			}
			if (this._isHorizontal)
			{
				this._recordOffsetX = this.items[1].GetComponent<RectTransform>().anchoredPosition.x - this.items[0].GetComponent<RectTransform>().anchoredPosition.x;
				this._disableMarginX = this._recordOffsetX * (float)this._itemCount / 2f;
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
			this._hasDisabledGridComponents = true;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0006E3E4 File Offset: 0x0006C5E4
		public void OnScroll(Vector2 pos)
		{
			if (!this._hasDisabledGridComponents)
			{
				this.DisableGridComponents();
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this._isHorizontal)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).x > this._disableMarginX + this._treshold)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.x = this._newAnchoredPosition.x - (float)this._itemCount * this._recordOffsetX;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(this._itemCount - 1).transform.SetAsFirstSibling();
					}
					else if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).x < -this._disableMarginX)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.x = this._newAnchoredPosition.x + (float)this._itemCount * this._recordOffsetX;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(0).transform.SetAsLastSibling();
					}
				}
				if (this._isVertical)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).y > this._disableMarginY + this._treshold)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.y = this._newAnchoredPosition.y - (float)this._itemCount * this._recordOffsetY;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(this._itemCount - 1).transform.SetAsFirstSibling();
					}
					else if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).y < -this._disableMarginY)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.y = this._newAnchoredPosition.y + (float)this._itemCount * this._recordOffsetY;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(0).transform.SetAsLastSibling();
					}
				}
			}
		}

		// Token: 0x0400103E RID: 4158
		[Tooltip("If false, will Init automatically, otherwise you need to call Init() method")]
		public bool InitByUser;

		// Token: 0x0400103F RID: 4159
		private ScrollRect _scrollRect;

		// Token: 0x04001040 RID: 4160
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x04001041 RID: 4161
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x04001042 RID: 4162
		private HorizontalLayoutGroup _horizontalLayoutGroup;

		// Token: 0x04001043 RID: 4163
		private GridLayoutGroup _gridLayoutGroup;

		// Token: 0x04001044 RID: 4164
		private bool _isVertical;

		// Token: 0x04001045 RID: 4165
		private bool _isHorizontal;

		// Token: 0x04001046 RID: 4166
		private float _disableMarginX;

		// Token: 0x04001047 RID: 4167
		private float _disableMarginY;

		// Token: 0x04001048 RID: 4168
		private bool _hasDisabledGridComponents;

		// Token: 0x04001049 RID: 4169
		private List<RectTransform> items = new List<RectTransform>();

		// Token: 0x0400104A RID: 4170
		private Vector2 _newAnchoredPosition = Vector2.zero;

		// Token: 0x0400104B RID: 4171
		private float _treshold = 100f;

		// Token: 0x0400104C RID: 4172
		private int _itemCount;

		// Token: 0x0400104D RID: 4173
		private float _recordOffsetX;

		// Token: 0x0400104E RID: 4174
		private float _recordOffsetY;
	}
}
