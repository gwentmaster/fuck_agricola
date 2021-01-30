using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x02000270 RID: 624
	public class JsonMockWrapper : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0002A062 File Offset: 0x00028262
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00074B8C File Offset: 0x00072D8C
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0002A062 File Offset: 0x00028262
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0002A062 File Offset: 0x00028262
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00074B97 File Offset: 0x00072D97
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0003860A File Offset: 0x0003680A
		public string GetString()
		{
			return "";
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00003022 File Offset: 0x00001222
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00003022 File Offset: 0x00001222
		public void SetDouble(double val)
		{
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00003022 File Offset: 0x00001222
		public void SetInt(int val)
		{
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00003022 File Offset: 0x00001222
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00003022 File Offset: 0x00001222
		public void SetLong(long val)
		{
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00003022 File Offset: 0x00001222
		public void SetString(string val)
		{
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0003860A File Offset: 0x0003680A
		public string ToJson()
		{
			return "";
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00003022 File Offset: 0x00001222
		public void ToJson(JsonWriter writer)
		{
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0000900B File Offset: 0x0000720B
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x0000900B File Offset: 0x0000720B
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019B RID: 411
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0002A062 File Offset: 0x00028262
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00003022 File Offset: 0x00001222
		void IList.Clear()
		{
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0002A062 File Offset: 0x00028262
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00074B9B File Offset: 0x00072D9B
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00003022 File Offset: 0x00001222
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00003022 File Offset: 0x00001222
		void IList.Remove(object value)
		{
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00003022 File Offset: 0x00001222
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x0002A062 File Offset: 0x00028262
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x0002A062 File Offset: 0x00028262
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0000301F File Offset: 0x0000121F
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00003022 File Offset: 0x00001222
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0000301F File Offset: 0x0000121F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x0000900B File Offset: 0x0000720B
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0000900B File Offset: 0x0000720B
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0000301F File Offset: 0x0000121F
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0000301F File Offset: 0x0000121F
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001A3 RID: 419
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00003022 File Offset: 0x00001222
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00003022 File Offset: 0x00001222
		void IDictionary.Clear()
		{
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0002A062 File Offset: 0x00028262
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00003022 File Offset: 0x00001222
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0000301F File Offset: 0x0000121F
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x170001A4 RID: 420
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0000301F File Offset: 0x0000121F
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00003022 File Offset: 0x00001222
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00003022 File Offset: 0x00001222
		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
