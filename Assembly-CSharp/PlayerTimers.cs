using System;

// Token: 0x020000F0 RID: 240
public class PlayerTimers
{
	// Token: 0x060008A9 RID: 2217 RVA: 0x0003BF14 File Offset: 0x0003A114
	public static string GetTimeStringFromSeconds(int seconds)
	{
		for (int i = 0; i < PlayerTimers.s_playerTimerOptions.Length; i++)
		{
			if (PlayerTimers.s_playerTimerOptions[i] == seconds)
			{
				return PlayerTimers.s_playerTimerText[i];
			}
		}
		for (int j = 0; j < PlayerTimers.s_DeprecatedPlayerTimerOptions.Length; j++)
		{
			if (PlayerTimers.s_DeprecatedPlayerTimerOptions[j] == seconds)
			{
				return PlayerTimers.s_DeprecatedPlayerTimerText[j];
			}
		}
		return "ERROR: Time Unknown";
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0003BF70 File Offset: 0x0003A170
	public static int GetClosestValue(int t, int[] value_array)
	{
		int num = Array.BinarySearch<int>(value_array, t);
		if (num < 0)
		{
			num = ~num;
			if (num >= value_array.Length)
			{
				num = value_array.Length - 1;
			}
			else if (num > 0)
			{
				int value = t - value_array[num];
				if (Math.Abs(t - value_array[num - 1]) <= Math.Abs(value))
				{
					num--;
				}
			}
		}
		return num;
	}

	// Token: 0x04000969 RID: 2409
	public static readonly int[] s_playerTimerOptions = new int[]
	{
		1800,
		3600,
		14400,
		86400,
		259200,
		604800,
		1814400,
		3888000
	};

	// Token: 0x0400096A RID: 2410
	public static readonly string[] s_playerTimerText = new string[]
	{
		"30 ${Key_Minutes}",
		"1 ${Key_Hour}",
		"4 ${Key_Hours}",
		"1 ${Key_Day}",
		"3 ${Key_Days}",
		"7 ${Key_Days}",
		"21 ${Key_Days}",
		"45 ${Key_Days}"
	};

	// Token: 0x0400096B RID: 2411
	public static readonly int[] s_DeprecatedPlayerTimerOptions = new int[]
	{
		2700,
		5400,
		10800,
		21600
	};

	// Token: 0x0400096C RID: 2412
	public static readonly string[] s_DeprecatedPlayerTimerText = new string[]
	{
		"45 Minutes",
		"90 Minutes",
		"3 Hours",
		"6 Hours"
	};
}
