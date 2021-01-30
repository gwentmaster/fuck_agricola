using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class AgricolaOverview_LogInAction : MonoBehaviour
{
	// Token: 0x060004FF RID: 1279 RVA: 0x00026FF7 File Offset: 0x000251F7
	public int GetPlayerIndex()
	{
		return this.m_playerIndex;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00026FFF File Offset: 0x000251FF
	public int GetRoundNumber()
	{
		return this.m_roundNumber;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00027007 File Offset: 0x00025207
	public int GetLineIndex()
	{
		return this.m_lineIndex;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0002700F File Offset: 0x0002520F
	public int GetCardInstanceID()
	{
		return this.m_cardInstanceID;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00027017 File Offset: 0x00025217
	public string GetCardName()
	{
		return this.m_cardName;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0002701F File Offset: 0x0002521F
	public void SetupCell(int playerIndex, int roundNumber, int factionIndex, int avatarIndex, int lineIndex)
	{
		this.m_playerIndex = playerIndex;
		this.m_roundNumber = roundNumber;
		this.m_lineIndex = lineIndex;
		this.m_colorizer.Colorize((uint)factionIndex);
		this.m_playerAvatar.SetAvatar(avatarIndex, true);
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00027051 File Offset: 0x00025251
	public void SetTokenVisible(bool bVisible)
	{
		this.m_playerToken.SetActive(bVisible);
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00027060 File Offset: 0x00025260
	public void SetActionCard(GameObject cardObj, int cardInstanceID, string cardName)
	{
		this.m_cardInstanceID = cardInstanceID;
		this.m_cardName = cardName;
		cardObj.SetActive(true);
		AnimateObject component = cardObj.GetComponent<AnimateObject>();
		if (component != null)
		{
			this.m_cardLocator.PlaceAnimateObject(component, true, true, false);
		}
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x000270A4 File Offset: 0x000252A4
	public void AddResCard(int instanceID, AgricolaCardManager cardManager)
	{
		for (int i = 0; i < this.m_attachedCards.Count; i++)
		{
			if (this.m_attachedCards[i] != null && instanceID == this.m_attachedCards[i].GetCardInstanceID())
			{
				return;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_cellResPrefabs[10]);
		if (gameObject != null)
		{
			gameObject.transform.SetParent(this.m_resourceLocator.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			GameObject gameObject2 = cardManager.CreateTemporaryCardFromInstanceID(instanceID);
			gameObject.GetComponent<AgricolaOverview_CalendarResource>().SetData(0, 0, 0, 0, gameObject2);
			this.m_attachedCards.Add(gameObject2.GetComponent<AgricolaCard>());
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00027168 File Offset: 0x00025368
	public void AddResources(short[] resources)
	{
		for (int i = 0; i < 10; i++)
		{
			if (resources[i] != 0)
			{
				if (this.m_createdResources[i] == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_cellResPrefabs[i]);
					if (gameObject != null)
					{
						gameObject.transform.SetParent(this.m_resourceLocator.transform);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localScale = Vector3.one;
						this.m_createdResources[i] = gameObject.GetComponent<AgricolaOverview_CalendarResource>();
					}
				}
				this.m_createdResources[i].AddResources((int)resources[i]);
			}
		}
	}

	// Token: 0x04000488 RID: 1160
	public GameObject m_playerToken;

	// Token: 0x04000489 RID: 1161
	public Avatar_UI m_playerAvatar;

	// Token: 0x0400048A RID: 1162
	public ColorByFaction m_colorizer;

	// Token: 0x0400048B RID: 1163
	public AgricolaAnimationLocator m_cardLocator;

	// Token: 0x0400048C RID: 1164
	public GameObject m_resourceLocator;

	// Token: 0x0400048D RID: 1165
	public GameObject[] m_cellResPrefabs;

	// Token: 0x0400048E RID: 1166
	private AgricolaOverview_CalendarResource[] m_createdResources = new AgricolaOverview_CalendarResource[10];

	// Token: 0x0400048F RID: 1167
	private List<AgricolaCard> m_attachedCards = new List<AgricolaCard>();

	// Token: 0x04000490 RID: 1168
	private int m_playerIndex = -1;

	// Token: 0x04000491 RID: 1169
	private int m_roundNumber = -1;

	// Token: 0x04000492 RID: 1170
	private int m_lineIndex = -1;

	// Token: 0x04000493 RID: 1171
	private int m_cardInstanceID;

	// Token: 0x04000494 RID: 1172
	private string m_cardName = string.Empty;
}
