using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C6 RID: 454
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Scrollrect Conflict Manager")]
	public class ScrollConflictManager : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x06001173 RID: 4467 RVA: 0x0006CDF4 File Offset: 0x0006AFF4
		private void Awake()
		{
			this._myScrollRect = base.GetComponent<ScrollRect>();
			this.scrollOtherHorizontally = this._myScrollRect.vertical;
			if (this.scrollOtherHorizontally)
			{
				if (this._myScrollRect.horizontal)
				{
					Debug.Log("You have added the SecondScrollRect to a scroll view that already has both directions selected");
				}
				if (!this.ParentScrollRect.horizontal)
				{
					Debug.Log("The other scroll rect doesnt support scrolling horizontally");
					return;
				}
			}
			else if (!this.ParentScrollRect.vertical)
			{
				Debug.Log("The other scroll rect doesnt support scrolling vertically");
			}
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0006CE6C File Offset: 0x0006B06C
		public void OnBeginDrag(PointerEventData eventData)
		{
			float num = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
			float num2 = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
			if (this.scrollOtherHorizontally)
			{
				if (num > num2)
				{
					this.scrollOther = true;
					this._myScrollRect.enabled = false;
					this.ParentScrollRect.OnBeginDrag(eventData);
					return;
				}
			}
			else if (num2 > num)
			{
				this.scrollOther = true;
				this._myScrollRect.enabled = false;
				this.ParentScrollRect.OnBeginDrag(eventData);
			}
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0006CF02 File Offset: 0x0006B102
		public void OnEndDrag(PointerEventData eventData)
		{
			if (this.scrollOther)
			{
				this.scrollOther = false;
				this._myScrollRect.enabled = true;
				this.ParentScrollRect.OnEndDrag(eventData);
			}
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0006CF2B File Offset: 0x0006B12B
		public void OnDrag(PointerEventData eventData)
		{
			if (this.scrollOther)
			{
				this.ParentScrollRect.OnDrag(eventData);
			}
		}

		// Token: 0x04001006 RID: 4102
		public ScrollRect ParentScrollRect;

		// Token: 0x04001007 RID: 4103
		private ScrollRect _myScrollRect;

		// Token: 0x04001008 RID: 4104
		private bool scrollOther;

		// Token: 0x04001009 RID: 4105
		private bool scrollOtherHorizontally;
	}
}
