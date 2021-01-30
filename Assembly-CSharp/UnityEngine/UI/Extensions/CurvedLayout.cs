using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000196 RID: 406
	[AddComponentMenu("Layout/Extensions/Curved Layout")]
	public class CurvedLayout : LayoutGroup
	{
		// Token: 0x06000FA0 RID: 4000 RVA: 0x000634ED File Offset: 0x000616ED
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CalculateRadial();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00003022 File Offset: 0x00001222
		public override void SetLayoutHorizontal()
		{
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00003022 File Offset: 0x00001222
		public override void SetLayoutVertical()
		{
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x000634FB File Offset: 0x000616FB
		public override void CalculateLayoutInputVertical()
		{
			this.CalculateRadial();
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x000634FB File Offset: 0x000616FB
		public override void CalculateLayoutInputHorizontal()
		{
			this.CalculateRadial();
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00063504 File Offset: 0x00061704
		private void CalculateRadial()
		{
			this.m_Tracker.Clear();
			if (base.transform.childCount == 0)
			{
				return;
			}
			Vector2 pivot = new Vector2((float)(base.childAlignment % TextAnchor.MiddleLeft) * 0.5f, (float)(base.childAlignment / TextAnchor.MiddleLeft) * 0.5f);
			Vector3 a = new Vector3(base.GetStartOffset(0, base.GetTotalPreferredSize(0)), base.GetStartOffset(1, base.GetTotalPreferredSize(1)), 0f);
			float num = 0f;
			float num2 = 1f / (float)base.transform.childCount;
			Vector3 b = this.itemAxis.normalized * this.itemSize;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != null)
				{
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY);
					Vector3 a2 = a + b;
					a = (rectTransform.localPosition = a2 + (num - this.centerpoint) * this.CurveOffset);
					rectTransform.pivot = pivot;
					RectTransform rectTransform2 = rectTransform;
					RectTransform rectTransform3 = rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform3.anchorMax = vector;
					rectTransform2.anchorMin = vector;
					num += num2;
				}
			}
		}

		// Token: 0x04000EE0 RID: 3808
		public Vector3 CurveOffset;

		// Token: 0x04000EE1 RID: 3809
		[Tooltip("axis along which to place the items, Normalized before use")]
		public Vector3 itemAxis;

		// Token: 0x04000EE2 RID: 3810
		[Tooltip("size of each item along the Normalized axis")]
		public float itemSize;

		// Token: 0x04000EE3 RID: 3811
		public float centerpoint = 0.5f;
	}
}
