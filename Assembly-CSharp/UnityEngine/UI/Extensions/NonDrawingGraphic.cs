using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C2 RID: 450
	[AddComponentMenu("Layout/Extensions/NonDrawingGraphic")]
	public class NonDrawingGraphic : MaskableGraphic
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x00003022 File Offset: 0x00001222
		public override void SetMaterialDirty()
		{
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00003022 File Offset: 0x00001222
		public override void SetVerticesDirty()
		{
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0006CBAA File Offset: 0x0006ADAA
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}
