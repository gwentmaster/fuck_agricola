using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D4 RID: 1236
	public abstract class Asn1TaggedObject : Asn1Object, Asn1TaggedObjectParser, IAsn1Convertible
	{
		// Token: 0x06002DDF RID: 11743 RVA: 0x000F0844 File Offset: 0x000EEA44
		internal static bool IsConstructed(bool isExplicit, Asn1Object obj)
		{
			if (isExplicit || obj is Asn1Sequence || obj is Asn1Set)
			{
				return true;
			}
			Asn1TaggedObject asn1TaggedObject = obj as Asn1TaggedObject;
			return asn1TaggedObject != null && Asn1TaggedObject.IsConstructed(asn1TaggedObject.IsExplicit(), asn1TaggedObject.GetObject());
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000F0883 File Offset: 0x000EEA83
		public static Asn1TaggedObject GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			if (explicitly)
			{
				return (Asn1TaggedObject)obj.GetObject();
			}
			throw new ArgumentException("implicitly tagged tagged object");
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000F089E File Offset: 0x000EEA9E
		public static Asn1TaggedObject GetInstance(object obj)
		{
			if (obj == null || obj is Asn1TaggedObject)
			{
				return (Asn1TaggedObject)obj;
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000F08CC File Offset: 0x000EEACC
		protected Asn1TaggedObject(int tagNo, Asn1Encodable obj)
		{
			this.explicitly = true;
			this.tagNo = tagNo;
			this.obj = obj;
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000F08F0 File Offset: 0x000EEAF0
		protected Asn1TaggedObject(bool explicitly, int tagNo, Asn1Encodable obj)
		{
			this.explicitly = (explicitly || obj is IAsn1Choice);
			this.tagNo = tagNo;
			this.obj = obj;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000F0924 File Offset: 0x000EEB24
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			Asn1TaggedObject asn1TaggedObject = asn1Object as Asn1TaggedObject;
			return asn1TaggedObject != null && (this.tagNo == asn1TaggedObject.tagNo && this.explicitly == asn1TaggedObject.explicitly) && object.Equals(this.GetObject(), asn1TaggedObject.GetObject());
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000F096C File Offset: 0x000EEB6C
		protected override int Asn1GetHashCode()
		{
			int num = this.tagNo.GetHashCode();
			if (this.obj != null)
			{
				num ^= this.obj.GetHashCode();
			}
			return num;
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x000F099C File Offset: 0x000EEB9C
		public int TagNo
		{
			get
			{
				return this.tagNo;
			}
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000F09A4 File Offset: 0x000EEBA4
		public bool IsExplicit()
		{
			return this.explicitly;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsEmpty()
		{
			return false;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000F09AC File Offset: 0x000EEBAC
		public Asn1Object GetObject()
		{
			if (this.obj != null)
			{
				return this.obj.ToAsn1Object();
			}
			return null;
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000F09C4 File Offset: 0x000EEBC4
		public IAsn1Convertible GetObjectParser(int tag, bool isExplicit)
		{
			if (tag == 4)
			{
				return Asn1OctetString.GetInstance(this, isExplicit).Parser;
			}
			if (tag == 16)
			{
				return Asn1Sequence.GetInstance(this, isExplicit).Parser;
			}
			if (tag == 17)
			{
				return Asn1Set.GetInstance(this, isExplicit).Parser;
			}
			if (isExplicit)
			{
				return this.GetObject();
			}
			throw Platform.CreateNotImplementedException("implicit tagging for tag: " + tag);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000F0A25 File Offset: 0x000EEC25
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				this.tagNo,
				"]",
				this.obj
			});
		}

		// Token: 0x04001DE9 RID: 7657
		internal int tagNo;

		// Token: 0x04001DEA RID: 7658
		internal bool explicitly = true;

		// Token: 0x04001DEB RID: 7659
		internal Asn1Encodable obj;
	}
}
