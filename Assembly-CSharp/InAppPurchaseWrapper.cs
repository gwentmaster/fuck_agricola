using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class InAppPurchaseWrapper : MonoBehaviour
{
	// Token: 0x060007FF RID: 2047 RVA: 0x00038690 File Offset: 0x00036890
	private void SelectActiveManager()
	{
		if (this.m_ActiveManager != null)
		{
			this.m_ActiveManager.enabled = false;
		}
		this.m_ActiveManager = null;
		if (this.m_PlatformManagers != null)
		{
			for (int i = 0; i < this.m_PlatformManagers.Length; i++)
			{
				InAppPurchaseInterface inAppPurchaseInterface = this.m_PlatformManagers[i];
				if (inAppPurchaseInterface != null && inAppPurchaseInterface.gameObject.activeInHierarchy)
				{
					this.m_ActiveManager = inAppPurchaseInterface;
					break;
				}
			}
		}
		if (this.m_ActiveManager != null)
		{
			this.m_ActiveManager.enabled = true;
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0003871B File Offset: 0x0003691B
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.UpdateAvailableFlags();
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00038730 File Offset: 0x00036930
	private void Start()
	{
		this.SelectActiveManager();
		InAppPurchaseWrapper.m_UnlockedCardSetFlags = 65535U;
		InAppPurchaseWrapper.m_UnlockedPromoPackFlags = 65535U;
		InAppPurchaseWrapper.m_UnlockedAvatarFlags = 65535U;
		this.m_RewardUnlockCode = 0;
		string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		string text = deviceUniqueIdentifier + "_unlock";
		string key = "reward_" + ObfuscateIAPName.CreateHashString(text);
		if (PlayerPrefs.HasKey(key))
		{
			string b = ObfuscateIAPName.CreateHashString("unlock_" + deviceUniqueIdentifier);
			string @string = PlayerPrefs.GetString(key);
			int num = @string.IndexOf("::");
			if (num != -1 && @string.Remove(num) == b)
			{
				string value = @string.Remove(0, num + 2);
				try
				{
					this.m_RewardUnlockCode = Convert.ToInt32(value);
				}
				catch (FormatException)
				{
					this.m_RewardUnlockCode = 0;
				}
				catch (OverflowException)
				{
					this.m_RewardUnlockCode = 0;
				}
			}
		}
		this.UpdateRewardUnlockCode();
		this.UpdateAvailableFlags();
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0003882C File Offset: 0x00036A2C
	private void Update()
	{
		if (this.UpdateOwnership())
		{
			this.UpdateRewardUnlockCode();
			this.UpdateAvailableFlags();
		}
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x00038843 File Offset: 0x00036A43
	public bool UpdateOwnership()
	{
		return this.m_ActiveManager != null && this.m_ActiveManager.UpdateOwnership();
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00038860 File Offset: 0x00036A60
	public EUnlockedContent IsUnlockedContent(string contentID)
	{
		if (InAppPurchaseWrapper.m_InAppPurchaseItems != null)
		{
			for (int i = 0; i < InAppPurchaseWrapper.m_InAppPurchaseItems.Length; i++)
			{
				if (!(InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetInAppPurchaseID() != contentID))
				{
					switch (InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlagType())
					{
					case EUnlockFlagType.CARD_SET:
						if ((this.m_RewardCardSetFlags & InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlags()) != 0U)
						{
							return EUnlockedContent.AVAILABLE_REWARD;
						}
						break;
					case EUnlockFlagType.ADDITIONAL_CARDS:
						if ((this.m_RewardPromoPackFlags & InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlags()) != 0U)
						{
							return EUnlockedContent.AVAILABLE_REWARD;
						}
						break;
					case EUnlockFlagType.AVATAR:
						if ((this.m_RewardAvatarFlags & InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlags()) != 0U)
						{
							return EUnlockedContent.AVAILABLE_REWARD;
						}
						break;
					}
				}
			}
		}
		if (this.m_ActiveManager != null)
		{
			return this.m_ActiveManager.IsUnlockedContent(contentID);
		}
		return EUnlockedContent.NOT_AVAILABLE;
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00038924 File Offset: 0x00036B24
	public bool UpdateAvailableFlags()
	{
		this.m_AvailableCardSetFlags = (InAppPurchaseWrapper.m_UnlockedCardSetFlags | this.m_RewardCardSetFlags);
		this.m_AvailablePromoPackFlags = (InAppPurchaseWrapper.m_UnlockedPromoPackFlags | this.m_RewardPromoPackFlags);
		this.m_AvailableAvatarFlags = (InAppPurchaseWrapper.m_UnlockedAvatarFlags | this.m_RewardAvatarFlags);
		if (InAppPurchaseWrapper.m_InAppPurchaseItems != null)
		{
			for (int i = 0; i < InAppPurchaseWrapper.m_InAppPurchaseItems.Length; i++)
			{
				if (InAppPurchaseWrapper.m_InAppPurchaseItems[i].UpdateOwnership(this) != EUnlockedContent.NOT_AVAILABLE)
				{
					switch (InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlagType())
					{
					case EUnlockFlagType.CARD_SET:
						this.m_AvailableCardSetFlags |= InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlags();
						break;
					case EUnlockFlagType.ADDITIONAL_CARDS:
						this.m_AvailablePromoPackFlags |= InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlags();
						break;
					case EUnlockFlagType.AVATAR:
						this.m_AvailableAvatarFlags |= InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetUnlockFlags();
						break;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00038A09 File Offset: 0x00036C09
	public uint GetAvailableCardSetFlags()
	{
		return this.m_AvailableCardSetFlags;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00038A11 File Offset: 0x00036C11
	public uint GetAvailablePromoPackFlags()
	{
		return this.m_AvailablePromoPackFlags;
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00038A1C File Offset: 0x00036C1C
	public void SetRewardUnlockCode(int unlock)
	{
		if (this.m_RewardUnlockCode == unlock)
		{
			return;
		}
		this.m_RewardUnlockCode = unlock;
		string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		string text = deviceUniqueIdentifier + "_unlock";
		string key = "reward_" + ObfuscateIAPName.CreateHashString(text);
		string text2 = ObfuscateIAPName.CreateHashString("unlock_" + deviceUniqueIdentifier);
		text2 += "::";
		text2 += unlock.ToString();
		PlayerPrefs.SetString(key, text2);
		PlayerPrefs.Save();
		this.UpdateRewardUnlockCode();
		this.UpdateAvailableFlags();
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00038A9F File Offset: 0x00036C9F
	private void UpdateRewardUnlockCode()
	{
		if (this.m_RewardUnlockCode != 1 && this.m_RewardUnlockCode != 2)
		{
			this.m_RewardCardSetFlags = 0U;
			this.m_RewardPromoPackFlags = 0U;
			this.m_RewardAvatarFlags = 0U;
		}
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00038AC8 File Offset: 0x00036CC8
	public void PurchaseItem(string purchase_name)
	{
		if (this.m_ActiveManager != null)
		{
			this.m_ActiveManager.PurchaseItem(purchase_name);
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00038AE4 File Offset: 0x00036CE4
	public void RestorePurchases()
	{
		if (this.m_ActiveManager != null)
		{
			this.m_ActiveManager.RestorePurchases();
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00038AFF File Offset: 0x00036CFF
	public bool IsUnlockedPackage(string iap_package_id)
	{
		if (iap_package_id == "reward")
		{
			return this.m_RewardUnlockCode != 0;
		}
		return this.m_ActiveManager != null && this.m_ActiveManager.IsUnlockedPackage(iap_package_id);
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00038B34 File Offset: 0x00036D34
	public string GetPackageLocalizedName(string iap_package_id)
	{
		if (iap_package_id == "reward")
		{
			return "Kickstarter Rewards";
		}
		if (this.m_ActiveManager != null)
		{
			return this.m_ActiveManager.GetPackageLocalizedName(iap_package_id);
		}
		return "";
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00038B69 File Offset: 0x00036D69
	public string GetPackageLocalizedPrice(string iap_package_id)
	{
		if (this.m_ActiveManager != null)
		{
			return this.m_ActiveManager.GetPackageLocalizedPrice(iap_package_id);
		}
		return "$0.99";
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x00038B8B File Offset: 0x00036D8B
	public string GetPackageLocalizedDescription(string iap_package_id)
	{
		if (iap_package_id == "reward")
		{
			return "";
		}
		if (this.m_ActiveManager != null)
		{
			return this.m_ActiveManager.GetPackageLocalizedDescription(iap_package_id);
		}
		return "";
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00038BC0 File Offset: 0x00036DC0
	public virtual string[] GetPackageContentList(string iap_package_id)
	{
		if (this.m_ActiveManager != null)
		{
			return this.m_ActiveManager.GetPackageContentList(iap_package_id);
		}
		return null;
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00038BE0 File Offset: 0x00036DE0
	public string GetContentLocalizedName(string contentID)
	{
		if (InAppPurchaseWrapper.m_InAppPurchaseItems != null)
		{
			for (int i = 0; i < InAppPurchaseWrapper.m_InAppPurchaseItems.Length; i++)
			{
				if (InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetInAppPurchaseID() == contentID)
				{
					return InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetInAppPurchaseName();
				}
			}
		}
		return "";
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00038C2C File Offset: 0x00036E2C
	public int GetContentIconIndex(string contentID)
	{
		if (InAppPurchaseWrapper.m_InAppPurchaseItems != null)
		{
			for (int i = 0; i < InAppPurchaseWrapper.m_InAppPurchaseItems.Length; i++)
			{
				if (InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetInAppPurchaseID() == contentID)
				{
					return InAppPurchaseWrapper.m_InAppPurchaseItems[i].GetIconIndex();
				}
			}
		}
		return 0;
	}

	// Token: 0x040008F8 RID: 2296
	private static InAppPurchaseItem[] m_InAppPurchaseItems = new InAppPurchaseItem[]
	{
		new InAppPurchaseItem("Default", "agricola.test", 1, EUnlockFlagType.CARD_SET, 1U)
	};

	// Token: 0x040008F9 RID: 2297
	[SerializeField]
	private InAppPurchaseInterface[] m_PlatformManagers;

	// Token: 0x040008FA RID: 2298
	private InAppPurchaseInterface m_ActiveManager;

	// Token: 0x040008FB RID: 2299
	private uint m_AvailableCardSetFlags;

	// Token: 0x040008FC RID: 2300
	private uint m_AvailablePromoPackFlags;

	// Token: 0x040008FD RID: 2301
	private uint m_AvailableAvatarFlags;

	// Token: 0x040008FE RID: 2302
	private int m_RewardUnlockCode;

	// Token: 0x040008FF RID: 2303
	private uint m_RewardCardSetFlags;

	// Token: 0x04000900 RID: 2304
	private uint m_RewardPromoPackFlags;

	// Token: 0x04000901 RID: 2305
	private uint m_RewardAvatarFlags;

	// Token: 0x04000902 RID: 2306
	private static uint m_UnlockedCardSetFlags = 0U;

	// Token: 0x04000903 RID: 2307
	private static uint m_UnlockedPromoPackFlags = 0U;

	// Token: 0x04000904 RID: 2308
	private static uint m_UnlockedAvatarFlags = 0U;
}
