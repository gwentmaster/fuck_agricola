using System;
using System.Collections;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.UserInterface.SSO;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000635 RID: 1589
	public class UserAccountContentController : MonoBehaviour
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06003A70 RID: 14960 RVA: 0x00122604 File Offset: 0x00120804
		// (remove) Token: 0x06003A71 RID: 14961 RVA: 0x0012263C File Offset: 0x0012083C
		public event Action UserAccountDidClose;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06003A72 RID: 14962 RVA: 0x00122674 File Offset: 0x00120874
		// (remove) Token: 0x06003A73 RID: 14963 RVA: 0x001226AC File Offset: 0x001208AC
		public event Action UserDidLogOut;

		// Token: 0x06003A74 RID: 14964 RVA: 0x001226E1 File Offset: 0x001208E1
		private void Start()
		{
			this._updateTitle();
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x001226E9 File Offset: 0x001208E9
		private void OnEnable()
		{
			this._needsLayout = true;
			this.Update();
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x001226F8 File Offset: 0x001208F8
		private void OnDisable()
		{
			if (this._resetPasswordEndpoint != null)
			{
				this._resetPasswordEndpoint.Abort();
				this._resetPasswordEndpoint = null;
			}
			if (this._avatarRetrievalHandle != null)
			{
				this._avatarRetrievalHandle.Abort();
				this._avatarRetrievalHandle = null;
			}
			this._state = UserAccountContentController.State.Unknown;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00122735 File Offset: 0x00120935
		private void Update()
		{
			this._UpdateCloseButton();
			this._UpdateAspect();
			this._UpdateState();
			this._UpdateDisplay();
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x0012274F File Offset: 0x0012094F
		private void _UpdateCloseButton()
		{
			if (this.closeButton != null)
			{
				this.closeButton.gameObject.SetActive(this.shouldDisplayCloseButton);
			}
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x00122778 File Offset: 0x00120978
		private void _UpdateAspect()
		{
			float num = this.container.rect.size.x / this.container.rect.size.y;
			if (!Mathf.Approximately(num, this._containerAspect))
			{
				if ((this._containerAspect >= 1f && num < 1f) || (this._containerAspect < 1f && num >= 1f))
				{
					this._needsLayout = true;
				}
				this._containerAspect = num;
			}
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00122800 File Offset: 0x00120A00
		private void _UpdateState()
		{
			UserAccountContentController.State state;
			if (CoreApplication.Instance.OAuthGate.HasPrivateToken)
			{
				if (CoreApplication.Instance.OAuthGate.UserDetails == null)
				{
					state = UserAccountContentController.State.FetchingUserData;
				}
				else
				{
					state = UserAccountContentController.State.ValidUserData;
				}
			}
			else
			{
				state = UserAccountContentController.State.NotAuthenticated;
			}
			bool flag = this._state != state;
			this._state = state;
			if (flag && this._state == UserAccountContentController.State.ValidUserData)
			{
				OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
				User userDetails = oauthGate.UserDetails;
				bool flag2 = userDetails.EmailValid ?? false;
				if (this.nameLabel != null)
				{
					this.nameLabel.text = userDetails.Name;
				}
				if (this.loginNameLabel != null)
				{
					this.loginNameLabel.text = userDetails.LoginName;
				}
				if (this.userIdentifierLabel != null)
				{
					this.userIdentifierLabel.text = "# " + userDetails.UserId.ToString();
				}
				if (this.emailLabel != null)
				{
					this.emailLabel.text = userDetails.Email;
				}
				if (this.coppaIndicator != null)
				{
					this.coppaIndicator.SetActive(userDetails.Coppa ?? false);
				}
				if (this.registeredLabel != null)
				{
					this.registeredLabel.text = ((userDetails.JoinDate != null) ? userDetails.JoinDate.Value.ToShortDateString() : "?");
				}
				AsmodeeNet.Foundation.SteamManager steamManager = oauthGate.SteamManager;
				if (steamManager == null || !steamManager.HasClient)
				{
					if (this.steamContainer != null)
					{
						this.steamContainer.SetActive(false);
					}
				}
				else
				{
					if (this.steamContainer != null)
					{
						this.steamContainer.SetActive(true);
					}
					PartnerAccount steamMe = steamManager.Me;
					if (this.steamIdentifierLabel != null)
					{
						this.steamIdentifierLabel.text = "Steam #" + steamMe.PartnerUser;
					}
					if (this.steamWarningStatus != null)
					{
						bool active = userDetails.Partners.FirstOrDefault((PartnerAccount p) => p.Equals(steamMe)) == null;
						this.steamWarningStatus.SetActive(active);
					}
				}
				if (this.getMyPasswordButton != null)
				{
					this.getMyPasswordButton.gameObject.SetActive(flag2);
				}
				if (this.getMyPasswordResultLabel != null)
				{
					this.getMyPasswordResultLabel.gameObject.SetActive(false);
				}
				if (this.updateMyEmailButton != null)
				{
					this.updateMyEmailButton.gameObject.SetActive(!flag2);
				}
				int userId = CoreApplication.Instance.OAuthGate.UserDetails.UserId;
				this._avatarRetrievalHandle = CoreApplication.Instance.AvatarManager.LoadPlayerAvatar(userId, this.avatarImage, delegate(bool succeeded)
				{
					this._avatarRetrievalHandle = null;
				});
				FontSizeHomogenizer[] array = this.fontSizeHomogenizers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetNeedsFontSizeUpdate();
				}
			}
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00122B24 File Offset: 0x00120D24
		private void _UpdateDisplay()
		{
			switch (this._state)
			{
			default:
				this.content.SetActive(false);
				this.activityIndicator.SetActive(false);
				this.errorIndicator.SetActive(false);
				this.notAuthenticatedIndicator.SetActive(true);
				return;
			case UserAccountContentController.State.FetchingUserData:
				this.content.SetActive(false);
				this.activityIndicator.SetActive(true);
				this.errorIndicator.SetActive(false);
				this.notAuthenticatedIndicator.SetActive(false);
				return;
			case UserAccountContentController.State.ValidUserData:
				this.content.SetActive(true);
				this.activityIndicator.SetActive(false);
				this.errorIndicator.SetActive(false);
				this.notAuthenticatedIndicator.SetActive(false);
				if (this._needsLayout)
				{
					this._needsLayout = false;
					base.StartCoroutine(this._UpdateLayout());
					return;
				}
				break;
			case UserAccountContentController.State.Error:
				this.content.SetActive(false);
				this.activityIndicator.SetActive(false);
				this.errorIndicator.SetActive(true);
				this.notAuthenticatedIndicator.SetActive(false);
				break;
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00122C33 File Offset: 0x00120E33
		private IEnumerator _UpdateLayout()
		{
			this.container.gameObject.SetActive(false);
			VerticalLayoutGroup[] components = this.container.gameObject.GetComponents<VerticalLayoutGroup>();
			for (int i = 0; i < components.Length; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
			HorizontalLayoutGroup[] components2 = this.container.gameObject.GetComponents<HorizontalLayoutGroup>();
			for (int i = 0; i < components2.Length; i++)
			{
				UnityEngine.Object.Destroy(components2[i]);
			}
			yield return new WaitForEndOfFrame();
			if (this._containerAspect >= 1f)
			{
				HorizontalLayoutGroup horizontalLayoutGroup = this.container.gameObject.AddComponent<HorizontalLayoutGroup>();
				horizontalLayoutGroup.childForceExpandWidth = true;
				horizontalLayoutGroup.childForceExpandHeight = false;
				horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			}
			else
			{
				VerticalLayoutGroup verticalLayoutGroup = this.container.gameObject.AddComponent<VerticalLayoutGroup>();
				verticalLayoutGroup.childForceExpandWidth = true;
				verticalLayoutGroup.childForceExpandHeight = false;
				verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			}
			this.container.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00122C42 File Offset: 0x00120E42
		public void OnCloseButtonClicked()
		{
			if (this.UserAccountDidClose != null)
			{
				this.UserAccountDidClose();
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x00122C57 File Offset: 0x00120E57
		public void OnToggleSDKVersion()
		{
			this._displaySDKVersion = !this._displaySDKVersion;
			this._updateTitle();
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00122C6E File Offset: 0x00120E6E
		private void _updateTitle()
		{
			if (this.title != null)
			{
				this.title.text = (this._displaySDKVersion ? ("Version " + SDKVersionManager.Version()) : "Asmodee.net");
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x00122CA7 File Offset: 0x00120EA7
		public void OnLogoutButtonClicked()
		{
			CoreApplication.Instance.OAuthGate.LogOut();
			if (this.UserDidLogOut != null)
			{
				this.UserDidLogOut();
			}
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x00122CCB File Offset: 0x00120ECB
		public void OnEditMyAccountButtonClicked()
		{
			Application.OpenURL("https://account.asmodee.net/profile");
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x00122CD7 File Offset: 0x00120ED7
		public void OnGetMyPasswordButtonClicked()
		{
			if (this._resetPasswordEndpoint == null)
			{
				this.getMyPasswordButton.interactable = false;
				this._resetPasswordEndpoint = new ResetPasswordEndpoint(null);
				this._resetPasswordEndpoint.Execute(delegate(WebError webError)
				{
					this.getMyPasswordButton.interactable = true;
					if (this.getMyPasswordResultLabel != null)
					{
						if (webError == null)
						{
							this.getMyPasswordResultLabel.text = "We sent you an e-mail. Please follow the instructions";
						}
						else
						{
							this.getMyPasswordResultLabel.text = "An error occured, please try again later";
						}
						this.getMyPasswordResultLabel.gameObject.SetActive(true);
						this.getMyPasswordButton.gameObject.SetActive(false);
					}
					this._resetPasswordEndpoint = null;
				});
			}
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x00122D10 File Offset: 0x00120F10
		public void OnUpdateMyEmailButtonClicked()
		{
			SSOManager.InstantiateUpdateEmailPopUp(null);
		}

		// Token: 0x040025C7 RID: 9671
		private const string _documentation = "<b>UserAccountContentController</b> displays account informations to an authenticated user. It also allows basic actions: logout, password rest, email update...";

		// Token: 0x040025C8 RID: 9672
		public RectTransform container;

		// Token: 0x040025C9 RID: 9673
		public bool shouldDisplayCloseButton = true;

		// Token: 0x040025CA RID: 9674
		public GameObject content;

		// Token: 0x040025CB RID: 9675
		public GameObject activityIndicator;

		// Token: 0x040025CC RID: 9676
		public GameObject errorIndicator;

		// Token: 0x040025CD RID: 9677
		public GameObject notAuthenticatedIndicator;

		// Token: 0x040025CE RID: 9678
		[Header("User Informations")]
		public TextMeshProUGUI title;

		// Token: 0x040025CF RID: 9679
		public Image avatarImage;

		// Token: 0x040025D0 RID: 9680
		public TextMeshProUGUI nameLabel;

		// Token: 0x040025D1 RID: 9681
		public TextMeshProUGUI loginNameLabel;

		// Token: 0x040025D2 RID: 9682
		public TextMeshProUGUI userIdentifierLabel;

		// Token: 0x040025D3 RID: 9683
		public TextMeshProUGUI emailLabel;

		// Token: 0x040025D4 RID: 9684
		public TextMeshProUGUI registeredLabel;

		// Token: 0x040025D5 RID: 9685
		public GameObject steamContainer;

		// Token: 0x040025D6 RID: 9686
		public TextMeshProUGUI steamIdentifierLabel;

		// Token: 0x040025D7 RID: 9687
		public GameObject steamWarningStatus;

		// Token: 0x040025D8 RID: 9688
		public GameObject coppaIndicator;

		// Token: 0x040025DB RID: 9691
		public FontSizeHomogenizer[] fontSizeHomogenizers;

		// Token: 0x040025DC RID: 9692
		[Header("Actions")]
		public Button closeButton;

		// Token: 0x040025DD RID: 9693
		public Button logoutButton;

		// Token: 0x040025DE RID: 9694
		public Button getMyPasswordButton;

		// Token: 0x040025DF RID: 9695
		public TextMeshProUGUI getMyPasswordResultLabel;

		// Token: 0x040025E0 RID: 9696
		public Button editMyProfileButton;

		// Token: 0x040025E1 RID: 9697
		public Button updateMyEmailButton;

		// Token: 0x040025E2 RID: 9698
		private float _containerAspect;

		// Token: 0x040025E3 RID: 9699
		private bool _needsLayout;

		// Token: 0x040025E4 RID: 9700
		private ResetPasswordEndpoint _resetPasswordEndpoint;

		// Token: 0x040025E5 RID: 9701
		private AsmodeeNet.Utils.AvatarManager.RetrievalHandle _avatarRetrievalHandle;

		// Token: 0x040025E6 RID: 9702
		private UserAccountContentController.State _state;

		// Token: 0x040025E7 RID: 9703
		private bool _displaySDKVersion;

		// Token: 0x02000928 RID: 2344
		private enum State
		{
			// Token: 0x040030C2 RID: 12482
			Unknown,
			// Token: 0x040030C3 RID: 12483
			NotAuthenticated,
			// Token: 0x040030C4 RID: 12484
			Authenticating,
			// Token: 0x040030C5 RID: 12485
			FetchingUserData,
			// Token: 0x040030C6 RID: 12486
			ValidUserData,
			// Token: 0x040030C7 RID: 12487
			Error
		}
	}
}
