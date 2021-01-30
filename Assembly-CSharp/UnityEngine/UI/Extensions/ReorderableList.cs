using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200017A RID: 378
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Extensions/Re-orderable list")]
	public class ReorderableList : MonoBehaviour
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0005CD4C File Offset: 0x0005AF4C
		public RectTransform Content
		{
			get
			{
				if (this._content == null)
				{
					this._content = this.ContentLayout.GetComponent<RectTransform>();
				}
				return this._content;
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0005CD74 File Offset: 0x0005AF74
		private Canvas GetCanvas()
		{
			Transform transform = base.transform;
			Canvas canvas = null;
			int num = 100;
			int num2 = 0;
			while (canvas == null && num2 < num)
			{
				canvas = transform.gameObject.GetComponent<Canvas>();
				if (canvas == null)
				{
					transform = transform.parent;
				}
				num2++;
			}
			return canvas;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0005CDC0 File Offset: 0x0005AFC0
		private void Awake()
		{
			if (this.ContentLayout == null)
			{
				Debug.LogError("You need to have a child LayoutGroup content set for the list: " + base.name, base.gameObject);
				return;
			}
			if (this.DraggableArea == null)
			{
				this.DraggableArea = base.transform.root.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
			}
			if (this.IsDropable && !base.GetComponent<Graphic>())
			{
				Debug.LogError("You need to have a Graphic control (such as an Image) for the list [" + base.name + "] to be droppable", base.gameObject);
				return;
			}
			this._listContent = this.ContentLayout.gameObject.AddComponent<ReorderableListContent>();
			this._listContent.Init(this);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0005CE78 File Offset: 0x0005B078
		public void TestReOrderableListTarget(ReorderableList.ReorderableListEventStruct item)
		{
			Debug.Log("Event Received");
			Debug.Log("Hello World, is my item a clone? [" + item.IsAClone.ToString() + "]");
		}

		// Token: 0x04000E3F RID: 3647
		[Tooltip("Child container with re-orderable items in a layout group")]
		public LayoutGroup ContentLayout;

		// Token: 0x04000E40 RID: 3648
		[Tooltip("Parent area to draw the dragged element on top of containers. Defaults to the root Canvas")]
		public RectTransform DraggableArea;

		// Token: 0x04000E41 RID: 3649
		[Tooltip("Can items be dragged from the container?")]
		public bool IsDraggable = true;

		// Token: 0x04000E42 RID: 3650
		[Tooltip("Should the draggable components be removed or cloned?")]
		public bool CloneDraggedObject;

		// Token: 0x04000E43 RID: 3651
		[Tooltip("Can new draggable items be dropped in to the container?")]
		public bool IsDropable = true;

		// Token: 0x04000E44 RID: 3652
		[Header("UI Re-orderable Events")]
		public ReorderableList.ReorderableListHandler OnElementDropped = new ReorderableList.ReorderableListHandler();

		// Token: 0x04000E45 RID: 3653
		public ReorderableList.ReorderableListHandler OnElementGrabbed = new ReorderableList.ReorderableListHandler();

		// Token: 0x04000E46 RID: 3654
		public ReorderableList.ReorderableListHandler OnElementRemoved = new ReorderableList.ReorderableListHandler();

		// Token: 0x04000E47 RID: 3655
		public ReorderableList.ReorderableListHandler OnElementAdded = new ReorderableList.ReorderableListHandler();

		// Token: 0x04000E48 RID: 3656
		private RectTransform _content;

		// Token: 0x04000E49 RID: 3657
		private ReorderableListContent _listContent;

		// Token: 0x02000845 RID: 2117
		[Serializable]
		public struct ReorderableListEventStruct
		{
			// Token: 0x060044A5 RID: 17573 RVA: 0x001427CF File Offset: 0x001409CF
			public void Cancel()
			{
				this.SourceObject.GetComponent<ReorderableListElement>().isValid = false;
			}

			// Token: 0x04002EA3 RID: 11939
			public GameObject DroppedObject;

			// Token: 0x04002EA4 RID: 11940
			public int FromIndex;

			// Token: 0x04002EA5 RID: 11941
			public ReorderableList FromList;

			// Token: 0x04002EA6 RID: 11942
			public bool IsAClone;

			// Token: 0x04002EA7 RID: 11943
			public GameObject SourceObject;

			// Token: 0x04002EA8 RID: 11944
			public int ToIndex;

			// Token: 0x04002EA9 RID: 11945
			public ReorderableList ToList;
		}

		// Token: 0x02000846 RID: 2118
		[Serializable]
		public class ReorderableListHandler : UnityEvent<ReorderableList.ReorderableListEventStruct>
		{
		}
	}
}
