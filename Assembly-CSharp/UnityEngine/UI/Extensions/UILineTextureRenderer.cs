using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001B1 RID: 433
	[AddComponentMenu("UI/Extensions/Primitives/UILineTextureRenderer")]
	public class UILineTextureRenderer : UIPrimitiveBase
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0006965E File Offset: 0x0006785E
		// (set) Token: 0x060010D0 RID: 4304 RVA: 0x00069666 File Offset: 0x00067866
		public Rect uvRect
		{
			get
			{
				return this.m_UVRect;
			}
			set
			{
				if (this.m_UVRect == value)
				{
					return;
				}
				this.m_UVRect = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x00069684 File Offset: 0x00067884
		// (set) Token: 0x060010D2 RID: 4306 RVA: 0x0006968C File Offset: 0x0006788C
		public Vector2[] Points
		{
			get
			{
				return this.m_points;
			}
			set
			{
				if (this.m_points == value)
				{
					return;
				}
				this.m_points = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x000696A8 File Offset: 0x000678A8
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this.m_points == null || this.m_points.Length < 2)
			{
				this.m_points = new Vector2[]
				{
					new Vector2(0f, 0f),
					new Vector2(1f, 1f)
				};
			}
			int num = 24;
			float num2 = base.rectTransform.rect.width;
			float num3 = base.rectTransform.rect.height;
			float num4 = -base.rectTransform.pivot.x * base.rectTransform.rect.width;
			float num5 = -base.rectTransform.pivot.y * base.rectTransform.rect.height;
			if (!this.relativeSize)
			{
				num2 = 1f;
				num3 = 1f;
			}
			List<Vector2> list = new List<Vector2>();
			list.Add(this.m_points[0]);
			Vector2 item = this.m_points[0] + (this.m_points[1] - this.m_points[0]).normalized * (float)num;
			list.Add(item);
			for (int i = 1; i < this.m_points.Length - 1; i++)
			{
				list.Add(this.m_points[i]);
			}
			item = this.m_points[this.m_points.Length - 1] - (this.m_points[this.m_points.Length - 1] - this.m_points[this.m_points.Length - 2]).normalized * (float)num;
			list.Add(item);
			list.Add(this.m_points[this.m_points.Length - 1]);
			Vector2[] array = list.ToArray();
			if (this.UseMargins)
			{
				num2 -= this.Margin.x;
				num3 -= this.Margin.y;
				num4 += this.Margin.x / 2f;
				num5 += this.Margin.y / 2f;
			}
			vh.Clear();
			Vector2 vector = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			for (int j = 1; j < array.Length; j++)
			{
				Vector2 vector3 = array[j - 1];
				Vector2 vector4 = array[j];
				vector3 = new Vector2(vector3.x * num2 + num4, vector3.y * num3 + num5);
				vector4 = new Vector2(vector4.x * num2 + num4, vector4.y * num3 + num5);
				float z = Mathf.Atan2(vector4.y - vector3.y, vector4.x - vector3.x) * 180f / 3.1415927f;
				Vector2 vector5 = vector3 + new Vector2(0f, -this.LineThickness / 2f);
				Vector2 vector6 = vector3 + new Vector2(0f, this.LineThickness / 2f);
				Vector2 vector7 = vector4 + new Vector2(0f, this.LineThickness / 2f);
				Vector2 vector8 = vector4 + new Vector2(0f, -this.LineThickness / 2f);
				vector5 = this.RotatePointAroundPivot(vector5, vector3, new Vector3(0f, 0f, z));
				vector6 = this.RotatePointAroundPivot(vector6, vector3, new Vector3(0f, 0f, z));
				vector7 = this.RotatePointAroundPivot(vector7, vector4, new Vector3(0f, 0f, z));
				vector8 = this.RotatePointAroundPivot(vector8, vector4, new Vector3(0f, 0f, z));
				Vector2 zero = Vector2.zero;
				Vector2 vector9 = new Vector2(0f, 1f);
				Vector2 vector10 = new Vector2(0.5f, 0f);
				Vector2 vector11 = new Vector2(0.5f, 1f);
				Vector2 vector12 = new Vector2(1f, 0f);
				Vector2 vector13 = new Vector2(1f, 1f);
				Vector2[] uvs = new Vector2[]
				{
					vector10,
					vector11,
					vector11,
					vector10
				};
				if (j > 1)
				{
					vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
					{
						vector,
						vector2,
						vector5,
						vector6
					}, uvs));
				}
				if (j == 1)
				{
					uvs = new Vector2[]
					{
						zero,
						vector9,
						vector11,
						vector10
					};
				}
				else if (j == array.Length - 1)
				{
					uvs = new Vector2[]
					{
						vector10,
						vector11,
						vector13,
						vector12
					};
				}
				vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
				{
					vector5,
					vector6,
					vector7,
					vector8
				}, uvs));
				vector = vector7;
				vector2 = vector8;
			}
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00069C48 File Offset: 0x00067E48
		public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
		{
			Vector3 vector = point - pivot;
			vector = Quaternion.Euler(angles) * vector;
			point = vector + pivot;
			return point;
		}

		// Token: 0x04000FA0 RID: 4000
		[SerializeField]
		private Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04000FA1 RID: 4001
		[SerializeField]
		private Vector2[] m_points;

		// Token: 0x04000FA2 RID: 4002
		public float LineThickness = 2f;

		// Token: 0x04000FA3 RID: 4003
		public bool UseMargins;

		// Token: 0x04000FA4 RID: 4004
		public Vector2 Margin;

		// Token: 0x04000FA5 RID: 4005
		public bool relativeSize;
	}
}
