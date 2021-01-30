using System;
using System.IO;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class InitializeAgricolaLib : MonoBehaviour
{
	// Token: 0x0600069E RID: 1694 RVA: 0x00032C5C File Offset: 0x00030E5C
	private void Awake()
	{
		Debug.Log("Initializing AgricolaLib...");
		string text = Path.Combine(Application.streamingAssetsPath, "Lua");
		Debug.Log("        dataPath = " + text);
		Debug.Log("        SystemInfo.processorCount = " + SystemInfo.processorCount);
		Debug.Log("    AgricolaLib.Initialize:");
		AgricolaLib.Initialize(text, SystemInfo.processorCount);
		Debug.Log("    AgricolaLib.SetSaveDataFunc:");
		AgricolaLib.SetSaveDataFunc(new AgricolaLib.SaveWorldDataDelegate(AgricolaGame.OnSaveData));
		Debug.Log("    AgricolaLib.SetGameOptionsListener:");
		AgricolaLib.SetGameOptionsListener(null);
		Debug.Log("AgricolaLib");
	}
}
