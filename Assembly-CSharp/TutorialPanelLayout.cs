using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class TutorialPanelLayout
{
	// Token: 0x060005DB RID: 1499 RVA: 0x0002D1E8 File Offset: 0x0002B3E8
	public TutorialPanelLayout(float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY, float anchoredPositionX, float anchoredPositionY, float sizeDeltaX, float sizeDeltaY)
	{
		this.m_AnchorMin = new Vector2(anchorMinX, anchorMinY);
		this.m_AnchorMax = new Vector2(anchorMaxX, anchorMaxY);
		this.m_AnchoredPosition = new Vector2(anchoredPositionX, anchoredPositionY);
		this.m_SizeDelta = new Vector2(sizeDeltaX, sizeDeltaY);
	}

	// Token: 0x040005ED RID: 1517
	public Vector2 m_AnchorMin;

	// Token: 0x040005EE RID: 1518
	public Vector2 m_AnchorMax;

	// Token: 0x040005EF RID: 1519
	public Vector2 m_AnchoredPosition;

	// Token: 0x040005F0 RID: 1520
	public Vector2 m_SizeDelta;
}
