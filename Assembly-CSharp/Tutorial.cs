using System;

// Token: 0x02000079 RID: 121
public class Tutorial
{
	// Token: 0x060005DC RID: 1500 RVA: 0x0002D234 File Offset: 0x0002B434
	public bool IsCompleted()
	{
		return this.m_CurrentStep >= this.m_TutorialSteps.Length;
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002D249 File Offset: 0x0002B449
	public int GetCurrentStepNumber()
	{
		if (this.IsCompleted())
		{
			return -1;
		}
		return this.m_CurrentStep;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002D25B File Offset: 0x0002B45B
	public int GetCurrentUserActionStepNumber()
	{
		if (this.IsCompleted())
		{
			return -1;
		}
		return this.m_CurrentStepUserActions;
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0002D26D File Offset: 0x0002B46D
	public TutorialStep GetCurrentStep()
	{
		if (this.IsCompleted())
		{
			return null;
		}
		return this.m_TutorialSteps[this.m_CurrentStep];
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0002D286 File Offset: 0x0002B486
	public TutorialStep GetPreviousStep()
	{
		if (this.IsCompleted() || this.m_CurrentStep < 1)
		{
			return null;
		}
		return this.m_TutorialSteps[this.m_CurrentStep - 1];
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0002D2AC File Offset: 0x0002B4AC
	public string GenerateCurrentStepName()
	{
		TutorialStep currentStep = this.GetCurrentStep();
		if (currentStep != null)
		{
			string text = this.m_CurrentStep.ToString("D3");
			return string.Concat(new string[]
			{
				text,
				"_",
				currentStep.m_StepType.ToString(),
				"_",
				(currentStep.m_SelectionInstanceName != string.Empty) ? currentStep.m_SelectionInstanceName : "None"
			});
		}
		return string.Empty;
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002D334 File Offset: 0x0002B534
	public TutorialPanelLayout GetTutorialPanelLayout()
	{
		if (this.IsCompleted())
		{
			return null;
		}
		int tutorialPanelLayoutIndex = this.m_TutorialSteps[this.m_CurrentStep].m_TutorialPanelLayoutIndex;
		if (tutorialPanelLayoutIndex < 0 || tutorialPanelLayoutIndex >= Tutorial.m_TutorialPanelLayouts.Length)
		{
			return null;
		}
		return Tutorial.m_TutorialPanelLayouts[tutorialPanelLayoutIndex];
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0002D378 File Offset: 0x0002B578
	public void Start()
	{
		this.m_TutorialSteps = Tutorial.m_CurrentTutorialSteps;
		this.m_CurrentStep = 0;
		this.m_CurrentStepUserActions = 0;
		if (this.m_CurrentStep < this.m_TutorialSteps.Length)
		{
			TutorialStep tutorialStep = this.m_TutorialSteps[this.m_CurrentStep];
			if (tutorialStep.m_StepType == TutorialStepType.OpponentAction)
			{
				if (!tutorialStep.IsWaitForAIAction())
				{
					this.Advance();
					return;
				}
			}
			else
			{
				if (tutorialStep.m_StepType == TutorialStepType.SetupAction || tutorialStep.m_StepType == TutorialStepType.SetupScript)
				{
					this.Advance();
					return;
				}
				if (tutorialStep.m_StepType == TutorialStepType.DrawCardFromDeck)
				{
					this.Advance();
					return;
				}
				if (tutorialStep.m_StepType == TutorialStepType.DieRollResult)
				{
					this.Advance();
				}
			}
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0002D40C File Offset: 0x0002B60C
	public void Advance()
	{
		if (this.IsCompleted())
		{
			return;
		}
		TutorialStep tutorialStep;
		do
		{
			this.m_CurrentStep++;
			if (this.m_CurrentStep >= this.m_TutorialSteps.Length)
			{
				break;
			}
			tutorialStep = this.m_TutorialSteps[this.m_CurrentStep];
			if (tutorialStep.m_StepType == TutorialStepType.UserAction)
			{
				this.m_CurrentStepUserActions++;
			}
		}
		while ((tutorialStep.m_StepType == TutorialStepType.OpponentAction && !tutorialStep.IsWaitForAIAction()) || tutorialStep.m_StepType == TutorialStepType.SetupAction || tutorialStep.m_StepType == TutorialStepType.SetupScript || tutorialStep.m_StepType == TutorialStepType.DrawCardFromDeck || tutorialStep.m_StepType == TutorialStepType.DieRollResult);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0002D49C File Offset: 0x0002B69C
	public void Backup()
	{
		if (this.IsCompleted())
		{
			return;
		}
		if (this.m_CurrentStep > 0)
		{
			this.m_CurrentStep--;
		}
		while (this.m_CurrentStep < this.m_TutorialSteps.Length)
		{
			if (this.m_CurrentStep + 1 < this.m_TutorialSteps.Length && this.m_TutorialSteps[this.m_CurrentStep].m_StepType == TutorialStepType.UserAction)
			{
				this.m_CurrentStepUserActions--;
			}
			TutorialStep tutorialStep = this.m_TutorialSteps[this.m_CurrentStep];
			if (tutorialStep.m_StepType == TutorialStepType.OpponentAction)
			{
				if (tutorialStep.IsWaitForAIAction())
				{
					break;
				}
				this.m_CurrentStep++;
			}
			else if (tutorialStep.m_StepType == TutorialStepType.SetupAction || tutorialStep.m_StepType == TutorialStepType.SetupScript)
			{
				this.m_CurrentStep++;
			}
			else if (tutorialStep.m_StepType == TutorialStepType.DrawCardFromDeck)
			{
				this.m_CurrentStep++;
			}
			else
			{
				if (tutorialStep.m_StepType != TutorialStepType.DieRollResult)
				{
					break;
				}
				this.m_CurrentStep++;
			}
		}
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0002D5A0 File Offset: 0x0002B7A0
	public void UpdateGameOptions()
	{
		if (this.IsCompleted())
		{
			GameOptions.UnhideAllOptions();
			return;
		}
		TutorialStep tutorialStep = this.m_TutorialSteps[this.m_CurrentStep];
		if (tutorialStep.m_StepType == TutorialStepType.UserAction)
		{
			GameOptions.HideAllOtherOptions(tutorialStep.m_SelectionHint, tutorialStep.GetSelectionID());
			return;
		}
		if (tutorialStep.m_StepType == TutorialStepType.Wait && (tutorialStep.m_StepFlags & 16384U) != 0U)
		{
			GameOptions.UnhideAllOptions();
			return;
		}
		GameOptions.HideAllOptions();
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002D608 File Offset: 0x0002B808
	public static string GetTutorialName(int tutorialIndex)
	{
		switch (tutorialIndex)
		{
		case 1:
			return "Intro New Players";
		case 2:
			return "Animals";
		case 3:
			return "Cooking";
		case 4:
			return "Renovations";
		case 5:
			return "Intro Adv Players";
		case 6:
			return "Basic Game";
		case 7:
			return "Scoring";
		default:
			return string.Empty;
		}
	}

	// Token: 0x040005F1 RID: 1521
	public static TutorialStep[] m_CurrentTutorialSteps = null;

	// Token: 0x040005F2 RID: 1522
	public static int s_CurrentTutorialIndex = 0;

	// Token: 0x040005F3 RID: 1523
	public static TutorialPanelLayout[] m_TutorialPanelLayouts = new TutorialPanelLayout[]
	{
		new TutorialPanelLayout(0f, 1f, 0f, 1f, 204f, -180f, 400f, 280f),
		new TutorialPanelLayout(0.5f, 0.5f, 0.5f, 0.5f, 0f, 0f, 900f, 500f),
		new TutorialPanelLayout(1f, 0.5f, 1f, 0.5f, -500f, 0f, 700f, 420f),
		new TutorialPanelLayout(0.5f, 0f, 0.5f, 0f, 0f, 240f, 900f, 320f),
		new TutorialPanelLayout(0f, 0.5f, 0f, 0.5f, 500f, 0f, 600f, 420f),
		new TutorialPanelLayout(0.5f, 0.5f, 0.5f, 0.5f, 0f, 0f, 700f, 400f),
		new TutorialPanelLayout(1f, 0.5f, 1f, 0.5f, -500f, 48f, 720f, 512f),
		new TutorialPanelLayout(0.5f, 1f, 0.5f, 1f, 0f, -240f, 900f, 320f),
		new TutorialPanelLayout(0.5f, 0f, 0.5f, 0f, 0f, 500f, 900f, 500f),
		new TutorialPanelLayout(0.5f, 0f, 0.5f, 0f, 0f, 170f, 900f, 280f),
		new TutorialPanelLayout(0f, 0.5f, 0f, 0.5f, 360f, 0f, 480f, 420f),
		new TutorialPanelLayout(1f, 0.5f, 1f, 0.5f, -420f, 256f, 600f, 442f),
		new TutorialPanelLayout(0.5f, 0.5f, 0.5f, 0.5f, 0f, -80f, 900f, 400f),
		new TutorialPanelLayout(1f, 0.5f, 1f, 0.5f, -370f, 0f, 500f, 400f),
		new TutorialPanelLayout(0.5f, 1f, 0.5f, 1f, 0f, -300f, 900f, 360f),
		new TutorialPanelLayout(1f, 0.5f, 1f, 0.5f, -500f, -2000f, 720f, 512f)
	};

	// Token: 0x040005F4 RID: 1524
	public static string[] m_TutorialCalloutCountryArrowNames = new string[]
	{
		"Arrow_N",
		"Arrow_NE",
		"Arrow_E",
		"Arrow_SE",
		"Arrow_S",
		"Arrow_SW",
		"Arrow_W",
		"Arrow_NW"
	};

	// Token: 0x040005F5 RID: 1525
	public TutorialStep[] m_TutorialSteps;

	// Token: 0x040005F6 RID: 1526
	private int m_CurrentStep;

	// Token: 0x040005F7 RID: 1527
	private int m_CurrentStepUserActions;
}
