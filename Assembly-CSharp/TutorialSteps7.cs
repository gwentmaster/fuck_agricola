using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class TutorialSteps7
{
	// Token: 0x04000616 RID: 1558
	private const string LOCALPLAYER_NAME = "Player";

	// Token: 0x04000617 RID: 1559
	private const string TEXTBEGIN_INTERNALNOTE = "<color=#ff00ffff><i>";

	// Token: 0x04000618 RID: 1560
	private const string TEXTEND_INTERNALNOTE = "</i></color>";

	// Token: 0x04000619 RID: 1561
	private const string TEXTBEGIN_COMPLETEDNOTE = "<color=#00ff00ff><i>";

	// Token: 0x0400061A RID: 1562
	public static TutorialStep[] m_TutorialSteps = new TutorialStep[]
	{
		new TutorialStep(TutorialStepType.SetupScript, 0U, 0, 4, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40995, 0, 0U, 0, "", "${Key_Tutorial_Text_102}", "${Key_Tutorial_Prompt_New_Animals}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 16384U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_103}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_104}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_105}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 268435456U, 0, 0, 0U, 0, "${Key_Tutorial_Text_106_a}", "${Key_Tutorial_Text_106}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_107}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_108}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_109}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_110}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_111}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_112}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_113}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_114}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_115}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_116}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_117}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_118}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_119}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_121}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_120}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_122}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 134217728U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_232}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 67108864U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 16, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_123}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_124}", "${Key_Tutorial_Prompt_Tap_Continue}", 8, 0, null),
		new TutorialStep(TutorialStepType.ExitTutorial, 0U, 0, 0, 0U, 0, "", "", "", 0, 0, null)
	};
}
