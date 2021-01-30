using System;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x020000E2 RID: 226
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000854 RID: 2132 RVA: 0x00039FDC File Offset: 0x000381DC
	private static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x0003A000 File Offset: 0x00038200
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0003A00C File Offset: 0x0003820C
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0003A014 File Offset: 0x00038214
	private void Awake()
	{
		if (this.m_EnableSteamDRM && File.Exists("steam_appid.txt"))
		{
			Debug.LogError("[Steamworks.NET] Deleting steam_appid.txt!");
			File.Delete("steam_appid.txt");
		}
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		if (this.m_SteamAppId == 0U)
		{
			Debug.LogError("[Steamworks.NET] Steam AppId is not set.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)this.m_SteamAppId))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0003A11C File Offset: 0x0003831C
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0003A16A File Offset: 0x0003836A
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0003A18E File Offset: 0x0003838E
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private uint m_SteamAppId;

	// Token: 0x04000927 RID: 2343
	[SerializeField]
	private bool m_EnableSteamDRM;

	// Token: 0x04000928 RID: 2344
	private static SteamManager s_instance;

	// Token: 0x04000929 RID: 2345
	private static bool s_EverInitialized;

	// Token: 0x0400092A RID: 2346
	private bool m_bInitialized;

	// Token: 0x0400092B RID: 2347
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
