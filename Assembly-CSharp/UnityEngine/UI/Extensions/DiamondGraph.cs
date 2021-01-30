using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001AC RID: 428
	[AddComponentMenu("UI/Extensions/Primitives/Diamond Graph")]
	public class DiamondGraph : UIPrimitiveBase
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x00067B5A File Offset: 0x00065D5A
		// (set) Token: 0x0600108B RID: 4235 RVA: 0x00067B62 File Offset: 0x00065D62
		public float A
		{
			get
			{
				return this.m_a;
			}
			set
			{
				this.m_a = value;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00067B6B File Offset: 0x00065D6B
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00067B73 File Offset: 0x00065D73
		public float B
		{
			get
			{
				return this.m_b;
			}
			set
			{
				this.m_b = value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x00067B7C File Offset: 0x00065D7C
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x00067B84 File Offset: 0x00065D84
		public float C
		{
			get
			{
				return this.m_c;
			}
			set
			{
				this.m_c = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x00067B8D File Offset: 0x00065D8D
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x00067B95 File Offset: 0x00065D95
		public float D
		{
			get
			{
				return this.m_d;
			}
			set
			{
				this.m_d = value;
			}
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00067BA0 File Offset: 0x00065DA0
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			float num = base.rectTransform.rect.width / 2f;
			this.m_a = Math.Min(1f, Math.Max(0f, this.m_a));
			this.m_b = Math.Min(1f, Math.Max(0f, this.m_b));
			this.m_c = Math.Min(1f, Math.Max(0f, this.m_c));
			this.m_d = Math.Min(1f, Math.Max(0f, this.m_d));
			Color32 color = this.color;
			vh.AddVert(new Vector3(-num * this.m_a, 0f), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(0f, num * this.m_b), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(num * this.m_c, 0f), color, new Vector2(1f, 1f));
			vh.AddVert(new Vector3(0f, -num * this.m_d), color, new Vector2(1f, 0f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		// Token: 0x04000F74 RID: 3956
		[SerializeField]
		private float m_a = 1f;

		// Token: 0x04000F75 RID: 3957
		[SerializeField]
		private float m_b = 1f;

		// Token: 0x04000F76 RID: 3958
		[SerializeField]
		private float m_c = 1f;

		// Token: 0x04000F77 RID: 3959
		[SerializeField]
		private float m_d = 1f;
	}
}
