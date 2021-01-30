using System;

// Token: 0x0200007E RID: 126
public class TutorialSteps5
{
	// Token: 0x0400060C RID: 1548
	private const string LOCALPLAYER_NAME = "Player";

	// Token: 0x0400060D RID: 1549
	private const string TEXTBEGIN_INTERNALNOTE = "<color=#ff00ffff><i>";

	// Token: 0x0400060E RID: 1550
	private const string TEXTEND_INTERNALNOTE = "</i></color>";

	// Token: 0x0400060F RID: 1551
	private const string TEXTBEGIN_COMPLETEDNOTE = "<color=#00ff00ff><i>";

	// Token: 0x04000610 RID: 1552
	public static TutorialStep[] m_TutorialSteps = new TutorialStep[]
	{
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.ExitTutorial, 0U, 0, 0, 0U, 0, "", "", "", 0, 0, null)
	};
}
