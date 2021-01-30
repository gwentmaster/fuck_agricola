using System;
using System.Linq;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006BC RID: 1724
	public class RecentGame
	{
		// Token: 0x06003DC9 RID: 15817 RVA: 0x0012EF4C File Offset: 0x0012D14C
		public RecentGame(ApiRecentGameResponse.Data.Game raw)
		{
			this._tableId = raw.table_id;
			this._date = raw.date;
			this._game = raw.game;
			this._options = raw.options;
			this._status = ((raw.status == null) ? null : new GameStatus?((GameStatus)Enum.Parse(typeof(GameStatus), raw.status)));
			this._variant = raw.variant;
			this._score = raw.score;
			RecentGame.OtherPlayer[] otherPlayers;
			if (raw.other_players != null)
			{
				otherPlayers = (from x in raw.other_players
				select new RecentGame.OtherPlayer(x)).ToArray<RecentGame.OtherPlayer>();
			}
			else
			{
				otherPlayers = null;
			}
			this._otherPlayers = otherPlayers;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0012F020 File Offset: 0x0012D220
		internal RecentGame(string tableId, string date, string game, string options, GameStatus? status, string variant, int score, RecentGame.OtherPlayer[] otherPlayers)
		{
			this._tableId = tableId;
			this._date = date;
			this._game = game;
			this._options = options;
			this._status = status;
			this._variant = variant;
			this._score = score;
			this._otherPlayers = otherPlayers;
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06003DCB RID: 15819 RVA: 0x0012F070 File Offset: 0x0012D270
		public string TableId
		{
			get
			{
				return this._tableId;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003DCC RID: 15820 RVA: 0x0012F078 File Offset: 0x0012D278
		public string Date
		{
			get
			{
				return this._date;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06003DCD RID: 15821 RVA: 0x0012F080 File Offset: 0x0012D280
		public string Game
		{
			get
			{
				return this._game;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003DCE RID: 15822 RVA: 0x0012F088 File Offset: 0x0012D288
		public string Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x0012F090 File Offset: 0x0012D290
		public GameStatus? Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x0012F098 File Offset: 0x0012D298
		public string Variant
		{
			get
			{
				return this._variant;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x0012F0A0 File Offset: 0x0012D2A0
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x0012F0A8 File Offset: 0x0012D2A8
		public RecentGame.OtherPlayer[] OtherPlayers
		{
			get
			{
				return this._otherPlayers;
			}
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0012F0B0 File Offset: 0x0012D2B0
		public override bool Equals(object obj)
		{
			RecentGame recentGame = obj as RecentGame;
			if (recentGame == null)
			{
				return false;
			}
			if (this.TableId == recentGame.TableId && this.Date == recentGame.Date && this.Game == recentGame.Game && this.Options == recentGame.Options)
			{
				GameStatus? status = this.Status;
				GameStatus? status2 = recentGame.Status;
				if ((status.GetValueOrDefault() == status2.GetValueOrDefault() & status != null == (status2 != null)) && this.Variant == recentGame.Variant && this.Score == recentGame.Score)
				{
					return this.OtherPlayers.Diff(recentGame.OtherPlayers).Count<RecentGame.OtherPlayer>() == 0;
				}
			}
			return false;
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0012F188 File Offset: 0x0012D388
		public override int GetHashCode()
		{
			return ((this.TableId == null) ? 0 : this.TableId.GetHashCode()) ^ ((this.Date == null) ? 0 : this.Date.GetHashCode()) ^ ((this.Game == null) ? 0 : this.Game.GetHashCode()) ^ ((this.Options == null) ? 0 : this.Options.GetHashCode()) ^ (int)((this.Status == null) ? GameStatus.reserved : this.Status.Value) ^ ((this.Variant == null) ? 0 : this.Variant.GetHashCode()) ^ this.Score ^ this.OtherPlayers.GetHashCode();
		}

		// Token: 0x040027E0 RID: 10208
		private string _tableId;

		// Token: 0x040027E1 RID: 10209
		private string _date;

		// Token: 0x040027E2 RID: 10210
		private string _game;

		// Token: 0x040027E3 RID: 10211
		private string _options;

		// Token: 0x040027E4 RID: 10212
		private GameStatus? _status;

		// Token: 0x040027E5 RID: 10213
		private string _variant;

		// Token: 0x040027E6 RID: 10214
		private int _score;

		// Token: 0x040027E7 RID: 10215
		private RecentGame.OtherPlayer[] _otherPlayers;

		// Token: 0x020009BF RID: 2495
		public class OtherPlayer
		{
			// Token: 0x060048BA RID: 18618 RVA: 0x0014C8F2 File Offset: 0x0014AAF2
			public OtherPlayer(ApiRecentGameResponse.Data.Game.OtherPlayer raw)
			{
				this._id = raw.id;
				this._loginName = raw.login_name;
				this._avatar = raw.avatar;
				this._score = raw.score;
			}

			// Token: 0x060048BB RID: 18619 RVA: 0x0014C92A File Offset: 0x0014AB2A
			internal OtherPlayer(int id, string loginName, string avatar, int score)
			{
				this._id = id;
				this._loginName = loginName;
				this._avatar = avatar;
				this._score = score;
			}

			// Token: 0x17000A6B RID: 2667
			// (get) Token: 0x060048BC RID: 18620 RVA: 0x0014C94F File Offset: 0x0014AB4F
			public int Id
			{
				get
				{
					return this._id;
				}
			}

			// Token: 0x17000A6C RID: 2668
			// (get) Token: 0x060048BD RID: 18621 RVA: 0x0014C957 File Offset: 0x0014AB57
			public string LoginName
			{
				get
				{
					return this._loginName;
				}
			}

			// Token: 0x17000A6D RID: 2669
			// (get) Token: 0x060048BE RID: 18622 RVA: 0x0014C95F File Offset: 0x0014AB5F
			public string Avatar
			{
				get
				{
					return this._avatar;
				}
			}

			// Token: 0x17000A6E RID: 2670
			// (get) Token: 0x060048BF RID: 18623 RVA: 0x0014C967 File Offset: 0x0014AB67
			public int Score
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x060048C0 RID: 18624 RVA: 0x0014C970 File Offset: 0x0014AB70
			public override bool Equals(object obj)
			{
				RecentGame.OtherPlayer otherPlayer = obj as RecentGame.OtherPlayer;
				return otherPlayer != null && (this.Id == otherPlayer.Id && this.LoginName == otherPlayer.LoginName && this.Avatar == otherPlayer.Avatar) && this.Score == otherPlayer.Score;
			}

			// Token: 0x060048C1 RID: 18625 RVA: 0x0014C9CD File Offset: 0x0014ABCD
			public override int GetHashCode()
			{
				return this.Id ^ ((this.LoginName == null) ? 0 : this.LoginName.GetHashCode()) ^ ((this.Avatar == null) ? 0 : this.Avatar.GetHashCode()) ^ this.Score;
			}

			// Token: 0x040032E8 RID: 13032
			private int _id;

			// Token: 0x040032E9 RID: 13033
			private string _loginName;

			// Token: 0x040032EA RID: 13034
			private string _avatar;

			// Token: 0x040032EB RID: 13035
			private int _score;
		}
	}
}
