using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000201 RID: 513
	public class AnimateEffects : MonoBehaviour
	{
		// Token: 0x060012B0 RID: 4784 RVA: 0x000712A1 File Offset: 0x0006F4A1
		private void Start()
		{
			this.cylinderTextRT = this.cylinderText.GetComponent<Transform>();
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000712B4 File Offset: 0x0006F4B4
		private void Update()
		{
			this.letterSpacing.spacing += this.letterSpacingModifier;
			if (this.letterSpacing.spacing > this.letterSpacingMax || this.letterSpacing.spacing < this.letterSpacingMin)
			{
				this.letterSpacingModifier = -this.letterSpacingModifier;
			}
			this.curvedText.CurveMultiplier += this.curvedTextModifier;
			if (this.curvedText.CurveMultiplier > this.curvedTextMax || this.curvedText.CurveMultiplier < this.curvedTextMin)
			{
				this.curvedTextModifier = -this.curvedTextModifier;
			}
			this.gradient2.Offset += this.gradient2Modifier;
			if (this.gradient2.Offset > this.gradient2Max || this.gradient2.Offset < this.gradient2Min)
			{
				this.gradient2Modifier = -this.gradient2Modifier;
			}
			this.cylinderTextRT.Rotate(this.cylinderRotation);
			this.SAUIM.CutOff += this.SAUIMModifier;
			if (this.SAUIM.CutOff > this.SAUIMMax || this.SAUIM.CutOff < this.SAUIMMin)
			{
				this.SAUIMModifier = -this.SAUIMModifier;
			}
		}

		// Token: 0x040010D5 RID: 4309
		public LetterSpacing letterSpacing;

		// Token: 0x040010D6 RID: 4310
		private float letterSpacingMax = 10f;

		// Token: 0x040010D7 RID: 4311
		private float letterSpacingMin = -10f;

		// Token: 0x040010D8 RID: 4312
		private float letterSpacingModifier = 0.1f;

		// Token: 0x040010D9 RID: 4313
		public CurvedText curvedText;

		// Token: 0x040010DA RID: 4314
		private float curvedTextMax = 0.05f;

		// Token: 0x040010DB RID: 4315
		private float curvedTextMin = -0.05f;

		// Token: 0x040010DC RID: 4316
		private float curvedTextModifier = 0.001f;

		// Token: 0x040010DD RID: 4317
		public Gradient2 gradient2;

		// Token: 0x040010DE RID: 4318
		private float gradient2Max = 1f;

		// Token: 0x040010DF RID: 4319
		private float gradient2Min = -1f;

		// Token: 0x040010E0 RID: 4320
		private float gradient2Modifier = 0.01f;

		// Token: 0x040010E1 RID: 4321
		public CylinderText cylinderText;

		// Token: 0x040010E2 RID: 4322
		private Transform cylinderTextRT;

		// Token: 0x040010E3 RID: 4323
		private Vector3 cylinderRotation = new Vector3(0f, 1f, 0f);

		// Token: 0x040010E4 RID: 4324
		public SoftMaskScript SAUIM;

		// Token: 0x040010E5 RID: 4325
		private float SAUIMMax = 1f;

		// Token: 0x040010E6 RID: 4326
		private float SAUIMMin;

		// Token: 0x040010E7 RID: 4327
		private float SAUIMModifier = 0.01f;
	}
}
