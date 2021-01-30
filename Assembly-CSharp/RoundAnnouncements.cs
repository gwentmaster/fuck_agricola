using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000072 RID: 114
public class RoundAnnouncements : PopupBase
{
	// Token: 0x060005C6 RID: 1478 RVA: 0x0002CAA0 File Offset: 0x0002ACA0
	public void ShowRoundAnnounce(uint roundNumber, string startingPlayerName, int factionIndex, AgricolaCardManager cardManager, int newActionCardInstanceID)
	{
		this.m_roundAnnounceRoot.SetActive(true);
		this.m_harvestAnnounceRoot.SetActive(false);
		this.m_bIgnoreDone = true;
		if (this.m_inputBlocker != null)
		{
			this.m_inputBlocker.SetActive(true);
		}
		if (roundNumber == 1U && AgricolaLib.GetGamePlayerCount() == 1 && !AgricolaLib.GetIsTutorialGame())
		{
			roundNumber = 0U;
			GameParameters gameParameters = (GameParameters)Marshal.PtrToStructure(AgricolaLib.GetGameParameters(), typeof(GameParameters));
			this.m_soloGameCountText.text = gameParameters.soloGameCount.ToString();
			string str = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_CurrentReq}");
			this.m_soloGameRequirementText.text = str + " " + AgricolaLib.GetSoloSeriesPointRequirement((uint)gameParameters.soloGameCount).ToString();
			this.m_roundAnimator.SetInteger("Faction", factionIndex);
		}
		else
		{
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(RoundAnnouncements.k_roundStrings[(int)(roundNumber - 1U)]);
			this.m_stageRoundText.text = text;
			int num = 0;
			switch (roundNumber)
			{
			case 1U:
				num = 0;
				break;
			case 2U:
			case 5U:
				num = 1;
				break;
			case 3U:
			case 6U:
			case 8U:
			case 10U:
			case 12U:
				num = 2;
				break;
			case 4U:
			case 7U:
			case 9U:
			case 11U:
			case 13U:
			case 14U:
				num = 3;
				break;
			}
			string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(RoundAnnouncements.k_harvestStrings[num]);
			this.m_harvestCountText.text = text2;
			this.m_startingPlayerText.text = startingPlayerName;
		}
		if (this.m_cardLocator != null && cardManager != null && newActionCardInstanceID != 0)
		{
			this.m_actionCard = cardManager.CreateTemporaryCardFromInstanceID(newActionCardInstanceID);
			if (this.m_actionCard != null)
			{
				this.m_actionCard.SetActive(true);
				AnimateObject component = this.m_actionCard.GetComponent<AnimateObject>();
				if (component != null)
				{
					this.m_cardLocator.PlaceAnimateObject(component, true, true, false);
					this.m_actionCard.transform.localPosition = Vector3.zero;
					base.StartCoroutine(this.DelayResetPos(this.m_actionCard));
				}
			}
			this.m_roundAnimator.SetBool("ShowCard", true);
			this.m_roundAnnounceCardRoot.SetActive(true);
		}
		else
		{
			this.m_roundAnimator.SetBool("ShowCard", false);
		}
		if (roundNumber == 10U || roundNumber == 12U)
		{
			roundNumber = 8U;
		}
		else if (roundNumber == 11U || roundNumber == 13U)
		{
			roundNumber = 9U;
		}
		this.m_roundAnimator.SetInteger("Round", (int)roundNumber);
		this.m_roundAnimator.SetTrigger("Execute");
		base.StartCoroutine(this.EndIgnore());
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0002CD44 File Offset: 0x0002AF44
	public void StartHarvestAnnounce(int harvestPhase)
	{
		this.m_roundAnnounceRoot.SetActive(false);
		this.m_harvestAnnounceRoot.SetActive(true);
		this.m_bIgnoreDone = true;
		if (this.m_inputBlocker != null)
		{
			this.m_inputBlocker.SetActive(true);
		}
		this.m_harvestAnimator.SetInteger("HarvestPhase", harvestPhase);
		this.m_harvestAnimator.SetTrigger("Execute");
		base.StartCoroutine(this.EndIgnore());
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0002CDB8 File Offset: 0x0002AFB8
	public void ForceQuit(BaseEventData baseData)
	{
		if (((PointerEventData)baseData).clickCount != 2)
		{
			return;
		}
		if (this.m_roundAnnounceRoot.activeSelf)
		{
			this.m_roundAnimator.SetTrigger("ForceQuit");
		}
		if (this.m_harvestAnnounceRoot.activeSelf)
		{
			this.m_harvestAnimator.SetTrigger("ForceQuit");
		}
		if (this.m_roundAnnounceCardRoot.activeSelf)
		{
			this.m_roundCardAnimator.SetTrigger("ForceQuit");
		}
		this.m_bIgnoreDone = false;
		this.Done();
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002CE38 File Offset: 0x0002B038
	public void OnButtonPress()
	{
		if (this.m_delayedPressTrigger != null)
		{
			if (this.m_roundAnnounceRoot.activeSelf)
			{
				this.m_roundAnimator.SetTrigger("ForceQuit");
			}
			if (this.m_harvestAnnounceRoot.activeSelf)
			{
				this.m_harvestAnimator.SetTrigger("ForceQuit");
			}
			if (this.m_roundAnnounceCardRoot.activeSelf)
			{
				this.m_roundCardAnimator.SetTrigger("ForceQuit");
			}
			this.m_bIgnoreDone = false;
			this.Done();
			this.m_delayedPressTrigger = null;
			return;
		}
		this.m_delayedPressTrigger = base.StartCoroutine(this.DelayedPressTrigger());
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0002CECB File Offset: 0x0002B0CB
	public void Done()
	{
		if (this.m_bIgnoreDone)
		{
			return;
		}
		this.m_bIgnoreDone = true;
		base.StartCoroutine(this.Exit());
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0002CEEA File Offset: 0x0002B0EA
	private IEnumerator DelayResetPos(GameObject obj)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (obj != null)
		{
			obj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}
		yield break;
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0002CEF9 File Offset: 0x0002B0F9
	private IEnumerator Exit()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (this.m_inputBlocker != null)
		{
			this.m_inputBlocker.SetActive(false);
		}
		if (this.m_actionCard != null)
		{
			UnityEngine.Object.Destroy(this.m_actionCard);
			this.m_actionCard = null;
		}
		this.m_roundAnnounceRoot.SetActive(false);
		this.m_harvestAnnounceRoot.SetActive(false);
		this.m_roundAnnounceCardRoot.SetActive(false);
		this.m_PopupManager.SetPopup(EPopups.NONE);
		yield break;
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0002CF08 File Offset: 0x0002B108
	private IEnumerator EndIgnore()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		this.m_bIgnoreDone = false;
		yield break;
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0002CF17 File Offset: 0x0002B117
	private IEnumerator DelayedPressTrigger()
	{
		yield return new WaitForSeconds(this.m_doubleClickThreshold);
		this.m_delayedPressTrigger = null;
		yield break;
	}

	// Token: 0x0400059B RID: 1435
	public GameObject m_roundAnnounceRoot;

	// Token: 0x0400059C RID: 1436
	public GameObject m_roundAnnounceCardRoot;

	// Token: 0x0400059D RID: 1437
	public TextMeshPro m_stageRoundText;

	// Token: 0x0400059E RID: 1438
	public TextMeshPro m_harvestCountText;

	// Token: 0x0400059F RID: 1439
	public TextMeshPro m_startingPlayerText;

	// Token: 0x040005A0 RID: 1440
	public TextMeshPro m_soloGameCountText;

	// Token: 0x040005A1 RID: 1441
	public TextMeshPro m_soloGameRequirementText;

	// Token: 0x040005A2 RID: 1442
	public Animator m_roundAnimator;

	// Token: 0x040005A3 RID: 1443
	public Animator m_roundCardAnimator;

	// Token: 0x040005A4 RID: 1444
	public AgricolaAnimationLocator m_cardLocator;

	// Token: 0x040005A5 RID: 1445
	public GameObject m_harvestAnnounceRoot;

	// Token: 0x040005A6 RID: 1446
	public Animator m_harvestAnimator;

	// Token: 0x040005A7 RID: 1447
	public GameObject m_inputBlocker;

	// Token: 0x040005A8 RID: 1448
	public float m_doubleClickThreshold = 0.7f;

	// Token: 0x040005A9 RID: 1449
	private Coroutine m_delayedPressTrigger;

	// Token: 0x040005AA RID: 1450
	private GameObject m_actionCard;

	// Token: 0x040005AB RID: 1451
	private bool m_bIgnoreDone = true;

	// Token: 0x040005AC RID: 1452
	private static readonly string[] k_roundStrings = new string[]
	{
		"${Key_Stage1Round1}",
		"${Key_Stage1Round2}",
		"${Key_Stage1Round3}",
		"${Key_Stage1Round4}",
		"${Key_Stage2Round5}",
		"${Key_Stage2Round6}",
		"${Key_Stage2Round7}",
		"${Key_Stage3Round8}",
		"${Key_Stage3Round9}",
		"${Key_Stage4Round10}",
		"${Key_Stage4Round11}",
		"${Key_Stage5Round12}",
		"${Key_Stage5Round13}",
		"${Key_Stage6Round14}"
	};

	// Token: 0x040005AD RID: 1453
	private static readonly string[] k_harvestStrings = new string[]
	{
		"${Key_RoundHarvest4}",
		"${Key_RoundHarvest3}",
		"${Key_RoundHarvest2}",
		"${Key_RoundHarvest1}"
	};
}
