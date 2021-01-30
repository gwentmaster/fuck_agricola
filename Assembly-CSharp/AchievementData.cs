using System;
using UnityEngine;

// Token: 0x020000C3 RID: 195
[Serializable]
public struct AchievementData
{
	// Token: 0x04000850 RID: 2128
	public EAchievements id;

	// Token: 0x04000851 RID: 2129
	[Tooltip("The value needed to unlock this achievement.\nNon-accumulating achievements should use 1")]
	public int achievedAtValue;

	// Token: 0x04000852 RID: 2130
	[Tooltip("Used for tracking specific instances, as opposed to numerical increases.\nAchieved at value will be read as # of bits needed")]
	public bool isBitField;

	// Token: 0x04000853 RID: 2131
	[Tooltip("Steam stat used for accumulating achievement. Ignored on other platforms")]
	public string steamStatName;

	// Token: 0x04000854 RID: 2132
	[Tooltip("ID string used for achievement on Steam. Ignored on other platforms")]
	public string idNameSteam;

	// Token: 0x04000855 RID: 2133
	[Tooltip("ID string used for achievement on iOS.")]
	public string idNameIOS;

	// Token: 0x04000856 RID: 2134
	[Tooltip("ID string used for achievement on Android.")]
	public string idNameAndroid;

	// Token: 0x04000857 RID: 2135
	public Sprite lockedSprite;

	// Token: 0x04000858 RID: 2136
	public Sprite unlockedSprite;

	// Token: 0x04000859 RID: 2137
	public string displayName;

	// Token: 0x0400085A RID: 2138
	[TextArea(3, 10)]
	public string displayDescription;

	// Token: 0x0400085B RID: 2139
	public int pointValue;

	// Token: 0x0400085C RID: 2140
	public bool isHiddenAchievement;

	// Token: 0x0400085D RID: 2141
	public bool dontShowAchievement;

	// Token: 0x0400085E RID: 2142
	[HideInInspector]
	public long currentValue;

	// Token: 0x0400085F RID: 2143
	[HideInInspector]
	public bool bHasAchieved;
}
