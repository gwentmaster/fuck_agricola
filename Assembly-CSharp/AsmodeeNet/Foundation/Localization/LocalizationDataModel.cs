using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x02000701 RID: 1793
	public class LocalizationDataModel
	{
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x001342B4 File Offset: 0x001324B4
		// (set) Token: 0x06003F66 RID: 16230 RVA: 0x001342BC File Offset: 0x001324BC
		public LocalizationManager.Language TargetLanguage { get; private set; }

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003F67 RID: 16231 RVA: 0x001342C8 File Offset: 0x001324C8
		public Dictionary<string, string> KeyToTranslation
		{
			get
			{
				return this._keyToTranslation.ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x0013431E File Offset: 0x0013251E
		// (set) Token: 0x06003F69 RID: 16233 RVA: 0x00134326 File Offset: 0x00132526
		public List<string> KeyNeedingTranslation { get; private set; } = new List<string>();

		// Token: 0x06003F6A RID: 16234 RVA: 0x0013432F File Offset: 0x0013252F
		private LocalizationDataModel()
		{
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x00134350 File Offset: 0x00132550
		public static LocalizationDataModel CreateModelFromTextAsset(TextAsset source)
		{
			LocalizationDataModel localizationDataModel = new LocalizationDataModel();
			localizationDataModel._xmlDocument = new XmlDocument();
			localizationDataModel._xmlDocument.LoadXml(source.text);
			localizationDataModel.TargetLanguage = LocalizationDataModel._GetFileTargetLanguage(localizationDataModel);
			if (localizationDataModel.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			localizationDataModel.Parse();
			return localizationDataModel;
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x001343A0 File Offset: 0x001325A0
		public static LocalizationDataModel CreateModelFromTextFile(string path)
		{
			LocalizationDataModel localizationDataModel = new LocalizationDataModel();
			localizationDataModel._xmlDocument = new XmlDocument();
			localizationDataModel._xmlDocument.Load(path);
			localizationDataModel.TargetLanguage = LocalizationDataModel._GetFileTargetLanguage(localizationDataModel);
			if (localizationDataModel.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			localizationDataModel.Parse();
			return localizationDataModel;
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x001343E8 File Offset: 0x001325E8
		public static LocalizationDataModel CreateModelFromString(string content)
		{
			LocalizationDataModel localizationDataModel = new LocalizationDataModel();
			localizationDataModel._xmlDocument = new XmlDocument();
			localizationDataModel._xmlDocument.LoadXml(content);
			localizationDataModel.TargetLanguage = LocalizationDataModel._GetFileTargetLanguage(localizationDataModel);
			if (localizationDataModel.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			localizationDataModel.Parse();
			return localizationDataModel;
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x00134430 File Offset: 0x00132630
		public static LocalizationDataModel CreateModelByMerging2Files(string importedFile, string previousFile)
		{
			LocalizationDataModel localizationDataModel = LocalizationDataModel.CreateModelFromTextFile(importedFile);
			LocalizationDataModel localizationDataModel2 = LocalizationDataModel.CreateModelFromTextFile(previousFile);
			if (localizationDataModel == null || localizationDataModel2 == null)
			{
				return null;
			}
			LocalizationDataModel localizationDataModel3 = new LocalizationDataModel();
			localizationDataModel3._xmlDocument = (localizationDataModel._xmlDocument.Clone() as XmlDocument);
			localizationDataModel3.TargetLanguage = localizationDataModel.TargetLanguage;
			if (localizationDataModel.TargetLanguage != localizationDataModel2.TargetLanguage)
			{
				return null;
			}
			bool flag = false;
			foreach (object obj in localizationDataModel._xmlDocument.GetElementsByTagName("trans-unit"))
			{
				XmlNode transUnit = (XmlNode)obj;
				flag = false;
				XmlNode xmlNode = LocalizationDataModel._GetSourceNodeFromTransUnit(transUnit);
				foreach (object obj2 in localizationDataModel._xmlDocument.GetElementsByTagName("trans-unit"))
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (LocalizationDataModel._GetSourceNodeFromTransUnit(transUnit).InnerText == xmlNode.InnerText)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					localizationDataModel3.AddTranslation(localizationDataModel.TargetLanguage, xmlNode.InnerText, LocalizationDataModel._GetTargetNodeFromTransUnit(transUnit).InnerText);
				}
			}
			return localizationDataModel3;
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x0013458C File Offset: 0x0013278C
		public Dictionary<string, string> Parse()
		{
			if (this.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			this.KeyNeedingTranslation.Clear();
			XmlNodeList elementsByTagName = this._xmlDocument.GetElementsByTagName("trans-unit");
			for (int i = 0; i < elementsByTagName.Count; i++)
			{
				XmlNode xmlNode = LocalizationDataModel._GetSourceNodeFromTransUnit(elementsByTagName[i]);
				XmlNode xmlNode2 = LocalizationDataModel._GetTargetNodeFromTransUnit(elementsByTagName[i]);
				if (xmlNode != null)
				{
					if (xmlNode2 == null)
					{
						AsmoLogger.Warning("LocalizationDataModel", string.Format("No traduction have been found for the key: '{0}' for the language: '{1}'. Key will be ignored at runtime.\nFile: {2}", xmlNode.InnerText, this.TargetLanguage, this._xmlDocument.BaseURI), null);
					}
					else
					{
						XmlNode namedItem = xmlNode2.Attributes.GetNamedItem("xml:lang");
						if (namedItem == null)
						{
							AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: '{0}' has a translation but not a target language associated. Key will be ignored at runtime\nFile: {1}", xmlNode.InnerText, this._xmlDocument.BaseURI), null);
						}
						else
						{
							if (namedItem.InnerText != this.TargetLanguage.ToXsdLanguage())
							{
								XmlNode xmlNode3 = xmlNode2.Attributes["state"];
								if (xmlNode3 == null || (!(xmlNode3.InnerText == "needs-translation") && !(xmlNode3.InnerText == "needs-review-translation")))
								{
									AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: '{0}' is defined for the following language: '{1}' which does not match the target language of the file : '{2}'. Key will be ignored at runtime\nFile: {3}", new object[]
									{
										xmlNode.InnerText,
										namedItem.InnerText,
										this.TargetLanguage,
										this._xmlDocument.BaseURI
									}), null);
									goto IL_1F6;
								}
								AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: '{0}' is still waiting for being translated. Key will be ignored at runtime\nFile: {1}", xmlNode.InnerText, this._xmlDocument.BaseURI), null);
								if (Application.isPlaying)
								{
									goto IL_1F6;
								}
								this.KeyNeedingTranslation.Add(xmlNode.InnerText);
							}
							if (dictionary.ContainsKey(xmlNode.InnerText))
							{
								AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: {0} is present more than one time in the file. The previous translation will be overridden.", xmlNode.InnerText), null);
							}
							else
							{
								dictionary[xmlNode.InnerText] = xmlNode2.InnerText;
							}
						}
					}
				}
				IL_1F6:;
			}
			this._keyToTranslation = (from x in dictionary
			select new KeyValuePair<string, string>(x.Key, x.Value)).ToList<KeyValuePair<string, string>>();
			return dictionary;
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x001347D0 File Offset: 0x001329D0
		public bool AddTranslation(LocalizationManager.Language targetLang, string key, string val)
		{
			if (this.TargetLanguage != targetLang)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to create the translation for the keyword: \"{0}\": the chosen _targetLang does not match with the file target lang", key), null);
				return false;
			}
			XmlNode xmlNode = this._xmlDocument.GetElementsByTagName("body")[0];
			XmlNode xmlNode2;
			if ((xmlNode2 = this._GetTransUnitFromKey(key)) != null)
			{
				if (LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode2) != null)
				{
					AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to create the translation for the keyword : \"{0}\" : this keyword already exists", key), null);
					return false;
				}
				XmlNode newChild = this._CreateTargetNode(val, targetLang);
				xmlNode2.AppendChild(newChild);
			}
			else
			{
				xmlNode2 = this._xmlDocument.CreateElement("trans-unit");
				XmlAttribute xmlAttribute = this._xmlDocument.CreateAttribute("xmlns");
				xmlAttribute.InnerText = this._xmlDocument.DocumentElement.NamespaceURI;
				xmlNode2.Attributes.Append(xmlAttribute);
				XmlAttribute xmlAttribute2 = this._xmlDocument.CreateAttribute("id");
				xmlAttribute2.InnerText = key.GetHashCode().ToString();
				xmlNode2.Attributes.Append(xmlAttribute2);
				XmlNode xmlNode3 = this._xmlDocument.CreateElement("source");
				xmlNode3.InnerText = key;
				XmlNode newChild = this._CreateTargetNode(val, targetLang);
				xmlNode2.AppendChild(xmlNode3);
				xmlNode2.AppendChild(newChild);
				xmlNode.AppendChild(xmlNode2);
			}
			this._keyToTranslation.Add(new KeyValuePair<string, string>(key, val));
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(key);
			}
			return true;
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x00134938 File Offset: 0x00132B38
		public bool UpdateTargetLangTranslation(LocalizationManager.Language targetLang, string key, string val)
		{
			if (this.TargetLanguage != targetLang)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation for the keyword : \"{0}\" : the chosen _targetLang does not match with the file target lang", key), null);
				return false;
			}
			XmlNode xmlNode = this._GetTransUnitFromKey(key);
			if (xmlNode == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation for the keyword : \"{0}\" : this keyword does not exist yet", key), null);
				return false;
			}
			XmlNode xmlNode2 = LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode);
			if (xmlNode2 == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation for the keyword : \"{0}\" : this keyword has no target field and you try to update it", key), null);
				return false;
			}
			xmlNode.RemoveChild(xmlNode2);
			xmlNode2 = this._CreateTargetNode(val, targetLang);
			xmlNode.AppendChild(xmlNode2);
			KeyValuePair<string, string> item = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
			this._keyToTranslation[this._keyToTranslation.IndexOf(item)] = new KeyValuePair<string, string>(key, val);
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(key);
			}
			return true;
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x00134A3C File Offset: 0x00132C3C
		public bool RemoveTranslation(string key)
		{
			XmlNode xmlNode = this._GetTransUnitFromKey(key);
			if (xmlNode == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to remove the translation associated with the keyword : \"{0}\" : this keyword does not exist", key), null);
				return false;
			}
			xmlNode.ParentNode.RemoveChild(xmlNode);
			this._keyToTranslation.Remove(this._keyToTranslation.Single((KeyValuePair<string, string> x) => x.Key == key));
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(key);
			}
			return true;
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00134AD0 File Offset: 0x00132CD0
		public bool UpdateKey(string oldKey, string newKey)
		{
			XmlNode xmlNode = this._GetTransUnitFromKey(oldKey);
			if (xmlNode == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation associated with the keyword : \"{0}\" : this keyword does not exist", oldKey), null);
				return false;
			}
			XmlNode xmlNode2 = LocalizationDataModel._GetSourceNodeFromTransUnit(xmlNode);
			if (xmlNode2 != null)
			{
				xmlNode2.InnerText = newKey;
			}
			XmlAttribute xmlAttribute = xmlNode.Attributes["id"];
			if (xmlAttribute != null)
			{
				xmlAttribute.InnerText = newKey.GetHashCode().ToString();
			}
			KeyValuePair<string, string> item = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == oldKey);
			KeyValuePair<string, string> value = new KeyValuePair<string, string>(newKey, item.Value);
			this._keyToTranslation[this._keyToTranslation.IndexOf(item)] = value;
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(newKey);
			}
			return true;
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x00134BA8 File Offset: 0x00132DA8
		public void MoveKeyUp(string key)
		{
			KeyValuePair<string, string> item = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
			int num = this._keyToTranslation.IndexOf(item);
			this._MoveKey(num, num - 1);
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x00134BF4 File Offset: 0x00132DF4
		public void MoveKeyDown(string key)
		{
			KeyValuePair<string, string> item = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
			int num = this._keyToTranslation.IndexOf(item);
			this._MoveKey(num, num + 1);
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x00134C40 File Offset: 0x00132E40
		public void SortKeys()
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(this._keyToTranslation);
			list.Sort((KeyValuePair<string, string> kTT1, KeyValuePair<string, string> kTT2) => string.Compare(kTT1.Key, kTT2.Key, true));
			for (int i = 0; i < list.Count; i++)
			{
				string key = list[i].Key;
				KeyValuePair<string, string> item = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
				int sourceIdx = this._keyToTranslation.IndexOf(item);
				this._MoveKey(sourceIdx, i);
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00134CDC File Offset: 0x00132EDC
		private void _MoveKey(int sourceIdx, int destinationIdx)
		{
			if (sourceIdx == destinationIdx)
			{
				return;
			}
			if (sourceIdx < 0 || destinationIdx < 0 || sourceIdx >= this._keyToTranslation.Count || destinationIdx >= this._keyToTranslation.Count)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Index out of bounds sourceIdx: {0} destinationIdx: {1} [0..{2}]", sourceIdx, destinationIdx, this._keyToTranslation.Count - 1), null);
			}
			string key = this._keyToTranslation[sourceIdx].Key;
			XmlNode xmlNode = this._GetTransUnitFromKey(key);
			string key2 = this._keyToTranslation[destinationIdx].Key;
			XmlNode xmlNode2 = this._GetTransUnitFromKey(key2);
			AsmoLogger.Warning("LocalizationDataModel", string.Format("{0} {1} - {2} {3}", new object[]
			{
				sourceIdx,
				key,
				destinationIdx,
				key2
			}), null);
			xmlNode.ParentNode.RemoveChild(xmlNode);
			xmlNode2.ParentNode.InsertBefore(xmlNode, xmlNode2);
			KeyValuePair<string, string> item = this._keyToTranslation[sourceIdx];
			this._keyToTranslation.RemoveAt(sourceIdx);
			this._keyToTranslation.Insert(destinationIdx, item);
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00134DF9 File Offset: 0x00132FF9
		public void SaveModification(string path = null)
		{
			if (this.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return;
			}
			this._xmlDocument.Save(path ?? new Uri(this._xmlDocument.BaseURI).LocalPath);
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00134E2C File Offset: 0x0013302C
		public void ExportFile(string path, LocalizationManager.Language exportLanguage, Dictionary<string, string> alreadyTranslatedKeys)
		{
			XmlDocument xmlDocument = this._xmlDocument.CloneNode(true) as XmlDocument;
			xmlDocument.GetElementsByTagName("file")[0].Attributes["target-language"].Value = exportLanguage.ToXsdLanguage();
			foreach (object obj in xmlDocument.GetElementsByTagName("trans-unit"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlNode xmlNode2 = LocalizationDataModel._GetSourceNodeFromTransUnit(xmlNode);
				if (xmlNode2 != null)
				{
					if (alreadyTranslatedKeys.ContainsKey(xmlNode2.InnerText))
					{
						XmlNode xmlNode3 = LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode);
						if (xmlNode3 != null)
						{
							xmlNode3.InnerText = alreadyTranslatedKeys[xmlNode2.InnerText];
							xmlNode3.Attributes["xml:lang"].Value = exportLanguage.ToXsdLanguage();
						}
						else
						{
							xmlNode.AppendChild(this._CreateTargetNode(alreadyTranslatedKeys[xmlNode2.InnerText], exportLanguage));
						}
					}
					else
					{
						XmlNode xmlNode4 = LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode);
						XmlAttribute xmlAttribute = xmlNode4.Attributes["state"];
						if (xmlAttribute == null)
						{
							xmlAttribute = xmlDocument.CreateAttribute("state");
							xmlNode4.Attributes.Append(xmlAttribute);
						}
						xmlNode4.Attributes["xml:lang"].Value = exportLanguage.ToXsdLanguage();
						xmlAttribute.Value = "needs-translation";
					}
				}
			}
			xmlDocument.Save(path);
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003F7A RID: 16250 RVA: 0x00134FA8 File Offset: 0x001331A8
		public string GetFileName
		{
			get
			{
				return Path.GetFileName(this._xmlDocument.BaseURI);
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06003F7B RID: 16251 RVA: 0x00134FBA File Offset: 0x001331BA
		public string GetUri
		{
			get
			{
				return this._xmlDocument.BaseURI;
			}
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x00134FC8 File Offset: 0x001331C8
		private static LocalizationManager.Language _GetFileTargetLanguage(LocalizationDataModel dm)
		{
			string value = dm._xmlDocument.GetElementsByTagName("file")[0].Attributes.GetNamedItem("target-language").Value;
			LocalizationManager.Language language = LanguageHelper.LanguageFromXsdLanguage(value);
			if (language == LocalizationManager.Language.unknown)
			{
				AsmoLogger.Error("LocalizationDataModel", string.Format("target-language {0} is not supported. Unable to use this file: {1}", value, dm._xmlDocument.BaseURI), null);
			}
			return language;
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0013502C File Offset: 0x0013322C
		private XmlNode _GetTransUnitFromKey(string key)
		{
			XmlNodeList elementsByTagName = this._xmlDocument.GetElementsByTagName("trans-unit");
			for (int i = 0; i < elementsByTagName.Count; i++)
			{
				for (int j = 0; j < elementsByTagName[i].ChildNodes.Count; j++)
				{
					if (elementsByTagName[i].ChildNodes[j].Name == "source" && elementsByTagName[i].ChildNodes[j].InnerText == key)
					{
						return elementsByTagName[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x001350C4 File Offset: 0x001332C4
		private static XmlNode _GetSourceNodeFromTransUnit(XmlNode transUnit)
		{
			foreach (object obj in transUnit.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "source")
				{
					return xmlNode;
				}
			}
			return null;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x00135130 File Offset: 0x00133330
		private static XmlNode _GetTargetNodeFromTransUnit(XmlNode transUnit)
		{
			foreach (object obj in transUnit.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "target")
				{
					return xmlNode;
				}
			}
			return null;
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0013519C File Offset: 0x0013339C
		private XmlNode _CreateTargetNode(string val, LocalizationManager.Language _targetLang)
		{
			XmlElement xmlElement = this._xmlDocument.CreateElement("target", this._xmlDocument.DocumentElement.NamespaceURI);
			xmlElement.InnerText = val;
			XmlAttribute xmlAttribute = this._xmlDocument.CreateAttribute("xml:lang");
			xmlAttribute.InnerText = _targetLang.ToXsdLanguage();
			xmlElement.Attributes.Append(xmlAttribute);
			return xmlElement;
		}

		// Token: 0x0400289A RID: 10394
		private const string _kModuleName = "LocalizationDataModel";

		// Token: 0x0400289B RID: 10395
		private XmlDocument _xmlDocument;

		// Token: 0x0400289D RID: 10397
		private List<KeyValuePair<string, string>> _keyToTranslation = new List<KeyValuePair<string, string>>();
	}
}
