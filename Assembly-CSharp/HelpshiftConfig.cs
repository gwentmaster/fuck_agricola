using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[Serializable]
public class HelpshiftConfig : ScriptableObject
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600013D RID: 317 RVA: 0x00007078 File Offset: 0x00005278
	public static HelpshiftConfig Instance
	{
		get
		{
			HelpshiftConfig.instance = (Resources.Load("HelpshiftConfig") as HelpshiftConfig);
			if (HelpshiftConfig.instance == null)
			{
				HelpshiftConfig.instance = ScriptableObject.CreateInstance<HelpshiftConfig>();
			}
			return HelpshiftConfig.instance;
		}
	}

	// Token: 0x04000094 RID: 148
	private static HelpshiftConfig instance;

	// Token: 0x04000095 RID: 149
	private const string helpshiftConfigAssetName = "HelpshiftConfig";

	// Token: 0x04000096 RID: 150
	private const string helpshiftConfigPath = "Helpshift/Resources";

	// Token: 0x04000097 RID: 151
	public const string pluginVersion = "5.3.2";
}
