using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E8 RID: 232
public class ColorByFaction : MonoBehaviour
{
	// Token: 0x0600088A RID: 2186 RVA: 0x0003AFED File Offset: 0x000391ED
	private void Start()
	{
		if (this.m_startupAction == ColorByFaction.StartupFunction.ColorizeWithValue)
		{
			this.Colorize(this.m_startupColorizeValue);
			return;
		}
		if (this.m_startupAction == ColorByFaction.StartupFunction.ColorizeByTheme)
		{
			this.ColorizeByTheme();
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0003B014 File Offset: 0x00039214
	private void Update()
	{
		if (this.m_bColorizeThemeLerp)
		{
			this.m_currentTime += Time.deltaTime;
			if (this.m_currentTime >= this.m_TimeInSecondsToBlendThemes)
			{
				this.m_bColorizeThemeLerp = false;
				this.m_currentTime = this.m_TimeInSecondsToBlendThemes;
				this.m_oldTheme = this.m_newTheme;
			}
			float t = this.m_currentTime / this.m_TimeInSecondsToBlendThemes;
			foreach (ColorByFaction.ColorizeImageTextEntry colorizeImageTextEntry in this.m_RecolorImageTextEntries)
			{
				if ((long)colorizeImageTextEntry.Faction_colors.Length > (long)((ulong)this.m_oldTheme) && (long)colorizeImageTextEntry.Faction_colors.Length > (long)((ulong)this.m_newTheme))
				{
					Color color = Color.Lerp(colorizeImageTextEntry.Faction_colors[(int)this.m_oldTheme], colorizeImageTextEntry.Faction_colors[(int)this.m_newTheme], t);
					foreach (Image image in colorizeImageTextEntry.images)
					{
						if (image != null)
						{
							image.color = color;
						}
					}
					foreach (TextMeshProUGUI textMeshProUGUI in colorizeImageTextEntry.textFields)
					{
						if (textMeshProUGUI != null)
						{
							textMeshProUGUI.color = color;
						}
					}
				}
			}
			foreach (ColorByFaction.ColorizeButtonEntry colorizeButtonEntry in this.m_RecolorButtonEntries)
			{
				if ((long)colorizeButtonEntry.Faction_colors.Length > (long)((ulong)this.m_oldTheme) && (long)colorizeButtonEntry.Faction_colors.Length > (long)((ulong)this.m_newTheme))
				{
					ColorBlock colors = colorizeButtonEntry.Faction_colors[(int)this.m_newTheme];
					colors.disabledColor = Color.Lerp(colorizeButtonEntry.Faction_colors[(int)this.m_oldTheme].disabledColor, colorizeButtonEntry.Faction_colors[(int)this.m_newTheme].disabledColor, t);
					colors.highlightedColor = Color.Lerp(colorizeButtonEntry.Faction_colors[(int)this.m_oldTheme].highlightedColor, colorizeButtonEntry.Faction_colors[(int)this.m_newTheme].highlightedColor, t);
					colors.normalColor = Color.Lerp(colorizeButtonEntry.Faction_colors[(int)this.m_oldTheme].normalColor, colorizeButtonEntry.Faction_colors[(int)this.m_newTheme].normalColor, t);
					colors.pressedColor = Color.Lerp(colorizeButtonEntry.Faction_colors[(int)this.m_oldTheme].pressedColor, colorizeButtonEntry.Faction_colors[(int)this.m_newTheme].pressedColor, t);
					foreach (Button button in colorizeButtonEntry.buttons)
					{
						if (button != null)
						{
							button.colors = colors;
						}
					}
					foreach (Toggle toggle in colorizeButtonEntry.toggles)
					{
						if (toggle != null)
						{
							toggle.colors = colors;
						}
					}
				}
			}
			if (!this.m_bColorizeThemeLerp)
			{
				foreach (ColorByFaction.ImageReplacementEntry imageReplacementEntry in this.m_ImageReplacementEntries)
				{
					if ((long)imageReplacementEntry.Faction_images.Length > (long)((ulong)this.m_newTheme))
					{
						Sprite sprite = imageReplacementEntry.Faction_images[(int)this.m_newTheme];
						foreach (Image image2 in imageReplacementEntry.images)
						{
							if (image2 != null)
							{
								image2.sprite = sprite;
							}
						}
					}
				}
				foreach (ColorByFaction.AnimatorReplacementEntry animatorReplacementEntry in this.m_AnimatorReplacementEntries)
				{
					if ((long)animatorReplacementEntry.Faction_controllers.Length > (long)((ulong)this.m_newTheme) && (animatorReplacementEntry.maskingObjects == null || animatorReplacementEntry.maskingObjects.Length == 0))
					{
						RuntimeAnimatorController runtimeAnimatorController = animatorReplacementEntry.Faction_controllers[(int)this.m_newTheme];
						foreach (Animator animator in animatorReplacementEntry.animators)
						{
							if (animator != null)
							{
								animator.runtimeAnimatorController = runtimeAnimatorController;
							}
						}
					}
					else
					{
						GameObject[] maskingObjects = animatorReplacementEntry.maskingObjects;
						for (int j = 0; j < maskingObjects.Length; j++)
						{
							maskingObjects[j].SetActive(false);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0003B44C File Offset: 0x0003964C
	public void ColorizeOverTime(uint newTheme)
	{
		this.m_newTheme = newTheme;
		this.m_currentTime = 0f;
		this.m_bColorizeThemeLerp = true;
		foreach (ColorByFaction.AnimatorReplacementEntry animatorReplacementEntry in this.m_AnimatorReplacementEntries)
		{
			if ((long)animatorReplacementEntry.Faction_controllers.Length > (long)((ulong)this.m_newTheme) && animatorReplacementEntry.maskingObjects != null && animatorReplacementEntry.maskingObjects.Length != 0)
			{
				RuntimeAnimatorController runtimeAnimatorController = animatorReplacementEntry.Faction_controllers[(int)this.m_newTheme];
				GameObject[] maskingObjects = animatorReplacementEntry.maskingObjects;
				for (int j = 0; j < maskingObjects.Length; j++)
				{
					maskingObjects[j].SetActive(true);
				}
				foreach (Animator animator in animatorReplacementEntry.animators)
				{
					if (animator != null)
					{
						animator.runtimeAnimatorController = runtimeAnimatorController;
					}
				}
			}
		}
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0003B51C File Offset: 0x0003971C
	public void ColorizeByTheme()
	{
		uint @int = (uint)PlayerPrefs.GetInt("Ascension_Theme", 0);
		this.m_oldTheme = @int;
		this.Colorize(@int);
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0003B544 File Offset: 0x00039744
	public void Colorize(uint factionIndex)
	{
		foreach (ColorByFaction.ColorizeImageTextEntry colorizeImageTextEntry in this.m_RecolorImageTextEntries)
		{
			if ((long)colorizeImageTextEntry.Faction_colors.Length > (long)((ulong)factionIndex))
			{
				Color color = colorizeImageTextEntry.Faction_colors[(int)factionIndex];
				foreach (Image image in colorizeImageTextEntry.images)
				{
					if (image != null)
					{
						image.color = color;
					}
				}
				foreach (TextMeshProUGUI textMeshProUGUI in colorizeImageTextEntry.textFields)
				{
					if (textMeshProUGUI != null)
					{
						textMeshProUGUI.color = color;
					}
				}
			}
		}
		foreach (ColorByFaction.ColorizeButtonEntry colorizeButtonEntry in this.m_RecolorButtonEntries)
		{
			if ((long)colorizeButtonEntry.Faction_colors.Length > (long)((ulong)factionIndex))
			{
				ColorBlock colors = colorizeButtonEntry.Faction_colors[(int)factionIndex];
				foreach (Button button in colorizeButtonEntry.buttons)
				{
					if (button != null)
					{
						button.colors = colors;
					}
				}
				foreach (Toggle toggle in colorizeButtonEntry.toggles)
				{
					if (toggle != null)
					{
						toggle.colors = colors;
					}
				}
			}
		}
		foreach (ColorByFaction.ImageReplacementEntry imageReplacementEntry in this.m_ImageReplacementEntries)
		{
			if ((long)imageReplacementEntry.Faction_images.Length > (long)((ulong)factionIndex))
			{
				Sprite sprite = imageReplacementEntry.Faction_images[(int)factionIndex];
				foreach (Image image2 in imageReplacementEntry.images)
				{
					if (image2 != null)
					{
						image2.sprite = sprite;
					}
				}
			}
		}
		foreach (ColorByFaction.AnimatorReplacementEntry animatorReplacementEntry in this.m_AnimatorReplacementEntries)
		{
			if ((long)animatorReplacementEntry.Faction_controllers.Length > (long)((ulong)factionIndex))
			{
				RuntimeAnimatorController runtimeAnimatorController = animatorReplacementEntry.Faction_controllers[(int)factionIndex];
				foreach (Animator animator in animatorReplacementEntry.animators)
				{
					if (animator != null)
					{
						animator.runtimeAnimatorController = runtimeAnimatorController;
					}
				}
			}
		}
	}

	// Token: 0x04000946 RID: 2374
	public ColorByFaction.ColorizeImageTextEntry[] m_RecolorImageTextEntries;

	// Token: 0x04000947 RID: 2375
	public ColorByFaction.ColorizeButtonEntry[] m_RecolorButtonEntries;

	// Token: 0x04000948 RID: 2376
	public ColorByFaction.ImageReplacementEntry[] m_ImageReplacementEntries;

	// Token: 0x04000949 RID: 2377
	public ColorByFaction.AnimatorReplacementEntry[] m_AnimatorReplacementEntries;

	// Token: 0x0400094A RID: 2378
	public float m_TimeInSecondsToBlendThemes = 0.5f;

	// Token: 0x0400094B RID: 2379
	public ColorByFaction.StartupFunction m_startupAction;

	// Token: 0x0400094C RID: 2380
	public uint m_startupColorizeValue;

	// Token: 0x0400094D RID: 2381
	private uint m_oldTheme;

	// Token: 0x0400094E RID: 2382
	private uint m_newTheme;

	// Token: 0x0400094F RID: 2383
	private bool m_bColorizeThemeLerp;

	// Token: 0x04000950 RID: 2384
	private float m_currentTime;

	// Token: 0x020007A9 RID: 1961
	[Serializable]
	public class ColorizeImageTextEntry
	{
		// Token: 0x04002C88 RID: 11400
		public Color[] Faction_colors;

		// Token: 0x04002C89 RID: 11401
		public Image[] images;

		// Token: 0x04002C8A RID: 11402
		public TextMeshProUGUI[] textFields;
	}

	// Token: 0x020007AA RID: 1962
	[Serializable]
	public class ColorizeButtonEntry
	{
		// Token: 0x04002C8B RID: 11403
		public ColorBlock[] Faction_colors;

		// Token: 0x04002C8C RID: 11404
		public Button[] buttons;

		// Token: 0x04002C8D RID: 11405
		public Toggle[] toggles;
	}

	// Token: 0x020007AB RID: 1963
	[Serializable]
	public class ImageReplacementEntry
	{
		// Token: 0x04002C8E RID: 11406
		public Sprite[] Faction_images;

		// Token: 0x04002C8F RID: 11407
		public Image[] images;
	}

	// Token: 0x020007AC RID: 1964
	[Serializable]
	public class AnimatorReplacementEntry
	{
		// Token: 0x04002C90 RID: 11408
		public RuntimeAnimatorController[] Faction_controllers;

		// Token: 0x04002C91 RID: 11409
		public Animator[] animators;

		// Token: 0x04002C92 RID: 11410
		[Tooltip("Only used when blending to hide pop of Animator swap")]
		public GameObject[] maskingObjects;
	}

	// Token: 0x020007AD RID: 1965
	public enum StartupFunction
	{
		// Token: 0x04002C94 RID: 11412
		DoNothing,
		// Token: 0x04002C95 RID: 11413
		ColorizeWithValue,
		// Token: 0x04002C96 RID: 11414
		ColorizeByTheme
	}
}
