using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	// Token: 0x02000159 RID: 345
	[AddComponentMenu("UI/Extensions/Extensions Toggle", 31)]
	[RequireComponent(typeof(RectTransform))]
	public class ExtensionsToggle : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICanvasElement
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x000578EE File Offset: 0x00055AEE
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x000578F6 File Offset: 0x00055AF6
		public ExtensionsToggleGroup Group
		{
			get
			{
				return this.m_Group;
			}
			set
			{
				this.m_Group = value;
				this.SetToggleGroup(this.m_Group, true);
				this.PlayEffect(true);
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00057913 File Offset: 0x00055B13
		protected ExtensionsToggle()
		{
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00057938 File Offset: 0x00055B38
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetToggleGroup(this.m_Group, false);
			this.PlayEffect(true);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00057954 File Offset: 0x00055B54
		protected override void OnDisable()
		{
			this.SetToggleGroup(null, false);
			base.OnDisable();
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00057964 File Offset: 0x00055B64
		protected override void OnDidApplyAnimationProperties()
		{
			if (this.graphic != null)
			{
				bool flag = !Mathf.Approximately(this.graphic.canvasRenderer.GetColor().a, 0f);
				if (this.m_IsOn != flag)
				{
					this.m_IsOn = flag;
					this.Set(!flag);
				}
			}
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000579C4 File Offset: 0x00055BC4
		private void SetToggleGroup(ExtensionsToggleGroup newGroup, bool setMemberValue)
		{
			ExtensionsToggleGroup group = this.m_Group;
			if (this.m_Group != null)
			{
				this.m_Group.UnregisterToggle(this);
			}
			if (setMemberValue)
			{
				this.m_Group = newGroup;
			}
			if (this.m_Group != null && this.IsActive())
			{
				this.m_Group.RegisterToggle(this);
			}
			if (newGroup != null && newGroup != group && this.IsOn && this.IsActive())
			{
				this.m_Group.NotifyToggleOn(this);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00057A4C File Offset: 0x00055C4C
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x00057A54 File Offset: 0x00055C54
		public bool IsOn
		{
			get
			{
				return this.m_IsOn;
			}
			set
			{
				this.Set(value);
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00057A5D File Offset: 0x00055C5D
		private void Set(bool value)
		{
			this.Set(value, true);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00057A68 File Offset: 0x00055C68
		private void Set(bool value, bool sendCallback)
		{
			if (this.m_IsOn == value)
			{
				return;
			}
			this.m_IsOn = value;
			if (this.m_Group != null && this.IsActive() && (this.m_IsOn || (!this.m_Group.AnyTogglesOn() && !this.m_Group.AllowSwitchOff)))
			{
				this.m_IsOn = true;
				this.m_Group.NotifyToggleOn(this);
			}
			this.PlayEffect(this.toggleTransition == ExtensionsToggle.ToggleTransition.None);
			if (sendCallback)
			{
				this.onValueChanged.Invoke(this.m_IsOn);
				this.onToggleChanged.Invoke(this);
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00057B00 File Offset: 0x00055D00
		private void PlayEffect(bool instant)
		{
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.CrossFadeAlpha(this.m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00057B40 File Offset: 0x00055D40
		protected override void Start()
		{
			this.PlayEffect(true);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00057B49 File Offset: 0x00055D49
		private void InternalToggle()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.IsOn = !this.IsOn;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00057B6B File Offset: 0x00055D6B
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.InternalToggle();
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00057B7C File Offset: 0x00055D7C
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.InternalToggle();
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00057B84 File Offset: 0x00055D84
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04000D60 RID: 3424
		public string UniqueID;

		// Token: 0x04000D61 RID: 3425
		public ExtensionsToggle.ToggleTransition toggleTransition = ExtensionsToggle.ToggleTransition.Fade;

		// Token: 0x04000D62 RID: 3426
		public Graphic graphic;

		// Token: 0x04000D63 RID: 3427
		[SerializeField]
		private ExtensionsToggleGroup m_Group;

		// Token: 0x04000D64 RID: 3428
		[Tooltip("Use this event if you only need the bool state of the toggle that was changed")]
		public ExtensionsToggle.ToggleEvent onValueChanged = new ExtensionsToggle.ToggleEvent();

		// Token: 0x04000D65 RID: 3429
		[Tooltip("Use this event if you need access to the toggle that was changed")]
		public ExtensionsToggle.ToggleEventObject onToggleChanged = new ExtensionsToggle.ToggleEventObject();

		// Token: 0x04000D66 RID: 3430
		[FormerlySerializedAs("m_IsActive")]
		[Tooltip("Is the toggle currently on or off?")]
		[SerializeField]
		private bool m_IsOn;

		// Token: 0x02000830 RID: 2096
		public enum ToggleTransition
		{
			// Token: 0x04002E8B RID: 11915
			None,
			// Token: 0x04002E8C RID: 11916
			Fade
		}

		// Token: 0x02000831 RID: 2097
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}

		// Token: 0x02000832 RID: 2098
		[Serializable]
		public class ToggleEventObject : UnityEvent<ExtensionsToggle>
		{
		}
	}
}
