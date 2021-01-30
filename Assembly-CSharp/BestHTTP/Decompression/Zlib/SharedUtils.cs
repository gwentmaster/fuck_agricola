using System;
using System.IO;
using System.Text;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x02000608 RID: 1544
	internal class SharedUtils
	{
		// Token: 0x0600388C RID: 14476 RVA: 0x00119ACD File Offset: 0x00117CCD
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x00119AD8 File Offset: 0x00117CD8
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x00079C68 File Offset: 0x00077E68
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x00119B19 File Offset: 0x00117D19
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
