using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001B4 RID: 436
	public class UIPrimitiveBase : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x0006A042 File Offset: 0x00068242
		// (set) Token: 0x060010DD RID: 4317 RVA: 0x0006A04A File Offset: 0x0006824A
		public Sprite sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
				{
					this.GeneratedUVs();
				}
				this.SetAllDirty();
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0006A066 File Offset: 0x00068266
		// (set) Token: 0x060010DF RID: 4319 RVA: 0x0006A06E File Offset: 0x0006826E
		public Sprite overrideSprite
		{
			get
			{
				return this.activeSprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
				{
					this.GeneratedUVs();
				}
				this.SetAllDirty();
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0006A08A File Offset: 0x0006828A
		protected Sprite activeSprite
		{
			get
			{
				if (!(this.m_OverrideSprite != null))
				{
					return this.sprite;
				}
				return this.m_OverrideSprite;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0006A0A7 File Offset: 0x000682A7
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x0006A0AF File Offset: 0x000682AF
		public float eventAlphaThreshold
		{
			get
			{
				return this.m_EventAlphaThreshold;
			}
			set
			{
				this.m_EventAlphaThreshold = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x0006A0B8 File Offset: 0x000682B8
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x0006A0C0 File Offset: 0x000682C0
		public ResolutionMode ImproveResolution
		{
			get
			{
				return this.m_improveResolution;
			}
			set
			{
				this.m_improveResolution = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x0006A0CF File Offset: 0x000682CF
		// (set) Token: 0x060010E6 RID: 4326 RVA: 0x0006A0D7 File Offset: 0x000682D7
		public float Resoloution
		{
			get
			{
				return this.m_Resolution;
			}
			set
			{
				this.m_Resolution = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x0006A0E6 File Offset: 0x000682E6
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x0006A0EE File Offset: 0x000682EE
		public bool UseNativeSize
		{
			get
			{
				return this.m_useNativeSize;
			}
			set
			{
				this.m_useNativeSize = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0006A0FD File Offset: 0x000682FD
		protected UIPrimitiveBase()
		{
			base.useLegacyMeshGeneration = false;
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x0006A117 File Offset: 0x00068317
		public static Material defaultETC1GraphicMaterial
		{
			get
			{
				if (UIPrimitiveBase.s_ETC1DefaultUI == null)
				{
					UIPrimitiveBase.s_ETC1DefaultUI = Canvas.GetETC1SupportedCanvasMaterial();
				}
				return UIPrimitiveBase.s_ETC1DefaultUI;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x0006A138 File Offset: 0x00068338
		public override Texture mainTexture
		{
			get
			{
				if (!(this.activeSprite == null))
				{
					return this.activeSprite.texture;
				}
				if (this.material != null && this.material.mainTexture != null)
				{
					return this.material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x0006A194 File Offset: 0x00068394
		public bool hasBorder
		{
			get
			{
				return this.activeSprite != null && this.activeSprite.border.sqrMagnitude > 0f;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x0006A1CC File Offset: 0x000683CC
		public float pixelsPerUnit
		{
			get
			{
				float num = 100f;
				if (this.activeSprite)
				{
					num = this.activeSprite.pixelsPerUnit;
				}
				float num2 = 100f;
				if (base.canvas)
				{
					num2 = base.canvas.referencePixelsPerUnit;
				}
				return num / num2;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0006A21C File Offset: 0x0006841C
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x0006A26A File Offset: 0x0006846A
		public override Material material
		{
			get
			{
				if (this.m_Material != null)
				{
					return this.m_Material;
				}
				if (this.activeSprite && this.activeSprite.associatedAlphaSplitTexture != null)
				{
					return UIPrimitiveBase.defaultETC1GraphicMaterial;
				}
				return this.defaultMaterial;
			}
			set
			{
				base.material = value;
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0006A274 File Offset: 0x00068474
		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
		{
			UIVertex[] array = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				UIVertex simpleVert = UIVertex.simpleVert;
				simpleVert.color = this.color;
				simpleVert.position = vertices[i];
				simpleVert.uv0 = uvs[i];
				array[i] = simpleVert;
			}
			return array;
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0006A2D8 File Offset: 0x000684D8
		protected Vector2[] IncreaseResolution(Vector2[] input)
		{
			List<Vector2> list = new List<Vector2>();
			ResolutionMode improveResolution = this.ImproveResolution;
			if (improveResolution != ResolutionMode.PerSegment)
			{
				if (improveResolution == ResolutionMode.PerLine)
				{
					float num = 0f;
					for (int i = 0; i < input.Length - 1; i++)
					{
						num += Vector2.Distance(input[i], input[i + 1]);
					}
					this.ResolutionToNativeSize(num);
					float num2 = num / this.m_Resolution;
					int num3 = 0;
					for (int j = 0; j < input.Length - 1; j++)
					{
						Vector2 vector = input[j];
						list.Add(vector);
						Vector2 vector2 = input[j + 1];
						float num4 = Vector2.Distance(vector, vector2) / num2;
						float num5 = 1f / num4;
						int num6 = 0;
						while ((float)num6 < num4)
						{
							list.Add(Vector2.Lerp(vector, vector2, (float)num6 * num5));
							num3++;
							num6++;
						}
						list.Add(vector2);
					}
				}
			}
			else
			{
				for (int k = 0; k < input.Length - 1; k++)
				{
					Vector2 vector3 = input[k];
					list.Add(vector3);
					Vector2 vector4 = input[k + 1];
					this.ResolutionToNativeSize(Vector2.Distance(vector3, vector4));
					float num2 = 1f / this.m_Resolution;
					for (float num7 = 1f; num7 < this.m_Resolution; num7 += 1f)
					{
						list.Add(Vector2.Lerp(vector3, vector4, num2 * num7));
					}
					list.Add(vector4);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00003022 File Offset: 0x00001222
		protected virtual void GeneratedUVs()
		{
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00003022 File Offset: 0x00001222
		protected virtual void ResolutionToNativeSize(float distance)
		{
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0006A45F File Offset: 0x0006865F
		public virtual float minWidth
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0006A468 File Offset: 0x00068668
		public virtual float preferredWidth
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				return this.overrideSprite.rect.size.x / this.pixelsPerUnit;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0006A4A8 File Offset: 0x000686A8
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0006A45F File Offset: 0x0006865F
		public virtual float minHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0006A4B0 File Offset: 0x000686B0
		public virtual float preferredHeight
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				return this.overrideSprite.rect.size.y / this.pixelsPerUnit;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x0006A4A8 File Offset: 0x000686A8
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0006A4F0 File Offset: 0x000686F0
		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (this.m_EventAlphaThreshold >= 1f)
			{
				return true;
			}
			Sprite overrideSprite = this.overrideSprite;
			if (overrideSprite == null)
			{
				return true;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out vector);
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			vector.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
			vector.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
			vector = this.MapCoordinate(vector, pixelAdjustedRect);
			Rect textureRect = overrideSprite.textureRect;
			Vector2 vector2 = new Vector2(vector.x / textureRect.width, vector.y / textureRect.height);
			float u = Mathf.Lerp(textureRect.x, textureRect.xMax, vector2.x) / (float)overrideSprite.texture.width;
			float v = Mathf.Lerp(textureRect.y, textureRect.yMax, vector2.y) / (float)overrideSprite.texture.height;
			bool result;
			try
			{
				result = (overrideSprite.texture.GetPixelBilinear(u, v).a >= this.m_EventAlphaThreshold);
			}
			catch (UnityException ex)
			{
				Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
				result = true;
			}
			return result;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0006A658 File Offset: 0x00068858
		private Vector2 MapCoordinate(Vector2 local, Rect rect)
		{
			Rect rect2 = this.sprite.rect;
			return new Vector2(local.x * rect.width, local.y * rect.height);
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0006A688 File Offset: 0x00068888
		private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
		{
			for (int i = 0; i <= 1; i++)
			{
				float num = border[i] + border[i + 2];
				if (rect.size[i] < num && num != 0f)
				{
					float num2 = rect.size[i] / num;
					ref Vector4 ptr = ref border;
					int index = i;
					ptr[index] *= num2;
					ptr = ref border;
					index = i + 2;
					ptr[index] *= num2;
				}
			}
			return border;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0006A71F File Offset: 0x0006891F
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetAllDirty();
		}

		// Token: 0x04000FB0 RID: 4016
		protected static Material s_ETC1DefaultUI;

		// Token: 0x04000FB1 RID: 4017
		[SerializeField]
		private Sprite m_Sprite;

		// Token: 0x04000FB2 RID: 4018
		[NonSerialized]
		private Sprite m_OverrideSprite;

		// Token: 0x04000FB3 RID: 4019
		internal float m_EventAlphaThreshold = 1f;

		// Token: 0x04000FB4 RID: 4020
		[SerializeField]
		private ResolutionMode m_improveResolution;

		// Token: 0x04000FB5 RID: 4021
		[SerializeField]
		protected float m_Resolution;

		// Token: 0x04000FB6 RID: 4022
		[SerializeField]
		private bool m_useNativeSize;
	}
}
