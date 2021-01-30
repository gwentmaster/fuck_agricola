using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fabric.Answers.Internal
{
	// Token: 0x0200025F RID: 607
	internal class AnswersStubImplementation : IAnswers
	{
		// Token: 0x0600133A RID: 4922 RVA: 0x000726DC File Offset: 0x000708DC
		public AnswersStubImplementation()
		{
			Debug.Log("Answers will no-op because it was initialized for a non-Android, non-Apple platform.");
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00003022 File Offset: 0x00001222
		public void LogSignUp(string method, bool? success, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00003022 File Offset: 0x00001222
		public void LogLogin(string method, bool? success, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00003022 File Offset: 0x00001222
		public void LogShare(string method, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00003022 File Offset: 0x00001222
		public void LogInvite(string method, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00003022 File Offset: 0x00001222
		public void LogLevelStart(string level, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00003022 File Offset: 0x00001222
		public void LogLevelEnd(string level, double? score, bool? success, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00003022 File Offset: 0x00001222
		public void LogAddToCart(decimal? itemPrice, string currency, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00003022 File Offset: 0x00001222
		public void LogPurchase(decimal? price, string currency, bool? success, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00003022 File Offset: 0x00001222
		public void LogStartCheckout(decimal? totalPrice, string currency, int? itemCount, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00003022 File Offset: 0x00001222
		public void LogRating(int? rating, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00003022 File Offset: 0x00001222
		public void LogContentView(string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00003022 File Offset: 0x00001222
		public void LogSearch(string query, Dictionary<string, object> customAttributes)
		{
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00003022 File Offset: 0x00001222
		public void LogCustom(string eventName, Dictionary<string, object> customAttributes)
		{
		}
	}
}
