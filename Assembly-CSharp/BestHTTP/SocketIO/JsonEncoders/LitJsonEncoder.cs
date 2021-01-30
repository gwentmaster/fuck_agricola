using System;
using System.Collections.Generic;
using LitJson;

namespace BestHTTP.SocketIO.JsonEncoders
{
	// Token: 0x020005A5 RID: 1445
	public sealed class LitJsonEncoder : IJsonEncoder
	{
		// Token: 0x06003536 RID: 13622 RVA: 0x0010B0BF File Offset: 0x001092BF
		public List<object> Decode(string json)
		{
			return JsonMapper.ToObject<List<object>>(new JsonReader(json));
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x0010B0CC File Offset: 0x001092CC
		public string Encode(List<object> obj)
		{
			JsonWriter jsonWriter = new JsonWriter();
			JsonMapper.ToJson(obj, jsonWriter);
			return jsonWriter.ToString();
		}
	}
}
