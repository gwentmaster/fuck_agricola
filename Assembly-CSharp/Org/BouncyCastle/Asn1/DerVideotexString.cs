using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000506 RID: 1286
	public class DerVideotexString : DerStringBase
	{
		// Token: 0x06002F56 RID: 12118 RVA: 0x000F45E4 File Offset: 0x000F27E4
		public static DerVideotexString GetInstance(object obj)
		{
			if (obj == null || obj is DerVideotexString)
			{
				return (DerVideotexString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerVideotexString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString(), "obj");
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x000F4668 File Offset: 0x000F2868
		public static DerVideotexString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerVideotexString)
			{
				return DerVideotexString.GetInstance(@object);
			}
			return new DerVideotexString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x000F469E File Offset: 0x000F289E
		public DerVideotexString(byte[] encoding)
		{
			this.mString = Arrays.Clone(encoding);
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000F46B2 File Offset: 0x000F28B2
		public override string GetString()
		{
			return Strings.FromByteArray(this.mString);
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000F46BF File Offset: 0x000F28BF
		public byte[] GetOctets()
		{
			return Arrays.Clone(this.mString);
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000F46CC File Offset: 0x000F28CC
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(21, this.mString);
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000F46DC File Offset: 0x000F28DC
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.mString);
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000F46EC File Offset: 0x000F28EC
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerVideotexString derVideotexString = asn1Object as DerVideotexString;
			return derVideotexString != null && Arrays.AreEqual(this.mString, derVideotexString.mString);
		}

		// Token: 0x04001E4A RID: 7754
		private readonly byte[] mString;
	}
}
