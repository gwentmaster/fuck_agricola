using System;
using TMPro;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class AgricolaOverview_LogSmallRoundCell : MonoBehaviour
{
	// Token: 0x0600050E RID: 1294 RVA: 0x00027381 File Offset: 0x00025581
	public void SetRoundNumber(int roundNumber)
	{
		if (this.m_roundText != null)
		{
			this.m_roundText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_Round}") + " " + roundNumber.ToString();
		}
	}

	// Token: 0x0400049B RID: 1179
	[SerializeField]
	private TextMeshProUGUI m_roundText;
}
