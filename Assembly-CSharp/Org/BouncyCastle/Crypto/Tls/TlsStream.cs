using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000403 RID: 1027
	internal class TlsStream : Stream
	{
		// Token: 0x060025C0 RID: 9664 RVA: 0x000BE27C File Offset: 0x000BC47C
		internal TlsStream(TlsProtocol handler)
		{
			this.handler = handler;
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x000BE28B File Offset: 0x000BC48B
		public override bool CanRead
		{
			get
			{
				return !this.handler.IsClosed;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x000BE28B File Offset: 0x000BC48B
		public override bool CanWrite
		{
			get
			{
				return !this.handler.IsClosed;
			}
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000BE29B File Offset: 0x000BC49B
		public override void Close()
		{
			this.handler.Close();
			base.Close();
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000BE2AE File Offset: 0x000BC4AE
		public override void Flush()
		{
			this.handler.Flush();
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060025C6 RID: 9670 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060025C7 RID: 9671 RVA: 0x0007F71F File Offset: 0x0007D91F
		// (set) Token: 0x060025C8 RID: 9672 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000BE2BB File Offset: 0x000BC4BB
		public override int Read(byte[] buf, int off, int len)
		{
			return this.handler.ReadApplicationData(buf, off, len);
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000BE2CC File Offset: 0x000BC4CC
		public override int ReadByte()
		{
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) <= 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000BE2F1 File Offset: 0x000BC4F1
		public override void Write(byte[] buf, int off, int len)
		{
			this.handler.WriteData(buf, off, len);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000BE301 File Offset: 0x000BC501
		public override void WriteByte(byte b)
		{
			this.handler.WriteData(new byte[]
			{
				b
			}, 0, 1);
		}

		// Token: 0x0400199C RID: 6556
		private readonly TlsProtocol handler;
	}
}
