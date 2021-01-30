using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200062B RID: 1579
	public class LoginBannerContentController : MonoBehaviour
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x001211A8 File Offset: 0x0011F3A8
		// (set) Token: 0x06003A27 RID: 14887 RVA: 0x001211B0 File Offset: 0x0011F3B0
		public bool AllowAutoCollapse
		{
			get
			{
				return this._allowAutoCollapse;
			}
			set
			{
				this._allowAutoCollapse = value;
				this._StopCollapseTimer();
				this._UpdateTargetTransform();
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x001211C5 File Offset: 0x0011F3C5
		// (set) Token: 0x06003A29 RID: 14889 RVA: 0x001211D0 File Offset: 0x0011F3D0
		private LoginBannerContentController.State _State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state == value)
				{
					return;
				}
				this._state = value;
				switch (this._state)
				{
				default:
					this._DisplayState = LoginBannerContentController.DisplayState.Partial;
					return;
				case LoginBannerContentController.State.NotAuthenticated:
					this._DisplayState = LoginBannerContentController.DisplayState.Full;
					return;
				case LoginBannerContentController.State.Authenticating:
					if (this._DisplayState == LoginBannerContentController.DisplayState.Full || this._DisplayState == LoginBannerContentController.DisplayState.TemporaryFull)
					{
						this._DisplayState = LoginBannerContentController.DisplayState.TemporaryFull;
						return;
					}
					this._DisplayState = LoginBannerContentController.DisplayState.Partial;
					return;
				case LoginBannerContentController.State.Authenticated:
					this._DisplayState = LoginBannerContentController.DisplayState.TemporaryFull;
					return;
				}
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x00121246 File Offset: 0x0011F446
		// (set) Token: 0x06003A2B RID: 14891 RVA: 0x0012124E File Offset: 0x0011F44E
		private LoginBannerContentController.DisplayState _DisplayState
		{
			get
			{
				return this._displayState;
			}
			set
			{
				if (this._displayState == value)
				{
					return;
				}
				this._displayState = value;
				this._StopCollapseTimer();
				this._UpdateTargetTransform();
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06003A2C RID: 14892 RVA: 0x00121270 File Offset: 0x0011F470
		// (remove) Token: 0x06003A2D RID: 14893 RVA: 0x001212A8 File Offset: 0x0011F4A8
		public event Action LoginBannerDidSelectAccount;

		// Token: 0x06003A2E RID: 14894 RVA: 0x001212DD File Offset: 0x0011F4DD
		public void OnLoginButtonClicked()
		{
			this._getTokenCallbackId = CoreApplication.Instance.OAuthGate.GetPrivateAccessToken(false, delegate(OAuthError error)
			{
				this._getTokenCallbackId = null;
				if (error != null)
				{
					this._State = LoginBannerContentController.State.Error;
				}
			});
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x00121301 File Offset: 0x0011F501
		public void OnAccountButtonClicked()
		{
			if (this.LoginBannerDidSelectAccount != null)
			{
				this.LoginBannerDidSelectAccount();
			}
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x00121316 File Offset: 0x0011F516
		public void OnBannerClicked()
		{
			if (this._DisplayState == LoginBannerContentController.DisplayState.Partial)
			{
				this._DisplayState = LoginBannerContentController.DisplayState.TemporaryFull;
				return;
			}
			if (this._State == LoginBannerContentController.State.NotAuthenticated)
			{
				this.OnLoginButtonClicked();
				return;
			}
			if (this._State == LoginBannerContentController.State.Authenticated)
			{
				this.OnAccountButtonClicked();
			}
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x00121348 File Offset: 0x0011F548
		private void OnEnable()
		{
			this._StopCollapseTimer();
			this._UpdateTargetTransform();
			this.contentTransform.anchoredPosition = this._targetTransform.anchoredPosition;
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x0012136C File Offset: 0x0011F56C
		private void OnDisable()
		{
			if (this._avatarRetrievalHandle != null)
			{
				this._avatarRetrievalHandle.Abort();
				this._avatarRetrievalHandle = null;
			}
			if (this._getTokenCallbackId != null)
			{
				CoreApplication.Instance.OAuthGate.CancelAccessTokenRequest(this._getTokenCallbackId.Value);
				this._getTokenCallbackId = null;
			}
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x001213C8 File Offset: 0x0011F5C8
		private void Update()
		{
			if (this._collapseTimeRunning)
			{
				this._collapseTimer -= Time.deltaTime;
				if (this._collapseTimer <= 0f)
				{
					this._StopCollapseTimer();
					this._DisplayState = LoginBannerContentController.DisplayState.Partial;
				}
			}
			LoginBannerContentController.State state;
			if (!CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				state = LoginBannerContentController.State.Error;
			}
			else if (CoreApplication.Instance.OAuthGate.HasPrivateToken)
			{
				if (CoreApplication.Instance.OAuthGate.UserDetails == null)
				{
					state = LoginBannerContentController.State.Authenticating;
				}
				else
				{
					state = LoginBannerContentController.State.Authenticated;
				}
			}
			else if (CoreApplication.Instance.OAuthGate.IsRetrievingPrivateToken)
			{
				state = LoginBannerContentController.State.Authenticating;
			}
			else
			{
				state = LoginBannerContentController.State.NotAuthenticated;
			}
			bool flag = this._State != state;
			this._State = state;
			this._UpdateIndicator();
			this._UpdateButton();
			this._UpdatePosition();
			if (flag && this._State == LoginBannerContentController.State.Authenticated)
			{
				int userId = CoreApplication.Instance.OAuthGate.UserDetails.UserId;
				this._avatarRetrievalHandle = CoreApplication.Instance.AvatarManager.LoadPlayerAvatar(userId, this.avatarImage, delegate(bool succeeded)
				{
					this._avatarRetrievalHandle = null;
				});
			}
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x001214D0 File Offset: 0x0011F6D0
		private void _UpdateIndicator()
		{
			switch (this._State)
			{
			default:
				this.networkErrorIndicator.SetActive(true);
				this.notAuthenticatedAvatarGroup.SetActive(false);
				this.authenticatingAvatarGroup.SetActive(false);
				this.authenticatedAvatarGroup.SetActive(false);
				return;
			case LoginBannerContentController.State.NotAuthenticated:
				this.networkErrorIndicator.SetActive(false);
				this.notAuthenticatedAvatarGroup.SetActive(true);
				this.authenticatingAvatarGroup.SetActive(false);
				this.authenticatedAvatarGroup.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticating:
				this.networkErrorIndicator.SetActive(false);
				this.notAuthenticatedAvatarGroup.SetActive(false);
				this.authenticatingAvatarGroup.SetActive(true);
				this.authenticatedAvatarGroup.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticated:
				if (this._avatarRetrievalHandle != null)
				{
					this.networkErrorIndicator.SetActive(false);
					this.notAuthenticatedAvatarGroup.SetActive(false);
					this.authenticatingAvatarGroup.SetActive(true);
					this.authenticatedAvatarGroup.SetActive(false);
					return;
				}
				this.networkErrorIndicator.SetActive(false);
				this.notAuthenticatedAvatarGroup.SetActive(false);
				this.authenticatingAvatarGroup.SetActive(false);
				this.authenticatedAvatarGroup.SetActive(true);
				return;
			}
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x001215F8 File Offset: 0x0011F7F8
		private void _UpdateButton()
		{
			switch (this._State)
			{
			default:
				this.loginButton.gameObject.SetActive(false);
				this.accountButton.gameObject.SetActive(false);
				return;
			case LoginBannerContentController.State.NotAuthenticated:
				this.loginButton.gameObject.SetActive(true);
				this.loginButton.interactable = true;
				this.accountButton.gameObject.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticating:
				this.loginButton.gameObject.SetActive(true);
				this.loginButton.interactable = false;
				this.accountButton.gameObject.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticated:
				this.accountButtonText.text = CoreApplication.Instance.OAuthGate.UserDetails.LoginName;
				this.loginButton.gameObject.SetActive(false);
				this.accountButton.gameObject.SetActive(true);
				return;
			}
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x001216E6 File Offset: 0x0011F8E6
		private void _UpdatePosition()
		{
			this.contentTransform.anchoredPosition = Vector2.Lerp(this.contentTransform.anchoredPosition, this._targetTransform.anchoredPosition, Time.deltaTime * 10f);
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x0012171C File Offset: 0x0011F91C
		private void _UpdateTargetTransform()
		{
			switch (this._DisplayState)
			{
			default:
				this._targetTransform = this.hiddenTransformRef;
				return;
			case LoginBannerContentController.DisplayState.Partial:
				this._targetTransform = this.collapsedTransformRef;
				return;
			case LoginBannerContentController.DisplayState.Full:
				this._targetTransform = this.expandedTransformRef;
				return;
			case LoginBannerContentController.DisplayState.TemporaryFull:
				this._targetTransform = this.expandedTransformRef;
				if (this._allowAutoCollapse)
				{
					this._StartCollapseTimer();
				}
				return;
			}
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x00121787 File Offset: 0x0011F987
		private void _StartCollapseTimer()
		{
			this._collapseTimeRunning = true;
			this._collapseTimer = 4f;
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x0012179B File Offset: 0x0011F99B
		private void _StopCollapseTimer()
		{
			this._collapseTimeRunning = false;
			this._collapseTimer = 0f;
		}

		// Token: 0x04002590 RID: 9616
		private const string _documentation = "<b>LoginBannerContentController</b> Allows a user to check it's authentication status to the <b>RestAPI</b>.";

		// Token: 0x04002591 RID: 9617
		[Header("Position References")]
		public RectTransform contentTransform;

		// Token: 0x04002592 RID: 9618
		private RectTransform _targetTransform;

		// Token: 0x04002593 RID: 9619
		[Tooltip("Fully visible")]
		public RectTransform expandedTransformRef;

		// Token: 0x04002594 RID: 9620
		[Tooltip("Only indicators")]
		public RectTransform collapsedTransformRef;

		// Token: 0x04002595 RID: 9621
		[Tooltip("Nothing visible")]
		public RectTransform hiddenTransformRef;

		// Token: 0x04002596 RID: 9622
		private bool _allowAutoCollapse = true;

		// Token: 0x04002597 RID: 9623
		[Header("Indicators")]
		[Tooltip("Displayed when the network is not reachable")]
		public GameObject networkErrorIndicator;

		// Token: 0x04002598 RID: 9624
		[Tooltip("Displayed when the user is not authenticated")]
		public GameObject notAuthenticatedAvatarGroup;

		// Token: 0x04002599 RID: 9625
		[Tooltip("Displayed when the user is authenticating")]
		public GameObject authenticatingAvatarGroup;

		// Token: 0x0400259A RID: 9626
		[Tooltip("Displayed when the user is authenticated and the avatar has been retrieved")]
		public GameObject authenticatedAvatarGroup;

		// Token: 0x0400259B RID: 9627
		public Image avatarImage;

		// Token: 0x0400259C RID: 9628
		[Header("Buttons")]
		[Tooltip("Displayed when the user is not authenticated")]
		public Button loginButton;

		// Token: 0x0400259D RID: 9629
		[Tooltip("Displayed when the user is authenticated")]
		public Button accountButton;

		// Token: 0x0400259E RID: 9630
		public TextMeshProUGUI accountButtonText;

		// Token: 0x0400259F RID: 9631
		private LoginBannerContentController.State _state;

		// Token: 0x040025A0 RID: 9632
		private LoginBannerContentController.DisplayState _displayState;

		// Token: 0x040025A2 RID: 9634
		private const float _collapseAnimationSmoothing = 10f;

		// Token: 0x040025A3 RID: 9635
		private const float _collapseTimerDuration = 4f;

		// Token: 0x040025A4 RID: 9636
		private float _collapseTimer;

		// Token: 0x040025A5 RID: 9637
		private bool _collapseTimeRunning;

		// Token: 0x040025A6 RID: 9638
		private AsmodeeNet.Utils.AvatarManager.RetrievalHandle _avatarRetrievalHandle;

		// Token: 0x040025A7 RID: 9639
		protected int? _getTokenCallbackId;

		// Token: 0x02000922 RID: 2338
		private enum State
		{
			// Token: 0x040030AB RID: 12459
			Unknown,
			// Token: 0x040030AC RID: 12460
			Error,
			// Token: 0x040030AD RID: 12461
			NotAuthenticated,
			// Token: 0x040030AE RID: 12462
			Authenticating,
			// Token: 0x040030AF RID: 12463
			Authenticated
		}

		// Token: 0x02000923 RID: 2339
		private enum DisplayState
		{
			// Token: 0x040030B1 RID: 12465
			Hidden,
			// Token: 0x040030B2 RID: 12466
			Partial,
			// Token: 0x040030B3 RID: 12467
			Full,
			// Token: 0x040030B4 RID: 12468
			TemporaryFull
		}
	}
}
