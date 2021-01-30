using System;
using UnityEngine.Events;
using UnityEngine.UI.Extensions.Tweens;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200016D RID: 365
	[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Element")]
	public class AccordionElement : Toggle
	{
		// Token: 0x06000DFD RID: 3581 RVA: 0x0005A051 File Offset: 0x00058251
		protected AccordionElement()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0005A084 File Offset: 0x00058284
		protected override void Awake()
		{
			base.Awake();
			base.transition = Selectable.Transition.None;
			this.toggleTransition = Toggle.ToggleTransition.None;
			this.m_Accordion = base.gameObject.GetComponentInParent<Accordion>();
			this.m_RectTransform = (base.transform as RectTransform);
			this.m_LayoutElement = base.gameObject.GetComponent<LayoutElement>();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0005A0F0 File Offset: 0x000582F0
		public void OnValueChanged(bool state)
		{
			if (this.m_LayoutElement == null)
			{
				return;
			}
			Accordion.Transition transition = (this.m_Accordion != null) ? this.m_Accordion.transition : Accordion.Transition.Instant;
			if (transition != Accordion.Transition.Instant)
			{
				if (transition == Accordion.Transition.Tween)
				{
					if (state)
					{
						this.StartTween(this.m_MinHeight, this.GetExpandedHeight());
						return;
					}
					this.StartTween(this.m_RectTransform.rect.height, this.m_MinHeight);
				}
				return;
			}
			if (state)
			{
				this.m_LayoutElement.preferredHeight = -1f;
				return;
			}
			this.m_LayoutElement.preferredHeight = this.m_MinHeight;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0005A18C File Offset: 0x0005838C
		protected float GetExpandedHeight()
		{
			if (this.m_LayoutElement == null)
			{
				return this.m_MinHeight;
			}
			float preferredHeight = this.m_LayoutElement.preferredHeight;
			this.m_LayoutElement.preferredHeight = -1f;
			float preferredHeight2 = LayoutUtility.GetPreferredHeight(this.m_RectTransform);
			this.m_LayoutElement.preferredHeight = preferredHeight;
			return preferredHeight2;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0005A1E4 File Offset: 0x000583E4
		protected void StartTween(float startFloat, float targetFloat)
		{
			float duration = (this.m_Accordion != null) ? this.m_Accordion.transitionDuration : 0.3f;
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = startFloat,
				targetFloat = targetFloat
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetHeight));
			info.ignoreTimeScale = true;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0005A25B File Offset: 0x0005845B
		protected void SetHeight(float height)
		{
			if (this.m_LayoutElement == null)
			{
				return;
			}
			this.m_LayoutElement.preferredHeight = height;
		}

		// Token: 0x04000DAD RID: 3501
		[SerializeField]
		private float m_MinHeight = 18f;

		// Token: 0x04000DAE RID: 3502
		private Accordion m_Accordion;

		// Token: 0x04000DAF RID: 3503
		private RectTransform m_RectTransform;

		// Token: 0x04000DB0 RID: 3504
		private LayoutElement m_LayoutElement;

		// Token: 0x04000DB1 RID: 3505
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
