using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F2 RID: 1010
	public interface TlsHandshakeHash : IDigest
	{
		// Token: 0x06002519 RID: 9497
		void Init(TlsContext context);

		// Token: 0x0600251A RID: 9498
		TlsHandshakeHash NotifyPrfDetermined();

		// Token: 0x0600251B RID: 9499
		void TrackHashAlgorithm(byte hashAlgorithm);

		// Token: 0x0600251C RID: 9500
		void SealHashAlgorithms();

		// Token: 0x0600251D RID: 9501
		TlsHandshakeHash StopTracking();

		// Token: 0x0600251E RID: 9502
		IDigest ForkPrfHash();

		// Token: 0x0600251F RID: 9503
		byte[] GetFinalHash(byte hashAlgorithm);
	}
}
