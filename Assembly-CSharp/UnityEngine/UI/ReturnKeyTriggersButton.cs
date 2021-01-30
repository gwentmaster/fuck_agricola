using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x0200015B RID: 347
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/Return Key Trigger")]
	public class ReturnKeyTriggersButton : MonoBehaviour, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x06000DA7 RID: 3495 RVA: 0x00057DD4 File Offset: 0x00055FD4
		private void Start()
		{
			this._system = EventSystem.current;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00057DE1 File Offset: 0x00055FE1
		private void RemoveHighlight()
		{
			this.button.OnPointerExit(new PointerEventData(this._system));
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00057DFC File Offset: 0x00055FFC
		public void OnSubmit(BaseEventData eventData)
		{
			if (this.highlight)
			{
				this.button.OnPointerEnter(new PointerEventData(this._system));
			}
			this.button.OnPointerClick(new PointerEventData(this._system));
			if (this.highlight)
			{
				base.Invoke("RemoveHighlight", this.highlightDuration);
			}
		}

		// Token: 0x04000D6C RID: 3436
		private EventSystem _system;

		// Token: 0x04000D6D RID: 3437
		public Button button;

		// Token: 0x04000D6E RID: 3438
		private bool highlight = true;

		// Token: 0x04000D6F RID: 3439
		public float highlightDuration = 0.2f;
	}
}
