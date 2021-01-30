using System;

// Token: 0x020000B6 RID: 182
public struct NetworkEvent
{
	// Token: 0x040007F9 RID: 2041
	public int type;

	// Token: 0x040007FA RID: 2042
	public int data;

	// Token: 0x0200078B RID: 1931
	public enum EventType
	{
		// Token: 0x04002C1A RID: 11290
		Event_LoginInitiated = 1,
		// Token: 0x04002C1B RID: 11291
		Event_LoginComplete,
		// Token: 0x04002C1C RID: 11292
		Event_LoginError,
		// Token: 0x04002C1D RID: 11293
		Event_CreateAccountReply,
		// Token: 0x04002C1E RID: 11294
		Event_AvailableGamesRefreshed,
		// Token: 0x04002C1F RID: 11295
		Event_UpdatedGameList,
		// Token: 0x04002C20 RID: 11296
		Event_ConnectionLost,
		// Token: 0x04002C21 RID: 11297
		Event_CreateGameReply,
		// Token: 0x04002C22 RID: 11298
		Event_JoinGameReply,
		// Token: 0x04002C23 RID: 11299
		Event_MatchmakingReply,
		// Token: 0x04002C24 RID: 11300
		Event_FriendRequestReply,
		// Token: 0x04002C25 RID: 11301
		Event_UpdatedMatchmakingList,
		// Token: 0x04002C26 RID: 11302
		Event_UpdatedPlayerProfile,
		// Token: 0x04002C27 RID: 11303
		Event_UpdatedOnlineStatus,
		// Token: 0x04002C28 RID: 11304
		Event_UpdatedFriendsList,
		// Token: 0x04002C29 RID: 11305
		Event_DeletedActiveGame,
		// Token: 0x04002C2A RID: 11306
		Event_VerifyAccount,
		// Token: 0x04002C2B RID: 11307
		Event_LinkAccount
	}
}
