using System;

// Token: 0x0200012D RID: 301
public class TutorialEntry
{
	// Token: 0x06000BA1 RID: 2977 RVA: 0x000520B9 File Offset: 0x000502B9
	public TutorialEntry(int tutorialIndex, ushort tutorialGameType, TutorialStep[] tutorialSteps)
	{
		this.m_TutorialIndex = tutorialIndex;
		this.m_TutorialGameType = tutorialGameType;
		this.m_TutorialSteps = tutorialSteps;
	}

	// Token: 0x04000CA6 RID: 3238
	public int m_TutorialIndex;

	// Token: 0x04000CA7 RID: 3239
	public ushort m_TutorialGameType;

	// Token: 0x04000CA8 RID: 3240
	public TutorialStep[] m_TutorialSteps;
}
