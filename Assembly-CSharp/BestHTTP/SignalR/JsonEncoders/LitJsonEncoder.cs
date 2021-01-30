using System;
using System.Collections.Generic;
using LitJson;

namespace BestHTTP.SignalR.JsonEncoders
{
	// Token: 0x020005CC RID: 1484
	public sealed class LitJsonEncoder : IJsonEncoder
	{
		// Token: 0x06003678 RID: 13944 RVA: 0x0010ED94 File Offset: 0x0010CF94
		public string Encode(object obj)
		{
			JsonWriter jsonWriter = new JsonWriter();
			JsonMapper.ToJson(obj, jsonWriter);
			return jsonWriter.ToString();
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x0010EDB4 File Offset: 0x0010CFB4
		public IDictionary<string, object> DecodeMessage(string json)
		{
			return JsonMapper.ToObject<Dictionary<string, object>>(new JsonReader(json));
		}
	}
}
