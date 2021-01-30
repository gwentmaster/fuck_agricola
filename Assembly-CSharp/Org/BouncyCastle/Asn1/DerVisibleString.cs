using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000507 RID: 1287
	public class DerVisibleString : DerStringBase
	{
		// Token: 0x06002F5E RID: 12126 RVA: 0x000F4718 File Offset: 0x000F2918
		public static DerVisibleString GetInstance(object obj)
		{
			if (obj == null || obj is DerVisibleString)
			{
				return (DerVisibleString)obj;
			}
			if (obj is Asn1OctetString)
			{
				return new DerVisibleString(((Asn1OctetString)obj).GetOctets());
			}
			if (obj is Asn1TaggedObject)
			{
				return DerVisibleString.GetInstance(((Asn1TaggedObject)obj).GetObject());
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000F477E File Offset: 0x000F297E
		public static DerVisibleString GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DerVisibleString.GetInstance(obj.GetObject());
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000F478B File Offset: 0x000F298B
		public DerVisibleString(byte[] str) : this(Strings.FromAsciiByteArray(str))
		{
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000F4799 File Offset: 0x000F2999
		public DerVisibleString(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000F47B6 File Offset: 0x000F29B6
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000F47BE File Offset: 0x000F29BE
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000F47CB File Offset: 0x000F29CB
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(26, this.GetOctets());
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000F47DC File Offset: 0x000F29DC
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerVisibleString derVisibleString = asn1Object as DerVisibleString;
			return derVisibleString != null && this.str.Equals(derVisibleString.str);
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000F4806 File Offset: 0x000F2A06
		protected override int Asn1GetHashCode()
		{
			return this.str.GetHashCode();
		}

		// Token: 0x04001E4B RID: 7755
		private readonly string str;
	}
}
