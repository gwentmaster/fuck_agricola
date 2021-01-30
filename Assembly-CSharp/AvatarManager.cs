using System;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class AvatarManager : MonoBehaviour
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x0600086B RID: 2155 RVA: 0x0003A694 File Offset: 0x00038894
	public static AvatarManager instance
	{
		get
		{
			if (AvatarManager._instance == null)
			{
				AvatarManager._instance = UnityEngine.Object.FindObjectOfType<AvatarManager>();
				if (AvatarManager._instance == null)
				{
					AvatarManager._instance = new GameObject
					{
						name = "ScreenManager"
					}.AddComponent<AvatarManager>();
				}
			}
			return AvatarManager._instance;
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0003A6E4 File Offset: 0x000388E4
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		GameObject gameObject = GameObject.Find("/ResourceManager - Startup");
		if (gameObject != null)
		{
			this.m_resourceManager = gameObject.GetComponent<ResourceManager>();
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0003A71C File Offset: 0x0003891C
	private void Start()
	{
		this.m_count = this.m_avatars.GetLength(0);
		for (int i = 0; i < this.m_count; i++)
		{
			this.m_avatars[i].referenceCount = 0U;
			this.m_avatars[i].avatar = null;
			this.m_avatars[i].referenceCountLarge = 0U;
			this.m_avatars[i].avatarLarge = null;
			this.m_avatars[i].avatarResource = null;
			this.m_avatars[i].avatarResourceLarge = null;
			this.m_avatars[i].callbacks = null;
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00003022 File Offset: 0x00001222
	private void Update()
	{
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0003A7D0 File Offset: 0x000389D0
	public int Count()
	{
		return this.m_count;
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0003A7D8 File Offset: 0x000389D8
	public int RangeCheck(int index)
	{
		if (index < 0 || index >= this.m_count)
		{
			index = 1;
		}
		return index;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x0003A7EB File Offset: 0x000389EB
	public bool IsAvatarAvailable(uint index, AvatarManager.EAvatarRestriction restriction)
	{
		return (ulong)index < (ulong)((long)this.m_count) && this.m_avatars[(int)index].restriction != AvatarManager.EAvatarRestriction.NeverAvailable && this.m_avatars[(int)index].restriction == restriction;
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0003A7D0 File Offset: 0x000389D0
	public int GetAvatarCount()
	{
		return this.m_count;
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0003A824 File Offset: 0x00038A24
	public string GetAvatarString(uint index)
	{
		if ((ulong)index >= (ulong)((long)this.m_count))
		{
			return "";
		}
		return this.m_avatars[(int)index].filename;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0003A848 File Offset: 0x00038A48
	public Sprite GetAvatar(uint index, Avatar_UI avatarObj)
	{
		if ((ulong)index >= (ulong)((long)this.m_count))
		{
			return null;
		}
		if (this.m_resourceManager != null)
		{
			if (this.m_avatars[(int)index].avatarResource == null)
			{
				string filename = this.m_smallAvatarPath + this.m_avatars[(int)index].filename;
				this.m_avatars[(int)index].avatarResource = this.m_resourceManager.LoadResource<Sprite>(filename);
				this.m_avatars[(int)index].avatarResource.IncrementCount();
				if (this.m_avatars[(int)index].avatarResource.HasLoadCompleted())
				{
					this.m_avatars[(int)index].avatar = this.m_avatars[(int)index].avatarResource.GetResource<Sprite>();
				}
				else
				{
					this.m_avatars[(int)index].avatarResource.AddOnLoadCompletedCallback(new ResourceEntry.ResourceLoadCallback(this.HandleResourceLoadCallback));
					AvatarManager.AvatarListing[] avatars = this.m_avatars;
					avatars[(int)index].callbacks = (AvatarManager.AvatarLoadCallback)Delegate.Combine(avatars[(int)index].callbacks, new AvatarManager.AvatarLoadCallback(avatarObj.SetAvatarSpriteCallback));
				}
			}
		}
		else if (this.m_avatars[(int)index].referenceCount == 0U)
		{
			string path = this.m_smallAvatarPath + this.m_avatars[(int)index].filename;
			this.m_avatars[(int)index].avatar = Resources.Load<Sprite>(path);
		}
		AvatarManager.AvatarListing[] avatars2 = this.m_avatars;
		avatars2[(int)index].referenceCount = avatars2[(int)index].referenceCount + 1U;
		return this.m_avatars[(int)index].avatar;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0003A9DC File Offset: 0x00038BDC
	public void ReleaseAvatar(uint index)
	{
		if ((ulong)index >= (ulong)((long)this.m_count))
		{
			return;
		}
		AvatarManager.AvatarListing[] avatars = this.m_avatars;
		avatars[(int)index].referenceCount = avatars[(int)index].referenceCount - 1U;
		if (this.m_avatars[(int)index].referenceCount == 0U)
		{
			if (this.m_resourceManager != null)
			{
				if (this.m_avatars[(int)index].avatarResource != null)
				{
					this.m_avatars[(int)index].avatarResource.ReleaseResourceEntry();
					this.m_avatars[(int)index].avatarResource = null;
				}
			}
			else
			{
				Resources.UnloadAsset(this.m_avatars[(int)index].avatar);
			}
			this.m_avatars[(int)index].avatar = null;
		}
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0003AA94 File Offset: 0x00038C94
	private void HandleResourceLoadCallback(ResourceEntry resource)
	{
		if (resource != null)
		{
			for (int i = 0; i < this.m_avatars.Length; i++)
			{
				if (this.m_avatars[i].avatarResource == resource)
				{
					this.m_avatars[i].avatar = this.m_avatars[i].avatarResource.GetResource<Sprite>();
					this.m_avatars[i].callbacks(this.m_avatars[i].avatar);
					this.m_avatars[i].callbacks = null;
					return;
				}
				if (this.m_avatars[i].avatarResourceLarge == resource)
				{
					this.m_avatars[i].avatarLarge = this.m_avatars[i].avatarResourceLarge.GetResource<Sprite>();
					this.m_avatars[i].callbacks(this.m_avatars[i].avatarLarge);
					this.m_avatars[i].callbacks = null;
					return;
				}
			}
		}
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0003ABAC File Offset: 0x00038DAC
	public void OutputManagerState()
	{
		Debug.Log("Avatar Manager stats START!");
		for (int i = 0; i < this.m_avatars.Length; i++)
		{
			if (this.m_avatars[i].referenceCount != 0U)
			{
				Debug.Log("AvatarManager has " + this.m_avatars[i].referenceCount.ToString() + " reference(s) to avatar index: " + i.ToString());
			}
			if (this.m_avatars[i].referenceCountLarge != 0U)
			{
				Debug.Log("AvatarManager has " + this.m_avatars[i].referenceCountLarge.ToString() + " reference(s) to large avatar index: " + i.ToString());
			}
		}
		Debug.Log("Avatar Manager stats END!");
	}

	// Token: 0x04000935 RID: 2357
	public string m_smallAvatarPath = "Avatars/";

	// Token: 0x04000936 RID: 2358
	public AvatarManager.AvatarListing[] m_avatars;

	// Token: 0x04000937 RID: 2359
	private int m_count;

	// Token: 0x04000938 RID: 2360
	private ResourceManager m_resourceManager;

	// Token: 0x04000939 RID: 2361
	private static AvatarManager _instance;

	// Token: 0x020007A6 RID: 1958
	public enum EAvatarRestriction
	{
		// Token: 0x04002C77 RID: 11383
		Red,
		// Token: 0x04002C78 RID: 11384
		Purple,
		// Token: 0x04002C79 RID: 11385
		Green,
		// Token: 0x04002C7A RID: 11386
		Blue,
		// Token: 0x04002C7B RID: 11387
		Tan,
		// Token: 0x04002C7C RID: 11388
		Player6,
		// Token: 0x04002C7D RID: 11389
		AI,
		// Token: 0x04002C7E RID: 11390
		NeverAvailable
	}

	// Token: 0x020007A7 RID: 1959
	// (Invoke) Token: 0x060042B9 RID: 17081
	public delegate void AvatarLoadCallback(Sprite avatarSprite);

	// Token: 0x020007A8 RID: 1960
	[Serializable]
	public struct AvatarListing
	{
		// Token: 0x04002C7F RID: 11391
		public string filename;

		// Token: 0x04002C80 RID: 11392
		public AvatarManager.EAvatarRestriction restriction;

		// Token: 0x04002C81 RID: 11393
		[HideInInspector]
		public uint referenceCount;

		// Token: 0x04002C82 RID: 11394
		[HideInInspector]
		public Sprite avatar;

		// Token: 0x04002C83 RID: 11395
		[HideInInspector]
		public uint referenceCountLarge;

		// Token: 0x04002C84 RID: 11396
		[HideInInspector]
		public Sprite avatarLarge;

		// Token: 0x04002C85 RID: 11397
		[HideInInspector]
		public ResourceEntry avatarResource;

		// Token: 0x04002C86 RID: 11398
		[HideInInspector]
		public ResourceEntry avatarResourceLarge;

		// Token: 0x04002C87 RID: 11399
		[HideInInspector]
		public AvatarManager.AvatarLoadCallback callbacks;
	}
}
