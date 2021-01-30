using System;
using TMPro;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class AgricolaOverview_LogInRoundStart : MonoBehaviour
{
	// Token: 0x0600050A RID: 1290 RVA: 0x00027248 File Offset: 0x00025448
	public void SetRound(int roundNumber)
	{
		string arg = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Stage}");
		string arg2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}");
		if (roundNumber == 0)
		{
			roundNumber = 1;
		}
		int num = 1;
		if (roundNumber >= 5 && roundNumber <= 7)
		{
			num = 2;
		}
		else if (roundNumber >= 8 && roundNumber <= 9)
		{
			num = 3;
		}
		else if (roundNumber >= 10 && roundNumber <= 11)
		{
			num = 4;
		}
		else if (roundNumber >= 12 && roundNumber <= 13)
		{
			num = 5;
		}
		else if (roundNumber == 14)
		{
			num = 6;
		}
		this.m_stageText.text = string.Format("{0} {1}", arg, num.ToString());
		this.m_roundText.text = string.Format("{0} {1}", arg2, roundNumber.ToString());
		EAgricolaSeason roundSeason = AgricolaLib.GetRoundSeason(roundNumber);
		this.m_springNode.SetActive(roundSeason == EAgricolaSeason.SPRING);
		this.m_summerNode.SetActive(roundSeason == EAgricolaSeason.SUMMER);
		this.m_autumnNode.SetActive(roundSeason == EAgricolaSeason.AUTUMN);
	}

	// Token: 0x04000495 RID: 1173
	[SerializeField]
	private TextMeshProUGUI m_stageText;

	// Token: 0x04000496 RID: 1174
	[SerializeField]
	private TextMeshProUGUI m_roundText;

	// Token: 0x04000497 RID: 1175
	[SerializeField]
	private GameObject m_springNode;

	// Token: 0x04000498 RID: 1176
	[SerializeField]
	private GameObject m_summerNode;

	// Token: 0x04000499 RID: 1177
	[SerializeField]
	private GameObject m_autumnNode;
}
