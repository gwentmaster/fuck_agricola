using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class AgricolaFarmTile_Base : MonoBehaviour
{
	// Token: 0x06000306 RID: 774 RVA: 0x0001458E File Offset: 0x0001278E
	public virtual void SetParent(AgricolaFarmTile parent)
	{
		this.m_parent = parent;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00014598 File Offset: 0x00012798
	public virtual void SetSeason(EAgricolaSeason season)
	{
		if (this.m_springNode != null)
		{
			this.m_springNode.SetActive(season == EAgricolaSeason.SPRING);
		}
		if (this.m_summerNode != null)
		{
			this.m_summerNode.SetActive(season == EAgricolaSeason.SUMMER);
		}
		if (this.m_autumnNode != null)
		{
			this.m_autumnNode.SetActive(season == EAgricolaSeason.AUTUMN);
		}
		if (this.m_winterNode != null)
		{
			this.m_winterNode.SetActive(season == EAgricolaSeason.WINTER);
		}
		this.m_currentSeason = season;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00014620 File Offset: 0x00012820
	public EAgricolaSeason GetCurrentSeason()
	{
		return this.m_currentSeason;
	}

	// Token: 0x0400025C RID: 604
	[SerializeField]
	private GameObject m_springNode;

	// Token: 0x0400025D RID: 605
	[SerializeField]
	private GameObject m_summerNode;

	// Token: 0x0400025E RID: 606
	[SerializeField]
	private GameObject m_autumnNode;

	// Token: 0x0400025F RID: 607
	[SerializeField]
	private GameObject m_winterNode;

	// Token: 0x04000260 RID: 608
	protected AgricolaFarmTile m_parent;

	// Token: 0x04000261 RID: 609
	protected EAgricolaSeason m_currentSeason;
}
