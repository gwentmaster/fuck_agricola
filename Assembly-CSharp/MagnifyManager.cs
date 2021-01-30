using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class MagnifyManager : MonoBehaviour
{
	// Token: 0x06000825 RID: 2085 RVA: 0x00039198 File Offset: 0x00037398
	public virtual AnimationLocator GetMagnifyCardLocator(CardObject magnifyCard)
	{
		return this.m_MagnifyCardLocator;
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x000391A0 File Offset: 0x000373A0
	public CardObject GetMagnifiedCard()
	{
		return this.m_MagnifiedCard;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x000391A8 File Offset: 0x000373A8
	public void AddOnMagnifyCallback(MagnifyManager.MagnifyCallback callback)
	{
		this.m_OnMagnifyCallback = (MagnifyManager.MagnifyCallback)Delegate.Combine(this.m_OnMagnifyCallback, callback);
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x000391C1 File Offset: 0x000373C1
	public void RemoveOnMagnifyCallback(MagnifyManager.MagnifyCallback callback)
	{
		this.m_OnMagnifyCallback = (MagnifyManager.MagnifyCallback)Delegate.Remove(this.m_OnMagnifyCallback, callback);
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x000391DA File Offset: 0x000373DA
	public void AddOnUnmagnifyCallback(MagnifyManager.MagnifyCallback callback)
	{
		this.m_OnUnmagnifyCallback = (MagnifyManager.MagnifyCallback)Delegate.Combine(this.m_OnUnmagnifyCallback, callback);
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x000391F3 File Offset: 0x000373F3
	public void RemoveOnUnmagnifyCallback(MagnifyManager.MagnifyCallback callback)
	{
		this.m_OnUnmagnifyCallback = (MagnifyManager.MagnifyCallback)Delegate.Remove(this.m_OnUnmagnifyCallback, callback);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x0003920C File Offset: 0x0003740C
	private void Start()
	{
		if (this.m_MagnifyCardPanel != null)
		{
			this.m_MagnifyCardPanel.SetActive(false);
		}
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00039228 File Offset: 0x00037428
	public bool PauseForMagnifyManager()
	{
		return this.m_MagnifiedCard != null;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00039236 File Offset: 0x00037436
	public void SetOverrideLayerObject(GameObject obj)
	{
		this.m_MagnifyCardOverrideLayer = obj;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0003923F File Offset: 0x0003743F
	public void SetOverridePanelObject(GameObject obj)
	{
		this.m_MagnifyCardPanelOverride = obj;
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00039248 File Offset: 0x00037448
	public void SetUseOverrideLayer(bool bUseLayer)
	{
		this.m_bUseOverrideLayer = bUseLayer;
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00039254 File Offset: 0x00037454
	public virtual bool StartMagnifyCard(CardObject magnify_card)
	{
		this.m_MagnifiedCard = magnify_card;
		if (this.m_OnMagnifyCallback != null)
		{
			this.m_OnMagnifyCallback(this.m_MagnifiedCard);
		}
		GameObject gameObject = (this.m_bUseOverrideLayer && this.m_MagnifyCardPanelOverride != null) ? this.m_MagnifyCardPanelOverride : this.m_MagnifyCardPanel;
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		AnimationLocator magnifyCardLocator = this.GetMagnifyCardLocator(magnify_card);
		if (magnifyCardLocator != null)
		{
			AnimateObject component = magnify_card.GetComponent<AnimateObject>();
			if (component != null)
			{
				AnimationManager animationManager = component.GetAnimationManager();
				if (animationManager != null && animationManager.StartAnimationToLocator(component, magnifyCardLocator, 0, null, 0f, 0f, true))
				{
					component.SetAdjustPlaceholderLayoutWidth(false);
					component.SetAdjustPlaceholderLayoutHeight(false);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00039310 File Offset: 0x00037510
	public virtual bool RemoveMagnifiedCard(CardObject magnify)
	{
		if (this.m_MagnifiedCard != magnify)
		{
			return false;
		}
		GameObject gameObject = (this.m_bUseOverrideLayer && this.m_MagnifyCardPanelOverride != null) ? this.m_MagnifyCardPanelOverride : this.m_MagnifyCardPanel;
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		DragObject component = this.m_MagnifiedCard.GetComponent<DragObject>();
		if (component != null)
		{
			component.AnimateToReturnPlaceholder();
			AnimationManager animationManager = component.GetAnimationManager();
			if (animationManager != null)
			{
				AnimationLayer animationLayer;
				if (this.m_bUseOverrideLayer && this.m_MagnifyCardOverrideLayer != null)
				{
					animationLayer = this.m_MagnifyCardOverrideLayer.GetComponent<AnimationLayer>();
				}
				else
				{
					animationLayer = animationManager.GetDefaultAnimationLayer();
				}
				if (animationLayer != null)
				{
					animationLayer.AddAnimation(component);
				}
			}
			component.SetAdjustPlaceholderLayoutWidth(false);
			component.SetAdjustPlaceholderLayoutHeight(false);
		}
		this.m_MagnifiedCard = null;
		if (this.m_OnUnmagnifyCallback != null)
		{
			this.m_OnUnmagnifyCallback(magnify);
		}
		return true;
	}

	// Token: 0x04000909 RID: 2313
	[SerializeField]
	private GameObject m_MagnifyCardPanel;

	// Token: 0x0400090A RID: 2314
	[SerializeField]
	private GameObject m_MagnifyCardPanelOverride;

	// Token: 0x0400090B RID: 2315
	[SerializeField]
	private AnimationLocator m_MagnifyCardLocator;

	// Token: 0x0400090C RID: 2316
	[SerializeField]
	private GameObject m_MagnifyCardOverrideLayer;

	// Token: 0x0400090D RID: 2317
	private CardObject m_MagnifiedCard;

	// Token: 0x0400090E RID: 2318
	private bool m_bUseOverrideLayer;

	// Token: 0x0400090F RID: 2319
	private MagnifyManager.MagnifyCallback m_OnMagnifyCallback;

	// Token: 0x04000910 RID: 2320
	private MagnifyManager.MagnifyCallback m_OnUnmagnifyCallback;

	// Token: 0x020007A3 RID: 1955
	// (Invoke) Token: 0x060042AB RID: 17067
	public delegate void MagnifyCallback(CardObject magnifyCard);
}
