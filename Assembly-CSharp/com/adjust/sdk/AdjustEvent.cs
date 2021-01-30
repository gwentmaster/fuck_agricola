using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	// Token: 0x02000744 RID: 1860
	public class AdjustEvent
	{
		// Token: 0x06004131 RID: 16689 RVA: 0x0013AF02 File Offset: 0x00139102
		public AdjustEvent(string eventToken)
		{
			this.eventToken = eventToken;
			this.isReceiptSet = false;
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x0013AF18 File Offset: 0x00139118
		public void setRevenue(double amount, string currency)
		{
			this.revenue = new double?(amount);
			this.currency = currency;
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x0013AF2D File Offset: 0x0013912D
		public void addCallbackParameter(string key, string value)
		{
			if (this.callbackList == null)
			{
				this.callbackList = new List<string>();
			}
			this.callbackList.Add(key);
			this.callbackList.Add(value);
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x0013AF5A File Offset: 0x0013915A
		public void addPartnerParameter(string key, string value)
		{
			if (this.partnerList == null)
			{
				this.partnerList = new List<string>();
			}
			this.partnerList.Add(key);
			this.partnerList.Add(value);
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x0013AF87 File Offset: 0x00139187
		public void setTransactionId(string transactionId)
		{
			this.transactionId = transactionId;
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x0013AF90 File Offset: 0x00139190
		public void setCallbackId(string callbackId)
		{
			this.callbackId = callbackId;
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x0013AF99 File Offset: 0x00139199
		[Obsolete("This is an obsolete method. Please use the adjust purchase SDK for purchase verification (https://github.com/adjust/unity_purchase_sdk)")]
		public void setReceipt(string receipt, string transactionId)
		{
			this.receipt = receipt;
			this.transactionId = transactionId;
			this.isReceiptSet = true;
		}

		// Token: 0x04002A09 RID: 10761
		internal string currency;

		// Token: 0x04002A0A RID: 10762
		internal string eventToken;

		// Token: 0x04002A0B RID: 10763
		internal string callbackId;

		// Token: 0x04002A0C RID: 10764
		internal string transactionId;

		// Token: 0x04002A0D RID: 10765
		internal double? revenue;

		// Token: 0x04002A0E RID: 10766
		internal List<string> partnerList;

		// Token: 0x04002A0F RID: 10767
		internal List<string> callbackList;

		// Token: 0x04002A10 RID: 10768
		internal string receipt;

		// Token: 0x04002A11 RID: 10769
		internal bool isReceiptSet;
	}
}
