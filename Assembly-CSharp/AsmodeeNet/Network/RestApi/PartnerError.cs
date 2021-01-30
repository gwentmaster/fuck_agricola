using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006BA RID: 1722
	[Serializable]
	public class PartnerError
	{
		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x0012EDC3 File Offset: 0x0012CFC3
		public PartnerAccount Partner
		{
			get
			{
				return this._partner;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003DC1 RID: 15809 RVA: 0x0012EDCB File Offset: 0x0012CFCB
		public string ApiErrorCode
		{
			get
			{
				return this._apiErrorCode;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x0012EDD3 File Offset: 0x0012CFD3
		public string ApiErrorDescription
		{
			get
			{
				return this._apiErrorDescription;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06003DC3 RID: 15811 RVA: 0x0012EDDB File Offset: 0x0012CFDB
		public PartnerError.ExtraDetails ExtraDetailsForAdd
		{
			get
			{
				return this._extraDetails;
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x0012EDE3 File Offset: 0x0012CFE3
		public PartnerError(PartnerAccount partnerAccount, string ApiErrorCode, string ApiErrorDescription, PartnerError.ExtraDetails extraDetails)
		{
			this._partner = partnerAccount;
			this._apiErrorCode = ApiErrorCode;
			this._apiErrorDescription = ApiErrorDescription;
			this._extraDetails = extraDetails;
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x0012EE08 File Offset: 0x0012D008
		public static List<PartnerError> ExtractRemovePartnerError(ApiResponseLinkUnlinkMultipleError.Details.RemoveDetails[] details)
		{
			if (details != null)
			{
				return (from x in details
				select new PartnerError(new PartnerAccount(x.partner, x.partner_user, null), x.error_code, x.error_description, null)).ToList<PartnerError>();
			}
			return null;
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x0012EE39 File Offset: 0x0012D039
		public static List<PartnerError> ExtractAddPartnerError(ApiResponseLinkUnlinkMultipleError.Details.AddDetails[] details)
		{
			if (details != null)
			{
				return (from x in details
				select new PartnerError(new PartnerAccount(x.partner, x.partner_user, null), x.error_code, x.error_description, (x.error_details == null) ? null : new PartnerError.ExtraDetails(x.error_details.conflict_id, x.error_details.conflict_login))).ToList<PartnerError>();
			}
			return null;
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x0012EE6C File Offset: 0x0012D06C
		public override bool Equals(object o)
		{
			PartnerError partnerError = o as PartnerError;
			return partnerError != null && (this.Partner.Equals(partnerError.Partner) && this.ApiErrorCode == partnerError.ApiErrorCode && this.ApiErrorDescription == partnerError.ApiErrorDescription) && ((this.ExtraDetailsForAdd == null && partnerError.ExtraDetailsForAdd == null) || this.ExtraDetailsForAdd.Equals(partnerError.ExtraDetailsForAdd));
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x0012EEE4 File Offset: 0x0012D0E4
		public override int GetHashCode()
		{
			return ((this.Partner == null) ? 0 : this.Partner.GetHashCode()) ^ ((this.ApiErrorCode == null) ? 0 : this.ApiErrorCode.GetHashCode()) ^ ((this.ApiErrorDescription == null) ? 0 : this.ApiErrorDescription.GetHashCode()) ^ ((this.ExtraDetailsForAdd == null) ? 0 : this.ExtraDetailsForAdd.GetHashCode());
		}

		// Token: 0x040027D8 RID: 10200
		[SerializeField]
		private PartnerAccount _partner;

		// Token: 0x040027D9 RID: 10201
		[SerializeField]
		private string _apiErrorCode;

		// Token: 0x040027DA RID: 10202
		[SerializeField]
		private string _apiErrorDescription;

		// Token: 0x040027DB RID: 10203
		[SerializeField]
		private PartnerError.ExtraDetails _extraDetails;

		// Token: 0x020009BD RID: 2493
		[Serializable]
		public class ExtraDetails
		{
			// Token: 0x17000A69 RID: 2665
			// (get) Token: 0x060048B1 RID: 18609 RVA: 0x0014C7C9 File Offset: 0x0014A9C9
			public string ConflictLogin
			{
				get
				{
					return this._conflictLogin;
				}
			}

			// Token: 0x17000A6A RID: 2666
			// (get) Token: 0x060048B2 RID: 18610 RVA: 0x0014C7D1 File Offset: 0x0014A9D1
			public int ConflictId
			{
				get
				{
					return this._conflictId;
				}
			}

			// Token: 0x060048B3 RID: 18611 RVA: 0x0014C7D9 File Offset: 0x0014A9D9
			public ExtraDetails(int conflictId, string conflictLogin)
			{
				this._conflictId = conflictId;
				this._conflictLogin = conflictLogin;
			}

			// Token: 0x060048B4 RID: 18612 RVA: 0x0014C7F0 File Offset: 0x0014A9F0
			public override bool Equals(object obj)
			{
				PartnerError.ExtraDetails extraDetails = obj as PartnerError.ExtraDetails;
				return extraDetails != null && this.ConflictId == extraDetails.ConflictId && this.ConflictLogin == extraDetails.ConflictLogin;
			}

			// Token: 0x060048B5 RID: 18613 RVA: 0x0014C82A File Offset: 0x0014AA2A
			public override int GetHashCode()
			{
				return this.ConflictId ^ ((this.ConflictLogin == null) ? 0 : this.ConflictLogin.GetHashCode());
			}

			// Token: 0x040032E3 RID: 13027
			[SerializeField]
			private string _conflictLogin;

			// Token: 0x040032E4 RID: 13028
			[SerializeField]
			private int _conflictId;
		}
	}
}
