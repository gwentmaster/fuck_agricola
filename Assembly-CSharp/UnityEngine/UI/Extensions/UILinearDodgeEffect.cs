using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000165 RID: 357
	[AddComponentMenu("UI/Effects/Extensions/UILinearDodgeEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UILinearDodgeEffect : MonoBehaviour
	{
		// Token: 0x06000DD6 RID: 3542 RVA: 0x00058FA6 File Offset: 0x000571A6
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00058FB0 File Offset: 0x000571B0
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UILinearDodge"));
					return;
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00058FA6 File Offset: 0x000571A6
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04000D8B RID: 3467
		private MaskableGraphic mGraphic;
	}
}
