using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x02000703 RID: 1795
	public class LocalizationManager
	{
		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003F83 RID: 16259 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public LocalizationManager.Language DefaultLanguage
		{
			get
			{
				return LocalizationManager.Language.en_US;
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06003F84 RID: 16260 RVA: 0x00135270 File Offset: 0x00133470
		// (remove) Token: 0x06003F85 RID: 16261 RVA: 0x001352A8 File Offset: 0x001334A8
		public event Action<LocalizationManager> OnLanguageChanged;

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003F86 RID: 16262 RVA: 0x001352DD File Offset: 0x001334DD
		// (set) Token: 0x06003F87 RID: 16263 RVA: 0x001352FC File Offset: 0x001334FC
		public LocalizationManager.Language CurrentLanguage
		{
			get
			{
				if (this._currentLanguage == null)
				{
					return LocalizationManager.Language.en_US;
				}
				return this._currentLanguage.Value;
			}
			set
			{
				if (this._supportedLanguages.Contains(value))
				{
					this._currentLanguage = new LocalizationManager.Language?(value);
				}
				else
				{
					AsmoLogger.Warning("LocalizationManager", string.Format("{0} is not part of supported languages (Check CoreApplication > Supported Languages). Fall back to default: {1}", value.ToString(), LocalizationManager.Language.en_US.ToString()), null);
					this._currentLanguage = new LocalizationManager.Language?(LocalizationManager.Language.en_US);
				}
				this._WritePreferredLanguage();
				this._LoadLanguage();
				Action<LocalizationManager> onLanguageChanged = this.OnLanguageChanged;
				if (onLanguageChanged == null)
				{
					return;
				}
				onLanguageChanged(this);
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06003F88 RID: 16264 RVA: 0x00135380 File Offset: 0x00133580
		public string CurrentLanguageCode
		{
			get
			{
				return this.CurrentLanguage.ToString().Substring(0, 2);
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06003F89 RID: 16265 RVA: 0x001353A8 File Offset: 0x001335A8
		public string DefaultLanguageCode
		{
			get
			{
				return LocalizationManager.Language.en_US.ToString().Substring(0, 2);
			}
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x001353CC File Offset: 0x001335CC
		public LocalizationManager.Language GetLanguageFromString(string lang)
		{
			if (string.IsNullOrWhiteSpace(lang) || (lang.Length != 2 && lang.Length != 5))
			{
				return this.DefaultLanguage;
			}
			LocalizationManager.Language result;
			if (Enum.TryParse<LocalizationManager.Language>(lang, out result))
			{
				return result;
			}
			foreach (string text in Enum.GetNames(typeof(LocalizationManager.Language)))
			{
				if (text.StartsWith(lang, StringComparison.Ordinal))
				{
					return (LocalizationManager.Language)Enum.Parse(typeof(LocalizationManager.Language), text);
				}
			}
			return this.DefaultLanguage;
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06003F8B RID: 16267 RVA: 0x00135450 File Offset: 0x00133650
		// (remove) Token: 0x06003F8C RID: 16268 RVA: 0x00135488 File Offset: 0x00133688
		public event LocalizationManager.LocalizationManagerInitialized LocalizationManagerInitializedEvent;

		// Token: 0x06003F8D RID: 16269 RVA: 0x001354C0 File Offset: 0x001336C0
		public LocalizationManager(List<LocalizationManager.Language> supportedLanguages)
		{
			this._supportedLanguages = (supportedLanguages ?? new List<LocalizationManager.Language>());
			AsmoLogger.Info("LocalizationManager", "Supported Languages: " + string.Join(", ", from lang in this._supportedLanguages
			select lang.ToString()), null);
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x001355BA File Offset: 0x001337BA
		public void Init()
		{
			this._LoadXliffFiles();
			this._InitLanguage();
			LocalizationManager.LocalizationManagerInitialized localizationManagerInitializedEvent = this.LocalizationManagerInitializedEvent;
			if (localizationManagerInitializedEvent != null)
			{
				localizationManagerInitializedEvent();
			}
			Action<LocalizationManager> onLanguageChanged = this.OnLanguageChanged;
			if (onLanguageChanged == null)
			{
				return;
			}
			onLanguageChanged(this);
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x001355EA File Offset: 0x001337EA
		public bool HasTranslationInCurrentLanguage(string key)
		{
			return this._ready && !this._useDefaultLanguage && this._keyToLocalizedText.ContainsKey(key);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0013560C File Offset: 0x0013380C
		public string GetLocalizedText(string key)
		{
			if (!this._ready)
			{
				return key;
			}
			if (!this._keyToLocalizedText.ContainsKey(key))
			{
				AsmoLogger.Warning("LocalizationManager", string.Format("The key: \"{0}\" is not defined in the .xliff file and therefore doesn't have any localization", key), null);
				return key;
			}
			if (this._keyToLocalizedText[key] == null)
			{
				AsmoLogger.Warning("LocalizationManager", string.Format("The key: \"{0}\" is defined but has no translation available", key), null);
				return key;
			}
			return this._keyToLocalizedText[key];
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x0013567C File Offset: 0x0013387C
		private void _WritePreferredLanguage()
		{
			string text = this.CurrentLanguage.ToString();
			AsmoLogger.Info("LocalizationManager", "Save language preference: " + text, null);
			KeyValueStore.SetString("CurrentLanguage", text);
			KeyValueStore.Save();
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x001356C4 File Offset: 0x001338C4
		private LocalizationManager.Language? _ReadPreferredLanguage()
		{
			string @string = KeyValueStore.GetString("CurrentLanguage", "");
			if (string.IsNullOrEmpty(@string))
			{
				AsmoLogger.Warning("LocalizationManager", "Couldn't find a saved language in PlayerPrefs", null);
				return null;
			}
			AsmoLogger.Info("LocalizationManager", "Using saved language preference: " + @string, null);
			return new LocalizationManager.Language?(LocalizationManager._GetLanguageFromString(@string));
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x00135724 File Offset: 0x00133924
		private LocalizationManager.Language? _GetSystemLanguage()
		{
			SystemLanguage systemLanguage = Application.systemLanguage;
			if (this._unityLanguageToIsoLanguage.ContainsKey(systemLanguage))
			{
				LocalizationManager.Language language = this._unityLanguageToIsoLanguage[systemLanguage];
				AsmoLogger.Info("LocalizationManager", "Using system language: " + language, null);
				return new LocalizationManager.Language?(language);
			}
			AsmoLogger.Warning("LocalizationManager", "System language is not supported: " + systemLanguage, null);
			return null;
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x00135798 File Offset: 0x00133998
		private void _LoadLanguage()
		{
			this._ready = false;
			this._keyToLocalizedText = null;
			IEnumerable<TextAsset> enumerable = from x in this.xliffFiles
			where XliffUtility.GetXliffTargetLangFromXml(x.text) == this.CurrentLanguage
			select x;
			if (enumerable.Count<TextAsset>() == 0)
			{
				this._useDefaultLanguage = true;
				enumerable = from x in this.xliffFiles
				where XliffUtility.GetXliffTargetLangFromXml(x.text) == LocalizationManager.Language.en_US
				select x;
				AsmoLogger.Warning("LocalizationManager", string.Format("No translation file for the language: {0} has been found, English (en-US) will be used instead", this.CurrentLanguage), null);
			}
			else
			{
				this._useDefaultLanguage = false;
			}
			foreach (TextAsset source in enumerable)
			{
				LocalizationDataModel localizationDataModel = LocalizationDataModel.CreateModelFromTextAsset(source);
				if (this._keyToLocalizedText == null)
				{
					this._keyToLocalizedText = localizationDataModel.Parse();
				}
				else
				{
					Dictionary<string, string> dictionary = localizationDataModel.Parse();
					IEnumerable<string> enumerable2 = this._keyToLocalizedText.Keys.Intersect(dictionary.Keys);
					if (enumerable2.Count<string>() != 0)
					{
						AsmoLogger.Warning("LocalizationManager", "2 files managing the same \"target-language\" have duplicated keys. Keys are", null);
						foreach (string message in enumerable2)
						{
							AsmoLogger.Warning("LocalizationManager", message, null);
						}
					}
					this._keyToLocalizedText = this._keyToLocalizedText.Union(dictionary, new CustomDictionnaryComparer()).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
				}
			}
			this._ready = (this._keyToLocalizedText != null);
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x00135984 File Offset: 0x00133B84
		private void _LoadXliffFiles()
		{
			this.xliffFiles = Resources.LoadAll<TextAsset>("LocalizationAsmodee/").ToArray<TextAsset>();
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x0013599C File Offset: 0x00133B9C
		private void _InitLanguage()
		{
			if (this._currentLanguage == null)
			{
				this._currentLanguage = this._ReadPreferredLanguage();
			}
			if (this._currentLanguage == null)
			{
				this._currentLanguage = this._GetSystemLanguage();
			}
			if (this._currentLanguage == null)
			{
				AsmoLogger.Warning("LocalizationManager", "Couldn't detect language, falling back on default: " + LocalizationManager.Language.en_US, null);
				this._currentLanguage = new LocalizationManager.Language?(LocalizationManager.Language.en_US);
			}
			this.CurrentLanguage = this._currentLanguage.Value;
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x00135A20 File Offset: 0x00133C20
		private static LocalizationManager.Language _GetLanguageFromString(string lang)
		{
			LocalizationManager.Language result;
			try
			{
				result = (LocalizationManager.Language)Enum.Parse(typeof(LocalizationManager.Language), lang);
			}
			catch
			{
				result = LocalizationManager.Language.en_US;
			}
			return result;
		}

		// Token: 0x0400289F RID: 10399
		private const string _kModuleName = "LocalizationManager";

		// Token: 0x040028A0 RID: 10400
		private const string _kCurrentLanguagePreferenceKey = "CurrentLanguage";

		// Token: 0x040028A1 RID: 10401
		private const LocalizationManager.Language _kDefaultLanguage = LocalizationManager.Language.en_US;

		// Token: 0x040028A3 RID: 10403
		private LocalizationManager.Language? _currentLanguage;

		// Token: 0x040028A4 RID: 10404
		private bool _useDefaultLanguage;

		// Token: 0x040028A5 RID: 10405
		private List<LocalizationManager.Language> _supportedLanguages;

		// Token: 0x040028A6 RID: 10406
		private readonly Dictionary<SystemLanguage, LocalizationManager.Language> _unityLanguageToIsoLanguage = new Dictionary<SystemLanguage, LocalizationManager.Language>
		{
			{
				SystemLanguage.Chinese,
				LocalizationManager.Language.zh_CN
			},
			{
				SystemLanguage.ChineseSimplified,
				LocalizationManager.Language.zh_CHS
			},
			{
				SystemLanguage.ChineseTraditional,
				LocalizationManager.Language.zh_CHT
			},
			{
				SystemLanguage.Dutch,
				LocalizationManager.Language.nl_NL
			},
			{
				SystemLanguage.English,
				LocalizationManager.Language.en_US
			},
			{
				SystemLanguage.French,
				LocalizationManager.Language.fr_FR
			},
			{
				SystemLanguage.German,
				LocalizationManager.Language.de_DE
			},
			{
				SystemLanguage.Italian,
				LocalizationManager.Language.it_IT
			},
			{
				SystemLanguage.Japanese,
				LocalizationManager.Language.ja_JP
			},
			{
				SystemLanguage.Korean,
				LocalizationManager.Language.ko_KR
			},
			{
				SystemLanguage.Portuguese,
				LocalizationManager.Language.pt_PT
			},
			{
				SystemLanguage.Russian,
				LocalizationManager.Language.ru_RU
			},
			{
				SystemLanguage.Spanish,
				LocalizationManager.Language.es_ES
			},
			{
				SystemLanguage.Swedish,
				LocalizationManager.Language.sv_SE
			}
		};

		// Token: 0x040028A7 RID: 10407
		public TextAsset[] xliffFiles;

		// Token: 0x040028A8 RID: 10408
		private Dictionary<string, string> _keyToLocalizedText;

		// Token: 0x040028A9 RID: 10409
		private bool _ready;

		// Token: 0x020009EC RID: 2540
		[Serializable]
		public enum Language
		{
			// Token: 0x0400338B RID: 13195
			unknown,
			// Token: 0x0400338C RID: 13196
			zh_CN,
			// Token: 0x0400338D RID: 13197
			zh_CHS,
			// Token: 0x0400338E RID: 13198
			zh_CHT,
			// Token: 0x0400338F RID: 13199
			nl_NL,
			// Token: 0x04003390 RID: 13200
			en_US,
			// Token: 0x04003391 RID: 13201
			fr_FR,
			// Token: 0x04003392 RID: 13202
			de_DE,
			// Token: 0x04003393 RID: 13203
			it_IT,
			// Token: 0x04003394 RID: 13204
			ja_JP,
			// Token: 0x04003395 RID: 13205
			ko_KR,
			// Token: 0x04003396 RID: 13206
			pt_PT,
			// Token: 0x04003397 RID: 13207
			ru_RU,
			// Token: 0x04003398 RID: 13208
			es_ES,
			// Token: 0x04003399 RID: 13209
			sv_SE
		}

		// Token: 0x020009ED RID: 2541
		// (Invoke) Token: 0x0600497A RID: 18810
		public delegate void LocalizationManagerInitialized();
	}
}
