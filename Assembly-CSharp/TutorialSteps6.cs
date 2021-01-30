using System;

// Token: 0x0200007F RID: 127
public class TutorialSteps6
{
	// Token: 0x04000611 RID: 1553
	private const string LOCALPLAYER_NAME = "Player";

	// Token: 0x04000612 RID: 1554
	private const string TEXTBEGIN_INTERNALNOTE = "<color=#ff00ffff><i>";

	// Token: 0x04000613 RID: 1555
	private const string TEXTEND_INTERNALNOTE = "</i></color>";

	// Token: 0x04000614 RID: 1556
	private const string TEXTBEGIN_COMPLETEDNOTE = "<color=#00ff00ff><i>";

	// Token: 0x04000615 RID: 1557
	public static TutorialStep[] m_TutorialSteps = new TutorialStep[]
	{
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.ExitTutorial, 0U, 0, 0, 0U, 0, "", "", "", 0, 0, null)
	};
}
