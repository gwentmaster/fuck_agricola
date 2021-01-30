using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fabric.Internal.ThirdParty.MiniJSON
{
	// Token: 0x02000259 RID: 601
	public static class Json
	{
		// Token: 0x06001304 RID: 4868 RVA: 0x00071FE7 File Offset: 0x000701E7
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00071FF4 File Offset: 0x000701F4
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x02000878 RID: 2168
		private sealed class Parser : IDisposable
		{
			// Token: 0x06004516 RID: 17686 RVA: 0x001434A2 File Offset: 0x001416A2
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x06004517 RID: 17687 RVA: 0x00143DB7 File Offset: 0x00141FB7
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06004518 RID: 17688 RVA: 0x00143DCC File Offset: 0x00141FCC
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x06004519 RID: 17689 RVA: 0x00143E04 File Offset: 0x00142004
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x0600451A RID: 17690 RVA: 0x00143E18 File Offset: 0x00142018
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

			// Token: 0x0600451B RID: 17691 RVA: 0x00143E80 File Offset: 0x00142080
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

			// Token: 0x0600451C RID: 17692 RVA: 0x00143ED0 File Offset: 0x001420D0
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x0600451D RID: 17693 RVA: 0x00143EEC File Offset: 0x001420EC
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

			// Token: 0x0600451E RID: 17694 RVA: 0x00143F5C File Offset: 0x0014215C
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

			// Token: 0x0600451F RID: 17695 RVA: 0x001440AC File Offset: 0x001422AC
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

			// Token: 0x06004520 RID: 17696 RVA: 0x001440EA File Offset: 0x001422EA
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

			// Token: 0x17000A11 RID: 2577
			// (get) Token: 0x06004521 RID: 17697 RVA: 0x00144115 File Offset: 0x00142315
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000A12 RID: 2578
			// (get) Token: 0x06004522 RID: 17698 RVA: 0x00144127 File Offset: 0x00142327
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000A13 RID: 2579
			// (get) Token: 0x06004523 RID: 17699 RVA: 0x0014413C File Offset: 0x0014233C
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

			// Token: 0x17000A14 RID: 2580
			// (get) Token: 0x06004524 RID: 17700 RVA: 0x00144180 File Offset: 0x00142380
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

			// Token: 0x04002F19 RID: 12057
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x04002F1A RID: 12058
			private StringReader json;

			// Token: 0x02000A5F RID: 2655
			private enum TOKEN
			{
				// Token: 0x040034A5 RID: 13477
				NONE,
				// Token: 0x040034A6 RID: 13478
				CURLY_OPEN,
				// Token: 0x040034A7 RID: 13479
				CURLY_CLOSE,
				// Token: 0x040034A8 RID: 13480
				SQUARED_OPEN,
				// Token: 0x040034A9 RID: 13481
				SQUARED_CLOSE,
				// Token: 0x040034AA RID: 13482
				COLON,
				// Token: 0x040034AB RID: 13483
				COMMA,
				// Token: 0x040034AC RID: 13484
				STRING,
				// Token: 0x040034AD RID: 13485
				NUMBER,
				// Token: 0x040034AE RID: 13486
				TRUE,
				// Token: 0x040034AF RID: 13487
				FALSE,
				// Token: 0x040034B0 RID: 13488
				NULL
			}
		}

		// Token: 0x02000879 RID: 2169
		private sealed class Serializer
		{
			// Token: 0x06004525 RID: 17701 RVA: 0x001442A2 File Offset: 0x001424A2
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06004526 RID: 17702 RVA: 0x001442B5 File Offset: 0x001424B5
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06004527 RID: 17703 RVA: 0x001442D0 File Offset: 0x001424D0
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

			// Token: 0x06004528 RID: 17704 RVA: 0x00144374 File Offset: 0x00142574
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

			// Token: 0x06004529 RID: 17705 RVA: 0x0014441C File Offset: 0x0014261C
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

			// Token: 0x0600452A RID: 17706 RVA: 0x0014449C File Offset: 0x0014269C
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

			// Token: 0x0600452B RID: 17707 RVA: 0x001445F0 File Offset: 0x001427F0
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

			// Token: 0x04002F1B RID: 12059
			private StringBuilder builder;
		}
	}
}
