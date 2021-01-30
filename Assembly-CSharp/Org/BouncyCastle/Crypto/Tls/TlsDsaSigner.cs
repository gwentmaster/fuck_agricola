using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E9 RID: 1001
	public abstract class TlsDsaSigner : AbstractTlsSigner
	{
		// Token: 0x060024A3 RID: 9379 RVA: 0x000BB174 File Offset: 0x000B9374
		public override byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, true, new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
			if (algorithm == null)
			{
				signer.BlockUpdate(hash, 16, 20);
			}
			else
			{
				signer.BlockUpdate(hash, 0, hash.Length);
			}
			return signer.GenerateSignature();
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000BB1C0 File Offset: 0x000B93C0
		public override bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, false, publicKey);
			if (algorithm == null)
			{
				signer.BlockUpdate(hash, 16, 20);
			}
			else
			{
				signer.BlockUpdate(hash, 0, hash.Length);
			}
			return signer.VerifySignature(sigBytes);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000BB1FD File Offset: 0x000B93FD
		public override ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey)
		{
			return this.MakeSigner(algorithm, false, true, privateKey);
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000BB209 File Offset: 0x000B9409
		public override ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey)
		{
			return this.MakeSigner(algorithm, false, false, publicKey);
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000BB215 File Offset: 0x000B9415
		protected virtual ICipherParameters MakeInitParameters(bool forSigning, ICipherParameters cp)
		{
			return cp;
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000BB218 File Offset: 0x000B9418
		protected virtual ISigner MakeSigner(SignatureAndHashAlgorithm algorithm, bool raw, bool forSigning, ICipherParameters cp)
		{
			if (algorithm != null != TlsUtilities.IsTlsV12(this.mContext))
			{
				throw new InvalidOperationException();
			}
			if (algorithm != null && algorithm.Signature != this.SignatureAlgorithm)
			{
				throw new InvalidOperationException();
			}
			byte hashAlgorithm = (algorithm == null) ? 2 : algorithm.Hash;
			IDigest digest;
			if (!raw)
			{
				digest = TlsUtilities.CreateHash(hashAlgorithm);
			}
			else
			{
				IDigest digest2 = new NullDigest();
				digest = digest2;
			}
			IDigest digest3 = digest;
			DsaDigestSigner dsaDigestSigner = new DsaDigestSigner(this.CreateDsaImpl(hashAlgorithm), digest3);
			((ISigner)dsaDigestSigner).Init(forSigning, this.MakeInitParameters(forSigning, cp));
			return dsaDigestSigner;
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060024A9 RID: 9385
		protected abstract byte SignatureAlgorithm { get; }

		// Token: 0x060024AA RID: 9386
		protected abstract IDsa CreateDsaImpl(byte hashAlgorithm);
	}
}
