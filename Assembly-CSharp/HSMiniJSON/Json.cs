using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HSMiniJSON
{
	// Token: 0x02000255 RID: 597
	public static class Json
	{
		// Token: 0x060012FA RID: 4858 RVA: 0x00071ED8 File Offset: 0x000700D8
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00071EE5 File Offset: 0x000700E5
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x02000876 RID: 2166
		private sealed class Parser : IDisposable
		{
			// Token: 0x06004500 RID: 17664 RVA: 0x001434A2 File Offset: 0x001416A2
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x06004501 RID: 17665 RVA: 0x001434BF File Offset: 0x001416BF
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06004502 RID: 17666 RVA: 0x001434D4 File Offset: 0x001416D4
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x06004503 RID: 17667 RVA: 0x0014350C File Offset: 0x0014170C
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06004504 RID: 17668 RVA: 0x00143520 File Offset: 0x00141720
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

			// Token: 0x06004505 RID: 17669 RVA: 0x00143588 File Offset: 0x00141788
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

			// Token: 0x06004506 RID: 17670 RVA: 0x001435D8 File Offset: 0x001417D8
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06004507 RID: 17671 RVA: 0x001435F4 File Offset: 0x001417F4
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

			// Token: 0x06004508 RID: 17672 RVA: 0x00143664 File Offset: 0x00141864
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

			// Token: 0x06004509 RID: 17673 RVA: 0x001437B4 File Offset: 0x001419B4
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

			// Token: 0x0600450A RID: 17674 RVA: 0x001437F2 File Offset: 0x001419F2
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

			// Token: 0x17000A0D RID: 2573
			// (get) Token: 0x0600450B RID: 17675 RVA: 0x0014381D File Offset: 0x00141A1D
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000A0E RID: 2574
			// (get) Token: 0x0600450C RID: 17676 RVA: 0x0014382F File Offset: 0x00141A2F
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000A0F RID: 2575
			// (get) Token: 0x0600450D RID: 17677 RVA: 0x00143844 File Offset: 0x00141A44
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

			// Token: 0x17000A10 RID: 2576
			// (get) Token: 0x0600450E RID: 17678 RVA: 0x00143888 File Offset: 0x00141A88
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

			// Token: 0x04002F16 RID: 12054
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x04002F17 RID: 12055
			private StringReader json;

			// Token: 0x02000A5E RID: 2654
			private enum TOKEN
			{
				// Token: 0x04003498 RID: 13464
				NONE,
				// Token: 0x04003499 RID: 13465
				CURLY_OPEN,
				// Token: 0x0400349A RID: 13466
				CURLY_CLOSE,
				// Token: 0x0400349B RID: 13467
				SQUARED_OPEN,
				// Token: 0x0400349C RID: 13468
				SQUARED_CLOSE,
				// Token: 0x0400349D RID: 13469
				COLON,
				// Token: 0x0400349E RID: 13470
				COMMA,
				// Token: 0x0400349F RID: 13471
				STRING,
				// Token: 0x040034A0 RID: 13472
				NUMBER,
				// Token: 0x040034A1 RID: 13473
				TRUE,
				// Token: 0x040034A2 RID: 13474
				FALSE,
				// Token: 0x040034A3 RID: 13475
				NULL
			}
		}

		// Token: 0x02000877 RID: 2167
		private sealed class Serializer
		{
			// Token: 0x0600450F RID: 17679 RVA: 0x001439AA File Offset: 0x00141BAA
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06004510 RID: 17680 RVA: 0x001439BD File Offset: 0x00141BBD
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06004511 RID: 17681 RVA: 0x001439D8 File Offset: 0x00141BD8
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

			// Token: 0x06004512 RID: 17682 RVA: 0x00143A7C File Offset: 0x00141C7C
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

			// Token: 0x06004513 RID: 17683 RVA: 0x00143B24 File Offset: 0x00141D24
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

			// Token: 0x06004514 RID: 17684 RVA: 0x00143BA4 File Offset: 0x00141DA4
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

			// Token: 0x06004515 RID: 17685 RVA: 0x00143CF8 File Offset: 0x00141EF8
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

			// Token: 0x04002F18 RID: 12056
			private StringBuilder builder;
		}
	}
}
