using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012A RID: 298
public class UI_Rulebook : UI_FrontEndAndInGameScene
{
	// Token: 0x06000B79 RID: 2937 RVA: 0x0004FDFB File Offset: 0x0004DFFB
	public void OnMenuEnter()
	{
		base.SetIsInGame(false);
		this.SetCurrentRulebook(this.m_currentRulebook);
		Canvas.ForceUpdateCanvases();
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x00003022 File Offset: 0x00001222
	public void OnMenuExit(bool bUnderPopup)
	{
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0004FE18 File Offset: 0x0004E018
	public void SetCurrentRulebook(int index)
	{
		base.StopAllCoroutines();
		this.m_currentRulebook = index;
		this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition = 1f;
		this.m_rulebooks[this.m_currentRulebook].m_ScrollRectPressme.raycastTarget = true;
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0004FE70 File Offset: 0x0004E070
	public void DisplayPos_Editor()
	{
		Debug.Log(" ScrollRect pos: " + this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition.ToString("F4"));
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0004FEB4 File Offset: 0x0004E0B4
	public void OnDownButton()
	{
		float num = 0f;
		float num2 = (float)Math.Round((double)this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition, 4);
		for (int i = 0; i < this.m_rulebooks[this.m_currentRulebook].m_snapPositions.Length; i++)
		{
			if (this.m_rulebooks[this.m_currentRulebook].m_snapPositions[i] > num && this.m_rulebooks[this.m_currentRulebook].m_snapPositions[i] < num2)
			{
				num = this.m_rulebooks[this.m_currentRulebook].m_snapPositions[i];
			}
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.LerpScrollRectVertical(this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition, num, this.m_animateTime));
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0004FF94 File Offset: 0x0004E194
	public void OnUpButton()
	{
		float num = 1f;
		float num2 = (float)Math.Round((double)this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition, 4);
		for (int i = 0; i < this.m_rulebooks[this.m_currentRulebook].m_snapPositions.Length; i++)
		{
			if (this.m_rulebooks[this.m_currentRulebook].m_snapPositions[i] < num && this.m_rulebooks[this.m_currentRulebook].m_snapPositions[i] > num2)
			{
				num = this.m_rulebooks[this.m_currentRulebook].m_snapPositions[i];
			}
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.LerpScrollRectVertical(this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition, num, this.m_animateTime));
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00050072 File Offset: 0x0004E272
	private IEnumerator LerpScrollRectVertical(float startNormalizedPos, float endNormalizedPos, float time)
	{
		float previousTime = Time.time;
		float currentAnimTime = 0f;
		bool bAnimating = true;
		this.m_rulebooks[this.m_currentRulebook].m_scrollRect.velocity = Vector2.zero;
		this.m_rulebooks[this.m_currentRulebook].m_ScrollRectPressme.raycastTarget = false;
		while (bAnimating)
		{
			currentAnimTime += Time.time - previousTime;
			previousTime = Time.time;
			if (currentAnimTime < time)
			{
				this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(startNormalizedPos, endNormalizedPos, currentAnimTime / time);
			}
			else
			{
				this.m_rulebooks[this.m_currentRulebook].m_scrollRect.verticalNormalizedPosition = endNormalizedPos;
				bAnimating = false;
			}
			yield return new WaitForEndOfFrame();
		}
		this.m_rulebooks[this.m_currentRulebook].m_ScrollRectPressme.raycastTarget = true;
		yield break;
	}

	// Token: 0x04000C56 RID: 3158
	public UI_Rulebook.RulebookCategory[] m_rulebooks;

	// Token: 0x04000C57 RID: 3159
	public float m_animateTime;

	// Token: 0x04000C58 RID: 3160
	private int m_currentRulebook;

	// Token: 0x02000807 RID: 2055
	[Serializable]
	public struct RulebookCategory
	{
		// Token: 0x04002DE2 RID: 11746
		public ScrollRect m_scrollRect;

		// Token: 0x04002DE3 RID: 11747
		public Image m_ScrollRectPressme;

		// Token: 0x04002DE4 RID: 11748
		public float[] m_snapPositions;
	}
}
