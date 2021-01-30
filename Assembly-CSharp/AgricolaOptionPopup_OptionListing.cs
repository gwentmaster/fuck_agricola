using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000047 RID: 71
public class AgricolaOptionPopup_OptionListing : MonoBehaviour
{
	// Token: 0x06000406 RID: 1030 RVA: 0x000205E7 File Offset: 0x0001E7E7
	public bool GetIsWideMode()
	{
		return this.m_bIsWide;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x000205EF File Offset: 0x0001E7EF
	public void AddResourceTogglePassThrough(AgricolaOptionPopup_OptionListing.OnResourcePassThrough passthrough)
	{
		this.m_passthrough = (AgricolaOptionPopup_OptionListing.OnResourcePassThrough)Delegate.Combine(this.m_passthrough, passthrough);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x00020608 File Offset: 0x0001E808
	public void RemoveResourceTogglePassThrough(AgricolaOptionPopup_OptionListing.OnResourcePassThrough passthrough)
	{
		this.m_passthrough = (AgricolaOptionPopup_OptionListing.OnResourcePassThrough)Delegate.Remove(this.m_passthrough, passthrough);
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00020621 File Offset: 0x0001E821
	public void AddToggleChangeCallback(AgricolaOptionPopup_OptionListing.OnCellCallback callback)
	{
		this.m_callback = (AgricolaOptionPopup_OptionListing.OnCellCallback)Delegate.Combine(this.m_callback, callback);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0002063A File Offset: 0x0001E83A
	public void RemoveToggleChangeCallback(AgricolaOptionPopup_OptionListing.OnCellCallback callback)
	{
		this.m_callback = (AgricolaOptionPopup_OptionListing.OnCellCallback)Delegate.Remove(this.m_callback, callback);
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00020653 File Offset: 0x0001E853
	public void Start()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0002068C File Offset: 0x0001E88C
	public void Reset()
	{
		if (!this.m_bInit)
		{
			this.m_bInit = true;
			this.m_resources = new AgricolaOptionPopup_Resource[4];
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_resourcePrefab);
				gameObject.transform.SetParent(this.m_resourceContainer.transform);
				gameObject.transform.localScale = Vector3.one;
				this.m_resources[i] = gameObject.GetComponent<AgricolaOptionPopup_Resource>();
				this.m_resources[i].AddToggleChangeCallback(new AgricolaOptionPopup_Resource.ToggleChangeCallback(this.ResourceToggleCallback));
			}
			this.m_OpponentTokens = new AgricolaOptionPopup_Resource[5];
			for (int j = 0; j < 5; j++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_oppAndResPrefab);
				gameObject2.transform.SetParent(this.m_oppAndResContainer.transform);
				gameObject2.transform.localScale = Vector3.one;
				this.m_OpponentTokens[j] = gameObject2.GetComponent<AgricolaOptionPopup_Resource>();
			}
		}
		this.SetRoomType(0);
		this.m_affectingCards.Clear();
		this.SetOpponentDisplaySecondRow(false);
		for (int k = 0; k < this.m_affectingCardObjs.Count; k++)
		{
			if (this.m_affectingCardObjs[k] != null)
			{
				UnityEngine.Object.Destroy(this.m_affectingCardObjs[k]);
			}
		}
		this.m_affectingCardObjs.Clear();
		if (this.m_pPurchaseListDivider != null)
		{
			this.m_pPurchaseListDivider.SetActive(false);
		}
		if (this.m_pPurchaseListDividerWide != null)
		{
			this.m_pPurchaseListDividerWide.SetActive(false);
		}
		this.m_optionToggle.isOn = false;
		this.m_pPurchaseListReturnCardTextNode.gameObject.SetActive(false);
		this.m_pOptionRootNode.SetActive(false);
		this.m_oppTokenRoot.SetActive(false);
		this.m_pPurchaseListRootNode.SetActive(false);
		this.m_pPurchaseListArrow.SetActive(false);
		this.m_purchaseListGrayout.SetActive(false);
		this.m_purchaseListToggle.interactable = true;
		this.m_purchaseListToggleImage.enabled = true;
		this.m_purchaseListToggle.isOn = false;
		this.SetToggleGroupOn(true);
		this.m_bShowArrows = false;
		this.m_countLeft = 0U;
		this.m_countRight = 0U;
		this.m_resourceCache.Clear();
		this.m_resourceCache2.Clear();
		this.m_bUseResourcePressme = false;
		this.m_bIsActive = false;
		this.m_bIsActive2 = false;
		this.m_mainResourceEntry = -1;
		this.m_linkedEntries.Clear();
		if (this.m_LeftCardObj != null)
		{
			UnityEngine.Object.Destroy(this.m_LeftCardObj);
			this.m_LeftCardObj = null;
		}
		for (int l = 0; l < this.m_resources.Length; l++)
		{
			this.m_resources[l].SetActive(false);
			this.m_resources[l].SetArrows(false, false);
			this.m_resources[l].SetIsSelected(false);
			this.m_resources[l].SetInteractable(false);
		}
		for (int m = 0; m < this.m_OpponentTokens.Length; m++)
		{
			this.m_OpponentTokens[m].SetActive(false);
			this.m_OpponentTokens[m].SetIsSelected(false);
			this.m_OpponentTokens[m].SetInteractable(false);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x000209A8 File Offset: 0x0001EBA8
	public GameObject GetLeftCard()
	{
		return this.m_LeftCardObj;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x000209B0 File Offset: 0x0001EBB0
	public void ShowLeftCard(GameObject cardObj)
	{
		if (this.m_pResourceRenovateRootNode == null || this.m_pResourceRenovateWoodNode == null || this.m_pResourceRenovateClayNode == null || this.m_pResourceRenovateStoneNode == null)
		{
			return;
		}
		if (cardObj == null || this.m_LeftCardLocator == null)
		{
			return;
		}
		this.m_pResourceRenovateWoodNode.SetActive(false);
		this.m_pResourceRenovateClayNode.SetActive(false);
		this.m_pResourceRenovateStoneNode.SetActive(false);
		this.m_pResourceRenovateRootNode.SetActive(true);
		this.m_LeftCardObj = cardObj;
		this.m_LeftCardObj.SetActive(true);
		AnimateObject component = cardObj.GetComponent<AnimateObject>();
		if (component != null)
		{
			this.m_LeftCardLocator.PlaceAnimateObject(component, true, true, false);
		}
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00020A70 File Offset: 0x0001EC70
	public void GenerateCardsFromIDs(AgricolaCardManager cardManager, AgricolaCardInPlayManager cardInPlayManager)
	{
		if (this.m_affectingCards.Count > 0 && this.m_affectingCardObjs.Count != this.m_affectingCards.Count)
		{
			for (int i = 0; i < this.m_affectingCardObjs.Count; i++)
			{
				if (this.m_affectingCardObjs[i] != null)
				{
					UnityEngine.Object.Destroy(this.m_affectingCardObjs[i]);
				}
			}
			this.m_affectingCardObjs.Clear();
			for (int j = 0; j < this.m_affectingCards.Count; j++)
			{
				GameObject gameObject = null;
				if (cardManager != null)
				{
					gameObject = cardManager.CreateTemporaryCardFromInstanceID((int)this.m_affectingCards[j]);
				}
				if (gameObject == null && cardInPlayManager != null)
				{
					GameObject gameObject2 = cardInPlayManager.CreateCardInPlayFromInstanceID((int)this.m_affectingCards[j]);
					if (gameObject2 != null)
					{
						AgricolaCardInPlay component = gameObject2.GetComponent<AgricolaCardInPlay>();
						if (component != null && component.GetSourceCard() != null && cardManager != null)
						{
							gameObject = cardManager.CreateTemporaryCardFromInstanceID(component.GetSourceCard().GetCardInstanceID());
						}
					}
				}
				if (gameObject != null)
				{
					this.m_affectingCardObjs.Add(gameObject);
					cardManager.PlaceCardInCardLimbo(gameObject.GetComponent<AgricolaCard>());
				}
			}
		}
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00020BB8 File Offset: 0x0001EDB8
	public bool SetOpponentDisplaySecondRow(bool bOn)
	{
		this.m_bIsWide = bOn;
		if (base.gameObject.activeInHierarchy)
		{
			if (this.m_wideModeAnimator != null)
			{
				this.m_wideModeAnimator.SetBool("bIsWide", bOn);
				HorizontalLayoutGroup component = this.m_oppAndResContainer.GetComponent<HorizontalLayoutGroup>();
				component.CalculateLayoutInputHorizontal();
				component.CalculateLayoutInputVertical();
				component.SetLayoutHorizontal();
				component.SetLayoutVertical();
				return true;
			}
		}
		else
		{
			this.m_bHoldingForAnimator = true;
		}
		return false;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00020C24 File Offset: 0x0001EE24
	private void Update()
	{
		if (this.m_bHoldingForAnimator)
		{
			this.m_bHoldingForAnimator = !this.SetOpponentDisplaySecondRow(this.m_bIsWide);
		}
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00020C43 File Offset: 0x0001EE43
	public void SetOpponentAreaVisible(bool bOn)
	{
		if (this.m_oppTokenRoot != null)
		{
			this.m_oppTokenRoot.SetActive(bOn);
		}
		HorizontalLayoutGroup component = this.m_oppAndResContainer.GetComponent<HorizontalLayoutGroup>();
		component.CalculateLayoutInputHorizontal();
		component.CalculateLayoutInputVertical();
		component.SetLayoutHorizontal();
		component.SetLayoutVertical();
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00020C81 File Offset: 0x0001EE81
	public void SetToggleGroup(ToggleGroup group)
	{
		this.m_toggleGroup = group;
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00020C8A File Offset: 0x0001EE8A
	public void SetToggleGroupOn(bool bOn)
	{
		this.m_optionToggle.group = (bOn ? this.m_toggleGroup : null);
		this.m_purchaseListToggle.group = (bOn ? this.m_toggleGroup : null);
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00020CBC File Offset: 0x0001EEBC
	public void SetRoomType(int resourceType)
	{
		if (this.m_pResourceRenovateRootNode == null || this.m_pResourceRenovateWoodNode == null || this.m_pResourceRenovateClayNode == null || this.m_pResourceRenovateStoneNode == null)
		{
			return;
		}
		this.m_pResourceRenovateWoodNode.SetActive(resourceType == 1);
		this.m_pResourceRenovateClayNode.SetActive(resourceType == 2);
		this.m_pResourceRenovateStoneNode.SetActive(resourceType == 3);
		this.m_pResourceRenovateRootNode.SetActive(this.m_pResourceRenovateWoodNode.activeSelf || this.m_pResourceRenovateClayNode.activeSelf || this.m_pResourceRenovateStoneNode.activeSelf);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00020D62 File Offset: 0x0001EF62
	public void OnCellToggle(bool bIsOn)
	{
		if (this.m_bIgnoreToggle)
		{
			return;
		}
		this.m_bIgnoreToggle = true;
		if (this.m_callback != null)
		{
			this.m_callback(this, bIsOn);
		}
		this.m_bIgnoreToggle = false;
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00020D90 File Offset: 0x0001EF90
	public void ResourceToggleCallback(AgricolaOptionPopup_Resource res, bool bIsOn)
	{
		if (this.m_bIgnoreToggle)
		{
			return;
		}
		this.m_bIgnoreToggle = true;
		if (this.m_passthrough != null)
		{
			int num = 0;
			while (num < this.m_resources.Length && !(res == this.m_resources[num]))
			{
				num++;
			}
			this.m_passthrough(res, this, num, bIsOn);
		}
		this.m_bIgnoreToggle = false;
	}

	// Token: 0x0400036C RID: 876
	public GameObject m_resourcePrefab;

	// Token: 0x0400036D RID: 877
	public GameObject m_oppAndResPrefab;

	// Token: 0x0400036E RID: 878
	public Animator m_wideModeAnimator;

	// Token: 0x0400036F RID: 879
	public GameObject m_pOptionRootNode;

	// Token: 0x04000370 RID: 880
	public TextMeshProUGUI m_pOptionTextNode;

	// Token: 0x04000371 RID: 881
	public Toggle m_optionToggle;

	// Token: 0x04000372 RID: 882
	public GameObject m_oppTokenRoot;

	// Token: 0x04000373 RID: 883
	[HideInInspector]
	public bool m_resourceOverride;

	// Token: 0x04000374 RID: 884
	public GameObject m_resourceContainer;

	// Token: 0x04000375 RID: 885
	public GameObject m_oppAndResContainer;

	// Token: 0x04000376 RID: 886
	public GameObject m_pPurchaseListRootNode;

	// Token: 0x04000377 RID: 887
	public GameObject m_pPurchaseListDivider;

	// Token: 0x04000378 RID: 888
	public GameObject m_pPurchaseListDividerWide;

	// Token: 0x04000379 RID: 889
	public Toggle m_purchaseListToggle;

	// Token: 0x0400037A RID: 890
	public Image m_purchaseListToggleImage;

	// Token: 0x0400037B RID: 891
	public GameObject m_pPurchaseListArrow;

	// Token: 0x0400037C RID: 892
	public GameObject m_purchaseListGrayout;

	// Token: 0x0400037D RID: 893
	public TextMeshProUGUI m_pPurchaseListReturnCardTextNode;

	// Token: 0x0400037E RID: 894
	[HideInInspector]
	public List<uint> m_affectingCards = new List<uint>();

	// Token: 0x0400037F RID: 895
	[HideInInspector]
	public List<GameObject> m_affectingCardObjs = new List<GameObject>();

	// Token: 0x04000380 RID: 896
	public GameObject m_pResourceRenovateRootNode;

	// Token: 0x04000381 RID: 897
	public AgricolaAnimationLocator m_LeftCardLocator;

	// Token: 0x04000382 RID: 898
	public GameObject m_pResourceRenovateWoodNode;

	// Token: 0x04000383 RID: 899
	public GameObject m_pResourceRenovateClayNode;

	// Token: 0x04000384 RID: 900
	public GameObject m_pResourceRenovateStoneNode;

	// Token: 0x04000385 RID: 901
	[HideInInspector]
	public string m_largeTextBuffer;

	// Token: 0x04000386 RID: 902
	[HideInInspector]
	public int m_mainResourceEntry;

	// Token: 0x04000387 RID: 903
	[HideInInspector]
	public List<int> m_linkedEntries = new List<int>();

	// Token: 0x04000388 RID: 904
	[HideInInspector]
	public ushort m_flagIndex;

	// Token: 0x04000389 RID: 905
	[HideInInspector]
	public bool m_bIsActive;

	// Token: 0x0400038A RID: 906
	[HideInInspector]
	public bool m_bIsInteractable;

	// Token: 0x0400038B RID: 907
	[HideInInspector]
	public bool m_bNoNegativeCost;

	// Token: 0x0400038C RID: 908
	[HideInInspector]
	public bool m_bTokenOnFirstRow;

	// Token: 0x0400038D RID: 909
	[HideInInspector]
	public bool m_bTokenHasQuestionMark;

	// Token: 0x0400038E RID: 910
	[HideInInspector]
	public CResourceCache m_resourceCache = new CResourceCache();

	// Token: 0x0400038F RID: 911
	[HideInInspector]
	public CResourceCache m_resourceCache2 = new CResourceCache();

	// Token: 0x04000390 RID: 912
	[HideInInspector]
	public bool m_bUseResourcePressme;

	// Token: 0x04000391 RID: 913
	[HideInInspector]
	public bool m_bIsActive2;

	// Token: 0x04000392 RID: 914
	[HideInInspector]
	public bool m_bShowArrows;

	// Token: 0x04000393 RID: 915
	[HideInInspector]
	public bool[] m_bLeftArrowSelected;

	// Token: 0x04000394 RID: 916
	[HideInInspector]
	public bool[] m_bRightArrowSelected;

	// Token: 0x04000395 RID: 917
	[HideInInspector]
	public bool[] m_bLeftArrowGrayed;

	// Token: 0x04000396 RID: 918
	[HideInInspector]
	public bool[] m_bRightArrowGrayed;

	// Token: 0x04000397 RID: 919
	[HideInInspector]
	public uint m_countLeft;

	// Token: 0x04000398 RID: 920
	[HideInInspector]
	public uint m_countRight;

	// Token: 0x04000399 RID: 921
	[HideInInspector]
	public int m_PlayerTurnOrderIndex;

	// Token: 0x0400039A RID: 922
	[HideInInspector]
	public ushort instanceID;

	// Token: 0x0400039B RID: 923
	[HideInInspector]
	public ushort hint;

	// Token: 0x0400039C RID: 924
	[HideInInspector]
	public int index;

	// Token: 0x0400039D RID: 925
	[HideInInspector]
	public uint m_numResourseTypes;

	// Token: 0x0400039E RID: 926
	[HideInInspector]
	public int[] m_resourceTypes;

	// Token: 0x0400039F RID: 927
	public TextMeshProUGUI[] m_textBuffer;

	// Token: 0x040003A0 RID: 928
	[HideInInspector]
	public AgricolaOptionPopup_Resource[] m_resources;

	// Token: 0x040003A1 RID: 929
	[HideInInspector]
	public AgricolaOptionPopup_Resource[] m_OpponentTokens = new AgricolaOptionPopup_Resource[5];

	// Token: 0x040003A2 RID: 930
	private AgricolaOptionPopup_OptionListing.OnResourcePassThrough m_passthrough;

	// Token: 0x040003A3 RID: 931
	private AgricolaOptionPopup_OptionListing.OnCellCallback m_callback;

	// Token: 0x040003A4 RID: 932
	private ToggleGroup m_toggleGroup;

	// Token: 0x040003A5 RID: 933
	private GameObject m_LeftCardObj;

	// Token: 0x040003A6 RID: 934
	private bool m_bIgnoreToggle;

	// Token: 0x040003A7 RID: 935
	private bool m_bInit;

	// Token: 0x040003A8 RID: 936
	private bool m_bHoldingForAnimator;

	// Token: 0x040003A9 RID: 937
	private bool m_bIsWide;

	// Token: 0x02000761 RID: 1889
	// (Invoke) Token: 0x060041BD RID: 16829
	public delegate void OnResourcePassThrough(AgricolaOptionPopup_Resource res, AgricolaOptionPopup_OptionListing cell, int index, bool bIsOn);

	// Token: 0x02000762 RID: 1890
	// (Invoke) Token: 0x060041C1 RID: 16833
	public delegate void OnCellCallback(AgricolaOptionPopup_OptionListing cell, bool bIsOn);
}
