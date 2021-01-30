using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	// Token: 0x0200074B RID: 1867
	public class AdjustUtils
	{
		// Token: 0x0600417A RID: 16762 RVA: 0x0013B89B File Offset: 0x00139A9B
		public static int ConvertLogLevel(AdjustLogLevel? logLevel)
		{
			if (logLevel == null)
			{
				return -1;
			}
			return (int)logLevel.Value;
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x0013B8AF File Offset: 0x00139AAF
		public static int ConvertBool(bool? value)
		{
			if (value == null)
			{
				return -1;
			}
			if (value.Value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x0013B8C8 File Offset: 0x00139AC8
		public static double ConvertDouble(double? value)
		{
			if (value == null)
			{
				return -1.0;
			}
			return value.Value;
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x0013B8E5 File Offset: 0x00139AE5
		public static long ConvertLong(long? value)
		{
			if (value == null)
			{
				return -1L;
			}
			return value.Value;
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0013B8FC File Offset: 0x00139AFC
		public static string ConvertListToJson(List<string> list)
		{
			if (list == null)
			{
				return null;
			}
			JSONArray jsonarray = new JSONArray();
			foreach (string aData in list)
			{
				jsonarray.Add(new JSONData(aData));
			}
			return jsonarray.ToString();
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x0013B960 File Offset: 0x00139B60
		public static string GetJsonResponseCompact(Dictionary<string, object> dictionary)
		{
			string text = "";
			if (dictionary == null)
			{
				return text;
			}
			int num = 0;
			text += "{";
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				string text2 = keyValuePair.Value as string;
				if (text2 != null)
				{
					if (++num > 1)
					{
						text += ",";
					}
					if (text2.StartsWith("{") && text2.EndsWith("}"))
					{
						text = string.Concat(new string[]
						{
							text,
							"\"",
							keyValuePair.Key,
							"\":",
							text2
						});
					}
					else
					{
						text = string.Concat(new string[]
						{
							text,
							"\"",
							keyValuePair.Key,
							"\":\"",
							text2,
							"\""
						});
					}
				}
				else
				{
					Dictionary<string, object> dictionary2 = keyValuePair.Value as Dictionary<string, object>;
					if (++num > 1)
					{
						text += ",";
					}
					text = text + "\"" + keyValuePair.Key + "\":";
					text += AdjustUtils.GetJsonResponseCompact(dictionary2);
				}
			}
			text += "}";
			return text;
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x0013BADC File Offset: 0x00139CDC
		public static string GetJsonString(JSONNode node, string key)
		{
			if (node == null)
			{
				return null;
			}
			JSONData jsondata = node[key] as JSONData;
			if (jsondata == null)
			{
				return null;
			}
			if (jsondata == "")
			{
				return null;
			}
			return jsondata.Value;
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x0013BB24 File Offset: 0x00139D24
		public static void WriteJsonResponseDictionary(JSONClass jsonObject, Dictionary<string, object> output)
		{
			foreach (object obj in jsonObject)
			{
				KeyValuePair<string, JSONNode> keyValuePair = (KeyValuePair<string, JSONNode>)obj;
				JSONClass asObject = keyValuePair.Value.AsObject;
				string key = keyValuePair.Key;
				if (asObject == null)
				{
					string value = keyValuePair.Value.Value;
					output.Add(key, value);
				}
				else
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					output.Add(key, dictionary);
					AdjustUtils.WriteJsonResponseDictionary(asObject, dictionary);
				}
			}
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x0013BBC4 File Offset: 0x00139DC4
		public static string TryGetValue(Dictionary<string, string> dictionary, string key)
		{
			string text;
			if (!dictionary.TryGetValue(key, out text))
			{
				return null;
			}
			if (text == "")
			{
				return null;
			}
			return text;
		}

		// Token: 0x04002A30 RID: 10800
		public static string KeyAdid = "adid";

		// Token: 0x04002A31 RID: 10801
		public static string KeyMessage = "message";

		// Token: 0x04002A32 RID: 10802
		public static string KeyNetwork = "network";

		// Token: 0x04002A33 RID: 10803
		public static string KeyAdgroup = "adgroup";

		// Token: 0x04002A34 RID: 10804
		public static string KeyCampaign = "campaign";

		// Token: 0x04002A35 RID: 10805
		public static string KeyCreative = "creative";

		// Token: 0x04002A36 RID: 10806
		public static string KeyWillRetry = "willRetry";

		// Token: 0x04002A37 RID: 10807
		public static string KeyTimestamp = "timestamp";

		// Token: 0x04002A38 RID: 10808
		public static string KeyCallbackId = "callbackId";

		// Token: 0x04002A39 RID: 10809
		public static string KeyEventToken = "eventToken";

		// Token: 0x04002A3A RID: 10810
		public static string KeyClickLabel = "clickLabel";

		// Token: 0x04002A3B RID: 10811
		public static string KeyTrackerName = "trackerName";

		// Token: 0x04002A3C RID: 10812
		public static string KeyTrackerToken = "trackerToken";

		// Token: 0x04002A3D RID: 10813
		public static string KeyJsonResponse = "jsonResponse";

		// Token: 0x04002A3E RID: 10814
		public static string KeyTestOptionsBaseUrl = "baseUrl";

		// Token: 0x04002A3F RID: 10815
		public static string KeyTestOptionsGdprUrl = "gdprUrl";

		// Token: 0x04002A40 RID: 10816
		public static string KeyTestOptionsBasePath = "basePath";

		// Token: 0x04002A41 RID: 10817
		public static string KeyTestOptionsGdprPath = "gdprPath";

		// Token: 0x04002A42 RID: 10818
		public static string KeyTestOptionsDeleteState = "deleteState";

		// Token: 0x04002A43 RID: 10819
		public static string KeyTestOptionsUseTestConnectionOptions = "useTestConnectionOptions";

		// Token: 0x04002A44 RID: 10820
		public static string KeyTestOptionsTimerIntervalInMilliseconds = "timerIntervalInMilliseconds";

		// Token: 0x04002A45 RID: 10821
		public static string KeyTestOptionsTimerStartInMilliseconds = "timerStartInMilliseconds";

		// Token: 0x04002A46 RID: 10822
		public static string KeyTestOptionsSessionIntervalInMilliseconds = "sessionIntervalInMilliseconds";

		// Token: 0x04002A47 RID: 10823
		public static string KeyTestOptionsSubsessionIntervalInMilliseconds = "subsessionIntervalInMilliseconds";

		// Token: 0x04002A48 RID: 10824
		public static string KeyTestOptionsTeardown = "teardown";

		// Token: 0x04002A49 RID: 10825
		public static string KeyTestOptionsNoBackoffWait = "noBackoffWait";

		// Token: 0x04002A4A RID: 10826
		public static string KeyTestOptionsiAdFrameworkEnabled = "iAdFrameworkEnabled";
	}
}
