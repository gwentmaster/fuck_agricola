using System;
using System.Collections;
using System.Runtime.InteropServices;
using GameData;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class AgricolaWorkerManager : MonoBehaviour
{
	// Token: 0x06000484 RID: 1156 RVA: 0x00023564 File Offset: 0x00021764
	public float GetDragWorkerTargetScale()
	{
		return this.m_DragWorkerTargetScale;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0002356C File Offset: 0x0002176C
	private void Awake()
	{
		this.m_WorkerListBuffer = new int[64];
		this.m_WorkerListBufferHandle = GCHandle.Alloc(this.m_WorkerListBuffer, GCHandleType.Pinned);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0002358D File Offset: 0x0002178D
	private void OnDestroy()
	{
		this.m_WorkerListBufferHandle.Free();
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0002359C File Offset: 0x0002179C
	public GameObject CreateWorkerFromInstanceID(int workerInstanceID)
	{
		GameObject gameObject = null;
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.GetInstanceData(6, workerInstanceID, intPtr, 1024);
		WorkerData workerData = (WorkerData)Marshal.PtrToStructure(intPtr, typeof(WorkerData));
		if ((int)workerData.worker_instance_id == workerInstanceID && this.m_WorkerPrefab != null)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_WorkerPrefab);
			if (gameObject != null)
			{
				if (workerData.owner_instance_id == -1)
				{
					gameObject.name = "Ambassador";
				}
				else
				{
					gameObject.name = "Worker: " + workerData.owner_name;
				}
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
				AgricolaWorker component3 = gameObject.GetComponent<AgricolaWorker>();
				if (component3 != null)
				{
					component3.SetWorkerManager(this);
					component3.SetWorkerInstanceID(workerInstanceID);
					if (workerData.owner_instance_id == -1)
					{
						component3.SetAmbassdor(true);
					}
					component3.SetOwner((int)workerData.owner_instance_id, (int)workerData.owner_faction_index, (int)workerData.avatar_id);
				}
				gameObject.SetActive(false);
				this.m_MasterWorkerList.Add(workerInstanceID, gameObject);
			}
		}
		gchandle.Free();
		return gameObject;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00023710 File Offset: 0x00021910
	public AgricolaWorker CreateTemporaryWorker(int ownerInstanceID, int factionIndex)
	{
		AgricolaWorker agricolaWorker = null;
		if (this.m_WorkerPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_WorkerPrefab);
			if (gameObject != null)
			{
				gameObject.name = "Temporary Worker: ";
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
				agricolaWorker = gameObject.GetComponent<AgricolaWorker>();
				if (agricolaWorker != null)
				{
					agricolaWorker.SetWorkerManager(this);
					agricolaWorker.SetWorkerInstanceID(0);
					agricolaWorker.SetTemporaryWorker(true);
					agricolaWorker.SetOwner(ownerInstanceID, factionIndex, 64);
				}
				gameObject.SetActive(false);
			}
		}
		return agricolaWorker;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x000237DE File Offset: 0x000219DE
	public void ReturnTemporaryWorker(AgricolaWorker returnWorker)
	{
		if (returnWorker == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(returnWorker.gameObject);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000237F8 File Offset: 0x000219F8
	public AgricolaFarmAction CreateTemporaryFarmAction(int ownerInstanceID, int factionIndex, ushort farmActionHint)
	{
		AgricolaFarmAction agricolaFarmAction = null;
		if (this.m_FarmActionPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_FarmActionPrefab);
			if (gameObject != null)
			{
				gameObject.name = "Farm Action: ";
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
				agricolaFarmAction = gameObject.GetComponent<AgricolaFarmAction>();
				if (agricolaFarmAction != null)
				{
					agricolaFarmAction.SetWorkerManager(this);
					agricolaFarmAction.SetFarmActionHint(farmActionHint);
					agricolaFarmAction.SetOwner(ownerInstanceID, factionIndex, 64);
				}
				gameObject.SetActive(false);
			}
		}
		return agricolaFarmAction;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x000237DE File Offset: 0x000219DE
	public void ReturnTemporaryFarmAction(AgricolaFarmAction returnFarmAction)
	{
		if (returnFarmAction == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(returnFarmAction.gameObject);
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000238C0 File Offset: 0x00021AC0
	public GameObject GetWorkerFromInstanceID(int instanceID, bool bCreateIfNecessary = false)
	{
		GameObject gameObject = (GameObject)this.m_MasterWorkerList[instanceID];
		if (gameObject == null && bCreateIfNecessary)
		{
			gameObject = this.CreateWorkerFromInstanceID(instanceID);
		}
		return gameObject;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x000238F8 File Offset: 0x00021AF8
	public void UpdateSelectionState(bool bHighlight)
	{
		foreach (object obj in this.m_MasterWorkerList.Values)
		{
			AgricolaWorker component = ((GameObject)obj).GetComponent<AgricolaWorker>();
			if (component != null)
			{
				component.UpdateSelectionState(bHighlight);
			}
		}
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00023964 File Offset: 0x00021B64
	public AgricolaWorker GetUnassignedWorker(int playerIndex)
	{
		AgricolaWorker result = null;
		IntPtr pInstanceIDs = this.m_WorkerListBufferHandle.AddrOfPinnedObject();
		int instanceList = AgricolaLib.GetInstanceList(6, playerIndex, pInstanceIDs, 64);
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		for (int i = 0; i < instanceList; i++)
		{
			int num = this.m_WorkerListBuffer[i];
			AgricolaLib.GetInstanceData(6, num, intPtr, 1024);
			WorkerData workerData = (WorkerData)Marshal.PtrToStructure(intPtr, typeof(WorkerData));
			if ((int)workerData.worker_instance_id == num && workerData.assigned_location_instance_id == 0)
			{
				GameObject workerFromInstanceID = this.GetWorkerFromInstanceID((int)workerData.worker_instance_id, true);
				if (workerFromInstanceID != null)
				{
					result = workerFromInstanceID.GetComponent<AgricolaWorker>();
					break;
				}
			}
		}
		gchandle.Free();
		return result;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00023A28 File Offset: 0x00021C28
	public bool PlaceWorkerInWorkerLimbo(AgricolaWorker card)
	{
		if (this.m_WorkerLimbo == null || card == null)
		{
			return false;
		}
		AnimateObject component = card.GetComponent<AnimateObject>();
		if (component == null)
		{
			return false;
		}
		this.m_WorkerLimbo.PlaceAnimateObject(component, true, true, false);
		return true;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00023A70 File Offset: 0x00021C70
	public void PlaceAllWorkersInLimbo()
	{
		if (this.m_WorkerLimbo == null)
		{
			return;
		}
		foreach (object obj in this.m_MasterWorkerList.Values)
		{
			AnimateObject component = ((GameObject)obj).GetComponent<AnimateObject>();
			if (component != null)
			{
				this.m_WorkerLimbo.PlaceAnimateObject(component, true, true, false);
			}
		}
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00023AF4 File Offset: 0x00021CF4
	public void RebuildWorkerList()
	{
		GCHandle gchandle = GCHandle.Alloc(new byte[1024], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		IntPtr pInstanceIDs = this.m_WorkerListBufferHandle.AddrOfPinnedObject();
		int num = 0;
		int instanceList;
		for (;;)
		{
			int localOpponentPlayerIndex = AgricolaLib.GetLocalOpponentPlayerIndex(num++);
			if (localOpponentPlayerIndex == 0)
			{
				break;
			}
			instanceList = AgricolaLib.GetInstanceList(6, localOpponentPlayerIndex, pInstanceIDs, 64);
			for (int i = 0; i < instanceList; i++)
			{
				int num2 = this.m_WorkerListBuffer[i];
				AgricolaLib.GetInstanceData(6, num2, intPtr, 1024);
				WorkerData workerData = (WorkerData)Marshal.PtrToStructure(intPtr, typeof(WorkerData));
				if ((int)workerData.worker_instance_id == num2)
				{
					GameObject workerFromInstanceID = this.GetWorkerFromInstanceID((int)workerData.worker_instance_id, true);
					if (workerFromInstanceID != null)
					{
						if (workerData.assigned_location_instance_id != 0 && this.m_BuildingManager != null)
						{
							AgricolaActionSpace actionSpaceByInstanceID = this.m_BuildingManager.GetActionSpaceByInstanceID((int)workerData.assigned_location_instance_id, false);
							if (actionSpaceByInstanceID != null)
							{
								AgricolaWorker component = workerFromInstanceID.GetComponent<AgricolaWorker>();
								if (component != null)
								{
									actionSpaceByInstanceID.AssignWorker(component);
									workerFromInstanceID.SetActive(true);
								}
							}
						}
						else if (this.m_WorkerLimbo != null)
						{
							AnimateObject component2 = workerFromInstanceID.GetComponent<AnimateObject>();
							if (component2 != null)
							{
								this.m_WorkerLimbo.PlaceAnimateObject(component2, true, true, false);
							}
						}
					}
				}
			}
		}
		instanceList = AgricolaLib.GetInstanceList(6, 0, pInstanceIDs, 64);
		for (int j = 0; j < instanceList; j++)
		{
			int num3 = this.m_WorkerListBuffer[j];
			AgricolaLib.GetInstanceData(6, num3, intPtr, 1024);
			WorkerData workerData2 = (WorkerData)Marshal.PtrToStructure(intPtr, typeof(WorkerData));
			if ((int)workerData2.worker_instance_id == num3)
			{
				GameObject workerFromInstanceID2 = this.GetWorkerFromInstanceID((int)workerData2.worker_instance_id, true);
				if (workerFromInstanceID2 != null)
				{
					if (workerData2.assigned_location_instance_id != 0 && this.m_BuildingManager != null)
					{
						AgricolaActionSpace actionSpaceByInstanceID2 = this.m_BuildingManager.GetActionSpaceByInstanceID((int)workerData2.assigned_location_instance_id, false);
						if (actionSpaceByInstanceID2 != null)
						{
							AgricolaWorker component3 = workerFromInstanceID2.GetComponent<AgricolaWorker>();
							if (component3 != null)
							{
								actionSpaceByInstanceID2.AssignWorker(component3);
								workerFromInstanceID2.SetActive(true);
							}
						}
					}
					else if (this.m_WorkerLimbo != null)
					{
						AnimateObject component4 = workerFromInstanceID2.GetComponent<AnimateObject>();
						if (component4 != null)
						{
							this.m_WorkerLimbo.PlaceAnimateObject(component4, true, true, false);
						}
					}
				}
			}
		}
		gchandle.Free();
	}

	// Token: 0x04000408 RID: 1032
	private const int k_maxDataSize = 1024;

	// Token: 0x04000409 RID: 1033
	private const int k_maxWorkerCount = 64;

	// Token: 0x0400040A RID: 1034
	private const int k_guestAvatarIndex = 64;

	// Token: 0x0400040B RID: 1035
	[SerializeField]
	protected AnimationManager m_AnimationManager;

	// Token: 0x0400040C RID: 1036
	[SerializeField]
	protected DragManager m_DragManager;

	// Token: 0x0400040D RID: 1037
	[SerializeField]
	protected AgricolaBuildingManager m_BuildingManager;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private AgricolaAnimationLocator m_WorkerLimbo;

	// Token: 0x0400040F RID: 1039
	[SerializeField]
	private GameObject m_WorkerPrefab;

	// Token: 0x04000410 RID: 1040
	[SerializeField]
	private GameObject m_FarmActionPrefab;

	// Token: 0x04000411 RID: 1041
	[SerializeField]
	private float m_DragWorkerTargetScale = 1f;

	// Token: 0x04000412 RID: 1042
	private Hashtable m_MasterWorkerList = new Hashtable();

	// Token: 0x04000413 RID: 1043
	private int[] m_WorkerListBuffer;

	// Token: 0x04000414 RID: 1044
	private GCHandle m_WorkerListBufferHandle;
}
