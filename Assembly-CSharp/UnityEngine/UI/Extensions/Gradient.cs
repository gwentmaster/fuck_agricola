using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200015D RID: 349
	[AddComponentMenu("UI/Effects/Extensions/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x00058018 File Offset: 0x00056218
		// (set) Token: 0x06000DAE RID: 3502 RVA: 0x00058020 File Offset: 0x00056220
		public GradientMode GradientMode
		{
			get
			{
				return this._gradientMode;
			}
			set
			{
				this._gradientMode = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x00058034 File Offset: 0x00056234
		// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x0005803C File Offset: 0x0005623C
		public GradientDir GradientDir
		{
			get
			{
				return this._gradientDir;
			}
			set
			{
				this._gradientDir = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00058050 File Offset: 0x00056250
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x00058058 File Offset: 0x00056258
		public bool OverwriteAllColor
		{
			get
			{
				return this._overwriteAllColor;
			}
			set
			{
				this._overwriteAllColor = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0005806C File Offset: 0x0005626C
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x00058074 File Offset: 0x00056274
		public Color Vertex1
		{
			get
			{
				return this._vertex1;
			}
			set
			{
				this._vertex1 = value;
				base.graphic.SetAllDirty();
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00058088 File Offset: 0x00056288
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x00058090 File Offset: 0x00056290
		public Color Vertex2
		{
			get
			{
				return this._vertex2;
			}
			set
			{
				this._vertex2 = value;
				base.graphic.SetAllDirty();
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000580A4 File Offset: 0x000562A4
		protected override void Awake()
		{
			this.targetGraphic = base.GetComponent<Graphic>();
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x000580B4 File Offset: 0x000562B4
		public override void ModifyMesh(VertexHelper vh)
		{
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			UIVertex uivertex = default(UIVertex);
			if (this._gradientMode == GradientMode.Global)
			{
				if (this._gradientDir == GradientDir.DiagonalLeftToRight || this._gradientDir == GradientDir.DiagonalRightToLeft)
				{
					this._gradientDir = GradientDir.Vertical;
				}
				float num = (this._gradientDir == GradientDir.Vertical) ? list[list.Count - 1].position.y : list[list.Count - 1].position.x;
				float num2 = ((this._gradientDir == GradientDir.Vertical) ? list[0].position.y : list[0].position.x) - num;
				for (int i = 0; i < currentVertCount; i++)
				{
					vh.PopulateUIVertex(ref uivertex, i);
					if (this._overwriteAllColor || !(uivertex.color != this.targetGraphic.color))
					{
						uivertex.color *= Color.Lerp(this._vertex2, this._vertex1, (((this._gradientDir == GradientDir.Vertical) ? uivertex.position.y : uivertex.position.x) - num) / num2);
						vh.SetUIVertex(uivertex, i);
					}
				}
				return;
			}
			for (int j = 0; j < currentVertCount; j++)
			{
				vh.PopulateUIVertex(ref uivertex, j);
				if (this._overwriteAllColor || this.CompareCarefully(uivertex.color, this.targetGraphic.color))
				{
					switch (this._gradientDir)
					{
					case GradientDir.Vertical:
						uivertex.color *= ((j % 4 == 0 || (j - 1) % 4 == 0) ? this._vertex1 : this._vertex2);
						break;
					case GradientDir.Horizontal:
						uivertex.color *= ((j % 4 == 0 || (j - 3) % 4 == 0) ? this._vertex1 : this._vertex2);
						break;
					case GradientDir.DiagonalLeftToRight:
						uivertex.color *= ((j % 4 == 0) ? this._vertex1 : (((j - 2) % 4 == 0) ? this._vertex2 : Color.Lerp(this._vertex2, this._vertex1, 0.5f)));
						break;
					case GradientDir.DiagonalRightToLeft:
						uivertex.color *= (((j - 1) % 4 == 0) ? this._vertex1 : (((j - 3) % 4 == 0) ? this._vertex2 : Color.Lerp(this._vertex2, this._vertex1, 0.5f)));
						break;
					}
					vh.SetUIVertex(uivertex, j);
				}
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000583C8 File Offset: 0x000565C8
		private bool CompareCarefully(Color col1, Color col2)
		{
			return Mathf.Abs(col1.r - col2.r) < 0.003f && Mathf.Abs(col1.g - col2.g) < 0.003f && Mathf.Abs(col1.b - col2.b) < 0.003f && Mathf.Abs(col1.a - col2.a) < 0.003f;
		}

		// Token: 0x04000D70 RID: 3440
		[SerializeField]
		private GradientMode _gradientMode;

		// Token: 0x04000D71 RID: 3441
		[SerializeField]
		private GradientDir _gradientDir;

		// Token: 0x04000D72 RID: 3442
		[SerializeField]
		private bool _overwriteAllColor;

		// Token: 0x04000D73 RID: 3443
		[SerializeField]
		private Color _vertex1 = Color.white;

		// Token: 0x04000D74 RID: 3444
		[SerializeField]
		private Color _vertex2 = Color.black;

		// Token: 0x04000D75 RID: 3445
		private Graphic targetGraphic;
	}
}
