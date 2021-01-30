using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class AgricolaFarmTile_Room : AgricolaFarmTile_Base
{
	// Token: 0x06000349 RID: 841 RVA: 0x00015838 File Offset: 0x00013A38
	public void ClearWorkers()
	{
		AgricolaFarmTile_Locators locators = this.m_parent.GetLocators();
		for (int i = 0; i < this.m_workers.Count; i++)
		{
			locators.RemoveObject(this.m_workers[i]);
		}
		this.m_workers.Clear();
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00015884 File Offset: 0x00013A84
	public void AddWorker(GameObject newWorker)
	{
		if (this.m_parent == null)
		{
			Debug.LogError("AgricolaFarmTile_Room: adding worker to uniintialized farm tile");
			return;
		}
		this.m_workers.Add(newWorker);
		AgricolaFarmTile_Locators locators = this.m_parent.GetLocators();
		if (locators != null)
		{
			locators.RemoveAllObjects();
			switch (this.m_workers.Count)
			{
			case 1:
				locators.AddObjectToLocator(this.m_workers[0], 4);
				break;
			case 2:
				locators.AddObjectToLocator(this.m_workers[0], 2);
				locators.AddObjectToLocator(this.m_workers[1], 6);
				break;
			case 3:
				locators.AddObjectToLocator(this.m_workers[0], 0);
				locators.AddObjectToLocator(this.m_workers[1], 2);
				locators.AddObjectToLocator(this.m_workers[2], 7);
				break;
			default:
				Debug.LogWarning("Too many workers in one farm tile!");
				break;
			}
			locators.PlaceOrphanedObjects();
		}
		else
		{
			newWorker.transform.SetParent(base.gameObject.transform, false);
			newWorker.transform.localPosition = Vector3.zero;
		}
		newWorker.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
	}

	// Token: 0x0600034B RID: 843 RVA: 0x000159D4 File Offset: 0x00013BD4
	public void SetHouseType(EFarmTileRoomType roomType)
	{
		if (roomType == EFarmTileRoomType.NONE)
		{
			roomType = EFarmTileRoomType.WOOD;
		}
		this.m_activeRoomType = (uint)(roomType - EFarmTileRoomType.WOOD);
		if (this.m_woodNode != null)
		{
			this.m_woodNode.SetActive(roomType == EFarmTileRoomType.WOOD);
		}
		if (this.m_clayNode != null)
		{
			this.m_clayNode.SetActive(roomType == EFarmTileRoomType.CLAY);
		}
		if (this.m_stoneNode != null)
		{
			this.m_stoneNode.SetActive(roomType == EFarmTileRoomType.STONE);
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00015A48 File Offset: 0x00013C48
	public void SetSouthConnection(bool bIsOn)
	{
		if (this.m_connectionSouth != null)
		{
			for (int i = 0; i < this.m_connectionSouth.Length; i++)
			{
				this.m_connectionSouth[i].SetActive(bIsOn);
			}
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00015A80 File Offset: 0x00013C80
	public void SetWestConnection(bool bIsOn)
	{
		if (this.m_connectionWest != null)
		{
			for (int i = 0; i < this.m_connectionWest.Length; i++)
			{
				this.m_connectionWest[i].SetActive(bIsOn);
			}
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00015AB8 File Offset: 0x00013CB8
	public void SetSWConnection(bool bIsOn)
	{
		if (this.m_connectionSWCorner != null)
		{
			for (int i = 0; i < this.m_connectionSWCorner.Length; i++)
			{
				this.m_connectionSWCorner[i].SetActive(bIsOn);
			}
		}
	}

	// Token: 0x04000299 RID: 665
	[SerializeField]
	private GameObject m_woodNode;

	// Token: 0x0400029A RID: 666
	[SerializeField]
	private GameObject m_clayNode;

	// Token: 0x0400029B RID: 667
	[SerializeField]
	private GameObject m_stoneNode;

	// Token: 0x0400029C RID: 668
	[SerializeField]
	private GameObject[] m_connectionSouth;

	// Token: 0x0400029D RID: 669
	[SerializeField]
	private GameObject[] m_connectionWest;

	// Token: 0x0400029E RID: 670
	[SerializeField]
	private GameObject[] m_connectionSWCorner;

	// Token: 0x0400029F RID: 671
	private uint m_activeRoomType;

	// Token: 0x040002A0 RID: 672
	private List<GameObject> m_workers = new List<GameObject>();
}
