using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class AnimationManager : MonoBehaviour
{
	// Token: 0x06000762 RID: 1890 RVA: 0x00035F59 File Offset: 0x00034159
	public void SetAnimationSpeedScale(float scale)
	{
		this.m_AnimationSpeedScale = scale;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00035F62 File Offset: 0x00034162
	public void AddOnBeginAnimationCallback(AnimationManager.AnimationManagerCallback callback)
	{
		this.m_OnBeginAnimationCallback = (AnimationManager.AnimationManagerCallback)Delegate.Combine(this.m_OnBeginAnimationCallback, callback);
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00035F7B File Offset: 0x0003417B
	public void RemoveOnBeginAnimationCallback(AnimationManager.AnimationManagerCallback callback)
	{
		this.m_OnBeginAnimationCallback = (AnimationManager.AnimationManagerCallback)Delegate.Remove(this.m_OnBeginAnimationCallback, callback);
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x00035F94 File Offset: 0x00034194
	public void AddOnEndAnimationCallback(AnimationManager.AnimationManagerCallback callback)
	{
		this.m_OnEndAnimationCallback = (AnimationManager.AnimationManagerCallback)Delegate.Combine(this.m_OnEndAnimationCallback, callback);
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00035FAD File Offset: 0x000341AD
	public void RemoveOnEndAnimationCallback(AnimationManager.AnimationManagerCallback callback)
	{
		this.m_OnEndAnimationCallback = (AnimationManager.AnimationManagerCallback)Delegate.Remove(this.m_OnEndAnimationCallback, callback);
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00035FC6 File Offset: 0x000341C6
	public AnimationLayer GetDefaultAnimationLayer()
	{
		return this.m_DefaultAnimationLayer;
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x00035FCE File Offset: 0x000341CE
	public bool GetHasAnimatingObject()
	{
		return this.m_AnimationList.Count != 0 || this.m_QueuedAnimationList.Count != 0;
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x00035FF0 File Offset: 0x000341F0
	public AnimationLocator GetAnimationLocator(int locatorIndex, int locatorID)
	{
		if (locatorIndex < 0 || this.m_LocatorMap == null || locatorIndex >= this.m_LocatorMap.Length)
		{
			return null;
		}
		if (this.m_LocatorMap[locatorIndex] == null)
		{
			return null;
		}
		foreach (AnimationLocatorEntry animationLocatorEntry in this.m_LocatorMap[locatorIndex])
		{
			if (animationLocatorEntry.m_LocatorID == locatorID)
			{
				return animationLocatorEntry.m_Locator;
			}
		}
		return null;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x00036078 File Offset: 0x00034278
	public bool SetAnimationLocator(int locatorIndex, int locatorID, AnimationLocator locator)
	{
		if (this.m_LocatorMap == null)
		{
			this.m_LocatorMap = new List<AnimationLocatorEntry>[64];
		}
		if (locatorIndex < 0 || locatorIndex >= this.m_LocatorMap.Length)
		{
			return false;
		}
		if (this.m_LocatorMap[locatorIndex] == null)
		{
			this.m_LocatorMap[locatorIndex] = new List<AnimationLocatorEntry>();
		}
		if (locator != null)
		{
			locator.SetAnimationManager(this);
		}
		foreach (AnimationLocatorEntry animationLocatorEntry in this.m_LocatorMap[locatorIndex])
		{
			if (animationLocatorEntry.m_LocatorID == locatorID)
			{
				animationLocatorEntry.m_Locator = locator;
				return true;
			}
		}
		AnimationLocatorEntry animationLocatorEntry2 = new AnimationLocatorEntry();
		animationLocatorEntry2.m_LocatorID = locatorID;
		animationLocatorEntry2.m_Locator = locator;
		this.m_LocatorMap[locatorIndex].Add(animationLocatorEntry2);
		return true;
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x0003614C File Offset: 0x0003434C
	public bool RemoveAnimationLocator(int locatorIndex, int locatorID)
	{
		if (locatorIndex < 0 || this.m_LocatorMap == null || locatorIndex >= this.m_LocatorMap.Length)
		{
			return false;
		}
		if (this.m_LocatorMap[locatorIndex] == null)
		{
			return false;
		}
		foreach (AnimationLocatorEntry animationLocatorEntry in this.m_LocatorMap[locatorIndex])
		{
			if (animationLocatorEntry.m_LocatorID == locatorID)
			{
				animationLocatorEntry.m_Locator.SetAnimationManager(null);
				this.m_LocatorMap[locatorIndex].Remove(animationLocatorEntry);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000361EC File Offset: 0x000343EC
	public void RemoveAllAnimationLocators()
	{
		if (this.m_LocatorMap == null)
		{
			return;
		}
		for (int i = 0; i < this.m_LocatorMap.Length; i++)
		{
			if (this.m_LocatorMap[i] != null)
			{
				foreach (AnimationLocatorEntry animationLocatorEntry in this.m_LocatorMap[i])
				{
					animationLocatorEntry.m_Locator.SetAnimationManager(null);
				}
				this.m_LocatorMap[i].Clear();
			}
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00036278 File Offset: 0x00034478
	public void ResetAnimationManager()
	{
		this.m_WaitForAllAnimation = false;
		this.m_WaitForDestinationFlags = 0L;
		this.m_AnimationList.Clear();
		this.m_QueuedAnimationList.Clear();
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x000362A0 File Offset: 0x000344A0
	public AnimationEntry GetCurrentAnimationEntry(AnimateObject animate_object)
	{
		for (int i = 0; i < this.m_AnimationList.Count; i++)
		{
			if (this.m_AnimationList[i].m_AnimateObject == animate_object)
			{
				return this.m_AnimationList[i];
			}
		}
		return null;
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x000362EC File Offset: 0x000344EC
	public int CountCurrentAnimationEntriesToDestination(int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
		int num = 0;
		for (int i = 0; i < this.m_AnimationList.Count; i++)
		{
			if (this.m_AnimationList[i].m_DestinationLocatorIndex == destinationLocatorIndex && this.m_AnimationList[i].m_DestinationLocatorInstanceID == destinationLocatorInstanceID)
			{
				num++;
			}
		}
		for (int j = 0; j < this.m_QueuedAnimationList.Count; j++)
		{
			if (this.m_QueuedAnimationList[j].m_DestinationLocatorIndex == destinationLocatorIndex && this.m_QueuedAnimationList[j].m_DestinationLocatorInstanceID == destinationLocatorInstanceID)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x00036380 File Offset: 0x00034580
	public List<AnimationEntry> GetCurrentAnimationEntriesToDestination(int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
		List<AnimationEntry> list = new List<AnimationEntry>();
		for (int i = 0; i < this.m_AnimationList.Count; i++)
		{
			if (this.m_AnimationList[i].m_DestinationLocatorIndex == destinationLocatorIndex && this.m_AnimationList[i].m_DestinationLocatorInstanceID == destinationLocatorInstanceID)
			{
				list.Add(this.m_AnimationList[i]);
			}
		}
		return list;
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x000363E4 File Offset: 0x000345E4
	public uint GetCurrentAnimationToDestinationFlags(int destinationLocatorIndex)
	{
		uint num = 0U;
		for (int i = 0; i < this.m_AnimationList.Count; i++)
		{
			if (this.m_AnimationList[i].m_DestinationLocatorIndex == destinationLocatorIndex)
			{
				num |= 1U << this.m_AnimationList[i].m_DestinationLocatorInstanceID;
			}
		}
		for (int j = 0; j < this.m_QueuedAnimationList.Count; j++)
		{
			if (this.m_QueuedAnimationList[j].m_DestinationLocatorIndex == destinationLocatorIndex)
			{
				num |= 1U << this.m_AnimationList[j].m_DestinationLocatorInstanceID;
			}
		}
		return num;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x0003647C File Offset: 0x0003467C
	public void AddToAnimationList(AnimateObject animate_object, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID, uint animationFlags, Vector3 additionalRotation, float delayAtStart, float pauseAtDestination, bool bCancelQueuedAnimation)
	{
		if (animate_object == null)
		{
			return;
		}
		this.RemoveFromAnimationList(animate_object, bCancelQueuedAnimation);
		AnimationEntry animationEntry = new AnimationEntry();
		animationEntry.m_AnimateObject = animate_object;
		animationEntry.m_SourceLocatorIndex = sourceLocatorIndex;
		animationEntry.m_SourceLocatorInstanceID = sourceLocatorInstanceID;
		animationEntry.m_DestinationLocatorIndex = destinationLocatorIndex;
		animationEntry.m_DestinationLocatorInstanceID = destinationLocatorInstanceID;
		animationEntry.m_AnimationFlags = animationFlags;
		animationEntry.m_DelayAtStart = delayAtStart;
		animationEntry.m_PauseAtDestination = pauseAtDestination;
		animationEntry.m_AdditionalRotation = additionalRotation;
		animationEntry.m_bApplyUpdateAtDestination = false;
		this.m_AnimationList.Add(animationEntry);
		if (this.m_OnBeginAnimationCallback != null)
		{
			this.m_OnBeginAnimationCallback(animate_object, animationFlags, sourceLocatorIndex, sourceLocatorInstanceID, destinationLocatorIndex, destinationLocatorInstanceID);
		}
		if (destinationLocatorIndex != 0)
		{
			this.m_AnimationDestinationFlags |= 1L << destinationLocatorIndex;
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00036530 File Offset: 0x00034730
	public bool RemoveFromAnimationList(AnimateObject animate_object, bool bCancelQueuedAnimation)
	{
		if (animate_object == null)
		{
			return false;
		}
		bool result = false;
		this.m_AnimationDestinationFlags = 0L;
		int i = 0;
		while (i < this.m_AnimationList.Count)
		{
			AnimationEntry animationEntry = this.m_AnimationList[i];
			if (animationEntry.m_AnimateObject == animate_object)
			{
				this.m_AnimationList.RemoveAt(i);
				result = true;
				if (this.m_OnEndAnimationCallback != null)
				{
					this.m_OnEndAnimationCallback(animate_object, animationEntry.m_AnimationFlags, animationEntry.m_SourceLocatorIndex, animationEntry.m_SourceLocatorInstanceID, animationEntry.m_DestinationLocatorIndex, animationEntry.m_DestinationLocatorInstanceID);
				}
			}
			else
			{
				if (animationEntry.m_DestinationLocatorIndex != 0)
				{
					this.m_AnimationDestinationFlags |= (long)(1UL << (animationEntry.m_DestinationLocatorIndex & 31));
				}
				i++;
			}
		}
		if (bCancelQueuedAnimation)
		{
			i = 0;
			while (i < this.m_QueuedAnimationList.Count)
			{
				AnimationEntry animationEntry2 = this.m_QueuedAnimationList[i];
				if (animationEntry2.m_AnimateObject == animate_object)
				{
					this.m_QueuedAnimationList.RemoveAt(i);
				}
				else
				{
					if (animationEntry2.m_DestinationLocatorIndex != 0)
					{
						this.m_AnimationDestinationFlags |= (long)(1UL << (animationEntry2.m_DestinationLocatorIndex & 31));
					}
					i++;
				}
			}
		}
		else
		{
			for (i = 0; i < this.m_QueuedAnimationList.Count; i++)
			{
				AnimationEntry animationEntry3 = this.m_QueuedAnimationList[i];
				if (animationEntry3.m_DestinationLocatorIndex != 0)
				{
					this.m_AnimationDestinationFlags |= (long)(1UL << (animationEntry3.m_DestinationLocatorIndex & 31));
				}
			}
		}
		this.m_WaitForDestinationFlags &= this.m_AnimationDestinationFlags;
		if (this.m_WaitForAllAnimation && this.m_AnimationList.Count == 0 && this.m_QueuedAnimationList.Count == 0)
		{
			this.m_WaitForAllAnimation = false;
		}
		return result;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x000366D8 File Offset: 0x000348D8
	public bool StartAnimation(AnimateObject animate_object, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID, uint animationFlags, Vector3 additionalRotation, float delayAtStart = 0f, float pauseAtDestination = 0f, bool bCancelQueuedAnimation = true)
	{
		if (animate_object == null)
		{
			return false;
		}
		Vector3 vector = additionalRotation;
		AnimationLocator animationLocator = this.GetAnimationLocator(sourceLocatorIndex, sourceLocatorInstanceID);
		if (animationLocator != null)
		{
			vector -= animationLocator.GetAdditionalRotation();
		}
		if ((!animate_object.gameObject.activeInHierarchy || (animationFlags & 2147483648U) != 0U) && animationLocator != null)
		{
			animationLocator.AnimateFromLocator(animate_object);
		}
		AnimationLocator animationLocator2 = this.GetAnimationLocator(destinationLocatorIndex, destinationLocatorInstanceID);
		if (animationLocator2 != null)
		{
			animationLocator2 = animationLocator2.GetOverrideDestinationLocator(animate_object);
			if (animationLocator2.AnimateToLocator(animate_object, animationLocator, delayAtStart, pauseAtDestination))
			{
				vector += animationLocator2.GetAdditionalRotation();
				animate_object.SetAnimateAdditionalRotation(vector);
				if (!animate_object.gameObject.activeInHierarchy)
				{
					animate_object.ResetCurrentScale();
					animate_object.gameObject.SetActive(true);
				}
				this.AddToAnimationList(animate_object, sourceLocatorIndex, sourceLocatorInstanceID, destinationLocatorIndex, destinationLocatorInstanceID, animationFlags, additionalRotation, delayAtStart, pauseAtDestination, bCancelQueuedAnimation);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x000367B0 File Offset: 0x000349B0
	public bool QueueAnimation(AnimateObject animate_object, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID, uint animationFlags, Vector3 additionalRotation, float delayAtStart = 0f, float pauseAtDestination = 0f)
	{
		for (int i = 0; i < this.m_AnimationList.Count; i++)
		{
			if (!(this.m_AnimationList[i].m_AnimateObject != animate_object))
			{
				AnimationEntry animationEntry = new AnimationEntry();
				animationEntry.m_AnimateObject = animate_object;
				animationEntry.m_SourceLocatorIndex = sourceLocatorIndex;
				animationEntry.m_SourceLocatorInstanceID = sourceLocatorInstanceID;
				animationEntry.m_DestinationLocatorIndex = destinationLocatorIndex;
				animationEntry.m_DestinationLocatorInstanceID = destinationLocatorInstanceID;
				animationEntry.m_AnimationFlags = animationFlags;
				animationEntry.m_DelayAtStart = delayAtStart;
				animationEntry.m_PauseAtDestination = pauseAtDestination;
				animationEntry.m_AdditionalRotation = additionalRotation;
				animationEntry.m_bApplyUpdateAtDestination = false;
				this.m_QueuedAnimationList.Add(animationEntry);
				if (destinationLocatorIndex != 0)
				{
					this.m_AnimationDestinationFlags |= (long)(1UL << (destinationLocatorIndex & 31));
				}
				return true;
			}
		}
		return this.StartAnimation(animate_object, sourceLocatorIndex, sourceLocatorInstanceID, destinationLocatorIndex, destinationLocatorInstanceID, animationFlags, additionalRotation, 0f, 0f, true);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0003688C File Offset: 0x00034A8C
	public bool CheckForQueuedAnimation(AnimateObject animate_object)
	{
		for (int i = 0; i < this.m_QueuedAnimationList.Count; i++)
		{
			AnimationEntry animationEntry = this.m_QueuedAnimationList[i];
			if (!(animationEntry.m_AnimateObject != animate_object))
			{
				this.StartAnimation(animate_object, animationEntry.m_SourceLocatorIndex, animationEntry.m_SourceLocatorInstanceID, animationEntry.m_DestinationLocatorIndex, animationEntry.m_DestinationLocatorInstanceID, animationEntry.m_AnimationFlags, animationEntry.m_AdditionalRotation, animationEntry.m_DelayAtStart, animationEntry.m_PauseAtDestination, false);
				this.m_QueuedAnimationList.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00036914 File Offset: 0x00034B14
	public bool StartAnimationToLocator(AnimateObject animate_object, AnimationLocator destinationLocator, int destinationLocatorIndex = 0, AnimationLocator sourceLocator = null, float delayAtStart = 0f, float pauseAtDestination = 0f, bool bCancelQueuedAnimation = true)
	{
		if (!animate_object.gameObject.activeInHierarchy)
		{
			if (sourceLocator != null)
			{
				sourceLocator.AnimateFromLocator(animate_object);
			}
			animate_object.gameObject.SetActive(true);
		}
		if (destinationLocator != null && destinationLocator.AnimateToLocator(animate_object, sourceLocator, delayAtStart, pauseAtDestination))
		{
			this.AddToAnimationList(animate_object, 0, 0, destinationLocatorIndex, 0, 0U, Vector3.zero, delayAtStart, pauseAtDestination, bCancelQueuedAnimation);
			return true;
		}
		return false;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00036980 File Offset: 0x00034B80
	public bool StartAnimationToPlaceholder(AnimateObject animate_object, GameObject target_placeholder, int destinationLocatorIndex = 0, int destinationLocatorInstanceID = 0, float delayAtStart = 0f, float pauseAtDestination = 0f, bool bCancelQueuedAnimation = true)
	{
		if (animate_object != null && target_placeholder != null)
		{
			animate_object.StartAnimationToPlaceholder(target_placeholder, delayAtStart, pauseAtDestination);
			this.AddToAnimationList(animate_object, 0, 0, destinationLocatorIndex, destinationLocatorInstanceID, 0U, Vector3.zero, delayAtStart, pauseAtDestination, bCancelQueuedAnimation);
			return true;
		}
		return false;
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00003022 File Offset: 0x00001222
	public virtual void SetAnimationRatesReturnDragObject(AnimateObject animate_object)
	{
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x000369C5 File Offset: 0x00034BC5
	public bool IsAnimationToDestination(int destinationLocatorIndex)
	{
		return (1L << (destinationLocatorIndex & 31) & this.m_AnimationDestinationFlags) != 0L;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x000369D9 File Offset: 0x00034BD9
	public virtual bool PauseForAnimationManager()
	{
		return this.m_WaitForAllAnimation || this.m_WaitForDestinationFlags != 0L;
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x000369EF File Offset: 0x00034BEF
	public bool AddWaitForAllAnimation()
	{
		if (this.m_AnimationList.Count > 0 || this.m_QueuedAnimationList.Count > 0)
		{
			this.m_WaitForAllAnimation = true;
			return true;
		}
		return false;
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00036A18 File Offset: 0x00034C18
	public bool AddWaitForDestination(int destinationLocatorIndex)
	{
		long num = 1L << (destinationLocatorIndex & 31);
		if ((num & this.m_AnimationDestinationFlags) != 0L)
		{
			this.m_WaitForDestinationFlags |= num;
			return true;
		}
		return false;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00036A48 File Offset: 0x00034C48
	public void AddWaitForDestinationFlags(long destinationFlags)
	{
		destinationFlags &= this.m_AnimationDestinationFlags;
		this.m_WaitForDestinationFlags |= destinationFlags;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00036A62 File Offset: 0x00034C62
	public void AddUpdateAnimateList(AnimateObject card)
	{
		if (this.m_UpdateAnimateList != null && !this.m_UpdateAnimateList.Contains(card))
		{
			this.m_UpdateAnimateList.Add(card);
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00036A86 File Offset: 0x00034C86
	public void RemoveUpdateAnimateList(AnimateObject card)
	{
		if (this.m_UpdateAnimateList != null)
		{
			this.m_UpdateAnimateList.Remove(card);
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00036AA0 File Offset: 0x00034CA0
	protected virtual void Update()
	{
		if (this.m_UpdateAnimateList != null)
		{
			float num = Mathf.Clamp(this.m_AnimationSpeedScale, 0f, 1f);
			float animationSpeedScale = 1f + (this.m_MaxAnimationSpeed - 1f) * num;
			for (int i = this.m_UpdateAnimateList.Count - 1; i >= 0; i--)
			{
				AnimateObject animateObject = this.m_UpdateAnimateList[i];
				if (animateObject == null || animateObject.UpdateAnimate(animationSpeedScale))
				{
					this.m_UpdateAnimateList.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x040008A1 RID: 2209
	public const uint k_AnimationFlagForceToStart = 2147483648U;

	// Token: 0x040008A2 RID: 2210
	[SerializeField]
	private AnimationLayer m_DefaultAnimationLayer;

	// Token: 0x040008A3 RID: 2211
	private List<AnimationLocatorEntry>[] m_LocatorMap;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private List<AnimationEntry> m_AnimationList = new List<AnimationEntry>();

	// Token: 0x040008A5 RID: 2213
	private List<AnimationEntry> m_QueuedAnimationList = new List<AnimationEntry>();

	// Token: 0x040008A6 RID: 2214
	private long m_AnimationDestinationFlags;

	// Token: 0x040008A7 RID: 2215
	private long m_WaitForDestinationFlags;

	// Token: 0x040008A8 RID: 2216
	private bool m_WaitForAllAnimation;

	// Token: 0x040008A9 RID: 2217
	private AnimationManager.AnimationManagerCallback m_OnBeginAnimationCallback;

	// Token: 0x040008AA RID: 2218
	private AnimationManager.AnimationManagerCallback m_OnEndAnimationCallback;

	// Token: 0x040008AB RID: 2219
	private List<AnimateObject> m_UpdateAnimateList = new List<AnimateObject>();

	// Token: 0x040008AC RID: 2220
	private float m_AnimationSpeedScale;

	// Token: 0x040008AD RID: 2221
	private float m_MaxAnimationSpeed = 5f;

	// Token: 0x0200079A RID: 1946
	// (Invoke) Token: 0x06004293 RID: 17043
	public delegate void AnimationManagerCallback(AnimateObject animateObject, uint animationFlags, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID);
}
