using System;
using System.Collections.Generic;
using Fabric.Answers.Internal;
using UnityEngine;

namespace Fabric.Answers
{
	// Token: 0x0200025E RID: 606
	public class Answers : MonoBehaviour
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x0007255C File Offset: 0x0007075C
		private static IAnswers Implementation
		{
			get
			{
				if (Answers.implementation == null)
				{
					Answers.implementation = new AnswersStubImplementation();
				}
				return Answers.implementation;
			}
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00072574 File Offset: 0x00070774
		public static void LogSignUp(string method = null, bool? success = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogSignUp(method, success, customAttributes);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0007258D File Offset: 0x0007078D
		public static void LogLogin(string method = null, bool? success = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogLogin(method, success, customAttributes);
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000725A6 File Offset: 0x000707A6
		public static void LogShare(string method = null, string contentName = null, string contentType = null, string contentId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogShare(method, contentName, contentType, contentId, customAttributes);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000725C3 File Offset: 0x000707C3
		public static void LogInvite(string method = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogInvite(method, customAttributes);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000725DB File Offset: 0x000707DB
		public static void LogLevelStart(string level = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogLevelStart(level, customAttributes);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000725F3 File Offset: 0x000707F3
		public static void LogLevelEnd(string level = null, double? score = null, bool? success = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogLevelEnd(level, score, success, customAttributes);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0007260D File Offset: 0x0007080D
		public static void LogAddToCart(decimal? itemPrice = null, string currency = null, string itemName = null, string itemType = null, string itemId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogAddToCart(itemPrice, currency, itemName, itemType, itemId, customAttributes);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0007262C File Offset: 0x0007082C
		public static void LogPurchase(decimal? price = null, string currency = null, bool? success = null, string itemName = null, string itemType = null, string itemId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogPurchase(price, currency, success, itemName, itemType, itemId, customAttributes);
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0007264D File Offset: 0x0007084D
		public static void LogStartCheckout(decimal? totalPrice = null, string currency = null, int? itemCount = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogStartCheckout(totalPrice, currency, itemCount, customAttributes);
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00072667 File Offset: 0x00070867
		public static void LogRating(int? rating = null, string contentName = null, string contentType = null, string contentId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogRating(rating, contentName, contentType, contentId, customAttributes);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00072684 File Offset: 0x00070884
		public static void LogContentView(string contentName = null, string contentType = null, string contentId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogContentView(contentName, contentType, contentId, customAttributes);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0007269E File Offset: 0x0007089E
		public static void LogSearch(string query = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogSearch(query, customAttributes);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000726B6 File Offset: 0x000708B6
		public static void LogCustom(string eventName, Dictionary<string, object> customAttributes = null)
		{
			if (eventName == null)
			{
				Debug.Log("Answers' Custom Events require event names. Skipping this event because its name is null.");
				return;
			}
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogCustom(eventName, customAttributes);
		}

		// Token: 0x040012F3 RID: 4851
		private static IAnswers implementation;
	}
}
