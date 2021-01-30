using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class SelectQualityLevel : MonoBehaviour
{
	// Token: 0x06000701 RID: 1793 RVA: 0x00034548 File Offset: 0x00032748
	private bool SetLevel(string qualityLevelName)
	{
		string[] names = QualitySettings.names;
		for (int i = 0; i < names.Length; i++)
		{
			if (names[i] == qualityLevelName)
			{
				Debug.Log("Setting Quality Level to " + qualityLevelName);
				QualitySettings.SetQualityLevel(i, true);
				return true;
			}
		}
		Debug.LogError("Quality Level " + qualityLevelName + " does not exist!");
		return false;
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00003022 File Offset: 0x00001222
	private void Awake()
	{
	}
}
