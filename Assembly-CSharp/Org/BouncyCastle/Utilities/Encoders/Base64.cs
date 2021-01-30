using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x020002A6 RID: 678
	public sealed class Base64
	{
		// Token: 0x06001676 RID: 5750 RVA: 0x00003425 File Offset: 0x00001625
		private Base64()
		{
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00080D73 File Offset: 0x0007EF73
		public static string ToBase64String(byte[] data)
		{
			return Convert.ToBase64String(data, 0, data.Length);
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00080D7F File Offset: 0x0007EF7F
		public static string ToBase64String(byte[] data, int off, int length)
		{
			return Convert.ToBase64String(data, off, length);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00080D89 File Offset: 0x0007EF89
		public static byte[] Encode(byte[] data)
		{
			return Base64.Encode(data, 0, data.Length);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00080D95 File Offset: 0x0007EF95
		public static byte[] Encode(byte[] data, int off, int length)
		{
			return Strings.ToAsciiByteArray(Convert.ToBase64String(data, off, length));
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00080DA4 File Offset: 0x0007EFA4
		public static int Encode(byte[] data, Stream outStream)
		{
			byte[] array = Base64.Encode(data);
			outStream.Write(array, 0, array.Length);
			return array.Length;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00080DC8 File Offset: 0x0007EFC8
		public static int Encode(byte[] data, int off, int length, Stream outStream)
		{
			byte[] array = Base64.Encode(data, off, length);
			outStream.Write(array, 0, array.Length);
			return array.Length;
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00080DEC File Offset: 0x0007EFEC
		public static byte[] Decode(byte[] data)
		{
			return Convert.FromBase64String(Strings.FromAsciiByteArray(data));
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00080DF9 File Offset: 0x0007EFF9
		public static byte[] Decode(string data)
		{
			return Convert.FromBase64String(data);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00080E04 File Offset: 0x0007F004
		public static int Decode(string data, Stream outStream)
		{
			byte[] array = Base64.Decode(data);
			outStream.Write(array, 0, array.Length);
			return array.Length;
		}
	}
}
