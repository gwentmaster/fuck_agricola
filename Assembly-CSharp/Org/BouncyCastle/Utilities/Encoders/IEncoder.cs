using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x020002AA RID: 682
	public interface IEncoder
	{
		// Token: 0x0600169A RID: 5786
		int Encode(byte[] data, int off, int length, Stream outStream);

		// Token: 0x0600169B RID: 5787
		int Decode(byte[] data, int off, int length, Stream outStream);

		// Token: 0x0600169C RID: 5788
		int DecodeString(string data, Stream outStream);
	}
}
