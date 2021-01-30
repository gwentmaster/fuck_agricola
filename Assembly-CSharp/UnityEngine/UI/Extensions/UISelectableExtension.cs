using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D2 RID: 466
	[AddComponentMenu("UI/Extensions/UI Selectable Extension")]
	[RequireComponent(typeof(Selectable))]
	public class UISelectableExtension : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x060011CE RID: 4558 RVA: 0x0006E071 File Offset: 0x0006C271
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.OnButtonPress != null)
			{
				this.OnButtonPress.Invoke(eventData.button);
			}
			this._pressed = true;
			this._heldEventData = eventData;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0006E09A File Offset: 0x0006C29A
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.OnButtonRelease != null)
			{
				this.OnButtonRelease.Invoke(eventData.button);
			}
			this._pressed = false;
			this._heldEventData = null;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0006E0C3 File Offset: 0x0006C2C3
		private void Update()
		{
			if (!this._pressed)
			{
				return;
			}
			if (this.OnButtonHeld != null)
			{
				this.OnButtonHeld.Invoke(this._heldEventData.button);
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00003022 File Offset: 0x00001222
		public void TestClicked()
		{
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00003022 File Offset: 0x00001222
		public void TestPressed()
		{
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00003022 File Offset: 0x00001222
		public void TestReleased()
		{
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00003022 File Offset: 0x00001222
		public void TestHold()
		{
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0006E0EC File Offset: 0x0006C2EC
		private void OnDisable()
		{
			this._pressed = false;
		}

		// Token: 0x04001039 RID: 4153
		[Tooltip("Event that fires when a button is initially pressed down")]
		public UISelectableExtension.UIButtonEvent OnButtonPress;

		// Token: 0x0400103A RID: 4154
		[Tooltip("Event that fires when a button is released")]
		public UISelectableExtension.UIButtonEvent OnButtonRelease;

		// Token: 0x0400103B RID: 4155
		[Tooltip("Event that continually fires while a button is held down")]
		public UISelectableExtension.UIButtonEvent OnButtonHeld;

		// Token: 0x0400103C RID: 4156
		private bool _pressed;

		// Token: 0x0400103D RID: 4157
		private PointerEventData _heldEventData;

		// Token: 0x0200086C RID: 2156
		[Serializable]
		public class UIButtonEvent : UnityEvent<PointerEventData.InputButton>
		{
		}
	}
}
