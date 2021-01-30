using System;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x0200064E RID: 1614
	public class UpdateEmailPopUp : MonoBehaviour
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x00128006 File Offset: 0x00126206
		// (set) Token: 0x06003BA4 RID: 15268 RVA: 0x00128018 File Offset: 0x00126218
		public string Email
		{
			get
			{
				return this._ui.emailInputField.text;
			}
			set
			{
				this._ui.emailInputField.text = value;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06003BA5 RID: 15269 RVA: 0x0012802B File Offset: 0x0012622B
		public bool AreRequirementsMet
		{
			get
			{
				return EmailFormatValidator.IsValidEmail(this.Email);
			}
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x00128038 File Offset: 0x00126238
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.emailInputField);
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x0012804A File Offset: 0x0012624A
		public void Init(Action<bool> onComplete)
		{
			this._onComplete = onComplete;
			this.OnEmailValueChanged();
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x00128059 File Offset: 0x00126259
		public void OnEmailValueChanged()
		{
			this._ui.submitButton.interactable = this.AreRequirementsMet;
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x00128071 File Offset: 0x00126271
		public void OnEmailEndEdit()
		{
			this.OnYesButtonClicked();
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x0012807C File Offset: 0x0012627C
		public void OnYesButtonClicked()
		{
			if (!this.AreRequirementsMet)
			{
				return;
			}
			ActivityIndicatorButton activityIndicatorButton = this._ui.submitButton.GetComponent<ActivityIndicatorButton>();
			if (activityIndicatorButton != null)
			{
				activityIndicatorButton.Waiting = true;
			}
			this._endpoint = new UpdateEmailAndNewsletterEndpoint(this._ui.emailInputField.text, this._ui.subscribeToNewsletterButton.IsOn, null);
			this._endpoint.Execute(delegate(WebError error)
			{
				if (activityIndicatorButton != null)
				{
					activityIndicatorButton.Waiting = false;
				}
				Action<bool> onComplete = this._onComplete;
				if (onComplete == null)
				{
					return;
				}
				onComplete(error == null);
			});
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x00128112 File Offset: 0x00126312
		public void OnNoButtonClicked()
		{
			if (this._endpoint != null)
			{
				this._endpoint.Abort();
				this._endpoint = null;
			}
			Action<bool> onComplete = this._onComplete;
			if (onComplete == null)
			{
				return;
			}
			onComplete(false);
		}

		// Token: 0x04002670 RID: 9840
		private const string _kModuleName = "UpdateEmailPopUp";

		// Token: 0x04002671 RID: 9841
		[SerializeField]
		private UpdateEmailPopUp.UI _ui;

		// Token: 0x04002672 RID: 9842
		private Action<bool> _onComplete;

		// Token: 0x04002673 RID: 9843
		private UpdateEmailAndNewsletterEndpoint _endpoint;

		// Token: 0x0200095D RID: 2397
		[Serializable]
		public class UI
		{
			// Token: 0x0400317A RID: 12666
			public TMP_InputField emailInputField;

			// Token: 0x0400317B RID: 12667
			public ToggleButton subscribeToNewsletterButton;

			// Token: 0x0400317C RID: 12668
			public Button submitButton;
		}
	}
}
