using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class UISM_HudSideController : StateMachineBehaviour
{
	// Token: 0x060005F8 RID: 1528 RVA: 0x00032B4C File Offset: 0x00030D4C
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.m_hudBase == null)
		{
			this.m_hudBase = animator.gameObject.GetComponent<HudSideController_Base>();
		}
		if (this.m_hudBase != null)
		{
			if (stateInfo.IsTag("AllButtonsOff"))
			{
				this.m_hudBase.HandleAllButtonsOff();
				return;
			}
			this.m_hudBase.UpdateButtonVisibility(true);
		}
	}

	// Token: 0x0400061B RID: 1563
	private HudSideController_Base m_hudBase;
}
