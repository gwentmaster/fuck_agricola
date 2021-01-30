using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x02000144 RID: 324
	public sealed class JsonUtils
	{
		// Token: 0x06000C3A RID: 3130 RVA: 0x000547DC File Offset: 0x000529DC
		public static JSONArray ToJson(ICollection<Button> buttons)
		{
			if (buttons == null || buttons.Count == 0)
			{
				return null;
			}
			JSONArray jsonarray = new JSONArray();
			foreach (Button button in buttons)
			{
				JSONClass jsonclass = new JSONClass();
				jsonclass.Add("title", button.title);
				JSONNode jsonnode = JsonUtils.ToJson(button.userData);
				if (jsonnode != null)
				{
					jsonclass.Add("userData", jsonnode);
				}
				jsonarray.Add(jsonclass);
			}
			return jsonarray;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00054878 File Offset: 0x00052A78
		public static ICollection<Button> ToButtons(JSONNode json)
		{
			if (json == null || !(json is JSONArray) || json.Count <= 0)
			{
				return null;
			}
			List<Button> list = new List<Button>(json.Count);
			for (int i = 0; i < json.Count; i++)
			{
				JSONClass jsonclass = json[i] as JSONClass;
				if (jsonclass != null)
				{
					list.Add(new Button(jsonclass["title"].Value, JsonUtils.ToUserData(jsonclass["userData"])));
				}
			}
			return list;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00054900 File Offset: 0x00052B00
		public static JSONNode ToJson(IDictionary<string, string> userData)
		{
			if (userData == null || userData.Count == 0)
			{
				return null;
			}
			JSONClass jsonclass = new JSONClass();
			foreach (KeyValuePair<string, string> keyValuePair in userData)
			{
				jsonclass.Add(keyValuePair.Key, new JSONData(keyValuePair.Value));
			}
			return jsonclass;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00054970 File Offset: 0x00052B70
		public static IDictionary<string, string> ToUserData(JSONNode json)
		{
			if (json == null || !(json is JSONClass) || json.Count <= 0)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (object obj in ((JSONClass)json))
			{
				KeyValuePair<string, JSONNode> keyValuePair = (KeyValuePair<string, JSONNode>)obj;
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.Value);
			}
			return dictionary;
		}
	}
}
