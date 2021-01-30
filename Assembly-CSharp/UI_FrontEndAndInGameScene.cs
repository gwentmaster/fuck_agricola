using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class UI_FrontEndAndInGameScene : MonoBehaviour
{
	// Token: 0x06000A10 RID: 2576 RVA: 0x000434A4 File Offset: 0x000416A4
	public void SetIsInGame(bool bInGame)
	{
		if (this.bSet)
		{
			return;
		}
		if (this.m_frontEndObjects != null)
		{
			GameObject[] array = this.m_frontEndObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!bInGame);
			}
		}
		if (this.m_inGameObjects != null)
		{
			GameObject[] array = this.m_inGameObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(bInGame);
			}
		}
		this.bSet = true;
	}

	// Token: 0x04000AA4 RID: 2724
	public GameObject[] m_frontEndObjects;

	// Token: 0x04000AA5 RID: 2725
	public GameObject[] m_inGameObjects;

	// Token: 0x04000AA6 RID: 2726
	private bool bSet;
}
