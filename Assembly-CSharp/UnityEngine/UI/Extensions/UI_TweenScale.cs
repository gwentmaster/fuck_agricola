using System;
using System.Collections;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D5 RID: 469
	[AddComponentMenu("UI/Extensions/UI Tween Scale")]
	public class UI_TweenScale : MonoBehaviour
	{
		// Token: 0x060011E1 RID: 4577 RVA: 0x0006EBF9 File Offset: 0x0006CDF9
		private void Awake()
		{
			this.myTransform = base.GetComponent<Transform>();
			this.initScale = this.myTransform.localScale;
			if (this.playAtAwake)
			{
				this.Play();
			}
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0006EC26 File Offset: 0x0006CE26
		public void Play()
		{
			base.StartCoroutine("Tween");
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0006EC34 File Offset: 0x0006CE34
		private IEnumerator Tween()
		{
			this.myTransform.localScale = this.initScale;
			float t = 0f;
			float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
			while (t < maxT || this.isLoop)
			{
				t += this.speed * Time.deltaTime;
				if (!this.isUniform)
				{
					this.newScale.x = 1f * this.animCurve.Evaluate(t);
					this.newScale.y = 1f * this.animCurveY.Evaluate(t);
					this.myTransform.localScale = this.newScale;
				}
				else
				{
					this.myTransform.localScale = Vector3.one * this.animCurve.Evaluate(t);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0006EC43 File Offset: 0x0006CE43
		public void ResetTween()
		{
			base.StopCoroutine("Tween");
			this.myTransform.localScale = this.initScale;
		}

		// Token: 0x0400105B RID: 4187
		public AnimationCurve animCurve;

		// Token: 0x0400105C RID: 4188
		[Tooltip("Animation speed multiplier")]
		public float speed = 1f;

		// Token: 0x0400105D RID: 4189
		[Tooltip("If true animation will loop, for best effect set animation curve to loop on start and end point")]
		public bool isLoop;

		// Token: 0x0400105E RID: 4190
		[Tooltip("If true animation will start automatically, otherwise you need to call Play() method to start the animation")]
		public bool playAtAwake;

		// Token: 0x0400105F RID: 4191
		[Space(10f)]
		[Header("Non uniform scale")]
		[Tooltip("If true component will scale by the same amount in X and Y axis, otherwise use animCurve for X scale and animCurveY for Y scale")]
		public bool isUniform = true;

		// Token: 0x04001060 RID: 4192
		public AnimationCurve animCurveY;

		// Token: 0x04001061 RID: 4193
		private Vector3 initScale;

		// Token: 0x04001062 RID: 4194
		private Transform myTransform;

		// Token: 0x04001063 RID: 4195
		private Vector3 newScale = Vector3.one;
	}
}
