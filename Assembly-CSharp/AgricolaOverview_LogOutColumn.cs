using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class AgricolaOverview_LogOutColumn : MonoBehaviour
{
	// Token: 0x0600050C RID: 1292 RVA: 0x00027328 File Offset: 0x00025528
	public void SetCard(GameObject cardObj, int index)
	{
		if (cardObj != null)
		{
			AnimateObject component = cardObj.GetComponent<AnimateObject>();
			if (component != null && this.m_cardLocators.Length > index && this.m_cardLocators[index] != null)
			{
				cardObj.SetActive(true);
				this.m_cardLocators[index].PlaceAnimateObject(component, true, true, false);
			}
		}
	}

	// Token: 0x0400049A RID: 1178
	[SerializeField]
	private AgricolaAnimationLocator[] m_cardLocators;
}
