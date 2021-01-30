using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F1 RID: 1265
	public class DerBoolean : Asn1Object
	{
		// Token: 0x06002E8B RID: 11915 RVA: 0x000F256A File Offset: 0x000F076A
		public static DerBoolean GetInstance(object obj)
		{
			if (obj == null || obj is DerBoolean)
			{
				return (DerBoolean)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000F2593 File Offset: 0x000F0793
		public static DerBoolean GetInstance(bool value)
		{
			if (!value)
			{
				return DerBoolean.False;
			}
			return DerBoolean.True;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000F25A4 File Offset: 0x000F07A4
		public static DerBoolean GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBoolean)
			{
				return DerBoolean.GetInstance(@object);
			}
			return DerBoolean.FromOctetString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000F25DA File Offset: 0x000F07DA
		public DerBoolean(byte[] val)
		{
			if (val.Length != 1)
			{
				throw new ArgumentException("byte value should have 1 byte in it", "val");
			}
			this.value = val[0];
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000F2601 File Offset: 0x000F0801
		private DerBoolean(bool value)
		{
			this.value = (value ? byte.MaxValue : 0);
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06002E90 RID: 11920 RVA: 0x000F261A File Offset: 0x000F081A
		public bool IsTrue
		{
			get
			{
				return this.value > 0;
			}
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000F2625 File Offset: 0x000F0825
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(1, new byte[]
			{
				this.value
			});
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x000F2640 File Offset: 0x000F0840
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBoolean derBoolean = asn1Object as DerBoolean;
			return derBoolean != null && this.IsTrue == derBoolean.IsTrue;
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x000F2668 File Offset: 0x000F0868
		protected override int Asn1GetHashCode()
		{
			return this.IsTrue.GetHashCode();
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000F2683 File Offset: 0x000F0883
		public override string ToString()
		{
			if (!this.IsTrue)
			{
				return "FALSE";
			}
			return "TRUE";
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000F2698 File Offset: 0x000F0898
		internal static DerBoolean FromOctetString(byte[] value)
		{
			if (value.Length != 1)
			{
				throw new ArgumentException("BOOLEAN value should have 1 byte in it", "value");
			}
			byte b = value[0];
			if (b == 0)
			{
				return DerBoolean.False;
			}
			if (b != 255)
			{
				return new DerBoolean(value);
			}
			return DerBoolean.True;
		}

		// Token: 0x04001E31 RID: 7729
		private readonly byte value;

		// Token: 0x04001E32 RID: 7730
		public static readonly DerBoolean False = new DerBoolean(false);

		// Token: 0x04001E33 RID: 7731
		public static readonly DerBoolean True = new DerBoolean(true);
	}
}
