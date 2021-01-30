using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200016B RID: 363
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer), typeof(ParticleSystem))]
	[AddComponentMenu("UI/Effects/Extensions/UIParticleSystem")]
	public class UIParticleSystem : MaskableGraphic
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00059604 File Offset: 0x00057804
		public override Texture mainTexture
		{
			get
			{
				return this.currentTexture;
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0005960C File Offset: 0x0005780C
		protected bool Initialize()
		{
			if (this._transform == null)
			{
				this._transform = base.transform;
			}
			if (this.pSystem == null)
			{
				this.pSystem = base.GetComponent<ParticleSystem>();
				if (this.pSystem == null)
				{
					return false;
				}
				this.mainModule = this.pSystem.main;
				if (this.pSystem.main.maxParticles > 14000)
				{
					this.mainModule.maxParticles = 14000;
				}
				this.pRenderer = this.pSystem.GetComponent<ParticleSystemRenderer>();
				if (this.pRenderer != null)
				{
					this.pRenderer.enabled = false;
				}
				Material material = new Material(Shader.Find("UI Extensions/Particles/Additive"));
				if (this.material == null)
				{
					this.material = material;
				}
				this.currentMaterial = this.material;
				if (this.currentMaterial && this.currentMaterial.HasProperty("_MainTex"))
				{
					this.currentTexture = this.currentMaterial.mainTexture;
					if (this.currentTexture == null)
					{
						this.currentTexture = Texture2D.whiteTexture;
					}
				}
				this.material = this.currentMaterial;
				this.mainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;
				this.particles = null;
			}
			if (this.particles == null)
			{
				this.particles = new ParticleSystem.Particle[this.pSystem.main.maxParticles];
			}
			this.imageUV = new Vector4(0f, 0f, 1f, 1f);
			this.textureSheetAnimation = this.pSystem.textureSheetAnimation;
			this.textureSheetAnimationFrames = 0;
			this.textureSheetAnimationFrameSize = Vector2.zero;
			if (this.textureSheetAnimation.enabled)
			{
				this.textureSheetAnimationFrames = this.textureSheetAnimation.numTilesX * this.textureSheetAnimation.numTilesY;
				this.textureSheetAnimationFrameSize = new Vector2(1f / (float)this.textureSheetAnimation.numTilesX, 1f / (float)this.textureSheetAnimation.numTilesY);
			}
			return true;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00059821 File Offset: 0x00057A21
		protected override void Awake()
		{
			base.Awake();
			if (!this.Initialize())
			{
				base.enabled = false;
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00059838 File Offset: 0x00057A38
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			int num = this.pSystem.GetParticles(this.particles);
			for (int i = 0; i < num; i++)
			{
				ParticleSystem.Particle particle = this.particles[i];
				Vector2 vector = (this.mainModule.simulationSpace == ParticleSystemSimulationSpace.Local) ? particle.position : this._transform.InverseTransformPoint(particle.position);
				float num2 = -particle.rotation * 0.017453292f;
				float f = num2 + 1.5707964f;
				Color32 currentColor = particle.GetCurrentColor(this.pSystem);
				float num3 = particle.GetCurrentSize(this.pSystem) * 0.5f;
				if (this.mainModule.scalingMode == ParticleSystemScalingMode.Shape)
				{
					vector /= base.canvas.scaleFactor;
				}
				Vector4 vector2 = this.imageUV;
				if (this.textureSheetAnimation.enabled)
				{
					float num4 = 1f - particle.remainingLifetime / particle.startLifetime;
					if (this.textureSheetAnimation.frameOverTime.curveMin != null)
					{
						num4 = this.textureSheetAnimation.frameOverTime.curveMin.Evaluate(1f - particle.remainingLifetime / particle.startLifetime);
					}
					else if (this.textureSheetAnimation.frameOverTime.curve != null)
					{
						num4 = this.textureSheetAnimation.frameOverTime.curve.Evaluate(1f - particle.remainingLifetime / particle.startLifetime);
					}
					else if (this.textureSheetAnimation.frameOverTime.constant > 0f)
					{
						num4 = this.textureSheetAnimation.frameOverTime.constant - particle.remainingLifetime / particle.startLifetime;
					}
					num4 = Mathf.Repeat(num4 * (float)this.textureSheetAnimation.cycleCount, 1f);
					int num5 = 0;
					ParticleSystemAnimationType animation = this.textureSheetAnimation.animation;
					if (animation != ParticleSystemAnimationType.WholeSheet)
					{
						if (animation == ParticleSystemAnimationType.SingleRow)
						{
							num5 = Mathf.FloorToInt(num4 * (float)this.textureSheetAnimation.numTilesX);
							int rowIndex = this.textureSheetAnimation.rowIndex;
							num5 += rowIndex * this.textureSheetAnimation.numTilesX;
						}
					}
					else
					{
						num5 = Mathf.FloorToInt(num4 * (float)this.textureSheetAnimationFrames);
					}
					num5 %= this.textureSheetAnimationFrames;
					vector2.x = (float)(num5 % this.textureSheetAnimation.numTilesX) * this.textureSheetAnimationFrameSize.x;
					vector2.y = (float)Mathf.FloorToInt((float)(num5 / this.textureSheetAnimation.numTilesX)) * this.textureSheetAnimationFrameSize.y;
					vector2.z = vector2.x + this.textureSheetAnimationFrameSize.x;
					vector2.w = vector2.y + this.textureSheetAnimationFrameSize.y;
				}
				zero.x = vector2.x;
				zero.y = vector2.y;
				this._quad[0] = UIVertex.simpleVert;
				this._quad[0].color = currentColor;
				this._quad[0].uv0 = zero;
				zero.x = vector2.x;
				zero.y = vector2.w;
				this._quad[1] = UIVertex.simpleVert;
				this._quad[1].color = currentColor;
				this._quad[1].uv0 = zero;
				zero.x = vector2.z;
				zero.y = vector2.w;
				this._quad[2] = UIVertex.simpleVert;
				this._quad[2].color = currentColor;
				this._quad[2].uv0 = zero;
				zero.x = vector2.z;
				zero.y = vector2.y;
				this._quad[3] = UIVertex.simpleVert;
				this._quad[3].color = currentColor;
				this._quad[3].uv0 = zero;
				if (num2 == 0f)
				{
					zero2.x = vector.x - num3;
					zero2.y = vector.y - num3;
					zero3.x = vector.x + num3;
					zero3.y = vector.y + num3;
					zero.x = zero2.x;
					zero.y = zero2.y;
					this._quad[0].position = zero;
					zero.x = zero2.x;
					zero.y = zero3.y;
					this._quad[1].position = zero;
					zero.x = zero3.x;
					zero.y = zero3.y;
					this._quad[2].position = zero;
					zero.x = zero3.x;
					zero.y = zero2.y;
					this._quad[3].position = zero;
				}
				else
				{
					Vector2 b = new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)) * num3;
					Vector2 b2 = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * num3;
					this._quad[0].position = vector - b - b2;
					this._quad[1].position = vector - b + b2;
					this._quad[2].position = vector + b + b2;
					this._quad[3].position = vector + b - b2;
				}
				vh.AddUIVertexQuad(this._quad);
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00059E7C File Offset: 0x0005807C
		private void Update()
		{
			if (!this.fixedTime && Application.isPlaying)
			{
				this.pSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
				this.SetAllDirty();
				if ((this.currentMaterial != null && this.currentTexture != this.currentMaterial.mainTexture) || (this.material != null && this.currentMaterial != null && this.material.shader != this.currentMaterial.shader))
				{
					this.pSystem = null;
					this.Initialize();
				}
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00059F24 File Offset: 0x00058124
		private void LateUpdate()
		{
			if (!Application.isPlaying)
			{
				this.SetAllDirty();
			}
			else if (this.fixedTime)
			{
				this.pSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
				this.SetAllDirty();
				if ((this.currentMaterial != null && this.currentTexture != this.currentMaterial.mainTexture) || (this.material != null && this.currentMaterial != null && this.material.shader != this.currentMaterial.shader))
				{
					this.pSystem = null;
					this.Initialize();
				}
			}
			if (this.material == this.currentMaterial)
			{
				return;
			}
			this.pSystem = null;
			this.Initialize();
		}

		// Token: 0x04000D9E RID: 3486
		[Tooltip("Having this enabled run the system in LateUpdate rather than in Update making it faster but less precise (more clunky)")]
		public bool fixedTime = true;

		// Token: 0x04000D9F RID: 3487
		private Transform _transform;

		// Token: 0x04000DA0 RID: 3488
		private ParticleSystem pSystem;

		// Token: 0x04000DA1 RID: 3489
		private ParticleSystem.Particle[] particles;

		// Token: 0x04000DA2 RID: 3490
		private UIVertex[] _quad = new UIVertex[4];

		// Token: 0x04000DA3 RID: 3491
		private Vector4 imageUV = Vector4.zero;

		// Token: 0x04000DA4 RID: 3492
		private ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;

		// Token: 0x04000DA5 RID: 3493
		private int textureSheetAnimationFrames;

		// Token: 0x04000DA6 RID: 3494
		private Vector2 textureSheetAnimationFrameSize;

		// Token: 0x04000DA7 RID: 3495
		private ParticleSystemRenderer pRenderer;

		// Token: 0x04000DA8 RID: 3496
		private Material currentMaterial;

		// Token: 0x04000DA9 RID: 3497
		private Texture currentTexture;

		// Token: 0x04000DAA RID: 3498
		private ParticleSystem.MainModule mainModule;
	}
}
