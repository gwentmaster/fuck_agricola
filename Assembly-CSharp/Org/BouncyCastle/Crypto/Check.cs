using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200036F RID: 879
	internal class Check
	{
		// Token: 0x060021C4 RID: 8644 RVA: 0x000B5077 File Offset: 0x000B3277
		internal static void DataLength(bool condition, string msg)
		{
			if (condition)
			{
				throw new DataLengthException(msg);
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000B5083 File Offset: 0x000B3283
		internal static void DataLength(byte[] buf, int off, int len, string msg)
		{
			if (off + len > buf.Length)
			{
				throw new DataLengthException(msg);
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000B5094 File Offset: 0x000B3294
		internal static void OutputLength(byte[] buf, int off, int len, string msg)
		{
			if (off + len > buf.Length)
			{
				throw new OutputLengthException(msg);
			}
		}
	}
}
