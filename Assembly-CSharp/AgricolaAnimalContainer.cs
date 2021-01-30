using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class AgricolaAnimalContainer
{
	// Token: 0x06000196 RID: 406 RVA: 0x000089C9 File Offset: 0x00006BC9
	public bool GetNeedToSubmitAnimals()
	{
		return this.m_resources != this.m_resourceTotalsFromRules;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x000089DC File Offset: 0x00006BDC
	public void FinalizeSubmission()
	{
		this.m_resourceTotalsFromRules.sheep = this.m_resources.sheep;
		this.m_resourceTotalsFromRules.boar = this.m_resources.boar;
		this.m_resourceTotalsFromRules.cattle = this.m_resources.cattle;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00008A2C File Offset: 0x00006C2C
	public void RevertSubmission()
	{
		this.m_resources.sheep = this.m_resourceTotalsFromRules.sheep;
		this.m_resources.boar = this.m_resourceTotalsFromRules.boar;
		this.m_resources.cattle = this.m_resourceTotalsFromRules.cattle;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00008A7B File Offset: 0x00006C7B
	public void SetParent(AgricolaFarm parent)
	{
		this.m_parent = parent;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00008A84 File Offset: 0x00006C84
	public void AddTile(AgricolaFarmTile tile)
	{
		if (!this.m_tiles.Contains(tile))
		{
			this.m_tiles.Add(tile);
			this.m_tilesBitfield |= 1 << tile.GetTileIndex();
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00008AB8 File Offset: 0x00006CB8
	public void RemoveTile(AgricolaFarmTile tile)
	{
		if (this.m_tiles.Contains(tile))
		{
			this.m_tiles.Remove(tile);
			this.m_tilesBitfield &= ~(1 << tile.GetTileIndex());
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00008AEE File Offset: 0x00006CEE
	public void ClearTiles()
	{
		this.m_tiles.Clear();
		this.m_tilesBitfield = 0;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00008B02 File Offset: 0x00006D02
	public int GetAnimalCount(EResourceType animalType)
	{
		switch (animalType)
		{
		case EResourceType.SHEEP:
			return (int)this.m_resources.sheep;
		case EResourceType.WILDBOAR:
			return (int)this.m_resources.boar;
		case EResourceType.CATTLE:
			return (int)this.m_resources.cattle;
		default:
			return 0;
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00008B40 File Offset: 0x00006D40
	public bool AddAnimal(AgricolaAnimal animal, EResourceType type, AgricolaFarmTile droppedTile = null)
	{
		if (this.m_resources.sheep + this.m_resources.boar + this.m_resources.cattle >= this.m_capacity)
		{
			return false;
		}
		AgricolaFarmTile_Locators agricolaFarmTile_Locators = null;
		int num = -1;
		if (droppedTile != null)
		{
			num = droppedTile.GetLocators().GetClosestOpenLocatorToMouse();
			if (num != -1)
			{
				agricolaFarmTile_Locators = droppedTile.GetLocators();
			}
		}
		if (agricolaFarmTile_Locators == null)
		{
			int num2 = 100;
			int num3 = -1;
			for (int i = 0; i < this.m_tiles.Count; i++)
			{
				int locatedObjectsCount = this.m_tiles[i].GetLocators().GetLocatedObjectsCount();
				if (locatedObjectsCount < num2)
				{
					num2 = locatedObjectsCount;
					num3 = i;
				}
			}
			if (num3 != -1)
			{
				agricolaFarmTile_Locators = this.m_tiles[num3].GetLocators();
				num = agricolaFarmTile_Locators.GetRandomEmptyLocator();
			}
		}
		if (agricolaFarmTile_Locators == null)
		{
			Debug.LogError("Unable to find an empty locator!");
			return false;
		}
		agricolaFarmTile_Locators.AddObjectToLocator(animal.gameObject, num);
		animal.ResetRotationAndScale();
		animal.SetContainerIndex(this.m_rulesIndex);
		if (type == EResourceType.SHEEP)
		{
			this.m_resources.sheep = this.m_resources.sheep + 1;
		}
		else if (type == EResourceType.WILDBOAR)
		{
			this.m_resources.boar = this.m_resources.boar + 1;
		}
		else if (type == EResourceType.CATTLE)
		{
			this.m_resources.cattle = this.m_resources.cattle + 1;
		}
		return true;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00008C84 File Offset: 0x00006E84
	public AgricolaAnimal RemoveAnimal(AgricolaAnimal animal, EResourceType type)
	{
		if (!(animal != null))
		{
			bool flag = false;
			int num = 0;
			while (num < this.m_tiles.Count && !flag)
			{
				AgricolaFarmTile_Locators locators = this.m_tiles[num].GetLocators();
				int locatorCount = locators.GetLocatorCount();
				for (int i = 0; i < locatorCount; i++)
				{
					GameObject objectAtLocator = locators.GetObjectAtLocator(i);
					if (objectAtLocator != null)
					{
						animal = objectAtLocator.GetComponent<AgricolaAnimal>();
						if (animal != null && animal.GetAnimalType() == type)
						{
							flag = true;
							break;
						}
					}
				}
				num++;
			}
			if (!flag)
			{
				return null;
			}
		}
		if (type == EResourceType.SHEEP)
		{
			this.m_resources.sheep = this.m_resources.sheep - 1;
		}
		else if (type == EResourceType.WILDBOAR)
		{
			this.m_resources.boar = this.m_resources.boar - 1;
		}
		else if (type == EResourceType.CATTLE)
		{
			this.m_resources.cattle = this.m_resources.cattle - 1;
		}
		return animal;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00008D60 File Offset: 0x00006F60
	public void RemoveAllAnimalsToLimbo()
	{
		for (int i = 0; i < this.m_tiles.Count; i++)
		{
			AgricolaFarmTile_Locators locators = this.m_tiles[i].GetLocators();
			int locatorCount = locators.GetLocatorCount();
			for (int j = 0; j < locatorCount; j++)
			{
				GameObject objectAtLocator = locators.GetObjectAtLocator(j);
				if (objectAtLocator != null)
				{
					AgricolaAnimal component = objectAtLocator.GetComponent<AgricolaAnimal>();
					if (component != null)
					{
						this.m_parent.PlaceAnimalInLimbo(component);
					}
				}
			}
		}
		this.m_resources.sheep = 0;
		this.m_resources.boar = 0;
		this.m_resources.cattle = 0;
	}

	// Token: 0x040000CF RID: 207
	public int m_rulesIndex;

	// Token: 0x040000D0 RID: 208
	public ushort m_id;

	// Token: 0x040000D1 RID: 209
	public int m_tilesBitfield;

	// Token: 0x040000D2 RID: 210
	public AnimalContainerType m_type;

	// Token: 0x040000D3 RID: 211
	public byte m_capacity;

	// Token: 0x040000D4 RID: 212
	public AnimalCapacityType m_capacityType;

	// Token: 0x040000D5 RID: 213
	public AnimalResources m_resourceTotalsFromRules;

	// Token: 0x040000D6 RID: 214
	public AnimalResources m_resources;

	// Token: 0x040000D7 RID: 215
	private AgricolaFarm m_parent;

	// Token: 0x040000D8 RID: 216
	private List<AgricolaFarmTile> m_tiles = new List<AgricolaFarmTile>();
}
