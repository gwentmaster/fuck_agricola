using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D3 RID: 1235
	public abstract class Asn1Set : Asn1Object, IEnumerable
	{
		// Token: 0x06002DCE RID: 11726 RVA: 0x000F0440 File Offset: 0x000EE640
		public static Asn1Set GetInstance(object obj)
		{
			if (obj == null || obj is Asn1Set)
			{
				return (Asn1Set)obj;
			}
			if (obj is Asn1SetParser)
			{
				return Asn1Set.GetInstance(((Asn1SetParser)obj).ToAsn1Object());
			}
			if (obj is byte[])
			{
				try
				{
					return Asn1Set.GetInstance(Asn1Object.FromByteArray((byte[])obj));
				}
				catch (IOException ex)
				{
					throw new ArgumentException("failed to construct set from byte[]: " + ex.Message);
				}
			}
			if (obj is Asn1Encodable)
			{
				Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
				if (asn1Object is Asn1Set)
				{
					return (Asn1Set)asn1Object;
				}
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000F04FC File Offset: 0x000EE6FC
		public static Asn1Set GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			Asn1Object @object = obj.GetObject();
			if (explicitly)
			{
				if (!obj.IsExplicit())
				{
					throw new ArgumentException("object implicit - explicit expected.");
				}
				return (Asn1Set)@object;
			}
			else
			{
				if (obj.IsExplicit())
				{
					return new DerSet(@object);
				}
				if (@object is Asn1Set)
				{
					return (Asn1Set)@object;
				}
				if (@object is Asn1Sequence)
				{
					Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					foreach (object obj2 in ((Asn1Sequence)@object))
					{
						Asn1Encodable asn1Encodable = (Asn1Encodable)obj2;
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							asn1Encodable
						});
					}
					return new DerSet(asn1EncodableVector, false);
				}
				throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000F05D8 File Offset: 0x000EE7D8
		protected internal Asn1Set(int capacity)
		{
			this._set = Platform.CreateArrayList(capacity);
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000F05EC File Offset: 0x000EE7EC
		public virtual IEnumerator GetEnumerator()
		{
			return this._set.GetEnumerator();
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000F05F9 File Offset: 0x000EE7F9
		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000538 RID: 1336
		public virtual Asn1Encodable this[int index]
		{
			get
			{
				return (Asn1Encodable)this._set[index];
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000F0614 File Offset: 0x000EE814
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetObjectAt(int index)
		{
			return this[index];
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x000F061D File Offset: 0x000EE81D
		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06002DD6 RID: 11734 RVA: 0x000F0625 File Offset: 0x000EE825
		public virtual int Count
		{
			get
			{
				return this._set.Count;
			}
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000F0634 File Offset: 0x000EE834
		public virtual Asn1Encodable[] ToArray()
		{
			Asn1Encodable[] array = new Asn1Encodable[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				array[i] = this[i];
			}
			return array;
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002DD8 RID: 11736 RVA: 0x000F0669 File Offset: 0x000EE869
		public Asn1SetParser Parser
		{
			get
			{
				return new Asn1Set.Asn1SetParserImpl(this);
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000F0674 File Offset: 0x000EE874
		protected override int Asn1GetHashCode()
		{
			int num = this.Count;
			foreach (object obj in this)
			{
				num *= 17;
				if (obj == null)
				{
					num ^= DerNull.Instance.GetHashCode();
				}
				else
				{
					num ^= obj.GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000F06E4 File Offset: 0x000EE8E4
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			Asn1Set asn1Set = asn1Object as Asn1Set;
			if (asn1Set == null)
			{
				return false;
			}
			if (this.Count != asn1Set.Count)
			{
				return false;
			}
			IEnumerator enumerator = this.GetEnumerator();
			IEnumerator enumerator2 = asn1Set.GetEnumerator();
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object obj = this.GetCurrent(enumerator).ToAsn1Object();
				Asn1Object obj2 = this.GetCurrent(enumerator2).ToAsn1Object();
				if (!obj.Equals(obj2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000F0754 File Offset: 0x000EE954
		private Asn1Encodable GetCurrent(IEnumerator e)
		{
			Asn1Encodable asn1Encodable = (Asn1Encodable)e.Current;
			if (asn1Encodable == null)
			{
				return DerNull.Instance;
			}
			return asn1Encodable;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000F0778 File Offset: 0x000EE978
		protected internal void Sort()
		{
			if (this._set.Count < 2)
			{
				return;
			}
			Asn1Encodable[] array = new Asn1Encodable[this._set.Count];
			byte[][] array2 = new byte[this._set.Count][];
			for (int i = 0; i < this._set.Count; i++)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)this._set[i];
				array[i] = asn1Encodable;
				array2[i] = asn1Encodable.GetEncoded("DER");
			}
			Array.Sort(array2, array, new Asn1Set.DerComparer());
			for (int j = 0; j < this._set.Count; j++)
			{
				this._set[j] = array[j];
			}
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000F0827 File Offset: 0x000EEA27
		protected internal void AddObject(Asn1Encodable obj)
		{
			this._set.Add(obj);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000F0836 File Offset: 0x000EEA36
		public override string ToString()
		{
			return CollectionUtilities.ToString(this._set);
		}

		// Token: 0x04001DE8 RID: 7656
		private readonly IList _set;

		// Token: 0x020008A8 RID: 2216
		private class Asn1SetParserImpl : Asn1SetParser, IAsn1Convertible
		{
			// Token: 0x060045CF RID: 17871 RVA: 0x001457CF File Offset: 0x001439CF
			public Asn1SetParserImpl(Asn1Set outer)
			{
				this.outer = outer;
				this.max = outer.Count;
			}

			// Token: 0x060045D0 RID: 17872 RVA: 0x001457EC File Offset: 0x001439EC
			public IAsn1Convertible ReadObject()
			{
				if (this.index == this.max)
				{
					return null;
				}
				Asn1Set asn1Set = this.outer;
				int num = this.index;
				this.index = num + 1;
				Asn1Encodable asn1Encodable = asn1Set[num];
				if (asn1Encodable is Asn1Sequence)
				{
					return ((Asn1Sequence)asn1Encodable).Parser;
				}
				if (asn1Encodable is Asn1Set)
				{
					return ((Asn1Set)asn1Encodable).Parser;
				}
				return asn1Encodable;
			}

			// Token: 0x060045D1 RID: 17873 RVA: 0x0014584F File Offset: 0x00143A4F
			public virtual Asn1Object ToAsn1Object()
			{
				return this.outer;
			}

			// Token: 0x04002F8D RID: 12173
			private readonly Asn1Set outer;

			// Token: 0x04002F8E RID: 12174
			private readonly int max;

			// Token: 0x04002F8F RID: 12175
			private int index;
		}

		// Token: 0x020008A9 RID: 2217
		private class DerComparer : IComparer
		{
			// Token: 0x060045D2 RID: 17874 RVA: 0x00145858 File Offset: 0x00143A58
			public int Compare(object x, object y)
			{
				byte[] array = (byte[])x;
				byte[] array2 = (byte[])y;
				int num = Math.Min(array.Length, array2.Length);
				int num2 = 0;
				while (num2 != num)
				{
					byte b = array[num2];
					byte b2 = array2[num2];
					if (b != b2)
					{
						if (b >= b2)
						{
							return 1;
						}
						return -1;
					}
					else
					{
						num2++;
					}
				}
				if (array.Length > array2.Length)
				{
					if (!this.AllZeroesFrom(array, num))
					{
						return 1;
					}
					return 0;
				}
				else
				{
					if (array.Length >= array2.Length)
					{
						return 0;
					}
					if (!this.AllZeroesFrom(array2, num))
					{
						return -1;
					}
					return 0;
				}
			}

			// Token: 0x060045D3 RID: 17875 RVA: 0x001458D2 File Offset: 0x00143AD2
			private bool AllZeroesFrom(byte[] bs, int pos)
			{
				while (pos < bs.Length)
				{
					if (bs[pos++] != 0)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
