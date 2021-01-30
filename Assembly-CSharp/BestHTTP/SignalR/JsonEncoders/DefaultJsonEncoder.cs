using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SignalR.JsonEncoders
{
	// Token: 0x020005CA RID: 1482
	public sealed class DefaultJsonEncoder : IJsonEncoder
	{
		// Token: 0x06003673 RID: 13939 RVA: 0x0010B0B7 File Offset: 0x001092B7
		public string Encode(object obj)
		{
			return Json.Encode(obj);
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x0010ED70 File Offset: 0x0010CF70
		public IDictionary<string, object> DecodeMessage(string json)
		{
			bool flag = false;
			IDictionary<string, object> result = Json.Decode(json, ref flag) as IDictionary<string, object>;
			if (!flag)
			{
				return null;
			}
			return result;
		}
	}
}
