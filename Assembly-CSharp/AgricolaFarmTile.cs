using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class AgricolaFarmTile : MonoBehaviour
{
	// Token: 0x060002EE RID: 750 RVA: 0x0001399F File Offset: 0x00011B9F
	public AgricolaFarmTile GetNorthNeighbor()
	{
		return this.m_northNeighbor;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000139A7 File Offset: 0x00011BA7
	public AgricolaFarmTile GetSouthNeighbor()
	{
		return this.m_southNeighbor;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x000139AF File Offset: 0x00011BAF
	public AgricolaFarmTile GetEastNeighbor()
	{
		return this.m_eastNeighbor;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000139B7 File Offset: 0x00011BB7
	public AgricolaFarmTile GetWestNeighbor()
	{
		return this.m_westNeighbor;
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x000139BF File Offset: 0x00011BBF
	public AgricolaFarmTile_Room GetRoom()
	{
		return this.m_roomNode;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000139C7 File Offset: 0x00011BC7
	public AgricolaFarmTile_Field GetField()
	{
		return this.m_fieldNode;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000139CF File Offset: 0x00011BCF
	public AgricolaFarmTile_Pasture GetPasture()
	{
		return this.m_pastureNode;
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000139D7 File Offset: 0x00011BD7
	public AgricolaFarmTile_Locators GetLocators()
	{
		return this.m_locatorsNode;
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x000139DF File Offset: 0x00011BDF
	public AgricolaFarm GetFarm()
	{
		return this.m_parent;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x000139E7 File Offset: 0x00011BE7
	public AgricolaFarmTile.FarmTileAssignment GetTileType()
	{
		return this.m_tileType;
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x000139EF File Offset: 0x00011BEF
	public int GetTileIndex()
	{
		return this.m_tileIndex;
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x000139F7 File Offset: 0x00011BF7
	public bool GetNeedsUpdate()
	{
		return this.m_bNeedsUpdate;
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00013A00 File Offset: 0x00011C00
	public void Init(AgricolaFarm parent, int tileIndex)
	{
		this.m_parent = parent;
		this.m_tileIndex = tileIndex;
		if (this.m_dataBuffer == null)
		{
			this.m_dataBuffer = new byte[1024];
			this.m_hDataBuffer = GCHandle.Alloc(this.m_dataBuffer, GCHandleType.Pinned);
			this.m_bufPtr = this.m_hDataBuffer.AddrOfPinnedObject();
		}
		if (this.m_groundNode != null)
		{
			this.m_groundNode.SetParent(this);
		}
		if (this.m_fieldNode != null)
		{
			this.m_fieldNode.SetParent(this);
		}
		if (this.m_roomNode != null)
		{
			this.m_roomNode.SetParent(this);
		}
		if (this.m_pastureNode != null)
		{
			this.m_pastureNode.SetParent(this);
		}
		if (this.m_locatorsNode != null)
		{
			this.m_locatorsNode.SetParent(this);
			this.m_locatorsNode.SetDragTargetIndex(tileIndex);
			this.m_locatorsNode.SetDragSelectionHint(0, Color.white, 0);
		}
		this.ResetTile();
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00013AFC File Offset: 0x00011CFC
	private void OnDestroy()
	{
		if (this.m_dataBuffer != null)
		{
			this.m_hDataBuffer.Free();
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00013B14 File Offset: 0x00011D14
	public void ResetTile()
	{
		if (this.m_bIsStartingRoom)
		{
			this.SetTileType(AgricolaFarmTile.FarmTileAssignment.Room, true);
		}
		else
		{
			this.SetTileType(AgricolaFarmTile.FarmTileAssignment.EmptyTile, true);
		}
		if (this.m_roomNode != null)
		{
			this.m_roomNode.ClearWorkers();
		}
		if (this.m_pastureNode != null)
		{
			this.m_pastureNode.Clear(false);
		}
		if (this.m_fieldNode != null)
		{
			this.m_fieldNode.Clear();
		}
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00013B87 File Offset: 0x00011D87
	public void SetTileType(AgricolaFarmTile.FarmTileAssignment newType, bool bForceUpdate = false)
	{
		if (bForceUpdate || newType != this.m_tileType)
		{
			this.m_bNeedsUpdate = true;
			this.m_tileType = newType;
		}
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00013BA3 File Offset: 0x00011DA3
	public void SetNeedsUpdated(bool bGetTileType = false)
	{
		this.m_bNeedsUpdate = true;
		this.m_bGetTileTypeFromRules = bGetTileType;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00013BB4 File Offset: 0x00011DB4
	public void SetSeason(EAgricolaSeason season)
	{
		if (this.m_groundNode != null)
		{
			this.m_groundNode.SetSeason(season);
		}
		if (this.m_fieldNode != null)
		{
			this.m_fieldNode.SetSeason(season);
		}
		if (this.m_roomNode != null)
		{
			this.m_roomNode.SetSeason(season);
		}
		if (this.m_pastureNode != null)
		{
			this.m_pastureNode.SetSeason(season);
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00013C29 File Offset: 0x00011E29
	public int GetPastureIndex()
	{
		if (this.m_pastureNode == null || this.m_tileType != AgricolaFarmTile.FarmTileAssignment.Pasture)
		{
			return -1;
		}
		return this.m_pastureNode.GetPastureIndex();
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00013C50 File Offset: 0x00011E50
	public bool ResolveUpdate()
	{
		if (!this.m_bNeedsUpdate)
		{
			return true;
		}
		this.m_bNeedsUpdate = false;
		if (AgricolaLib.GetGamePlayerFarmTileState(this.m_parent.GetDisplayedPlayerIndex(), this.m_tileIndex, this.m_bufPtr, 1024) != 0)
		{
			GamePlayerFarmTileState gamePlayerFarmTileState = (GamePlayerFarmTileState)Marshal.PtrToStructure(this.m_bufPtr, typeof(GamePlayerFarmTileState));
			if (this.m_bGetTileTypeFromRules)
			{
				this.m_bGetTileTypeFromRules = false;
				if (gamePlayerFarmTileState.tileType > 3)
				{
					this.m_tileType = AgricolaFarmTile.FarmTileAssignment.Pasture;
				}
				else
				{
					this.m_tileType = (AgricolaFarmTile.FarmTileAssignment)gamePlayerFarmTileState.tileType;
				}
			}
			switch (this.m_tileType)
			{
			case AgricolaFarmTile.FarmTileAssignment.EmptyTile:
				if (this.m_groundNode != null)
				{
					this.m_groundNode.gameObject.SetActive(true);
				}
				if (this.m_roomNode != null)
				{
					this.m_roomNode.gameObject.SetActive(false);
				}
				if (this.m_fieldNode != null)
				{
					this.m_fieldNode.SetActive(false, true);
				}
				if (this.m_pastureNode != null)
				{
					this.m_pastureNode.Clear(true);
					this.m_pastureNode.SetStable(gamePlayerFarmTileState.hasStable != 0);
				}
				break;
			case AgricolaFarmTile.FarmTileAssignment.Room:
				if (gamePlayerFarmTileState.tileType != 1)
				{
					this.m_bNeedsUpdate = true;
					return true;
				}
				if (this.m_groundNode != null)
				{
					this.m_groundNode.gameObject.SetActive(false);
				}
				if (this.m_fieldNode != null)
				{
					this.m_fieldNode.SetActive(false, true);
				}
				if (this.m_pastureNode != null)
				{
					this.m_pastureNode.Clear(true);
				}
				if (this.m_roomNode != null)
				{
					this.m_roomNode.gameObject.SetActive(true);
					if (gamePlayerFarmTileState.data0[0] == 3)
					{
						this.m_roomNode.SetHouseType(EFarmTileRoomType.STONE);
					}
					else if (gamePlayerFarmTileState.data0[0] == 2)
					{
						this.m_roomNode.SetHouseType(EFarmTileRoomType.CLAY);
					}
					else
					{
						this.m_roomNode.SetHouseType(EFarmTileRoomType.WOOD);
					}
					bool flag = false;
					bool flag2 = false;
					bool swconnection = false;
					if (this.m_southNeighbor != null)
					{
						flag = (this.m_southNeighbor.GetTileType() == AgricolaFarmTile.FarmTileAssignment.Room);
					}
					if (this.m_westNeighbor != null)
					{
						flag2 = (this.m_westNeighbor.GetTileType() == AgricolaFarmTile.FarmTileAssignment.Room);
					}
					if (flag && flag2)
					{
						AgricolaFarmTile southNeighbor = this.m_westNeighbor.GetSouthNeighbor();
						if (southNeighbor != null)
						{
							swconnection = (southNeighbor.GetTileType() == AgricolaFarmTile.FarmTileAssignment.Room);
						}
					}
					this.m_roomNode.SetSouthConnection(flag);
					this.m_roomNode.SetWestConnection(flag2);
					this.m_roomNode.SetSWConnection(swconnection);
				}
				break;
			case AgricolaFarmTile.FarmTileAssignment.Field:
				if (gamePlayerFarmTileState.tileType != 2)
				{
					this.m_bNeedsUpdate = true;
					return true;
				}
				if (this.m_groundNode != null)
				{
					this.m_groundNode.gameObject.SetActive(false);
				}
				if (this.m_roomNode != null)
				{
					this.m_roomNode.gameObject.SetActive(false);
				}
				if (this.m_pastureNode != null)
				{
					this.m_pastureNode.Clear(true);
				}
				if (this.m_fieldNode != null)
				{
					this.m_fieldNode.SetActive(true, false);
					if (gamePlayerFarmTileState.data0[0] == 0 || gamePlayerFarmTileState.data0[1] == 0)
					{
						this.m_fieldNode.SetActive(true, true);
					}
					else
					{
						GameObject cropObject = null;
						if (gamePlayerFarmTileState.data0[0] != this.m_fieldNode.GetTypeCropsPlanted())
						{
							if (gamePlayerFarmTileState.data0[0] == 5)
							{
								cropObject = this.m_parent.GetFieldGrainInstance();
							}
							else if (gamePlayerFarmTileState.data0[0] == 6)
							{
								cropObject = this.m_parent.GetFieldVeggiInstance();
							}
						}
						this.m_fieldNode.SetFieldData(cropObject, gamePlayerFarmTileState.data0[0], gamePlayerFarmTileState.data0[1], gamePlayerFarmTileState.data0[2]);
					}
				}
				break;
			case AgricolaFarmTile.FarmTileAssignment.Pasture:
				if (this.m_groundNode != null)
				{
					this.m_groundNode.gameObject.SetActive(true);
				}
				if (this.m_roomNode != null)
				{
					this.m_roomNode.gameObject.SetActive(false);
				}
				if (this.m_fieldNode != null)
				{
					this.m_fieldNode.SetActive(false, true);
				}
				if (this.m_pastureNode != null)
				{
					this.m_pastureNode.Clear(true);
					this.m_pastureNode.SetStable(gamePlayerFarmTileState.hasStable != 0);
					this.m_pastureNode.SetPastureIndex(gamePlayerFarmTileState.data0[0]);
					this.m_pastureNode.SetPastureCapacity(gamePlayerFarmTileState.data0[5]);
					this.m_pastureNode.SetFencingNeedsUpdate(true);
					return false;
				}
				break;
			}
		}
		return true;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x000140C0 File Offset: 0x000122C0
	public void OnDropCallback(DragObject dragObject, ushort selectionHint)
	{
		if (dragObject.GetComponent<AgricolaFarmAction>() != null)
		{
			this.m_parent.OnDropWorker(this, selectionHint, dragObject);
		}
		if (dragObject.GetComponent<AgricolaResource>() != null)
		{
			this.m_parent.OnDropWorker(this, selectionHint, dragObject);
		}
		dragObject.GetComponent<AgricolaAnimal>() != null;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00014114 File Offset: 0x00012314
	public void BeginDragCallback(DragObject dragObject)
	{
		if (this.m_parent.GetDisplayedPlayerIndex() != AgricolaLib.GetLocalPlayerIndex())
		{
			return;
		}
		AgricolaFarmAction component = dragObject.GetComponent<AgricolaFarmAction>();
		if (component != null)
		{
			if (component.GetFarmActionHint() == 40977 && AgricolaLib.IsPlowableTile(this.m_tileIndex))
			{
				if (AgricolaLib.GetIsTutorialGame())
				{
					Tutorial tutorial = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>().GetTutorial();
					if (tutorial != null && !tutorial.IsCompleted())
					{
						TutorialStep currentStep = tutorial.GetCurrentStep();
						if (currentStep != null && currentStep.m_SelectionHint == 40977 && (ulong)currentStep.m_SelectionOptionalData != (ulong)((long)this.m_tileIndex))
						{
							return;
						}
					}
				}
				AgricolaFarmTile_Locators locators = this.GetLocators();
				if (locators != null)
				{
					locators.SetDragSelectionHint(40977, Color.white, (ushort)(this.m_tileIndex + 1));
				}
			}
			if (component.GetFarmActionHint() == 40984 && AgricolaLib.IsFenceableTile(this.m_tileIndex))
			{
				uint instanceIDFromHint = (uint)GameOptions.GetInstanceIDFromHint(40984);
				if ((1U << this.m_tileIndex & instanceIDFromHint) != 0U)
				{
					AgricolaFarmTile_Locators locators2 = this.GetLocators();
					if (locators2 != null)
					{
						locators2.SetDragSelectionHint(40984, Color.white, (ushort)(this.m_tileIndex + 1));
					}
				}
			}
			if (GameOptions.IsSelectableHint(40976) && AgricolaLib.IsBuildableTile(this.m_tileIndex))
			{
				if (AgricolaLib.GetIsTutorialGame())
				{
					Tutorial tutorial2 = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>().GetTutorial();
					if (tutorial2 != null && !tutorial2.IsCompleted())
					{
						TutorialStep currentStep2 = tutorial2.GetCurrentStep();
						if (currentStep2 != null && currentStep2.m_SelectionHint == 40976 && (ulong)currentStep2.m_SelectionOptionalData != (ulong)((long)this.m_tileIndex))
						{
							return;
						}
					}
				}
				AgricolaFarmTile_Locators locators3 = this.GetLocators();
				if (locators3 != null)
				{
					locators3.SetDragSelectionHint(40976, Color.white, (ushort)(this.m_tileIndex + 1));
				}
			}
			if (GameOptions.IsSelectableHint(40980) && AgricolaLib.CanPlaceStableInTile(this.m_tileIndex))
			{
				if (AgricolaLib.GetIsTutorialGame())
				{
					Tutorial tutorial3 = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>().GetTutorial();
					if (tutorial3 != null && !tutorial3.IsCompleted())
					{
						TutorialStep currentStep3 = tutorial3.GetCurrentStep();
						if (currentStep3 != null && currentStep3.m_SelectionHint == 40980 && (ulong)currentStep3.m_SelectionOptionalData != (ulong)((long)this.m_tileIndex))
						{
							return;
						}
					}
				}
				AgricolaFarmTile_Locators locators4 = this.GetLocators();
				if (locators4 != null)
				{
					locators4.SetDragSelectionHint(40980, Color.white, (ushort)(this.m_tileIndex + 1));
				}
			}
		}
		AgricolaResource component2 = dragObject.GetComponent<AgricolaResource>();
		if (component2 != null)
		{
			AgricolaFarmTile.FarmTileAssignment tileType = this.m_tileType;
			if (tileType == AgricolaFarmTile.FarmTileAssignment.Field)
			{
				if ((component2.GetGameOptionIndex() - 1 == 5 || component2.GetGameOptionIndex() - 1 == 6) && GameOptions.IsSelectableHint(40978) && this.m_fieldNode.GetNumCropsPlanted() == 0)
				{
					if (AgricolaLib.GetIsTutorialGame())
					{
						Tutorial tutorial4 = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>().GetTutorial();
						if (tutorial4 != null && !tutorial4.IsCompleted())
						{
							TutorialStep currentStep4 = tutorial4.GetCurrentStep();
							if (currentStep4 != null && currentStep4.m_SelectionHint == 40978)
							{
								int tileSowLocationIndex = AgricolaLib.GetTileSowLocationIndex(this.m_tileIndex);
								int num = AgricolaLib.GetSowLocationOptionIndex(tileSowLocationIndex, component2.GetGameOptionIndex() - 1) << 16 | tileSowLocationIndex;
								if ((ulong)currentStep4.m_SelectionOptionalData != (ulong)((long)num))
								{
									return;
								}
							}
						}
					}
					AgricolaFarmTile_Locators locators5 = this.GetLocators();
					if (locators5 != null)
					{
						locators5.SetDragSelectionHint(40978, Color.white, (ushort)(this.m_tileIndex + 1));
					}
				}
				if ((component2.GetResourceType() == 5 || component2.GetResourceType() == 6) && GameOptions.IsSelectableHint(41017) && this.m_fieldNode.GetNumCropsPlanted() == 0)
				{
					AgricolaFarmTile_Locators locators6 = this.GetLocators();
					if (locators6 != null)
					{
						locators6.SetDragSelectionHint(41017, Color.white, (ushort)(this.m_tileIndex + 1));
					}
				}
			}
		}
		AgricolaAnimal component3 = dragObject.GetComponent<AgricolaAnimal>();
		if (component3 != null && this.m_parent != null)
		{
			int ifTileHasAnimalContainer = this.m_parent.GetIfTileHasAnimalContainer(this.m_tileIndex);
			if (ifTileHasAnimalContainer != -1 && this.m_parent.CanPlaceAnimalInConatiner(component3.GetAnimalType(), this.m_parent.GetAnimalContainerRulesIndex(ifTileHasAnimalContainer), 1))
			{
				AgricolaFarmTile_Locators locators7 = this.GetLocators();
				if (locators7 != null)
				{
					locators7.SetDragSelectionHint(1, Color.white, (ushort)(this.m_tileIndex + 1));
				}
			}
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00014564 File Offset: 0x00012764
	public void EndDragCallback(DragObject dragObject)
	{
		AgricolaFarmTile_Locators locators = this.GetLocators();
		if (locators != null)
		{
			locators.SetDragSelectionHint(0, Color.white, 0);
		}
	}

	// Token: 0x04000248 RID: 584
	private const int k_maxDataSize = 1024;

	// Token: 0x04000249 RID: 585
	[SerializeField]
	private AgricolaFarmTile m_northNeighbor;

	// Token: 0x0400024A RID: 586
	[SerializeField]
	private AgricolaFarmTile m_southNeighbor;

	// Token: 0x0400024B RID: 587
	[SerializeField]
	private AgricolaFarmTile m_eastNeighbor;

	// Token: 0x0400024C RID: 588
	[SerializeField]
	private AgricolaFarmTile m_westNeighbor;

	// Token: 0x0400024D RID: 589
	[SerializeField]
	private AgricolaFarmTile_Base m_groundNode;

	// Token: 0x0400024E RID: 590
	[SerializeField]
	private AgricolaFarmTile_Room m_roomNode;

	// Token: 0x0400024F RID: 591
	[SerializeField]
	private AgricolaFarmTile_Field m_fieldNode;

	// Token: 0x04000250 RID: 592
	[SerializeField]
	private AgricolaFarmTile_Pasture m_pastureNode;

	// Token: 0x04000251 RID: 593
	[SerializeField]
	private AgricolaFarmTile_Locators m_locatorsNode;

	// Token: 0x04000252 RID: 594
	[SerializeField]
	private GameObject m_glow;

	// Token: 0x04000253 RID: 595
	[SerializeField]
	private bool m_bIsStartingRoom;

	// Token: 0x04000254 RID: 596
	private AgricolaFarm m_parent;

	// Token: 0x04000255 RID: 597
	private int m_tileIndex;

	// Token: 0x04000256 RID: 598
	private bool m_bNeedsUpdate;

	// Token: 0x04000257 RID: 599
	private bool m_bGetTileTypeFromRules;

	// Token: 0x04000258 RID: 600
	[SerializeField]
	private AgricolaFarmTile.FarmTileAssignment m_tileType;

	// Token: 0x04000259 RID: 601
	private byte[] m_dataBuffer;

	// Token: 0x0400025A RID: 602
	private GCHandle m_hDataBuffer;

	// Token: 0x0400025B RID: 603
	private IntPtr m_bufPtr;

	// Token: 0x0200075B RID: 1883
	public enum FarmTileAssignment
	{
		// Token: 0x04002B68 RID: 11112
		EmptyTile,
		// Token: 0x04002B69 RID: 11113
		Room,
		// Token: 0x04002B6A RID: 11114
		Field,
		// Token: 0x04002B6B RID: 11115
		Pasture
	}
}
