using System;
using System.Runtime.InteropServices;
using GameData;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class AgricolaCardManager : CardManager
{
	// Token: 0x06000286 RID: 646 RVA: 0x0000E7AE File Offset: 0x0000C9AE
	public float GetDragCardScale()
	{
		return this.m_DragCardScale;
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000E7B6 File Offset: 0x0000C9B6
	public float GetDragCardTargetScale()
	{
		return this.m_DragCardTargetScale;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0000E7BE File Offset: 0x0000C9BE
	public AgricolaAnimationLocator GetIconMagnifyLocator()
	{
		return this.m_CardIconMagnfyLocator;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
	private void Awake()
	{
		GameObject gameObject = GameObject.Find("/ResourceManager - Startup");
		if (gameObject != null)
		{
			this.m_ResourceManager = gameObject.GetComponent<ResourceManager>();
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
	private GameObject FindCardPrefabByName(string prefabName)
	{
		if (this.m_PrefabBasicCards != null)
		{
			for (int i = 0; i < this.m_PrefabBasicCards.Length; i++)
			{
				if (this.m_PrefabBasicCards[i].name == prefabName)
				{
					return this.m_PrefabBasicCards[i];
				}
			}
		}
		if (this.m_PrefabSpecificCards != null)
		{
			for (int j = 0; j < this.m_PrefabSpecificCards.Length; j++)
			{
				if (this.m_PrefabSpecificCards[j].name == prefabName)
				{
					return this.m_PrefabSpecificCards[j];
				}
			}
		}
		return null;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000E878 File Offset: 0x0000CA78
	private ResourceEntry FindSpriteCardArtByName(string cardArtPathname)
	{
		if (this.m_ResourceManager != null)
		{
			ResourceEntry resourceEntry = this.m_ResourceManager.LoadResource<Sprite>(cardArtPathname);
			if (resourceEntry != null)
			{
				return resourceEntry;
			}
		}
		return null;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000E8A8 File Offset: 0x0000CAA8
	public string GetCardImagePath(short cardType, short cardDeck, short cardNumber)
	{
		string text = "Cards/";
		if (this.m_CardTypePath != null && cardType >= 0 && (int)cardType < this.m_CardTypePath.Length)
		{
			text = text + this.m_CardTypePath[(int)cardType] + "/";
		}
		if (this.m_CardDeckIds != null && cardDeck >= 0 && (int)cardDeck < this.m_CardDeckIds.Length)
		{
			text += string.Format("{0}{1:D3}", this.m_CardDeckIds[(int)cardDeck], cardNumber);
		}
		return text;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000E920 File Offset: 0x0000CB20
	public GameObject CreateCardFromName(string cardName, bool bAddToMasterList = true, bool bIgnoreCardBack = true)
	{
		GameObject result = null;
		GCHandle gchandle = GCHandle.Alloc(new byte[512], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		if (AgricolaLib.GetCardDataByName(cardName, intPtr, 512) != 0)
		{
			CardData card_info = (CardData)Marshal.PtrToStructure(intPtr, typeof(CardData));
			result = this.CreateCard(card_info, 0, bAddToMasterList);
		}
		gchandle.Free();
		return result;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000E980 File Offset: 0x0000CB80
	public GameObject CreateCardFromCompressedNumber(uint compressedCardNumber, bool bAddToMasterList = true, bool bIgnoreCardBack = true)
	{
		GameObject result = null;
		GCHandle gchandle = GCHandle.Alloc(new byte[512], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		if (AgricolaLib.GetCardDataByCompressedNumber(compressedCardNumber, intPtr, 512) != 0)
		{
			CardData card_info = (CardData)Marshal.PtrToStructure(intPtr, typeof(CardData));
			result = this.CreateCard(card_info, 0, bAddToMasterList);
		}
		gchandle.Free();
		return result;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
	public override GameObject CreateCardFromInstanceID(int instanceID, bool bAddToMasterList = true)
	{
		GameObject result = null;
		GCHandle gchandle = GCHandle.Alloc(new byte[512], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		bool flag = (instanceID & AgricolaCardManager.s_MajImpDisplayCardMask) != 0;
		AgricolaLib.GetInstanceData(7, flag ? (instanceID & ~AgricolaCardManager.s_MajImpDisplayCardMask) : instanceID, intPtr, 512);
		CardData cardData = (CardData)Marshal.PtrToStructure(intPtr, typeof(CardData));
		if ((!flag && (int)cardData.card_instance_id == instanceID) || (flag && (int)cardData.card_instance_id == (instanceID & ~AgricolaCardManager.s_MajImpDisplayCardMask)))
		{
			result = this.CreateCard(cardData, instanceID, bAddToMasterList);
		}
		gchandle.Free();
		return result;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000EA7C File Offset: 0x0000CC7C
	private GameObject CreateCard(CardData card_info, int instanceID, bool bAddToMasterList = true)
	{
		GameObject gameObject = null;
		AgricolaCard agricolaCard = null;
		bool flag = (instanceID & AgricolaCardManager.s_MajImpDisplayCardMask) != 0;
		if ((!flag && (int)card_info.card_instance_id == instanceID) || (flag && (int)card_info.card_instance_id == (instanceID & ~AgricolaCardManager.s_MajImpDisplayCardMask)))
		{
			bool flag2 = true;
			GameObject gameObject2 = null;
			GameObject gameObject3 = null;
			GameObject gameObject4 = null;
			GameObject gameObject5 = null;
			GameObject gameObject6 = null;
			switch (card_info.card_type)
			{
			case 0:
				gameObject4 = this.m_PrefabGlow;
				gameObject5 = this.m_PrefabGlowHalf;
				break;
			case 1:
				if ((card_info.card_flags & 1) == 0)
				{
					gameObject2 = this.m_PrefabCardMinorImprovement;
					gameObject3 = this.m_PrefabCardMinorImprovementHalf;
					gameObject6 = this.m_PrefabCardMinorImprovementIcon;
				}
				gameObject4 = this.m_PrefabGlow;
				gameObject5 = this.m_PrefabGlowHalf;
				break;
			case 2:
				gameObject2 = this.m_PrefabCardOccupation;
				gameObject3 = this.m_PrefabCardOccupationHalf;
				gameObject6 = this.m_PrefabCardOccupationIcon;
				gameObject4 = this.m_PrefabGlow;
				gameObject5 = this.m_PrefabGlowHalf;
				break;
			case 3:
				gameObject2 = this.m_PrefabCardBegging;
				gameObject3 = this.m_PrefabCardBeggingHalf;
				gameObject4 = this.m_PrefabGlow;
				gameObject5 = this.m_PrefabGlowHalf;
				break;
			}
			bool flag3 = true;
			GameObject gameObject7 = this.FindCardPrefabByName(card_info.scene_name);
			GameObject gameObject8 = null;
			if (flag3)
			{
				gameObject8 = this.FindCardPrefabByName(card_info.scene_name + "_HH");
			}
			GameObject gameObject9 = this.FindCardPrefabByName(card_info.scene_name + "_SH");
			if (gameObject7 != null)
			{
				if (gameObject2 != null)
				{
					gameObject = AgricolaCard.CreateFromPrefab(gameObject2);
					if (gameObject != null)
					{
						agricolaCard = gameObject.GetComponent<AgricolaCard>();
						if (agricolaCard != null)
						{
							GameObject gameObject10 = gameObject7.transform.Find("CardBase").gameObject;
							agricolaCard.MergeCardFrontFullFromPrefab(gameObject10);
							if (gameObject8 != null)
							{
								agricolaCard.MergeCardFrontHalfFromPrefab(gameObject8);
							}
							if (gameObject9 != null)
							{
								agricolaCard.MergeCardFrontIconFromPrefab(gameObject9);
							}
						}
					}
				}
				if (gameObject == null)
				{
					flag2 = false;
					gameObject = AgricolaCard.CreateFromPrefab(gameObject7);
					agricolaCard = gameObject.GetComponent<AgricolaCard>();
					if (gameObject8 != null)
					{
						GameObject gameObject11 = gameObject8.gameObject;
						agricolaCard.MergeCardFrontHalfFromPrefab(gameObject11);
					}
					if (gameObject9 != null)
					{
						GameObject gameObject12 = gameObject9.gameObject;
						agricolaCard.MergeCardFrontIconFromPrefab(gameObject12);
					}
				}
			}
			else if (gameObject2 != null)
			{
				gameObject = AgricolaCard.CreateFromPrefab(gameObject2);
				if (gameObject != null)
				{
					agricolaCard = gameObject.GetComponent<AgricolaCard>();
					if (gameObject3 != null)
					{
						agricolaCard.MergeCardFrontHalfFromPrefab(gameObject3);
					}
					if (gameObject6 != null)
					{
						agricolaCard.MergeCardFrontIconFromPrefab(gameObject6);
					}
				}
			}
			if (gameObject != null)
			{
				gameObject.name = "Card: " + card_info.card_name;
				base.FinishCreateCard(gameObject, (int)card_info.card_instance_id);
				if (agricolaCard != null)
				{
					agricolaCard.SetCardName(card_info.card_name);
					agricolaCard.SetCardType((int)card_info.card_type, card_info.card_orchard_row);
					string cardTitle = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(card_info.card_title);
					agricolaCard.SetCardTitle(cardTitle);
					string cardTextEffect = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(card_info.card_effect_text);
					agricolaCard.SetCardTextEffect(cardTextEffect);
					string cardTextRequirement = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(card_info.card_requirement_text);
					agricolaCard.SetCardTextRequirement(cardTextRequirement);
					agricolaCard.SetCardDeckNumber((uint)card_info.card_deck);
					agricolaCard.SetCardNumber((uint)card_info.card_number);
					agricolaCard.SetCardUniqueIndex((int)card_info.unique_id);
					agricolaCard.SetCardOrchardSize(card_info.card_orchard_row);
					if (this.m_CardDeckIds != null && card_info.card_deck >= 0 && (int)card_info.card_deck < this.m_CardDeckIds.Length)
					{
						string cardNumberText = string.Format("{0}{1:D3}", this.m_CardDeckIds[(int)card_info.card_deck], card_info.card_number);
						agricolaCard.SetCardNumberText(cardNumberText);
					}
					string cardImagePath = this.GetCardImagePath(card_info.card_type, card_info.card_deck, card_info.card_number);
					if (flag2)
					{
						ResourceEntry resourceEntry = this.FindSpriteCardArtByName(cardImagePath);
						if (resourceEntry != null)
						{
							agricolaCard.SetCardImage(resourceEntry);
						}
						if (this.m_CardDeckIcons != null && card_info.card_deck >= 0 && (int)card_info.card_deck <= this.m_CardDeckIcons.Length)
						{
							agricolaCard.SetCardDeckIcon(this.m_CardDeckIcons[(int)card_info.card_deck]);
						}
						else
						{
							agricolaCard.SetCardDeckIcon(null);
						}
						if (this.m_CardPlayerCountIcons != null && card_info.card_player_count >= 0 && (int)card_info.card_player_count < this.m_CardPlayerCountIcons.Length)
						{
							agricolaCard.SetCardPlayerCountIcon(this.m_CardPlayerCountIcons[(int)card_info.card_player_count]);
						}
						else
						{
							agricolaCard.SetCardPlayerCountIcon(null);
						}
						if (this.m_CardCategoryIcons != null && card_info.card_category >= 0 && (int)card_info.card_category < this.m_CardCategoryIcons.Length)
						{
							agricolaCard.SetCardCategoryIcon(this.m_CardCategoryIcons[(int)card_info.card_category]);
						}
						else
						{
							agricolaCard.SetCardCategoryIcon(null);
						}
					}
					agricolaCard.SetCardBonusPointIcon((card_info.card_flags & 64) > 0);
					Sprite victoryPointIcon = null;
					if (card_info.card_victory_points > 0)
					{
						victoryPointIcon = this.m_CardVictoryPointsPositiveIcon;
					}
					else if (card_info.card_victory_points < 0)
					{
						victoryPointIcon = this.m_CardVictoryPointsNegativeIcon;
					}
					agricolaCard.SetVictoryPointCount((int)card_info.card_victory_points, victoryPointIcon);
					agricolaCard.SetCardFramePassArrows((card_info.card_flags & 16) > 0);
					agricolaCard.SetCardPassText((card_info.card_flags & 16) > 0);
					if ((card_info.card_flags & 32) == 0)
					{
						agricolaCard.HideResourceCost();
					}
					else
					{
						agricolaCard.ShowResourceCost(card_info.resource_cost);
					}
					if (gameObject4 != null)
					{
						agricolaCard.MergeCardGlowFullFromPrefab(gameObject4);
					}
					if (flag3 && gameObject5 != null)
					{
						agricolaCard.MergeCardGlowHalfFromPrefab(gameObject5);
					}
				}
				if (bAddToMasterList)
				{
					this.m_MasterCardList.Add(instanceID, gameObject);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
	public bool PlaceCardInCardLimbo(AgricolaCard card)
	{
		if (this.m_CardLimbo == null || card == null)
		{
			return false;
		}
		AnimateObject component = card.GetComponent<AnimateObject>();
		if (component == null)
		{
			return false;
		}
		this.m_CardLimbo.PlaceAnimateObject(component, true, true, false);
		return true;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000F00C File Offset: 0x0000D20C
	public void PlaceAllCardsInLimbo()
	{
		if (this.m_CardLimbo == null)
		{
			return;
		}
		foreach (object obj in this.m_MasterCardList.Values)
		{
			AnimateObject component = ((GameObject)obj).GetComponent<AnimateObject>();
			if (component != null)
			{
				this.m_CardLimbo.PlaceAnimateObject(component, true, true, false);
			}
		}
	}

	// Token: 0x040001E1 RID: 481
	public static int s_MajImpDisplayCardMask = 40960;

	// Token: 0x040001E2 RID: 482
	[SerializeField]
	private GameObject[] m_PrefabBasicCards;

	// Token: 0x040001E3 RID: 483
	[SerializeField]
	private GameObject[] m_PrefabSpecificCards;

	// Token: 0x040001E4 RID: 484
	[SerializeField]
	private GameObject m_PrefabCardMinorImprovement;

	// Token: 0x040001E5 RID: 485
	[SerializeField]
	private GameObject m_PrefabCardMinorImprovementHalf;

	// Token: 0x040001E6 RID: 486
	[SerializeField]
	private GameObject m_PrefabCardMinorImprovementIcon;

	// Token: 0x040001E7 RID: 487
	[SerializeField]
	private GameObject m_PrefabCardOccupation;

	// Token: 0x040001E8 RID: 488
	[SerializeField]
	private GameObject m_PrefabCardOccupationHalf;

	// Token: 0x040001E9 RID: 489
	[SerializeField]
	private GameObject m_PrefabCardOccupationIcon;

	// Token: 0x040001EA RID: 490
	[SerializeField]
	private GameObject m_PrefabCardBegging;

	// Token: 0x040001EB RID: 491
	[SerializeField]
	private GameObject m_PrefabCardBeggingHalf;

	// Token: 0x040001EC RID: 492
	[SerializeField]
	private GameObject m_PrefabGlow;

	// Token: 0x040001ED RID: 493
	[SerializeField]
	private GameObject m_PrefabGlowHalf;

	// Token: 0x040001EE RID: 494
	[SerializeField]
	private string[] m_CardTypePath;

	// Token: 0x040001EF RID: 495
	[SerializeField]
	private string[] m_CardDeckIds;

	// Token: 0x040001F0 RID: 496
	[SerializeField]
	private Sprite[] m_CardDeckIcons;

	// Token: 0x040001F1 RID: 497
	[SerializeField]
	private Sprite[] m_CardPlayerCountIcons;

	// Token: 0x040001F2 RID: 498
	[SerializeField]
	private Sprite[] m_CardCategoryIcons;

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	private Sprite m_CardVictoryPointsPositiveIcon;

	// Token: 0x040001F4 RID: 500
	[SerializeField]
	private Sprite m_CardVictoryPointsNegativeIcon;

	// Token: 0x040001F5 RID: 501
	[SerializeField]
	private AgricolaAnimationLocator m_CardLimbo;

	// Token: 0x040001F6 RID: 502
	[SerializeField]
	private AgricolaAnimationLocator m_CardIconMagnfyLocator;

	// Token: 0x040001F7 RID: 503
	[SerializeField]
	private float m_DragCardScale = 1f;

	// Token: 0x040001F8 RID: 504
	[SerializeField]
	private float m_DragCardTargetScale = 1f;

	// Token: 0x040001F9 RID: 505
	private ResourceManager m_ResourceManager;
}
