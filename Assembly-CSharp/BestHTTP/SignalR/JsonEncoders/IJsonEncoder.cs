using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.JsonEncoders
{
	// Token: 0x020005CB RID: 1483
	public interface IJsonEncoder
	{
		// Token: 0x06003676 RID: 13942
		string Encode(object obj);

		// Token: 0x06003677 RID: 13943
		IDictionary<string, object> DecodeMessage(string json);
	}
}
