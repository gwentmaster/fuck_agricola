﻿using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A6 RID: 422
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroller")]
	public class UIVerticalScroller : MonoBehaviour
	{
		// Token: 0x06001052 RID: 4178 RVA: 0x00066B31 File Offset: 0x00064D31
		public UIVerticalScroller()
		{
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00066B40 File Offset: 0x00064D40
		public UIVerticalScroller(RectTransform scrollingPanel, GameObject[] arrayOfElements, RectTransform center)
		{
			this._scrollingPanel = scrollingPanel;
			this._arrayOfElements = arrayOfElements;
			this._center = center;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00066B64 File Offset: 0x00064D64
		public void Awake()
		{
			ScrollRect component = base.GetComponent<ScrollRect>();
			if (!this._scrollingPanel)
			{
				this._scrollingPanel = component.content;
			}
			if (!this._center)
			{
				Debug.LogError("Please define the RectTransform for the Center viewport of the scrollable area");
			}
			if (this._arrayOfElements == null || this._arrayOfElements.Length == 0)
			{
				int childCount = component.content.childCount;
				if (childCount > 0)
				{
					this._arrayOfElements = new GameObject[childCount];
					for (int i = 0; i < childCount; i++)
					{
						this._arrayOfElements[i] = component.content.GetChild(i).gameObject;
					}
				}
			}
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00066BFC File Offset: 0x00064DFC
		public void Start()
		{
			if (this._arrayOfElements.Length < 1)
			{
				Debug.Log("No child content found, exiting..");
				return;
			}
			this.elementLength = this._arrayOfElements.Length;
			this.distance = new float[this.elementLength];
			this.distReposition = new float[this.elementLength];
			this.deltaY = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height * (float)this.elementLength / 3f * 2f;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, -this.deltaY);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
			for (int i = 0; i < this._arrayOfElements.Length; i++)
			{
				this.AddListener(this._arrayOfElements[i], i);
			}
			if (this.ScrollUpButton)
			{
				this.ScrollUpButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ScrollUp();
				});
			}
			if (this.ScrollDownButton)
			{
				this.ScrollDownButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ScrollDown();
				});
			}
			if (this.StartingIndex > -1)
			{
				this.StartingIndex = ((this.StartingIndex > this._arrayOfElements.Length) ? (this._arrayOfElements.Length - 1) : this.StartingIndex);
				this.SnapToElement(this.StartingIndex);
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00066D68 File Offset: 0x00064F68
		private void AddListener(GameObject button, int index)
		{
			button.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.DoSomething(index);
			});
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00066DA5 File Offset: 0x00064FA5
		private void DoSomething(int index)
		{
			if (this.ButtonClicked != null)
			{
				this.ButtonClicked.Invoke(index);
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00066DBC File Offset: 0x00064FBC
		public void Update()
		{
			if (this._arrayOfElements.Length < 1)
			{
				return;
			}
			for (int i = 0; i < this.elementLength; i++)
			{
				this.distReposition[i] = this._center.GetComponent<RectTransform>().position.y - this._arrayOfElements[i].GetComponent<RectTransform>().position.y;
				this.distance[i] = Mathf.Abs(this.distReposition[i]);
				float num = Mathf.Max(0.7f, 1f / (1f + this.distance[i] / 200f));
				this._arrayOfElements[i].GetComponent<RectTransform>().transform.localScale = new Vector3(num, num, 1f);
			}
			float num2 = Mathf.Min(this.distance);
			for (int j = 0; j < this.elementLength; j++)
			{
				this._arrayOfElements[j].GetComponent<CanvasGroup>().interactable = false;
				if (num2 == this.distance[j])
				{
					this.minElementsNum = j;
					this._arrayOfElements[j].GetComponent<CanvasGroup>().interactable = true;
					this.result = this._arrayOfElements[j].GetComponentInChildren<Text>().text;
				}
			}
			this.ScrollingElements(-this._arrayOfElements[this.minElementsNum].GetComponent<RectTransform>().anchoredPosition.y);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00066F10 File Offset: 0x00065110
		private void ScrollingElements(float position)
		{
			float y = Mathf.Lerp(this._scrollingPanel.anchoredPosition.y, position, Time.deltaTime * 1f);
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, y);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00066F63 File Offset: 0x00065163
		public string GetResults()
		{
			return this.result;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00066F6C File Offset: 0x0006516C
		public void SnapToElement(int element)
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height * (float)element;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, -num);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00066FBC File Offset: 0x000651BC
		public void ScrollUp()
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
			Vector2 b = new Vector2(this._scrollingPanel.anchoredPosition.x, this._scrollingPanel.anchoredPosition.y - num);
			this._scrollingPanel.anchoredPosition = Vector2.Lerp(this._scrollingPanel.anchoredPosition, b, 1f);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00067034 File Offset: 0x00065234
		public void ScrollDown()
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, this._scrollingPanel.anchoredPosition.y + num);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x04000F60 RID: 3936
		[Tooltip("Scrollable area (content of desired ScrollRect)")]
		public RectTransform _scrollingPanel;

		// Token: 0x04000F61 RID: 3937
		[Tooltip("Elements to populate inside the scroller")]
		public GameObject[] _arrayOfElements;

		// Token: 0x04000F62 RID: 3938
		[Tooltip("Center display area (position of zoomed content)")]
		public RectTransform _center;

		// Token: 0x04000F63 RID: 3939
		[Tooltip("Select the item to be in center on start. (optional)")]
		public int StartingIndex = -1;

		// Token: 0x04000F64 RID: 3940
		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject ScrollUpButton;

		// Token: 0x04000F65 RID: 3941
		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject ScrollDownButton;

		// Token: 0x04000F66 RID: 3942
		[Tooltip("Event fired when a specific item is clicked, exposes index number of item. (optional)")]
		public UnityEvent<int> ButtonClicked;

		// Token: 0x04000F67 RID: 3943
		private float[] distReposition;

		// Token: 0x04000F68 RID: 3944
		private float[] distance;

		// Token: 0x04000F69 RID: 3945
		private int minElementsNum;

		// Token: 0x04000F6A RID: 3946
		private int elementLength;

		// Token: 0x04000F6B RID: 3947
		private float deltaY;

		// Token: 0x04000F6C RID: 3948
		private string result;
	}
}
