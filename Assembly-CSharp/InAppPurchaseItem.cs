using System;

// Token: 0x020000DA RID: 218
public class InAppPurchaseItem
{
	// Token: 0x060007F8 RID: 2040 RVA: 0x00038618 File Offset: 0x00036818
	public InAppPurchaseItem(string name, string id, int icon_index, EUnlockFlagType unlockFlagType, uint unlockFlags)
	{
		this.m_InAppPurchaseName = name;
		this.m_InAppPurchaseID = id;
		this.m_IconIndex = icon_index;
		this.m_UnlockFlagType = unlockFlagType;
		this.m_UnlockFlags = unlockFlags;
		this.m_bOwned = EUnlockedContent.NOT_AVAILABLE;
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0003864C File Offset: 0x0003684C
	public EUnlockedContent UpdateOwnership(InAppPurchaseWrapper in_app_manager)
	{
		this.m_bOwned = in_app_manager.IsUnlockedContent(this.m_InAppPurchaseID);
		return this.m_bOwned;
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00038666 File Offset: 0x00036866
	public string GetInAppPurchaseName()
	{
		return this.m_InAppPurchaseName;
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0003866E File Offset: 0x0003686E
	public string GetInAppPurchaseID()
	{
		return this.m_InAppPurchaseID;
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00038676 File Offset: 0x00036876
	public int GetIconIndex()
	{
		return this.m_IconIndex;
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0003867E File Offset: 0x0003687E
	public EUnlockFlagType GetUnlockFlagType()
	{
		return this.m_UnlockFlagType;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00038686 File Offset: 0x00036886
	public uint GetUnlockFlags()
	{
		return this.m_UnlockFlags;
	}

	// Token: 0x040008F2 RID: 2290
	private string m_InAppPurchaseName;

	// Token: 0x040008F3 RID: 2291
	private string m_InAppPurchaseID;

	// Token: 0x040008F4 RID: 2292
	private int m_IconIndex;

	// Token: 0x040008F5 RID: 2293
	private EUnlockFlagType m_UnlockFlagType;

	// Token: 0x040008F6 RID: 2294
	private uint m_UnlockFlags;

	// Token: 0x040008F7 RID: 2295
	private EUnlockedContent m_bOwned;
}
