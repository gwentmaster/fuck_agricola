using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000185 RID: 389
	[RequireComponent(typeof(Canvas))]
	[AddComponentMenu("UI/Extensions/Selection Box")]
	public class SelectionBox : MonoBehaviour
	{
		// Token: 0x06000EFC RID: 3836 RVA: 0x0005E898 File Offset: 0x0005CA98
		private void ValidateCanvas()
		{
			if (base.gameObject.GetComponent<Canvas>().renderMode != RenderMode.ScreenSpaceOverlay)
			{
				throw new Exception("SelectionBox component must be placed on a canvas in Screen Space Overlay mode.");
			}
			CanvasScaler component = base.gameObject.GetComponent<CanvasScaler>();
			if (component && component.enabled && (!Mathf.Approximately(component.scaleFactor, 1f) || component.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize))
			{
				Object.Destroy(component);
				Debug.LogWarning("SelectionBox component is on a gameObject with a Canvas Scaler component. As of now, Canvas Scalers without the default settings throw off the coordinates of the selection box. Canvas Scaler has been removed.");
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0005E908 File Offset: 0x0005CB08
		private void SetSelectableGroup(IEnumerable<MonoBehaviour> behaviourCollection)
		{
			if (behaviourCollection == null)
			{
				this.selectableGroup = null;
				return;
			}
			List<MonoBehaviour> list = new List<MonoBehaviour>();
			foreach (MonoBehaviour monoBehaviour in behaviourCollection)
			{
				if (monoBehaviour is IBoxSelectable)
				{
					list.Add(monoBehaviour);
				}
			}
			this.selectableGroup = list.ToArray();
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0005E978 File Offset: 0x0005CB78
		private void CreateBoxRect()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "Selection Box";
			gameObject.transform.parent = base.transform;
			gameObject.AddComponent<Image>();
			this.boxRect = (gameObject.transform as RectTransform);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0005E9C0 File Offset: 0x0005CBC0
		private void ResetBoxRect()
		{
			Image component = this.boxRect.GetComponent<Image>();
			component.color = this.color;
			component.sprite = this.art;
			this.origin = Vector2.zero;
			this.boxRect.anchoredPosition = Vector2.zero;
			this.boxRect.sizeDelta = Vector2.zero;
			this.boxRect.anchorMax = Vector2.zero;
			this.boxRect.anchorMin = Vector2.zero;
			this.boxRect.pivot = Vector2.zero;
			this.boxRect.gameObject.SetActive(false);
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0005EA5C File Offset: 0x0005CC5C
		private void BeginSelection()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			this.boxRect.gameObject.SetActive(true);
			this.origin = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if (!this.PointIsValidAgainstSelectionMask(this.origin))
			{
				this.ResetBoxRect();
				return;
			}
			this.boxRect.anchoredPosition = this.origin;
			MonoBehaviour[] array;
			if (this.selectableGroup == null)
			{
				array = Object.FindObjectsOfType<MonoBehaviour>();
			}
			else
			{
				array = this.selectableGroup;
			}
			List<IBoxSelectable> list = new List<IBoxSelectable>();
			MonoBehaviour[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				IBoxSelectable boxSelectable = array2[i] as IBoxSelectable;
				if (boxSelectable != null)
				{
					list.Add(boxSelectable);
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						boxSelectable.selected = false;
					}
				}
			}
			this.selectables = list.ToArray();
			this.clickedBeforeDrag = this.GetSelectableAtMousePosition();
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0005EB38 File Offset: 0x0005CD38
		private bool PointIsValidAgainstSelectionMask(Vector2 screenPoint)
		{
			if (!this.selectionMask)
			{
				return true;
			}
			Camera screenPointCamera = this.GetScreenPointCamera(this.selectionMask);
			return RectTransformUtility.RectangleContainsScreenPoint(this.selectionMask, screenPoint, screenPointCamera);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0005EB70 File Offset: 0x0005CD70
		private IBoxSelectable GetSelectableAtMousePosition()
		{
			if (!this.PointIsValidAgainstSelectionMask(Input.mousePosition))
			{
				return null;
			}
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				RectTransform rectTransform = boxSelectable.transform as RectTransform;
				if (rectTransform)
				{
					Camera screenPointCamera = this.GetScreenPointCamera(rectTransform);
					if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, screenPointCamera))
					{
						return boxSelectable;
					}
				}
				else
				{
					float magnitude = boxSelectable.transform.GetComponent<Renderer>().bounds.extents.magnitude;
					if (Vector2.Distance(this.GetScreenPointOfSelectable(boxSelectable), Input.mousePosition) <= magnitude)
					{
						return boxSelectable;
					}
				}
			}
			return null;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0005EC24 File Offset: 0x0005CE24
		private void DragSelection()
		{
			if (!Input.GetMouseButton(0) || !this.boxRect.gameObject.activeSelf)
			{
				return;
			}
			Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector2 vector2 = vector - this.origin;
			Vector2 anchoredPosition = this.origin;
			if (vector2.x < 0f)
			{
				anchoredPosition.x = vector.x;
				vector2.x = -vector2.x;
			}
			if (vector2.y < 0f)
			{
				anchoredPosition.y = vector.y;
				vector2.y = -vector2.y;
			}
			this.boxRect.anchoredPosition = anchoredPosition;
			this.boxRect.sizeDelta = vector2;
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				Vector3 v = this.GetScreenPointOfSelectable(boxSelectable);
				boxSelectable.preSelected = (RectTransformUtility.RectangleContainsScreenPoint(this.boxRect, v, null) && this.PointIsValidAgainstSelectionMask(v));
			}
			if (this.clickedBeforeDrag != null)
			{
				this.clickedBeforeDrag.preSelected = true;
			}
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0005ED54 File Offset: 0x0005CF54
		private void ApplySingleClickDeselection()
		{
			if (this.clickedBeforeDrag == null)
			{
				return;
			}
			if (this.clickedAfterDrag != null && this.clickedBeforeDrag.selected && this.clickedBeforeDrag.transform == this.clickedAfterDrag.transform)
			{
				this.clickedBeforeDrag.selected = false;
				this.clickedBeforeDrag.preSelected = false;
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0005EDB4 File Offset: 0x0005CFB4
		private void ApplyPreSelections()
		{
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				if (boxSelectable.preSelected)
				{
					boxSelectable.selected = true;
					boxSelectable.preSelected = false;
				}
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0005EDF0 File Offset: 0x0005CFF0
		private Vector2 GetScreenPointOfSelectable(IBoxSelectable selectable)
		{
			RectTransform rectTransform = selectable.transform as RectTransform;
			if (rectTransform)
			{
				return RectTransformUtility.WorldToScreenPoint(this.GetScreenPointCamera(rectTransform), selectable.transform.position);
			}
			return Camera.main.WorldToScreenPoint(selectable.transform.position);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0005EE44 File Offset: 0x0005D044
		private Camera GetScreenPointCamera(RectTransform rectTransform)
		{
			RectTransform rectTransform2 = rectTransform;
			Canvas canvas;
			do
			{
				canvas = rectTransform2.GetComponent<Canvas>();
				if (canvas && !canvas.isRootCanvas)
				{
					canvas = null;
				}
				rectTransform2 = (RectTransform)rectTransform2.parent;
			}
			while (canvas == null);
			switch (canvas.renderMode)
			{
			case RenderMode.ScreenSpaceOverlay:
				return null;
			case RenderMode.ScreenSpaceCamera:
				if (!canvas.worldCamera)
				{
					return Camera.main;
				}
				return canvas.worldCamera;
			}
			return Camera.main;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0005EEC0 File Offset: 0x0005D0C0
		public IBoxSelectable[] GetAllSelected()
		{
			if (this.selectables == null)
			{
				return new IBoxSelectable[0];
			}
			List<IBoxSelectable> list = new List<IBoxSelectable>();
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				if (boxSelectable.selected)
				{
					list.Add(boxSelectable);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0005EF10 File Offset: 0x0005D110
		private void EndSelection()
		{
			if (!Input.GetMouseButtonUp(0) || !this.boxRect.gameObject.activeSelf)
			{
				return;
			}
			this.clickedAfterDrag = this.GetSelectableAtMousePosition();
			this.ApplySingleClickDeselection();
			this.ApplyPreSelections();
			this.ResetBoxRect();
			this.onSelectionChange.Invoke(this.GetAllSelected());
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0005EF67 File Offset: 0x0005D167
		private void Start()
		{
			this.ValidateCanvas();
			this.CreateBoxRect();
			this.ResetBoxRect();
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0005EF7B File Offset: 0x0005D17B
		private void Update()
		{
			this.BeginSelection();
			this.DragSelection();
			this.EndSelection();
		}

		// Token: 0x04000E80 RID: 3712
		public Color color;

		// Token: 0x04000E81 RID: 3713
		public Sprite art;

		// Token: 0x04000E82 RID: 3714
		private Vector2 origin;

		// Token: 0x04000E83 RID: 3715
		public RectTransform selectionMask;

		// Token: 0x04000E84 RID: 3716
		private RectTransform boxRect;

		// Token: 0x04000E85 RID: 3717
		private IBoxSelectable[] selectables;

		// Token: 0x04000E86 RID: 3718
		private MonoBehaviour[] selectableGroup;

		// Token: 0x04000E87 RID: 3719
		private IBoxSelectable clickedBeforeDrag;

		// Token: 0x04000E88 RID: 3720
		private IBoxSelectable clickedAfterDrag;

		// Token: 0x04000E89 RID: 3721
		public SelectionBox.SelectionEvent onSelectionChange = new SelectionBox.SelectionEvent();

		// Token: 0x02000849 RID: 2121
		public class SelectionEvent : UnityEvent<IBoxSelectable[]>
		{
		}
	}
}
