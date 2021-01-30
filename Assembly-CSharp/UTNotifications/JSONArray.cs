using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UTNotifications
{
	// Token: 0x02000151 RID: 337
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x17000065 RID: 101
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		// Token: 0x17000066 RID: 102
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00056709 File Offset: 0x00054909
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x000566FB File Offset: 0x000548FB
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00056716 File Offset: 0x00054916
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode result = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00056744 File Offset: 0x00054944
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00056754 File Offset: 0x00054954
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00056764 File Offset: 0x00054964
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode jsonnode in this.m_List)
			{
				yield return jsonnode;
			}
			List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00056774 File Offset: 0x00054974
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000567F8 File Offset: 0x000549F8
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0005689C File Offset: 0x00054A9C
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x04000D4E RID: 3406
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
