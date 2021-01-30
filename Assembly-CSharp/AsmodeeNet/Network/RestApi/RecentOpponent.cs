using System;
using System.Linq;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006BD RID: 1725
	public class RecentOpponent
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x0012F23C File Offset: 0x0012D43C
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x0012F244 File Offset: 0x0012D444
		public string LoginName
		{
			get
			{
				return this._loginName;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x0012F24C File Offset: 0x0012D44C
		public string Avatar
		{
			get
			{
				return this._avatar;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x0012F254 File Offset: 0x0012D454
		public string LastGameDate
		{
			get
			{
				return this._lastGameDate;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x0012F25C File Offset: 0x0012D45C
		public RecentOpponent.Game[] Games
		{
			get
			{
				return this._games;
			}
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0012F264 File Offset: 0x0012D464
		public RecentOpponent(ApiRecentOpponentsResponse.Data.Opponent raw)
		{
			this._id = raw.id;
			this._loginName = raw.login_name;
			this._avatar = raw.avatar;
			this._lastGameDate = raw.last_game_date;
			this._games = (from x in raw.games
			select new RecentOpponent.Game(x)).ToArray<RecentOpponent.Game>();
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0012F2DC File Offset: 0x0012D4DC
		internal RecentOpponent(int id, string loginName, string avatar, string lastGameDate, RecentOpponent.Game[] games)
		{
			this._id = id;
			this._loginName = loginName;
			this._avatar = avatar;
			this._lastGameDate = lastGameDate;
			this._games = games;
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x0012F30C File Offset: 0x0012D50C
		public override bool Equals(object obj)
		{
			RecentOpponent recentOpponent = obj as RecentOpponent;
			return recentOpponent != null && (this.Id == recentOpponent.Id && this.LoginName == recentOpponent.LoginName && this.Avatar == recentOpponent.Avatar && this.LastGameDate == recentOpponent.LastGameDate) && this.Games.Diff(recentOpponent.Games).Count<RecentOpponent.Game>() == 0;
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x0012F388 File Offset: 0x0012D588
		public override int GetHashCode()
		{
			return this.Id ^ ((this.LoginName == null) ? 0 : this.LoginName.GetHashCode()) ^ ((this.Avatar == null) ? 0 : this.Avatar.GetHashCode()) ^ ((this.LastGameDate == null) ? 0 : this.LastGameDate.GetHashCode()) ^ this.Games.GetHashCode();
		}

		// Token: 0x040027E8 RID: 10216
		private int _id;

		// Token: 0x040027E9 RID: 10217
		private string _loginName;

		// Token: 0x040027EA RID: 10218
		private string _avatar;

		// Token: 0x040027EB RID: 10219
		private string _lastGameDate;

		// Token: 0x040027EC RID: 10220
		private RecentOpponent.Game[] _games;

		// Token: 0x020009C1 RID: 2497
		public class Game
		{
			// Token: 0x17000A6F RID: 2671
			// (get) Token: 0x060048C5 RID: 18629 RVA: 0x0014CA1E File Offset: 0x0014AC1E
			public string TableId
			{
				get
				{
					return this._tableId;
				}
			}

			// Token: 0x17000A70 RID: 2672
			// (get) Token: 0x060048C6 RID: 18630 RVA: 0x0014CA26 File Offset: 0x0014AC26
			public string GameName
			{
				get
				{
					return this._gameName;
				}
			}

			// Token: 0x17000A71 RID: 2673
			// (get) Token: 0x060048C7 RID: 18631 RVA: 0x0014CA2E File Offset: 0x0014AC2E
			public string Date
			{
				get
				{
					return this._date;
				}
			}

			// Token: 0x17000A72 RID: 2674
			// (get) Token: 0x060048C8 RID: 18632 RVA: 0x0014CA36 File Offset: 0x0014AC36
			public GameStatus? Status
			{
				get
				{
					return this._status;
				}
			}

			// Token: 0x17000A73 RID: 2675
			// (get) Token: 0x060048C9 RID: 18633 RVA: 0x0014CA3E File Offset: 0x0014AC3E
			public int OtherScore
			{
				get
				{
					return this._otherScore;
				}
			}

			// Token: 0x17000A74 RID: 2676
			// (get) Token: 0x060048CA RID: 18634 RVA: 0x0014CA46 File Offset: 0x0014AC46
			public int Score
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x060048CB RID: 18635 RVA: 0x0014CA50 File Offset: 0x0014AC50
			public Game(ApiRecentOpponentsResponse.Data.Opponent.Game raw)
			{
				this._tableId = raw.table_id;
				this._gameName = raw.game;
				this._date = raw.date;
				this._status = ((raw.status == null) ? null : new GameStatus?((GameStatus)Enum.Parse(typeof(GameStatus), raw.status)));
				this._otherScore = raw.other_score;
				this._score = raw.score;
			}

			// Token: 0x060048CC RID: 18636 RVA: 0x0014CAD7 File Offset: 0x0014ACD7
			internal Game(string tableId, string gameName, string date, GameStatus? status, int otherScore, int score)
			{
				this._tableId = tableId;
				this._gameName = gameName;
				this._date = date;
				this._status = status;
				this._otherScore = otherScore;
				this._score = score;
			}

			// Token: 0x060048CD RID: 18637 RVA: 0x0014CB0C File Offset: 0x0014AD0C
			public override bool Equals(object obj)
			{
				RecentOpponent.Game game = obj as RecentOpponent.Game;
				if (game == null)
				{
					return false;
				}
				if (this.TableId == game.TableId && this.GameName == game.GameName && this.Date == game.Date)
				{
					GameStatus? status = this.Status;
					GameStatus? status2 = game.Status;
					if ((status.GetValueOrDefault() == status2.GetValueOrDefault() & status != null == (status2 != null)) && this.OtherScore == game.OtherScore)
					{
						return this.Score == game.Score;
					}
				}
				return false;
			}

			// Token: 0x060048CE RID: 18638 RVA: 0x0014CBB0 File Offset: 0x0014ADB0
			public override int GetHashCode()
			{
				return ((this.TableId == null) ? 0 : this.TableId.GetHashCode()) ^ ((this.GameName == null) ? 0 : this.GameName.GetHashCode()) ^ ((this.Date == null) ? 0 : this.Date.GetHashCode()) ^ (int)this.Status.Value ^ this.OtherScore ^ this.Score;
			}

			// Token: 0x040032EE RID: 13038
			private string _tableId;

			// Token: 0x040032EF RID: 13039
			private string _gameName;

			// Token: 0x040032F0 RID: 13040
			private string _date;

			// Token: 0x040032F1 RID: 13041
			private GameStatus? _status;

			// Token: 0x040032F2 RID: 13042
			private int _otherScore;

			// Token: 0x040032F3 RID: 13043
			private int _score;
		}
	}
}
