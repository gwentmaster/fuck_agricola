using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000191 RID: 401
	[RequireComponent(typeof(Text), typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/Extensions/Cylinder Text")]
	public class CylinderText : BaseMeshEffect
	{
		// Token: 0x06000F7E RID: 3966 RVA: 0x00062105 File Offset: 0x00060305
		protected override void Awake()
		{
			base.Awake();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0006211F File Offset: 0x0006031F
		protected override void OnEnable()
		{
			base.OnEnable();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0006213C File Offset: 0x0006033C
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				UIVertex uivertex = default(UIVertex);
				vh.PopulateUIVertex(ref uivertex, i);
				float x = uivertex.position.x;
				uivertex.position.z = -this.radius * Mathf.Cos(x / this.radius);
				uivertex.position.x = this.radius * Mathf.Sin(x / this.radius);
				vh.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x04000ECE RID: 3790
		public float radius;

		// Token: 0x04000ECF RID: 3791
		private RectTransform rectTrans;
	}
}
