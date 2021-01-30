using System;

namespace BestHTTP.SocketIO.Events
{
	// Token: 0x020005A9 RID: 1449
	public static class EventNames
	{
		// Token: 0x06003549 RID: 13641 RVA: 0x0010B23C File Offset: 0x0010943C
		public static string GetNameFor(SocketIOEventTypes type)
		{
			return EventNames.SocketIONames[(int)(type + 1)];
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x0010B247 File Offset: 0x00109447
		public static string GetNameFor(TransportEventTypes transEvent)
		{
			return EventNames.TransportNames[(int)(transEvent + 1)];
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x0010B254 File Offset: 0x00109454
		public static bool IsBlacklisted(string eventName)
		{
			for (int i = 0; i < EventNames.BlacklistedEvents.Length; i++)
			{
				if (string.Compare(EventNames.BlacklistedEvents[i], eventName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040022BB RID: 8891
		public const string Connect = "connect";

		// Token: 0x040022BC RID: 8892
		public const string Disconnect = "disconnect";

		// Token: 0x040022BD RID: 8893
		public const string Event = "event";

		// Token: 0x040022BE RID: 8894
		public const string Ack = "ack";

		// Token: 0x040022BF RID: 8895
		public const string Error = "error";

		// Token: 0x040022C0 RID: 8896
		public const string BinaryEvent = "binaryevent";

		// Token: 0x040022C1 RID: 8897
		public const string BinaryAck = "binaryack";

		// Token: 0x040022C2 RID: 8898
		private static string[] SocketIONames = new string[]
		{
			"unknown",
			"connect",
			"disconnect",
			"event",
			"ack",
			"error",
			"binaryevent",
			"binaryack"
		};

		// Token: 0x040022C3 RID: 8899
		private static string[] TransportNames = new string[]
		{
			"unknown",
			"open",
			"close",
			"ping",
			"pong",
			"message",
			"upgrade",
			"noop"
		};

		// Token: 0x040022C4 RID: 8900
		private static string[] BlacklistedEvents = new string[]
		{
			"connect",
			"connect_error",
			"connect_timeout",
			"disconnect",
			"error",
			"reconnect",
			"reconnect_attempt",
			"reconnect_failed",
			"reconnect_error",
			"reconnecting"
		};
	}
}
