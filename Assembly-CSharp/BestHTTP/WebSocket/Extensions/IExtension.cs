using System;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket.Extensions
{
	// Token: 0x0200058F RID: 1423
	public interface IExtension
	{
		// Token: 0x06003430 RID: 13360
		void AddNegotiation(HTTPRequest request);

		// Token: 0x06003431 RID: 13361
		bool ParseNegotiation(WebSocketResponse resp);

		// Token: 0x06003432 RID: 13362
		byte GetFrameHeader(WebSocketFrame writer, byte inFlag);

		// Token: 0x06003433 RID: 13363
		byte[] Encode(WebSocketFrame writer);

		// Token: 0x06003434 RID: 13364
		byte[] Decode(byte header, byte[] data);
	}
}
