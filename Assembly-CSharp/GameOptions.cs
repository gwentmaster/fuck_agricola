using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000056 RID: 86
public struct GameOptions
{
	// Token: 0x060004C7 RID: 1223 RVA: 0x00025688 File Offset: 0x00023888
	[MonoPInvokeCallback(typeof(AgricolaLib.GameOptionsListenerDelegate))]
	public static void OnGameOptionsEndList(int playerID, IntPtr pOptionPrompt, int numOptions, IntPtr pGameOptions)
	{
		int num = Marshal.SizeOf(typeof(GameOption));
		if (numOptions > 0)
		{
			GameOptions.s_bNewGameOptions = true;
		}
		for (int i = 0; i < numOptions; i++)
		{
			IntPtr ptr = new IntPtr(pGameOptions.ToInt64() + (long)(num * i));
			GameOptions.m_GameOption[i] = (GameOption)Marshal.PtrToStructure(ptr, typeof(GameOption));
		}
		GameOptions.m_OptionPrompt = Marshal.PtrToStringAnsi(pOptionPrompt);
		GameOptions.m_OptionCount = numOptions;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x000256FF File Offset: 0x000238FF
	public static bool Update()
	{
		if (GameOptions.s_bNewGameOptions)
		{
			GameOptions.s_bNewGameOptions = false;
			return true;
		}
		return false;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00025711 File Offset: 0x00023911
	public static void Initialize(AgricolaFarm agricolaFarm)
	{
		AgricolaLib.SetGameOptionsListener(new AgricolaLib.GameOptionsListenerDelegate(GameOptions.OnGameOptionsEndList));
		GameOptions.m_GameOption = new GameOption[140];
		GameOptions.m_OptionCount = 0;
		GameOptions.s_bNewGameOptions = false;
		GameOptions.s_agricolaFarm = agricolaFarm;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00025745 File Offset: 0x00023945
	public static void Reset()
	{
		GameOptions.m_OptionCount = 0;
		GameOptions.m_OptionPrompt = "";
		GameOptions.s_bNewGameOptions = false;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0002575D File Offset: 0x0002395D
	public static void ResendGameOptionsList()
	{
		GameOptions.Reset();
		AgricolaLib.ResendGameOptionsList();
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0002576C File Offset: 0x0002396C
	public static void SelectOption(int optionIndex)
	{
		if (GameOptions.s_agricolaFarm != null)
		{
			GameOptions.s_agricolaFarm.SubmitAnimalPlacement();
		}
		ushort selectionHint = GameOptions.m_GameOption[optionIndex].selectionHint;
		AgricolaLib.SelectGameOption(optionIndex);
		GameOptions.m_OptionCount = 0;
		GameOptions.m_OptionPrompt = "";
		GameOptions.s_bNewGameOptions = true;
		AgricolaGame component = GameObject.Find("/AgricolaGame").GetComponent<AgricolaGame>();
		if (selectionHint != 41040 && selectionHint != 41041 && selectionHint != 41042)
		{
			component.HandleTutorialGameOptionSelection();
		}
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000257EC File Offset: 0x000239EC
	public static void SelectOptionWithData(int optionIndex, uint selectionData)
	{
		if (GameOptions.s_agricolaFarm != null)
		{
			GameOptions.s_agricolaFarm.SubmitAnimalPlacement();
		}
		ushort selectionHint = GameOptions.m_GameOption[optionIndex].selectionHint;
		AgricolaLib.SelectGameOptionWithData(optionIndex, selectionData);
		GameOptions.m_OptionCount = 0;
		GameOptions.m_OptionPrompt = "";
		GameOptions.s_bNewGameOptions = true;
		AgricolaGame component = GameObject.Find("/AgricolaGame").GetComponent<AgricolaGame>();
		if (selectionHint != 41040 && selectionHint != 41041 && selectionHint != 41042)
		{
			component.HandleTutorialGameOptionSelection();
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0002586C File Offset: 0x00023A6C
	public static bool SelectOptionByInstanceID(ushort instanceID)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID)
			{
				GameOptions.SelectOption(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x000258A8 File Offset: 0x00023AA8
	public static bool SelectOptionByHint(ushort selectionHint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == selectionHint)
			{
				GameOptions.SelectOption(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x000258E4 File Offset: 0x00023AE4
	public static bool SelectOptionByHintAndOptionText(ushort selectionHint, string optionText)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == selectionHint && GameOptions.m_GameOption[i].optionText == optionText)
			{
				GameOptions.SelectOption(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00025938 File Offset: 0x00023B38
	public static bool SelectOptionByInstanceIDAndHint(ushort instanceID, ushort selectionHint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID && GameOptions.m_GameOption[i].selectionHint == selectionHint)
			{
				GameOptions.SelectOption(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00025984 File Offset: 0x00023B84
	public static bool SelectOptionByHintWithData(ushort selectionHint, uint selectionData)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == selectionHint)
			{
				GameOptions.SelectOptionWithData(i, selectionData);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x000259C0 File Offset: 0x00023BC0
	public static bool SelectOptionByInstanceIDAndHintWithData(ushort instanceID, ushort selectionHint, uint selectionData)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID && GameOptions.m_GameOption[i].selectionHint == selectionHint)
			{
				GameOptions.SelectOptionWithData(i, selectionData);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00025A10 File Offset: 0x00023C10
	public static ushort GetInstanceIDFromHint(ushort hint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == hint && GameOptions.m_GameOption[i].isHidden == 0)
			{
				return GameOptions.m_GameOption[i].selectionID;
			}
		}
		return 0;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00025A64 File Offset: 0x00023C64
	public static string GetOptionPromptFromHint(ushort hint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == hint && GameOptions.m_GameOption[i].isHidden == 0)
			{
				return GameOptions.m_GameOption[i].optionText;
			}
		}
		return "NONE";
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00025ABC File Offset: 0x00023CBC
	public static string GetOptionPromptFromIDAndHint(ushort selectionID, ushort selectionHint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == selectionID && GameOptions.m_GameOption[i].selectionHint == selectionHint && GameOptions.m_GameOption[i].isHidden == 0)
			{
				return GameOptions.m_GameOption[i].optionText;
			}
		}
		return "NONE";
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00025B28 File Offset: 0x00023D28
	public static bool IsSelectableHint(ushort hint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == hint && GameOptions.m_GameOption[i].isHidden == 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00025B70 File Offset: 0x00023D70
	public static bool IsSelectableHintAllowHidden(ushort hint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == hint)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00025BA4 File Offset: 0x00023DA4
	public static bool IsSelectableHintWithOptionText(ushort hint, string optionText)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == hint && GameOptions.m_GameOption[i].isHidden == 0 && GameOptions.m_GameOption[i].optionText == optionText)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00025C04 File Offset: 0x00023E04
	public static bool IsSelectableInstanceID(ushort instanceID)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID && GameOptions.m_GameOption[i].isHidden == 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00025C4C File Offset: 0x00023E4C
	public static bool IsSelectableInstanceIDWithHint(ushort instanceID, ushort hint)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID && GameOptions.m_GameOption[i].selectionHint == hint && GameOptions.m_GameOption[i].isHidden == 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00025CA4 File Offset: 0x00023EA4
	public static bool IsSelectableInstanceIDWithHint(ushort instanceID, ushort hint, ref int isHidden)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID && GameOptions.m_GameOption[i].selectionHint == hint)
			{
				isHidden = (int)GameOptions.m_GameOption[i].isHidden;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00025CFC File Offset: 0x00023EFC
	public static int GetSelectableIndex(ushort instanceID, ushort hint, bool bAllowHidden = false)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID && GameOptions.m_GameOption[i].selectionHint == hint && (bAllowHidden || GameOptions.m_GameOption[i].isHidden == 0))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00025D57 File Offset: 0x00023F57
	public static ushort GetSelectionHintForOptionIndex(int optionIndex)
	{
		if (optionIndex < 0 || optionIndex >= GameOptions.m_OptionCount)
		{
			return 0;
		}
		return GameOptions.m_GameOption[optionIndex].selectionHint;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00025D78 File Offset: 0x00023F78
	public static List<ushort> GetSelectionHints(ushort instanceID)
	{
		List<ushort> list = new List<ushort>();
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionID == instanceID && GameOptions.m_GameOption[i].isHidden == 0)
			{
				list.Add(GameOptions.m_GameOption[i].selectionHint);
			}
		}
		return list;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00025DD8 File Offset: 0x00023FD8
	public static List<ushort> GetSelectionIDsForHint(ushort hint, bool bAllowHidden = false)
	{
		List<ushort> list = new List<ushort>();
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].selectionHint == hint && (bAllowHidden || GameOptions.m_GameOption[i].isHidden == 0))
			{
				list.Add(GameOptions.m_GameOption[i].selectionID);
			}
		}
		return list;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00025E3A File Offset: 0x0002403A
	public static bool IsOptionIndexHidden(int optionIndex)
	{
		return optionIndex >= 0 && optionIndex < GameOptions.m_OptionCount && GameOptions.m_GameOption[optionIndex].isHidden > 0;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00025E60 File Offset: 0x00024060
	public static void UnhideAllOptions()
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			GameOptions.m_GameOption[i].isHidden = 0;
		}
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00025E90 File Offset: 0x00024090
	public static void HideAllOptions()
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			GameOptions.m_GameOption[i].isHidden = 1;
		}
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00025EC0 File Offset: 0x000240C0
	public static void HideAllOtherOptions(ushort selectionHint, ushort selectionID)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			GameOptions.m_GameOption[i].isHidden = ((GameOptions.m_GameOption[i].selectionHint != selectionHint || GameOptions.m_GameOption[i].selectionID != selectionID) ? 1 : 0);
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00025F18 File Offset: 0x00024118
	public static void HideAllOtherOptions(ushort selectionHint, List<ushort> selectionIDList)
	{
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			GameOptions.m_GameOption[i].isHidden = ((GameOptions.m_GameOption[i].selectionHint != selectionHint || !selectionIDList.Contains(GameOptions.m_GameOption[i].selectionID)) ? 1 : 0);
		}
	}

	// Token: 0x0400044B RID: 1099
	public static GameOption[] m_GameOption;

	// Token: 0x0400044C RID: 1100
	public static int m_OptionCount;

	// Token: 0x0400044D RID: 1101
	public static string m_OptionPrompt;

	// Token: 0x0400044E RID: 1102
	public static bool s_bNewGameOptions;

	// Token: 0x0400044F RID: 1103
	private static AgricolaFarm s_agricolaFarm;

	// Token: 0x02000771 RID: 1905
	// (Invoke) Token: 0x06004205 RID: 16901
	public delegate void GameOptionsNotifierDelegate(GameOption option, bool bEnable);
}
