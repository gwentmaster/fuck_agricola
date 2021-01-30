using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000FF RID: 255
public class UIP_FriendSlot : MonoBehaviour
{
	// Token: 0x06000989 RID: 2441 RVA: 0x00040119 File Offset: 0x0003E319
	public void SetClickListener(UIP_FriendSlot.ClickCallback cb)
	{
		this.m_Callback = cb;
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00040122 File Offset: 0x0003E322
	public string GetDisplayName()
	{
		return this.m_FriendData.displayName;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0004012F File Offset: 0x0003E32F
	public uint GetUserID()
	{
		return this.m_FriendData.userID;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0004013C File Offset: 0x0003E33C
	public FriendInfo GetFriendInfo()
	{
		return this.m_FriendData;
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00040144 File Offset: 0x0003E344
	public ProfileManager.OfflineProfileEntry GetOfflineProfile()
	{
		return this.m_OfflineProfile;
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0004014C File Offset: 0x0003E34C
	public void SetOverrideFactionIndex(int factionIndex)
	{
		this.m_overrideFactionIndex = factionIndex;
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00040158 File Offset: 0x0003E358
	public void SetFriendData(FriendInfo info)
	{
		this.m_FriendData = info;
		this.SetFriendName(info.displayName);
		this.SetRating(info.userRating);
		this.SetAvatar(info.userAvatar);
		this.SetUserID(info.userID);
		if (this.m_activePlayerImage != null)
		{
			this.m_activePlayerImage.sprite = this.m_activePlayerSprites[(info.userRating != 0) ? 0 : 1];
		}
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x000401C8 File Offset: 0x0003E3C8
	public void SetOfflineData(ProfileManager.OfflineProfileEntry profile, int index)
	{
		this.m_OfflineProfile = profile;
		this.SetFriendName(profile.name);
		this.SetRating(0);
		int num = (int)profile.factionIndex;
		if (this.m_overrideFactionIndex != -1)
		{
			num = this.m_overrideFactionIndex;
		}
		this.SetAvatar((ushort)((int)profile.gameAvatar1 + 10 * num));
		this.SetAvatar2((ushort)((int)profile.gameAvatar2 + 10 * num));
		this.SetFactionColor((uint)num);
		if (this.m_statusBase != null)
		{
			this.m_statusBase.SetActive(false);
		}
		if (this.m_statusUnknown != null)
		{
			this.m_statusUnknown.SetActive(false);
		}
		if (this.m_statusOnline != null)
		{
			this.m_statusOnline.SetActive(false);
		}
		if (this.m_statusOffline != null)
		{
			this.m_statusOffline.SetActive(false);
		}
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00040299 File Offset: 0x0003E499
	public void SetIsSelected(bool bActive)
	{
		if (this.m_selectedGlow != null)
		{
			this.m_selectedGlow.SetActive(bActive);
		}
		if (this.m_selectedGlowImage != null)
		{
			this.m_selectedGlowImage.color = this.m_selectedColor;
		}
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x000402D4 File Offset: 0x0003E4D4
	public void SetDeleteMode(bool bActive)
	{
		if (this.m_selectedGlow != null)
		{
			this.m_selectedGlow.SetActive(bActive);
		}
		if (this.m_selectedGlowImage != null)
		{
			this.m_selectedGlowImage.color = this.m_deleteColor;
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0004030F File Offset: 0x0003E50F
	public bool GetIsSelected()
	{
		return this.m_selectedGlow != null && this.m_selectedGlow.activeSelf;
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0004032C File Offset: 0x0003E52C
	public void SetFriendName(string displayName)
	{
		if (this.m_friendNameLabel != null)
		{
			this.m_friendNameLabel.text = displayName;
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00040348 File Offset: 0x0003E548
	public void SetRating(ushort rating)
	{
		if (this.m_ratingLabel != null)
		{
			if (rating > 0)
			{
				this.m_ratingLabel.text = rating.ToString();
				return;
			}
			this.m_ratingLabel.text = string.Empty;
		}
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0004037F File Offset: 0x0003E57F
	public void SetAvatar(ushort avatarIndex)
	{
		if (this.m_avatar != null)
		{
			this.m_avatar.SetAvatar((int)avatarIndex, false);
		}
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0004039C File Offset: 0x0003E59C
	public void SetAvatar2(ushort avatarIndex)
	{
		if (this.m_avatar2 != null)
		{
			this.m_avatar2.SetAvatar((int)avatarIndex, false);
		}
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00003022 File Offset: 0x00001222
	public void SetUserID(uint userID)
	{
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x000403B9 File Offset: 0x0003E5B9
	public void SetFactionColor(uint factionIndex)
	{
		if (this.m_colorizer != null)
		{
			this.m_colorizer.Colorize(factionIndex);
		}
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x000403D5 File Offset: 0x0003E5D5
	public PlayerOnlineStatus GetOnlineStatus()
	{
		return this.m_OnlineStatus;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x000403E0 File Offset: 0x0003E5E0
	public void SetOnlineStatus(PlayerOnlineStatus status)
	{
		this.m_OnlineStatus = status;
		if (this.m_statusBase != null)
		{
			this.m_statusBase.SetActive(true);
		}
		switch (status)
		{
		case PlayerOnlineStatus.STATUS_UNKNOWN:
			if (this.m_statusUnknown != null)
			{
				this.m_statusUnknown.SetActive(true);
			}
			if (this.m_statusOnline != null)
			{
				this.m_statusOnline.SetActive(false);
			}
			if (this.m_statusOffline != null)
			{
				this.m_statusOffline.SetActive(false);
				return;
			}
			break;
		case PlayerOnlineStatus.STATUS_OFFLINE:
			if (this.m_statusUnknown != null)
			{
				this.m_statusUnknown.SetActive(false);
			}
			if (this.m_statusOnline != null)
			{
				this.m_statusOnline.SetActive(false);
			}
			if (this.m_statusOffline != null)
			{
				this.m_statusOffline.SetActive(true);
			}
			break;
		case PlayerOnlineStatus.STATUS_AWAY:
		case PlayerOnlineStatus.STATUS_ONLINE:
			if (this.m_statusUnknown != null)
			{
				this.m_statusUnknown.SetActive(false);
			}
			if (this.m_statusOnline != null)
			{
				this.m_statusOnline.SetActive(true);
			}
			if (this.m_statusOffline != null)
			{
				this.m_statusOffline.SetActive(false);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00040514 File Offset: 0x0003E714
	public void OnSelect()
	{
		if (this.m_Callback != null)
		{
			this.m_Callback(this, UIP_FriendSlot.ClickEventType.Evt_Click_Slot);
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0000900B File Offset: 0x0000720B
	public bool IsVisible()
	{
		return true;
	}

	// Token: 0x04000A17 RID: 2583
	public TextMeshProUGUI m_friendNameLabel;

	// Token: 0x04000A18 RID: 2584
	public TextMeshProUGUI m_ratingLabel;

	// Token: 0x04000A19 RID: 2585
	public GameObject m_selectedGlow;

	// Token: 0x04000A1A RID: 2586
	public Image m_selectedGlowImage;

	// Token: 0x04000A1B RID: 2587
	public Color m_selectedColor;

	// Token: 0x04000A1C RID: 2588
	public Color m_deleteColor;

	// Token: 0x04000A1D RID: 2589
	public GameObject m_statusUnknown;

	// Token: 0x04000A1E RID: 2590
	public GameObject m_statusOnline;

	// Token: 0x04000A1F RID: 2591
	public GameObject m_statusOffline;

	// Token: 0x04000A20 RID: 2592
	public GameObject m_statusBase;

	// Token: 0x04000A21 RID: 2593
	public RectTransform m_RectTransform;

	// Token: 0x04000A22 RID: 2594
	public UI_DragSource m_dragSource;

	// Token: 0x04000A23 RID: 2595
	public Avatar_UI m_avatar;

	// Token: 0x04000A24 RID: 2596
	public Avatar_UI m_avatar2;

	// Token: 0x04000A25 RID: 2597
	public ColorByFaction m_colorizer;

	// Token: 0x04000A26 RID: 2598
	public Image m_activePlayerImage;

	// Token: 0x04000A27 RID: 2599
	public Sprite[] m_activePlayerSprites;

	// Token: 0x04000A28 RID: 2600
	private UIP_FriendSlot.ClickCallback m_Callback;

	// Token: 0x04000A29 RID: 2601
	private FriendInfo m_FriendData;

	// Token: 0x04000A2A RID: 2602
	private ProfileManager.OfflineProfileEntry m_OfflineProfile;

	// Token: 0x04000A2B RID: 2603
	private PlayerOnlineStatus m_OnlineStatus;

	// Token: 0x04000A2C RID: 2604
	private int m_overrideFactionIndex = -1;

	// Token: 0x020007CC RID: 1996
	public enum ClickEventType
	{
		// Token: 0x04002CFD RID: 11517
		Evt_Click_Slot
	}

	// Token: 0x020007CD RID: 1997
	// (Invoke) Token: 0x06004337 RID: 17207
	public delegate void ClickCallback(UIP_FriendSlot slot, UIP_FriendSlot.ClickEventType evt);
}
