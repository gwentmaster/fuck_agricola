using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.Logger;

namespace BestHTTP.SocketIO.Transports
{
	// Token: 0x020005A1 RID: 1441
	internal sealed class PollingTransport : ITransport
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x0002A062 File Offset: 0x00028262
		public TransportTypes Type
		{
			get
			{
				return TransportTypes.Polling;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600350C RID: 13580 RVA: 0x00109EB5 File Offset: 0x001080B5
		// (set) Token: 0x0600350D RID: 13581 RVA: 0x00109EBD File Offset: 0x001080BD
		public TransportStates State { get; private set; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600350E RID: 13582 RVA: 0x00109EC6 File Offset: 0x001080C6
		// (set) Token: 0x0600350F RID: 13583 RVA: 0x00109ECE File Offset: 0x001080CE
		public SocketManager Manager { get; private set; }

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x00109ED7 File Offset: 0x001080D7
		public bool IsRequestInProgress
		{
			get
			{
				return this.LastRequest != null;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x00109EE2 File Offset: 0x001080E2
		public bool IsPollingInProgress
		{
			get
			{
				return this.PollRequest != null;
			}
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x00109EED File Offset: 0x001080ED
		public PollingTransport(SocketManager manager)
		{
			this.Manager = manager;
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x00109F08 File Offset: 0x00108108
		public void Open()
		{
			string text = "{0}?EIO={1}&transport=polling&t={2}-{3}{5}";
			if (this.Manager.Handshake != null)
			{
				text += "&sid={4}";
			}
			bool flag = !this.Manager.Options.QueryParamsOnlyForHandshake || (this.Manager.Options.QueryParamsOnlyForHandshake && this.Manager.Handshake == null);
			string format = text;
			object[] array = new object[6];
			array[0] = this.Manager.Uri.ToString();
			array[1] = 4;
			array[2] = this.Manager.Timestamp.ToString();
			int num = 3;
			SocketManager manager = this.Manager;
			ulong requestCounter = manager.RequestCounter;
			manager.RequestCounter = requestCounter + 1UL;
			array[num] = requestCounter.ToString();
			array[4] = ((this.Manager.Handshake != null) ? this.Manager.Handshake.Sid : string.Empty);
			array[5] = (flag ? this.Manager.Options.BuildQueryParams() : string.Empty);
			new HTTPRequest(new Uri(string.Format(format, array)), new OnRequestFinishedDelegate(this.OnRequestFinished))
			{
				DisableCache = true,
				DisableRetry = true
			}.Send();
			this.State = TransportStates.Opening;
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x0010A040 File Offset: 0x00108240
		public void Close()
		{
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			this.State = TransportStates.Closed;
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x0010A054 File Offset: 0x00108254
		public void Send(Packet packet)
		{
			try
			{
				this.lonelyPacketList.Add(packet);
				this.Send(this.lonelyPacketList);
			}
			finally
			{
				this.lonelyPacketList.Clear();
			}
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x0010A098 File Offset: 0x00108298
		public void Send(List<Packet> packets)
		{
			if (this.State != TransportStates.Opening && this.State != TransportStates.Open)
			{
				return;
			}
			if (this.IsRequestInProgress)
			{
				throw new Exception("Sending packets are still in progress!");
			}
			byte[] array = null;
			try
			{
				array = packets[0].EncodeBinary();
				for (int i = 1; i < packets.Count; i++)
				{
					byte[] array2 = packets[i].EncodeBinary();
					Array.Resize<byte>(ref array, array.Length + array2.Length);
					Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
				}
				packets.Clear();
			}
			catch (Exception ex)
			{
				((IManager)this.Manager).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
				return;
			}
			string format = "{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}";
			object[] array3 = new object[6];
			array3[0] = this.Manager.Uri.ToString();
			array3[1] = 4;
			array3[2] = this.Manager.Timestamp.ToString();
			int num = 3;
			SocketManager manager = this.Manager;
			ulong requestCounter = manager.RequestCounter;
			manager.RequestCounter = requestCounter + 1UL;
			array3[num] = requestCounter.ToString();
			array3[4] = this.Manager.Handshake.Sid;
			array3[5] = ((!this.Manager.Options.QueryParamsOnlyForHandshake) ? this.Manager.Options.BuildQueryParams() : string.Empty);
			this.LastRequest = new HTTPRequest(new Uri(string.Format(format, array3)), HTTPMethods.Post, new OnRequestFinishedDelegate(this.OnRequestFinished));
			this.LastRequest.DisableCache = true;
			this.LastRequest.SetHeader("Content-Type", "application/octet-stream");
			this.LastRequest.RawData = array;
			this.LastRequest.Send();
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x0010A254 File Offset: 0x00108454
		private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.LastRequest = null;
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			string text = null;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (HTTPManager.Logger.Level <= Loglevels.All)
				{
					HTTPManager.Logger.Verbose("PollingTransport", "OnRequestFinished: " + resp.DataAsText);
				}
				if (resp.IsSuccess)
				{
					if (req.MethodType != HTTPMethods.Post)
					{
						this.ParseResponse(resp);
					}
				}
				else
				{
					text = string.Format("Polling - Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					});
				}
				break;
			case HTTPRequestStates.Error:
				text = ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = string.Format("Polling - Request({0}) Aborted!", req.CurrentUri);
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = string.Format("Polling - Connection Timed Out! Uri: {0}", req.CurrentUri);
				break;
			case HTTPRequestStates.TimedOut:
				text = string.Format("Polling - Processing the request({0}) Timed Out!", req.CurrentUri);
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((IManager)this.Manager).OnTransportError(this, text);
			}
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x0010A3A4 File Offset: 0x001085A4
		public void Poll()
		{
			if (this.PollRequest != null || this.State == TransportStates.Paused)
			{
				return;
			}
			string format = "{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}";
			object[] array = new object[6];
			array[0] = this.Manager.Uri.ToString();
			array[1] = 4;
			array[2] = this.Manager.Timestamp.ToString();
			int num = 3;
			SocketManager manager = this.Manager;
			ulong requestCounter = manager.RequestCounter;
			manager.RequestCounter = requestCounter + 1UL;
			array[num] = requestCounter.ToString();
			array[4] = this.Manager.Handshake.Sid;
			array[5] = ((!this.Manager.Options.QueryParamsOnlyForHandshake) ? this.Manager.Options.BuildQueryParams() : string.Empty);
			this.PollRequest = new HTTPRequest(new Uri(string.Format(format, array)), HTTPMethods.Get, new OnRequestFinishedDelegate(this.OnPollRequestFinished));
			this.PollRequest.DisableCache = true;
			this.PollRequest.DisableRetry = true;
			this.PollRequest.Send();
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x0010A4A8 File Offset: 0x001086A8
		private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.PollRequest = null;
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			string text = null;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (HTTPManager.Logger.Level <= Loglevels.All)
				{
					HTTPManager.Logger.Verbose("PollingTransport", "OnPollRequestFinished: " + resp.DataAsText);
				}
				if (resp.IsSuccess)
				{
					this.ParseResponse(resp);
				}
				else
				{
					text = string.Format("Polling - Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					});
				}
				break;
			case HTTPRequestStates.Error:
				text = ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = string.Format("Polling - Request({0}) Aborted!", req.CurrentUri);
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = string.Format("Polling - Connection Timed Out! Uri: {0}", req.CurrentUri);
				break;
			case HTTPRequestStates.TimedOut:
				text = string.Format("Polling - Processing the request({0}) Timed Out!", req.CurrentUri);
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((IManager)this.Manager).OnTransportError(this, text);
			}
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x0010A5EC File Offset: 0x001087EC
		private void OnPacket(Packet packet)
		{
			if (packet.AttachmentCount != 0 && !packet.HasAllAttachment)
			{
				this.PacketWithAttachment = packet;
				return;
			}
			TransportEventTypes transportEvent = packet.TransportEvent;
			if (transportEvent != TransportEventTypes.Open)
			{
				if (transportEvent == TransportEventTypes.Message)
				{
					if (packet.SocketIOEvent == SocketIOEventTypes.Connect)
					{
						this.State = TransportStates.Open;
					}
				}
			}
			else if (this.State != TransportStates.Opening)
			{
				HTTPManager.Logger.Warning("PollingTransport", "Received 'Open' packet while state is '" + this.State.ToString() + "'");
			}
			else
			{
				this.State = TransportStates.Open;
			}
			((IManager)this.Manager).OnPacket(packet);
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x0010A684 File Offset: 0x00108884
		private void ParseResponse(HTTPResponse resp)
		{
			try
			{
				if (resp != null && resp.Data != null && resp.Data.Length >= 1)
				{
					int num;
					for (int i = 0; i < resp.Data.Length; i += num)
					{
						PollingTransport.PayloadTypes payloadTypes = PollingTransport.PayloadTypes.Text;
						num = 0;
						if (resp.Data[i] < 48)
						{
							payloadTypes = (PollingTransport.PayloadTypes)resp.Data[i++];
							for (byte b = resp.Data[i++]; b != 255; b = resp.Data[i++])
							{
								num = num * 10 + (int)b;
							}
						}
						else
						{
							for (byte b2 = resp.Data[i++]; b2 != 58; b2 = resp.Data[i++])
							{
								num = num * 10 + (int)(b2 - 48);
							}
						}
						Packet packet = null;
						if (payloadTypes != PollingTransport.PayloadTypes.Text)
						{
							if (payloadTypes == PollingTransport.PayloadTypes.Binary)
							{
								if (this.PacketWithAttachment != null)
								{
									i++;
									num--;
									byte[] array = new byte[num];
									Array.Copy(resp.Data, i, array, 0, num);
									this.PacketWithAttachment.AddAttachmentFromServer(array, true);
									if (this.PacketWithAttachment.HasAllAttachment)
									{
										packet = this.PacketWithAttachment;
										this.PacketWithAttachment = null;
									}
								}
							}
						}
						else
						{
							packet = new Packet(Encoding.UTF8.GetString(resp.Data, i, num));
						}
						if (packet != null)
						{
							try
							{
								this.OnPacket(packet);
							}
							catch (Exception ex)
							{
								HTTPManager.Logger.Exception("PollingTransport", "ParseResponse - OnPacket", ex);
								((IManager)this.Manager).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				((IManager)this.Manager).EmitError(SocketIOErrors.Internal, ex2.Message + " " + ex2.StackTrace);
				HTTPManager.Logger.Exception("PollingTransport", "ParseResponse", ex2);
			}
		}

		// Token: 0x040022AE RID: 8878
		private HTTPRequest LastRequest;

		// Token: 0x040022AF RID: 8879
		private HTTPRequest PollRequest;

		// Token: 0x040022B0 RID: 8880
		private Packet PacketWithAttachment;

		// Token: 0x040022B1 RID: 8881
		private List<Packet> lonelyPacketList = new List<Packet>(1);

		// Token: 0x020008FB RID: 2299
		private enum PayloadTypes : byte
		{
			// Token: 0x0400302E RID: 12334
			Text,
			// Token: 0x0400302F RID: 12335
			Binary
		}
	}
}
