using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;

namespace BestHTTP.WebSocket.Frames
{
	// Token: 0x0200058D RID: 1421
	public sealed class WebSocketFrameReader
	{
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x00107485 File Offset: 0x00105685
		// (set) Token: 0x0600341C RID: 13340 RVA: 0x0010748D File Offset: 0x0010568D
		public byte Header { get; private set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x00107496 File Offset: 0x00105696
		// (set) Token: 0x0600341E RID: 13342 RVA: 0x0010749E File Offset: 0x0010569E
		public bool IsFinal { get; private set; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600341F RID: 13343 RVA: 0x001074A7 File Offset: 0x001056A7
		// (set) Token: 0x06003420 RID: 13344 RVA: 0x001074AF File Offset: 0x001056AF
		public WebSocketFrameTypes Type { get; private set; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x001074B8 File Offset: 0x001056B8
		// (set) Token: 0x06003422 RID: 13346 RVA: 0x001074C0 File Offset: 0x001056C0
		public bool HasMask { get; private set; }

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x001074C9 File Offset: 0x001056C9
		// (set) Token: 0x06003424 RID: 13348 RVA: 0x001074D1 File Offset: 0x001056D1
		public ulong Length { get; private set; }

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x001074DA File Offset: 0x001056DA
		// (set) Token: 0x06003426 RID: 13350 RVA: 0x001074E2 File Offset: 0x001056E2
		public byte[] Mask { get; private set; }

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x001074EB File Offset: 0x001056EB
		// (set) Token: 0x06003428 RID: 13352 RVA: 0x001074F3 File Offset: 0x001056F3
		public byte[] Data { get; private set; }

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06003429 RID: 13353 RVA: 0x001074FC File Offset: 0x001056FC
		// (set) Token: 0x0600342A RID: 13354 RVA: 0x00107504 File Offset: 0x00105704
		public string DataAsText { get; private set; }

		// Token: 0x0600342B RID: 13355 RVA: 0x00107510 File Offset: 0x00105710
		internal void Read(Stream stream)
		{
			this.Header = this.ReadByte(stream);
			this.IsFinal = ((this.Header & 128) > 0);
			this.Type = (WebSocketFrameTypes)(this.Header & 15);
			byte b = this.ReadByte(stream);
			this.HasMask = ((b & 128) > 0);
			this.Length = (ulong)((long)(b & 127));
			if (this.Length == 126UL)
			{
				byte[] array = new byte[2];
				stream.ReadBuffer(array);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(array, 0, array.Length);
				}
				this.Length = (ulong)BitConverter.ToUInt16(array, 0);
			}
			else if (this.Length == 127UL)
			{
				byte[] array2 = new byte[8];
				stream.ReadBuffer(array2);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(array2, 0, array2.Length);
				}
				this.Length = BitConverter.ToUInt64(array2, 0);
			}
			if (this.HasMask)
			{
				this.Mask = new byte[4];
				if (stream.Read(this.Mask, 0, 4) < this.Mask.Length)
				{
					throw ExceptionHelper.ServerClosedTCPStream();
				}
			}
			this.Data = new byte[this.Length];
			if (this.Length == 0UL)
			{
				return;
			}
			int num = 0;
			for (;;)
			{
				int num2 = stream.Read(this.Data, num, this.Data.Length - num);
				if (num2 <= 0)
				{
					break;
				}
				num += num2;
				if (num >= this.Data.Length)
				{
					goto Block_9;
				}
			}
			throw ExceptionHelper.ServerClosedTCPStream();
			Block_9:
			if (this.HasMask)
			{
				for (int i = 0; i < this.Data.Length; i++)
				{
					this.Data[i] = (this.Data[i] ^ this.Mask[i % 4]);
				}
			}
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x001076A4 File Offset: 0x001058A4
		private byte ReadByte(Stream stream)
		{
			int num = stream.ReadByte();
			if (num < 0)
			{
				throw ExceptionHelper.ServerClosedTCPStream();
			}
			return (byte)num;
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x001076B8 File Offset: 0x001058B8
		public void Assemble(List<WebSocketFrameReader> fragments)
		{
			fragments.Add(this);
			ulong num = 0UL;
			for (int i = 0; i < fragments.Count; i++)
			{
				num += fragments[i].Length;
			}
			byte[] array = new byte[num];
			ulong num2 = 0UL;
			for (int j = 0; j < fragments.Count; j++)
			{
				Array.Copy(fragments[j].Data, 0, array, (int)num2, (int)fragments[j].Length);
				num2 += fragments[j].Length;
			}
			this.Type = fragments[0].Type;
			this.Header = fragments[0].Header;
			this.Length = num;
			this.Data = array;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x00107778 File Offset: 0x00105978
		public void DecodeWithExtensions(WebSocket webSocket)
		{
			if (webSocket.Extensions != null)
			{
				for (int i = 0; i < webSocket.Extensions.Length; i++)
				{
					IExtension extension = webSocket.Extensions[i];
					if (extension != null)
					{
						this.Data = extension.Decode(this.Header, this.Data);
					}
				}
			}
			if (this.Type == WebSocketFrameTypes.Text && this.Data != null)
			{
				this.DataAsText = Encoding.UTF8.GetString(this.Data, 0, this.Data.Length);
			}
		}
	}
}
