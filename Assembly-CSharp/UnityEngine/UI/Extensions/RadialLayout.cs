using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200019F RID: 415
	[AddComponentMenu("Layout/Extensions/Radial Layout")]
	public class RadialLayout : LayoutGroup
	{
		// Token: 0x06000FDA RID: 4058 RVA: 0x000645A9 File Offset: 0x000627A9
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CalculateRadial();
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00003022 File Offset: 0x00001222
		public override void SetLayoutHorizontal()
		{
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00003022 File Offset: 0x00001222
		public override void SetLayoutVertical()
		{
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x000645B7 File Offset: 0x000627B7
		public override void CalculateLayoutInputVertical()
		{
			this.CalculateRadial();
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x000645B7 File Offset: 0x000627B7
		public override void CalculateLayoutInputHorizontal()
		{
			this.CalculateRadial();
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x000645C0 File Offset: 0x000627C0
		private void CalculateRadial()
		{
			this.m_Tracker.Clear();
			if (base.transform.childCount == 0)
			{
				return;
			}
			float num = (this.MaxAngle - this.MinAngle) / (float)base.transform.childCount;
			float num2 = this.StartAngle;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != null)
				{
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY);
					Vector3 a = new Vector3(Mathf.Cos(num2 * 0.017453292f), Mathf.Sin(num2 * 0.017453292f), 0f);
					rectTransform.localPosition = a * this.fDistance;
					RectTransform rectTransform2 = rectTransform;
					RectTransform rectTransform3 = rectTransform;
					RectTransform rectTransform4 = rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform4.pivot = vector;
					rectTransform2.anchorMin = (rectTransform3.anchorMax = vector);
					num2 += num;
				}
			}
		}

		// Token: 0x04000EF4 RID: 3828
		public float fDistance;

		// Token: 0x04000EF5 RID: 3829
		[Range(0f, 360f)]
		public float MinAngle;

		// Token: 0x04000EF6 RID: 3830
		[Range(0f, 360f)]
		public float MaxAngle;

		// Token: 0x04000EF7 RID: 3831
		[Range(0f, 360f)]
		public float StartAngle;
	}
}
