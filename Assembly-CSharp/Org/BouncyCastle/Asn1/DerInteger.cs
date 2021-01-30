using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F7 RID: 1271
	public class DerInteger : Asn1Object
	{
		// Token: 0x06002ECE RID: 11982 RVA: 0x000F306E File Offset: 0x000F126E
		public static DerInteger GetInstance(object obj)
		{
			if (obj == null || obj is DerInteger)
			{
				return (DerInteger)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000F3098 File Offset: 0x000F1298
		public static DerInteger GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerInteger)
			{
				return DerInteger.GetInstance(@object);
			}
			return new DerInteger(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000F30DC File Offset: 0x000F12DC
		public DerInteger(int value)
		{
			this.bytes = BigInteger.ValueOf((long)value).ToByteArray();
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000F30F6 File Offset: 0x000F12F6
		public DerInteger(BigInteger value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.bytes = value.ToByteArray();
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x000F3118 File Offset: 0x000F1318
		public DerInteger(byte[] bytes)
		{
			this.bytes = bytes;
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x000F3127 File Offset: 0x000F1327
		public BigInteger Value
		{
			get
			{
				return new BigInteger(this.bytes);
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x000F3134 File Offset: 0x000F1334
		public BigInteger PositiveValue
		{
			get
			{
				return new BigInteger(1, this.bytes);
			}
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000F3142 File Offset: 0x000F1342
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(2, this.bytes);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000F3151 File Offset: 0x000F1351
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.bytes);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000F3160 File Offset: 0x000F1360
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerInteger derInteger = asn1Object as DerInteger;
			return derInteger != null && Arrays.AreEqual(this.bytes, derInteger.bytes);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000F318A File Offset: 0x000F138A
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x04001E3A RID: 7738
		private readonly byte[] bytes;
	}
}
