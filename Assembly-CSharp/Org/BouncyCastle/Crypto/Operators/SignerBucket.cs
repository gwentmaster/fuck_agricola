using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000451 RID: 1105
	internal class SignerBucket : Stream
	{
		// Token: 0x06002830 RID: 10288 RVA: 0x000C6F49 File Offset: 0x000C5149
		public SignerBucket(ISigner signer)
		{
			this.signer = signer;
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x00003A58 File Offset: 0x00001C58
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x00003A58 File Offset: 0x00001C58
		public override int ReadByte()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000C6F58 File Offset: 0x000C5158
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this.signer.BlockUpdate(buffer, offset, count);
			}
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000C6F6C File Offset: 0x000C516C
		public override void WriteByte(byte b)
		{
			this.signer.Update(b);
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06002835 RID: 10293 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002836 RID: 10294 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06002837 RID: 10295 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06002838 RID: 10296 RVA: 0x00074B97 File Offset: 0x00072D97
		public override long Length
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x00003A58 File Offset: 0x00001C58
		// (set) Token: 0x0600283A RID: 10298 RVA: 0x00003A58 File Offset: 0x00001C58
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

		// Token: 0x0600283B RID: 10299 RVA: 0x00003022 File Offset: 0x00001222
		public override void Flush()
		{
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00003A58 File Offset: 0x00001C58
		public override void SetLength(long length)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001A7C RID: 6780
		protected readonly ISigner signer;
	}
}
