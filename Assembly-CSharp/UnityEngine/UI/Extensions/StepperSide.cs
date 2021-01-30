using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000187 RID: 391
	[RequireComponent(typeof(Selectable))]
	public class StepperSide : UIBehaviour, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0005E36B File Offset: 0x0005C56B
		private Selectable button
		{
			get
			{
				return base.GetComponent<Selectable>();
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0005F4B0 File Offset: 0x0005D6B0
		private Stepper stepper
		{
			get
			{
				return base.GetComponentInParent<Stepper>();
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0005F4B8 File Offset: 0x0005D6B8
		private bool leftmost
		{
			get
			{
				return this.button == this.stepper.sides[0];
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0005E373 File Offset: 0x0005C573
		protected StepperSide()
		{
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0005F4D2 File Offset: 0x0005D6D2
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0005F4E3 File Offset: 0x0005D6E3
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Press();
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0005F4EB File Offset: 0x0005D6EB
		private void Press()
		{
			if (!this.button.IsActive() || !this.button.IsInteractable())
			{
				return;
			}
			if (this.leftmost)
			{
				this.stepper.StepDown();
				return;
			}
			this.stepper.StepUp();
		}
	}
}
