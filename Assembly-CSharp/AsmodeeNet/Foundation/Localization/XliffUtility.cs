using System;
using System.Xml;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x02000705 RID: 1797
	public static class XliffUtility
	{
		// Token: 0x06003F9F RID: 16287 RVA: 0x00135B76 File Offset: 0x00133D76
		public static LocalizationManager.Language GetXliffTargetLang(string xliffFilePath)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xliffFilePath);
			return LanguageHelper.LanguageFromXsdLanguage(xmlDocument.GetElementsByTagName("file")[0].Attributes.GetNamedItem("target-language").Value);
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x00135BAD File Offset: 0x00133DAD
		public static LocalizationManager.Language GetXliffTargetLangFromXml(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			return LanguageHelper.LanguageFromXsdLanguage(xmlDocument.GetElementsByTagName("file")[0].Attributes.GetNamedItem("target-language").Value);
		}
	}
}
