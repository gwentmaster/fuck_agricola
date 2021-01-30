using System;
using System.Runtime.InteropServices;
using GameData;

// Token: 0x02000083 RID: 131
public class AgricolaLib
{
	// Token: 0x060005FE RID: 1534
	[DllImport("AgricolaLib")]
	public static extern void SetGameOptionsListener(AgricolaLib.GameOptionsListenerDelegate pGameOptionsListenerFunc);

	// Token: 0x060005FF RID: 1535
	[DllImport("AgricolaLib")]
	public static extern void SelectGameOption(int optionIndex);

	// Token: 0x06000600 RID: 1536
	[DllImport("AgricolaLib")]
	public static extern void SelectGameOptionWithData(int optionIndex, uint selectionData);

	// Token: 0x06000601 RID: 1537
	[DllImport("AgricolaLib")]
	public static extern void ResendGameOptionsList();

	// Token: 0x06000602 RID: 1538
	[DllImport("AgricolaLib")]
	public static extern IntPtr GetGameParameters();

	// Token: 0x06000603 RID: 1539
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerInfo(int playerID, IntPtr pData, int maxDataSize);

	// Token: 0x06000604 RID: 1540
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerTimer(int playerID, IntPtr pData, int maxDataSize);

	// Token: 0x06000605 RID: 1541
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerState(int playerIndex, IntPtr pData, int maxDataSize);

	// Token: 0x06000606 RID: 1542
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerAnimalContainers(int playerIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x06000607 RID: 1543
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerFarmState(int playerIndex, IntPtr pData, int maxDataSize);

	// Token: 0x06000608 RID: 1544
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerFarmTileState(int playerIndex, int tileIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x06000609 RID: 1545
	[DllImport("AgricolaLib")]
	public static extern int GetCardInstanceIDFromSubID(int instanceID, bool checkWorkAssignment, bool ignoreWorldCards);

	// Token: 0x0600060A RID: 1546
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerAdditionalResourceQuery(int playerIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x0600060B RID: 1547
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerBuildImprovementCostQuery(int playerIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x0600060C RID: 1548
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerRenovateCostQuery(int playerIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x0600060D RID: 1549
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerBuildRoomCostQuery(int playerIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x0600060E RID: 1550
	[DllImport("AgricolaLib")]
	public static extern bool GetDoesPlayerHaveCalendarResources(int playerIndex);

	// Token: 0x0600060F RID: 1551
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerCalendar(int playerIndex, int roundNumber, IntPtr pBuffer, int bufLen);

	// Token: 0x06000610 RID: 1552
	[DllImport("AgricolaLib")]
	public static extern int GetConvertDefinition(uint convertInstanceID, IntPtr pBuffer, int bufLen);

	// Token: 0x06000611 RID: 1553
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerPayResourcesState(int playerIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x06000612 RID: 1554
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerMaxBuildableFences(int playerIndex);

	// Token: 0x06000613 RID: 1555
	[DllImport("AgricolaLib")]
	public static extern int GetActionDefinition(uint actionInstanceID, IntPtr pBuffer, int bufLen);

	// Token: 0x06000614 RID: 1556
	[DllImport("AgricolaLib")]
	public static extern int GetGameCardUniqueData(uint cardInPlayInstanceID, IntPtr pBuffer, int bufLen);

	// Token: 0x06000615 RID: 1557
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerScoreState(int playerIndex, IntPtr pData, int maxDataSize);

	// Token: 0x06000616 RID: 1558
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerEndgameAchievementState(int playerIndex, IntPtr pBuffer, int bufLen);

	// Token: 0x06000617 RID: 1559
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerHandState(int playerID, IntPtr pData, int maxDataSize);

	// Token: 0x06000618 RID: 1560
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerAIState(int playerID, IntPtr pData, int maxDataSize);

	// Token: 0x06000619 RID: 1561
	[DllImport("AgricolaLib")]
	public static extern int GetOutputMessageLogAtIndex(uint index, IntPtr pBuffer, int bufLen);

	// Token: 0x0600061A RID: 1562
	[DllImport("AgricolaLib")]
	public static extern uint GetOutputMessageLogCount();

	// Token: 0x0600061B RID: 1563
	[DllImport("AgricolaLib")]
	public static extern int GetGameTurnLogCount();

	// Token: 0x0600061C RID: 1564
	[DllImport("AgricolaLib")]
	public static extern int GetGameTurnLogBuffer(int logIndex, IntPtr pData, int maxDataSize);

	// Token: 0x0600061D RID: 1565
	[DllImport("AgricolaLib")]
	public static extern int GetGameTurnPlayedCards(int turnNumber, IntPtr pData, int maxDataSize);

	// Token: 0x0600061E RID: 1566
	[DllImport("AgricolaLib")]
	public unsafe static extern int GetGameDeckCounts(GameDeckCounts* pData, int maxDataSize);

	// Token: 0x0600061F RID: 1567
	[DllImport("AgricolaLib")]
	public static extern int GetCurrentRound();

	// Token: 0x06000620 RID: 1568
	[DllImport("AgricolaLib")]
	public static extern int GetCurrentHarvestMode();

	// Token: 0x06000621 RID: 1569
	[DllImport("AgricolaLib")]
	public static extern int GetMajorImprovementOwnerIndex(ushort majorImprovementInstanceID);

	// Token: 0x06000622 RID: 1570
	[DllImport("AgricolaLib")]
	public static extern int GetAnimalContainerCapicity(int[] sheepArrangement, int[] boarArrangement, int[] cattleArrangement, int containerId, int animalType);

	// Token: 0x06000623 RID: 1571
	[DllImport("AgricolaLib")]
	public static extern bool IsPlowableTile(int tileIndex);

	// Token: 0x06000624 RID: 1572
	[DllImport("AgricolaLib")]
	public static extern bool IsBuildableTile(int tileIndex);

	// Token: 0x06000625 RID: 1573
	[DllImport("AgricolaLib")]
	public static extern bool IsFenceableTile(int tileIndex);

	// Token: 0x06000626 RID: 1574
	[DllImport("AgricolaLib")]
	public static extern bool CanPlaceStableInTile(int tileIndex);

	// Token: 0x06000627 RID: 1575
	[DllImport("AgricolaLib")]
	public static extern bool IsPlowableOrConvertableResource(int resource);

	// Token: 0x06000628 RID: 1576
	[DllImport("AgricolaLib")]
	public static extern int GetTileSowLocationIndex(int tileIndex);

	// Token: 0x06000629 RID: 1577
	[DllImport("AgricolaLib")]
	public static extern int GetSowLocationOptionIndex(int sowLocationIndex, int resourceType);

	// Token: 0x0600062A RID: 1578
	[DllImport("AgricolaLib")]
	public static extern int GetAssignAgentInstanceID();

	// Token: 0x0600062B RID: 1579
	[DllImport("AgricolaLib")]
	public static extern bool GetCardEventResolveState(int cardCount, IntPtr pCardData);

	// Token: 0x0600062C RID: 1580
	[DllImport("AgricolaLib")]
	public static extern int GetInstanceID(int instanceType, string instanceName);

	// Token: 0x0600062D RID: 1581
	[DllImport("AgricolaLib")]
	public static extern int GetInstanceList(int instanceType, int playerIndex, IntPtr pInstanceIDs, int maxInstanceCount);

	// Token: 0x0600062E RID: 1582
	[DllImport("AgricolaLib")]
	public static extern int GetInstanceData(int instanceType, int instanceID, IntPtr pData, int maxDataSize);

	// Token: 0x0600062F RID: 1583
	[DllImport("AgricolaLib")]
	public static extern int GetCardDataByName(string pCardName, IntPtr pData, int maxDataSize);

	// Token: 0x06000630 RID: 1584
	[DllImport("AgricolaLib")]
	public static extern int GetCardDataByCompressedNumber(uint compressedNumber, IntPtr pBuffer, int bufLen);

	// Token: 0x06000631 RID: 1585
	[DllImport("AgricolaLib")]
	public static extern void Initialize(string data_path, int processorCount);

	// Token: 0x06000632 RID: 1586
	[DllImport("AgricolaLib")]
	public static extern void Shutdown();

	// Token: 0x06000633 RID: 1587
	[DllImport("AgricolaLib")]
	public static extern void StartGame(ref GameParameters pGameParameters, int numPlayers, AppPlayerData[] pAppPlayerData, uint randomSeed);

	// Token: 0x06000634 RID: 1588
	[DllImport("AgricolaLib")]
	public static extern void StartTutorial(ref GameParameters pGameParameters, int numPlayers, AppPlayerData[] pAppPlayerData, int tutorialIndex, uint randomSeed, int tutorialSetupStepCount, TutorialAIStep[] tutorialSetupSteps, int tutorialAIStepCount, TutorialAIStep[] tutorialAISteps, int drawDeckOrderCount, ushort[] drawDeckOrder, int randomDieResultCount, byte[] randomDieResults);

	// Token: 0x06000635 RID: 1589
	[DllImport("AgricolaLib")]
	public static extern void ResumeGame(IntPtr pGameWorldData, int size, int WorldDataVersion);

	// Token: 0x06000636 RID: 1590
	[DllImport("AgricolaLib")]
	public static extern void ExitCurrentGame();

	// Token: 0x06000637 RID: 1591
	[DllImport("AgricolaLib")]
	public static extern int UpdateGame(IntPtr pGameEvents, int maxEvents);

	// Token: 0x06000638 RID: 1592
	[DllImport("AgricolaLib")]
	public static extern int ForceUpdateStateMachineInput(IntPtr pGameEvents, int maxEvents);

	// Token: 0x06000639 RID: 1593
	[DllImport("AgricolaLib")]
	public static extern bool HasTemporaryMoveBuffer();

	// Token: 0x0600063A RID: 1594
	[DllImport("AgricolaLib")]
	public static extern void CommitTemporaryMoveBuffer();

	// Token: 0x0600063B RID: 1595
	[DllImport("AgricolaLib")]
	public static extern void RevertTemporaryMoveBuffer();

	// Token: 0x0600063C RID: 1596
	[DllImport("AgricolaLib")]
	public static extern uint GetCurrentGameID();

	// Token: 0x0600063D RID: 1597
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerCount();

	// Token: 0x0600063E RID: 1598
	[DllImport("AgricolaLib")]
	public static extern int GetGamePlayerAICount();

	// Token: 0x0600063F RID: 1599
	[DllImport("AgricolaLib")]
	public static extern int GetGameResultsPosition(int playerIndex);

	// Token: 0x06000640 RID: 1600
	[DllImport("AgricolaLib")]
	public static extern uint GetGameRandomSeed();

	// Token: 0x06000641 RID: 1601
	[DllImport("AgricolaLib")]
	public static extern int GetLocalPlayerIndex();

	// Token: 0x06000642 RID: 1602
	[DllImport("AgricolaLib")]
	public static extern int GetLocalPlayerInstanceID();

	// Token: 0x06000643 RID: 1603
	[DllImport("AgricolaLib")]
	public static extern int GetLocalOpponentPlayerIndex(int opponent_index);

	// Token: 0x06000644 RID: 1604
	[DllImport("AgricolaLib")]
	public static extern int GetLocalOpponentInstanceID(int opponent_index);

	// Token: 0x06000645 RID: 1605
	[DllImport("AgricolaLib")]
	public static extern int GetStartingPlayerInstanceID();

	// Token: 0x06000646 RID: 1606
	[DllImport("AgricolaLib")]
	public static extern int GetStartingPlayerIndex();

	// Token: 0x06000647 RID: 1607
	[DllImport("AgricolaLib")]
	public static extern int GetPlayerIndexFromInstanceID(int instance_ID);

	// Token: 0x06000648 RID: 1608
	[DllImport("AgricolaLib")]
	public static extern int GetPlayerInstanceIDFromIndex(int index);

	// Token: 0x06000649 RID: 1609
	[DllImport("AgricolaLib")]
	public static extern bool GetIsOnlineGame();

	// Token: 0x0600064A RID: 1610
	[DllImport("AgricolaLib")]
	public static extern bool GetIsTutorialGame();

	// Token: 0x0600064B RID: 1611
	[DllImport("AgricolaLib")]
	public static extern bool GetIsHotseatGame();

	// Token: 0x0600064C RID: 1612
	[DllImport("AgricolaLib")]
	public static extern bool GetIsCompletedGame();

	// Token: 0x0600064D RID: 1613
	[DllImport("AgricolaLib")]
	public static extern int GetTutorialIndex();

	// Token: 0x0600064E RID: 1614
	[DllImport("AgricolaLib")]
	public static extern int GetNewLocalPlayerID();

	// Token: 0x0600064F RID: 1615
	[DllImport("AgricolaLib")]
	public static extern uint GetChecksumString(string stringToHash);

	// Token: 0x06000650 RID: 1616
	[DllImport("AgricolaLib")]
	public static extern void SetSaveDataFunc(AgricolaLib.SaveWorldDataDelegate pOutputMessageFunc);

	// Token: 0x06000651 RID: 1617 RVA: 0x00032BC8 File Offset: 0x00030DC8
	public static EAgricolaSeason GetRoundSeason(int roundNumber)
	{
		EAgricolaSeason result = EAgricolaSeason.SPRING;
		switch (roundNumber)
		{
		case 1:
		case 2:
		case 5:
			result = EAgricolaSeason.SPRING;
			break;
		case 3:
		case 6:
		case 8:
		case 10:
		case 12:
			result = EAgricolaSeason.SUMMER;
			break;
		case 4:
		case 7:
		case 9:
		case 11:
		case 13:
		case 14:
			result = EAgricolaSeason.AUTUMN;
			break;
		}
		return result;
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00032C24 File Offset: 0x00030E24
	public static uint GetSoloSeriesPointRequirement(uint gameNumber)
	{
		if (gameNumber == 0U)
		{
			return 0U;
		}
		if (gameNumber <= 8U)
		{
			return AgricolaLib.s_kPointThresholds[(int)(gameNumber - 1U)];
		}
		return AgricolaLib.s_kPointThresholds[7] + (gameNumber - 8U);
	}

	// Token: 0x06000653 RID: 1619
	[DllImport("AgricolaLib")]
	public static extern void NetworkCreate();

	// Token: 0x06000654 RID: 1620
	[DllImport("AgricolaLib")]
	public static extern void NetworkLogin(string pLoginEmail, string pLoginPassword);

	// Token: 0x06000655 RID: 1621
	[DllImport("AgricolaLib")]
	public static extern void NetworkCreateAccount(string pLoginEmail, string pLoginPassword, string pLoginUsername);

	// Token: 0x06000656 RID: 1622
	[DllImport("AgricolaLib")]
	public static extern void NetworkSetLoginPassword(string pLoginUsername, string pLoginPassword);

	// Token: 0x06000657 RID: 1623
	[DllImport("AgricolaLib")]
	public static extern void NetworkSetCreateAccount(string pLoginEmail, string pLoginPassword, string pLoginUsername);

	// Token: 0x06000658 RID: 1624
	[DllImport("AgricolaLib")]
	public static extern void NetworkConnect();

	// Token: 0x06000659 RID: 1625
	[DllImport("AgricolaLib")]
	public static extern void NetworkVerifyAccount(string pLoginEmail, string pLoginPassword);

	// Token: 0x0600065A RID: 1626
	[DllImport("AgricolaLib")]
	public static extern void NetworkLinkAccount(string playdekEmail, string playdekPassword, long asmodeeUserID, string accessToken);

	// Token: 0x0600065B RID: 1627
	[DllImport("AgricolaLib")]
	public static extern void NetworkLinkAccountPlaceholder(string asmodeeEmail, string asmodeePassword, string asmodeeLoginName, long asmodeeUserID, string accessToken);

	// Token: 0x0600065C RID: 1628
	[DllImport("AgricolaLib")]
	public static extern void NetworkConnectAsmodee(string platformAccount, string accessToken);

	// Token: 0x0600065D RID: 1629
	[DllImport("AgricolaLib")]
	public static extern int NetworkUpdate(IntPtr pNetworkEvents, int maxEvents);

	// Token: 0x0600065E RID: 1630
	[DllImport("AgricolaLib")]
	public static extern void NetworkDisconnect();

	// Token: 0x0600065F RID: 1631
	[DllImport("AgricolaLib")]
	public static extern void NetworkDestroy();

	// Token: 0x06000660 RID: 1632
	[DllImport("AgricolaLib")]
	public static extern void NetworkResumeGame(int gameIndex);

	// Token: 0x06000661 RID: 1633
	[DllImport("AgricolaLib")]
	public static extern void NetworkGameFinished();

	// Token: 0x06000662 RID: 1634
	[DllImport("AgricolaLib")]
	public static extern bool NetworkGetCurrentGameFinished();

	// Token: 0x06000663 RID: 1635
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetActiveGames(IntPtr pGames, int maxGames);

	// Token: 0x06000664 RID: 1636
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetActiveGameWithID(IntPtr pGames, int maxGames, uint gameID);

	// Token: 0x06000665 RID: 1637
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetCompletedGames(IntPtr pGames, int maxGames);

	// Token: 0x06000666 RID: 1638
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetGamePlayerTimers(uint gameID, IntPtr pTimers);

	// Token: 0x06000667 RID: 1639
	[DllImport("AgricolaLib")]
	public static extern bool NetworkGetGameResultData(uint gameID, IntPtr pData, int maxDataSize);

	// Token: 0x06000668 RID: 1640
	[DllImport("AgricolaLib")]
	public static extern bool NetworkGameHasLocalPlayerMoves(uint gameID);

	// Token: 0x06000669 RID: 1641
	[DllImport("AgricolaLib")]
	public static extern void NetworkRefreshAvailableGames();

	// Token: 0x0600066A RID: 1642
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetAvailableGames(IntPtr pGames, int maxGames);

	// Token: 0x0600066B RID: 1643
	[DllImport("AgricolaLib")]
	public static extern void NetworkJoinGame(int gameIndex);

	// Token: 0x0600066C RID: 1644
	[DllImport("AgricolaLib")]
	public static extern bool NetworkCreateGame(uint maxPlayerCount, IntPtr invitationIds, uint startPlayerTime, ref GameParameters pGameParameters);

	// Token: 0x0600066D RID: 1645
	[DllImport("AgricolaLib")]
	public static extern void NetworkSetNotifyDeviceId(IntPtr pId, int idLen);

	// Token: 0x0600066E RID: 1646
	[DllImport("AgricolaLib")]
	public static extern void NetworkSendLocalAvatarIndex(uint avatarIndex);

	// Token: 0x0600066F RID: 1647
	[DllImport("AgricolaLib")]
	public static extern void NetworkRequestFriendsList(string accessToken);

	// Token: 0x06000670 RID: 1648
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetFriendsList(IntPtr pFriendsBuffer, int maxFriends);

	// Token: 0x06000671 RID: 1649
	[DllImport("AgricolaLib")]
	public static extern bool NetworkAddFriendFromUserId(uint friendID, string accessToken);

	// Token: 0x06000672 RID: 1650
	[DllImport("AgricolaLib")]
	public static extern int NetworkAddFriendFromUsername(string userName, string accessToken);

	// Token: 0x06000673 RID: 1651
	[DllImport("AgricolaLib")]
	public static extern bool NetworkRemoveFriendWithUserId(uint exFriendID, string accessToken);

	// Token: 0x06000674 RID: 1652
	[DllImport("AgricolaLib")]
	public static extern void NetworkAddFriendFromEmail(string pEmail, string accessToken);

	// Token: 0x06000675 RID: 1653
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetUserOnlineStatus(uint userId);

	// Token: 0x06000676 RID: 1654
	[DllImport("AgricolaLib")]
	public static extern void NetworkRequestUsersOnlineStatus(IntPtr pUsers, int numUsers);

	// Token: 0x06000677 RID: 1655
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetLocalID();

	// Token: 0x06000678 RID: 1656
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetLocalAvatar();

	// Token: 0x06000679 RID: 1657
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetLocalRating();

	// Token: 0x0600067A RID: 1658
	[DllImport("AgricolaLib")]
	public static extern IntPtr NetworkGetLocalName();

	// Token: 0x0600067B RID: 1659
	[DllImport("AgricolaLib")]
	public static extern void NetworkRemotePlayerProfile(int userID, IntPtr c);

	// Token: 0x0600067C RID: 1660
	[DllImport("AgricolaLib")]
	public static extern void NetworkLocalPlayerProfile(IntPtr c);

	// Token: 0x0600067D RID: 1661
	[DllImport("AgricolaLib")]
	public static extern void NetworkRequestPlayerProfile(uint userID);

	// Token: 0x0600067E RID: 1662
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetGameFriends(IntPtr pFriendsBuffer, int maxFriends);

	// Token: 0x0600067F RID: 1663
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetNumGameFriends();

	// Token: 0x06000680 RID: 1664
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetMatchmakingGames(IntPtr pGameBuffer, int maxGames);

	// Token: 0x06000681 RID: 1665
	[DllImport("AgricolaLib")]
	public static extern bool NetworkDeleteMatchmakingGame(uint requestID);

	// Token: 0x06000682 RID: 1666
	[DllImport("AgricolaLib")]
	public static extern bool NetworkRequestMatchmaking(uint numPlayers, uint startPlayerTimer, uint maxRatingRange, uint maxWaitTime, ref GameParameters pGameParameters);

	// Token: 0x06000683 RID: 1667
	[DllImport("AgricolaLib")]
	public static extern bool NetworkForfeitGame(uint gameID, bool bLastPlayer);

	// Token: 0x06000684 RID: 1668
	[DllImport("AgricolaLib")]
	public static extern void NetworkSetMonitorGame(uint gameID);

	// Token: 0x06000685 RID: 1669
	[DllImport("AgricolaLib")]
	public static extern void NetworkClearMonitorGame();

	// Token: 0x06000686 RID: 1670
	[DllImport("AgricolaLib")]
	public static extern void NetworkRequestGameMonitor(uint gameID);

	// Token: 0x06000687 RID: 1671
	[DllImport("AgricolaLib")]
	public static extern bool NetworkDeleteGame(uint gameID);

	// Token: 0x06000688 RID: 1672
	[DllImport("AgricolaLib")]
	public static extern bool NetworkWithdrawFromGame(uint gameID);

	// Token: 0x06000689 RID: 1673
	[DllImport("AgricolaLib")]
	public static extern bool NetworkLaunchGame(uint gameID);

	// Token: 0x0600068A RID: 1674
	[DllImport("AgricolaLib")]
	public static extern bool NetworkAcceptGameInvite(uint gameID);

	// Token: 0x0600068B RID: 1675
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetChatChannelMessageCount(uint channelID);

	// Token: 0x0600068C RID: 1676
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetChatChannelMessageList(uint channelID, uint firstMessageIndex, uint maxMessageCount, IntPtr pData, int maxDataSize);

	// Token: 0x0600068D RID: 1677
	[DllImport("AgricolaLib")]
	public static extern void NetworkSubmitChatMessage(uint channelID, uint chatLength, byte[] chatMessage);

	// Token: 0x0600068E RID: 1678
	[DllImport("AgricolaLib")]
	public static extern void NetworkSubmitChatPosition(uint channelID, uint chatPositionIndex);

	// Token: 0x0600068F RID: 1679
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetChatPosition(uint channelID);

	// Token: 0x06000690 RID: 1680
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetLastChatMessageIndex(uint channelID);

	// Token: 0x06000691 RID: 1681
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetGameWaitingCount();

	// Token: 0x06000692 RID: 1682
	[DllImport("AgricolaLib")]
	public static extern uint NetworkGetNextActiveGameID(uint allowedScenarioFlags, uint allowedAdditionalCardFlags);

	// Token: 0x06000693 RID: 1683
	[DllImport("AgricolaLib")]
	public static extern void NetworkRematchGame(uint gameID);

	// Token: 0x06000694 RID: 1684
	[DllImport("AgricolaLib")]
	public static extern void NetworkSendPlayerParameters(IntPtr pGameParameters, uint gameParameterSize);

	// Token: 0x06000695 RID: 1685
	[DllImport("AgricolaLib")]
	public static extern bool NetworkIsConnectedToServer();

	// Token: 0x06000696 RID: 1686
	[DllImport("AgricolaLib")]
	public static extern bool NetworkIsCreatingAccount();

	// Token: 0x06000697 RID: 1687
	[DllImport("AgricolaLib")]
	public static extern bool NetworkIsRetreiveAvailableGameList();

	// Token: 0x06000698 RID: 1688
	[DllImport("AgricolaLib")]
	public static extern int NetworkGetNotificationSetting(IntPtr pGameParameters, int index);

	// Token: 0x06000699 RID: 1689
	[DllImport("AgricolaLib")]
	public static extern bool NetworkSetNotificationSetting(int index, bool bEnabled);

	// Token: 0x0600069A RID: 1690
	[DllImport("AgricolaLib")]
	public static extern void NetworkGetVerifyAccountName(IntPtr pData, int maxDataSize);

	// Token: 0x0400061D RID: 1565
	private const string DLL_NAME = "AgricolaLib";

	// Token: 0x0400061E RID: 1566
	private static readonly uint[] s_kPointThresholds = new uint[]
	{
		50U,
		55U,
		59U,
		62U,
		64U,
		65U,
		66U,
		67U
	};

	// Token: 0x02000787 RID: 1927
	// (Invoke) Token: 0x0600424D RID: 16973
	public delegate void GameOptionsListenerDelegate(int playerID, IntPtr pOptionPrompt, int numOptions, IntPtr pOptions);

	// Token: 0x02000788 RID: 1928
	// (Invoke) Token: 0x06004251 RID: 16977
	public delegate void SaveWorldDataDelegate(IntPtr pSaveData, int size, IntPtr pShortSave);
}
