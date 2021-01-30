using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.Extensions;
using BestHTTP.JSON;
using BestHTTP.Logger;
using BestHTTP.SignalR.Authentication;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.Transports;
using PlatformSupport.Collections.ObjectModel;
using PlatformSupport.Collections.Specialized;

namespace BestHTTP.SignalR
{
	// Token: 0x020005B3 RID: 1459
	public sealed class Connection : IHeartbeat, IConnection
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600357D RID: 13693 RVA: 0x0010B635 File Offset: 0x00109835
		// (set) Token: 0x0600357E RID: 13694 RVA: 0x0010B63D File Offset: 0x0010983D
		public Uri Uri { get; private set; }

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600357F RID: 13695 RVA: 0x0010B646 File Offset: 0x00109846
		// (set) Token: 0x06003580 RID: 13696 RVA: 0x0010B650 File Offset: 0x00109850
		public ConnectionStates State
		{
			get
			{
				return this._state;
			}
			private set
			{
				ConnectionStates state = this._state;
				this._state = value;
				if (this.OnStateChanged != null)
				{
					this.OnStateChanged(this, state, this._state);
				}
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06003581 RID: 13697 RVA: 0x0010B686 File Offset: 0x00109886
		// (set) Token: 0x06003582 RID: 13698 RVA: 0x0010B68E File Offset: 0x0010988E
		public NegotiationData NegotiationResult { get; private set; }

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06003583 RID: 13699 RVA: 0x0010B697 File Offset: 0x00109897
		// (set) Token: 0x06003584 RID: 13700 RVA: 0x0010B69F File Offset: 0x0010989F
		public Hub[] Hubs { get; private set; }

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06003585 RID: 13701 RVA: 0x0010B6A8 File Offset: 0x001098A8
		// (set) Token: 0x06003586 RID: 13702 RVA: 0x0010B6B0 File Offset: 0x001098B0
		public TransportBase Transport { get; private set; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06003587 RID: 13703 RVA: 0x0010B6B9 File Offset: 0x001098B9
		// (set) Token: 0x06003588 RID: 13704 RVA: 0x0010B6C1 File Offset: 0x001098C1
		public ProtocolVersions Protocol { get; private set; }

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06003589 RID: 13705 RVA: 0x0010B6CA File Offset: 0x001098CA
		// (set) Token: 0x0600358A RID: 13706 RVA: 0x0010B6D4 File Offset: 0x001098D4
		public ObservableDictionary<string, string> AdditionalQueryParams
		{
			get
			{
				return this.additionalQueryParams;
			}
			set
			{
				if (this.additionalQueryParams != null)
				{
					this.additionalQueryParams.CollectionChanged -= this.AdditionalQueryParams_CollectionChanged;
				}
				this.additionalQueryParams = value;
				this.BuiltQueryParams = null;
				if (value != null)
				{
					value.CollectionChanged += this.AdditionalQueryParams_CollectionChanged;
				}
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600358B RID: 13707 RVA: 0x0010B723 File Offset: 0x00109923
		// (set) Token: 0x0600358C RID: 13708 RVA: 0x0010B72B File Offset: 0x0010992B
		public bool QueryParamsOnlyForHandshake { get; set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600358D RID: 13709 RVA: 0x0010B734 File Offset: 0x00109934
		// (set) Token: 0x0600358E RID: 13710 RVA: 0x0010B73C File Offset: 0x0010993C
		public IJsonEncoder JsonEncoder { get; set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600358F RID: 13711 RVA: 0x0010B745 File Offset: 0x00109945
		// (set) Token: 0x06003590 RID: 13712 RVA: 0x0010B74D File Offset: 0x0010994D
		public IAuthenticationProvider AuthenticationProvider { get; set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06003591 RID: 13713 RVA: 0x0010B756 File Offset: 0x00109956
		// (set) Token: 0x06003592 RID: 13714 RVA: 0x0010B75E File Offset: 0x0010995E
		public TimeSpan PingInterval { get; set; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06003593 RID: 13715 RVA: 0x0010B767 File Offset: 0x00109967
		// (set) Token: 0x06003594 RID: 13716 RVA: 0x0010B76F File Offset: 0x0010996F
		public TimeSpan ReconnectDelay { get; set; }

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06003595 RID: 13717 RVA: 0x0010B778 File Offset: 0x00109978
		// (remove) Token: 0x06003596 RID: 13718 RVA: 0x0010B7B0 File Offset: 0x001099B0
		public event OnConnectedDelegate OnConnected;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06003597 RID: 13719 RVA: 0x0010B7E8 File Offset: 0x001099E8
		// (remove) Token: 0x06003598 RID: 13720 RVA: 0x0010B820 File Offset: 0x00109A20
		public event OnClosedDelegate OnClosed;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06003599 RID: 13721 RVA: 0x0010B858 File Offset: 0x00109A58
		// (remove) Token: 0x0600359A RID: 13722 RVA: 0x0010B890 File Offset: 0x00109A90
		public event OnErrorDelegate OnError;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600359B RID: 13723 RVA: 0x0010B8C8 File Offset: 0x00109AC8
		// (remove) Token: 0x0600359C RID: 13724 RVA: 0x0010B900 File Offset: 0x00109B00
		public event OnConnectedDelegate OnReconnecting;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600359D RID: 13725 RVA: 0x0010B938 File Offset: 0x00109B38
		// (remove) Token: 0x0600359E RID: 13726 RVA: 0x0010B970 File Offset: 0x00109B70
		public event OnConnectedDelegate OnReconnected;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600359F RID: 13727 RVA: 0x0010B9A8 File Offset: 0x00109BA8
		// (remove) Token: 0x060035A0 RID: 13728 RVA: 0x0010B9E0 File Offset: 0x00109BE0
		public event OnStateChanged OnStateChanged;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060035A1 RID: 13729 RVA: 0x0010BA18 File Offset: 0x00109C18
		// (remove) Token: 0x060035A2 RID: 13730 RVA: 0x0010BA50 File Offset: 0x00109C50
		public event OnNonHubMessageDelegate OnNonHubMessage;

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060035A3 RID: 13731 RVA: 0x0010BA85 File Offset: 0x00109C85
		// (set) Token: 0x060035A4 RID: 13732 RVA: 0x0010BA8D File Offset: 0x00109C8D
		public OnPrepareRequestDelegate RequestPreparator { get; set; }

		// Token: 0x170006DA RID: 1754
		public Hub this[int idx]
		{
			get
			{
				return this.Hubs[idx];
			}
		}

		// Token: 0x170006DB RID: 1755
		public Hub this[string hubName]
		{
			get
			{
				for (int i = 0; i < this.Hubs.Length; i++)
				{
					Hub hub = this.Hubs[i];
					if (hub.Name.Equals(hubName, StringComparison.OrdinalIgnoreCase))
					{
						return hub;
					}
				}
				return null;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060035A7 RID: 13735 RVA: 0x0010BADB File Offset: 0x00109CDB
		// (set) Token: 0x060035A8 RID: 13736 RVA: 0x0010BAE3 File Offset: 0x00109CE3
		internal ulong ClientMessageCounter { get; set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x0010BAEC File Offset: 0x00109CEC
		private uint Timestamp
		{
			get
			{
				return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060035AA RID: 13738 RVA: 0x0010BB1C File Offset: 0x00109D1C
		private string ConnectionData
		{
			get
			{
				if (!string.IsNullOrEmpty(this.BuiltConnectionData))
				{
					return this.BuiltConnectionData;
				}
				StringBuilder stringBuilder = new StringBuilder("[", this.Hubs.Length * 4);
				if (this.Hubs != null)
				{
					for (int i = 0; i < this.Hubs.Length; i++)
					{
						stringBuilder.Append("{\"Name\":\"");
						stringBuilder.Append(this.Hubs[i].Name);
						stringBuilder.Append("\"}");
						if (i < this.Hubs.Length - 1)
						{
							stringBuilder.Append(",");
						}
					}
				}
				stringBuilder.Append("]");
				return this.BuiltConnectionData = Uri.EscapeUriString(stringBuilder.ToString());
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060035AB RID: 13739 RVA: 0x0010BBD4 File Offset: 0x00109DD4
		private string QueryParams
		{
			get
			{
				if (this.AdditionalQueryParams == null || this.AdditionalQueryParams.Count == 0)
				{
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(this.BuiltQueryParams))
				{
					return this.BuiltQueryParams;
				}
				StringBuilder stringBuilder = new StringBuilder(this.AdditionalQueryParams.Count * 4);
				foreach (KeyValuePair<string, string> keyValuePair in this.AdditionalQueryParams)
				{
					stringBuilder.Append("&");
					stringBuilder.Append(keyValuePair.Key);
					if (!string.IsNullOrEmpty(keyValuePair.Value))
					{
						stringBuilder.Append("=");
						stringBuilder.Append(Uri.EscapeDataString(keyValuePair.Value));
					}
				}
				return this.BuiltQueryParams = stringBuilder.ToString();
			}
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x0010BCB4 File Offset: 0x00109EB4
		public Connection(Uri uri, params string[] hubNames) : this(uri)
		{
			if (hubNames != null && hubNames.Length != 0)
			{
				this.Hubs = new Hub[hubNames.Length];
				for (int i = 0; i < hubNames.Length; i++)
				{
					this.Hubs[i] = new Hub(hubNames[i], this);
				}
			}
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x0010BCFC File Offset: 0x00109EFC
		public Connection(Uri uri, params Hub[] hubs) : this(uri)
		{
			this.Hubs = hubs;
			if (hubs != null)
			{
				for (int i = 0; i < hubs.Length; i++)
				{
					((IHub)hubs[i]).Connection = this;
				}
			}
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x0010BD34 File Offset: 0x00109F34
		public Connection(Uri uri)
		{
			this.State = ConnectionStates.Initial;
			this.Uri = uri;
			this.JsonEncoder = Connection.DefaultEncoder;
			this.PingInterval = TimeSpan.FromMinutes(5.0);
			this.Protocol = ProtocolVersions.Protocol_2_2;
			this.ReconnectDelay = TimeSpan.FromSeconds(5.0);
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x0010BDCC File Offset: 0x00109FCC
		public void Open()
		{
			if (this.State != ConnectionStates.Initial && this.State != ConnectionStates.Closed)
			{
				return;
			}
			if (this.AuthenticationProvider != null && this.AuthenticationProvider.IsPreAuthRequired)
			{
				this.State = ConnectionStates.Authenticating;
				this.AuthenticationProvider.OnAuthenticationSucceded += this.OnAuthenticationSucceded;
				this.AuthenticationProvider.OnAuthenticationFailed += this.OnAuthenticationFailed;
				this.AuthenticationProvider.StartAuthentication();
				return;
			}
			this.StartImpl();
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x0010BE47 File Offset: 0x0010A047
		private void OnAuthenticationSucceded(IAuthenticationProvider provider)
		{
			provider.OnAuthenticationSucceded -= this.OnAuthenticationSucceded;
			this.StartImpl();
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0010BE61 File Offset: 0x0010A061
		private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
		{
			provider.OnAuthenticationFailed -= this.OnAuthenticationFailed;
			((IConnection)this).Error(reason);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0010BE7C File Offset: 0x0010A07C
		private void StartImpl()
		{
			this.State = ConnectionStates.Negotiating;
			this.NegotiationResult = new NegotiationData(this);
			this.NegotiationResult.OnReceived = new Action<NegotiationData>(this.OnNegotiationDataReceived);
			this.NegotiationResult.OnError = new Action<NegotiationData, string>(this.OnNegotiationError);
			this.NegotiationResult.Start();
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0010BED8 File Offset: 0x0010A0D8
		private void OnNegotiationDataReceived(NegotiationData data)
		{
			int num = -1;
			int num2 = 0;
			while (num2 < this.ClientProtocols.Length && num == -1)
			{
				if (data.ProtocolVersion == this.ClientProtocols[num2])
				{
					num = num2;
				}
				num2++;
			}
			if (num == -1)
			{
				num = 2;
				HTTPManager.Logger.Warning("SignalR Connection", "Unknown protocol version: " + data.ProtocolVersion);
			}
			this.Protocol = (ProtocolVersions)num;
			if (data.TryWebSockets)
			{
				this.Transport = new WebSocketTransport(this);
				this.NextProtocolToTry = SupportedProtocols.ServerSentEvents;
			}
			else
			{
				this.Transport = new ServerSentEventsTransport(this);
				this.NextProtocolToTry = SupportedProtocols.HTTP;
			}
			this.State = ConnectionStates.Connecting;
			this.TransportConnectionStartedAt = new DateTime?(DateTime.UtcNow);
			this.Transport.Connect();
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x0010BF94 File Offset: 0x0010A194
		private void OnNegotiationError(NegotiationData data, string error)
		{
			((IConnection)this).Error(error);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0010BFA0 File Offset: 0x0010A1A0
		public void Close()
		{
			if (this.State == ConnectionStates.Closed)
			{
				return;
			}
			this.State = ConnectionStates.Closed;
			this.ReconnectStarted = false;
			this.TransportConnectionStartedAt = null;
			if (this.Transport != null)
			{
				this.Transport.Abort();
				this.Transport = null;
			}
			this.NegotiationResult = null;
			HTTPManager.Heartbeats.Unsubscribe(this);
			this.LastReceivedMessage = null;
			if (this.Hubs != null)
			{
				for (int i = 0; i < this.Hubs.Length; i++)
				{
					((IHub)this.Hubs[i]).Close();
				}
			}
			if (this.BufferedMessages != null)
			{
				this.BufferedMessages.Clear();
				this.BufferedMessages = null;
			}
			if (this.OnClosed != null)
			{
				try
				{
					this.OnClosed(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnClosed", ex);
				}
			}
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x0010C084 File Offset: 0x0010A284
		public void Reconnect()
		{
			if (this.ReconnectStarted)
			{
				return;
			}
			this.ReconnectStarted = true;
			if (this.State != ConnectionStates.Reconnecting)
			{
				this.ReconnectStartedAt = DateTime.UtcNow;
			}
			this.State = ConnectionStates.Reconnecting;
			HTTPManager.Logger.Warning("SignalR Connection", "Reconnecting");
			this.Transport.Reconnect();
			if (this.PingRequest != null)
			{
				this.PingRequest.Abort();
			}
			if (this.OnReconnecting != null)
			{
				try
				{
					this.OnReconnecting(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnReconnecting", ex);
				}
			}
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x0010C12C File Offset: 0x0010A32C
		public bool Send(object arg)
		{
			if (arg == null)
			{
				throw new ArgumentNullException("arg");
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				if (this.State != ConnectionStates.Connected)
				{
					return false;
				}
				string text = this.JsonEncoder.Encode(arg);
				if (string.IsNullOrEmpty(text))
				{
					HTTPManager.Logger.Error("SignalR Connection", "Failed to JSon encode the given argument. Please try to use an advanced JSon encoder(check the documentation how you can do it).");
				}
				else
				{
					this.Transport.Send(text);
				}
			}
			return true;
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x0010C1BC File Offset: 0x0010A3BC
		public bool SendJson(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				if (this.State != ConnectionStates.Connected)
				{
					return false;
				}
				this.Transport.Send(json);
			}
			return true;
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x0010C220 File Offset: 0x0010A420
		void IConnection.OnMessage(IServerMessage msg)
		{
			if (this.State == ConnectionStates.Closed)
			{
				return;
			}
			if (this.State == ConnectionStates.Connecting)
			{
				if (this.BufferedMessages == null)
				{
					this.BufferedMessages = new List<IServerMessage>();
				}
				this.BufferedMessages.Add(msg);
				return;
			}
			this.LastMessageReceivedAt = DateTime.UtcNow;
			switch (msg.Type)
			{
			case MessageTypes.KeepAlive:
				break;
			case MessageTypes.Data:
				if (this.OnNonHubMessage != null)
				{
					this.OnNonHubMessage(this, (msg as DataMessage).Data);
					return;
				}
				break;
			case MessageTypes.Multiple:
				this.LastReceivedMessage = (msg as MultiMessage);
				if (this.LastReceivedMessage.IsInitialization)
				{
					HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Init");
				}
				if (this.LastReceivedMessage.GroupsToken != null)
				{
					this.GroupsToken = this.LastReceivedMessage.GroupsToken;
				}
				if (this.LastReceivedMessage.ShouldReconnect)
				{
					HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Should Reconnect");
					this.Reconnect();
				}
				if (this.LastReceivedMessage.Data != null)
				{
					for (int i = 0; i < this.LastReceivedMessage.Data.Count; i++)
					{
						((IConnection)this).OnMessage(this.LastReceivedMessage.Data[i]);
					}
					return;
				}
				break;
			case MessageTypes.Result:
			case MessageTypes.Failure:
			case MessageTypes.Progress:
			{
				ulong invocationId = (msg as IHubMessage).InvocationId;
				Hub hub = this.FindHub(invocationId);
				if (hub != null)
				{
					((IHub)hub).OnMessage(msg);
					return;
				}
				HTTPManager.Logger.Warning("SignalR Connection", string.Format("No Hub found for Progress message! Id: {0}", invocationId.ToString()));
				return;
			}
			case MessageTypes.MethodCall:
			{
				MethodCallMessage methodCallMessage = msg as MethodCallMessage;
				Hub hub = this[methodCallMessage.Hub];
				if (hub != null)
				{
					((IHub)hub).OnMethod(methodCallMessage);
					return;
				}
				HTTPManager.Logger.Warning("SignalR Connection", string.Format("Hub \"{0}\" not found!", methodCallMessage.Hub));
				return;
			}
			default:
				HTTPManager.Logger.Warning("SignalR Connection", "Unknown message type received: " + msg.Type.ToString());
				break;
			}
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x0010C420 File Offset: 0x0010A620
		void IConnection.TransportStarted()
		{
			if (this.State != ConnectionStates.Connecting)
			{
				return;
			}
			this.InitOnStart();
			if (this.OnConnected != null)
			{
				try
				{
					this.OnConnected(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnOpened", ex);
				}
			}
			if (this.BufferedMessages != null)
			{
				for (int i = 0; i < this.BufferedMessages.Count; i++)
				{
					((IConnection)this).OnMessage(this.BufferedMessages[i]);
				}
				this.BufferedMessages.Clear();
				this.BufferedMessages = null;
			}
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x0010C4C0 File Offset: 0x0010A6C0
		void IConnection.TransportReconnected()
		{
			if (this.State != ConnectionStates.Reconnecting)
			{
				return;
			}
			HTTPManager.Logger.Information("SignalR Connection", "Transport Reconnected");
			this.InitOnStart();
			if (this.OnReconnected != null)
			{
				try
				{
					this.OnReconnected(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnReconnected", ex);
				}
			}
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x0010C530 File Offset: 0x0010A730
		void IConnection.TransportAborted()
		{
			this.Close();
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x0010C538 File Offset: 0x0010A738
		void IConnection.Error(string reason)
		{
			if (this.State == ConnectionStates.Closed)
			{
				return;
			}
			HTTPManager.Logger.Error("SignalR Connection", reason);
			this.ReconnectStarted = false;
			if (this.OnError != null)
			{
				this.OnError(this, reason);
			}
			if (this.State == ConnectionStates.Connected || this.State == ConnectionStates.Reconnecting)
			{
				this.ReconnectDelayStartedAt = DateTime.UtcNow;
				if (this.State != ConnectionStates.Reconnecting)
				{
					this.ReconnectStartedAt = DateTime.UtcNow;
					return;
				}
			}
			else if (this.State != ConnectionStates.Connecting || !this.TryFallbackTransport())
			{
				this.Close();
			}
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x0010C5C4 File Offset: 0x0010A7C4
		Uri IConnection.BuildUri(RequestTypes type)
		{
			return ((IConnection)this).BuildUri(type, null);
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x0010C5D0 File Offset: 0x0010A7D0
		Uri IConnection.BuildUri(RequestTypes type, TransportBase transport)
		{
			object syncRoot = this.SyncRoot;
			Uri uri;
			lock (syncRoot)
			{
				this.queryBuilder.Length = 0;
				UriBuilder uriBuilder = new UriBuilder(this.Uri);
				if (!uriBuilder.Path.EndsWith("/"))
				{
					UriBuilder uriBuilder2 = uriBuilder;
					uriBuilder2.Path += "/";
				}
				this.RequestCounter %= ulong.MaxValue;
				ulong requestCounter;
				switch (type)
				{
				case RequestTypes.Negotiate:
				{
					UriBuilder uriBuilder3 = uriBuilder;
					uriBuilder3.Path += "negotiate";
					break;
				}
				case RequestTypes.Connect:
				{
					if (transport != null && transport.Type == TransportTypes.WebSocket)
					{
						uriBuilder.Scheme = (HTTPProtocolFactory.IsSecureProtocol(this.Uri) ? "wss" : "ws");
					}
					UriBuilder uriBuilder4 = uriBuilder;
					uriBuilder4.Path += "connect";
					break;
				}
				case RequestTypes.Start:
				{
					UriBuilder uriBuilder5 = uriBuilder;
					uriBuilder5.Path += "start";
					break;
				}
				case RequestTypes.Poll:
				{
					UriBuilder uriBuilder6 = uriBuilder;
					uriBuilder6.Path += "poll";
					if (this.LastReceivedMessage != null)
					{
						this.queryBuilder.Append("messageId=");
						this.queryBuilder.Append(this.LastReceivedMessage.MessageId);
					}
					if (!string.IsNullOrEmpty(this.GroupsToken))
					{
						if (this.queryBuilder.Length > 0)
						{
							this.queryBuilder.Append("&");
						}
						this.queryBuilder.Append("groupsToken=");
						this.queryBuilder.Append(this.GroupsToken);
					}
					break;
				}
				case RequestTypes.Send:
				{
					UriBuilder uriBuilder7 = uriBuilder;
					uriBuilder7.Path += "send";
					break;
				}
				case RequestTypes.Reconnect:
				{
					if (transport != null && transport.Type == TransportTypes.WebSocket)
					{
						uriBuilder.Scheme = (HTTPProtocolFactory.IsSecureProtocol(this.Uri) ? "wss" : "ws");
					}
					UriBuilder uriBuilder8 = uriBuilder;
					uriBuilder8.Path += "reconnect";
					if (this.LastReceivedMessage != null)
					{
						this.queryBuilder.Append("messageId=");
						this.queryBuilder.Append(this.LastReceivedMessage.MessageId);
					}
					if (!string.IsNullOrEmpty(this.GroupsToken))
					{
						if (this.queryBuilder.Length > 0)
						{
							this.queryBuilder.Append("&");
						}
						this.queryBuilder.Append("groupsToken=");
						this.queryBuilder.Append(this.GroupsToken);
					}
					break;
				}
				case RequestTypes.Abort:
				{
					UriBuilder uriBuilder9 = uriBuilder;
					uriBuilder9.Path += "abort";
					break;
				}
				case RequestTypes.Ping:
				{
					UriBuilder uriBuilder10 = uriBuilder;
					uriBuilder10.Path += "ping";
					this.queryBuilder.Append("&tid=");
					StringBuilder stringBuilder = this.queryBuilder;
					requestCounter = this.RequestCounter;
					this.RequestCounter = requestCounter + 1UL;
					stringBuilder.Append(requestCounter.ToString());
					this.queryBuilder.Append("&_=");
					this.queryBuilder.Append(this.Timestamp.ToString());
					goto IL_45F;
				}
				}
				if (this.queryBuilder.Length > 0)
				{
					this.queryBuilder.Append("&");
				}
				this.queryBuilder.Append("tid=");
				StringBuilder stringBuilder2 = this.queryBuilder;
				requestCounter = this.RequestCounter;
				this.RequestCounter = requestCounter + 1UL;
				stringBuilder2.Append(requestCounter.ToString());
				this.queryBuilder.Append("&_=");
				this.queryBuilder.Append(this.Timestamp.ToString());
				if (transport != null)
				{
					this.queryBuilder.Append("&transport=");
					this.queryBuilder.Append(transport.Name);
				}
				this.queryBuilder.Append("&clientProtocol=");
				this.queryBuilder.Append(this.ClientProtocols[(int)this.Protocol]);
				if (this.NegotiationResult != null && !string.IsNullOrEmpty(this.NegotiationResult.ConnectionToken))
				{
					this.queryBuilder.Append("&connectionToken=");
					this.queryBuilder.Append(this.NegotiationResult.ConnectionToken);
				}
				if (this.Hubs != null && this.Hubs.Length != 0)
				{
					this.queryBuilder.Append("&connectionData=");
					this.queryBuilder.Append(this.ConnectionData);
				}
				IL_45F:
				if (this.AdditionalQueryParams != null && this.AdditionalQueryParams.Count > 0)
				{
					this.queryBuilder.Append(this.QueryParams);
				}
				uriBuilder.Query = this.queryBuilder.ToString();
				this.queryBuilder.Length = 0;
				uri = uriBuilder.Uri;
			}
			return uri;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x0010CAB4 File Offset: 0x0010ACB4
		HTTPRequest IConnection.PrepareRequest(HTTPRequest req, RequestTypes type)
		{
			if (req != null && this.AuthenticationProvider != null)
			{
				this.AuthenticationProvider.PrepareRequest(req, type);
			}
			if (this.RequestPreparator != null)
			{
				this.RequestPreparator(this, req, type);
			}
			return req;
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x0010CAE8 File Offset: 0x0010ACE8
		string IConnection.ParseResponse(string responseStr)
		{
			Dictionary<string, object> dictionary = Json.Decode(responseStr) as Dictionary<string, object>;
			if (dictionary == null)
			{
				((IConnection)this).Error("Failed to parse Start response: " + responseStr);
				return string.Empty;
			}
			object obj;
			if (!dictionary.TryGetValue("Response", out obj) || obj == null)
			{
				((IConnection)this).Error("No 'Response' key found in response: " + responseStr);
				return string.Empty;
			}
			return obj.ToString();
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x0010CB4C File Offset: 0x0010AD4C
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			ConnectionStates state = this.State;
			if (state != ConnectionStates.Connected)
			{
				if (state != ConnectionStates.Reconnecting)
				{
					if (this.TransportConnectionStartedAt != null && DateTime.UtcNow - this.TransportConnectionStartedAt >= this.NegotiationResult.TransportConnectTimeout)
					{
						HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Transport failed to connect in the given time!");
						((IConnection)this).Error("Transport failed to connect in the given time!");
					}
				}
				else
				{
					if (DateTime.UtcNow - this.ReconnectStartedAt >= this.NegotiationResult.DisconnectTimeout)
					{
						HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Failed to reconnect in the given time!");
						this.Close();
						return;
					}
					if (DateTime.UtcNow - this.ReconnectDelayStartedAt >= this.ReconnectDelay)
					{
						if (HTTPManager.Logger.Level <= Loglevels.Warning)
						{
							HTTPManager.Logger.Warning("SignalR Connection", string.Concat(new string[]
							{
								this.ReconnectStarted.ToString(),
								" ",
								this.ReconnectStartedAt.ToString(),
								" ",
								this.NegotiationResult.DisconnectTimeout.ToString()
							}));
						}
						this.Reconnect();
						return;
					}
				}
			}
			else
			{
				if (this.Transport.SupportsKeepAlive && this.NegotiationResult.KeepAliveTimeout != null && DateTime.UtcNow - this.LastMessageReceivedAt >= this.NegotiationResult.KeepAliveTimeout)
				{
					this.Reconnect();
				}
				if (this.PingRequest == null && DateTime.UtcNow - this.LastPingSentAt >= this.PingInterval)
				{
					this.Ping();
					return;
				}
			}
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x0010CD5E File Offset: 0x0010AF5E
		private void InitOnStart()
		{
			this.State = ConnectionStates.Connected;
			this.ReconnectStarted = false;
			this.TransportConnectionStartedAt = null;
			this.LastPingSentAt = DateTime.UtcNow;
			this.LastMessageReceivedAt = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x0010CD9C File Offset: 0x0010AF9C
		private Hub FindHub(ulong msgId)
		{
			if (this.Hubs != null)
			{
				for (int i = 0; i < this.Hubs.Length; i++)
				{
					if (((IHub)this.Hubs[i]).HasSentMessageId(msgId))
					{
						return this.Hubs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0010CDE0 File Offset: 0x0010AFE0
		private bool TryFallbackTransport()
		{
			if (this.State == ConnectionStates.Connecting)
			{
				if (this.BufferedMessages != null)
				{
					this.BufferedMessages.Clear();
				}
				this.Transport.Stop();
				this.Transport = null;
				switch (this.NextProtocolToTry)
				{
				case SupportedProtocols.Unknown:
					return false;
				case SupportedProtocols.HTTP:
					this.Transport = new PollingTransport(this);
					this.NextProtocolToTry = SupportedProtocols.Unknown;
					break;
				case SupportedProtocols.WebSocket:
					this.Transport = new WebSocketTransport(this);
					break;
				case SupportedProtocols.ServerSentEvents:
					this.Transport = new ServerSentEventsTransport(this);
					this.NextProtocolToTry = SupportedProtocols.HTTP;
					break;
				}
				this.TransportConnectionStartedAt = new DateTime?(DateTime.UtcNow);
				this.Transport.Connect();
				if (this.PingRequest != null)
				{
					this.PingRequest.Abort();
				}
				return true;
			}
			return false;
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x0010CEA8 File Offset: 0x0010B0A8
		private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.BuiltQueryParams = null;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0010CEB4 File Offset: 0x0010B0B4
		private void Ping()
		{
			HTTPManager.Logger.Information("SignalR Connection", "Sending Ping request.");
			this.PingRequest = new HTTPRequest(((IConnection)this).BuildUri(RequestTypes.Ping), new OnRequestFinishedDelegate(this.OnPingRequestFinished));
			this.PingRequest.ConnectTimeout = this.PingInterval;
			((IConnection)this).PrepareRequest(this.PingRequest, RequestTypes.Ping);
			this.PingRequest.Send();
			this.LastPingSentAt = DateTime.UtcNow;
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x0010CF2C File Offset: 0x0010B12C
		private void OnPingRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.PingRequest = null;
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					string text2 = ((IConnection)this).ParseResponse(resp.DataAsText);
					if (text2 != "pong")
					{
						text = "Wrong answer for ping request: " + text2;
					}
					else
					{
						HTTPManager.Logger.Information("SignalR Connection", "Pong received.");
					}
				}
				else
				{
					text = string.Format("Ping - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Ping - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Ping - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Ping - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((IConnection)this).Error(text);
			}
		}

		// Token: 0x040022CB RID: 8907
		public static IJsonEncoder DefaultEncoder = new DefaultJsonEncoder();

		// Token: 0x040022CD RID: 8909
		private ConnectionStates _state;

		// Token: 0x040022D2 RID: 8914
		private ObservableDictionary<string, string> additionalQueryParams;

		// Token: 0x040022E0 RID: 8928
		internal object SyncRoot = new object();

		// Token: 0x040022E2 RID: 8930
		private readonly string[] ClientProtocols = new string[]
		{
			"1.3",
			"1.4",
			"1.5"
		};

		// Token: 0x040022E3 RID: 8931
		private ulong RequestCounter;

		// Token: 0x040022E4 RID: 8932
		private MultiMessage LastReceivedMessage;

		// Token: 0x040022E5 RID: 8933
		private string GroupsToken;

		// Token: 0x040022E6 RID: 8934
		private List<IServerMessage> BufferedMessages;

		// Token: 0x040022E7 RID: 8935
		private DateTime LastMessageReceivedAt;

		// Token: 0x040022E8 RID: 8936
		private DateTime ReconnectStartedAt;

		// Token: 0x040022E9 RID: 8937
		private DateTime ReconnectDelayStartedAt;

		// Token: 0x040022EA RID: 8938
		private bool ReconnectStarted;

		// Token: 0x040022EB RID: 8939
		private DateTime LastPingSentAt;

		// Token: 0x040022EC RID: 8940
		private HTTPRequest PingRequest;

		// Token: 0x040022ED RID: 8941
		private DateTime? TransportConnectionStartedAt;

		// Token: 0x040022EE RID: 8942
		private StringBuilder queryBuilder = new StringBuilder();

		// Token: 0x040022EF RID: 8943
		private string BuiltConnectionData;

		// Token: 0x040022F0 RID: 8944
		private string BuiltQueryParams;

		// Token: 0x040022F1 RID: 8945
		private SupportedProtocols NextProtocolToTry;
	}
}
