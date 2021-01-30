using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000166 RID: 358
	[AddComponentMenu("UI/Effects/Extensions/UIMultiplyEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIMultiplyEffect : MonoBehaviour
	{
		// Token: 0x06000DDA RID: 3546 RVA: 0x0005902B File Offset: 0x0005722B
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00059034 File Offset: 0x00057234
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIMultiply"));
					return;
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0005902B File Offset: 0x0005722B
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04000D8C RID: 3468
		private MaskableGraphic mGraphic;
	}
}
