using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F0 RID: 1520
	public static class Extensions
	{
		// Token: 0x060037C5 RID: 14277 RVA: 0x00112574 File Offset: 0x00110774
		public static string AsciiToString(this byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder(bytes.Length);
			foreach (byte b in bytes)
			{
				stringBuilder.Append((char)((b <= 127) ? b : 63));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x001125B8 File Offset: 0x001107B8
		public static byte[] GetASCIIBytes(this string str)
		{
			byte[] array = new byte[str.Length];
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				array[i] = (byte)((c < '\u0080') ? c : '?');
			}
			return array;
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x001125FC File Offset: 0x001107FC
		public static void SendAsASCII(this BinaryWriter stream, string str)
		{
			foreach (char c in str)
			{
				stream.Write((byte)((c < '\u0080') ? c : '?'));
			}
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x00112636 File Offset: 0x00110836
		public static void WriteLine(this FileStream fs)
		{
			fs.Write(HTTPRequest.EOL, 0, 2);
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x00112648 File Offset: 0x00110848
		public static void WriteLine(this FileStream fs, string line)
		{
			byte[] asciibytes = line.GetASCIIBytes();
			fs.Write(asciibytes, 0, asciibytes.Length);
			fs.WriteLine();
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x00112670 File Offset: 0x00110870
		public static void WriteLine(this FileStream fs, string format, params object[] values)
		{
			byte[] asciibytes = string.Format(format, values).GetASCIIBytes();
			fs.Write(asciibytes, 0, asciibytes.Length);
			fs.WriteLine();
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x0011269C File Offset: 0x0011089C
		public static string GetRequestPathAndQueryURL(this Uri uri)
		{
			string text = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped);
			if (string.IsNullOrEmpty(text))
			{
				text = "/";
			}
			return text;
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x001126C4 File Offset: 0x001108C4
		public static string[] FindOption(this string str, string option)
		{
			string[] array = str.ToLower().Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			option = option.ToLower();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Contains(option))
				{
					return array[i].Split(new char[]
					{
						'='
					}, StringSplitOptions.RemoveEmptyEntries);
				}
			}
			return null;
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x00080502 File Offset: 0x0007E702
		public static void WriteArray(this Stream stream, byte[] array)
		{
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x00112720 File Offset: 0x00110920
		public static int ToInt32(this string str, int defaultValue = 0)
		{
			if (str == null)
			{
				return defaultValue;
			}
			int result;
			try
			{
				result = int.Parse(str);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x00112754 File Offset: 0x00110954
		public static long ToInt64(this string str, long defaultValue = 0L)
		{
			if (str == null)
			{
				return defaultValue;
			}
			long result;
			try
			{
				result = long.Parse(str);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x00112788 File Offset: 0x00110988
		public static DateTime ToDateTime(this string str, DateTime defaultValue = default(DateTime))
		{
			if (str == null)
			{
				return defaultValue;
			}
			DateTime result;
			try
			{
				DateTime.TryParse(str, out defaultValue);
				result = defaultValue.ToUniversalTime();
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x001127C4 File Offset: 0x001109C4
		public static string ToStrOrEmpty(this string str)
		{
			if (str == null)
			{
				return string.Empty;
			}
			return str;
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x001127D0 File Offset: 0x001109D0
		public static string CalculateMD5Hash(this string input)
		{
			return input.GetASCIIBytes().CalculateMD5Hash();
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x001127E0 File Offset: 0x001109E0
		public static string CalculateMD5Hash(this byte[] input)
		{
			byte[] array = MD5.Create().ComputeHash(input);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0011282C File Offset: 0x00110A2C
		internal static string Read(this string str, ref int pos, char block, bool needResult = true)
		{
			return str.Read(ref pos, (char ch) => ch != block, needResult);
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x0011285C File Offset: 0x00110A5C
		internal static string Read(this string str, ref int pos, Func<char, bool> block, bool needResult = true)
		{
			if (pos >= str.Length)
			{
				return string.Empty;
			}
			str.SkipWhiteSpace(ref pos);
			int num = pos;
			while (pos < str.Length && block(str[pos]))
			{
				pos++;
			}
			string result = needResult ? str.Substring(num, pos - num) : null;
			pos++;
			return result;
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x001128BC File Offset: 0x00110ABC
		internal static string ReadPossibleQuotedText(this string str, ref int pos)
		{
			string result = string.Empty;
			if (str == null)
			{
				return result;
			}
			if (str[pos] == '"')
			{
				str.Read(ref pos, '"', false);
				result = str.Read(ref pos, '"', true);
				str.Read(ref pos, ',', false);
			}
			else
			{
				result = str.Read(ref pos, (char ch) => ch != ',' && ch != ';', true);
			}
			return result;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x0011292C File Offset: 0x00110B2C
		internal static void SkipWhiteSpace(this string str, ref int pos)
		{
			if (pos >= str.Length)
			{
				return;
			}
			while (pos < str.Length && char.IsWhiteSpace(str[pos]))
			{
				pos++;
			}
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x00112958 File Offset: 0x00110B58
		internal static string TrimAndLower(this string str)
		{
			if (str == null)
			{
				return null;
			}
			char[] array = new char[str.Length];
			int length = 0;
			foreach (char c in str)
			{
				if (!char.IsWhiteSpace(c) && !char.IsControl(c))
				{
					array[length++] = char.ToLowerInvariant(c);
				}
			}
			return new string(array, 0, length);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x001129B8 File Offset: 0x00110BB8
		internal static char? Peek(this string str, int pos)
		{
			if (pos < 0 || pos >= str.Length)
			{
				return null;
			}
			return new char?(str[pos]);
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x001129E8 File Offset: 0x00110BE8
		internal static List<HeaderValue> ParseOptionalHeader(this string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str == null)
			{
				return list;
			}
			int i = 0;
			while (i < str.Length)
			{
				HeaderValue headerValue = new HeaderValue(str.Read(ref i, (char ch) => ch != '=' && ch != ',', true).TrimAndLower());
				if (str[i - 1] == '=')
				{
					headerValue.Value = str.ReadPossibleQuotedText(ref i);
				}
				list.Add(headerValue);
			}
			return list;
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x00112A64 File Offset: 0x00110C64
		internal static List<HeaderValue> ParseQualityParams(this string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str == null)
			{
				return list;
			}
			int i = 0;
			while (i < str.Length)
			{
				HeaderValue headerValue = new HeaderValue(str.Read(ref i, (char ch) => ch != ',' && ch != ';', true).TrimAndLower());
				if (str[i - 1] == ';')
				{
					str.Read(ref i, '=', false);
					headerValue.Value = str.Read(ref i, ',', true);
				}
				list.Add(headerValue);
			}
			return list;
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x00112AF0 File Offset: 0x00110CF0
		public static void ReadBuffer(this Stream stream, byte[] buffer)
		{
			int num = 0;
			for (;;)
			{
				int num2 = stream.Read(buffer, num, buffer.Length - num);
				if (num2 <= 0)
				{
					break;
				}
				num += num2;
				if (num >= buffer.Length)
				{
					return;
				}
			}
			throw ExceptionHelper.ServerClosedTCPStream();
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x00080502 File Offset: 0x0007E702
		public static void WriteAll(this MemoryStream ms, byte[] buffer)
		{
			ms.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x00112B24 File Offset: 0x00110D24
		public static void WriteString(this MemoryStream ms, string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			ms.WriteAll(bytes);
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x00112B44 File Offset: 0x00110D44
		public static void WriteLine(this MemoryStream ms)
		{
			ms.WriteAll(HTTPRequest.EOL);
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x00112B51 File Offset: 0x00110D51
		public static void WriteLine(this MemoryStream ms, string str)
		{
			ms.WriteString(str);
			ms.WriteLine();
		}
	}
}
