using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000113 RID: 275
public class UI_Achievements : MonoBehaviour
{
	// Token: 0x06000A45 RID: 2629 RVA: 0x0004416C File Offset: 0x0004236C
	public void OnEnterMenu()
	{
		if (this.m_achievementManager == null)
		{
			this.m_achievementManager = AchievementManagerWrapper.instance;
		}
		if (this.m_achievementSlots == null)
		{
			this.m_achievementSlots = new List<UIP_AchievementSlot>();
		}
		if (this.m_achievementManager != null)
		{
			for (int i = 0; i < this.m_achievementManager.m_achievements.Length; i++)
			{
				if (!this.m_achievementManager.m_achievements[i].dontShowAchievement)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_achievementSlotPrefab);
					UIP_AchievementSlot component = gameObject.GetComponent<UIP_AchievementSlot>();
					component.Init(this.m_achievementManager.m_achievements[i]);
					this.m_achievementSlots.Add(component);
					gameObject.transform.SetParent(this.m_contentContainer);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
				}
			}
		}
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x0004427C File Offset: 0x0004247C
	public void OnExitMenu(bool bUnderPopup)
	{
		for (int i = 0; i < this.m_achievementSlots.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_achievementSlots[i].gameObject);
		}
		this.m_achievementSlots.Clear();
	}

	// Token: 0x04000AD6 RID: 2774
	public GameObject m_achievementSlotPrefab;

	// Token: 0x04000AD7 RID: 2775
	public Transform m_contentContainer;

	// Token: 0x04000AD8 RID: 2776
	public ScrollRect m_scrollView;

	// Token: 0x04000AD9 RID: 2777
	private AchievementManagerWrapper m_achievementManager;

	// Token: 0x04000ADA RID: 2778
	private List<UIP_AchievementSlot> m_achievementSlots;
}
