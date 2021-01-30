using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000128 RID: 296
public class UI_ProfileList : MonoBehaviour
{
	// Token: 0x06000B54 RID: 2900 RVA: 0x0004EB70 File Offset: 0x0004CD70
	public void OnEnterMenu()
	{
		if (this.m_bIgnoreNextEnter)
		{
			this.m_bIgnoreNextEnter = false;
			return;
		}
		this.m_profileList.Initialize(null, new UIC_FriendsList.ClickCallback(this.HandleClickOnSlot), this);
		this.m_addProfileButton.SetActive(this.m_callback == null);
		this.m_deleteProfileButton.SetActive(this.m_callback == null);
		this.m_deleteProfileToggle.isOn = false;
		if (this.m_bUsePassedProfile)
		{
			this.m_bUsePassedProfile = false;
			this.m_profileList.SetOverrideFactionColor((uint)this.m_passedProfile.factionIndex);
			UIP_FriendSlot friendSlotByDisplayName = this.m_profileList.GetFriendSlotByDisplayName(this.m_passedProfile.name);
			if (friendSlotByDisplayName != null)
			{
				this.DisplayProfile(friendSlotByDisplayName.GetOfflineProfile());
				this.m_profileList.SetSelectedSlot(friendSlotByDisplayName);
				return;
			}
			this.DisplayProfile(this.m_passedProfile);
			this.m_profileList.SetSelected(-1);
			return;
		}
		else
		{
			UIP_FriendSlot friendSlotByDisplayName2 = this.m_profileList.GetFriendSlotByDisplayName(ProfileManager.instance.GetCurrentProfile().name);
			if (friendSlotByDisplayName2 != null)
			{
				this.DisplayProfile(friendSlotByDisplayName2.GetOfflineProfile());
				this.m_profileList.SetSelectedSlot(friendSlotByDisplayName2);
				return;
			}
			Debug.LogError("Unable to get current profile!");
			return;
		}
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x0004EC98 File Offset: 0x0004CE98
	public void OnExitMenu(bool bUnderPopup)
	{
		if (!this.m_bIgnoreNextEnter)
		{
			this.m_profileList.Destroy();
			this.m_callback = null;
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x0004ECB4 File Offset: 0x0004CEB4
	public void SetCallback(UI_ProfileList.ProfileCallback callback, ProfileManager.OfflineProfileEntry passedProfile)
	{
		this.m_callback = callback;
		this.m_passedProfile = passedProfile;
		this.m_bUsePassedProfile = true;
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0004ECCC File Offset: 0x0004CECC
	public void OnExitClicked(bool bConfirm)
	{
		if (this.m_callback != null)
		{
			UIP_FriendSlot friendSlotByDisplayName = this.m_profileList.GetFriendSlotByDisplayName(this.m_profileName.text);
			if (friendSlotByDisplayName != null)
			{
				this.m_callback(friendSlotByDisplayName.GetOfflineProfile(), bConfirm);
				return;
			}
			this.m_callback(null, bConfirm);
		}
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0004ED21 File Offset: 0x0004CF21
	public void OnCreateProfileClicked()
	{
		if (this.m_callback == null)
		{
			this.m_bHandleCreateProfile = true;
			this.OnAvatarSelectClicked();
		}
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0004ED38 File Offset: 0x0004CF38
	public void OnDeleteProfileClicked()
	{
		if (this.m_deleteProfileToggle != null)
		{
			this.m_profileList.SetDeleteMode(this.m_deleteProfileToggle.isOn);
		}
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0004ED60 File Offset: 0x0004CF60
	public void OnAvatarSelectClicked()
	{
		if (this.m_callback == null)
		{
			GameObject scene = ScreenManager.instance.GetScene(this.m_AvatarSelectionScreenName);
			if (scene != null)
			{
				UI_AvatarSelect component = scene.GetComponent<UI_AvatarSelect>();
				if (this.m_bHandleCreateProfile)
				{
					component.SetProfile(new ProfileManager.OfflineProfileEntry
					{
						name = "New Profile",
						factionIndex = (this.m_bUsePassedProfile ? this.m_passedProfile.factionIndex : 0),
						gameAvatar1 = 1,
						gameAvatar2 = 2,
						gameAvatar3 = 3,
						gameAvatar4 = 4,
						gameAvatar5 = 5
					}, new UI_AvatarSelect.AvatarCallback(this.AvatarSelectCallback), true, true, true);
				}
				else
				{
					UIP_FriendSlot selectedSlot = this.m_profileList.GetSelectedSlot();
					component.SetProfile(selectedSlot.GetOfflineProfile(), new UI_AvatarSelect.AvatarCallback(this.AvatarSelectCallback), false, true, false);
				}
				ScreenManager.instance.PushScene(this.m_AvatarSelectionScreenName);
			}
		}
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0004EE44 File Offset: 0x0004D044
	public void AvatarSelectCallback(ProfileManager.OfflineProfileEntry profile, bool bConfirm)
	{
		if (bConfirm)
		{
			if (this.m_bHandleCreateProfile)
			{
				this.m_bHandleCreateProfile = false;
				ProfileManager.instance.Create(profile.name);
				ProfileManager.instance.SetCurrentProfile(ProfileManager.instance.GetProfile(profile.name));
			}
			ProfileManager.OfflineProfileEntry currentProfileRef = ProfileManager.instance.GetCurrentProfileRef();
			currentProfileRef.name = profile.name;
			currentProfileRef.gameAvatar1 = profile.gameAvatar1;
			currentProfileRef.gameAvatar2 = profile.gameAvatar2;
			currentProfileRef.gameAvatar3 = profile.gameAvatar3;
			currentProfileRef.gameAvatar4 = profile.gameAvatar4;
			currentProfileRef.gameAvatar5 = profile.gameAvatar5;
			currentProfileRef.factionIndex = profile.factionIndex;
			ProfileManager.instance.UpdateProfileName(ref currentProfileRef);
			ProfileManager.instance.Save();
			return;
		}
		this.m_bHandleCreateProfile = false;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0004EF10 File Offset: 0x0004D110
	private void HandleClickOnSlot(UIP_FriendSlot slot, UIP_FriendSlot.ClickEventType evt)
	{
		if (slot != null)
		{
			if (this.m_deleteProfileToggle.isOn)
			{
				GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
				if (scene != null)
				{
					UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
					if (component)
					{
						this.m_bIgnoreNextEnter = true;
						if (this.m_profileName.text == slot.GetOfflineProfile().name)
						{
							component.Setup(null, "${Key_NoDeleteProfileActive}", UI_ConfirmPopup.ButtonFormat.OneButton);
						}
						else
						{
							component.Setup(new UI_ConfirmPopup.ClickCallback(this.OnConfirmPopupPressed), "${Key_DeleteProfilePrompt}", UI_ConfirmPopup.ButtonFormat.TwoButtons);
							this.m_deleteSlot = slot;
						}
						ScreenManager.instance.PushScene("ConfirmPopup");
						return;
					}
				}
			}
			else
			{
				this.DisplayProfile(slot.GetOfflineProfile());
				this.m_profileList.SetSelectedSlot(slot);
				if (this.m_callback == null)
				{
					ProfileManager.instance.SetCurrentProfile(slot.GetOfflineProfile());
				}
			}
		}
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0004EFFC File Offset: 0x0004D1FC
	private void OnConfirmPopupPressed(bool bConfirm)
	{
		if (bConfirm)
		{
			ProfileManager.instance.Delete(this.m_deleteSlot.GetOfflineProfile().name);
			this.m_profileList.RebuildOfflineList(-1);
			UIP_FriendSlot friendSlotByDisplayName = this.m_profileList.GetFriendSlotByDisplayName(ProfileManager.instance.GetCurrentProfile().name);
			if (friendSlotByDisplayName != null)
			{
				this.DisplayProfile(friendSlotByDisplayName.GetOfflineProfile());
				this.m_profileList.SetSelectedSlot(friendSlotByDisplayName);
			}
			else
			{
				Debug.LogError("Unable to get current profile!");
			}
		}
		this.m_deleteSlot = null;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0004F084 File Offset: 0x0004D284
	private void DisplayProfile(ProfileManager.OfflineProfileEntry profile)
	{
		this.m_profileName.text = profile.name;
		this.m_avatar.SetAvatar((int)(10 * profile.factionIndex + profile.gameAvatar1), true);
		this.m_avatar2.SetAvatar((int)(10 * profile.factionIndex + profile.gameAvatar2), true);
		this.m_colorizer.Colorize((uint)profile.factionIndex);
		this.m_completedGames.text = profile.completed.ToString();
		this.m_2pRecord.text = profile.wins_2p.ToString() + " - " + profile.losses_2p.ToString();
		this.m_3pRecord.text = profile.wins_3p.ToString() + " - " + profile.losses_3p.ToString();
		this.m_4pRecord.text = profile.wins_4p.ToString() + " - " + profile.losses_4p.ToString();
		this.m_5pRecord.text = profile.wins_5p.ToString() + " - " + profile.losses_5p.ToString();
	}

	// Token: 0x04000C23 RID: 3107
	public string m_AvatarSelectionScreenName = "FamilySelection";

	// Token: 0x04000C24 RID: 3108
	public UIC_FriendsList m_profileList;

	// Token: 0x04000C25 RID: 3109
	public GameObject m_addProfileButton;

	// Token: 0x04000C26 RID: 3110
	public GameObject m_deleteProfileButton;

	// Token: 0x04000C27 RID: 3111
	public Toggle m_deleteProfileToggle;

	// Token: 0x04000C28 RID: 3112
	public TextMeshProUGUI m_profileName;

	// Token: 0x04000C29 RID: 3113
	public TextMeshProUGUI m_completedGames;

	// Token: 0x04000C2A RID: 3114
	public TextMeshProUGUI m_2pRecord;

	// Token: 0x04000C2B RID: 3115
	public TextMeshProUGUI m_3pRecord;

	// Token: 0x04000C2C RID: 3116
	public TextMeshProUGUI m_4pRecord;

	// Token: 0x04000C2D RID: 3117
	public TextMeshProUGUI m_5pRecord;

	// Token: 0x04000C2E RID: 3118
	public Avatar_UI m_avatar;

	// Token: 0x04000C2F RID: 3119
	public Avatar_UI m_avatar2;

	// Token: 0x04000C30 RID: 3120
	public ColorByFaction m_colorizer;

	// Token: 0x04000C31 RID: 3121
	private UI_ProfileList.ProfileCallback m_callback;

	// Token: 0x04000C32 RID: 3122
	private ProfileManager.OfflineProfileEntry m_passedProfile;

	// Token: 0x04000C33 RID: 3123
	private UIP_FriendSlot m_deleteSlot;

	// Token: 0x04000C34 RID: 3124
	private bool m_bUsePassedProfile;

	// Token: 0x04000C35 RID: 3125
	private bool m_bHandleCreateProfile;

	// Token: 0x04000C36 RID: 3126
	private bool m_bIgnoreNextEnter;

	// Token: 0x02000804 RID: 2052
	// (Invoke) Token: 0x060043E1 RID: 17377
	public delegate void ProfileCallback(ProfileManager.OfflineProfileEntry profile, bool bConfirm);
}
