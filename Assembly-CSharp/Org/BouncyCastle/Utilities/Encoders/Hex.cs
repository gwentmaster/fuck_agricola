using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x020002A8 RID: 680
	public sealed class Hex
	{
		// Token: 0x06001689 RID: 5769 RVA: 0x00003425 File Offset: 0x00001625
		private Hex()
		{
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00081453 File Offset: 0x0007F653
		public static string ToHexString(byte[] data)
		{
			return Hex.ToHexString(data, 0, data.Length);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0008145F File Offset: 0x0007F65F
		public static string ToHexString(byte[] data, int off, int length)
		{
			return Strings.FromAsciiByteArray(Hex.Encode(data, off, length));
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0008146E File Offset: 0x0007F66E
		public static byte[] Encode(byte[] data)
		{
			return Hex.Encode(data, 0, data.Length);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0008147C File Offset: 0x0007F67C
		public static byte[] Encode(byte[] data, int off, int length)
		{
			MemoryStream memoryStream = new MemoryStream(length * 2);
			Hex.encoder.Encode(data, off, length, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x000814A7 File Offset: 0x0007F6A7
		public static int Encode(byte[] data, Stream outStream)
		{
			return Hex.encoder.Encode(data, 0, data.Length, outStream);
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x000814B9 File Offset: 0x0007F6B9
		public static int Encode(byte[] data, int off, int length, Stream outStream)
		{
			return Hex.encoder.Encode(data, off, length, outStream);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x000814CC File Offset: 0x0007F6CC
		public static byte[] Decode(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream((data.Length + 1) / 2);
			Hex.encoder.Decode(data, 0, data.Length, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00081500 File Offset: 0x0007F700
		public static byte[] Decode(string data)
		{
			MemoryStream memoryStream = new MemoryStream((data.Length + 1) / 2);
			Hex.encoder.DecodeString(data, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00081530 File Offset: 0x0007F730
		public static int Decode(string data, Stream outStream)
		{
			return Hex.encoder.DecodeString(data, outStream);
		}

		// Token: 0x04001516 RID: 5398
		private static readonly IEncoder encoder = new HexEncoder();
	}
}
