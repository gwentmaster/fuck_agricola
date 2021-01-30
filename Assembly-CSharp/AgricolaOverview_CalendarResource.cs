using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000059 RID: 89
public class AgricolaOverview_CalendarResource : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x060004FA RID: 1274 RVA: 0x00026EF8 File Offset: 0x000250F8
	public void SetData(byte costType, byte costAmount, byte gainedType, byte gainedAmount, GameObject cardObj)
	{
		this.m_resCount = (int)gainedAmount;
		if (this.m_countText != null)
		{
			this.m_countText.text = gainedAmount.ToString();
		}
		if (cardObj != null)
		{
			cardObj.SetActive(true);
			this.m_savedCard = cardObj.GetComponent<AgricolaCard>();
			AnimateObject component = cardObj.GetComponent<AnimateObject>();
			if (component != null)
			{
				this.m_cardLocator.PlaceAnimateObject(component, true, true, false);
			}
		}
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00026F6C File Offset: 0x0002516C
	public void AddResources(int appendRes)
	{
		this.m_resCount += appendRes;
		if (this.m_countText != null)
		{
			this.m_countText.text = this.m_resCount.ToString();
		}
		base.gameObject.SetActive(this.m_resCount != 0);
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00026FBF File Offset: 0x000251BF
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.m_savedCard != null)
		{
			this.m_savedCard.OnPointerEnter(eventData);
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00026FDB File Offset: 0x000251DB
	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.m_savedCard != null)
		{
			this.m_savedCard.OnPointerExit(eventData);
		}
	}

	// Token: 0x04000483 RID: 1155
	public AgricolaOverview_CalendarResource.ResCell[] m_resourceCells;

	// Token: 0x04000484 RID: 1156
	public TextMeshProUGUI m_countText;

	// Token: 0x04000485 RID: 1157
	public AgricolaAnimationLocator m_cardLocator;

	// Token: 0x04000486 RID: 1158
	private AgricolaCard m_savedCard;

	// Token: 0x04000487 RID: 1159
	private int m_resCount;

	// Token: 0x02000777 RID: 1911
	[Serializable]
	public struct ResCell
	{
		// Token: 0x04002BD2 RID: 11218
		public GameObject root;

		// Token: 0x04002BD3 RID: 11219
		public TextMeshProUGUI countText;

		// Token: 0x04002BD4 RID: 11220
		public AgricolaAnimationLocator cardLocator;
	}
}
