using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class AnimationEntry
{
	// Token: 0x04000897 RID: 2199
	public AnimateObject m_AnimateObject;

	// Token: 0x04000898 RID: 2200
	public int m_SourceLocatorIndex;

	// Token: 0x04000899 RID: 2201
	public int m_SourceLocatorInstanceID;

	// Token: 0x0400089A RID: 2202
	public int m_DestinationLocatorIndex;

	// Token: 0x0400089B RID: 2203
	public int m_DestinationLocatorInstanceID;

	// Token: 0x0400089C RID: 2204
	public uint m_AnimationFlags;

	// Token: 0x0400089D RID: 2205
	public float m_DelayAtStart;

	// Token: 0x0400089E RID: 2206
	public float m_PauseAtDestination;

	// Token: 0x0400089F RID: 2207
	public Vector3 m_AdditionalRotation;

	// Token: 0x040008A0 RID: 2208
	public bool m_bApplyUpdateAtDestination;
}
