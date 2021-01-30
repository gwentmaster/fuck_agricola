using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006AD RID: 1709
	[Serializable]
	public class Award
	{
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003D70 RID: 15728 RVA: 0x0012E2CC File Offset: 0x0012C4CC
		public int Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06003D71 RID: 15729 RVA: 0x0012E2D4 File Offset: 0x0012C4D4
		public string Tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003D72 RID: 15730 RVA: 0x0012E2DC File Offset: 0x0012C4DC
		public int TableId
		{
			get
			{
				return this._tableId;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003D73 RID: 15731 RVA: 0x0012E2E4 File Offset: 0x0012C4E4
		public int InfoId
		{
			get
			{
				return this._infoId;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003D74 RID: 15732 RVA: 0x0012E2EC File Offset: 0x0012C4EC
		public DateTime? AwardedUTC
		{
			get
			{
				return this._awardedUTC;
			}
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0012E2F4 File Offset: 0x0012C4F4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Achievement : \n\tid : ",
				this.Id.ToString(),
				"\n\ttag : ",
				(this.Tag == null) ? "" : this.Tag,
				"\n\ttableId : ",
				this.TableId.ToString(),
				"\n\tinfoId : ",
				this.InfoId.ToString(),
				"\n\tAwardedUTC : ",
				(this.AwardedUTC == null) ? "?" : this.AwardedUTC.Value.ToString()
			});
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0012E3B1 File Offset: 0x0012C5B1
		public Award()
		{
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0012E3D0 File Offset: 0x0012C5D0
		public Award(int id, string tag, int tableId = -1, int infoId = -1, DateTime? awardedUTC = null)
		{
			if (tag != null)
			{
				if (tag.All((char x) => char.IsDigit(x)))
				{
					throw new ArgumentException("The \"tag\" parameter must not be only composed by digit characters");
				}
			}
			this._id = id;
			this._tag = tag;
			this._tableId = tableId;
			this._infoId = infoId;
			this._awardedUTC = awardedUTC;
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x0012E454 File Offset: 0x0012C654
		public Award(int id, int tableId = -1, int infoId = -1, DateTime? awardedUTC = null)
		{
			this._id = id;
			this._tag = null;
			this._tableId = tableId;
			this._infoId = infoId;
			this._awardedUTC = awardedUTC;
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x0012E4A0 File Offset: 0x0012C6A0
		public Award(string tag, int tableId = -1, int infoId = -1, DateTime? awardedUTC = null)
		{
			if (tag != null)
			{
				if (tag.All((char x) => char.IsDigit(x)))
				{
					throw new ArgumentException("The \"tag\" parameter must not be only composed by digit characters");
				}
			}
			this._id = -1;
			this._tag = tag;
			this._tableId = tableId;
			this._infoId = infoId;
			this._awardedUTC = awardedUTC;
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x0012E524 File Offset: 0x0012C724
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Award award = obj as Award;
			return award != null && (this.Tag != null || this.Id != -1) && (award.Tag != null || award.Id != -1) && (this.Tag == award.Tag && this.Id == award.Id && this.TableId == award.TableId && this.InfoId == award.InfoId) && this.AwardedUTC == award.AwardedUTC;
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x0012E5E4 File Offset: 0x0012C7E4
		public override int GetHashCode()
		{
			return ((this.Tag == null) ? 0 : this.Tag.GetHashCode()) ^ ((this.AwardedUTC == null) ? 0 : this.AwardedUTC.GetHashCode()) ^ this.Id ^ this.InfoId ^ this.TableId;
		}

		// Token: 0x040027A0 RID: 10144
		[SerializeField]
		private int _id = -1;

		// Token: 0x040027A1 RID: 10145
		[SerializeField]
		private string _tag;

		// Token: 0x040027A2 RID: 10146
		[SerializeField]
		private int _tableId = -1;

		// Token: 0x040027A3 RID: 10147
		[SerializeField]
		private int _infoId = -1;

		// Token: 0x040027A4 RID: 10148
		private DateTime? _awardedUTC;
	}
}
