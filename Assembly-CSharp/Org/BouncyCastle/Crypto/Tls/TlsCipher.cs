using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003DC RID: 988
	public interface TlsCipher
	{
		// Token: 0x0600243F RID: 9279
		int GetPlaintextLimit(int ciphertextLimit);

		// Token: 0x06002440 RID: 9280
		byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len);

		// Token: 0x06002441 RID: 9281
		byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len);
	}
}
