using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000639 RID: 1593
	[CreateAssetMenu]
	public class InterfaceSkin : ScriptableObject
	{
		// Token: 0x040025F8 RID: 9720
		public GameObject alertControllerPrefab;

		// Token: 0x040025F9 RID: 9721
		[Header("SSO")]
		public GameObject ssoPrefab;

		// Token: 0x040025FA RID: 9722
		public GameObject updateEmailPopUpPrefab;

		// Token: 0x040025FB RID: 9723
		[Header("Cross-Promotion")]
		public GameObject BannerPrefab;

		// Token: 0x040025FC RID: 9724
		public GameObject InterstitialPopupPrefab;

		// Token: 0x040025FD RID: 9725
		public GameObject MoreGamesPopupPrefab;

		// Token: 0x040025FE RID: 9726
		public GameObject GameDetailsPopupPrefab;
	}
}
