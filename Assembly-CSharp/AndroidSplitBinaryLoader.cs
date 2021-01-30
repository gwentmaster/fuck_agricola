using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000BE RID: 190
public class AndroidSplitBinaryLoader : MonoBehaviour
{
	// Token: 0x060006EF RID: 1775 RVA: 0x00003022 File Offset: 0x00001222
	private void Start()
	{
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00033E24 File Offset: 0x00032024
	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Application.dataPath.Contains(".obb") && !this.obbisok)
			{
				Debug.Log("StartCoroutine(CheckSetUp())");
				base.StartCoroutine(this.CheckSetUp());
				this.obbisok = true;
				return;
			}
		}
		else if (!this.loading)
		{
			this.StartApp();
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00033E80 File Offset: 0x00032080
	private void OnGUI()
	{
		GUI.skin = this.mySkin;
		GUILayout.BeginArea(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (!this.obbisok)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.Label("There is an installation error with this application, Please re-install!", Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00033F38 File Offset: 0x00032138
	public void StartApp()
	{
		this.loading = true;
		SceneManager.LoadScene(this.nextScene);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00033F4C File Offset: 0x0003214C
	public IEnumerator CheckSetUp()
	{
		int num;
		for (int i = 0; i < this.paths.Length; i = num)
		{
			yield return base.StartCoroutine(this.PullStreamingAssetFromObb(this.paths[i]));
			num = i + 1;
		}
		yield return new WaitForSeconds(1f);
		this.StartApp();
		yield break;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00033F5B File Offset: 0x0003215B
	public IEnumerator PullStreamingAssetFromObb(string sapath)
	{
		if (!File.Exists(Application.persistentDataPath + sapath) || this.replacefiles)
		{
			WWW unpackerWWW = new WWW(Application.streamingAssetsPath + "/" + sapath);
			while (!unpackerWWW.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(unpackerWWW.error))
			{
				Debug.Log("Error unpacking:" + unpackerWWW.error + " path: " + unpackerWWW.url);
				yield break;
			}
			Debug.Log("Extracting " + sapath + " to Persistant Data");
			if (!Directory.Exists(Path.GetDirectoryName(Application.persistentDataPath + "/" + sapath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath + "/" + sapath));
			}
			File.WriteAllBytes(Application.persistentDataPath + "/" + sapath, unpackerWWW.bytes);
			unpackerWWW = null;
		}
		yield return 0;
		yield break;
	}

	// Token: 0x04000833 RID: 2099
	private string nextScene = "Startup";

	// Token: 0x04000834 RID: 2100
	public Texture2D background;

	// Token: 0x04000835 RID: 2101
	public GUISkin mySkin;

	// Token: 0x04000836 RID: 2102
	private bool obbisok;

	// Token: 0x04000837 RID: 2103
	private bool loading;

	// Token: 0x04000838 RID: 2104
	private bool replacefiles;

	// Token: 0x04000839 RID: 2105
	private string[] paths = new string[]
	{
		"Lua/load_database.lua",
		"Lua/actions.lua",
		"Lua/actions_load.lua",
		"Lua/set1_cards.lua",
		"Lua/set1_load.lua",
		"Lua/cards_major_imp.lua",
		"Lua/major_imp_load.lua",
		"Lua/cards_a_minor_imp.lua",
		"Lua/cards_a_occupations.lua",
		"Lua/deck_a_load.lua",
		"Lua/cards_b_minor_imp.lua",
		"Lua/cards_b_occupations.lua",
		"Lua/deck_b_load.lua"
	};
}
