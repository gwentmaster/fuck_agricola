using System;
using System.Collections.Generic;
using BestHTTP.Extensions;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x020005E0 RID: 1504
	public class EventSource : IHeartbeat
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060036F2 RID: 14066 RVA: 0x0010F9B7 File Offset: 0x0010DBB7
		// (set) Token: 0x060036F3 RID: 14067 RVA: 0x0010F9BF File Offset: 0x0010DBBF
		public Uri Uri { get; private set; }

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060036F4 RID: 14068 RVA: 0x0010F9C8 File Offset: 0x0010DBC8
		// (set) Token: 0x060036F5 RID: 14069 RVA: 0x0010F9D0 File Offset: 0x0010DBD0
		public States State
		{
			get
			{
				return this._state;
			}
			private set
			{
				States state = this._state;
				this._state = value;
				if (this.OnStateChanged != null)
				{
					try
					{
						this.OnStateChanged(this, state, this._state);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("EventSource", "OnStateChanged", ex);
					}
				}
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060036F6 RID: 14070 RVA: 0x0010FA30 File Offset: 0x0010DC30
		// (set) Token: 0x060036F7 RID: 14071 RVA: 0x0010FA38 File Offset: 0x0010DC38
		public TimeSpan ReconnectionTime { get; set; }

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060036F8 RID: 14072 RVA: 0x0010FA41 File Offset: 0x0010DC41
		// (set) Token: 0x060036F9 RID: 14073 RVA: 0x0010FA49 File Offset: 0x0010DC49
		public string LastEventId { get; private set; }

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x0010FA52 File Offset: 0x0010DC52
		// (set) Token: 0x060036FB RID: 14075 RVA: 0x0010FA5A File Offset: 0x0010DC5A
		public HTTPRequest InternalRequest { get; private set; }

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060036FC RID: 14076 RVA: 0x0010FA64 File Offset: 0x0010DC64
		// (remove) Token: 0x060036FD RID: 14077 RVA: 0x0010FA9C File Offset: 0x0010DC9C
		public event OnGeneralEventDelegate OnOpen;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060036FE RID: 14078 RVA: 0x0010FAD4 File Offset: 0x0010DCD4
		// (remove) Token: 0x060036FF RID: 14079 RVA: 0x0010FB0C File Offset: 0x0010DD0C
		public event OnMessageDelegate OnMessage;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06003700 RID: 14080 RVA: 0x0010FB44 File Offset: 0x0010DD44
		// (remove) Token: 0x06003701 RID: 14081 RVA: 0x0010FB7C File Offset: 0x0010DD7C
		public event OnErrorDelegate OnError;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06003702 RID: 14082 RVA: 0x0010FBB4 File Offset: 0x0010DDB4
		// (remove) Token: 0x06003703 RID: 14083 RVA: 0x0010FBEC File Offset: 0x0010DDEC
		public event OnRetryDelegate OnRetry;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06003704 RID: 14084 RVA: 0x0010FC24 File Offset: 0x0010DE24
		// (remove) Token: 0x06003705 RID: 14085 RVA: 0x0010FC5C File Offset: 0x0010DE5C
		public event OnGeneralEventDelegate OnClosed;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06003706 RID: 14086 RVA: 0x0010FC94 File Offset: 0x0010DE94
		// (remove) Token: 0x06003707 RID: 14087 RVA: 0x0010FCCC File Offset: 0x0010DECC
		public event OnStateChangedDelegate OnStateChanged;

		// Token: 0x06003708 RID: 14088 RVA: 0x0010FD04 File Offset: 0x0010DF04
		public EventSource(Uri uri)
		{
			this.Uri = uri;
			this.ReconnectionTime = TimeSpan.FromMilliseconds(2000.0);
			this.InternalRequest = new HTTPRequest(this.Uri, HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnRequestFinished));
			this.InternalRequest.SetHeader("Accept", "text/event-stream");
			this.InternalRequest.SetHeader("Cache-Control", "no-cache");
			this.InternalRequest.SetHeader("Accept-Encoding", "identity");
			this.InternalRequest.ProtocolHandler = SupportedProtocols.ServerSentEvents;
			this.InternalRequest.OnUpgraded = new OnRequestFinishedDelegate(this.OnUpgraded);
			this.InternalRequest.DisableRetry = true;
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x0010FDC0 File Offset: 0x0010DFC0
		public void Open()
		{
			if (this.State != States.Initial && this.State != States.Retrying && this.State != States.Closed)
			{
				return;
			}
			this.State = States.Connecting;
			if (!string.IsNullOrEmpty(this.LastEventId))
			{
				this.InternalRequest.SetHeader("Last-Event-ID", this.LastEventId);
			}
			this.InternalRequest.Send();
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x0010FE1E File Offset: 0x0010E01E
		public void Close()
		{
			if (this.State == States.Closing || this.State == States.Closed)
			{
				return;
			}
			this.State = States.Closing;
			if (this.InternalRequest != null)
			{
				this.InternalRequest.Abort();
				return;
			}
			this.State = States.Closed;
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x0010FE55 File Offset: 0x0010E055
		public void On(string eventName, OnEventDelegate action)
		{
			if (this.EventTable == null)
			{
				this.EventTable = new Dictionary<string, OnEventDelegate>();
			}
			this.EventTable[eventName] = action;
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x0010FE77 File Offset: 0x0010E077
		public void Off(string eventName)
		{
			if (eventName == null || this.EventTable == null)
			{
				return;
			}
			this.EventTable.Remove(eventName);
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x0010FE94 File Offset: 0x0010E094
		private void CallOnError(string error, string msg)
		{
			if (this.OnError != null)
			{
				try
				{
					this.OnError(this, error);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", msg + " - OnError", ex);
				}
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x0010FEE8 File Offset: 0x0010E0E8
		private bool CallOnRetry()
		{
			if (this.OnRetry != null)
			{
				try
				{
					return this.OnRetry(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", "CallOnRetry", ex);
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x0010FF38 File Offset: 0x0010E138
		private void SetClosed(string msg)
		{
			this.State = States.Closed;
			if (this.OnClosed != null)
			{
				try
				{
					this.OnClosed(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", msg + " - OnClosed", ex);
				}
			}
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x0010FF90 File Offset: 0x0010E190
		private void Retry()
		{
			if (this.RetryCount > 0 || !this.CallOnRetry())
			{
				this.SetClosed("Retry");
				return;
			}
			this.RetryCount += 1;
			this.RetryCalled = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
			this.State = States.Retrying;
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x0010FFE8 File Offset: 0x0010E1E8
		private void OnUpgraded(HTTPRequest originalRequest, HTTPResponse response)
		{
			EventSourceResponse eventSourceResponse = response as EventSourceResponse;
			if (eventSourceResponse == null)
			{
				this.CallOnError("Not an EventSourceResponse!", "OnUpgraded");
				return;
			}
			if (this.OnOpen != null)
			{
				try
				{
					this.OnOpen(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", "OnOpen", ex);
				}
			}
			EventSourceResponse eventSourceResponse2 = eventSourceResponse;
			eventSourceResponse2.OnMessage = (Action<EventSourceResponse, Message>)Delegate.Combine(eventSourceResponse2.OnMessage, new Action<EventSourceResponse, Message>(this.OnMessageReceived));
			eventSourceResponse.StartReceive();
			this.RetryCount = 0;
			this.State = States.Open;
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x00110084 File Offset: 0x0010E284
		private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			if (this.State == States.Closed)
			{
				return;
			}
			if (this.State == States.Closing || req.State == HTTPRequestStates.Aborted)
			{
				this.SetClosed("OnRequestFinished");
				return;
			}
			string text = string.Empty;
			bool flag = true;
			switch (req.State)
			{
			case HTTPRequestStates.Processing:
				flag = !resp.HasHeader("content-length");
				break;
			case HTTPRequestStates.Finished:
				if (resp.StatusCode == 200 && !resp.HasHeaderWithValue("content-type", "text/event-stream"))
				{
					text = "No Content-Type header with value 'text/event-stream' present.";
					flag = false;
				}
				if (flag && resp.StatusCode != 500 && resp.StatusCode != 502 && resp.StatusCode != 503 && resp.StatusCode != 504)
				{
					flag = false;
					text = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = "OnRequestFinished - Aborted without request. EventSource's State: " + this.State;
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Processing the request Timed Out!";
				break;
			}
			if (this.State >= States.Closing)
			{
				this.SetClosed("OnRequestFinished");
				return;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.CallOnError(text, "OnRequestFinished");
			}
			if (flag)
			{
				this.Retry();
				return;
			}
			this.SetClosed("OnRequestFinished");
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x00110234 File Offset: 0x0010E434
		private void OnMessageReceived(EventSourceResponse resp, Message message)
		{
			if (this.State >= States.Closing)
			{
				return;
			}
			if (message.Id != null)
			{
				this.LastEventId = message.Id;
			}
			if (message.Retry.TotalMilliseconds > 0.0)
			{
				this.ReconnectionTime = message.Retry;
			}
			if (string.IsNullOrEmpty(message.Data))
			{
				return;
			}
			if (this.OnMessage != null)
			{
				try
				{
					this.OnMessage(this, message);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - OnMessage", ex);
				}
			}
			OnEventDelegate onEventDelegate;
			if (this.EventTable != null && !string.IsNullOrEmpty(message.Event) && this.EventTable.TryGetValue(message.Event, out onEventDelegate) && onEventDelegate != null)
			{
				try
				{
					onEventDelegate(this, message);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("EventSource", "OnMessageReceived - action", ex2);
				}
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x0011032C File Offset: 0x0010E52C
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			if (this.State != States.Retrying)
			{
				HTTPManager.Heartbeats.Unsubscribe(this);
				return;
			}
			if (DateTime.UtcNow - this.RetryCalled >= this.ReconnectionTime)
			{
				this.Open();
				if (this.State != States.Connecting)
				{
					this.SetClosed("OnHeartbeatUpdate");
				}
				HTTPManager.Heartbeats.Unsubscribe(this);
			}
		}

		// Token: 0x0400236B RID: 9067
		private States _state;

		// Token: 0x04002375 RID: 9077
		private Dictionary<string, OnEventDelegate> EventTable;

		// Token: 0x04002376 RID: 9078
		private byte RetryCount;

		// Token: 0x04002377 RID: 9079
		private DateTime RetryCalled;
	}
}
