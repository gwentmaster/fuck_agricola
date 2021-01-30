using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001CB RID: 459
	[AddComponentMenu("UI/Extensions/Selectable Scalar")]
	[RequireComponent(typeof(Button))]
	public class SelectableScaler : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x0006D503 File Offset: 0x0006B703
		public Selectable Target
		{
			get
			{
				if (this.selectable == null)
				{
					this.selectable = base.GetComponent<Selectable>();
				}
				return this.selectable;
			}
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0006D525 File Offset: 0x0006B725
		private void Awake()
		{
			if (this.target == null)
			{
				this.target = base.transform;
			}
			this.initScale = this.target.localScale;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0006D552 File Offset: 0x0006B752
		private void OnEnable()
		{
			this.target.localScale = this.initScale;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0006D565 File Offset: 0x0006B765
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.Target != null && !this.Target.interactable)
			{
				return;
			}
			base.StopCoroutine("ScaleOUT");
			base.StartCoroutine("ScaleIN");
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0006D59A File Offset: 0x0006B79A
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.Target != null && !this.Target.interactable)
			{
				return;
			}
			base.StopCoroutine("ScaleIN");
			base.StartCoroutine("ScaleOUT");
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0006D5CF File Offset: 0x0006B7CF
		private IEnumerator ScaleIN()
		{
			if (this.animCurve.keys.Length != 0)
			{
				this.target.localScale = this.initScale;
				float t = 0f;
				float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
				while (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(t);
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0006D5DE File Offset: 0x0006B7DE
		private IEnumerator ScaleOUT()
		{
			if (this.animCurve.keys.Length != 0)
			{
				float t = 0f;
				float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
				while (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(maxT - t);
					yield return null;
				}
				base.transform.localScale = this.initScale;
			}
			yield break;
		}

		// Token: 0x04001015 RID: 4117
		public AnimationCurve animCurve;

		// Token: 0x04001016 RID: 4118
		[Tooltip("Animation speed multiplier")]
		public float speed = 1f;

		// Token: 0x04001017 RID: 4119
		private Vector3 initScale;

		// Token: 0x04001018 RID: 4120
		public Transform target;

		// Token: 0x04001019 RID: 4121
		private Selectable selectable;
	}
}
