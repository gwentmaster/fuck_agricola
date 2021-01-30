using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace com.adjust.sdk
{
	// Token: 0x0200073A RID: 1850
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x1700092A RID: 2346
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

		// Token: 0x1700092B RID: 2347
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

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0013A005 File Offset: 0x00138205
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x00139FF7 File Offset: 0x001381F7
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0013A012 File Offset: 0x00138212
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

		// Token: 0x060040AC RID: 16556 RVA: 0x0013A040 File Offset: 0x00138240
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060040AD RID: 16557 RVA: 0x0013A050 File Offset: 0x00138250
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

		// Token: 0x060040AE RID: 16558 RVA: 0x0013A060 File Offset: 0x00138260
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

		// Token: 0x060040AF RID: 16559 RVA: 0x0013A070 File Offset: 0x00138270
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

		// Token: 0x060040B0 RID: 16560 RVA: 0x0013A0F4 File Offset: 0x001382F4
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

		// Token: 0x060040B1 RID: 16561 RVA: 0x0013A198 File Offset: 0x00138398
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x040029D4 RID: 10708
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
