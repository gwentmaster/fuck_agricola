using System;
using System.Collections.Generic;
using System.Globalization;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class ThirdPartyManager : MonoBehaviour
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000846 RID: 2118 RVA: 0x00039724 File Offset: 0x00037924
	public static ThirdPartyManager instance
	{
		get
		{
			if (ThirdPartyManager._instance == null)
			{
				ThirdPartyManager._instance = UnityEngine.Object.FindObjectOfType<ThirdPartyManager>();
				if (ThirdPartyManager._instance == null)
				{
					ThirdPartyManager._instance = new GameObject
					{
						name = "ThirdPartyManager"
					}.AddComponent<ThirdPartyManager>();
				}
				UnityEngine.Object.DontDestroyOnLoad(ThirdPartyManager._instance.gameObject);
			}
			return ThirdPartyManager._instance;
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00007945 File Offset: 0x00005B45
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00039784 File Offset: 0x00037984
	private void Update()
	{
		if (this.m_framesToDelay > 0)
		{
			int num = this.m_framesToDelay - 1;
			this.m_framesToDelay = num;
			if (num == 0)
			{
				AnalyticsManager analyticsManager = CoreApplication.Instance.AnalyticsManager;
				analyticsManager.SetVersionBuildNumber(VersionManager.instance.GetVersionTextString());
				string platformBasedUsername = AchievementManagerWrapper.instance.GetPlatformBasedUsername();
				if (platformBasedUsername != string.Empty)
				{
					analyticsManager.SetUserIdFirstParty(platformBasedUsername);
					this.m_bSentFirstPartyUserID = true;
				}
				RuntimePlatform platform = Application.platform;
				if (platform == RuntimePlatform.IPhonePlayer)
				{
					analyticsManager.SetFirstParty("AppStore");
				}
				else if (platform == RuntimePlatform.Android)
				{
					analyticsManager.SetFirstParty("GooglePlay");
				}
				else
				{
					analyticsManager.SetFirstParty("Steam");
				}
				CoreApplication.Instance.AnalyticsManager.UserId = "null";
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["dlc_owned"] = string.Empty;
				analyticsManager.SetUserProperties(dictionary);
				AnalyticsEvents.LogAppBootEvent("boot", null);
				return;
			}
		}
		else
		{
			this.m_secondCount += Time.deltaTime;
			if (this.m_secondCount >= 600f)
			{
				this.m_secondCount -= 600f;
				CoreApplication.Instance.AnalyticsManager.LogEvent("HEARTBEAT", this.m_blankDict);
			}
			if (!this.m_bSentFirstPartyUserID)
			{
				string platformBasedUsername2 = AchievementManagerWrapper.instance.GetPlatformBasedUsername();
				if (platformBasedUsername2 != string.Empty)
				{
					CoreApplication.Instance.AnalyticsManager.SetUserIdFirstParty(platformBasedUsername2);
					this.m_bSentFirstPartyUserID = true;
				}
			}
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x000398F0 File Offset: 0x00037AF0
	public static string[] GenerateDlcString(uint setFlags)
	{
		int num = (int)AchievementManagerWrapper.CountBits((long)((ulong)setFlags));
		if (num == 0)
		{
			return null;
		}
		string[] array = new string[num];
		int num2 = 0;
		if ((setFlags & 4096U) != 0U)
		{
			array[num2++] = "A";
		}
		if ((setFlags & 8192U) != 0U)
		{
			array[num2++] = "B";
		}
		if ((setFlags & 16384U) != 0U)
		{
			array[num2++] = "Revised Core";
		}
		return array;
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00039954 File Offset: 0x00037B54
	public static string GenerateOfflineMatchID(int slotIndex)
	{
		string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		return string.Concat(new string[]
		{
			deviceUniqueIdentifier,
			"_",
			slotIndex.ToString(),
			"_",
			DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture)
		});
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x000399AC File Offset: 0x00037BAC
	public static string GenerateAIDifficultyString(ShortSaveStruct save_data, out int numHumans, out int numAI)
	{
		string text = string.Empty;
		bool flag = false;
		numHumans = 0;
		numAI = 0;
		if (save_data.player1State == 1 && save_data.player1Name != string.Empty)
		{
			numHumans++;
		}
		else if (save_data.player1State == 2 && save_data.player1Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "easy";
			flag = true;
		}
		else if (save_data.player1State == 3 && save_data.player1Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "medium";
			flag = true;
		}
		else if (save_data.player1State == 4 && save_data.player1Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "hard";
			flag = true;
		}
		if (save_data.player2State == 1 && save_data.player2Name != string.Empty)
		{
			numHumans++;
		}
		else if (save_data.player2State == 2 && save_data.player2Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "easy";
			flag = true;
		}
		else if (save_data.player2State == 3 && save_data.player2Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "medium";
			flag = true;
		}
		else if (save_data.player2State == 4 && save_data.player2Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "hard";
			flag = true;
		}
		if (save_data.player3State == 1 && save_data.player3Name != string.Empty)
		{
			numHumans++;
		}
		else if (save_data.player3State == 2 && save_data.player3Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "easy";
			flag = true;
		}
		else if (save_data.player3State == 3 && save_data.player3Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "medium";
			flag = true;
		}
		else if (save_data.player3State == 4 && save_data.player3Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "hard";
			flag = true;
		}
		if (save_data.player4State == 1 && save_data.player4Name != string.Empty)
		{
			numHumans++;
		}
		else if (save_data.player4State == 2 && save_data.player4Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "easy";
			flag = true;
		}
		else if (save_data.player4State == 3 && save_data.player4Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "medium";
			flag = true;
		}
		else if (save_data.player4State == 4 && save_data.player4Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "hard";
			flag = true;
		}
		if (save_data.player5State == 1 && save_data.player5Name != string.Empty)
		{
			numHumans++;
		}
		else if (save_data.player5State == 2 && save_data.player5Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "easy";
			flag = true;
		}
		else if (save_data.player5State == 3 && save_data.player5Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "medium";
			flag = true;
		}
		else if (save_data.player5State == 4 && save_data.player5Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "hard";
			flag = true;
		}
		if (save_data.player6State == 1 && save_data.player6Name != string.Empty)
		{
			numHumans++;
		}
		else if (save_data.player6State == 2 && save_data.player6Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "easy";
		}
		else if (save_data.player6State == 3 && save_data.player6Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "medium";
		}
		else if (save_data.player6State == 4 && save_data.player6Name != string.Empty)
		{
			numAI++;
			text = text + (flag ? "," : "") + "hard";
		}
		return text;
	}

	// Token: 0x0400091A RID: 2330
	private bool m_bSentFirstPartyUserID;

	// Token: 0x0400091B RID: 2331
	private bool m_bGotPaused;

	// Token: 0x0400091C RID: 2332
	private DateTime m_pausedStartTime = DateTime.UtcNow;

	// Token: 0x0400091D RID: 2333
	private double m_sendResumeThreshold = 120.0;

	// Token: 0x0400091E RID: 2334
	private float m_secondCount;

	// Token: 0x0400091F RID: 2335
	private int m_framesToDelay = 5;

	// Token: 0x04000920 RID: 2336
	private Dictionary<string, object> m_blankDict = new Dictionary<string, object>();

	// Token: 0x04000921 RID: 2337
	private static ThirdPartyManager _instance;
}
