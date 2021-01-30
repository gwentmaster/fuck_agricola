using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003B RID: 59
public class AgricolaFarmTile_Locators : AgricolaFarmTile_Base
{
	// Token: 0x06000313 RID: 787 RVA: 0x000148C1 File Offset: 0x00012AC1
	public void SetDragTargetIndex(int tileIndex)
	{
		base.GetComponent<DragTargetZone>().SetDragTargetInstanceID((ushort)tileIndex);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x000148D0 File Offset: 0x00012AD0
	public void SetDragSelectionHint(ushort hint, Color hintColor, ushort overrideID = 0)
	{
		base.GetComponent<DragTargetZone>().SetDragSelectionHint(hint, hintColor, overrideID);
		this.m_dragTargetImage.raycastTarget = (hint > 0);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x000148EF File Offset: 0x00012AEF
	public int GetLocatorCount()
	{
		if (this.m_locators != null)
		{
			return this.m_locators.Length;
		}
		return 0;
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00014903 File Offset: 0x00012B03
	public GameObject GetLocator(int index)
	{
		if (index >= 0 && index < this.m_locators.Length)
		{
			return this.m_locators[index];
		}
		return null;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00014920 File Offset: 0x00012B20
	public int GetLocatedObjectsCount()
	{
		int num = 0;
		for (int i = 0; i < this.m_locators.Length; i++)
		{
			if (this.m_locators[i] != null)
			{
				num += this.m_locators[i].transform.childCount;
			}
		}
		return num;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00014968 File Offset: 0x00012B68
	public bool AddObjectToLocator(GameObject obj, int index)
	{
		if (obj != null && !this.GetIsLocatorOccupied(index))
		{
			obj.transform.SetParent(this.m_locators[index].transform, false);
			obj.transform.localPosition = Vector3.zero;
			return true;
		}
		return false;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x000149A8 File Offset: 0x00012BA8
	public bool AddObjectToNextLocator(GameObject obj)
	{
		int nextEmptyLocator = this.GetNextEmptyLocator();
		return nextEmptyLocator != -1 && this.AddObjectToLocator(obj, nextEmptyLocator);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x000149CC File Offset: 0x00012BCC
	public bool AddObjectToRandomLocator(GameObject obj)
	{
		int randomEmptyLocator = this.GetRandomEmptyLocator();
		return randomEmptyLocator != -1 && this.AddObjectToLocator(obj, randomEmptyLocator);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000149F0 File Offset: 0x00012BF0
	public void RemoveAllObjects()
	{
		if (this.m_locators != null)
		{
			for (int i = 0; i < this.m_locators.Length; i++)
			{
				this.RemoveObjectAtIndex(i);
			}
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00014A20 File Offset: 0x00012C20
	public void RemoveObjectAtIndex(int index)
	{
		if (this.m_locators != null && this.m_bootedObjects != null && this.m_locators.Length > index && this.m_locators[index].transform.childCount > 0)
		{
			this.m_locators[index].transform.GetChild(0).transform.SetParent(this.m_bootedObjects.transform);
		}
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00014A8C File Offset: 0x00012C8C
	public void RemoveObject(GameObject obj)
	{
		if (this.m_locators != null && this.m_bootedObjects != null)
		{
			for (int i = 0; i < this.m_locators.Length; i++)
			{
				if (obj == this.GetObjectAtLocator(i))
				{
					this.m_locators[i].transform.GetChild(0).transform.SetParent(this.m_bootedObjects.transform);
					return;
				}
			}
		}
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00014AFC File Offset: 0x00012CFC
	public bool PlaceOrphanedObjects()
	{
		if (this.m_bootedObjects != null)
		{
			foreach (object obj in this.m_bootedObjects.transform)
			{
				Transform transform = (Transform)obj;
				this.AddObjectToRandomLocator(transform.gameObject);
			}
		}
		return this.m_bootedObjects.transform.childCount == 0;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00014B84 File Offset: 0x00012D84
	public bool GetIsLocatorOccupied(int index)
	{
		return this.m_locators == null || this.m_locators.Length <= index || this.m_locators[index].transform.childCount > 0;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00014BB0 File Offset: 0x00012DB0
	public bool GetHasObject(GameObject obj)
	{
		for (int i = 0; i < this.m_locators.Length; i++)
		{
			if (this.m_locators[i].transform.childCount > 0 && this.m_locators[i].transform.GetChild(0).gameObject == obj)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00014C08 File Offset: 0x00012E08
	public GameObject GetObjectAtLocator(int index)
	{
		if (this.m_locators != null && this.m_locators.Length > index && this.m_locators[index].transform.childCount > 0)
		{
			return this.m_locators[index].transform.GetChild(0).gameObject;
		}
		return null;
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00014C58 File Offset: 0x00012E58
	public int GetNextEmptyLocator()
	{
		if (this.m_locators != null)
		{
			for (int i = 0; i < this.m_locators.Length; i++)
			{
				if (this.m_locators[i] != null && this.m_locators[i].transform.childCount == 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00014CA8 File Offset: 0x00012EA8
	public int GetRandomEmptyLocator()
	{
		if (this.m_locators != null)
		{
			System.Random r = new System.Random();
			IEnumerable<int> source = Enumerable.Range(0, this.m_locators.Length);
			Func<int, int> keySelector;
			Func<int, int> <>9__0;
			if ((keySelector = <>9__0) == null)
			{
				keySelector = (<>9__0 = ((int x) => r.Next()));
			}
			foreach (int num in source.OrderBy(keySelector))
			{
				if (this.m_locators[num] != null && this.m_locators[num].transform.childCount == 0)
				{
					return num;
				}
			}
			return -1;
		}
		return -1;
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00014D64 File Offset: 0x00012F64
	public int GetClosestOpenLocatorToMouse()
	{
		float num = float.PositiveInfinity;
		int result = -1;
		Vector3 a = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		for (int i = 0; i < this.m_locators.Length; i++)
		{
			if (this.m_locators[i] != null && this.m_locators[i].transform.childCount == 0)
			{
				float num2 = Vector3.Distance(a, this.m_locators[i].transform.position);
				if (num2 < num)
				{
					result = i;
					num = num2;
				}
			}
		}
		return result;
	}

	// Token: 0x0400026D RID: 621
	[SerializeField]
	private GameObject[] m_locators;

	// Token: 0x0400026E RID: 622
	[SerializeField]
	private GameObject m_bootedObjects;

	// Token: 0x0400026F RID: 623
	[SerializeField]
	private Image m_dragTargetImage;
}
