using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000719 RID: 1817
	public struct SHOP_ITEMFOCUS
	{
		// Token: 0x0400294B RID: 10571
		public string shop_session_id;

		// Token: 0x0400294C RID: 10572
		public string entry_point;

		// Token: 0x0400294D RID: 10573
		public string item_id;

		// Token: 0x0400294E RID: 10574
		public float item_price;

		// Token: 0x0400294F RID: 10575
		public string item_currency;

		// Token: 0x04002950 RID: 10576
		public int item_quantity;

		// Token: 0x04002951 RID: 10577
		public bool item_view;

		// Token: 0x04002952 RID: 10578
		public bool item_purchase;

		// Token: 0x04002953 RID: 10579
		public bool item_purchase_confirmed_user;

		// Token: 0x04002954 RID: 10580
		public bool item_purchase_confirmed_first_party;

		// Token: 0x04002955 RID: 10581
		public string transaction_backend_id;

		// Token: 0x04002956 RID: 10582
		public string transaction_first_party_id;

		// Token: 0x04002957 RID: 10583
		public float transaction_price;

		// Token: 0x04002958 RID: 10584
		public string transaction_currency;

		// Token: 0x04002959 RID: 10585
		public bool purchases_outside_shop;

		// Token: 0x0400295A RID: 10586
		public bool? is_default_item;
	}
}
