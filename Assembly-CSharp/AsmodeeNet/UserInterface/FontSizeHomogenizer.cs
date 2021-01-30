using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000636 RID: 1590
	public class FontSizeHomogenizer : MonoBehaviour
	{
		// Token: 0x06003A87 RID: 14983 RVA: 0x00122DA8 File Offset: 0x00120FA8
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.Preferences.AspectDidChange += this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.LocalizationManager.OnLanguageChanged += this._OnLanguageChanged;
			this.SetNeedsFontSizeUpdate();
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x00122E0C File Offset: 0x0012100C
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.Preferences.AspectDidChange -= this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.LocalizationManager.OnLanguageChanged -= this._OnLanguageChanged;
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x00122E72 File Offset: 0x00121072
		private void OnRectTransformDimensionsChange()
		{
			this.SetNeedsFontSizeUpdate();
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x00122E72 File Offset: 0x00121072
		private void _OnLanguageChanged(LocalizationManager localizationManager)
		{
			this.SetNeedsFontSizeUpdate();
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x00122E7A File Offset: 0x0012107A
		public void SetNeedsFontSizeUpdate()
		{
			this._updateCount = 2;
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x00122E83 File Offset: 0x00121083
		private void LateUpdate()
		{
			if (this._updateCount > 0)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this._UpdateFontSizeAndWait());
			}
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x00122EA1 File Offset: 0x001210A1
		private IEnumerator _UpdateFontSizeAndWait()
		{
			while (this._updateCount > 0)
			{
				this._UpdateFontSize();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x00122EB0 File Offset: 0x001210B0
		private void _UpdateFontSize()
		{
			IEnumerable<TextMeshProUGUI> enumerable = from x in this.labels
			where x.IsActive()
			select x;
			if (!enumerable.Any<TextMeshProUGUI>())
			{
				return;
			}
			foreach (TextMeshProUGUI textMeshProUGUI in enumerable)
			{
				textMeshProUGUI.enableAutoSizing = true;
				textMeshProUGUI.ForceMeshUpdate();
			}
			float num = enumerable.Min((TextMeshProUGUI label) => label.fontSize);
			foreach (TextMeshProUGUI textMeshProUGUI2 in enumerable)
			{
				textMeshProUGUI2.fontSize = num;
				textMeshProUGUI2.enableAutoSizing = false;
			}
			this._updateCount--;
			if (this._updateCount == 0 && !MathUtils.Approximately(this._stabilizedFontSize, num, 1f))
			{
				this._updateCount = 1;
			}
			this._stabilizedFontSize = num;
		}

		// Token: 0x040025E8 RID: 9704
		private const string _documentation = "Based on <i>Auto Sizing</i> all the referenced TextMeshProUGUI texts will have the same font size.";

		// Token: 0x040025E9 RID: 9705
		public TextMeshProUGUI[] labels;

		// Token: 0x040025EA RID: 9706
		private const int _minimalUpdateCount = 2;

		// Token: 0x040025EB RID: 9707
		private int _updateCount;

		// Token: 0x040025EC RID: 9708
		private float _stabilizedFontSize;
	}
}
