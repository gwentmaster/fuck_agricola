using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006B2 RID: 1714
	[Serializable]
	public class FetchRank
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x0012E883 File Offset: 0x0012CA83
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x0012E88B File Offset: 0x0012CA8B
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x0012E893 File Offset: 0x0012CA93
		public int NbGames
		{
			get
			{
				return this._nbgames;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x0012E89B File Offset: 0x0012CA9B
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06003D92 RID: 15762 RVA: 0x0012E8A3 File Offset: 0x0012CAA3
		public int Karma
		{
			get
			{
				return this._karma;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x0012E8AB File Offset: 0x0012CAAB
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x0012E8B3 File Offset: 0x0012CAB3
		public string Ranking
		{
			get
			{
				return this._ranking;
			}
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0012E8BB File Offset: 0x0012CABB
		public FetchRank(int id, string name, int nbGames, int rank, int karma, int score, string ranking)
		{
			this._id = id;
			this._name = name;
			this._nbgames = nbGames;
			this._rank = rank;
			this._karma = karma;
			this._score = score;
			this._ranking = ranking;
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x0012E8F8 File Offset: 0x0012CAF8
		public FetchRank(ApiFetchRankResponse.Data.User raw)
		{
			this._id = raw.id;
			this._name = raw.name;
			this._nbgames = raw.nbgames;
			this._rank = raw.rank;
			this._karma = raw.karma;
			this._score = raw.score;
			this._ranking = raw.ranking;
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x0012E960 File Offset: 0x0012CB60
		public override bool Equals(object o)
		{
			FetchRank fetchRank = o as FetchRank;
			return fetchRank != null && (this.Id == fetchRank.Id && this.Name == fetchRank.Name && this.NbGames == fetchRank.NbGames && this.Rank == fetchRank.Rank && this.Karma == fetchRank.Karma && this.Score == fetchRank.Score) && this.Ranking == fetchRank.Ranking;
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x0012E9E8 File Offset: 0x0012CBE8
		public override int GetHashCode()
		{
			return this.Id ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ this.NbGames ^ this.Rank ^ this.Karma ^ this.Score ^ ((this.Ranking == null) ? 0 : this.Ranking.GetHashCode());
		}

		// Token: 0x040027B3 RID: 10163
		[SerializeField]
		private int _id;

		// Token: 0x040027B4 RID: 10164
		[SerializeField]
		private string _name;

		// Token: 0x040027B5 RID: 10165
		[SerializeField]
		private int _nbgames;

		// Token: 0x040027B6 RID: 10166
		[SerializeField]
		private int _rank;

		// Token: 0x040027B7 RID: 10167
		[SerializeField]
		private int _karma;

		// Token: 0x040027B8 RID: 10168
		[SerializeField]
		private int _score;

		// Token: 0x040027B9 RID: 10169
		[SerializeField]
		private string _ranking;
	}
}
