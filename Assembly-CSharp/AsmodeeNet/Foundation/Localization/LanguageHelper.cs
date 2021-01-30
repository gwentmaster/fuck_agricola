using System;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x02000702 RID: 1794
	public static class LanguageHelper
	{
		// Token: 0x06003F81 RID: 16257 RVA: 0x001351FA File Offset: 0x001333FA
		public static string ToXsdLanguage(this LocalizationManager.Language lang)
		{
			return lang.ToString().Replace("_", "-");
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x00135218 File Offset: 0x00133418
		public static LocalizationManager.Language LanguageFromXsdLanguage(string xsdLang)
		{
			if (string.IsNullOrEmpty(xsdLang))
			{
				return LocalizationManager.Language.unknown;
			}
			LocalizationManager.Language result;
			try
			{
				result = (LocalizationManager.Language)Enum.Parse(typeof(LocalizationManager.Language), xsdLang.Replace("-", "_"));
			}
			catch
			{
				result = LocalizationManager.Language.unknown;
			}
			return result;
		}
	}
}
