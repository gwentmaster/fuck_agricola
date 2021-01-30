using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x02000649 RID: 1609
	public class SSOChooseALoginNamePanel : SSOBasePanel
	{
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06003B49 RID: 15177 RVA: 0x00126B4B File Offset: 0x00124D4B
		// (set) Token: 0x06003B4A RID: 15178 RVA: 0x00126B5D File Offset: 0x00124D5D
		public string LoginName
		{
			get
			{
				return this._ui.loginNameInputField.text;
			}
			set
			{
				this._ui.loginNameInputField.text = value;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06003B4B RID: 15179 RVA: 0x00126B70 File Offset: 0x00124D70
		// (set) Token: 0x06003B4C RID: 15180 RVA: 0x00126B82 File Offset: 0x00124D82
		public string Password
		{
			get
			{
				return this._ui.passwordInputField.text;
			}
			set
			{
				this._ui.passwordInputField.text = value;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06003B4D RID: 15181 RVA: 0x00126B95 File Offset: 0x00124D95
		public bool AreRequirementsMet
		{
			get
			{
				return this._ui.loginNameInputField.text.Length >= 4 && this._ui.passwordInputField.text.Length >= 1 && this._currentState == SSOChooseALoginNamePanel.LoginState.Available;
			}
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x00126BD2 File Offset: 0x00124DD2
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.loginNameInputField);
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.passwordInputField);
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x00126BF4 File Offset: 0x00124DF4
		public override void Show()
		{
			this._ui.loginNameInputField.text = string.Empty;
			this._ui.passwordInputField.text = string.Empty;
			this.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Empty);
			this.ShowPasswordStrength(-1);
			if (this._ui.showPasswordToggle != null)
			{
				this._ui.showPasswordToggle.isOn = (this._ui.passwordInputField.contentType == TMP_InputField.ContentType.Password);
			}
			base.Show();
			this._UpdateYesButtonState();
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x00126C7B File Offset: 0x00124E7B
		private void _UpdateYesButtonState()
		{
			this._buttons[1].interactable = this.AreRequirementsMet;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x00126C90 File Offset: 0x00124E90
		public void ShowLoginNameState(SSOChooseALoginNamePanel.LoginState state)
		{
			this._currentState = state;
			switch (state)
			{
			default:
				this._ui.loginNameMessage.gameObject.SetActive(false);
				this._ui.searchingLoginNameSpinner.SetActive(false);
				this._ui.loginNameIcon.sprite = this._ui.placeholderLoginNameIcon;
				break;
			case SSOChooseALoginNamePanel.LoginState.Searching:
				this._ui.loginNameMessage.gameObject.SetActive(false);
				this._ui.searchingLoginNameSpinner.SetActive(true);
				this._ui.loginNameIcon.sprite = this._ui.placeholderLoginNameIcon;
				break;
			case SSOChooseALoginNamePanel.LoginState.Available:
				this._ui.loginNameMessage.gameObject.SetActive(true);
				this._ui.searchingLoginNameSpinner.SetActive(false);
				this._ui.loginNameIcon.sprite = this._ui.validLoginNameIcon;
				this._ui.loginNameMessage.color = this._ui.validLoginNameColor;
				this._ui.loginNameMessage.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.ChooseALoginPanel.LoginNameAvailable");
				break;
			case SSOChooseALoginNamePanel.LoginState.Unavailable:
				this._ui.loginNameMessage.gameObject.SetActive(true);
				this._ui.searchingLoginNameSpinner.SetActive(false);
				this._ui.loginNameIcon.sprite = this._ui.invalidLoginNameIcon;
				this._ui.loginNameMessage.color = this._ui.invalidLoginNameColor;
				this._ui.loginNameMessage.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.ChooseALoginPanel.LoginNameUnAvailable");
				break;
			}
			this._UpdateYesButtonState();
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x00126E58 File Offset: 0x00125058
		public void ShowPasswordStrength(int strength)
		{
			if (strength < 0 || strength > 4)
			{
				this._ui.passwordIcon.sprite = this._ui.placeholderPasswordIcon;
				this._ui.passwordMessage.gameObject.SetActive(false);
			}
			else
			{
				if (strength == 0 || strength == 1)
				{
					this._ui.passwordIcon.sprite = this._ui.weakPasswordIcon;
					this._ui.passwordMessage.color = this._ui.weakPasswordColor;
				}
				else if (strength == 2)
				{
					this._ui.passwordIcon.sprite = this._ui.mediumPasswordIcon;
					this._ui.passwordMessage.color = this._ui.mediumPasswordColor;
				}
				else
				{
					this._ui.passwordIcon.sprite = this._ui.strongPasswordIcon;
					this._ui.passwordMessage.color = this._ui.strongPasswordColor;
				}
				this._ui.passwordMessage.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText(string.Format("SSO.ChooseALoginPanel.PasswordLevel{0}", strength));
				this._ui.passwordMessage.gameObject.SetActive(true);
			}
			this._UpdateYesButtonState();
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x00126F9F File Offset: 0x0012519F
		public void ToggleInputFieldContentType()
		{
			this._ui.passwordInputField.contentType = (this._ui.showPasswordToggle.isOn ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard);
			this._ui.passwordInputField.ForceLabelUpdate();
		}

		// Token: 0x0400265C RID: 9820
		[SerializeField]
		private SSOChooseALoginNamePanel.UI _ui;

		// Token: 0x0400265D RID: 9821
		private SSOChooseALoginNamePanel.LoginState _currentState;

		// Token: 0x02000950 RID: 2384
		[Serializable]
		public class UI
		{
			// Token: 0x0400313D RID: 12605
			public TMP_InputField loginNameInputField;

			// Token: 0x0400313E RID: 12606
			public Image loginNameIcon;

			// Token: 0x0400313F RID: 12607
			public Sprite placeholderLoginNameIcon;

			// Token: 0x04003140 RID: 12608
			public Sprite validLoginNameIcon;

			// Token: 0x04003141 RID: 12609
			public Color validLoginNameColor;

			// Token: 0x04003142 RID: 12610
			public Sprite invalidLoginNameIcon;

			// Token: 0x04003143 RID: 12611
			public Color invalidLoginNameColor;

			// Token: 0x04003144 RID: 12612
			public GameObject searchingLoginNameSpinner;

			// Token: 0x04003145 RID: 12613
			public TextMeshProUGUI loginNameMessage;

			// Token: 0x04003146 RID: 12614
			public TMP_InputField passwordInputField;

			// Token: 0x04003147 RID: 12615
			public Toggle showPasswordToggle;

			// Token: 0x04003148 RID: 12616
			public Image passwordIcon;

			// Token: 0x04003149 RID: 12617
			public Sprite placeholderPasswordIcon;

			// Token: 0x0400314A RID: 12618
			public Sprite strongPasswordIcon;

			// Token: 0x0400314B RID: 12619
			public Color strongPasswordColor;

			// Token: 0x0400314C RID: 12620
			public Sprite mediumPasswordIcon;

			// Token: 0x0400314D RID: 12621
			public Color mediumPasswordColor;

			// Token: 0x0400314E RID: 12622
			public Sprite weakPasswordIcon;

			// Token: 0x0400314F RID: 12623
			public Color weakPasswordColor;

			// Token: 0x04003150 RID: 12624
			public TextMeshProUGUI passwordMessage;
		}

		// Token: 0x02000951 RID: 2385
		public enum LoginState
		{
			// Token: 0x04003152 RID: 12626
			Empty,
			// Token: 0x04003153 RID: 12627
			Searching,
			// Token: 0x04003154 RID: 12628
			Available,
			// Token: 0x04003155 RID: 12629
			Unavailable
		}
	}
}
