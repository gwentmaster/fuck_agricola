using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001BA RID: 442
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Tooltip")]
	public class ToolTip : MonoBehaviour
	{
		// Token: 0x06001120 RID: 4384 RVA: 0x0006B380 File Offset: 0x00069580
		public void Awake()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			this._guiCamera = componentInParent.worldCamera;
			this._guiMode = componentInParent.renderMode;
			this._rectTransform = base.GetComponent<RectTransform>();
			this._text = base.GetComponentInChildren<Text>();
			this._inside = false;
			this.xShift = 0f;
			this.YShift = -30f;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0006B3F0 File Offset: 0x000695F0
		public void SetTooltip(string ttext)
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this._text.text = ttext;
				this._rectTransform.sizeDelta = new Vector2(this._text.preferredWidth + 40f, this._text.preferredHeight + 25f);
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0006B44A File Offset: 0x0006964A
		public void HideTooltip()
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				base.gameObject.SetActive(false);
				this._inside = false;
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0006B468 File Offset: 0x00069668
		private void FixedUpdate()
		{
			if (this._inside && this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0006B484 File Offset: 0x00069684
		public void OnScreenSpaceCamera()
		{
			Vector3 vector = this._guiCamera.ScreenToViewportPoint(Input.mousePosition - new Vector3(this.xShift, this.YShift, 0f));
			Vector3 vector2 = this._guiCamera.ViewportToWorldPoint(vector);
			this.width = this._rectTransform.sizeDelta[0];
			this.height = this._rectTransform.sizeDelta[1];
			Vector3 vector3 = this._guiCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
			Vector3 vector4 = this._guiCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
			float num = vector2.x + this.width / 2f;
			if (num > vector4.x)
			{
				Vector3 vector5 = new Vector3(num - vector4.x, 0f, 0f);
				Vector3 position = new Vector3(vector2.x - vector5.x, vector.y, 0f);
				vector.x = this._guiCamera.WorldToViewportPoint(position).x;
			}
			num = vector2.x - this.width / 2f;
			if (num < vector3.x)
			{
				Vector3 vector6 = new Vector3(vector3.x - num, 0f, 0f);
				Vector3 position2 = new Vector3(vector2.x + vector6.x, vector.y, 0f);
				vector.x = this._guiCamera.WorldToViewportPoint(position2).x;
			}
			num = vector2.y + this.height / 2f;
			if (num > vector4.y)
			{
				Vector3 vector7 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position3 = new Vector3(vector.x, vector2.y - vector7.y, 0f);
				vector.y = this._guiCamera.WorldToViewportPoint(position3).y;
			}
			num = vector2.y - this.height / 2f;
			if (num < vector3.y)
			{
				Vector3 vector8 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position4 = new Vector3(vector.x, vector2.y + vector8.y, 0f);
				vector.y = this._guiCamera.WorldToViewportPoint(position4).y;
			}
			base.transform.position = new Vector3(vector2.x, vector2.y, 0f);
			base.gameObject.SetActive(true);
			this._inside = true;
		}

		// Token: 0x04000FD8 RID: 4056
		private Text _text;

		// Token: 0x04000FD9 RID: 4057
		private RectTransform _rectTransform;

		// Token: 0x04000FDA RID: 4058
		private bool _inside;

		// Token: 0x04000FDB RID: 4059
		private float width;

		// Token: 0x04000FDC RID: 4060
		private float height;

		// Token: 0x04000FDD RID: 4061
		private float YShift;

		// Token: 0x04000FDE RID: 4062
		private float xShift;

		// Token: 0x04000FDF RID: 4063
		private RenderMode _guiMode;

		// Token: 0x04000FE0 RID: 4064
		private Camera _guiCamera;
	}
}
