using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class AgricolaActionSpace_VariableWorkerLocators : AgricolaActionSpace
{
	// Token: 0x06000181 RID: 385 RVA: 0x00008364 File Offset: 0x00006564
	public override void RebuildAnimationManager(AnimationManager animation_manager)
	{
		if (this.m_PossibleWorkerLocators != null)
		{
			int num = AgricolaLib.GetGamePlayerCount() - 1;
			if (num >= 0 && num < this.m_PossibleWorkerLocators.Length && this.m_PossibleWorkerLocators[num] != null)
			{
				this.m_WorkerLocator = this.m_PossibleWorkerLocators[num];
			}
		}
		base.RebuildAnimationManager(animation_manager);
	}

	// Token: 0x040000BC RID: 188
	[Header("Add All Possible Player Counts (1p = index 0)")]
	[SerializeField]
	private AgricolaAnimationLocator[] m_PossibleWorkerLocators;
}
