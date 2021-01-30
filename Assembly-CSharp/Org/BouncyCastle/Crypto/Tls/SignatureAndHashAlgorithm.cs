using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D4 RID: 980
	public class SignatureAndHashAlgorithm
	{
		// Token: 0x06002417 RID: 9239 RVA: 0x000B8C30 File Offset: 0x000B6E30
		public SignatureAndHashAlgorithm(byte hash, byte signature)
		{
			if (!TlsUtilities.IsValidUint8((int)hash))
			{
				throw new ArgumentException("should be a uint8", "hash");
			}
			if (!TlsUtilities.IsValidUint8((int)signature))
			{
				throw new ArgumentException("should be a uint8", "signature");
			}
			if (signature == 0)
			{
				throw new ArgumentException("MUST NOT be \"anonymous\"", "signature");
			}
			this.mHash = hash;
			this.mSignature = signature;
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x000B8C94 File Offset: 0x000B6E94
		public virtual byte Hash
		{
			get
			{
				return this.mHash;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000B8C9C File Offset: 0x000B6E9C
		public virtual byte Signature
		{
			get
			{
				return this.mSignature;
			}
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000B8CA4 File Offset: 0x000B6EA4
		public override bool Equals(object obj)
		{
			if (!(obj is SignatureAndHashAlgorithm))
			{
				return false;
			}
			SignatureAndHashAlgorithm signatureAndHashAlgorithm = (SignatureAndHashAlgorithm)obj;
			return signatureAndHashAlgorithm.Hash == this.Hash && signatureAndHashAlgorithm.Signature == this.Signature;
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000B8CE0 File Offset: 0x000B6EE0
		public override int GetHashCode()
		{
			return (int)this.Hash << 16 | (int)this.Signature;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000B8CF2 File Offset: 0x000B6EF2
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.Hash, output);
			TlsUtilities.WriteUint8(this.Signature, output);
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000B8D0C File Offset: 0x000B6F0C
		public static SignatureAndHashAlgorithm Parse(Stream input)
		{
			byte hash = TlsUtilities.ReadUint8(input);
			byte signature = TlsUtilities.ReadUint8(input);
			return new SignatureAndHashAlgorithm(hash, signature);
		}

		// Token: 0x04001914 RID: 6420
		protected readonly byte mHash;

		// Token: 0x04001915 RID: 6421
		protected readonly byte mSignature;
	}
}
