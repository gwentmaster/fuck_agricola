using System;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000454 RID: 1108
	internal class SigResult : IBlockResult
	{
		// Token: 0x06002846 RID: 10310 RVA: 0x000C7051 File Offset: 0x000C5251
		internal SigResult(ISigner sig)
		{
			this.sig = sig;
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000C7060 File Offset: 0x000C5260
		public byte[] Collect()
		{
			return this.sig.GenerateSignature();
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x000C7070 File Offset: 0x000C5270
		public int Collect(byte[] destination, int offset)
		{
			byte[] array = this.Collect();
			Array.Copy(array, 0, destination, offset, array.Length);
			return array.Length;
		}

		// Token: 0x04001A83 RID: 6787
		private readonly ISigner sig;
	}
}
