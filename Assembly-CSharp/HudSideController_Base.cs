using System;
using UnityEngine;

// Token: 0x02000061 RID: 97
public abstract class HudSideController_Base : MonoBehaviour
{
	// Token: 0x0600051F RID: 1311
	public abstract void HandleAllButtonsOff();

	// Token: 0x06000520 RID: 1312
	public abstract void UpdateButtonVisibility(bool bForceState);

	// Token: 0x040004CE RID: 1230
	[SerializeField]
	protected Animator m_animator;
}
