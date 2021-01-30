using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001F RID: 31
public class UILocalizedText : MonoBehaviour
{
	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000156 RID: 342 RVA: 0x000077A9 File Offset: 0x000059A9
	// (set) Token: 0x06000157 RID: 343 RVA: 0x000077B1 File Offset: 0x000059B1
	public string KeyText
	{
		get
		{
			return this.m_KeyText;
		}
		set
		{
			this.m_KeyText = value;
			this.Localize();
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000077C0 File Offset: 0x000059C0
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06000159 RID: 345 RVA: 0x000077C8 File Offset: 0x000059C8
	private void Initialize()
	{
		LocalizationService instance = LocalizationService.Instance;
		instance.OnChangeLocalization = (Action)Delegate.Combine(instance.OnChangeLocalization, new Action(this.OnChangeLocalization));
		this.UiText = base.gameObject.GetComponent<Text>();
		this.m_TextTMP = base.gameObject.GetComponent<TextMeshProUGUI>();
		this.m_TextTMP_3D = base.gameObject.GetComponent<TextMeshPro>();
		this.OnChangeLocalization();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00007834 File Offset: 0x00005A34
	private void OnChangeLocalization()
	{
		this.Localize();
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000783C File Offset: 0x00005A3C
	private void Localize()
	{
		string text = string.Empty;
		if (this.m_KeyText.Contains("${"))
		{
			text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(this.m_KeyText);
		}
		else
		{
			text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${" + this.m_KeyText + "}");
		}
		this.SetTextValue((text != string.Empty) ? text : this.m_KeyText);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x000078B0 File Offset: 0x00005AB0
	private void SetTextValue(string text)
	{
		if (this.UiText != null)
		{
			this.UiText.text = text;
		}
		if (this.m_TextTMP != null)
		{
			this.m_TextTMP.SetText(text);
		}
		if (this.m_TextTMP_3D != null)
		{
			this.m_TextTMP_3D.SetText(text);
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000790B File Offset: 0x00005B0B
	private void OnDestroy()
	{
		LocalizationService instance = LocalizationService.Instance;
		instance.OnChangeLocalization = (Action)Delegate.Remove(instance.OnChangeLocalization, new Action(this.OnChangeLocalization));
	}

	// Token: 0x040000A1 RID: 161
	[SerializeField]
	[TextArea(5, 20)]
	private string m_KeyText;

	// Token: 0x040000A2 RID: 162
	private Text UiText;

	// Token: 0x040000A3 RID: 163
	private TextMeshProUGUI m_TextTMP;

	// Token: 0x040000A4 RID: 164
	private TextMeshPro m_TextTMP_3D;
}
