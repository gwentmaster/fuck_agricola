using System;
using System.Collections;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;
using Zxcvbn;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x0200064B RID: 1611
	public class SSOManager : MonoBehaviour
	{
		// Token: 0x06003B60 RID: 15200 RVA: 0x001271B8 File Offset: 0x001253B8
		public static void InstantiateSSO(SSOAuthenticate authenticate, OnSSOSucceeded successCallback, AbortSSO abortCallback)
		{
			if ((SSOManager)UnityEngine.Object.FindObjectOfType(typeof(SSOManager)) == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.ssoPrefab);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Action <>9__2;
				Action <>9__3;
				gameObject.GetComponent<SSOManager>().Init(authenticate, delegate
				{
					ResponsivePopUp component = gameObject.GetComponent<ResponsivePopUp>();
					Action completion;
					if ((completion = <>9__2) == null)
					{
						completion = (<>9__2 = delegate()
						{
							UnityEngine.Object.Destroy(gameObject);
							OnSSOSucceeded successCallback2 = successCallback;
							if (successCallback2 == null)
							{
								return;
							}
							successCallback2();
						});
					}
					component.FadeOut(completion);
				}, delegate
				{
					ResponsivePopUp component = gameObject.GetComponent<ResponsivePopUp>();
					Action completion;
					if ((completion = <>9__3) == null)
					{
						completion = (<>9__3 = delegate()
						{
							UnityEngine.Object.Destroy(gameObject);
							AbortSSO abortCallback2 = abortCallback;
							if (abortCallback2 == null)
							{
								return;
							}
							abortCallback2();
						});
					}
					component.FadeOut(completion);
				});
				return;
			}
			AsmoLogger.Error("SSOManager", "Try to InstantiateSSO twice", null);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x00127264 File Offset: 0x00125464
		public static void InstantiateUpdateEmailPopUp(Action<bool> onComplete)
		{
			if ((UpdateEmailPopUp)UnityEngine.Object.FindObjectOfType(typeof(UpdateEmailPopUp)) == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.updateEmailPopUpPrefab);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				gameObject.GetComponent<UpdateEmailPopUp>().Init(delegate(bool success)
				{
					if (success)
					{
						Action <>9__2;
						CoreApplication.Instance.OAuthGate.UpdateUserDetails(delegate
						{
							ResponsivePopUp component = gameObject.GetComponent<ResponsivePopUp>();
							Action completion;
							if ((completion = <>9__2) == null)
							{
								completion = (<>9__2 = delegate()
								{
									UnityEngine.Object.Destroy(gameObject);
									Action<bool> onComplete2 = onComplete;
									if (onComplete2 == null)
									{
										return;
									}
									onComplete2(success);
								});
							}
							component.FadeOut(completion);
						}, false);
						return;
					}
					gameObject.GetComponent<ResponsivePopUp>().FadeOut(delegate
					{
						UnityEngine.Object.Destroy(gameObject);
						Action<bool> onComplete2 = onComplete;
						if (onComplete2 == null)
						{
							return;
						}
						onComplete2(success);
					});
				});
				return;
			}
			AsmoLogger.Error("SSOManager", "Try to InstantiateUpdateEmailPopUp twice", null);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x001272F9 File Offset: 0x001254F9
		public void Init(SSOAuthenticate ssoAuthenticate, OnSSOSucceeded onSSOSucceded, AbortSSO abortSSO)
		{
			this._SSOAuthenticate = ssoAuthenticate;
			this._onSSOSucceed = onSSOSucceded;
			this._abortSSO = abortSSO;
			AnalyticsEvents.LogConnectAsmodeeNetStartEvent("automatic", null);
			this._DisplayDoYouHaveAnAccountPanel();
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x00127321 File Offset: 0x00125521
		public void Abort()
		{
			this._AbortLoginPanel();
			this._AbortPasswordPanel();
			this._AbortWelcomePanel();
			this._AbortChooseALoginNamePanel();
			this._HideAllPanels();
			AnalyticsEvents.LogConnectAsmodeeNetStopEvent(false, this._lastError, this._didResetPassword, null);
			this._abortSSO();
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x00127360 File Offset: 0x00125560
		private void _HideAllPanels()
		{
			this._ui.DoYouHaveAnAccountPanel.Hide();
			this._ui.LoginPanel.Hide();
			this._ui.PasswordPanel.Hide();
			this._ui.ChooseALoginNamePanel.Hide();
			this._ui.WelcomePanel.Hide();
			this._ui.OkPanel.Hide();
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x001273CD File Offset: 0x001255CD
		private void _DisplayDoYouHaveAnAccountPanel()
		{
			this._HideAllPanels();
			this._ui.DoYouHaveAnAccountPanel.Show();
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x001273E5 File Offset: 0x001255E5
		public void DoYouHaveAnAccountPanel_No()
		{
			this._DisplayWelcomePanel(true);
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x001273EE File Offset: 0x001255EE
		public void DoYouHaveAnAccountPanel_Yes()
		{
			this._DisplayLoginPanel(true);
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x001273F7 File Offset: 0x001255F7
		private void _DisplayLoginPanel(bool resetLogin = true)
		{
			this._HideAllPanels();
			this._ui.LoginPanel.Show(resetLogin);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x00127410 File Offset: 0x00125610
		private void _AbortLoginPanel()
		{
			if (this._searchByLoginOrEmailEndpoint != null)
			{
				this._searchByLoginOrEmailEndpoint.Abort();
				this._searchByLoginOrEmailEndpoint = null;
			}
			this._ui.LoginPanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0012743E File Offset: 0x0012563E
		public void LoginPanel_No()
		{
			this._AbortLoginPanel();
			this._DisplayDoYouHaveAnAccountPanel();
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0012744C File Offset: 0x0012564C
		public void LoginPanel_Yes()
		{
			if (!this._ui.LoginPanel.AreRequirementsMet)
			{
				return;
			}
			this._ui.LoginPanel.SwitchWaitingPanelMode(true, 1);
			string text = this._ui.LoginPanel.Text;
			try
			{
				if (EmailFormatValidator.IsValidEmail(text))
				{
					this._searchByLoginOrEmailEndpoint = new SearchByEmailEndpoint(text, Extras.None, null);
				}
				else
				{
					this._searchByLoginOrEmailEndpoint = new SearchByLoginEndpoint(text, Extras.None, -1, -1, null);
				}
			}
			catch (Exception ex)
			{
				this._LoginOrMailNotFound(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.LoginPanel.LoginEmailInvalidMessage"));
				AsmoLogger.Error("SSOManager", ex.Message, null);
				return;
			}
			this._searchByLoginOrEmailEndpoint.Execute(delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				this._ui.LoginPanel.SwitchWaitingPanelMode(false, -1);
				if (webError == null)
				{
					if (result.TotalElement > 0)
					{
						this._LoginOrMailFound(result.Elements[0]);
					}
					else
					{
						this._LoginOrMailNotFound(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.LoginOrEmailNotFound"));
					}
				}
				else
				{
					this._lastError = webError.ToString();
					string message;
					if (webError.ToChildError<ApiResponseError>() != null)
					{
						message = string.Format("{0}\n{1}", CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.LoginOrEmailNotFound"), webError);
					}
					else
					{
						message = CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect");
					}
					this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), message, delegate
					{
						this._DisplayLoginPanel(false);
					}, SSOOKPanel.MessageType.Error);
				}
				this._searchByLoginOrEmailEndpoint = null;
			});
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x00127510 File Offset: 0x00125710
		private void _LoginOrMailFound(UserSearchResult user)
		{
			this._user = user;
			this._DisplayPasswordPanel();
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x0012751F File Offset: 0x0012571F
		private void _LoginOrMailNotFound(string errorMessage)
		{
			this._ui.LoginPanel.DisplayErrorMessage(errorMessage);
			this._ui.LoginPanel.SelectInputFieldContent();
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x00127542 File Offset: 0x00125742
		private void _DisplayPasswordPanel()
		{
			this._HideAllPanels();
			this._ui.PasswordPanel.Show(true);
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0012755B File Offset: 0x0012575B
		private void _AbortPasswordPanel()
		{
			if (this._resetPasswordEndpoint != null)
			{
				this._resetPasswordEndpoint.Abort();
				this._resetPasswordEndpoint = null;
			}
			this._ui.PasswordPanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00127589 File Offset: 0x00125789
		public void PasswordPanel_No()
		{
			this._AbortPasswordPanel();
			this._user = null;
			this._DisplayLoginPanel(false);
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x001275A0 File Offset: 0x001257A0
		public void PasswordPanel_Yes()
		{
			if (!this._ui.PasswordPanel.AreRequirementsMet)
			{
				return;
			}
			this._ui.PasswordPanel.SwitchWaitingPanelMode(true, 1);
			string text = this._ui.PasswordPanel.Text;
			this._SSOAuthenticate(this._user.LoginName, text, delegate(OAuthError error)
			{
				this._ui.PasswordPanel.SwitchWaitingPanelMode(false, -1);
				if (error == null)
				{
					AnalyticsEvents.LogConnectAsmodeeNetStopEvent(true, this._lastError, this._didResetPassword, null);
					this._onSSOSucceed();
					return;
				}
				if (error.status / 100 == 4)
				{
					this._ui.PasswordPanel.DisplayErrorMessage(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.BadLoginPasswordCombination"));
					this._ui.PasswordPanel.SelectInputFieldContent();
					return;
				}
				this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
				{
					this._DisplayPasswordPanel();
				}, SSOOKPanel.MessageType.Error);
			});
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x00127608 File Offset: 0x00125808
		public void PasswordPanel_ResetPassword()
		{
			this._ui.PasswordPanel.SwitchWaitingPanelMode(true, 2);
			this._resetPasswordEndpoint = new ResetPasswordEndpoint(this._user.UserId, null);
			this._resetPasswordEndpoint.Execute(delegate(WebError webError)
			{
				if (webError != null)
				{
					this._lastError = webError.ToString();
				}
				this._ui.PasswordPanel.SwitchWaitingPanelMode(false, -1);
				string title = (webError == null) ? CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.PasswordReset.Title") : CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title");
				string message = (webError == null) ? CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.PasswordReset.Message") : CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect");
				SSOOKPanel.MessageType messageType = (webError == null) ? SSOOKPanel.MessageType.Standard : SSOOKPanel.MessageType.Error;
				this._DisplayOkPanel(title, message, delegate
				{
					this._DisplayPasswordPanel();
				}, messageType);
				this._resetPasswordEndpoint = null;
				this._didResetPassword = true;
			});
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x00127655 File Offset: 0x00125855
		private void _DisplayWelcomePanel(bool resetEmail = true)
		{
			this._HideAllPanels();
			this._ui.WelcomePanel.Show(resetEmail);
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0012766E File Offset: 0x0012586E
		private void _AbortWelcomePanel()
		{
			if (this._searchByEmailEndpoint != null)
			{
				this._searchByEmailEndpoint.Abort();
				this._searchByEmailEndpoint = null;
			}
			this._ui.WelcomePanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0012769C File Offset: 0x0012589C
		public void WelcomePanel_No()
		{
			this._AbortWelcomePanel();
			this._user = null;
			this._DisplayDoYouHaveAnAccountPanel();
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x001276B4 File Offset: 0x001258B4
		public void WelcomePanel_Yes()
		{
			if (!this._ui.WelcomePanel.AreRequirementsMet)
			{
				return;
			}
			this._ui.WelcomePanel.SwitchWaitingPanelMode(true, 1);
			string email = this._ui.WelcomePanel.Email;
			this._searchByEmailEndpoint = new SearchByEmailEndpoint(email, Extras.None, null);
			this._searchByEmailEndpoint.Execute(delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				this._ui.WelcomePanel.SwitchWaitingPanelMode(false, -1);
				if (webError == null)
				{
					if (result.TotalElement > 0)
					{
						this._WelcomePanelEmailFound(result.Elements[0]);
					}
					else
					{
						this._WelcomePanelContinueAccountCreation();
					}
				}
				else
				{
					this._lastError = webError.ToString();
					this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
					{
						this._DisplayWelcomePanel(false);
					}, SSOOKPanel.MessageType.Error);
				}
				this._searchByEmailEndpoint = null;
			});
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0012771C File Offset: 0x0012591C
		private void _WelcomePanelEmailFound(UserSearchResult user)
		{
			this._user = user;
			this._ui.LoginPanel.Text = this._ui.WelcomePanel.Email;
			this._DisplayPasswordPanel();
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0012774B File Offset: 0x0012594B
		private void _WelcomePanelContinueAccountCreation()
		{
			this._DisplayChooseALoginNamePanel();
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x00127753 File Offset: 0x00125953
		private void _DisplayChooseALoginNamePanel()
		{
			this._HideAllPanels();
			this._ui.ChooseALoginNamePanel.Show();
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0012776C File Offset: 0x0012596C
		private void _AbortChooseALoginNamePanel()
		{
			if (this._userSignUpEndpoint != null)
			{
				this._userSignUpEndpoint.Abort();
				this._userSignUpEndpoint = null;
			}
			if (this._searchByLoginEndpoint != null)
			{
				this._searchByLoginEndpoint.Abort();
				this._searchByLoginEndpoint = null;
			}
			this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x001277BF File Offset: 0x001259BF
		public void ChooseALoginNamePanel_No()
		{
			this._AbortChooseALoginNamePanel();
			this._DisplayWelcomePanel(true);
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x001277D0 File Offset: 0x001259D0
		public void ChooseALoginNamePanel_Yes()
		{
			if (!this._ui.ChooseALoginNamePanel.AreRequirementsMet)
			{
				return;
			}
			try
			{
				this._userSignUpEndpoint = new UserSignUpEndpoint(this._ui.ChooseALoginNamePanel.LoginName, this._ui.ChooseALoginNamePanel.Password, this._ui.WelcomePanel.Email, this._ui.WelcomePanel.SubscribeToNewsletter, null);
			}
			catch (Exception ex)
			{
				this._userSignUpEndpoint = null;
				this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), ex.Message, delegate
				{
					this._DisplayChooseALoginNamePanel();
				}, SSOOKPanel.MessageType.Error);
				return;
			}
			this._userSignUpEndpoint.Execute(delegate(ApiSignUpResponse endpoint, WebError webError)
			{
				if (webError == null)
				{
					this._SSOAuthenticate(this._ui.ChooseALoginNamePanel.LoginName, this._ui.ChooseALoginNamePanel.Password, delegate(OAuthError error)
					{
						this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(false, -1);
						if (error == null)
						{
							this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.Message"), delegate
							{
								AnalyticsEvents.LogCreateAsmodeeNetAccountEvent(this._ui.WelcomePanel.SubscribeToNewsletter, null);
								AnalyticsEvents.LogConnectAsmodeeNetStopEvent(true, this._lastError, this._didResetPassword, null);
								this._onSSOSucceed();
							}, SSOOKPanel.MessageType.Standard);
							return;
						}
						this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.LoginErrorMessage"), delegate
						{
							this._DisplayDoYouHaveAnAccountPanel();
						}, SSOOKPanel.MessageType.Standard);
					});
				}
				else
				{
					this._lastError = webError.ToString();
					this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(false, -1);
					if (webError.ToChildError<ApiResponseError>() != null)
					{
						this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnexpectedSignUpError"), delegate
						{
							this._DisplayWelcomePanel(true);
						}, SSOOKPanel.MessageType.Error);
					}
					else
					{
						this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
						{
							this._DisplayChooseALoginNamePanel();
						}, SSOOKPanel.MessageType.Error);
					}
				}
				this._userSignUpEndpoint = null;
			});
			this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(true, 1);
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x001278B0 File Offset: 0x00125AB0
		public void ChooseALoginNamePanel_OnLoginNameInputChange(string value)
		{
			if (value.Length < 4)
			{
				this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Unavailable);
				return;
			}
			this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Searching);
			base.StopAllCoroutines();
			base.StartCoroutine(this._ChooseALoginNamePanel_OnLoginNameInputChange(value, this._delayBetweenChecks));
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x00127903 File Offset: 0x00125B03
		private IEnumerator _ChooseALoginNamePanel_OnLoginNameInputChange(string value, float delay)
		{
			yield return new WaitForSeconds(delay);
			try
			{
				this._searchByLoginEndpoint = new SearchByLoginEndpoint(value, Extras.None, -1, -1, null);
			}
			catch (Exception ex)
			{
				this._searchByLoginEndpoint = null;
				this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), ex.Message, delegate
				{
					this._DisplayChooseALoginNamePanel();
				}, SSOOKPanel.MessageType.Error);
				yield break;
			}
			this._searchByLoginEndpoint.Execute(delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				if (webError == null)
				{
					if (result.TotalElement > 0)
					{
						this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Unavailable);
					}
					else
					{
						this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Available);
					}
				}
				else
				{
					this._lastError = webError.ToString();
					this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
					{
						this._DisplayChooseALoginNamePanel();
					}, SSOOKPanel.MessageType.Error);
				}
				this._searchByLoginEndpoint = null;
			});
			yield break;
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x00127920 File Offset: 0x00125B20
		public void ChooseALoginNamePanel_OnPasswordInputChange(string value)
		{
			this._ui.ChooseALoginNamePanel.ShowPasswordStrength(Zxcvbn.MatchPassword(value, null).Score);
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0012793E File Offset: 0x00125B3E
		private void _DisplayOkPanel(string title, string message, Action onHideAction, SSOOKPanel.MessageType messageType = SSOOKPanel.MessageType.Standard)
		{
			this._HideAllPanels();
			this._ui.OkPanel.Show(title, message, onHideAction, messageType);
		}

		// Token: 0x0400265F RID: 9823
		private const string _kModuleName = "SSOManager";

		// Token: 0x04002660 RID: 9824
		[SerializeField]
		private SSOManager.UI _ui;

		// Token: 0x04002661 RID: 9825
		private UserSearchResult _user;

		// Token: 0x04002662 RID: 9826
		private bool _didResetPassword;

		// Token: 0x04002663 RID: 9827
		private string _lastError;

		// Token: 0x04002664 RID: 9828
		private SSOAuthenticate _SSOAuthenticate;

		// Token: 0x04002665 RID: 9829
		private OnSSOSucceeded _onSSOSucceed;

		// Token: 0x04002666 RID: 9830
		private AbortSSO _abortSSO;

		// Token: 0x04002667 RID: 9831
		private EndpointWithPaginatedResponse<UserSearchResult> _searchByLoginOrEmailEndpoint;

		// Token: 0x04002668 RID: 9832
		private ResetPasswordEndpoint _resetPasswordEndpoint;

		// Token: 0x04002669 RID: 9833
		private SearchByEmailEndpoint _searchByEmailEndpoint;

		// Token: 0x0400266A RID: 9834
		private UserSignUpEndpoint _userSignUpEndpoint;

		// Token: 0x0400266B RID: 9835
		private SearchByLoginEndpoint _searchByLoginEndpoint;

		// Token: 0x0400266C RID: 9836
		private float _delayBetweenChecks = 0.25f;

		// Token: 0x02000953 RID: 2387
		[Serializable]
		public class UI
		{
			// Token: 0x0400315A RID: 12634
			public SSOBasePanel DoYouHaveAnAccountPanel;

			// Token: 0x0400315B RID: 12635
			public SSOLoginAndPasswordPanel LoginPanel;

			// Token: 0x0400315C RID: 12636
			public SSOLoginAndPasswordPanel PasswordPanel;

			// Token: 0x0400315D RID: 12637
			public SSOChooseALoginNamePanel ChooseALoginNamePanel;

			// Token: 0x0400315E RID: 12638
			public SSOWelcomePanel WelcomePanel;

			// Token: 0x0400315F RID: 12639
			public SSOOKPanel OkPanel;
		}
	}
}
