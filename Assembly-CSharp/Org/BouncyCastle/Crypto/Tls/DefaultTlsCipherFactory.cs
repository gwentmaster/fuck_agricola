using System;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003AF RID: 943
	public class DefaultTlsCipherFactory : AbstractTlsCipherFactory
	{
		// Token: 0x06002358 RID: 9048 RVA: 0x000B727C File Offset: 0x000B547C
		public override TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
		{
			switch (encryptionAlgorithm)
			{
			case 0:
				return this.CreateNullCipher(context, macAlgorithm);
			case 1:
			case 3:
			case 4:
			case 5:
			case 6:
				break;
			case 2:
				return this.CreateRC4Cipher(context, 16, macAlgorithm);
			case 7:
				return this.CreateDesEdeCipher(context, macAlgorithm);
			case 8:
				return this.CreateAESCipher(context, 16, macAlgorithm);
			case 9:
				return this.CreateAESCipher(context, 32, macAlgorithm);
			case 10:
				return this.CreateCipher_Aes_Gcm(context, 16, 16);
			case 11:
				return this.CreateCipher_Aes_Gcm(context, 32, 16);
			case 12:
				return this.CreateCamelliaCipher(context, 16, macAlgorithm);
			case 13:
				return this.CreateCamelliaCipher(context, 32, macAlgorithm);
			case 14:
				return this.CreateSeedCipher(context, macAlgorithm);
			case 15:
				return this.CreateCipher_Aes_Ccm(context, 16, 16);
			case 16:
				return this.CreateCipher_Aes_Ccm(context, 16, 8);
			case 17:
				return this.CreateCipher_Aes_Ccm(context, 32, 16);
			case 18:
				return this.CreateCipher_Aes_Ccm(context, 32, 8);
			case 19:
				return this.CreateCipher_Camellia_Gcm(context, 16, 16);
			case 20:
				return this.CreateCipher_Camellia_Gcm(context, 32, 16);
			default:
				switch (encryptionAlgorithm)
				{
				case 102:
					return this.CreateChaCha20Poly1305(context);
				case 103:
					return this.CreateCipher_Aes_Ocb(context, 16, 12);
				case 104:
					return this.CreateCipher_Aes_Ocb(context, 32, 12);
				}
				break;
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000B73D4 File Offset: 0x000B55D4
		protected virtual TlsBlockCipher CreateAESCipher(TlsContext context, int cipherKeySize, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateAesBlockCipher(), this.CreateAesBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000B73F7 File Offset: 0x000B55F7
		protected virtual TlsBlockCipher CreateCamelliaCipher(TlsContext context, int cipherKeySize, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateCamelliaBlockCipher(), this.CreateCamelliaBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize);
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000B741A File Offset: 0x000B561A
		protected virtual TlsCipher CreateChaCha20Poly1305(TlsContext context)
		{
			return new Chacha20Poly1305(context);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000B7422 File Offset: 0x000B5622
		protected virtual TlsAeadCipher CreateCipher_Aes_Ccm(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Ccm(), this.CreateAeadBlockCipher_Aes_Ccm(), cipherKeySize, macSize);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x000B7438 File Offset: 0x000B5638
		protected virtual TlsAeadCipher CreateCipher_Aes_Gcm(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Gcm(), this.CreateAeadBlockCipher_Aes_Gcm(), cipherKeySize, macSize);
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000B744E File Offset: 0x000B564E
		protected virtual TlsAeadCipher CreateCipher_Aes_Ocb(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Ocb(), this.CreateAeadBlockCipher_Aes_Ocb(), cipherKeySize, macSize, 2);
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000B7465 File Offset: 0x000B5665
		protected virtual TlsAeadCipher CreateCipher_Camellia_Gcm(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Camellia_Gcm(), this.CreateAeadBlockCipher_Camellia_Gcm(), cipherKeySize, macSize);
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000B747B File Offset: 0x000B567B
		protected virtual TlsBlockCipher CreateDesEdeCipher(TlsContext context, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateDesEdeBlockCipher(), this.CreateDesEdeBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), 24);
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x000B749F File Offset: 0x000B569F
		protected virtual TlsNullCipher CreateNullCipher(TlsContext context, int macAlgorithm)
		{
			return new TlsNullCipher(context, this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm));
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x000B74B5 File Offset: 0x000B56B5
		protected virtual TlsStreamCipher CreateRC4Cipher(TlsContext context, int cipherKeySize, int macAlgorithm)
		{
			return new TlsStreamCipher(context, this.CreateRC4StreamCipher(), this.CreateRC4StreamCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize, false);
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x000B74D9 File Offset: 0x000B56D9
		protected virtual TlsBlockCipher CreateSeedCipher(TlsContext context, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateSeedBlockCipher(), this.CreateSeedBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), 16);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x000B74FD File Offset: 0x000B56FD
		protected virtual IBlockCipher CreateAesEngine()
		{
			return new AesEngine();
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x000B7504 File Offset: 0x000B5704
		protected virtual IBlockCipher CreateCamelliaEngine()
		{
			return new CamelliaEngine();
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000B750B File Offset: 0x000B570B
		protected virtual IBlockCipher CreateAesBlockCipher()
		{
			return new CbcBlockCipher(this.CreateAesEngine());
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000B7518 File Offset: 0x000B5718
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Ccm()
		{
			return new CcmBlockCipher(this.CreateAesEngine());
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000B7525 File Offset: 0x000B5725
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Gcm()
		{
			return new GcmBlockCipher(this.CreateAesEngine());
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x000B7532 File Offset: 0x000B5732
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Ocb()
		{
			return new OcbBlockCipher(this.CreateAesEngine(), this.CreateAesEngine());
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x000B7545 File Offset: 0x000B5745
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Camellia_Gcm()
		{
			return new GcmBlockCipher(this.CreateCamelliaEngine());
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000B7552 File Offset: 0x000B5752
		protected virtual IBlockCipher CreateCamelliaBlockCipher()
		{
			return new CbcBlockCipher(this.CreateCamelliaEngine());
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000B755F File Offset: 0x000B575F
		protected virtual IBlockCipher CreateDesEdeBlockCipher()
		{
			return new CbcBlockCipher(new DesEdeEngine());
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000B756B File Offset: 0x000B576B
		protected virtual IStreamCipher CreateRC4StreamCipher()
		{
			return new RC4Engine();
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x000B7572 File Offset: 0x000B5772
		protected virtual IBlockCipher CreateSeedBlockCipher()
		{
			return new CbcBlockCipher(new SeedEngine());
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000B7580 File Offset: 0x000B5780
		protected virtual IDigest CreateHMacDigest(int macAlgorithm)
		{
			switch (macAlgorithm)
			{
			case 0:
				return null;
			case 1:
				return TlsUtilities.CreateHash(1);
			case 2:
				return TlsUtilities.CreateHash(2);
			case 3:
				return TlsUtilities.CreateHash(4);
			case 4:
				return TlsUtilities.CreateHash(5);
			case 5:
				return TlsUtilities.CreateHash(6);
			default:
				throw new TlsFatalAlert(80);
			}
		}
	}
}
