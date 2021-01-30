using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class TutorialCallout
{
	// Token: 0x060005D7 RID: 1495 RVA: 0x0002D110 File Offset: 0x0002B310
	public TutorialCallout(TutorialCalloutType calloutType, int calloutIndex, Vector2 mapPos = default(Vector2), float mapAnimTime = 0f)
	{
		this.m_CalloutType = calloutType;
		this.m_CalloutIndex = calloutIndex;
		this.m_MapPosition = mapPos;
		this.m_mapAnimTime = mapAnimTime;
	}

	// Token: 0x040005C5 RID: 1477
	public TutorialCalloutType m_CalloutType;

	// Token: 0x040005C6 RID: 1478
	public int m_CalloutIndex;

	// Token: 0x040005C7 RID: 1479
	public Vector2 m_MapPosition;

	// Token: 0x040005C8 RID: 1480
	public float m_mapAnimTime;
}
