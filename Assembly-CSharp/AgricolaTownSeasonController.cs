using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class AgricolaTownSeasonController : MonoBehaviour
{
	// Token: 0x06000462 RID: 1122 RVA: 0x00022DF0 File Offset: 0x00020FF0
	public void SetSeason(EAgricolaSeason season)
	{
		if (this.m_springNodes != null && this.m_springNodes.Length != 0)
		{
			for (int i = 0; i < this.m_springNodes.Length; i++)
			{
				if (this.m_springNodes[i] != null)
				{
					this.m_springNodes[i].SetActive(season == EAgricolaSeason.SPRING);
				}
			}
		}
		if (this.m_summerNodes != null && this.m_summerNodes.Length != 0)
		{
			for (int j = 0; j < this.m_summerNodes.Length; j++)
			{
				if (this.m_summerNodes[j] != null)
				{
					this.m_summerNodes[j].SetActive(season == EAgricolaSeason.SUMMER);
				}
			}
		}
		if (this.m_autumnNodes != null && this.m_autumnNodes.Length != 0)
		{
			for (int k = 0; k < this.m_autumnNodes.Length; k++)
			{
				if (this.m_autumnNodes[k] != null)
				{
					this.m_autumnNodes[k].SetActive(season == EAgricolaSeason.AUTUMN);
				}
			}
		}
		if (this.m_winterNodes != null && this.m_winterNodes.Length != 0)
		{
			for (int l = 0; l < this.m_winterNodes.Length; l++)
			{
				if (this.m_winterNodes[l] != null)
				{
					this.m_winterNodes[l].SetActive(season == EAgricolaSeason.WINTER);
				}
			}
		}
	}

	// Token: 0x040003F4 RID: 1012
	[SerializeField]
	private GameObject[] m_springNodes;

	// Token: 0x040003F5 RID: 1013
	[SerializeField]
	private GameObject[] m_summerNodes;

	// Token: 0x040003F6 RID: 1014
	[SerializeField]
	private GameObject[] m_autumnNodes;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private GameObject[] m_winterNodes;
}
