using System;
using System.Collections.Generic;
using System.Text;
using SA.Foundation.Patterns;
using SA.Productivity.GoogleSheets;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class LocalizationService
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000144 RID: 324 RVA: 0x00007212 File Offset: 0x00005412
	public static LocalizationService Instance
	{
		get
		{
			if (LocalizationService.s_Instance == null)
			{
				LocalizationService.s_Instance = new LocalizationService();
				LocalizationService.s_Instance.Initialize();
			}
			return LocalizationService.s_Instance;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000145 RID: 325 RVA: 0x00007234 File Offset: 0x00005434
	// (set) Token: 0x06000146 RID: 326 RVA: 0x0000723B File Offset: 0x0000543B
	public string LocalizationLanguage
	{
		get
		{
			return LocalizationService.s_localizationLanguage;
		}
		set
		{
			LocalizationService.s_localizationLanguage = value;
			this.s_LocalizationLibrary = this.LoadLocalizeFileHelper();
			this.SetLocalizationLanguage(value);
			if (this.OnChangeLocalization != null)
			{
				this.OnChangeLocalization();
			}
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00007269 File Offset: 0x00005469
	private void Initialize()
	{
		this.LocalizationLanguage = this.GetLocalizationLanguage();
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00007278 File Offset: 0x00005478
	private static void ParseLocalizeFile(Dictionary<string, string> dictionary, byte[] csv_bytes)
	{
		CSVReader csvreader = new CSVReader(csv_bytes);
		for (;;)
		{
			string[] array = csvreader.ParseNextEntry();
			if (array == null || array.Length < 2)
			{
				break;
			}
			if (!string.IsNullOrEmpty(array[0]))
			{
				if (!dictionary.ContainsKey(array[0]))
				{
					dictionary.Add(array[0], array[1]);
				}
				else
				{
					Debug.LogError(string.Format("Key {0} already exist", array[0]));
				}
			}
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x000072D4 File Offset: 0x000054D4
	public string GetTextByKey(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return "";
		}
		string result;
		if (this.s_LocalizationLibrary.TryGetValue(key, out result))
		{
			return result;
		}
		return "";
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00007306 File Offset: 0x00005506
	public string GetLocalizationLanguage()
	{
		return PlayerPrefs.GetString("localization", "EN");
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00007317 File Offset: 0x00005517
	private void SetLocalizationLanguage(string localize)
	{
		PlayerPrefs.SetString("localization", localize);
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00007324 File Offset: 0x00005524
	private void LoadLocalizationTextFromFile(Dictionary<string, string> dictionary, string filepath)
	{
		TextAsset textAsset = Resources.Load(filepath, typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			LocalizationService.ParseLocalizeFile(dictionary, textAsset.bytes);
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000735C File Offset: 0x0000555C
	private void LoadLocalizationTextFromGoogleSheet(Dictionary<string, string> dictionary, string document_name, string sheet_name, string find_language)
	{
		GD_DocTemplate docByName = SA_ScriptableSingleton<GD_Settings>.Instance.GetDocByName(document_name);
		if (docByName == null)
		{
			return;
		}
		int worksheetId = docByName.GetWorksheetId(sheet_name);
		int num = -1;
		int num2 = 2;
		for (;;)
		{
			string value = GD_API.GetValue<string>(document_name, 1, num2, worksheetId);
			if (string.IsNullOrEmpty(value))
			{
				goto IL_45;
			}
			if (value == find_language)
			{
				break;
			}
			num2++;
		}
		num = num2;
		IL_45:
		if (num == -1)
		{
			return;
		}
		int num3 = 2;
		for (;;)
		{
			string value2 = GD_API.GetValue<string>(document_name, num3, 1, worksheetId);
			if (string.IsNullOrEmpty(value2))
			{
				break;
			}
			string value3 = GD_API.GetValue<string>(document_name, num3, num, worksheetId);
			if (!dictionary.ContainsKey(value2))
			{
				dictionary.Add(value2, value3);
			}
			else
			{
				Debug.LogError(string.Format("Key {0} already exist", value2));
			}
			num3++;
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00007408 File Offset: 0x00005608
	public Dictionary<string, string> LoadLocalizeFileHelper()
	{
		string empty = string.Empty;
		switch (PlatformManager.s_instance.GetDeviceType())
		{
		default:
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			this.LoadLocalizationTextFromGoogleSheet(dictionary, "Strings", "Strings", LocalizationService.s_localizationLanguage);
			this.LoadLocalizationTextFromGoogleSheet(dictionary, "InGame", "InGame", LocalizationService.s_localizationLanguage);
			this.LoadLocalizationTextFromGoogleSheet(dictionary, "CardsRevised", "CardsRevised", LocalizationService.s_localizationLanguage);
			this.LoadLocalizationTextFromGoogleSheet(dictionary, "Rulebook", "Rulebook", LocalizationService.s_localizationLanguage);
			return dictionary;
		}
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00007497 File Offset: 0x00005697
	private void LoadDefault()
	{
		this.LocalizationLanguage = "EN";
	}

	// Token: 0x06000150 RID: 336 RVA: 0x000074A4 File Offset: 0x000056A4
	public string ConvertLocalizationKeys(string input_text)
	{
		string text = input_text;
		int num = 0;
		while ((num = text.IndexOf("${", num)) != -1)
		{
			int num2 = text.IndexOf("}", num);
			if (num2 == -1)
			{
				break;
			}
			int num3 = num2 - num + 1;
			int startIndex = num + 2;
			int length = num3 - 3;
			string key = text.Substring(startIndex, length);
			string textByKey = this.GetTextByKey(key);
			text = text.Remove(num, num3);
			text = text.Insert(num, textByKey);
		}
		return text;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00007514 File Offset: 0x00005714
	public string ConvertEmbeddedLocalizationKeys(string input_text)
	{
		string text = input_text;
		int num = 0;
		while ((num = text.IndexOf("$[", num)) != -1)
		{
			int num2 = text.IndexOf("]", num);
			if (num2 == -1)
			{
				break;
			}
			int num3 = num2 - num + 1;
			int startIndex = num + 2;
			int length = num3 - 3;
			string key = text.Substring(startIndex, length);
			string textByKey = this.GetTextByKey(key);
			text = text.Remove(num, num3);
			text = text.Insert(num, textByKey);
		}
		return text;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00007584 File Offset: 0x00005784
	private void AppendStringToStringBuilder(StringBuilder builder, string append_text)
	{
		if (string.IsNullOrEmpty(append_text))
		{
			return;
		}
		int length = append_text.Length;
		int num = 0;
		int num2;
		while ((num2 = append_text.IndexOf("${", num)) != -1)
		{
			int num3 = append_text.IndexOf("}", num2);
			if (num3 == -1)
			{
				break;
			}
			if (num < num2)
			{
				builder.Append(append_text, num, num2 - num);
			}
			int num4 = num3 - num2 + 1;
			int startIndex = num2 + 2;
			int length2 = num4 - 3;
			string text = append_text.Substring(startIndex, length2);
			int num5 = text.IndexOf(":");
			string text2;
			if (num5 != -1)
			{
				List<string> list = new List<string>();
				string key = text.Substring(0, num5);
				int num6 = num5 + 1;
				bool flag = true;
				while (flag)
				{
					num5 = text.IndexOf(":", num6);
					string input_text;
					if (num5 != -1)
					{
						int length3 = num5 - num6;
						input_text = text.Substring(num6, length3);
						num6 = num5 + 1;
						flag = true;
					}
					else
					{
						input_text = text.Substring(num6);
						flag = false;
					}
					string item = this.ConvertEmbeddedLocalizationKeys(input_text);
					list.Add(item);
				}
				text2 = this.GetTextByKey(key);
				int i = 0;
				int num7 = 0;
				while (i < list.Count)
				{
					num7 = text2.IndexOf("%s", num7);
					if (num7 == -1)
					{
						break;
					}
					text2 = text2.Remove(num7, 2).Insert(num7, list[i]);
					i++;
				}
				for (int j = 0; j < list.Count; j++)
				{
					string oldValue = string.Format("%{0}s", j);
					text2 = text2.Replace(oldValue, list[j]);
				}
			}
			else
			{
				text2 = this.GetTextByKey(text);
			}
			this.AppendStringToStringBuilder(builder, text2);
			num = num3 + 1;
		}
		if (num < length)
		{
			builder.Append(append_text, num, length - num);
		}
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00007743 File Offset: 0x00005943
	public string ConvertLocalizationKeysWithStringBuilder(string input_text)
	{
		this.m_StringBuilder.Remove(0, this.m_StringBuilder.Length);
		this.AppendStringToStringBuilder(this.m_StringBuilder, input_text);
		return this.m_StringBuilder.ToString();
	}

	// Token: 0x0400009A RID: 154
	private static LocalizationService s_Instance = null;

	// Token: 0x0400009B RID: 155
	private const string DefaultLocalizationLanguage = "EN";

	// Token: 0x0400009C RID: 156
	public static string LocalizationPath = "Localization/";

	// Token: 0x0400009D RID: 157
	public Action OnChangeLocalization;

	// Token: 0x0400009E RID: 158
	private static string s_localizationLanguage = "EN";

	// Token: 0x0400009F RID: 159
	private Dictionary<string, string> s_LocalizationLibrary;

	// Token: 0x040000A0 RID: 160
	public StringBuilder m_StringBuilder = new StringBuilder(1024);
}
