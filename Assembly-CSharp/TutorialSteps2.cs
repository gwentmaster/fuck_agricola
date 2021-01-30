using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class TutorialSteps2
{
	// Token: 0x040005FD RID: 1533
	private const string LOCALPLAYER_NAME = "Player";

	// Token: 0x040005FE RID: 1534
	private const string TEXTBEGIN_INTERNALNOTE = "<color=#ff00ffff><i>";

	// Token: 0x040005FF RID: 1535
	private const string TEXTEND_INTERNALNOTE = "</i></color>";

	// Token: 0x04000600 RID: 1536
	private const string TEXTBEGIN_COMPLETEDNOTE = "<color=#00ff00ff><i>";

	// Token: 0x04000601 RID: 1537
	public static TutorialStep[] m_TutorialSteps = new TutorialStep[]
	{
		new TutorialStep(TutorialStepType.SetupScript, 0U, 0, 1, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_030}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(729f, 121f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_030_a}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_030_c}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - 3 Wood", "${Key_Tutorial_Text_030_b}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "S1 - Fences", "${Key_Tutorial_Text_031}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_032}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40981, 0, 16896U, 0, "", "${Key_Tutorial_Text_034}", "${Key_Tutorial_Prompt_Fences}", 4, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40981, 0, 16U, 0, "", "${Key_Tutorial_Text_034}", "${Key_Tutorial_Prompt_Fences}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Text_034_a}", "${Key_Tutorial_Prompt_End_Turn}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "S1 - 1 Sheep", "${Key_Tutorial_Text_035}", "${Key_Tutorial_Prompt_Assign_Worker}", 6, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.SetupAction, 16777216U, 41040, 0, 0U, 0, "", "", "", 6, 0, null),
		new TutorialStep(TutorialStepType.SetupAction, 16777216U, 41041, 0, 0U, 1, "Player", "", "", 6, 0, null),
		new TutorialStep(TutorialStepType.SetupAction, 16777216U, 41041, 0, 16777216U, 1, "Player", "", "", 6, 0, null),
		new TutorialStep(TutorialStepType.SetupAction, 16777216U, 41041, 0, 33554432U, 1, "Player", "", "", 6, 0, null),
		new TutorialStep(TutorialStepType.SetupAction, 16777216U, 41041, 0, 67108868U, 1, "Player", "", "", 6, 0, null),
		new TutorialStep(TutorialStepType.SetupAction, 16777216U, 41041, 0, 83886082U, 1, "Player", "", "", 6, 0, null),
		new TutorialStep(TutorialStepType.SetupAction, 16777216U, 41042, 0, 0U, 1, "Player", "", "", 6, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Text_036}", "${Key_Tutorial_Prompt_End_Turn}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - 1 Reed", "${Key_Tutorial_Text_037}", "${Key_Tutorial_Prompt_Assign_Worker}", 6, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Text_038}", "${Key_Tutorial_Prompt_End_Turn}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.ExitTutorial, 0U, 0, 0, 0U, 0, "", "", "", 0, 0, null)
	};
}
