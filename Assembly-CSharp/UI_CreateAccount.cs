using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000118 RID: 280
public class UI_CreateAccount : MonoBehaviour
{
	// Token: 0x06000A76 RID: 2678 RVA: 0x000456BB File Offset: 0x000438BB
	private void Start()
	{
		this.m_system = EventSystem.current;
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x000456C8 File Offset: 0x000438C8
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable selectable = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? this.m_system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() : this.m_system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			if (selectable != null)
			{
				InputField component = selectable.GetComponent<InputField>();
				if (component != null)
				{
					component.OnPointerClick(new PointerEventData(this.m_system));
				}
			}
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00045750 File Offset: 0x00043950
	public void OnEnterMenu()
	{
		if (this.m_bHandlePopup)
		{
			this.m_bHandlePopup = false;
			return;
		}
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.DisableToggleSoundEffects();
		}
		this.m_tosAgree.isOn = false;
		if (ScreenManager.instance.m_audioHandler != null)
		{
			ScreenManager.instance.m_audioHandler.EnableToggleSoundEffects();
		}
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00045440 File Offset: 0x00043640
	public void OnExitMenu(bool bUnderPopup)
	{
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x000457BC File Offset: 0x000439BC
	public void OnCreateAccount()
	{
		string text = string.Empty;
		if (string.IsNullOrEmpty(this.m_emailLabel.text))
		{
			text = "Key_EmailRequried";
		}
		if (text == string.Empty && string.IsNullOrEmpty(this.m_nameLabel.text))
		{
			text = "Key_UsernameRequired";
		}
		if (text == string.Empty && string.IsNullOrEmpty(this.m_pwdLabel.text))
		{
			text = "Key_PasswordRequired";
		}
		if (text == string.Empty && !this.m_pwdLabel.text.Equals(this.m_confirmPwdLabel.text))
		{
			text = "Key_PasswordNotVerified";
		}
		if (text == string.Empty && !this.m_tosAgree.isOn)
		{
			text = "Key_MustAcceptTerms";
		}
		if (text != string.Empty)
		{
			GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
			if (scene != null)
			{
				UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
				if (component)
				{
					this.m_bHandlePopup = true;
					component.Setup(null, text, UI_ConfirmPopup.ButtonFormat.OneButton);
					ScreenManager.instance.PushScene("ConfirmPopup");
					return;
				}
			}
		}
		else
		{
			Network.SetCreateAccount(this.m_emailLabel.text, this.m_pwdLabel.text, this.m_nameLabel.text);
			ScreenManager.instance.PushScene("Connecting");
		}
	}

	// Token: 0x04000B17 RID: 2839
	public TMP_InputField m_emailLabel;

	// Token: 0x04000B18 RID: 2840
	public TMP_InputField m_nameLabel;

	// Token: 0x04000B19 RID: 2841
	public TMP_InputField m_pwdLabel;

	// Token: 0x04000B1A RID: 2842
	public TMP_InputField m_confirmPwdLabel;

	// Token: 0x04000B1B RID: 2843
	public Toggle m_tosAgree;

	// Token: 0x04000B1C RID: 2844
	private bool m_bHandlePopup;

	// Token: 0x04000B1D RID: 2845
	private EventSystem m_system;
}
