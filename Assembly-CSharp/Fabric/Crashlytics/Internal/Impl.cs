using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Fabric.Internal.Runtime;

namespace Fabric.Crashlytics.Internal
{
	// Token: 0x0200025D RID: 605
	internal class Impl
	{
		// Token: 0x0600131C RID: 4892 RVA: 0x0007222F File Offset: 0x0007042F
		public static Impl Make()
		{
			return new Impl();
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void SetDebugMode(bool mode)
		{
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00072236 File Offset: 0x00070436
		public virtual void Crash()
		{
			Utils.Log("Crashlytics", "Method Crash () is unimplemented on this platform");
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00072248 File Offset: 0x00070448
		public virtual void ThrowNonFatal()
		{
			string message = ((string)null).ToLower();
			Utils.Log("Crashlytics", message);
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00072267 File Offset: 0x00070467
		public virtual void Log(string message)
		{
			Utils.Log("Crashlytics", "Would log custom message if running on a physical device: " + message);
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0007227E File Offset: 0x0007047E
		public virtual void SetKeyValue(string key, string value)
		{
			Utils.Log("Crashlytics", "Would set key-value if running on a physical device: " + key + ":" + value);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0007229B File Offset: 0x0007049B
		public virtual void SetUserIdentifier(string identifier)
		{
			Utils.Log("Crashlytics", "Would set user identifier if running on a physical device: " + identifier);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x000722B2 File Offset: 0x000704B2
		public virtual void SetUserEmail(string email)
		{
			Utils.Log("Crashlytics", "Would set user email if running on a physical device: " + email);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x000722C9 File Offset: 0x000704C9
		public virtual void SetUserName(string name)
		{
			Utils.Log("Crashlytics", "Would set user name if running on a physical device: " + name);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x000722E0 File Offset: 0x000704E0
		public virtual void RecordCustomException(string name, string reason, StackTrace stackTrace)
		{
			Utils.Log("Crashlytics", "Would record custom exception if running on a physical device: " + name + ", " + reason);
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000722E0 File Offset: 0x000704E0
		public virtual void RecordCustomException(string name, string reason, string stackTraceString)
		{
			Utils.Log("Crashlytics", "Would record custom exception if running on a physical device: " + name + ", " + reason);
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00072300 File Offset: 0x00070500
		private static Dictionary<string, string> ParseFrameString(string regex, string frameString)
		{
			MatchCollection matchCollection = Regex.Matches(frameString, regex);
			if (matchCollection.Count < 1)
			{
				return null;
			}
			Match match = matchCollection[0];
			if (!match.Groups["class"].Success || !match.Groups["method"].Success)
			{
				return null;
			}
			string text = match.Groups["file"].Success ? match.Groups["file"].Value : match.Groups["class"].Value;
			string value = match.Groups["line"].Success ? match.Groups["line"].Value : "0";
			if (text == Impl.MonoFilenameUnknownString)
			{
				text = match.Groups["class"].Value;
				value = "0";
			}
			return new Dictionary<string, string>
			{
				{
					"class",
					match.Groups["class"].Value
				},
				{
					"method",
					match.Groups["method"].Value
				},
				{
					"file",
					text
				},
				{
					"line",
					value
				}
			};
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x00072458 File Offset: 0x00070658
		protected static Dictionary<string, string>[] ParseStackTraceString(string stackTraceString)
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			string[] array = stackTraceString.Split(Impl.StringDelimiters, StringSplitOptions.None);
			if (array.Length < 1)
			{
				return list.ToArray();
			}
			string regex;
			if (Regex.Matches(array[0], Impl.FrameRegexWithFileInfo).Count == 1)
			{
				regex = Impl.FrameRegexWithFileInfo;
			}
			else
			{
				if (Regex.Matches(array[0], Impl.FrameRegexWithoutFileInfo).Count != 1)
				{
					return list.ToArray();
				}
				regex = Impl.FrameRegexWithoutFileInfo;
			}
			foreach (string frameString in array)
			{
				Dictionary<string, string> dictionary = Impl.ParseFrameString(regex, frameString);
				if (dictionary != null)
				{
					list.Add(dictionary);
				}
			}
			return list.ToArray();
		}

		// Token: 0x040012ED RID: 4845
		protected const string KitName = "Crashlytics";

		// Token: 0x040012EE RID: 4846
		private static readonly string FrameArgsRegex = "\\s?\\(.*\\)";

		// Token: 0x040012EF RID: 4847
		private static readonly string FrameRegexWithoutFileInfo = "(?<class>[^\\s]+)\\.(?<method>[^\\s\\.]+)" + Impl.FrameArgsRegex;

		// Token: 0x040012F0 RID: 4848
		private static readonly string FrameRegexWithFileInfo = Impl.FrameRegexWithoutFileInfo + " .*[/|\\\\](?<file>.+):(?<line>\\d+)";

		// Token: 0x040012F1 RID: 4849
		private static readonly string MonoFilenameUnknownString = "<filename unknown>";

		// Token: 0x040012F2 RID: 4850
		private static readonly string[] StringDelimiters = new string[]
		{
			Environment.NewLine
		};

		// Token: 0x0200087A RID: 2170
		// (Invoke) Token: 0x0600452D RID: 17709
		private delegate Dictionary<string, string> FrameParser(string frameString);
	}
}
