using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000164 RID: 356
	[AddComponentMenu("UI/Effects/Extensions/UIImageCrop")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIImageCrop : MonoBehaviour
	{
		// Token: 0x06000DD0 RID: 3536 RVA: 0x00058E87 File Offset: 0x00057087
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00058E90 File Offset: 0x00057090
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			this.XCropProperty = Shader.PropertyToID("_XCrop");
			this.YCropProperty = Shader.PropertyToID("_YCrop");
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UI Image Crop"));
				}
				this.mat = this.mGraphic.material;
				return;
			}
			Debug.LogError("Please attach component to a Graphical UI component");
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00058F3C File Offset: 0x0005713C
		public void OnValidate()
		{
			this.SetMaterial();
			this.SetXCrop(this.XCrop);
			this.SetYCrop(this.YCrop);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00058F5C File Offset: 0x0005715C
		public void SetXCrop(float xcrop)
		{
			this.XCrop = Mathf.Clamp01(xcrop);
			this.mat.SetFloat(this.XCropProperty, this.XCrop);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00058F81 File Offset: 0x00057181
		public void SetYCrop(float ycrop)
		{
			this.YCrop = Mathf.Clamp01(ycrop);
			this.mat.SetFloat(this.YCropProperty, this.YCrop);
		}

		// Token: 0x04000D85 RID: 3461
		private MaskableGraphic mGraphic;

		// Token: 0x04000D86 RID: 3462
		private Material mat;

		// Token: 0x04000D87 RID: 3463
		private int XCropProperty;

		// Token: 0x04000D88 RID: 3464
		private int YCropProperty;

		// Token: 0x04000D89 RID: 3465
		public float XCrop;

		// Token: 0x04000D8A RID: 3466
		public float YCrop;
	}
}
