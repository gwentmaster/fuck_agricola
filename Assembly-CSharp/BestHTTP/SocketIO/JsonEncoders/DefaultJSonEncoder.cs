using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SocketIO.JsonEncoders
{
	// Token: 0x020005A3 RID: 1443
	public sealed class DefaultJSonEncoder : IJsonEncoder
	{
		// Token: 0x06003531 RID: 13617 RVA: 0x0010B0AA File Offset: 0x001092AA
		public List<object> Decode(string json)
		{
			return Json.Decode(json) as List<object>;
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x0010B0B7 File Offset: 0x001092B7
		public string Encode(List<object> obj)
		{
			return Json.Encode(obj);
		}
	}
}
