using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000FC RID: 252
public class UIP_AchievementSlot : MonoBehaviour
{
	// Token: 0x0600097B RID: 2427 RVA: 0x00003022 File Offset: 0x00001222
	private void OnDestroy()
	{
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0003FCA0 File Offset: 0x0003DEA0
	public void Init(AchievementData achievement)
	{
		this.m_displayImage.sprite = (achievement.bHasAchieved ? achievement.unlockedSprite : achievement.lockedSprite);
		if (this.m_lockedObject != null)
		{
			this.m_lockedObject.SetActive(!achievement.bHasAchieved);
		}
		if (achievement.bHasAchieved || achievement.achievedAtValue == 1)
		{
			if (this.m_progresObject != null)
			{
				this.m_progresObject.SetActive(false);
			}
			this.m_progressTextGoal.text = "";
			this.m_progressTextCurrent.text = "";
		}
		else
		{
			if (this.m_progresObject != null)
			{
				this.m_progresObject.SetActive(true);
			}
			int num = (int)(achievement.isBitField ? AchievementManagerWrapper.CountBits(achievement.currentValue) : achievement.currentValue);
			if (this.m_progresSlider != null)
			{
				this.m_progresSlider.fillAmount = (float)num / (float)achievement.achievedAtValue;
			}
			this.m_progressTextGoal.text = "/" + achievement.achievedAtValue.ToString();
			this.m_progressTextCurrent.text = num.ToString();
		}
		if (achievement.isHiddenAchievement && !achievement.bHasAchieved)
		{
			this.m_titleText.KeyText = this.m_hiddenTitle;
			this.m_descText.KeyText = this.m_hiddenDesc;
			if (this.m_progresSlider != null)
			{
				this.m_progresSlider.gameObject.SetActive(false);
			}
			this.m_progressTextGoal.text = "";
			this.m_progressTextCurrent.text = "";
			if (this.m_hiddenObject != null)
			{
				this.m_hiddenObject.SetActive(true);
				return;
			}
		}
		else
		{
			this.m_titleText.KeyText = achievement.displayName;
			this.m_descText.KeyText = achievement.displayDescription;
			if (this.m_hiddenObject != null)
			{
				this.m_hiddenObject.SetActive(false);
			}
		}
	}

	// Token: 0x040009FE RID: 2558
	public Image m_displayImage;

	// Token: 0x040009FF RID: 2559
	public Image m_progresSlider;

	// Token: 0x04000A00 RID: 2560
	public GameObject m_progresObject;

	// Token: 0x04000A01 RID: 2561
	public UILocalizedText m_titleText;

	// Token: 0x04000A02 RID: 2562
	public UILocalizedText m_descText;

	// Token: 0x04000A03 RID: 2563
	public TextMeshProUGUI m_progressTextCurrent;

	// Token: 0x04000A04 RID: 2564
	public TextMeshProUGUI m_progressTextGoal;

	// Token: 0x04000A05 RID: 2565
	public GameObject m_lockedObject;

	// Token: 0x04000A06 RID: 2566
	public GameObject m_hiddenObject;

	// Token: 0x04000A07 RID: 2567
	public string m_hiddenTitle = "Hidden Achievement";

	// Token: 0x04000A08 RID: 2568
	public string m_hiddenDesc = "TOP SECRET";
}
