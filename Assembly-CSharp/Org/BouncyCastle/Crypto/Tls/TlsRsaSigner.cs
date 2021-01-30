using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FA RID: 1018
	public class TlsRsaSigner : AbstractTlsSigner
	{
		// Token: 0x0600258C RID: 9612 RVA: 0x000BDE7C File Offset: 0x000BC07C
		public override byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, true, new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
			signer.BlockUpdate(hash, 0, hash.Length);
			return signer.GenerateSignature();
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000BDEA8 File Offset: 0x000BC0A8
		public override bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, false, publicKey);
			signer.BlockUpdate(hash, 0, hash.Length);
			return signer.VerifySignature(sigBytes);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000BDEC7 File Offset: 0x000BC0C7
		public override ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey)
		{
			return this.MakeSigner(algorithm, false, true, new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000BDEE3 File Offset: 0x000BC0E3
		public override ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey)
		{
			return this.MakeSigner(algorithm, false, false, publicKey);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000BDEEF File Offset: 0x000BC0EF
		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is RsaKeyParameters && !publicKey.IsPrivate;
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000BDF04 File Offset: 0x000BC104
		protected virtual ISigner MakeSigner(SignatureAndHashAlgorithm algorithm, bool raw, bool forSigning, ICipherParameters cp)
		{
			if (algorithm != null != TlsUtilities.IsTlsV12(this.mContext))
			{
				throw new InvalidOperationException();
			}
			if (algorithm != null && algorithm.Signature != 1)
			{
				throw new InvalidOperationException();
			}
			IDigest digest;
			if (raw)
			{
				digest = new NullDigest();
			}
			else if (algorithm == null)
			{
				digest = new CombinedHash();
			}
			else
			{
				digest = TlsUtilities.CreateHash(algorithm.Hash);
			}
			ISigner signer;
			if (algorithm != null)
			{
				signer = new RsaDigestSigner(digest, TlsUtilities.GetOidForHashAlgorithm(algorithm.Hash));
			}
			else
			{
				signer = new GenericSigner(this.CreateRsaImpl(), digest);
			}
			signer.Init(forSigning, cp);
			return signer;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000BDF8A File Offset: 0x000BC18A
		protected virtual IAsymmetricBlockCipher CreateRsaImpl()
		{
			return new Pkcs1Encoding(new RsaBlindedEngine());
		}
	}
}
