using System;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x02000704 RID: 1796
	public class LocalizedText : MonoBehaviour
	{
		// Token: 0x06003F99 RID: 16281 RVA: 0x00135A74 File Offset: 0x00133C74
		private void Awake()
		{
			this._txt = base.GetComponent<Text>();
			if (this._txt == null)
			{
				this._tmpTxt = base.GetComponent<TextMeshProUGUI>();
				if (this._tmpTxt == null)
				{
					AsmoLogger.Error("LocalizedText", string.Format("Missing Text or TextMeshProUGUI component in {0}", base.gameObject.name), null);
				}
			}
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x00135AD8 File Offset: 0x00133CD8
		private void OnEnable()
		{
			LocalizationManager localizationManager = CoreApplication.Instance.LocalizationManager;
			localizationManager.OnLanguageChanged += this._OnLanguageChanged;
			this._UpdateDisplayedText(localizationManager.GetLocalizedText(this.key));
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x00135B14 File Offset: 0x00133D14
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.LocalizationManager.OnLanguageChanged -= this._OnLanguageChanged;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x00135B39 File Offset: 0x00133D39
		private void _OnLanguageChanged(LocalizationManager localizationManager)
		{
			this._UpdateDisplayedText(localizationManager.GetLocalizedText(this.key));
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x00135B4D File Offset: 0x00133D4D
		private void _UpdateDisplayedText(string msg)
		{
			if (this._txt != null)
			{
				this._txt.text = msg;
				return;
			}
			this._tmpTxt.text = msg;
		}

		// Token: 0x040028AB RID: 10411
		private const string _documentation = "<b>LocalizedText</b> automatically retrieves the <b>key</b> to localize. Check the <b>Asmodee.net/Localization</b> menu.";

		// Token: 0x040028AC RID: 10412
		private const string _kModuleName = "LocalizedText";

		// Token: 0x040028AD RID: 10413
		public string key;

		// Token: 0x040028AE RID: 10414
		private Text _txt;

		// Token: 0x040028AF RID: 10415
		private TextMeshProUGUI _tmpTxt;
	}
}
