using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A4 RID: 420
	[AddComponentMenu("Layout/Extensions/Table Layout Group")]
	public class TableLayoutGroup : LayoutGroup
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x00066431 File Offset: 0x00064631
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x00066439 File Offset: 0x00064639
		public TableLayoutGroup.Corner StartCorner
		{
			get
			{
				return this.startCorner;
			}
			set
			{
				base.SetProperty<TableLayoutGroup.Corner>(ref this.startCorner, value);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00066448 File Offset: 0x00064648
		// (set) Token: 0x06001036 RID: 4150 RVA: 0x00066450 File Offset: 0x00064650
		public float[] ColumnWidths
		{
			get
			{
				return this.columnWidths;
			}
			set
			{
				base.SetProperty<float[]>(ref this.columnWidths, value);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0006645F File Offset: 0x0006465F
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x00066467 File Offset: 0x00064667
		public float MinimumRowHeight
		{
			get
			{
				return this.minimumRowHeight;
			}
			set
			{
				base.SetProperty<float>(ref this.minimumRowHeight, value);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x00066476 File Offset: 0x00064676
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x0006647E File Offset: 0x0006467E
		public bool FlexibleRowHeight
		{
			get
			{
				return this.flexibleRowHeight;
			}
			set
			{
				base.SetProperty<bool>(ref this.flexibleRowHeight, value);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x0006648D File Offset: 0x0006468D
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x00066495 File Offset: 0x00064695
		public float ColumnSpacing
		{
			get
			{
				return this.columnSpacing;
			}
			set
			{
				base.SetProperty<float>(ref this.columnSpacing, value);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x000664A4 File Offset: 0x000646A4
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x000664AC File Offset: 0x000646AC
		public float RowSpacing
		{
			get
			{
				return this.rowSpacing;
			}
			set
			{
				base.SetProperty<float>(ref this.rowSpacing, value);
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000664BC File Offset: 0x000646BC
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			float num = (float)base.padding.horizontal;
			int num2 = Mathf.Min(base.rectChildren.Count, this.columnWidths.Length);
			for (int i = 0; i < num2; i++)
			{
				num += this.columnWidths[i];
				num += this.columnSpacing;
			}
			num -= this.columnSpacing;
			base.SetLayoutInputForAxis(num, num, 0f, 0);
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0006652C File Offset: 0x0006472C
		public override void CalculateLayoutInputVertical()
		{
			int num = this.columnWidths.Length;
			int num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num);
			this.preferredRowHeights = new float[num2];
			float num3 = (float)base.padding.vertical;
			float num4 = (float)base.padding.vertical;
			if (num2 > 1)
			{
				float num5 = (float)(num2 - 1) * this.rowSpacing;
				num3 += num5;
				num4 += num5;
			}
			if (this.flexibleRowHeight)
			{
				for (int i = 0; i < num2; i++)
				{
					float num6 = this.minimumRowHeight;
					float num7 = this.minimumRowHeight;
					for (int j = 0; j < num; j++)
					{
						int num8 = i * num + j;
						if (num8 == base.rectChildren.Count)
						{
							break;
						}
						num7 = Mathf.Max(LayoutUtility.GetPreferredHeight(base.rectChildren[num8]), num7);
						num6 = Mathf.Max(LayoutUtility.GetMinHeight(base.rectChildren[num8]), num6);
					}
					num3 += num6;
					num4 += num7;
					this.preferredRowHeights[i] = num7;
				}
			}
			else
			{
				for (int k = 0; k < num2; k++)
				{
					this.preferredRowHeights[k] = this.minimumRowHeight;
				}
				num3 += (float)num2 * this.minimumRowHeight;
				num4 = num3;
			}
			num4 = Mathf.Max(num3, num4);
			base.SetLayoutInputForAxis(num3, num4, 1f, 1);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00066690 File Offset: 0x00064890
		public override void SetLayoutHorizontal()
		{
			if (this.columnWidths.Length == 0)
			{
				this.columnWidths = new float[1];
			}
			int num = this.columnWidths.Length;
			int num2 = (int)(this.startCorner % TableLayoutGroup.Corner.LowerLeft);
			float num3 = 0f;
			int num4 = Mathf.Min(base.rectChildren.Count, this.columnWidths.Length);
			for (int i = 0; i < num4; i++)
			{
				num3 += this.columnWidths[i];
				num3 += this.columnSpacing;
			}
			num3 -= this.columnSpacing;
			float num5 = base.GetStartOffset(0, num3);
			if (num2 == 1)
			{
				num5 += num3;
			}
			float num6 = num5;
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				int num7 = j % num;
				if (num7 == 0)
				{
					num6 = num5;
				}
				if (num2 == 1)
				{
					num6 -= this.columnWidths[num7];
				}
				base.SetChildAlongAxis(base.rectChildren[j], 0, num6, this.columnWidths[num7]);
				if (num2 == 1)
				{
					num6 -= this.columnSpacing;
				}
				else
				{
					num6 += this.columnWidths[num7] + this.columnSpacing;
				}
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x000667AC File Offset: 0x000649AC
		public override void SetLayoutVertical()
		{
			int num = this.columnWidths.Length;
			int num2 = this.preferredRowHeights.Length;
			int num3 = (int)(this.startCorner / TableLayoutGroup.Corner.LowerLeft);
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				num4 += this.preferredRowHeights[i];
			}
			if (num2 > 1)
			{
				num4 += (float)(num2 - 1) * this.rowSpacing;
			}
			float num5 = base.GetStartOffset(1, num4);
			if (num3 == 1)
			{
				num5 += num4;
			}
			float num6 = num5;
			for (int j = 0; j < num2; j++)
			{
				if (num3 == 1)
				{
					num6 -= this.preferredRowHeights[j];
				}
				for (int k = 0; k < num; k++)
				{
					int num7 = j * num + k;
					if (num7 == base.rectChildren.Count)
					{
						break;
					}
					base.SetChildAlongAxis(base.rectChildren[num7], 1, num6, this.preferredRowHeights[j]);
				}
				if (num3 == 1)
				{
					num6 -= this.rowSpacing;
				}
				else
				{
					num6 += this.preferredRowHeights[j] + this.rowSpacing;
				}
			}
			this.preferredRowHeights = null;
		}

		// Token: 0x04000F55 RID: 3925
		[SerializeField]
		protected TableLayoutGroup.Corner startCorner;

		// Token: 0x04000F56 RID: 3926
		[SerializeField]
		protected float[] columnWidths = new float[]
		{
			96f
		};

		// Token: 0x04000F57 RID: 3927
		[SerializeField]
		protected float minimumRowHeight = 32f;

		// Token: 0x04000F58 RID: 3928
		[SerializeField]
		protected bool flexibleRowHeight = true;

		// Token: 0x04000F59 RID: 3929
		[SerializeField]
		protected float columnSpacing;

		// Token: 0x04000F5A RID: 3930
		[SerializeField]
		protected float rowSpacing;

		// Token: 0x04000F5B RID: 3931
		private float[] preferredRowHeights;

		// Token: 0x0200085C RID: 2140
		public enum Corner
		{
			// Token: 0x04002ED1 RID: 11985
			UpperLeft,
			// Token: 0x04002ED2 RID: 11986
			UpperRight,
			// Token: 0x04002ED3 RID: 11987
			LowerLeft,
			// Token: 0x04002ED4 RID: 11988
			LowerRight
		}
	}
}
