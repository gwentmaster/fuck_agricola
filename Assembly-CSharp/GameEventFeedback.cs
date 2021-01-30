using System;

// Token: 0x020000D3 RID: 211
public class GameEventFeedback
{
	// Token: 0x060007E1 RID: 2017 RVA: 0x0003836F File Offset: 0x0003656F
	public void Reset()
	{
		this.bBreakFromUpdateLoop = false;
	}

	// Token: 0x040008E0 RID: 2272
	public bool bBreakFromUpdateLoop;
}
