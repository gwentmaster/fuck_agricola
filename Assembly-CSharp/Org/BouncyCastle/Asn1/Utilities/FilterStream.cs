using System;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Utilities
{
	// Token: 0x0200053E RID: 1342
	public class FilterStream : Stream
	{
		// Token: 0x060030FE RID: 12542 RVA: 0x000FB635 File Offset: 0x000F9835
		public FilterStream(Stream s)
		{
			this.s = s;
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060030FF RID: 12543 RVA: 0x000FB644 File Offset: 0x000F9844
		public override bool CanRead
		{
			get
			{
				return this.s.CanRead;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x000FB651 File Offset: 0x000F9851
		public override bool CanSeek
		{
			get
			{
				return this.s.CanSeek;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x000FB65E File Offset: 0x000F985E
		public override bool CanWrite
		{
			get
			{
				return this.s.CanWrite;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x000FB66B File Offset: 0x000F986B
		public override long Length
		{
			get
			{
				return this.s.Length;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x000FB678 File Offset: 0x000F9878
		// (set) Token: 0x06003104 RID: 12548 RVA: 0x000FB685 File Offset: 0x000F9885
		public override long Position
		{
			get
			{
				return this.s.Position;
			}
			set
			{
				this.s.Position = value;
			}
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x000FB693 File Offset: 0x000F9893
		public override void Close()
		{
			Platform.Dispose(this.s);
			base.Close();
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x000FB6A6 File Offset: 0x000F98A6
		public override void Flush()
		{
			this.s.Flush();
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x000FB6B3 File Offset: 0x000F98B3
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.s.Seek(offset, origin);
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x000FB6C2 File Offset: 0x000F98C2
		public override void SetLength(long value)
		{
			this.s.SetLength(value);
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x000FB6D0 File Offset: 0x000F98D0
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.s.Read(buffer, offset, count);
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000FB6E0 File Offset: 0x000F98E0
		public override int ReadByte()
		{
			return this.s.ReadByte();
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000FB6ED File Offset: 0x000F98ED
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.s.Write(buffer, offset, count);
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000FB6FD File Offset: 0x000F98FD
		public override void WriteByte(byte value)
		{
			this.s.WriteByte(value);
		}

		// Token: 0x04001F80 RID: 8064
		protected readonly Stream s;
	}
}
