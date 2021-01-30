using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AmplitudeNS.MiniJSON
{
	// Token: 0x02000737 RID: 1847
	public static class Json
	{
		// Token: 0x06004075 RID: 16501 RVA: 0x0013979C File Offset: 0x0013799C
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x001397A9 File Offset: 0x001379A9
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x02000A2F RID: 2607
		private sealed class Parser : IDisposable
		{
			// Token: 0x06004A1E RID: 18974 RVA: 0x0014E278 File Offset: 0x0014C478
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06004A1F RID: 18975 RVA: 0x0014E28C File Offset: 0x0014C48C
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x06004A20 RID: 18976 RVA: 0x0014E2C4 File Offset: 0x0014C4C4
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06004A21 RID: 18977 RVA: 0x0014E2D8 File Offset: 0x0014C4D8
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

			// Token: 0x06004A22 RID: 18978 RVA: 0x0014E340 File Offset: 0x0014C540
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

			// Token: 0x06004A23 RID: 18979 RVA: 0x0014E390 File Offset: 0x0014C590
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06004A24 RID: 18980 RVA: 0x0014E3AC File Offset: 0x0014C5AC
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

			// Token: 0x06004A25 RID: 18981 RVA: 0x0014E41C File Offset: 0x0014C61C
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
									StringBuilder stringBuilder2 = new StringBuilder();
									for (int i = 0; i < 4; i++)
									{
										stringBuilder2.Append(this.NextChar);
									}
									stringBuilder.Append((char)Convert.ToInt32(stringBuilder2.ToString(), 16));
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

			// Token: 0x06004A26 RID: 18982 RVA: 0x0014E570 File Offset: 0x0014C770
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					return int.Parse(nextWord);
				}
				return float.Parse(nextWord);
			}

			// Token: 0x06004A27 RID: 18983 RVA: 0x0014E5A6 File Offset: 0x0014C7A6
			private void EatWhitespace()
			{
				while (" \t\n\r".IndexOf(this.PeekChar) != -1)
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x17000A95 RID: 2709
			// (get) Token: 0x06004A28 RID: 18984 RVA: 0x0014E5D7 File Offset: 0x0014C7D7
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000A96 RID: 2710
			// (get) Token: 0x06004A29 RID: 18985 RVA: 0x0014E5E9 File Offset: 0x0014C7E9
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000A97 RID: 2711
			// (get) Token: 0x06004A2A RID: 18986 RVA: 0x0014E5FC File Offset: 0x0014C7FC
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (" \t\n\r{}[],:\"".IndexOf(this.PeekChar) == -1)
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

			// Token: 0x17000A98 RID: 2712
			// (get) Token: 0x06004A2B RID: 18987 RVA: 0x0014E648 File Offset: 0x0014C848
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

			// Token: 0x04003477 RID: 13431
			private const string WHITE_SPACE = " \t\n\r";

			// Token: 0x04003478 RID: 13432
			private const string WORD_BREAK = " \t\n\r{}[],:\"";

			// Token: 0x04003479 RID: 13433
			private StringReader json;

			// Token: 0x02000A71 RID: 2673
			private enum TOKEN
			{
				// Token: 0x0400351C RID: 13596
				NONE,
				// Token: 0x0400351D RID: 13597
				CURLY_OPEN,
				// Token: 0x0400351E RID: 13598
				CURLY_CLOSE,
				// Token: 0x0400351F RID: 13599
				SQUARED_OPEN,
				// Token: 0x04003520 RID: 13600
				SQUARED_CLOSE,
				// Token: 0x04003521 RID: 13601
				COLON,
				// Token: 0x04003522 RID: 13602
				COMMA,
				// Token: 0x04003523 RID: 13603
				STRING,
				// Token: 0x04003524 RID: 13604
				NUMBER,
				// Token: 0x04003525 RID: 13605
				TRUE,
				// Token: 0x04003526 RID: 13606
				FALSE,
				// Token: 0x04003527 RID: 13607
				NULL
			}
		}

		// Token: 0x02000A30 RID: 2608
		private sealed class Serializer
		{
			// Token: 0x06004A2C RID: 18988 RVA: 0x0014E76A File Offset: 0x0014C96A
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06004A2D RID: 18989 RVA: 0x0014E77D File Offset: 0x0014C97D
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06004A2E RID: 18990 RVA: 0x0014E798 File Offset: 0x0014C998
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
					this.builder.Append(value.ToString().ToLower());
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
					this.SerializeString(value.ToString());
					return;
				}
				this.SerializeOther(value);
			}

			// Token: 0x06004A2F RID: 18991 RVA: 0x0014E82C File Offset: 0x0014CA2C
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

			// Token: 0x06004A30 RID: 18992 RVA: 0x0014E8D4 File Offset: 0x0014CAD4
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

			// Token: 0x06004A31 RID: 18993 RVA: 0x0014E954 File Offset: 0x0014CB54
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
						goto IL_DD;
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
								goto IL_DD;
							}
							this.builder.Append("\\\\");
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					}
					IL_123:
					i++;
					continue;
					IL_DD:
					int num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						this.builder.Append(c);
						goto IL_123;
					}
					this.builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
					goto IL_123;
				}
				this.builder.Append('"');
			}

			// Token: 0x06004A32 RID: 18994 RVA: 0x0014EAA0 File Offset: 0x0014CCA0
			private void SerializeOther(object value)
			{
				if (value is float || value is int || value is uint || value is long || value is double || value is sbyte || value is byte || value is short || value is ushort || value is ulong || value is decimal)
				{
					this.builder.Append(value.ToString());
					return;
				}
				this.SerializeString(value.ToString());
			}

			// Token: 0x0400347A RID: 13434
			private StringBuilder builder;
		}
	}
}
