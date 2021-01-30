using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000190 RID: 400
	[RequireComponent(typeof(Text), typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/Extensions/Curved Text")]
	public class CurvedText : BaseMeshEffect
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x00061F62 File Offset: 0x00060162
		// (set) Token: 0x06000F76 RID: 3958 RVA: 0x00061F6A File Offset: 0x0006016A
		public AnimationCurve CurveForText
		{
			get
			{
				return this._curveForText;
			}
			set
			{
				this._curveForText = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x00061F7E File Offset: 0x0006017E
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x00061F86 File Offset: 0x00060186
		public float CurveMultiplier
		{
			get
			{
				return this._curveMultiplier;
			}
			set
			{
				this._curveMultiplier = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00061F9A File Offset: 0x0006019A
		protected override void Awake()
		{
			base.Awake();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00061FB4 File Offset: 0x000601B4
		protected override void OnEnable()
		{
			base.OnEnable();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00061FD0 File Offset: 0x000601D0
		public override void ModifyMesh(VertexHelper vh)
		{
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				UIVertex uivertex = default(UIVertex);
				vh.PopulateUIVertex(ref uivertex, i);
				uivertex.position.y = uivertex.position.y + this._curveForText.Evaluate(this.rectTrans.rect.width * this.rectTrans.pivot.x + uivertex.position.x) * this._curveMultiplier;
				vh.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0006206C File Offset: 0x0006026C
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.rectTrans)
			{
				Keyframe key = this._curveForText[this._curveForText.length - 1];
				key.time = this.rectTrans.rect.width;
				this._curveForText.MoveKey(this._curveForText.length - 1, key);
			}
		}

		// Token: 0x04000ECB RID: 3787
		[SerializeField]
		private AnimationCurve _curveForText = AnimationCurve.Linear(0f, 0f, 1f, 10f);

		// Token: 0x04000ECC RID: 3788
		[SerializeField]
		private float _curveMultiplier = 1f;

		// Token: 0x04000ECD RID: 3789
		private RectTransform rectTrans;
	}
}
