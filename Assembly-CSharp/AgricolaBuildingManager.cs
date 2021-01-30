using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameData;
using GameEvent;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200002E RID: 46
public class AgricolaBuildingManager : MonoBehaviour
{
	// Token: 0x060001EF RID: 495 RVA: 0x0000AD90 File Offset: 0x00008F90
	private void Awake()
	{
		this.m_MasterActionSpaceList = new Hashtable();
		this.m_ActiveBuildingList = new List<AgricolaBuilding>();
		this.m_ActiveActionSpaceList = new List<AgricolaActionSpace>();
		if (this.m_DefaultBuildingList != null)
		{
			for (int i = 0; i < this.m_DefaultBuildingList.Length; i++)
			{
				if (this.m_DefaultBuildingList[i] != null && !this.m_ActiveBuildingList.Contains(this.m_DefaultBuildingList[i]))
				{
					this.m_ActiveBuildingList.Add(this.m_DefaultBuildingList[i]);
				}
			}
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000AE11 File Offset: 0x00009011
	public GameObject GetDefaultBuildingObject(int index)
	{
		if (this.m_DefaultBuildingList != null && index >= 0 && index < this.m_DefaultBuildingList.Length && this.m_DefaultBuildingList[index] != null)
		{
			return this.m_DefaultBuildingList[index].gameObject;
		}
		return null;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000AE49 File Offset: 0x00009049
	public void RegisterEventHandlers(GameEventBuffer game_event_buffer)
	{
		if (game_event_buffer == null)
		{
			return;
		}
		game_event_buffer.RegisterEventHandler(7, new UnityAction<IntPtr, GameEventFeedback>(this.HandleUpdateActionSpace));
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000AE62 File Offset: 0x00009062
	public Sprite GetBuildingFactionSprite(int faction_index)
	{
		if (this.m_BuildingFactionSprites == null)
		{
			return null;
		}
		if (faction_index < 0 || faction_index >= this.m_BuildingFactionSprites.Length)
		{
			return null;
		}
		return this.m_BuildingFactionSprites[faction_index];
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000AE88 File Offset: 0x00009088
	private void HandleUpdateActionSpace(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		ActionSpaceStatus actionSpaceStatus = (ActionSpaceStatus)Marshal.PtrToStructure(event_buffer, typeof(ActionSpaceStatus));
		AgricolaActionSpace actionSpaceByInstanceID = this.GetActionSpaceByInstanceID(actionSpaceStatus.cardinplay_instance_id, false);
		if (actionSpaceByInstanceID != null)
		{
			actionSpaceByInstanceID.SetAccumulateResourceCount(actionSpaceStatus.accumulate_resource);
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000AED0 File Offset: 0x000090D0
	public void OnTownHelpPressed()
	{
		this.m_bBuildingHelpToggle = !this.m_bBuildingHelpToggle;
		for (int i = 0; i < this.m_ActiveBuildingList.Count; i++)
		{
			this.m_ActiveBuildingList[i].ToggleHelpObject(this.m_bBuildingHelpToggle);
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000AF1C File Offset: 0x0000911C
	public void RebuildBuildingList()
	{
		List<AgricolaBuilding> activeBuildingList = this.m_ActiveBuildingList;
		this.m_ActiveBuildingList = new List<AgricolaBuilding>();
		List<AgricolaActionSpace> activeActionSpaceList = this.m_ActiveActionSpaceList;
		this.m_ActiveActionSpaceList = new List<AgricolaActionSpace>();
		if (this.m_DefaultBuildingList != null)
		{
			for (int i = 0; i < this.m_DefaultBuildingList.Length; i++)
			{
				AgricolaBuilding agricolaBuilding = this.m_DefaultBuildingList[i];
				if (agricolaBuilding != null)
				{
					agricolaBuilding.ClearActiveActionSpaces();
				}
			}
		}
		int[] array = new int[64];
		GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr pInstanceIDs = gchandle.AddrOfPinnedObject();
		int instanceList = AgricolaLib.GetInstanceList(3, 0, pInstanceIDs, 64);
		GCHandle gchandle2 = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle2.AddrOfPinnedObject();
		for (int j = 0; j < instanceList; j++)
		{
			int num = array[j];
			AgricolaLib.GetInstanceData(3, num, intPtr, 512);
			BuildingData buildingData = (BuildingData)Marshal.PtrToStructure(intPtr, typeof(BuildingData));
			if ((int)buildingData.building_instance_id == num)
			{
				string building_name = buildingData.building_name;
				bool flag = false;
				if (this.m_DefaultBuildingList != null)
				{
					for (int k = 0; k < this.m_DefaultBuildingList.Length; k++)
					{
						AgricolaBuilding agricolaBuilding2 = this.m_DefaultBuildingList[k];
						if (!(agricolaBuilding2 == null))
						{
							AgricolaActionSpace agricolaActionSpace = agricolaBuilding2.FindActionSpace(building_name);
							if (!(agricolaActionSpace == null))
							{
								agricolaBuilding2.SetBuildingActive(true);
								agricolaBuilding2.AddActiveActionSpace(agricolaActionSpace);
								agricolaActionSpace.SetActiveActionSpaceName(building_name);
								Debug.Log("Action Space " + building_name + ": " + buildingData.building_instance_id.ToString());
								agricolaActionSpace.SetActionSpaceInstanceID((int)buildingData.building_instance_id);
								agricolaActionSpace.SetSourceCardInstanceID((int)buildingData.sourcecard_instance_id);
								DragTargetZone component = agricolaActionSpace.GetComponent<DragTargetZone>();
								if (component != null)
								{
									this.m_DragManager.AddAdditionalDragTargetZone(component);
								}
								agricolaActionSpace.SetLockedPopupCard(false);
								agricolaActionSpace.SetAccumulateResourceCount((int)buildingData.accumulate_resource);
								activeActionSpaceList.Remove(agricolaActionSpace);
								if (!this.m_ActiveActionSpaceList.Contains(agricolaActionSpace))
								{
									this.m_ActiveActionSpaceList.Add(agricolaActionSpace);
								}
								activeBuildingList.Remove(agricolaBuilding2);
								if (!this.m_ActiveBuildingList.Contains(agricolaBuilding2))
								{
									this.m_ActiveBuildingList.Add(agricolaBuilding2);
								}
								this.m_MasterActionSpaceList.Remove(num);
								this.m_MasterActionSpaceList.Add(num, agricolaActionSpace);
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					Debug.LogError("Could not find action space " + building_name);
				}
			}
		}
		gchandle2.Free();
		gchandle.Free();
		if (this.m_DefaultBuildingList != null)
		{
			for (int l = 0; l < this.m_DefaultBuildingList.Length; l++)
			{
				AgricolaBuilding agricolaBuilding3 = this.m_DefaultBuildingList[l];
				if (agricolaBuilding3 != null)
				{
					AgricolaActionSpace agricolaActionSpace2 = agricolaBuilding3.SetLockedActionSpaces();
					if (agricolaActionSpace2 != null)
					{
						activeActionSpaceList.Remove(agricolaActionSpace2);
						if (!this.m_ActiveActionSpaceList.Contains(agricolaActionSpace2))
						{
							this.m_ActiveActionSpaceList.Add(agricolaActionSpace2);
						}
						activeBuildingList.Remove(agricolaBuilding3);
						if (!this.m_ActiveBuildingList.Contains(agricolaBuilding3))
						{
							this.m_ActiveBuildingList.Add(agricolaBuilding3);
						}
					}
				}
			}
		}
		while (activeBuildingList.Count > 0)
		{
			AgricolaBuilding agricolaBuilding4 = activeBuildingList[0];
			agricolaBuilding4.SetBuildingActive(false);
			activeBuildingList.Remove(agricolaBuilding4);
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00003022 File Offset: 0x00001222
	public void ClearBuildings()
	{
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000B264 File Offset: 0x00009464
	public void RebuildAnimationManager(AnimationManager animation_manager)
	{
		if (this.m_ActiveBuildingList != null)
		{
			for (int i = 0; i < this.m_ActiveBuildingList.Count; i++)
			{
				if (this.m_ActiveBuildingList[i] != null)
				{
					this.m_ActiveBuildingList[i].RebuildAnimationManager(animation_manager);
				}
			}
		}
		if (this.m_ActiveActionSpaceList != null)
		{
			for (int j = 0; j < this.m_ActiveActionSpaceList.Count; j++)
			{
				if (this.m_ActiveActionSpaceList[j] != null)
				{
					this.m_ActiveActionSpaceList[j].RebuildAnimationManager(animation_manager);
				}
			}
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000B2FC File Offset: 0x000094FC
	public AgricolaBuilding CreateBuildingInPlay(int building_instance_id)
	{
		if (this.m_BuildingInPlayPrefab == null)
		{
			return null;
		}
		AgricolaBuilding agricolaBuilding = null;
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.GetInstanceData(2, building_instance_id, intPtr, 1024);
		BuildingData buildingData = (BuildingData)Marshal.PtrToStructure(intPtr, typeof(BuildingData));
		if ((int)buildingData.building_instance_id == building_instance_id)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_BuildingInPlayPrefab);
			if (gameObject != null)
			{
				agricolaBuilding = gameObject.GetComponent<AgricolaBuilding>();
				if (agricolaBuilding != null)
				{
					agricolaBuilding.name = "[Building] " + buildingData.building_name;
					DragTargetZone component = agricolaBuilding.GetComponent<DragTargetZone>();
					if (component != null)
					{
						this.m_DragManager.AddAdditionalDragTargetZone(component);
					}
					if (this.m_CardManager != null)
					{
						GameObject cardFromInstanceID = this.m_CardManager.GetCardFromInstanceID((int)buildingData.sourcecard_instance_id, true);
						if (cardFromInstanceID != null)
						{
							cardFromInstanceID.GetComponent<AgricolaCard>() != null;
						}
					}
				}
			}
		}
		gchandle.Free();
		return agricolaBuilding;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000B406 File Offset: 0x00009606
	public AgricolaActionSpace GetActionSpaceByInstanceID(int actionSpaceInstanceID, bool bCreateIfNecessary = false)
	{
		return (AgricolaActionSpace)this.m_MasterActionSpaceList[actionSpaceInstanceID];
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000B420 File Offset: 0x00009620
	public void UpdateSelectionState(bool bHighlight)
	{
		foreach (AgricolaBuilding x in this.m_ActiveBuildingList)
		{
			x != null;
		}
		foreach (AgricolaActionSpace agricolaActionSpace in this.m_ActiveActionSpaceList)
		{
			if (agricolaActionSpace != null)
			{
				agricolaActionSpace.UpdateSelectionState(bHighlight);
			}
		}
	}

	// Token: 0x0400016A RID: 362
	private const int k_maxDataSize = 1024;

	// Token: 0x0400016B RID: 363
	private const int k_maxBuildingCount = 64;

	// Token: 0x0400016C RID: 364
	private const int k_maxActionSpaceCount = 64;

	// Token: 0x0400016D RID: 365
	[SerializeField]
	protected AnimationManager m_AnimationManager;

	// Token: 0x0400016E RID: 366
	[SerializeField]
	protected DragManager m_DragManager;

	// Token: 0x0400016F RID: 367
	[SerializeField]
	protected CardManager m_CardManager;

	// Token: 0x04000170 RID: 368
	[SerializeField]
	private GameObject m_BuildingInPlayPrefab;

	// Token: 0x04000171 RID: 369
	[SerializeField]
	private GameObject m_BuildingVPConnectPrefab;

	// Token: 0x04000172 RID: 370
	[SerializeField]
	private GameObject m_BuildingVPTokenPrefab;

	// Token: 0x04000173 RID: 371
	[SerializeField]
	private GameObject m_AuxiliaryResourceTrayMapPrefab;

	// Token: 0x04000174 RID: 372
	[SerializeField]
	private GameObject m_AuxiliaryResourceTrayOwnedPrefab;

	// Token: 0x04000175 RID: 373
	[SerializeField]
	private GameObject[] m_AuxiliaryResourcePrefabs;

	// Token: 0x04000176 RID: 374
	[SerializeField]
	private AgricolaBuilding[] m_DefaultBuildingList;

	// Token: 0x04000177 RID: 375
	[SerializeField]
	private Sprite[] m_BuildingFactionSprites;

	// Token: 0x04000178 RID: 376
	private Hashtable m_MasterActionSpaceList;

	// Token: 0x04000179 RID: 377
	private bool m_bBuildingHelpToggle;

	// Token: 0x0400017A RID: 378
	private List<AgricolaBuilding> m_ActiveBuildingList;

	// Token: 0x0400017B RID: 379
	private List<AgricolaActionSpace> m_ActiveActionSpaceList;
}
