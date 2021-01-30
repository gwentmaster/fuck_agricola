using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000167 RID: 359
	[AddComponentMenu("UI/Effects/Extensions/UIScreenEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIScreenEffect : MonoBehaviour
	{
		// Token: 0x06000DDE RID: 3550 RVA: 0x000590AF File Offset: 0x000572AF
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000590B8 File Offset: 0x000572B8
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIScreen"));
					return;
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x000590AF File Offset: 0x000572AF
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04000D8D RID: 3469
		private MaskableGraphic mGraphic;
	}
}
