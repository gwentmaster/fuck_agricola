using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.JSON;
using BestHTTP.SocketIO.JsonEncoders;

namespace BestHTTP.SocketIO
{
	// Token: 0x0200059A RID: 1434
	public sealed class Packet
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x00107F12 File Offset: 0x00106112
		// (set) Token: 0x06003472 RID: 13426 RVA: 0x00107F1A File Offset: 0x0010611A
		public TransportEventTypes TransportEvent { get; private set; }

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x00107F23 File Offset: 0x00106123
		// (set) Token: 0x06003474 RID: 13428 RVA: 0x00107F2B File Offset: 0x0010612B
		public SocketIOEventTypes SocketIOEvent { get; private set; }

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x00107F34 File Offset: 0x00106134
		// (set) Token: 0x06003476 RID: 13430 RVA: 0x00107F3C File Offset: 0x0010613C
		public int AttachmentCount { get; private set; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x00107F45 File Offset: 0x00106145
		// (set) Token: 0x06003478 RID: 13432 RVA: 0x00107F4D File Offset: 0x0010614D
		public int Id { get; private set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x00107F56 File Offset: 0x00106156
		// (set) Token: 0x0600347A RID: 13434 RVA: 0x00107F5E File Offset: 0x0010615E
		public string Namespace { get; private set; }

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x00107F67 File Offset: 0x00106167
		// (set) Token: 0x0600347C RID: 13436 RVA: 0x00107F6F File Offset: 0x0010616F
		public string Payload { get; private set; }

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x00107F78 File Offset: 0x00106178
		// (set) Token: 0x0600347E RID: 13438 RVA: 0x00107F80 File Offset: 0x00106180
		public string EventName { get; private set; }

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x00107F89 File Offset: 0x00106189
		// (set) Token: 0x06003480 RID: 13440 RVA: 0x00107F91 File Offset: 0x00106191
		public List<byte[]> Attachments
		{
			get
			{
				return this.attachments;
			}
			set
			{
				this.attachments = value;
				this.AttachmentCount = ((this.attachments != null) ? this.attachments.Count : 0);
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x00107FB6 File Offset: 0x001061B6
		public bool HasAllAttachment
		{
			get
			{
				return this.Attachments != null && this.Attachments.Count == this.AttachmentCount;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x00107FD5 File Offset: 0x001061D5
		// (set) Token: 0x06003483 RID: 13443 RVA: 0x00107FDD File Offset: 0x001061DD
		public bool IsDecoded { get; private set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x00107FE6 File Offset: 0x001061E6
		// (set) Token: 0x06003485 RID: 13445 RVA: 0x00107FEE File Offset: 0x001061EE
		public object[] DecodedArgs { get; private set; }

		// Token: 0x06003486 RID: 13446 RVA: 0x00107FF7 File Offset: 0x001061F7
		internal Packet()
		{
			this.TransportEvent = TransportEventTypes.Unknown;
			this.SocketIOEvent = SocketIOEventTypes.Unknown;
			this.Payload = string.Empty;
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00108018 File Offset: 0x00106218
		internal Packet(string from)
		{
			this.Parse(from);
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00108027 File Offset: 0x00106227
		public Packet(TransportEventTypes transportEvent, SocketIOEventTypes packetType, string nsp, string payload, int attachment = 0, int id = 0)
		{
			this.TransportEvent = transportEvent;
			this.SocketIOEvent = packetType;
			this.Namespace = nsp;
			this.Payload = payload;
			this.AttachmentCount = attachment;
			this.Id = id;
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x0010805C File Offset: 0x0010625C
		public object[] Decode(IJsonEncoder encoder)
		{
			if (this.IsDecoded || encoder == null)
			{
				return this.DecodedArgs;
			}
			this.IsDecoded = true;
			if (string.IsNullOrEmpty(this.Payload))
			{
				return this.DecodedArgs;
			}
			List<object> list = encoder.Decode(this.Payload);
			if (list != null && list.Count > 0)
			{
				if (this.SocketIOEvent == SocketIOEventTypes.Ack || this.SocketIOEvent == SocketIOEventTypes.BinaryAck)
				{
					this.DecodedArgs = list.ToArray();
				}
				else
				{
					list.RemoveAt(0);
					this.DecodedArgs = list.ToArray();
				}
			}
			return this.DecodedArgs;
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x001080E8 File Offset: 0x001062E8
		public string DecodeEventName()
		{
			if (!string.IsNullOrEmpty(this.EventName))
			{
				return this.EventName;
			}
			if (string.IsNullOrEmpty(this.Payload))
			{
				return string.Empty;
			}
			if (this.Payload[0] != '[')
			{
				return string.Empty;
			}
			int num = 1;
			while (this.Payload.Length > num && this.Payload[num] != '"' && this.Payload[num] != '\'')
			{
				num++;
			}
			if (this.Payload.Length <= num)
			{
				return string.Empty;
			}
			int num2;
			num = (num2 = num + 1);
			while (this.Payload.Length > num && this.Payload[num] != '"' && this.Payload[num] != '\'')
			{
				num++;
			}
			if (this.Payload.Length <= num)
			{
				return string.Empty;
			}
			return this.EventName = this.Payload.Substring(num2, num - num2);
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x001081E4 File Offset: 0x001063E4
		public string RemoveEventName(bool removeArrayMarks)
		{
			if (string.IsNullOrEmpty(this.Payload))
			{
				return string.Empty;
			}
			if (this.Payload[0] != '[')
			{
				return string.Empty;
			}
			int num = 1;
			while (this.Payload.Length > num && this.Payload[num] != '"' && this.Payload[num] != '\'')
			{
				num++;
			}
			if (this.Payload.Length <= num)
			{
				return string.Empty;
			}
			int num2 = num;
			while (this.Payload.Length > num && this.Payload[num] != ',' && this.Payload[num] != ']')
			{
				num++;
			}
			if (this.Payload.Length <= ++num)
			{
				return string.Empty;
			}
			string text = this.Payload.Remove(num2, num - num2);
			if (removeArrayMarks)
			{
				text = text.Substring(1, text.Length - 2);
			}
			return text;
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x001082D6 File Offset: 0x001064D6
		public bool ReconstructAttachmentAsIndex()
		{
			return this.PlaceholderReplacer(delegate(string json, Dictionary<string, object> obj)
			{
				int num = Convert.ToInt32(obj["num"]);
				this.Payload = this.Payload.Replace(json, num.ToString());
				this.IsDecoded = false;
			});
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x001082EA File Offset: 0x001064EA
		public bool ReconstructAttachmentAsBase64()
		{
			return this.HasAllAttachment && this.PlaceholderReplacer(delegate(string json, Dictionary<string, object> obj)
			{
				int index = Convert.ToInt32(obj["num"]);
				this.Payload = this.Payload.Replace(json, string.Format("\"{0}\"", Convert.ToBase64String(this.Attachments[index])));
				this.IsDecoded = false;
			});
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x00108308 File Offset: 0x00106508
		internal void Parse(string from)
		{
			int num = 0;
			this.TransportEvent = (TransportEventTypes)char.GetNumericValue(from, num++);
			if (from.Length > num && char.GetNumericValue(from, num) >= 0.0)
			{
				this.SocketIOEvent = (SocketIOEventTypes)char.GetNumericValue(from, num++);
			}
			else
			{
				this.SocketIOEvent = SocketIOEventTypes.Unknown;
			}
			if (this.SocketIOEvent == SocketIOEventTypes.BinaryEvent || this.SocketIOEvent == SocketIOEventTypes.BinaryAck)
			{
				int num2 = from.IndexOf('-', num);
				if (num2 == -1)
				{
					num2 = from.Length;
				}
				int attachmentCount = 0;
				int.TryParse(from.Substring(num, num2 - num), out attachmentCount);
				this.AttachmentCount = attachmentCount;
				num = num2 + 1;
			}
			if (from.Length > num && from[num] == '/')
			{
				int num3 = from.IndexOf(',', num);
				if (num3 == -1)
				{
					num3 = from.Length;
				}
				this.Namespace = from.Substring(num, num3 - num);
				num = num3 + 1;
			}
			else
			{
				this.Namespace = "/";
			}
			if (from.Length > num && char.GetNumericValue(from[num]) >= 0.0)
			{
				int num4 = num++;
				while (from.Length > num && char.GetNumericValue(from[num]) >= 0.0)
				{
					num++;
				}
				int id = 0;
				int.TryParse(from.Substring(num4, num - num4), out id);
				this.Id = id;
			}
			if (from.Length > num)
			{
				this.Payload = from.Substring(num);
				return;
			}
			this.Payload = string.Empty;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x00108480 File Offset: 0x00106680
		internal string Encode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.TransportEvent == TransportEventTypes.Unknown && this.AttachmentCount > 0)
			{
				this.TransportEvent = TransportEventTypes.Message;
			}
			if (this.TransportEvent != TransportEventTypes.Unknown)
			{
				stringBuilder.Append(((int)this.TransportEvent).ToString());
			}
			if (this.SocketIOEvent == SocketIOEventTypes.Unknown && this.AttachmentCount > 0)
			{
				this.SocketIOEvent = SocketIOEventTypes.BinaryEvent;
			}
			if (this.SocketIOEvent != SocketIOEventTypes.Unknown)
			{
				stringBuilder.Append(((int)this.SocketIOEvent).ToString());
			}
			if (this.SocketIOEvent == SocketIOEventTypes.BinaryEvent || this.SocketIOEvent == SocketIOEventTypes.BinaryAck)
			{
				stringBuilder.Append(this.AttachmentCount.ToString());
				stringBuilder.Append("-");
			}
			bool flag = false;
			if (this.Namespace != "/")
			{
				stringBuilder.Append(this.Namespace);
				flag = true;
			}
			if (this.Id != 0)
			{
				if (flag)
				{
					stringBuilder.Append(",");
					flag = false;
				}
				stringBuilder.Append(this.Id.ToString());
			}
			if (!string.IsNullOrEmpty(this.Payload))
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(this.Payload);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x001085B8 File Offset: 0x001067B8
		internal byte[] EncodeBinary()
		{
			if (this.AttachmentCount != 0 || (this.Attachments != null && this.Attachments.Count != 0))
			{
				if (this.Attachments == null)
				{
					throw new ArgumentException("packet.Attachments are null!");
				}
				if (this.AttachmentCount != this.Attachments.Count)
				{
					throw new ArgumentException("packet.AttachmentCount != packet.Attachments.Count. Use the packet.AddAttachment function to add data to a packet!");
				}
			}
			string s = this.Encode();
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] array = this.EncodeData(bytes, Packet.PayloadTypes.Textual, null);
			if (this.AttachmentCount != 0)
			{
				int num = array.Length;
				List<byte[]> list = new List<byte[]>(this.AttachmentCount);
				int num2 = 0;
				for (int i = 0; i < this.AttachmentCount; i++)
				{
					byte[] array2 = this.EncodeData(this.Attachments[i], Packet.PayloadTypes.Binary, new byte[]
					{
						4
					});
					list.Add(array2);
					num2 += array2.Length;
				}
				Array.Resize<byte>(ref array, array.Length + num2);
				for (int j = 0; j < this.AttachmentCount; j++)
				{
					byte[] array3 = list[j];
					Array.Copy(array3, 0, array, num, array3.Length);
					num += array3.Length;
				}
			}
			return array;
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x001086DC File Offset: 0x001068DC
		internal void AddAttachmentFromServer(byte[] data, bool copyFull)
		{
			if (data == null || data.Length == 0)
			{
				return;
			}
			if (this.attachments == null)
			{
				this.attachments = new List<byte[]>(this.AttachmentCount);
			}
			if (copyFull)
			{
				this.Attachments.Add(data);
				return;
			}
			byte[] array = new byte[data.Length - 1];
			Array.Copy(data, 1, array, 0, data.Length - 1);
			this.Attachments.Add(array);
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x00108740 File Offset: 0x00106940
		private byte[] EncodeData(byte[] data, Packet.PayloadTypes type, byte[] afterHeaderData)
		{
			int num = (afterHeaderData != null) ? afterHeaderData.Length : 0;
			string text = (data.Length + num).ToString();
			byte[] array = new byte[text.Length];
			for (int i = 0; i < text.Length; i++)
			{
				array[i] = (byte)char.GetNumericValue(text[i]);
			}
			byte[] array2 = new byte[data.Length + array.Length + 2 + num];
			array2[0] = (byte)type;
			for (int j = 0; j < array.Length; j++)
			{
				array2[1 + j] = array[j];
			}
			int num2 = 1 + array.Length;
			array2[num2++] = byte.MaxValue;
			if (afterHeaderData != null && afterHeaderData.Length != 0)
			{
				Array.Copy(afterHeaderData, 0, array2, num2, afterHeaderData.Length);
				num2 += afterHeaderData.Length;
			}
			Array.Copy(data, 0, array2, num2, data.Length);
			return array2;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x0010880C File Offset: 0x00106A0C
		private bool PlaceholderReplacer(Action<string, Dictionary<string, object>> onFound)
		{
			if (string.IsNullOrEmpty(this.Payload))
			{
				return false;
			}
			for (int i = this.Payload.IndexOf("_placeholder"); i >= 0; i = this.Payload.IndexOf("_placeholder"))
			{
				int num = i;
				while (this.Payload[num] != '{')
				{
					num--;
				}
				int num2 = i;
				while (this.Payload.Length > num2 && this.Payload[num2] != '}')
				{
					num2++;
				}
				if (this.Payload.Length <= num2)
				{
					return false;
				}
				string text = this.Payload.Substring(num, num2 - num + 1);
				bool flag = false;
				Dictionary<string, object> dictionary = Json.Decode(text, ref flag) as Dictionary<string, object>;
				if (!flag)
				{
					return false;
				}
				object obj;
				if (!dictionary.TryGetValue("_placeholder", out obj) || !(bool)obj)
				{
					return false;
				}
				if (!dictionary.TryGetValue("num", out obj))
				{
					return false;
				}
				onFound(text, dictionary);
			}
			return true;
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x00108905 File Offset: 0x00106B05
		public override string ToString()
		{
			return this.Payload;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x00108910 File Offset: 0x00106B10
		internal Packet Clone()
		{
			return new Packet(this.TransportEvent, this.SocketIOEvent, this.Namespace, this.Payload, 0, this.Id)
			{
				EventName = this.EventName,
				AttachmentCount = this.AttachmentCount,
				attachments = this.attachments
			};
		}

		// Token: 0x04002270 RID: 8816
		public const string Placeholder = "_placeholder";

		// Token: 0x04002278 RID: 8824
		private List<byte[]> attachments;

		// Token: 0x020008F9 RID: 2297
		private enum PayloadTypes : byte
		{
			// Token: 0x04003024 RID: 12324
			Textual,
			// Token: 0x04003025 RID: 12325
			Binary
		}
	}
}
