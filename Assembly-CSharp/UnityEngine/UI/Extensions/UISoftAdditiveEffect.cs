using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000168 RID: 360
	[AddComponentMenu("UI/Effects/Extensions/UISoftAdditiveEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UISoftAdditiveEffect : MonoBehaviour
	{
		// Token: 0x06000DE2 RID: 3554 RVA: 0x00059133 File Offset: 0x00057333
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0005913C File Offset: 0x0005733C
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UISoftAdditive"));
					return;
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00059133 File Offset: 0x00057333
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04000D8E RID: 3470
		private MaskableGraphic mGraphic;
	}
}
