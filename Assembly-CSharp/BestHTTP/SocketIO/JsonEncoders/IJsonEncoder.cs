using System;
using System.Collections.Generic;

namespace BestHTTP.SocketIO.JsonEncoders
{
	// Token: 0x020005A4 RID: 1444
	public interface IJsonEncoder
	{
		// Token: 0x06003534 RID: 13620
		List<object> Decode(string json);

		// Token: 0x06003535 RID: 13621
		string Encode(List<object> obj);
	}
}
