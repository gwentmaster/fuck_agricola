using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000510 RID: 1296
	internal abstract class LimitedInputStream : BaseInputStream
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x000F4B3C File Offset: 0x000F2D3C
		internal LimitedInputStream(Stream inStream, int limit)
		{
			this._in = inStream;
			this._limit = limit;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000F4B52 File Offset: 0x000F2D52
		internal virtual int GetRemaining()
		{
			return this._limit;
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000F4B5A File Offset: 0x000F2D5A
		protected virtual void SetParentEofDetect(bool on)
		{
			if (this._in is IndefiniteLengthInputStream)
			{
				((IndefiniteLengthInputStream)this._in).SetEofOn00(on);
			}
		}

		// Token: 0x04001E50 RID: 7760
		protected readonly Stream _in;

		// Token: 0x04001E51 RID: 7761
		private int _limit;
	}
}
