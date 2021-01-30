using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class UI_GameWaitingIcon : MonoBehaviour
{
	// Token: 0x06000A12 RID: 2578 RVA: 0x00043510 File Offset: 0x00041710
	private void Start()
	{
		this.m_gameWaitingIcon.SetActive(false);
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0004351E File Offset: 0x0004171E
	private void Update()
	{
		if (Network.m_Network == null)
		{
			this.m_gameWaitingIcon.SetActive(false);
			return;
		}
		if (Network.m_Network.m_bConnectedToServer)
		{
			this.m_gameWaitingIcon.SetActive(AgricolaLib.NetworkGetGameWaitingCount() > 0);
		}
	}

	// Token: 0x04000AA7 RID: 2727
	public GameObject m_gameWaitingIcon;
}
