using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000163 RID: 355
	[AddComponentMenu("UI/Effects/Extensions/UIAdditiveEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIAdditiveEffect : MonoBehaviour
	{
		// Token: 0x06000DCC RID: 3532 RVA: 0x00058E04 File Offset: 0x00057004
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00058E0C File Offset: 0x0005700C
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIAdditive"));
					return;
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00058E04 File Offset: 0x00057004
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04000D84 RID: 3460
		private MaskableGraphic mGraphic;
	}
}
