using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class ResourceEntry
{
	// Token: 0x06000833 RID: 2099 RVA: 0x000393F4 File Offset: 0x000375F4
	public ResourceEntry(ResourceManager resourceManager, string resourceName, UnityEngine.Object resourceObject)
	{
		this.m_ResourceManager = resourceManager;
		this.m_ResourceName = resourceName;
		this.m_ResourceRequest = null;
		this.m_ResourceObject = resourceObject;
		this.m_bLoadCompleted = true;
		this.m_ReferenceCount = 0;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00039428 File Offset: 0x00037628
	public ResourceEntry(ResourceManager resourceManager, string resourceName, ResourceRequest resourceRequest)
	{
		this.m_ResourceManager = resourceManager;
		this.m_ResourceName = resourceName;
		this.m_ResourceRequest = resourceRequest;
		this.m_ResourceObject = null;
		this.m_bLoadCompleted = false;
		this.m_ReferenceCount = 0;
		if (this.m_ResourceRequest != null)
		{
			this.m_ResourceRequest.completed += this.DelayResourceRequestCompleted;
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00039484 File Offset: 0x00037684
	private IEnumerator DelayResourceRequest(AsyncOperation completedRequest)
	{
		float accumulatedTime = 0f;
		while (accumulatedTime < 3f)
		{
			accumulatedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		this.HandleResourceRequestCompleted(completedRequest);
		yield break;
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0003949A File Offset: 0x0003769A
	private void DelayResourceRequestCompleted(AsyncOperation completedRequest)
	{
		if (this.m_ResourceRequest != completedRequest)
		{
			return;
		}
		this.m_ResourceManager.StartCoroutine(this.DelayResourceRequest(completedRequest));
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x000394BC File Offset: 0x000376BC
	private void HandleResourceRequestCompleted(AsyncOperation completedRequest)
	{
		if (this.m_ResourceRequest != completedRequest)
		{
			return;
		}
		this.m_ResourceObject = this.m_ResourceRequest.asset;
		this.m_bLoadCompleted = true;
		this.m_ResourceRequest = null;
		if (this.m_OnLoadCompletedCallback != null)
		{
			this.m_OnLoadCompletedCallback(this);
			this.m_OnLoadCompletedCallback = null;
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0003950D File Offset: 0x0003770D
	public void AddOnLoadCompletedCallback(ResourceEntry.ResourceLoadCallback callback)
	{
		this.m_OnLoadCompletedCallback = (ResourceEntry.ResourceLoadCallback)Delegate.Combine(this.m_OnLoadCompletedCallback, callback);
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00039526 File Offset: 0x00037726
	public string GetResourceName()
	{
		return this.m_ResourceName;
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0003952E File Offset: 0x0003772E
	public bool HasLoadCompleted()
	{
		return this.m_bLoadCompleted;
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00039536 File Offset: 0x00037736
	public int GetCount()
	{
		return this.m_ReferenceCount;
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0003953E File Offset: 0x0003773E
	public void IncrementCount()
	{
		this.m_ReferenceCount++;
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0003954E File Offset: 0x0003774E
	public void DecrementCount()
	{
		this.m_ReferenceCount--;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0003955E File Offset: 0x0003775E
	public void ReleaseResourceEntry()
	{
		if (this.m_ResourceManager != null)
		{
			this.m_ResourceManager.ReleaseResourceEntry(this);
		}
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x0003957A File Offset: 0x0003777A
	public UnityEngine.Object GetResourceObject()
	{
		return this.m_ResourceObject;
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00039582 File Offset: 0x00037782
	public T GetResource<T>() where T : UnityEngine.Object
	{
		return this.m_ResourceObject as !!0;
	}

	// Token: 0x04000911 RID: 2321
	public const float k_DelayLoadCompletion = 3f;

	// Token: 0x04000912 RID: 2322
	private ResourceManager m_ResourceManager;

	// Token: 0x04000913 RID: 2323
	private string m_ResourceName;

	// Token: 0x04000914 RID: 2324
	private bool m_bLoadCompleted;

	// Token: 0x04000915 RID: 2325
	private ResourceRequest m_ResourceRequest;

	// Token: 0x04000916 RID: 2326
	private UnityEngine.Object m_ResourceObject;

	// Token: 0x04000917 RID: 2327
	private int m_ReferenceCount;

	// Token: 0x04000918 RID: 2328
	private ResourceEntry.ResourceLoadCallback m_OnLoadCompletedCallback;

	// Token: 0x020007A4 RID: 1956
	// (Invoke) Token: 0x060042AF RID: 17071
	public delegate void ResourceLoadCallback(ResourceEntry resourceEntry);
}
