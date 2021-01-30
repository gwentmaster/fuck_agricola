using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200017D RID: 381
	[RequireComponent(typeof(RectTransform))]
	public class ReorderableListElement : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
	{
		// Token: 0x06000EB2 RID: 3762 RVA: 0x0005D0BC File Offset: 0x0005B2BC
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.isValid = true;
			if (this._reorderableList == null)
			{
				return;
			}
			if (!this._reorderableList.IsDraggable || !this.IsGrabbable)
			{
				this._draggingObject = null;
				return;
			}
			if (!this._reorderableList.CloneDraggedObject)
			{
				this._draggingObject = this._rect;
				this._fromIndex = this._rect.GetSiblingIndex();
				if (this._reorderableList.OnElementRemoved != null)
				{
					UnityEvent<ReorderableList.ReorderableListEventStruct> onElementRemoved = this._reorderableList.OnElementRemoved;
					ReorderableList.ReorderableListEventStruct arg = new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = (this._reorderableList.CloneDraggedObject ? base.gameObject : this._draggingObject.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex
					};
					onElementRemoved.Invoke(arg);
				}
				if (!this.isValid)
				{
					this._draggingObject = null;
					return;
				}
			}
			else
			{
				GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
				this._draggingObject = gameObject.GetComponent<RectTransform>();
			}
			this._draggingObjectOriginalSize = base.gameObject.GetComponent<RectTransform>().rect.size;
			this._draggingObjectLE = this._draggingObject.GetComponent<LayoutElement>();
			this._draggingObject.SetParent(this._reorderableList.DraggableArea, true);
			this._draggingObject.SetAsLastSibling();
			this._fakeElement = new GameObject("Fake").AddComponent<RectTransform>();
			this._fakeElementLE = this._fakeElement.gameObject.AddComponent<LayoutElement>();
			this.RefreshSizes();
			if (this._reorderableList.OnElementGrabbed != null)
			{
				UnityEvent<ReorderableList.ReorderableListEventStruct> onElementGrabbed = this._reorderableList.OnElementGrabbed;
				ReorderableList.ReorderableListEventStruct arg = new ReorderableList.ReorderableListEventStruct
				{
					DroppedObject = this._draggingObject.gameObject,
					IsAClone = this._reorderableList.CloneDraggedObject,
					SourceObject = (this._reorderableList.CloneDraggedObject ? base.gameObject : this._draggingObject.gameObject),
					FromList = this._reorderableList,
					FromIndex = this._fromIndex
				};
				onElementGrabbed.Invoke(arg);
				if (!this.isValid)
				{
					this.CancelDrag();
					return;
				}
			}
			this._isDragging = true;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0005D304 File Offset: 0x0005B504
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._isDragging)
			{
				return;
			}
			if (!this.isValid)
			{
				this.CancelDrag();
				return;
			}
			Canvas componentInParent = this._draggingObject.GetComponentInParent<Canvas>();
			Vector3 vector;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(componentInParent.GetComponent<RectTransform>(), eventData.position, componentInParent.worldCamera, out vector);
			this._draggingObject.position = vector;
			EventSystem.current.RaycastAll(eventData, this._raycastResults);
			for (int i = 0; i < this._raycastResults.Count; i++)
			{
				this._currentReorderableListRaycasted = this._raycastResults[i].gameObject.GetComponent<ReorderableList>();
				if (this._currentReorderableListRaycasted != null)
				{
					break;
				}
			}
			if (this._currentReorderableListRaycasted == null || !this._currentReorderableListRaycasted.IsDropable)
			{
				this.RefreshSizes();
				this._fakeElement.transform.SetParent(this._reorderableList.DraggableArea, false);
				return;
			}
			if (this._fakeElement.parent != this._currentReorderableListRaycasted)
			{
				this._fakeElement.SetParent(this._currentReorderableListRaycasted.Content, false);
			}
			float num = float.PositiveInfinity;
			int siblingIndex = 0;
			float num2 = 0f;
			for (int j = 0; j < this._currentReorderableListRaycasted.Content.childCount; j++)
			{
				RectTransform component = this._currentReorderableListRaycasted.Content.GetChild(j).GetComponent<RectTransform>();
				if (this._currentReorderableListRaycasted.ContentLayout is VerticalLayoutGroup)
				{
					num2 = Mathf.Abs(component.position.y - vector.y);
				}
				else if (this._currentReorderableListRaycasted.ContentLayout is HorizontalLayoutGroup)
				{
					num2 = Mathf.Abs(component.position.x - vector.x);
				}
				else if (this._currentReorderableListRaycasted.ContentLayout is GridLayoutGroup)
				{
					num2 = Mathf.Abs(component.position.x - vector.x) + Mathf.Abs(component.position.y - vector.y);
				}
				if (num2 < num)
				{
					num = num2;
					siblingIndex = j;
				}
			}
			this.RefreshSizes();
			this._fakeElement.SetSiblingIndex(siblingIndex);
			this._fakeElement.gameObject.SetActive(true);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0005D540 File Offset: 0x0005B740
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isDragging = false;
			if (this._draggingObject != null)
			{
				if (this._currentReorderableListRaycasted != null && this._currentReorderableListRaycasted.IsDropable && (this.IsTransferable || this._currentReorderableListRaycasted == this._reorderableList))
				{
					ReorderableList.ReorderableListEventStruct reorderableListEventStruct = new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = (this._reorderableList.CloneDraggedObject ? base.gameObject : this._draggingObject.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex,
						ToList = this._currentReorderableListRaycasted,
						ToIndex = this._fakeElement.GetSiblingIndex()
					};
					ReorderableList.ReorderableListEventStruct arg = reorderableListEventStruct;
					if (this._reorderableList && this._reorderableList.OnElementDropped != null)
					{
						this._reorderableList.OnElementDropped.Invoke(arg);
					}
					if (!this.isValid)
					{
						this.CancelDrag();
						return;
					}
					this.RefreshSizes();
					this._draggingObject.SetParent(this._currentReorderableListRaycasted.Content, false);
					this._draggingObject.rotation = this._currentReorderableListRaycasted.transform.rotation;
					this._draggingObject.SetSiblingIndex(this._fakeElement.GetSiblingIndex());
					this._reorderableList.OnElementAdded.Invoke(arg);
					if (!this.isValid)
					{
						throw new Exception("It's too late to cancel the Transfer! Do so in OnElementDropped!");
					}
				}
				else if (this.isDroppableInSpace)
				{
					UnityEvent<ReorderableList.ReorderableListEventStruct> onElementDropped = this._reorderableList.OnElementDropped;
					ReorderableList.ReorderableListEventStruct reorderableListEventStruct = new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = (this._reorderableList.CloneDraggedObject ? base.gameObject : this._draggingObject.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex
					};
					onElementDropped.Invoke(reorderableListEventStruct);
				}
				else
				{
					this.CancelDrag();
				}
			}
			if (this._fakeElement != null)
			{
				Object.Destroy(this._fakeElement.gameObject);
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0005D790 File Offset: 0x0005B990
		private void CancelDrag()
		{
			this._isDragging = false;
			if (this._reorderableList.CloneDraggedObject)
			{
				Object.Destroy(this._draggingObject.gameObject);
			}
			else
			{
				this.RefreshSizes();
				this._draggingObject.SetParent(this._reorderableList.Content, false);
				this._draggingObject.rotation = this._reorderableList.Content.transform.rotation;
				this._draggingObject.SetSiblingIndex(this._fromIndex);
				ReorderableList.ReorderableListEventStruct arg = new ReorderableList.ReorderableListEventStruct
				{
					DroppedObject = this._draggingObject.gameObject,
					IsAClone = this._reorderableList.CloneDraggedObject,
					SourceObject = (this._reorderableList.CloneDraggedObject ? base.gameObject : this._draggingObject.gameObject),
					FromList = this._reorderableList,
					FromIndex = this._fromIndex,
					ToList = this._reorderableList,
					ToIndex = this._fromIndex
				};
				this._reorderableList.OnElementAdded.Invoke(arg);
				if (!this.isValid)
				{
					throw new Exception("Transfer is already Cancelled.");
				}
			}
			if (this._fakeElement != null)
			{
				Object.Destroy(this._fakeElement.gameObject);
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0005D8E0 File Offset: 0x0005BAE0
		private void RefreshSizes()
		{
			Vector2 vector = this._draggingObjectOriginalSize;
			if (this._currentReorderableListRaycasted != null && this._currentReorderableListRaycasted.IsDropable && this._currentReorderableListRaycasted.Content.childCount > 0)
			{
				Transform child = this._currentReorderableListRaycasted.Content.GetChild(0);
				if (child != null)
				{
					vector = child.GetComponent<RectTransform>().rect.size;
				}
			}
			this._draggingObject.sizeDelta = vector;
			this._fakeElementLE.preferredHeight = (this._draggingObjectLE.preferredHeight = vector.y);
			this._fakeElementLE.preferredWidth = (this._draggingObjectLE.preferredWidth = vector.x);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0005D99B File Offset: 0x0005BB9B
		public void Init(ReorderableList reorderableList)
		{
			this._reorderableList = reorderableList;
			this._rect = base.GetComponent<RectTransform>();
		}

		// Token: 0x04000E50 RID: 3664
		[Tooltip("Can this element be dragged?")]
		public bool IsGrabbable = true;

		// Token: 0x04000E51 RID: 3665
		[Tooltip("Can this element be transfered to another list")]
		public bool IsTransferable = true;

		// Token: 0x04000E52 RID: 3666
		[Tooltip("Can this element be dropped in space?")]
		public bool isDroppableInSpace;

		// Token: 0x04000E53 RID: 3667
		private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

		// Token: 0x04000E54 RID: 3668
		private ReorderableList _currentReorderableListRaycasted;

		// Token: 0x04000E55 RID: 3669
		private RectTransform _draggingObject;

		// Token: 0x04000E56 RID: 3670
		private LayoutElement _draggingObjectLE;

		// Token: 0x04000E57 RID: 3671
		private Vector2 _draggingObjectOriginalSize;

		// Token: 0x04000E58 RID: 3672
		private RectTransform _fakeElement;

		// Token: 0x04000E59 RID: 3673
		private LayoutElement _fakeElementLE;

		// Token: 0x04000E5A RID: 3674
		private int _fromIndex;

		// Token: 0x04000E5B RID: 3675
		private bool _isDragging;

		// Token: 0x04000E5C RID: 3676
		private RectTransform _rect;

		// Token: 0x04000E5D RID: 3677
		private ReorderableList _reorderableList;

		// Token: 0x04000E5E RID: 3678
		internal bool isValid;
	}
}
