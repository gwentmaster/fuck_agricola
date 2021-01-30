using System;
using System.Runtime.InteropServices;

// Token: 0x0200008B RID: 139
public struct GamePlayerFarmState
{
	// Token: 0x04000691 RID: 1681
	public int playerIndex;

	// Token: 0x04000692 RID: 1682
	public int playerInstanceID;

	// Token: 0x04000693 RID: 1683
	public int playerFaction;

	// Token: 0x04000694 RID: 1684
	public int roundNumber;

	// Token: 0x04000695 RID: 1685
	public int currentScore;

	// Token: 0x04000696 RID: 1686
	public int numBeggingCards;

	// Token: 0x04000697 RID: 1687
	public byte isNextStartPlayer;

	// Token: 0x04000698 RID: 1688
	public byte harvestPhase;

	// Token: 0x04000699 RID: 1689
	public int workerCount;

	// Token: 0x0400069A RID: 1690
	public int unusedWorkerFlags;

	// Token: 0x0400069B RID: 1691
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	public int[] workerAvatarIDs;

	// Token: 0x0400069C RID: 1692
	public int houseType;

	// Token: 0x0400069D RID: 1693
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
	public int[] tileTypes;
}
