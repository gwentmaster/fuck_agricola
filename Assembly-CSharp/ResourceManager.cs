using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class ResourceManager : MonoBehaviour
{
	// Token: 0x06000841 RID: 2113 RVA: 0x00039594 File Offset: 0x00037794
	private void Awake()
	{
		this.m_DictionaryResources = new Dictionary<string, ResourceEntry>();
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x000395A4 File Offset: 0x000377A4
	public ResourceEntry LoadResource<T>(string filename) where T : UnityEngine.Object
	{
		ResourceEntry resourceEntry = null;
		if (this.m_DictionaryResources.TryGetValue(filename, out resourceEntry))
		{
			if (!resourceEntry.HasLoadCompleted())
			{
				return resourceEntry;
			}
			if (resourceEntry.GetResource<T>() != null)
			{
				return resourceEntry;
			}
		}
		T t = Resources.Load<T>(filename);
		if (t == null)
		{
			Debug.LogWarning("Unable to load resource at path: " + filename);
			return null;
		}
		Debug.Log(" ResourceManager.LoadResource: " + filename);
		ResourceEntry resourceEntry2 = new ResourceEntry(this, filename, t);
		this.m_DictionaryResources.Add(filename, resourceEntry2);
		return resourceEntry2;
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00039638 File Offset: 0x00037838
	public ResourceEntry LoadResourceAsync<T>(string filename) where T : UnityEngine.Object
	{
		ResourceEntry resourceEntry = null;
		if (this.m_DictionaryResources.TryGetValue(filename, out resourceEntry))
		{
			if (!resourceEntry.HasLoadCompleted())
			{
				return resourceEntry;
			}
			if (resourceEntry.GetResource<T>() != null)
			{
				return resourceEntry;
			}
		}
		ResourceRequest resourceRequest = Resources.LoadAsync<T>(filename);
		if (resourceRequest == null)
		{
			Debug.LogWarning("Unable to load resource at path: " + filename);
			return null;
		}
		Debug.Log(" ResourceManager.LoadResourceAsync: " + filename);
		ResourceEntry resourceEntry2 = new ResourceEntry(this, filename, resourceRequest);
		this.m_DictionaryResources.Add(filename, resourceEntry2);
		return resourceEntry2;
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x000396BC File Offset: 0x000378BC
	public void ReleaseResourceEntry(ResourceEntry resource_entry)
	{
		if (resource_entry == null)
		{
			return;
		}
		string resourceName = resource_entry.GetResourceName();
		ResourceEntry resourceEntry = null;
		if (!this.m_DictionaryResources.TryGetValue(resourceName, out resourceEntry))
		{
			return;
		}
		if (resourceEntry != resource_entry)
		{
			return;
		}
		resourceEntry.DecrementCount();
		if (resourceEntry.GetCount() <= 0)
		{
			Debug.Log(" ResourceManager.ReleaseResource: " + resourceName);
			this.m_DictionaryResources.Remove(resourceName);
			Resources.UnloadAsset(resourceEntry.GetResourceObject());
		}
	}

	// Token: 0x04000919 RID: 2329
	private Dictionary<string, ResourceEntry> m_DictionaryResources;
}
