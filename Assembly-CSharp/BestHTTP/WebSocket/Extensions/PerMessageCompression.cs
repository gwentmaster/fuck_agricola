using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket.Extensions
{
	// Token: 0x02000590 RID: 1424
	public sealed class PerMessageCompression : IExtension
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x001077F4 File Offset: 0x001059F4
		// (set) Token: 0x06003436 RID: 13366 RVA: 0x001077FC File Offset: 0x001059FC
		public bool ClientNoContextTakeover { get; private set; }

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06003437 RID: 13367 RVA: 0x00107805 File Offset: 0x00105A05
		// (set) Token: 0x06003438 RID: 13368 RVA: 0x0010780D File Offset: 0x00105A0D
		public bool ServerNoContextTakeover { get; private set; }

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06003439 RID: 13369 RVA: 0x00107816 File Offset: 0x00105A16
		// (set) Token: 0x0600343A RID: 13370 RVA: 0x0010781E File Offset: 0x00105A1E
		public int ClientMaxWindowBits { get; private set; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x00107827 File Offset: 0x00105A27
		// (set) Token: 0x0600343C RID: 13372 RVA: 0x0010782F File Offset: 0x00105A2F
		public int ServerMaxWindowBits { get; private set; }

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x00107838 File Offset: 0x00105A38
		// (set) Token: 0x0600343E RID: 13374 RVA: 0x00107840 File Offset: 0x00105A40
		public CompressionLevel Level { get; private set; }

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600343F RID: 13375 RVA: 0x00107849 File Offset: 0x00105A49
		// (set) Token: 0x06003440 RID: 13376 RVA: 0x00107851 File Offset: 0x00105A51
		public int MinimumDataLegthToCompress { get; set; }

		// Token: 0x06003441 RID: 13377 RVA: 0x0010785A File Offset: 0x00105A5A
		public PerMessageCompression() : this(CompressionLevel.Default, false, false, 15, 15, 10)
		{
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x0010786C File Offset: 0x00105A6C
		public PerMessageCompression(CompressionLevel level, bool clientNoContextTakeover, bool serverNoContextTakeover, int desiredClientMaxWindowBits, int desiredServerMaxWindowBits, int minDatalengthToCompress)
		{
			this.Level = level;
			this.ClientNoContextTakeover = clientNoContextTakeover;
			this.ServerNoContextTakeover = this.ServerNoContextTakeover;
			this.ClientMaxWindowBits = desiredClientMaxWindowBits;
			this.ServerMaxWindowBits = desiredServerMaxWindowBits;
			this.MinimumDataLegthToCompress = minDatalengthToCompress;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x001078C4 File Offset: 0x00105AC4
		public void AddNegotiation(HTTPRequest request)
		{
			string text = "permessage-deflate";
			if (this.ServerNoContextTakeover)
			{
				text += "; server_no_context_takeover";
			}
			if (this.ClientNoContextTakeover)
			{
				text += "; client_no_context_takeover";
			}
			if (this.ServerMaxWindowBits != 15)
			{
				text = text + "; server_max_window_bits=" + this.ServerMaxWindowBits.ToString();
			}
			else
			{
				this.ServerMaxWindowBits = 15;
			}
			if (this.ClientMaxWindowBits != 15)
			{
				text = text + "; client_max_window_bits=" + this.ClientMaxWindowBits.ToString();
			}
			else
			{
				text += "; client_max_window_bits";
				this.ClientMaxWindowBits = 15;
			}
			request.AddHeader("Sec-WebSocket-Extensions", text);
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x00107974 File Offset: 0x00105B74
		public bool ParseNegotiation(WebSocketResponse resp)
		{
			List<string> headerValues = resp.GetHeaderValues("Sec-WebSocket-Extensions");
			if (headerValues == null)
			{
				return false;
			}
			for (int i = 0; i < headerValues.Count; i++)
			{
				HeaderParser headerParser = new HeaderParser(headerValues[i]);
				for (int j = 0; j < headerParser.Values.Count; j++)
				{
					HeaderValue headerValue = headerParser.Values[i];
					if (!string.IsNullOrEmpty(headerValue.Key) && headerValue.Key.StartsWith("permessage-deflate", StringComparison.OrdinalIgnoreCase))
					{
						HTTPManager.Logger.Information("PerMessageCompression", "Enabled with header: " + headerValues[i]);
						HeaderValue headerValue2;
						if (headerValue.TryGetOption("client_no_context_takeover", out headerValue2))
						{
							this.ClientNoContextTakeover = true;
						}
						if (headerValue.TryGetOption("server_no_context_takeover", out headerValue2))
						{
							this.ServerNoContextTakeover = true;
						}
						int clientMaxWindowBits;
						if (headerValue.TryGetOption("client_max_window_bits", out headerValue2) && headerValue2.HasValue && int.TryParse(headerValue2.Value, out clientMaxWindowBits))
						{
							this.ClientMaxWindowBits = clientMaxWindowBits;
						}
						int serverMaxWindowBits;
						if (headerValue.TryGetOption("server_max_window_bits", out headerValue2) && headerValue2.HasValue && int.TryParse(headerValue2.Value, out serverMaxWindowBits))
						{
							this.ServerMaxWindowBits = serverMaxWindowBits;
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x00107ABB File Offset: 0x00105CBB
		public byte GetFrameHeader(WebSocketFrame writer, byte inFlag)
		{
			if ((writer.Type == WebSocketFrameTypes.Binary || writer.Type == WebSocketFrameTypes.Text) && writer.Data != null && writer.Data.Length >= this.MinimumDataLegthToCompress)
			{
				return inFlag | 64;
			}
			return inFlag;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x00107AEE File Offset: 0x00105CEE
		public byte[] Encode(WebSocketFrame writer)
		{
			if (writer.Data == null)
			{
				return WebSocketFrame.NoData;
			}
			if ((writer.Header & 64) != 0)
			{
				return this.Compress(writer.Data);
			}
			return writer.Data;
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x00107B1C File Offset: 0x00105D1C
		public byte[] Decode(byte header, byte[] data)
		{
			if ((header & 64) != 0)
			{
				return this.Decompress(data);
			}
			return data;
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x00107B30 File Offset: 0x00105D30
		private byte[] Compress(byte[] data)
		{
			if (this.compressorOutputStream == null)
			{
				this.compressorOutputStream = new MemoryStream();
			}
			this.compressorOutputStream.SetLength(0L);
			if (this.compressorDeflateStream == null)
			{
				this.compressorDeflateStream = new DeflateStream(this.compressorOutputStream, CompressionMode.Compress, this.Level, true, this.ClientMaxWindowBits);
				this.compressorDeflateStream.FlushMode = FlushType.Sync;
			}
			byte[] result = null;
			try
			{
				this.compressorDeflateStream.Write(data, 0, data.Length);
				this.compressorDeflateStream.Flush();
				this.compressorOutputStream.Position = 0L;
				this.compressorOutputStream.SetLength(this.compressorOutputStream.Length - 4L);
				result = this.compressorOutputStream.ToArray();
			}
			finally
			{
				if (this.ClientNoContextTakeover)
				{
					this.compressorDeflateStream.Dispose();
					this.compressorDeflateStream = null;
				}
			}
			return result;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x00107C10 File Offset: 0x00105E10
		private byte[] Decompress(byte[] data)
		{
			if (this.decompressorInputStream == null)
			{
				this.decompressorInputStream = new MemoryStream(data.Length + 4);
			}
			this.decompressorInputStream.Write(data, 0, data.Length);
			this.decompressorInputStream.Write(PerMessageCompression.Trailer, 0, PerMessageCompression.Trailer.Length);
			this.decompressorInputStream.Position = 0L;
			if (this.decompressorDeflateStream == null)
			{
				this.decompressorDeflateStream = new DeflateStream(this.decompressorInputStream, CompressionMode.Decompress, CompressionLevel.Default, true, this.ServerMaxWindowBits);
				this.decompressorDeflateStream.FlushMode = FlushType.Sync;
			}
			if (this.decompressorOutputStream == null)
			{
				this.decompressorOutputStream = new MemoryStream();
			}
			this.decompressorOutputStream.SetLength(0L);
			int count;
			while ((count = this.decompressorDeflateStream.Read(this.copyBuffer, 0, this.copyBuffer.Length)) != 0)
			{
				this.decompressorOutputStream.Write(this.copyBuffer, 0, count);
			}
			this.decompressorDeflateStream.SetLength(0L);
			byte[] result = this.decompressorOutputStream.ToArray();
			if (this.ServerNoContextTakeover)
			{
				this.decompressorDeflateStream.Dispose();
				this.decompressorDeflateStream = null;
			}
			return result;
		}

		// Token: 0x04002234 RID: 8756
		private static readonly byte[] Trailer = new byte[]
		{
			0,
			0,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x0400223B RID: 8763
		private MemoryStream compressorOutputStream;

		// Token: 0x0400223C RID: 8764
		private DeflateStream compressorDeflateStream;

		// Token: 0x0400223D RID: 8765
		private MemoryStream decompressorInputStream;

		// Token: 0x0400223E RID: 8766
		private MemoryStream decompressorOutputStream;

		// Token: 0x0400223F RID: 8767
		private DeflateStream decompressorDeflateStream;

		// Token: 0x04002240 RID: 8768
		private byte[] copyBuffer = new byte[1024];
	}
}
