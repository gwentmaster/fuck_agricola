using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004B RID: 75
public class AgricolaResourceTray : MonoBehaviour
{
	// Token: 0x06000459 RID: 1113 RVA: 0x00022640 File Offset: 0x00020840
	private void Awake()
	{
		if (this.m_ResourceNodes != null)
		{
			for (int i = 0; i < this.m_ResourceNodes.Length; i++)
			{
				if (this.m_ResourceNodes[i] != null)
				{
					this.m_ResourceNodes[i].SetActive(false);
				}
			}
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00022688 File Offset: 0x00020888
	public void SetResourceCount(int resource_index, short resource_count)
	{
		if (this.m_ResourceNodes != null && resource_index >= 0 && resource_index < this.m_ResourceNodes.Length && this.m_ResourceNodes[resource_index] != null)
		{
			this.m_ResourceNodes[resource_index].SetActive(resource_count > 0);
		}
		if (this.m_ResourceCountText != null && resource_index >= 0 && resource_index < this.m_ResourceCountText.Length && this.m_ResourceCountText[resource_index] != null)
		{
			this.m_ResourceCountText[resource_index].text = resource_count.ToString();
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00022708 File Offset: 0x00020908
	public void SetResourceCounts(short[] resource_counts)
	{
		for (int i = 0; i < this.m_ResourceNodes.Length; i++)
		{
			if (this.m_ResourceNodes[i] != null)
			{
				this.m_ResourceNodes[i].SetActive(resource_counts[i] > 0);
			}
		}
		for (int j = 0; j < this.m_ResourceCountText.Length; j++)
		{
			if (this.m_ResourceCountText[j] != null)
			{
				this.m_ResourceCountText[j].text = resource_counts[j].ToString();
			}
		}
	}

	// Token: 0x040003EB RID: 1003
	[SerializeField]
	private GameObject[] m_ResourceNodes;

	// Token: 0x040003EC RID: 1004
	[SerializeField]
	private Text[] m_ResourceCountText;
}
