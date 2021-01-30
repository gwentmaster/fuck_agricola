using System;
using TMPro;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class Popup_EndGameCell : MonoBehaviour
{
	// Token: 0x04000563 RID: 1379
	public ColorByFaction colorizer;

	// Token: 0x04000564 RID: 1380
	public new TextMeshProUGUI name;

	// Token: 0x04000565 RID: 1381
	public TextMeshProUGUI totalScore;

	// Token: 0x04000566 RID: 1382
	public TextMeshProUGUI[] scoreCatScore;

	// Token: 0x04000567 RID: 1383
	[HideInInspector]
	public int playerIndex;

	// Token: 0x04000568 RID: 1384
	[HideInInspector]
	public int currentTotal;

	// Token: 0x04000569 RID: 1385
	[HideInInspector]
	public GamePlayerScoreState scoreState;

	// Token: 0x0400056A RID: 1386
	[HideInInspector]
	public bool bWinner;
}
