using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000184 RID: 388
	public interface IBoxSelectable
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000EF7 RID: 3831
		// (set) Token: 0x06000EF8 RID: 3832
		bool selected { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000EF9 RID: 3833
		// (set) Token: 0x06000EFA RID: 3834
		bool preSelected { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000EFB RID: 3835
		Transform transform { get; }
	}
}
