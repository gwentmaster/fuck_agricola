using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000632 RID: 1586
	public struct TabBarItem
	{
		// Token: 0x06003A55 RID: 14933 RVA: 0x00121BAB File Offset: 0x0011FDAB
		public TabBarItem(int tag, Tab tab, RectTransform transform)
		{
			this.tag = tag;
			this.tab = tab;
			this.transform = transform;
		}

		// Token: 0x040025B4 RID: 9652
		public int tag;

		// Token: 0x040025B5 RID: 9653
		public Tab tab;

		// Token: 0x040025B6 RID: 9654
		public RectTransform transform;
	}
}
