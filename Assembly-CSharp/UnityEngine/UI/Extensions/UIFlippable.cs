using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200016A RID: 362
	[RequireComponent(typeof(RectTransform), typeof(Graphic))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Effects/Extensions/Flippable")]
	public class UIFlippable : BaseMeshEffect
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x000594F4 File Offset: 0x000576F4
		// (set) Token: 0x06000DEC RID: 3564 RVA: 0x000594FC File Offset: 0x000576FC
		public bool horizontal
		{
			get
			{
				return this.m_Horizontal;
			}
			set
			{
				this.m_Horizontal = value;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x00059505 File Offset: 0x00057705
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x0005950D File Offset: 0x0005770D
		public bool vertical
		{
			get
			{
				return this.m_Veritical;
			}
			set
			{
				this.m_Veritical = value;
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00059518 File Offset: 0x00057718
		public override void ModifyMesh(VertexHelper verts)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			for (int i = 0; i < verts.currentVertCount; i++)
			{
				UIVertex uivertex = default(UIVertex);
				verts.PopulateUIVertex(ref uivertex, i);
				uivertex.position = new Vector3(this.m_Horizontal ? (uivertex.position.x + (rectTransform.rect.center.x - uivertex.position.x) * 2f) : uivertex.position.x, this.m_Veritical ? (uivertex.position.y + (rectTransform.rect.center.y - uivertex.position.y) * 2f) : uivertex.position.y, uivertex.position.z);
				verts.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x04000D9C RID: 3484
		[SerializeField]
		private bool m_Horizontal;

		// Token: 0x04000D9D RID: 3485
		[SerializeField]
		private bool m_Veritical;
	}
}
