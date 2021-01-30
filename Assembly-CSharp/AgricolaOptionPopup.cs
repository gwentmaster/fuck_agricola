using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameData;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000046 RID: 70
public class AgricolaOptionPopup : PopupBase
{
	// Token: 0x060003F5 RID: 1013 RVA: 0x0001BB73 File Offset: 0x00019D73
	public void SetPopupHidden(bool bHidden)
	{
		if (bHidden)
		{
			this.m_PopupManager.HideActivePopup(this);
			return;
		}
		this.m_PopupManager.RestoreHiddenPopup(this);
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001BB94 File Offset: 0x00019D94
	public bool DisplayOptionList(AgricolaOptionPopup.OptionPopupMode displayMode, AgricolaGame gameController, AgricolaOptionPopup.OptionPopupRestriction restrictions)
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		if (displayMode == AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_NONE)
		{
			return false;
		}
		this.m_UsingMode = AgricolaOptionPopup.ListModes.E_MODE_REGULAR;
		if (GameOptions.IsSelectableHint(41027) || GameOptions.IsSelectableHint(41011))
		{
			this.m_UsingMode = AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_COUNTERS;
		}
		if ((GameOptions.IsSelectableHint(41026) || GameOptions.IsSelectableHint(41024)) && displayMode == AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_SHOW_NON_FOOD)
		{
			displayMode = AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_PRUCHASE_LIST;
			this.m_UsingMode = AgricolaOptionPopup.ListModes.E_MODE_PURCHASE_LIST;
		}
		else if (GameOptions.IsSelectableHint(40999) && !GameOptions.IsSelectableHintAllowHidden(40963) && displayMode == AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_SHOW_NON_FOOD)
		{
			this.m_UsingMode = AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION;
		}
		else if (GameOptions.IsSelectableHintAllowHidden(40963))
		{
			for (int i = 0; i < GameOptions.m_OptionCount; i++)
			{
				if (GameOptions.m_GameOption[i].selectionHint == 40963 && AgricolaLib.GetActionDefinition((uint)GameOptions.m_GameOption[i].selectionID, this.m_bufPtr, 1024) != 0)
				{
					GameActionDefinition gameActionDefinition = (GameActionDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameActionDefinition));
					for (int j = 0; j < 10; j++)
					{
						if (gameActionDefinition.resources[j] > 0)
						{
							this.m_UsingMode = AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_ACTION;
							break;
						}
					}
				}
			}
		}
		else if (displayMode == AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_SHOW_PLAYERCHOICES)
		{
			this.m_UsingMode = AgricolaOptionPopup.ListModes.E_MODE_PLAYERCHOICES;
		}
		this.Reset();
		int optionCount = GameOptions.m_OptionCount;
		int num = 0;
		int num2 = 0;
		AgricolaLib.GetGamePlayerBuildRoomCostQuery(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
		GameQueryBuildRoomCost gameQueryBuildRoomCost = (GameQueryBuildRoomCost)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameQueryBuildRoomCost));
		AgricolaLib.GetGamePlayerRenovateCostQuery(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
		GameQueryRenovateCost gameQueryRenovateCost = (GameQueryRenovateCost)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameQueryRenovateCost));
		AgricolaLib.GetGamePlayerBuildImprovementCostQuery(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
		GameQueryBuildImprovementCost gameQueryBuildImprovementCost = (GameQueryBuildImprovementCost)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameQueryBuildImprovementCost));
		int possibleCostsCount = gameQueryBuildRoomCost.possibleCostsCount;
		int possibleCostsCount2 = gameQueryRenovateCost.possibleCostsCount;
		int possibleCostsCount3 = gameQueryBuildImprovementCost.possibleCostsCount;
		bool[] array = new bool[possibleCostsCount];
		bool[] array2 = new bool[possibleCostsCount2];
		bool[] array3 = new bool[possibleCostsCount3];
		List<int> list = new List<int>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_ACTION)
		{
			this.m_startingCache.Clear();
			this.m_totalCache.Clear();
			int num3 = 0;
			bool flag4 = false;
			bool flag5 = true;
			List<AgricolaOptionPopup.ActionResource> list2 = new List<AgricolaOptionPopup.ActionResource>();
			for (int k = 0; k < GameOptions.m_OptionCount; k++)
			{
				if (GameOptions.m_GameOption[k].selectionHint == 40963)
				{
					num3++;
					int num4 = 0;
					if (AgricolaLib.GetActionDefinition((uint)GameOptions.m_GameOption[k].selectionID, this.m_bufPtr, 1024) != 0)
					{
						GameActionDefinition gameActionDefinition2 = (GameActionDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameActionDefinition));
						CResourceCache cresourceCache = new CResourceCache(gameActionDefinition2.resources);
						string displayName = gameActionDefinition2.displayName;
						if (this.m_totalCache == cresourceCache)
						{
							list.Add(k);
						}
						if (displayName.Contains("Pay1Food"))
						{
							bool flag6 = false;
							int num5 = 0;
							while (!flag6 && num5 < list2.Count)
							{
								if (list2[num5].type == 0 && list2[num5].count == -1)
								{
									flag6 = true;
									flag4 = true;
									AgricolaOptionPopup.ActionResource value;
									value.type = list2[num5].type;
									value.count = list2[num5].count;
									value.occurences = list2[num5].occurences + 1;
									list2[num5] = value;
								}
								num5++;
							}
							if (!flag6)
							{
								AgricolaOptionPopup.ActionResource item;
								item.type = 0;
								item.count = -1;
								item.occurences = 1;
								list2.Add(item);
							}
							num4++;
						}
						for (int l = 0; l < 10; l++)
						{
							if (cresourceCache[l] != 0)
							{
								num4++;
								bool flag7 = false;
								int num6 = 0;
								while (!flag7 && num6 < list2.Count)
								{
									if (list2[num6].type == l && list2[num6].count == cresourceCache[l])
									{
										flag7 = true;
										flag4 = true;
										AgricolaOptionPopup.ActionResource value2;
										value2.type = list2[num6].type;
										value2.count = list2[num6].count;
										value2.occurences = list2[num6].occurences + 1;
										list2[num6] = value2;
									}
									num6++;
								}
								if (!flag7)
								{
									AgricolaOptionPopup.ActionResource item2;
									item2.type = l;
									item2.count = cresourceCache[l];
									item2.occurences = 1;
									list2.Add(item2);
								}
							}
						}
						if (num4 > 1)
						{
							flag5 = false;
						}
					}
				}
			}
			num = 0;
			while (list.Count > 0)
			{
				this.m_OptionListings[num].instanceID = GameOptions.m_GameOption[list[0]].selectionID;
				this.m_OptionListings[num].hint = GameOptions.m_GameOption[list[0]].selectionHint;
				this.m_OptionListings[num].index = list[0];
				string text = GameOptions.m_GameOption[list[0]].optionText;
				bool flag8 = false;
				if (text.Contains("%s") && AgricolaLib.GetInstanceData(7, (int)GameOptions.m_GameOption[list[0]].selectionID, this.m_bufPtr, 1024) != 0)
				{
					string card_name = ((CardData)Marshal.PtrToStructure(this.m_bufPtr, typeof(CardData))).card_name;
					text = text.Replace("%s", card_name);
					this.m_OptionListings[num].m_largeTextBuffer = text;
					flag8 = true;
				}
				bool flag9 = GameOptions.m_GameOption[list[0]].isHidden > 0;
				if (!flag8)
				{
					string input_text = text;
					string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
					text2 = text2.Replace("<br>", " ");
					this.m_OptionListings[num].m_pOptionTextNode.text = ((text2 != string.Empty) ? text2 : text);
					this.m_OptionListings[num].m_pOptionTextNode.color = (flag9 ? Color.gray : Color.white);
				}
				else
				{
					this.m_OptionListings[num].m_pOptionTextNode.text = this.m_OptionListings[num].m_largeTextBuffer;
					this.m_OptionListings[num].m_pOptionTextNode.color = (flag9 ? Color.gray : Color.white);
				}
				this.m_OptionListings[num].m_pOptionRootNode.SetActive(true);
				this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(false);
				this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
				this.m_OptionListings[num].m_purchaseListToggleImage.enabled = false;
				num++;
				list.RemoveAt(0);
			}
			if (flag4 || flag5)
			{
				bool flag10 = false;
				for (int m = 0; m < list2.Count; m++)
				{
					if (list2[m].occurences == num3)
					{
						CResourceCache startingCache = this.m_startingCache;
						int type = list2[m].type;
						startingCache[type] += list2[m].count;
					}
					else if (!flag10)
					{
						this.m_OptionListings[num].m_bUseResourcePressme = true;
						this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
						this.m_OptionListings[num].m_purchaseListToggleImage.enabled = false;
						this.m_OptionListings[num].m_resourceTypes[0] = list2[m].type;
						this.m_OptionListings[num].m_resourceCache[list2[m].type] = list2[m].count;
						this.m_OptionListings[num].m_resources[0].SetActive(true);
						this.m_OptionListings[num].m_resources[0].SetInteractable(true);
						this.m_OptionListings[num].m_resources[0].SetResourceData((EResourceType)list2[m].type, list2[m].count);
						this.m_OptionListings[num].m_numResourseTypes = 1U;
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].hint = 1;
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(false);
						this.m_OptionListings[num].m_bIsActive = false;
						this.m_OptionListings[num].m_bShowArrows = false;
						this.m_OptionListings[num].m_bIsInteractable = true;
						flag10 = true;
					}
					else
					{
						this.m_OptionListings[num].m_resourceTypes[1] = list2[m].type;
						this.m_OptionListings[num].m_resourceCache2[list2[m].type] = list2[m].count;
						this.m_OptionListings[num].m_resources[1].SetActive(true);
						this.m_OptionListings[num].m_resources[1].SetInteractable(true);
						this.m_OptionListings[num].m_resources[1].SetResourceData((EResourceType)list2[m].type, list2[m].count);
						this.m_OptionListings[num].m_numResourseTypes = 2U;
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].hint = 1;
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(false);
						this.m_OptionListings[num].m_bIsActive = false;
						this.m_OptionListings[num].m_bIsActive2 = false;
						this.m_OptionListings[num].m_bShowArrows = false;
						this.m_OptionListings[num].m_bIsInteractable = true;
						flag10 = false;
						num++;
					}
				}
				if (flag10)
				{
					num++;
				}
				if (new CResourceCache() == this.m_startingCache && !flag5)
				{
					this.m_maxSelectables = 2;
				}
				else
				{
					this.m_maxSelectables = 1;
				}
			}
			else
			{
				this.m_maxSelectables = 1;
				for (int n = 0; n < GameOptions.m_OptionCount; n++)
				{
					if (GameOptions.m_GameOption[n].selectionHint == 40963)
					{
						uint num7 = 0U;
						if (AgricolaLib.GetActionDefinition((uint)GameOptions.m_GameOption[n].selectionID, this.m_bufPtr, 1024) != 0)
						{
							GameActionDefinition gameActionDefinition3 = (GameActionDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameActionDefinition));
							CResourceCache cresourceCache2 = new CResourceCache(gameActionDefinition3.resources);
							if (gameActionDefinition3.displayName.Contains("Pay1Food"))
							{
								this.m_OptionListings[num].m_resourceTypes[(int)num7] = 0;
								this.m_OptionListings[num].m_resourceCache[0] = -1;
								this.m_OptionListings[num].m_resources[(int)num7].SetActive(true);
								this.m_OptionListings[num].m_resources[(int)num7].SetInteractable(false);
								this.m_OptionListings[num].m_resources[(int)num7].SetResourceData(EResourceType.FOOD, -1);
								num7 += 1U;
							}
							for (int num8 = 0; num8 < 10; num8++)
							{
								if (cresourceCache2[num8] != 0)
								{
									this.m_OptionListings[num].m_resourceTypes[(int)num7] = num8;
									this.m_OptionListings[num].m_resourceCache[num8] = cresourceCache2[num8];
									int num9 = cresourceCache2[num8];
									this.m_OptionListings[num].m_resources[(int)num7].SetResourceData((EResourceType)num8, cresourceCache2[num8]);
									num7 += 1U;
								}
							}
							this.m_OptionListings[num].m_numResourseTypes = num7;
							this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
							this.m_OptionListings[num].hint = 1;
							this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(false);
							this.m_OptionListings[num].m_bIsActive = false;
							this.m_OptionListings[num].m_bShowArrows = false;
							this.m_OptionListings[num].m_bIsInteractable = true;
							num++;
						}
					}
				}
			}
			this.CalculateResources(false);
		}
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_COUNTERS)
		{
			this.m_startingCache.Clear();
			this.m_totalCache.Clear();
			this.m_counterMultiplierCache.Clear();
			this.m_counterCurValue = 0U;
			this.m_counterStoredHint = 0;
			CResourceCache cresourceCache3 = new CResourceCache();
			if (GameOptions.IsSelectableHint(41027))
			{
				this.m_bCounterNotLimitedByPlayerResources = false;
				this.m_counterStoredHint = 41027;
				if (AgricolaLib.GetGamePlayerPayResourcesState(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024) != 0)
				{
					GamePlayerPayResourcesState gamePlayerPayResourcesState = (GamePlayerPayResourcesState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerPayResourcesState));
					List<ushort> selectionIDsForHint = GameOptions.GetSelectionIDsForHint(41027, false);
					this.m_OptionListings[0].instanceID = selectionIDsForHint[0];
					cresourceCache3 = (this.m_counterMultiplierCache = new CResourceCache(gamePlayerPayResourcesState.m_PayResourceValues));
					this.m_counterMaxValue = (uint)gamePlayerPayResourcesState.m_PayResourceCount;
					this.m_bCounterCanSubmitUnderMaxValue = (gamePlayerPayResourcesState.m_bMayPayPartialCount != 0);
					this.m_bCounterNotLimitedByPlayerResources = (gamePlayerPayResourcesState.m_bLimitedByPlayerSupply == 0);
				}
			}
			else if (GameOptions.IsSelectableHint(41011))
			{
				this.m_bCounterCanSubmitUnderMaxValue = true;
				this.m_counterStoredHint = 41011;
				this.m_counterMaxValue = 0U;
				this.m_counterMultiplierCache[0] = 1;
				cresourceCache3 = this.m_counterMultiplierCache;
				List<ushort> selectionIDsForHint2 = GameOptions.GetSelectionIDsForHint(41011, false);
				this.m_OptionListings[0].instanceID = selectionIDsForHint2[0];
				if (AgricolaLib.GetGamePlayerPayResourcesState(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024) != 0)
				{
					GamePlayerPayResourcesState gamePlayerPayResourcesState2 = (GamePlayerPayResourcesState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerPayResourcesState));
					this.m_startingCache.SetArray(gamePlayerPayResourcesState2.m_PayResourceValues);
				}
			}
			bool flag11 = false;
			for (int num10 = 0; num10 < 10; num10++)
			{
				if (cresourceCache3[num10] != 0)
				{
					if (cresourceCache3[num10] > 1)
					{
						cresourceCache3[num10] = 1;
					}
					if (!flag11)
					{
						this.m_OptionListings[num].m_bUseResourcePressme = true;
						this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
						this.m_OptionListings[num].m_purchaseListToggleImage.enabled = false;
						this.m_OptionListings[num].m_resourceTypes[0] = num10;
						this.m_OptionListings[num].m_resourceCache[num10] = cresourceCache3[num10];
						this.m_OptionListings[num].m_countLeft = 0U;
						this.m_OptionListings[num].m_resources[0].SetActive(true);
						this.m_OptionListings[num].m_resources[0].SetInteractable(false);
						this.m_OptionListings[num].m_resources[0].SetArrows(true, true);
						this.m_OptionListings[num].m_resources[0].SetResourceData((EResourceType)num10, (int)this.m_OptionListings[num].m_countLeft);
						this.m_OptionListings[num].m_numResourseTypes = 1U;
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].hint = 1;
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(false);
						this.m_OptionListings[num].m_bIsActive = true;
						this.m_OptionListings[num].m_bShowArrows = true;
						this.m_OptionListings[num].m_bIsInteractable = true;
						flag11 = true;
					}
					else
					{
						this.m_OptionListings[num].m_resourceTypes[1] = num10;
						this.m_OptionListings[num].m_resourceCache2[num10] = cresourceCache3[num10];
						this.m_OptionListings[num].m_countRight = 0U;
						this.m_OptionListings[num].m_resources[1].SetActive(true);
						this.m_OptionListings[num].m_resources[1].SetInteractable(false);
						this.m_OptionListings[num].m_resources[1].SetArrows(true, true);
						this.m_OptionListings[num].m_resources[1].SetResourceData((EResourceType)num10, (int)this.m_OptionListings[num].m_countRight);
						this.m_OptionListings[num].m_numResourseTypes = 2U;
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].hint = 1;
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(false);
						this.m_OptionListings[num].m_bIsActive = true;
						this.m_OptionListings[num].m_bIsActive2 = true;
						this.m_OptionListings[num].m_bShowArrows = true;
						this.m_OptionListings[num].m_bIsInteractable = true;
						flag11 = false;
						num++;
					}
				}
			}
			if (flag11)
			{
				num++;
			}
		}
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION)
		{
			AgricolaLib.GetGamePlayerAdditionalResourceQuery(AgricolaLib.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
			this.m_pEventResources = (GameQueryAdditionalResources)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameQueryAdditionalResources));
			this.m_confirmButton.SetActive(true);
			this.m_startingCache.Clear();
			this.m_totalCache.Clear();
			for (int num11 = 0; num11 < 10; num11++)
			{
				this.m_startingCache[num11] = (int)this.m_pEventResources.baseResources[num11];
			}
			this.m_totalCache += this.m_startingCache;
			this.m_resourceFlags = 0U;
			ushort num12 = 0;
			num = 0;
			for (int num13 = 0; num13 < this.m_pEventResources.additionalEntryCount; num13++)
			{
				switch (this.m_pEventResources.additionalResourceType[num13])
				{
				case 0U:
				{
					this.m_OptionListings[num].m_mainResourceEntry = num13;
					int num14 = 0;
					if (this.m_pEventResources.costAmount[num13] > 0U)
					{
						this.m_OptionListings[num].m_resourceTypes[num14] = (int)this.m_pEventResources.costType[num13];
						this.m_OptionListings[num].m_resources[num14].SetActive(true);
						this.m_OptionListings[num].m_resources[num14].SetResourceData((EResourceType)this.m_pEventResources.costType[num13], (int)(uint.MaxValue * this.m_pEventResources.costAmount[num13]));
						num14++;
					}
					for (int num15 = 0; num15 < 10; num15++)
					{
						int num16 = 11 * num13 + num15;
						if (this.m_pEventResources.additionalResources[num16] > 0)
						{
							this.m_OptionListings[num].m_resourceTypes[num14] = num15;
							this.m_OptionListings[num].m_resources[num14].SetActive(true);
							this.m_OptionListings[num].m_resources[num14].SetResourceData((EResourceType)num15, (int)this.m_pEventResources.additionalResources[num16]);
							num14++;
						}
					}
					this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
					this.m_OptionListings[num].m_numResourseTypes = (uint)num14;
					this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
					this.m_OptionListings[num].SetToggleGroupOn(false);
					this.m_OptionListings[num].m_bIsActive = true;
					this.m_OptionListings[num].m_bShowArrows = false;
					this.m_OptionListings[num].m_bIsInteractable = false;
					this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
					this.m_OptionListings[num].m_purchaseListToggleImage.enabled = false;
					this.m_OptionListings[num].m_bNoNegativeCost = false;
					if (this.m_pEventResources.sourceCardInstanceID[num13] != 0U)
					{
						this.m_OptionListings[num].ShowLeftCard(gameController.GetCardManager().CreateTemporaryCardFromInstanceID((int)this.m_pEventResources.sourceCardInstanceID[num13]));
					}
					num++;
					break;
				}
				case 1U:
				case 3U:
				case 4U:
				{
					this.m_OptionListings[num].m_mainResourceEntry = num13;
					int num14 = 0;
					if (this.m_pEventResources.costAmount[num13] > 0U)
					{
						this.m_OptionListings[num].m_resourceTypes[num14] = (int)this.m_pEventResources.costType[num13];
						this.m_OptionListings[num].m_resources[num14].SetActive(true);
						this.m_OptionListings[num].m_resources[num14].SetResourceData((EResourceType)this.m_pEventResources.costType[num13], (int)(uint.MaxValue * this.m_pEventResources.costAmount[num13]));
						num14++;
					}
					for (int num17 = 0; num17 < 10; num17++)
					{
						int num18 = 11 * num13 + num17;
						if (this.m_pEventResources.additionalResources[num18] > 0)
						{
							this.m_OptionListings[num].m_resourceTypes[num14] = num17;
							this.m_OptionListings[num].m_resources[num14].SetActive(true);
							this.m_OptionListings[num].m_resources[num14].SetResourceData((EResourceType)num17, (int)this.m_pEventResources.additionalResources[num18]);
							num14++;
						}
					}
					this.m_OptionListings[num].m_numResourseTypes = (uint)num14;
					this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
					this.m_OptionListings[num].hint = 1;
					this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
					this.m_OptionListings[num].SetToggleGroupOn(false);
					this.m_OptionListings[num].m_bIsActive = false;
					this.m_OptionListings[num].m_bShowArrows = false;
					this.m_OptionListings[num].m_bIsInteractable = true;
					this.m_OptionListings[num].m_flagIndex = num12;
					this.m_OptionListings[num].m_bNoNegativeCost = (this.m_pEventResources.additionalResourceType[num13] == 3U);
					if (this.m_pEventResources.sourceCardInstanceID[num13] != 0U)
					{
						this.m_OptionListings[num].ShowLeftCard(gameController.GetCardManager().CreateTemporaryCardFromInstanceID((int)this.m_pEventResources.sourceCardInstanceID[num13]));
					}
					num12 += 1;
					num++;
					break;
				}
				case 2U:
				{
					this.m_OptionListings[num].m_mainResourceEntry = num13;
					int num14 = 0;
					bool flag12 = false;
					int num19 = 0;
					for (int num20 = 0; num20 < 10; num20++)
					{
						int num21 = 11 * num13 + num20;
						if (this.m_pEventResources.additionalResources[num21] > 0)
						{
							this.m_OptionListings[num].m_resourceTypes[num14] = num20;
							this.m_OptionListings[num].m_resources[num14].SetResourceData((EResourceType)num20, (int)this.m_pEventResources.additionalResources[num21]);
							this.m_OptionListings[num].m_resources[num14].SetActive(true);
							this.m_OptionListings[num].m_resources[num14].SetInteractable(true);
							if (flag12)
							{
								num19 = num20;
								this.m_OptionListings[num].m_resources[num14].SetIsSelected(true);
							}
							else
							{
								flag12 = !flag12;
								this.m_OptionListings[num].m_resources[num14].SetIsSelected(false);
							}
							num14++;
						}
					}
					this.m_OptionListings[num].m_numResourseTypes = (uint)num14;
					uint num22 = 15U << (int)num12;
					this.m_resourceFlags &= ~num22;
					this.m_resourceFlags |= (uint)((uint)num19 << (int)num12);
					this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
					this.m_OptionListings[num].hint = 1;
					this.m_OptionListings[num].m_bUseResourcePressme = true;
					this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
					this.m_OptionListings[num].m_purchaseListToggleImage.enabled = false;
					this.m_OptionListings[num].SetToggleGroupOn(false);
					this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
					this.m_OptionListings[num].m_bIsActive = false;
					this.m_OptionListings[num].m_bShowArrows = false;
					this.m_OptionListings[num].m_bIsInteractable = true;
					this.m_OptionListings[num].m_flagIndex = num12;
					this.m_OptionListings[num].m_bNoNegativeCost = false;
					if (this.m_pEventResources.sourceCardInstanceID[num13] != 0U)
					{
						this.m_OptionListings[num].ShowLeftCard(gameController.GetCardManager().CreateTemporaryCardFromInstanceID((int)this.m_pEventResources.sourceCardInstanceID[num13]));
					}
					num12 += 4;
					num++;
					break;
				}
				case 5U:
				case 6U:
				{
					int num14 = this.m_pEventResources.triggerAdditionalResourceIndex[num13];
					if (num14 == -1)
					{
						this.m_OptionListings[num].m_mainResourceEntry = num13;
						int count = this.m_OptionListings[num].m_linkedEntries.Count;
						this.m_OptionListings[num].m_OpponentTokens[count].SetActive(true);
						this.m_OptionListings[num].m_OpponentTokens[count].SetResourceData((EResourceType)this.m_pEventResources.costType[num13], (int)this.m_pEventResources.costAmount[num13]);
						this.m_OptionListings[num].m_OpponentTokens[count].SetOppTokenData((int)this.m_pEventResources.opponentInstanceID[num13], gameController);
						this.m_OptionListings[num].m_resourceTypes[0] = (this.m_OptionListings[num].m_resourceTypes[1] = 10);
						this.m_OptionListings[num].SetOpponentAreaVisible(true);
						this.m_OptionListings[num].SetOpponentDisplaySecondRow(false);
						this.m_OptionListings[num].m_numResourseTypes = ((this.m_pEventResources.costAmount[num13] > 0U) ? 2U : 1U);
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].m_linkedEntries.Add(num13);
						this.m_OptionListings[num].m_bIsInteractable = false;
						this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
						this.m_OptionListings[num].m_purchaseListToggleImage.enabled = false;
						this.m_OptionListings[num].SetToggleGroupOn(false);
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
						this.m_OptionListings[num].m_bIsActive = true;
						this.m_OptionListings[num].m_bShowArrows = false;
						if (this.m_pEventResources.sourceCardInstanceID[num13] != 0U)
						{
							this.m_OptionListings[num].ShowLeftCard(gameController.GetCardManager().CreateTemporaryCardFromInstanceID((int)this.m_pEventResources.sourceCardInstanceID[num13]));
						}
						num++;
					}
					else
					{
						for (int num23 = 0; num23 < num; num23++)
						{
							if (num14 == this.m_OptionListings[num23].m_mainResourceEntry)
							{
								int count2 = this.m_OptionListings[num23].m_linkedEntries.Count;
								this.m_OptionListings[num23].SetOpponentAreaVisible(true);
								this.m_OptionListings[num23].SetOpponentDisplaySecondRow(true);
								this.m_OptionListings[num23].m_OpponentTokens[count2].SetActive(true);
								this.m_OptionListings[num23].m_OpponentTokens[count2].SetResourceData((EResourceType)this.m_pEventResources.costType[num13], (int)this.m_pEventResources.costAmount[num13]);
								this.m_OptionListings[num23].m_OpponentTokens[count2].SetOppTokenData((int)this.m_pEventResources.opponentInstanceID[num13], gameController);
								this.m_OptionListings[num23].m_linkedEntries.Add(num13);
								break;
							}
						}
					}
					break;
				}
				case 7U:
				{
					this.m_OptionListings[num].m_mainResourceEntry = num13;
					int num14 = 1;
					this.m_OptionListings[num].m_resourceTypes[0] = 10;
					this.m_OptionListings[num].m_bTokenOnFirstRow = true;
					this.m_OptionListings[num].m_bTokenHasQuestionMark = true;
					if (this.m_pEventResources.costAmount[num13] > 0U)
					{
						this.m_OptionListings[num].m_resourceTypes[num14] = (int)this.m_pEventResources.costType[num13];
						this.m_OptionListings[num].m_resources[num14].SetActive(true);
						this.m_OptionListings[num].m_resources[num14].SetResourceData((EResourceType)this.m_pEventResources.costType[num13], (int)(uint.MaxValue * this.m_pEventResources.costAmount[num13]));
						num14++;
					}
					for (int num24 = 0; num24 < 10; num24++)
					{
						int num25 = 11 * num13 + num24;
						if (this.m_pEventResources.additionalResources[num25] > 0)
						{
							this.m_OptionListings[num].m_resourceTypes[num14] = num24;
							this.m_OptionListings[num].m_resources[num14].SetActive(true);
							this.m_OptionListings[num].m_resources[num14].SetResourceData((EResourceType)num24, (int)this.m_pEventResources.additionalResources[num25]);
							num14++;
						}
					}
					int count3 = this.m_OptionListings[num].m_linkedEntries.Count;
					this.m_OptionListings[num].m_linkedEntries.Add(num13);
					this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
					this.m_OptionListings[num].SetToggleGroupOn(false);
					this.m_OptionListings[num].m_numResourseTypes = (uint)num14;
					this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
					this.m_OptionListings[num].m_bIsActive = true;
					this.m_OptionListings[num].m_bShowArrows = false;
					this.m_OptionListings[num].m_bIsInteractable = true;
					this.m_OptionListings[num].m_bNoNegativeCost = false;
					if (this.m_pEventResources.sourceCardInstanceID[num13] != 0U)
					{
						this.m_OptionListings[num].ShowLeftCard(gameController.GetCardManager().CreateTemporaryCardFromInstanceID((int)this.m_pEventResources.sourceCardInstanceID[num13]));
					}
					num++;
					break;
				}
				}
			}
			this.m_currentListSize = num;
			this.CalculateResources(false);
		}
		int num26 = 0;
		while (num26 < GameOptions.m_OptionCount && this.m_UsingMode < AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION)
		{
			if ((GameOptions.m_GameOption[num26].selectionHint != 40960 || GameOptions.m_GameOption[num26].isHidden != 0) && !this.IgnoreHint(GameOptions.m_GameOption[num26].selectionHint, displayMode))
			{
				if (restrictions.bUseRestriction)
				{
					if (restrictions.resourceUseLimit != -1)
					{
						this.m_topResourceDisplay.SetActive(true);
						this.m_bottomResourceDisplay.SetActive(false);
						this.m_startingCache.Clear();
						this.m_numTopResourceTypes = 1;
						if (restrictions.resource != EResourceType.COUNT)
						{
							this.m_topResourceTypes[0] = (int)restrictions.resource;
							this.m_startingCache[(int)restrictions.resource] = restrictions.resourceUseLimit;
						}
						else
						{
							this.m_topResourceTypes[0] = 0;
							this.m_startingCache[0] = restrictions.resourceUseLimit;
						}
						this.m_topResources[0].SetActive(true);
						this.m_topResources[0].SetResourceData((EResourceType)this.m_topResourceTypes[0], restrictions.resourceUseLimit);
					}
					if (restrictions.selectionHint != 0 && restrictions.selectionHint != GameOptions.m_GameOption[num26].selectionHint)
					{
						goto IL_275A;
					}
					int cardInstanceIDFromSubID = AgricolaLib.GetCardInstanceIDFromSubID((int)GameOptions.m_GameOption[num26].selectionID, this.m_UsingMode != AgricolaOptionPopup.ListModes.E_MODE_REGULAR, true);
					if (cardInstanceIDFromSubID == 0 || (restrictions.instanceID != 0 && restrictions.instanceID != cardInstanceIDFromSubID) || (restrictions.resource != EResourceType.COUNT && restrictions.selectionHint == 40992 && AgricolaLib.GetConvertDefinition((uint)GameOptions.m_GameOption[num26].selectionID, this.m_bufPtr, 1024) != 0 && ((GameConvertDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameConvertDefinition))).resourceCostType != (int)restrictions.resource))
					{
						goto IL_275A;
					}
				}
				this.m_OptionListings[num].instanceID = GameOptions.m_GameOption[num26].selectionID;
				this.m_OptionListings[num].hint = GameOptions.m_GameOption[num26].selectionHint;
				this.m_OptionListings[num].index = num26;
				if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_REGULAR)
				{
					if (this.m_OptionListings[num].hint == 40992)
					{
						if (AgricolaLib.GetConvertDefinition((uint)this.m_OptionListings[num].instanceID, this.m_bufPtr, 1024) != 0)
						{
							GameConvertDefinition gameConvertDefinition = (GameConvertDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameConvertDefinition));
							this.m_OptionListings[num].m_resourceOverride = true;
							this.m_OptionListings[num].m_pOptionRootNode.SetActive(false);
							this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
							this.m_OptionListings[num].m_resourceTypes[0] = gameConvertDefinition.resourceCostType;
							int resourceCostAmount = gameConvertDefinition.resourceCostAmount;
							if (resourceCostAmount == 0)
							{
								this.m_OptionListings[num].m_resourceTypes[0] = gameConvertDefinition.resourceProducedType;
								this.m_OptionListings[num].m_resources[0].SetActive(true);
								this.m_OptionListings[num].m_resources[0].SetResourceData((EResourceType)gameConvertDefinition.resourceProducedType, resourceCostAmount);
								this.m_OptionListings[num].m_numResourseTypes = 1U;
							}
							else
							{
								this.m_OptionListings[num].m_resources[0].SetActive(true);
								this.m_OptionListings[num].m_resources[0].SetResourceData((EResourceType)gameConvertDefinition.resourceCostType, resourceCostAmount);
								this.m_OptionListings[num].m_resources[1].SetActive(true);
								this.m_OptionListings[num].m_resources[1].SetResourceData((EResourceType)gameConvertDefinition.resourceProducedType, gameConvertDefinition.resourceProducedAmount);
								this.m_OptionListings[num].m_resourceTypes[1] = gameConvertDefinition.resourceProducedType;
								this.m_OptionListings[num].m_numResourseTypes = 2U;
								this.m_OptionListings[num].m_pPurchaseListArrow.SetActive(true);
							}
						}
						else
						{
							Debug.LogError("AgricolaOptionPopup - Didn't get conversion instance");
						}
					}
					else
					{
						bool flag13 = false;
						string text3 = GameOptions.m_GameOption[num26].optionText;
						if (text3.Contains("%s") && AgricolaLib.GetInstanceData(7, (int)GameOptions.m_GameOption[list[0]].selectionID, this.m_bufPtr, 1024) != 0)
						{
							string card_name2 = ((CardData)Marshal.PtrToStructure(this.m_bufPtr, typeof(CardData))).card_name;
							text3 = text3.Replace("%s", card_name2);
							this.m_OptionListings[num].m_largeTextBuffer = text3;
							flag13 = true;
						}
						bool flag14 = GameOptions.m_GameOption[num26].isHidden != 0 || this.m_OptionListings[num].hint == 41000;
						if (flag14)
						{
							num2++;
						}
						if (!flag13)
						{
							string optionText = GameOptions.m_GameOption[num26].optionText;
							string input_text2 = optionText;
							string text4 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text2);
							text4 = text4.Replace("<br>", " ");
							this.m_OptionListings[num].m_pOptionTextNode.text = ((text4 != string.Empty) ? text4 : optionText);
							this.m_OptionListings[num].m_pOptionTextNode.color = (flag14 ? Color.gray : Color.white);
							this.m_OptionListings[num].m_optionToggle.interactable = !flag14;
						}
						else
						{
							this.m_OptionListings[num].m_pOptionTextNode.text = this.m_OptionListings[num].m_largeTextBuffer;
							this.m_OptionListings[num].m_pOptionTextNode.color = (flag14 ? Color.gray : Color.white);
							this.m_OptionListings[num].m_optionToggle.interactable = !flag14;
						}
						this.m_OptionListings[num].m_pOptionRootNode.SetActive(true);
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(false);
						this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
						this.m_OptionListings[num].m_purchaseListToggleImage.enabled = false;
					}
				}
				else
				{
					CResourceCache cresourceCache4 = new CResourceCache();
					if ((this.m_OptionListings[num].instanceID & 36864) == 36864)
					{
						flag = true;
						this.m_OptionListings[num].SetRoomType(gameQueryBuildRoomCost.houseType);
						int num27 = (int)(this.m_OptionListings[num].instanceID & 15);
						int num28 = 11 * num27;
						int num29 = 0;
						while (num29 < 11 && num29 < 10)
						{
							cresourceCache4[num29] = (int)gameQueryBuildRoomCost.possibleCostsResources[num28++];
							num29++;
						}
						array[(int)(this.m_OptionListings[num].instanceID & 15)] = true;
						if (gameQueryBuildRoomCost.availableResourcesCardInstanceID != 0U)
						{
							this.m_OptionListings[num].m_affectingCards.Add(gameQueryBuildRoomCost.availableResourcesCardInstanceID);
						}
						for (int num30 = 0; num30 < 8; num30++)
						{
							int num31 = 8 * num27 + num30;
							if (gameQueryBuildRoomCost.buildRoomEffectCardIDs[num31] != 0)
							{
								this.m_OptionListings[num].m_affectingCards.Add((uint)gameQueryBuildRoomCost.buildRoomEffectCardIDs[num31]);
							}
						}
					}
					else if ((this.m_OptionListings[num].instanceID & 40960) == 40960)
					{
						flag2 = true;
						int num32 = (int)(this.m_OptionListings[num].instanceID & 15);
						int num33 = 11 * num32;
						int num34 = 0;
						while (num34 < 11 && num34 < 10)
						{
							cresourceCache4[num34] = (int)gameQueryRenovateCost.possibleCostsResources[num33++];
							num34++;
						}
						array2[(int)(this.m_OptionListings[num].instanceID & 15)] = true;
						this.m_OptionListings[num].SetRoomType(gameQueryRenovateCost.houseType[num32]);
						if (gameQueryRenovateCost.availableResourcesCardInstanceID != 0U)
						{
							this.m_OptionListings[num].m_affectingCards.Add(gameQueryRenovateCost.availableResourcesCardInstanceID);
						}
						for (int num35 = 0; num35 < 8; num35++)
						{
							int num36 = 8 * num32 + num35;
							if (gameQueryRenovateCost.renovateEffectCardIDs[num36] != 0)
							{
								this.m_OptionListings[num].m_affectingCards.Add((uint)gameQueryRenovateCost.renovateEffectCardIDs[num36]);
							}
						}
					}
					else if ((this.m_OptionListings[num].instanceID & 49152) == 49152)
					{
						flag3 = true;
						int num37 = (int)(this.m_OptionListings[num].instanceID & 15);
						int num38 = 11 * num37;
						int num39 = 0;
						while (num39 < 11 && num39 < 10)
						{
							cresourceCache4[num39] = (int)gameQueryBuildImprovementCost.possibleCostsResources[num38++];
							num39++;
						}
						array3[(int)(this.m_OptionListings[num].instanceID & 15)] = true;
						for (int num40 = 0; num40 < 7; num40++)
						{
							int num41 = 7 * num37 + num40;
							if (gameQueryBuildImprovementCost.buildImprovementEffectCardIDs[num41] != 0)
							{
								this.m_OptionListings[num].m_affectingCards.Add((uint)gameQueryBuildImprovementCost.buildImprovementEffectCardIDs[num41]);
							}
						}
					}
					else
					{
						if (AgricolaLib.GetInstanceData(7, (int)this.m_OptionListings[num].instanceID, this.m_bufPtr, 1024) == 0)
						{
							goto IL_275A;
						}
						CardData cardData = (CardData)Marshal.PtrToStructure(this.m_bufPtr, typeof(CardData));
						this.m_OptionListings[num].ShowLeftCard(gameController.GetCardManager().CreateTemporaryCardFromInstanceID((int)cardData.card_instance_id));
						this.m_OptionListings[num].m_pPurchaseListReturnCardTextNode.gameObject.SetActive(true);
						flag3 = true;
						string optionText2 = GameOptions.m_GameOption[num26].optionText;
						string text5 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(optionText2);
						this.m_OptionListings[num].m_pPurchaseListReturnCardTextNode.text = text5;
					}
					if (this.m_OptionListings[num].GetLeftCard() == null)
					{
						int num42 = 0;
						for (int num43 = 0; num43 < 10; num43++)
						{
							if (cresourceCache4[num43] > 0 && num42 < this.m_OptionListings[num].m_resources.Length)
							{
								this.m_OptionListings[num].m_resourceTypes[num42] = num43;
								int count4 = cresourceCache4[num43];
								this.m_OptionListings[num].m_resources[num42].SetActive(true);
								this.m_OptionListings[num].m_resources[num42].SetResourceData((EResourceType)num43, count4);
								num42++;
							}
						}
						this.m_OptionListings[num].m_numResourseTypes = (uint)num42;
					}
					this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
					this.m_OptionListings[num].m_pOptionRootNode.SetActive(false);
					this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
				}
				if (GameOptions.m_GameOption[num26].isHidden != 0 || this.m_OptionListings[num].hint == 41000)
				{
					this.m_OptionListings[num].index = ((this.m_OptionListings[num].hint == 41000) ? 41000 : 0);
					this.m_OptionListings[num].instanceID = 0;
					this.m_OptionListings[num].hint = 0;
				}
				num++;
			}
			IL_275A:
			num26++;
		}
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_PURCHASE_LIST)
		{
			CResourceCache cresourceCache5 = new CResourceCache();
			if (flag)
			{
				int num44 = 0;
				while (num44 < possibleCostsCount && num < this.m_OptionListings.Length)
				{
					if (!array[num44])
					{
						this.m_OptionListings[num].instanceID = 0;
						this.m_OptionListings[num].hint = 0;
						this.m_OptionListings[num].index = 0;
						this.m_OptionListings[num].SetRoomType(gameQueryBuildRoomCost.houseType);
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
						this.m_OptionListings[num].m_pOptionRootNode.SetActive(false);
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
						this.m_OptionListings[num].m_purchaseListGrayout.SetActive(true);
						int num45 = num44;
						int num46 = 11 * num45;
						int num47 = 0;
						while (num47 < 11 && num47 < 10)
						{
							cresourceCache5[num47] = (int)gameQueryBuildRoomCost.possibleCostsResources[num46++];
							num47++;
						}
						int num48 = 0;
						for (int num49 = 0; num49 < 10; num49++)
						{
							if (cresourceCache5[num49] > 0)
							{
								this.m_OptionListings[num].m_resourceTypes[num48] = num49;
								int count5 = cresourceCache5[num49];
								this.m_OptionListings[num].m_resources[num48].SetActive(true);
								this.m_OptionListings[num].m_resources[num48].SetResourceData((EResourceType)num49, count5);
								num48++;
							}
						}
						this.m_OptionListings[num].m_numResourseTypes = (uint)num48;
						if (num48 > 0)
						{
							num++;
						}
					}
					num44++;
				}
			}
			if (flag2)
			{
				int num50 = 0;
				while (num50 < possibleCostsCount2 && num < this.m_OptionListings.Length)
				{
					if (!array2[num50])
					{
						this.m_OptionListings[num].instanceID = 0;
						this.m_OptionListings[num].hint = 0;
						this.m_OptionListings[num].index = 0;
						this.m_OptionListings[num].SetRoomType(gameQueryRenovateCost.houseType[num50]);
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
						this.m_OptionListings[num].m_pOptionRootNode.SetActive(false);
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
						this.m_OptionListings[num].m_purchaseListGrayout.SetActive(true);
						int num51 = num50;
						int num52 = 11 * num51;
						int num53 = 0;
						while (num53 < 11 && num53 < 10)
						{
							cresourceCache5[num53] = (int)gameQueryRenovateCost.possibleCostsResources[num52++];
							num53++;
						}
						int num54 = 0;
						for (int num55 = 0; num55 < 10; num55++)
						{
							if (cresourceCache5[num55] > 0)
							{
								this.m_OptionListings[num].m_resourceTypes[num54] = num55;
								int count6 = cresourceCache5[num55];
								this.m_OptionListings[num].m_resources[num54].SetActive(true);
								this.m_OptionListings[num].m_resources[num54].SetResourceData((EResourceType)num55, count6);
								num54++;
							}
						}
						this.m_OptionListings[num].m_numResourseTypes = (uint)num54;
						if (num54 > 0)
						{
							num++;
						}
					}
					num50++;
				}
			}
			if (flag3)
			{
				int num56 = 0;
				while (num56 < possibleCostsCount3 && num < this.m_OptionListings.Length)
				{
					if (!array3[num56])
					{
						this.m_OptionListings[num].instanceID = 0;
						this.m_OptionListings[num].hint = 0;
						this.m_OptionListings[num].index = 0;
						this.m_OptionListings[num].m_pPurchaseListDivider.SetActive(true);
						this.m_OptionListings[num].m_pOptionRootNode.SetActive(false);
						this.m_OptionListings[num].m_pPurchaseListRootNode.SetActive(true);
						this.m_OptionListings[num].m_purchaseListToggle.interactable = false;
						this.m_OptionListings[num].m_purchaseListGrayout.SetActive(true);
						int num57 = num56;
						int num58 = 11 * num57;
						int num59 = 0;
						while (num59 < 11 && num59 < 10)
						{
							cresourceCache5[num59] = (int)gameQueryBuildImprovementCost.possibleCostsResources[num58++];
							num59++;
						}
						int num60 = 0;
						for (int num61 = 0; num61 < 10; num61++)
						{
							if (cresourceCache5[num61] > 0)
							{
								this.m_OptionListings[num].m_resourceTypes[num60] = num61;
								int count7 = cresourceCache5[num61];
								this.m_OptionListings[num].m_resources[num60].SetActive(true);
								this.m_OptionListings[num].m_resources[num60].SetResourceData((EResourceType)num61, count7);
								num60++;
							}
						}
						this.m_OptionListings[num].m_numResourseTypes = (uint)num60;
						if (num60 > 0)
						{
							num++;
						}
					}
					num56++;
				}
			}
		}
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_PLAYERCHOICES)
		{
			Debug.LogError("AgricolaOptionPopup - Need to Implement E_MODE_PLAYERCHOICES");
		}
		this.m_currentListSize = num;
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_PURCHASE_LIST)
		{
			this.SortPurchaseListMode();
		}
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_COUNTERS)
		{
			this.CalculateArrows();
		}
		for (int num62 = 0; num62 < this.m_currentListSize; num62++)
		{
			this.m_OptionListings[num62].gameObject.SetActive(true);
			if (this.m_OptionListings[num62].GetIsWideMode() && this.m_OptionListings[num62].m_pPurchaseListDivider.activeSelf)
			{
				this.m_OptionListings[num62].m_pPurchaseListDivider.SetActive(false);
				this.m_OptionListings[num62].m_pPurchaseListDividerWide.SetActive(true);
			}
		}
		for (int num63 = this.m_currentListSize; num63 < this.m_OptionListings.Length; num63++)
		{
			this.m_OptionListings[num63].gameObject.SetActive(false);
		}
		if (this.m_currentListSize > 0)
		{
			this.m_OptionListings[this.m_currentListSize - 1].m_pPurchaseListDivider.SetActive(false);
			this.m_OptionListings[this.m_currentListSize - 1].m_pPurchaseListDividerWide.SetActive(false);
		}
		if (flag3)
		{
			int buildingCardInstanceID = (int)gameQueryBuildImprovementCost.buildingCardInstanceID;
			if (buildingCardInstanceID != 0)
			{
				this.m_LeftCard = gameController.GetCardManager().CreateTemporaryCardFromInstanceID(buildingCardInstanceID);
			}
		}
		int num64 = 0;
		while (this.m_LeftCard == null && num64 < this.m_currentListSize)
		{
			int cardInstanceIDFromSubID2 = AgricolaLib.GetCardInstanceIDFromSubID((int)this.m_OptionListings[num64].instanceID, this.m_UsingMode != AgricolaOptionPopup.ListModes.E_MODE_REGULAR, true);
			if (cardInstanceIDFromSubID2 != 0)
			{
				this.m_LeftCard = gameController.GetCardManager().CreateTemporaryCardFromInstanceID(cardInstanceIDFromSubID2);
			}
			num64++;
		}
		if (this.m_LeftCard == null)
		{
			int cardInstanceIDFromSubID3 = AgricolaLib.GetCardInstanceIDFromSubID((int)this.m_OptionListings[0].instanceID, this.m_UsingMode != AgricolaOptionPopup.ListModes.E_MODE_REGULAR, false);
			if (cardInstanceIDFromSubID3 != 0)
			{
				this.m_LeftCard = gameController.GetCardManager().CreateTemporaryCardFromInstanceID(cardInstanceIDFromSubID3);
			}
		}
		if (this.m_LeftCard != null && this.m_LeftCardLocator != null)
		{
			this.m_LeftCard.SetActive(true);
			AnimateObject component = this.m_LeftCard.GetComponent<AnimateObject>();
			if (component != null)
			{
				this.m_LeftCardLocator.PlaceAnimateObject(component, true, true, false);
			}
		}
		if (this.m_currentListSize > 0 && this.m_currentListSize > num2)
		{
			if (this.m_noneButton != null)
			{
				this.m_noneButton.SetActive(GameOptions.IsSelectableHint(40960));
			}
			if (this.m_undoButton != null)
			{
				if (AgricolaLib.GetIsTutorialGame())
				{
					this.m_undoButton.SetActive(false);
				}
				else
				{
					this.m_undoButton.SetActive(true);
				}
			}
			return true;
		}
		this.Reset();
		return false;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0001EA64 File Offset: 0x0001CC64
	public void HandleButtonNone()
	{
		this.m_PopupManager.SetPopup(EPopups.NONE);
		this.m_gameController.OnButtonPressedNoAction();
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0001EA7D File Offset: 0x0001CC7D
	public void HandleButtonUndo()
	{
		this.m_PopupManager.SetPopup(EPopups.NONE);
		this.m_gameController.OnButtonPressedUndo();
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0001EA98 File Offset: 0x0001CC98
	public void HandleButtonCommit()
	{
		bool flag = false;
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION)
		{
			GameOptions.SelectOptionByHintWithData(40999, this.m_resourceFlags);
			flag = true;
		}
		else if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_COUNTERS)
		{
			if (this.m_counterStoredHint == 41027)
			{
				uint num = 0U;
				int num2 = 0;
				for (int i = 0; i < 10; i++)
				{
					if (this.m_counterMultiplierCache[i] > 0)
					{
						num |= (uint)((uint)this.m_totalCache[i] << num2);
						num2 += 4;
					}
				}
				GameOptions.SelectOptionByHintWithData(41027, num);
			}
			else if (this.m_counterStoredHint == 41011)
			{
				GameOptions.SelectOptionByHintWithData(41011, (uint)this.m_totalCache[0]);
			}
			flag = true;
		}
		else if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_ACTION)
		{
			CResourceCache thisCache = new CResourceCache();
			bool flag2 = false;
			int optionIndex = -1;
			for (int j = 0; j < GameOptions.m_OptionCount; j++)
			{
				if (GameOptions.m_GameOption[j].selectionHint == 40963 && GameOptions.m_GameOption[j].isHidden == 0 && AgricolaLib.GetActionDefinition((uint)GameOptions.m_GameOption[j].selectionID, this.m_bufPtr, 1024) != 0)
				{
					GameActionDefinition gameActionDefinition = (GameActionDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameActionDefinition));
					CResourceCache cresourceCache = new CResourceCache(gameActionDefinition.resources);
					if (gameActionDefinition.displayName.Contains("Pay1Food"))
					{
						CResourceCache cresourceCache2 = cresourceCache;
						int value = cresourceCache2[0] - 1;
						cresourceCache2[0] = value;
					}
					if (cresourceCache == this.m_totalCache && this.m_totalCache != thisCache)
					{
						flag2 = true;
						optionIndex = j;
						break;
					}
				}
			}
			if (flag2)
			{
				GameOptions.SelectOption(optionIndex);
				flag = true;
			}
			else if (this.m_selectedIndex != -1)
			{
				GameOptions.SelectOption(this.m_OptionListings[this.m_selectedIndex].index);
				flag = true;
			}
		}
		else if (this.m_selectedIndex != -1)
		{
			GameOptions.SelectOption(this.m_OptionListings[this.m_selectedIndex].index);
			flag = true;
		}
		if (flag)
		{
			this.m_UsingMode = AgricolaOptionPopup.ListModes.E_MODE_OFF;
			this.Reset();
			this.m_PopupManager.SetPopup(EPopups.NONE);
		}
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0001ECBC File Offset: 0x0001CEBC
	private void Init()
	{
		if (this.m_bInitialized)
		{
			return;
		}
		this.m_bInitialized = true;
		this.m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		this.m_startingCache = new CResourceCache();
		this.m_totalCache = new CResourceCache();
		this.m_counterMultiplierCache = new CResourceCache();
		this.m_topResources = new AgricolaOptionPopup_Resource[4];
		this.m_bottomResources = new AgricolaOptionPopup_Resource[4];
		this.m_OptionListings = new AgricolaOptionPopup_OptionListing[16];
		for (int i = 0; i < 16; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_cellPrefab);
			gameObject.transform.SetParent(this.m_cellContainer.transform);
			gameObject.transform.localScale = Vector3.one;
			this.m_OptionListings[i] = gameObject.GetComponent<AgricolaOptionPopup_OptionListing>();
			this.m_OptionListings[i].Reset();
			this.m_OptionListings[i].SetToggleGroup(this.m_toggleGroup);
			this.m_OptionListings[i].SetToggleGroupOn(true);
			this.m_OptionListings[i].m_optionToggle.group = this.m_toggleGroup;
			this.m_OptionListings[i].AddToggleChangeCallback(new AgricolaOptionPopup_OptionListing.OnCellCallback(this.HandleOnCellCallback));
			this.m_OptionListings[i].AddResourceTogglePassThrough(new AgricolaOptionPopup_OptionListing.OnResourcePassThrough(this.HandleOnResourcePassThrough));
		}
		for (int j = 0; j < 4; j++)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_resourcePrefab);
			gameObject2.transform.SetParent(this.m_topResourceContainer.transform);
			gameObject2.transform.localScale = Vector3.one;
			this.m_topResources[j] = gameObject2.GetComponent<AgricolaOptionPopup_Resource>();
			this.m_topResources[j].SetActive(false);
			this.m_topResources[j].SetArrows(false, false);
			this.m_topResources[j].SetInteractable(false);
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.m_resourcePrefab);
			gameObject3.transform.SetParent(this.m_bottomResourceContainer.transform);
			gameObject3.transform.localScale = Vector3.one;
			this.m_bottomResources[j] = gameObject3.GetComponent<AgricolaOptionPopup_Resource>();
			this.m_bottomResources[j].SetActive(false);
			this.m_bottomResources[j].SetArrows(false, false);
			this.m_bottomResources[j].SetInteractable(false);
		}
		if (this.m_OptionListings != null)
		{
			for (int k = 0; k < this.m_OptionListings.Length; k++)
			{
				this.m_OptionListings[k].m_resourceTypes = new int[4];
				this.m_OptionListings[k].m_bLeftArrowSelected = new bool[2];
				this.m_OptionListings[k].m_bRightArrowSelected = new bool[2];
				this.m_OptionListings[k].m_bLeftArrowGrayed = new bool[2];
				this.m_OptionListings[k].m_bRightArrowGrayed = new bool[2];
			}
		}
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0001EFA4 File Offset: 0x0001D1A4
	private void HandleOnResourcePassThrough(AgricolaOptionPopup_Resource res, AgricolaOptionPopup_OptionListing cell, int index, bool bIsOn)
	{
		if (this.m_toggleSound != null)
		{
			this.m_toggleSound.Play();
		}
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_COUNTERS)
		{
			this.CalculateArrows();
			return;
		}
		if (cell.m_resources[0] == res)
		{
			cell.m_bIsActive = bIsOn;
		}
		else if (cell.m_resources[1] == res)
		{
			cell.m_bIsActive2 = bIsOn;
		}
		int num = 0;
		for (int i = 0; i < this.m_OptionListings.Length; i++)
		{
			if (this.m_OptionListings[i].m_bIsActive)
			{
				num++;
			}
			if (this.m_OptionListings[i].m_bIsActive2)
			{
				num++;
			}
		}
		if (num > this.m_maxSelectables)
		{
			if (this.m_lastSelectedRes != null)
			{
				this.m_lastSelectedRes.SetIsSelected(false);
				for (int j = 0; j < this.m_OptionListings.Length; j++)
				{
					if (this.m_OptionListings[j].m_resources[0] == this.m_lastSelectedRes)
					{
						this.m_OptionListings[j].m_bIsActive = false;
					}
					else if (this.m_OptionListings[j].m_resources[1] == this.m_lastSelectedRes)
					{
						this.m_OptionListings[j].m_bIsActive2 = false;
					}
				}
			}
			else if (cell.m_resources[0] == res)
			{
				cell.m_bIsActive = false;
			}
			else if (cell.m_resources[1] == res)
			{
				cell.m_bIsActive2 = false;
			}
		}
		this.m_lastSelectedRes = res;
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION)
		{
			uint num2 = 15U << (int)cell.m_flagIndex;
			this.m_resourceFlags &= ~num2;
			bool flag = cell.m_bIsActive;
			for (int k = 0; k < 10; k++)
			{
				int num3 = 11 * cell.m_mainResourceEntry + k;
				if (this.m_pEventResources.additionalResources[num3] > 0)
				{
					if (flag)
					{
						this.m_resourceFlags |= (uint)((uint)k << (int)cell.m_flagIndex);
						break;
					}
					flag = !flag;
				}
			}
		}
		this.CalculateResources(false);
		if (this.m_UsingMode != AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION)
		{
			CResourceCache thisCache = new CResourceCache();
			bool active = false;
			for (int l = 0; l < GameOptions.m_OptionCount; l++)
			{
				if (GameOptions.m_GameOption[l].selectionHint == 40963 && GameOptions.m_GameOption[l].isHidden == 0 && AgricolaLib.GetActionDefinition((uint)GameOptions.m_GameOption[l].selectionID, this.m_bufPtr, 1024) != 0)
				{
					GameActionDefinition gameActionDefinition = (GameActionDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameActionDefinition));
					CResourceCache cresourceCache = new CResourceCache(gameActionDefinition.resources);
					if (gameActionDefinition.displayName.Contains("Pay1Food"))
					{
						CResourceCache cresourceCache2 = cresourceCache;
						int value = cresourceCache2[0] - 1;
						cresourceCache2[0] = value;
					}
					if (cresourceCache == this.m_totalCache && this.m_totalCache != thisCache)
					{
						active = true;
						break;
					}
				}
			}
			this.m_confirmButton.SetActive(active);
		}
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0001F29C File Offset: 0x0001D49C
	private void HandleOnCellCallback(AgricolaOptionPopup_OptionListing cell, bool bIsOn)
	{
		if (this.m_toggleSound != null)
		{
			this.m_toggleSound.Play();
		}
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION)
		{
			for (int i = 0; i < this.m_OptionListings.Length; i++)
			{
				if (this.m_OptionListings[i] == cell)
				{
					this.m_selectedIndex = i;
					break;
				}
			}
			cell.m_bIsActive = cell.m_purchaseListToggle.isOn;
			if (this.m_pEventResources.additionalResourceType[cell.m_mainResourceEntry] == 2U)
			{
				uint num = 15U << (int)cell.m_flagIndex;
				this.m_resourceFlags &= ~num;
				bool flag = cell.m_bIsActive;
				for (int j = 0; j < 10; j++)
				{
					int num2 = 11 * cell.m_mainResourceEntry + j;
					if (this.m_pEventResources.additionalResources[num2] > 0)
					{
						if (flag)
						{
							this.m_resourceFlags |= (uint)((uint)j << (int)cell.m_flagIndex);
							break;
						}
						flag = !flag;
					}
				}
			}
			else
			{
				this.m_resourceFlags ^= 1U << (int)cell.m_flagIndex;
			}
		}
		else
		{
			this.ClearRightCardLocator();
			if (bIsOn)
			{
				for (int k = 0; k < this.m_OptionListings.Length; k++)
				{
					if (this.m_OptionListings[k] == cell)
					{
						this.m_selectedIndex = k;
						break;
					}
				}
				if (this.m_gameController != null && this.m_RightCardsLocator != null)
				{
					cell.GenerateCardsFromIDs(this.m_gameController.GetCardManager(), this.m_gameController.GetCardInPlayManager());
					for (int l = 0; l < cell.m_affectingCardObjs.Count; l++)
					{
						if (cell.m_affectingCardObjs[l] != null)
						{
							cell.m_affectingCardObjs[l].SetActive(true);
							AnimateObject component = cell.m_affectingCardObjs[l].GetComponent<AnimateObject>();
							if (component != null)
							{
								this.m_RightCardsLocator.PlaceAnimateObject(component, true, true, false);
							}
						}
					}
				}
			}
			else
			{
				for (int m = 0; m < this.m_OptionListings.Length; m++)
				{
					if (this.m_OptionListings[m] == cell && this.m_selectedIndex == m)
					{
						this.m_selectedIndex = -1;
						break;
					}
				}
			}
		}
		this.CalculateResources(false);
		this.m_confirmButton.SetActive(this.m_selectedIndex != -1);
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0001F4FC File Offset: 0x0001D6FC
	private void ClearRightCardLocator()
	{
		if (this.m_gameController != null && this.m_RightCardsLocator != null)
		{
			AgricolaCardManager cardManager = this.m_gameController.GetCardManager();
			while (this.m_RightCardsLocator.transform.childCount > 0)
			{
				AgricolaCard component = this.m_RightCardsLocator.transform.GetChild(0).gameObject.GetComponent<AgricolaCard>();
				if (component != null)
				{
					cardManager.PlaceCardInCardLimbo(component);
				}
			}
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0001F573 File Offset: 0x0001D773
	private void OnDestroy()
	{
		if (this.m_bInitialized)
		{
			this.m_bInitialized = false;
			this.m_hDataBuffer.Free();
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0001F590 File Offset: 0x0001D790
	private void Reset()
	{
		for (int i = 0; i < this.m_topResources.Length; i++)
		{
			if (this.m_topResources[i] != null)
			{
				this.m_topResources[i].SetActive(false);
				this.m_topResources[i].SetArrows(false, false);
				this.m_topResources[i].SetInteractable(false);
			}
		}
		for (int j = 0; j < this.m_bottomResources.Length; j++)
		{
			if (this.m_bottomResources[j] != null)
			{
				this.m_bottomResources[j].SetActive(false);
				this.m_bottomResources[j].SetArrows(false, false);
				this.m_bottomResources[j].SetInteractable(false);
			}
		}
		for (int k = 0; k < this.m_OptionListings.Length; k++)
		{
			this.m_OptionListings[k].Reset();
		}
		this.m_topResourceDisplay.SetActive(false);
		this.m_bottomResourceDisplay.SetActive(false);
		if (this.m_LeftCard != null)
		{
			UnityEngine.Object.Destroy(this.m_LeftCard);
			this.m_LeftCard = null;
		}
		this.m_startingCache.Clear();
		this.m_totalCache.Clear();
		this.m_confirmButton.SetActive(false);
		this.m_maxSelectables = 1;
		this.m_currentListSize = 0;
		this.m_selectedIndex = -1;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00003022 File Offset: 0x00001222
	private void SortPurchaseListMode()
	{
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001F6C8 File Offset: 0x0001D8C8
	private void CalculateResources(bool secondPass = false)
	{
		int num = 0;
		this.m_totalCache.Clear();
		this.m_totalCache += this.m_startingCache;
		this.m_topResourceDisplay.SetActive(false);
		this.m_bottomResourceDisplay.SetActive(false);
		for (int i = 0; i < this.m_topResources.Length; i++)
		{
			if (this.m_topResources[i] != null)
			{
				this.m_topResources[i].SetActive(false);
				this.m_topResources[i].SetArrows(false, false);
				this.m_topResources[i].SetInteractable(false);
			}
		}
		for (int j = 0; j < this.m_bottomResources.Length; j++)
		{
			if (this.m_bottomResources[j] != null)
			{
				this.m_bottomResources[j].SetActive(false);
				this.m_bottomResources[j].SetArrows(false, false);
				this.m_bottomResources[j].SetInteractable(false);
			}
		}
		for (int k = 0; k < 10; k++)
		{
			if (this.m_startingCache[k] != 0)
			{
				this.m_topResourceTypes[num] = k;
				this.m_topResources[num].SetActive(true);
				this.m_topResources[num].SetResourceData((EResourceType)k, this.m_startingCache[k]);
				this.m_topResourceDisplay.SetActive(true);
				num++;
			}
		}
		this.m_numTopResourceTypes = num;
		if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_SELECTION)
		{
			for (int l = 0; l < this.m_currentListSize; l++)
			{
				if (this.m_OptionListings[l].m_mainResourceEntry != -1)
				{
					if (this.m_pEventResources.additionalResourceType[this.m_OptionListings[l].m_mainResourceEntry] == 2U)
					{
						bool flag = this.m_OptionListings[l].m_bIsActive;
						for (int m = 0; m < 10; m++)
						{
							int num2 = 11 * this.m_OptionListings[l].m_mainResourceEntry + m;
							if (this.m_pEventResources.additionalResources[num2] > 0)
							{
								if (flag)
								{
									CResourceCache totalCache = this.m_totalCache;
									int num3 = m;
									totalCache[num3] += (int)this.m_pEventResources.additionalResources[num2];
								}
								flag = !flag;
							}
						}
					}
					else if (this.m_pEventResources.additionalResourceType[this.m_OptionListings[l].m_mainResourceEntry] != 7U && this.m_OptionListings[l].m_bIsActive)
					{
						for (int n = 0; n < 10; n++)
						{
							int num4 = 11 * this.m_OptionListings[l].m_mainResourceEntry + n;
							CResourceCache totalCache = this.m_totalCache;
							int num3 = n;
							totalCache[num3] += (int)this.m_pEventResources.additionalResources[num4];
						}
						if (this.m_pEventResources.costAmount[this.m_OptionListings[l].m_mainResourceEntry] > 0U && this.m_pEventResources.additionalResourceType[this.m_OptionListings[l].m_mainResourceEntry] != 5U && this.m_pEventResources.additionalResourceType[this.m_OptionListings[l].m_mainResourceEntry] != 6U)
						{
							CResourceCache totalCache = this.m_totalCache;
							int num3 = (int)this.m_pEventResources.costType[this.m_OptionListings[l].m_mainResourceEntry];
							totalCache[num3] -= (int)this.m_pEventResources.costAmount[this.m_OptionListings[l].m_mainResourceEntry];
						}
						for (int num5 = 0; num5 < this.m_OptionListings[l].m_linkedEntries.Count; num5++)
						{
							if (this.m_pEventResources.additionalResourceType[this.m_OptionListings[l].m_linkedEntries[num5]] == 5U)
							{
								CResourceCache totalCache = this.m_totalCache;
								int num3 = (int)this.m_pEventResources.costType[this.m_OptionListings[l].m_linkedEntries[num5]];
								totalCache[num3] -= (int)this.m_pEventResources.costAmount[this.m_OptionListings[l].m_linkedEntries[num5]];
							}
						}
					}
				}
			}
			for (int num6 = 0; num6 < this.m_currentListSize; num6++)
			{
				if (this.m_OptionListings[num6].m_bNoNegativeCost)
				{
					if (this.m_OptionListings[num6].m_bIsActive && this.m_totalCache[(int)this.m_pEventResources.costType[this.m_OptionListings[num6].m_mainResourceEntry]] < 0)
					{
						this.m_OptionListings[num6].m_bIsInteractable = false;
						this.m_OptionListings[num6].m_purchaseListToggle.interactable = false;
						for (int num7 = 0; num7 < 10; num7++)
						{
							int num8 = 11 * this.m_OptionListings[num6].m_mainResourceEntry + num7;
							CResourceCache totalCache = this.m_totalCache;
							int num3 = num7;
							totalCache[num3] -= (int)this.m_pEventResources.additionalResources[num8];
						}
						if (this.m_pEventResources.costAmount[this.m_OptionListings[num6].m_mainResourceEntry] > 0U)
						{
							CResourceCache totalCache = this.m_totalCache;
							int num3 = (int)this.m_pEventResources.costType[this.m_OptionListings[num6].m_mainResourceEntry];
							totalCache[num3] += (int)this.m_pEventResources.costAmount[this.m_OptionListings[num6].m_mainResourceEntry];
						}
						for (int num9 = 0; num9 < this.m_OptionListings[num6].m_linkedEntries.Count; num9++)
						{
							if (this.m_pEventResources.additionalResourceType[this.m_OptionListings[num6].m_linkedEntries[num9]] == 5U)
							{
								CResourceCache totalCache = this.m_totalCache;
								int num3 = (int)this.m_pEventResources.costType[this.m_OptionListings[num6].m_linkedEntries[num9]];
								totalCache[num3] += (int)this.m_pEventResources.costAmount[this.m_OptionListings[num6].m_linkedEntries[num9]];
							}
						}
					}
					else if (!this.m_OptionListings[num6].m_bIsActive && this.m_totalCache[(int)this.m_pEventResources.costType[this.m_OptionListings[num6].m_mainResourceEntry]] - (int)this.m_pEventResources.costAmount[this.m_OptionListings[num6].m_mainResourceEntry] < 0)
					{
						this.m_OptionListings[num6].m_bIsInteractable = false;
						this.m_OptionListings[num6].m_purchaseListToggle.interactable = false;
					}
					else
					{
						this.m_OptionListings[num6].m_bIsInteractable = true;
						this.m_OptionListings[num6].m_purchaseListToggle.interactable = true;
					}
				}
			}
			AgricolaLib.GetGamePlayerState(this.m_gameController.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
			GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
			CResourceCache cresourceCache = new CResourceCache();
			cresourceCache[0] = gamePlayerState.resourceCountFood;
			cresourceCache[1] = gamePlayerState.resourceCountWood;
			cresourceCache[2] = gamePlayerState.resourceCountClay;
			cresourceCache[3] = gamePlayerState.resourceCountStone;
			cresourceCache[4] = gamePlayerState.resourceCountReed;
			cresourceCache[5] = gamePlayerState.resourceCountGrain;
			cresourceCache[6] = gamePlayerState.resourceCountVeggie;
			cresourceCache[7] = gamePlayerState.resourceCountSheep;
			cresourceCache[8] = gamePlayerState.resourceCountWildBoar;
			cresourceCache[9] = gamePlayerState.resourceCountCattle;
			cresourceCache += this.m_totalCache;
			for (int num10 = 0; num10 < 10; num10++)
			{
				if (cresourceCache[num10] < 0 && !secondPass && this.m_selectedIndex != -1)
				{
					this.m_OptionListings[this.m_selectedIndex].m_purchaseListToggle.isOn = !this.m_OptionListings[this.m_selectedIndex].m_bIsActive;
					this.m_OptionListings[this.m_selectedIndex].m_bIsActive = !this.m_OptionListings[this.m_selectedIndex].m_bIsActive;
					this.CalculateResources(true);
					return;
				}
			}
		}
		else if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_COUNTERS)
		{
			this.m_totalCache.Clear();
			for (int num11 = 0; num11 < this.m_currentListSize; num11++)
			{
				this.m_OptionListings[num11].m_bLeftArrowGrayed[0] = (this.m_OptionListings[num11].m_bLeftArrowGrayed[1] = false);
				this.m_OptionListings[num11].m_bRightArrowGrayed[0] = (this.m_OptionListings[num11].m_bRightArrowGrayed[1] = false);
				if (this.m_OptionListings[num11].m_bIsActive)
				{
					this.m_totalCache += this.m_OptionListings[num11].m_resourceCache * (int)this.m_OptionListings[num11].m_countLeft;
				}
				if (this.m_OptionListings[num11].m_bIsActive2)
				{
					this.m_totalCache += this.m_OptionListings[num11].m_resourceCache2 * (int)this.m_OptionListings[num11].m_countRight;
				}
			}
			this.m_counterCurValue = 0U;
			for (int num12 = 0; num12 < 10; num12++)
			{
				if (this.m_counterMultiplierCache[num12] != 0)
				{
					this.m_counterCurValue += (uint)(this.m_totalCache[num12] * this.m_counterMultiplierCache[num12]);
				}
				else
				{
					this.m_counterCurValue += (uint)this.m_totalCache[num12];
				}
			}
			if ((this.m_bCounterCanSubmitUnderMaxValue && (this.m_counterMaxValue == 0U || this.m_counterCurValue <= this.m_counterMaxValue)) || (!this.m_bCounterCanSubmitUnderMaxValue && this.m_counterCurValue == this.m_counterMaxValue))
			{
				this.m_confirmButton.SetActive(true);
			}
			else
			{
				this.m_confirmButton.SetActive(false);
			}
		}
		else if (this.m_UsingMode == AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_ACTION)
		{
			for (int num13 = 0; num13 < this.m_currentListSize; num13++)
			{
				if (this.m_OptionListings[num13].m_bIsActive)
				{
					this.m_totalCache += this.m_OptionListings[num13].m_resourceCache;
				}
				if (this.m_OptionListings[num13].m_bIsActive2)
				{
					this.m_totalCache += this.m_OptionListings[num13].m_resourceCache2;
				}
			}
			bool active = false;
			for (int num14 = 0; num14 < GameOptions.m_OptionCount; num14++)
			{
				if (GameOptions.m_GameOption[num14].selectionHint == 40963 && GameOptions.m_GameOption[num14].isHidden == 0 && AgricolaLib.GetActionDefinition((uint)GameOptions.m_GameOption[num14].selectionID, this.m_bufPtr, 1024) != 0)
				{
					GameActionDefinition gameActionDefinition = (GameActionDefinition)Marshal.PtrToStructure(this.m_bufPtr, typeof(GameActionDefinition));
					CResourceCache cresourceCache2 = new CResourceCache(gameActionDefinition.resources);
					if (gameActionDefinition.displayName.Contains("Pay1Food"))
					{
						CResourceCache cresourceCache3 = cresourceCache2;
						int num3 = cresourceCache3[0] - 1;
						cresourceCache3[0] = num3;
					}
					if (cresourceCache2 == this.m_totalCache)
					{
						active = true;
						break;
					}
				}
			}
			CResourceCache cresourceCache4 = new CResourceCache();
			cresourceCache4.Clear();
			if (this.m_totalCache == cresourceCache4 && this.m_selectedIndex == -1)
			{
				active = false;
			}
			this.m_confirmButton.SetActive(active);
		}
		num = 0;
		int num15 = 0;
		while (num15 < 10 && num < 4)
		{
			if (this.m_totalCache[num15] != 0)
			{
				this.m_resourceTypes[num] = num15;
				this.m_bottomResources[num].SetActive(true);
				this.m_bottomResources[num].SetResourceData((EResourceType)num15, this.m_totalCache[num15]);
				this.m_bottomResourceDisplay.SetActive(true);
				num++;
			}
			num15++;
		}
		this.m_numResourceTypes = num;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00020294 File Offset: 0x0001E494
	private void CalculateArrows()
	{
		if (this.m_UsingMode != AgricolaOptionPopup.ListModes.E_MODE_RESOURCE_COUNTERS)
		{
			return;
		}
		AgricolaLib.GetGamePlayerState(this.m_gameController.GetLocalPlayerIndex(), this.m_bufPtr, 1024);
		GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
		CResourceCache cresourceCache = new CResourceCache();
		cresourceCache[0] = gamePlayerState.resourceCountFood;
		cresourceCache[1] = gamePlayerState.resourceCountWood;
		cresourceCache[2] = gamePlayerState.resourceCountClay;
		cresourceCache[3] = gamePlayerState.resourceCountStone;
		cresourceCache[4] = gamePlayerState.resourceCountReed;
		cresourceCache[5] = gamePlayerState.resourceCountGrain;
		cresourceCache[6] = gamePlayerState.resourceCountVeggie;
		cresourceCache[7] = gamePlayerState.resourceCountSheep;
		cresourceCache[8] = gamePlayerState.resourceCountWildBoar;
		cresourceCache[9] = gamePlayerState.resourceCountCattle;
		for (int i = 0; i < this.m_currentListSize; i++)
		{
			this.m_OptionListings[i].m_countLeft = (uint)this.m_OptionListings[i].m_resources[0].GetDisplayedResourceCount();
			if (this.m_OptionListings[i].m_numResourseTypes > 1U)
			{
				this.m_OptionListings[i].m_countRight = (uint)this.m_OptionListings[i].m_resources[1].GetDisplayedResourceCount();
			}
		}
		this.CalculateResources(false);
		for (int j = 0; j < this.m_currentListSize; j++)
		{
			bool bLeftOn = true;
			bool bRightOn = true;
			if (this.m_OptionListings[j].m_countLeft <= 0U)
			{
				bLeftOn = false;
			}
			int num = cresourceCache[this.m_OptionListings[j].m_resourceTypes[0]];
			if (!this.m_bCounterNotLimitedByPlayerResources && (long)num <= (long)((ulong)this.m_OptionListings[j].m_countLeft))
			{
				bRightOn = false;
			}
			int num2 = this.m_counterMultiplierCache[this.m_OptionListings[j].m_resourceTypes[0]];
			if (this.m_counterMaxValue > 0U && (ulong)this.m_counterCurValue + (ulong)((long)num2) > (ulong)this.m_counterMaxValue)
			{
				bRightOn = false;
			}
			this.m_OptionListings[j].m_resources[0].SetArrowInteractable(bLeftOn, bRightOn);
			if (this.m_OptionListings[j].m_numResourseTypes > 1U)
			{
				bLeftOn = true;
				bRightOn = true;
				if (this.m_OptionListings[j].m_countRight <= 0U)
				{
					bLeftOn = false;
				}
				num = cresourceCache[this.m_OptionListings[j].m_resourceTypes[1]];
				if (!this.m_bCounterNotLimitedByPlayerResources && (long)num <= (long)((ulong)this.m_OptionListings[j].m_countRight))
				{
					bRightOn = false;
				}
				num2 = this.m_counterMultiplierCache[this.m_OptionListings[j].m_resourceTypes[1]];
				if (this.m_counterMaxValue > 0U && (ulong)this.m_counterCurValue + (ulong)((long)num2) > (ulong)this.m_counterMaxValue)
				{
					bRightOn = false;
				}
				this.m_OptionListings[j].m_resources[1].SetArrowInteractable(bLeftOn, bRightOn);
			}
		}
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00020544 File Offset: 0x0001E744
	private bool IgnoreHint(ushort hint, AgricolaOptionPopup.OptionPopupMode displayMode)
	{
		if (displayMode == AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_SHOW_ALL)
		{
			return false;
		}
		if (displayMode == AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_PRUCHASE_LIST)
		{
			return hint != 41026 && hint != 41024;
		}
		if (displayMode == AgricolaOptionPopup.OptionPopupMode.E_OPTIONPOPUP_MODE_SHOW_FOOD)
		{
			return hint != 40992;
		}
		for (int i = 0; i < AgricolaOptionPopup.s_IgnoreList.Length; i++)
		{
			if (AgricolaOptionPopup.s_IgnoreList[i] == hint)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000332 RID: 818
	private static ushort[] s_IgnoreList = new ushort[]
	{
		40961,
		40962,
		40964,
		40965,
		40966,
		40967,
		41019,
		41021,
		41022,
		41020,
		41018,
		41017,
		40976,
		40977,
		40991,
		40978,
		40979,
		40980,
		40981,
		41023,
		40984,
		40982,
		40983,
		40984,
		40986,
		40987,
		40988,
		40989,
		40990,
		40992,
		40993,
		40994,
		40995,
		40998,
		40999,
		41024,
		41026,
		41056,
		41057,
		41040,
		41041,
		41042
	};

	// Token: 0x04000333 RID: 819
	private const uint k_AdditionalResourceGain = 0U;

	// Token: 0x04000334 RID: 820
	private const uint k_AdditionalResourceOptionalGain = 1U;

	// Token: 0x04000335 RID: 821
	private const uint k_AdditionalResourceSelection = 2U;

	// Token: 0x04000336 RID: 822
	private const uint k_AdditionalResourceLeaveOnSpace = 3U;

	// Token: 0x04000337 RID: 823
	private const uint k_AdditionalResourcePayCost = 4U;

	// Token: 0x04000338 RID: 824
	private const uint k_AdditionalResourceGiveToOpponent = 5U;

	// Token: 0x04000339 RID: 825
	private const uint k_AdditionalResourceGiveToOpponentFromSupply = 6U;

	// Token: 0x0400033A RID: 826
	private const uint k_AdditionalResourceOpponentMayBuy = 7U;

	// Token: 0x0400033B RID: 827
	private const uint s_ResourceBuildFencesMask = 28672U;

	// Token: 0x0400033C RID: 828
	private const uint s_ResourceBuildRoomMask = 36864U;

	// Token: 0x0400033D RID: 829
	private const uint s_ResourceRenovateMask = 40960U;

	// Token: 0x0400033E RID: 830
	private const uint s_ResourceImprovementMask = 49152U;

	// Token: 0x0400033F RID: 831
	private const uint s_ResourceIndexMask = 15U;

	// Token: 0x04000340 RID: 832
	private const int s_maxResourcesDisplay = 4;

	// Token: 0x04000341 RID: 833
	private const int k_maxDataSize = 1024;

	// Token: 0x04000342 RID: 834
	[SerializeField]
	private GameObject m_confirmButton;

	// Token: 0x04000343 RID: 835
	[SerializeField]
	private GameObject m_noneButton;

	// Token: 0x04000344 RID: 836
	[SerializeField]
	private GameObject m_undoButton;

	// Token: 0x04000345 RID: 837
	[SerializeField]
	private GameObject m_cellContainer;

	// Token: 0x04000346 RID: 838
	[SerializeField]
	private GameObject m_topResourceContainer;

	// Token: 0x04000347 RID: 839
	[SerializeField]
	private GameObject m_bottomResourceContainer;

	// Token: 0x04000348 RID: 840
	[SerializeField]
	private ToggleGroup m_toggleGroup;

	// Token: 0x04000349 RID: 841
	[SerializeField]
	private AgricolaAnimationLocator m_LeftCardLocator;

	// Token: 0x0400034A RID: 842
	[SerializeField]
	private AgricolaAnimationLocator m_RightCardsLocator;

	// Token: 0x0400034B RID: 843
	[SerializeField]
	private AudioSource m_toggleSound;

	// Token: 0x0400034C RID: 844
	[SerializeField]
	private GameObject m_resourcePrefab;

	// Token: 0x0400034D RID: 845
	[SerializeField]
	private GameObject m_cellPrefab;

	// Token: 0x0400034E RID: 846
	private AgricolaOptionPopup.ListModes m_UsingMode;

	// Token: 0x0400034F RID: 847
	private AgricolaOptionPopup_Resource m_lastSelectedRes;

	// Token: 0x04000350 RID: 848
	private bool m_bInitialized;

	// Token: 0x04000351 RID: 849
	private int m_maxSelectables = 1;

	// Token: 0x04000352 RID: 850
	private int m_currentListSize;

	// Token: 0x04000353 RID: 851
	private int m_selectedIndex = -1;

	// Token: 0x04000354 RID: 852
	private byte[] m_dataBuffer;

	// Token: 0x04000355 RID: 853
	private GCHandle m_hDataBuffer;

	// Token: 0x04000356 RID: 854
	private IntPtr m_bufPtr;

	// Token: 0x04000357 RID: 855
	private AgricolaOptionPopup_OptionListing[] m_OptionListings;

	// Token: 0x04000358 RID: 856
	private GameObject m_LeftCard;

	// Token: 0x04000359 RID: 857
	[SerializeField]
	private GameObject m_topResourceDisplay;

	// Token: 0x0400035A RID: 858
	[SerializeField]
	private GameObject m_bottomResourceDisplay;

	// Token: 0x0400035B RID: 859
	private AgricolaOptionPopup_Resource[] m_topResources;

	// Token: 0x0400035C RID: 860
	private AgricolaOptionPopup_Resource[] m_bottomResources;

	// Token: 0x0400035D RID: 861
	private int m_numTopResourceTypes;

	// Token: 0x0400035E RID: 862
	private uint m_resourceFlags;

	// Token: 0x0400035F RID: 863
	private int[] m_topResourceTypes = new int[4];

	// Token: 0x04000360 RID: 864
	private int m_numResourceTypes;

	// Token: 0x04000361 RID: 865
	private int[] m_resourceTypes = new int[4];

	// Token: 0x04000362 RID: 866
	private GameQueryAdditionalResources m_pEventResources;

	// Token: 0x04000363 RID: 867
	private CResourceCache m_startingCache;

	// Token: 0x04000364 RID: 868
	private CResourceCache m_totalCache;

	// Token: 0x04000365 RID: 869
	private CResourceCache m_counterMultiplierCache;

	// Token: 0x04000366 RID: 870
	private uint m_counterCurValue;

	// Token: 0x04000367 RID: 871
	private uint m_counterMaxValue;

	// Token: 0x04000368 RID: 872
	private ushort m_counterStoredHint;

	// Token: 0x04000369 RID: 873
	private bool m_bCounterCanSubmitUnderMaxValue;

	// Token: 0x0400036A RID: 874
	private bool m_bCounterNotLimitedByPlayerResources;

	// Token: 0x0400036B RID: 875
	private AgricolaGame m_gameController;

	// Token: 0x0200075D RID: 1885
	public enum OptionPopupMode
	{
		// Token: 0x04002B6F RID: 11119
		E_OPTIONPOPUP_MODE_NONE,
		// Token: 0x04002B70 RID: 11120
		E_OPTIONPOPUP_MODE_SHOW_FOOD,
		// Token: 0x04002B71 RID: 11121
		E_OPTIONPOPUP_MODE_SHOW_NON_FOOD,
		// Token: 0x04002B72 RID: 11122
		E_OPTIONPOPUP_MODE_PRUCHASE_LIST,
		// Token: 0x04002B73 RID: 11123
		E_OPTIONPOPUP_MODE_SHOW_ALL,
		// Token: 0x04002B74 RID: 11124
		E_OPTIONPOPUP_MODE_SHOW_PLAYERCHOICES
	}

	// Token: 0x0200075E RID: 1886
	private enum ListModes
	{
		// Token: 0x04002B76 RID: 11126
		E_MODE_OFF,
		// Token: 0x04002B77 RID: 11127
		E_MODE_REGULAR,
		// Token: 0x04002B78 RID: 11128
		E_MODE_PURCHASE_LIST,
		// Token: 0x04002B79 RID: 11129
		E_MODE_RESOURCE_SELECTION,
		// Token: 0x04002B7A RID: 11130
		E_MODE_RESOURCE_ACTION,
		// Token: 0x04002B7B RID: 11131
		E_MODE_PLAYERCHOICES,
		// Token: 0x04002B7C RID: 11132
		E_MODE_RESOURCE_COUNTERS
	}

	// Token: 0x0200075F RID: 1887
	private struct ActionResource
	{
		// Token: 0x060041BB RID: 16827 RVA: 0x0013C421 File Offset: 0x0013A621
		public void IncrementOccurences()
		{
			this.occurences++;
		}

		// Token: 0x04002B7D RID: 11133
		public int type;

		// Token: 0x04002B7E RID: 11134
		public int count;

		// Token: 0x04002B7F RID: 11135
		public int occurences;
	}

	// Token: 0x02000760 RID: 1888
	public struct OptionPopupRestriction
	{
		// Token: 0x04002B80 RID: 11136
		public int instanceID;

		// Token: 0x04002B81 RID: 11137
		public ushort selectionHint;

		// Token: 0x04002B82 RID: 11138
		public EResourceType resource;

		// Token: 0x04002B83 RID: 11139
		public int resourceUseLimit;

		// Token: 0x04002B84 RID: 11140
		public bool bUseRestriction;
	}
}
