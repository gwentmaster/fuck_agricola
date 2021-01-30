using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C6 RID: 198
public class AnimateObject : MonoBehaviour
{
	// Token: 0x06000719 RID: 1817 RVA: 0x000349DD File Offset: 0x00032BDD
	public AnimationManager GetAnimationManager()
	{
		return this.m_AnimationManager;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x000349E5 File Offset: 0x00032BE5
	public void SetAnimationManager(AnimationManager manager)
	{
		this.m_AnimationManager = manager;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x000349EE File Offset: 0x00032BEE
	public void AddOnBeginAnimationCallback(AnimateObject.AnimationCallback callback)
	{
		this.m_OnBeginAnimationCallback = (AnimateObject.AnimationCallback)Delegate.Combine(this.m_OnBeginAnimationCallback, callback);
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00034A07 File Offset: 0x00032C07
	public void RemoveOnBeginAnimationCallback(AnimateObject.AnimationCallback callback)
	{
		this.m_OnBeginAnimationCallback = (AnimateObject.AnimationCallback)Delegate.Remove(this.m_OnBeginAnimationCallback, callback);
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00034A20 File Offset: 0x00032C20
	public void AddOnReachDestinationCallback(AnimateObject.AnimationCallback callback)
	{
		this.m_OnReachDestinationCallback = (AnimateObject.AnimationCallback)Delegate.Combine(this.m_OnReachDestinationCallback, callback);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00034A39 File Offset: 0x00032C39
	public void RemoveOnReachDestinationCallback(AnimateObject.AnimationCallback callback)
	{
		this.m_OnReachDestinationCallback = (AnimateObject.AnimationCallback)Delegate.Remove(this.m_OnReachDestinationCallback, callback);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00034A52 File Offset: 0x00032C52
	public void AddOnEndAnimationCallback(AnimateObject.AnimationCallback callback)
	{
		this.m_OnEndAnimationCallback = (AnimateObject.AnimationCallback)Delegate.Combine(this.m_OnEndAnimationCallback, callback);
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00034A6B File Offset: 0x00032C6B
	public void RemoveOnEndAnimationCallback(AnimateObject.AnimationCallback callback)
	{
		this.m_OnEndAnimationCallback = (AnimateObject.AnimationCallback)Delegate.Remove(this.m_OnEndAnimationCallback, callback);
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00034A84 File Offset: 0x00032C84
	public void ResetAnimationRates()
	{
		this.m_AnimateMovementRateXY = 1024f;
		this.m_AnimateMovementRateZ = 384f;
		this.m_AnimateRotationRate = 720f;
		this.m_AnimateScaleRate = 1.8f;
		this.m_AnimateInternalRotationRate = 252f;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00034ABD File Offset: 0x00032CBD
	public void SetAnimateMovementRateXY(float rate)
	{
		this.m_AnimateMovementRateXY = rate;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00034AC6 File Offset: 0x00032CC6
	public void SetAnimateMovementRateZ(float rate)
	{
		this.m_AnimateMovementRateZ = rate;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00034ACF File Offset: 0x00032CCF
	public void SetAnimateRotationRate(float rate)
	{
		this.m_AnimateRotationRate = rate;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00034AD8 File Offset: 0x00032CD8
	public void SetAnimateScaleRate(float rate)
	{
		this.m_AnimateScaleRate = rate;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00034AE1 File Offset: 0x00032CE1
	public void SetAnimateInternalRotationRate(float rate)
	{
		this.m_AnimateInternalRotationRate = rate;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00034AEA File Offset: 0x00032CEA
	public float GetAnimateCurrentHeight()
	{
		return this.m_AnimateCurrentHeight;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00034AF2 File Offset: 0x00032CF2
	public float GetAnimateTargetHeight()
	{
		return this.m_AnimateTargetHeight;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00034AFA File Offset: 0x00032CFA
	public void SetAnimateCurrentHeight(float height)
	{
		this.m_AnimateCurrentHeight = height;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00034B03 File Offset: 0x00032D03
	public void SetAnimateTargetHeight(float height)
	{
		this.m_AnimateTargetHeight = height;
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00034B0C File Offset: 0x00032D0C
	public void SetAnimateAdditionalRotation(Vector3 rotation)
	{
		this.m_AnimateAdditionalRotation = rotation;
		this.m_bHasAdditionalRotation = (rotation.magnitude > 0.1f);
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00034B29 File Offset: 0x00032D29
	public bool IsAnimating()
	{
		return this.m_bIsAnimating;
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00034B31 File Offset: 0x00032D31
	public void SetAdjustPlaceholderLayoutWidth(bool bAdjust)
	{
		this.m_bAdjustPlaceholderLayoutWidth = bAdjust;
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00034B3A File Offset: 0x00032D3A
	public void SetAdjustPlaceholderLayoutHeight(bool bAdjust)
	{
		this.m_bAdjustPlaceholderLayoutHeight = bAdjust;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x00034B43 File Offset: 0x00032D43
	public float GetCurrentScale()
	{
		return this.m_CurrentScale;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00034B4B File Offset: 0x00032D4B
	public void SetCurrentScale(float scale)
	{
		this.m_CurrentScale = scale;
		this.ResetCurrentScale();
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.AddUpdateAnimateList(this);
		}
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x00034B74 File Offset: 0x00032D74
	public void ResetCurrentScale()
	{
		base.transform.localScale = new Vector3(this.m_CurrentScale, this.m_CurrentScale, 1f);
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00034B98 File Offset: 0x00032D98
	public void InheritCurrentScale()
	{
		float num = (base.transform.localScale.x + base.transform.localScale.y) * 0.5f;
		this.SetCurrentScale(num);
		this.SetTargetScale(num);
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x00034BDB File Offset: 0x00032DDB
	public float GetTargetScale()
	{
		return this.m_TargetScale;
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00034BE3 File Offset: 0x00032DE3
	public void SetTargetScale(float scale)
	{
		this.m_TargetScale = scale;
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.AddUpdateAnimateList(this);
		}
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00034C06 File Offset: 0x00032E06
	public void SetInternalRotationNode(GameObject rotationNode)
	{
		this.m_InternalRotationNode = rotationNode;
		if (this.m_InternalRotationNode != null)
		{
			this.m_TargetInternalRotation = this.m_InternalRotationNode.transform.localRotation;
		}
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00034C34 File Offset: 0x00032E34
	public void SetTargetInternalRotation(Vector3 rotation, bool bImmediate)
	{
		if (this.m_InternalRotationNode != null)
		{
			this.m_TargetInternalRotation = Quaternion.Euler(rotation);
			if (bImmediate)
			{
				this.m_InternalRotationNode.transform.localRotation = this.m_TargetInternalRotation;
				return;
			}
			if (this.m_AnimationManager != null)
			{
				this.m_AnimationManager.AddUpdateAnimateList(this);
			}
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00034C8F File Offset: 0x00032E8F
	public void IncrementDontUpdateInternalRotation()
	{
		this.m_DontUpdateInternalRotation++;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00034C9F File Offset: 0x00032E9F
	public void DecrementDontUpdateInternalRotation()
	{
		if (this.m_DontUpdateInternalRotation > 0)
		{
			this.m_DontUpdateInternalRotation--;
		}
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00034CB8 File Offset: 0x00032EB8
	public bool SetAnimationPauseAtDestination(float pause)
	{
		if (this.IsAnimating())
		{
			this.m_AnimationPauseAtDestination = pause;
			return false;
		}
		return false;
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00034CCC File Offset: 0x00032ECC
	public void SetDestroyAfterAnimation()
	{
		this.m_bDestroyAfterAnimation = true;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00034CD8 File Offset: 0x00032ED8
	public GameObject GetPlaceholder(bool bCreateIfNeeded)
	{
		if (this.m_Placeholder == null && bCreateIfNeeded)
		{
			this.m_Placeholder = new GameObject();
			this.m_Placeholder.name = "[placeholder] " + base.name;
			this.m_Placeholder.AddComponent<AnimatePlaceholder>().SetOwner(this);
		}
		return this.m_Placeholder;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00034D34 File Offset: 0x00032F34
	public void ResetPlaceholderLayout(float scaleMin = 1f, float scalePreferred = 1f)
	{
		if (this.m_Placeholder == null)
		{
			return;
		}
		LayoutElement component = base.GetComponent<LayoutElement>();
		if (component != null)
		{
			LayoutElement layoutElement = this.m_Placeholder.AddComponent<LayoutElement>();
			if (layoutElement != null)
			{
				layoutElement.minWidth = component.minWidth * scaleMin;
				layoutElement.minHeight = component.minHeight * scaleMin;
				layoutElement.preferredWidth = component.preferredWidth * scalePreferred;
				layoutElement.preferredHeight = component.preferredHeight * scalePreferred;
				layoutElement.flexibleWidth = 0f;
				layoutElement.flexibleHeight = 0f;
			}
		}
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00034DC3 File Offset: 0x00032FC3
	private GameObject AssignPlaceholder(GameObject placeholder)
	{
		if (this.m_Placeholder != null && this.m_Placeholder != placeholder)
		{
			this.DestroyPlaceholder();
		}
		this.m_Placeholder = placeholder;
		return this.m_Placeholder;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00034DF4 File Offset: 0x00032FF4
	public void DestroyPlaceholder()
	{
		if (this.m_Placeholder != null)
		{
			UnityEngine.Object.Destroy(this.m_Placeholder);
			this.m_Placeholder = null;
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00034E18 File Offset: 0x00033018
	private void StopAnimation()
	{
		if (!this.m_bIsAnimating)
		{
			return;
		}
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.RemoveFromAnimationList(this, true);
		}
		this.DestroyPlaceholder();
		this.m_DestinationLocator = null;
		this.m_bIsAnimating = false;
		this.m_bAnimateToPlaceholder = false;
		this.m_AnimationDelayAtStart = 0f;
		this.m_AnimationPauseAtDestination = 0f;
		this.m_bHasAdditionalRotation = false;
		this.m_AnimateAdditionalRotation = Vector3.zero;
		this.m_bAdjustPlaceholderLayoutWidth = false;
		this.m_bAdjustPlaceholderLayoutHeight = false;
		this.m_bReachedDestination = false;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00034EA2 File Offset: 0x000330A2
	public bool DestroyAnimation()
	{
		if (!this.m_bDestroyAfterAnimation)
		{
			return false;
		}
		if (this.m_OnEndAnimationCallback != null)
		{
			this.m_OnEndAnimationCallback(this, null, null);
		}
		this.StopAnimation();
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		return true;
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00034ED8 File Offset: 0x000330D8
	private void StartAnimation(float delayAtStart, float pauseAtDestination, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		this.m_bIsAnimating = true;
		if (this.m_AnimationManager != null)
		{
			this.m_AnimationManager.AddUpdateAnimateList(this);
		}
		this.m_bAnimateToPlaceholder = true;
		this.m_AnimationDelayAtStart = delayAtStart;
		this.m_AnimationPauseAtDestination = pauseAtDestination;
		this.m_bHasAdditionalRotation = false;
		this.m_AnimateAdditionalRotation = Vector3.zero;
		this.m_bReachedDestination = false;
		this.m_DestinationLocator = destinationLocator;
		if (destinationLocator != null)
		{
			this.m_bAdjustPlaceholderLayoutWidth = destinationLocator.GetAdjustPlaceholderLayoutWidth();
			this.m_bAdjustPlaceholderLayoutHeight = destinationLocator.GetAdjustPlaceholderLayoutHeight();
		}
		else
		{
			this.m_bAdjustPlaceholderLayoutWidth = false;
			this.m_bAdjustPlaceholderLayoutHeight = false;
		}
		if (this.m_OnBeginAnimationCallback != null)
		{
			this.m_OnBeginAnimationCallback(this, sourceLocator, this.m_DestinationLocator);
		}
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00034F8C File Offset: 0x0003318C
	public void StartAnimationToPlaceholder(GameObject placeholder, float delayAtStart = 0f, float pauseAtDestination = 0f)
	{
		this.AssignPlaceholder(placeholder);
		AnimationLocator destinationLocator = null;
		if (placeholder != null)
		{
			Transform parent = placeholder.transform.parent;
			if (parent != null)
			{
				destinationLocator = parent.gameObject.GetComponent<AnimationLocator>();
			}
		}
		this.StartAnimation(delayAtStart, pauseAtDestination, null, destinationLocator);
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00034FD7 File Offset: 0x000331D7
	public void StartAnimationToLocator(AnimationLocator destinationLocator, AnimationLocator sourceLocator, float delayAtStart = 0f, float pauseAtDestination = 0f)
	{
		this.StartAnimation(delayAtStart, pauseAtDestination, sourceLocator, destinationLocator);
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00034FE4 File Offset: 0x000331E4
	public void PlaceOnAnimationLocator(AnimationLocator destinationLocator, bool bSetPosition, bool bSetRotation, int setSiblingIndex)
	{
		if (this.m_Placeholder != null && this.m_Placeholder.transform.parent == destinationLocator.transform)
		{
			int siblingIndex = this.m_Placeholder.transform.GetSiblingIndex();
			base.transform.SetParent(destinationLocator.transform, true);
			base.transform.SetSiblingIndex(siblingIndex);
		}
		else if (setSiblingIndex >= 0 && setSiblingIndex <= destinationLocator.transform.childCount)
		{
			base.transform.SetParent(destinationLocator.transform, true);
			base.transform.SetSiblingIndex(setSiblingIndex);
		}
		else
		{
			base.transform.SetParent(destinationLocator.transform, true);
		}
		if (bSetPosition)
		{
			base.transform.position = destinationLocator.transform.position;
		}
		if (bSetRotation)
		{
			base.transform.rotation = destinationLocator.transform.rotation;
		}
		this.SetAnimateCurrentHeight(destinationLocator.GetAnimationHeight());
		this.SetAnimateTargetHeight(destinationLocator.GetAnimationHeight());
		if (this.m_OnEndAnimationCallback != null)
		{
			this.m_OnEndAnimationCallback(this, null, destinationLocator);
		}
		this.StopAnimation();
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x000350FC File Offset: 0x000332FC
	private void UpdatePlaceholder()
	{
		if (this.m_Placeholder != null)
		{
			LayoutElement component = base.GetComponent<LayoutElement>();
			if (component != null)
			{
				float preferredWidth = component.preferredWidth;
				float preferredHeight = component.preferredHeight;
				float num = Mathf.Sqrt(preferredWidth * preferredWidth + preferredHeight * preferredHeight);
				Transform parent = base.transform.parent;
				Vector3 vector = parent.InverseTransformPoint(base.transform.position);
				Vector3 vector2 = parent.InverseTransformPoint(this.m_Placeholder.transform.position);
				float num2 = vector.x - vector2.x;
				float num3 = vector.y - vector2.y;
				float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
				float num5 = num * 1.2f;
				float x = parent.lossyScale.x;
				float x2 = this.m_Placeholder.transform.parent.lossyScale.x;
				num5 *= x2 / x;
				float num6 = num4 / num5;
				if (num6 > 1f)
				{
					num6 = 1f;
				}
				float num7 = 1f - num6;
				LayoutElement component2 = this.m_Placeholder.GetComponent<LayoutElement>();
				if (component2 != null)
				{
					if (this.m_bAdjustPlaceholderLayoutWidth)
					{
						component2.minWidth = preferredWidth * num7;
						component2.preferredWidth = preferredWidth * num7;
					}
					else
					{
						component2.minWidth = preferredWidth;
						component2.preferredWidth = preferredWidth;
					}
					if (this.m_bAdjustPlaceholderLayoutHeight)
					{
						component2.minHeight = preferredHeight * num7;
						component2.preferredHeight = preferredHeight * num7;
						return;
					}
					component2.minHeight = preferredHeight;
					component2.preferredHeight = preferredHeight;
				}
			}
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00035288 File Offset: 0x00033488
	public bool UpdateAnimation(float animationSpeedScale)
	{
		if (!this.m_bAnimateToPlaceholder || this.m_Placeholder == null)
		{
			return true;
		}
		float num = Time.deltaTime;
		if (this.m_AnimationDelayAtStart > num)
		{
			this.m_AnimationDelayAtStart -= num;
			this.UpdatePlaceholder();
			return false;
		}
		num -= this.m_AnimationDelayAtStart;
		this.m_AnimationDelayAtStart = 0f;
		float num2 = 1f;
		bool flag = false;
		float num3 = this.m_AnimateMovementRateXY * num * animationSpeedScale;
		float num4 = this.m_AnimateMovementRateZ * num;
		float num5 = this.m_AnimateRotationRate * num;
		float num6 = this.m_AnimateScaleRate * num;
		float num7 = this.m_AnimateInternalRotationRate * num;
		Transform parent = base.transform.parent;
		Vector3 vector = parent.InverseTransformPoint(base.transform.position);
		Vector3 vector2 = parent.InverseTransformPoint(this.m_Placeholder.transform.position);
		float num8 = vector2.x - vector.x;
		float num9 = vector2.y - vector.y;
		float num10 = vector2.z - vector.z;
		float num11 = Mathf.Sqrt(num8 * num8 + num9 * num9);
		float num12 = Mathf.Abs(num10);
		float animateCurrentHeight = this.m_AnimateCurrentHeight;
		if (num11 > num3)
		{
			num2 = num3 / num11;
			flag = true;
		}
		if (num12 > num4)
		{
			float b = num4 / num12;
			num2 = Mathf.Min(num2, b);
			flag = true;
		}
		Quaternion rotation = base.transform.rotation;
		Quaternion rotation2 = this.m_Placeholder.transform.rotation;
		Vector3 vector3 = Vector2.zero;
		if (this.m_bHasAdditionalRotation)
		{
			num5 *= 2.5f;
			Vector3 eulerAngles = rotation.eulerAngles;
			vector3 = rotation2.eulerAngles + this.m_AnimateAdditionalRotation - eulerAngles;
			float num13 = Mathf.Abs(vector3.x);
			if (num13 > num5)
			{
				float b2 = num5 / num13;
				num2 = Mathf.Min(num2, b2);
			}
			float num14 = Mathf.Abs(vector3.y);
			if (num14 > num5)
			{
				float b3 = num5 / num14;
				num2 = Mathf.Min(num2, b3);
			}
			float num15 = Mathf.Abs(vector3.z);
			if (num15 > num5)
			{
				float b4 = num5 / num15;
				num2 = Mathf.Min(num2, b4);
			}
		}
		else
		{
			float num16 = Quaternion.Angle(rotation2, rotation);
			if (num16 > num5)
			{
				float b5 = num5 / num16;
				num2 = Mathf.Min(num2, b5);
				flag = true;
			}
		}
		float num17 = Mathf.Abs(this.m_TargetScale - this.m_CurrentScale);
		if (num17 > num6)
		{
			float b6 = num6 / num17;
			num2 = Mathf.Min(num2, b6);
			flag = true;
		}
		Quaternion b7 = Quaternion.identity;
		if (this.m_InternalRotationNode != null)
		{
			if (this.m_DontUpdateInternalRotation > 0)
			{
				flag = true;
			}
			else
			{
				b7 = this.m_InternalRotationNode.transform.localRotation;
				float num18 = Quaternion.Angle(this.m_TargetInternalRotation, b7);
				if (num18 > num7)
				{
					num2 = Mathf.Min(num2, num7 / num18);
					flag = true;
				}
			}
		}
		if (flag)
		{
			if (!this.m_bReachedDestination && (num2 < 0.99f || this.m_DontUpdateInternalRotation > 0))
			{
				Vector3 localPosition = base.transform.localPosition;
				localPosition.x += num8 * num2;
				localPosition.y += num9 * num2;
				localPosition.z += num10 * num2;
				base.transform.localPosition = localPosition;
				if (this.m_bHasAdditionalRotation)
				{
					Vector3 vector4 = vector3 * num2;
					base.transform.Rotate(vector4);
					Vector3 a = vector3 - vector4;
					Vector3 eulerAngles2 = base.transform.rotation.eulerAngles;
					this.m_AnimateAdditionalRotation = a + eulerAngles2 - rotation2.eulerAngles;
				}
				else
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, rotation2, num2);
				}
				if (this.m_CurrentScale != this.m_TargetScale)
				{
					float num19 = this.m_TargetScale - this.m_CurrentScale;
					this.SetCurrentScale(this.m_CurrentScale + num19 * num2);
				}
				if (this.m_InternalRotationNode != null && this.m_DontUpdateInternalRotation == 0)
				{
					this.m_InternalRotationNode.transform.localRotation = Quaternion.Slerp(this.m_InternalRotationNode.transform.localRotation, this.m_TargetInternalRotation, num2);
				}
				this.m_AnimateCurrentHeight += (this.m_AnimateTargetHeight - this.m_AnimateCurrentHeight) * num2;
				AnimationLayer component = base.transform.parent.GetComponent<AnimationLayer>();
				if (component != null)
				{
					component.UpdateAnimation(this, animateCurrentHeight, this.m_AnimateCurrentHeight);
				}
				this.UpdatePlaceholder();
				return false;
			}
			base.transform.position = this.m_Placeholder.transform.position;
			base.transform.rotation = this.m_Placeholder.transform.rotation;
			if (this.m_CurrentScale != this.m_TargetScale)
			{
				this.SetCurrentScale(this.m_TargetScale);
			}
			if (this.m_InternalRotationNode != null && this.m_DontUpdateInternalRotation == 0)
			{
				this.m_InternalRotationNode.transform.localRotation = this.m_TargetInternalRotation;
			}
			this.m_AnimateCurrentHeight = this.m_AnimateTargetHeight;
			AnimationLayer component2 = base.transform.parent.GetComponent<AnimationLayer>();
			if (component2 != null)
			{
				component2.UpdateAnimation(this, animateCurrentHeight, this.m_AnimateCurrentHeight);
			}
			this.UpdatePlaceholder();
			if (!this.m_bReachedDestination)
			{
				this.m_bReachedDestination = true;
				if (this.m_OnReachDestinationCallback != null)
				{
					this.m_OnReachDestinationCallback(this, null, null);
				}
			}
			if (this.m_AnimationPauseAtDestination > num)
			{
				this.m_AnimationPauseAtDestination -= num;
			}
			return false;
		}
		else
		{
			if (!this.m_bReachedDestination)
			{
				base.transform.position = this.m_Placeholder.transform.position;
				base.transform.rotation = this.m_Placeholder.transform.rotation;
				if (this.m_CurrentScale != this.m_TargetScale)
				{
					this.SetCurrentScale(this.m_TargetScale);
				}
				if (this.m_InternalRotationNode != null && this.m_DontUpdateInternalRotation == 0)
				{
					this.m_InternalRotationNode.transform.localRotation = this.m_TargetInternalRotation;
				}
				this.m_AnimateCurrentHeight = this.m_AnimateTargetHeight;
				AnimationLayer component3 = base.transform.parent.GetComponent<AnimationLayer>();
				if (component3 != null)
				{
					component3.UpdateAnimation(this, animateCurrentHeight, this.m_AnimateCurrentHeight);
				}
				this.UpdatePlaceholder();
				this.m_bReachedDestination = true;
				if (this.m_OnReachDestinationCallback != null)
				{
					this.m_OnReachDestinationCallback(this, null, null);
				}
			}
			if (this.m_AnimationPauseAtDestination > num)
			{
				this.m_AnimationPauseAtDestination -= num;
				return false;
			}
			num -= this.m_AnimationPauseAtDestination;
			this.m_AnimationPauseAtDestination = 0f;
			if (this.m_AnimationManager != null && this.m_AnimationManager.CheckForQueuedAnimation(this))
			{
				return false;
			}
			if (this.m_bDestroyAfterAnimation)
			{
				this.DestroyAnimation();
				return true;
			}
			if (this.m_DestinationLocator != null)
			{
				this.m_DestinationLocator.PlaceAnimateObject(this, true, true, false);
			}
			else
			{
				int siblingIndex = this.m_Placeholder.transform.GetSiblingIndex();
				base.transform.SetParent(this.m_Placeholder.transform.parent);
				base.transform.SetSiblingIndex(siblingIndex);
				if (this.m_OnEndAnimationCallback != null)
				{
					this.m_OnEndAnimationCallback(this, null, null);
				}
				this.StopAnimation();
			}
			return true;
		}
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0003599C File Offset: 0x00033B9C
	public bool UpdateScale()
	{
		if (this.m_CurrentScale == this.m_TargetScale)
		{
			return true;
		}
		float num = this.m_TargetScale - this.m_CurrentScale;
		float num2 = Mathf.Abs(num);
		float num3 = this.m_AnimateScaleRate * Time.deltaTime;
		if (num2 < num3)
		{
			this.SetCurrentScale(this.m_TargetScale);
			return true;
		}
		float num4 = num3 / num2;
		this.SetCurrentScale(this.m_CurrentScale + num * num4);
		return false;
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x00035A04 File Offset: 0x00033C04
	public bool UpdateInternalRotation()
	{
		if (this.m_InternalRotationNode == null)
		{
			return false;
		}
		if (this.m_DontUpdateInternalRotation > 0)
		{
			return false;
		}
		float deltaTime = Time.deltaTime;
		float num = 1f;
		bool flag = false;
		float num2 = this.m_AnimateInternalRotationRate * deltaTime;
		Quaternion localRotation = this.m_InternalRotationNode.transform.localRotation;
		float num3 = Quaternion.Angle(this.m_TargetInternalRotation, localRotation);
		if (num3 > num2)
		{
			num = Mathf.Min(num, num2 / num3);
			flag = true;
		}
		if (flag)
		{
			this.m_InternalRotationNode.transform.localRotation = Quaternion.Slerp(this.m_InternalRotationNode.transform.localRotation, this.m_TargetInternalRotation, num);
			return false;
		}
		this.m_InternalRotationNode.transform.localRotation = this.m_TargetInternalRotation;
		return true;
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00035AC0 File Offset: 0x00033CC0
	public virtual bool UpdateAnimate(float animationSpeedScale)
	{
		bool result;
		if (this.m_bIsAnimating)
		{
			result = this.UpdateAnimation(animationSpeedScale);
		}
		else
		{
			bool flag = this.UpdateScale();
			bool flag2 = this.UpdateInternalRotation();
			result = (flag && flag2);
		}
		return result;
	}

	// Token: 0x04000867 RID: 2151
	private AnimationManager m_AnimationManager;

	// Token: 0x04000868 RID: 2152
	[SerializeField]
	private GameObject m_Placeholder;

	// Token: 0x04000869 RID: 2153
	[SerializeField]
	private float m_CurrentScale = 1f;

	// Token: 0x0400086A RID: 2154
	[SerializeField]
	private float m_TargetScale = 1f;

	// Token: 0x0400086B RID: 2155
	private bool m_bIsAnimating;

	// Token: 0x0400086C RID: 2156
	private bool m_bAnimateToPlaceholder;

	// Token: 0x0400086D RID: 2157
	private float m_AnimationDelayAtStart;

	// Token: 0x0400086E RID: 2158
	private float m_AnimationPauseAtDestination;

	// Token: 0x0400086F RID: 2159
	[SerializeField]
	private float m_AnimateCurrentHeight;

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	private float m_AnimateTargetHeight;

	// Token: 0x04000871 RID: 2161
	private bool m_bHasAdditionalRotation;

	// Token: 0x04000872 RID: 2162
	private Vector3 m_AnimateAdditionalRotation = Vector3.zero;

	// Token: 0x04000873 RID: 2163
	private bool m_bAdjustPlaceholderLayoutWidth;

	// Token: 0x04000874 RID: 2164
	private bool m_bAdjustPlaceholderLayoutHeight;

	// Token: 0x04000875 RID: 2165
	private bool m_bReachedDestination;

	// Token: 0x04000876 RID: 2166
	private bool m_bDestroyAfterAnimation;

	// Token: 0x04000877 RID: 2167
	private GameObject m_InternalRotationNode;

	// Token: 0x04000878 RID: 2168
	private Quaternion m_TargetInternalRotation = Quaternion.identity;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private int m_DontUpdateInternalRotation;

	// Token: 0x0400087A RID: 2170
	private const float k_DefaultInternalRotationRate = 252f;

	// Token: 0x0400087B RID: 2171
	private AnimationLocator m_DestinationLocator;

	// Token: 0x0400087C RID: 2172
	private AnimateObject.AnimationCallback m_OnBeginAnimationCallback;

	// Token: 0x0400087D RID: 2173
	private AnimateObject.AnimationCallback m_OnReachDestinationCallback;

	// Token: 0x0400087E RID: 2174
	private AnimateObject.AnimationCallback m_OnEndAnimationCallback;

	// Token: 0x0400087F RID: 2175
	private const float k_ScalePlaceholderByDistance = 0.5f;

	// Token: 0x04000880 RID: 2176
	private const float k_DefaultMovementRateXY = 1024f;

	// Token: 0x04000881 RID: 2177
	private const float k_DefaultMovementRateZ = 384f;

	// Token: 0x04000882 RID: 2178
	private const float k_DefaultRotationRate = 720f;

	// Token: 0x04000883 RID: 2179
	private const float k_DefaultScaleRate = 1.8f;

	// Token: 0x04000884 RID: 2180
	private float m_AnimateMovementRateXY = 1024f;

	// Token: 0x04000885 RID: 2181
	private float m_AnimateMovementRateZ = 384f;

	// Token: 0x04000886 RID: 2182
	private float m_AnimateRotationRate = 720f;

	// Token: 0x04000887 RID: 2183
	private float m_AnimateScaleRate = 1.8f;

	// Token: 0x04000888 RID: 2184
	private float m_AnimateInternalRotationRate = 252f;

	// Token: 0x02000799 RID: 1945
	// (Invoke) Token: 0x0600428F RID: 17039
	public delegate void AnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator);
}
