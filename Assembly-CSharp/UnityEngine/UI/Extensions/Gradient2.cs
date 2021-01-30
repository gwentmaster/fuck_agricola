using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000192 RID: 402
	[AddComponentMenu("UI/Effects/Extensions/Gradient2")]
	public class Gradient2 : BaseMeshEffect
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x000621DC File Offset: 0x000603DC
		// (set) Token: 0x06000F83 RID: 3971 RVA: 0x000621E4 File Offset: 0x000603E4
		public Gradient2.Blend BlendMode
		{
			get
			{
				return this._blendMode;
			}
			set
			{
				this._blendMode = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x000621F8 File Offset: 0x000603F8
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x00062200 File Offset: 0x00060400
		public Gradient EffectGradient
		{
			get
			{
				return this._effectGradient;
			}
			set
			{
				this._effectGradient = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00062214 File Offset: 0x00060414
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0006221C File Offset: 0x0006041C
		public Gradient2.Type GradientType
		{
			get
			{
				return this._gradientType;
			}
			set
			{
				this._gradientType = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x00062230 File Offset: 0x00060430
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x00062238 File Offset: 0x00060438
		public float Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0006224C File Offset: 0x0006044C
		public override void ModifyMesh(VertexHelper helper)
		{
			if (!this.IsActive() || helper.currentVertCount == 0)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			helper.GetUIVertexStream(list);
			int count = list.Count;
			switch (this.GradientType)
			{
			case Gradient2.Type.Horizontal:
			{
				float num = list[0].position.x;
				float num2 = list[0].position.x;
				for (int i = count - 1; i >= 1; i--)
				{
					float x = list[i].position.x;
					if (x > num2)
					{
						num2 = x;
					}
					else if (x < num)
					{
						num = x;
					}
				}
				float num3 = 1f / (num2 - num);
				UIVertex uivertex = default(UIVertex);
				for (int j = 0; j < helper.currentVertCount; j++)
				{
					helper.PopulateUIVertex(ref uivertex, j);
					uivertex.color = this.BlendColor(uivertex.color, this.EffectGradient.Evaluate((uivertex.position.x - num) * num3 - this.Offset));
					helper.SetUIVertex(uivertex, j);
				}
				return;
			}
			case Gradient2.Type.Vertical:
			{
				float num4 = list[0].position.y;
				float num5 = list[0].position.y;
				for (int k = count - 1; k >= 1; k--)
				{
					float y = list[k].position.y;
					if (y > num5)
					{
						num5 = y;
					}
					else if (y < num4)
					{
						num4 = y;
					}
				}
				float num6 = 1f / (num5 - num4);
				UIVertex uivertex2 = default(UIVertex);
				for (int l = 0; l < helper.currentVertCount; l++)
				{
					helper.PopulateUIVertex(ref uivertex2, l);
					uivertex2.color = this.BlendColor(uivertex2.color, this.EffectGradient.Evaluate((uivertex2.position.y - num4) * num6 - this.Offset));
					helper.SetUIVertex(uivertex2, l);
				}
				return;
			}
			case Gradient2.Type.Radial:
			{
				float num7 = list[0].position.x;
				float num8 = list[0].position.x;
				float num9 = list[0].position.y;
				float num10 = list[0].position.y;
				for (int m = count - 1; m >= 1; m--)
				{
					float x2 = list[m].position.x;
					if (x2 > num8)
					{
						num8 = x2;
					}
					else if (x2 < num7)
					{
						num7 = x2;
					}
					float y2 = list[m].position.y;
					if (y2 > num10)
					{
						num10 = y2;
					}
					else if (y2 < num9)
					{
						num9 = y2;
					}
				}
				float num11 = 1f / (num8 - num7);
				float num12 = 1f / (num10 - num9);
				helper.Clear();
				float num13 = (num8 + num7) / 2f;
				float num14 = (num9 + num10) / 2f;
				float num15 = (num8 - num7) / 2f;
				float num16 = (num10 - num9) / 2f;
				UIVertex v = default(UIVertex);
				v.position = Vector3.right * num13 + Vector3.up * num14 + Vector3.forward * list[0].position.z;
				v.normal = list[0].normal;
				v.color = Color.white;
				int num17 = 64;
				for (int n = 0; n < num17; n++)
				{
					UIVertex v2 = default(UIVertex);
					float num18 = (float)n * 360f / (float)num17;
					float d = Mathf.Cos(0.017453292f * num18) * num15;
					float d2 = Mathf.Sin(0.017453292f * num18) * num16;
					v2.position = Vector3.right * d + Vector3.up * d2 + Vector3.forward * list[0].position.z;
					v2.normal = list[0].normal;
					v2.color = Color.white;
					helper.AddVert(v2);
				}
				helper.AddVert(v);
				for (int num19 = 1; num19 < num17; num19++)
				{
					helper.AddTriangle(num19 - 1, num19, num17);
				}
				helper.AddTriangle(0, num17 - 1, num17);
				UIVertex uivertex3 = default(UIVertex);
				for (int num20 = 0; num20 < helper.currentVertCount; num20++)
				{
					helper.PopulateUIVertex(ref uivertex3, num20);
					uivertex3.color = this.BlendColor(uivertex3.color, this.EffectGradient.Evaluate(Mathf.Sqrt(Mathf.Pow(Mathf.Abs(uivertex3.position.x - num13) * num11, 2f) + Mathf.Pow(Mathf.Abs(uivertex3.position.y - num14) * num12, 2f)) * 2f - this.Offset));
					helper.SetUIVertex(uivertex3, num20);
				}
				return;
			}
			case Gradient2.Type.Diamond:
			{
				float num21 = list[0].position.y;
				float num22 = list[0].position.y;
				for (int num23 = count - 1; num23 >= 1; num23--)
				{
					float y3 = list[num23].position.y;
					if (y3 > num22)
					{
						num22 = y3;
					}
					else if (y3 < num21)
					{
						num21 = y3;
					}
				}
				float num24 = 1f / (num22 - num21);
				helper.Clear();
				for (int num25 = 0; num25 < count; num25++)
				{
					helper.AddVert(list[num25]);
				}
				float d3 = (num21 + num22) / 2f;
				UIVertex uivertex4 = new UIVertex
				{
					position = (Vector3.right + Vector3.up) * d3 + Vector3.forward * list[0].position.z,
					normal = list[0].normal,
					color = Color.white
				};
				helper.AddVert(uivertex4);
				for (int num26 = 1; num26 < count; num26++)
				{
					helper.AddTriangle(num26 - 1, num26, count);
				}
				helper.AddTriangle(0, count - 1, count);
				UIVertex uivertex5 = default(UIVertex);
				for (int num27 = 0; num27 < helper.currentVertCount; num27++)
				{
					helper.PopulateUIVertex(ref uivertex5, num27);
					uivertex5.color = this.BlendColor(uivertex5.color, this.EffectGradient.Evaluate(Vector3.Distance(uivertex5.position, uivertex4.position) * num24 - this.Offset));
					helper.SetUIVertex(uivertex5, num27);
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00062958 File Offset: 0x00060B58
		private Color BlendColor(Color colorA, Color colorB)
		{
			Gradient2.Blend blendMode = this.BlendMode;
			if (blendMode == Gradient2.Blend.Add)
			{
				return colorA + colorB;
			}
			if (blendMode != Gradient2.Blend.Multiply)
			{
				return colorB;
			}
			return colorA * colorB;
		}

		// Token: 0x04000ED0 RID: 3792
		[SerializeField]
		private Gradient2.Type _gradientType;

		// Token: 0x04000ED1 RID: 3793
		[SerializeField]
		private Gradient2.Blend _blendMode = Gradient2.Blend.Multiply;

		// Token: 0x04000ED2 RID: 3794
		[SerializeField]
		[Range(-1f, 1f)]
		private float _offset;

		// Token: 0x04000ED3 RID: 3795
		[SerializeField]
		private Gradient _effectGradient = new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.black, 0f),
				new GradientColorKey(Color.white, 1f)
			}
		};

		// Token: 0x02000850 RID: 2128
		public enum Type
		{
			// Token: 0x04002EBB RID: 11963
			Horizontal,
			// Token: 0x04002EBC RID: 11964
			Vertical,
			// Token: 0x04002EBD RID: 11965
			Radial,
			// Token: 0x04002EBE RID: 11966
			Diamond
		}

		// Token: 0x02000851 RID: 2129
		public enum Blend
		{
			// Token: 0x04002EC0 RID: 11968
			Override,
			// Token: 0x04002EC1 RID: 11969
			Add,
			// Token: 0x04002EC2 RID: 11970
			Multiply
		}
	}
}
