using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class CardManager : MonoBehaviour
{
	// Token: 0x0600078B RID: 1931 RVA: 0x00036BB0 File Offset: 0x00034DB0
	public MagnifyManager GetMagnifyManager()
	{
		return this.m_MagnifyManager;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00036BB8 File Offset: 0x00034DB8
	protected void FinishCreateCard(GameObject cardObject, int cardInstanceID)
	{
		if (this.m_AnimationManager != null)
		{
			AnimateObject component = cardObject.GetComponent<AnimateObject>();
			if (component != null)
			{
				component.SetAnimationManager(this.m_AnimationManager);
			}
		}
		if (this.m_DragManager != null)
		{
			DragObject component2 = cardObject.GetComponent<DragObject>();
			if (component2 != null)
			{
				component2.SetDragManager(this.m_DragManager);
			}
		}
		CardObject component3 = cardObject.GetComponent<CardObject>();
		if (component3 != null)
		{
			component3.SetCardInstanceID(cardInstanceID);
			component3.SetupCallbacks(this);
		}
		cardObject.SetActive(false);
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0000301F File Offset: 0x0000121F
	public virtual GameObject CreateCard(int cardInstanceID, string cardName)
	{
		return null;
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00036C40 File Offset: 0x00034E40
	public virtual GameObject CreateCardFromInstanceID(int instanceID, bool bAddToMasterList = true)
	{
		GameObject gameObject = this.CreateCard(instanceID, "Card: " + instanceID.ToString());
		if (gameObject != null && bAddToMasterList)
		{
			this.m_MasterCardList.Add(instanceID, gameObject);
		}
		return gameObject;
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00036C88 File Offset: 0x00034E88
	public GameObject GetCardFromInstanceID(int instanceID, bool bCreateIfNecessary = false)
	{
		GameObject gameObject = (GameObject)this.m_MasterCardList[instanceID];
		if (gameObject == null && bCreateIfNecessary)
		{
			gameObject = this.CreateCardFromInstanceID(instanceID, true);
		}
		return gameObject;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00036CC4 File Offset: 0x00034EC4
	public GameObject CreateTemporaryCardFromInstanceID(int instanceID)
	{
		GameObject gameObject = this.CreateCardFromInstanceID(instanceID, false);
		if (gameObject != null)
		{
			GameObject gameObject2 = gameObject;
			gameObject2.name += " (temp)";
		}
		return gameObject;
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00036CFC File Offset: 0x00034EFC
	public void DestroyCard(CardObject destroyCard)
	{
		if (destroyCard == null)
		{
			return;
		}
		int cardInstanceID = destroyCard.GetCardInstanceID();
		GameObject gameObject = (GameObject)this.m_MasterCardList[cardInstanceID];
		if (gameObject == null || destroyCard.gameObject == gameObject)
		{
			this.m_MasterCardList.Remove(cardInstanceID);
		}
		this.m_UpdateCardList.Remove(destroyCard);
		if (destroyCard.gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(destroyCard.gameObject);
		}
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00036D80 File Offset: 0x00034F80
	public void UpdateSelectionState(bool bHighlight, bool bExcludeIfAnimating = false)
	{
		foreach (object obj in this.m_MasterCardList.Values)
		{
			CardObject component = ((GameObject)obj).GetComponent<CardObject>();
			if (component != null)
			{
				component.UpdateSelectionState(bHighlight, bExcludeIfAnimating);
			}
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00036DF0 File Offset: 0x00034FF0
	public void AddUpdateCardList(CardObject card)
	{
		if (this.m_UpdateCardList != null && !this.m_UpdateCardList.Contains(card))
		{
			this.m_UpdateCardList.Add(card);
		}
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00036E14 File Offset: 0x00035014
	private void Update()
	{
		if (this.m_UpdateCardList != null)
		{
			for (int i = this.m_UpdateCardList.Count - 1; i >= 0; i--)
			{
				CardObject cardObject = this.m_UpdateCardList[i];
				if (cardObject == null || (cardObject != null && cardObject.UpdateCard()))
				{
					this.m_UpdateCardList.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	protected AnimationManager m_AnimationManager;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	protected DragManager m_DragManager;

	// Token: 0x040008B3 RID: 2227
	[SerializeField]
	protected MagnifyManager m_MagnifyManager;

	// Token: 0x040008B4 RID: 2228
	protected Hashtable m_MasterCardList = new Hashtable();

	// Token: 0x040008B5 RID: 2229
	private List<CardObject> m_UpdateCardList = new List<CardObject>();
}
