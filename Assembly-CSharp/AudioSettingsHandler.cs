using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E3 RID: 227
public class AudioSettingsHandler : MonoBehaviour
{
	// Token: 0x0600085C RID: 2140 RVA: 0x0003A1A0 File Offset: 0x000383A0
	private void Awake()
	{
		float @float = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		AudioListener.volume = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);
		foreach (object obj in base.transform)
		{
			((Transform)obj).gameObject.SetActive(true);
		}
		this.m_musicSource.ignoreListenerVolume = true;
		this.m_musicSource.volume = @float;
		string text = string.Empty;
		for (int i = 0; i < this.m_musicList.Length; i++)
		{
			int num = UnityEngine.Random.Range(0, this.m_musicList.Length);
			text = this.m_musicList[num];
			this.m_musicList[num] = this.m_musicList[i];
			this.m_musicList[i] = text;
		}
		this.PlayNextMusic();
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0003A294 File Offset: 0x00038494
	private void OnDestroy()
	{
		if (this.m_currentAudio != null)
		{
			Resources.UnloadAsset(this.m_currentAudio);
			this.m_currentAudio = null;
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0003A2B8 File Offset: 0x000384B8
	private void PlayNextMusic()
	{
		int num = this.m_currentAudioIndex + 1;
		if (num >= this.m_musicList.Length)
		{
			num = 0;
		}
		if (num != this.m_currentAudioIndex && num < this.m_musicList.Length)
		{
			this.m_bCheckForNextTrack = true;
			this.m_musicSource.Stop();
			this.m_musicSource.clip = null;
			if (this.m_currentAudio != null)
			{
				Resources.UnloadAsset(this.m_currentAudio);
				this.m_currentAudio = null;
			}
			this.m_currentAudio = Resources.Load<AudioClip>(this.m_musicList[num]);
			this.m_musicSource.clip = this.m_currentAudio;
			this.m_musicSource.loop = false;
			this.m_musicSource.Play();
			this.m_currentAudioIndex = num;
			return;
		}
		if (num == this.m_currentAudioIndex)
		{
			this.m_musicSource.Play();
		}
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0003A38A File Offset: 0x0003858A
	private void Update()
	{
		if (this.m_bCheckForNextTrack && this.m_musicSource != null && !this.m_musicSource.isPlaying)
		{
			this.PlayNextMusic();
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0003A3B8 File Offset: 0x000385B8
	public void PlayEndGameMusic(bool bWinner)
	{
		this.m_bCheckForNextTrack = false;
		string path = bWinner ? this.m_winMusic : this.m_loseMusic;
		this.m_musicSource.Stop();
		this.m_musicSource.clip = null;
		if (this.m_currentAudio != null)
		{
			Resources.UnloadAsset(this.m_currentAudio);
			this.m_currentAudio = null;
		}
		this.m_currentAudio = Resources.Load<AudioClip>(path);
		this.m_musicSource.clip = this.m_currentAudio;
		this.m_musicSource.loop = true;
		this.m_musicSource.Play();
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0003A44C File Offset: 0x0003864C
	public void SetMusicVolume(Slider slider, bool bStoreAnalytics)
	{
		if (bStoreAnalytics)
		{
			float @float = PlayerPrefs.GetFloat("MusicVolume_LastSent", 0.5f);
			if (Mathf.Abs(@float - slider.value) > 0.01f)
			{
				(@float * 100f).ToString("F0");
				(slider.value * 100f).ToString("F0");
				PlayerPrefs.SetFloat("MusicVolume_LastSent", slider.value);
			}
		}
		PlayerPrefs.SetFloat("MusicVolume", slider.value);
		this.m_musicSource.volume = slider.value;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0003A4E0 File Offset: 0x000386E0
	public void SetEffectsVolume(Slider slider, bool bStoreAnalytics)
	{
		if (bStoreAnalytics)
		{
			float @float = PlayerPrefs.GetFloat("EffectsVolume_LastSent", 0.5f);
			if (Mathf.Abs(@float - slider.value) > 0.01f)
			{
				(@float * 100f).ToString("F0");
				(slider.value * 100f).ToString("F0");
				PlayerPrefs.SetFloat("EffectsVolume_LastSent", slider.value);
			}
		}
		PlayerPrefs.SetFloat("EffectsVolume", slider.value);
		AudioListener.volume = slider.value;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0003A570 File Offset: 0x00038770
	public void RetrieveAudioSliderSettings(Slider musicSlider, Slider effectsSlider)
	{
		float @float = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		float float2 = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);
		musicSlider.value = @float;
		effectsSlider.value = float2;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0003A5AB File Offset: 0x000387AB
	public void StoreAudioSliderSettings(Slider musicSlider, Slider effectsSlider, bool bStoreAnalytics)
	{
		this.SetMusicVolume(musicSlider, bStoreAnalytics);
		this.SetEffectsVolume(effectsSlider, bStoreAnalytics);
		PlayerPrefs.Save();
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0003A5C4 File Offset: 0x000387C4
	public void DisableSoundEffects()
	{
		if (this.m_soundEffectList == null)
		{
			return;
		}
		AudioSource[] soundEffectList = this.m_soundEffectList;
		for (int i = 0; i < soundEffectList.Length; i++)
		{
			soundEffectList[i].enabled = false;
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0003A5F8 File Offset: 0x000387F8
	public void EnableSoundEffects()
	{
		if (this.m_soundEffectList == null)
		{
			return;
		}
		AudioSource[] soundEffectList = this.m_soundEffectList;
		for (int i = 0; i < soundEffectList.Length; i++)
		{
			soundEffectList[i].enabled = true;
		}
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0003A62C File Offset: 0x0003882C
	public void DisableToggleSoundEffects()
	{
		if (this.m_soundEffectToggle == null)
		{
			return;
		}
		this.m_soundEffectToggle.enabled = false;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0003A649 File Offset: 0x00038849
	public void EnableToggleSoundEffects()
	{
		if (this.m_soundEffectToggle == null)
		{
			return;
		}
		this.m_soundEffectToggle.enabled = true;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0003A666 File Offset: 0x00038866
	public void StopAllMusic()
	{
		if (this.m_musicList == null)
		{
			return;
		}
		this.m_musicSource.Stop();
	}

	// Token: 0x0400092C RID: 2348
	public string[] m_musicList;

	// Token: 0x0400092D RID: 2349
	public string m_winMusic;

	// Token: 0x0400092E RID: 2350
	public string m_loseMusic;

	// Token: 0x0400092F RID: 2351
	public AudioSource[] m_soundEffectList;

	// Token: 0x04000930 RID: 2352
	public AudioSource m_musicSource;

	// Token: 0x04000931 RID: 2353
	public AudioSource m_soundEffectToggle;

	// Token: 0x04000932 RID: 2354
	private AudioClip m_currentAudio;

	// Token: 0x04000933 RID: 2355
	private int m_currentAudioIndex = -1;

	// Token: 0x04000934 RID: 2356
	private bool m_bCheckForNextTrack = true;
}
