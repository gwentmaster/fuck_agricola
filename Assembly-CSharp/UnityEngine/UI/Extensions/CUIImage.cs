using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200018D RID: 397
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Image")]
	public class CUIImage : CUIGraphic
	{
		// Token: 0x06000F6A RID: 3946 RVA: 0x000619DD File Offset: 0x0005FBDD
		public static int ImageTypeCornerRefVertexIdx(Image.Type _type)
		{
			if (_type == Image.Type.Sliced)
			{
				return CUIImage.SlicedImageCornerRefVertexIdx;
			}
			return CUIImage.FilledImageCornerRefVertexIdx;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x000619EE File Offset: 0x0005FBEE
		public Vector2 OriCornerPosRatio
		{
			get
			{
				return this.oriCornerPosRatio;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x000619F6 File Offset: 0x0005FBF6
		public Image UIImage
		{
			get
			{
				return (Image)this.uiGraphic;
			}
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x00061A03 File Offset: 0x0005FC03
		public override void ReportSet()
		{
			if (this.uiGraphic == null)
			{
				this.uiGraphic = base.GetComponent<Image>();
			}
			base.ReportSet();
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00061A28 File Offset: 0x0005FC28
		protected override void modifyVertices(List<UIVertex> _verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.UIImage.type == Image.Type.Filled)
			{
				Debug.LogWarning("Might not work well Radial Filled at the moment!");
			}
			else if (this.UIImage.type == Image.Type.Sliced || this.UIImage.type == Image.Type.Tiled)
			{
				if (this.cornerPosRatio == Vector2.one * -1f)
				{
					this.cornerPosRatio = _verts[CUIImage.ImageTypeCornerRefVertexIdx(this.UIImage.type)].position;
					this.cornerPosRatio.x = (this.cornerPosRatio.x + this.rectTrans.pivot.x * this.rectTrans.rect.width) / this.rectTrans.rect.width;
					this.cornerPosRatio.y = (this.cornerPosRatio.y + this.rectTrans.pivot.y * this.rectTrans.rect.height) / this.rectTrans.rect.height;
					this.oriCornerPosRatio = this.cornerPosRatio;
				}
				if (this.cornerPosRatio.x < 0f)
				{
					this.cornerPosRatio.x = 0f;
				}
				if (this.cornerPosRatio.x >= 0.5f)
				{
					this.cornerPosRatio.x = 0.5f;
				}
				if (this.cornerPosRatio.y < 0f)
				{
					this.cornerPosRatio.y = 0f;
				}
				if (this.cornerPosRatio.y >= 0.5f)
				{
					this.cornerPosRatio.y = 0.5f;
				}
				for (int i = 0; i < _verts.Count; i++)
				{
					UIVertex uivertex = _verts[i];
					float num = (uivertex.position.x + this.rectTrans.rect.width * this.rectTrans.pivot.x) / this.rectTrans.rect.width;
					float num2 = (uivertex.position.y + this.rectTrans.rect.height * this.rectTrans.pivot.y) / this.rectTrans.rect.height;
					if (num < this.oriCornerPosRatio.x)
					{
						num = Mathf.Lerp(0f, this.cornerPosRatio.x, num / this.oriCornerPosRatio.x);
					}
					else if (num > 1f - this.oriCornerPosRatio.x)
					{
						num = Mathf.Lerp(1f - this.cornerPosRatio.x, 1f, (num - (1f - this.oriCornerPosRatio.x)) / this.oriCornerPosRatio.x);
					}
					else
					{
						num = Mathf.Lerp(this.cornerPosRatio.x, 1f - this.cornerPosRatio.x, (num - this.oriCornerPosRatio.x) / (1f - this.oriCornerPosRatio.x * 2f));
					}
					if (num2 < this.oriCornerPosRatio.y)
					{
						num2 = Mathf.Lerp(0f, this.cornerPosRatio.y, num2 / this.oriCornerPosRatio.y);
					}
					else if (num2 > 1f - this.oriCornerPosRatio.y)
					{
						num2 = Mathf.Lerp(1f - this.cornerPosRatio.y, 1f, (num2 - (1f - this.oriCornerPosRatio.y)) / this.oriCornerPosRatio.y);
					}
					else
					{
						num2 = Mathf.Lerp(this.cornerPosRatio.y, 1f - this.cornerPosRatio.y, (num2 - this.oriCornerPosRatio.y) / (1f - this.oriCornerPosRatio.y * 2f));
					}
					uivertex.position.x = num * this.rectTrans.rect.width - this.rectTrans.rect.width * this.rectTrans.pivot.x;
					uivertex.position.y = num2 * this.rectTrans.rect.height - this.rectTrans.rect.height * this.rectTrans.pivot.y;
					_verts[i] = uivertex;
				}
			}
			base.modifyVertices(_verts);
		}

		// Token: 0x04000EC6 RID: 3782
		public static int SlicedImageCornerRefVertexIdx = 2;

		// Token: 0x04000EC7 RID: 3783
		public static int FilledImageCornerRefVertexIdx = 0;

		// Token: 0x04000EC8 RID: 3784
		[Tooltip("For changing the size of the corner for tiled or sliced Image")]
		[HideInInspector]
		[SerializeField]
		public Vector2 cornerPosRatio = Vector2.one * -1f;

		// Token: 0x04000EC9 RID: 3785
		[HideInInspector]
		[SerializeField]
		protected Vector2 oriCornerPosRatio = Vector2.one * -1f;
	}
}
