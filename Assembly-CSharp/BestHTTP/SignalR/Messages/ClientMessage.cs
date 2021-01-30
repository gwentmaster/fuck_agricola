using System;
using BestHTTP.SignalR.Hubs;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C0 RID: 1472
	public struct ClientMessage
	{
		// Token: 0x0600362E RID: 13870 RVA: 0x0010E7DE File Offset: 0x0010C9DE
		public ClientMessage(Hub hub, string method, object[] args, ulong callIdx, OnMethodResultDelegate resultCallback, OnMethodFailedDelegate resultErrorCallback, OnMethodProgressDelegate progressCallback)
		{
			this.Hub = hub;
			this.Method = method;
			this.Args = args;
			this.CallIdx = callIdx;
			this.ResultCallback = resultCallback;
			this.ResultErrorCallback = resultErrorCallback;
			this.ProgressCallback = progressCallback;
		}

		// Token: 0x04002332 RID: 9010
		public readonly Hub Hub;

		// Token: 0x04002333 RID: 9011
		public readonly string Method;

		// Token: 0x04002334 RID: 9012
		public readonly object[] Args;

		// Token: 0x04002335 RID: 9013
		public readonly ulong CallIdx;

		// Token: 0x04002336 RID: 9014
		public readonly OnMethodResultDelegate ResultCallback;

		// Token: 0x04002337 RID: 9015
		public readonly OnMethodFailedDelegate ResultErrorCallback;

		// Token: 0x04002338 RID: 9016
		public readonly OnMethodProgressDelegate ProgressCallback;
	}
}
