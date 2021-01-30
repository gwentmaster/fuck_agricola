using System;
using System.IO;
using BestHTTP.WebSocket.Extensions;

namespace BestHTTP.WebSocket.Frames
{
	// Token: 0x0200058C RID: 1420
	public sealed class WebSocketFrame
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600340A RID: 13322 RVA: 0x001070D8 File Offset: 0x001052D8
		// (set) Token: 0x0600340B RID: 13323 RVA: 0x001070E0 File Offset: 0x001052E0
		public WebSocketFrameTypes Type { get; private set; }

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600340C RID: 13324 RVA: 0x001070E9 File Offset: 0x001052E9
		// (set) Token: 0x0600340D RID: 13325 RVA: 0x001070F1 File Offset: 0x001052F1
		public bool IsFinal { get; private set; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x0600340E RID: 13326 RVA: 0x001070FA File Offset: 0x001052FA
		// (set) Token: 0x0600340F RID: 13327 RVA: 0x00107102 File Offset: 0x00105302
		public byte Header { get; private set; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06003410 RID: 13328 RVA: 0x0010710B File Offset: 0x0010530B
		// (set) Token: 0x06003411 RID: 13329 RVA: 0x00107113 File Offset: 0x00105313
		public byte[] Data { get; private set; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06003412 RID: 13330 RVA: 0x0010711C File Offset: 0x0010531C
		// (set) Token: 0x06003413 RID: 13331 RVA: 0x00107124 File Offset: 0x00105324
		public bool UseExtensions { get; private set; }

		// Token: 0x06003414 RID: 13332 RVA: 0x0010712D File Offset: 0x0010532D
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data) : this(webSocket, type, data, true)
		{
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x00107139 File Offset: 0x00105339
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool useExtensions) : this(webSocket, type, data, 0UL, (ulong)((data != null) ? ((long)data.Length) : 0L), true, useExtensions)
		{
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x00107154 File Offset: 0x00105354
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool isFinal, bool useExtensions) : this(webSocket, type, data, 0UL, (ulong)((data != null) ? ((long)data.Length) : 0L), isFinal, useExtensions)
		{
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x00107170 File Offset: 0x00105370
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, ulong pos, ulong length, bool isFinal, bool useExtensions)
		{
			this.Type = type;
			this.IsFinal = isFinal;
			this.UseExtensions = useExtensions;
			if (data != null)
			{
				this.Data = new byte[length];
				Array.Copy(data, (int)pos, this.Data, 0, (int)length);
			}
			else
			{
				data = WebSocketFrame.NoData;
			}
			byte b = this.IsFinal ? 128 : 0;
			this.Header = (b | (byte)this.Type);
			if (this.UseExtensions && webSocket != null && webSocket.Extensions != null)
			{
				for (int i = 0; i < webSocket.Extensions.Length; i++)
				{
					IExtension extension = webSocket.Extensions[i];
					if (extension != null)
					{
						this.Header |= extension.GetFrameHeader(this, this.Header);
						this.Data = extension.Encode(this);
					}
				}
			}
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x00107244 File Offset: 0x00105444
		public byte[] Get()
		{
			if (this.Data == null)
			{
				this.Data = WebSocketFrame.NoData;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(this.Data.Length + 9))
			{
				memoryStream.WriteByte(this.Header);
				if (this.Data.Length < 126)
				{
					memoryStream.WriteByte(128 | (byte)this.Data.Length);
				}
				else if (this.Data.Length < 65535)
				{
					memoryStream.WriteByte(254);
					byte[] bytes = BitConverter.GetBytes((ushort)this.Data.Length);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse(bytes, 0, bytes.Length);
					}
					memoryStream.Write(bytes, 0, bytes.Length);
				}
				else
				{
					memoryStream.WriteByte(byte.MaxValue);
					byte[] bytes2 = BitConverter.GetBytes((ulong)((long)this.Data.Length));
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse(bytes2, 0, bytes2.Length);
					}
					memoryStream.Write(bytes2, 0, bytes2.Length);
				}
				byte[] bytes3 = BitConverter.GetBytes(this.GetHashCode());
				memoryStream.Write(bytes3, 0, bytes3.Length);
				for (int i = 0; i < this.Data.Length; i++)
				{
					memoryStream.WriteByte(this.Data[i] ^ bytes3[i % 4]);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x00107390 File Offset: 0x00105590
		public WebSocketFrame[] Fragment(ushort maxFragmentSize)
		{
			if (this.Data == null)
			{
				return null;
			}
			if (this.Type != WebSocketFrameTypes.Binary && this.Type != WebSocketFrameTypes.Text)
			{
				return null;
			}
			if (this.Data.Length <= (int)maxFragmentSize)
			{
				return null;
			}
			this.IsFinal = false;
			this.Header &= 127;
			int num = this.Data.Length / (int)maxFragmentSize + ((this.Data.Length % (int)maxFragmentSize == 0) ? -1 : 0);
			WebSocketFrame[] array = new WebSocketFrame[num];
			ulong num3;
			for (ulong num2 = (ulong)maxFragmentSize; num2 < (ulong)((long)this.Data.Length); num2 += num3)
			{
				num3 = Math.Min((ulong)maxFragmentSize, (ulong)((long)this.Data.Length - (long)num2));
				array[array.Length - num--] = new WebSocketFrame(null, WebSocketFrameTypes.Continuation, this.Data, num2, num3, num2 + num3 >= (ulong)((long)this.Data.Length), false);
			}
			byte[] array2 = new byte[(int)maxFragmentSize];
			Array.Copy(this.Data, 0, array2, 0, (int)maxFragmentSize);
			this.Data = array2;
			return array;
		}

		// Token: 0x0400221F RID: 8735
		public static readonly byte[] NoData = new byte[0];
	}
}
