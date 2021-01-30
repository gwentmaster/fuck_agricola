using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class TutorialSteps1
{
	// Token: 0x040005F8 RID: 1528
	private const string LOCALPLAYER_NAME = "Player";

	// Token: 0x040005F9 RID: 1529
	private const string TEXTBEGIN_INTERNALNOTE = "<color=#ff00ffff><i>";

	// Token: 0x040005FA RID: 1530
	private const string TEXTEND_INTERNALNOTE = "</i></color>";

	// Token: 0x040005FB RID: 1531
	private const string TEXTBEGIN_COMPLETEDNOTE = "<color=#00ff00ff><i>";

	// Token: 0x040005FC RID: 1532
	public static TutorialStep[] m_TutorialSteps = new TutorialStep[]
	{
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_000_a}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_000_f}", "${Key_Tutorial_Prompt_Tap_Continue}", 5, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_000_e}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(825f, 44f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - 3 Wood", "${Key_Tutorial_Text_001}", "${Key_Tutorial_Prompt_Assign_Worker}", 6, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 7, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 8, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapCalloutPositionAtBuilding, 7, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_001_a}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 6, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopupHelpButton, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_002}", "${Key_Tutorial_Prompt_Question_Mark}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 9, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopupHelpButton, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_002_a}", "${Key_Tutorial_Prompt_Question_Mark}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 9, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Text_002_b}", "${Key_Tutorial_Prompt_End_Turn}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(95f, -98f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - Day Laborer", "${Key_Tutorial_Text_004}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 10, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapCalloutPositionAtBuilding, 6, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40963, 0, 0U, 4, "Take 1 Food, 1 Wood", "${Key_Tutorial_Text_005}", "${Key_Tutorial_Prompt_Choose_Confirm}", 3, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 11, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_006_a}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(830f, -59f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - 1 Reed", "${Key_Tutorial_Text_007}", "${Key_Tutorial_Prompt_Assign_Worker}", 7, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 10, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapCalloutPositionAtBuilding, 9, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Text_008}", "${Key_Tutorial_Prompt_End_Turn}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 4, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 12, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 13, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - 3 Wood", "${Key_Tutorial_Text_009}", "${Key_Tutorial_Prompt_Assign_Worker}", 7, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 7, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapCalloutPositionAtBuilding, 7, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - Build Room(s) and/or Build Stable(s)", "${Key_Tutorial_Text_010}", "${Key_Tutorial_Prompt_Assign_Worker}", 6, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.VisibleElement, 7, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapCalloutPositionAtBuilding, 1, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40963, 0, 0U, 4, "Build rooms", "${Key_Tutorial_Text_011}", "${Key_Tutorial_Prompt_Choose_Confirm}", 3, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 1, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(-500f, 26.3f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40976, 0, 11U, 1, "Player", "${Key_Tutorial_Text_012}", "${Key_Tutorial_Prompt_Build_Room}", 4, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 8, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 41026, 36864, 0U, 0, "", "${Key_Tutorial_Text_013}", "${Key_Tutorial_Prompt_Choose_Confirm}", 3, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40963, 0, 0U, 4, "Build Stables for 2 Wood", "${Key_Tutorial_Text_014}", "${Key_Tutorial_Prompt_Choose_Confirm}", 3, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 1, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(-1160f, -13.5f), 0.3f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40980, 0, 4U, 1, "Player", "${Key_Tutorial_Text_015}", "${Key_Tutorial_Prompt_Build_Stable}", 4, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 8, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(735f, -0.5f), 0.3f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - Starting Player and Storehouse", "${Key_Tutorial_Text_016}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "APF - 1 Clay", "${Key_Tutorial_Text_017}", "${Key_Tutorial_Prompt_Assign_Worker}", 6, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(-954f, 26f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "S1 - 1 Major Improvement", "${Key_Tutorial_Text_018}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 1, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(-531.5f, 31.6f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 33554432U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_019}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 14, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40982, 0, 12U, 7, "MJ1 - Fireplace", "${Key_Tutorial_Text_019_a}", "${Key_Tutorial_Prompt_Fireplace}", 6, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 14, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Prompt_End_Your_Turn}", "${Key_Tutorial_Prompt_End_Turn}", 15, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_022}", "${Key_Tutorial_Prompt_Tap_Continue}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40994, 0, 0U, 0, "", "${Key_Tutorial_Text_024}", "${Key_Tutorial_Prompt_Green_Checkmark}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_025}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.Wait, 131072U, 0, 0, 0U, 0, "", "", "", 0, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.MapSwap, 0, default(Vector2), 0f),
			new TutorialCallout(TutorialCalloutType.MapAnimate, 0, new Vector2(-1765.5f, 20.7f), 0.5f)
		}),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40962, 0, 0U, 3, "S2 - Family Growth", "${Key_Tutorial_Text_026}", "${Key_Tutorial_Prompt_Assign_Worker}", 4, 0, null),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.UserAction, 0U, 40961, 0, 0U, 0, "", "${Key_Tutorial_Text_027}", "${Key_Tutorial_Prompt_End_Turn}", 4, 0, new TutorialCallout[]
		{
			new TutorialCallout(TutorialCalloutType.VisibleElement, 5, default(Vector2), 0f)
		}),
		new TutorialStep(TutorialStepType.Wait, 32768U, 0, 0, 0U, 0, "", "", "", 0, 0, null),
		new TutorialStep(TutorialStepType.TextPopup, 0U, 0, 0, 0U, 0, "", "${Key_Tutorial_Text_029}", "${Key_Tutorial_Prompt_Tap_Continue}", 1, 0, null),
		new TutorialStep(TutorialStepType.ExitTutorial, 0U, 0, 0, 0U, 0, "", "", "", 0, 0, null)
	};
}
