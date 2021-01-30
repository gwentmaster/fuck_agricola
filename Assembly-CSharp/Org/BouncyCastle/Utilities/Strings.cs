using System;
using System.Text;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x0200028A RID: 650
	public abstract class Strings
	{
		// Token: 0x06001581 RID: 5505 RVA: 0x00079B50 File Offset: 0x00077D50
		internal static bool IsOneOf(string s, params string[] candidates)
		{
			foreach (string b in candidates)
			{
				if (s == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00079B80 File Offset: 0x00077D80
		public static string FromByteArray(byte[] bs)
		{
			char[] array = new char[bs.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToChar(bs[i]);
			}
			return new string(array);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00079BB8 File Offset: 0x00077DB8
		public static byte[] ToByteArray(char[] cs)
		{
			byte[] array = new byte[cs.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(cs[i]);
			}
			return array;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00079BE8 File Offset: 0x00077DE8
		public static byte[] ToByteArray(string s)
		{
			byte[] array = new byte[s.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(s[i]);
			}
			return array;
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00079C1F File Offset: 0x00077E1F
		public static string FromAsciiByteArray(byte[] bytes)
		{
			return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00079C30 File Offset: 0x00077E30
		public static byte[] ToAsciiByteArray(char[] cs)
		{
			return Encoding.ASCII.GetBytes(cs);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00079C3D File Offset: 0x00077E3D
		public static byte[] ToAsciiByteArray(string s)
		{
			return Encoding.ASCII.GetBytes(s);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00079C4A File Offset: 0x00077E4A
		public static string FromUtf8ByteArray(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00079C5B File Offset: 0x00077E5B
		public static byte[] ToUtf8ByteArray(char[] cs)
		{
			return Encoding.UTF8.GetBytes(cs);
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00079C68 File Offset: 0x00077E68
		public static byte[] ToUtf8ByteArray(string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}
	}
}
