using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000114 RID: 276
public class UI_AvatarSelect : MonoBehaviour
{
	// Token: 0x06000A48 RID: 2632 RVA: 0x000442C0 File Offset: 0x000424C0
	public void OnEnterMenu()
	{
		if (!this.m_bInit)
		{
			this.m_bInit = true;
		}
		if (Network.m_Network.m_bConnectedToServer)
		{
			NetworkPlayerProfileInfo networkPlayerProfileInfo;
			Network.GetLocalPlayerProfileInfo(out networkPlayerProfileInfo);
			this.m_renameField.text = networkPlayerProfileInfo.displayName;
		}
		else
		{
			this.m_renameField.text = this.m_profile.name;
		}
		this.m_bIgnoreToggles = true;
		for (int i = 0; i < this.m_factionToggles.Length; i++)
		{
			this.m_factionToggles[i].isOn = ((int)this.m_profile.factionIndex == i);
		}
		this.m_bIgnoreToggles = false;
		this.SetTopData();
		this.SetBottomData();
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00044360 File Offset: 0x00042560
	public void OnExitMenu(bool bUnderPopup)
	{
		if (this.m_doneButton != null)
		{
			this.m_doneButton.SupressNextPress(false);
		}
		if (this.m_bIgnoreExit)
		{
			this.m_bIgnoreExit = false;
			return;
		}
		this.m_callback = null;
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00044394 File Offset: 0x00042594
	public void OnExitPressed(bool bConfirm)
	{
		if (bConfirm && this.m_bCheckForProfileOverlap && ProfileManager.instance.CheckIfProfileExistWithDisplayName(this.m_renameField.text) != null)
		{
			this.m_bIgnoreExit = true;
			GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
			if (scene != null)
			{
				UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
				if (component)
				{
					component.Setup(null, "Key_DuplicateProfileName", UI_ConfirmPopup.ButtonFormat.OneButton);
					ScreenManager.instance.PushScene("ConfirmPopup");
					ScreenManager.instance.PushScene("ConfirmPopup");
				}
			}
			return;
		}
		this.m_profile.name = this.m_renameField.text;
		if (this.m_callback != null)
		{
			this.m_callback(this.m_profile, bConfirm);
		}
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0004444E File Offset: 0x0004264E
	public void SetProfile(ProfileManager.OfflineProfileEntry profile)
	{
		this.m_profile = profile;
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00044458 File Offset: 0x00042658
	public void SetProfile(ProfileManager.OfflineProfileEntry profile, UI_AvatarSelect.AvatarCallback callback, bool bAllowRename, bool bAllowColorSelect, bool bCheckForExistingProfile = false)
	{
		this.m_profile = profile;
		this.m_callback = callback;
		this.m_bCheckForProfileOverlap = bCheckForExistingProfile;
		this.m_renameField.interactable = bAllowRename;
		this.m_factionSelection.SetActive(bAllowColorSelect);
		this.m_profileSelection.SetActive(!bAllowColorSelect);
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x000444A4 File Offset: 0x000426A4
	public void SetSupressDoneNav()
	{
		if (this.m_doneButton != null)
		{
			this.m_doneButton.SupressNextPress(true);
		}
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x000444C0 File Offset: 0x000426C0
	public void HandleClickOnFaction()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		byte factionIndex = this.m_profile.factionIndex;
		byte b = 0;
		for (int i = 0; i < this.m_factionToggles.Length; i++)
		{
			if (this.m_factionToggles[i].isOn)
			{
				b = (byte)i;
				break;
			}
		}
		if (factionIndex != b)
		{
			this.m_profile.factionIndex = b;
			this.SetTopData();
			this.SetBottomData();
		}
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x00044528 File Offset: 0x00042728
	public void HandleClickOnProfileSelection()
	{
		GameObject scene = ScreenManager.instance.GetScene(this.m_profileSelectScreenName);
		if (scene != null)
		{
			this.m_bIgnoreExit = true;
			scene.GetComponent<UI_ProfileList>().SetCallback(new UI_ProfileList.ProfileCallback(this.ProfileSelectCallback), this.m_profile);
			ScreenManager.instance.PushScene(this.m_profileSelectScreenName);
		}
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00044584 File Offset: 0x00042784
	public void ProfileSelectCallback(ProfileManager.OfflineProfileEntry profile, bool bConfirm)
	{
		if (bConfirm && profile != null)
		{
			byte factionIndex = this.m_profile.factionIndex;
			this.m_profile = profile;
			if (!this.m_factionSelection.activeSelf)
			{
				this.m_profile.factionIndex = factionIndex;
			}
			this.SetTopData();
			this.SetBottomData();
		}
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x000445D0 File Offset: 0x000427D0
	public void HandleDrop(UI_DragSource src, UI_DropTarget target)
	{
		UIP_CreateGameDragToken component = src.GetComponent<UIP_CreateGameDragToken>();
		UIP_CreateGameDragToken component2 = target.GetComponent<UIP_CreateGameDragToken>();
		if (component != null && component2 != null)
		{
			byte b = (byte)(component.m_avatar.GetIndex() % 10);
			if (b == 0)
			{
				b = 10;
			}
			if (b == this.m_profile.gameAvatar1 || b == this.m_profile.gameAvatar2 || b == this.m_profile.gameAvatar3 || b == this.m_profile.gameAvatar4 || b == this.m_profile.gameAvatar5)
			{
				return;
			}
			if (component2 == this.m_selectedAvatars[0])
			{
				this.m_profile.gameAvatar1 = b;
			}
			else if (component2 == this.m_selectedAvatars[1])
			{
				this.m_profile.gameAvatar2 = b;
			}
			else if (component2 == this.m_selectedAvatars[2])
			{
				this.m_profile.gameAvatar3 = b;
			}
			else if (component2 == this.m_selectedAvatars[3])
			{
				this.m_profile.gameAvatar4 = b;
			}
			else if (component2 == this.m_selectedAvatars[4])
			{
				this.m_profile.gameAvatar5 = b;
			}
			this.SetBottomData();
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x000446FC File Offset: 0x000428FC
	private void SetTopData()
	{
		int factionIndex = (int)this.m_profile.factionIndex;
		for (int i = 0; i < this.m_availableAvatars.Length; i++)
		{
			this.m_availableAvatars[i].Setup(10 * factionIndex + i + 1, (uint)factionIndex);
		}
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00044740 File Offset: 0x00042940
	private void SetBottomData()
	{
		int factionIndex = (int)this.m_profile.factionIndex;
		this.m_selectedAvatars[0].Setup(10 * factionIndex + (int)this.m_profile.gameAvatar1, (uint)factionIndex);
		this.m_selectedAvatars[1].Setup(10 * factionIndex + (int)this.m_profile.gameAvatar2, (uint)factionIndex);
		this.m_selectedAvatars[2].Setup(10 * factionIndex + (int)this.m_profile.gameAvatar3, (uint)factionIndex);
		this.m_selectedAvatars[3].Setup(10 * factionIndex + (int)this.m_profile.gameAvatar4, (uint)factionIndex);
		this.m_selectedAvatars[4].Setup(10 * factionIndex + (int)this.m_profile.gameAvatar5, (uint)factionIndex);
	}

	// Token: 0x04000ADB RID: 2779
	public string m_profileSelectScreenName = "OfflineProfiles";

	// Token: 0x04000ADC RID: 2780
	public UIP_CreateGameDragToken[] m_availableAvatars;

	// Token: 0x04000ADD RID: 2781
	public UIP_CreateGameDragToken[] m_selectedAvatars;

	// Token: 0x04000ADE RID: 2782
	public GameObject m_profileSelection;

	// Token: 0x04000ADF RID: 2783
	public GameObject m_factionSelection;

	// Token: 0x04000AE0 RID: 2784
	public Toggle[] m_factionToggles;

	// Token: 0x04000AE1 RID: 2785
	public TMP_InputField m_renameField;

	// Token: 0x04000AE2 RID: 2786
	public NavigationButton m_doneButton;

	// Token: 0x04000AE3 RID: 2787
	private bool m_bInit;

	// Token: 0x04000AE4 RID: 2788
	private bool m_bIgnoreToggles;

	// Token: 0x04000AE5 RID: 2789
	private bool m_bIgnoreExit;

	// Token: 0x04000AE6 RID: 2790
	private bool m_bCheckForProfileOverlap;

	// Token: 0x04000AE7 RID: 2791
	private ProfileManager.OfflineProfileEntry m_profile;

	// Token: 0x04000AE8 RID: 2792
	private UI_AvatarSelect.AvatarCallback m_callback;

	// Token: 0x020007E7 RID: 2023
	// (Invoke) Token: 0x0600436F RID: 17263
	public delegate void AvatarCallback(ProfileManager.OfflineProfileEntry profile, bool bComplete);
}
