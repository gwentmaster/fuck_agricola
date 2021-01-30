using System;
using System.Runtime.InteropServices;

// Token: 0x020000A7 RID: 167
[Serializable]
public struct AppPlayerData
{
	// Token: 0x0400074E RID: 1870
	public int id;

	// Token: 0x0400074F RID: 1871
	public ushort userAvatar;

	// Token: 0x04000750 RID: 1872
	public ushort userRating;

	// Token: 0x04000751 RID: 1873
	public sbyte playerType;

	// Token: 0x04000752 RID: 1874
	public sbyte aiDifficultyLevel;

	// Token: 0x04000753 RID: 1875
	public PlayerParameters playerParameters;

	// Token: 0x04000754 RID: 1876
	public ushort networkPlayerState;

	// Token: 0x04000755 RID: 1877
	public uint networkPlayerTimer;

	// Token: 0x04000756 RID: 1878
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string name;

	// Token: 0x02000789 RID: 1929
	public enum PlayerType
	{
		// Token: 0x04002C11 RID: 11281
		PlayerType_Local,
		// Token: 0x04002C12 RID: 11282
		PlayerType_Human,
		// Token: 0x04002C13 RID: 11283
		PlayerType_AI
	}

	// Token: 0x0200078A RID: 1930
	public enum AIDifficultyLevel
	{
		// Token: 0x04002C15 RID: 11285
		AIDifficultyLevel_Easy,
		// Token: 0x04002C16 RID: 11286
		AIDifficultyLevel_Medium,
		// Token: 0x04002C17 RID: 11287
		AIDifficultyLevel_Hard,
		// Token: 0x04002C18 RID: 11288
		AIDifficultyLevel_Tutorial
	}
}
