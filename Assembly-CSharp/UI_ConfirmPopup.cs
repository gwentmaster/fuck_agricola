using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class UI_ConfirmPopup : MonoBehaviour
{
	// Token: 0x06000A68 RID: 2664 RVA: 0x00045280 File Offset: 0x00043480
	public void Setup(UI_ConfirmPopup.ClickCallback callback, string messageKey, UI_ConfirmPopup.ButtonFormat format)
	{
		this.m_Callback = callback;
		this.m_MessageText.KeyText = messageKey;
		switch (format)
		{
		case UI_ConfirmPopup.ButtonFormat.NoButtons:
			this.m_ConfirmButton.SetActive(false);
			this.m_CancelButton.SetActive(false);
			return;
		case UI_ConfirmPopup.ButtonFormat.OneButton:
			this.m_ConfirmButton.SetActive(false);
			this.m_CancelButton.SetActive(true);
			this.m_CancelButtonText.KeyText = "${Key_Back}";
			return;
		case UI_ConfirmPopup.ButtonFormat.TwoButtons:
			this.m_ConfirmButton.SetActive(true);
			this.m_CancelButton.SetActive(true);
			this.m_CancelButtonText.KeyText = "${Key_No}";
			this.m_ConfirmButtonText.KeyText = "${Key_Yes}";
			return;
		default:
			return;
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0004532D File Offset: 0x0004352D
	public void OnButtonPress(bool bConfirm)
	{
		if (this.m_Callback != null)
		{
			this.m_Callback(bConfirm);
		}
		this.m_Callback = null;
		ScreenManager.instance.PopScene();
	}

	// Token: 0x04000B02 RID: 2818
	public UILocalizedText m_MessageText;

	// Token: 0x04000B03 RID: 2819
	public GameObject m_ConfirmButton;

	// Token: 0x04000B04 RID: 2820
	public GameObject m_CancelButton;

	// Token: 0x04000B05 RID: 2821
	public UILocalizedText m_ConfirmButtonText;

	// Token: 0x04000B06 RID: 2822
	public UILocalizedText m_CancelButtonText;

	// Token: 0x04000B07 RID: 2823
	private UI_ConfirmPopup.ClickCallback m_Callback;

	// Token: 0x020007EE RID: 2030
	public enum ButtonFormat
	{
		// Token: 0x04002D6E RID: 11630
		NoButtons,
		// Token: 0x04002D6F RID: 11631
		OneButton,
		// Token: 0x04002D70 RID: 11632
		TwoButtons
	}

	// Token: 0x020007EF RID: 2031
	// (Invoke) Token: 0x0600437F RID: 17279
	public delegate void ClickCallback(bool bConfirm);
}
