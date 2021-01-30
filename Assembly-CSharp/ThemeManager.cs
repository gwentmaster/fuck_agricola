using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class ThemeManager : MonoBehaviour
{
	// Token: 0x0600092B RID: 2347 RVA: 0x0003E00D File Offset: 0x0003C20D
	private void Awake()
	{
		this.SetThemeObjectsActive();
		base.StartCoroutine(this.Startup());
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0003E024 File Offset: 0x0003C224
	public void SetThemeObjectsActive()
	{
		int num = PlayerPrefs.GetInt("Ascension_Theme", 0);
		if (num >= this.m_themeObjects.Length || num < 0)
		{
			Debug.LogWarning("No theme object with index: " + num.ToString());
			num = 0;
		}
		if (this.m_currentTheme != num)
		{
			this.m_currentTheme = num;
			if (this.m_bLoadThemesFromResources)
			{
				if (this.m_ActiveThemeObj != null)
				{
					this.m_OldThemeObj = this.m_ActiveThemeObj;
					if (this.m_animatorTriggerFadeOutVariableName != null && this.m_ActiveThemeAnimator != null)
					{
						this.m_ActiveThemeAnimator.SetTrigger(this.m_animatorTriggerFadeOutVariableName);
					}
					this.m_ActiveThemeObj = null;
					this.m_ActiveThemeAnimator = null;
				}
				GameObject gameObject = Resources.Load<GameObject>(this.m_themeObjects[this.m_currentTheme].path);
				if (gameObject == null)
				{
					Debug.LogError("Unable to get theme object at path: " + this.m_themeObjects[this.m_currentTheme].path);
					return;
				}
				this.m_ActiveThemeObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				this.m_ActiveThemeObj.transform.SetParent(base.gameObject.transform, false);
				this.m_ActiveThemeObj.transform.localPosition = Vector3.zero;
				this.m_ActiveThemeObj.transform.localScale = Vector3.one;
				this.m_ActiveThemeObj.transform.SetAsFirstSibling();
				this.m_ActiveThemeAnimator = this.m_ActiveThemeObj.GetComponent<Animator>();
				base.StartCoroutine(this.Startup());
				Resources.UnloadUnusedAssets();
				return;
			}
			else
			{
				for (int i = 0; i < this.m_themeObjects.Length; i++)
				{
					if (this.m_themeObjects[i].root != null)
					{
						this.m_themeObjects[i].root.SetActive(i == num);
					}
				}
			}
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0003E1EC File Offset: 0x0003C3EC
	public void SetOnStartScreen(bool bOnStartScreen)
	{
		if (this.m_bLoadThemesFromResources)
		{
			if (this.m_ActiveThemeAnimator != null)
			{
				this.m_ActiveThemeAnimator.SetBool(this.m_animatorBooleanVariableName, !bOnStartScreen);
			}
		}
		else
		{
			for (int i = 0; i < this.m_themeObjects.Length; i++)
			{
				if (this.m_themeObjects[i].animator != null)
				{
					this.m_themeObjects[i].animator.SetBool(this.m_animatorBooleanVariableName, !bOnStartScreen);
				}
			}
		}
		this.m_bOnStartScreen = bOnStartScreen;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0003E279 File Offset: 0x0003C479
	private IEnumerator Startup()
	{
		yield return new WaitForEndOfFrame();
		this.SetOnStartScreen(this.m_bOnStartScreen);
		if (this.m_OldThemeObj != null)
		{
			yield return new WaitForSeconds(this.m_waitTimeUntilDeletionOfOldObj);
			UnityEngine.Object.Destroy(this.m_OldThemeObj);
			this.m_OldThemeObj = null;
			Resources.UnloadUnusedAssets();
		}
		yield break;
	}

	// Token: 0x040009B8 RID: 2488
	public bool m_bLoadThemesFromResources = true;

	// Token: 0x040009B9 RID: 2489
	public ThemeManager.Theme[] m_themeObjects;

	// Token: 0x040009BA RID: 2490
	public string m_animatorBooleanVariableName;

	// Token: 0x040009BB RID: 2491
	public string m_animatorTriggerFadeOutVariableName = "ThemeOff";

	// Token: 0x040009BC RID: 2492
	public float m_waitTimeUntilDeletionOfOldObj = 1f;

	// Token: 0x040009BD RID: 2493
	private GameObject m_ActiveThemeObj;

	// Token: 0x040009BE RID: 2494
	private GameObject m_OldThemeObj;

	// Token: 0x040009BF RID: 2495
	private Animator m_ActiveThemeAnimator;

	// Token: 0x040009C0 RID: 2496
	private int m_currentTheme = -1;

	// Token: 0x040009C1 RID: 2497
	private bool m_bOnStartScreen;

	// Token: 0x020007BC RID: 1980
	[Serializable]
	public struct Theme
	{
		// Token: 0x04002CDB RID: 11483
		public GameObject root;

		// Token: 0x04002CDC RID: 11484
		public Animator animator;

		// Token: 0x04002CDD RID: 11485
		public string path;
	}
}
