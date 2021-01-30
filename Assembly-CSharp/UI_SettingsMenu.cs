using System;
using System.Collections;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012B RID: 299
public class UI_SettingsMenu : UI_FrontEndAndInGameScene, INotifyParentMenu
{
	// Token: 0x06000B81 RID: 2945 RVA: 0x000500A0 File Offset: 0x0004E2A0
	public virtual void OnEnterMenu()
	{
		base.SetIsInGame(false);
		if (this.m_audioSettingsHandler != null)
		{
			this.m_audioSettingsHandler.RetrieveAudioSliderSettings(this.m_musicVolumeSlider, this.m_effectsVolumeSlider);
			this.m_audioSettingsHandler.DisableToggleSoundEffects();
		}
		else
		{
			this.m_musicVolumeSlider.value = 0f;
			this.m_effectsVolumeSlider.value = 0f;
			Debug.Log("Warning: Audio settings manager not connected to settings screen!");
		}
		if (this.m_versionText != null)
		{
			this.m_versionText.enabled = true;
			this.m_versionText.text = VersionManager.instance.GetVersionTextString();
		}
		int @int = PlayerPrefs.GetInt("Option_Confirmations", 1);
		this.m_confirmationsToggle.isOn = (@int == 1);
		this.m_animSpeedValue = PlayerPrefs.GetFloat("Option_AnimationSpeed", 1f);
		this.OnAnimSpeedChanged();
		this.m_gameSpeedValue = PlayerPrefs.GetInt("Option_GameSpeed", 1);
		this.OnGameSpeedChanged();
		if (this.m_resolutionCountdownPopupText != null)
		{
			this.m_resolutionCountdownPopupText.text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${KeepThisResolution}");
		}
		string localizationLanguage = LocalizationService.Instance.GetLocalizationLanguage();
		for (int i = 0; i < this.m_languageToggles.Length; i++)
		{
			this.m_languageToggles[i].isOn = (this.m_languagePrefixes[i] == localizationLanguage);
			if (this.m_languagePrefixes[i] == localizationLanguage)
			{
				this.m_languageButtonText.text = this.m_languageTexts[i].text;
			}
		}
		this.m_bIgnoreToggles = false;
		if (this.m_audioSettingsHandler != null)
		{
			this.m_audioSettingsHandler.EnableToggleSoundEffects();
		}
		this.HandleWindowModeChange();
		if (this.m_resoultionTitleText != null)
		{
			this.m_resoultionTitleText.text = Screen.width.ToString() + "x" + Screen.height.ToString();
		}
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00050278 File Offset: 0x0004E478
	public virtual void OnExitMenu(bool bUnderPopup)
	{
		if (this.m_audioSettingsHandler != null)
		{
			this.m_audioSettingsHandler.StoreAudioSliderSettings(this.m_musicVolumeSlider, this.m_effectsVolumeSlider, true);
		}
		int value = 0;
		if (this.m_confirmationsToggle.isOn)
		{
			value = 1;
		}
		int @int = PlayerPrefs.GetInt("Option_Confirmations", 1);
		PlayerPrefs.SetInt("Option_Confirmations", value);
		float @float = PlayerPrefs.GetFloat("Option_AnimationSpeed", 1f);
		if (@float != this.m_animSpeedValue)
		{
			if (@float != 0f)
			{
			}
			if (this.m_animSpeedValue != 0f)
			{
				float animSpeedValue = this.m_animSpeedValue;
			}
		}
		PlayerPrefs.SetFloat("Option_AnimationSpeed", this.m_animSpeedValue);
		@int = PlayerPrefs.GetInt("Option_GameSpeed", 1);
		if (@int != this.m_gameSpeedValue)
		{
			if (@int != 0)
			{
			}
			if (this.m_gameSpeedValue != 0)
			{
				int gameSpeedValue = this.m_gameSpeedValue;
			}
		}
		PlayerPrefs.SetInt("Option_GameSpeed", this.m_gameSpeedValue);
		@int = PlayerPrefs.GetInt("Option_GameSpeed", 1);
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x00050373 File Offset: 0x0004E573
	public void OnVolumeSettingsChanged()
	{
		if (this.m_audioSettingsHandler != null)
		{
			this.m_audioSettingsHandler.StoreAudioSliderSettings(this.m_musicVolumeSlider, this.m_effectsVolumeSlider, false);
		}
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0005039C File Offset: 0x0004E59C
	public void OnAnimSpeedButtonPressed()
	{
		float num = this.m_animSpeedValue + 1f;
		this.m_animSpeedValue = num;
		if (num > 2f)
		{
			this.m_animSpeedValue = 0f;
		}
		this.OnAnimSpeedChanged();
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x000503D8 File Offset: 0x0004E5D8
	protected void OnAnimSpeedChanged()
	{
		string text = string.Empty;
		if (this.m_animSpeedValue == 0f)
		{
			this.m_animSpeedBG.sprite = this.m_animSpeedSlow;
			text = "${Key_Slow}";
		}
		else if (this.m_animSpeedValue == 1f)
		{
			this.m_animSpeedBG.sprite = this.m_animSpeedMed;
			text = "${Key_Normal}";
		}
		else if (this.m_animSpeedValue == 2f)
		{
			this.m_animSpeedBG.sprite = this.m_animSpeedFast;
			text = "${Key_Fast}";
		}
		string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(text);
		this.m_animSpeedText.text = ((text2 != string.Empty) ? text2 : text);
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00050484 File Offset: 0x0004E684
	public void OnGameSpeedButtonPressed()
	{
		int num = this.m_gameSpeedValue + 1;
		this.m_gameSpeedValue = num;
		if (num > 2)
		{
			this.m_gameSpeedValue = 0;
		}
		this.OnGameSpeedChanged();
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x000504B4 File Offset: 0x0004E6B4
	protected void OnGameSpeedChanged()
	{
		string text = string.Empty;
		if (this.m_gameSpeedValue == 0)
		{
			this.m_gameSpeedBG.sprite = this.m_gameSpeedSlow;
			text = "${Key_Slow}";
		}
		else if (this.m_gameSpeedValue == 1)
		{
			this.m_gameSpeedBG.sprite = this.m_gameSpeedMed;
			text = "${Key_Normal}";
		}
		else if (this.m_gameSpeedValue == 2)
		{
			this.m_gameSpeedBG.sprite = this.m_gameSpeedFast;
			text = "${Key_Fast}";
		}
		string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(text);
		this.m_gameSpeedText.text = ((text2 != string.Empty) ? text2 : text);
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x00050552 File Offset: 0x0004E752
	public void OnConfirmationsButtonPressed()
	{
		this.OnConfirmationsChanged();
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0005055C File Offset: 0x0004E75C
	protected void OnConfirmationsChanged()
	{
		string text = string.Empty;
		if (this.m_confirmationsToggle.isOn)
		{
			text = "${Key_On}";
			this.m_confirmationsImage.sprite = this.m_gameSpeedFast;
		}
		else
		{
			text = "${Key_Off}";
			this.m_confirmationsImage.sprite = this.m_gameSpeedSlow;
		}
		string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(text);
		this.m_confirmationsToggleText.text = ((text2 != string.Empty) ? text2 : text);
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x000505D4 File Offset: 0x0004E7D4
	public void OnLocalizedLanguagePressed()
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_popupBackButton != null)
		{
			this.m_popupBackButton.SetActive(false);
		}
		string localizationLanguage = LocalizationService.Instance.LocalizationLanguage;
		for (int i = 0; i < this.m_languageToggles.Length; i++)
		{
			if (this.m_languageToggles[i].isOn && localizationLanguage != this.m_languagePrefixes[i])
			{
				LocalizationService.Instance.LocalizationLanguage = this.m_languagePrefixes[i];
				this.m_languageButtonText.text = this.m_languageTexts[i].text;
				CoreApplication.Instance.LocalizationManager.CurrentLanguage = this.m_asmodeeLanguagePrefixes[i];
				return;
			}
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x00050683 File Offset: 0x0004E883
	public void OnResolutionPressed()
	{
		this.m_resolutionListPopup.OpenPopup();
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00050690 File Offset: 0x0004E890
	public void Notified(PopupNotificationType type, string childPopupName, object data = null)
	{
		switch (type)
		{
		case PopupNotificationType.PopupOpened:
			if (childPopupName == "Resolution List Popup")
			{
				this.m_screenResolutionList.PopulateScreenResolutionList();
				this.m_screenResolutionList.SetClickListener(new UIC_ScreenResolutionList.ClickCallback(this.HandleResolutionSelection));
				return;
			}
			break;
		case PopupNotificationType.PopupData:
			break;
		case PopupNotificationType.PopupClosedConfirm:
			if (childPopupName == "Confirm Resolution")
			{
				if (this.m_screenResolutionSelect != null)
				{
					this.m_screenResolutionSelect.Play();
				}
				Screen.SetResolution(this.m_pendingResolution.width, this.m_pendingResolution.height, this.m_windowedScreenToggle.isOn);
				this.m_resolutionCountdownPopup.OpenPopup();
				this.m_countdownCoroutine = base.StartCoroutine(this.ResolutionCountdownTimerHandler(15f, this.m_resolutionCountdownPopupTextCount));
				return;
			}
			if (childPopupName == "Countdown Resolution")
			{
				if (this.m_resoultionTitleText != null)
				{
					this.m_resoultionTitleText.text = Screen.width.ToString() + "x" + Screen.height.ToString();
				}
				base.StopCoroutine(this.m_countdownCoroutine);
				this.m_resolutionListPopup.ClosePopupConfirm();
				if (this.m_popupBackButton != null)
				{
					this.m_popupBackButton.SetActive(false);
				}
				CoreApplication.Instance.AnalyticsManager.SetScreenResolution(Screen.width.ToString() + "*" + Screen.height.ToString());
				return;
			}
			break;
		case PopupNotificationType.PopupClosedCancel:
			if (childPopupName == "Countdown Resolution")
			{
				base.StopCoroutine(this.m_countdownCoroutine);
				Screen.SetResolution(this.m_currentScreenWidth, this.m_currentScreenHeight, this.m_windowedScreenToggle.isOn);
				this.m_resolutionListPopup.ClosePopupCancel();
				if (this.m_popupBackButton != null)
				{
					this.m_popupBackButton.SetActive(false);
				}
				if (this.m_resoultionTitleText != null)
				{
					this.m_resoultionTitleText.text = this.m_currentScreenWidth.ToString() + "x" + this.m_currentScreenHeight.ToString();
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x000508A4 File Offset: 0x0004EAA4
	public void HandleWindowModeChange()
	{
		if (Screen.fullScreen != this.m_windowedScreenToggle.isOn)
		{
			bool fullScreen = Screen.fullScreen;
			bool isOn = this.m_windowedScreenToggle.isOn;
		}
		Screen.SetResolution(Screen.width, Screen.height, this.m_windowedScreenToggle.isOn);
		string text = string.Empty;
		if (this.m_windowedScreenToggle.isOn)
		{
			text = "${Key_On}";
			this.m_windowedScreenImage.sprite = this.m_gameSpeedFast;
		}
		else
		{
			text = "${Key_Off}";
			this.m_windowedScreenImage.sprite = this.m_gameSpeedSlow;
		}
		string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(text);
		this.m_windowedScreenToggleText.text = ((text2 != string.Empty) ? text2 : text);
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x0005095A File Offset: 0x0004EB5A
	public void PlayEffectsSliderAudio()
	{
		if (this.m_effectsSliderAudio != null && !this.m_effectsSliderAudio.isPlaying && !this.m_bIgnoreToggles)
		{
			this.m_effectsSliderAudio.Play();
		}
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0005098A File Offset: 0x0004EB8A
	private void Start()
	{
		if (this.m_windowedScreenToggle != null)
		{
			this.m_windowedScreenToggle.isOn = Screen.fullScreen;
		}
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x000509AC File Offset: 0x0004EBAC
	protected void CheckThemeManager()
	{
		if (this.m_themeManager == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Theme Manager");
			if (gameObject != null)
			{
				this.m_themeManager = gameObject.GetComponent<ThemeManager>();
			}
		}
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x000509E8 File Offset: 0x0004EBE8
	protected void HandleResolutionSelection(Resolution selectedResolution)
	{
		if (Screen.width == selectedResolution.width && Screen.height == selectedResolution.height)
		{
			this.m_resolutionListPopup.ClosePopupCancel();
			if (this.m_popupBackButton != null)
			{
				this.m_popupBackButton.SetActive(false);
			}
			return;
		}
		this.m_currentScreenWidth = Screen.width;
		this.m_currentScreenHeight = Screen.height;
		this.m_pendingResolution = selectedResolution;
		string format = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder("${Key_ChangeResolution}");
		this.m_resolutionConfirmPopupText.text = string.Format(format, selectedResolution.width, selectedResolution.height);
		this.m_resolutionConfirmPopup.OpenPopup();
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00050A98 File Offset: 0x0004EC98
	protected IEnumerator ResolutionCountdownTimerHandler(float totalTime, TextMeshProUGUI textObject)
	{
		float previousTime = Time.time;
		float timeCounter = 0f;
		bool bCounting = true;
		while (bCounting)
		{
			timeCounter += Time.time - previousTime;
			previousTime = Time.time;
			int num = (int)(totalTime - timeCounter);
			if (num < 0)
			{
				num = 0;
			}
			textObject.text = num.ToString();
			if (timeCounter < totalTime)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				bCounting = false;
			}
		}
		this.m_resolutionCountdownPopup.ClosePopupCancel();
		yield break;
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00003022 File Offset: 0x00001222
	public void OnAchievementsButtonPressed()
	{
	}

	// Token: 0x04000C59 RID: 3161
	public UIC_ScreenResolutionList m_screenResolutionList;

	// Token: 0x04000C5A RID: 3162
	public RegisterPopup m_resolutionListPopup;

	// Token: 0x04000C5B RID: 3163
	public RegisterPopup m_resolutionConfirmPopup;

	// Token: 0x04000C5C RID: 3164
	public TextMeshProUGUI m_resolutionConfirmPopupText;

	// Token: 0x04000C5D RID: 3165
	public TextMeshProUGUI m_resoultionTitleText;

	// Token: 0x04000C5E RID: 3166
	public GameObject m_popupBackButton;

	// Token: 0x04000C5F RID: 3167
	public RegisterPopup m_resolutionCountdownPopup;

	// Token: 0x04000C60 RID: 3168
	public TextMeshProUGUI m_resolutionCountdownPopupText;

	// Token: 0x04000C61 RID: 3169
	public TextMeshProUGUI m_resolutionCountdownPopupTextCount;

	// Token: 0x04000C62 RID: 3170
	public Toggle[] m_languageToggles;

	// Token: 0x04000C63 RID: 3171
	public TextMeshProUGUI[] m_languageTexts;

	// Token: 0x04000C64 RID: 3172
	public TextMeshProUGUI m_languageButtonText;

	// Token: 0x04000C65 RID: 3173
	public Toggle m_windowedScreenToggle;

	// Token: 0x04000C66 RID: 3174
	public Image m_windowedScreenImage;

	// Token: 0x04000C67 RID: 3175
	public TextMeshProUGUI m_windowedScreenToggleText;

	// Token: 0x04000C68 RID: 3176
	public TextMeshProUGUI m_versionText;

	// Token: 0x04000C69 RID: 3177
	public TextMeshProUGUI m_animSpeedText;

	// Token: 0x04000C6A RID: 3178
	public Image m_animSpeedBG;

	// Token: 0x04000C6B RID: 3179
	public Sprite m_animSpeedSlow;

	// Token: 0x04000C6C RID: 3180
	public Sprite m_animSpeedMed;

	// Token: 0x04000C6D RID: 3181
	public Sprite m_animSpeedFast;

	// Token: 0x04000C6E RID: 3182
	public TextMeshProUGUI m_gameSpeedText;

	// Token: 0x04000C6F RID: 3183
	public Image m_gameSpeedBG;

	// Token: 0x04000C70 RID: 3184
	public Sprite m_gameSpeedSlow;

	// Token: 0x04000C71 RID: 3185
	public Sprite m_gameSpeedMed;

	// Token: 0x04000C72 RID: 3186
	public Sprite m_gameSpeedFast;

	// Token: 0x04000C73 RID: 3187
	public AudioSettingsHandler m_audioSettingsHandler;

	// Token: 0x04000C74 RID: 3188
	public Slider m_musicVolumeSlider;

	// Token: 0x04000C75 RID: 3189
	public Slider m_effectsVolumeSlider;

	// Token: 0x04000C76 RID: 3190
	public AudioSource m_effectsSliderAudio;

	// Token: 0x04000C77 RID: 3191
	public Toggle m_confirmationsToggle;

	// Token: 0x04000C78 RID: 3192
	public Image m_confirmationsImage;

	// Token: 0x04000C79 RID: 3193
	public TextMeshProUGUI m_confirmationsToggleText;

	// Token: 0x04000C7A RID: 3194
	public AudioSource m_screenResolutionSelect;

	// Token: 0x04000C7B RID: 3195
	public GameObject[] m_ButtonsToDisableOnMobile;

	// Token: 0x04000C7C RID: 3196
	protected Coroutine m_countdownCoroutine;

	// Token: 0x04000C7D RID: 3197
	protected Resolution m_pendingResolution;

	// Token: 0x04000C7E RID: 3198
	protected ThemeManager m_themeManager;

	// Token: 0x04000C7F RID: 3199
	protected int m_currentScreenWidth;

	// Token: 0x04000C80 RID: 3200
	protected int m_currentScreenHeight;

	// Token: 0x04000C81 RID: 3201
	protected bool m_bIgnoreToggles;

	// Token: 0x04000C82 RID: 3202
	protected float m_animSpeedValue;

	// Token: 0x04000C83 RID: 3203
	protected int m_gameSpeedValue;

	// Token: 0x04000C84 RID: 3204
	protected string[] m_languagePrefixes = new string[]
	{
		"EN",
		"FR",
		"IT",
		"DE",
		"ES",
		"PT",
		"RU"
	};

	// Token: 0x04000C85 RID: 3205
	protected LocalizationManager.Language[] m_asmodeeLanguagePrefixes = new LocalizationManager.Language[]
	{
		LocalizationManager.Language.en_US,
		LocalizationManager.Language.fr_FR,
		LocalizationManager.Language.it_IT,
		LocalizationManager.Language.de_DE,
		LocalizationManager.Language.es_ES,
		LocalizationManager.Language.pt_PT,
		LocalizationManager.Language.ru_RU
	};
}
