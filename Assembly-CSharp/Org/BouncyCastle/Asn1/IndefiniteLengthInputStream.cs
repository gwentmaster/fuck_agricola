using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x0200050C RID: 1292
	internal class IndefiniteLengthInputStream : LimitedInputStream
	{
		// Token: 0x06002F6A RID: 12138 RVA: 0x000F4813 File Offset: 0x000F2A13
		internal IndefiniteLengthInputStream(Stream inStream, int limit) : base(inStream, limit)
		{
			this._lookAhead = this.RequireByte();
			this.CheckForEof();
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000F4837 File Offset: 0x000F2A37
		internal void SetEofOn00(bool eofOn00)
		{
			this._eofOn00 = eofOn00;
			if (this._eofOn00)
			{
				this.CheckForEof();
			}
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000F484F File Offset: 0x000F2A4F
		private bool CheckForEof()
		{
			if (this._lookAhead != 0)
			{
				return this._lookAhead < 0;
			}
			if (this.RequireByte() != 0)
			{
				throw new IOException("malformed end-of-contents marker");
			}
			this._lookAhead = -1;
			this.SetParentEofDetect(true);
			return true;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000F4888 File Offset: 0x000F2A88
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._eofOn00 || count <= 1)
			{
				return base.Read(buffer, offset, count);
			}
			if (this._lookAhead < 0)
			{
				return 0;
			}
			int num = this._in.Read(buffer, offset + 1, count - 1);
			if (num <= 0)
			{
				throw new EndOfStreamException();
			}
			buffer[offset] = (byte)this._lookAhead;
			this._lookAhead = this.RequireByte();
			return num + 1;
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000F48EA File Offset: 0x000F2AEA
		public override int ReadByte()
		{
			if (this._eofOn00 && this.CheckForEof())
			{
				return -1;
			}
			int lookAhead = this._lookAhead;
			this._lookAhead = this.RequireByte();
			return lookAhead;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000F4910 File Offset: 0x000F2B10
		private int RequireByte()
		{
			int num = this._in.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return num;
		}

		// Token: 0x04001E4C RID: 7756
		private int _lookAhead;

		// Token: 0x04001E4D RID: 7757
		private bool _eofOn00 = true;
	}
}
