using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace UTNotifications
{
	// Token: 0x02000150 RID: 336
	public class JSONNode
	{
		// Token: 0x06000CEE RID: 3310 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000059 RID: 89
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

		// Token: 0x1700005A RID: 90
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0003860A File Offset: 0x0003680A
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x00003022 File Offset: 0x00001222
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

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00055DBA File Offset: 0x00053FBA
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00055DC8 File Offset: 0x00053FC8
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00055DCB File Offset: 0x00053FCB
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00055DD4 File Offset: 0x00053FD4
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

		// Token: 0x06000CFC RID: 3324 RVA: 0x00055DE4 File Offset: 0x00053FE4
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00055DE4 File Offset: 0x00053FE4
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00055DEC File Offset: 0x00053FEC
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x00055E0D File Offset: 0x0005400D
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00055E1C File Offset: 0x0005401C
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00055E45 File Offset: 0x00054045
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00055E54 File Offset: 0x00054054
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x00055E85 File Offset: 0x00054085
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00055E94 File Offset: 0x00054094
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x00055EC2 File Offset: 0x000540C2
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00055ED9 File Offset: 0x000540D9
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00055EE1 File Offset: 0x000540E1
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00055EE9 File Offset: 0x000540E9
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00055EF1 File Offset: 0x000540F1
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00055F04 File Offset: 0x00054104
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00055F17 File Offset: 0x00054117
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00055F23 File Offset: 0x00054123
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00055F29 File Offset: 0x00054129
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00055F34 File Offset: 0x00054134
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

		// Token: 0x06000D0F RID: 3343 RVA: 0x00056004 File Offset: 0x00054204
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

		// Token: 0x06000D10 RID: 3344 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0005645C File Offset: 0x0005465C
		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00056477 File Offset: 0x00054677
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00056477 File Offset: 0x00054677
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00056477 File Offset: 0x00054677
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00056484 File Offset: 0x00054684
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000564D4 File Offset: 0x000546D4
		public string SaveToBase64()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00056520 File Offset: 0x00054720
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

		// Token: 0x06000D18 RID: 3352 RVA: 0x00056477 File Offset: 0x00054677
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00056477 File Offset: 0x00054677
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00056477 File Offset: 0x00054677
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00056614 File Offset: 0x00054814
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0005664C File Offset: 0x0005484C
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode result;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				result = JSONNode.LoadFromStream(fileStream);
			}
			return result;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00056684 File Offset: 0x00054884
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
