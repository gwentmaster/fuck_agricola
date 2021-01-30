using System;

namespace UTNotifications
{
	// Token: 0x02000154 RID: 340
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06000D46 RID: 3398 RVA: 0x00056EC7 File Offset: 0x000550C7
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00056EDD File Offset: 0x000550DD
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00056EF3 File Offset: 0x000550F3
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

		// Token: 0x1700006E RID: 110
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

		// Token: 0x1700006F RID: 111
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

		// Token: 0x06000D4D RID: 3405 RVA: 0x00056F74 File Offset: 0x00055174
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray
			{
				aItem
			});
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00056F98 File Offset: 0x00055198
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

		// Token: 0x06000D4F RID: 3407 RVA: 0x00056FBA File Offset: 0x000551BA
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00056FC5 File Offset: 0x000551C5
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00056FBA File Offset: 0x000551BA
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00056FD1 File Offset: 0x000551D1
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003860A File Offset: 0x0003680A
		public override string ToString()
		{
			return "";
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0003860A File Offset: 0x0003680A
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x00056FDC File Offset: 0x000551DC
		// (set) Token: 0x06000D56 RID: 3414 RVA: 0x00056FF8 File Offset: 0x000551F8
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00057014 File Offset: 0x00055214
		// (set) Token: 0x06000D58 RID: 3416 RVA: 0x00057038 File Offset: 0x00055238
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00057054 File Offset: 0x00055254
		// (set) Token: 0x06000D5A RID: 3418 RVA: 0x00057080 File Offset: 0x00055280
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

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x0005709C File Offset: 0x0005529C
		// (set) Token: 0x06000D5C RID: 3420 RVA: 0x000570B8 File Offset: 0x000552B8
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

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x000570D4 File Offset: 0x000552D4
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x000570F0 File Offset: 0x000552F0
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x04000D51 RID: 3409
		private JSONNode m_Node;

		// Token: 0x04000D52 RID: 3410
		private string m_Key;
	}
}
