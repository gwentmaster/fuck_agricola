using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000194 RID: 404
	public class ShineEffect : MaskableGraphic
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00062E49 File Offset: 0x00061049
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x00062E51 File Offset: 0x00061051
		public float Yoffset
		{
			get
			{
				return this.yoffset;
			}
			set
			{
				this.SetVerticesDirty();
				this.yoffset = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x00062E60 File Offset: 0x00061060
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x00062E68 File Offset: 0x00061068
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.SetAllDirty();
				this.width = value;
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00062E78 File Offset: 0x00061078
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			float num = (vector.w - vector.y) * 2f;
			Color32 color = this.color;
			vh.Clear();
			color.a = 0;
			vh.AddVert(new Vector3(vector.x - 50f, this.width * vector.y + this.yoffset * num), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * vector.y + this.yoffset * num), color, new Vector2(1f, 0f));
			color.a = (byte)(this.color.a * 255f);
			vh.AddVert(new Vector3(vector.x - 50f, this.width * (vector.y / 4f) + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * (vector.y / 4f) + this.yoffset * num), color, new Vector2(1f, 1f));
			color.a = (byte)(this.color.a * 255f);
			vh.AddVert(new Vector3(vector.x - 50f, this.width * (vector.w / 4f) + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * (vector.w / 4f) + this.yoffset * num), color, new Vector2(1f, 1f));
			color.a = (byte)(this.color.a * 255f);
			color.a = 0;
			vh.AddVert(new Vector3(vector.x - 50f, this.width * vector.w + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * vector.w + this.yoffset * num), color, new Vector2(1f, 1f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 1);
			vh.AddTriangle(2, 3, 4);
			vh.AddTriangle(4, 5, 3);
			vh.AddTriangle(4, 5, 6);
			vh.AddTriangle(6, 7, 5);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00063178 File Offset: 0x00061378
		public void Triangulate(VertexHelper vh)
		{
			int num = vh.currentVertCount - 2;
			Debug.Log(num);
			for (int i = 0; i <= num / 2 + 1; i += 2)
			{
				vh.AddTriangle(i, i + 1, i + 2);
				vh.AddTriangle(i + 2, i + 3, i + 1);
			}
		}

		// Token: 0x04000ED9 RID: 3801
		[SerializeField]
		private float yoffset = -1f;

		// Token: 0x04000EDA RID: 3802
		[SerializeField]
		private float width = 1f;
	}
}
