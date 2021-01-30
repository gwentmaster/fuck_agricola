using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200019C RID: 412
	[AddComponentMenu("Layout/Extensions/Flow Layout Group")]
	public class FlowLayoutGroup : LayoutGroup
	{
		// Token: 0x06000FB9 RID: 4025 RVA: 0x0006399C File Offset: 0x00061B9C
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			float totalMin = this.GetGreatestMinimumChildWidth() + (float)base.padding.left + (float)base.padding.right;
			base.SetLayoutInputForAxis(totalMin, -1f, -1f, 0);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x000639E4 File Offset: 0x00061BE4
		public override void SetLayoutHorizontal()
		{
			this.SetLayout(base.rectTransform.rect.width, 0, false);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00063A10 File Offset: 0x00061C10
		public override void SetLayoutVertical()
		{
			this.SetLayout(base.rectTransform.rect.width, 1, false);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00063A3C File Offset: 0x00061C3C
		public override void CalculateLayoutInputVertical()
		{
			this._layoutHeight = this.SetLayout(base.rectTransform.rect.width, 1, true);
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00063A6A File Offset: 0x00061C6A
		protected bool IsCenterAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerCenter || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.UpperCenter;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00063A89 File Offset: 0x00061C89
		protected bool IsRightAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.UpperRight;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x00063AA8 File Offset: 0x00061CA8
		protected bool IsMiddleAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.MiddleCenter;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00063AC7 File Offset: 0x00061CC7
		protected bool IsLowerAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerLeft || base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.LowerCenter;
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00063AE8 File Offset: 0x00061CE8
		public float SetLayout(float width, int axis, bool layoutInput)
		{
			float height = base.rectTransform.rect.height;
			float num = base.rectTransform.rect.width - (float)base.padding.left - (float)base.padding.right;
			float num2 = this.IsLowerAlign ? ((float)base.padding.bottom) : ((float)base.padding.top);
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				int index = this.IsLowerAlign ? (base.rectChildren.Count - 1 - i) : i;
				RectTransform rectTransform = base.rectChildren[index];
				float num5 = LayoutUtility.GetPreferredSize(rectTransform, 0);
				float preferredSize = LayoutUtility.GetPreferredSize(rectTransform, 1);
				num5 = Mathf.Min(num5, num);
				if (num3 + num5 > num)
				{
					num3 -= this.SpacingX;
					if (!layoutInput)
					{
						float yOffset = this.CalculateRowVerticalOffset(height, num2, num4);
						this.LayoutRow(this._rowList, num3, num4, num, (float)base.padding.left, yOffset, axis);
					}
					this._rowList.Clear();
					num2 += num4;
					num2 += this.SpacingY;
					num4 = 0f;
					num3 = 0f;
				}
				num3 += num5;
				this._rowList.Add(rectTransform);
				if (preferredSize > num4)
				{
					num4 = preferredSize;
				}
				if (i < base.rectChildren.Count - 1)
				{
					num3 += this.SpacingX;
				}
			}
			if (!layoutInput)
			{
				float yOffset2 = this.CalculateRowVerticalOffset(height, num2, num4);
				num3 -= this.SpacingX;
				this.LayoutRow(this._rowList, num3, num4, num - ((this._rowList.Count > 1) ? this.SpacingX : 0f), (float)base.padding.left, yOffset2, axis);
			}
			this._rowList.Clear();
			num2 += num4;
			num2 += (float)(this.IsLowerAlign ? base.padding.top : base.padding.bottom);
			if (layoutInput && axis == 1)
			{
				base.SetLayoutInputForAxis(num2, num2, -1f, axis);
			}
			return num2;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00063D0C File Offset: 0x00061F0C
		private float CalculateRowVerticalOffset(float groupHeight, float yOffset, float currentRowHeight)
		{
			float result;
			if (this.IsLowerAlign)
			{
				result = groupHeight - yOffset - currentRowHeight;
			}
			else if (this.IsMiddleAlign)
			{
				result = groupHeight * 0.5f - this._layoutHeight * 0.5f + yOffset;
			}
			else
			{
				result = yOffset;
			}
			return result;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00063D50 File Offset: 0x00061F50
		protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{
			float num = xOffset;
			if (!this.ChildForceExpandWidth && this.IsCenterAlign)
			{
				num += (maxWidth - rowWidth) * 0.5f;
			}
			else if (!this.ChildForceExpandWidth && this.IsRightAlign)
			{
				num += maxWidth - rowWidth;
			}
			float num2 = 0f;
			float num3 = 0f;
			if (this.ChildForceExpandWidth)
			{
				num2 = (maxWidth - rowWidth) / (float)this._rowList.Count;
			}
			else if (this.ExpandHorizontalSpacing)
			{
				num3 = (maxWidth - rowWidth) / (float)(this._rowList.Count - 1);
				if (this._rowList.Count > 1)
				{
					if (this.IsCenterAlign)
					{
						num -= num3 * 0.5f * (float)(this._rowList.Count - 1);
					}
					else if (this.IsRightAlign)
					{
						num -= num3 * (float)(this._rowList.Count - 1);
					}
				}
			}
			for (int i = 0; i < this._rowList.Count; i++)
			{
				int index = this.IsLowerAlign ? (this._rowList.Count - 1 - i) : i;
				RectTransform rect = this._rowList[index];
				float num4 = LayoutUtility.GetPreferredSize(rect, 0) + num2;
				float num5 = LayoutUtility.GetPreferredSize(rect, 1);
				if (this.ChildForceExpandHeight)
				{
					num5 = rowHeight;
				}
				num4 = Mathf.Min(num4, maxWidth);
				float num6 = yOffset;
				if (this.IsMiddleAlign)
				{
					num6 += (rowHeight - num5) * 0.5f;
				}
				else if (this.IsLowerAlign)
				{
					num6 += rowHeight - num5;
				}
				if (this.ExpandHorizontalSpacing && i > 0)
				{
					num += num3;
				}
				if (axis == 0)
				{
					base.SetChildAlongAxis(rect, 0, num, num4);
				}
				else
				{
					base.SetChildAlongAxis(rect, 1, num6, num5);
				}
				if (i < this._rowList.Count - 1)
				{
					num += num4 + this.SpacingX;
				}
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00063F14 File Offset: 0x00062114
		public float GetGreatestMinimumChildWidth()
		{
			float num = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				num = Mathf.Max(LayoutUtility.GetMinWidth(base.rectChildren[i]), num);
			}
			return num;
		}

		// Token: 0x04000EED RID: 3821
		public float SpacingX;

		// Token: 0x04000EEE RID: 3822
		public float SpacingY;

		// Token: 0x04000EEF RID: 3823
		public bool ExpandHorizontalSpacing;

		// Token: 0x04000EF0 RID: 3824
		public bool ChildForceExpandWidth;

		// Token: 0x04000EF1 RID: 3825
		public bool ChildForceExpandHeight;

		// Token: 0x04000EF2 RID: 3826
		private float _layoutHeight;

		// Token: 0x04000EF3 RID: 3827
		private readonly IList<RectTransform> _rowList = new List<RectTransform>();
	}
}
