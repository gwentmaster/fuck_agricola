using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class RoundAnnounceAnimEvent : MonoBehaviour
{
	// Token: 0x060005C4 RID: 1476 RVA: 0x0002CA85 File Offset: 0x0002AC85
	public void Done()
	{
		if (this.m_parent != null)
		{
			this.m_parent.Done();
		}
	}

	// Token: 0x0400059A RID: 1434
	public RoundAnnouncements m_parent;
}
