using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class HudSideController_Left : HudSideController_Base
{
	// Token: 0x06000522 RID: 1314 RVA: 0x00027D0D File Offset: 0x00025F0D
	public void SetUndoButtonVisible(bool bVisible)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_UndoAnimatorVar, bVisible);
		}
		this.UpdateButtonVisibility(false);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00027D36 File Offset: 0x00025F36
	public void SetCancelButtonVisible(bool bVisible)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_CancelAnimatorVar, bVisible);
		}
		this.UpdateButtonVisibility(false);
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00027D5F File Offset: 0x00025F5F
	public override void HandleAllButtonsOff()
	{
		if (this.m_Button_Undo != null)
		{
			this.m_Button_Undo.SetActive(false);
		}
		if (this.m_Button_Cancel != null)
		{
			this.m_Button_Cancel.SetActive(false);
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00027D98 File Offset: 0x00025F98
	public override void UpdateButtonVisibility(bool bForceState)
	{
		if (this.m_animator != null)
		{
			if (bForceState)
			{
				if (this.m_Button_Undo != null)
				{
					this.m_Button_Undo.SetActive(this.m_animator.GetBool(this.m_UndoAnimatorVar));
				}
				if (this.m_Button_Cancel != null)
				{
					this.m_Button_Cancel.SetActive(this.m_animator.GetBool(this.m_CancelAnimatorVar));
					return;
				}
			}
			else
			{
				if (this.m_Button_Undo != null)
				{
					this.m_Button_Undo.SetActive(this.m_Button_Undo.activeSelf || this.m_animator.GetBool(this.m_UndoAnimatorVar));
				}
				if (this.m_Button_Cancel != null)
				{
					this.m_Button_Cancel.SetActive(this.m_Button_Cancel.activeSelf || this.m_animator.GetBool(this.m_CancelAnimatorVar));
				}
			}
		}
	}

	// Token: 0x040004CF RID: 1231
	[SerializeField]
	private GameObject m_Button_Undo;

	// Token: 0x040004D0 RID: 1232
	[SerializeField]
	private GameObject m_Button_Cancel;

	// Token: 0x040004D1 RID: 1233
	[SerializeField]
	private string m_UndoAnimatorVar;

	// Token: 0x040004D2 RID: 1234
	[SerializeField]
	private string m_CancelAnimatorVar;
}
