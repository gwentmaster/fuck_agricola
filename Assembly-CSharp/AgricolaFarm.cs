using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameEvent;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000036 RID: 54
public class AgricolaFarm : MonoBehaviour
{
	// Token: 0x06000295 RID: 661 RVA: 0x0000F0BA File Offset: 0x0000D2BA
	public int GetDisplayedPlayerIndex()
	{
		return this.m_displayedPlayerIndex;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000F0C2 File Offset: 0x0000D2C2
	public int GetDisplayedPlayerInstanceID()
	{
		return this.m_displayedPlayerInstanceID;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000F0CA File Offset: 0x0000D2CA
	public bool GetIsDisplayingLocalPlayer()
	{
		return this.m_bIsLocalPlayer;
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000F0D2 File Offset: 0x0000D2D2
	public bool GetIsFencingMode()
	{
		return this.m_bIsFencingMode;
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000F0DA File Offset: 0x0000D2DA
	public bool GetIsFencingArrangementValid()
	{
		return this.m_bIsFencingMode && this.m_bIsFencingValid;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000F0EC File Offset: 0x0000D2EC
	public bool GetIsFeedingMode()
	{
		return this.m_bIsFeedingMode;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000F0F4 File Offset: 0x0000D2F4
	private void Awake()
	{
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		if (this.m_displayedPlayerIndex == 0)
		{
			this.m_displayedPlayerIndex = AgricolaLib.GetLocalPlayerIndex();
			this.m_displayedPlayerInstanceID = AgricolaLib.GetLocalPlayerInstanceID();
		}
		this.RebuildFarmToDisplayedPlayer();
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000F128 File Offset: 0x0000D328
	private void Start()
	{
		if (this.m_DragManager != null)
		{
			this.m_DragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.BeginDragCallback));
			this.m_DragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.EndDragCallback));
			if (this.m_farmTiles != null)
			{
				for (int i = 0; i < this.m_farmTiles.Length; i++)
				{
					AgricolaFarmTile_Locators locators = this.m_farmTiles[i].GetLocators();
					if (locators != null)
					{
						DragTargetZone component = locators.GetComponent<DragTargetZone>();
						if (component != null)
						{
							this.m_DragManager.AddOnBeginDragCallback(new DragManager.DragManagerCallback(this.m_farmTiles[i].BeginDragCallback));
							this.m_DragManager.AddOnEndDragCallback(new DragManager.DragManagerCallback(this.m_farmTiles[i].EndDragCallback));
							component.AddOnDropCallback(new DragTargetZone.OnDropCallback(this.m_farmTiles[i].OnDropCallback));
						}
					}
				}
			}
		}
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0000F214 File Offset: 0x0000D414
	private void OnDestroy()
	{
		if (this.m_bInitialized)
		{
			this.m_bInitialized = false;
			this.m_hDataBuffer.Free();
		}
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0000F230 File Offset: 0x0000D430
	public void RegisterEventHandlers(GameEventBuffer game_event_buffer)
	{
		if (game_event_buffer == null)
		{
			return;
		}
		game_event_buffer.RegisterEventHandler(8, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventCardInPlayStatus));
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0000F24C File Offset: 0x0000D44C
	public void RegisterAnimationLocators(AgricolaAnimationManager animManager)
	{
		if (animManager != null)
		{
			for (int i = 0; i < this.m_farmTiles.Length; i++)
			{
				if (this.m_farmTiles[i] != null)
				{
					AgricolaFarmTile_Locators locators = this.m_farmTiles[i].GetLocators();
					if (locators != null)
					{
						animManager.SetAnimationLocator(17, i, locators.GetLocator(4).GetComponent<AgricolaAnimationLocator>());
					}
				}
			}
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
	private void Update()
	{
		this.ResolveTileUpdate();
		if (this.m_displayedPlayerIndex != 0)
		{
			AgricolaLib.GetGamePlayerFarmState(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
			GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
			if (this.m_UpperHudScore != null)
			{
				this.m_UpperHudScore.text = gamePlayerFarmState.currentScore.ToString();
			}
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000F328 File Offset: 0x0000D528
	private void Init()
	{
		this.m_bInitialized = true;
		this.m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		for (int i = 0; i < this.m_farmTiles.Length; i++)
		{
			this.m_farmTiles[i].Init(this, i);
		}
		for (int j = 0; j < this.m_validFencesBitfield.Length; j++)
		{
			this.m_validFencesBitfield[j] = false;
			this.m_proposedFencesBitfield[j] = false;
			this.m_preBuiltFencesBitfield[j] = false;
		}
		this.m_dataBuffer = new byte[1024];
		this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
		this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
	public bool GetIsDraggingAnimal()
	{
		return this.m_draggingAnimal != null && this.m_draggingAnimalContainerRulesIndex != -2;
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0000F3F3 File Offset: 0x0000D5F3
	public void SetAnimalDragging(AgricolaAnimal animal, int rulesIndex)
	{
		this.m_draggingAnimal = animal;
		this.m_draggingAnimalContainerRulesIndex = rulesIndex;
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0000F404 File Offset: 0x0000D604
	public GameObject GetFieldGrainInstance()
	{
		GameObject result = null;
		if (this.m_fieldGrainPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<GameObject>(this.m_fieldGrainPrefab);
		}
		return result;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000F430 File Offset: 0x0000D630
	public GameObject GetFieldVeggiInstance()
	{
		GameObject result = null;
		if (this.m_fieldVeggiPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<GameObject>(this.m_fieldVeggiPrefab);
		}
		return result;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0000F45A File Offset: 0x0000D65A
	public void UpdateBeggingCardSign(int count)
	{
		if (this.m_CardOrchardBeggingCardSign != null && this.m_BeggingCardText != null)
		{
			this.m_CardOrchardBeggingCardSign.SetActive(count != 0);
			this.m_BeggingCardText.text = count.ToString();
		}
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0000F499 File Offset: 0x0000D699
	public GameObject CreateSheepInstance()
	{
		return this.CreateAnimalInstance(this.m_sheepPrefab);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0000F4A7 File Offset: 0x0000D6A7
	public GameObject CreateWildBoarInstance()
	{
		return this.CreateAnimalInstance(this.m_boarPrefab);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0000F4B5 File Offset: 0x0000D6B5
	public GameObject CreateCattleInstance()
	{
		return this.CreateAnimalInstance(this.m_cattlePrefab);
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
	private GameObject CreateAnimalInstance(GameObject prefab)
	{
		if (prefab == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		this.m_animals.Add(gameObject.GetComponent<AgricolaAnimal>());
		gameObject.GetComponent<AgricolaAnimal>().SetFarm(this);
		if (this.m_AnimationManager != null)
		{
			AnimateObject component = gameObject.GetComponent<AnimateObject>();
			if (component != null)
			{
				component.SetAnimationManager(this.m_AnimationManager);
			}
		}
		if (this.m_DragManager != null)
		{
			DragObject component2 = gameObject.GetComponent<DragObject>();
			if (component2 != null)
			{
				component2.SetDragManager(this.m_DragManager);
			}
		}
		this.SetAreAnimalsDraggable();
		this.PlaceAnimalInLimbo(gameObject.GetComponent<AgricolaAnimal>());
		return gameObject;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000F567 File Offset: 0x0000D767
	public void Debug_CreateSheepInLimbo()
	{
		this.CreateSheepInstance().transform.SetParent(this.m_animalLimbo.transform);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0000F584 File Offset: 0x0000D784
	public void PlaceAllAnimalsInLimbo()
	{
		for (int i = 0; i < this.m_animals.Count; i++)
		{
			if (this.m_animals[i] == null)
			{
				this.m_animals.RemoveAt(i--);
			}
			else
			{
				this.m_animals[i].gameObject.transform.SetParent(this.m_animalLimbo.transform);
				this.m_animals[i].ResetRotationAndScale();
				this.m_animals[i].SetContainerIndex(-2);
			}
		}
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0000F61A File Offset: 0x0000D81A
	public void PlaceAnimalInLimbo(AgricolaAnimal animal)
	{
		animal.gameObject.transform.SetParent(this.m_animalLimbo.transform);
		animal.ResetRotationAndScale();
		animal.SetContainerIndex(-2);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0000F648 File Offset: 0x0000D848
	public int GetActiveAnimalsCount(EResourceType animalType)
	{
		int num = 0;
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			if (animalType == EResourceType.SHEEP)
			{
				num += (int)this.m_animalContainers[i].m_resources.sheep;
			}
			else if (animalType == EResourceType.WILDBOAR)
			{
				num += (int)this.m_animalContainers[i].m_resources.boar;
			}
			else if (animalType == EResourceType.CATTLE)
			{
				num += (int)this.m_animalContainers[i].m_resources.cattle;
			}
		}
		ExcessAnimalTray excessAnimalTray = this.m_gameController.GetExcessAnimalTray();
		return num + excessAnimalTray.GetExcessAnimalCount(animalType);
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
	public void HandleAnimalDropOnContainer(AgricolaAnimal animal, int tileIndex, int improvementID)
	{
		if (animal == null)
		{
			return;
		}
		int containerIndex = animal.GetContainerIndex();
		if (containerIndex >= 0)
		{
			for (int i = 0; i < this.m_animalContainers.Count; i++)
			{
				if (this.m_animalContainers[i].m_rulesIndex == containerIndex && this.m_animalContainers[i].RemoveAnimal(animal, animal.GetAnimalType()) != null)
				{
					break;
				}
			}
		}
		else if (containerIndex == -1)
		{
			this.m_gameController.GetExcessAnimalTray().ModifyExcessAnimalCount(animal.GetAnimalType(), -1);
		}
		if (tileIndex < 0 && improvementID < 0)
		{
			animal.gameObject.transform.SetParent(this.m_animalLimbo.transform);
			animal.ResetRotationAndScale();
			animal.SetContainerIndex(-2);
			if (tileIndex == -1)
			{
				this.m_gameController.GetExcessAnimalTray().ModifyExcessAnimalCount(animal.GetAnimalType(), 1);
				return;
			}
		}
		else if (tileIndex >= 0 && improvementID < 0)
		{
			int ifTileHasAnimalContainer = this.GetIfTileHasAnimalContainer(tileIndex);
			if (ifTileHasAnimalContainer != -1)
			{
				this.m_animalContainers[ifTileHasAnimalContainer].AddAnimal(animal, animal.GetAnimalType(), this.m_farmTiles[tileIndex]);
				return;
			}
		}
		else if (tileIndex < 0)
		{
		}
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0000F7F4 File Offset: 0x0000D9F4
	public void SetAreAnimalsDraggable()
	{
		bool animalsDraggable = false;
		if (this.m_displayedPlayerIndex == AgricolaLib.GetLocalPlayerIndex() && (GameOptions.IsSelectableHint(41040) || GameOptions.IsSelectableHint(40992)))
		{
			animalsDraggable = true;
		}
		this.SetAnimalsDraggable(animalsDraggable);
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x0000F834 File Offset: 0x0000DA34
	private void SetAnimalsDraggable(bool bDraggable)
	{
		for (int i = 0; i < this.m_animals.Count; i++)
		{
			this.m_animals[i].SetIsDraggable(bDraggable);
		}
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0000F86C File Offset: 0x0000DA6C
	public AgricolaAnimal GetAnimalInLimbo(EResourceType animalType, bool bCreateIfNecessary = false)
	{
		for (int i = 0; i < this.m_animals.Count; i++)
		{
			if (this.m_animals[i] != null && this.m_animals[i].GetAnimalType() == animalType && this.m_animals[i].gameObject.transform.parent == this.m_animalLimbo.transform)
			{
				return this.m_animals[i];
			}
		}
		if (bCreateIfNecessary)
		{
			GameObject gameObject = null;
			if (animalType == EResourceType.SHEEP)
			{
				gameObject = this.CreateSheepInstance();
			}
			else if (animalType == EResourceType.WILDBOAR)
			{
				gameObject = this.CreateWildBoarInstance();
			}
			else if (animalType == EResourceType.CATTLE)
			{
				gameObject = this.CreateCattleInstance();
			}
			if (gameObject != null)
			{
				return gameObject.GetComponent<AgricolaAnimal>();
			}
		}
		return null;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0000F92D File Offset: 0x0000DB2D
	public bool CanPlaceAnimalInConatiner(EResourceType animalType, int containerRulesId, int count)
	{
		return this.GetAnimalContainerCapacity(animalType, containerRulesId) >= count;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000F940 File Offset: 0x0000DB40
	public int GetAnimalContainerCapacity(EResourceType animalType, int containerRulesId)
	{
		int[] array = new int[32];
		int[] array2 = new int[32];
		int[] array3 = new int[32];
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			if (this.m_animalContainers[i] != null)
			{
				int rulesIndex = this.m_animalContainers[i].m_rulesIndex;
				array[rulesIndex] = (int)this.m_animalContainers[i].m_resources.sheep;
				array2[rulesIndex] = (int)this.m_animalContainers[i].m_resources.boar;
				array3[rulesIndex] = (int)this.m_animalContainers[i].m_resources.cattle;
			}
		}
		if (this.m_draggingAnimal != null && this.m_draggingAnimalContainerRulesIndex >= 0)
		{
			if (this.m_draggingAnimal.GetAnimalType() == EResourceType.SHEEP)
			{
				array[this.m_draggingAnimalContainerRulesIndex]--;
			}
			else if (this.m_draggingAnimal.GetAnimalType() == EResourceType.WILDBOAR)
			{
				array2[this.m_draggingAnimalContainerRulesIndex]--;
			}
			else if (this.m_draggingAnimal.GetAnimalType() == EResourceType.CATTLE)
			{
				array3[this.m_draggingAnimalContainerRulesIndex]--;
			}
		}
		return AgricolaLib.GetAnimalContainerCapicity(array, array2, array3, containerRulesId, (int)animalType);
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000FA70 File Offset: 0x0000DC70
	public void CheckForUpdatedAnimalContainers()
	{
		if (!base.gameObject.activeSelf || !this.m_bIsLocalPlayer || this.m_bIsSubmittingPastures)
		{
			return;
		}
		AgricolaLib.GetGamePlayerAnimalContainers(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
		GamePlayerAnimalContainers gamePlayerAnimalContainers = (GamePlayerAnimalContainers)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerAnimalContainers));
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		this.m_orphanedAnimalList.Clear();
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			int rulesIndex = this.m_animalContainers[i].m_rulesIndex;
			if (rulesIndex >= gamePlayerAnimalContainers.containerCount || gamePlayerAnimalContainers.type[rulesIndex] == 0)
			{
				num += (int)this.m_animalContainers[i].m_resources.sheep;
				num2 += (int)this.m_animalContainers[i].m_resources.boar;
				num3 += (int)this.m_animalContainers[i].m_resources.cattle;
				this.m_animalContainers[i].RemoveAllAnimalsToLimbo();
				this.m_animalContainers.RemoveAt(i--);
			}
		}
		for (int j = 0; j < gamePlayerAnimalContainers.containerCount; j++)
		{
			if (gamePlayerAnimalContainers.type[j] != 0)
			{
				bool flag = false;
				int k = 0;
				while (k < this.m_animalContainers.Count)
				{
					if (this.m_animalContainers[k].m_rulesIndex == j)
					{
						flag = true;
						int tilesBitfield = this.m_animalContainers[k].m_tilesBitfield;
						this.m_animalContainers[k].ClearTiles();
						if (this.m_animalContainers[k].m_type == AnimalContainerType.ROOM || this.m_animalContainers[k].m_type == AnimalContainerType.STABLE)
						{
							this.m_animalContainers[k].AddTile(this.m_farmTiles[(int)gamePlayerAnimalContainers.id[j]]);
						}
						else if (this.m_animalContainers[k].m_type == AnimalContainerType.PASTURE)
						{
							for (int l = 0; l < this.m_farmTiles.Length; l++)
							{
								if (this.m_farmTiles[l].GetPastureIndex() == (int)this.m_animalContainers[k].m_id)
								{
									this.m_animalContainers[k].AddTile(this.m_farmTiles[l]);
								}
							}
						}
						if (tilesBitfield != this.m_animalContainers[k].m_tilesBitfield)
						{
							for (int m = 0; m < this.m_farmTiles.Length; m++)
							{
								if ((1 << m & tilesBitfield) != 0 && (1 << m & this.m_animalContainers[k].m_tilesBitfield) == 0)
								{
									AgricolaFarmTile_Locators locators = this.m_farmTiles[m].GetLocators();
									for (int n = 0; n < locators.GetLocatorCount(); n++)
									{
										GameObject objectAtLocator = locators.GetObjectAtLocator(n);
										if (objectAtLocator != null)
										{
											AgricolaAnimal component = objectAtLocator.GetComponent<AgricolaAnimal>();
											if (component != null)
											{
												this.m_animalContainers[k].RemoveAnimal(component, component.GetAnimalType());
												this.m_orphanedAnimalList.Add(new Tuple<AgricolaAnimal, int>(component, m));
											}
										}
									}
								}
							}
							break;
						}
						break;
					}
					else
					{
						k++;
					}
				}
				if (!flag)
				{
					AgricolaAnimalContainer agricolaAnimalContainer = new AgricolaAnimalContainer();
					agricolaAnimalContainer.SetParent(this);
					agricolaAnimalContainer.m_rulesIndex = j;
					agricolaAnimalContainer.m_id = gamePlayerAnimalContainers.id[j];
					agricolaAnimalContainer.m_type = (AnimalContainerType)gamePlayerAnimalContainers.type[j];
					agricolaAnimalContainer.m_capacity = gamePlayerAnimalContainers.capacity[j];
					agricolaAnimalContainer.m_capacityType = (AnimalCapacityType)gamePlayerAnimalContainers.capacityType[j];
					agricolaAnimalContainer.m_resourceTotalsFromRules.sheep = gamePlayerAnimalContainers.sheep[j];
					agricolaAnimalContainer.m_resourceTotalsFromRules.boar = gamePlayerAnimalContainers.boar[j];
					agricolaAnimalContainer.m_resourceTotalsFromRules.cattle = gamePlayerAnimalContainers.cattle[j];
					if (agricolaAnimalContainer.m_type == AnimalContainerType.ROOM || agricolaAnimalContainer.m_type == AnimalContainerType.STABLE)
					{
						agricolaAnimalContainer.AddTile(this.m_farmTiles[(int)gamePlayerAnimalContainers.id[j]]);
					}
					else if (agricolaAnimalContainer.m_type == AnimalContainerType.PASTURE)
					{
						for (int num4 = 0; num4 < this.m_farmTiles.Length; num4++)
						{
							if (this.m_farmTiles[num4].GetPastureIndex() == (int)agricolaAnimalContainer.m_id)
							{
								agricolaAnimalContainer.AddTile(this.m_farmTiles[num4]);
							}
						}
					}
					this.m_animalContainers.Add(agricolaAnimalContainer);
					if (agricolaAnimalContainer.m_resourceTotalsFromRules.sheep > 0)
					{
						for (int num5 = 0; num5 < (int)agricolaAnimalContainer.m_resourceTotalsFromRules.sheep; num5++)
						{
							agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.SHEEP, true), EResourceType.SHEEP, null);
						}
					}
					if (agricolaAnimalContainer.m_resourceTotalsFromRules.boar > 0)
					{
						for (int num6 = 0; num6 < (int)agricolaAnimalContainer.m_resourceTotalsFromRules.boar; num6++)
						{
							agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.WILDBOAR, true), EResourceType.WILDBOAR, null);
						}
					}
					if (agricolaAnimalContainer.m_resourceTotalsFromRules.cattle > 0)
					{
						for (int num7 = 0; num7 < (int)agricolaAnimalContainer.m_resourceTotalsFromRules.cattle; num7++)
						{
							agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.CATTLE, true), EResourceType.CATTLE, null);
						}
					}
				}
			}
		}
		for (int num8 = 0; num8 < this.m_orphanedAnimalList.Count; num8++)
		{
			int item = this.m_orphanedAnimalList[num8].Item2;
			int ifTileHasAnimalContainer = this.GetIfTileHasAnimalContainer(item);
			bool flag2 = false;
			if (ifTileHasAnimalContainer != -1)
			{
				flag2 = this.m_animalContainers[ifTileHasAnimalContainer].AddAnimal(this.m_orphanedAnimalList[num8].Item1, this.m_orphanedAnimalList[num8].Item1.GetAnimalType(), null);
			}
			if (!flag2)
			{
				this.PlaceAnimalInLimbo(this.m_orphanedAnimalList[num8].Item1);
				if (this.m_orphanedAnimalList[num8].Item1.GetAnimalType() == EResourceType.SHEEP)
				{
					num++;
				}
				else if (this.m_orphanedAnimalList[num8].Item1.GetAnimalType() == EResourceType.WILDBOAR)
				{
					num2++;
				}
				else if (this.m_orphanedAnimalList[num8].Item1.GetAnimalType() == EResourceType.CATTLE)
				{
					num3++;
				}
			}
		}
		this.m_orphanedAnimalList.Clear();
		num = this.AttemptToPlaceAnimals(EResourceType.SHEEP, num);
		num2 = this.AttemptToPlaceAnimals(EResourceType.WILDBOAR, num2);
		num3 = this.AttemptToPlaceAnimals(EResourceType.CATTLE, num3);
		ExcessAnimalTray excessAnimalTray = this.m_gameController.GetExcessAnimalTray();
		excessAnimalTray.ModifyExcessAnimalCount(EResourceType.SHEEP, num);
		excessAnimalTray.ModifyExcessAnimalCount(EResourceType.WILDBOAR, num2);
		excessAnimalTray.ModifyExcessAnimalCount(EResourceType.CATTLE, num3);
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x000100E0 File Offset: 0x0000E2E0
	public void SetSeason(EAgricolaSeason season)
	{
		if (this.m_springNodes != null && this.m_springNodes.Length != 0)
		{
			for (int i = 0; i < this.m_springNodes.Length; i++)
			{
				if (this.m_springNodes[i] != null)
				{
					this.m_springNodes[i].SetActive(season == EAgricolaSeason.SPRING);
				}
			}
		}
		if (this.m_summerNodes != null && this.m_summerNodes.Length != 0)
		{
			for (int j = 0; j < this.m_summerNodes.Length; j++)
			{
				if (this.m_summerNodes[j] != null)
				{
					this.m_summerNodes[j].SetActive(season == EAgricolaSeason.SUMMER);
				}
			}
		}
		if (this.m_autumnNodes != null && this.m_autumnNodes.Length != 0)
		{
			for (int k = 0; k < this.m_autumnNodes.Length; k++)
			{
				if (this.m_autumnNodes[k] != null)
				{
					this.m_autumnNodes[k].SetActive(season == EAgricolaSeason.AUTUMN);
				}
			}
		}
		if (this.m_winterNodes != null && this.m_winterNodes.Length != 0)
		{
			for (int l = 0; l < this.m_winterNodes.Length; l++)
			{
				if (this.m_winterNodes[l] != null)
				{
					this.m_winterNodes[l].SetActive(season == EAgricolaSeason.WINTER);
				}
			}
		}
		for (int m = 0; m < this.m_farmTiles.Length; m++)
		{
			this.m_farmTiles[m].SetSeason(season);
		}
		for (int n = 0; n < this.m_CardOrchardRows.Length; n++)
		{
			for (int num = 0; num < this.m_CardOrchardRows[n].m_CardOrchardNodes.Length; num++)
			{
				AgricolaFarmTile_Base component = this.m_CardOrchardRows[n].m_CardOrchardNodes[num].GetComponent<AgricolaFarmTile_Base>();
				if (component != null)
				{
					component.SetSeason(season);
				}
			}
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0001028F File Offset: 0x0000E48F
	public void RebuildFarm()
	{
		this.RebuildFarmToDisplayedPlayer();
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00010297 File Offset: 0x0000E497
	public void RebuildFarm(int playerIndex, int playerInstanceID)
	{
		if (this.HasAnimalPlacementToCommit())
		{
			this.BufferAnimalContainers();
		}
		this.m_displayedPlayerIndex = playerIndex;
		this.m_displayedPlayerInstanceID = playerInstanceID;
		this.RebuildFarmToDisplayedPlayer();
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x000102BB File Offset: 0x0000E4BB
	public void RebuildFarmToLocalPlayer()
	{
		if (this.HasAnimalPlacementToCommit())
		{
			this.BufferAnimalContainers();
		}
		this.m_displayedPlayerIndex = AgricolaLib.GetLocalPlayerIndex();
		this.m_displayedPlayerInstanceID = AgricolaLib.GetLocalPlayerInstanceID();
		this.RebuildFarmToDisplayedPlayer();
	}

	// Token: 0x060002BA RID: 698 RVA: 0x000102E8 File Offset: 0x0000E4E8
	private void RebuildFarmToDisplayedPlayer()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (!this.m_bInitialized)
		{
			this.Init();
		}
		this.SetFencingMode(false);
		this.m_bIsLocalPlayer = (AgricolaLib.GetLocalPlayerIndex() == this.m_displayedPlayerIndex);
		for (int i = 0; i < this.m_createdResources.Count; i++)
		{
			if (this.m_createdResources[i] != null)
			{
				this.m_createdResources[i].gameObject.SetActive(this.m_bIsLocalPlayer);
			}
		}
		if (this.m_bIsFeedingMode && !this.m_bIsLocalPlayer)
		{
			this.m_bHasFeedingReserve = true;
			this.m_reserveFeedingGiven.Initialize();
			for (int j = 0; j < this.m_workers.Count; j++)
			{
				AgricolaFeedingBubble feedingBubble = this.m_workers[j].GetComponent<AgricolaWorker>().GetFeedingBubble();
				this.m_reserveFeedingGiven[j] = feedingBubble.GetFoodGiven();
			}
		}
		for (int k = 0; k < this.m_workers.Count; k++)
		{
			UnityEngine.Object.Destroy(this.m_workers[k]);
		}
		this.m_workers.Clear();
		this.PlaceAllAnimalsInLimbo();
		for (int l = 0; l < this.m_validFencesBitfield.Length; l++)
		{
			this.m_validFencesBitfield[l] = false;
			this.m_proposedFencesBitfield[l] = false;
			this.m_preBuiltFencesBitfield[l] = false;
		}
		if (this.m_gameController != null)
		{
			AgricolaAnimationManager agricolaAnimationManager = this.m_gameController.GetAnimationManager() as AgricolaAnimationManager;
			if (agricolaAnimationManager != null)
			{
				agricolaAnimationManager.SetAnimationLocator(15, this.m_displayedPlayerInstanceID, this.m_CardOrchardBeggingCardLocator);
			}
		}
		this.RebuildCardOrchard();
		AgricolaLib.GetGamePlayerFarmState(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
		GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
		TrayToggle component = this.m_DisplayedPlayerResources.GetComponent<TrayToggle>();
		if (component != null)
		{
			component.SetTrayState(!this.m_bIsLocalPlayer);
		}
		else
		{
			this.m_DisplayedPlayerResources.gameObject.SetActive(!this.m_bIsLocalPlayer);
		}
		this.m_DisplayedPlayerResources.SetPlayerIndex(gamePlayerFarmState.playerIndex, gamePlayerFarmState.playerInstanceID);
		this.m_DisplayedPlayerResources.SetFactionIndex(gamePlayerFarmState.playerFaction);
		this.m_DisplayedPlayerResources.SetActivated(true, null);
		if (this.m_BeggingCard != null && this.m_CardOrchardBeggingCardSign != null && this.m_BeggingCardText != null)
		{
			if (this.m_BeggingCard.transform.parent != this.m_CardOrchardBeggingCardLocator.transform)
			{
				AgricolaCard component2 = this.m_BeggingCard.GetComponent<AgricolaCard>();
				component2.ShowFullCard(-1f);
				component2.ShowHalfCard(-1f);
				AnimateObject component3 = this.m_BeggingCard.GetComponent<AnimateObject>();
				if (component3 != null)
				{
					this.m_BeggingCard.SetActive(true);
					this.m_CardOrchardBeggingCardLocator.PlaceAnimateObject(component3, true, true, false);
				}
			}
			this.m_BeggingCard.SetActive(gamePlayerFarmState.numBeggingCards != 0);
			this.m_CardOrchardBeggingCardSign.SetActive(gamePlayerFarmState.numBeggingCards != 0);
			this.m_BeggingCardText.text = gamePlayerFarmState.numBeggingCards.ToString();
		}
		if (gamePlayerFarmState.playerIndex != 0 && gamePlayerFarmState.playerIndex == this.m_displayedPlayerIndex)
		{
			if (this.m_UpperHudScore != null)
			{
				this.m_UpperHudScore.text = gamePlayerFarmState.currentScore.ToString();
			}
			for (int m = 0; m < this.m_playerSpecificNodes.Length; m++)
			{
				this.m_playerSpecificNodes[m].SetActive(m == gamePlayerFarmState.playerFaction);
			}
			int num = 0;
			for (int n = 0; n < this.m_farmTiles.Length; n++)
			{
				this.m_farmTiles[n].ResetTile();
				if (gamePlayerFarmState.tileTypes[n] > 3)
				{
					gamePlayerFarmState.tileTypes[n] = 3;
				}
				this.m_farmTiles[n].SetTileType((AgricolaFarmTile.FarmTileAssignment)gamePlayerFarmState.tileTypes[n], true);
				if (gamePlayerFarmState.tileTypes[n] == 1)
				{
					num++;
				}
			}
			if (this.m_startingPlayerObject != null)
			{
				this.m_startingPlayerObject.SetActive(AgricolaLib.GetStartingPlayerIndex() == this.m_displayedPlayerIndex);
			}
			if (this.m_workerManager != null)
			{
				int num2 = Mathf.CeilToInt((float)gamePlayerFarmState.workerCount / (float)num);
				int num3 = 0;
				while (num2 > 0)
				{
					int num4 = -1;
					while (num3 < gamePlayerFarmState.workerCount)
					{
						AgricolaWorker agricolaWorker = this.m_workerManager.CreateTemporaryWorker(0, gamePlayerFarmState.playerFaction);
						if (agricolaWorker != null)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_feedingBubblePrefab);
							gameObject.GetComponent<AgricolaFeedingBubble>().Init(this, this.m_workers.Count, this.m_DragManager);
							agricolaWorker.gameObject.SetActive(true);
							agricolaWorker.SetAvatar(gamePlayerFarmState.workerAvatarIDs[num3]);
							agricolaWorker.SetDragType(ECardDragType.Never);
							agricolaWorker.SetSelectable(false, Color.white);
							agricolaWorker.transform.localScale = new Vector3(this.m_workerScale, this.m_workerScale, 1f);
							agricolaWorker.AddFeedingBubble(gameObject.GetComponent<AgricolaFeedingBubble>());
							this.m_workers.Add(agricolaWorker.gameObject);
							agricolaWorker.gameObject.SetActive(this.m_bIsFeedingMode || (gamePlayerFarmState.unusedWorkerFlags & 1 << num3) != 0);
							for (int num5 = num4 + 1; num5 != num4; num5++)
							{
								if (num5 >= this.m_farmTiles.Length)
								{
									num5 = 0;
								}
								if (this.m_farmTiles[num5].GetTileType() == AgricolaFarmTile.FarmTileAssignment.Room)
								{
									AgricolaFarmTile_Room room = this.m_farmTiles[num5].GetRoom();
									if (room != null)
									{
										room.AddWorker(agricolaWorker.gameObject);
										num4 = num5;
										break;
									}
								}
							}
						}
						num3++;
					}
					num2--;
				}
			}
			AgricolaLib.GetGamePlayerState(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
			GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
			this.ResolveTileUpdate();
			this.m_animalContainers.Clear();
			AgricolaLib.GetGamePlayerAnimalContainers(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
			GamePlayerAnimalContainers gamePlayerAnimalContainers = (GamePlayerAnimalContainers)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerAnimalContainers));
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			for (int num9 = 0; num9 < gamePlayerAnimalContainers.containerCount; num9++)
			{
				if (gamePlayerAnimalContainers.type[num9] != 0)
				{
					AgricolaAnimalContainer agricolaAnimalContainer = new AgricolaAnimalContainer();
					agricolaAnimalContainer.SetParent(this);
					agricolaAnimalContainer.m_rulesIndex = num9;
					agricolaAnimalContainer.m_id = gamePlayerAnimalContainers.id[num9];
					agricolaAnimalContainer.m_type = (AnimalContainerType)gamePlayerAnimalContainers.type[num9];
					agricolaAnimalContainer.m_capacity = gamePlayerAnimalContainers.capacity[num9];
					agricolaAnimalContainer.m_capacityType = (AnimalCapacityType)gamePlayerAnimalContainers.capacityType[num9];
					agricolaAnimalContainer.m_resourceTotalsFromRules.sheep = gamePlayerAnimalContainers.sheep[num9];
					agricolaAnimalContainer.m_resourceTotalsFromRules.boar = gamePlayerAnimalContainers.boar[num9];
					agricolaAnimalContainer.m_resourceTotalsFromRules.cattle = gamePlayerAnimalContainers.cattle[num9];
					if (agricolaAnimalContainer.m_type == AnimalContainerType.ROOM || agricolaAnimalContainer.m_type == AnimalContainerType.STABLE)
					{
						agricolaAnimalContainer.AddTile(this.m_farmTiles[(int)gamePlayerAnimalContainers.id[num9]]);
					}
					else if (agricolaAnimalContainer.m_type == AnimalContainerType.PASTURE)
					{
						for (int num10 = 0; num10 < this.m_farmTiles.Length; num10++)
						{
							if (this.m_farmTiles[num10].GetPastureIndex() == (int)agricolaAnimalContainer.m_id)
							{
								agricolaAnimalContainer.AddTile(this.m_farmTiles[num10]);
							}
						}
					}
					this.m_animalContainers.Add(agricolaAnimalContainer);
					bool flag = false;
					int index = -1;
					if (this.m_bIsLocalPlayer && this.m_bHasBufferedAnimals)
					{
						for (int num11 = 0; num11 < this.m_bufferedLocalAnimalContainers.Count; num11++)
						{
							if ((this.m_bufferedLocalAnimalContainers[num11] >> 24 & 255U) == (uint)agricolaAnimalContainer.m_rulesIndex)
							{
								flag = true;
								index = num11;
								break;
							}
						}
					}
					if (flag)
					{
						int num12 = (int)(this.m_bufferedLocalAnimalContainers[index] & 255U);
						int num13 = (int)(this.m_bufferedLocalAnimalContainers[index] >> 8 & 255U);
						int num14 = (int)(this.m_bufferedLocalAnimalContainers[index] >> 16 & 255U);
						if (num12 > 0)
						{
							for (int num15 = 0; num15 < num12; num15++)
							{
								if (num6 < gamePlayerState.resourceCountSheep)
								{
									agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.SHEEP, true), EResourceType.SHEEP, null);
									num6++;
								}
							}
						}
						if (num13 > 0)
						{
							for (int num16 = 0; num16 < num13; num16++)
							{
								if (num7 < gamePlayerState.resourceCountWildBoar)
								{
									agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.WILDBOAR, true), EResourceType.WILDBOAR, null);
									num7++;
								}
							}
						}
						if (num14 > 0)
						{
							for (int num17 = 0; num17 < (int)agricolaAnimalContainer.m_resourceTotalsFromRules.cattle; num17++)
							{
								if (num8 < gamePlayerState.resourceCountCattle)
								{
									agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.CATTLE, true), EResourceType.CATTLE, null);
									num8++;
								}
							}
						}
					}
					else
					{
						if (agricolaAnimalContainer.m_resourceTotalsFromRules.sheep > 0)
						{
							for (int num18 = 0; num18 < (int)agricolaAnimalContainer.m_resourceTotalsFromRules.sheep; num18++)
							{
								if (num6 < gamePlayerState.resourceCountSheep)
								{
									agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.SHEEP, true), EResourceType.SHEEP, null);
									num6++;
								}
							}
						}
						if (agricolaAnimalContainer.m_resourceTotalsFromRules.boar > 0)
						{
							for (int num19 = 0; num19 < (int)agricolaAnimalContainer.m_resourceTotalsFromRules.boar; num19++)
							{
								if (num7 < gamePlayerState.resourceCountWildBoar)
								{
									agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.WILDBOAR, true), EResourceType.WILDBOAR, null);
									num7++;
								}
							}
						}
						if (agricolaAnimalContainer.m_resourceTotalsFromRules.cattle > 0)
						{
							for (int num20 = 0; num20 < (int)agricolaAnimalContainer.m_resourceTotalsFromRules.cattle; num20++)
							{
								if (num8 < gamePlayerState.resourceCountCattle)
								{
									agricolaAnimalContainer.AddAnimal(this.GetAnimalInLimbo(EResourceType.CATTLE, true), EResourceType.CATTLE, null);
									num8++;
								}
							}
						}
					}
				}
			}
			if (this.m_bIsLocalPlayer)
			{
				ExcessAnimalTray excessAnimalTray = this.m_gameController.GetExcessAnimalTray();
				excessAnimalTray.Reset();
				if (num6 < gamePlayerState.resourceCountSheep)
				{
					int num21 = gamePlayerState.resourceCountSheep - num6;
					num21 = this.AttemptToPlaceAnimals(EResourceType.SHEEP, num21);
					excessAnimalTray.ModifyExcessAnimalCount(EResourceType.SHEEP, num21);
				}
				if (num7 < gamePlayerState.resourceCountWildBoar)
				{
					int num22 = gamePlayerState.resourceCountWildBoar - num7;
					num22 = this.AttemptToPlaceAnimals(EResourceType.WILDBOAR, num22);
					excessAnimalTray.ModifyExcessAnimalCount(EResourceType.WILDBOAR, num22);
				}
				if (num8 < gamePlayerState.resourceCountCattle)
				{
					int num23 = gamePlayerState.resourceCountCattle - num8;
					num23 = this.AttemptToPlaceAnimals(EResourceType.CATTLE, num23);
					excessAnimalTray.ModifyExcessAnimalCount(EResourceType.CATTLE, num23);
				}
			}
			bool animalsDraggable = false;
			if (this.m_displayedPlayerIndex == AgricolaLib.GetLocalPlayerIndex() && (GameOptions.IsSelectableHint(41040) || GameOptions.IsSelectableHint(40992)))
			{
				animalsDraggable = true;
			}
			if (this.m_bIsLocalPlayer)
			{
				this.m_bHasBufferedAnimals = false;
				this.m_bufferedLocalAnimalContainers.Clear();
			}
			this.SetAnimalsDraggable(animalsDraggable);
			this.SetFeedingMode(GameOptions.IsSelectableHint(40994) && this.m_bIsLocalPlayer, false);
			return;
		}
		Debug.LogError("Unable to get farm data for player index: " + this.m_displayedPlayerIndex.ToString());
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00010DF4 File Offset: 0x0000EFF4
	private void RebuildCardOrchard()
	{
		if (this.m_CardInPlayManager == null || this.m_workerManager == null)
		{
			return;
		}
		if (this.m_CardOrchardRows != null)
		{
			for (int i = 0; i < this.m_CardOrchardRows.Length; i++)
			{
				if (this.m_CardOrchardRows[i].m_CardOrchardNodes != null)
				{
					for (int j = 0; j < this.m_CardOrchardRows[i].m_NodesUsed; j++)
					{
						GameObject gameObject = this.m_CardOrchardRows[i].m_CardOrchardNodes[j];
						if (gameObject != null)
						{
							int k = 0;
							while (k < gameObject.transform.childCount)
							{
								AgricolaCardInPlay component = gameObject.transform.GetChild(k).gameObject.GetComponent<AgricolaCardInPlay>();
								if (component != null)
								{
									this.m_CardInPlayManager.PlaceCardInPlayInLimbo(component);
								}
								else
								{
									k++;
								}
							}
						}
					}
				}
				this.m_CardOrchardRows[i].m_NodesUsed = 0;
			}
		}
		List<AgricolaCardInPlay> playerCardInPlayList = this.m_CardInPlayManager.GetPlayerCardInPlayList(this.m_displayedPlayerInstanceID);
		if (playerCardInPlayList != null)
		{
			for (int l = 0; l < playerCardInPlayList.Count; l++)
			{
				AgricolaCardInPlay agricolaCardInPlay = playerCardInPlayList[l];
				if (!(agricolaCardInPlay == null))
				{
					AgricolaCard component2 = agricolaCardInPlay.GetSourceCard().GetComponent<AgricolaCard>();
					if (agricolaCardInPlay.GetCardOrchardRow() == 0)
					{
						if (this.m_CardOrchardGlobalConversion != null)
						{
							agricolaCardInPlay.transform.SetParent(this.m_CardOrchardGlobalConversion.transform, false);
							agricolaCardInPlay.gameObject.SetActive(true);
						}
					}
					else if (this.m_CardOrchardRows != null)
					{
						int num = agricolaCardInPlay.GetCardOrchardRow();
						if (agricolaCardInPlay.GetOwnerInstanceID() != this.m_displayedPlayerInstanceID)
						{
							num = 0;
							AgricolaLib.GetGamePlayerFarmState(this.m_gameController.GetUpperHud().FindPlayerIndexByInstanceID(agricolaCardInPlay.GetOwnerInstanceID()), this.m_bufPtr, 1024);
							GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
							AgricolaWorker agricolaWorker = this.m_workerManager.CreateTemporaryWorker(0, gamePlayerFarmState.playerFaction);
							if (agricolaWorker != null)
							{
								agricolaWorker.gameObject.SetActive(true);
								agricolaWorker.SetAvatar(gamePlayerFarmState.workerAvatarIDs[0]);
								agricolaWorker.SetDragType(ECardDragType.Never);
								agricolaWorker.SetSelectable(false, Color.white);
								agricolaCardInPlay.SetPlayerToken(agricolaWorker.gameObject);
							}
						}
						else
						{
							agricolaCardInPlay.SetPlayerToken(null);
						}
						if (num >= 0 && num < this.m_CardOrchardRows.Length)
						{
							int num2 = this.m_CardOrchardRows[num].m_NodesUsed;
							if (this.m_CardOrchardRows[num].m_CardOrchardNodes != null && num2 + (int)component2.GetCardOrchardSize() - 1 < this.m_CardOrchardRows[num].m_CardOrchardNodes.Length)
							{
								GameObject gameObject2 = this.m_CardOrchardRows[num].m_CardOrchardNodes[num2];
								if (gameObject2 != null)
								{
									agricolaCardInPlay.transform.SetParent(gameObject2.transform, false);
									agricolaCardInPlay.gameObject.SetActive(true);
								}
								AgricolaFarm.CardOrchardRow[] cardOrchardRows = this.m_CardOrchardRows;
								int num3 = num;
								cardOrchardRows[num3].m_NodesUsed = cardOrchardRows[num3].m_NodesUsed + 1;
								List<AgricolaImprovementBase> linkedTiles = agricolaCardInPlay.GetLinkedTiles();
								if (linkedTiles != null)
								{
									for (int m = 0; m < linkedTiles.Count; m++)
									{
										gameObject2 = this.m_CardOrchardRows[num].m_CardOrchardNodes[++num2];
										if (gameObject2 != null)
										{
											linkedTiles[m].transform.SetParent(gameObject2.transform, false);
											linkedTiles[m].gameObject.SetActive(true);
										}
										AgricolaFarm.CardOrchardRow[] cardOrchardRows2 = this.m_CardOrchardRows;
										int num4 = num;
										cardOrchardRows2[num4].m_NodesUsed = cardOrchardRows2[num4].m_NodesUsed + 1;
									}
								}
								agricolaCardInPlay.PlaceSourceCard();
							}
						}
					}
				}
			}
		}
		if (this.m_BeggingCard == null && this.m_CardManager != null && this.m_CardOrchardBeggingCardLocator != null)
		{
			int instanceID = AgricolaLib.GetInstanceID(12, string.Empty);
			if (instanceID != 0)
			{
				this.m_BeggingCard = this.m_CardManager.GetCardFromInstanceID(instanceID, true);
				if (this.m_BeggingCard != null)
				{
					AgricolaCard component3 = this.m_BeggingCard.GetComponent<AgricolaCard>();
					component3.ShowFullCard(-1f);
					component3.ShowHalfCard(-1f);
					AnimateObject component4 = this.m_BeggingCard.GetComponent<AnimateObject>();
					if (component4 != null)
					{
						this.m_BeggingCard.SetActive(true);
						this.m_CardOrchardBeggingCardLocator.PlaceAnimateObject(component4, true, true, false);
					}
				}
			}
		}
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00011278 File Offset: 0x0000F478
	private void BeginDragCallback(DragObject dragObject)
	{
		AgricolaCard component = dragObject.GetComponent<AgricolaCard>();
		if (component != null)
		{
			int cardInstanceID = component.GetCardInstanceID();
			if ((GameOptions.IsSelectableInstanceIDWithHint((ushort)cardInstanceID, 40982) || GameOptions.IsSelectableInstanceIDWithHint((ushort)cardInstanceID, 40983)) && this.m_CardOrchardDragTarget != null && this.m_CardOrchardRows != null)
			{
				int cardOrchardRow = (int)component.GetCardOrchardRow();
				if (cardOrchardRow >= 0 && cardOrchardRow < this.m_CardOrchardRows.Length)
				{
					int nodesUsed = this.m_CardOrchardRows[cardOrchardRow].m_NodesUsed;
					if (this.m_CardOrchardRows[cardOrchardRow].m_CardOrchardNodes != null && nodesUsed < this.m_CardOrchardRows[cardOrchardRow].m_CardOrchardNodes.Length)
					{
						GameObject gameObject = this.m_CardOrchardRows[cardOrchardRow].m_CardOrchardNodes[nodesUsed];
						if (gameObject != null)
						{
							this.m_CardOrchardDragTarget.transform.SetParent(gameObject.transform, false);
							this.m_CardOrchardDragTarget.SetActive(true);
						}
					}
				}
			}
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00011371 File Offset: 0x0000F571
	private void EndDragCallback(DragObject dragObject)
	{
		if (dragObject.GetComponent<AgricolaCard>() != null && this.m_CardOrchardDragTarget != null && this.m_CardOrchardDragTarget.activeSelf)
		{
			this.m_CardOrchardDragTarget.SetActive(false);
		}
	}

	// Token: 0x060002BE RID: 702 RVA: 0x000113A8 File Offset: 0x0000F5A8
	private void HandleEventCardInPlayStatus(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		CardInPlayStatus cardInPlayStatus = (CardInPlayStatus)Marshal.PtrToStructure(event_buffer, typeof(CardInPlayStatus));
		if (cardInPlayStatus.inplay == 0)
		{
			if (cardInPlayStatus.owner_instance_id == this.m_displayedPlayerInstanceID && this.m_CardInPlayManager != null)
			{
				GameObject cardInPlayFromInstanceID = this.m_CardInPlayManager.GetCardInPlayFromInstanceID(cardInPlayStatus.cardinplay_instance_id, true);
				if (cardInPlayFromInstanceID != null)
				{
					AgricolaCardInPlay component = cardInPlayFromInstanceID.GetComponent<AgricolaCardInPlay>();
					int cardOrchardRow = component.GetCardOrchardRow();
					if (component != null)
					{
						int cardOrchardSize = (int)component.GetSourceCard().GetComponent<AgricolaCard>().GetCardOrchardSize();
						if (this.m_CardInPlayManager.PlaceCardInPlayInLimbo(component) && this.m_CardOrchardRows != null && this.m_CardOrchardRows.Length > cardOrchardRow)
						{
							int num = -1;
							int num2 = 0;
							while (num2 < this.m_CardOrchardRows[cardOrchardRow].m_CardOrchardNodes.Length && num2 < this.m_CardOrchardRows[cardOrchardRow].m_NodesUsed)
							{
								AgricolaCardInPlay componentInChildren = this.m_CardOrchardRows[cardOrchardRow].m_CardOrchardNodes[num2].GetComponentInChildren<AgricolaCardInPlay>();
								if (componentInChildren != null && num != -1 && num2 - cardOrchardSize >= 0)
								{
									componentInChildren.transform.SetParent(this.m_CardOrchardRows[cardOrchardRow].m_CardOrchardNodes[num2 - cardOrchardSize].transform);
								}
								else if (componentInChildren == null && num == -1)
								{
									num = num2;
								}
								num2++;
							}
							AgricolaFarm.CardOrchardRow[] cardOrchardRows = this.m_CardOrchardRows;
							int num3 = cardOrchardRow;
							cardOrchardRows[num3].m_NodesUsed = cardOrchardRows[num3].m_NodesUsed - cardOrchardSize;
						}
					}
				}
			}
			return;
		}
		if (cardInPlayStatus.owner_instance_id == this.m_displayedPlayerInstanceID && this.m_CardInPlayManager != null)
		{
			GameObject cardInPlayFromInstanceID2 = this.m_CardInPlayManager.GetCardInPlayFromInstanceID(cardInPlayStatus.cardinplay_instance_id, true);
			if (cardInPlayFromInstanceID2 != null)
			{
				AgricolaCardInPlay component2 = cardInPlayFromInstanceID2.GetComponent<AgricolaCardInPlay>();
				if (component2 != null)
				{
					AgricolaCard component3 = component2.GetSourceCard().GetComponent<AgricolaCard>();
					if (component2.GetCardOrchardRow() == 0)
					{
						if (this.m_CardOrchardGlobalConversion != null)
						{
							component2.transform.SetParent(this.m_CardOrchardGlobalConversion.transform, false);
							component2.gameObject.SetActive(true);
						}
					}
					else if (this.m_CardOrchardRows != null)
					{
						int cardOrchardRow2 = component2.GetCardOrchardRow();
						if (cardOrchardRow2 >= 0 && cardOrchardRow2 < this.m_CardOrchardRows.Length)
						{
							int num4 = this.m_CardOrchardRows[cardOrchardRow2].m_NodesUsed;
							if (this.m_CardOrchardRows[cardOrchardRow2].m_CardOrchardNodes != null && num4 + (int)component3.GetCardOrchardSize() - 1 < this.m_CardOrchardRows[cardOrchardRow2].m_CardOrchardNodes.Length)
							{
								GameObject gameObject = this.m_CardOrchardRows[cardOrchardRow2].m_CardOrchardNodes[num4];
								if (gameObject != null)
								{
									component2.transform.SetParent(gameObject.transform, false);
									component2.gameObject.SetActive(true);
								}
								AgricolaFarm.CardOrchardRow[] cardOrchardRows2 = this.m_CardOrchardRows;
								int num5 = cardOrchardRow2;
								cardOrchardRows2[num5].m_NodesUsed = cardOrchardRows2[num5].m_NodesUsed + 1;
								List<AgricolaImprovementBase> linkedTiles = component2.GetLinkedTiles();
								if (linkedTiles != null)
								{
									for (int i = 0; i < linkedTiles.Count; i++)
									{
										gameObject = this.m_CardOrchardRows[cardOrchardRow2].m_CardOrchardNodes[++num4];
										if (gameObject != null)
										{
											linkedTiles[i].transform.SetParent(gameObject.transform, false);
											linkedTiles[i].gameObject.SetActive(true);
										}
										AgricolaFarm.CardOrchardRow[] cardOrchardRows3 = this.m_CardOrchardRows;
										int num6 = cardOrchardRow2;
										cardOrchardRows3[num6].m_NodesUsed = cardOrchardRows3[num6].m_NodesUsed + 1;
									}
								}
							}
						}
					}
					if (this.m_AnimationManager != null)
					{
						component2.RebuildAnimationManager(this.m_AnimationManager);
					}
				}
			}
		}
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00011760 File Offset: 0x0000F960
	private void ResolveTileUpdate()
	{
		bool flag = true;
		for (int i = 0; i < this.m_farmTiles.Length; i++)
		{
			flag &= this.m_farmTiles[i].ResolveUpdate();
		}
		if (!flag)
		{
			for (int j = 0; j < this.m_farmTiles.Length; j++)
			{
				if (this.m_farmTiles[j].GetTileType() == AgricolaFarmTile.FarmTileAssignment.Pasture)
				{
					AgricolaFarmTile_Pasture pasture = this.m_farmTiles[j].GetPasture();
					if (pasture != null)
					{
						AgricolaFarmTile agricolaFarmTile = this.m_farmTiles[j].GetNorthNeighbor();
						if ((agricolaFarmTile == null || agricolaFarmTile.GetPastureIndex() != pasture.GetPastureIndex()) && !this.m_validFencesBitfield[pasture.GetNorthFenceDataIndex()])
						{
							pasture.SetNorthFence(true, false);
						}
						agricolaFarmTile = this.m_farmTiles[j].GetSouthNeighbor();
						if ((agricolaFarmTile == null || agricolaFarmTile.GetPastureIndex() != pasture.GetPastureIndex()) && !this.m_validFencesBitfield[pasture.GetSouthFenceDataIndex()])
						{
							pasture.SetSouthFence(true, false);
						}
						agricolaFarmTile = this.m_farmTiles[j].GetEastNeighbor();
						if ((agricolaFarmTile == null || agricolaFarmTile.GetPastureIndex() != pasture.GetPastureIndex()) && !this.m_validFencesBitfield[pasture.GetEastFenceDataIndex()])
						{
							pasture.SetEastFence(true, false);
						}
						agricolaFarmTile = this.m_farmTiles[j].GetWestNeighbor();
						if ((agricolaFarmTile == null || agricolaFarmTile.GetPastureIndex() != pasture.GetPastureIndex()) && !this.m_validFencesBitfield[pasture.GetWestFenceDataIndex()])
						{
							pasture.SetWestFence(true, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x000118DA File Offset: 0x0000FADA
	public void MarkFarmTileForUpdate(int tileIndex)
	{
		if (tileIndex >= 0 && tileIndex < this.m_farmTiles.Length)
		{
			this.m_farmTiles[tileIndex].SetNeedsUpdated(false);
		}
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x000118FC File Offset: 0x0000FAFC
	public void SetFeedingMode(bool bOn, bool bCheckForNewFood)
	{
		if (bOn && !this.m_bIsFeedingMode)
		{
			this.m_bIsFeedingMode = true;
			AgricolaLib.GetGamePlayerState(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
			GamePlayerState gamePlayerState = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
			int resourceCountFood = gamePlayerState.resourceCountFood;
			this.m_foodAvailable = resourceCountFood;
			this.m_foodNeeded = gamePlayerState.foodRequirement;
			if (this.m_bHasFeedingReserve)
			{
				for (int i = 0; i < this.m_reserveFeedingGiven.Length; i++)
				{
					this.m_foodUsed += this.m_reserveFeedingGiven[i];
				}
			}
			else
			{
				this.m_foodUsed = this.m_foodNeeded;
				if (this.m_foodUsed > resourceCountFood)
				{
					this.m_foodUsed = resourceCountFood;
				}
			}
			int num = this.m_foodUsed;
			for (int j = 0; j < this.m_workers.Count; j++)
			{
				this.m_workers[j].SetActive(true);
				AgricolaWorker component = this.m_workers[j].GetComponent<AgricolaWorker>();
				AgricolaFeedingBubble feedingBubble = component.GetFeedingBubble();
				component.gameObject.SetActive(true);
				component.SetFeedingBubbleVisible(true);
				if (this.m_bHasFeedingReserve)
				{
					feedingBubble.SetFoodData(gamePlayerState.foodRequirementByWorker[j], this.m_reserveFeedingGiven[j], null);
				}
				else
				{
					if (num >= gamePlayerState.foodRequirementByWorker[j])
					{
						feedingBubble.SetFoodData(gamePlayerState.foodRequirementByWorker[j], gamePlayerState.foodRequirementByWorker[j], null);
					}
					else
					{
						feedingBubble.SetFoodData(gamePlayerState.foodRequirementByWorker[j], num, null);
					}
					num -= feedingBubble.GetFoodGiven();
				}
			}
			return;
		}
		if (!bOn && this.m_bIsFeedingMode)
		{
			this.m_bIsFeedingMode = false;
			this.m_bHasFeedingReserve = false;
			for (int k = 0; k < this.m_workers.Count; k++)
			{
				AgricolaWorker component2 = this.m_workers[k].GetComponent<AgricolaWorker>();
				component2.GetFeedingBubble();
				component2.SetFeedingBubbleVisible(false);
			}
			return;
		}
		if (bOn && this.m_bIsFeedingMode && bCheckForNewFood)
		{
			AgricolaLib.GetGamePlayerState(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
			GamePlayerState gamePlayerState2 = (GamePlayerState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerState));
			int resourceCountFood2 = gamePlayerState2.resourceCountFood;
			if (resourceCountFood2 > this.m_foodAvailable)
			{
				int num2 = resourceCountFood2 - this.m_foodAvailable;
				this.m_foodAvailable = gamePlayerState2.resourceCountFood;
				this.m_foodUsed = 0;
				for (int l = 0; l < this.m_workers.Count; l++)
				{
					AgricolaFeedingBubble feedingBubble2 = this.m_workers[l].GetComponent<AgricolaWorker>().GetFeedingBubble();
					if (num2 > 0 && feedingBubble2.GetFoodRequirement() > feedingBubble2.GetFoodGiven())
					{
						int num3 = Math.Min(num2, feedingBubble2.GetFoodRequirement() - feedingBubble2.GetFoodGiven());
						feedingBubble2.SetFoodData(feedingBubble2.GetFoodRequirement(), feedingBubble2.GetFoodGiven() + num3, null);
						num2 -= num3;
					}
					this.m_foodUsed += feedingBubble2.GetFoodGiven();
				}
			}
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00011BF4 File Offset: 0x0000FDF4
	public int GetFeedingAmountUsed()
	{
		if (this.m_bHasFeedingReserve)
		{
			int num = 0;
			for (int i = 0; i < this.m_reserveFeedingGiven.Length; i++)
			{
				num += this.m_reserveFeedingGiven[i];
			}
			return num;
		}
		return this.m_foodUsed;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00011C31 File Offset: 0x0000FE31
	public int GetFeedingAmountNeeded()
	{
		return this.m_foodNeeded;
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00011C3C File Offset: 0x0000FE3C
	public void HandleFeedingFoodDrop(int droppedWorkerIndex, bool bSuccessfulDrop, AgricolaResource droppedResource)
	{
		if (bSuccessfulDrop)
		{
			this.m_foodUsed = 0;
			for (int i = 0; i < this.m_workers.Count; i++)
			{
				AgricolaFeedingBubble feedingBubble = this.m_workers[i].GetComponent<AgricolaWorker>().GetFeedingBubble();
				if (feedingBubble.GetIsDraggingFrom())
				{
					feedingBubble.SetFoodData(feedingBubble.GetFoodRequirement(), feedingBubble.GetFoodGiven() - 1, droppedResource);
				}
				if (i == droppedWorkerIndex)
				{
					feedingBubble.SetFoodData(feedingBubble.GetFoodRequirement(), feedingBubble.GetFoodGiven() + 1, droppedResource);
				}
				this.m_foodUsed += feedingBubble.GetFoodGiven();
			}
			return;
		}
		for (int j = 0; j < this.m_workers.Count; j++)
		{
			AgricolaFeedingBubble feedingBubble2 = this.m_workers[j].GetComponent<AgricolaWorker>().GetFeedingBubble();
			feedingBubble2.SetFoodData(feedingBubble2.GetFoodRequirement(), feedingBubble2.GetFoodGiven(), droppedResource);
		}
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00011D0C File Offset: 0x0000FF0C
	public void ClearAnimalContainerSubmission()
	{
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			this.m_animalContainers[i].RevertSubmission();
		}
		this.m_bHasBufferedAnimals = false;
		this.m_bufferedLocalAnimalContainers.Clear();
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00011D54 File Offset: 0x0000FF54
	public bool HasAnimalPlacementToCommit()
	{
		if (this.m_bHasBufferedAnimals && GameOptions.IsSelectableHintAllowHidden(41040))
		{
			return true;
		}
		if (GameOptions.IsSelectableHintAllowHidden(41040) && this.m_displayedPlayerIndex == AgricolaLib.GetLocalPlayerIndex())
		{
			for (int i = 0; i < this.m_animalContainers.Count; i++)
			{
				if (this.m_animalContainers[i].GetNeedToSubmitAnimals())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00011DBC File Offset: 0x0000FFBC
	public void BufferAnimalContainers()
	{
		if (GameOptions.IsSelectableHintAllowHidden(41040) && this.m_displayedPlayerIndex == AgricolaLib.GetLocalPlayerIndex())
		{
			this.m_bHasBufferedAnimals = false;
			this.m_bufferedLocalAnimalContainers.Clear();
			for (int i = 0; i < this.m_animalContainers.Count; i++)
			{
				if (this.m_animalContainers[i].GetNeedToSubmitAnimals())
				{
					uint item = (uint)(this.m_animalContainers[i].m_rulesIndex << 24 | (int)this.m_animalContainers[i].m_resources.cattle << 16 | (int)this.m_animalContainers[i].m_resources.boar << 8 | (int)this.m_animalContainers[i].m_resources.sheep);
					this.m_bufferedLocalAnimalContainers.Add(item);
					this.m_bHasBufferedAnimals = true;
				}
			}
		}
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00011E9C File Offset: 0x0001009C
	public void SubmitAnimalPlacement()
	{
		if (!this.m_bInitialized || this.m_bIsSubmittingAnimalPlacement || !this.HasAnimalPlacementToCommit())
		{
			return;
		}
		this.m_bIsSubmittingAnimalPlacement = true;
		GameOptions.SelectOptionByHint(41040);
		AgricolaLib.ForceUpdateStateMachineInput(IntPtr.Zero, 0);
		if (this.m_bHasBufferedAnimals)
		{
			for (int i = 0; i < this.m_bufferedLocalAnimalContainers.Count; i++)
			{
				GameOptions.SelectOptionByHintWithData(41041, this.m_bufferedLocalAnimalContainers[i]);
				AgricolaLib.ForceUpdateStateMachineInput(IntPtr.Zero, 0);
			}
			this.m_bHasBufferedAnimals = false;
			this.m_bufferedLocalAnimalContainers.Clear();
		}
		else
		{
			for (int j = 0; j < this.m_animalContainers.Count; j++)
			{
				if (this.m_animalContainers[j].GetNeedToSubmitAnimals())
				{
					uint selectionData = (uint)(this.m_animalContainers[j].m_rulesIndex << 24 | (int)this.m_animalContainers[j].m_resources.cattle << 16 | (int)this.m_animalContainers[j].m_resources.boar << 8 | (int)this.m_animalContainers[j].m_resources.sheep);
					GameOptions.SelectOptionByHintWithData(41041, selectionData);
					AgricolaLib.ForceUpdateStateMachineInput(IntPtr.Zero, 0);
					this.m_animalContainers[j].FinalizeSubmission();
				}
			}
		}
		GameOptions.SelectOptionByHint(41042);
		AgricolaLib.ForceUpdateStateMachineInput(IntPtr.Zero, 0);
		this.m_bIsSubmittingAnimalPlacement = false;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00012014 File Offset: 0x00010214
	public int ForcePlaceAnimalsInContainer(EResourceType animalType, int count, int containerIndex)
	{
		int num = -1;
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			if (this.m_animalContainers[i].m_rulesIndex == containerIndex)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			while (count > 0)
			{
				AgricolaAnimal animalInLimbo = this.GetAnimalInLimbo(animalType, true);
				if (!this.CanPlaceAnimalInConatiner(animalInLimbo.GetAnimalType(), this.m_animalContainers[num].m_rulesIndex, count) || !this.m_animalContainers[num].AddAnimal(animalInLimbo, animalType, null))
				{
					break;
				}
				count--;
			}
		}
		return count;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x000120A0 File Offset: 0x000102A0
	public int AttemptToPlaceAnimals(EResourceType animalType, int count)
	{
		int num = -1;
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			int animalContainerCapacity = this.GetAnimalContainerCapacity(animalType, this.m_animalContainers[i].m_rulesIndex);
			if (animalContainerCapacity > 0 && animalContainerCapacity >= count && this.m_animalContainers[i].GetAnimalCount(animalType) > 0)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			num = 0;
		}
		bool flag = true;
		while (count > 0 && num < this.m_animalContainers.Count)
		{
			if (flag && this.m_animalContainers[num].m_type != AnimalContainerType.PASTURE)
			{
				num++;
				if (num >= this.m_animalContainers.Count)
				{
					num = 0;
					flag = false;
				}
			}
			else
			{
				AgricolaAnimal animalInLimbo = this.GetAnimalInLimbo(animalType, true);
				if (animalInLimbo == null)
				{
					Debug.LogError("Unable to create animal instance!");
					return count;
				}
				if (this.CanPlaceAnimalInConatiner(animalInLimbo.GetAnimalType(), this.m_animalContainers[num].m_rulesIndex, count) && this.m_animalContainers[num].AddAnimal(animalInLimbo, animalType, null))
				{
					count--;
				}
				else
				{
					num++;
					if (flag && num >= this.m_animalContainers.Count)
					{
						num = 0;
						flag = false;
					}
				}
			}
		}
		num = 0;
		flag = true;
		while (count > 0 && num < this.m_animalContainers.Count)
		{
			if (flag && this.m_animalContainers[num].m_type != AnimalContainerType.PASTURE)
			{
				num++;
				if (num >= this.m_animalContainers.Count)
				{
					num = 0;
					flag = false;
				}
			}
			else
			{
				AgricolaAnimal animalInLimbo2 = this.GetAnimalInLimbo(animalType, true);
				if (animalInLimbo2 == null)
				{
					Debug.LogError("Unable to create animal instance!");
					return count;
				}
				if (this.CanPlaceAnimalInConatiner(animalInLimbo2.GetAnimalType(), this.m_animalContainers[num].m_rulesIndex, 1) && this.m_animalContainers[num].AddAnimal(animalInLimbo2, animalType, null))
				{
					count--;
				}
				else
				{
					num++;
					if (flag && num >= this.m_animalContainers.Count)
					{
						num = 0;
						flag = false;
					}
				}
			}
		}
		return count;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00012290 File Offset: 0x00010490
	public void RemoveAnimalFromContainer(EResourceType animalType)
	{
		ExcessAnimalTray excessAnimalTray = this.m_gameController.GetExcessAnimalTray();
		if (excessAnimalTray.GetExcessAnimalCount(animalType) > 0)
		{
			excessAnimalTray.ModifyExcessAnimalCount(animalType, -1);
			return;
		}
		int num = -1;
		int num2 = 999;
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			int animalCount = this.m_animalContainers[i].GetAnimalCount(animalType);
			if (animalCount > 0 && animalCount < num2)
			{
				num = i;
				num2 = animalCount;
			}
		}
		if (num != -1)
		{
			AgricolaAnimal agricolaAnimal = this.m_animalContainers[num].RemoveAnimal(null, animalType);
			if (agricolaAnimal != null)
			{
				this.PlaceAnimalInLimbo(agricolaAnimal);
				return;
			}
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0001232C File Offset: 0x0001052C
	public int GetIfTileHasAnimalContainer(int tileindex)
	{
		int num = 1 << tileindex;
		for (int i = 0; i < this.m_animalContainers.Count; i++)
		{
			if ((num & this.m_animalContainers[i].m_tilesBitfield) != 0)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060002CD RID: 717 RVA: 0x0001236E File Offset: 0x0001056E
	public int GetAnimalContainerRulesIndex(int containerIndex)
	{
		if (containerIndex < 0 || containerIndex >= this.m_animalContainers.Count)
		{
			return -1;
		}
		return this.m_animalContainers[containerIndex].m_rulesIndex;
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00012395 File Offset: 0x00010595
	public void SetFenceData(int index, bool bOn, bool bProposed)
	{
		if (bOn)
		{
			if (bProposed)
			{
				this.m_proposedFencesBitfield[index] = true;
				return;
			}
			this.m_validFencesBitfield[index] = true;
			return;
		}
		else
		{
			if (bProposed)
			{
				this.m_proposedFencesBitfield[index] = false;
				return;
			}
			this.m_validFencesBitfield[index] = false;
			return;
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x000123C8 File Offset: 0x000105C8
	public void SetFencingMode(bool bOn)
	{
		if (this.m_bIsFencingMode == bOn)
		{
			return;
		}
		this.m_bIsFencingMode = bOn;
		if (this.m_bIsFencingMode)
		{
			this.m_bUseFencingTutorialLayout = false;
			if (AgricolaLib.GetIsTutorialGame() && this.m_gameController != null)
			{
				Tutorial tutorial = this.m_gameController.GetTutorial();
				if (tutorial != null && !tutorial.IsCompleted())
				{
					for (int i = 0; i < this.m_validFencesBitfield.Length; i++)
					{
						this.m_tutorialFencesBitfield[i] = false;
					}
					if (Tutorial.s_CurrentTutorialIndex == 2)
					{
						this.m_bUseFencingTutorialLayout = true;
						this.m_tutorialFencesBitfield[4] = true;
						this.m_tutorialFencesBitfield[9] = true;
						this.m_tutorialFencesBitfield[19] = true;
						this.m_tutorialFencesBitfield[24] = true;
						this.m_tutorialFencesBitfield[25] = true;
						this.m_tutorialFencesBitfield[30] = true;
						this.m_tutorialFencesBitfield[31] = true;
						this.m_tutorialFencesBitfield[36] = true;
						this.m_tutorialFencesBitfield[37] = true;
					}
					else if (Tutorial.s_CurrentTutorialIndex == 4)
					{
						this.m_bUseFencingTutorialLayout = true;
						this.m_validFencesBitfield[4] = true;
						this.m_validFencesBitfield[9] = true;
						this.m_validFencesBitfield[19] = true;
						this.m_validFencesBitfield[24] = true;
						this.m_validFencesBitfield[25] = true;
						this.m_validFencesBitfield[30] = true;
						this.m_validFencesBitfield[31] = true;
						this.m_validFencesBitfield[36] = true;
						this.m_validFencesBitfield[37] = true;
						this.m_tutorialFencesBitfield[3] = true;
						this.m_tutorialFencesBitfield[4] = true;
						this.m_tutorialFencesBitfield[9] = true;
						this.m_tutorialFencesBitfield[13] = true;
						this.m_tutorialFencesBitfield[18] = true;
						this.m_tutorialFencesBitfield[19] = true;
						this.m_tutorialFencesBitfield[23] = true;
						this.m_tutorialFencesBitfield[24] = true;
						this.m_tutorialFencesBitfield[25] = true;
						this.m_tutorialFencesBitfield[29] = true;
						this.m_tutorialFencesBitfield[30] = true;
						this.m_tutorialFencesBitfield[31] = true;
						this.m_tutorialFencesBitfield[35] = true;
						this.m_tutorialFencesBitfield[36] = true;
						this.m_tutorialFencesBitfield[37] = true;
					}
				}
			}
			this.m_maxFencesAvailable = AgricolaLib.GetGamePlayerMaxBuildableFences(this.m_displayedPlayerIndex);
			this.m_numFencesPurchased = 0;
			this.m_proposedPastureStableCount = 0;
			this.m_bProposedPastureFailed = false;
			this.m_bIsFencingValid = false;
			for (int j = 0; j < this.m_validFencesBitfield.Length; j++)
			{
				this.m_proposedFencesBitfield[j] = this.m_validFencesBitfield[j];
				this.m_preBuiltFencesBitfield[j] = this.m_validFencesBitfield[j];
			}
			for (int k = 0; k < this.m_farmTiles.Length; k++)
			{
				AgricolaFarmTile.FarmTileAssignment tileType = this.m_farmTiles[k].GetTileType();
				if (tileType == AgricolaFarmTile.FarmTileAssignment.EmptyTile || tileType == AgricolaFarmTile.FarmTileAssignment.Pasture)
				{
					AgricolaFarmTile_Pasture pasture = this.m_farmTiles[k].GetPasture();
					if (pasture != null)
					{
						pasture.SetFencingMode(true, this.m_preBuiltFencesBitfield);
					}
				}
			}
			if (this.m_bUseFencingTutorialLayout)
			{
				this.CalculateProposedPastures();
				return;
			}
		}
		else
		{
			this.m_maxFencesAvailable = 0;
			this.m_numFencesPurchased = 0;
			this.m_proposedPastureStableCount = 0;
			this.m_bProposedPastureFailed = false;
			this.m_bIsFencingValid = false;
			for (int l = 0; l < this.m_validFencesBitfield.Length; l++)
			{
				this.m_proposedFencesBitfield[l] = false;
			}
			for (int m = 0; m < this.m_farmTiles.Length; m++)
			{
				AgricolaFarmTile_Pasture pasture2 = this.m_farmTiles[m].GetPasture();
				if (pasture2 != null)
				{
					pasture2.SetFencingMode(false, this.m_preBuiltFencesBitfield);
					pasture2.HandleFenceGlows(null, null, null);
				}
			}
		}
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00012709 File Offset: 0x00010909
	public bool PurchaseFence()
	{
		if (this.m_numFencesPurchased < this.m_maxFencesAvailable)
		{
			this.m_numFencesPurchased++;
			return true;
		}
		return false;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0001272A File Offset: 0x0001092A
	public void ReturnFence()
	{
		this.m_numFencesPurchased--;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0001273C File Offset: 0x0001093C
	public void CalculateProposedPastures()
	{
		int num = 0;
		for (int i = 0; i < this.m_validFencesBitfield.Length; i++)
		{
			this.m_validFencesBitfield[i] = this.m_preBuiltFencesBitfield[i];
		}
		for (int j = 0; j < this.m_farmTiles.Length; j++)
		{
			AgricolaFarmTile_Pasture pasture = this.m_farmTiles[j].GetPasture();
			if (pasture != null)
			{
				pasture.SetProposedPastureIndex(-1);
			}
		}
		for (int k = 0; k < this.m_farmTiles.Length; k++)
		{
			AgricolaFarmTile.FarmTileAssignment tileType = this.m_farmTiles[k].GetTileType();
			if (tileType == AgricolaFarmTile.FarmTileAssignment.EmptyTile || tileType == AgricolaFarmTile.FarmTileAssignment.Pasture)
			{
				AgricolaFarmTile_Pasture pasture2 = this.m_farmTiles[k].GetPasture();
				if (pasture2 != null && pasture2.GetProposedIndex() == -1 && this.CalculatePastureWorkHorse(k, num))
				{
					num++;
				}
			}
		}
		bool flag = num > 0;
		if (num > 1)
		{
			for (int l = 0; l < num; l++)
			{
				bool flag2 = false;
				for (int m = 0; m < this.m_farmTiles.Length; m++)
				{
					AgricolaFarmTile_Pasture pasture3 = this.m_farmTiles[m].GetPasture();
					if (!(pasture3 == null) && pasture3.GetProposedIndex() == l)
					{
						if (this.m_farmTiles[m].GetNorthNeighbor() != null && this.m_farmTiles[m].GetNorthNeighbor().GetPasture() != null && this.m_farmTiles[m].GetNorthNeighbor().GetPasture().GetProposedIndex() != -1 && this.m_farmTiles[m].GetNorthNeighbor().GetPasture().GetProposedIndex() != l)
						{
							flag2 = true;
							break;
						}
						if (this.m_farmTiles[m].GetSouthNeighbor() != null && this.m_farmTiles[m].GetSouthNeighbor().GetPasture() != null && this.m_farmTiles[m].GetSouthNeighbor().GetPasture().GetProposedIndex() != -1 && this.m_farmTiles[m].GetSouthNeighbor().GetPasture().GetProposedIndex() != l)
						{
							flag2 = true;
							break;
						}
						if (this.m_farmTiles[m].GetEastNeighbor() != null && this.m_farmTiles[m].GetEastNeighbor().GetPasture() != null && this.m_farmTiles[m].GetEastNeighbor().GetPasture().GetProposedIndex() != -1 && this.m_farmTiles[m].GetEastNeighbor().GetPasture().GetProposedIndex() != l)
						{
							flag2 = true;
							break;
						}
						if (this.m_farmTiles[m].GetWestNeighbor() != null && this.m_farmTiles[m].GetWestNeighbor().GetPasture() != null && this.m_farmTiles[m].GetWestNeighbor().GetPasture().GetProposedIndex() != -1 && this.m_farmTiles[m].GetWestNeighbor().GetPasture().GetProposedIndex() != l)
						{
							flag2 = true;
							break;
						}
					}
				}
				flag = (flag && flag2);
			}
		}
		for (int n = 0; n < this.m_validFencesBitfield.Length; n++)
		{
			if (this.m_validFencesBitfield[n] != this.m_proposedFencesBitfield[n])
			{
				flag = false;
				break;
			}
		}
		if (this.m_bUseFencingTutorialLayout)
		{
			for (int num2 = 0; num2 < this.m_tutorialFencesBitfield.Length; num2++)
			{
				if (this.m_validFencesBitfield[num2] != this.m_tutorialFencesBitfield[num2])
				{
					flag = false;
					break;
				}
			}
		}
		this.m_bIsFencingValid = (flag && this.m_numFencesPurchased > 0);
		this.m_numProposedPastures = (flag ? num : 0);
		Debug.Log("Fencing pastures result: " + ((!flag) ? "FAIL!" : "Success!"));
		for (int num3 = 0; num3 < this.m_farmTiles.Length; num3++)
		{
			AgricolaFarmTile_Pasture pasture4 = this.m_farmTiles[num3].GetPasture();
			if (pasture4 != null)
			{
				if (this.m_bUseFencingTutorialLayout)
				{
					pasture4.HandleFenceGlows(this.m_tutorialFencesBitfield, this.m_tutorialFencesBitfield, this.m_preBuiltFencesBitfield);
				}
				else
				{
					pasture4.HandleFenceGlows(this.m_validFencesBitfield, this.m_proposedFencesBitfield, this.m_preBuiltFencesBitfield);
				}
			}
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00012B48 File Offset: 0x00010D48
	private bool CalculatePastureWorkHorse(int tileIndex, int proposedPastureIndex)
	{
		this.m_proposedPastureStableCount = 0;
		this.m_bProposedPastureFailed = false;
		for (int i = 0; i < this.m_farmTiles.Length; i++)
		{
			AgricolaFarmTile_Pasture pasture = this.m_farmTiles[i].GetPasture();
			if (pasture != null)
			{
				pasture.SetIsProposedChecked(false);
			}
		}
		if (!this.CalculatePastureRecursive(tileIndex))
		{
			return false;
		}
		for (int j = 0; j < this.m_farmTiles.Length; j++)
		{
			AgricolaFarmTile_Pasture pasture2 = this.m_farmTiles[j].GetPasture();
			if (pasture2 != null && pasture2.GetIsProposedChecked())
			{
				AgricolaFarmTile agricolaFarmTile = null;
				int num = 0;
				pasture2.SetIsProposedChecked(false);
				pasture2.SetProposedPastureIndex(proposedPastureIndex);
				for (int k = 0; k < 4; k++)
				{
					if (k == 0)
					{
						agricolaFarmTile = this.m_farmTiles[j].GetNorthNeighbor();
						num = pasture2.GetNorthFenceDataIndex();
					}
					else if (k == 1)
					{
						agricolaFarmTile = this.m_farmTiles[j].GetSouthNeighbor();
						num = pasture2.GetSouthFenceDataIndex();
					}
					else if (k == 2)
					{
						agricolaFarmTile = this.m_farmTiles[j].GetEastNeighbor();
						num = pasture2.GetEastFenceDataIndex();
					}
					else if (k == 3)
					{
						agricolaFarmTile = this.m_farmTiles[j].GetWestNeighbor();
						num = pasture2.GetWestFenceDataIndex();
					}
					if (this.m_proposedFencesBitfield[num] && (agricolaFarmTile == null || agricolaFarmTile.GetPasture() == null || (agricolaFarmTile.GetPasture().GetProposedIndex() != proposedPastureIndex && !agricolaFarmTile.GetPasture().GetIsProposedChecked())))
					{
						this.m_validFencesBitfield[num] = true;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00012CC8 File Offset: 0x00010EC8
	private bool CalculatePastureRecursive(int tileIndex)
	{
		if (this.m_bProposedPastureFailed)
		{
			return false;
		}
		bool flag = true;
		AgricolaFarmTile_Pasture pasture = this.m_farmTiles[tileIndex].GetPasture();
		if (!(pasture != null))
		{
			this.m_bProposedPastureFailed = false;
			return false;
		}
		if (pasture.GetHasStable())
		{
			this.m_proposedPastureStableCount++;
		}
		pasture.SetIsProposedChecked(true);
		AgricolaFarmTile agricolaFarmTile = null;
		bool flag2 = false;
		for (int i = 0; i < 4; i++)
		{
			if (!flag)
			{
				this.m_bProposedPastureFailed = false;
				return false;
			}
			switch (i)
			{
			case 0:
				agricolaFarmTile = this.m_farmTiles[tileIndex].GetNorthNeighbor();
				flag2 = this.m_proposedFencesBitfield[pasture.GetNorthFenceDataIndex()];
				break;
			case 1:
				agricolaFarmTile = this.m_farmTiles[tileIndex].GetSouthNeighbor();
				flag2 = this.m_proposedFencesBitfield[pasture.GetSouthFenceDataIndex()];
				break;
			case 2:
				agricolaFarmTile = this.m_farmTiles[tileIndex].GetEastNeighbor();
				flag2 = this.m_proposedFencesBitfield[pasture.GetEastFenceDataIndex()];
				break;
			case 3:
				agricolaFarmTile = this.m_farmTiles[tileIndex].GetWestNeighbor();
				flag2 = this.m_proposedFencesBitfield[pasture.GetWestFenceDataIndex()];
				break;
			}
			if (!flag2)
			{
				if (agricolaFarmTile == null)
				{
					this.m_bProposedPastureFailed = false;
					return false;
				}
				AgricolaFarmTile_Pasture pasture2 = agricolaFarmTile.GetPasture();
				if (!(pasture2 != null) || !pasture2.GetIsProposedChecked())
				{
					AgricolaFarmTile.FarmTileAssignment tileType = agricolaFarmTile.GetTileType();
					if (tileType != AgricolaFarmTile.FarmTileAssignment.EmptyTile && tileType != AgricolaFarmTile.FarmTileAssignment.Pasture)
					{
						this.m_bProposedPastureFailed = false;
						return false;
					}
					flag = this.CalculatePastureRecursive(agricolaFarmTile.GetTileIndex());
				}
			}
		}
		if (!flag)
		{
			this.m_bProposedPastureFailed = false;
			return false;
		}
		return true;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00012E3C File Offset: 0x0001103C
	public void SubmitFencedPastures(GameEventBuffer eventBuffer)
	{
		AgricolaLib.GetGamePlayerFarmState(this.m_displayedPlayerIndex, this.m_bufPtr, 1024);
		GamePlayerFarmState gamePlayerFarmState = (GamePlayerFarmState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmState));
		this.m_bIsSubmittingPastures = true;
		int[] array = new int[10];
		int[] array2 = new int[10];
		for (int i = 0; i < 15; i++)
		{
			if (gamePlayerFarmState.tileTypes[i] >= 3)
			{
				array[gamePlayerFarmState.tileTypes[i] - 3] |= 1 << i;
			}
		}
		for (int j = 0; j < this.m_farmTiles.Length; j++)
		{
			if (this.m_farmTiles[j].GetPasture() != null && this.m_farmTiles[j].GetPasture().GetProposedIndex() != -1)
			{
				array2[this.m_farmTiles[j].GetPasture().GetProposedIndex()] |= 1 << j;
				this.m_farmTiles[j].SetTileType(AgricolaFarmTile.FarmTileAssignment.Pasture, false);
				this.m_farmTiles[j].GetPasture().FinalizeProposedPasture();
			}
		}
		for (int k = 0; k < array2.Length; k++)
		{
			bool flag = array2[k] != 0;
			if (flag)
			{
				for (int l = 0; l < array.Length; l++)
				{
					if (array[l] == array2[k])
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				if (GameOptions.IsSelectableHint(40981))
				{
					GameOptions.SelectOptionByHintWithData(40981, (uint)array2[k]);
				}
				else if (GameOptions.IsSelectableHint(41023))
				{
					GameOptions.SelectOptionByHintWithData(41023, (uint)array2[k]);
				}
				else if (GameOptions.IsSelectableHint(40984))
				{
					GameOptions.SelectOptionByHintWithData(40984, (uint)array2[k]);
				}
				eventBuffer.Update();
				this.m_gameController.UpdateGameOptionsSelectionState(true);
			}
		}
		for (int m = 0; m < this.m_validFencesBitfield.Length; m++)
		{
			this.m_preBuiltFencesBitfield[m] = this.m_validFencesBitfield[m];
		}
		for (int n = 0; n < this.m_farmTiles.Length; n++)
		{
			if (this.m_farmTiles[n].GetTileType() == AgricolaFarmTile.FarmTileAssignment.Pasture)
			{
				this.m_farmTiles[n].SetNeedsUpdated(false);
			}
		}
		this.ResolveTileUpdate();
		this.SetFencingMode(false);
		this.m_bIsSubmittingPastures = false;
		this.m_gameController.UpdateGameOptionsSelectionState(true);
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00003022 File Offset: 0x00001222
	public void OnDragWorkerStart()
	{
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00013084 File Offset: 0x00011284
	public void OnDropWorker(AgricolaFarmTile droppedTile, ushort workerHint, DragObject dragObject)
	{
		switch (workerHint)
		{
		case 40976:
			Debug.Log("AgricolaFarm.OnDropWorker: Build Room");
			if (droppedTile != null && AgricolaLib.IsBuildableTile(droppedTile.GetTileIndex()))
			{
				droppedTile.SetTileType(AgricolaFarmTile.FarmTileAssignment.Room, false);
			}
			if (this.m_gameController != null)
			{
				this.m_gameController.UpdateGameOptionsSelectionState(true);
				return;
			}
			return;
		case 40977:
			Debug.Log("AgricolaFarm.OnDropWorker: Plow Field");
			if (droppedTile != null && AgricolaLib.IsPlowableTile(droppedTile.GetTileIndex()))
			{
				droppedTile.SetTileType(AgricolaFarmTile.FarmTileAssignment.Field, false);
			}
			if (this.m_gameController != null)
			{
				this.m_gameController.UpdateGameOptionsSelectionState(true);
				return;
			}
			return;
		case 40978:
			Debug.Log("AgricolaFarm.OnDropWorker: Sow Field");
			if (droppedTile != null && droppedTile.GetTileType() == AgricolaFarmTile.FarmTileAssignment.Field && droppedTile.GetField().GetNumCropsPlanted() == 0)
			{
				droppedTile.SetNeedsUpdated(false);
			}
			if (this.m_gameController != null)
			{
				this.m_gameController.UpdateGameOptionsSelectionState(true);
				return;
			}
			return;
		case 40979:
		case 40981:
		case 40982:
		case 40983:
			break;
		case 40980:
			Debug.Log("AgricolaFarm.OnDropWorker: Build Stable");
			if (droppedTile != null && AgricolaLib.CanPlaceStableInTile(droppedTile.GetTileIndex()))
			{
				droppedTile.SetNeedsUpdated(false);
			}
			if (this.m_gameController != null)
			{
				this.m_gameController.UpdateGameOptionsSelectionState(true);
				return;
			}
			return;
		case 40984:
			Debug.Log("AgricolaFarm.OnDropWorker: Fence Single Pasture");
			if (droppedTile != null && AgricolaLib.IsFenceableTile(droppedTile.GetTileIndex()))
			{
				droppedTile.SetTileType(AgricolaFarmTile.FarmTileAssignment.Pasture, false);
				droppedTile.SetNeedsUpdated(false);
			}
			if (this.m_gameController != null)
			{
				this.m_gameController.UpdateGameOptionsSelectionState(true);
				return;
			}
			return;
		default:
			if (workerHint == 41017)
			{
				Debug.Log("AgricolaFarm.OnDropWorker: Move Crop to Empty Field");
				if (droppedTile != null && droppedTile.GetTileType() == AgricolaFarmTile.FarmTileAssignment.Field && droppedTile.GetField().GetNumCropsPlanted() == 0)
				{
					droppedTile.SetNeedsUpdated(false);
				}
				AgricolaResource component = dragObject.GetComponent<AgricolaResource>();
				if (component != null)
				{
					int num = component.GetGameOptionIndex() - 1;
					if (num >= 0 && num < this.m_farmTiles.Length)
					{
						this.m_farmTiles[num].SetNeedsUpdated(false);
					}
				}
				if (this.m_gameController != null)
				{
					this.m_gameController.UpdateGameOptionsSelectionState(true);
					return;
				}
				return;
			}
			break;
		}
		Debug.LogWarning("AgricolaFarm.OnDropWorker: Unhandled hint: " + workerHint.ToString());
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000132D4 File Offset: 0x000114D4
	public void UpdateSelectionState(bool bHighlight)
	{
		if (!bHighlight)
		{
			return;
		}
		if (!GameOptions.IsSelectableHint(41017))
		{
			for (int i = 0; i < this.m_createdResources.Count; i++)
			{
				if (this.m_createdResources[i] != null)
				{
					UnityEngine.Object.Destroy(this.m_createdResources[i].gameObject);
				}
			}
			this.m_createdResources.Clear();
		}
		if (this.m_bIsLocalPlayer && this.m_gameController != null && GameOptions.IsSelectableHint(41017) && this.m_createdResources.Count == 0)
		{
			PlayerInterface interfaceStatic = this.m_gameController.GetPlayerInterfaceByLocalPlayerOrder(0).GetInterfaceStatic();
			int instanceIDFromHint = (int)GameOptions.GetInstanceIDFromHint(41017);
			for (int j = 0; j < this.m_farmTiles.Length; j++)
			{
				if (this.m_farmTiles[j].GetTileType() == AgricolaFarmTile.FarmTileAssignment.Field)
				{
					AgricolaFarmTile_Field field = this.m_farmTiles[j].GetField();
					if (field != null && field.GetNumCropsPlanted() >= instanceIDFromHint)
					{
						AgricolaFarmTile_Locators locators = this.m_farmTiles[j].GetLocators();
						AgricolaResource agricolaResource = interfaceStatic.CreateResourceToken((EResourceType)field.GetTypeCropsPlanted(), locators.GetLocator(4), this.m_farmTiles[j].GetTileIndex() + 1);
						agricolaResource.SetResourceValue(field.GetTypeCropsPlanted(), 1);
						this.m_createdResources.Add(agricolaResource);
					}
				}
			}
		}
	}

	// Token: 0x040001FA RID: 506
	private const int k_maxDataSize = 1024;

	// Token: 0x040001FB RID: 507
	public AgricolaFarmTile[] m_farmTiles;

	// Token: 0x040001FC RID: 508
	[SerializeField]
	private GameObject m_CardOrchardGlobalConversion;

	// Token: 0x040001FD RID: 509
	[SerializeField]
	private GameObject m_CardOrchardDragTarget;

	// Token: 0x040001FE RID: 510
	[SerializeField]
	private AgricolaFarm.CardOrchardRow[] m_CardOrchardRows;

	// Token: 0x040001FF RID: 511
	[SerializeField]
	private AgricolaAnimationLocator m_CardOrchardBeggingCardLocator;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	private GameObject m_CardOrchardBeggingCardSign;

	// Token: 0x04000201 RID: 513
	[SerializeField]
	private TextMeshProUGUI m_BeggingCardText;

	// Token: 0x04000202 RID: 514
	public GameObject m_fieldGrainPrefab;

	// Token: 0x04000203 RID: 515
	public GameObject m_fieldVeggiPrefab;

	// Token: 0x04000204 RID: 516
	public GameObject m_sheepPrefab;

	// Token: 0x04000205 RID: 517
	public GameObject m_boarPrefab;

	// Token: 0x04000206 RID: 518
	public GameObject m_cattlePrefab;

	// Token: 0x04000207 RID: 519
	[SerializeField]
	private AgricolaAnimationManager m_AnimationManager;

	// Token: 0x04000208 RID: 520
	[SerializeField]
	private DragManager m_DragManager;

	// Token: 0x04000209 RID: 521
	[SerializeField]
	private AgricolaCardInPlayManager m_CardInPlayManager;

	// Token: 0x0400020A RID: 522
	[SerializeField]
	private AgricolaCardManager m_CardManager;

	// Token: 0x0400020B RID: 523
	[SerializeField]
	private PlayerData m_DisplayedPlayerResources;

	// Token: 0x0400020C RID: 524
	[SerializeField]
	private GameObject m_animalLimbo;

	// Token: 0x0400020D RID: 525
	[SerializeField]
	private GameObject m_feedingBubblePrefab;

	// Token: 0x0400020E RID: 526
	[SerializeField]
	private AgricolaWorkerManager m_workerManager;

	// Token: 0x0400020F RID: 527
	[SerializeField]
	private float m_workerScale = 0.6f;

	// Token: 0x04000210 RID: 528
	[SerializeField]
	private TextMeshProUGUI m_UpperHudScore;

	// Token: 0x04000211 RID: 529
	[SerializeField]
	private GameObject[] m_playerSpecificNodes;

	// Token: 0x04000212 RID: 530
	[SerializeField]
	private GameObject m_startingPlayerObject;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	private GameObject[] m_springNodes;

	// Token: 0x04000214 RID: 532
	[SerializeField]
	private GameObject[] m_summerNodes;

	// Token: 0x04000215 RID: 533
	[SerializeField]
	private GameObject[] m_autumnNodes;

	// Token: 0x04000216 RID: 534
	[SerializeField]
	private GameObject[] m_winterNodes;

	// Token: 0x04000217 RID: 535
	private AgricolaGame m_gameController;

	// Token: 0x04000218 RID: 536
	private bool m_bInitialized;

	// Token: 0x04000219 RID: 537
	private int m_displayedPlayerIndex;

	// Token: 0x0400021A RID: 538
	private int m_displayedPlayerInstanceID;

	// Token: 0x0400021B RID: 539
	private bool m_bIsLocalPlayer;

	// Token: 0x0400021C RID: 540
	private bool m_bIsSubmittingAnimalPlacement;

	// Token: 0x0400021D RID: 541
	private bool m_bIsSubmittingPastures;

	// Token: 0x0400021E RID: 542
	private List<GameObject> m_workers = new List<GameObject>();

	// Token: 0x0400021F RID: 543
	private List<AgricolaAnimal> m_animals = new List<AgricolaAnimal>();

	// Token: 0x04000220 RID: 544
	private List<AgricolaAnimalContainer> m_animalContainers = new List<AgricolaAnimalContainer>();

	// Token: 0x04000221 RID: 545
	private List<uint> m_bufferedLocalAnimalContainers = new List<uint>();

	// Token: 0x04000222 RID: 546
	private bool m_bHasBufferedAnimals;

	// Token: 0x04000223 RID: 547
	private List<AgricolaResource> m_createdResources = new List<AgricolaResource>();

	// Token: 0x04000224 RID: 548
	private List<Tuple<AgricolaAnimal, int>> m_orphanedAnimalList = new List<Tuple<AgricolaAnimal, int>>();

	// Token: 0x04000225 RID: 549
	private AgricolaAnimal m_draggingAnimal;

	// Token: 0x04000226 RID: 550
	private int m_draggingAnimalContainerRulesIndex = -2;

	// Token: 0x04000227 RID: 551
	private GameObject m_BeggingCard;

	// Token: 0x04000228 RID: 552
	private bool m_bIsFencingMode;

	// Token: 0x04000229 RID: 553
	private bool m_bIsFencingValid;

	// Token: 0x0400022A RID: 554
	private bool m_bUseFencingTutorialLayout;

	// Token: 0x0400022B RID: 555
	private int m_numFencesPurchased;

	// Token: 0x0400022C RID: 556
	private int m_maxFencesAvailable;

	// Token: 0x0400022D RID: 557
	private int m_proposedPastureStableCount;

	// Token: 0x0400022E RID: 558
	private int m_numProposedPastures;

	// Token: 0x0400022F RID: 559
	private bool m_bProposedPastureFailed;

	// Token: 0x04000230 RID: 560
	private bool[] m_preBuiltFencesBitfield = new bool[40];

	// Token: 0x04000231 RID: 561
	private bool[] m_validFencesBitfield = new bool[40];

	// Token: 0x04000232 RID: 562
	private bool[] m_proposedFencesBitfield = new bool[40];

	// Token: 0x04000233 RID: 563
	private bool[] m_tutorialFencesBitfield = new bool[40];

	// Token: 0x04000234 RID: 564
	private bool m_bIsFeedingMode;

	// Token: 0x04000235 RID: 565
	private int m_foodUsed;

	// Token: 0x04000236 RID: 566
	private int m_foodNeeded;

	// Token: 0x04000237 RID: 567
	private int m_foodAvailable;

	// Token: 0x04000238 RID: 568
	private bool m_bHasFeedingReserve;

	// Token: 0x04000239 RID: 569
	private int[] m_reserveFeedingGiven = new int[7];

	// Token: 0x0400023A RID: 570
	private byte[] m_dataBuffer;

	// Token: 0x0400023B RID: 571
	private GCHandle m_hDataBuffer;

	// Token: 0x0400023C RID: 572
	private IntPtr m_bufPtr;

	// Token: 0x0200075A RID: 1882
	[Serializable]
	public struct CardOrchardRow
	{
		// Token: 0x04002B65 RID: 11109
		public int m_NodesUsed;

		// Token: 0x04002B66 RID: 11110
		public GameObject[] m_CardOrchardNodes;
	}
}
