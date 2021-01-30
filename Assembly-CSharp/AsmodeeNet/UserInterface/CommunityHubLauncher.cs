using System;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000622 RID: 1570
	public class CommunityHubLauncher : MonoBehaviour
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060039D7 RID: 14807 RVA: 0x0011F136 File Offset: 0x0011D336
		public CommunityHub CommunityHub
		{
			get
			{
				return this._communityHub;
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060039D8 RID: 14808 RVA: 0x0011F140 File Offset: 0x0011D340
		// (remove) Token: 0x060039D9 RID: 14809 RVA: 0x0011F178 File Offset: 0x0011D378
		public event Action communityHubDidStart;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060039DA RID: 14810 RVA: 0x0011F1B0 File Offset: 0x0011D3B0
		// (remove) Token: 0x060039DB RID: 14811 RVA: 0x0011F1E8 File Offset: 0x0011D3E8
		public event Action communityHubDidStop;

		// Token: 0x060039DC RID: 14812 RVA: 0x0011F21D File Offset: 0x0011D41D
		private void Start()
		{
			if (this.autoLaunch)
			{
				this.LaunchCommunityHub();
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x0011F22D File Offset: 0x0011D42D
		public bool IsCommunityHubLaunched
		{
			get
			{
				return this._communityHub != null;
			}
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x0011F23C File Offset: 0x0011D43C
		public void LaunchCommunityHub()
		{
			if (this.IsCommunityHubLaunched)
			{
				return;
			}
			AsmoLogger.Debug("CommunityHubLauncher", "Launch Community Hub", null);
			if (this.communityHubSkin == null)
			{
				AsmoLogger.Warning("CommunityHubLauncher", "Skin not provided -> Fall back to default", null);
				this.communityHubSkin = (UnityEngine.Object.Instantiate(Resources.Load("CommunityHubDefaultSkin", typeof(CommunityHubSkin))) as CommunityHubSkin);
			}
			this._communityHub = base.gameObject.AddComponent<CommunityHub>();
			this._communityHub.CommunityHubLayout.HandleSafeArea = this.handleSafeArea;
			if (this.communityHubDidStart != null)
			{
				this.communityHubDidStart();
			}
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x0011F2DE File Offset: 0x0011D4DE
		public void StopCommunityHub()
		{
			if (!this.IsCommunityHubLaunched)
			{
				return;
			}
			AsmoLogger.Debug("CommunityHubLauncher", "Stop Community Hub", null);
			UnityEngine.Object.Destroy(this._communityHub);
			this._communityHub = null;
			if (this.communityHubDidStop != null)
			{
				this.communityHubDidStop();
			}
		}

		// Token: 0x04002556 RID: 9558
		private const string _documentation = "<b>Community Hub</b> offers standard UI elements:\n- SSO\n- Players lists\n- Chat";

		// Token: 0x04002557 RID: 9559
		private const string _consoleModuleName = "CommunityHubLauncher";

		// Token: 0x04002558 RID: 9560
		private CommunityHub _communityHub;

		// Token: 0x0400255B RID: 9563
		public CommunityHubSkin communityHubSkin;

		// Token: 0x0400255C RID: 9564
		public bool autoLaunch;

		// Token: 0x0400255D RID: 9565
		public bool handleSafeArea = true;
	}
}
