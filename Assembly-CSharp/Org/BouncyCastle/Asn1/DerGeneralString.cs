using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F3 RID: 1267
	public class DerGeneralString : DerStringBase
	{
		// Token: 0x06002EA2 RID: 11938 RVA: 0x000F2859 File Offset: 0x000F0A59
		public static DerGeneralString GetInstance(object obj)
		{
			if (obj == null || obj is DerGeneralString)
			{
				return (DerGeneralString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x000F2884 File Offset: 0x000F0A84
		public static DerGeneralString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGeneralString)
			{
				return DerGeneralString.GetInstance(@object);
			}
			return new DerGeneralString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x000F28BA File Offset: 0x000F0ABA
		public DerGeneralString(byte[] str) : this(Strings.FromAsciiByteArray(str))
		{
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x000F28C8 File Offset: 0x000F0AC8
		public DerGeneralString(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000F28E5 File Offset: 0x000F0AE5
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000F28ED File Offset: 0x000F0AED
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000F28FA File Offset: 0x000F0AFA
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(27, this.GetOctets());
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000F290C File Offset: 0x000F0B0C
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGeneralString derGeneralString = asn1Object as DerGeneralString;
			return derGeneralString != null && this.str.Equals(derGeneralString.str);
		}

		// Token: 0x04001E36 RID: 7734
		private readonly string str;
	}
}
