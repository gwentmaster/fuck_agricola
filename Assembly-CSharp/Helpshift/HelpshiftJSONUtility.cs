using System;
using System.Collections.Generic;
using HSMiniJSON;

namespace Helpshift
{
	// Token: 0x02000253 RID: 595
	public static class HelpshiftJSONUtility
	{
		// Token: 0x060012F7 RID: 4855 RVA: 0x00071DF0 File Offset: 0x0006FFF0
		public static HelpshiftUser getHelpshiftUser(string serializedJSONHelpshiftUser)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(serializedJSONHelpshiftUser);
			string identifier = Convert.ToString(dictionary["identifier"]);
			string email = Convert.ToString(dictionary["email"]);
			string authToken = Convert.ToString(dictionary["authToken"]);
			string name = Convert.ToString(dictionary["name"]);
			HelpshiftUser.Builder builder = new HelpshiftUser.Builder(identifier, email);
			builder.setName(name);
			builder.setAuthToken(authToken);
			return builder.build();
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00071E68 File Offset: 0x00070068
		public static HelpshiftAuthFailureReason getAuthFailureReason(string serializedJSONAuthFailure)
		{
			string value = Convert.ToString(((Dictionary<string, object>)Json.Deserialize(serializedJSONAuthFailure))["authFailureReason"]);
			HelpshiftAuthFailureReason result = HelpshiftAuthFailureReason.INVALID_AUTH_TOKEN;
			if ("0".Equals(value))
			{
				result = HelpshiftAuthFailureReason.AUTH_TOKEN_NOT_PROVIDED;
			}
			else if ("1".Equals(value))
			{
				result = HelpshiftAuthFailureReason.INVALID_AUTH_TOKEN;
			}
			return result;
		}
	}
}
