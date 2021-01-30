using System;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006B9 RID: 1721
	[Serializable]
	public class PartnerAccount
	{
		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x0012ED11 File Offset: 0x0012CF11
		public int PartnerId
		{
			get
			{
				return this._partnerId;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x0012ED19 File Offset: 0x0012CF19
		public string PartnerUser
		{
			get
			{
				return this._partnerUser;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x0012ED21 File Offset: 0x0012CF21
		public DateTime? CreatedAt
		{
			get
			{
				return this._createdAt;
			}
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00003425 File Offset: 0x00001625
		public PartnerAccount()
		{
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x0012ED29 File Offset: 0x0012CF29
		public PartnerAccount(int partnerId, string partnerUser, DateTime? createdAt = null)
		{
			this._partnerId = partnerId;
			this._partnerUser = partnerUser;
			this._createdAt = createdAt;
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x0012ED48 File Offset: 0x0012CF48
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			PartnerAccount partnerAccount = obj as PartnerAccount;
			return partnerAccount != null && this.PartnerId == partnerAccount.PartnerId && this.PartnerUser == partnerAccount.PartnerUser;
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x0012ED87 File Offset: 0x0012CF87
		public override int GetHashCode()
		{
			return this._partnerId ^ ((this._partnerUser == null) ? 0 : this._partnerUser.GetHashCode());
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0012EDA6 File Offset: 0x0012CFA6
		public override string ToString()
		{
			return string.Format("Partner({0}, {1})", this._partnerId, this._partnerUser);
		}

		// Token: 0x040027D4 RID: 10196
		public const int kSteamPartner = 12;

		// Token: 0x040027D5 RID: 10197
		[SerializeField]
		private int _partnerId;

		// Token: 0x040027D6 RID: 10198
		[SerializeField]
		private string _partnerUser;

		// Token: 0x040027D7 RID: 10199
		[SerializeField]
		private DateTime? _createdAt;
	}
}
