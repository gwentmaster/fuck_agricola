using System;
using System.Runtime.InteropServices;

// Token: 0x0200008A RID: 138
public struct GamePlayerState
{
	// Token: 0x04000674 RID: 1652
	public int playerIndex;

	// Token: 0x04000675 RID: 1653
	public int playerInstanceID;

	// Token: 0x04000676 RID: 1654
	public int playerAvatar;

	// Token: 0x04000677 RID: 1655
	public int playerFaction;

	// Token: 0x04000678 RID: 1656
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string displayName;

	// Token: 0x04000679 RID: 1657
	public int resourceCountFood;

	// Token: 0x0400067A RID: 1658
	public int resourceCountWood;

	// Token: 0x0400067B RID: 1659
	public int resourceCountClay;

	// Token: 0x0400067C RID: 1660
	public int resourceCountStone;

	// Token: 0x0400067D RID: 1661
	public int resourceCountReed;

	// Token: 0x0400067E RID: 1662
	public int resourceCountGrain;

	// Token: 0x0400067F RID: 1663
	public int resourceCountVeggie;

	// Token: 0x04000680 RID: 1664
	public int resourceCountSheep;

	// Token: 0x04000681 RID: 1665
	public int resourceCountWildBoar;

	// Token: 0x04000682 RID: 1666
	public int resourceCountCattle;

	// Token: 0x04000683 RID: 1667
	public int foodRequirement;

	// Token: 0x04000684 RID: 1668
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public int[] foodRequirementByWorker;

	// Token: 0x04000685 RID: 1669
	public int stablesCount;

	// Token: 0x04000686 RID: 1670
	public int fencesCount;

	// Token: 0x04000687 RID: 1671
	public int maxFences;

	// Token: 0x04000688 RID: 1672
	public int workerCount;

	// Token: 0x04000689 RID: 1673
	public int hasAmbassador;

	// Token: 0x0400068A RID: 1674
	public int cardCountIntrigue;

	// Token: 0x0400068B RID: 1675
	public int cardCountActiveQuests;

	// Token: 0x0400068C RID: 1676
	public int cardCountCompletedQuests;

	// Token: 0x0400068D RID: 1677
	public int ownedBuildingCount;

	// Token: 0x0400068E RID: 1678
	public int firstPlayer;

	// Token: 0x0400068F RID: 1679
	public int activePlayer;

	// Token: 0x04000690 RID: 1680
	public int playerRating;
}
