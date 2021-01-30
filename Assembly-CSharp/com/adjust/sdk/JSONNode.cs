using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace com.adjust.sdk
{
	// Token: 0x02000739 RID: 1849
	public class JSONNode
	{
		// Token: 0x06004077 RID: 16503 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x1700091E RID: 2334
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x1700091F RID: 2335
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x0600407C RID: 16508 RVA: 0x0003860A File Offset: 0x0003680A
		// (set) Token: 0x0600407D RID: 16509 RVA: 0x00003022 File Offset: 0x00001222
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600407E RID: 16510 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x001397B1 File Offset: 0x001379B1
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x00055DC8 File Offset: 0x00053FC8
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06004083 RID: 16515 RVA: 0x001397BF File Offset: 0x001379BF
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x001397C8 File Offset: 0x001379C8
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode jsonnode in this.Childs)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChilds)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x00055DE4 File Offset: 0x00053FE4
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x00055DE4 File Offset: 0x00053FE4
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06004087 RID: 16519 RVA: 0x001397D8 File Offset: 0x001379D8
		// (set) Token: 0x06004088 RID: 16520 RVA: 0x001397F9 File Offset: 0x001379F9
		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06004089 RID: 16521 RVA: 0x00139808 File Offset: 0x00137A08
		// (set) Token: 0x0600408A RID: 16522 RVA: 0x00139831 File Offset: 0x00137A31
		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x0600408B RID: 16523 RVA: 0x00139840 File Offset: 0x00137A40
		// (set) Token: 0x0600408C RID: 16524 RVA: 0x00139871 File Offset: 0x00137A71
		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600408D RID: 16525 RVA: 0x00139880 File Offset: 0x00137A80
		// (set) Token: 0x0600408E RID: 16526 RVA: 0x001398AE File Offset: 0x00137AAE
		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600408F RID: 16527 RVA: 0x001398C5 File Offset: 0x00137AC5
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06004090 RID: 16528 RVA: 0x001398CD File Offset: 0x00137ACD
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x001398D5 File Offset: 0x00137AD5
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x001398DD File Offset: 0x00137ADD
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x001398F0 File Offset: 0x00137AF0
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x00139903 File Offset: 0x00137B03
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x00055F23 File Offset: 0x00054123
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x00055F29 File Offset: 0x00054129
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x00139910 File Offset: 0x00137B10
		internal static string Escape(string aText)
		{
			string text = "";
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				case '\v':
					goto IL_A3;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_A3;
						}
						text += "\\\\";
					}
					else
					{
						text += "\\\"";
					}
					break;
				}
				IL_B1:
				i++;
				continue;
				IL_A3:
				text += c.ToString();
				goto IL_B1;
			}
			return text;
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x001399E0 File Offset: 0x00137BE0
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = "";
			string text2 = "";
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_429;
						case '\v':
						case '\f':
							goto IL_412;
						default:
							if (c != ' ')
							{
								goto IL_412;
							}
							break;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_412;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
						else
						{
							if (text != "")
							{
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(text);
								}
								else if (text2 != "")
								{
									jsonnode.Add(text2, text);
								}
							}
							text2 = "";
							text = "";
						}
					}
					else
					{
						flag = !flag;
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									text += aJSON[i].ToString();
									goto IL_429;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != "")
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = "";
								text = "";
								jsonnode = stack.Peek();
								goto IL_429;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											text += "\b";
											goto IL_429;
										}
										if (c2 == 'f')
										{
											text += "\f";
											goto IL_429;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											text += "\n";
											goto IL_429;
										}
										switch (c2)
										{
										case 'r':
											text += "\r";
											goto IL_429;
										case 't':
											text += "\t";
											goto IL_429;
										case 'u':
										{
											string s = aJSON.Substring(i + 1, 4);
											text += ((char)int.Parse(s, NumberStyles.AllowHexSpecifier)).ToString();
											i += 4;
											goto IL_429;
										}
										}
									}
									text += c2.ToString();
									goto IL_429;
								}
								goto IL_429;
							case ']':
								break;
							default:
								goto IL_412;
							}
						}
						else
						{
							if (flag)
							{
								text += aJSON[i].ToString();
								goto IL_429;
							}
							text2 = text;
							text = "";
							goto IL_429;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_412;
						}
					}
					else
					{
						if (flag)
						{
							text += aJSON[i].ToString();
							goto IL_429;
						}
						stack.Push(new JSONClass());
						if (jsonnode != null)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stack.Peek());
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, stack.Peek());
							}
						}
						text2 = "";
						text = "";
						jsonnode = stack.Peek();
						goto IL_429;
					}
					if (flag)
					{
						text += aJSON[i].ToString();
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != "")
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(text);
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, text);
							}
						}
						text2 = "";
						text = "";
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_429:
				i++;
				continue;
				IL_412:
				text += aJSON[i].ToString();
				goto IL_429;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x00139E38 File Offset: 0x00138038
		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x00056477 File Offset: 0x00054677
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x00056477 File Offset: 0x00054677
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x00056477 File Offset: 0x00054677
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x00139E54 File Offset: 0x00138054
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string aKey = aReader.ReadString();
					JSONNode aItem = JSONNode.Deserialize(aReader);
					jsonclass.Add(aKey, aItem);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x00056477 File Offset: 0x00054677
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x00056477 File Offset: 0x00054677
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x00056477 File Offset: 0x00054677
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x00139F48 File Offset: 0x00138148
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x00139F80 File Offset: 0x00138180
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
