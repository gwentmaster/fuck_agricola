using System;
using GameData;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class ChatTextLine_Agricola : ChatTextLine
{
	// Token: 0x06000888 RID: 2184 RVA: 0x0003AF3C File Offset: 0x0003913C
	public override void SetChatTextLine(ChatChannelMessageEntry entry, string color_string)
	{
		int num = 0;
		if (this.m_parent != null)
		{
			num = this.m_parent.GetFactionIndex(entry.chatUserID);
		}
		color_string = "<color=" + string.Format("#{0:X2}{1:X2}{2:X2}", (int)(this.m_colors[num].r * 255f), (int)(this.m_colors[num].g * 255f), (int)(this.m_colors[num].b * 255f)) + "ff>";
		base.SetChatTextLine(entry, color_string);
	}

	// Token: 0x04000945 RID: 2373
	[SerializeField]
	private Color[] m_colors;
}
