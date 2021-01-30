using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x02000264 RID: 612
	public class JsonData : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary, IEquatable<JsonData>
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x000726EE File Offset: 0x000708EE
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x000726FB File Offset: 0x000708FB
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00072706 File Offset: 0x00070906
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00072711 File Offset: 0x00070911
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0007271C File Offset: 0x0007091C
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x00072727 File Offset: 0x00070927
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x00072732 File Offset: 0x00070932
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x0007273D File Offset: 0x0007093D
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00072748 File Offset: 0x00070948
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x0007275C File Offset: 0x0007095C
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x00072764 File Offset: 0x00070964
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x00072771 File Offset: 0x00070971
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x0007277E File Offset: 0x0007097E
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x0007278B File Offset: 0x0007098B
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x00072798 File Offset: 0x00070998
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x00072800 File Offset: 0x00070A00
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x00072868 File Offset: 0x00070A68
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00072870 File Offset: 0x00070A70
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x00072878 File Offset: 0x00070A78
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00072880 File Offset: 0x00070A80
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x00072888 File Offset: 0x00070A88
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x00072890 File Offset: 0x00070A90
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x00072898 File Offset: 0x00070A98
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x000728A0 File Offset: 0x00070AA0
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x000728AD File Offset: 0x00070AAD
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x17000183 RID: 387
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData value2 = this.ToJsonData(value);
				this[(string)key] = value2;
			}
		}

		// Token: 0x17000184 RID: 388
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData value2 = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = value2;
				KeyValuePair<string, JsonData> value3 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value2);
				this.object_list[idx] = value3;
			}
		}

		// Token: 0x17000185 RID: 389
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData value2 = this.ToJsonData(value);
				this[index] = value2;
			}
		}

		// Token: 0x17000186 RID: 390
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x17000187 RID: 391
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> value2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = value2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00003425 File Offset: 0x00001625
		public JsonData()
		{
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00072B0B File Offset: 0x00070D0B
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00072B21 File Offset: 0x00070D21
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00072B37 File Offset: 0x00070D37
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00072B4D File Offset: 0x00070D4D
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00072B64 File Offset: 0x00070D64
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00072C0D File Offset: 0x00070E0D
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00072C23 File Offset: 0x00070E23
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00072C2B File Offset: 0x00070E2B
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00072C33 File Offset: 0x00070E33
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00072C3B File Offset: 0x00070E3B
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00072C43 File Offset: 0x00070E43
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00072C4B File Offset: 0x00070E4B
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00072C67 File Offset: 0x00070E67
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00072C83 File Offset: 0x00070E83
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00072C9F File Offset: 0x00070E9F
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00072CBB File Offset: 0x00070EBB
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00072CD7 File Offset: 0x00070ED7
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00072CE8 File Offset: 0x00070EE8
		void IDictionary.Add(object key, object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, value2);
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
			this.object_list.Add(item);
			this.json = null;
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00072D2B File Offset: 0x00070F2B
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00072D4A File Offset: 0x00070F4A
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00072D58 File Offset: 0x00070F58
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00072D60 File Offset: 0x00070F60
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00072DC5 File Offset: 0x00070FC5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00072DD2 File Offset: 0x00070FD2
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00072DEE File Offset: 0x00070FEE
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00072E0A File Offset: 0x0007100A
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00072E26 File Offset: 0x00071026
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00072E42 File Offset: 0x00071042
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00072E5E File Offset: 0x0007105E
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00072E75 File Offset: 0x00071075
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00072E8C File Offset: 0x0007108C
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00072EA3 File Offset: 0x000710A3
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00072EBA File Offset: 0x000710BA
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00072ED1 File Offset: 0x000710D1
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00072ED9 File Offset: 0x000710D9
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00072EE2 File Offset: 0x000710E2
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00072EEB File Offset: 0x000710EB
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00072EFF File Offset: 0x000710FF
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00072F0D File Offset: 0x0007110D
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00072F1B File Offset: 0x0007111B
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00072F31 File Offset: 0x00071131
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00072F46 File Offset: 0x00071146
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00072F5B File Offset: 0x0007115B
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00072F74 File Offset: 0x00071174
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData value2 = this.ToJsonData(value);
			this[text] = value2;
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
			this.object_list.Insert(idx, item);
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00072FB0 File Offset: 0x000711B0
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00072FF0 File Offset: 0x000711F0
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00073028 File Offset: 0x00071228
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00073088 File Offset: 0x00071288
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x000730DA File Offset: 0x000712DA
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x000730F8 File Offset: 0x000712F8
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				foreach (object obj2 in obj)
				{
					JsonData.WriteJson((JsonData)obj2, writer);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				foreach (object obj3 in obj)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					writer.WritePropertyName((string)dictionaryEntry.Key);
					JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00073248 File Offset: 0x00071448
		public int Add(object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(value2);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00073270 File Offset: 0x00071470
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00073290 File Offset: 0x00071490
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00073365 File Offset: 0x00071565
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00073370 File Offset: 0x00071570
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00073410 File Offset: 0x00071610
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0007345C File Offset: 0x0007165C
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00073488 File Offset: 0x00071688
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x040012FD RID: 4861
		private IList<JsonData> inst_array;

		// Token: 0x040012FE RID: 4862
		private bool inst_boolean;

		// Token: 0x040012FF RID: 4863
		private double inst_double;

		// Token: 0x04001300 RID: 4864
		private int inst_int;

		// Token: 0x04001301 RID: 4865
		private long inst_long;

		// Token: 0x04001302 RID: 4866
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x04001303 RID: 4867
		private string inst_string;

		// Token: 0x04001304 RID: 4868
		private string json;

		// Token: 0x04001305 RID: 4869
		private JsonType type;

		// Token: 0x04001306 RID: 4870
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
