using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001B7 RID: 439
	[AddComponentMenu("UI/Extensions/Bound Tooltip/Tooltip Item")]
	public class BoundTooltipItem : MonoBehaviour
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0006AA73 File Offset: 0x00068C73
		public bool IsActive
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0006AA80 File Offset: 0x00068C80
		private void Awake()
		{
			BoundTooltipItem.instance = this;
			if (!this.TooltipText)
			{
				this.TooltipText = base.GetComponentInChildren<Text>();
			}
			this.HideTooltip();
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0006AAA8 File Offset: 0x00068CA8
		public void ShowTooltip(string text, Vector3 pos)
		{
			if (this.TooltipText.text != text)
			{
				this.TooltipText.text = text;
			}
			base.transform.position = pos + this.ToolTipOffset;
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0002A073 File Offset: 0x00028273
		public void HideTooltip()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0006AAF7 File Offset: 0x00068CF7
		public static BoundTooltipItem Instance
		{
			get
			{
				if (BoundTooltipItem.instance == null)
				{
					BoundTooltipItem.instance = Object.FindObjectOfType<BoundTooltipItem>();
				}
				return BoundTooltipItem.instance;
			}
		}

		// Token: 0x04000FC0 RID: 4032
		public Text TooltipText;

		// Token: 0x04000FC1 RID: 4033
		public Vector3 ToolTipOffset;

		// Token: 0x04000FC2 RID: 4034
		private static BoundTooltipItem instance;
	}
}
