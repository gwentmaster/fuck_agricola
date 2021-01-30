using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001CE RID: 462
	[AddComponentMenu("UI/Extensions/UI Highlightable Extension")]
	[RequireComponent(typeof(RectTransform), typeof(Graphic))]
	public class UIHighlightable : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x0006D709 File Offset: 0x0006B909
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x0006D711 File Offset: 0x0006B911
		public bool Interactable
		{
			get
			{
				return this.m_Interactable;
			}
			set
			{
				this.m_Interactable = value;
				this.HighlightInteractable(this.m_Graphic);
				this.OnInteractableChanged.Invoke(this.m_Interactable);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x0006D737 File Offset: 0x0006B937
		// (set) Token: 0x060011A4 RID: 4516 RVA: 0x0006D73F File Offset: 0x0006B93F
		public bool ClickToHold
		{
			get
			{
				return this.m_ClickToHold;
			}
			set
			{
				this.m_ClickToHold = value;
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0006D748 File Offset: 0x0006B948
		private void Awake()
		{
			this.m_Graphic = base.GetComponent<Graphic>();
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0006D756 File Offset: 0x0006B956
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.Interactable && !this.m_Pressed)
			{
				this.m_Highlighted = true;
				this.m_Graphic.color = this.HighlightedColor;
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0006D780 File Offset: 0x0006B980
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.Interactable && !this.m_Pressed)
			{
				this.m_Highlighted = false;
				this.m_Graphic.color = this.NormalColor;
			}
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0006D7AA File Offset: 0x0006B9AA
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.Interactable)
			{
				this.m_Graphic.color = this.PressedColor;
				if (this.ClickToHold)
				{
					this.m_Pressed = !this.m_Pressed;
				}
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0006D7DC File Offset: 0x0006B9DC
		public void OnPointerUp(PointerEventData eventData)
		{
			if (!this.m_Pressed)
			{
				this.HighlightInteractable(this.m_Graphic);
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0006D7F2 File Offset: 0x0006B9F2
		private void HighlightInteractable(Graphic graphic)
		{
			if (!this.m_Interactable)
			{
				graphic.color = this.DisabledColor;
				return;
			}
			if (this.m_Highlighted)
			{
				graphic.color = this.HighlightedColor;
				return;
			}
			graphic.color = this.NormalColor;
		}

		// Token: 0x0400101A RID: 4122
		private Graphic m_Graphic;

		// Token: 0x0400101B RID: 4123
		private bool m_Highlighted;

		// Token: 0x0400101C RID: 4124
		private bool m_Pressed;

		// Token: 0x0400101D RID: 4125
		[SerializeField]
		[Tooltip("Can this panel be interacted with or is it disabled? (does not affect child components)")]
		private bool m_Interactable = true;

		// Token: 0x0400101E RID: 4126
		[SerializeField]
		[Tooltip("Does the panel remain in the pressed state when clicked? (default false)")]
		private bool m_ClickToHold;

		// Token: 0x0400101F RID: 4127
		[Tooltip("The default color for the panel")]
		public Color NormalColor = Color.grey;

		// Token: 0x04001020 RID: 4128
		[Tooltip("The color for the panel when a mouse is over it or it is in focus")]
		public Color HighlightedColor = Color.yellow;

		// Token: 0x04001021 RID: 4129
		[Tooltip("The color for the panel when it is clicked/held")]
		public Color PressedColor = Color.green;

		// Token: 0x04001022 RID: 4130
		[Tooltip("The color for the panel when it is not interactable (see Interactable)")]
		public Color DisabledColor = Color.gray;

		// Token: 0x04001023 RID: 4131
		[Tooltip("Event for when the panel is enabled / disabled, to enable disabling / enabling of child or other gameobjects")]
		public UIHighlightable.InteractableChangedEvent OnInteractableChanged;

		// Token: 0x0200086A RID: 2154
		[Serializable]
		public class InteractableChangedEvent : UnityEvent<bool>
		{
		}
	}
}
