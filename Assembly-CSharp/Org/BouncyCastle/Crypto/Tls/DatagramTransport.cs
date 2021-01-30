using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003AE RID: 942
	public interface DatagramTransport
	{
		// Token: 0x06002353 RID: 9043
		int GetReceiveLimit();

		// Token: 0x06002354 RID: 9044
		int GetSendLimit();

		// Token: 0x06002355 RID: 9045
		int Receive(byte[] buf, int off, int len, int waitMillis);

		// Token: 0x06002356 RID: 9046
		void Send(byte[] buf, int off, int len);

		// Token: 0x06002357 RID: 9047
		void Close();
	}
}
