using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
[Serializable]
public class UIP_CardGalleryNode : MonoBehaviour
{
	// Token: 0x0600097E RID: 2430 RVA: 0x0003FEB6 File Offset: 0x0003E0B6
	public void Awake()
	{
		if (this.m_animator == null)
		{
			this.m_animator = base.gameObject.GetComponent<Animator>();
		}
		this.m_index = 0;
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0003FEE0 File Offset: 0x0003E0E0
	public void SetCard(GameObject card, GameObject flipCard, int index, float scale)
	{
		this.ClearCard();
		this.m_flipCard = flipCard;
		if (card != null)
		{
			this.m_card = card;
			this.m_card.SetActive(true);
			this.m_card.transform.SetParent(base.transform, false);
			this.m_card.transform.localPosition = Vector3.zero;
			Vector3 localScale = new Vector3(scale, scale, 1f);
			this.m_card.transform.localScale = localScale;
			this.m_index = index;
		}
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0003FF6C File Offset: 0x0003E16C
	public void ClearCard()
	{
		if (this.m_flipCard != null)
		{
			this.m_flipCard = null;
		}
		if (this.m_card != null)
		{
			this.m_card.transform.SetParent(this.m_limbo.transform);
			this.m_card = null;
		}
		this.bFlipped = false;
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x0003FFC5 File Offset: 0x0003E1C5
	public void MoveLeft()
	{
		if (this.bFlipped)
		{
			this.bFlipped = false;
			this.m_flipCard.transform.SetAsFirstSibling();
		}
		this.m_animator.SetTrigger(UIP_CardGalleryNode.s_moveLeft);
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0003FFF6 File Offset: 0x0003E1F6
	public void MoveRight()
	{
		if (this.bFlipped)
		{
			this.bFlipped = false;
			this.m_flipCard.transform.SetAsFirstSibling();
		}
		this.m_animator.SetTrigger(UIP_CardGalleryNode.s_moveRight);
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00040028 File Offset: 0x0003E228
	public void FlipCard(bool bIsHorizontal)
	{
		this.m_animator.SetTrigger(bIsHorizontal ? UIP_CardGalleryNode.s_flipCardH : UIP_CardGalleryNode.s_flipCard);
		if (this.m_flipCard != null)
		{
			if (this.bFlipped)
			{
				this.m_flipCard.transform.SetAsFirstSibling();
			}
			else
			{
				this.m_flipCard.transform.SetAsLastSibling();
			}
		}
		this.bFlipped = !this.bFlipped;
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00040096 File Offset: 0x0003E296
	public void ForceMove(int index)
	{
		this.m_animator.SetInteger(UIP_CardGalleryNode.s_forceMoveValue, index);
		this.m_animator.SetTrigger(UIP_CardGalleryNode.s_forceMoveTrigger);
		this.bFlipped = false;
	}

	// Token: 0x04000A09 RID: 2569
	private static string s_moveLeft = "MoveLeft";

	// Token: 0x04000A0A RID: 2570
	private static string s_moveRight = "MoveRight";

	// Token: 0x04000A0B RID: 2571
	private static string s_forceMoveTrigger = "UseInitialState";

	// Token: 0x04000A0C RID: 2572
	private static string s_forceMoveValue = "InitialState";

	// Token: 0x04000A0D RID: 2573
	private static string s_flipCard = "FlipCard";

	// Token: 0x04000A0E RID: 2574
	private static string s_flipCardH = "FlipCardHoriz";

	// Token: 0x04000A0F RID: 2575
	public Animator m_animator;

	// Token: 0x04000A10 RID: 2576
	public GameObject m_limbo;

	// Token: 0x04000A11 RID: 2577
	[HideInInspector]
	public int m_index;

	// Token: 0x04000A12 RID: 2578
	private GameObject m_card;

	// Token: 0x04000A13 RID: 2579
	private GameObject m_flipCard;

	// Token: 0x04000A14 RID: 2580
	private bool bFlipped;
}
