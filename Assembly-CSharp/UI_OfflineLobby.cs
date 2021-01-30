using System;
using TMPro;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class UI_OfflineLobby : MonoBehaviour
{
	// Token: 0x06000AE2 RID: 2786 RVA: 0x0004A86C File Offset: 0x00048A6C
	public virtual void OnEnterMenu()
	{
		if (this.m_backgroundAnimator != null && this.m_backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("Intro"))
		{
			this.m_backgroundAnimator.Play("Reset_OfflineLobby");
		}
		ProfileManager.OfflineProfileEntry currentProfile = ProfileManager.instance.GetCurrentProfile();
		this.m_profileName.text = currentProfile.name;
		this.m_soloCurrentStreak.text = currentProfile.soloCurrentStreak.ToString();
		this.m_soloSeriesTopStreak.text = currentProfile.soloTopStreak.ToString();
		this.m_completedGames.text = currentProfile.completed.ToString();
		this.m_soloTopScore.text = currentProfile.soloTopScore.ToString();
		this.m_2pRecord.text = currentProfile.wins_2p.ToString() + " - " + currentProfile.losses_2p.ToString();
		this.m_3pRecord.text = currentProfile.wins_3p.ToString() + " - " + currentProfile.losses_3p.ToString();
		this.m_4pRecord.text = currentProfile.wins_4p.ToString() + " - " + currentProfile.losses_4p.ToString();
		this.m_5pRecord.text = currentProfile.wins_5p.ToString() + " - " + currentProfile.losses_5p.ToString();
		this.m_6pRecord.text = currentProfile.wins_6p.ToString() + " - " + currentProfile.losses_6p.ToString();
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void OnExitMenu(bool bUnderPopup)
	{
	}

	// Token: 0x04000B99 RID: 2969
	public Animator m_backgroundAnimator;

	// Token: 0x04000B9A RID: 2970
	public TextMeshProUGUI m_profileName;

	// Token: 0x04000B9B RID: 2971
	public TextMeshProUGUI m_soloCurrentStreak;

	// Token: 0x04000B9C RID: 2972
	public TextMeshProUGUI m_soloSeriesTopStreak;

	// Token: 0x04000B9D RID: 2973
	public TextMeshProUGUI m_completedGames;

	// Token: 0x04000B9E RID: 2974
	public TextMeshProUGUI m_soloTopScore;

	// Token: 0x04000B9F RID: 2975
	public TextMeshProUGUI m_2pRecord;

	// Token: 0x04000BA0 RID: 2976
	public TextMeshProUGUI m_3pRecord;

	// Token: 0x04000BA1 RID: 2977
	public TextMeshProUGUI m_4pRecord;

	// Token: 0x04000BA2 RID: 2978
	public TextMeshProUGUI m_5pRecord;

	// Token: 0x04000BA3 RID: 2979
	public TextMeshProUGUI m_6pRecord;
}
