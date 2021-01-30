using System;
using System.Collections.Generic;

namespace Helpshift
{
	// Token: 0x02000250 RID: 592
	public class HelpshiftSdk
	{
		// Token: 0x060012D8 RID: 4824 RVA: 0x00003425 File Offset: 0x00001625
		private HelpshiftSdk()
		{
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0000301F File Offset: 0x0000121F
		public static HelpshiftSdk getInstance()
		{
			return null;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00071DE3 File Offset: 0x0006FFE3
		public void install(string apiKey, string domainName, string appId, Dictionary<string, object> config = null)
		{
			if (config == null)
			{
				config = new Dictionary<string, object>();
			}
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00003022 File Offset: 0x00001222
		public void requestUnreadMessagesCount(bool isAsync)
		{
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00003022 File Offset: 0x00001222
		[Obsolete]
		public void setNameAndEmail(string userName, string email)
		{
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00003022 File Offset: 0x00001222
		[Obsolete]
		public void setUserIdentifier(string identifier)
		{
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00003022 File Offset: 0x00001222
		[Obsolete("Use the login(HelpshiftUser user) api instead.")]
		public void login(string identifier, string name, string email)
		{
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00003022 File Offset: 0x00001222
		public void login(HelpshiftUser helpshiftUser)
		{
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00003022 File Offset: 0x00001222
		public void clearAnonymousUser()
		{
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00003022 File Offset: 0x00001222
		public void logout()
		{
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00003022 File Offset: 0x00001222
		public void registerDeviceToken(string deviceToken)
		{
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00003022 File Offset: 0x00001222
		public void leaveBreadCrumb(string breadCrumb)
		{
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00003022 File Offset: 0x00001222
		public void clearBreadCrumbs()
		{
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00003022 File Offset: 0x00001222
		public void showConversation(Dictionary<string, object> configMap = null)
		{
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00003022 File Offset: 0x00001222
		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap = null)
		{
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00003022 File Offset: 0x00001222
		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap = null)
		{
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00003022 File Offset: 0x00001222
		public void showFAQs(Dictionary<string, object> configMap = null)
		{
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00003022 File Offset: 0x00001222
		public void updateMetaData(Dictionary<string, object> metaData)
		{
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00003022 File Offset: 0x00001222
		public void handlePushNotification(Dictionary<string, object> pushNotificationData)
		{
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00003022 File Offset: 0x00001222
		public void showAlertToRateAppWithURL(string url)
		{
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00003022 File Offset: 0x00001222
		public void setSDKLanguage(string locale)
		{
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00003022 File Offset: 0x00001222
		public void setTheme(string themeName)
		{
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00003022 File Offset: 0x00001222
		public void showDynamicForm(string title, Dictionary<string, object>[] flows)
		{
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00003022 File Offset: 0x00001222
		public void showDynamicForm(string title, Dictionary<string, object>[] flows, Dictionary<string, object> configMap = null)
		{
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00003022 File Offset: 0x00001222
		public void checkIfConversationActive()
		{
		}

		// Token: 0x040012BF RID: 4799
		public const string HS_RATE_ALERT_CLOSE = "HS_RATE_ALERT_CLOSE";

		// Token: 0x040012C0 RID: 4800
		public const string HS_RATE_ALERT_FEEDBACK = "HS_RATE_ALERT_FEEDBACK";

		// Token: 0x040012C1 RID: 4801
		public const string HS_RATE_ALERT_SUCCESS = "HS_RATE_ALERT_SUCCESS";

		// Token: 0x040012C2 RID: 4802
		public const string HS_RATE_ALERT_FAIL = "HS_RATE_ALERT_FAIL";

		// Token: 0x040012C3 RID: 4803
		public const string HSTAGSKEY = "hs-tags";

		// Token: 0x040012C4 RID: 4804
		public const string HSCUSTOMMETADATAKEY = "hs-custom-metadata";

		// Token: 0x040012C5 RID: 4805
		public const string UNITY_GAME_OBJECT = "unityGameObject";

		// Token: 0x040012C6 RID: 4806
		public const string ENABLE_IN_APP_NOTIFICATION = "enableInAppNotification";

		// Token: 0x040012C7 RID: 4807
		public const string ENABLE_DEFAULT_FALLBACK_LANGUAGE = "enableDefaultFallbackLanguage";

		// Token: 0x040012C8 RID: 4808
		public const string ENABLE_LOGGING = "enableLogging";

		// Token: 0x040012C9 RID: 4809
		public const string ENABLE_INBOX_POLLING = "enableInboxPolling";

		// Token: 0x040012CA RID: 4810
		public const string ENABLE_AUTOMATIC_THEME_SWITCHING = "enableAutomaticThemeSwitching";

		// Token: 0x040012CB RID: 4811
		public const string DISABLE_ENTRY_EXIT_ANIMATIONS = "disableEntryExitAnimations";

		// Token: 0x040012CC RID: 4812
		public const string DISABLE_ERROR_REPORTING = "disableErrorReporting";

		// Token: 0x040012CD RID: 4813
		public const string HSTAGSMATCHINGKEY = "withTagsMatching";

		// Token: 0x040012CE RID: 4814
		public const string CONTACT_US_ALWAYS = "always";

		// Token: 0x040012CF RID: 4815
		public const string CONTACT_US_NEVER = "never";

		// Token: 0x040012D0 RID: 4816
		public const string CONTACT_US_AFTER_VIEWING_FAQS = "after_viewing_faqs";

		// Token: 0x040012D1 RID: 4817
		public const string CONTACT_US_AFTER_MARKING_ANSWER_UNHELPFUL = "after_marking_answer_unhelpful";

		// Token: 0x040012D2 RID: 4818
		public const string HSUserAcceptedTheSolution = "User accepted the solution";

		// Token: 0x040012D3 RID: 4819
		public const string HSUserRejectedTheSolution = "User rejected the solution";

		// Token: 0x040012D4 RID: 4820
		public const string HSUserSentScreenShot = "User sent a screenshot";

		// Token: 0x040012D5 RID: 4821
		public const string HSUserReviewedTheApp = "User reviewed the app";

		// Token: 0x040012D6 RID: 4822
		public const string HsFlowTypeDefault = "defaultFlow";

		// Token: 0x040012D7 RID: 4823
		public const string HsFlowTypeConversation = "conversationFlow";

		// Token: 0x040012D8 RID: 4824
		public const string HsFlowTypeFaqs = "faqsFlow";

		// Token: 0x040012D9 RID: 4825
		public const string HsFlowTypeFaqSection = "faqSectionFlow";

		// Token: 0x040012DA RID: 4826
		public const string HsFlowTypeSingleFaq = "singleFaqFlow";

		// Token: 0x040012DB RID: 4827
		public const string HsFlowTypeNested = "dynamicFormFlow";

		// Token: 0x040012DC RID: 4828
		public const string HsCustomContactUsFlows = "customContactUsFlows";

		// Token: 0x040012DD RID: 4829
		public const string HsFlowType = "type";

		// Token: 0x040012DE RID: 4830
		public const string HsFlowConfig = "config";

		// Token: 0x040012DF RID: 4831
		public const string HsFlowData = "data";

		// Token: 0x040012E0 RID: 4832
		public const string HsFlowTitle = "title";
	}
}
