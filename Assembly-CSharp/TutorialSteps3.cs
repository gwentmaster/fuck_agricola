using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class TutorialSteps3
{
	// Token: 0x04000602 RID: 1538
	private const string LOCALPLAYER_NAME = "Player";

	// Token: 0x04000603 RID: 1539
	private const string TEXTBEGIN_INTERNALNOTE = "<color=#ff00ffff><i>";

	// Token: 0x04000604 RID: 1540
	private const string TEXTEND_INTERNALNOTE = "</i></color>";

	// Token: 0x04000605 RID: 1541
	private const string TEXTBEGIN_COMPLETEDNOTE = "<color=#00ff00ff><i>";

	// Token: 0x04000606 RID: 1542
	public static TutorialStep[] m_TutorialSteps = new TutorialStep[]
	{
		new TutorialStep(TutorialStepType.SetupScript, 0U, 0, 2, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_039}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_040}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 40992, 0, 0U, 3, "Convert 1 Sheep to 2 Food", "${Key_Tutorial_Text_041}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 4, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 15, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40992, 0, 0U, 5, "Convert 1 Sheep to 2 Food", "${Key_Tutorial_Text_042}", "${Key_Tutorial_Prompt_Sheep}", 6, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 1, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40992, 0, 0U, 5, "Convert 1 Sheep to 2 Food", "${Key_Tutorial_Text_042}", "${Key_Tutorial_Prompt_Sheep1}", 6, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopupTownButton, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_043}", "${Key_Tutorial_Prompt_Town}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 2, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - Plow 1 Field", "${Key_Tutorial_Text_044}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40977, 0, 12U, 1, "Player", "${Key_Tutorial_Text_045}", "${Key_Tutorial_Prompt_Plow}", 7, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - Take 1 Grain", "${Key_Tutorial_Text_046}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "S1 - Sow and/or Bake Bread", "${Key_Tutorial_Text_049}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40963, 449, 0U, 4, "Sow", "", "", 15, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40978, 0, 0U, 0, "", "${Key_Tutorial_Text_050}", "${Key_Tutorial_Sow_Grain}", 6, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40989, 0, 0U, 0, "", "", "", 15, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40994, 0, 0U, 0, "", "${Key_Tutorial_Text_051}", "${Key_Tutorial_Prompt_Green_Checkmark}", 7, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40995, 0, 0U, 0, "", "${Key_Tutorial_Text_052}", "${Key_Tutorial_Prompt_Green_Checkmark}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_053}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.ExitTutorial, 0U, 0, 0, 0U, 0, "", "", "", 0, 0, null)
	};
}
