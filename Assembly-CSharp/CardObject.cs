using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CF RID: 207
[RequireComponent(typeof(DragObject))]
public class CardObject : MonoBehaviour
{
	// Token: 0x06000796 RID: 1942 RVA: 0x00036E92 File Offset: 0x00035092
	public virtual void SetupCallbacks(CardManager card_manager)
	{
		this.m_CardManager = card_manager;
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0000900B File Offset: 0x0000720B
	public virtual bool UpdateCard()
	{
		return true;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00036E9B File Offset: 0x0003509B
	public int GetCardInstanceID()
	{
		return this.m_CardInstanceID;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00036EA4 File Offset: 0x000350A4
	public void SetCardInstanceID(int instanceID)
	{
		this.m_CardInstanceID = instanceID;
		DragObject component = base.GetComponent<DragObject>();
		if (component != null)
		{
			component.SetDragSelectionID((ushort)instanceID);
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00036ED0 File Offset: 0x000350D0
	public bool IsSelectable()
	{
		return this.m_bSelectable;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00036ED8 File Offset: 0x000350D8
	public virtual void SetSelectable(bool bSelectable, Color highlightColor)
	{
		this.m_bSelectable = bSelectable;
		if (this.m_CardHighlight != null)
		{
			if (this.m_bSelectable)
			{
				Image component = this.m_CardHighlight.GetComponent<Image>();
				if (component != null)
				{
					component.color = highlightColor;
				}
				this.m_CardHighlight.SetActive(this.m_bSelectable);
				return;
			}
			this.m_CardHighlight.SetActive(false);
		}
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00036F3C File Offset: 0x0003513C
	public void UpdateSelectionState(bool bHighlight, bool bExcludeIfAnimating = false)
	{
		if (bExcludeIfAnimating)
		{
			AnimateObject component = base.GetComponent<AnimateObject>();
			if (component != null && component.IsAnimating())
			{
				return;
			}
		}
		if (this.m_CardInstanceID == 0 || !bHighlight)
		{
			this.SetSelectable(false, Color.white);
			return;
		}
		foreach (ushort selectionHint in GameOptions.GetSelectionHints((ushort)this.m_CardInstanceID))
		{
			DragSelectionHintDefinition dragSelectionHintDefinition = InterfaceSelectionHints.FindSelectionHintDefinition((int)selectionHint);
			if (dragSelectionHintDefinition != null)
			{
				this.SetSelectable(true, dragSelectionHintDefinition.m_HighlightColor);
				return;
			}
		}
		this.SetSelectable(false, Color.white);
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00036FE8 File Offset: 0x000351E8
	public bool IsMagnified()
	{
		return this.m_bMagnified;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00036FF0 File Offset: 0x000351F0
	public bool IsMagnifying()
	{
		return this.m_bMagnifying;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00036FF8 File Offset: 0x000351F8
	protected void SetIsMagnified(bool bMagnified)
	{
		this.m_bMagnified = bMagnified;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00037001 File Offset: 0x00035201
	protected void SetIsMagnifying(bool bMagnifying)
	{
		this.m_bMagnifying = bMagnifying;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0002A062 File Offset: 0x00028262
	public virtual bool CanMagnify(bool bAllowShowingCardBack)
	{
		return false;
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0003700A File Offset: 0x0003520A
	public void FinishMagnify()
	{
		this.m_bMagnifying = false;
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x00037014 File Offset: 0x00035214
	public virtual bool Magnify(bool bAllowShowingCardBack, bool bTutorialMagnify)
	{
		if (!this.CanMagnify(bAllowShowingCardBack))
		{
			return false;
		}
		DragObject component = base.GetComponent<DragObject>();
		if (component == null)
		{
			return false;
		}
		this.m_bMagnified = true;
		Transform parent = base.transform.parent;
		int siblingIndex = base.transform.GetSiblingIndex();
		Vector3 position = base.transform.position;
		Vector3 localScale = base.transform.localScale;
		if (this.m_CardManager != null)
		{
			MagnifyManager magnifyManager = this.m_CardManager.GetMagnifyManager();
			if (magnifyManager != null)
			{
				this.m_bMagnifying = true;
				this.m_bMagnifying = magnifyManager.StartMagnifyCard(this);
			}
		}
		GameObject returnPlaceholder = component.GetReturnPlaceholder(true);
		returnPlaceholder.transform.SetParent(parent);
		returnPlaceholder.transform.SetSiblingIndex(siblingIndex);
		returnPlaceholder.transform.position = position;
		returnPlaceholder.transform.localScale = localScale;
		return true;
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0003700A File Offset: 0x0003520A
	public virtual void FinishUnmagnify()
	{
		this.m_bMagnifying = false;
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x000370E8 File Offset: 0x000352E8
	public virtual bool Unmagnify(bool bTutorialUnmagnify, bool bAllowCancelOption = true)
	{
		this.m_bMagnifying = true;
		this.m_bMagnified = false;
		if (this.m_CardManager != null)
		{
			MagnifyManager magnifyManager = this.m_CardManager.GetMagnifyManager();
			if (magnifyManager != null)
			{
				magnifyManager.RemoveMagnifiedCard(this);
			}
		}
		return true;
	}

	// Token: 0x040008B6 RID: 2230
	public GameObject m_CardHighlight;

	// Token: 0x040008B7 RID: 2231
	[SerializeField]
	private int m_CardInstanceID;

	// Token: 0x040008B8 RID: 2232
	private bool m_bSelectable;

	// Token: 0x040008B9 RID: 2233
	private bool m_bMagnified;

	// Token: 0x040008BA RID: 2234
	private bool m_bMagnifying;

	// Token: 0x040008BB RID: 2235
	protected CardManager m_CardManager;
}
