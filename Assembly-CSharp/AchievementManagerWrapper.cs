using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class AchievementManagerWrapper : MonoBehaviour
{
	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x000345AC File Offset: 0x000327AC
	public static AchievementManagerWrapper instance
	{
		get
		{
			if (AchievementManagerWrapper._instance == null)
			{
				AchievementManagerWrapper._instance = UnityEngine.Object.FindObjectOfType<AchievementManagerWrapper>();
				if (AchievementManagerWrapper._instance == null)
				{
					AchievementManagerWrapper._instance = new GameObject
					{
						name = "ScreenManager"
					}.AddComponent<AchievementManagerWrapper>();
				}
				UnityEngine.Object.DontDestroyOnLoad(AchievementManagerWrapper._instance.gameObject);
			}
			return AchievementManagerWrapper._instance;
		}
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0003460C File Offset: 0x0003280C
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (this.m_achievements != null)
		{
			for (int i = 0; i < this.m_achievements.Length; i++)
			{
				string @string = PlayerPrefs.GetString("Achievement_" + this.m_achievements[i].id.ToString(), "0");
				this.m_achievements[i].currentValue = Convert.ToInt64(@string);
				long num = this.m_achievements[i].isBitField ? AchievementManagerWrapper.CountBits(this.m_achievements[i].currentValue) : this.m_achievements[i].currentValue;
				this.m_achievements[i].bHasAchieved = (num >= (long)this.m_achievements[i].achievedAtValue);
				if (VersionManager.instance.UsePlaytestVersion())
				{
					AchievementData[] achievements = this.m_achievements;
					int num2 = i;
					achievements[num2].idNameIOS = achievements[num2].idNameIOS + ".pt";
				}
			}
		}
		this.m_ActiveManager = null;
		if (this.m_PlatformManagers != null)
		{
			for (int j = 0; j < this.m_PlatformManagers.Length; j++)
			{
				AchievementInterface achievementInterface = this.m_PlatformManagers[j];
				if (achievementInterface != null && achievementInterface.gameObject.activeInHierarchy)
				{
					this.m_ActiveManager = achievementInterface;
					break;
				}
			}
		}
		if (this.m_ActiveManager != null)
		{
			this.m_ActiveManager.enabled = true;
			this.m_ActiveManager.Initialize(this.m_achievements);
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00003022 File Offset: 0x00001222
	private void Update()
	{
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00034799 File Offset: 0x00032999
	private void OnDestroy()
	{
		this.SaveData();
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x000347A1 File Offset: 0x000329A1
	public void SetEnabled(bool bEnabled)
	{
		this.m_bEnabled = bEnabled;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x000347AC File Offset: 0x000329AC
	public void IncrementAchievement(EAchievements id, long addedValue)
	{
		if (this.m_bEnabled)
		{
			AchievementManagerWrapper.QueuedAchievementProgress item;
			item.achievement = id;
			item.progress = addedValue;
			this.m_queue.Add(item);
			this.CommitQueue();
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00003022 File Offset: 0x00001222
	public void ResetAchievementData()
	{
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x000347E3 File Offset: 0x000329E3
	public void ClearQueue()
	{
		this.m_queue.Clear();
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x000347F0 File Offset: 0x000329F0
	public void CommitQueue()
	{
		if (this.m_queue.Count == 0)
		{
			return;
		}
		foreach (AchievementManagerWrapper.QueuedAchievementProgress queuedAchievementProgress in this.m_queue)
		{
			this.IncrementQueuedAchievement(queuedAchievementProgress.achievement, queuedAchievementProgress.progress);
		}
		this.SaveData();
		this.m_queue.Clear();
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00034870 File Offset: 0x00032A70
	private void IncrementQueuedAchievement(EAchievements id, long addedValue)
	{
		if (this.m_ActiveManager != null && this.m_ActiveManager.IncrementAchievement(id, addedValue))
		{
			AnalyticsEvents.LogAchievementUnlockEvent(Enum.GetName(typeof(EAchievements), id), null);
		}
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x000348AC File Offset: 0x00032AAC
	public void SaveData()
	{
		if (this.m_achievements != null)
		{
			for (int i = 0; i < this.m_achievements.Length; i++)
			{
				PlayerPrefs.SetString("Achievement_" + this.m_achievements[i].id.ToString(), string.Concat(this.m_achievements[i].currentValue));
			}
		}
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0003491C File Offset: 0x00032B1C
	public bool IsAchievementUnlocked(EAchievements id)
	{
		for (int i = 0; i < this.m_achievements.GetLength(0); i++)
		{
			if (this.m_achievements[i].id == id)
			{
				return this.m_achievements[i].bHasAchieved;
			}
		}
		string str = "Unable to Find Achievement with ID: ";
		int num = (int)id;
		Debug.LogError(str + num.ToString());
		return false;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00034980 File Offset: 0x00032B80
	public static long CountBits(long value)
	{
		int num = 0;
		while (value != 0L)
		{
			num++;
			value &= value - 1L;
		}
		return (long)num;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x000349A2 File Offset: 0x00032BA2
	public string GetPlatformBasedUsername()
	{
		if (this.m_ActiveManager != null)
		{
			return this.m_ActiveManager.GetUsername();
		}
		return string.Empty;
	}

	// Token: 0x04000860 RID: 2144
	[SerializeField]
	private VersionManager m_VersionManager;

	// Token: 0x04000861 RID: 2145
	public AchievementData[] m_achievements;

	// Token: 0x04000862 RID: 2146
	[SerializeField]
	private AchievementInterface[] m_PlatformManagers;

	// Token: 0x04000863 RID: 2147
	private AchievementInterface m_ActiveManager;

	// Token: 0x04000864 RID: 2148
	private bool m_bEnabled = true;

	// Token: 0x04000865 RID: 2149
	private List<AchievementManagerWrapper.QueuedAchievementProgress> m_queue = new List<AchievementManagerWrapper.QueuedAchievementProgress>();

	// Token: 0x04000866 RID: 2150
	private static AchievementManagerWrapper _instance;

	// Token: 0x02000798 RID: 1944
	public struct QueuedAchievementProgress
	{
		// Token: 0x04002C62 RID: 11362
		public EAchievements achievement;

		// Token: 0x04002C63 RID: 11363
		public long progress;
	}
}
