using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200029D RID: 669
	public sealed class Streams
	{
		// Token: 0x0600164B RID: 5707 RVA: 0x00003425 File Offset: 0x00001625
		private Streams()
		{
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00080680 File Offset: 0x0007E880
		public static void Drain(Stream inStr)
		{
			byte[] array = new byte[512];
			while (inStr.Read(array, 0, array.Length) > 0)
			{
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000806A8 File Offset: 0x0007E8A8
		public static byte[] ReadAll(Stream inStr)
		{
			MemoryStream memoryStream = new MemoryStream();
			Streams.PipeAll(inStr, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000806C8 File Offset: 0x0007E8C8
		public static byte[] ReadAllLimited(Stream inStr, int limit)
		{
			MemoryStream memoryStream = new MemoryStream();
			Streams.PipeAllLimited(inStr, (long)limit, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x000806EB File Offset: 0x0007E8EB
		public static int ReadFully(Stream inStr, byte[] buf)
		{
			return Streams.ReadFully(inStr, buf, 0, buf.Length);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x000806F8 File Offset: 0x0007E8F8
		public static int ReadFully(Stream inStr, byte[] buf, int off, int len)
		{
			int i;
			int num;
			for (i = 0; i < len; i += num)
			{
				num = inStr.Read(buf, off + i, len - i);
				if (num < 1)
				{
					break;
				}
			}
			return i;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00080724 File Offset: 0x0007E924
		public static void PipeAll(Stream inStr, Stream outStr)
		{
			byte[] array = new byte[512];
			int count;
			while ((count = inStr.Read(array, 0, array.Length)) > 0)
			{
				outStr.Write(array, 0, count);
			}
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00080758 File Offset: 0x0007E958
		public static long PipeAllLimited(Stream inStr, long limit, Stream outStr)
		{
			byte[] array = new byte[512];
			long num = 0L;
			int num2;
			while ((num2 = inStr.Read(array, 0, array.Length)) > 0)
			{
				if (limit - num < (long)num2)
				{
					throw new StreamOverflowException("Data Overflow");
				}
				num += (long)num2;
				outStr.Write(array, 0, num2);
			}
			return num;
		}

		// Token: 0x04001502 RID: 5378
		private const int BufferSize = 512;
	}
}
