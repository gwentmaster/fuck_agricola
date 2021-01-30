using System;
using System.Collections.Generic;

namespace Fabric.Answers.Internal
{
	// Token: 0x02000260 RID: 608
	internal interface IAnswers
	{
		// Token: 0x06001348 RID: 4936
		void LogSignUp(string method, bool? success, Dictionary<string, object> customAttributes);

		// Token: 0x06001349 RID: 4937
		void LogLogin(string method, bool? success, Dictionary<string, object> customAttributes);

		// Token: 0x0600134A RID: 4938
		void LogShare(string method, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes);

		// Token: 0x0600134B RID: 4939
		void LogInvite(string method, Dictionary<string, object> customAttributes);

		// Token: 0x0600134C RID: 4940
		void LogLevelStart(string level, Dictionary<string, object> customAttributes);

		// Token: 0x0600134D RID: 4941
		void LogLevelEnd(string level, double? score, bool? success, Dictionary<string, object> customAttributes);

		// Token: 0x0600134E RID: 4942
		void LogPurchase(decimal? price, string currency, bool? success, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes);

		// Token: 0x0600134F RID: 4943
		void LogAddToCart(decimal? itemPrice, string currency, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes);

		// Token: 0x06001350 RID: 4944
		void LogStartCheckout(decimal? totalPrice, string currency, int? itemCount, Dictionary<string, object> customAttributes);

		// Token: 0x06001351 RID: 4945
		void LogRating(int? rating, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes);

		// Token: 0x06001352 RID: 4946
		void LogContentView(string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes);

		// Token: 0x06001353 RID: 4947
		void LogSearch(string query, Dictionary<string, object> customAttributes);

		// Token: 0x06001354 RID: 4948
		void LogCustom(string eventName, Dictionary<string, object> customAttributes);
	}
}
