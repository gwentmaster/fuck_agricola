using System;
using System.IO;
using BestHTTP.ServerSentEvents;
using BestHTTP.WebSocket;

namespace BestHTTP
{
	// Token: 0x0200056D RID: 1389
	internal static class HTTPProtocolFactory
	{
		// Token: 0x06003277 RID: 12919 RVA: 0x00101C28 File Offset: 0x000FFE28
		public static HTTPResponse Get(SupportedProtocols protocol, HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
		{
			if (protocol == SupportedProtocols.WebSocket)
			{
				return new WebSocketResponse(request, stream, isStreamed, isFromCache);
			}
			if (protocol != SupportedProtocols.ServerSentEvents)
			{
				return new HTTPResponse(request, stream, isStreamed, isFromCache);
			}
			return new EventSourceResponse(request, stream, isStreamed, isFromCache);
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x00101C54 File Offset: 0x000FFE54
		public static SupportedProtocols GetProtocolFromUri(Uri uri)
		{
			if (uri == null || uri.Scheme == null)
			{
				throw new Exception("Malformed URI in GetProtocolFromUri");
			}
			string a = uri.Scheme.ToLowerInvariant();
			if (a == "ws" || a == "wss")
			{
				return SupportedProtocols.WebSocket;
			}
			return SupportedProtocols.HTTP;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x00101CA8 File Offset: 0x000FFEA8
		public static bool IsSecureProtocol(Uri uri)
		{
			if (uri == null || uri.Scheme == null)
			{
				throw new Exception("Malformed URI in IsSecureProtocol");
			}
			string a = uri.Scheme.ToLowerInvariant();
			return a == "https" || a == "wss";
		}
	}
}
