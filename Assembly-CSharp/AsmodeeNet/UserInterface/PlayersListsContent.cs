using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200062D RID: 1581
	public class PlayersListsContent : CommunityHubContent
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06003A3F RID: 14911 RVA: 0x0000900B File Offset: 0x0000720B
		public override CommunityHubContent.Layout DisplayedLayout
		{
			get
			{
				return CommunityHubContent.Layout.Tab;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06003A40 RID: 14912 RVA: 0x001217EF File Offset: 0x0011F9EF
		public PlayersListsContentController Controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x001217F8 File Offset: 0x0011F9F8
		public override void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
			if (this._root != null || this._controller != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._root = UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.playersListsContentPrefabs[displayMode]);
			this._controller = this._root.GetComponent<PlayersListsContentController>();
			if (this._root == null || this._controller == null)
			{
				AsmoLogger.Debug("PlayersListsContent", "Incomplete PlayersListsContent Prefab", null);
				this.DisposeDisplayedContent();
			}
			base.TabIcons = communityHubSkin.playersListsContentIcons;
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x00121898 File Offset: 0x0011FA98
		public override void DisposeDisplayedContent()
		{
			UnityEngine.Object.Destroy(this._root);
			this._root = null;
			this._controller = null;
		}

		// Token: 0x040025A8 RID: 9640
		private const string _debugModuleName = "PlayersListsContent";

		// Token: 0x040025A9 RID: 9641
		private PlayersListsContentController _controller;
	}
}
