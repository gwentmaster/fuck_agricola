using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B3 RID: 947
	public class DigitallySigned
	{
		// Token: 0x0600238E RID: 9102 RVA: 0x000B7B3B File Offset: 0x000B5D3B
		public DigitallySigned(SignatureAndHashAlgorithm algorithm, byte[] signature)
		{
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			this.mAlgorithm = algorithm;
			this.mSignature = signature;
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600238F RID: 9103 RVA: 0x000B7B5F File Offset: 0x000B5D5F
		public virtual SignatureAndHashAlgorithm Algorithm
		{
			get
			{
				return this.mAlgorithm;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x000B7B67 File Offset: 0x000B5D67
		public virtual byte[] Signature
		{
			get
			{
				return this.mSignature;
			}
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x000B7B6F File Offset: 0x000B5D6F
		public virtual void Encode(Stream output)
		{
			if (this.mAlgorithm != null)
			{
				this.mAlgorithm.Encode(output);
			}
			TlsUtilities.WriteOpaque16(this.mSignature, output);
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000B7B94 File Offset: 0x000B5D94
		public static DigitallySigned Parse(TlsContext context, Stream input)
		{
			SignatureAndHashAlgorithm algorithm = null;
			if (TlsUtilities.IsTlsV12(context))
			{
				algorithm = SignatureAndHashAlgorithm.Parse(input);
			}
			byte[] signature = TlsUtilities.ReadOpaque16(input);
			return new DigitallySigned(algorithm, signature);
		}

		// Token: 0x0400181D RID: 6173
		protected readonly SignatureAndHashAlgorithm mAlgorithm;

		// Token: 0x0400181E RID: 6174
		protected readonly byte[] mSignature;
	}
}
