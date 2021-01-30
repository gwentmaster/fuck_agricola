using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001AD RID: 429
	[AddComponentMenu("UI/Extensions/Primitives/UI Circle")]
	public class UICircle : UIPrimitiveBase
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00067D41 File Offset: 0x00065F41
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x00067D49 File Offset: 0x00065F49
		public int FillPercent
		{
			get
			{
				return this.m_fillPercent;
			}
			set
			{
				this.m_fillPercent = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x00067D58 File Offset: 0x00065F58
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x00067D60 File Offset: 0x00065F60
		public bool Fill
		{
			get
			{
				return this.m_fill;
			}
			set
			{
				this.m_fill = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00067D6F File Offset: 0x00065F6F
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x00067D77 File Offset: 0x00065F77
		public float Thickness
		{
			get
			{
				return this.m_thickness;
			}
			set
			{
				this.m_thickness = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00067D88 File Offset: 0x00065F88
		private void Update()
		{
			this.m_thickness = Mathf.Clamp(this.m_thickness, 0f, base.rectTransform.rect.width / 2f);
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x00067DC5 File Offset: 0x00065FC5
		// (set) Token: 0x0600109C RID: 4252 RVA: 0x00067DCD File Offset: 0x00065FCD
		public int Segments
		{
			get
			{
				return this.m_segments;
			}
			set
			{
				this.m_segments = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00067DDC File Offset: 0x00065FDC
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			float outer = -base.rectTransform.pivot.x * base.rectTransform.rect.width;
			float inner = -base.rectTransform.pivot.x * base.rectTransform.rect.width + this.m_thickness;
			vh.Clear();
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 vector = new Vector2(0f, 0f);
			Vector2 vector2 = new Vector2(0f, 1f);
			Vector2 vector3 = new Vector2(1f, 1f);
			Vector2 vector4 = new Vector2(1f, 0f);
			if (this.FixedToSegments)
			{
				float num = (float)this.m_fillPercent / 100f;
				float num2 = 360f / (float)this.m_segments;
				int num3 = (int)((float)(this.m_segments + 1) * num);
				for (int i = 0; i < num3; i++)
				{
					float f = 0.017453292f * ((float)i * num2);
					float c = Mathf.Cos(f);
					float s = Mathf.Sin(f);
					vector = new Vector2(0f, 1f);
					vector2 = new Vector2(1f, 1f);
					vector3 = new Vector2(1f, 0f);
					vector4 = new Vector2(0f, 0f);
					Vector2 vector5;
					Vector2 vector6;
					Vector2 vector7;
					Vector2 vector8;
					this.StepThroughPointsAndFill(outer, inner, ref zero, ref zero2, out vector5, out vector6, out vector7, out vector8, c, s);
					vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
					{
						vector5,
						vector6,
						vector7,
						vector8
					}, new Vector2[]
					{
						vector,
						vector2,
						vector3,
						vector4
					}));
				}
				return;
			}
			float width = base.rectTransform.rect.width;
			float height = base.rectTransform.rect.height;
			float num4 = (float)this.m_fillPercent / 100f * 6.2831855f / (float)this.m_segments;
			float num5 = 0f;
			for (int j = 0; j < this.m_segments + 1; j++)
			{
				float c2 = Mathf.Cos(num5);
				float s2 = Mathf.Sin(num5);
				Vector2 vector5;
				Vector2 vector6;
				Vector2 vector7;
				Vector2 vector8;
				this.StepThroughPointsAndFill(outer, inner, ref zero, ref zero2, out vector5, out vector6, out vector7, out vector8, c2, s2);
				vector = new Vector2(vector5.x / width + 0.5f, vector5.y / height + 0.5f);
				vector2 = new Vector2(vector6.x / width + 0.5f, vector6.y / height + 0.5f);
				vector3 = new Vector2(vector7.x / width + 0.5f, vector7.y / height + 0.5f);
				vector4 = new Vector2(vector8.x / width + 0.5f, vector8.y / height + 0.5f);
				vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
				{
					vector5,
					vector6,
					vector7,
					vector8
				}, new Vector2[]
				{
					vector,
					vector2,
					vector3,
					vector4
				}));
				num5 += num4;
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00068158 File Offset: 0x00066358
		private void StepThroughPointsAndFill(float outer, float inner, ref Vector2 prevX, ref Vector2 prevY, out Vector2 pos0, out Vector2 pos1, out Vector2 pos2, out Vector2 pos3, float c, float s)
		{
			pos0 = prevX;
			pos1 = new Vector2(outer * c, outer * s);
			if (this.m_fill)
			{
				pos2 = Vector2.zero;
				pos3 = Vector2.zero;
			}
			else
			{
				pos2 = new Vector2(inner * c, inner * s);
				pos3 = prevY;
			}
			prevX = pos1;
			prevY = pos2;
		}

		// Token: 0x04000F78 RID: 3960
		[Tooltip("The circular fill percentage of the primitive, affected by FixedToSegments")]
		[Range(0f, 100f)]
		[SerializeField]
		private int m_fillPercent = 100;

		// Token: 0x04000F79 RID: 3961
		[Tooltip("Should the primitive fill draw by segments or absolute percentage")]
		public bool FixedToSegments;

		// Token: 0x04000F7A RID: 3962
		[Tooltip("Draw the primitive filled or as a line")]
		[SerializeField]
		private bool m_fill = true;

		// Token: 0x04000F7B RID: 3963
		[Tooltip("If not filled, the thickness of the primitive line")]
		[SerializeField]
		private float m_thickness = 5f;

		// Token: 0x04000F7C RID: 3964
		[Tooltip("The number of segments to draw the primitive, more segments = smoother primitive")]
		[Range(0f, 360f)]
		[SerializeField]
		private int m_segments = 360;
	}
}
