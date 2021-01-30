using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000174 RID: 372
	[RequireComponent(typeof(RectTransform), typeof(Button))]
	public class DropDownListButton
	{
		// Token: 0x06000E63 RID: 3683 RVA: 0x0005C4A8 File Offset: 0x0005A6A8
		public DropDownListButton(GameObject btnObj)
		{
			this.gameobject = btnObj;
			this.rectTransform = btnObj.GetComponent<RectTransform>();
			this.btnImg = btnObj.GetComponent<Image>();
			this.btn = btnObj.GetComponent<Button>();
			this.txt = this.rectTransform.Find("Text").GetComponent<Text>();
			this.img = this.rectTransform.Find("Image").GetComponent<Image>();
		}

		// Token: 0x04000E13 RID: 3603
		public RectTransform rectTransform;

		// Token: 0x04000E14 RID: 3604
		public Button btn;

		// Token: 0x04000E15 RID: 3605
		public Text txt;

		// Token: 0x04000E16 RID: 3606
		public Image btnImg;

		// Token: 0x04000E17 RID: 3607
		public Image img;

		// Token: 0x04000E18 RID: 3608
		public GameObject gameobject;
	}
}
