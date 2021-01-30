using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000634 RID: 1588
	public class UserAccountContent : CommunityHubContent
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06003A65 RID: 14949 RVA: 0x0000900B File Offset: 0x0000720B
		public override CommunityHubContent.Layout DisplayedLayout
		{
			get
			{
				return CommunityHubContent.Layout.Tab;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x001223E1 File Offset: 0x001205E1
		public UserAccountContentController Controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06003A67 RID: 14951 RVA: 0x001223EC File Offset: 0x001205EC
		// (remove) Token: 0x06003A68 RID: 14952 RVA: 0x00122424 File Offset: 0x00120624
		public event Action UserAccountDidClose;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06003A69 RID: 14953 RVA: 0x0012245C File Offset: 0x0012065C
		// (remove) Token: 0x06003A6A RID: 14954 RVA: 0x00122494 File Offset: 0x00120694
		public event Action UserDidLogOut;

		// Token: 0x06003A6B RID: 14955 RVA: 0x001224CC File Offset: 0x001206CC
		public override void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
			if (this._root != null || this._controller != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._root = UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.userAccountContentPrefabs[displayMode]);
			this._controller = this._root.GetComponent<UserAccountContentController>();
			if (this._root == null || this._controller == null)
			{
				AsmoLogger.Debug("UserAccountContent", "Incomplete UserAccountContent Prefab", null);
				this.DisposeDisplayedContent();
			}
			else
			{
				this._controller.shouldDisplayCloseButton = this.shouldDisplayCloseButton;
				this._controller.UserAccountDidClose += this._UserAccountDidClose;
				this._controller.UserDidLogOut += this._UserDidLogOut;
			}
			base.TabIcons = communityHubSkin.userAccountContentIcons;
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x001225AD File Offset: 0x001207AD
		public override void DisposeDisplayedContent()
		{
			UnityEngine.Object.Destroy(this._root);
			this._root = null;
			this._controller = null;
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x001225C8 File Offset: 0x001207C8
		private void _UserAccountDidClose()
		{
			if (this.UserAccountDidClose != null)
			{
				this.UserAccountDidClose();
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x001225DD File Offset: 0x001207DD
		private void _UserDidLogOut()
		{
			if (this.UserDidLogOut != null)
			{
				this.UserDidLogOut();
			}
		}

		// Token: 0x040025C2 RID: 9666
		private const string _debugModuleName = "UserAccountContent";

		// Token: 0x040025C3 RID: 9667
		private UserAccountContentController _controller;

		// Token: 0x040025C6 RID: 9670
		public bool shouldDisplayCloseButton = true;
	}
}
