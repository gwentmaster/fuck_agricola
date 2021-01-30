using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000670 RID: 1648
	[Serializable]
	public class ActionState
	{
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06003CA9 RID: 15529 RVA: 0x0012BB2C File Offset: 0x00129D2C
		// (set) Token: 0x06003CAA RID: 15530 RVA: 0x0012BB34 File Offset: 0x00129D34
		public string Name { get; set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06003CAB RID: 15531 RVA: 0x0012BB3D File Offset: 0x00129D3D
		// (set) Token: 0x06003CAC RID: 15532 RVA: 0x0012BB45 File Offset: 0x00129D45
		public Action ActionEnter { get; set; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06003CAD RID: 15533 RVA: 0x0012BB4E File Offset: 0x00129D4E
		// (set) Token: 0x06003CAE RID: 15534 RVA: 0x0012BB56 File Offset: 0x00129D56
		public Action ActionUpdate { get; set; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06003CAF RID: 15535 RVA: 0x0012BB5F File Offset: 0x00129D5F
		// (set) Token: 0x06003CB0 RID: 15536 RVA: 0x0012BB67 File Offset: 0x00129D67
		public Action ActionExit { get; set; }

		// Token: 0x06003CB1 RID: 15537 RVA: 0x0012BB70 File Offset: 0x00129D70
		public ActionState(string name, Action actionEnter, Action actionUpdate, Action actionExit)
		{
			this.Name = name;
			this.ActionEnter = actionEnter;
			this.ActionUpdate = actionUpdate;
			this.ActionExit = actionExit;
		}
	}
}
