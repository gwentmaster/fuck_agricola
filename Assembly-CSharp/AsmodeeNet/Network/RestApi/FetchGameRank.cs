using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006B1 RID: 1713
	[Serializable]
	public class FetchGameRank
	{
		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06003D85 RID: 15749 RVA: 0x0012E747 File Offset: 0x0012C947
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06003D86 RID: 15750 RVA: 0x0012E74F File Offset: 0x0012C94F
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06003D87 RID: 15751 RVA: 0x0012E757 File Offset: 0x0012C957
		public int NbGames
		{
			get
			{
				return this._nbGames;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06003D88 RID: 15752 RVA: 0x0012E75F File Offset: 0x0012C95F
		public int Karma
		{
			get
			{
				return this._karma;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06003D89 RID: 15753 RVA: 0x0012E767 File Offset: 0x0012C967
		public int Score
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x0012E76F File Offset: 0x0012C96F
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x0012E778 File Offset: 0x0012C978
		public FetchGameRank(ApiFetchGameRankResponse.Data.Rank raw)
		{
			this._id = raw.id;
			this._name = raw.name;
			this._nbGames = raw.nbgames;
			this._karma = raw.karma;
			this._score = raw.score;
			this._rank = raw.rank;
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x0012E7D4 File Offset: 0x0012C9D4
		public override bool Equals(object obj)
		{
			FetchGameRank fetchGameRank = obj as FetchGameRank;
			return fetchGameRank != null && (this.Id == fetchGameRank.Id && this.Name == fetchGameRank.Name && this.NbGames == fetchGameRank.NbGames && this.Karma == fetchGameRank.Karma && this.Score == fetchGameRank.Score) && this.Rank == fetchGameRank.Rank;
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x0012E848 File Offset: 0x0012CA48
		public override int GetHashCode()
		{
			return this.Id ^ ((this.Name == null) ? 0 : this.Name.GetHashCode()) ^ this.NbGames ^ this.Karma ^ this.Score ^ this.Rank;
		}

		// Token: 0x040027AD RID: 10157
		[SerializeField]
		private int _id;

		// Token: 0x040027AE RID: 10158
		[SerializeField]
		private string _name;

		// Token: 0x040027AF RID: 10159
		[SerializeField]
		private int _nbGames;

		// Token: 0x040027B0 RID: 10160
		[SerializeField]
		private int _karma;

		// Token: 0x040027B1 RID: 10161
		[SerializeField]
		private int _score;

		// Token: 0x040027B2 RID: 10162
		[SerializeField]
		private int _rank;
	}
}
