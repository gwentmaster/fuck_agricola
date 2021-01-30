using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class Popup_ScoreCardAndRoundOverview : PopupBase
{
	// Token: 0x060005A9 RID: 1449 RVA: 0x0002C0D8 File Offset: 0x0002A2D8
	public void DisplayRoundOverviewCard()
	{
		if (!this.m_bInit)
		{
			this.Init();
		}
		if (this.m_roundOverviewCard != null)
		{
			this.m_roundOverviewCard.SetActive(true);
		}
		if (this.m_scoreOverviewCard != null)
		{
			this.m_scoreOverviewCard.SetActive(false);
		}
		if (this.m_PopupManager.GetActivePopup() == EPopups.NONE)
		{
			this.m_PopupManager.SetPopup(EPopups.SCORE_OVERVIEW);
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0002C140 File Offset: 0x0002A340
	public void DisplayScoreOverviewCard()
	{
		if (!this.m_bInit)
		{
			this.Init();
		}
		if (this.m_roundOverviewCard != null)
		{
			this.m_roundOverviewCard.SetActive(false);
		}
		if (this.m_scoreOverviewCard != null)
		{
			this.m_scoreOverviewCard.SetActive(true);
		}
		if (this.m_PopupManager.GetActivePopup() == EPopups.NONE)
		{
			this.m_PopupManager.SetPopup(EPopups.SCORE_OVERVIEW);
		}
		if (this.GetPlayerToDisplay())
		{
			this.UpdateScoreCard();
		}
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0002C1B6 File Offset: 0x0002A3B6
	public void Close()
	{
		this.m_PopupManager.SetPopup(EPopups.NONE);
		this.m_displayedPlayerIndex = 0;
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0002C1CB File Offset: 0x0002A3CB
	private void Init()
	{
		this.m_bInit = true;
		this.m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0002C1E9 File Offset: 0x0002A3E9
	private void Update()
	{
		if (this.GetPlayerToDisplay())
		{
			this.UpdateScoreCard();
		}
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0002C1FC File Offset: 0x0002A3FC
	private bool GetPlayerToDisplay()
	{
		AgricolaFarm farm = this.m_gameController.GetFarm();
		if (farm != null)
		{
			if (farm.gameObject.activeSelf && farm.GetDisplayedPlayerIndex() != this.m_displayedPlayerIndex)
			{
				this.m_displayedPlayerIndex = farm.GetDisplayedPlayerIndex();
				return true;
			}
			if (!farm.gameObject.activeSelf && this.m_gameController.GetLocalPlayerIndex() != this.m_displayedPlayerIndex)
			{
				this.m_displayedPlayerIndex = this.m_gameController.GetLocalPlayerIndex();
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0002C27C File Offset: 0x0002A47C
	private void HandleRow(TextMeshProUGUI[] rowText, int[] rowThresholds, int count)
	{
		bool flag = false;
		for (int i = 0; i < rowText.Length; i++)
		{
			if (!flag && rowThresholds[i] >= count)
			{
				flag = true;
				rowText[i].font = this.m_selectedFont;
				rowText[i].color = this.m_selectedColor;
			}
			else
			{
				rowText[i].font = this.m_unselectedFont;
				rowText[i].color = this.m_unselectedColor;
			}
		}
		if (!flag)
		{
			rowText[rowText.Length - 1].font = this.m_selectedFont;
			rowText[rowText.Length - 1].color = this.m_selectedColor;
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0002C308 File Offset: 0x0002A508
	private void UpdateScoreCard()
	{
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.GetGamePlayerScoreState(this.m_displayedPlayerIndex, intPtr, 1024);
		GamePlayerScoreState gamePlayerScoreState = (GamePlayerScoreState)Marshal.PtrToStructure(intPtr, typeof(GamePlayerScoreState));
		this.HandleRow(this.m_fieldTextFields, Popup_ScoreCardAndRoundOverview.k_fieldThresholds, gamePlayerScoreState.count[0]);
		this.HandleRow(this.m_pasturesTextFields, Popup_ScoreCardAndRoundOverview.k_pastureThresholds, gamePlayerScoreState.count[1]);
		this.HandleRow(this.m_grainTextFields, Popup_ScoreCardAndRoundOverview.k_grainThresholds, gamePlayerScoreState.count[2]);
		this.HandleRow(this.m_vegetablesTextFields, Popup_ScoreCardAndRoundOverview.k_vegetableThresholds, gamePlayerScoreState.count[3]);
		this.HandleRow(this.m_sheepTextFields, Popup_ScoreCardAndRoundOverview.k_sheepThresholds, gamePlayerScoreState.count[4]);
		this.HandleRow(this.m_boarTextFields, Popup_ScoreCardAndRoundOverview.k_boarThresholds, gamePlayerScoreState.count[5]);
		this.HandleRow(this.m_cattleTextFields, Popup_ScoreCardAndRoundOverview.k_cattleThresholds, gamePlayerScoreState.count[6]);
		this.m_emptySpaceCount.text = gamePlayerScoreState.count[7].ToString();
		this.m_emptySpaceValue.text = gamePlayerScoreState.score[7].ToString();
		this.m_fencedStableCount.text = gamePlayerScoreState.count[8].ToString();
		this.m_fencedStableValue.text = gamePlayerScoreState.score[8].ToString();
		this.m_familyCount.text = gamePlayerScoreState.count[10].ToString();
		this.m_familyValue.text = gamePlayerScoreState.score[10].ToString();
		this.m_roomCount.text = gamePlayerScoreState.count[9].ToString();
		this.m_roomValue.text = gamePlayerScoreState.score[9].ToString();
		this.m_colorizer.Colorize((uint)gamePlayerScoreState.playerFaction);
		gchandle.Free();
	}

	// Token: 0x0400056B RID: 1387
	private const int k_maxDataSize = 1024;

	// Token: 0x0400056C RID: 1388
	private static readonly int[] k_fieldThresholds = new int[]
	{
		1,
		2,
		3,
		4,
		5
	};

	// Token: 0x0400056D RID: 1389
	private static readonly int[] k_pastureThresholds = new int[]
	{
		0,
		1,
		2,
		3,
		4
	};

	// Token: 0x0400056E RID: 1390
	private static readonly int[] k_grainThresholds = new int[]
	{
		0,
		3,
		5,
		7,
		8
	};

	// Token: 0x0400056F RID: 1391
	private static readonly int[] k_vegetableThresholds = new int[]
	{
		0,
		1,
		2,
		3,
		4
	};

	// Token: 0x04000570 RID: 1392
	private static readonly int[] k_sheepThresholds = new int[]
	{
		0,
		3,
		5,
		7,
		8
	};

	// Token: 0x04000571 RID: 1393
	private static readonly int[] k_boarThresholds = new int[]
	{
		0,
		2,
		4,
		6,
		7
	};

	// Token: 0x04000572 RID: 1394
	private static readonly int[] k_cattleThresholds = new int[]
	{
		0,
		1,
		3,
		5,
		6
	};

	// Token: 0x04000573 RID: 1395
	public GameObject m_roundOverviewCard;

	// Token: 0x04000574 RID: 1396
	public GameObject m_scoreOverviewCard;

	// Token: 0x04000575 RID: 1397
	public TMP_FontAsset m_selectedFont;

	// Token: 0x04000576 RID: 1398
	public TMP_FontAsset m_unselectedFont;

	// Token: 0x04000577 RID: 1399
	public Color m_selectedColor = Color.white;

	// Token: 0x04000578 RID: 1400
	public Color m_unselectedColor = Color.black;

	// Token: 0x04000579 RID: 1401
	public TextMeshProUGUI[] m_fieldTextFields;

	// Token: 0x0400057A RID: 1402
	public TextMeshProUGUI[] m_pasturesTextFields;

	// Token: 0x0400057B RID: 1403
	public TextMeshProUGUI[] m_grainTextFields;

	// Token: 0x0400057C RID: 1404
	public TextMeshProUGUI[] m_vegetablesTextFields;

	// Token: 0x0400057D RID: 1405
	public TextMeshProUGUI[] m_sheepTextFields;

	// Token: 0x0400057E RID: 1406
	public TextMeshProUGUI[] m_boarTextFields;

	// Token: 0x0400057F RID: 1407
	public TextMeshProUGUI[] m_cattleTextFields;

	// Token: 0x04000580 RID: 1408
	public TextMeshProUGUI m_emptySpaceCount;

	// Token: 0x04000581 RID: 1409
	public TextMeshProUGUI m_emptySpaceValue;

	// Token: 0x04000582 RID: 1410
	public TextMeshProUGUI m_fencedStableCount;

	// Token: 0x04000583 RID: 1411
	public TextMeshProUGUI m_fencedStableValue;

	// Token: 0x04000584 RID: 1412
	public TextMeshProUGUI m_familyCount;

	// Token: 0x04000585 RID: 1413
	public TextMeshProUGUI m_familyValue;

	// Token: 0x04000586 RID: 1414
	public TextMeshProUGUI m_roomCount;

	// Token: 0x04000587 RID: 1415
	public TextMeshProUGUI m_roomValue;

	// Token: 0x04000588 RID: 1416
	public ColorByFaction m_colorizer;

	// Token: 0x04000589 RID: 1417
	private AgricolaGame m_gameController;

	// Token: 0x0400058A RID: 1418
	private int m_displayedPlayerIndex = -1;

	// Token: 0x0400058B RID: 1419
	private bool m_bInit;
}
