using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000453 RID: 1107
	internal class SigCalculator : IStreamCalculator
	{
		// Token: 0x06002843 RID: 10307 RVA: 0x000C7021 File Offset: 0x000C5221
		internal SigCalculator(ISigner sig)
		{
			this.sig = sig;
			this.stream = new SignerBucket(sig);
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06002844 RID: 10308 RVA: 0x000C703C File Offset: 0x000C523C
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000C7044 File Offset: 0x000C5244
		public object GetResult()
		{
			return new SigResult(this.sig);
		}

		// Token: 0x04001A81 RID: 6785
		private readonly ISigner sig;

		// Token: 0x04001A82 RID: 6786
		private readonly Stream stream;
	}
}
