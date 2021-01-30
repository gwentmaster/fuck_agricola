using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class AchievementInterface : MonoBehaviour
{
	// Token: 0x06000704 RID: 1796 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void Initialize(AchievementData[] achievementList)
	{
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0002A062 File Offset: 0x00028262
	public virtual bool IncrementAchievement(EAchievements id, long plusValue)
	{
		return false;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void ResetAllAchievements()
	{
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x000345A3 File Offset: 0x000327A3
	public virtual string GetUsername()
	{
		return string.Empty;
	}
}
