using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000620 RID: 1568
	public class CommunityHub : MonoBehaviour
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060039A5 RID: 14757 RVA: 0x0011E9BB File Offset: 0x0011CBBB
		// (set) Token: 0x060039A6 RID: 14758 RVA: 0x0011E9C3 File Offset: 0x0011CBC3
		public CommunityHubLayout CommunityHubLayout { get; private set; }

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060039A7 RID: 14759 RVA: 0x0011E9CC File Offset: 0x0011CBCC
		public IList<RectTransform> TransformsToAutoLayout
		{
			get
			{
				return this._transformsToAutoLayout.AsReadOnly();
			}
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x0011E9D9 File Offset: 0x0011CBD9
		public void AddTransformToAutoLayout(RectTransform transform)
		{
			if (!this._transformsToAutoLayout.Contains(transform))
			{
				this._transformsToAutoLayout.Add(transform);
			}
			if (this.CommunityHubLayout != null)
			{
				this.CommunityHubLayout.SetNeedsUpdateLayout();
			}
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x0011EA0E File Offset: 0x0011CC0E
		public void RemoveTransformToAutoLayout(RectTransform transform)
		{
			if (this._transformsToAutoLayout.Contains(transform))
			{
				this._transformsToAutoLayout.Remove(transform);
			}
			if (this.CommunityHubLayout != null)
			{
				this.CommunityHubLayout.SetNeedsUpdateLayout();
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060039AA RID: 14762 RVA: 0x0011EA44 File Offset: 0x0011CC44
		// (remove) Token: 0x060039AB RID: 14763 RVA: 0x0011EA7C File Offset: 0x0011CC7C
		public event Action<Vector2, Vector2> layoutDidChange;

		// Token: 0x060039AC RID: 14764 RVA: 0x0011EAB1 File Offset: 0x0011CCB1
		public void CallLayoutDidChangeEvent()
		{
			if (this.layoutDidChange != null)
			{
				this.layoutDidChange(this.LayoutSafeAreaAnchorMin, this.LayoutSafeAreaAnchorMax);
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x0011EAD2 File Offset: 0x0011CCD2
		public Vector2 LayoutSafeAreaAnchorMin
		{
			get
			{
				if (!(this.CommunityHubLayout != null))
				{
					return Vector2.zero;
				}
				return this.CommunityHubLayout.SafeAreaAnchorMin;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x0011EAF3 File Offset: 0x0011CCF3
		public Vector2 LayoutSafeAreaAnchorMax
		{
			get
			{
				if (!(this.CommunityHubLayout != null))
				{
					return Vector2.one;
				}
				return this.CommunityHubLayout.SafeAreaAnchorMin;
			}
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x0011EB14 File Offset: 0x0011CD14
		private void Awake()
		{
			this._RegisterLiteSDKRequiredOnlineStatus();
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x0011EB1C File Offset: 0x0011CD1C
		private void OnEnable()
		{
			this._AttachInterface();
			if (this.CommunityHubLayout != null)
			{
				this.CommunityHubLayout.SetNeedsUpdateLayout();
			}
			this.Update();
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x0011EB44 File Offset: 0x0011CD44
		private void Update()
		{
			bool isNetworkReachable = this._isNetworkReachable;
			this._isNetworkReachable = WebChecker.IsNetworkReachable;
			if (isNetworkReachable != this._isNetworkReachable && this.networkReachabilityDidChange != null)
			{
				this.networkReachabilityDidChange(this._isNetworkReachable);
			}
			OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
			bool hasPublicScopeToken = this._hasPublicScopeToken;
			this._hasPublicScopeToken = oauthGate.HasPublicToken;
			if (hasPublicScopeToken != this._hasPublicScopeToken && this.publicScopeTokenDidChange != null)
			{
				this.publicScopeTokenDidChange(this._hasPublicScopeToken);
			}
			bool hasPrivateScopeToken = this._hasPrivateScopeToken;
			this._hasPrivateScopeToken = oauthGate.HasPrivateToken;
			if (hasPrivateScopeToken != this._hasPrivateScopeToken && this.privateScopeTokenDidChange != null)
			{
				this.privateScopeTokenDidChange(this._hasPrivateScopeToken);
			}
			User userDetails = oauthGate.UserDetails;
			if (userDetails != this._userDetails)
			{
				this._userDetails = userDetails;
				if (this.UserDetailsDidChange != null)
				{
					this.UserDetailsDidChange(this._userDetails);
				}
				AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
				User userDetails2 = this._userDetails;
				analyticsManager.UserId = ((userDetails2 != null) ? userDetails2.UserId.ToString() : null);
			}
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0011EC4D File Offset: 0x0011CE4D
		private void OnDisable()
		{
			this._DettachInterface();
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x0011EC58 File Offset: 0x0011CE58
		private void _AttachInterface()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("CommunityHubLayout", typeof(GameObject))) as GameObject;
			this.CommunityHubLayout = gameObject.GetComponent<CommunityHubLayout>();
			gameObject.transform.SetParent(base.transform);
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x0011ECA1 File Offset: 0x0011CEA1
		private void _DettachInterface()
		{
			UnityEngine.Object.Destroy(this.CommunityHubLayout.gameObject);
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060039B5 RID: 14773 RVA: 0x0011ECB3 File Offset: 0x0011CEB3
		public bool IsNetworkReachable
		{
			get
			{
				return this._isNetworkReachable;
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060039B6 RID: 14774 RVA: 0x0011ECBC File Offset: 0x0011CEBC
		// (remove) Token: 0x060039B7 RID: 14775 RVA: 0x0011ECF4 File Offset: 0x0011CEF4
		public event Action<bool> networkReachabilityDidChange;

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x0011ED29 File Offset: 0x0011CF29
		public bool HasPublicScopeToken
		{
			get
			{
				return this._hasPublicScopeToken;
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060039B9 RID: 14777 RVA: 0x0011ED34 File Offset: 0x0011CF34
		// (remove) Token: 0x060039BA RID: 14778 RVA: 0x0011ED6C File Offset: 0x0011CF6C
		public event Action<bool> publicScopeTokenDidChange;

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060039BB RID: 14779 RVA: 0x0011EDA1 File Offset: 0x0011CFA1
		public bool HasPrivateScopeToken
		{
			get
			{
				return this._hasPrivateScopeToken;
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060039BC RID: 14780 RVA: 0x0011EDAC File Offset: 0x0011CFAC
		// (remove) Token: 0x060039BD RID: 14781 RVA: 0x0011EDE4 File Offset: 0x0011CFE4
		public event Action<bool> privateScopeTokenDidChange;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060039BE RID: 14782 RVA: 0x0011EE1C File Offset: 0x0011D01C
		// (remove) Token: 0x060039BF RID: 14783 RVA: 0x0011EE54 File Offset: 0x0011D054
		public event Action<User> UserDetailsDidChange;

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x0011EE89 File Offset: 0x0011D089
		public List<Type> RequiredOnlineStatuses
		{
			get
			{
				return new List<Type>(this._requiredOnlineStatuses.Keys);
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060039C1 RID: 14785 RVA: 0x0011EE9B File Offset: 0x0011D09B
		public int RequiredOnlineStatusesCount
		{
			get
			{
				return this._requiredOnlineStatuses.Count;
			}
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x0011EEA8 File Offset: 0x0011D0A8
		public RequiredOnlineStatus RequiredOnlineStatusForType(Type requiredOnlineStatusType)
		{
			return this._requiredOnlineStatuses[requiredOnlineStatusType];
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x0011EEB8 File Offset: 0x0011D0B8
		private void _RegisterLiteSDKRequiredOnlineStatus()
		{
			this._RegisterRequiredOnlineStatus(typeof(NoOnlineConnectionStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionWithPublicScopeTokenStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionWithPrivateScopeTokenOptionalStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionWithPrivateScopeTokenRequiredStatus));
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x0011EF18 File Offset: 0x0011D118
		private void _RegisterRequiredOnlineStatus(Type requiredOnlineStatusType)
		{
			if (!requiredOnlineStatusType.IsSubclassOf(typeof(RequiredOnlineStatus)))
			{
				throw new ArgumentException("Must be a subclass of RequiredOnlineStatus");
			}
			AsmoLogger.Debug("CommunityHub", "Register Online Status: " + requiredOnlineStatusType.Name, null);
			RequiredOnlineStatus value = (RequiredOnlineStatus)Activator.CreateInstance(requiredOnlineStatusType);
			this._requiredOnlineStatuses.Add(requiredOnlineStatusType, value);
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x0011EF76 File Offset: 0x0011D176
		public void RequireOnlineStatus(Type requiredOnlineStatusType, Action onSuccess, Action onFailure)
		{
			AsmoLogger.Debug("CommunityHub", "Require Online Status: " + requiredOnlineStatusType.Name, null);
			this.RequiredOnlineStatusForType(requiredOnlineStatusType).MeetRequirements(onSuccess, onFailure);
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x0011EFA1 File Offset: 0x0011D1A1
		// (set) Token: 0x060039C7 RID: 14791 RVA: 0x0011EFAE File Offset: 0x0011D1AE
		public List<CommunityHubContent> CommunityHubContents
		{
			get
			{
				return new List<CommunityHubContent>(this._communityHubContents);
			}
			set
			{
				this._communityHubContents = value;
				this._UpdateCommunityHubContent();
			}
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x0011EFC0 File Offset: 0x0011D1C0
		public void AddContent(CommunityHubContent content)
		{
			if (!this._communityHubContents.Contains(content))
			{
				this._communityHubContents.Add(content);
				content.Start();
				this._UpdateCommunityHubContent();
				return;
			}
			AsmoLogger.Warning("CommunityHub", () => "Content already displayed: " + content.ToString(), null);
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x0011F028 File Offset: 0x0011D228
		public void RemoveContent(CommunityHubContent content)
		{
			if (this._communityHubContents.Contains(content))
			{
				content.Stop();
				this._communityHubContents.Remove(content);
				this._UpdateCommunityHubContent();
				return;
			}
			AsmoLogger.Warning("CommunityHub", () => "Can't remove displayed content: " + content.ToString(), null);
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x0011F090 File Offset: 0x0011D290
		public void RemoveAllContent()
		{
			this._communityHubContents.ForEach(delegate(CommunityHubContent content)
			{
				content.Stop();
			});
			this._communityHubContents.Clear();
			this._UpdateCommunityHubContent();
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x0011F0CD File Offset: 0x0011D2CD
		private void _UpdateCommunityHubContent()
		{
			this.CommunityHubLayout.SetNeedsUpdateLayout();
		}

		// Token: 0x04002546 RID: 9542
		private const string _debugModuleName = "CommunityHub";

		// Token: 0x04002548 RID: 9544
		private List<RectTransform> _transformsToAutoLayout = new List<RectTransform>();

		// Token: 0x0400254A RID: 9546
		private bool _isNetworkReachable;

		// Token: 0x0400254C RID: 9548
		private bool _hasPublicScopeToken;

		// Token: 0x0400254E RID: 9550
		private bool _hasPrivateScopeToken;

		// Token: 0x04002550 RID: 9552
		private User _userDetails;

		// Token: 0x04002552 RID: 9554
		private Dictionary<Type, RequiredOnlineStatus> _requiredOnlineStatuses = new Dictionary<Type, RequiredOnlineStatus>();

		// Token: 0x04002553 RID: 9555
		private List<CommunityHubContent> _communityHubContents = new List<CommunityHubContent>();
	}
}
