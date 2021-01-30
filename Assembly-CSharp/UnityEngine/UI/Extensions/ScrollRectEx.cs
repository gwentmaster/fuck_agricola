using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C7 RID: 455
	[AddComponentMenu("UI/Extensions/ScrollRectEx")]
	public class ScrollRectEx : ScrollRect
	{
		// Token: 0x06001178 RID: 4472 RVA: 0x0006CF44 File Offset: 0x0006B144
		private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				foreach (Component component in parent.GetComponents<Component>())
				{
					if (component is !!0)
					{
						action((!!0)((object)((IEventSystemHandler)component)));
					}
				}
				parent = parent.parent;
			}
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0006CFA4 File Offset: 0x0006B1A4
		public override void OnInitializePotentialDrag(PointerEventData eventData)
		{
			this.DoForParents<IInitializePotentialDragHandler>(delegate(IInitializePotentialDragHandler parent)
			{
				parent.OnInitializePotentialDrag(eventData);
			});
			base.OnInitializePotentialDrag(eventData);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0006CFDC File Offset: 0x0006B1DC
		public override void OnDrag(PointerEventData eventData)
		{
			if (this.routeToParent)
			{
				this.DoForParents<IDragHandler>(delegate(IDragHandler parent)
				{
					parent.OnDrag(eventData);
				});
				return;
			}
			base.OnDrag(eventData);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0006D020 File Offset: 0x0006B220
		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (!base.horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
			{
				this.routeToParent = true;
			}
			else if (!base.vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
			{
				this.routeToParent = true;
			}
			else
			{
				this.routeToParent = false;
			}
			if (this.routeToParent)
			{
				this.DoForParents<IBeginDragHandler>(delegate(IBeginDragHandler parent)
				{
					parent.OnBeginDrag(eventData);
				});
				return;
			}
			base.OnBeginDrag(eventData);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0006D0E4 File Offset: 0x0006B2E4
		public override void OnEndDrag(PointerEventData eventData)
		{
			if (this.routeToParent)
			{
				this.DoForParents<IEndDragHandler>(delegate(IEndDragHandler parent)
				{
					parent.OnEndDrag(eventData);
				});
			}
			else
			{
				base.OnEndDrag(eventData);
			}
			this.routeToParent = false;
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0006D130 File Offset: 0x0006B330
		public override void OnScroll(PointerEventData eventData)
		{
			if (!base.horizontal && Math.Abs(eventData.scrollDelta.x) > Math.Abs(eventData.scrollDelta.y))
			{
				this.routeToParent = true;
			}
			else if (!base.vertical && Math.Abs(eventData.scrollDelta.x) < Math.Abs(eventData.scrollDelta.y))
			{
				this.routeToParent = true;
			}
			else
			{
				this.routeToParent = false;
			}
			if (this.routeToParent)
			{
				this.DoForParents<IScrollHandler>(delegate(IScrollHandler parent)
				{
					parent.OnScroll(eventData);
				});
				return;
			}
			base.OnScroll(eventData);
		}

		// Token: 0x0400100A RID: 4106
		private bool routeToParent;
	}
}
