using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class CardInPlay : MonoBehaviour
{
	// Token: 0x06000783 RID: 1923 RVA: 0x00036B57 File Offset: 0x00034D57
	public int GetCardInPlayInstanceID()
	{
		return this.m_CardInPlayInstanceID;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00036B5F File Offset: 0x00034D5F
	public void SetCardInPlayInstanceID(int instanceID)
	{
		this.m_CardInPlayInstanceID = instanceID;
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00036B68 File Offset: 0x00034D68
	public int GetSourceCardInstanceID()
	{
		return this.m_SourceCardInstanceID;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00036B70 File Offset: 0x00034D70
	public CardObject GetSourceCard()
	{
		return this.m_SourceCard;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00036B78 File Offset: 0x00034D78
	public virtual void SetSourceCard(CardObject sourceCard)
	{
		this.m_SourceCard = sourceCard;
		this.m_SourceCardInstanceID = ((sourceCard == null) ? 0 : sourceCard.GetCardInstanceID());
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00036B99 File Offset: 0x00034D99
	public void SetSourceCardInstanceID(int sourceCardInstanceID)
	{
		if (this.m_SourceCard == null)
		{
			this.m_SourceCardInstanceID = sourceCardInstanceID;
		}
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void UpdateSelectionState(bool bHighlight)
	{
	}

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private int m_CardInPlayInstanceID;

	// Token: 0x040008AF RID: 2223
	private int m_SourceCardInstanceID;

	// Token: 0x040008B0 RID: 2224
	protected CardObject m_SourceCard;
}
