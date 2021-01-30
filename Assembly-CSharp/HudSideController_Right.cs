using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class HudSideController_Right : HudSideController_Base
{
	// Token: 0x06000527 RID: 1319 RVA: 0x00027E8D File Offset: 0x0002608D
	public void SetEndTurnButtonVisible(bool bVisible)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_EndTurnAnimatorVar, bVisible);
		}
		this.UpdateButtonVisibility(false);
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00027EB6 File Offset: 0x000260B6
	public void SetPriceTrayButtonVisible(bool bVisible)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_PriceTrayAnimatorVar, bVisible);
		}
		this.UpdateButtonVisibility(false);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00027EDF File Offset: 0x000260DF
	public void SetConfirmButtonVisible(bool bVisible)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_ConfirmAnimatorVar, bVisible);
		}
		this.UpdateButtonVisibility(false);
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00027F08 File Offset: 0x00026108
	public void SetDoneButtonVisible(bool bVisible)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_DoneAnimatorVar, bVisible);
		}
		this.UpdateButtonVisibility(false);
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00027F31 File Offset: 0x00026131
	public void SetNextGameButtonVisible(bool bVisible)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_NextGameAnimatorVar, bVisible);
		}
		this.UpdateButtonVisibility(false);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00027F5C File Offset: 0x0002615C
	public override void HandleAllButtonsOff()
	{
		if (this.m_Button_EndTurn != null)
		{
			this.m_Button_EndTurn.SetActive(false);
		}
		if (this.m_Button_PriceTray != null)
		{
			this.m_Button_PriceTray.SetActive(false);
		}
		if (this.m_Button_Confirm != null)
		{
			this.m_Button_Confirm.SetActive(false);
		}
		if (this.m_Button_Done != null)
		{
			this.m_Button_Done.SetActive(false);
		}
		if (this.m_Button_NextGame != null)
		{
			this.m_Button_NextGame.SetActive(false);
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00027FEC File Offset: 0x000261EC
	public override void UpdateButtonVisibility(bool bForceState)
	{
		if (this.m_animator != null)
		{
			if (bForceState)
			{
				if (this.m_Button_EndTurn != null)
				{
					this.m_Button_EndTurn.SetActive(this.m_animator.GetBool(this.m_EndTurnAnimatorVar));
				}
				if (this.m_Button_PriceTray != null)
				{
					this.m_Button_PriceTray.SetActive(this.m_animator.GetBool(this.m_PriceTrayAnimatorVar));
				}
				if (this.m_Button_Confirm != null)
				{
					this.m_Button_Confirm.SetActive(this.m_animator.GetBool(this.m_ConfirmAnimatorVar));
				}
				if (this.m_Button_Done != null)
				{
					this.m_Button_Done.SetActive(this.m_animator.GetBool(this.m_DoneAnimatorVar));
				}
				if (this.m_Button_NextGame != null)
				{
					this.m_Button_NextGame.SetActive(this.m_animator.GetBool(this.m_NextGameAnimatorVar));
					return;
				}
			}
			else if (this.m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Open") && !this.m_animator.IsInTransition(0))
			{
				if (this.m_Button_EndTurn != null)
				{
					this.m_Button_EndTurn.SetActive(this.m_animator.GetBool(this.m_EndTurnAnimatorVar));
				}
				if (this.m_Button_PriceTray != null)
				{
					this.m_Button_PriceTray.SetActive(this.m_animator.GetBool(this.m_PriceTrayAnimatorVar));
				}
				if (this.m_Button_Confirm != null)
				{
					this.m_Button_Confirm.SetActive(this.m_animator.GetBool(this.m_ConfirmAnimatorVar));
				}
				if (this.m_Button_Done != null)
				{
					this.m_Button_Done.SetActive(this.m_animator.GetBool(this.m_DoneAnimatorVar));
				}
				if (this.m_Button_NextGame != null)
				{
					this.m_Button_NextGame.SetActive(this.m_animator.GetBool(this.m_NextGameAnimatorVar));
					return;
				}
			}
			else
			{
				if (this.m_Button_EndTurn != null)
				{
					this.m_Button_EndTurn.SetActive(this.m_Button_EndTurn.activeSelf || this.m_animator.GetBool(this.m_EndTurnAnimatorVar));
				}
				if (this.m_Button_PriceTray != null)
				{
					this.m_Button_PriceTray.SetActive(this.m_Button_PriceTray.activeSelf || this.m_animator.GetBool(this.m_PriceTrayAnimatorVar));
				}
				if (this.m_Button_Confirm != null)
				{
					this.m_Button_Confirm.SetActive(this.m_Button_Confirm.activeSelf || this.m_animator.GetBool(this.m_ConfirmAnimatorVar));
				}
				if (this.m_Button_Done != null)
				{
					this.m_Button_Done.SetActive(this.m_Button_Done.activeSelf || this.m_animator.GetBool(this.m_DoneAnimatorVar));
				}
				if (this.m_Button_NextGame != null)
				{
					this.m_Button_NextGame.SetActive(this.m_Button_NextGame.activeSelf || this.m_animator.GetBool(this.m_NextGameAnimatorVar));
				}
			}
		}
	}

	// Token: 0x040004D3 RID: 1235
	[SerializeField]
	private GameObject m_Button_EndTurn;

	// Token: 0x040004D4 RID: 1236
	[SerializeField]
	private GameObject m_Button_PriceTray;

	// Token: 0x040004D5 RID: 1237
	[SerializeField]
	private GameObject m_Button_Confirm;

	// Token: 0x040004D6 RID: 1238
	[SerializeField]
	private GameObject m_Button_Done;

	// Token: 0x040004D7 RID: 1239
	[SerializeField]
	private GameObject m_Button_NextGame;

	// Token: 0x040004D8 RID: 1240
	[SerializeField]
	private string m_EndTurnAnimatorVar;

	// Token: 0x040004D9 RID: 1241
	[SerializeField]
	private string m_PriceTrayAnimatorVar;

	// Token: 0x040004DA RID: 1242
	[SerializeField]
	private string m_ConfirmAnimatorVar;

	// Token: 0x040004DB RID: 1243
	[SerializeField]
	private string m_DoneAnimatorVar;

	// Token: 0x040004DC RID: 1244
	[SerializeField]
	private string m_NextGameAnimatorVar;
}
