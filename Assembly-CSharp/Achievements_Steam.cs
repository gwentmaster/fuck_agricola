using System;
using Steamworks;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class Achievements_Steam : AchievementInterface
{
	// Token: 0x060006F6 RID: 1782 RVA: 0x0003400C File Offset: 0x0003220C
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!this.m_bRequestedStats)
		{
			bool bRequestedStats = SteamUserStats.RequestCurrentStats();
			this.m_bRequestedStats = bRequestedStats;
		}
		if (!this.m_bStatsValid)
		{
			return;
		}
		if (this.m_bStoreStats)
		{
			bool flag = SteamUserStats.StoreStats();
			if (!flag)
			{
				Debug.Log("SteamUserStats.StoreStats() FAILED!");
			}
			this.m_bStoreStats = !flag;
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00034064 File Offset: 0x00032264
	public override void Initialize(AchievementData[] achievementList)
	{
		this.m_achievements = achievementList;
		if (!SteamManager.Initialized)
		{
			base.enabled = false;
			return;
		}
		this.m_GameID = new CGameID(SteamUtils.GetAppID());
		this.m_CallbackUserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.m_CallbackUserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.m_CallbackUserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnAchievementStored));
		this.m_bRequestedStats = false;
		this.m_bStatsValid = false;
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00003022 File Offset: 0x00001222
	public override void ResetAllAchievements()
	{
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x000340EC File Offset: 0x000322EC
	public override bool IncrementAchievement(EAchievements id, long plusValue)
	{
		if (!base.enabled)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < this.m_achievements.GetLength(0); i++)
		{
			if (id == this.m_achievements[i].id && !this.m_achievements[i].bHasAchieved && !this.m_achievements[i].dontShowAchievement)
			{
				int num2;
				if (this.m_achievements[i].isBitField)
				{
					AchievementData[] achievements = this.m_achievements;
					int num = i;
					achievements[num].currentValue = (achievements[num].currentValue | plusValue);
					num2 = (int)AchievementManagerWrapper.CountBits(this.m_achievements[i].currentValue);
				}
				else
				{
					AchievementData[] achievements2 = this.m_achievements;
					int num3 = i;
					achievements2[num3].currentValue = achievements2[num3].currentValue + plusValue;
					num2 = (int)this.m_achievements[i].currentValue;
				}
				if (num2 >= this.m_achievements[i].achievedAtValue)
				{
					this.m_achievements[i].bHasAchieved = true;
					result = true;
					if (!this.m_achievements[i].isBitField)
					{
						this.m_achievements[i].currentValue = (long)this.m_achievements[i].achievedAtValue;
					}
					SteamUserStats.SetAchievement(this.m_achievements[i].idNameSteam);
				}
				if (!string.IsNullOrEmpty(this.m_achievements[i].steamStatName))
				{
					SteamUserStats.SetStat(this.m_achievements[i].steamStatName, num2);
				}
				this.m_bStoreStats = true;
			}
		}
		return result;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00034280 File Offset: 0x00032480
	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_nMaxProgress == 0U)
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
				return;
			}
			Debug.Log(string.Concat(new object[]
			{
				"Achievement '",
				pCallback.m_rgchAchievementName,
				"' progress callback, (",
				pCallback.m_nCurProgress,
				",",
				pCallback.m_nMaxProgress,
				")"
			}));
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00034318 File Offset: 0x00032518
	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("StoreStats - success");
				return;
			}
			if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
			{
				Debug.Log("StoreStats - some failed to validate");
				this.OnUserStatsReceived(new UserStatsReceived_t
				{
					m_eResult = EResult.k_EResultOK,
					m_nGameID = (ulong)this.m_GameID
				});
				return;
			}
			Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
		}
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x000343A4 File Offset: 0x000325A4
	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("Received stats and achievements from Steam, " + SteamUserStats.GetNumAchievements().ToString() + " achievements loaded");
				this.m_bStatsValid = true;
				for (int i = 0; i < this.m_achievements.Length; i++)
				{
					if (!SteamUserStats.GetAchievement(this.m_achievements[i].idNameSteam, out this.m_achievements[i].bHasAchieved) && !string.IsNullOrEmpty(this.m_achievements[i].idNameSteam))
					{
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + this.m_achievements[i].idNameSteam);
					}
					if (!string.IsNullOrEmpty(this.m_achievements[i].steamStatName) && !this.m_achievements[i].isBitField)
					{
						int num = 0;
						if (SteamUserStats.GetStat(this.m_achievements[i].steamStatName, out num))
						{
							this.m_achievements[i].currentValue = (long)num;
						}
						else
						{
							Debug.LogWarning("SteamUserStats.GetStat failed for Stat " + this.m_achievements[i].steamStatName);
						}
					}
				}
				return;
			}
			Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
		}
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00034514 File Offset: 0x00032714
	public override string GetUsername()
	{
		return SteamUser.GetSteamID().m_SteamID.ToString();
	}

	// Token: 0x0400083A RID: 2106
	[Header("This script should start DISABLED!")]
	[Header("It is enabled by the Achievement Manager Wrapper")]
	private AchievementData[] m_achievements;

	// Token: 0x0400083B RID: 2107
	private CGameID m_GameID;

	// Token: 0x0400083C RID: 2108
	private bool m_bRequestedStats;

	// Token: 0x0400083D RID: 2109
	private bool m_bStatsValid;

	// Token: 0x0400083E RID: 2110
	private bool m_bStoreStats;

	// Token: 0x0400083F RID: 2111
	protected Callback<UserStatsReceived_t> m_CallbackUserStatsReceived;

	// Token: 0x04000840 RID: 2112
	protected Callback<UserStatsStored_t> m_CallbackUserStatsStored;

	// Token: 0x04000841 RID: 2113
	protected Callback<UserAchievementStored_t> m_CallbackUserAchievementStored;
}
