using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class CorfirmPopup : PopupBase
{
	// Token: 0x060004A7 RID: 1191 RVA: 0x00024A89 File Offset: 0x00022C89
	public override void OnActionButtonCancel()
	{
		this.m_PopupManager.SetPopup(EPopups.NONE);
		if (this.m_gameController != null)
		{
			this.m_gameController.UpdateGameOptionsSelectionState(true);
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00024AB1 File Offset: 0x00022CB1
	public override void OnActionButtonCommit()
	{
		if (this.m_gameController != null)
		{
			this.m_gameController.OnConfirmPopupConfirmation();
		}
		this.m_PopupManager.SetPopup(EPopups.NONE);
	}

	// Token: 0x0400042A RID: 1066
	[SerializeField]
	private AgricolaGame m_gameController;
}
