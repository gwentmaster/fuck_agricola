using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class AnimationLocator : MonoBehaviour
{
	// Token: 0x06000751 RID: 1873 RVA: 0x00035D3C File Offset: 0x00033F3C
	public void SetAnimationManager(AnimationManager manager)
	{
		this.m_AnimationManager = manager;
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00035D45 File Offset: 0x00033F45
	public AnimationLayer GetOverrideAnimationLayer()
	{
		return this.m_OverrideAnimationLayer;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00035D4D File Offset: 0x00033F4D
	public void SetOverrideAnimationLayer(AnimationLayer animation_layer)
	{
		this.m_OverrideAnimationLayer = animation_layer;
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x00035D56 File Offset: 0x00033F56
	public AnimationLocator GetOverridePlaceLocator()
	{
		return this.m_OverridePlaceLocator;
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00035D5E File Offset: 0x00033F5E
	public void SetOverridePlaceLocator(AnimationLocator locator)
	{
		this.m_OverridePlaceLocator = locator;
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00035D67 File Offset: 0x00033F67
	public virtual AnimationLocator GetOverrideDestinationLocator(AnimateObject animate_object)
	{
		return this;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00035D6A File Offset: 0x00033F6A
	public float GetAnimationHeight()
	{
		return this.m_AnimationHeight;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00035D72 File Offset: 0x00033F72
	public Vector3 GetAdditionalRotation()
	{
		return this.m_AdditionalRotation;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00035D7A File Offset: 0x00033F7A
	public bool GetAdjustPlaceholderLayoutWidth()
	{
		return this.m_AdjustPlaceholderLayoutWidth;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00035D82 File Offset: 0x00033F82
	public bool GetAdjustPlaceholderLayoutHeight()
	{
		return this.m_AdjustPlaceholderLayoutHeight;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00035D8C File Offset: 0x00033F8C
	public virtual void AnimateFromLocator(AnimateObject animate_object)
	{
		if (animate_object == null)
		{
			return;
		}
		animate_object.transform.SetParent(base.transform, false);
		animate_object.transform.position = base.transform.position;
		animate_object.transform.rotation = base.transform.rotation;
		animate_object.SetAnimateCurrentHeight(this.m_AnimationHeight);
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00035DED File Offset: 0x00033FED
	protected virtual void SetPlaceholderParent(GameObject placeholder)
	{
		if (placeholder == null)
		{
			return;
		}
		placeholder.transform.SetParent(base.transform, false);
		placeholder.transform.position = base.transform.position;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x00035E24 File Offset: 0x00034024
	public virtual bool AnimateToLocator(AnimateObject animate_object, AnimationLocator sourceLocator, float delayAtStart = 0f, float pauseAtDestination = 0f)
	{
		if (animate_object == null)
		{
			return false;
		}
		AnimationLayer animationLayer = null;
		if (this.m_OverrideAnimationLayer != null)
		{
			animationLayer = this.m_OverrideAnimationLayer;
		}
		else if (this.m_AnimationManager != null)
		{
			animationLayer = this.m_AnimationManager.GetDefaultAnimationLayer();
		}
		GameObject placeholder = animate_object.GetPlaceholder(true);
		animate_object.ResetPlaceholderLayout(0f, 0f);
		this.SetPlaceholderParent(placeholder);
		if (animationLayer != null)
		{
			bool activeInHierarchy = animate_object.gameObject.activeInHierarchy;
			if (!activeInHierarchy && sourceLocator != null)
			{
				animate_object.SetAnimateCurrentHeight(sourceLocator.m_AnimationHeight);
			}
			animate_object.SetAnimateTargetHeight(this.m_AnimationHeight);
			animationLayer.AddAnimation(animate_object);
			if (!activeInHierarchy)
			{
				animate_object.ResetCurrentScale();
			}
			else
			{
				animate_object.InheritCurrentScale();
			}
		}
		animate_object.StartAnimationToLocator(this, sourceLocator, delayAtStart, pauseAtDestination);
		return true;
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00035EEC File Offset: 0x000340EC
	public virtual void PlaceAnimateObject(AnimateObject animate_object, bool bSetPosition = false, bool bSetRotation = false, bool bIgnoreOverride = false)
	{
		if (this.m_OverridePlaceLocator != null && !bIgnoreOverride)
		{
			this.m_OverridePlaceLocator.PlaceAnimateObject(animate_object, bSetPosition, true, false);
			return;
		}
		if (animate_object != null)
		{
			bool activeInHierarchy = animate_object.gameObject.activeInHierarchy;
			animate_object.PlaceOnAnimationLocator(this, bSetPosition, bSetRotation, -1);
			if (!activeInHierarchy)
			{
				animate_object.ResetCurrentScale();
			}
		}
	}

	// Token: 0x0400088E RID: 2190
	[SerializeField]
	private AnimationLayer m_OverrideAnimationLayer;

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private AnimationLocator m_OverridePlaceLocator;

	// Token: 0x04000890 RID: 2192
	[SerializeField]
	private float m_AnimationHeight;

	// Token: 0x04000891 RID: 2193
	[SerializeField]
	private Vector3 m_AdditionalRotation = Vector2.zero;

	// Token: 0x04000892 RID: 2194
	[SerializeField]
	private bool m_AdjustPlaceholderLayoutWidth;

	// Token: 0x04000893 RID: 2195
	[SerializeField]
	private bool m_AdjustPlaceholderLayoutHeight;

	// Token: 0x04000894 RID: 2196
	private AnimationManager m_AnimationManager;
}
