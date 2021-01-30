using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class AnimationLayer : MonoBehaviour
{
	// Token: 0x0600074E RID: 1870 RVA: 0x00035B7C File Offset: 0x00033D7C
	public void AddAnimation(AnimateObject animate_object)
	{
		float animateCurrentHeight = animate_object.GetAnimateCurrentHeight();
		if (this.m_LayerAbove != null && animateCurrentHeight > this.m_LayerAboveThreshold)
		{
			this.m_LayerAbove.AddAnimation(animate_object);
			return;
		}
		if (this.m_LayerBelow != null && animateCurrentHeight < this.m_LayerBelowThreshold)
		{
			this.m_LayerBelow.AddAnimation(animate_object);
			return;
		}
		animate_object.transform.SetParent(base.transform);
		animate_object.transform.SetAsLastSibling();
		int childCount = base.transform.childCount;
		int num = 0;
		int num2 = childCount - 1;
		while (num != num2)
		{
			int num3 = (num2 - num) / 2;
			int num4 = num + num3;
			AnimateObject component = base.transform.GetChild(num4).GetComponent<AnimateObject>();
			if (animateCurrentHeight < component.GetAnimateCurrentHeight())
			{
				num2 = num4;
			}
			else
			{
				num = num4 + 1;
			}
		}
		animate_object.transform.SetSiblingIndex(num);
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00035C4C File Offset: 0x00033E4C
	public void UpdateAnimation(AnimateObject animate_object, float previousHeight, float updateHeight)
	{
		if (animate_object.transform.parent != base.transform)
		{
			return;
		}
		int siblingIndex = animate_object.transform.GetSiblingIndex();
		int i = siblingIndex;
		int childCount = base.transform.childCount;
		if (updateHeight > previousHeight)
		{
			if (this.m_LayerAbove != null && updateHeight > this.m_LayerAboveThreshold)
			{
				this.m_LayerAbove.AddAnimation(animate_object);
				return;
			}
			while (i < childCount - 1)
			{
				AnimateObject component = base.transform.GetChild(i + 1).GetComponent<AnimateObject>();
				if (updateHeight < component.GetAnimateCurrentHeight())
				{
					break;
				}
				i++;
			}
		}
		else if (updateHeight < previousHeight)
		{
			if (this.m_LayerBelow != null && updateHeight < this.m_LayerBelowThreshold)
			{
				this.m_LayerBelow.AddAnimation(animate_object);
				return;
			}
			while (i > 0)
			{
				AnimateObject component2 = base.transform.GetChild(i - 1).GetComponent<AnimateObject>();
				if (updateHeight > component2.GetAnimateCurrentHeight())
				{
					break;
				}
				i--;
			}
		}
		if (i != siblingIndex)
		{
			animate_object.transform.SetSiblingIndex(i);
		}
	}

	// Token: 0x0400088A RID: 2186
	[SerializeField]
	private AnimationLayer m_LayerAbove;

	// Token: 0x0400088B RID: 2187
	[SerializeField]
	private float m_LayerAboveThreshold;

	// Token: 0x0400088C RID: 2188
	[SerializeField]
	private AnimationLayer m_LayerBelow;

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private float m_LayerBelowThreshold;
}
