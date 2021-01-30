using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class InAppPurchaseInterface : MonoBehaviour
{
	// Token: 0x060007EE RID: 2030 RVA: 0x0002A062 File Offset: 0x00028262
	public virtual bool UpdateOwnership()
	{
		return false;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0002A062 File Offset: 0x00028262
	public virtual EUnlockedContent IsUnlockedContent(string contentID)
	{
		return EUnlockedContent.NOT_AVAILABLE;
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void PurchaseItem(string purchase_name)
	{
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void RestorePurchases()
	{
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0002A062 File Offset: 0x00028262
	public virtual bool IsUnlockedPackage(string iap_package_id)
	{
		return false;
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x0003860A File Offset: 0x0003680A
	public virtual string GetPackageLocalizedName(string iap_package_id)
	{
		return "";
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0003860A File Offset: 0x0003680A
	public virtual string GetPackageLocalizedDescription(string iap_package_id)
	{
		return "";
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00038611 File Offset: 0x00036811
	public virtual string GetPackageLocalizedPrice(string iap_package_id)
	{
		return "$0.99";
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0000301F File Offset: 0x0000121F
	public virtual string[] GetPackageContentList(string iap_package_id)
	{
		return null;
	}
}
