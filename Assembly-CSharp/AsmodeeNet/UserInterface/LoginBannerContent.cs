using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200062A RID: 1578
	public class LoginBannerContent : CommunityHubContent
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06003A1C RID: 14876 RVA: 0x0002A062 File Offset: 0x00028262
		public override CommunityHubContent.Layout DisplayedLayout
		{
			get
			{
				return CommunityHubContent.Layout.Header;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x00121002 File Offset: 0x0011F202
		public LoginBannerContentController Controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06003A1E RID: 14878 RVA: 0x0012100C File Offset: 0x0011F20C
		// (remove) Token: 0x06003A1F RID: 14879 RVA: 0x00121044 File Offset: 0x0011F244
		public event Action LoginBannerDidSelectAccount;

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x00121079 File Offset: 0x0011F279
		// (set) Token: 0x06003A21 RID: 14881 RVA: 0x00121081 File Offset: 0x0011F281
		public bool AllowAutoCollapse
		{
			get
			{
				return this._allowAutoCollapse;
			}
			set
			{
				this._allowAutoCollapse = value;
				if (this._controller != null)
				{
					this._controller.AllowAutoCollapse = this._allowAutoCollapse;
				}
			}
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x001210AC File Offset: 0x0011F2AC
		public override void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
			if (this._root != null || this._controller != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._root = UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.loginBannerContentPrefabs[displayMode]);
			this._controller = this._root.GetComponent<LoginBannerContentController>();
			if (this._root == null || this._controller == null)
			{
				AsmoLogger.Debug("LoginBannerContent", "Incomplete LoginBannerContent Prefab", null);
				this.DisposeDisplayedContent();
				return;
			}
			this._controller.AllowAutoCollapse = this._allowAutoCollapse;
			this._controller.LoginBannerDidSelectAccount += this._LoginBannerDidSelectAccount;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00121169 File Offset: 0x0011F369
		public override void DisposeDisplayedContent()
		{
			UnityEngine.Object.Destroy(this._root);
			this._root = null;
			this._controller = null;
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00121184 File Offset: 0x0011F384
		private void _LoginBannerDidSelectAccount()
		{
			if (this.LoginBannerDidSelectAccount != null)
			{
				this.LoginBannerDidSelectAccount();
			}
		}

		// Token: 0x0400258C RID: 9612
		private const string _debugModuleName = "LoginBannerContent";

		// Token: 0x0400258D RID: 9613
		private LoginBannerContentController _controller;

		// Token: 0x0400258F RID: 9615
		private bool _allowAutoCollapse = true;
	}
}
