using System;
using TMPro;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class Chat_InGame_Panel : MonoBehaviour
{
	// Token: 0x060004A4 RID: 1188 RVA: 0x00024A30 File Offset: 0x00022C30
	public void SetOnChatPanelOpenCallback(Chat_InGame_Panel.OnChatPanelOpen cb)
	{
		this.OnChatPanelOpenCallback = cb;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00024A3C File Offset: 0x00022C3C
	public void ButtonPressed()
	{
		this.IsShowing = !this.IsShowing;
		if (this.IsShowing)
		{
			this.m_ChatInputField.Select();
			this.m_ChatInputField.ActivateInputField();
		}
		if (this.OnChatPanelOpenCallback != null)
		{
			this.OnChatPanelOpenCallback();
		}
	}

	// Token: 0x04000426 RID: 1062
	public Animator InGame_Chat;

	// Token: 0x04000427 RID: 1063
	public TMP_InputField m_ChatInputField;

	// Token: 0x04000428 RID: 1064
	public bool IsShowing;

	// Token: 0x04000429 RID: 1065
	public Chat_InGame_Panel.OnChatPanelOpen OnChatPanelOpenCallback;

	// Token: 0x02000770 RID: 1904
	// (Invoke) Token: 0x06004201 RID: 16897
	public delegate void OnChatPanelOpen();
}
