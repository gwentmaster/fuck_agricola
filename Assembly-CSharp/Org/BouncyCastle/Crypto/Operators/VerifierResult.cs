using System;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000457 RID: 1111
	internal class VerifierResult : IVerifier
	{
		// Token: 0x06002850 RID: 10320 RVA: 0x000C7134 File Offset: 0x000C5334
		internal VerifierResult(ISigner sig)
		{
			this.sig = sig;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000C7143 File Offset: 0x000C5343
		public bool IsVerified(byte[] signature)
		{
			return this.sig.VerifySignature(signature);
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000C7154 File Offset: 0x000C5354
		public bool IsVerified(byte[] signature, int off, int length)
		{
			byte[] array = new byte[length];
			Array.Copy(signature, 0, array, off, array.Length);
			return this.sig.VerifySignature(signature);
		}

		// Token: 0x04001A88 RID: 6792
		private readonly ISigner sig;
	}
}
