using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000161 RID: 353
	[AddComponentMenu("UI/Effects/Extensions/Nicer Outline")]
	public class NicerOutline : BaseMeshEffect
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00058717 File Offset: 0x00056917
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x0005871F File Offset: 0x0005691F
		public Color effectColor
		{
			get
			{
				return this.m_EffectColor;
			}
			set
			{
				this.m_EffectColor = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00058741 File Offset: 0x00056941
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x0005874C File Offset: 0x0005694C
		public Vector2 effectDistance
		{
			get
			{
				return this.m_EffectDistance;
			}
			set
			{
				if (value.x > 600f)
				{
					value.x = 600f;
				}
				if (value.x < -600f)
				{
					value.x = -600f;
				}
				if (value.y > 600f)
				{
					value.y = 600f;
				}
				if (value.y < -600f)
				{
					value.y = -600f;
				}
				if (this.m_EffectDistance == value)
				{
					return;
				}
				this.m_EffectDistance = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x000587EC File Offset: 0x000569EC
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x000587F4 File Offset: 0x000569F4
		public bool useGraphicAlpha
		{
			get
			{
				return this.m_UseGraphicAlpha;
			}
			set
			{
				this.m_UseGraphicAlpha = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00058818 File Offset: 0x00056A18
		protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			for (int i = start; i < end; i++)
			{
				UIVertex uivertex = verts[i];
				verts.Add(uivertex);
				Vector3 position = uivertex.position;
				position.x += x;
				position.y += y;
				uivertex.position = position;
				Color32 color2 = color;
				if (this.m_UseGraphicAlpha)
				{
					color2.a = color2.a * verts[i].color.a / byte.MaxValue;
				}
				uivertex.color = color2;
				verts[i] = uivertex;
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x000588CC File Offset: 0x00056ACC
		protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			this.ApplyShadowZeroAlloc(verts, color, start, end, x, y);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00058904 File Offset: 0x00056B04
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			Text component = base.GetComponent<Text>();
			float num = 1f;
			if (component && component.resizeTextForBestFit)
			{
				num = (float)component.cachedTextGenerator.fontSizeUsedForBestFit / (float)(component.resizeTextMaxSize - 1);
			}
			float num2 = this.effectDistance.x * num;
			float num3 = this.effectDistance.y * num;
			int start = 0;
			int count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, num3);
			start = count;
			int count2 = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, -num3);
			start = count2;
			int count3 = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, num3);
			start = count3;
			int count4 = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, -num3);
			start = count4;
			int count5 = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, 0f);
			start = count5;
			int count6 = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, 0f);
			start = count6;
			int count7 = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, 0f, num3);
			start = count7;
			int count8 = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, 0f, -num3);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x04000D7F RID: 3455
		[SerializeField]
		private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04000D80 RID: 3456
		[SerializeField]
		private Vector2 m_EffectDistance = new Vector2(1f, -1f);

		// Token: 0x04000D81 RID: 3457
		[SerializeField]
		private bool m_UseGraphicAlpha = true;
	}
}
