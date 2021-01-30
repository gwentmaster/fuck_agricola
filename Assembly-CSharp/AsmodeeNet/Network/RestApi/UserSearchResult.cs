using System;
using System.Linq;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C6 RID: 1734
	[Serializable]
	public class UserSearchResult
	{
		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06003E2A RID: 15914 RVA: 0x0012FD34 File Offset: 0x0012DF34
		public int UserId
		{
			get
			{
				return this._userId;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06003E2B RID: 15915 RVA: 0x0012FD3C File Offset: 0x0012DF3C
		public string LoginName
		{
			get
			{
				return this._loginName;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06003E2C RID: 15916 RVA: 0x0012FD44 File Offset: 0x0012DF44
		public string[] Features
		{
			get
			{
				return this._features;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003E2D RID: 15917 RVA: 0x0012FD4C File Offset: 0x0012DF4C
		public UserSearchResult.BoardGame[] Boardgames
		{
			get
			{
				return this._boardGames;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06003E2E RID: 15918 RVA: 0x0012FD54 File Offset: 0x0012DF54
		public UserSearchResult.OnlineGame[] Onlinegames
		{
			get
			{
				return this._onlineGames;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06003E2F RID: 15919 RVA: 0x0012FD5C File Offset: 0x0012DF5C
		public string Avatar
		{
			get
			{
				return this._avatar;
			}
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x0012FD64 File Offset: 0x0012DF64
		public UserSearchResult(int userId, string loginName, string avatar, string[] features, UserSearchResult.BoardGame[] boardGames, UserSearchResult.OnlineGame[] onlineGames)
		{
			this._loginName = loginName;
			this._userId = userId;
			this._avatar = avatar;
			this._features = features;
			this._boardGames = boardGames;
			this._onlineGames = onlineGames;
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x0012FDA0 File Offset: 0x0012DFA0
		public UserSearchResult(ApiSearchUserResponse.Data.User apiSearchUserResponseUser)
		{
			this._userId = apiSearchUserResponseUser.user_id;
			this._loginName = apiSearchUserResponseUser.login_name;
			this._avatar = apiSearchUserResponseUser.avatar;
			this._features = ((apiSearchUserResponseUser.features == null) ? null : (apiSearchUserResponseUser.features.Clone() as string[]));
			UserSearchResult.BoardGame[] boardGames;
			if (apiSearchUserResponseUser.boardgames != null)
			{
				boardGames = (from y in apiSearchUserResponseUser.boardgames
				select new UserSearchResult.BoardGame(y.code, y.name, (y.registered_date == null) ? null : new DateTime?(DateTime.Parse(y.registered_date)))).ToArray<UserSearchResult.BoardGame>();
			}
			else
			{
				boardGames = null;
			}
			this._boardGames = boardGames;
			UserSearchResult.OnlineGame[] onlineGames;
			if (apiSearchUserResponseUser.onlinegames != null)
			{
				onlineGames = (from z in apiSearchUserResponseUser.onlinegames
				select new UserSearchResult.OnlineGame(z.game, z.nbgames, z.karma, z.rankscore, z.rank, (z.lastgame == null) ? null : new DateTime?(DateTime.Parse(z.lastgame)), z.variant)).ToArray<UserSearchResult.OnlineGame>();
			}
			else
			{
				onlineGames = null;
			}
			this._onlineGames = onlineGames;
		}

		// Token: 0x04002829 RID: 10281
		private string _loginName;

		// Token: 0x0400282A RID: 10282
		private int _userId = -1;

		// Token: 0x0400282B RID: 10283
		private string _avatar;

		// Token: 0x0400282C RID: 10284
		private string[] _features;

		// Token: 0x0400282D RID: 10285
		private UserSearchResult.BoardGame[] _boardGames;

		// Token: 0x0400282E RID: 10286
		private UserSearchResult.OnlineGame[] _onlineGames;

		// Token: 0x020009C8 RID: 2504
		public class BoardGame
		{
			// Token: 0x06004910 RID: 18704 RVA: 0x0014D49B File Offset: 0x0014B69B
			public BoardGame(string code, string name, DateTime? registeredDate)
			{
				this.code = code;
				this.name = name;
				this.registeredDate = registeredDate;
			}

			// Token: 0x04003327 RID: 13095
			public string code;

			// Token: 0x04003328 RID: 13096
			public string name;

			// Token: 0x04003329 RID: 13097
			public DateTime? registeredDate;
		}

		// Token: 0x020009C9 RID: 2505
		public class OnlineGame
		{
			// Token: 0x06004911 RID: 18705 RVA: 0x0014D4B8 File Offset: 0x0014B6B8
			public OnlineGame(string game, int nbGames, int karma, float rankScore, int rank, DateTime? lastGame, string variant)
			{
				this.game = game;
				this.nbGames = nbGames;
				this.karma = karma;
				this.rankScore = rankScore;
				this.rank = rank;
				this.lastGame = lastGame;
				this.variant = variant;
			}

			// Token: 0x0400332A RID: 13098
			public string game;

			// Token: 0x0400332B RID: 13099
			public int nbGames;

			// Token: 0x0400332C RID: 13100
			public int karma;

			// Token: 0x0400332D RID: 13101
			public float rankScore;

			// Token: 0x0400332E RID: 13102
			public int rank;

			// Token: 0x0400332F RID: 13103
			public DateTime? lastGame;

			// Token: 0x04003330 RID: 13104
			public string variant;
		}
	}
}
