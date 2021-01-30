using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B0 RID: 944
	public abstract class DefaultTlsClient : AbstractTlsClient
	{
		// Token: 0x06002371 RID: 9073 RVA: 0x000B75E1 File Offset: 0x000B57E1
		public DefaultTlsClient()
		{
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000B75E9 File Offset: 0x000B57E9
		public DefaultTlsClient(TlsCipherFactory cipherFactory) : base(cipherFactory)
		{
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000B75F2 File Offset: 0x000B57F2
		public override int[] GetCipherSuites()
		{
			return new int[]
			{
				49195,
				49187,
				49161,
				49199,
				49191,
				49171,
				162,
				64,
				50,
				158,
				103,
				51,
				156,
				60,
				47
			};
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000B7608 File Offset: 0x000B5808
		public override TlsKeyExchange GetKeyExchange()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			switch (keyExchangeAlgorithm)
			{
			case 1:
				return this.CreateRsaKeyExchange();
			case 2:
			case 4:
			case 6:
			case 8:
				break;
			case 3:
			case 5:
				return this.CreateDheKeyExchange(keyExchangeAlgorithm);
			case 7:
			case 9:
				return this.CreateDHKeyExchange(keyExchangeAlgorithm);
			default:
				switch (keyExchangeAlgorithm)
				{
				case 16:
				case 18:
				case 20:
					return this.CreateECDHKeyExchange(keyExchangeAlgorithm);
				case 17:
				case 19:
					return this.CreateECDheKeyExchange(keyExchangeAlgorithm);
				}
				break;
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000B769A File Offset: 0x000B589A
		protected virtual TlsKeyExchange CreateDHKeyExchange(int keyExchange)
		{
			return new TlsDHKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, null);
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x000B76A9 File Offset: 0x000B58A9
		protected virtual TlsKeyExchange CreateDheKeyExchange(int keyExchange)
		{
			return new TlsDheKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, null);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000B76B8 File Offset: 0x000B58B8
		protected virtual TlsKeyExchange CreateECDHKeyExchange(int keyExchange)
		{
			return new TlsECDHKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000B76D8 File Offset: 0x000B58D8
		protected virtual TlsKeyExchange CreateECDheKeyExchange(int keyExchange)
		{
			return new TlsECDheKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000B76F8 File Offset: 0x000B58F8
		protected virtual TlsKeyExchange CreateRsaKeyExchange()
		{
			return new TlsRsaKeyExchange(this.mSupportedSignatureAlgorithms);
		}
	}
}
