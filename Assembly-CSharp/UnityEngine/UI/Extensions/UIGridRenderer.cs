using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001AF RID: 431
	[AddComponentMenu("UI/Extensions/Primitives/UIGridRenderer")]
	public class UIGridRenderer : UILineRenderer
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x000687F5 File Offset: 0x000669F5
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x000687FD File Offset: 0x000669FD
		public int GridColumns
		{
			get
			{
				return this.m_GridColumns;
			}
			set
			{
				if (this.m_GridColumns == value)
				{
					return;
				}
				this.m_GridColumns = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x00068816 File Offset: 0x00066A16
		// (set) Token: 0x060010BA RID: 4282 RVA: 0x0006881E File Offset: 0x00066A1E
		public int GridRows
		{
			get
			{
				return this.m_GridRows;
			}
			set
			{
				if (this.m_GridRows == value)
				{
					return;
				}
				this.m_GridRows = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00068838 File Offset: 0x00066A38
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			this.relativeSize = true;
			int num = this.GridRows * 3 + 1;
			if (this.GridRows % 2 == 0)
			{
				num++;
			}
			num += this.GridColumns * 3 + 1;
			this.m_points = new Vector2[num];
			int num2 = 0;
			for (int i = 0; i < this.GridRows; i++)
			{
				float x = 1f;
				float x2 = 0f;
				if (i % 2 == 0)
				{
					x = 0f;
					x2 = 1f;
				}
				float y = (float)i / (float)this.GridRows;
				this.m_points[num2].x = x;
				this.m_points[num2].y = y;
				num2++;
				this.m_points[num2].x = x2;
				this.m_points[num2].y = y;
				num2++;
				this.m_points[num2].x = x2;
				this.m_points[num2].y = (float)(i + 1) / (float)this.GridRows;
				num2++;
			}
			if (this.GridRows % 2 == 0)
			{
				this.m_points[num2].x = 1f;
				this.m_points[num2].y = 1f;
				num2++;
			}
			this.m_points[num2].x = 0f;
			this.m_points[num2].y = 1f;
			num2++;
			for (int j = 0; j < this.GridColumns; j++)
			{
				float y2 = 1f;
				float y3 = 0f;
				if (j % 2 == 0)
				{
					y2 = 0f;
					y3 = 1f;
				}
				float x3 = (float)j / (float)this.GridColumns;
				this.m_points[num2].x = x3;
				this.m_points[num2].y = y2;
				num2++;
				this.m_points[num2].x = x3;
				this.m_points[num2].y = y3;
				num2++;
				this.m_points[num2].x = (float)(j + 1) / (float)this.GridColumns;
				this.m_points[num2].y = y3;
				num2++;
			}
			if (this.GridColumns % 2 == 0)
			{
				this.m_points[num2].x = 1f;
				this.m_points[num2].y = 1f;
			}
			else
			{
				this.m_points[num2].x = 1f;
				this.m_points[num2].y = 0f;
			}
			base.OnPopulateMesh(vh);
		}

		// Token: 0x04000F87 RID: 3975
		[SerializeField]
		private int m_GridColumns = 10;

		// Token: 0x04000F88 RID: 3976
		[SerializeField]
		private int m_GridRows = 10;
	}
}
