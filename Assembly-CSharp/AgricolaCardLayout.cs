using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000034 RID: 52
public class AgricolaCardLayout : MonoBehaviour
{
	// Token: 0x06000275 RID: 629 RVA: 0x0000E43E File Offset: 0x0000C63E
	public GameObject GetMajImpHHShadeObj()
	{
		return this.m_MajImpHHShade;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000E446 File Offset: 0x0000C646
	public GameObject GetMajImpHHShaTokenLocator()
	{
		return this.m_MajImpHHTokenLocator;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000E44E File Offset: 0x0000C64E
	public void SetCardImage(Sprite cardImage)
	{
		if (this.m_CardArt != null)
		{
			this.m_CardArt.sprite = cardImage;
		}
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000E46C File Offset: 0x0000C66C
	public void SetCardDeckIcon(Sprite deckIcon)
	{
		if (this.m_CardDeckIcon != null)
		{
			if (deckIcon != null)
			{
				this.m_CardDeckIcon.sprite = deckIcon;
				this.m_CardDeckIcon.gameObject.SetActive(true);
				return;
			}
			this.m_CardDeckIcon.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
	public void SetCardPlayerCountIcon(Sprite playerCountIcon)
	{
		if (this.m_CardPlayerCountIcon != null)
		{
			if (playerCountIcon != null)
			{
				this.m_CardPlayerCountIcon.sprite = playerCountIcon;
				this.m_CardPlayerCountIcon.gameObject.SetActive(true);
				return;
			}
			this.m_CardPlayerCountIcon.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000E514 File Offset: 0x0000C714
	public void SetCardCategoryIcon(Sprite categoryIcon)
	{
		if (this.m_CardCategoryIcon != null)
		{
			if (categoryIcon != null)
			{
				this.m_CardCategoryIcon.sprite = categoryIcon;
				this.m_CardCategoryIcon.gameObject.SetActive(true);
				return;
			}
			this.m_CardCategoryIcon.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000E567 File Offset: 0x0000C767
	public void SetCardBonusPointIcon(bool bBonusPointIcon)
	{
		if (this.m_CardBonusPointIcon != null)
		{
			this.m_CardBonusPointIcon.SetActive(bBonusPointIcon);
		}
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000E584 File Offset: 0x0000C784
	public void SetVictoryPointCount(int victoryPointCount, Sprite victoryPointIcon)
	{
		if (this.m_VictoryPointsRoot != null)
		{
			this.m_VictoryPointsRoot.SetActive(victoryPointCount != 0);
		}
		if (this.m_VictoryPointsIcon != null)
		{
			this.m_VictoryPointsIcon.sprite = victoryPointIcon;
		}
		if (this.m_VictoryPointsCountText != null)
		{
			this.m_VictoryPointsCountText.SetText(victoryPointCount.ToString());
		}
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
	public void SetCardFramePassArrows(bool bShowArrows)
	{
		if (this.m_CardFrameArrows != null)
		{
			this.m_CardFrameArrows.SetActive(bShowArrows);
		}
		if (this.m_CardFrameLeaves != null)
		{
			this.m_CardFrameLeaves.SetActive(!bShowArrows);
		}
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000E621 File Offset: 0x0000C821
	public void SetCardPassText(bool bShowText)
	{
		if (this.m_CardPassText != null)
		{
			this.m_CardPassText.SetActive(bShowText);
		}
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000E63D File Offset: 0x0000C83D
	public void HideResourceCost()
	{
		if (this.m_ResourceCostRoot != null)
		{
			this.m_ResourceCostRoot.SetActive(false);
		}
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0000E65C File Offset: 0x0000C85C
	public void ShowResourceCost(byte[] resourceCost)
	{
		if (this.m_ResourceCostRoot != null)
		{
			this.m_ResourceCostRoot.SetActive(true);
		}
		if (this.m_ResourceCostEntries != null)
		{
			for (int i = 0; i < this.m_ResourceCostEntries.Length; i++)
			{
				int num = 0;
				if (resourceCost != null && i < resourceCost.Length)
				{
					num = (int)resourceCost[i];
				}
				if (this.m_ResourceCostEntries[i].m_ResourceRoot != null)
				{
					this.m_ResourceCostEntries[i].m_ResourceRoot.SetActive(num > 0);
				}
				if (this.m_ResourceCostEntries[i].m_ResourceValue != null)
				{
					this.m_ResourceCostEntries[i].m_ResourceValue.SetText(num.ToString());
				}
			}
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000E71C File Offset: 0x0000C91C
	public void SetCardNumberText(string card_number)
	{
		if (this.m_CardNumberText != null)
		{
			this.m_CardNumberText.SetText(card_number);
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000E738 File Offset: 0x0000C938
	public void SetCardTitle(string card_title)
	{
		if (this.m_CardTextName != null)
		{
			this.m_CardTextName.SetText(card_title);
		}
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000E754 File Offset: 0x0000C954
	public void SetCardTextEffect(string card_effect_text)
	{
		if (this.m_CardTextEffect != null)
		{
			this.m_CardTextEffect.SetText(card_effect_text);
		}
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000E770 File Offset: 0x0000C970
	public void SetCardTextRequirement(string card_requirement_text)
	{
		if (this.m_CardRequirementRoot != null)
		{
			this.m_CardRequirementRoot.SetActive(!string.IsNullOrEmpty(card_requirement_text));
		}
		if (this.m_CardRequirementText != null)
		{
			this.m_CardRequirementText.SetText(card_requirement_text);
		}
	}

	// Token: 0x040001CD RID: 461
	[SerializeField]
	private Image m_CardArt;

	// Token: 0x040001CE RID: 462
	[SerializeField]
	private Image m_CardDeckIcon;

	// Token: 0x040001CF RID: 463
	[SerializeField]
	private Image m_CardPlayerCountIcon;

	// Token: 0x040001D0 RID: 464
	[SerializeField]
	private Image m_CardCategoryIcon;

	// Token: 0x040001D1 RID: 465
	[SerializeField]
	private GameObject m_CardBonusPointIcon;

	// Token: 0x040001D2 RID: 466
	[SerializeField]
	private GameObject m_VictoryPointsRoot;

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private Image m_VictoryPointsIcon;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	private TextMeshProUGUI m_VictoryPointsCountText;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	private GameObject m_CardFrameArrows;

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	private GameObject m_CardFrameLeaves;

	// Token: 0x040001D7 RID: 471
	[SerializeField]
	private GameObject m_CardPassText;

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	private GameObject m_ResourceCostRoot;

	// Token: 0x040001D9 RID: 473
	[SerializeField]
	private AgricolaCardLayout.ResourceCostEntry[] m_ResourceCostEntries;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	private TextMeshProUGUI m_CardNumberText;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	private TextMeshProUGUI m_CardTextName;

	// Token: 0x040001DC RID: 476
	[SerializeField]
	private TextMeshProUGUI m_CardTextEffect;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	private GameObject m_CardRequirementRoot;

	// Token: 0x040001DE RID: 478
	[SerializeField]
	private TextMeshProUGUI m_CardRequirementText;

	// Token: 0x040001DF RID: 479
	[SerializeField]
	private GameObject m_MajImpHHShade;

	// Token: 0x040001E0 RID: 480
	[SerializeField]
	private GameObject m_MajImpHHTokenLocator;

	// Token: 0x02000759 RID: 1881
	[Serializable]
	public struct ResourceCostEntry
	{
		// Token: 0x04002B63 RID: 11107
		public GameObject m_ResourceRoot;

		// Token: 0x04002B64 RID: 11108
		public TextMeshProUGUI m_ResourceValue;
	}
}
