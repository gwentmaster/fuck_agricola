using System;
using System.Linq;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006B3 RID: 1715
	public class GameLeaderboard
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x0012EA45 File Offset: 0x0012CC45
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06003D9A RID: 15770 RVA: 0x0012EA4D File Offset: 0x0012CC4D
		public string Game
		{
			get
			{
				return this._game;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06003D9B RID: 15771 RVA: 0x0012EA55 File Offset: 0x0012CC55
		public Period Period
		{
			get
			{
				return this._period;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06003D9C RID: 15772 RVA: 0x0012EA5D File Offset: 0x0012CC5D
		public GameLeaderboard.Player[] Players
		{
			get
			{
				return this._players;
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0012EA65 File Offset: 0x0012CC65
		internal GameLeaderboard(string id, string game, Period period, GameLeaderboard.Player[] players)
		{
			this._id = id;
			this._game = game;
			this._period = period;
			this._players = players;
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x0012EA8C File Offset: 0x0012CC8C
		public GameLeaderboard(ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard raw)
		{
			this._id = raw.id;
			this._game = raw.game;
			this._period = (Period)Enum.Parse(typeof(Period), raw.period);
			this._players = (from x in raw.players
			select new GameLeaderboard.Player(x)).ToArray<GameLeaderboard.Player>();
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x0012EB0C File Offset: 0x0012CD0C
		public override bool Equals(object obj)
		{
			GameLeaderboard gameLeaderboard = obj as GameLeaderboard;
			return gameLeaderboard != null && (this.Id == gameLeaderboard.Id && this.Game == gameLeaderboard.Game && this.Period == gameLeaderboard.Period) && this.Players.Diff(gameLeaderboard.Players).Count<GameLeaderboard.Player>() == 0;
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x0012EB74 File Offset: 0x0012CD74
		public override int GetHashCode()
		{
			return ((this.Id == null) ? 0 : this.Id.GetHashCode()) ^ ((this.Game == null) ? 0 : this.Game.GetHashCode()) ^ (int)this.Period ^ this.Players.GetHashCode();
		}

		// Token: 0x040027BA RID: 10170
		private string _id;

		// Token: 0x040027BB RID: 10171
		private string _game;

		// Token: 0x040027BC RID: 10172
		private Period _period;

		// Token: 0x040027BD RID: 10173
		private GameLeaderboard.Player[] _players;

		// Token: 0x020009BA RID: 2490
		public class Player
		{
			// Token: 0x17000A5D RID: 2653
			// (get) Token: 0x06004896 RID: 18582 RVA: 0x0014C3FD File Offset: 0x0014A5FD
			public int Rank
			{
				get
				{
					return this._rank;
				}
			}

			// Token: 0x17000A5E RID: 2654
			// (get) Token: 0x06004897 RID: 18583 RVA: 0x0014C405 File Offset: 0x0014A605
			public int Id
			{
				get
				{
					return this._id;
				}
			}

			// Token: 0x17000A5F RID: 2655
			// (get) Token: 0x06004898 RID: 18584 RVA: 0x0014C40D File Offset: 0x0014A60D
			public int Score
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x17000A60 RID: 2656
			// (get) Token: 0x06004899 RID: 18585 RVA: 0x0014C415 File Offset: 0x0014A615
			public string Context
			{
				get
				{
					return this._context;
				}
			}

			// Token: 0x17000A61 RID: 2657
			// (get) Token: 0x0600489A RID: 18586 RVA: 0x0014C41D File Offset: 0x0014A61D
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x0600489B RID: 18587 RVA: 0x0014C425 File Offset: 0x0014A625
			public string Avatar
			{
				get
				{
					return this._avatar;
				}
			}

			// Token: 0x17000A63 RID: 2659
			// (get) Token: 0x0600489C RID: 18588 RVA: 0x0014C42D File Offset: 0x0014A62D
			public DateTime? When
			{
				get
				{
					return this._when;
				}
			}

			// Token: 0x0600489D RID: 18589 RVA: 0x0014C438 File Offset: 0x0014A638
			public override bool Equals(object obj)
			{
				GameLeaderboard.Player player = obj as GameLeaderboard.Player;
				return player != null && (this.Rank == player.Rank && this.Id == player.Id && this.Score == player.Score && this.Context == player.Context && this.Name == player.Name && this.Avatar == player.Avatar) && this.When == player.When;
			}

			// Token: 0x0600489E RID: 18590 RVA: 0x0014C4FC File Offset: 0x0014A6FC
			public override int GetHashCode()
			{
				return this.Rank ^ this.Id ^ this.Score ^ ((this.Context == null) ? 0 : this.Context.GetHashCode()) ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ ((this.Avatar == null) ? 0 : this.Avatar.GetHashCode()) ^ ((this.When == null) ? 0 : this.When.GetHashCode());
			}

			// Token: 0x0600489F RID: 18591 RVA: 0x0014C58C File Offset: 0x0014A78C
			public Player(ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard.Player raw)
			{
				this._rank = raw.rank;
				this._id = raw.id;
				this._score = raw.score;
				this._context = raw.context;
				this._name = raw.name;
				this._avatar = raw.avatar;
				this._when = ((raw.when == null) ? null : new DateTime?(DateTime.Parse(raw.when)));
			}

			// Token: 0x060048A0 RID: 18592 RVA: 0x0014C610 File Offset: 0x0014A810
			internal Player(int rank, int id, int score, string context, string name, string avatar, DateTime? when)
			{
				this._rank = rank;
				this._id = id;
				this._score = score;
				this._context = context;
				this._name = name;
				this._avatar = avatar;
				this._when = when;
			}

			// Token: 0x040032D5 RID: 13013
			private int _rank;

			// Token: 0x040032D6 RID: 13014
			private int _id;

			// Token: 0x040032D7 RID: 13015
			private int _score;

			// Token: 0x040032D8 RID: 13016
			private string _context;

			// Token: 0x040032D9 RID: 13017
			private string _name;

			// Token: 0x040032DA RID: 13018
			private string _avatar;

			// Token: 0x040032DB RID: 13019
			private DateTime? _when;
		}
	}
}
