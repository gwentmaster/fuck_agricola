using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001B9 RID: 441
	[AddComponentMenu("UI/Extensions/HoverTooltip")]
	public class HoverTooltip : MonoBehaviour
	{
		// Token: 0x06001114 RID: 4372 RVA: 0x0006ABAC File Offset: 0x00068DAC
		private void Start()
		{
			this.GUICamera = GameObject.Find("GUICamera").GetComponent<Camera>();
			this.GUIMode = base.transform.parent.parent.GetComponent<Canvas>().renderMode;
			this.bgImageSource = this.bgImage.GetComponent<Image>();
			this.inside = false;
			this.HideTooltipVisibility();
			base.transform.parent.gameObject.SetActive(false);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0006AC22 File Offset: 0x00068E22
		public void SetTooltip(string text)
		{
			this.NewTooltip();
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0006AC3C File Offset: 0x00068E3C
		public void SetTooltip(string[] texts)
		{
			this.NewTooltip();
			string text = "";
			int num = 0;
			foreach (string text2 in texts)
			{
				if (num == 0)
				{
					text += text2;
				}
				else
				{
					text = text + "\n" + text2;
				}
				num++;
			}
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0006AC22 File Offset: 0x00068E22
		public void SetTooltip(string text, bool test)
		{
			this.NewTooltip();
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0006ACA0 File Offset: 0x00068EA0
		public void OnScreenSpaceCamera()
		{
			Vector3 vector = this.GUICamera.ScreenToViewportPoint(Input.mousePosition);
			float num = this.GUICamera.ViewportToScreenPoint(vector).x + this.tooltipRealWidth * this.bgImage.pivot.x;
			if (num > this.upperRight.x)
			{
				float num2 = this.upperRight.x - num;
				float num3;
				if ((double)num2 > (double)this.defaultXOffset * 0.75)
				{
					num3 = num2;
				}
				else
				{
					num3 = this.defaultXOffset - this.tooltipRealWidth * 2f;
				}
				Vector3 position = new Vector3(this.GUICamera.ViewportToScreenPoint(vector).x + num3, 0f, 0f);
				vector.x = this.GUICamera.ScreenToViewportPoint(position).x;
			}
			num = this.GUICamera.ViewportToScreenPoint(vector).x - this.tooltipRealWidth * this.bgImage.pivot.x;
			if (num < this.lowerLeft.x)
			{
				float num4 = this.lowerLeft.x - num;
				float num3;
				if ((double)num4 < (double)this.defaultXOffset * 0.75 - (double)this.tooltipRealWidth)
				{
					num3 = -num4;
				}
				else
				{
					num3 = this.tooltipRealWidth * 2f;
				}
				Vector3 position2 = new Vector3(this.GUICamera.ViewportToScreenPoint(vector).x - num3, 0f, 0f);
				vector.x = this.GUICamera.ScreenToViewportPoint(position2).x;
			}
			num = this.GUICamera.ViewportToScreenPoint(vector).y - (this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y - this.tooltipRealHeight);
			if (num > this.upperRight.y)
			{
				float num5 = this.upperRight.y - num;
				float num6 = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				if ((double)num5 > (double)this.defaultYOffset * 0.75)
				{
					num6 = num5;
				}
				else
				{
					num6 = this.defaultYOffset - this.tooltipRealHeight * 2f;
				}
				Vector3 position3 = new Vector3(vector.x, this.GUICamera.ViewportToScreenPoint(vector).y + num6, 0f);
				vector.y = this.GUICamera.ScreenToViewportPoint(position3).y;
			}
			num = this.GUICamera.ViewportToScreenPoint(vector).y - this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
			if (num < this.lowerLeft.y)
			{
				float num7 = this.lowerLeft.y - num;
				float num6 = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				if ((double)num7 < (double)this.defaultYOffset * 0.75 - (double)this.tooltipRealHeight)
				{
					num6 = num7;
				}
				else
				{
					num6 = this.tooltipRealHeight * 2f;
				}
				Vector3 position4 = new Vector3(vector.x, this.GUICamera.ViewportToScreenPoint(vector).y + num6, 0f);
				vector.y = this.GUICamera.ScreenToViewportPoint(position4).y;
			}
			base.transform.parent.transform.position = new Vector3(this.GUICamera.ViewportToWorldPoint(vector).x, this.GUICamera.ViewportToWorldPoint(vector).y, 0f);
			base.transform.parent.gameObject.SetActive(true);
			this.inside = true;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0006B07A File Offset: 0x0006927A
		public void HideTooltip()
		{
			if (this.GUIMode == RenderMode.ScreenSpaceCamera && this != null)
			{
				base.transform.parent.gameObject.SetActive(false);
				this.inside = false;
				this.HideTooltipVisibility();
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0006B0B1 File Offset: 0x000692B1
		private void Update()
		{
			this.LayoutInit();
			if (this.inside && this.GUIMode == RenderMode.ScreenSpaceCamera)
			{
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0006B0D0 File Offset: 0x000692D0
		private void LayoutInit()
		{
			if (this.firstUpdate)
			{
				this.firstUpdate = false;
				this.bgImage.sizeDelta = new Vector2(this.hlG.preferredWidth + (float)this.horizontalPadding, this.hlG.preferredHeight + (float)this.verticalPadding);
				this.defaultYOffset = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				this.defaultXOffset = this.bgImage.sizeDelta.x * this.currentXScaleFactor * this.bgImage.pivot.x;
				this.tooltipRealHeight = this.bgImage.sizeDelta.y * this.currentYScaleFactor;
				this.tooltipRealWidth = this.bgImage.sizeDelta.x * this.currentXScaleFactor;
				this.ActivateTooltipVisibility();
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0006B1C4 File Offset: 0x000693C4
		private void NewTooltip()
		{
			this.firstUpdate = true;
			this.lowerLeft = this.GUICamera.ViewportToScreenPoint(new Vector3(0f, 0f, 0f));
			this.upperRight = this.GUICamera.ViewportToScreenPoint(new Vector3(1f, 1f, 0f));
			this.currentYScaleFactor = (float)Screen.height / base.transform.root.GetComponent<CanvasScaler>().referenceResolution.y;
			this.currentXScaleFactor = (float)Screen.width / base.transform.root.GetComponent<CanvasScaler>().referenceResolution.x;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0006B270 File Offset: 0x00069470
		public void ActivateTooltipVisibility()
		{
			Color color = this.thisText.color;
			this.thisText.color = new Color(color.r, color.g, color.b, 1f);
			this.bgImageSource.color = new Color(this.bgImageSource.color.r, this.bgImageSource.color.g, this.bgImageSource.color.b, 0.8f);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0006B2F8 File Offset: 0x000694F8
		public void HideTooltipVisibility()
		{
			Color color = this.thisText.color;
			this.thisText.color = new Color(color.r, color.g, color.b, 0f);
			this.bgImageSource.color = new Color(this.bgImageSource.color.r, this.bgImageSource.color.g, this.bgImageSource.color.b, 0f);
		}

		// Token: 0x04000FC6 RID: 4038
		public int horizontalPadding;

		// Token: 0x04000FC7 RID: 4039
		public int verticalPadding;

		// Token: 0x04000FC8 RID: 4040
		public Text thisText;

		// Token: 0x04000FC9 RID: 4041
		public HorizontalLayoutGroup hlG;

		// Token: 0x04000FCA RID: 4042
		public RectTransform bgImage;

		// Token: 0x04000FCB RID: 4043
		private Image bgImageSource;

		// Token: 0x04000FCC RID: 4044
		private bool firstUpdate;

		// Token: 0x04000FCD RID: 4045
		private bool inside;

		// Token: 0x04000FCE RID: 4046
		private RenderMode GUIMode;

		// Token: 0x04000FCF RID: 4047
		private Camera GUICamera;

		// Token: 0x04000FD0 RID: 4048
		private Vector3 lowerLeft;

		// Token: 0x04000FD1 RID: 4049
		private Vector3 upperRight;

		// Token: 0x04000FD2 RID: 4050
		private float currentYScaleFactor;

		// Token: 0x04000FD3 RID: 4051
		private float currentXScaleFactor;

		// Token: 0x04000FD4 RID: 4052
		private float defaultYOffset;

		// Token: 0x04000FD5 RID: 4053
		private float defaultXOffset;

		// Token: 0x04000FD6 RID: 4054
		private float tooltipRealHeight;

		// Token: 0x04000FD7 RID: 4055
		private float tooltipRealWidth;
	}
}
