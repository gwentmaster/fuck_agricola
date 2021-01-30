using System;
using System.IO;
using Org.BouncyCastle.Utilities.Zlib;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E7 RID: 999
	public class TlsDeflateCompression : TlsCompression
	{
		// Token: 0x0600249A RID: 9370 RVA: 0x000BAF54 File Offset: 0x000B9154
		public TlsDeflateCompression() : this(-1)
		{
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x000BAF5D File Offset: 0x000B915D
		public TlsDeflateCompression(int level)
		{
			this.zIn = new ZStream();
			this.zIn.inflateInit();
			this.zOut = new ZStream();
			this.zOut.deflateInit(level);
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000BAF94 File Offset: 0x000B9194
		public virtual Stream Compress(Stream output)
		{
			return new TlsDeflateCompression.DeflateOutputStream(output, this.zOut, true);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000BAFA3 File Offset: 0x000B91A3
		public virtual Stream Decompress(Stream output)
		{
			return new TlsDeflateCompression.DeflateOutputStream(output, this.zIn, false);
		}

		// Token: 0x04001949 RID: 6473
		public const int LEVEL_NONE = 0;

		// Token: 0x0400194A RID: 6474
		public const int LEVEL_FASTEST = 1;

		// Token: 0x0400194B RID: 6475
		public const int LEVEL_SMALLEST = 9;

		// Token: 0x0400194C RID: 6476
		public const int LEVEL_DEFAULT = -1;

		// Token: 0x0400194D RID: 6477
		protected readonly ZStream zIn;

		// Token: 0x0400194E RID: 6478
		protected readonly ZStream zOut;

		// Token: 0x02000885 RID: 2181
		protected class DeflateOutputStream : ZOutputStream
		{
			// Token: 0x06004569 RID: 17769 RVA: 0x00144AD3 File Offset: 0x00142CD3
			public DeflateOutputStream(Stream output, ZStream z, bool compress) : base(output, z)
			{
				this.compress = compress;
				this.FlushMode = 2;
			}

			// Token: 0x0600456A RID: 17770 RVA: 0x00003022 File Offset: 0x00001222
			public override void Flush()
			{
			}
		}
	}
}
