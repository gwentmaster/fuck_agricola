using System;
using System.Runtime.InteropServices;

// Token: 0x02000077 RID: 119
[StructLayout(LayoutKind.Sequential)]
public class TutorialStep
{
	// Token: 0x060005D8 RID: 1496 RVA: 0x0002D138 File Offset: 0x0002B338
	public TutorialStep(TutorialStepType step_type, uint step_flags, ushort selection_hint, ushort selection_id, uint selectionOptionalData, int selection_instance_type, string selection_instance_name, string tutorialText, string tutorialPrompt, int tutorialPanelLayoutIndex, int autoAnimateRegionMap, TutorialCallout[] callouts = null)
	{
		this.m_StepType = step_type;
		this.m_StepFlags = step_flags;
		this.m_SelectionHint = selection_hint;
		this.m_SelectionID = selection_id;
		this.m_SelectionOptionalData = selectionOptionalData;
		this.m_SelectionInstanceType = selection_instance_type;
		this.m_SelectionInstanceName = selection_instance_name;
		this.m_TutorialText = tutorialText;
		this.m_TutorialPrompt = tutorialPrompt;
		this.m_TutorialPanelLayoutIndex = tutorialPanelLayoutIndex;
		this.m_AutoAnimateRegionMap = autoAnimateRegionMap;
		this.m_Callouts = callouts;
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002D1A8 File Offset: 0x0002B3A8
	public bool IsWaitForAIAction()
	{
		return (this.m_StepFlags & 4096U) > 0U;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0002D1B9 File Offset: 0x0002B3B9
	public ushort GetSelectionID()
	{
		if (this.m_SelectionID != 0)
		{
			return this.m_SelectionID;
		}
		if (this.m_SelectionInstanceType != 0)
		{
			return (ushort)AgricolaLib.GetInstanceID(this.m_SelectionInstanceType, this.m_SelectionInstanceName);
		}
		return 0;
	}

	// Token: 0x040005C9 RID: 1481
	public const uint HideTutorialButtonCheck = 1U;

	// Token: 0x040005CA RID: 1482
	public const uint ShowButtonUndo = 2U;

	// Token: 0x040005CB RID: 1483
	public const uint ShowButtonEndTurn = 4U;

	// Token: 0x040005CC RID: 1484
	public const uint DismissCurrentPopup = 8U;

	// Token: 0x040005CD RID: 1485
	public const uint HidePopupButtonCancel = 16U;

	// Token: 0x040005CE RID: 1486
	public const uint HidePopupButtonCheck = 32U;

	// Token: 0x040005CF RID: 1487
	public const uint WaitForAIAction = 4096U;

	// Token: 0x040005D0 RID: 1488
	public const uint WaitForAnimation = 8192U;

	// Token: 0x040005D1 RID: 1489
	public const uint WaitForEndOfGame = 16384U;

	// Token: 0x040005D2 RID: 1490
	public const uint WaitForAvailableOptions = 32768U;

	// Token: 0x040005D3 RID: 1491
	public const uint WaitForAnimationSync = 65536U;

	// Token: 0x040005D4 RID: 1492
	public const uint WaitForMapAnimation = 131072U;

	// Token: 0x040005D5 RID: 1493
	public const uint WaitForLogButtonPressed = 262144U;

	// Token: 0x040005D6 RID: 1494
	public const uint WaitForLogBackPressed = 524288U;

	// Token: 0x040005D7 RID: 1495
	public const uint MagnifyCard = 1048576U;

	// Token: 0x040005D8 RID: 1496
	public const uint UnmagnifyCard = 2097152U;

	// Token: 0x040005D9 RID: 1497
	public const uint WaitForMagnify = 4194304U;

	// Token: 0x040005DA RID: 1498
	public const uint WaitForUnmagnify = 8388608U;

	// Token: 0x040005DB RID: 1499
	public const uint UserActionSetupOnly = 16777216U;

	// Token: 0x040005DC RID: 1500
	public const uint OpenMajorImpTray = 33554432U;

	// Token: 0x040005DD RID: 1501
	public const uint WaitEndGameStepStart = 67108864U;

	// Token: 0x040005DE RID: 1502
	public const uint WaitEndGameStepEnd = 134217728U;

	// Token: 0x040005DF RID: 1503
	public const uint TextPopupUseSplitText = 268435456U;

	// Token: 0x040005E0 RID: 1504
	public const uint TutorialSortForLog = 2147483648U;

	// Token: 0x040005E1 RID: 1505
	public TutorialStepType m_StepType;

	// Token: 0x040005E2 RID: 1506
	public uint m_StepFlags;

	// Token: 0x040005E3 RID: 1507
	public ushort m_SelectionHint;

	// Token: 0x040005E4 RID: 1508
	public ushort m_SelectionID;

	// Token: 0x040005E5 RID: 1509
	public uint m_SelectionOptionalData;

	// Token: 0x040005E6 RID: 1510
	public int m_SelectionInstanceType;

	// Token: 0x040005E7 RID: 1511
	public string m_SelectionInstanceName;

	// Token: 0x040005E8 RID: 1512
	public string m_TutorialText;

	// Token: 0x040005E9 RID: 1513
	public string m_TutorialPrompt;

	// Token: 0x040005EA RID: 1514
	public int m_TutorialPanelLayoutIndex;

	// Token: 0x040005EB RID: 1515
	public int m_AutoAnimateRegionMap;

	// Token: 0x040005EC RID: 1516
	public TutorialCallout[] m_Callouts;
}
