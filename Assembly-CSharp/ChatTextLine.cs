using System;
using GameData;
using TMPro;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class ChatTextLine : MonoBehaviour
{
	// Token: 0x06000884 RID: 2180 RVA: 0x0003AE13 File Offset: 0x00039013
	public void SetParent(Popup_Chat parent)
	{
		this.m_parent = parent;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x0003AE1C File Offset: 0x0003901C
	public void OnClick()
	{
		if (this.m_parent != null)
		{
			this.m_parent.OnChatEntryClicked(this.m_ChatUserDisplayName, this.m_ChatUserID);
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0003AE44 File Offset: 0x00039044
	public virtual void SetChatTextLine(ChatChannelMessageEntry entry, string color_string)
	{
		this.m_ChatUserID = entry.chatUserID;
		this.m_ChatUserDisplayName = entry.chatUserDisplayName;
		this.m_ChatTimestamp = entry.chatTimestamp;
		this.m_ChatMessage = entry.chatMessage;
		string text = color_string;
		string text2 = color_string;
		if (!string.IsNullOrEmpty(this.m_ChatTimestamp))
		{
			text2 = text2 + "<voffset=.1em>[</voffset>" + this.m_ChatTimestamp + "<voffset=.1em>]</voffset> ";
		}
		if (!string.IsNullOrEmpty(this.m_ChatUserDisplayName))
		{
			text = text + "<b>" + this.m_ChatUserDisplayName + "</b>: ";
		}
		text += this.m_ChatMessage;
		if (!string.IsNullOrEmpty(color_string))
		{
			text += "</color>";
			text2 += "</color>";
		}
		if (this.m_messageText != null)
		{
			if (!string.IsNullOrEmpty(text2))
			{
				this.m_messageText.text = "<size=80%><smallcaps>" + text2 + "</smallcaps></size><indent=4.8em>" + text;
				return;
			}
			this.m_messageText.text = text;
		}
	}

	// Token: 0x0400093E RID: 2366
	public TextMeshProUGUI m_timestampText;

	// Token: 0x0400093F RID: 2367
	public TextMeshProUGUI m_messageText;

	// Token: 0x04000940 RID: 2368
	protected uint m_ChatUserID;

	// Token: 0x04000941 RID: 2369
	protected string m_ChatUserDisplayName;

	// Token: 0x04000942 RID: 2370
	protected string m_ChatTimestamp;

	// Token: 0x04000943 RID: 2371
	protected string m_ChatMessage;

	// Token: 0x04000944 RID: 2372
	protected Popup_Chat m_parent;
}
