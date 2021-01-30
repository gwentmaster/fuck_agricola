using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000624 RID: 1572
	[CreateAssetMenu]
	public class CommunityHubSkin : ScriptableObject
	{
		// Token: 0x04002576 RID: 9590
		[Header("Community Hub")]
		[SerializeField]
		public CommunityHubSkin.PerDisplayModeGameObject expandCollapseButtonPrefabs;

		// Token: 0x04002577 RID: 9591
		public Sprite collapseButtonInHorizontalTabBarIcon;

		// Token: 0x04002578 RID: 9592
		public Sprite expandButtonInHorizontalTabBarIcon;

		// Token: 0x04002579 RID: 9593
		public Sprite collapseButtonInVerticalTabBarIcon;

		// Token: 0x0400257A RID: 9594
		public Sprite expandButtonInVerticalTabBarIcon;

		// Token: 0x0400257B RID: 9595
		public CommunityHubSkin.PerDisplayModeGameObject tabBarPrefabs;

		// Token: 0x0400257C RID: 9596
		public CommunityHubSkin.PerDisplayModeGameObject tabPrefabs;

		// Token: 0x0400257D RID: 9597
		[Header("Login Banner Content")]
		public CommunityHubSkin.PerDisplayModeGameObject loginBannerContentPrefabs;

		// Token: 0x0400257E RID: 9598
		[Header("User Account Content")]
		public CommunityHubSkin.PerDisplayModeGameObject userAccountContentPrefabs;

		// Token: 0x0400257F RID: 9599
		public CommunityHubSkin.TabIcons userAccountContentIcons;

		// Token: 0x04002580 RID: 9600
		[Header("Players Lists Content")]
		public CommunityHubSkin.PerDisplayModeGameObject playersListsContentPrefabs;

		// Token: 0x04002581 RID: 9601
		public CommunityHubSkin.TabIcons playersListsContentIcons;

		// Token: 0x0200091F RID: 2335
		[Serializable]
		public class TabIcons
		{
			// Token: 0x040030A1 RID: 12449
			public Sprite spriteOff;

			// Token: 0x040030A2 RID: 12450
			public Sprite spriteOn;
		}

		// Token: 0x02000920 RID: 2336
		[Serializable]
		public class PerDisplayModeGameObject
		{
			// Token: 0x17000A1D RID: 2589
			public GameObject this[Preferences.DisplayMode displayMode]
			{
				get
				{
					GameObject gameObject = null;
					switch (displayMode)
					{
					case Preferences.DisplayMode.Small:
						gameObject = this.small;
						break;
					case Preferences.DisplayMode.Regular:
						gameObject = this.regular;
						break;
					case Preferences.DisplayMode.Big:
						gameObject = this.big;
						break;
					}
					if (!(gameObject != null))
					{
						return this.generic;
					}
					return gameObject;
				}
			}

			// Token: 0x040030A3 RID: 12451
			public GameObject generic;

			// Token: 0x040030A4 RID: 12452
			public GameObject small;

			// Token: 0x040030A5 RID: 12453
			public GameObject regular;

			// Token: 0x040030A6 RID: 12454
			public GameObject big;
		}
	}
}
