using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs
{
	// Token: 0x020005D2 RID: 1490
	public class Hub : IHub
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x0010EDC1 File Offset: 0x0010CFC1
		// (set) Token: 0x06003690 RID: 13968 RVA: 0x0010EDC9 File Offset: 0x0010CFC9
		public string Name { get; private set; }

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x0010EDD2 File Offset: 0x0010CFD2
		public Dictionary<string, object> State
		{
			get
			{
				if (this.state == null)
				{
					this.state = new Dictionary<string, object>();
				}
				return this.state;
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06003692 RID: 13970 RVA: 0x0010EDF0 File Offset: 0x0010CFF0
		// (remove) Token: 0x06003693 RID: 13971 RVA: 0x0010EE28 File Offset: 0x0010D028
		public event OnMethodCallDelegate OnMethodCall;

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06003694 RID: 13972 RVA: 0x0010EE5D File Offset: 0x0010D05D
		// (set) Token: 0x06003695 RID: 13973 RVA: 0x0010EE65 File Offset: 0x0010D065
		Connection IHub.Connection { get; set; }

		// Token: 0x06003696 RID: 13974 RVA: 0x0010EE6E File Offset: 0x0010D06E
		public Hub(string name) : this(name, null)
		{
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x0010EE78 File Offset: 0x0010D078
		public Hub(string name, Connection manager)
		{
			this.Name = name;
			((IHub)this).Connection = manager;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x0010EEAF File Offset: 0x0010D0AF
		public void On(string method, OnMethodCallCallbackDelegate callback)
		{
			this.MethodTable[method] = callback;
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x0010EEBE File Offset: 0x0010D0BE
		public void Off(string method)
		{
			this.MethodTable[method] = null;
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x0010EECD File Offset: 0x0010D0CD
		public bool Call(string method, params object[] args)
		{
			return this.Call(method, null, null, null, args);
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x0010EEDA File Offset: 0x0010D0DA
		public bool Call(string method, OnMethodResultDelegate onResult, params object[] args)
		{
			return this.Call(method, onResult, null, null, args);
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x0010EEE7 File Offset: 0x0010D0E7
		public bool Call(string method, OnMethodResultDelegate onResult, OnMethodFailedDelegate onResultError, params object[] args)
		{
			return this.Call(method, onResult, onResultError, null, args);
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x0010EEF5 File Offset: 0x0010D0F5
		public bool Call(string method, OnMethodResultDelegate onResult, OnMethodProgressDelegate onProgress, params object[] args)
		{
			return this.Call(method, onResult, null, onProgress, args);
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x0010EF04 File Offset: 0x0010D104
		public bool Call(string method, OnMethodResultDelegate onResult, OnMethodFailedDelegate onResultError, OnMethodProgressDelegate onProgress, params object[] args)
		{
			object syncRoot = ((IHub)this).Connection.SyncRoot;
			bool result;
			lock (syncRoot)
			{
				((IHub)this).Connection.ClientMessageCounter %= ulong.MaxValue;
				Connection connection = ((IHub)this).Connection;
				ulong clientMessageCounter = connection.ClientMessageCounter;
				connection.ClientMessageCounter = clientMessageCounter + 1UL;
				result = ((IHub)this).Call(new ClientMessage(this, method, args, clientMessageCounter, onResult, onResultError, onProgress));
			}
			return result;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x0010EF88 File Offset: 0x0010D188
		bool IHub.Call(ClientMessage msg)
		{
			object syncRoot = ((IHub)this).Connection.SyncRoot;
			lock (syncRoot)
			{
				if (!((IHub)this).Connection.SendJson(this.BuildMessage(msg)))
				{
					return false;
				}
				this.SentMessages.Add(msg.CallIdx, msg);
			}
			return true;
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x0010EFF8 File Offset: 0x0010D1F8
		bool IHub.HasSentMessageId(ulong id)
		{
			return this.SentMessages.ContainsKey(id);
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x0010F006 File Offset: 0x0010D206
		void IHub.Close()
		{
			this.SentMessages.Clear();
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x0010F014 File Offset: 0x0010D214
		void IHub.OnMethod(MethodCallMessage msg)
		{
			this.MergeState(msg.State);
			if (this.OnMethodCall != null)
			{
				try
				{
					this.OnMethodCall(this, msg.Method, msg.Arguments);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("Hub - " + this.Name, "IHub.OnMethod - OnMethodCall", ex);
				}
			}
			OnMethodCallCallbackDelegate onMethodCallCallbackDelegate;
			if (this.MethodTable.TryGetValue(msg.Method, out onMethodCallCallbackDelegate) && onMethodCallCallbackDelegate != null)
			{
				try
				{
					onMethodCallCallbackDelegate(this, msg);
					return;
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("Hub - " + this.Name, "IHub.OnMethod - callback", ex2);
					return;
				}
			}
			HTTPManager.Logger.Warning("Hub - " + this.Name, string.Format("[Client] {0}.{1} (args: {2})", this.Name, msg.Method, msg.Arguments.Length));
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0010F110 File Offset: 0x0010D310
		void IHub.OnMessage(IServerMessage msg)
		{
			ulong invocationId = (msg as IHubMessage).InvocationId;
			ClientMessage clientMessage;
			if (!this.SentMessages.TryGetValue(invocationId, out clientMessage))
			{
				HTTPManager.Logger.Warning("Hub - " + this.Name, "OnMessage - Sent message not found with id: " + invocationId.ToString());
				return;
			}
			switch (msg.Type)
			{
			case MessageTypes.Result:
			{
				ResultMessage resultMessage = msg as ResultMessage;
				this.MergeState(resultMessage.State);
				if (clientMessage.ResultCallback != null)
				{
					try
					{
						clientMessage.ResultCallback(this, clientMessage, resultMessage);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ResultCallback", ex);
					}
				}
				this.SentMessages.Remove(invocationId);
				return;
			}
			case MessageTypes.Failure:
			{
				FailureMessage failureMessage = msg as FailureMessage;
				this.MergeState(failureMessage.State);
				if (clientMessage.ResultErrorCallback != null)
				{
					try
					{
						clientMessage.ResultErrorCallback(this, clientMessage, failureMessage);
					}
					catch (Exception ex2)
					{
						HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ResultErrorCallback", ex2);
					}
				}
				this.SentMessages.Remove(invocationId);
				return;
			}
			case MessageTypes.MethodCall:
				break;
			case MessageTypes.Progress:
				if (clientMessage.ProgressCallback != null)
				{
					try
					{
						clientMessage.ProgressCallback(this, clientMessage, msg as ProgressMessage);
					}
					catch (Exception ex3)
					{
						HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ProgressCallback", ex3);
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x0010F2B0 File Offset: 0x0010D4B0
		private void MergeState(IDictionary<string, object> state)
		{
			if (state != null && state.Count > 0)
			{
				foreach (KeyValuePair<string, object> keyValuePair in state)
				{
					this.State[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x0010F318 File Offset: 0x0010D518
		private string BuildMessage(ClientMessage msg)
		{
			string result;
			try
			{
				this.builder.Append("{\"H\":\"");
				this.builder.Append(this.Name);
				this.builder.Append("\",\"M\":\"");
				this.builder.Append(msg.Method);
				this.builder.Append("\",\"A\":");
				string value = string.Empty;
				if (msg.Args != null && msg.Args.Length != 0)
				{
					value = ((IHub)this).Connection.JsonEncoder.Encode(msg.Args);
				}
				else
				{
					value = "[]";
				}
				this.builder.Append(value);
				this.builder.Append(",\"I\":\"");
				this.builder.Append(msg.CallIdx.ToString());
				this.builder.Append("\"");
				if (msg.Hub.state != null && msg.Hub.state.Count > 0)
				{
					this.builder.Append(",\"S\":");
					value = ((IHub)this).Connection.JsonEncoder.Encode(msg.Hub.state);
					this.builder.Append(value);
				}
				this.builder.Append("}");
				result = this.builder.ToString();
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("Hub - " + this.Name, "Send", ex);
				result = null;
			}
			finally
			{
				this.builder.Length = 0;
			}
			return result;
		}

		// Token: 0x04002350 RID: 9040
		private Dictionary<string, object> state;

		// Token: 0x04002352 RID: 9042
		private Dictionary<ulong, ClientMessage> SentMessages = new Dictionary<ulong, ClientMessage>();

		// Token: 0x04002353 RID: 9043
		private Dictionary<string, OnMethodCallCallbackDelegate> MethodTable = new Dictionary<string, OnMethodCallCallbackDelegate>();

		// Token: 0x04002354 RID: 9044
		private StringBuilder builder = new StringBuilder();
	}
}
