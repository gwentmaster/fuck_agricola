using System;
using System.Text.RegularExpressions;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000664 RID: 1636
	public static class EmailFormatValidator
	{
		// Token: 0x06003C77 RID: 15479 RVA: 0x0012ACA4 File Offset: 0x00128EA4
		public static bool IsValidEmail(string emailCandidate)
		{
			return Regex.IsMatch(emailCandidate, "^(?(\")(\"[^\"]+?\"@)|(([0-9a-zA-Z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-zA-Z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,6}))$");
		}
	}
}
