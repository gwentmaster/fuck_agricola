using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace com.adjust.sdk
{
	// Token: 0x0200073B RID: 1851
	public class JSONClass : JSONNode, IEnumerable
	{
		// Token: 0x1700092E RID: 2350
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
					return;
				}
				this.m_Dict.Add(aKey, value);
			}
		}

		// Token: 0x1700092F RID: 2351
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x0013A2C2 File Offset: 0x001384C2
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0013A2D0 File Offset: 0x001384D0
		public override void Add(string aKey, JSONNode aItem)
		{
			if (string.IsNullOrEmpty(aKey))
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
				return;
			}
			if (this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict[aKey] = aItem;
				return;
			}
			this.m_Dict.Add(aKey, aItem);
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x0013A32E File Offset: 0x0013852E
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode result = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return result;
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x0013A35C File Offset: 0x0013855C
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0013A3A4 File Offset: 0x001385A4
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode result;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = (from k in this.m_Dict
				where k.Value == aNode
				select k).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				result = aNode;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060040BC RID: 16572 RVA: 0x0013A410 File Offset: 0x00138610
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
				{
					yield return keyValuePair.Value;
				}
				Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x0013A420 File Offset: 0x00138620
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				yield return keyValuePair;
			}
			Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x0013A430 File Offset: 0x00138630
		public override string ToString()
		{
			string text = "{";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text = string.Concat(new string[]
				{
					text,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\":",
					keyValuePair.Value.ToString()
				});
			}
			text += "}";
			return text;
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x0013A4E4 File Offset: 0x001386E4
		public override string ToString(string aPrefix)
		{
			string text = "{ ";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text = string.Concat(new string[]
				{
					text,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\" : ",
					keyValuePair.Value.ToString(aPrefix + "   ")
				});
			}
			text = text + "\n" + aPrefix + "}";
			return text;
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x0013A5BC File Offset: 0x001387BC
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(2);
			aWriter.Write(this.m_Dict.Count);
			foreach (string text in this.m_Dict.Keys)
			{
				aWriter.Write(text);
				this.m_Dict[text].Serialize(aWriter);
			}
		}

		// Token: 0x040029D5 RID: 10709
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();
	}
}
