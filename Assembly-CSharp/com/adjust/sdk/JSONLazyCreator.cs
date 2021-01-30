using System;

namespace com.adjust.sdk
{
	// Token: 0x0200073D RID: 1853
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x060040CC RID: 16588 RVA: 0x0013A7C3 File Offset: 0x001389C3
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x0013A7D9 File Offset: 0x001389D9
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x0013A7EF File Offset: 0x001389EF
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x17000933 RID: 2355
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray
				{
					value
				});
			}
		}

		// Token: 0x17000934 RID: 2356
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass
				{
					{
						aKey,
						value
					}
				});
			}
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x0013A870 File Offset: 0x00138A70
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray
			{
				aItem
			});
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x0013A894 File Offset: 0x00138A94
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass
			{
				{
					aKey,
					aItem
				}
			});
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00056FBA File Offset: 0x000551BA
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x0013A8B6 File Offset: 0x00138AB6
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x00056FBA File Offset: 0x000551BA
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x0013A8C2 File Offset: 0x00138AC2
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x0003860A File Offset: 0x0003680A
		public override string ToString()
		{
			return "";
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x0003860A File Offset: 0x0003680A
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060040DB RID: 16603 RVA: 0x0013A8CC File Offset: 0x00138ACC
		// (set) Token: 0x060040DC RID: 16604 RVA: 0x0013A8E8 File Offset: 0x00138AE8
		public override int AsInt
		{
			get
			{
				JSONData aVal = new JSONData(0);
				this.Set(aVal);
				return 0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x060040DD RID: 16605 RVA: 0x0013A904 File Offset: 0x00138B04
		// (set) Token: 0x060040DE RID: 16606 RVA: 0x0013A928 File Offset: 0x00138B28
		public override float AsFloat
		{
			get
			{
				JSONData aVal = new JSONData(0f);
				this.Set(aVal);
				return 0f;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060040DF RID: 16607 RVA: 0x0013A944 File Offset: 0x00138B44
		// (set) Token: 0x060040E0 RID: 16608 RVA: 0x0013A970 File Offset: 0x00138B70
		public override double AsDouble
		{
			get
			{
				JSONData aVal = new JSONData(0.0);
				this.Set(aVal);
				return 0.0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060040E1 RID: 16609 RVA: 0x0013A98C File Offset: 0x00138B8C
		// (set) Token: 0x060040E2 RID: 16610 RVA: 0x0013A9A8 File Offset: 0x00138BA8
		public override bool AsBool
		{
			get
			{
				JSONData aVal = new JSONData(false);
				this.Set(aVal);
				return false;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x0013A9C4 File Offset: 0x00138BC4
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060040E4 RID: 16612 RVA: 0x0013A9E0 File Offset: 0x00138BE0
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x040029D7 RID: 10711
		private JSONNode m_Node;

		// Token: 0x040029D8 RID: 10712
		private string m_Key;
	}
}
