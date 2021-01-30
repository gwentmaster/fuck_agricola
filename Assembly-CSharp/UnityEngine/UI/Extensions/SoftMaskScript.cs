using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000169 RID: 361
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effects/Extensions/SoftMaskScript")]
	public class SoftMaskScript : MonoBehaviour
	{
		// Token: 0x06000DE6 RID: 3558 RVA: 0x000591B8 File Offset: 0x000573B8
		private void Start()
		{
			if (this.MaskArea == null)
			{
				this.MaskArea = base.GetComponent<RectTransform>();
			}
			Text component = base.GetComponent<Text>();
			if (component != null)
			{
				this.mat = new Material(Shader.Find("UI Extensions/SoftMaskShader"));
				component.material = this.mat;
				this.cachedCanvas = component.canvas;
				this.cachedCanvasTransform = this.cachedCanvas.transform;
				if (base.transform.parent.GetComponent<Mask>() == null)
				{
					base.transform.parent.gameObject.AddComponent<Mask>();
				}
				base.transform.parent.GetComponent<Mask>().enabled = false;
				return;
			}
			Graphic component2 = base.GetComponent<Graphic>();
			if (component2 != null)
			{
				this.mat = new Material(Shader.Find("UI Extensions/SoftMaskShader"));
				component2.material = this.mat;
				this.cachedCanvas = component2.canvas;
				this.cachedCanvasTransform = this.cachedCanvas.transform;
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x000592C3 File Offset: 0x000574C3
		private void Update()
		{
			if (this.cachedCanvas != null)
			{
				this.SetMask();
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x000592DC File Offset: 0x000574DC
		private void SetMask()
		{
			Rect canvasRect = this.GetCanvasRect();
			Vector2 size = canvasRect.size;
			this.maskScale.Set(1f / size.x, 1f / size.y);
			this.maskOffset = -canvasRect.min;
			this.maskOffset.Scale(this.maskScale);
			this.mat.SetTextureOffset("_AlphaMask", this.maskOffset);
			this.mat.SetTextureScale("_AlphaMask", this.maskScale);
			this.mat.SetTexture("_AlphaMask", this.AlphaMask);
			this.mat.SetFloat("_HardBlend", (float)(this.HardBlend ? 1 : 0));
			this.mat.SetInt("_FlipAlphaMask", this.FlipAlphaMask ? 1 : 0);
			this.mat.SetInt("_NoOuterClip", this.DontClipMaskScalingRect ? 1 : 0);
			this.mat.SetFloat("_CutOff", this.CutOff);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x000593EC File Offset: 0x000575EC
		public Rect GetCanvasRect()
		{
			if (this.cachedCanvas == null)
			{
				return default(Rect);
			}
			this.MaskArea.GetWorldCorners(this.m_WorldCorners);
			for (int i = 0; i < 4; i++)
			{
				this.m_CanvasCorners[i] = this.cachedCanvasTransform.InverseTransformPoint(this.m_WorldCorners[i]);
			}
			return new Rect(this.m_CanvasCorners[0].x, this.m_CanvasCorners[0].y, this.m_CanvasCorners[2].x - this.m_CanvasCorners[0].x, this.m_CanvasCorners[2].y - this.m_CanvasCorners[0].y);
		}

		// Token: 0x04000D8F RID: 3471
		private Material mat;

		// Token: 0x04000D90 RID: 3472
		private Canvas cachedCanvas;

		// Token: 0x04000D91 RID: 3473
		private Transform cachedCanvasTransform;

		// Token: 0x04000D92 RID: 3474
		private readonly Vector3[] m_WorldCorners = new Vector3[4];

		// Token: 0x04000D93 RID: 3475
		private readonly Vector3[] m_CanvasCorners = new Vector3[4];

		// Token: 0x04000D94 RID: 3476
		[Tooltip("The area that is to be used as the container.")]
		public RectTransform MaskArea;

		// Token: 0x04000D95 RID: 3477
		[Tooltip("Texture to be used to do the soft alpha")]
		public Texture AlphaMask;

		// Token: 0x04000D96 RID: 3478
		[Tooltip("At what point to apply the alpha min range 0-1")]
		[Range(0f, 1f)]
		public float CutOff;

		// Token: 0x04000D97 RID: 3479
		[Tooltip("Implement a hard blend based on the Cutoff")]
		public bool HardBlend;

		// Token: 0x04000D98 RID: 3480
		[Tooltip("Flip the masks alpha value")]
		public bool FlipAlphaMask;

		// Token: 0x04000D99 RID: 3481
		[Tooltip("If a different Mask Scaling Rect is given, and this value is true, the area around the mask will not be clipped")]
		public bool DontClipMaskScalingRect;

		// Token: 0x04000D9A RID: 3482
		private Vector2 maskOffset = Vector2.zero;

		// Token: 0x04000D9B RID: 3483
		private Vector2 maskScale = Vector2.one;
	}
}
