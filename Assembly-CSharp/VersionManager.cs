using System;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class VersionManager : MonoBehaviour
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600084E RID: 2126 RVA: 0x00039F54 File Offset: 0x00038154
	public static VersionManager instance
	{
		get
		{
			if (VersionManager._instance == null)
			{
				VersionManager._instance = UnityEngine.Object.FindObjectOfType<VersionManager>();
				if (VersionManager._instance == null)
				{
					VersionManager._instance = new GameObject
					{
						name = "VersionManager"
					}.AddComponent<VersionManager>();
				}
				UnityEngine.Object.DontDestroyOnLoad(VersionManager._instance.gameObject);
			}
			return VersionManager._instance;
		}
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00039FB3 File Offset: 0x000381B3
	public string GetVersionTextString()
	{
		return this.m_VersionTextString;
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00039FBB File Offset: 0x000381BB
	public bool UsePlaytestVersion()
	{
		return this.m_UsePlaytestVersion;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00039FC3 File Offset: 0x000381C3
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.m_VersionTextString = this.m_VersionNumber;
	}

	// Token: 0x04000922 RID: 2338
	[SerializeField]
	private string m_VersionNumber;

	// Token: 0x04000923 RID: 2339
	[SerializeField]
	public bool m_UsePlaytestVersion;

	// Token: 0x04000924 RID: 2340
	private string m_VersionTextString;

	// Token: 0x04000925 RID: 2341
	private static VersionManager _instance;
}
