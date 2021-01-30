using System;
using System.IO;
using BestHTTP.Extensions;

namespace BestHTTP
{
	// Token: 0x02000564 RID: 1380
	internal sealed class StreamList : Stream
	{
		// Token: 0x0600321C RID: 12828 RVA: 0x000FF579 File Offset: 0x000FD779
		public StreamList(params Stream[] streams)
		{
			this.Streams = streams;
			this.CurrentIdx = 0;
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x0600321D RID: 12829 RVA: 0x000FF58F File Offset: 0x000FD78F
		public override bool CanRead
		{
			get
			{
				return this.CurrentIdx < this.Streams.Length && this.Streams[this.CurrentIdx].CanRead;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x0600321F RID: 12831 RVA: 0x000FF5B5 File Offset: 0x000FD7B5
		public override bool CanWrite
		{
			get
			{
				return this.CurrentIdx < this.Streams.Length && this.Streams[this.CurrentIdx].CanWrite;
			}
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000FF5DC File Offset: 0x000FD7DC
		public override void Flush()
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return;
			}
			for (int i = 0; i <= this.CurrentIdx; i++)
			{
				this.Streams[i].Flush();
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x000FF618 File Offset: 0x000FD818
		public override long Length
		{
			get
			{
				if (this.CurrentIdx >= this.Streams.Length)
				{
					return 0L;
				}
				long num = 0L;
				for (int i = 0; i < this.Streams.Length; i++)
				{
					num += this.Streams[i].Length;
				}
				return num;
			}
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000FF660 File Offset: 0x000FD860
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return -1;
			}
			int i;
			for (i = this.Streams[this.CurrentIdx].Read(buffer, offset, count); i < count; i += this.Streams[this.CurrentIdx].Read(buffer, offset + i, count - i))
			{
				int currentIdx = this.CurrentIdx;
				this.CurrentIdx = currentIdx + 1;
				if (currentIdx >= this.Streams.Length)
				{
					break;
				}
			}
			return i;
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000FF6D3 File Offset: 0x000FD8D3
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return;
			}
			this.Streams[this.CurrentIdx].Write(buffer, offset, count);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000FF6FC File Offset: 0x000FD8FC
		public void Write(string str)
		{
			byte[] asciibytes = str.GetASCIIBytes();
			this.Write(asciibytes, 0, asciibytes.Length);
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000FF71C File Offset: 0x000FD91C
		protected override void Dispose(bool disposing)
		{
			for (int i = 0; i < this.Streams.Length; i++)
			{
				try
				{
					this.Streams[i].Dispose();
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("StreamList", "Dispose", ex);
				}
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x000FF774 File Offset: 0x000FD974
		// (set) Token: 0x06003227 RID: 12839 RVA: 0x000FF780 File Offset: 0x000FD980
		public override long Position
		{
			get
			{
				throw new NotImplementedException("Position get");
			}
			set
			{
				throw new NotImplementedException("Position set");
			}
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000FF78C File Offset: 0x000FD98C
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return 0L;
			}
			return this.Streams[this.CurrentIdx].Seek(offset, origin);
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000FF7B5 File Offset: 0x000FD9B5
		public override void SetLength(long value)
		{
			throw new NotImplementedException("SetLength");
		}

		// Token: 0x04002119 RID: 8473
		private Stream[] Streams;

		// Token: 0x0400211A RID: 8474
		private int CurrentIdx;
	}
}
