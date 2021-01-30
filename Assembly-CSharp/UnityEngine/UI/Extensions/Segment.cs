using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000182 RID: 386
	[RequireComponent(typeof(Selectable))]
	public class Segment : UIBehaviour, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0005E31E File Offset: 0x0005C51E
		internal bool leftmost
		{
			get
			{
				return this.index == 0;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0005E329 File Offset: 0x0005C529
		internal bool rightmost
		{
			get
			{
				return this.index == this.segmentControl.segments.Length - 1;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x0005E342 File Offset: 0x0005C542
		// (set) Token: 0x06000ED9 RID: 3801 RVA: 0x0005E35A File Offset: 0x0005C55A
		public bool selected
		{
			get
			{
				return this.segmentControl.selectedSegment == this.button;
			}
			set
			{
				this.SetSelected(value);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x0005E363 File Offset: 0x0005C563
		internal SegmentedControl segmentControl
		{
			get
			{
				return base.GetComponentInParent<SegmentedControl>();
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0005E36B File Offset: 0x0005C56B
		internal Selectable button
		{
			get
			{
				return base.GetComponent<Selectable>();
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0005E373 File Offset: 0x0005C573
		protected Segment()
		{
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0005E37B File Offset: 0x0005C57B
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.selected = true;
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0005E38D File Offset: 0x0005C58D
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0005E38D File Offset: 0x0005C58D
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0005E38D File Offset: 0x0005C58D
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0005E38D File Offset: 0x0005C58D
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0005E38D File Offset: 0x0005C58D
		public virtual void OnSelect(BaseEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0005E38D File Offset: 0x0005C58D
		public virtual void OnDeselect(BaseEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0005E395 File Offset: 0x0005C595
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.selected = true;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0005E3A0 File Offset: 0x0005C5A0
		private void SetSelected(bool value)
		{
			if (!value || !this.button.IsActive() || !this.button.IsInteractable())
			{
				if (this.segmentControl.selectedSegment == this.button)
				{
					this.Deselect();
				}
				return;
			}
			if (!(this.segmentControl.selectedSegment == this.button))
			{
				if (this.segmentControl.selectedSegment)
				{
					Segment component = this.segmentControl.selectedSegment.GetComponent<Segment>();
					this.segmentControl.selectedSegment = null;
					component.TransitionButton();
				}
				this.segmentControl.selectedSegment = this.button;
				this.StoreTextColor();
				this.TransitionButton();
				this.segmentControl.onValueChanged.Invoke(this.index);
				return;
			}
			if (this.segmentControl.allowSwitchingOff)
			{
				this.Deselect();
				return;
			}
			this.MaintainSelection();
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0005E48B File Offset: 0x0005C68B
		private void Deselect()
		{
			this.segmentControl.selectedSegment = null;
			this.TransitionButton();
			this.segmentControl.onValueChanged.Invoke(-1);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0005E4B0 File Offset: 0x0005C6B0
		private void MaintainSelection()
		{
			if (this.button != this.segmentControl.selectedSegment)
			{
				return;
			}
			this.TransitionButton(true);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0005E4D2 File Offset: 0x0005C6D2
		internal void TransitionButton()
		{
			this.TransitionButton(false);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0005E4DC File Offset: 0x0005C6DC
		internal void TransitionButton(bool instant)
		{
			Color a = this.selected ? this.segmentControl.selectedColor : this.button.colors.normalColor;
			Color a2 = this.selected ? this.button.colors.normalColor : this.textColor;
			Sprite newSprite = this.selected ? this.button.spriteState.pressedSprite : null;
			string triggername = this.selected ? this.button.animationTriggers.pressedTrigger : this.button.animationTriggers.normalTrigger;
			switch (this.button.transition)
			{
			case Selectable.Transition.ColorTint:
				this.StartColorTween(a * this.button.colors.colorMultiplier, instant);
				this.ChangeTextColor(a2 * this.button.colors.colorMultiplier);
				return;
			case Selectable.Transition.SpriteSwap:
				this.DoSpriteSwap(newSprite);
				return;
			case Selectable.Transition.Animation:
				this.TriggerAnimation(triggername);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0005E5F8 File Offset: 0x0005C7F8
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (this.button.targetGraphic == null)
			{
				return;
			}
			this.button.targetGraphic.CrossFadeColor(targetColor, instant ? 0f : this.button.colors.fadeDuration, true, true);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0005E64C File Offset: 0x0005C84C
		internal void StoreTextColor()
		{
			Text componentInChildren = base.GetComponentInChildren<Text>();
			if (!componentInChildren)
			{
				return;
			}
			this.textColor = componentInChildren.color;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0005E678 File Offset: 0x0005C878
		private void ChangeTextColor(Color targetColor)
		{
			Text componentInChildren = base.GetComponentInChildren<Text>();
			if (!componentInChildren)
			{
				return;
			}
			componentInChildren.color = targetColor;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0005E69C File Offset: 0x0005C89C
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (this.button.image == null)
			{
				return;
			}
			this.button.image.overrideSprite = newSprite;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0005E6C4 File Offset: 0x0005C8C4
		private void TriggerAnimation(string triggername)
		{
			if (this.button.animator == null || !this.button.animator.isActiveAndEnabled || !this.button.animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			this.button.animator.ResetTrigger(this.button.animationTriggers.normalTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.pressedTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.highlightedTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.disabledTrigger);
			this.button.animator.SetTrigger(triggername);
		}

		// Token: 0x04000E79 RID: 3705
		internal int index;

		// Token: 0x04000E7A RID: 3706
		[SerializeField]
		private Color textColor;
	}
}
