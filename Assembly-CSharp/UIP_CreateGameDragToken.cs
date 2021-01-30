using System;
using UnityEngine;

// Token: 0x020000FE RID: 254
public class UIP_CreateGameDragToken : MonoBehaviour
{
	// Token: 0x06000987 RID: 2439 RVA: 0x000400FE File Offset: 0x0003E2FE
	public void Setup(int avatarIndex, uint factionIndex)
	{
		this.m_avatar.SetAvatar(avatarIndex, true);
		this.m_colorizer.Colorize(factionIndex);
	}

	// Token: 0x04000A15 RID: 2581
	public Avatar_UI m_avatar;

	// Token: 0x04000A16 RID: 2582
	public ColorByFaction m_colorizer;
}
