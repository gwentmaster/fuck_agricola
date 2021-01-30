using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000456 RID: 1110
	internal class VerifierCalculator : IStreamCalculator
	{
		// Token: 0x0600284D RID: 10317 RVA: 0x000C7104 File Offset: 0x000C5304
		internal VerifierCalculator(ISigner sig)
		{
			this.sig = sig;
			this.stream = new SignerBucket(sig);
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x000C711F File Offset: 0x000C531F
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000C7127 File Offset: 0x000C5327
		public object GetResult()
		{
			return new VerifierResult(this.sig);
		}

		// Token: 0x04001A86 RID: 6790
		private readonly ISigner sig;

		// Token: 0x04001A87 RID: 6791
		private readonly Stream stream;
	}
}
