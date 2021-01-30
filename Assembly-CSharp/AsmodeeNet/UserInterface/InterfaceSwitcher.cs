using System;
using System.Linq;
using System.Runtime.CompilerServices;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200063B RID: 1595
	public class InterfaceSwitcher : MonoBehaviour
	{
		// Token: 0x06003AA0 RID: 15008 RVA: 0x00123508 File Offset: 0x00121708
		private void Awake()
		{
			this._languages = (from x in CoreApplication.Instance.LocalizationManager.xliffFiles
			select XliffUtility.GetXliffTargetLangFromXml(x.text)).Distinct<LocalizationManager.Language>().ToArray<LocalizationManager.Language>();
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x00123558 File Offset: 0x00121758
		private void Update()
		{
			if (KeyCombinationChecker.IsDebugKeyCombination())
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					CoreApplication.Instance.Preferences.InterfaceDisplayMode = Preferences.DisplayMode.Small;
					return;
				}
				if (Input.GetKeyDown(KeyCode.R))
				{
					CoreApplication.Instance.Preferences.InterfaceDisplayMode = Preferences.DisplayMode.Regular;
					return;
				}
				if (Input.GetKeyDown(KeyCode.B))
				{
					CoreApplication.Instance.Preferences.InterfaceDisplayMode = Preferences.DisplayMode.Big;
					return;
				}
				for (int i = 0; i < this._keyCodes.Length; i++)
				{
					if (Input.GetKeyDown(this._keyCodes[i]))
					{
						int num = Mathf.Min(i, this._languages.Length - 1);
						CoreApplication.Instance.LocalizationManager.CurrentLanguage = this._languages[num];
					}
				}
			}
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x00123607 File Offset: 0x00121807
		public InterfaceSwitcher()
		{
			KeyCode[] array = new KeyCode[9];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.8EC8BC14FB8B7B53D3B439F977F9DE17FACD74CD).FieldHandle);
			this._keyCodes = array;
			base..ctor();
		}

		// Token: 0x040025FF RID: 9727
		private const string _documentation = "[Ctrl] + [Alt] + S ➜ Small\n[Ctrl] + [Alt] + R ➜ Regular\n[Ctrl] + [Alt] + B ➜ Big\n\n[Ctrl] + [Alt] + [F1], [F2] ... ➜ Language ";

		// Token: 0x04002600 RID: 9728
		private LocalizationManager.Language[] _languages;

		// Token: 0x04002601 RID: 9729
		private KeyCode[] _keyCodes;
	}
}
