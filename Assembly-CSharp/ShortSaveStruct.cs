using System;
using System.Runtime.InteropServices;

// Token: 0x02000084 RID: 132
[Serializable]
public struct ShortSaveStruct
{
	// Token: 0x0400061F RID: 1567
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
	public string playdekHeader;

	// Token: 0x04000620 RID: 1568
	public uint saveFileVersionNumber;

	// Token: 0x04000621 RID: 1569
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string player1Name;

	// Token: 0x04000622 RID: 1570
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string player2Name;

	// Token: 0x04000623 RID: 1571
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string player3Name;

	// Token: 0x04000624 RID: 1572
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string player4Name;

	// Token: 0x04000625 RID: 1573
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string player5Name;

	// Token: 0x04000626 RID: 1574
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string player6Name;

	// Token: 0x04000627 RID: 1575
	public int player1Faction;

	// Token: 0x04000628 RID: 1576
	public int player2Faction;

	// Token: 0x04000629 RID: 1577
	public int player3Faction;

	// Token: 0x0400062A RID: 1578
	public int player4Faction;

	// Token: 0x0400062B RID: 1579
	public int player5Faction;

	// Token: 0x0400062C RID: 1580
	public int player6Faction;

	// Token: 0x0400062D RID: 1581
	public int gameID;

	// Token: 0x0400062E RID: 1582
	public int rematchGameID;

	// Token: 0x0400062F RID: 1583
	public ushort gameType;

	// Token: 0x04000630 RID: 1584
	public ushort deckFlags;

	// Token: 0x04000631 RID: 1585
	public int soloGameCurrentScore;

	// Token: 0x04000632 RID: 1586
	public ushort soloGameStartFood;

	// Token: 0x04000633 RID: 1587
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
	public ushort[] soloGameStartOccupations;

	// Token: 0x04000634 RID: 1588
	public ushort soloGameCount;

	// Token: 0x04000635 RID: 1589
	public uint soloGameRandomSeed;

	// Token: 0x04000636 RID: 1590
	public int roundNumber;

	// Token: 0x04000637 RID: 1591
	public int decisionPlayerFlags;

	// Token: 0x04000638 RID: 1592
	public int currentTurnPlayerIndex;

	// Token: 0x04000639 RID: 1593
	public int worldDataVersion;

	// Token: 0x0400063A RID: 1594
	public int savedDataSize;

	// Token: 0x0400063B RID: 1595
	public uint gameState;

	// Token: 0x0400063C RID: 1596
	public ushort player1State;

	// Token: 0x0400063D RID: 1597
	public ushort player2State;

	// Token: 0x0400063E RID: 1598
	public ushort player3State;

	// Token: 0x0400063F RID: 1599
	public ushort player4State;

	// Token: 0x04000640 RID: 1600
	public ushort player5State;

	// Token: 0x04000641 RID: 1601
	public ushort player6State;

	// Token: 0x04000642 RID: 1602
	public ushort player1Rating;

	// Token: 0x04000643 RID: 1603
	public ushort player2Rating;

	// Token: 0x04000644 RID: 1604
	public ushort player3Rating;

	// Token: 0x04000645 RID: 1605
	public ushort player4Rating;

	// Token: 0x04000646 RID: 1606
	public ushort player5Rating;

	// Token: 0x04000647 RID: 1607
	public ushort player6Rating;

	// Token: 0x04000648 RID: 1608
	public uint player1Timer;

	// Token: 0x04000649 RID: 1609
	public uint player2Timer;

	// Token: 0x0400064A RID: 1610
	public uint player3Timer;

	// Token: 0x0400064B RID: 1611
	public uint player4Timer;

	// Token: 0x0400064C RID: 1612
	public uint player5Timer;

	// Token: 0x0400064D RID: 1613
	public uint player6Timer;

	// Token: 0x0400064E RID: 1614
	public uint updateTime;

	// Token: 0x0400064F RID: 1615
	public int player1ID;

	// Token: 0x04000650 RID: 1616
	public int player2ID;

	// Token: 0x04000651 RID: 1617
	public int player3ID;

	// Token: 0x04000652 RID: 1618
	public int player4ID;

	// Token: 0x04000653 RID: 1619
	public int player5ID;

	// Token: 0x04000654 RID: 1620
	public int player6ID;

	// Token: 0x04000655 RID: 1621
	public int player1Avatar;

	// Token: 0x04000656 RID: 1622
	public int player2Avatar;

	// Token: 0x04000657 RID: 1623
	public int player3Avatar;

	// Token: 0x04000658 RID: 1624
	public int player4Avatar;

	// Token: 0x04000659 RID: 1625
	public int player5Avatar;

	// Token: 0x0400065A RID: 1626
	public int player6Avatar;

	// Token: 0x0400065B RID: 1627
	public uint packedPlayerCount;

	// Token: 0x0400065C RID: 1628
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
	public string playdekFooter;
}
