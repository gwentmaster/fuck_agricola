using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000102 RID: 258
public class UIP_Profile : MonoBehaviour
{
	// Token: 0x060009CA RID: 2506 RVA: 0x0004260E File Offset: 0x0004080E
	public void SetClickListener(UIP_Profile.ClickCallback cb)
	{
		this.m_Callback = cb;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00042618 File Offset: 0x00040818
	public void Awake()
	{
		GameObject[] baseNodes = this.m_baseNodes;
		for (int i = 0; i < baseNodes.Length; i++)
		{
			baseNodes[i].SetActive(false);
		}
		this.m_onlineIndicator.gameObject.SetActive(false);
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00042654 File Offset: 0x00040854
	public string GetDisplayName()
	{
		return this.m_FriendData.displayName;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x00042661 File Offset: 0x00040861
	public uint GetUserID()
	{
		return this.m_FriendData.userID;
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0004266E File Offset: 0x0004086E
	public FriendInfo GetFriendInfo()
	{
		return this.m_FriendData;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00042676 File Offset: 0x00040876
	public ProfileManager.OfflineProfileEntry GetOfflineProfile()
	{
		return this.m_OfflineProfile;
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0002A073 File Offset: 0x00028273
	public void HideObject()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0002A065 File Offset: 0x00028265
	public void ShowObject()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00042680 File Offset: 0x00040880
	public void SetFriendData(FriendInfo info)
	{
		GameObject[] baseNodes = this.m_baseNodes;
		for (int i = 0; i < baseNodes.Length; i++)
		{
			baseNodes[i].SetActive(true);
		}
		this.m_FriendData = info;
		if (this.m_ratingBase != null)
		{
			this.m_ratingBase.SetActive(false);
		}
		this.SetFriendName(info.displayName);
		this.SetRating(info.userRating);
		this.SetAvatar(info.userAvatar);
		this.SetUserID(info.userID);
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x000426FC File Offset: 0x000408FC
	public void SetOfflineData(ProfileManager.OfflineProfileEntry profile, int index)
	{
		GameObject[] baseNodes = this.m_baseNodes;
		for (int i = 0; i < baseNodes.Length; i++)
		{
			baseNodes[i].SetActive(true);
		}
		this.m_OfflineProfile = profile;
		this.SetFriendName(profile.name);
		this.SetRating(0);
		this.SetAvatar((ushort)profile.gameAvatar1);
		if (this.m_statusBase != null)
		{
			this.m_statusBase.SetActive(false);
		}
		if (this.m_ratingBase != null)
		{
			this.m_ratingBase.SetActive(false);
		}
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00003022 File Offset: 0x00001222
	public void SetDraggable(bool bDraggable, Canvas forceBaseCanvas)
	{
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00003022 File Offset: 0x00001222
	public void SetInteractable(bool bInteractable)
	{
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00042781 File Offset: 0x00040981
	public void SetIsSelected(bool bActive)
	{
		if (this.m_activeGlow != null)
		{
			this.m_activeGlow.SetActive(bActive);
		}
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0004279D File Offset: 0x0004099D
	public bool GetIsSelected()
	{
		return this.m_activeGlow.activeSelf;
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000427AA File Offset: 0x000409AA
	public void SetFriendName(string displayName)
	{
		this.m_friendNameLabel.text = displayName;
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x000427B8 File Offset: 0x000409B8
	public void SetRating(ushort rating)
	{
		if (this.m_ratingLabel != null)
		{
			this.m_ratingLabel.text = ((rating > 0) ? rating.ToString() : string.Empty);
		}
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x000427E5 File Offset: 0x000409E5
	public void SetAvatar(ushort avatarIndex)
	{
		if (this.m_avatar != null)
		{
			this.m_avatar.SetAvatar((int)avatarIndex, true);
		}
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x00003022 File Offset: 0x00001222
	public void SetUserID(uint userID)
	{
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x00042802 File Offset: 0x00040A02
	public PlayerOnlineStatus GetOnlineStatus()
	{
		return this.m_OnlineStatus;
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0004280C File Offset: 0x00040A0C
	public void SetOnlineStatus(PlayerOnlineStatus status)
	{
		this.m_OnlineStatus = status;
		this.m_onlineIndicator.gameObject.SetActive(true);
		this.m_onlineIndicator.color = this.m_indicatorColors[(int)status];
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0004284A File Offset: 0x00040A4A
	public void OnSelect()
	{
		if (this.m_Callback != null)
		{
			this.m_Callback(this, UIP_Profile.ClickEventType.Evt_Click_Slot);
		}
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00042864 File Offset: 0x00040A64
	public bool IsVisible()
	{
		Renderer component = base.GetComponent<Renderer>();
		return !(component != null) || component.isVisible;
	}

	// Token: 0x04000A63 RID: 2659
	public TextMeshProUGUI m_friendNameLabel;

	// Token: 0x04000A64 RID: 2660
	public TextMeshProUGUI m_ratingLabel;

	// Token: 0x04000A65 RID: 2661
	public GameObject m_ratingBase;

	// Token: 0x04000A66 RID: 2662
	public GameObject m_activeGlow;

	// Token: 0x04000A67 RID: 2663
	public GameObject m_statusBase;

	// Token: 0x04000A68 RID: 2664
	public RectTransform m_RectTransform;

	// Token: 0x04000A69 RID: 2665
	public UI_DragSource m_dragSource;

	// Token: 0x04000A6A RID: 2666
	public Avatar_UI m_avatar;

	// Token: 0x04000A6B RID: 2667
	public Button m_button;

	// Token: 0x04000A6C RID: 2668
	public Image m_onlineIndicator;

	// Token: 0x04000A6D RID: 2669
	public Color[] m_indicatorColors;

	// Token: 0x04000A6E RID: 2670
	public GameObject[] m_baseNodes;

	// Token: 0x04000A6F RID: 2671
	private UIP_Profile.ClickCallback m_Callback;

	// Token: 0x04000A70 RID: 2672
	private FriendInfo m_FriendData;

	// Token: 0x04000A71 RID: 2673
	private ProfileManager.OfflineProfileEntry m_OfflineProfile;

	// Token: 0x04000A72 RID: 2674
	private PlayerOnlineStatus m_OnlineStatus;

	// Token: 0x020007D7 RID: 2007
	public enum ClickEventType
	{
		// Token: 0x04002D33 RID: 11571
		Evt_Click_Slot
	}

	// Token: 0x020007D8 RID: 2008
	// (Invoke) Token: 0x06004345 RID: 17221
	public delegate void ClickCallback(UIP_Profile slot, UIP_Profile.ClickEventType evt);
}
