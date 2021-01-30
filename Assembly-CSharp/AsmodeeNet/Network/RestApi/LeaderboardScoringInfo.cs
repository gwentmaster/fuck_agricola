using System;
using System.Collections.Generic;
using AsmodeeNet.Utils;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006B5 RID: 1717
	[Serializable]
	public class LeaderboardScoringInfo
	{
		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x0012EBC1 File Offset: 0x0012CDC1
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x0012EBC9 File Offset: 0x0012CDC9
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x0012EBD1 File Offset: 0x0012CDD1
		public string Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x0012EBD9 File Offset: 0x0012CDD9
		public DateTime? When
		{
			get
			{
				return this._when;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x0012EBE1 File Offset: 0x0012CDE1
		private bool? IsNew
		{
			get
			{
				return this._isNew;
			}
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x0012EBEC File Offset: 0x0012CDEC
		private LeaderboardScoringInfo(Builder<LeaderboardScoringInfo> builder)
		{
			LeaderboardScoringInfo.Builder builder2 = builder as LeaderboardScoringInfo.Builder;
			this._rank = builder2.GetRank;
			this._score = builder2.GetScore;
			this._context = builder2.GetContext;
			this._when = builder2.GetWhen;
		}

		// Token: 0x040027C4 RID: 10180
		private int _score = -1;

		// Token: 0x040027C5 RID: 10181
		private string _context;

		// Token: 0x040027C6 RID: 10182
		private int _rank = -1;

		// Token: 0x040027C7 RID: 10183
		private DateTime? _when;

		// Token: 0x040027C8 RID: 10184
		private bool? _isNew;

		// Token: 0x020009BC RID: 2492
		public class Builder : Builder<LeaderboardScoringInfo>
		{
			// Token: 0x060048A4 RID: 18596 RVA: 0x0014C661 File Offset: 0x0014A861
			public Builder()
			{
			}

			// Token: 0x060048A5 RID: 18597 RVA: 0x0014C678 File Offset: 0x0014A878
			public Builder(ApiLeaderboardGetRankAndScoreResponse raw)
			{
				this._score = raw.data.user.score;
				this._context = raw.data.user.context;
				this._rank = raw.data.user.rank;
				this._when = ((raw.data.user.when == null) ? null : new DateTime?(DateTime.Parse(raw.data.user.when)));
			}

			// Token: 0x060048A6 RID: 18598 RVA: 0x0014C718 File Offset: 0x0014A918
			public override Builder<LeaderboardScoringInfo>.BuilderErrors[] Validate()
			{
				List<Builder<LeaderboardScoringInfo>.BuilderErrors> list = null;
				if (this._context != null && this._context.Length > 200)
				{
					if (list == null)
					{
						list = new List<Builder<LeaderboardScoringInfo>.BuilderErrors>();
					}
					list.Add(new Builder<LeaderboardScoringInfo>.BuilderErrors("Context", "\"context\" length must not exceed 200 characters"));
				}
				if (list != null)
				{
					return list.ToArray();
				}
				return null;
			}

			// Token: 0x060048A7 RID: 18599 RVA: 0x0014C76A File Offset: 0x0014A96A
			public LeaderboardScoringInfo.Builder Score(int score)
			{
				this._score = score;
				return this;
			}

			// Token: 0x060048A8 RID: 18600 RVA: 0x0014C774 File Offset: 0x0014A974
			public LeaderboardScoringInfo.Builder Context(string context)
			{
				this._context = context;
				return this;
			}

			// Token: 0x060048A9 RID: 18601 RVA: 0x0014C77E File Offset: 0x0014A97E
			public LeaderboardScoringInfo.Builder Rank(int rank)
			{
				this._rank = rank;
				return this;
			}

			// Token: 0x060048AA RID: 18602 RVA: 0x0014C788 File Offset: 0x0014A988
			public LeaderboardScoringInfo.Builder When(DateTime? when)
			{
				this._when = when;
				return this;
			}

			// Token: 0x060048AB RID: 18603 RVA: 0x0014C792 File Offset: 0x0014A992
			public LeaderboardScoringInfo.Builder IsNew(bool isNew)
			{
				this._isNew = new bool?(isNew);
				return this;
			}

			// Token: 0x17000A64 RID: 2660
			// (get) Token: 0x060048AC RID: 18604 RVA: 0x0014C7A1 File Offset: 0x0014A9A1
			public int GetScore
			{
				get
				{
					return this._score;
				}
			}

			// Token: 0x17000A65 RID: 2661
			// (get) Token: 0x060048AD RID: 18605 RVA: 0x0014C7A9 File Offset: 0x0014A9A9
			public string GetContext
			{
				get
				{
					return this._context;
				}
			}

			// Token: 0x17000A66 RID: 2662
			// (get) Token: 0x060048AE RID: 18606 RVA: 0x0014C7B1 File Offset: 0x0014A9B1
			public int GetRank
			{
				get
				{
					return this._rank;
				}
			}

			// Token: 0x17000A67 RID: 2663
			// (get) Token: 0x060048AF RID: 18607 RVA: 0x0014C7B9 File Offset: 0x0014A9B9
			public DateTime? GetWhen
			{
				get
				{
					return this._when;
				}
			}

			// Token: 0x17000A68 RID: 2664
			// (get) Token: 0x060048B0 RID: 18608 RVA: 0x0014C7C1 File Offset: 0x0014A9C1
			public bool? GetIsNew
			{
				get
				{
					return this._isNew;
				}
			}

			// Token: 0x040032DE RID: 13022
			private int _score = -1;

			// Token: 0x040032DF RID: 13023
			private string _context;

			// Token: 0x040032E0 RID: 13024
			private int _rank = -1;

			// Token: 0x040032E1 RID: 13025
			private DateTime? _when;

			// Token: 0x040032E2 RID: 13026
			private bool? _isNew;
		}
	}
}
