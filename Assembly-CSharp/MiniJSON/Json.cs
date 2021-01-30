using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniJSON
{
	// Token: 0x0200061C RID: 1564
	public static class Json
	{
		// Token: 0x06003991 RID: 14737 RVA: 0x0011E5CE File Offset: 0x0011C7CE
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x0011E5DB File Offset: 0x0011C7DB
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x02000910 RID: 2320
		private sealed class Parser : IDisposable
		{
			// Token: 0x060046F2 RID: 18162 RVA: 0x001485C4 File Offset: 0x001467C4
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x060046F3 RID: 18163 RVA: 0x001485E1 File Offset: 0x001467E1
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x060046F4 RID: 18164 RVA: 0x001485F8 File Offset: 0x001467F8
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x060046F5 RID: 18165 RVA: 0x00148630 File Offset: 0x00146830
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x060046F6 RID: 18166 RVA: 0x00148644 File Offset: 0x00146844
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						break;
					}
					if (nextToken == Json.Parser.TOKEN.CURLY_CLOSE)
					{
						return dictionary;
					}
					if (nextToken != Json.Parser.TOKEN.COMMA)
					{
						string text = this.ParseString();
						if (text == null)
						{
							goto Block_4;
						}
						if (this.NextToken != Json.Parser.TOKEN.COLON)
						{
							goto Block_5;
						}
						this.json.Read();
						dictionary[text] = this.ParseValue();
					}
				}
				return null;
				Block_4:
				return null;
				Block_5:
				return null;
			}

			// Token: 0x060046F7 RID: 18167 RVA: 0x001486AC File Offset: 0x001468AC
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						return null;
					}
					if (nextToken != Json.Parser.TOKEN.SQUARED_CLOSE)
					{
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							object item = this.ParseByToken(nextToken);
							list.Add(item);
						}
					}
					else
					{
						flag = false;
					}
				}
				return list;
			}

			// Token: 0x060046F8 RID: 18168 RVA: 0x001486FC File Offset: 0x001468FC
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x060046F9 RID: 18169 RVA: 0x00148718 File Offset: 0x00146918
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case Json.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x060046FA RID: 18170 RVA: 0x00148788 File Offset: 0x00146988
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char nextChar = this.NextChar;
					if (nextChar != '"')
					{
						if (nextChar != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							if (nextChar <= '\\')
							{
								if (nextChar == '"' || nextChar == '/' || nextChar == '\\')
								{
									stringBuilder.Append(nextChar);
								}
							}
							else if (nextChar <= 'f')
							{
								if (nextChar != 'b')
								{
									if (nextChar == 'f')
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
							}
							else if (nextChar != 'n')
							{
								switch (nextChar)
								{
								case 'r':
									stringBuilder.Append('\r');
									break;
								case 't':
									stringBuilder.Append('\t');
									break;
								case 'u':
								{
									char[] array = new char[4];
									for (int i = 0; i < 4; i++)
									{
										array[i] = this.NextChar;
									}
									stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
									break;
								}
								}
							}
							else
							{
								stringBuilder.Append('\n');
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060046FB RID: 18171 RVA: 0x001488D8 File Offset: 0x00146AD8
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x060046FC RID: 18172 RVA: 0x00148916 File Offset: 0x00146B16
			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x17000A19 RID: 2585
			// (get) Token: 0x060046FD RID: 18173 RVA: 0x00148941 File Offset: 0x00146B41
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000A1A RID: 2586
			// (get) Token: 0x060046FE RID: 18174 RVA: 0x00148953 File Offset: 0x00146B53
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000A1B RID: 2587
			// (get) Token: 0x060046FF RID: 18175 RVA: 0x00148968 File Offset: 0x00146B68
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!Json.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x17000A1C RID: 2588
			// (get) Token: 0x06004700 RID: 18176 RVA: 0x001489AC File Offset: 0x00146BAC
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return Json.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					if (peekChar <= '[')
					{
						switch (peekChar)
						{
						case '"':
							return Json.Parser.TOKEN.STRING;
						case '#':
						case '$':
						case '%':
						case '&':
						case '\'':
						case '(':
						case ')':
						case '*':
						case '+':
						case '.':
						case '/':
							break;
						case ',':
							this.json.Read();
							return Json.Parser.TOKEN.COMMA;
						case '-':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							return Json.Parser.TOKEN.NUMBER;
						case ':':
							return Json.Parser.TOKEN.COLON;
						default:
							if (peekChar == '[')
							{
								return Json.Parser.TOKEN.SQUARED_OPEN;
							}
							break;
						}
					}
					else
					{
						if (peekChar == ']')
						{
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
						if (peekChar == '{')
						{
							return Json.Parser.TOKEN.CURLY_OPEN;
						}
						if (peekChar == '}')
						{
							this.json.Read();
							return Json.Parser.TOKEN.CURLY_CLOSE;
						}
					}
					string nextWord = this.NextWord;
					if (nextWord == "false")
					{
						return Json.Parser.TOKEN.FALSE;
					}
					if (nextWord == "true")
					{
						return Json.Parser.TOKEN.TRUE;
					}
					if (!(nextWord == "null"))
					{
						return Json.Parser.TOKEN.NONE;
					}
					return Json.Parser.TOKEN.NULL;
				}
			}

			// Token: 0x04003077 RID: 12407
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x04003078 RID: 12408
			private StringReader json;

			// Token: 0x02000A61 RID: 2657
			private enum TOKEN
			{
				// Token: 0x040034B3 RID: 13491
				NONE,
				// Token: 0x040034B4 RID: 13492
				CURLY_OPEN,
				// Token: 0x040034B5 RID: 13493
				CURLY_CLOSE,
				// Token: 0x040034B6 RID: 13494
				SQUARED_OPEN,
				// Token: 0x040034B7 RID: 13495
				SQUARED_CLOSE,
				// Token: 0x040034B8 RID: 13496
				COLON,
				// Token: 0x040034B9 RID: 13497
				COMMA,
				// Token: 0x040034BA RID: 13498
				STRING,
				// Token: 0x040034BB RID: 13499
				NUMBER,
				// Token: 0x040034BC RID: 13500
				TRUE,
				// Token: 0x040034BD RID: 13501
				FALSE,
				// Token: 0x040034BE RID: 13502
				NULL
			}
		}

		// Token: 0x02000911 RID: 2321
		private sealed class Serializer
		{
			// Token: 0x06004701 RID: 18177 RVA: 0x00148ACE File Offset: 0x00146CCE
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06004702 RID: 18178 RVA: 0x00148AE1 File Offset: 0x00146CE1
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06004703 RID: 18179 RVA: 0x00148AFC File Offset: 0x00146CFC
			private void SerializeValue(object value)
			{
				if (value == null)
				{
					this.builder.Append("null");
					return;
				}
				string str;
				if ((str = (value as string)) != null)
				{
					this.SerializeString(str);
					return;
				}
				if (value is bool)
				{
					this.builder.Append(((bool)value) ? "true" : "false");
					return;
				}
				IList anArray;
				if ((anArray = (value as IList)) != null)
				{
					this.SerializeArray(anArray);
					return;
				}
				IDictionary obj;
				if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj);
					return;
				}
				if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
					return;
				}
				this.SerializeOther(value);
			}

			// Token: 0x06004704 RID: 18180 RVA: 0x00148BA0 File Offset: 0x00146DA0
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (object obj2 in obj.Keys)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x06004705 RID: 18181 RVA: 0x00148C48 File Offset: 0x00146E48
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object value in anArray)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x06004706 RID: 18182 RVA: 0x00148CC8 File Offset: 0x00146EC8
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				int i = 0;
				while (i < array.Length)
				{
					char c = array[i];
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					case '\v':
						goto IL_E0;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								goto IL_E0;
							}
							this.builder.Append("\\\\");
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					}
					IL_129:
					i++;
					continue;
					IL_E0:
					int num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						this.builder.Append(c);
						goto IL_129;
					}
					this.builder.Append("\\u");
					this.builder.Append(num.ToString("x4"));
					goto IL_129;
				}
				this.builder.Append('"');
			}

			// Token: 0x06004707 RID: 18183 RVA: 0x00148E1C File Offset: 0x0014701C
			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R"));
					return;
				}
				if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
					return;
				}
				if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R"));
					return;
				}
				this.SerializeString(value.ToString());
			}

			// Token: 0x04003079 RID: 12409
			private StringBuilder builder;
		}
	}
}
