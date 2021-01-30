using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000621 RID: 1569
	public abstract class CommunityHubContent
	{
		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060039CD RID: 14797
		public abstract CommunityHubContent.Layout DisplayedLayout { get; }

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual CommunityHubContent.ActivityType BackgroundActivity
		{
			get
			{
				return CommunityHubContent.ActivityType.Inactive;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060039CF RID: 14799 RVA: 0x0011F103 File Offset: 0x0011D303
		public RectTransform DisplayedContent
		{
			get
			{
				if (!(this._root != null))
				{
					return null;
				}
				return this._root.transform as RectTransform;
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Start()
		{
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Stop()
		{
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void DisposeDisplayedContent()
		{
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060039D4 RID: 14804 RVA: 0x0011F125 File Offset: 0x0011D325
		// (set) Token: 0x060039D5 RID: 14805 RVA: 0x0011F12D File Offset: 0x0011D32D
		public CommunityHubSkin.TabIcons TabIcons { get; protected set; }

		// Token: 0x04002554 RID: 9556
		protected GameObject _root;

		// Token: 0x0200091A RID: 2330
		public enum Layout
		{
			// Token: 0x04003090 RID: 12432
			Header,
			// Token: 0x04003091 RID: 12433
			Tab
		}

		// Token: 0x0200091B RID: 2331
		public enum ActivityType
		{
			// Token: 0x04003093 RID: 12435
			Active,
			// Token: 0x04003094 RID: 12436
			Inactive
		}
	}
}
