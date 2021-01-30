using System;
using System.Collections;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004CA RID: 1226
	public class Asn1EncodableVector : IEnumerable
	{
		// Token: 0x06002D8B RID: 11659 RVA: 0x000EF864 File Offset: 0x000EDA64
		public static Asn1EncodableVector FromEnumerable(IEnumerable e)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in e)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					asn1Encodable
				});
			}
			return asn1EncodableVector;
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000EF8D0 File Offset: 0x000EDAD0
		public Asn1EncodableVector(params Asn1Encodable[] v)
		{
			this.Add(v);
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000EF8EC File Offset: 0x000EDAEC
		public void Add(params Asn1Encodable[] objs)
		{
			foreach (Asn1Encodable value in objs)
			{
				this.v.Add(value);
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000EF91C File Offset: 0x000EDB1C
		public void AddOptional(params Asn1Encodable[] objs)
		{
			if (objs != null)
			{
				foreach (Asn1Encodable asn1Encodable in objs)
				{
					if (asn1Encodable != null)
					{
						this.v.Add(asn1Encodable);
					}
				}
			}
		}

		// Token: 0x17000530 RID: 1328
		public Asn1Encodable this[int index]
		{
			get
			{
				return (Asn1Encodable)this.v[index];
			}
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000EF963 File Offset: 0x000EDB63
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable Get(int index)
		{
			return this[index];
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002D91 RID: 11665 RVA: 0x000EF96C File Offset: 0x000EDB6C
		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.v.Count;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002D92 RID: 11666 RVA: 0x000EF96C File Offset: 0x000EDB6C
		public int Count
		{
			get
			{
				return this.v.Count;
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000EF979 File Offset: 0x000EDB79
		public IEnumerator GetEnumerator()
		{
			return this.v.GetEnumerator();
		}

		// Token: 0x04001DE3 RID: 7651
		private IList v = Platform.CreateArrayList();
	}
}
