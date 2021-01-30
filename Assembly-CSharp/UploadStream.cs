using System;
using System.IO;
using System.Threading;
using BestHTTP;

// Token: 0x0200000C RID: 12
public sealed class UploadStream : Stream
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000077 RID: 119 RVA: 0x000036A4 File Offset: 0x000018A4
	// (set) Token: 0x06000078 RID: 120 RVA: 0x000036AC File Offset: 0x000018AC
	public string Name { get; private set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000079 RID: 121 RVA: 0x000036B8 File Offset: 0x000018B8
	private bool IsReadBufferEmpty
	{
		get
		{
			object obj = this.locker;
			bool result;
			lock (obj)
			{
				result = (this.ReadBuffer.Position == this.ReadBuffer.Length);
			}
			return result;
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x0000370C File Offset: 0x0000190C
	public UploadStream(string name) : this()
	{
		this.Name = name;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000371C File Offset: 0x0000191C
	public UploadStream()
	{
		this.ReadBuffer = new MemoryStream();
		this.WriteBuffer = new MemoryStream();
		this.Name = string.Empty;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00003780 File Offset: 0x00001980
	public override int Read(byte[] buffer, int offset, int count)
	{
		if (this.noMoreData)
		{
			if (this.ReadBuffer.Position != this.ReadBuffer.Length)
			{
				return this.ReadBuffer.Read(buffer, offset, count);
			}
			if (this.WriteBuffer.Length <= 0L)
			{
				HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Read - End Of Stream", this.Name));
				return -1;
			}
			this.SwitchBuffers();
		}
		object obj;
		if (this.IsReadBufferEmpty)
		{
			this.ARE.WaitOne();
			obj = this.locker;
			lock (obj)
			{
				if (this.IsReadBufferEmpty && this.WriteBuffer.Length > 0L)
				{
					this.SwitchBuffers();
				}
			}
		}
		int result = -1;
		obj = this.locker;
		lock (obj)
		{
			result = this.ReadBuffer.Read(buffer, offset, count);
		}
		return result;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00003890 File Offset: 0x00001A90
	public override void Write(byte[] buffer, int offset, int count)
	{
		if (this.noMoreData)
		{
			throw new ArgumentException("noMoreData already set!");
		}
		object obj = this.locker;
		lock (obj)
		{
			this.WriteBuffer.Write(buffer, offset, count);
			this.SwitchBuffers();
		}
		this.ARE.Set();
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00003900 File Offset: 0x00001B00
	public override void Flush()
	{
		this.Finish();
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00003908 File Offset: 0x00001B08
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Dispose", this.Name));
			this.ReadBuffer.Dispose();
			this.ReadBuffer = null;
			this.WriteBuffer.Dispose();
			this.WriteBuffer = null;
			this.ARE.Close();
			this.ARE = null;
		}
		base.Dispose(disposing);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00003974 File Offset: 0x00001B74
	public void Finish()
	{
		if (this.noMoreData)
		{
			throw new ArgumentException("noMoreData already set!");
		}
		HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Finish", this.Name));
		this.noMoreData = true;
		this.ARE.Set();
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000039C8 File Offset: 0x00001BC8
	private bool SwitchBuffers()
	{
		object obj = this.locker;
		lock (obj)
		{
			if (this.ReadBuffer.Position == this.ReadBuffer.Length)
			{
				this.WriteBuffer.Seek(0L, SeekOrigin.Begin);
				this.ReadBuffer.SetLength(0L);
				MemoryStream writeBuffer = this.WriteBuffer;
				this.WriteBuffer = this.ReadBuffer;
				this.ReadBuffer = writeBuffer;
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000082 RID: 130 RVA: 0x00003A58 File Offset: 0x00001C58
	public override bool CanRead
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000083 RID: 131 RVA: 0x00003A58 File Offset: 0x00001C58
	public override bool CanSeek
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000084 RID: 132 RVA: 0x00003A58 File Offset: 0x00001C58
	public override bool CanWrite
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000085 RID: 133 RVA: 0x00003A58 File Offset: 0x00001C58
	public override long Length
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000086 RID: 134 RVA: 0x00003A58 File Offset: 0x00001C58
	// (set) Token: 0x06000087 RID: 135 RVA: 0x00003A58 File Offset: 0x00001C58
	public override long Position
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00003A58 File Offset: 0x00001C58
	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00003A58 File Offset: 0x00001C58
	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04000039 RID: 57
	private MemoryStream ReadBuffer = new MemoryStream();

	// Token: 0x0400003A RID: 58
	private MemoryStream WriteBuffer = new MemoryStream();

	// Token: 0x0400003B RID: 59
	private bool noMoreData;

	// Token: 0x0400003C RID: 60
	private AutoResetEvent ARE = new AutoResetEvent(false);

	// Token: 0x0400003D RID: 61
	private object locker = new object();
}
