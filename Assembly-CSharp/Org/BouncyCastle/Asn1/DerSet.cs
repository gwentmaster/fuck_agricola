using System;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004FF RID: 1279
	public class DerSet : Asn1Set
	{
		// Token: 0x06002F1E RID: 12062 RVA: 0x000F3D9C File Offset: 0x000F1F9C
		public static DerSet FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new DerSet(v);
			}
			return DerSet.Empty;
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x000F3DB3 File Offset: 0x000F1FB3
		internal static DerSet FromVector(Asn1EncodableVector v, bool needsSorting)
		{
			if (v.Count >= 1)
			{
				return new DerSet(v, needsSorting);
			}
			return DerSet.Empty;
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000F3DCB File Offset: 0x000F1FCB
		public DerSet() : base(0)
		{
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000F3DD4 File Offset: 0x000F1FD4
		public DerSet(Asn1Encodable obj) : base(1)
		{
			base.AddObject(obj);
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000F3DE4 File Offset: 0x000F1FE4
		public DerSet(params Asn1Encodable[] v) : base(v.Length)
		{
			foreach (Asn1Encodable obj in v)
			{
				base.AddObject(obj);
			}
			base.Sort();
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000F3E1B File Offset: 0x000F201B
		public DerSet(Asn1EncodableVector v) : this(v, true)
		{
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000F3E28 File Offset: 0x000F2028
		internal DerSet(Asn1EncodableVector v, bool needsSorting) : base(v.Count)
		{
			foreach (object obj in v)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				base.AddObject(obj2);
			}
			if (needsSorting)
			{
				base.Sort();
			}
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000F3E94 File Offset: 0x000F2094
		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerOutputStream derOutputStream = new DerOutputStream(memoryStream);
			foreach (object obj in this)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				derOutputStream.WriteObject(obj2);
			}
			Platform.Dispose(derOutputStream);
			byte[] bytes = memoryStream.ToArray();
			derOut.WriteEncoded(49, bytes);
		}

		// Token: 0x04001E44 RID: 7748
		public static readonly DerSet Empty = new DerSet();
	}
}
