using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001AE RID: 430
	[AddComponentMenu("UI/Extensions/Primitives/Cut Corners")]
	public class UICornerCut : UIPrimitiveBase
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00068212 File Offset: 0x00066412
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x0006821A File Offset: 0x0006641A
		public bool CutUL
		{
			get
			{
				return this.m_cutUL;
			}
			set
			{
				this.m_cutUL = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00068229 File Offset: 0x00066429
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x00068231 File Offset: 0x00066431
		public bool CutUR
		{
			get
			{
				return this.m_cutUR;
			}
			set
			{
				this.m_cutUR = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00068240 File Offset: 0x00066440
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x00068248 File Offset: 0x00066448
		public bool CutLL
		{
			get
			{
				return this.m_cutLL;
			}
			set
			{
				this.m_cutLL = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x00068257 File Offset: 0x00066457
		// (set) Token: 0x060010A7 RID: 4263 RVA: 0x0006825F File Offset: 0x0006645F
		public bool CutLR
		{
			get
			{
				return this.m_cutLR;
			}
			set
			{
				this.m_cutLR = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0006826E File Offset: 0x0006646E
		// (set) Token: 0x060010A9 RID: 4265 RVA: 0x00068276 File Offset: 0x00066476
		public bool MakeColumns
		{
			get
			{
				return this.m_makeColumns;
			}
			set
			{
				this.m_makeColumns = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x00068285 File Offset: 0x00066485
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x0006828D File Offset: 0x0006648D
		public bool UseColorUp
		{
			get
			{
				return this.m_useColorUp;
			}
			set
			{
				this.m_useColorUp = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x00068296 File Offset: 0x00066496
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x0006829E File Offset: 0x0006649E
		public Color32 ColorUp
		{
			get
			{
				return this.m_colorUp;
			}
			set
			{
				this.m_colorUp = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x000682A7 File Offset: 0x000664A7
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x000682AF File Offset: 0x000664AF
		public bool UseColorDown
		{
			get
			{
				return this.m_useColorDown;
			}
			set
			{
				this.m_useColorDown = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x000682B8 File Offset: 0x000664B8
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x000682C0 File Offset: 0x000664C0
		public Color32 ColorDown
		{
			get
			{
				return this.m_colorDown;
			}
			set
			{
				this.m_colorDown = value;
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x000682CC File Offset: 0x000664CC
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			Rect rect = base.rectTransform.rect;
			Rect rect2 = rect;
			Color32 color = this.color;
			bool flag = this.m_cutUL | this.m_cutUR;
			bool flag2 = this.m_cutLL | this.m_cutLR;
			bool flag3 = this.m_cutLL | this.m_cutUL;
			bool flag4 = this.m_cutLR | this.m_cutUR;
			if ((flag || flag2) && this.cornerSize.sqrMagnitude > 0f)
			{
				vh.Clear();
				if (flag3)
				{
					rect2.xMin += this.cornerSize.x;
				}
				if (flag2)
				{
					rect2.yMin += this.cornerSize.y;
				}
				if (flag)
				{
					rect2.yMax -= this.cornerSize.y;
				}
				if (flag4)
				{
					rect2.xMax -= this.cornerSize.x;
				}
				if (this.m_makeColumns)
				{
					Vector2 vector = new Vector2(rect.xMin, this.m_cutUL ? rect2.yMax : rect.yMax);
					Vector2 vector2 = new Vector2(rect.xMax, this.m_cutUR ? rect2.yMax : rect.yMax);
					Vector2 vector3 = new Vector2(rect.xMin, this.m_cutLL ? rect2.yMin : rect.yMin);
					Vector2 vector4 = new Vector2(rect.xMax, this.m_cutLR ? rect2.yMin : rect.yMin);
					if (flag3)
					{
						UICornerCut.AddSquare(vector3, vector, new Vector2(rect2.xMin, rect.yMax), new Vector2(rect2.xMin, rect.yMin), rect, this.m_useColorUp ? this.m_colorUp : color, vh);
					}
					if (flag4)
					{
						UICornerCut.AddSquare(vector2, vector4, new Vector2(rect2.xMax, rect.yMin), new Vector2(rect2.xMax, rect.yMax), rect, this.m_useColorDown ? this.m_colorDown : color, vh);
					}
				}
				else
				{
					Vector2 vector = new Vector2(this.m_cutUL ? rect2.xMin : rect.xMin, rect.yMax);
					Vector2 vector2 = new Vector2(this.m_cutUR ? rect2.xMax : rect.xMax, rect.yMax);
					Vector2 vector3 = new Vector2(this.m_cutLL ? rect2.xMin : rect.xMin, rect.yMin);
					Vector2 vector4 = new Vector2(this.m_cutLR ? rect2.xMax : rect.xMax, rect.yMin);
					if (flag2)
					{
						UICornerCut.AddSquare(vector4, vector3, new Vector2(rect.xMin, rect2.yMin), new Vector2(rect.xMax, rect2.yMin), rect, this.m_useColorDown ? this.m_colorDown : color, vh);
					}
					if (flag)
					{
						UICornerCut.AddSquare(vector, vector2, new Vector2(rect.xMax, rect2.yMax), new Vector2(rect.xMin, rect2.yMax), rect, this.m_useColorUp ? this.m_colorUp : color, vh);
					}
				}
				if (this.m_makeColumns)
				{
					UICornerCut.AddSquare(new Rect(rect2.xMin, rect.yMin, rect2.width, rect.height), rect, color, vh);
					return;
				}
				UICornerCut.AddSquare(new Rect(rect.xMin, rect2.yMin, rect.width, rect2.height), rect, color, vh);
			}
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0006867C File Offset: 0x0006687C
		private static void AddSquare(Rect rect, Rect rectUV, Color32 color32, VertexHelper vh)
		{
			int num = UICornerCut.AddVert(rect.xMin, rect.yMin, rectUV, color32, vh);
			int idx = UICornerCut.AddVert(rect.xMin, rect.yMax, rectUV, color32, vh);
			int num2 = UICornerCut.AddVert(rect.xMax, rect.yMax, rectUV, color32, vh);
			int idx2 = UICornerCut.AddVert(rect.xMax, rect.yMin, rectUV, color32, vh);
			vh.AddTriangle(num, idx, num2);
			vh.AddTriangle(num2, idx2, num);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x000686F8 File Offset: 0x000668F8
		private static void AddSquare(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Rect rectUV, Color32 color32, VertexHelper vh)
		{
			int num = UICornerCut.AddVert(a.x, a.y, rectUV, color32, vh);
			int idx = UICornerCut.AddVert(b.x, b.y, rectUV, color32, vh);
			int num2 = UICornerCut.AddVert(c.x, c.y, rectUV, color32, vh);
			int idx2 = UICornerCut.AddVert(d.x, d.y, rectUV, color32, vh);
			vh.AddTriangle(num, idx, num2);
			vh.AddTriangle(num2, idx2, num);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0006877C File Offset: 0x0006697C
		private static int AddVert(float x, float y, Rect area, Color32 color32, VertexHelper vh)
		{
			Vector2 uv = new Vector2(Mathf.InverseLerp(area.xMin, area.xMax, x), Mathf.InverseLerp(area.yMin, area.yMax, y));
			vh.AddVert(new Vector3(x, y), color32, uv);
			return vh.currentVertCount - 1;
		}

		// Token: 0x04000F7D RID: 3965
		public Vector2 cornerSize = new Vector2(16f, 16f);

		// Token: 0x04000F7E RID: 3966
		[Header("Corners to cut")]
		[SerializeField]
		private bool m_cutUL = true;

		// Token: 0x04000F7F RID: 3967
		[SerializeField]
		private bool m_cutUR;

		// Token: 0x04000F80 RID: 3968
		[SerializeField]
		private bool m_cutLL;

		// Token: 0x04000F81 RID: 3969
		[SerializeField]
		private bool m_cutLR;

		// Token: 0x04000F82 RID: 3970
		[Tooltip("Up-Down colors become Left-Right colors")]
		[SerializeField]
		private bool m_makeColumns;

		// Token: 0x04000F83 RID: 3971
		[Header("Color the cut bars differently")]
		[SerializeField]
		private bool m_useColorUp;

		// Token: 0x04000F84 RID: 3972
		[SerializeField]
		private Color32 m_colorUp;

		// Token: 0x04000F85 RID: 3973
		[SerializeField]
		private bool m_useColorDown;

		// Token: 0x04000F86 RID: 3974
		[SerializeField]
		private Color32 m_colorDown;
	}
}
